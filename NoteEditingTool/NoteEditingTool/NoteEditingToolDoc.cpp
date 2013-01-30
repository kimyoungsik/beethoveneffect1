
// NoteEditingToolDoc.cpp : CNoteEditingToolDoc Ŭ������ ����
//

#include "stdafx.h"
// SHARED_HANDLERS�� �̸� ����, ����� �׸� �� �˻� ���� ó���⸦ �����ϴ� ATL ������Ʈ���� ������ �� ������
// �ش� ������Ʈ�� ���� �ڵ带 �����ϵ��� �� �ݴϴ�.
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


// CNoteEditingToolDoc ����/�Ҹ�

CNoteEditingToolDoc::CNoteEditingToolDoc()
{
	// TODO: ���⿡ ��ȸ�� ���� �ڵ带 �߰��մϴ�.
	noteEditingMode = EDIT_MODE_WRITE;			// �⺻�� �׸��� ���
	noteWriteType = NOTE_T_RIGHT;				// �⺻�� ������ ��Ʈ

	nowEditingNote = NULL;						// ���� �����߿� �ִ� ��Ʈ�� ���Ѵ�.
	onFocusEditingFlag = false;
	noteHeadDir = 0;
	nowConfigingNote = NULL;						// ���� �����߿� �ִ� ��Ʈ�� ���Ѵ�. CONFG��� �ܿ��� �׻� NULL�̾�� �Ѵ�.
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



// �뷫 � ������ ��Ʈ������ Ȯ���Ѵ�.
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


// ����� ����� �Լ�.
int CNoteEditingToolDoc::SaveFileHeader(CArchive &ar)
{
//	string tmpString;
	char tmpCharArray[20];
	double noteTimeDouble;


	CString dataString;
	CString endlString;						// �ٹٲ��� ���� String (�����ڵ� ����)
	endlString.AppendChar(0x000d);			// Endian ������ �̿� ���� �Է�.
	endlString.AppendChar(0x000a);

	// �����ڵ� ���ڵ��� ���ؼ� FFFE�� �� ó���� �־��ش�.
	dataString.AppendChar(0xFEFF);
	ar.WriteString(dataString);

	// ���� ���ڿ���
	dataString.Format(TEXT("MNF FILE"));
	dataString += endlString;
	dataString += endlString;
	dataString += _T("#################### [HEADER] ####################");
	dataString += endlString;
	ar.WriteString(dataString);

	// ��Ʈ ���� ����
	dataString.Format(TEXT("%d"), newNoteFile.getNoteFileVersion());
	dataString += endlString;
	ar.WriteString(dataString);

	// �� ����
	//dataString = _T("Title : ");
	dataString = _T("");
	dataString += newNoteFile.getNoteFileTitle().c_str();
	dataString += endlString;
	ar.WriteString(dataString);

	// ��Ƽ��Ʈ
	//dataString += _T("Artist : ");
	dataString = _T("");
	dataString += newNoteFile.getNoteFileArtist().c_str();
	dataString += endlString;
	ar.WriteString(dataString);

	// ���̵�
	dataString.Format(TEXT("%d"), newNoteFile.getNoteFileLevel());
	dataString += endlString;
	ar.WriteString(dataString);

	// mp3���� �̸� [Ȥ�ó� ���ϸ��� ��û���� �� ���� �����Ƿ�, ���� CString�� ����]
	//dataString = _T("mp3 File name : ");
	dataString = _T("");
	dataString += newNoteFile.getMp3FileName().c_str();
	dataString += endlString;
	ar.WriteString(dataString);

	// Ÿ��Ʋ �׸� �̸� [Ȥ�ó� ���ϸ��� ��û���� �� ���� �����Ƿ�, ���� CString�� ����]
	//dataString = _T("Title picture : ");
	dataString = _T("");
	dataString += newNoteFile.getTitlePicture().c_str();
	dataString += endlString;
	ar.WriteString(dataString);

	// ���۽ð�.
	noteTimeDouble = newNoteFile.getStartSongTime().noteTimeSec;
	noteTimeDouble += (newNoteFile.getStartSongTime().noteTimeMilSec / (double)1000);
	dataString.Format(TEXT("%.3lf"), noteTimeDouble);
	dataString += endlString;
	ar.WriteString(dataString);

	// ����ð�.
	noteTimeDouble = newNoteFile.getEndSongTime().noteTimeSec;
	noteTimeDouble += (newNoteFile.getEndSongTime().noteTimeMilSec / (double)1000);
	dataString.Format(TEXT("%.3lf"), noteTimeDouble);
	dataString += endlString;
	ar.WriteString(dataString);

	// ���� BPM
	//dataString = _T("Initial BPM : ");
	dataString = _T("");
	_itoa_s(newNoteFile.getNoteFileInitBPM(), tmpCharArray, 18, 10);				// int���� 10�������� ���ۿ� �����ֱ� (�ִ� 18)
	//_itoa(newNoteFile.getNoteFileInitBPM(), tmpCharArray, 10);
	dataString += tmpCharArray;
	dataString += endlString;
	dataString += endlString;
	ar.WriteString(dataString);



	return 1;
}

// ���� ��ü�� ����� �Լ�.
int CNoteEditingToolDoc::SaveFileBody(CArchive &ar)
{
	CNoteData *tempNote = NULL;

	CString dataString;
	CString endlString;						// �ٹٲ��� ���� String (�����ڵ� ����)
	endlString.AppendChar(0x000d);			// Endian ������ �̿� ���� �Է�.
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

			// ������� ���� ���.
			if( SaveBodyNote(ar, tempNote) < 0 )
			{
				// ����
				AfxMessageBox(_T("CNoteEditingToolDoc::SaveFileBody ����!"));
				return -1;
			}
		}
	}

	//ar.WriteString(endlString);


	return 1;
}


// ��Ʈ �ϳ��� ���ؼ� ����ϴ� �Լ�.
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
	slashString.AppendChar(0x002F);			// '/' �� �Է�
	CString endlString;						// �ٹٲ��� ���� String (�����ڵ� ����)
	endlString.AppendChar(0x000d);			// Endian ������ �̿� ���� �Է�.
	endlString.AppendChar(0x000a);

	// ���� �ð�
	dataString.Format(TEXT("%.3lf"), noteTimeDouble);
	dataString += slashString;

	// ��Ʈ Ÿ��
	tmpString.Format(TEXT("%c"), targetNote->getNoteType());
	dataString += tmpString;
	dataString += slashString;

	switch ( targetNote->getNoteType())
	{
		// ��Ʈ�� Ÿ�Կ� ���� ����� �ٲ��.
	case NOTE_T_RIGHT:
	case NOTE_T_LEFT:
	case NOTE_T_PHOTO:
		// Ÿ�� ��Ŀ
		tmpString.Format(TEXT("%c"), targetNote->getTargetMarker());
		dataString += tmpString;
		dataString += slashString;
		break;


	case NOTE_T_BPM:
		// BPM ��Ʈ�� ���, Ÿ�� ��Ŀ�� ���ڷ� ���.
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
		// Ÿ�� ��Ŀ
		tmpString.Format(TEXT("%c"), targetNote->getTargetMarker());
		dataString += tmpString;
		dataString += slashString;

		// ������ �ð� ���.
		noteTimeDouble = (unsigned char)(targetNote->getNoteEndTimeSec());
		noteTimeDouble += targetNote->getNoteEndTimeMilSec() / (double)1000;
		tmpString.Format(TEXT("%.3lf"), noteTimeDouble);
		dataString += tmpString;
		dataString += slashString;
		break;


	case NOTE_T_DRAG:
		// ������ �ð� ���.
		noteTimeDouble = targetNote->getNoteEndTimeSec();
		noteTimeDouble += targetNote->getNoteEndTimeMilSec() / (double)1000;
		tmpString.Format(TEXT("%.3lf"), noteTimeDouble);
		dataString += tmpString;
		dataString += slashString;

		// �巡�� ��Ʈ�� ����Ʈ ���
		for(i=0 ; i < 4 ; i++ )
		{
			if( targetNote->getDragPoint(i,tempDotData) < 0 )
			{
				AfxMessageBox(_T("CNoteEditingToolDoc::SaveBodyNote���� Drag ���ϰ� ����"));
				return -1;
			}
			tmpString.Format(TEXT("%d,%d"), tempDotData.x, tempDotData.y);
			dataString += tmpString;
			dataString += slashString;
		}
		break;
		

	default:
		// ���� �߸��� ��Ʈ�� ���� ���
		AfxMessageBox(_T("��Ʈ ������� CNoteEditingToolDoc::SaveBodyNote���� Ÿ�� ����!"));
		return -1;
	}

	dataString += endlString;
	ar.WriteString(dataString);

	return 1;
}



// ����� �д� �Լ�.
int CNoteEditingToolDoc::LoadFileHeader(CArchive &ar)
{
	CString getString;
	CString compString;						// �񱳸� ���� String

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
		// �� ù ���� "MNF FILE"�� �´��� Ȯ���Ѵ�.
		AfxMessageBox(_T("Load Header Error!"));
		return -1;
	}

	ar.ReadString(getString);		// ����
	ar.ReadString(getString);		// HEADER
	compString.Format(TEXT("#################### [HEADER] ####################"));
	if( getString != compString )
	{
		// �� ù ���� "MNF FILE"�� �´��� Ȯ���Ѵ�.
		AfxMessageBox(_T("Load Header Error!"));
		return -1;
	}

	// ��Ʈ ���� ����
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	tempBPM = atoi(tempString.c_str());					// ���������� ��ȯ
	newNoteFile.setNoteFileVersion(tempBPM);

	// Ÿ��Ʋ
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	newNoteFile.setNoteFileTitle(tempString);

	// ��Ƽ��Ʈ
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	newNoteFile.setNoteFileArtist(tempString);
	
	// ��Ʈ ���� ���̵�
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	tempBPM = atoi(tempString.c_str());					// ���������� ��ȯ
	newNoteFile.setNoteFileLevel(tempBPM);

	// mp3���� �̸�
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	newNoteFile.setMp3FileName(tempString);			///////////////

	// Ÿ��Ʋ �׸�
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	newNoteFile.setTitlePicture(tempString);			/////////////////

	// ���� �ð�
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
			// �ʸ� �� �Է¹ޱ⵵ ����, ��Ʈ���� ���� �Դٸ�, ����.
			return -1;
		}
		if( tempString[i] == '.' )
		{
			// string���� ��(Sec)�� �����Ѵ�.
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
			// string���� �и��ʸ� ���� �� ���� �� ���̹Ƿ� ����.
			break;
		}
		if ( tempString[i] == 0 )
		{	return -1;	}

		timeMilSec *= 10;
		timeMilSec += (unsigned int)(tempString[i] - '0');
		i++;
		j++;
	}

	// �Է�
	tempTime.noteTimeSec = timeSec;
	tempTime.noteTimeMilSec = timeMilSec;
	newNoteFile.setStartSongTime(tempTime);


	// ���� �ð�
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
			// �ʸ� �� �Է¹ޱ⵵ ����, ��Ʈ���� ���� �Դٸ�, ����.
			return -1;
		}
		if( tempString[i] == '.' )
		{
			// string���� ��(Sec)�� �����Ѵ�.
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
		{	// string���� �и��ʸ� ���� �� ���� �� ���̹Ƿ� ����.
			break;
		}

		if ( tempString[i] == 0 )
		{	return -1;	}

		timeMilSec *= 10;
		timeMilSec += (unsigned int)(tempString[i] - '0');
		i++;
		j++;
	}

	// �Է�
	tempTime.noteTimeSec = timeSec;
	tempTime.noteTimeMilSec = timeMilSec;
	newNoteFile.setEndSongTime(tempTime);


	// BPM
	ar.ReadString(getString);
	tempString = (CT2CA)getString;
	tempBPM = atoi(tempString.c_str());					// ���������� ��ȯ
	newNoteFile.setNoteFileInitBPM(tempBPM);


	return 1;
}


// ���� ��ü�� �д� �Լ�.
int CNoteEditingToolDoc::LoadFileBody(CArchive &ar)
{
	CString getString;
	CString compString;						// �񱳸� ���� String
	bool loopFlag = true;

	ar.ReadString(getString);		// ����
	ar.ReadString(getString);		// BODY
	compString.Format(TEXT("##################### [BODY] #####################"));
	if( getString != compString )
	{
		// �� ù ���� "MNF FILE"�� �´��� Ȯ���Ѵ�.
		AfxMessageBox(_T("Load Body Error!"));
		return -1;
	}

	while ( loopFlag )
	{
		switch ( LoadBodyNote(ar, newNoteFile) )
		{
		case -1:
			// ������ ���� ��
			AfxMessageBox(_T("Note format Error!"));
			break;
		case 0:
			// �� ������ ��Ʈ�� ��
			loopFlag = false;
			break;

		case 1:
			// ���� ������ ��,
			break;
		}
	}



	return 1;
}

// ��Ʈ �ϳ��� ���ؼ� �о���� �Լ�.
int CNoteEditingToolDoc::LoadBodyNote(CArchive &ar, CNoteFile &targetNoteFile)
{
	CString getString;
	CString compString;						// �񱳸� ���� String

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

	// ��� ��Ʈ�� �� �а� �� ������ Ȯ��
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

	// ���۽ð� �Ľ�
	for( strCurr=0 ; (tempString[strCurr] != '.') ; strCurr++ )
	{
		// Ȥ�ó� i���� �ʹ��� Ŀ�� �� �����Ƿ�,
		if( strCurr > 20 )
		{
			return -1;
		}
		if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
		{
			// ���ڰ� �ƴ� �̻��� ������ ���
			return -1;
		}

		timeSec *= 10;
		timeSec += (tempString[strCurr] - '0');
	}
	strCurr++;				// '.' ������ ���ڷ� �Ѿ��.

	for( j=0 ; j < 3 ; j++ )
	{
		if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
		{
			// ���ڰ� �ƴ� �̻��� ������ ���
			return -1;
		}

		timeMilSec *= 10;
		timeMilSec += (tempString[strCurr] - '0');

		strCurr++;
	}

	tempStartTime.noteTimeSec = timeSec;
	tempStartTime.noteTimeMilSec = timeMilSec%1000;

	// '/' Ȯ��
	if( tempString[strCurr++] != '/' )
	{
		// �߸��� �м��� �� ���̹Ƿ� ����
		return -1;
	}

	// ��Ʈ Ÿ�� Ȯ��
	tempNoteType = tempString[strCurr++];

	// '/' Ȯ��
	if( tempString[strCurr++] != '/' )
	{
		// �߸��� �м��� �� ���̹Ƿ� ����
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
		// ���� Ÿ�� ��Ŀ�� ���� ���.
		tempTargetNote = tempString[strCurr++];
		if( tempString[strCurr] != '/' )
		{
			// ����
			return -1;
		}

		// ���������� ��Ʈ �߰�
		newNoteFile.addNewNote(tempStartTime.noteTimeSec, tempStartTime.noteTimeMilSec, tempNoteType, tempTargetNote);

		break;


	case NOTE_T_BPM:
		// ���� Ÿ�� ��Ŀ (char������ ��ȯ)
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
			// ����
			return -1;
		}

		// ���������� ��Ʈ �߰�
		newNoteFile.addNewNote(tempStartTime.noteTimeSec, tempStartTime.noteTimeMilSec, tempNoteType, tempTargetNote);

		break;

	case NOTE_T_LONG:
	case NOTE_T_CHARISMA:
	case NOTE_T_NEUTRAL:
	case NOTE_T_PATTERN:
		// Ÿ�ٸ�Ŀ/����ð� ���� ���� ���.

		tempTargetNote = tempString[strCurr++];
		if( tempString[strCurr++] != '/' )
		{
			// ����
			return -1;
		}


		// ����ð� �Ľ�
		timeSec = 0;
		timeMilSec = 0;

		for( ; (tempString[strCurr] != '.') ; strCurr++ )
		{
			// Ȥ�ó� i���� �ʹ��� Ŀ�� �� �����Ƿ�,
			if( strCurr > 20 || tempString[strCurr] == NULL )
			{
				return -1;
			}
			if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
			{
				// ���ڰ� �ƴ� �̻��� ������ ���
				return -1;
			}

			timeSec *= 10;
			timeSec += (tempString[strCurr] - '0');
		}
		strCurr++;				// '.' ������ ���ڷ� �Ѿ��.

		for( j=0 ; j < 3 ; j++ )
		{
			if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
			{
				// ���ڰ� �ƴ� �̻��� ������ ���
				return -1;
			}

			timeMilSec *= 10;
			timeMilSec += (tempString[strCurr] - '0');

			strCurr++;
		}

		tempEndTime.noteTimeSec = timeSec;
		tempEndTime.noteTimeMilSec = timeMilSec%1000;


		// ���������� ��Ʈ �߰�
		newNoteFile.addNewNote(tempStartTime.noteTimeSec, tempStartTime.noteTimeMilSec, tempNoteType, tempTargetNote, tempEndTime.noteTimeSec, tempEndTime.noteTimeMilSec);

		break;

	
	case NOTE_T_DRAG:
		// ����ð�/����Ʈ * 4�� ���� ���.
		// ����ð� �Ľ�
		timeSec = 0;
		timeMilSec = 0;

		for( ; (tempString[strCurr] != '.') ; strCurr++ )
		{
			// Ȥ�ó� i���� �ʹ��� Ŀ�� �� �����Ƿ�,
			if( strCurr > 20 || tempString[strCurr] == NULL )
			{
				return -1;
			}
			if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
			{
				// ���ڰ� �ƴ� �̻��� ������ ���
				return -1;
			}

			timeSec *= 10;
			timeSec += (tempString[strCurr] - '0');
		}
		strCurr++;				// '.' ������ ���ڷ� �Ѿ��.

		for( j=0 ; j < 3 ; j++ )
		{
			if( tempString[strCurr] < '0' || tempString[strCurr] > '9' )
			{
				// ���ڰ� �ƴ� �̻��� ������ ���
				return -1;
			}

			timeMilSec *= 10;
			timeMilSec += (tempString[strCurr] - '0');

			strCurr++;
		}

		tempEndTime.noteTimeSec = timeSec;
		tempEndTime.noteTimeMilSec = timeMilSec%1000;

		// '/' Ȯ��
		if( tempString[strCurr++] != '/' )
		{
			return -1;
		}

		// ��ǥ Get.
		for( j=0 ; j<4 ; j++ )
		{
			// ��ǥ x
			while( tempString[strCurr] != ',' )
			{
				if( tempString[strCurr] == NULL )
				{
					// ���� ���߿� ������ ���.
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

			// ��ǥ y
			while( tempString[strCurr] != '/' )
			{			
				if( tempString[strCurr] == NULL )
				{
					// ���� ���߿� ������ ���.
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
		// �ƹ��� �ش���׵� ���� ��,
		return -1;
	}






	


	return 1;

}

//////////////////////////////////////////////////////////////////////////////
//////////////////////////////   [�������Լ�]   //////////////////////////////
//////////////////////////////////////////////////////////////////////////////



// ��Ʈ�� �ϳ� ������ �迭�� �߰��ϴ� �Լ�.
int CNoteEditingToolDoc::addNewNote(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	return newNoteFile.addNewNote(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
}

// �Է¹��� �Ϲ� ��Ʈ�� �����ϴ� �Լ�.
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


// �Է¹��� Ư�� ��Ʈ �κ��� �����ϴ� �Լ�.
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

// �Է¹��� �巡�� ��Ʈ�� �� ��ǥ���� �����ϴ� �Լ�.
int CNoteEditingToolDoc::editDragNotePoint(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
	const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPointY)
{
	return newNoteFile.editDragNotePoint(orgNoteTimeSec,  orgNoteTimeMilSec, pointNumber, newPointX, newPointY);
}

// �Է¹��� ��Ʈ�� �����ϴ� �Լ�.
int CNoteEditingToolDoc::deleteNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker)
{
	return newNoteFile.deleteNote(orgNoteTimeSec, orgNoteTimeMilSec, orgNoteType, orgTargetMarker);
}

int CNoteEditingToolDoc::deleteNote(CNoteData *targetNote)
{
	return newNoteFile.deleteNote(targetNote);
}


// ��Ʈ �迭���� ��Ʈ�� �ּҰ��� �޾Ƴ��� �Լ�.
CNoteData* CNoteEditingToolDoc::getNoteAddr(const unsigned int i, const unsigned int j)
{
	return newNoteFile.getNoteAddr(i, j);
}



// ������ �ִ� ��� ��Ʈ���� ����
int CNoteEditingToolDoc::clearAllNotes(void)
{
	setNowConfigingNote(NULL, 0);
	setNowEditingNote(NULL, 0);
	setOnFocusEditingFlag(false);

	// �� ���Ǻ����� �ʱ�ȭ
	mnfFilePath = "";
	isNewFileFlag = true;

	return newNoteFile.clearAllNotes();
}


//////////////////////////////////////////////////////////////////////////////
////////////////////////   [Get & Set�� �������Լ�]   ////////////////////////
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

	// TODO: ���⿡ ���ʱ�ȭ �ڵ带 �߰��մϴ�.
	clearAllNotes();

	// SDI ������ �� ������ �ٽ� ����մϴ�.

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
		// TODO: ���⿡ ���� �ڵ带 �߰��մϴ�.
		SaveFileHeader(ar);
		SaveFileBody(ar);

		ar.WriteString(_T("END"));



	}
	else
	{
		// TODO: ���⿡ �ε� �ڵ带 �߰��մϴ�.


		// ��� Ŭ���� ����
		clearAllNotes();

		// �ε��̹Ƿ� newFile�� �ƴϴ�.
		isNewFileFlag = FALSE;

		// Header Read
		if( LoadFileHeader(ar) < 0 )
		{
			// ���� ��� �κп��� ������ �Ͼ�� �� �о���� ���,
			AfxMessageBox(_T("Header �ε� �������� ������ �߻��߽��ϴ�!"));
			return;
		}

		// Body Read
		if ( LoadFileBody(ar) < 0 )
		{
			// ���� ��� �κп��� ������ �Ͼ�� �� �о���� ���,
			AfxMessageBox(_T("Body �ε� �������� ������ �߻��߽��ϴ�!"));
		}

		// MNF������ ��θ� ����
		tempPath = ar.m_strFileName;
		
		// ��� �κ��� ã�� �� ����
		while ( tempPath[i] != NULL )
		{
			if (i > MAX_PATH)
			{
				AfxMessageBox(_T("���� ����� �ɰ��� ���� �߻�!"));
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
			AfxMessageBox(_T("���� ��ΰ� �������� �ʽ��ϴ�."));
		}

		// ��� �κ� ������ �����ؼ� ����.
		mnfFilePath = "";					// �ʱ�ȭ �� ����
		for ( i=0 ; i<=pathEndIndex ; i++ )
		{
			mnfFilePath += tempPath[i];
		}
		
		
	}
}



//////////////////////////////////////////////////////////////////////////////
//////////////////////////////     [Public]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////



// �Է¹��� �ð� �ֺ��� ������ ��Ʈ�� �ִ� �� Ȯ���Ѵ�. ������ 1, ������ 0 ����.
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
		// 0��°���� ������ �ð����� ��Ʈ���� Ȯ���Ѵ�.
		for ( j=0 ; j < MAX_HALF_ARRAY ; j++ )
		{
			tmpNote = newNoteFile.getNoteAddr(i, j);
			if( tmpNote == NULL )
			{
				// ���� �� �̻� �� Array�� ���.
				// Ȥ�� �𸣴� �� �� Array���� ������ ������ ��Ʈ�� EndŸ���� �˻� �� ����.

				// �׷��� ������ ����
				break;

				//return 0;
			}

			// �ð��� ������ ����.
			tmpTimeInt = ( tmpNote->getNoteTimeSec() * 1000 ) + tmpNote->getNoteTimeMilSec();


			// �ð��� Ȯ���Ѵ�.
			if ( tmpTimeInt >= (compTimeInt - NOTE_COMP_RANGE1))
			{
				// ��Ʈ�� Ÿ�԰� Ÿ�ٸ�Ŀ�� �´��� Ȯ���Ѵ�.
				if ( tmpTimeInt > (compTimeInt + NOTE_COMP_RANGE2) )
				{
					// ���� ������ �������� �ξ� ũ�ٸ�, �� �̻� �ش��ϴ� ��Ʈ�� ���� ������, �Լ��� �����Ѵ�.
// 					*notePointer = NULL;
// 					nowEditingNote = NULL;
// 					noteHeadDir = 0;
// 					return 0;
					break;
				}
				else
				{
					// ��Ʈ Ÿ�� Ȯ��
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
							// ���� ���� ��Ʈ���� ����, Ÿ�� ��Ŀ�� �ǹ̰� ���� ������ �����Ѵ�.
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


			// ��Ʈ�� ������ �ð��� �ִ��� Ȯ���Ѵ�.
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
					// ��Ʈ Ÿ�� Ȯ��
					if( noteType == chkTypeToBigType(tmpNote->getNoteType()))
					{
						// ���� �ȿ� ���� ���, ��Ŀ�� ������ Ȯ��.
						if( tmpNote->getNoteType() == NOTE_T_DRAG ||
							tmpNote->getNoteType() == NOTE_T_CHARISMA ||
							tmpNote->getNoteType() == NOTE_T_NEUTRAL ||
							tmpNote->getNoteType() == NOTE_T_PATTERN
							)
						{
							// ���� ���� ��Ʈ���� ����, Ÿ�� ��Ŀ�� �ǹ̰� ���� ������ �����Ѵ�.
							*notePointer = tmpNote;
							nowEditingNote = tmpNote;
							noteHeadDir = 1;
							return 1;
						}
						else if( tmpNote->getTargetMarker() == targetMarker )
						{
							*notePointer = tmpNote;
							nowEditingNote = tmpNote;
							noteHeadDir = 1;				// tail �̹Ƿ�.
							return 1;
						}
					}
				}
			}
		}

	}

	// ������� ���� ���� ��
	//AfxMessageBox(_T("�浹 ó�� �Լ����� ����"));
	return 0;

}









//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [Get&Set]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////


// ���� � ����ΰ��� �����ϴ� �Լ�.
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

/////////////// set �Լ���



int CNoteEditingToolDoc::setNoteEditingMode(const char editingMode)
{
	switch(editingMode)
	{
	case EDIT_MODE_WRITE:
		noteEditingMode = EDIT_MODE_WRITE;

		// �����͵����ʱ�ȭ
		nowEditingNote = NULL;
		noteHeadDir = 0;
		nowConfigingNote = NULL;
		noteConfigHeadDir = 0;
		 
		// ���� ����
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

		// �����͵����ʱ�ȭ
		nowEditingNote = NULL;
		noteHeadDir = 0;
		nowConfigingNote = NULL;
		noteConfigHeadDir = 0;

		// ���� ����
		if ( notePickingViewPtr != NULL )
		{
			notePickingViewPtr->PostMessageW(CM_REDRAW_DC2);
		}

		return 1;
		break;

	case EDIT_MODE_ERASE:
		noteEditingMode = EDIT_MODE_ERASE;

		// �����͵����ʱ�ȭ
		nowEditingNote = NULL;
		noteHeadDir = 0;
		nowConfigingNote = NULL;
		noteConfigHeadDir = 0;

		// ���� ����
		if ( notePickingViewPtr != NULL )
		{
			notePickingViewPtr->PostMessageW(CM_REDRAW_DC2);
		}
		return 1;
		break;

	case EDIT_MODE_CONFG:
		noteEditingMode = EDIT_MODE_CONFG;

		// �����͵����ʱ�ȭ
		nowEditingNote = NULL;
		noteHeadDir = 0;
		nowConfigingNote = NULL;
		noteConfigHeadDir = 0;

		// ���� ����
		if ( notePickingViewPtr != NULL )
		{
			notePickingViewPtr->PostMessageW(CM_REDRAW_DC2);
		}
		return 1;
		break;

	}

	// �ƹ� �ش���� ������ �ٲ��� �ʰ�, ���� ����.
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

	// �ƹ� �ش���� ������ �ٲ��� �ʰ�, ���� ����.
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

// ����� �׸��� �����մϴ�.
void CNoteEditingToolDoc::OnDrawThumbnail(CDC& dc, LPRECT lprcBounds)
{
	// ������ �����͸� �׸����� �� �ڵ带 �����Ͻʽÿ�.
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

// �˻� ó���⸦ �����մϴ�.
void CNoteEditingToolDoc::InitializeSearchContent()
{
	CString strSearchContent;
	// ������ �����Ϳ��� �˻� �������� �����մϴ�.
	// ������ �κ��� ";"�� ���еǾ�� �մϴ�.

	// ��: strSearchContent = _T("point;rectangle;circle;ole object;");
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

// CNoteEditingToolDoc ����

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


// CNoteEditingToolDoc ���


void CNoteEditingToolDoc::OnOpenNewPictureFile()
{
	// TODO: ���⿡ ��� ó���� �ڵ带 �߰��մϴ�.

	// ���̾�α� �ʱ�ȭ
	importPicDlg.m_PictureFilename = CString::CStringT( CA2CT(newNoteFile.getTitlePicture().c_str()) );

	// ���̾�α� ����
	if( importPicDlg.DoModal() == IDOK )
	{
		// ������Ʈ
		newNoteFile.setTitlePicture( std::string(CT2CA(importPicDlg.m_PictureFilename.operator LPCWSTR())) );
	}


}
