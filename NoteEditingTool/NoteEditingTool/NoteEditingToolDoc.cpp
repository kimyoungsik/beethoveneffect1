
// NoteEditingToolDoc.cpp : CNoteEditingToolDoc 클래스의 구현
//

#include "stdafx.h"
// SHARED_HANDLERS는 미리 보기, 축소판 그림 및 검색 필터 처리기를 구현하는 ATL 프로젝트에서 정의할 수 있으며
// 해당 프로젝트와 문서 코드를 공유하도록 해 줍니다.
#ifndef SHARED_HANDLERS
#include "NoteEditingTool.h"
#endif

#include "NoteEditingToolDoc.h"
#include "NotePickingView.h"

#include <propkey.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// CNoteEditingToolDoc



UINT CM_REDRAW_DC2 = RegisterWindowMessage(_T("CM_REDRAW_DC"));
IMPLEMENT_DYNCREATE(CNoteEditingToolDoc, CDocument)

BEGIN_MESSAGE_MAP(CNoteEditingToolDoc, CDocument)
	ON_COMMAND(ID_OPEN_NEW_PICTURE_FILE, &CNoteEditingToolDoc::OnOpenNewPictureFile)
END_MESSAGE_MAP()


// CNoteEditingToolDoc 생성/소멸

CNoteEditingToolDoc::CNoteEditingToolDoc()
{
	// TODO: 여기에 일회성 생성 코드를 추가합니다.
	noteEditingMode = EDIT_MODE_WRITE;			// 기본은 그리기 모드
	noteWriteType = NOTE_T_RIGHT;				// 기본은 오른손 노트

	nowEditingNote = NULL;						// 현재 수정중에 있는 노트를 말한다.
	onFocusEditingFlag = false;
	noteHeadDir = 0;
	nowConfigingNote = NULL;						// 현재 설정중에 있는 노트를 말한다. CONFG모드 외에는 항상 NULL이어야 한다.
	noteConfigHeadDir = 0;

	noteEditingToolViewPtr = NULL;
	notePickingViewPtr = NULL;
	editModeSelectViewPtr = NULL;

	mnfFilePath = "";
	isNewFileFlag = true;
}

CNoteEditingToolDoc::~CNoteEditingToolDoc()
{
}



//////////////////////////////////////////////////////////////////////////////
//////////////////////////////   [Protected]    //////////////////////////////
//////////////////////////////////////////////////////////////////////////////



// 대략 어떤 종류의 노트인지를 확인한다.
char CNoteEditingToolDoc::chkTypeToBigType(const char noteType)
{
	switch( noteType )
	{	
	case NOTE_T_RIGHT:
	case NOTE_T_LEFT:
	case NOTE_T_LONG:
		return NOTE_TYPE_LINE_1;
		break;

	case NOTE_T_PATTERN:
	case NOTE_T_CHARISMA:
	case NOTE_T_NEUTRAL:
	case NOTE_T_DRAG:
		return NOTE_TYPE_LINE_2;
		break;

	case NOTE_T_BPM:
	case NOTE_T_PHOTO:
		return NOTE_TYPE_LINE_3;
		break;
	}

	return '/';
}


// 헤더를 만드는 함수.
int CNoteEditingToolDoc::SaveFileHeader(CArchive &ar)
{
//	string tmpString;
	char tmpCharArray[20];
	double noteTimeDouble;


	CString dataString;
	CString endlString;						// 줄바꿈을 위한 String (유니코드 문제)
	endlString.AppendChar(0x000d);			// Endian 문제로 이와 같이 입력.
	endlString.AppendChar(0x000a);

	// 유니코드 인코딩을 위해서 FFFE를 맨 처음에 넣어준다.
	dataString.AppendChar(0xFEFF);
	ar.WriteString(dataString);

	// 최초 문자열들
	dataString.Format(TEXT("MNF FILE"));
	dataString += endlString;
	dataString += endlString;
	dataString += _T("#################### [HEADER] ####################");
	dataString += endlString;
	ar.WriteString(dataString);

	// 노트 파일 버전
	dataString.Format(TEXT("%d"), newNoteFile.getNoteFileVersion());
	dataString += endlString;
	ar.WriteString(dataString);

	// 곡 제목
	//dataString = _T("Title : ");
	dataString = _T("");
	dataString += newNoteFile.getNoteFileTitle().c_str();
	dataString += endlString;
	ar.WriteString(dataString);

	// 아티스트
	//dataString += _T("Artist : ");
	dataString = _T("");
	dataString += newNoteFile.getNoteFileArtist().c_str();
	dataString += endlString;
	ar.WriteString(dataString);

	// 난이도
	dataString.Format(TEXT("%d"), newNoteFile.getNoteFileLevel());
	dataString += endlString;
	ar.WriteString(dataString);

	// mp3파일 이름 [혹시나 파일명이 엄청나게 길 수도 있으므로, 새로 CString에 저장]
	//dataString = _T("mp3 File name : ");
	dataString = _T("");
	dataString += newNoteFile.getMp3FileName().c_str();
	dataString += endlString;
	ar.WriteString(dataString);

	// 타이틀 그림 이름 [혹시나 파일명이 엄청나게 길 수도 있으므로, 새로 CString에 저장]
	//dataString = _T("Title picture : ");
	dataString = _T("");
	dataString += newNoteFile.getTitlePicture().c_str();
	dataString += endlString;
	ar.WriteString(dataString);

	// 시작시간.
	noteTimeDouble = newNoteFile.getStartSongTime().noteTimeSec;
	noteTimeDouble += (newNoteFile.getStartSongTime().noteTimeMilSec / (double)1000);
	dataString.Format(TEXT("%.3lf"), noteTimeDouble);
	dataString += endlString;
	ar.WriteString(dataString);

	// 종료시간.
	noteTimeDouble = newNoteFile.getEndSongTime().noteTimeSec;
	noteTimeDouble += (newNoteFile.getEndSongTime().noteTimeMilSec / (double)1000);
	dataString.Format(TEXT("%.3lf"), noteTimeDouble);
	dataString += endlString;
	ar.WriteString(dataString);

	// 최초 BPM
	//dataString = _T("Initial BPM : ");
	dataString = _T("");
	_itoa_s(newNoteFile.getNoteFileInitBPM(), tmpCharArray, 18, 10);				// int형을 10진법으로 버퍼에 때려넣기 (최대 18)
	//_itoa(newNoteFile.getNoteFileInitBPM(), tmpCharArray, 10);
	dataString += tmpCharArray;
	dataString += endlString;
	dataString += endlString;
	ar.WriteString(dataString);



	return 1;
}

// 파일 몸체를 만드는 함수.
int CNoteEditingToolDoc::SaveFileBody(CArchive &ar)
{
	CNoteData *tempNote = NULL;

	CString dataString;
	CString endlString;						// 줄바꿈을 위한 String (유니코드 문제)
	endlString.AppendChar(0x000d);			// Endian 문제로 이와 같이 입력.
	endlString.AppendChar(0x000a);

	//dataString += _T("#################### [HEADER] ####################");
	dataString = _T("##################### [BODY] #####################");
	dataString += endlString;
	ar.WriteString(dataString);

	for( int i=0 ; i<MAX_NOTE_ARRAY ; i++ )
	{
		for( int j=0 ; j<MAX_HALF_ARRAY ; j++ )
		{
			tempNote = newNoteFile.getNoteAddr(i, j);
			if( tempNote == NULL )
			{
				break;
			}

			// 비어있지 않을 경우.
			if( SaveBodyNote(ar, tempNote) < 0 )
			{
				// 에러
				AfxMessageBox(_T("CNoteEditingToolDoc::SaveFileBody 에러!"));
				return -1;
			}
		}
	}

	//ar.WriteString(endlString);


	return 1;
}


// 노트 하나에 대해서 기록하는 함수.
int CNoteEditingToolDoc::SaveBodyNote(CArchive &ar, CNoteData *targetNote)
{
//	unsigned int tempBPM = 0;
	int i=0;
	dotData tempDotData;
	double noteTimeDouble = targetNote->getNoteTimeSec();
	noteTimeDouble += (targetNote->getNoteTimeMilSec() / (double)1000);

	CString dataString;
	CString tmpString;
	CString slashString;
	slashString.AppendChar(0x002F);			// '/' 를 입력
	CString endlString;						// 줄바꿈을 위한 String (유니코드 문제)
	endlString.AppendChar(0x000d);			// Endian 문제로 이와 같이 입력.
	endlString.AppendChar(0x000a);

	// 시작 시간
	dataString.Format(TEXT("%.3lf"), noteTimeDouble);
	dataString += slashString;

	// 노트 타입
	tmpString.Format(TEXT("%c"), targetNote->getNoteType());
	dataString += tmpString;
	dataString += slashString;

	switch ( targetNote->getNoteType())
	{
		// 노트의 타입에 따라서 출력이 바뀐다.
	case NOTE_T_RIGHT:
	case NOTE_T_LEFT:
	case NOTE_T_PHOTO:
		// 타겟 마커
		tmpString.Format(TEXT("%c"), targetNote->getTargetMarker());
		dataString += tmpString;
		dataString += slashString;
		break;


	case NOTE_T_BPM:
		// BPM 노트의 경우, 타겟 마커를 숫자로 출력.
		//tempBPM = targetNote->getTargetMarker();
		//tempBPM = (0x000000ff & (unsigned int)(targetNote->getTargetMarker()));
		tmpString.Format(TEXT("%d"), (0x000000ff &  (unsigned int)(targetNote->getTargetMarker())));
		//tmpString.Format(TEXT("%d"), tempBPM);
		dataString += tmpString;
		dataString += slashString;
		break;


	case NOTE_T_LONG:
	case NOTE_T_PATTERN:
	case NOTE_T_CHARISMA:
	case NOTE_T_NEUTRAL:
		// 타겟 마커
		tmpString.Format(TEXT("%c"), targetNote->getTargetMarker());
		dataString += tmpString;
		dataString += slashString;

		// 끝나는 시간 출력.
		noteTimeDouble = (unsigned char)(targetNote->getNoteEndTimeSec());
		noteTimeDouble += targetNote->getNoteEndTimeMilSec() / (double)1000;
		tmpString.Format(TEXT("%.3lf"), noteTimeDouble);
		dataString += tmpString;
		dataString += slashString;
		break;


	case NOTE_T_DRAG:
		// 끝나는 시간 출력.
		noteTimeDouble = targetNote->getNoteEndTimeSec();
		noteTimeDouble += targetNote->getNoteEndTimeMilSec() / (double)1000;
		tmpString.Format(TEXT("%.3lf"), noteTimeDouble);
		dataString += tmpString;
		dataString += slashString;

		// 드래그 노트의 포인트 출력
		for(i=0 ; i < 4 ; i++ )
		{
			if( targetNote->getDragPoint(i,tempDotData) < 0 )
			{
				AfxMessageBox(_T("CNoteEditingToolDoc::SaveBodyNote에서 Drag 리턴값 에러"));
				return -1;
			}
			tmpString.Format(TEXT("%d,%d"), tempDotData.x, tempDotData.y);
			dataString += tmpString;
			dataString += slashString;
		}
		break;
		

	default:
		// 뭔가 잘못된 노트가 있을 경우
		AfxMessageBox(_T("노트 저장과정 CNoteEditingToolDoc::SaveBodyNote에서 타입 에러!"));
		return -1;
	}

	dataString += endlString;
	ar.WriteString(dataString);

	return 1;
}



// 헤더를 읽는 함수.
int CNoteEditingToolDoc::LoadFileHeader(CArchive &ar)
{
	CString getString;
	CString compString;						// 비교를 위한 String

	string tempString;
	unsigned int tempBPM = 0;

	NoteTime tempTime;
	unsigned int timeSec = 0;
	unsigned int timeMilSec = 0;
	int i=0;
	int j=0;


	compString.AppendChar(0xFEFF); 
	getString.Format(TEXT("MNF FILE"));
	compString += getString;

	ar.ReadString(getString);
	

	if( getString != compString )
	{
		// 맨 첫 문장 "MNF FILE"이 맞는지 확인한다.
		AfxMessageBox(_T("Load Header Error!"));
		return -1;
	}

	ar.ReadString(getString);		// 공백
	ar.ReadString(getString);		// HEADER
	compString.Format(TEXT("#################### [HEADER] ####################"));
	if( getString != compString )
	{
		// 맨 첫 문장 "MNF FILE"이 맞는지 확인한다.
		AfxMessageBox(_T("Load Header Error!"));
		return -1;
	}

	// 노트 파일 버전
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	tempBPM = atoi(tempString.c_str());					// 정수형으로 변환
	newNoteFile.setNoteFileVersion(tempBPM);

	// 타이틀
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	newNoteFile.setNoteFileTitle(tempString);

	// 아티스트
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	newNoteFile.setNoteFileArtist(tempString);
	
	// 노트 파일 난이도
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	tempBPM = atoi(tempString.c_str());					// 정수형으로 변환
	newNoteFile.setNoteFileLevel(tempBPM);

	// mp3파일 이름
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	newNoteFile.setMp3FileName(tempString);			///////////////

	// 타이틀 그림
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	newNoteFile.setTitlePicture(tempString);			/////////////////

	// 시작 시간
	ar.ReadString(getString);
	tempString = (CT2CA)getString;

	i=0;
	j=0;
	timeSec = 0;
	timeMilSec = 0;
	while( TRUE )
	{
		if( tempString[i] == 0 )
		{
			// 초를 다 입력받기도 전에, 스트링의 끝에 왔다면, 에러.
			return -1;
		}
		if( tempString[i] == '.' )
		{
			// string에서 초(Sec)를 추출한다.
			i++;
			break;
		}

		timeSec *= 10;
		timeSec += (unsigned int)(tempString[i] - '0');
		i++;
	}
	while( TRUE )
	{
		if( j >= 3 )
		{
			// string에서 밀리초를 전부 다 추출 한 것이므로 종료.
			break;
		}
		if ( tempString[i] == 0 )
		{	return -1;	}

		timeMilSec *= 10;
		timeMilSec += (unsigned int)(tempString[i] - '0');
		i++;
		j++;
	}

	// 입력
	tempTime.noteTimeSec = timeSec;
	tempTime.noteTimeMilSec = timeMilSec;
	newNoteFile.setStartSongTime(tempTime);


	// 종료 시간
	ar.ReadString(getString);
	tempString = (CT2CA)getString;

	i=0;
	j=0;
	timeSec = 0;
	timeMilSec = 0;
	while( TRUE )
	{
		if( tempString[i] == 0 )
		{
			// 초를 다 입력받기도 전에, 스트링의 끝에 왔다면, 에러.
			return -1;
		}
		if( tempString[i] == '.' )
		{
			// string에서 초(Sec)를 추출한다.
			i++;
			break;
		}

		timeSec *= 10;
		timeSec += (unsigned int)(tempString[i] - '0');
		i++;
	}
	while( TRUE )
	{
		if( j >= 3 )
		{	// string에서 밀리초를 전부 다 추출 한 것이므로 종료.
			break;
		}

		if ( tempString[i] == 0 )
		{	return -1;	}

		timeMilSec *= 10;
		timeMilSec += (unsigned int)(tempString[i] - '0');
		i++;
		j++;
	}

	// 입력
	tempTime.noteTimeSec = timeSec;
	tempTime.noteTimeMilSec = timeMilSec;
	newNoteFile.setEndSongTime(tempTime);


	// BPM
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	tempBPM = atoi(tempString.c_str());					// 정수형으로 변환
	newNoteFile.setNoteFileInitBPM(tempBPM);


	return 1;
}


// 파일 몸체를 읽는 함수.
int CNoteEditingToolDoc::LoadFileBody(CArchive &ar)
{
	CString getString;
	CString compString;						// 비교를 위한 String
	bool loopFlag = true;

	ar.ReadString(getString);		// 공백
	ar.ReadString(getString);		// BODY
	compString.Format(TEXT("##################### [BODY] #####################"));
	if( getString != compString )
	{
		// 맨 첫 문장 "MNF FILE"이 맞는지 확인한다.
		AfxMessageBox(_T("Load Body Error!"));
		return -1;
	}

	while ( loopFlag )
	{
		switch ( LoadBodyNote(ar, newNoteFile) )
		{
		case -1:
			// 에러가 났을 때
			AfxMessageBox(_T("Note format Error!"));
			break;
		case 0:
			// 맨 마지막 노트일 때
			loopFlag = false;
			break;

		case 1:
			// 정상 종료일 때,
			break;
		}
	}



	return 1;
}

// 노트 하나에 대해서 읽어오는 함수.
int CNoteEditingToolDoc::LoadBodyNote(CArchive &ar, CNoteFile &targetNoteFile)
{
	CString getString;
	CString compString;						// 비교를 위한 String

	string tempString;


	NoteTime tempStartTime;
	NoteTime tempEndTime;
	char tempNoteType = 0;
	char tempTargetNote = 0;
	dotData tempDotData[MAX_DOT_DATA_NUM];

	unsigned int timeSec = 0;
	unsigned int timeMilSec = 0;
	int strCurr=0;
	int j=0;


	ar.ReadString(getString);

	// 모든 노트를 다 읽고 난 후인지 확인
	if ( getString == "END" )
	{
		return 0;
	}


	for( j=0 ; j<MAX_DOT_DATA_NUM ; j++ )
	{
		tempDotData[j].x = 0;
		tempDotData[j].y = 0;
	}
	
	tempString = (CT2CA)getString;

	timeSec = 0;
	timeMilSec = 0;

	// 시작시간 파싱
	for( strCurr=0 ; (tempString[strCurr] != '.') ; strCurr++ )
	{
		// 혹시나 i값이 너무나 커질 수 있으므로,
		if( strCurr > 20 )
		{
			return -1;
		}
		if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
		{
			// 숫자가 아닌 이상한 글자일 경우
			return -1;
		}

		timeSec *= 10;
		timeSec += (tempString[strCurr] - '0');
	}
	strCurr++;				// '.' 다음의 숫자로 넘어간다.

	for( j=0 ; j < 3 ; j++ )
	{
		if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
		{
			// 숫자가 아닌 이상한 글자일 경우
			return -1;
		}

		timeMilSec *= 10;
		timeMilSec += (tempString[strCurr] - '0');

		strCurr++;
	}

	tempStartTime.noteTimeSec = timeSec;
	tempStartTime.noteTimeMilSec = timeMilSec%1000;

	// '/' 확인
	if( tempString[strCurr++] != '/' )
	{
		// 잘못된 분석을 한 것이므로 에러
		return -1;
	}

	// 노트 타입 확인
	tempNoteType = tempString[strCurr++];

	// '/' 확인
	if( tempString[strCurr++] != '/' )
	{
		// 잘못된 분석을 한 것이므로 에러
		return -1;
	}

	switch( tempNoteType )
	{
		//const char NOTE_T_RIGHT = '1';
		//const char NOTE_T_LEFT = '2';
		//const char NOTE_T_LONG = '4';
		//const char NOTE_T_DRAG = 'D';					// Drag Note
		//const char NOTE_T_CHARISMA = 'C';				// Charisma Time
		//const char NOTE_T_NEUTRAL = 'N';				// Neutral Position
		//const char NOTE_T_PATTERN = 'P';				// Pattern Change
		//const char NOTE_T_BPM = 'B';					// BPM Change
		//const char NOTE_T_PHOTO = 'H';				// Photo Time
	case NOTE_T_RIGHT:
	case NOTE_T_LEFT:
	case NOTE_T_PHOTO:
		// 다음 타겟 마커로 끝날 경우.
		tempTargetNote = tempString[strCurr++];
		if( tempString[strCurr] != '/' )
		{
			// 에러
			return -1;
		}

		// 실질적으로 노트 추가
		newNoteFile.addNewNote(tempStartTime.noteTimeSec, tempStartTime.noteTimeMilSec, tempNoteType, tempTargetNote);

		break;


	case NOTE_T_BPM:
		// 다음 타겟 마커 (char형으로 변환)
		tempTargetNote = 0;
		for( ; tempString[strCurr] != '/' ; )
		{
			if ( tempString[strCurr] == NULL )
			{
				return -1;
			}
			if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
			{
				return -1;
			}

			tempTargetNote *= 10;
			tempTargetNote += (tempString[strCurr++] - '0');
		}

		if( tempString[strCurr] != '/' )
		{
			// 에러
			return -1;
		}

		// 실질적으로 노트 추가
		newNoteFile.addNewNote(tempStartTime.noteTimeSec, tempStartTime.noteTimeMilSec, tempNoteType, tempTargetNote);

		break;

	case NOTE_T_LONG:
	case NOTE_T_CHARISMA:
	case NOTE_T_NEUTRAL:
	case NOTE_T_PATTERN:
		// 타겟마커/종료시간 으로 끝날 경우.

		tempTargetNote = tempString[strCurr++];
		if( tempString[strCurr++] != '/' )
		{
			// 에러
			return -1;
		}


		// 종료시간 파싱
		timeSec = 0;
		timeMilSec = 0;

		for( ; (tempString[strCurr] != '.') ; strCurr++ )
		{
			// 혹시나 i값이 너무나 커질 수 있으므로,
			if( strCurr > 20 || tempString[strCurr] == NULL )
			{
				return -1;
			}
			if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
			{
				// 숫자가 아닌 이상한 글자일 경우
				return -1;
			}

			timeSec *= 10;
			timeSec += (tempString[strCurr] - '0');
		}
		strCurr++;				// '.' 다음의 숫자로 넘어간다.

		for( j=0 ; j < 3 ; j++ )
		{
			if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
			{
				// 숫자가 아닌 이상한 글자일 경우
				return -1;
			}

			timeMilSec *= 10;
			timeMilSec += (tempString[strCurr] - '0');

			strCurr++;
		}

		tempEndTime.noteTimeSec = timeSec;
		tempEndTime.noteTimeMilSec = timeMilSec%1000;


		// 실질적으로 노트 추가
		newNoteFile.addNewNote(tempStartTime.noteTimeSec, tempStartTime.noteTimeMilSec, tempNoteType, tempTargetNote, tempEndTime.noteTimeSec, tempEndTime.noteTimeMilSec);

		break;

	
	case NOTE_T_DRAG:
		// 종료시간/포인트 * 4로 끝날 경우.
		// 종료시간 파싱
		timeSec = 0;
		timeMilSec = 0;

		for( ; (tempString[strCurr] != '.') ; strCurr++ )
		{
			// 혹시나 i값이 너무나 커질 수 있으므로,
			if( strCurr > 20 || tempString[strCurr] == NULL )
			{
				return -1;
			}
			if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
			{
				// 숫자가 아닌 이상한 글자일 경우
				return -1;
			}

			timeSec *= 10;
			timeSec += (tempString[strCurr] - '0');
		}
		strCurr++;				// '.' 다음의 숫자로 넘어간다.

		for( j=0 ; j < 3 ; j++ )
		{
			if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
			{
				// 숫자가 아닌 이상한 글자일 경우
				return -1;
			}

			timeMilSec *= 10;
			timeMilSec += (tempString[strCurr] - '0');

			strCurr++;
		}

		tempEndTime.noteTimeSec = timeSec;
		tempEndTime.noteTimeMilSec = timeMilSec%1000;

		// '/' 확인
		if( tempString[strCurr++] != '/' )
		{
			return -1;
		}

		// 좌표 Get.
		for( j=0 ; j<4 ; j++ )
		{
			// 좌표 x
			while( tempString[strCurr] != ',' )
			{
				if( tempString[strCurr] == NULL )
				{
					// 만약 도중에 끊겼을 경우.
					return -1;
				}

				if ( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
				{
					return -1;
				}

				(tempDotData[j].x) *= 10;
				(tempDotData[j].x) += (tempString[strCurr++] - '0');
			}
			strCurr++;

			// 좌표 y
			while( tempString[strCurr] != '/' )
			{			
				if( tempString[strCurr] == NULL )
				{
					// 만약 도중에 끊겼을 경우.
					return -1;
				}

				if ( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
				{
					return -1;
				}

				(tempDotData[j].y) *= 10;
				(tempDotData[j].y) += (tempString[strCurr++] - '0');
			}
			strCurr++;
		}

		newNoteFile.addNewNote(tempStartTime.noteTimeSec, tempStartTime.noteTimeMilSec, tempNoteType, tempTargetNote, tempEndTime.noteTimeSec, tempEndTime.noteTimeMilSec,
			tempDotData[0], tempDotData[1], tempDotData[2], tempDotData[3]);

		break;

	default:
		// 아무런 해당사항도 없을 때,
		return -1;
	}






	


	return 1;

}

//////////////////////////////////////////////////////////////////////////////
//////////////////////////////   [껍데기함수]   //////////////////////////////
//////////////////////////////////////////////////////////////////////////////



// 노트를 하나 적절한 배열에 추가하는 함수.
int CNoteEditingToolDoc::addNewNote(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	return newNoteFile.addNewNote(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
}

// 입력받은 일반 노트를 수정하는 함수.
int CNoteEditingToolDoc::editNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker)
{
	return newNoteFile.editNote(orgNoteTimeSec, orgNoteTimeMilSec, orgNoteType, orgTargetMarker, editNoteTimeSec, editNoteTimeMilSec, editNoteType, editTargetMarker);
}

int CNoteEditingToolDoc::editNote(CNoteData *notePointer,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker)
{
	return newNoteFile.editNote(notePointer, editNoteTimeSec, editNoteTimeMilSec, editNoteType, editTargetMarker);
}


// 입력받은 특수 노트 부분을 수정하는 함수.
int CNoteEditingToolDoc::editLongNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	return newNoteFile.editLongNote(orgNoteTimeSec, orgNoteTimeMilSec, orgNoteType, orgTargetMarker, editNoteTimeSec, editNoteTimeMilSec, editNoteType, editTargetMarker,
		editNoteEndTimeSec, editNoteEndTimeMilSec);
}

int CNoteEditingToolDoc::editLongNote(CNoteData *targetNote,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	return newNoteFile.editLongNote(targetNote, editNoteTimeSec, editNoteTimeMilSec, editNoteType, editTargetMarker,
		editNoteEndTimeSec, editNoteEndTimeMilSec);
}

// 입력받은 드래그 노트의 각 좌표점을 수정하는 함수.
int CNoteEditingToolDoc::editDragNotePoint(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
	const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPointY)
{
	return newNoteFile.editDragNotePoint(orgNoteTimeSec,  orgNoteTimeMilSec, pointNumber, newPointX, newPointY);
}

// 입력받은 노트를 삭제하는 함수.
int CNoteEditingToolDoc::deleteNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker)
{
	return newNoteFile.deleteNote(orgNoteTimeSec, orgNoteTimeMilSec, orgNoteType, orgTargetMarker);
}

int CNoteEditingToolDoc::deleteNote(CNoteData *targetNote)
{
	return newNoteFile.deleteNote(targetNote);
}


// 노트 배열에서 노트의 주소값을 받아내는 함수.
CNoteData* CNoteEditingToolDoc::getNoteAddr(const unsigned int i, const unsigned int j)
{
	return newNoteFile.getNoteAddr(i, j);
}



// 가지고 있는 모든 노트들을 삭제
int CNoteEditingToolDoc::clearAllNotes(void)
{
	setNowConfigingNote(NULL, 0);
	setNowEditingNote(NULL, 0);
	setOnFocusEditingFlag(false);

	// 그 외의변수들 초기화
	mnfFilePath = "";
	isNewFileFlag = true;

	return newNoteFile.clearAllNotes();
}


//////////////////////////////////////////////////////////////////////////////
////////////////////////   [Get & Set의 껍데기함수]   ////////////////////////
//////////////////////////////////////////////////////////////////////////////

string CNoteEditingToolDoc::getMp3FileName(void)
{
	return newNoteFile.getMp3FileName();
}

string CNoteEditingToolDoc::getTitlePicture(void)
{
	return newNoteFile.getTitlePicture();
}

string CNoteEditingToolDoc::getNoteFileTitle(void)
{
	return newNoteFile.getNoteFileTitle();
}

string CNoteEditingToolDoc::getNoteFileArtist(void)
{
	return newNoteFile.getNoteFileArtist();
}

unsigned int CNoteEditingToolDoc::getNoteFileLevel(void)
{
	return newNoteFile.getNoteFileLevel();
}

unsigned int CNoteEditingToolDoc::getNoteFileInitBPM(void)
{
	return newNoteFile.getNoteFileInitBPM();
}


NoteTime CNoteEditingToolDoc::getStartSongTime(void)
{
	return newNoteFile.getStartSongTime();
}

NoteTime CNoteEditingToolDoc::getEndSongTime(void)
{
	return newNoteFile.getEndSongTime();
}


void CNoteEditingToolDoc::setNoteFileTitle(string noteFileTitle)
{
	newNoteFile.setNoteFileTitle(noteFileTitle);
}

void CNoteEditingToolDoc::setNoteFileArtist(string noteFileArtist)
{
	newNoteFile.setNoteFileArtist(noteFileArtist);
}

void CNoteEditingToolDoc::setNoteFileLevel(const unsigned int noteFileLevel)
{
	newNoteFile.setNoteFileLevel(noteFileLevel);
}

void CNoteEditingToolDoc::setMp3FileName(string mp3FileName)
{
	newNoteFile.setMp3FileName(mp3FileName);
}

void CNoteEditingToolDoc::setTitlePicture(string titlePicture)
{
	newNoteFile.setTitlePicture(titlePicture);
}

void CNoteEditingToolDoc::setStartSongTime(NoteTime startSongTime)
{
	newNoteFile.setStartSongTime(startSongTime);
}

void CNoteEditingToolDoc::setEndSongTime(NoteTime endSongTime)
{
	newNoteFile.setEndSongTime(endSongTime);
}

void CNoteEditingToolDoc::setNoteFileInitBPM(unsigned int noteFileInitBPM)
{
	newNoteFile.setNoteFileInitBPM(noteFileInitBPM);
}


//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [Virtual]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////



BOOL CNoteEditingToolDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	// TODO: 여기에 재초기화 코드를 추가합니다.
	clearAllNotes();

	// SDI 문서는 이 문서를 다시 사용합니다.

	return TRUE;
}




// CNoteEditingToolDoc serialization

void CNoteEditingToolDoc::Serialize(CArchive& ar)
{
	int pathEndIndex = MAX_PATH-1;
	int i=0;
	CString tempPath;


	if (ar.IsStoring())
	{
		// TODO: 여기에 저장 코드를 추가합니다.
		SaveFileHeader(ar);
		SaveFileBody(ar);

		ar.WriteString(_T("END"));



	}
	else
	{
		// TODO: 여기에 로딩 코드를 추가합니다.


		// 모든 클래스 삭제
		clearAllNotes();

		// 로드이므로 newFile이 아니다.
		isNewFileFlag = FALSE;

		// Header Read
		if( LoadFileHeader(ar) < 0 )
		{
			// 만약 헤더 부분에서 문제가 일어나서 못 읽어들일 경우,
			AfxMessageBox(_T("Header 로드 과정에서 문제가 발생했습니다!"));
			return;
		}

		// Body Read
		if ( LoadFileBody(ar) < 0 )
		{
			// 만약 헤더 부분에서 문제가 일어나서 못 읽어들일 경우,
			AfxMessageBox(_T("Body 로드 과정에서 문제가 발생했습니다!"));
		}

		// MNF파일의 경로만 추출
		tempPath = ar.m_strFileName;
		
		// 경로 부분을 찾아 낸 다음
		while ( tempPath[i] != NULL )
		{
			if (i > MAX_PATH)
			{
				AfxMessageBox(_T("파일 경로의 심각한 에러 발생!"));
				return;
			}

			if ( tempPath[i] == '\\' )
			{
				pathEndIndex = i;
			}
			i++;
		}

		if ( i <= 0 )
		{
			AfxMessageBox(_T("파일 경로가 존재하지 않습니다."));
		}

		// 경로 부분 까지만 복사해서 저장.
		mnfFilePath = "";					// 초기화 한 다음
		for ( i=0 ; i<=pathEndIndex ; i++ )
		{
			mnfFilePath += tempPath[i];
		}
		
		
	}
}



//////////////////////////////////////////////////////////////////////////////
//////////////////////////////     [Public]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////



// 입력받은 시간 주변에 적당한 노트가 있는 지 확인한다. 있으면 1, 없으면 0 리턴.
int CNoteEditingToolDoc::chkNoteTime(const NoteTime targetTime, const char noteType, const char targetMarker, CNoteData **notePointer)
{
	unsigned int i=0;
	int j=0;
	CNoteData *tmpNote = NULL;
	unsigned int tmpTimeInt = 0;
	unsigned int tmpEndTimeInt = 0;
	unsigned int compTimeInt = (targetTime.noteTimeSec * 1000) + targetTime.noteTimeMilSec;
	char tmpNoteType = 0;


	*notePointer = NULL;
	nowEditingNote = NULL;
	noteHeadDir = 0;

	
	for( i = 0 ; i < MAX_NOTE_ARRAY && i <= (targetTime.noteTimeSec / SECOND_PER_ARRAY) ; i++ )
	{
		// 0번째부터 적절한 시간대의 노트까지 확인한다.
		for ( j=0 ; j < MAX_HALF_ARRAY ; j++ )
		{
			tmpNote = newNoteFile.getNoteAddr(i, j);
			if( tmpNote == NULL )
			{
				// 만약 더 이상 빈 Array일 경우.
				// 혹시 모르니 그 전 Array에서 마지막 끝나는 노트의 End타임을 검사 해 본다.

				// 그래도 없으면 종료
				break;

				//return 0;
			}

			// 시간을 변수에 저장.
			tmpTimeInt = ( tmpNote->getNoteTimeSec() * 1000 ) + tmpNote->getNoteTimeMilSec();


			// 시간을 확인한다.
			if ( tmpTimeInt >= (compTimeInt - NOTE_COMP_RANGE1))
			{
				// 노트의 타입과 타겟마커가 맞는지 확인한다.
				if ( tmpTimeInt > (compTimeInt + NOTE_COMP_RANGE2) )
				{
					// 만약 정해진 범위보다 훨씬 크다면, 더 이상 해당하는 노트가 없기 때문에, 함수를 종료한다.
// 					*notePointer = NULL;
// 					nowEditingNote = NULL;
// 					noteHeadDir = 0;
// 					return 0;
					break;
				}
				else
				{
					// 노트 타입 확인
					if( noteType == chkTypeToBigType(tmpNote->getNoteType()))
					{
						if( tmpNote->getNoteType() == NOTE_T_DRAG ||
							tmpNote->getNoteType() == NOTE_T_CHARISMA ||
							tmpNote->getNoteType() == NOTE_T_NEUTRAL ||
							tmpNote->getNoteType() == NOTE_T_PATTERN ||
							tmpNote->getNoteType() == NOTE_T_BPM ||
							tmpNote->getNoteType() == NOTE_T_PHOTO
							)
						{
							// 만약 위의 노트들의 경우는, 타겟 마커가 의미가 없기 때문에 생략한다.
							*notePointer = tmpNote;
							nowEditingNote = tmpNote;
							noteHeadDir = 0;
							return 1;
						}
						else if( tmpNote->getTargetMarker() == targetMarker )
						{
							*notePointer = tmpNote;
							nowEditingNote = tmpNote;
							noteHeadDir = 0;
							return 1;
						}
					}
				}
			}


			// 노트의 끝나는 시간이 있는지 확인한다.
			tmpNoteType = tmpNote->getNoteType();
			if( tmpNoteType == NOTE_T_LONG ||
				tmpNoteType == NOTE_T_DRAG ||
				tmpNoteType == NOTE_T_CHARISMA ||
				tmpNoteType == NOTE_T_NEUTRAL ||
				tmpNoteType == NOTE_T_PATTERN
				)
			{
				tmpEndTimeInt = ( tmpNote->getNoteEndTimeSec() * 1000 ) + tmpNote->getNoteEndTimeMilSec();
				if( tmpEndTimeInt > (compTimeInt - NOTE_COMP_RANGE1) &&
					tmpEndTimeInt < (compTimeInt + NOTE_COMP_RANGE2)
					)
				{
					// 노트 타입 확인
					if( noteType == chkTypeToBigType(tmpNote->getNoteType()))
					{
						// 범위 안에 있을 경우, 마커가 같은지 확인.
						if( tmpNote->getNoteType() == NOTE_T_DRAG ||
							tmpNote->getNoteType() == NOTE_T_CHARISMA ||
							tmpNote->getNoteType() == NOTE_T_NEUTRAL ||
							tmpNote->getNoteType() == NOTE_T_PATTERN
							)
						{
							// 만약 위의 노트들의 경우는, 타겟 마커가 의미가 없기 때문에 생략한다.
							*notePointer = tmpNote;
							nowEditingNote = tmpNote;
							noteHeadDir = 1;
							return 1;
						}
						else if( tmpNote->getTargetMarker() == targetMarker )
						{
							*notePointer = tmpNote;
							nowEditingNote = tmpNote;
							noteHeadDir = 1;				// tail 이므로.
							return 1;
						}
					}
				}
			}
		}

	}

	// 여기까지 오면 없는 것
	//AfxMessageBox(_T("충돌 처리 함수에서 예외"));
	return 0;

}









//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [Get&Set]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////


// 현재 어떤 모드인가를 리턴하는 함수.
char CNoteEditingToolDoc::getNoteEditingMode(void)
{
	return noteEditingMode;
}

char CNoteEditingToolDoc::getNoteWriteType(void)
{
	return noteWriteType;
}

CNoteData* CNoteEditingToolDoc::getNowEditingNote(int &noteHeadDir)
{
	noteHeadDir = this->noteHeadDir;
	return nowEditingNote;
}

CNoteData* CNoteEditingToolDoc::getNowConfigingNote(int &noteConfigHeadDir)
{
	noteConfigHeadDir = this->noteConfigHeadDir;
	return nowConfigingNote;
}

bool CNoteEditingToolDoc::getOnFocusEditingFlag(void)
{
	return onFocusEditingFlag;
}

int CNoteEditingToolDoc::getNoteHeadDir(void)
{
	return noteHeadDir;
}

int CNoteEditingToolDoc::getNoteConfigHeadDir(void)
{
	return noteConfigHeadDir;
}

CString CNoteEditingToolDoc::getMnfFilePath(void)
{
	return mnfFilePath;
}

bool CNoteEditingToolDoc::getIsNewFileFlag(void)
{
	return isNewFileFlag;
}

CNoteEditingToolView* CNoteEditingToolDoc::getNoteEditingToolViewPtr(void)
{
	return this->noteEditingToolViewPtr;
}

CNotePickingView* CNoteEditingToolDoc::getNotePickingViewPtr(void)
{
	return this->notePickingViewPtr;
}


CEditModeSelectView* CNoteEditingToolDoc::getEditModeSelectViewPtr(void)
{
	return this->editModeSelectViewPtr;
}

/////////////// set 함수들



int CNoteEditingToolDoc::setNoteEditingMode(const char editingMode)
{
	switch(editingMode)
	{
	case EDIT_MODE_WRITE:
		noteEditingMode = EDIT_MODE_WRITE;

		// 포인터들의초기화
		nowEditingNote = NULL;
		noteHeadDir = 0;
		nowConfigingNote = NULL;
		noteConfigHeadDir = 0;
		 
		// 뷰의 갱신
		//notePickingViewPtr->DrawDoubleBuffering();
		//notePickingViewPtr->Invalidate();
		if ( notePickingViewPtr != NULL )
		{
			notePickingViewPtr->PostMessageW(CM_REDRAW_DC2);
		}
		

		return 1;
		break;

	case EDIT_MODE_MOVE:
		noteEditingMode = EDIT_MODE_MOVE;

		// 포인터들의초기화
		nowEditingNote = NULL;
		noteHeadDir = 0;
		nowConfigingNote = NULL;
		noteConfigHeadDir = 0;

		// 뷰의 갱신
		if ( notePickingViewPtr != NULL )
		{
			notePickingViewPtr->PostMessageW(CM_REDRAW_DC2);
		}

		return 1;
		break;

	case EDIT_MODE_ERASE:
		noteEditingMode = EDIT_MODE_ERASE;

		// 포인터들의초기화
		nowEditingNote = NULL;
		noteHeadDir = 0;
		nowConfigingNote = NULL;
		noteConfigHeadDir = 0;

		// 뷰의 갱신
		if ( notePickingViewPtr != NULL )
		{
			notePickingViewPtr->PostMessageW(CM_REDRAW_DC2);
		}
		return 1;
		break;

	case EDIT_MODE_CONFG:
		noteEditingMode = EDIT_MODE_CONFG;

		// 포인터들의초기화
		nowEditingNote = NULL;
		noteHeadDir = 0;
		nowConfigingNote = NULL;
		noteConfigHeadDir = 0;

		// 뷰의 갱신
		if ( notePickingViewPtr != NULL )
		{
			notePickingViewPtr->PostMessageW(CM_REDRAW_DC2);
		}
		return 1;
		break;

	}

	// 아무 해당사항 없으면 바꾸지 않고, 에러 리턴.
	return -1;
}

int CNoteEditingToolDoc::setNoteWriteType(const char writeType)
{
	switch( writeType )
	{
	case NOTE_T_RIGHT:
		noteWriteType = NOTE_T_RIGHT;
		return 1;
		break;

	case NOTE_T_LEFT:
		noteWriteType = NOTE_T_LEFT;
		return 1;
		break;

	case NOTE_T_LONG:
		noteWriteType = NOTE_T_LONG;
		return 1;
		break;


	case NOTE_T_PATTERN:
		noteWriteType = NOTE_T_PATTERN;
		return 1;
		break;

	case NOTE_T_CHARISMA:
		noteWriteType = NOTE_T_CHARISMA;
		return 1;
		break;

	case NOTE_T_NEUTRAL:
		noteWriteType = NOTE_T_NEUTRAL;
		return 1;
		break;

	case NOTE_T_BPM:
		noteWriteType = NOTE_T_BPM;
		return 1;
		break;


	case NOTE_T_DRAG:
		noteWriteType = NOTE_T_DRAG;
		return 1;
		break;

	case NOTE_T_PHOTO:
		noteWriteType = NOTE_T_PHOTO;
		return 1;
		break;
	}

	// 아무 해당사항 없으면 바꾸지 않고, 에러 리턴.
	return -1;
}


int CNoteEditingToolDoc::setNowEditingNote(CNoteData* const nowEditingNote,  const int noteHeadDir)
{
	this->noteHeadDir = noteHeadDir;
	this->nowEditingNote = nowEditingNote;
	return 1;
}

int CNoteEditingToolDoc::setNowConfigingNote(CNoteData* const nowConfigingNote,  const int noteConfigHeadDir)
{
	this->noteConfigHeadDir = noteConfigHeadDir;
	this->nowConfigingNote = nowConfigingNote;
	return 1;
}

int CNoteEditingToolDoc::setOnFocusEditingFlag(const bool onFocusEditingFlag)
{
	this->onFocusEditingFlag = onFocusEditingFlag;
	return 1;
}


int CNoteEditingToolDoc::setNoteEditingToolViewPtr(CNoteEditingToolView* noteEditingToolViewPtr)
{
	this->noteEditingToolViewPtr = noteEditingToolViewPtr;
	return 1;
}



int CNoteEditingToolDoc::setNotePickingViewPtr(CNotePickingView *notePickingViewPtr)
{
	this->notePickingViewPtr = notePickingViewPtr;
	return 1;
}

int CNoteEditingToolDoc::setEditModeSelectViewPtr(CEditModeSelectView *editModeSelectViewPtr)
{
	this->editModeSelectViewPtr = editModeSelectViewPtr;
	return 1;
}




#ifdef SHARED_HANDLERS

// 축소판 그림을 지원합니다.
void CNoteEditingToolDoc::OnDrawThumbnail(CDC& dc, LPRECT lprcBounds)
{
	// 문서의 데이터를 그리려면 이 코드를 수정하십시오.
	dc.FillSolidRect(lprcBounds, RGB(255, 255, 255));

	CString strText = _T("TODO: implement thumbnail drawing here");
	LOGFONT lf;

	CFont* pDefaultGUIFont = CFont::FromHandle((HFONT) GetStockObject(DEFAULT_GUI_FONT));
	pDefaultGUIFont->GetLogFont(&lf);
	lf.lfHeight = 36;

	CFont fontDraw;
	fontDraw.CreateFontIndirect(&lf);

	CFont* pOldFont = dc.SelectObject(&fontDraw);
	dc.DrawText(strText, lprcBounds, DT_CENTER | DT_WORDBREAK);
	dc.SelectObject(pOldFont);
}

// 검색 처리기를 지원합니다.
void CNoteEditingToolDoc::InitializeSearchContent()
{
	CString strSearchContent;
	// 문서의 데이터에서 검색 콘텐츠를 설정합니다.
	// 콘텐츠 부분은 ";"로 구분되어야 합니다.

	// 예: strSearchContent = _T("point;rectangle;circle;ole object;");
	SetSearchContent(strSearchContent);
}

void CNoteEditingToolDoc::SetSearchContent(const CString& value)
{
	if (value.IsEmpty())
	{
		RemoveChunk(PKEY_Search_Contents.fmtid, PKEY_Search_Contents.pid);
	}
	else
	{
		CMFCFilterChunkValueImpl *pChunk = NULL;
		ATLTRY(pChunk = new CMFCFilterChunkValueImpl);
		if (pChunk != NULL)
		{
			pChunk->SetTextValue(PKEY_Search_Contents, value, CHUNK_TEXT);
			SetChunkValue(pChunk);
		}
	}
}

#endif // SHARED_HANDLERS

// CNoteEditingToolDoc 진단

#ifdef _DEBUG
void CNoteEditingToolDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CNoteEditingToolDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG


// CNoteEditingToolDoc 명령


void CNoteEditingToolDoc::OnOpenNewPictureFile()
{
	// TODO: 여기에 명령 처리기 코드를 추가합니다.

	// 다이얼로그 초기화
	importPicDlg.m_PictureFilename = CString::CStringT( CA2CT(newNoteFile.getTitlePicture().c_str()) );

	// 다이얼로그 실행
	if( importPicDlg.DoModal() == IDOK )
	{
		// 업데이트
		newNoteFile.setTitlePicture( std::string(CT2CA(importPicDlg.m_PictureFilename.operator LPCWSTR())) );
	}


}
