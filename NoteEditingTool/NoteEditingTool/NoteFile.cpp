//#pragma once


#include "NoteFile.h"
#include "stdafx.h"




CNoteFile::CNoteFile(void)
{
	// 파일 생성시, 노트 파일 배열 초기화.
	for( int i=0 ; i<MAX_NOTE_ARRAY ; i++ )
	{
		for( int j=0 ; j<MAX_HALF_ARRAY ; j++ )
		{
			NoteAllList[i][j] = 0;
		}
	}

	// 기타 변수들 초기화
	this->noteFileVersion = 100;				// Version 1.00

	this->noteFileTitle = "Untitled";			// 제목
	this->noteFileArtist = "Unknown";			// 음악가

	this->noteFileLevel = 1;					// 기본 난이도는 항상 1

	this->mp3FileName = "";						// mp3파일 제목
	this->titlePicture = "";					// 타이틀 그림 경로

	// 시작 시간
	startSongTime.noteTimeSec = 0;
	startSongTime.noteTimeMilSec = 0;

	// 종료 시간 (임의로 2분으로 정함)
	endSongTime.noteTimeSec = 120;
	endSongTime.noteTimeMilSec = 0;


	this->noteFileInitBPM = 120;

	
}


CNoteFile::~CNoteFile(void)
{
	int i, j;

	// 메모리에 올려놨던 노트파일들 모두 삭제
	for( i=0 ; i< MAX_NOTE_ARRAY ; i++ )
	{
		for( j=0 ; j < MAX_HALF_ARRAY ; j++ )
		{
			delete NoteAllList[i][j];
		}
	}
}




//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [Private]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////


// 특정 주소에 적절한 노트를 생성해서 배열에 링크 시킨다.
int CNoteFile::makeNewNote(CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{	
	switch( noteType )
	{
	case NOTE_T_RIGHT:			// 오른손 노트
	case NOTE_T_LEFT:			// 왼손 노트
	case NOTE_T_BPM:			// bpm change
	case NOTE_T_PHOTO:			// 사진 노트
		// 1, 2, 3의 경우 같은 NoteData Class를 사용한다.
		// 이 부분은 함수를 따로 만들어서 작성 요망
		*targetAddr = new CNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	case NOTE_T_LONG:				// 롱 노트
	case NOTE_T_PATTERN:			// Pattern-Change
	case NOTE_T_CHARISMA:			// Charisma Time
	case NOTE_T_NEUTRAL:			// Nutral Position
		*targetAddr = new CLongNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	case NOTE_T_DRAG:			// Drag Note
		*targetAddr = new CDragNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	default:
		// 여기까지 해당사항이 없으면 에러 출력
		cout << "Error! Non-matching Note type!" << endl;
		return -1;
	}

	// 정상 종료.
	return 1;
}


// 특정 주소에 적절한 노트를 생성해서 배열에 링크 시킨다.
int CNoteFile::makeNewNote(CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
	const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec)
{	
	switch( noteType )
	{
	case NOTE_T_RIGHT:			// 오른손 노트
	case NOTE_T_LEFT:			// 왼손 노트
	case NOTE_T_BPM:			// bpm change
	case NOTE_T_PHOTO:			// 사진 노트
		// 1, 2, 3의 경우 같은 NoteData Class를 사용한다.
		// 이 부분은 함수를 따로 만들어서 작성 요망
		*targetAddr = new CNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	case NOTE_T_LONG:				// 롱 노트
	case NOTE_T_PATTERN:			// Pattern-Change
	case NOTE_T_CHARISMA:			// Charisma Time
	case NOTE_T_NEUTRAL:			// Nutral Position
		*targetAddr = new CLongNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker, noteEndTimeSec, noteEndTimeMilSec);
		break;

	case NOTE_T_DRAG:			// Drag Note
		*targetAddr = new CDragNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	default:
		// 여기까지 해당사항이 없으면 에러 출력
		cout << "Error! Non-matching Note type!" << endl;
		return -1;
	}

	// 정상 종료.
	return 1;
}


// 특정 주소에 적절한 노트를 생성해서 배열에 링크 시킨다.
int CNoteFile::makeNewNote(CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
	const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec,
	dotData point1, dotData point2, dotData point3, dotData point4)
{	
	switch( noteType )
	{
	case NOTE_T_RIGHT:			// 오른손 노트
	case NOTE_T_LEFT:			// 왼손 노트
	case NOTE_T_BPM:			// bpm change
	case NOTE_T_PHOTO:			// 사진 노트
		// 1, 2, 3의 경우 같은 NoteData Class를 사용한다.
		// 이 부분은 함수를 따로 만들어서 작성 요망
		*targetAddr = new CNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	case NOTE_T_LONG:				// 롱 노트
	case NOTE_T_PATTERN:			// Pattern-Change
	case NOTE_T_CHARISMA:			// Charisma Time
	case NOTE_T_NEUTRAL:			// Nutral Position
		*targetAddr = new CLongNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker, noteEndTimeSec, noteEndTimeMilSec);
		break;

	case NOTE_T_DRAG:			// Drag Note
		*targetAddr = new CDragNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker, noteEndTimeSec, noteEndTimeMilSec, point1, point2, point3, point4);
		break;

	default:
		// 여기까지 해당사항이 없으면 에러 출력
		cout << "Error! Non-matching Note type!" << endl;
		return -1;
	}

	// 정상 종료.
	return 1;
}


// 새로 만들 노트 형식이 올바른 형식인가를 체크하는 함수.
int CNoteFile::checkNoteFormat(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	// 1. Second 체크
	// 없다...

	// 2. mili-Second 체크
	// 0~999 사이의 정수인지 확인
	if( noteTimeMilSec >= 1000 )
	{
		return -1;
	}

	// 3. noteType 체크
	switch( noteType )
	{
		// 4. 각각의 타입에 따른 targetMarker 체크
	case NOTE_T_RIGHT:			// 오른손 노트
	case NOTE_T_LEFT:			// 왼손 노트
		switch( targetMarker )
		{
		case MARKER_NUM1:
		case MARKER_NUM2:
		case MARKER_NUM3:
		case MARKER_NUM4:
		case MARKER_NUM5:
		case MARKER_NUM6:
			break;

		default:
			return -1;
		}
		break;


	// BPM change
	case NOTE_T_BPM:			// bpm change
		break;

	// Photo Time
	case NOTE_T_PHOTO:
		break;


	// Long Note Class를 사용하는 노트들.
	case NOTE_T_LONG:				// 롱 노트
	case NOTE_T_PATTERN:			// Pattern-Change
	case NOTE_T_CHARISMA:			// Charisma Time
	case NOTE_T_NEUTRAL:			// Nutral Position


		break;

	// Drag Note Class를 사용하는 노트
	case NOTE_T_DRAG:
		
		break;

	default:
		return -1;
	}


	// 위 체크들을 무사히 마쳤을 경우
	return 1;
}
	

// 수정 할 노트 형식이 올바른 형식인가를 체크하는 함수.
int CNoteFile::checkNoteFormat(
	const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	int &targetArrayNum, CNoteData** notePointer
	)
{
	// 먼저 수정하려고 하는 값이 올바른 값인지 확인 한 다음,
	if( checkNoteFormat(editNoteTimeSec, editNoteTimeMilSec, editNoteType, editTargetMarker) < 0 )
	{
		// 올바른 값이 아닐 경우,
		cout << "Error! not correct edit Value!" << endl;
		return -1;
	}


	// 원래 존재하는 값을 가지고 있는 노트의 주소를 알아온다.
	*notePointer = findNotePointer(orgNoteTimeSec, orgNoteTimeMilSec, orgNoteType, orgTargetMarker, targetArrayNum);
	if( (*notePointer) == NULL || targetArrayNum < 0 )
	{
		// 해당 노트를 찾지 못했을 경우
		cout << "There has no 해당 note!" << endl;
		return -2;
	}


	// 롱노트->일반노트, 혹은 그 반대로 변하는지 확인하는 작업
	if( classTypeCheck(orgNoteType) != classTypeCheck(editNoteType) )
	{
		cout << "Error! Note Class Type Error!" << endl;
		return -8;
	}

	// 모든 검사들을 무사히 잘 통과했을 경우.
	return 1;
}


int CNoteFile::checkNoteFormat(
	CNoteData *notePointer,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker, int &targetArrayNum)
{
	// 먼저 수정하려고 하는 값이 올바른 값인지 확인 한 다음,
	if( checkNoteFormat(editNoteTimeSec, editNoteTimeMilSec, editNoteType, editTargetMarker) < 0 )
	{
		// 올바른 값이 아닐 경우,
		cout << "Error! not correct edit Value!" << endl;
		return -1;
	}


	// 원래 존재하는 값을 가지고 있는 노트의 주소를 알아온다.
	findNotePointer(notePointer->getNoteTimeSec(), notePointer->getNoteTimeMilSec(), notePointer->getNoteType(), notePointer->getTargetMarker(), targetArrayNum);
	if( notePointer == NULL )
	{
		// 해당 노트를 찾지 못했을 경우
		cout << "There has no 해당 note!" << endl;
		return -2;
	}


	// 롱노트->일반노트, 혹은 그 반대로 변하는지 확인하는 작업
	if( classTypeCheck( notePointer->getNoteType() ) != classTypeCheck(editNoteType) )
	{
		cout << "Error! Note Class Type Error!" << endl;
		return -8;
	}

	// 모든 검사들을 무사히 잘 통과했을 경우.
	return 1;
}



// 수정 할 드래그 노트 노트 형식이 올바른 형식인가를 체크하는 함수.
int CNoteFile::checkNoteFormat(
	const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
	const unsigned int editPointX, const unsigned int editPointY,
	int &targetArrayNum, CNoteData** notePointer)
{
	/*
	// 먼저 수정하려고 하는 값이 올바른 값인지 확인 한 다음,
	if( checkNoteFormat(editNoteTimeSec, editNoteTimeMilSec, editNoteType, editTargetMarker) < 0 )
	{
		// 올바른 값이 아닐 경우,
		cout << "Error! not correct edit Value!" << endl;
		return -1;
	}
	*/

	// 원래 존재하는 값을 가지고 있는 노트의 주소를 알아온다.
	*notePointer = findNotePointer(orgNoteTimeSec, orgNoteTimeMilSec, NOTE_T_DRAG, targetArrayNum);
	if( (*notePointer) == NULL || targetArrayNum < 0 )
	{
		// 해당 노트를 찾지 못했을 경우
		cout << "There has no 해당 note!" << endl;
		return -2;
	}

	 //드래그 노트가 맞는지 확인한다.
	if( (*notePointer)->getNoteType() != NOTE_T_DRAG )
	{
		cout << "Error! Target Note is not a Drag Note" << endl;
		return -9;
	}
	

	// 모든 검사들을 무사히 잘 통과했을 경우.
	return 1;
}



// 특정 값이 일치하는 노트가 있는지 찾는 함수
CNoteData* CNoteFile::findNotePointer(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker, int &targetArrayNum)
{
	// 필요한 지역변수 선언
	int i=0;
	unsigned int noteArrayNumber = noteTimeSec / SECOND_PER_ARRAY;		// 어느 Array에 넣어야 할 지 결정.


	// 배열을 탐색하면서 모든 정보가 일치하는 노트가 있다면 그 노트의 주소 출력.
	for( i=0 ; (NoteAllList[noteArrayNumber][i] != NULL) && (i < MAX_HALF_ARRAY) ; i++ )
	{
		if( NoteAllList[noteArrayNumber][i]->compareNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker) == true )
		{
			targetArrayNum = i;							// 해당 위치를 넘겨준다.
			return  NoteAllList[noteArrayNumber][i];
		}
	}
	// 만약 배열을 전부 탐색 할 때까지 일치하는 노트를 발견하지 못했다면, NULL 값 리턴



	targetArrayNum = -1;
	return NULL;
}



// 특정 값이 일치하는 노트가 있는지 찾는 함수
CNoteData* CNoteFile::findNotePointer(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, int &targetArrayNum)
{
	// 필요한 지역변수 선언
	int i=0;
	unsigned int noteArrayNumber = noteTimeSec / SECOND_PER_ARRAY;		// 어느 Array에 넣어야 할 지 결정.


	// 배열을 탐색하면서 모든 정보가 일치하는 노트가 있다면 그 노트의 주소 출력.
	for( i=0 ; (NoteAllList[noteArrayNumber][i] != NULL) && (i < MAX_HALF_ARRAY) ; i++ )
	{
		if( NoteAllList[noteArrayNumber][i]->compareNoteData(noteTimeSec, noteTimeMilSec, noteType) == true )
		{
			targetArrayNum = i;							// 해당 위치를 넘겨준다.
			return  NoteAllList[noteArrayNumber][i];
		}
	}
	// 만약 배열을 전부 탐색 할 때까지 일치하는 노트를 발견하지 못했다면, NULL 값 리턴



	targetArrayNum = -1;
	return NULL;
}

CNoteData* CNoteFile::findNotePointer(CNoteData *targetNote, int &targetArrayNum)
{
	// 필요한 지역변수 선언
	int i=0;
	unsigned int noteArrayNumber = targetNote->getNoteTimeSec() / SECOND_PER_ARRAY;		// 어느 Array에 넣어야 할 지 결정.


	// 배열을 탐색하면서 모든 정보가 일치하는 노트가 있다면 그 노트의 주소 출력.
	for( i=0 ; (NoteAllList[noteArrayNumber][i] != NULL) && (i < MAX_HALF_ARRAY) ; i++ )
	{
		if( NoteAllList[noteArrayNumber][i]->compareNoteData(targetNote->getNoteTimeSec(), targetNote->getNoteTimeMilSec(), targetNote->getNoteType(), targetNote->getTargetMarker()) == true )
		{
			targetArrayNum = i;							// 해당 위치를 넘겨준다.
			return  NoteAllList[noteArrayNumber][i];
		}
	}
	// 만약 배열을 전부 탐색 할 때까지 일치하는 노트를 발견하지 못했다면, NULL 값 리턴



	targetArrayNum = -1;
	return NULL;
}

// 해당하는 노트 클래스를 적절한 위치에 끼워넣는 함수. (정상적으로 이미 존재하는 노트 객체라는 가정)
int CNoteFile::addNoteObject(CNoteData* const targetNote)
{
	// 변수 선언
	int j=0;
	
	const unsigned int noteTimeSec = targetNote->getNoteTimeSec();
	const unsigned int noteTimeMilSec = targetNote->getNoteTimeMilSec();

	const unsigned int noteArrayNumber = targetNote->getNoteTimeSec() / SECOND_PER_ARRAY;		// 어느 Array에 넣어야 할 지 결정.
	int arrayAddIndex = 0;				// 배열 중간의 어떤 칸에 입력 할 지 결정하기 위해 필요 한 인덱스.
	


	// MilSec의 조정
	// noteTimeMilSec %= 1000; 필요없을 듯?


	// 노트 형식이 맞는지 확인한다.
	//if( checkNoteFormat(noteTimeSec, noteTimeMilSec, noteType, targetMarker) < 0 )
	//{
	//	cout << "Error! Not Profit for Note Format!" << endl;
	//	return -1;
	//}


	// 현재 입력하a려는 시간보다 더 작은 값이 이미 존재한다면, 어디까지 존재하는지 확인,
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// 빈 공간이 아닌지 먼저 확인
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() < noteTimeSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
			// -1을 한 이유는 새로 추가 할 공간까지 고려 한 것이다.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}
	
	// 초는 같으나 밀리초가 다를 수 있으므로 밀리초 검사를 한번 더 한다.
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// 빈 공간이 아닌지 먼저 확인
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() == noteTimeSec	 &&			// 빈 공간이 아닐 경우, 크기 비교
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeMilSec() < noteTimeMilSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
			// -1을 한 이유는 새로 추가 할 공간까지 고려 한 것이다.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}


	// 더 큰 값들을 한 칸씩 밀어낸다.
	// 먼저 배열의 끝을 알아 낸 다음,
	j=arrayAddIndex;					// 여기서부터 시작.

	if( NoteAllList[noteArrayNumber][j] == NULL )
	{
		// 만약 맨 끝에 추가해야 하는 상황이 생겼을 경우.


	}
	else
	{
		// 중간에 끼워 넣어야 할 상황이 생겼을 경우.

		while( NoteAllList[noteArrayNumber][j] != NULL )
		{
			j++;
			if( j >= MAX_HALF_ARRAY )
			{
				// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
				cout << "Error! not enough array size 2!" << endl;
				return -1;
			}
		}

		// 끝에서부터 arrayAddIndex까지 차근차근 밀어낸다.
		do
		{
			NoteAllList[noteArrayNumber][j] = NoteAllList[noteArrayNumber][j-1];
			j--;
		}
		while( j > arrayAddIndex );
	}

	// arrayAddIndex가 가리키는 공간에 노트 클래스 끼워넣기
	NoteAllList[noteArrayNumber][arrayAddIndex] = targetNote;

	// 무사 종료.
	return 1;

}



// 타입을 입력하면 어떤 노트 클래스를 사용하는 지 리턴하는 함수
char CNoteFile::classTypeCheck(const char noteType)
{
	switch( noteType )
	{
	case NOTE_T_RIGHT:
	case NOTE_T_LEFT:
	case NOTE_T_BPM:
	case NOTE_T_PHOTO:
		return 'N';
		break;

	case NOTE_T_LONG:
	case NOTE_T_CHARISMA:
	case NOTE_T_NEUTRAL:
	case NOTE_T_PATTERN:
		return 'L';
		break;

	case NOTE_T_DRAG:
		return 'D';
		break;

	default:
		return '?';			// 에러
	}

	// 여기는 올 일이 없다.
	return '?';
}




//////////////////////////////////////////////////////////////////////////////
//////////////////////////////     [Public]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////


// 가지고 있는 모든 노트의 데이터를 출력하는 함수. (-1은 전부 출력)
int CNoteFile::printAllNotesData(const int noteArrayNumber)
{	
	cout << "Note file data" << endl;

	if( noteArrayNumber < 0 )
	{
		// 만약 음수일 경우 전체 출력
		for( int i=0 ; i<MAX_NOTE_ARRAY ; i++ )
		{
			cout << "Array[" << i << "]" << endl;
			for( int j=0 ; j<MAX_HALF_ARRAY && NoteAllList[i][j] != 0 ; j++ )
			{	// 노트 정보가 들어있지 않거나, 마지막 리스트 정보까지 갔을 경우 출력 종료.
				NoteAllList[i][j]->printNoteData();
			}
			cout << endl;
		}
		
		// 전체 출력 후, 정상 종료.
		return 0;
	}
	else if( noteArrayNumber >= MAX_NOTE_ARRAY )
	{
		// 음수는 아니지만, 지정한 노트 배열의 개수보다 클 경우, 그냥 출력하지 않고 종료.

		cout << "Please input right number Parameter!" << endl;
		
		// 비정상 종료
		return -1;
	}
	else
	{
		// 해당 노트 배열의 모든 노트 출력.

		for( int j=0 ; j<MAX_HALF_ARRAY && NoteAllList[noteArrayNumber][j] != 0 ; j++ )
		{	// 노트 정보가 들어있지 않거나, 마지막 리스트 정보까지 갔을 경우 출력 종료.
			NoteAllList[noteArrayNumber][j]->printNoteData();
		}

		// 정상 종료.
		return 0;
	}


	// 치명적 비정상 종료.
	return -1;	
}


// 노트를 하나 배열의 마지막에 그냥 추가하는 함수.
int CNoteFile::addNewNoteBrute(const int noteArrayNumber, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	int j=0;

	// 빈 공간이 나타 날 때까지 Loop
	while( NoteAllList[noteArrayNumber][j] != NULL )
	{
		j++;
		
		if( j >= MAX_HALF_ARRAY )
		{
			// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}

	// 빈 공간에 노트 클래스 추가
	if( makeNewNote(&NoteAllList[noteArrayNumber][j], noteTimeSec, noteTimeMilSec, noteType, targetMarker) <= 0 )
	{
		// 에러 체크
		cout << "Note making Error!" << endl;
		return -1;
	}

	// 무사 종료.
	return 1;
}


// 노트를 하나 적절한 배열에 추가하는 함수.
int CNoteFile::addNewNote(const unsigned int noteTimeSec, unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	// 변수 선언
	int j=0;
	unsigned int noteArrayNumber = noteTimeSec / SECOND_PER_ARRAY;		// 어느 Array에 넣어야 할 지 결정.
	int arrayAddIndex = 0;				// 배열 중간의 어떤 칸에 입력 할 지 결정하기 위해 필요 한 인덱스.

	// MilSec의 조정
	noteTimeMilSec %= 1000;


	// 노트 형식이 맞는지 확인한다.
	if( checkNoteFormat(noteTimeSec, noteTimeMilSec, noteType, targetMarker) < 0 )
	{
		cout << "Error! Not Profit for Note Format!" << endl;
		return -1;
	}


	// 현재 입력하려는 시간보다 더 작은 값이 이미 존재한다면, 어디까지 존재하는지 확인,
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// 빈 공간이 아닌지 먼저 확인
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() < noteTimeSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
			// -1을 한 이유는 새로 추가 할 공간까지 고려 한 것이다.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}
	
	// 초는 같으나 밀리초가 다를 수 있으므로 밀리초 검사를 한번 더 한다.
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// 빈 공간이 아닌지 먼저 확인
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() == noteTimeSec	 &&			// 빈 공간이 아닐 경우, 크기 비교
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeMilSec() < noteTimeMilSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
			// -1을 한 이유는 새로 추가 할 공간까지 고려 한 것이다.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}


	// 더 큰 값들을 한 칸씩 밀어낸다.
	// 먼저 배열의 끝을 알아 낸 다음,
	j=arrayAddIndex;					// 여기서부터 시작.

	if( NoteAllList[noteArrayNumber][j] == NULL )
	{
		// 만약 맨 끝에 추가해야 하는 상황이 생겼을 경우.


	}
	else
	{
		// 중간에 끼워 넣어야 할 상황이 생겼을 경우.

		while( NoteAllList[noteArrayNumber][j] != NULL )
		{
			j++;

			if( j >= MAX_HALF_ARRAY )
			{
				// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
				cout << "Error! not enough array size 2!" << endl;
				return -1;
			}
		}

		// 끝에서부터 arrayAddIndex까지 차근차근 밀어낸다.
		do
		{
			NoteAllList[noteArrayNumber][j] = NoteAllList[noteArrayNumber][j-1];
			j--;
		}
		while( j > arrayAddIndex );

	}

	// arrayAddIndex가 가리키는 공간에 노트 클래스 추가
	if( makeNewNote(&NoteAllList[noteArrayNumber][arrayAddIndex], noteTimeSec, noteTimeMilSec, noteType, targetMarker) <= 0 )
	{
		// 에러 체크
		cout << "Note making Error!" << endl;
		return -1;
	}


	// 무사 종료.
	return 1;
}


// 노트를 하나 적절한 배열에 추가하는 함수.
int CNoteFile::addNewNote(const unsigned int noteTimeSec, unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
	const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec)
{
	// 변수 선언
	int j=0;
	unsigned int noteArrayNumber = noteTimeSec / SECOND_PER_ARRAY;		// 어느 Array에 넣어야 할 지 결정.
	int arrayAddIndex = 0;				// 배열 중간의 어떤 칸에 입력 할 지 결정하기 위해 필요 한 인덱스.

	// MilSec의 조정
	noteTimeMilSec %= 1000;


	// 노트 형식이 맞는지 확인한다.
	if( checkNoteFormat(noteTimeSec, noteTimeMilSec, noteType, targetMarker) < 0 )
	{
		cout << "Error! Not Profit for Note Format!" << endl;
		return -1;
	}


	// 현재 입력하려는 시간보다 더 작은 값이 이미 존재한다면, 어디까지 존재하는지 확인,
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// 빈 공간이 아닌지 먼저 확인
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() < noteTimeSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
			// -1을 한 이유는 새로 추가 할 공간까지 고려 한 것이다.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}

	// 초는 같으나 밀리초가 다를 수 있으므로 밀리초 검사를 한번 더 한다.
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// 빈 공간이 아닌지 먼저 확인
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() == noteTimeSec	 &&			// 빈 공간이 아닐 경우, 크기 비교
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeMilSec() < noteTimeMilSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
			// -1을 한 이유는 새로 추가 할 공간까지 고려 한 것이다.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}


	// 더 큰 값들을 한 칸씩 밀어낸다.
	// 먼저 배열의 끝을 알아 낸 다음,
	j=arrayAddIndex;					// 여기서부터 시작.

	if( NoteAllList[noteArrayNumber][j] == NULL )
	{
		// 만약 맨 끝에 추가해야 하는 상황이 생겼을 경우.


	}
	else
	{
		// 중간에 끼워 넣어야 할 상황이 생겼을 경우.

		while( NoteAllList[noteArrayNumber][j] != NULL )
		{
			j++;

			if( j >= MAX_HALF_ARRAY )
			{
				// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
				cout << "Error! not enough array size 2!" << endl;
				return -1;
			}
		}

		// 끝에서부터 arrayAddIndex까지 차근차근 밀어낸다.
		do
		{
			NoteAllList[noteArrayNumber][j] = NoteAllList[noteArrayNumber][j-1];
			j--;
		}
		while( j > arrayAddIndex );

	}

	// arrayAddIndex가 가리키는 공간에 노트 클래스 추가
	if( makeNewNote(&NoteAllList[noteArrayNumber][arrayAddIndex], noteTimeSec, noteTimeMilSec, noteType, targetMarker, noteEndTimeSec, noteEndTimeMilSec) <= 0 )
	{
		// 에러 체크
		cout << "Note making Error!" << endl;
		return -1;
	}


	// 무사 종료.
	return 1;
}

// 노트를 하나 적절한 배열에 추가하는 함수.
int CNoteFile::addNewNote(const unsigned int noteTimeSec, unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
	const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec,
	dotData point1, dotData point2, dotData point3, dotData point4 )
{
	// 변수 선언
	int j=0;
	unsigned int noteArrayNumber = noteTimeSec / SECOND_PER_ARRAY;		// 어느 Array에 넣어야 할 지 결정.
	int arrayAddIndex = 0;				// 배열 중간의 어떤 칸에 입력 할 지 결정하기 위해 필요 한 인덱스.

	// MilSec의 조정
	noteTimeMilSec %= 1000;


	// 노트 형식이 맞는지 확인한다.
	if( checkNoteFormat(noteTimeSec, noteTimeMilSec, noteType, targetMarker) < 0 )
	{
		cout << "Error! Not Profit for Note Format!" << endl;
		return -1;
	}


	// 현재 입력하려는 시간보다 더 작은 값이 이미 존재한다면, 어디까지 존재하는지 확인,
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// 빈 공간이 아닌지 먼저 확인
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() < noteTimeSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
			// -1을 한 이유는 새로 추가 할 공간까지 고려 한 것이다.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}

	// 초는 같으나 밀리초가 다를 수 있으므로 밀리초 검사를 한번 더 한다.
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// 빈 공간이 아닌지 먼저 확인
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() == noteTimeSec	 &&			// 빈 공간이 아닐 경우, 크기 비교
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeMilSec() < noteTimeMilSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
			// -1을 한 이유는 새로 추가 할 공간까지 고려 한 것이다.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}


	// 더 큰 값들을 한 칸씩 밀어낸다.
	// 먼저 배열의 끝을 알아 낸 다음,
	j=arrayAddIndex;					// 여기서부터 시작.

	if( NoteAllList[noteArrayNumber][j] == NULL )
	{
		// 만약 맨 끝에 추가해야 하는 상황이 생겼을 경우.


	}
	else
	{
		// 중간에 끼워 넣어야 할 상황이 생겼을 경우.

		while( NoteAllList[noteArrayNumber][j] != NULL )
		{
			j++;

			if( j >= MAX_HALF_ARRAY )
			{
				// 만약 저장공간이 전부 꽉 찼을 경우, 에러 출력.
				cout << "Error! not enough array size 2!" << endl;
				return -1;
			}
		}

		// 끝에서부터 arrayAddIndex까지 차근차근 밀어낸다.
		do
		{
			NoteAllList[noteArrayNumber][j] = NoteAllList[noteArrayNumber][j-1];
			j--;
		}
		while( j > arrayAddIndex );

	}

	// arrayAddIndex가 가리키는 공간에 노트 클래스 추가
	if( makeNewNote(&NoteAllList[noteArrayNumber][arrayAddIndex], noteTimeSec, noteTimeMilSec, noteType, targetMarker, noteEndTimeSec, noteEndTimeMilSec, point1, point2, point3, point4) <= 0 )
	{
		// 에러 체크
		cout << "Note making Error!" << endl;
		return -1;
	}


	// 무사 종료.
	return 1;
}



//입력받은 노트를 수정하는 함수.
int CNoteFile::editNote(
	const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker
	)
{
	// 필요한 지역변수 선언
	CNoteData* notePointer = NULL;
	// 밀리초 단위 변환
	unsigned int orgNoteTimeMilSec2 = orgNoteTimeMilSec % 1000;
	unsigned int editNoteTimeMilSec2 = editNoteTimeMilSec % 1000;
	int targetArrayNum = -1;

	// 먼저 수정하려고 하는 값이 올바른 값인지 확인 한 다음, 원래 존재하는 값을 가지고 있는 노트의 주소를 알아내고, 롱노트->일반노트, 혹은 그 반대로 변하는지 확인하는 작업
	int tempChkFlag = checkNoteFormat(orgNoteTimeSec, orgNoteTimeMilSec2, orgNoteType, orgTargetMarker, editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, targetArrayNum, &notePointer);
	if( tempChkFlag < 0 )
	{
		return tempChkFlag;
	}

	// 그 노트의 값을 실질적으로 변경.
	if( notePointer->editNoteData(editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker) < 0 )
	{
		// 만약 일반노트->롱노트로 바꾸려고 했다면,
		cout << "Error! Invalid note Type Change!" << endl;
		return -8;
	}




	// 일단 해당 배열에서 제외 한 다음,
	int i = targetArrayNum+1;
	unsigned int editNoteArrayNumber = editNoteTimeSec / SECOND_PER_ARRAY;		// 이제 어느 Array에 넣어야 할 지 결정.
	unsigned int orgNoteArrayNumber = orgNoteTimeSec / SECOND_PER_ARRAY;		// 원래 어떤 Array에 있었는지,

	// 해당 배열에서 제외
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // 노트가 맨 끝에 있었을 수 있기 때문에 일단 NULL을 넣어 준다.

	// 그 이후의 배열의 내용을 한 칸씩 당겨온다.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// 마무리. 마지막에 i++을 했기 때문에 i-1을 입력한다.

	// 혹시 배열의 최대치를 넘어갔는지 확인
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// 다시 추가한다.
	if( addNoteObject(notePointer) < 0 )
	{
		cout << "Error! 끼워넣기 error" << endl;
		return -4;
	}


	return 0;
}


int CNoteFile::editNote(CNoteData *targetNote,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker)
{
		// 필요한 지역변수 선언
	unsigned int orgNoteArrayNumber = targetNote->getNoteTimeSec() / SECOND_PER_ARRAY;		// 원래 어떤 Array에 있었는지,


	//CNoteData* notePointer;
	// 밀리초 단위 변환
	unsigned int orgNoteTimeMilSec2 = targetNote->getNoteTimeMilSec() % 1000;
	unsigned int editNoteTimeMilSec2 = editNoteTimeMilSec % 1000;
	int targetArrayNum = -1;


	// 먼저 수정하려고 하는 값이 올바른 값인지 확인 한 다음, 원래 존재하는 값을 가지고 있는 노트의 주소를 알아내고, 롱노트->일반노트, 혹은 그 반대로 변하는지 확인하는 작업
	int tempChkFlag = checkNoteFormat(targetNote, editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, targetArrayNum);
	if( tempChkFlag < 0 )
	{
		return tempChkFlag;
	}


	// 그 노트의 값을 실질적으로 변경.
	if( targetNote->editNoteData(editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker) < 0 )
	{
		// 만약 일반노트 -> 롱노트 로 바꾸려고 했다면,
		// 이 구문 굳이 필요 한 부분인가???
		AfxMessageBox(_T("Error! Invalid note Type Change!"));
		return -8;
	}


	// 일단 해당 배열에서 제외 한 다음,
	int i = targetArrayNum+1;
	unsigned int editNoteArrayNumber = editNoteTimeSec / SECOND_PER_ARRAY;		// 이제 어느 Array에 넣어야 할 지 결정.

	// 해당 배열에서 제외
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // 노트가 맨 끝에 있었을 수 있기 때문에 일단 NULL을 넣어 준다.

	// 그 이후의 배열의 내용을 한 칸씩 당겨온다.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// 마무리. 마지막에 i++을 했기 때문에 i-1을 입력한다.

	// 혹시 배열의 최대치를 넘어갔는지 확인
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// 다시 추가한다.
	if( addNoteObject(targetNote) < 0 )
	{
		cout << "Error! 끼워넣기 error" << endl;
		return -4;
	}


	return 0;
}




int CNoteFile::editLongNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	// 필요한 지역변수 선언
	CNoteData* notePointer;
	// 밀리초 단위 변환
	unsigned int orgNoteTimeMilSec2 = orgNoteTimeMilSec % 1000;
	unsigned int editNoteTimeMilSec2 = editNoteTimeMilSec % 1000;
	int targetArrayNum = -1;

	/*
	// 먼저 수정하려고 하는 값이 올바른 값인지 확인 한 다음,
	if( checkNoteFormat(orgNoteTimeSec, orgNoteTimeMilSec2, orgNoteType, orgTargetMarker) < 0 )
	{
		// 올바른 값이 아닐 경우,
		cout << "Error! not correct edit Value!" << endl;
		return -1;
	}


	// 원래 존재하는 값을 가지고 있는 노트의 주소를 알아온다.
	notePointer = findNotePointer(orgNoteTimeSec, orgNoteTimeMilSec2, orgNoteType, orgTargetMarker, targetArrayNum);
	if( notePointer == NULL || targetArrayNum < 0 )
	{
		// 해당 노트를 찾지 못했을 경우
		cout << "There has no 해당 note!" << endl;
		return -2;
	}


	// 롱노트->일반노트, 혹은 그 반대로 변하는지 확인하는 작업
	if( classTypeCheck(orgNoteType) != classTypeCheck(editNoteType) )
	{
		cout << "Error! Note Class Type Error!" << endl;
		return -8;
	}
	*/

	// 먼저 수정하려고 하는 값이 올바른 값인지 확인 한 다음, 원래 존재하는 값을 가지고 있는 노트의 주소를 알아내고, 롱노트->일반노트, 혹은 그 반대로 변하는지 확인하는 작업
	int tempChkFlag = checkNoteFormat(orgNoteTimeSec, orgNoteTimeMilSec2, orgNoteType, orgTargetMarker, editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, targetArrayNum, &notePointer);
	if( tempChkFlag < 0 )
	{
		return tempChkFlag;
	}


	// 그 노트의 값을 실질적으로 변경.
	if( notePointer->editNoteData(editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, editNoteEndTimeSec, editNoteEndTimeMilSec) < 0 )
	{
		// 만약 일반노트 -> 롱노트 로 바꾸려고 했다면,
		// 이 구문 굳이 필요 한 부분인가???
		cout << "Error! Invalid note Type Change!" << endl;
		return -8;
	}


	// 일단 해당 배열에서 제외 한 다음,
	int i = targetArrayNum+1;
	unsigned int editNoteArrayNumber = editNoteTimeSec / SECOND_PER_ARRAY;		// 이제 어느 Array에 넣어야 할 지 결정.
	unsigned int orgNoteArrayNumber = orgNoteTimeSec / SECOND_PER_ARRAY;		// 원래 어떤 Array에 있었는지,

	// 해당 배열에서 제외
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // 노트가 맨 끝에 있었을 수 있기 때문에 일단 NULL을 넣어 준다.

	// 그 이후의 배열의 내용을 한 칸씩 당겨온다.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// 마무리. 마지막에 i++을 했기 때문에 i-1을 입력한다.

	// 혹시 배열의 최대치를 넘어갔는지 확인
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// 다시 추가한다.
	if( addNoteObject(notePointer) < 0 )
	{
		cout << "Error! 끼워넣기 error" << endl;
		return -4;
	}


	return 0;
}


int CNoteFile::editLongNote(CNoteData *targetNote,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	
	// 필요한 지역변수 선언
	//CNoteData* notePointer;
	// 밀리초 단위 변환
	unsigned int orgNoteTimeMilSec2 = targetNote->getNoteTimeMilSec() % 1000;
	unsigned int editNoteTimeMilSec2 = editNoteTimeMilSec % 1000;
	int targetArrayNum = -1;
	
	// 먼저 수정하려고 하는 값이 올바른 값인지 확인 한 다음, 원래 존재하는 값을 가지고 있는 노트의 주소를 알아내고, 롱노트->일반노트, 혹은 그 반대로 변하는지 확인하는 작업
	int tempChkFlag = checkNoteFormat(targetNote,
		editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, targetArrayNum);
	if( tempChkFlag < 0 )
	{
		return tempChkFlag;
	}


	// 그 노트의 값을 실질적으로 변경.
	if( targetNote->editNoteData(editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, editNoteEndTimeSec, editNoteEndTimeMilSec) < 0 )
	{
		// 만약 일반노트 -> 롱노트 로 바꾸려고 했다면,
		// 이 구문 굳이 필요 한 부분인가???
		cout << "Error! Invalid note Type Change!" << endl;
		return -8;
	}


	// 일단 해당 배열에서 제외 한 다음,
	int i = targetArrayNum+1;
	unsigned int editNoteArrayNumber = editNoteTimeSec / SECOND_PER_ARRAY;		// 이제 어느 Array에 넣어야 할 지 결정.
	unsigned int orgNoteArrayNumber = targetNote->getNoteTimeSec() / SECOND_PER_ARRAY;		// 원래 어떤 Array에 있었는지,

	// 해당 배열에서 제외
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // 노트가 맨 끝에 있었을 수 있기 때문에 일단 NULL을 넣어 준다.

	// 그 이후의 배열의 내용을 한 칸씩 당겨온다.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// 마무리. 마지막에 i++을 했기 때문에 i-1을 입력한다.

	// 혹시 배열의 최대치를 넘어갔는지 확인
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// 다시 추가한다.
	if( addNoteObject(targetNote) < 0 )
	{
		cout << "Error! 끼워넣기 error" << endl;
		return -4;
	}


	return 0;
}



// 입력받은 드래그 노트의 각 좌표점을 수정하는 함수.
int CNoteFile::editDragNotePoint(
	const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
	const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPointY )
{
	// 필요한 지역변수 선언
	CNoteData* notePointer;
	// 밀리초 단위 변환
	unsigned int orgNoteTimeMilSec2 = orgNoteTimeMilSec % 1000;
	int targetArrayNum = -1;


	// 먼저 수정하려고 하는 값이 올바른 값인지 확인 한 다음, 원래 존재하는 값을 가지고 있는 노트의 주소를 알아내고, 롱노트->일반노트, 혹은 그 반대로 변하는지 확인하는 작업
	int tempChkFlag = checkNoteFormat(orgNoteTimeSec, orgNoteTimeMilSec2, newPointX, newPointY, targetArrayNum, &notePointer);
	if( tempChkFlag < 0 )
	{
		return tempChkFlag;
	}


	// 그 노트의 값을 실질적으로 변경.
	if( notePointer->editNoteData(pointNumber, newPointX, newPointY) < 0 )
	{
		// 만약 일반노트 -> 롱노트 로 바꾸려고 했다면,
		// 이 구문 굳이 필요 한 부분인가???
		cout << "Error! Invalid note Type Change!" << endl;
		return -8;
	}

	// 단순 드래그 노트의 값을 변경하는 것이기 때문에, 따로 끼워넣는 작업은 하지 않는다.


	//// 일단 해당 배열에서 제외 한 다음,
	//int i = targetArrayNum+1;
	//unsigned int editNoteArrayNumber = editNoteTimeSec / SECOND_PER_ARRAY;		// 이제 어느 Array에 넣어야 할 지 결정.
	//unsigned int orgNoteArrayNumber = orgNoteTimeSec / SECOND_PER_ARRAY;		// 원래 어떤 Array에 있었는지,

	//// 해당 배열에서 제외
	//NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // 노트가 맨 끝에 있었을 수 있기 때문에 일단 NULL을 넣어 준다.

	//// 그 이후의 배열의 내용을 한 칸씩 당겨온다.
	//while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	//{
	//	NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

	//	i++;
	//}
	//NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// 마무리. 마지막에 i++을 했기 때문에 i-1을 입력한다.

	//// 혹시 배열의 최대치를 넘어갔는지 확인
	//if( i >= MAX_HALF_ARRAY )
	//{
	//	cout << "Error! Out of Array Range!" << endl;
	//	return -10;
	//}


	//// 다시 추가한다.
	//if( addNoteObject(notePointer) < 0 )
	//{
	//	cout << "Error! 끼워넣기 error" << endl;
	//	return -4;
	//}

	return 0;
}


// 입력받은 노트를 삭제하는 함수.
int CNoteFile::deleteNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker)
{
	// 필요한 지역변수 선언
	CNoteData* notePointer;
	// 밀리초 단위 변환
	unsigned int orgNoteTimeMilSec2 = orgNoteTimeMilSec % 1000;
	int targetArrayNum = -1;


	// 원래 존재하는 값을 가지고 있는 노트의 주소를 알아온다.
	notePointer = findNotePointer(orgNoteTimeSec, orgNoteTimeMilSec2, orgNoteType, orgTargetMarker, targetArrayNum);
	if( notePointer == NULL || targetArrayNum < 0 )
	{
		// 해당 노트를 찾지 못했을 경우
		cout << "There has no 해당 note!" << endl;
		return -2;
	}


	// 일단 해당 배열에서 제외 한 다음,
	int i = targetArrayNum+1;
	unsigned int orgNoteArrayNumber = orgNoteTimeSec / SECOND_PER_ARRAY;		// 원래 어떤 Array에 있었는지,

	// 해당 배열에서 제외
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // 노트가 맨 끝에 있었을 수 있기 때문에 일단 NULL을 넣어 준다.


	// 그 이후의 배열의 내용을 한 칸씩 당겨온다.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// 마무리. 마지막에 i++을 했기 때문에 i-1을 입력한다.


	// 혹시 배열의 최대치를 넘어갔는지 확인
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// 여기가지 왔으면 전부 완료가 된 것이므로, 노트 클래스 삭제
	delete notePointer;

	return 0;
}

int CNoteFile::deleteNote(CNoteData *targetNote)
{

	// 필요한 지역변수 선언
	//CNoteData* notePointer;
	// 밀리초 단위 변환
	unsigned int orgNoteTimeMilSec2 = targetNote->getNoteTimeMilSec() % 1000;
	int targetArrayNum = -1;


	// 원래 존재하는 값을 가지고 있는 노트의 주소를 알아온다.
	findNotePointer(targetNote, targetArrayNum);
	if( targetNote == NULL || targetArrayNum < 0 )
	{
		// 해당 노트를 찾지 못했을 경우
		cout << "There has no 해당 note!" << endl;
		return -2;
	}


	// 일단 해당 배열에서 제외 한 다음,
	int i = targetArrayNum+1;
	unsigned int orgNoteArrayNumber = targetNote->getNoteTimeSec() / SECOND_PER_ARRAY;		// 원래 어떤 Array에 있었는지,

	// 해당 배열에서 제외
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // 노트가 맨 끝에 있었을 수 있기 때문에 일단 NULL을 넣어 준다.


	// 그 이후의 배열의 내용을 한 칸씩 당겨온다.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// 마무리. 마지막에 i++을 했기 때문에 i-1을 입력한다.


	// 혹시 배열의 최대치를 넘어갔는지 확인
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// 여기가지 왔으면 전부 완료가 된 것이므로, 노트 클래스 삭제
	//delete notePointer;

	return 0;
}

// 가지고 있는 모든 노트들을 삭제
int CNoteFile::clearAllNotes(void)
{
	int i, j;

	// 메모리에 올려놨던 노트파일들 모두 삭제
	for( i=0 ; i< MAX_NOTE_ARRAY ; i++ )
	{
		for( j=0 ; j < MAX_HALF_ARRAY ; j++ )
		{
			delete NoteAllList[i][j];
			NoteAllList[i][j] = NULL;
		}
	}

	// 기타 변수들 초기화
	this->noteFileVersion = 100;				// Version 1.00

	this->noteFileTitle = "Untitled";			// 제목
	this->noteFileArtist = "Unknown";			// 음악가

	this->noteFileLevel = 1;					// 기본 난이도는 항상 1

	this->mp3FileName = "";						// mp3파일 제목
	this->titlePicture = "";					// 타이틀 그림 경로

	// 시작 시간
	startSongTime.noteTimeSec = 0;
	startSongTime.noteTimeMilSec = 0;

	// 종료 시간 (임의로 2분으로 정함)
	endSongTime.noteTimeSec = 120;
	endSongTime.noteTimeMilSec = 0;


	this->noteFileInitBPM = 120;


	return 1;
}






// GET 함수들
// 노트 배열에서 노트의 주소값을 받아내는 함수.
CNoteData* CNoteFile::getNoteAddr(const unsigned int i, const unsigned int j)
{
	if( i >= MAX_NOTE_ARRAY || j >= MAX_HALF_ARRAY )
	{
		// 일정 범위 이상일 경우.
		return NULL;
	}

	return NoteAllList[i][j];
}

unsigned int CNoteFile::getNoteFileVersion(void)
{
	return noteFileVersion;
}

string CNoteFile::getNoteFileTitle(void)
{
	return noteFileTitle;
}

string CNoteFile::getNoteFileArtist(void)
{
	return noteFileArtist;
}

unsigned int CNoteFile::getNoteFileLevel(void)
{
	return noteFileLevel;
}

string CNoteFile::getMp3FileName(void)
{
	return mp3FileName;
}

string CNoteFile::getTitlePicture(void)
{
	return titlePicture;
}

NoteTime CNoteFile::getStartSongTime(void)
{
	return startSongTime;
}

NoteTime CNoteFile::getEndSongTime(void)
{
	return endSongTime;
}

unsigned int CNoteFile::getNoteFileInitBPM(void)
{
	return noteFileInitBPM;
}





// SET 함수들
void CNoteFile::setNoteFileVersion(const unsigned int noteFileVersion)
{
	this->noteFileVersion = noteFileVersion;
}

void CNoteFile::setNoteFileTitle(string noteFileTitle)
{
	this->noteFileTitle = noteFileTitle;
}

void CNoteFile::setNoteFileArtist(string noteFileArtist)
{
	this->noteFileArtist = noteFileArtist;
}

void CNoteFile::setNoteFileLevel(const unsigned int noteFileLevel)
{
	this->noteFileLevel = noteFileLevel;
}

void CNoteFile::setMp3FileName(string mp3FileName)
{
	this->mp3FileName = mp3FileName;
}

void CNoteFile::setTitlePicture(string titlePicture)
{
	this->titlePicture = titlePicture;
}

void CNoteFile::setStartSongTime(NoteTime startSongTime)
{
	this->startSongTime = startSongTime;
}

void CNoteFile::setEndSongTime(NoteTime endSongTime)
{
	this->endSongTime = endSongTime;
}

void CNoteFile::setNoteFileInitBPM(unsigned int noteFileInitBPM)
{
	this->noteFileInitBPM = noteFileInitBPM;
}


