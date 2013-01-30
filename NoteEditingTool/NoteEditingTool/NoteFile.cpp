//#pragma once


#include "NoteFile.h"
#include "stdafx.h"




CNoteFile::CNoteFile(void)
{
	// ���� ������, ��Ʈ ���� �迭 �ʱ�ȭ.
	for( int i=0 ; i<MAX_NOTE_ARRAY ; i++ )
	{
		for( int j=0 ; j<MAX_HALF_ARRAY ; j++ )
		{
			NoteAllList[i][j] = 0;
		}
	}

	// ��Ÿ ������ �ʱ�ȭ
	this->noteFileVersion = 100;				// Version 1.00

	this->noteFileTitle = "Untitled";			// ����
	this->noteFileArtist = "Unknown";			// ���ǰ�

	this->noteFileLevel = 1;					// �⺻ ���̵��� �׻� 1

	this->mp3FileName = "";						// mp3���� ����
	this->titlePicture = "";					// Ÿ��Ʋ �׸� ���

	// ���� �ð�
	startSongTime.noteTimeSec = 0;
	startSongTime.noteTimeMilSec = 0;

	// ���� �ð� (���Ƿ� 2������ ����)
	endSongTime.noteTimeSec = 120;
	endSongTime.noteTimeMilSec = 0;


	this->noteFileInitBPM = 120;

	
}


CNoteFile::~CNoteFile(void)
{
	int i, j;

	// �޸𸮿� �÷����� ��Ʈ���ϵ� ��� ����
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


// Ư�� �ּҿ� ������ ��Ʈ�� �����ؼ� �迭�� ��ũ ��Ų��.
int CNoteFile::makeNewNote(CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{	
	switch( noteType )
	{
	case NOTE_T_RIGHT:			// ������ ��Ʈ
	case NOTE_T_LEFT:			// �޼� ��Ʈ
	case NOTE_T_BPM:			// bpm change
	case NOTE_T_PHOTO:			// ���� ��Ʈ
		// 1, 2, 3�� ��� ���� NoteData Class�� ����Ѵ�.
		// �� �κ��� �Լ��� ���� ���� �ۼ� ���
		*targetAddr = new CNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	case NOTE_T_LONG:				// �� ��Ʈ
	case NOTE_T_PATTERN:			// Pattern-Change
	case NOTE_T_CHARISMA:			// Charisma Time
	case NOTE_T_NEUTRAL:			// Nutral Position
		*targetAddr = new CLongNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	case NOTE_T_DRAG:			// Drag Note
		*targetAddr = new CDragNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	default:
		// ������� �ش������ ������ ���� ���
		cout << "Error! Non-matching Note type!" << endl;
		return -1;
	}

	// ���� ����.
	return 1;
}


// Ư�� �ּҿ� ������ ��Ʈ�� �����ؼ� �迭�� ��ũ ��Ų��.
int CNoteFile::makeNewNote(CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
	const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec)
{	
	switch( noteType )
	{
	case NOTE_T_RIGHT:			// ������ ��Ʈ
	case NOTE_T_LEFT:			// �޼� ��Ʈ
	case NOTE_T_BPM:			// bpm change
	case NOTE_T_PHOTO:			// ���� ��Ʈ
		// 1, 2, 3�� ��� ���� NoteData Class�� ����Ѵ�.
		// �� �κ��� �Լ��� ���� ���� �ۼ� ���
		*targetAddr = new CNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	case NOTE_T_LONG:				// �� ��Ʈ
	case NOTE_T_PATTERN:			// Pattern-Change
	case NOTE_T_CHARISMA:			// Charisma Time
	case NOTE_T_NEUTRAL:			// Nutral Position
		*targetAddr = new CLongNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker, noteEndTimeSec, noteEndTimeMilSec);
		break;

	case NOTE_T_DRAG:			// Drag Note
		*targetAddr = new CDragNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	default:
		// ������� �ش������ ������ ���� ���
		cout << "Error! Non-matching Note type!" << endl;
		return -1;
	}

	// ���� ����.
	return 1;
}


// Ư�� �ּҿ� ������ ��Ʈ�� �����ؼ� �迭�� ��ũ ��Ų��.
int CNoteFile::makeNewNote(CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
	const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec,
	dotData point1, dotData point2, dotData point3, dotData point4)
{	
	switch( noteType )
	{
	case NOTE_T_RIGHT:			// ������ ��Ʈ
	case NOTE_T_LEFT:			// �޼� ��Ʈ
	case NOTE_T_BPM:			// bpm change
	case NOTE_T_PHOTO:			// ���� ��Ʈ
		// 1, 2, 3�� ��� ���� NoteData Class�� ����Ѵ�.
		// �� �κ��� �Լ��� ���� ���� �ۼ� ���
		*targetAddr = new CNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker);
		break;

	case NOTE_T_LONG:				// �� ��Ʈ
	case NOTE_T_PATTERN:			// Pattern-Change
	case NOTE_T_CHARISMA:			// Charisma Time
	case NOTE_T_NEUTRAL:			// Nutral Position
		*targetAddr = new CLongNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker, noteEndTimeSec, noteEndTimeMilSec);
		break;

	case NOTE_T_DRAG:			// Drag Note
		*targetAddr = new CDragNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker, noteEndTimeSec, noteEndTimeMilSec, point1, point2, point3, point4);
		break;

	default:
		// ������� �ش������ ������ ���� ���
		cout << "Error! Non-matching Note type!" << endl;
		return -1;
	}

	// ���� ����.
	return 1;
}


// ���� ���� ��Ʈ ������ �ùٸ� �����ΰ��� üũ�ϴ� �Լ�.
int CNoteFile::checkNoteFormat(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	// 1. Second üũ
	// ����...

	// 2. mili-Second üũ
	// 0~999 ������ �������� Ȯ��
	if( noteTimeMilSec >= 1000 )
	{
		return -1;
	}

	// 3. noteType üũ
	switch( noteType )
	{
		// 4. ������ Ÿ�Կ� ���� targetMarker üũ
	case NOTE_T_RIGHT:			// ������ ��Ʈ
	case NOTE_T_LEFT:			// �޼� ��Ʈ
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


	// Long Note Class�� ����ϴ� ��Ʈ��.
	case NOTE_T_LONG:				// �� ��Ʈ
	case NOTE_T_PATTERN:			// Pattern-Change
	case NOTE_T_CHARISMA:			// Charisma Time
	case NOTE_T_NEUTRAL:			// Nutral Position


		break;

	// Drag Note Class�� ����ϴ� ��Ʈ
	case NOTE_T_DRAG:
		
		break;

	default:
		return -1;
	}


	// �� üũ���� ������ ������ ���
	return 1;
}
	

// ���� �� ��Ʈ ������ �ùٸ� �����ΰ��� üũ�ϴ� �Լ�.
int CNoteFile::checkNoteFormat(
	const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	int &targetArrayNum, CNoteData** notePointer
	)
{
	// ���� �����Ϸ��� �ϴ� ���� �ùٸ� ������ Ȯ�� �� ����,
	if( checkNoteFormat(editNoteTimeSec, editNoteTimeMilSec, editNoteType, editTargetMarker) < 0 )
	{
		// �ùٸ� ���� �ƴ� ���,
		cout << "Error! not correct edit Value!" << endl;
		return -1;
	}


	// ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾ƿ´�.
	*notePointer = findNotePointer(orgNoteTimeSec, orgNoteTimeMilSec, orgNoteType, orgTargetMarker, targetArrayNum);
	if( (*notePointer) == NULL || targetArrayNum < 0 )
	{
		// �ش� ��Ʈ�� ã�� ������ ���
		cout << "There has no �ش� note!" << endl;
		return -2;
	}


	// �ճ�Ʈ->�Ϲݳ�Ʈ, Ȥ�� �� �ݴ�� ���ϴ��� Ȯ���ϴ� �۾�
	if( classTypeCheck(orgNoteType) != classTypeCheck(editNoteType) )
	{
		cout << "Error! Note Class Type Error!" << endl;
		return -8;
	}

	// ��� �˻���� ������ �� ������� ���.
	return 1;
}


int CNoteFile::checkNoteFormat(
	CNoteData *notePointer,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker, int &targetArrayNum)
{
	// ���� �����Ϸ��� �ϴ� ���� �ùٸ� ������ Ȯ�� �� ����,
	if( checkNoteFormat(editNoteTimeSec, editNoteTimeMilSec, editNoteType, editTargetMarker) < 0 )
	{
		// �ùٸ� ���� �ƴ� ���,
		cout << "Error! not correct edit Value!" << endl;
		return -1;
	}


	// ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾ƿ´�.
	findNotePointer(notePointer->getNoteTimeSec(), notePointer->getNoteTimeMilSec(), notePointer->getNoteType(), notePointer->getTargetMarker(), targetArrayNum);
	if( notePointer == NULL )
	{
		// �ش� ��Ʈ�� ã�� ������ ���
		cout << "There has no �ش� note!" << endl;
		return -2;
	}


	// �ճ�Ʈ->�Ϲݳ�Ʈ, Ȥ�� �� �ݴ�� ���ϴ��� Ȯ���ϴ� �۾�
	if( classTypeCheck( notePointer->getNoteType() ) != classTypeCheck(editNoteType) )
	{
		cout << "Error! Note Class Type Error!" << endl;
		return -8;
	}

	// ��� �˻���� ������ �� ������� ���.
	return 1;
}



// ���� �� �巡�� ��Ʈ ��Ʈ ������ �ùٸ� �����ΰ��� üũ�ϴ� �Լ�.
int CNoteFile::checkNoteFormat(
	const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
	const unsigned int editPointX, const unsigned int editPointY,
	int &targetArrayNum, CNoteData** notePointer)
{
	/*
	// ���� �����Ϸ��� �ϴ� ���� �ùٸ� ������ Ȯ�� �� ����,
	if( checkNoteFormat(editNoteTimeSec, editNoteTimeMilSec, editNoteType, editTargetMarker) < 0 )
	{
		// �ùٸ� ���� �ƴ� ���,
		cout << "Error! not correct edit Value!" << endl;
		return -1;
	}
	*/

	// ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾ƿ´�.
	*notePointer = findNotePointer(orgNoteTimeSec, orgNoteTimeMilSec, NOTE_T_DRAG, targetArrayNum);
	if( (*notePointer) == NULL || targetArrayNum < 0 )
	{
		// �ش� ��Ʈ�� ã�� ������ ���
		cout << "There has no �ش� note!" << endl;
		return -2;
	}

	 //�巡�� ��Ʈ�� �´��� Ȯ���Ѵ�.
	if( (*notePointer)->getNoteType() != NOTE_T_DRAG )
	{
		cout << "Error! Target Note is not a Drag Note" << endl;
		return -9;
	}
	

	// ��� �˻���� ������ �� ������� ���.
	return 1;
}



// Ư�� ���� ��ġ�ϴ� ��Ʈ�� �ִ��� ã�� �Լ�
CNoteData* CNoteFile::findNotePointer(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker, int &targetArrayNum)
{
	// �ʿ��� �������� ����
	int i=0;
	unsigned int noteArrayNumber = noteTimeSec / SECOND_PER_ARRAY;		// ��� Array�� �־�� �� �� ����.


	// �迭�� Ž���ϸ鼭 ��� ������ ��ġ�ϴ� ��Ʈ�� �ִٸ� �� ��Ʈ�� �ּ� ���.
	for( i=0 ; (NoteAllList[noteArrayNumber][i] != NULL) && (i < MAX_HALF_ARRAY) ; i++ )
	{
		if( NoteAllList[noteArrayNumber][i]->compareNoteData(noteTimeSec, noteTimeMilSec, noteType, targetMarker) == true )
		{
			targetArrayNum = i;							// �ش� ��ġ�� �Ѱ��ش�.
			return  NoteAllList[noteArrayNumber][i];
		}
	}
	// ���� �迭�� ���� Ž�� �� ������ ��ġ�ϴ� ��Ʈ�� �߰����� ���ߴٸ�, NULL �� ����



	targetArrayNum = -1;
	return NULL;
}



// Ư�� ���� ��ġ�ϴ� ��Ʈ�� �ִ��� ã�� �Լ�
CNoteData* CNoteFile::findNotePointer(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, int &targetArrayNum)
{
	// �ʿ��� �������� ����
	int i=0;
	unsigned int noteArrayNumber = noteTimeSec / SECOND_PER_ARRAY;		// ��� Array�� �־�� �� �� ����.


	// �迭�� Ž���ϸ鼭 ��� ������ ��ġ�ϴ� ��Ʈ�� �ִٸ� �� ��Ʈ�� �ּ� ���.
	for( i=0 ; (NoteAllList[noteArrayNumber][i] != NULL) && (i < MAX_HALF_ARRAY) ; i++ )
	{
		if( NoteAllList[noteArrayNumber][i]->compareNoteData(noteTimeSec, noteTimeMilSec, noteType) == true )
		{
			targetArrayNum = i;							// �ش� ��ġ�� �Ѱ��ش�.
			return  NoteAllList[noteArrayNumber][i];
		}
	}
	// ���� �迭�� ���� Ž�� �� ������ ��ġ�ϴ� ��Ʈ�� �߰����� ���ߴٸ�, NULL �� ����



	targetArrayNum = -1;
	return NULL;
}

CNoteData* CNoteFile::findNotePointer(CNoteData *targetNote, int &targetArrayNum)
{
	// �ʿ��� �������� ����
	int i=0;
	unsigned int noteArrayNumber = targetNote->getNoteTimeSec() / SECOND_PER_ARRAY;		// ��� Array�� �־�� �� �� ����.


	// �迭�� Ž���ϸ鼭 ��� ������ ��ġ�ϴ� ��Ʈ�� �ִٸ� �� ��Ʈ�� �ּ� ���.
	for( i=0 ; (NoteAllList[noteArrayNumber][i] != NULL) && (i < MAX_HALF_ARRAY) ; i++ )
	{
		if( NoteAllList[noteArrayNumber][i]->compareNoteData(targetNote->getNoteTimeSec(), targetNote->getNoteTimeMilSec(), targetNote->getNoteType(), targetNote->getTargetMarker()) == true )
		{
			targetArrayNum = i;							// �ش� ��ġ�� �Ѱ��ش�.
			return  NoteAllList[noteArrayNumber][i];
		}
	}
	// ���� �迭�� ���� Ž�� �� ������ ��ġ�ϴ� ��Ʈ�� �߰����� ���ߴٸ�, NULL �� ����



	targetArrayNum = -1;
	return NULL;
}

// �ش��ϴ� ��Ʈ Ŭ������ ������ ��ġ�� �����ִ� �Լ�. (���������� �̹� �����ϴ� ��Ʈ ��ü��� ����)
int CNoteFile::addNoteObject(CNoteData* const targetNote)
{
	// ���� ����
	int j=0;
	
	const unsigned int noteTimeSec = targetNote->getNoteTimeSec();
	const unsigned int noteTimeMilSec = targetNote->getNoteTimeMilSec();

	const unsigned int noteArrayNumber = targetNote->getNoteTimeSec() / SECOND_PER_ARRAY;		// ��� Array�� �־�� �� �� ����.
	int arrayAddIndex = 0;				// �迭 �߰��� � ĭ�� �Է� �� �� �����ϱ� ���� �ʿ� �� �ε���.
	


	// MilSec�� ����
	// noteTimeMilSec %= 1000; �ʿ���� ��?


	// ��Ʈ ������ �´��� Ȯ���Ѵ�.
	//if( checkNoteFormat(noteTimeSec, noteTimeMilSec, noteType, targetMarker) < 0 )
	//{
	//	cout << "Error! Not Profit for Note Format!" << endl;
	//	return -1;
	//}


	// ���� �Է���a���� �ð����� �� ���� ���� �̹� �����Ѵٸ�, ������ �����ϴ��� Ȯ��,
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// �� ������ �ƴ��� ���� Ȯ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() < noteTimeSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// ���� ��������� ���� �� á�� ���, ���� ���.
			// -1�� �� ������ ���� �߰� �� �������� ��� �� ���̴�.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}
	
	// �ʴ� ������ �и��ʰ� �ٸ� �� �����Ƿ� �и��� �˻縦 �ѹ� �� �Ѵ�.
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// �� ������ �ƴ��� ���� Ȯ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() == noteTimeSec	 &&			// �� ������ �ƴ� ���, ũ�� ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeMilSec() < noteTimeMilSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// ���� ��������� ���� �� á�� ���, ���� ���.
			// -1�� �� ������ ���� �߰� �� �������� ��� �� ���̴�.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}


	// �� ū ������ �� ĭ�� �о��.
	// ���� �迭�� ���� �˾� �� ����,
	j=arrayAddIndex;					// ���⼭���� ����.

	if( NoteAllList[noteArrayNumber][j] == NULL )
	{
		// ���� �� ���� �߰��ؾ� �ϴ� ��Ȳ�� ������ ���.


	}
	else
	{
		// �߰��� ���� �־�� �� ��Ȳ�� ������ ���.

		while( NoteAllList[noteArrayNumber][j] != NULL )
		{
			j++;
			if( j >= MAX_HALF_ARRAY )
			{
				// ���� ��������� ���� �� á�� ���, ���� ���.
				cout << "Error! not enough array size 2!" << endl;
				return -1;
			}
		}

		// ���������� arrayAddIndex���� �������� �о��.
		do
		{
			NoteAllList[noteArrayNumber][j] = NoteAllList[noteArrayNumber][j-1];
			j--;
		}
		while( j > arrayAddIndex );
	}

	// arrayAddIndex�� ����Ű�� ������ ��Ʈ Ŭ���� �����ֱ�
	NoteAllList[noteArrayNumber][arrayAddIndex] = targetNote;

	// ���� ����.
	return 1;

}



// Ÿ���� �Է��ϸ� � ��Ʈ Ŭ������ ����ϴ� �� �����ϴ� �Լ�
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
		return '?';			// ����
	}

	// ����� �� ���� ����.
	return '?';
}




//////////////////////////////////////////////////////////////////////////////
//////////////////////////////     [Public]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////


// ������ �ִ� ��� ��Ʈ�� �����͸� ����ϴ� �Լ�. (-1�� ���� ���)
int CNoteFile::printAllNotesData(const int noteArrayNumber)
{	
	cout << "Note file data" << endl;

	if( noteArrayNumber < 0 )
	{
		// ���� ������ ��� ��ü ���
		for( int i=0 ; i<MAX_NOTE_ARRAY ; i++ )
		{
			cout << "Array[" << i << "]" << endl;
			for( int j=0 ; j<MAX_HALF_ARRAY && NoteAllList[i][j] != 0 ; j++ )
			{	// ��Ʈ ������ ������� �ʰų�, ������ ����Ʈ �������� ���� ��� ��� ����.
				NoteAllList[i][j]->printNoteData();
			}
			cout << endl;
		}
		
		// ��ü ��� ��, ���� ����.
		return 0;
	}
	else if( noteArrayNumber >= MAX_NOTE_ARRAY )
	{
		// ������ �ƴ�����, ������ ��Ʈ �迭�� �������� Ŭ ���, �׳� ������� �ʰ� ����.

		cout << "Please input right number Parameter!" << endl;
		
		// ������ ����
		return -1;
	}
	else
	{
		// �ش� ��Ʈ �迭�� ��� ��Ʈ ���.

		for( int j=0 ; j<MAX_HALF_ARRAY && NoteAllList[noteArrayNumber][j] != 0 ; j++ )
		{	// ��Ʈ ������ ������� �ʰų�, ������ ����Ʈ �������� ���� ��� ��� ����.
			NoteAllList[noteArrayNumber][j]->printNoteData();
		}

		// ���� ����.
		return 0;
	}


	// ġ���� ������ ����.
	return -1;	
}


// ��Ʈ�� �ϳ� �迭�� �������� �׳� �߰��ϴ� �Լ�.
int CNoteFile::addNewNoteBrute(const int noteArrayNumber, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	int j=0;

	// �� ������ ��Ÿ �� ������ Loop
	while( NoteAllList[noteArrayNumber][j] != NULL )
	{
		j++;
		
		if( j >= MAX_HALF_ARRAY )
		{
			// ���� ��������� ���� �� á�� ���, ���� ���.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}

	// �� ������ ��Ʈ Ŭ���� �߰�
	if( makeNewNote(&NoteAllList[noteArrayNumber][j], noteTimeSec, noteTimeMilSec, noteType, targetMarker) <= 0 )
	{
		// ���� üũ
		cout << "Note making Error!" << endl;
		return -1;
	}

	// ���� ����.
	return 1;
}


// ��Ʈ�� �ϳ� ������ �迭�� �߰��ϴ� �Լ�.
int CNoteFile::addNewNote(const unsigned int noteTimeSec, unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	// ���� ����
	int j=0;
	unsigned int noteArrayNumber = noteTimeSec / SECOND_PER_ARRAY;		// ��� Array�� �־�� �� �� ����.
	int arrayAddIndex = 0;				// �迭 �߰��� � ĭ�� �Է� �� �� �����ϱ� ���� �ʿ� �� �ε���.

	// MilSec�� ����
	noteTimeMilSec %= 1000;


	// ��Ʈ ������ �´��� Ȯ���Ѵ�.
	if( checkNoteFormat(noteTimeSec, noteTimeMilSec, noteType, targetMarker) < 0 )
	{
		cout << "Error! Not Profit for Note Format!" << endl;
		return -1;
	}


	// ���� �Է��Ϸ��� �ð����� �� ���� ���� �̹� �����Ѵٸ�, ������ �����ϴ��� Ȯ��,
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// �� ������ �ƴ��� ���� Ȯ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() < noteTimeSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// ���� ��������� ���� �� á�� ���, ���� ���.
			// -1�� �� ������ ���� �߰� �� �������� ��� �� ���̴�.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}
	
	// �ʴ� ������ �и��ʰ� �ٸ� �� �����Ƿ� �и��� �˻縦 �ѹ� �� �Ѵ�.
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// �� ������ �ƴ��� ���� Ȯ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() == noteTimeSec	 &&			// �� ������ �ƴ� ���, ũ�� ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeMilSec() < noteTimeMilSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// ���� ��������� ���� �� á�� ���, ���� ���.
			// -1�� �� ������ ���� �߰� �� �������� ��� �� ���̴�.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}


	// �� ū ������ �� ĭ�� �о��.
	// ���� �迭�� ���� �˾� �� ����,
	j=arrayAddIndex;					// ���⼭���� ����.

	if( NoteAllList[noteArrayNumber][j] == NULL )
	{
		// ���� �� ���� �߰��ؾ� �ϴ� ��Ȳ�� ������ ���.


	}
	else
	{
		// �߰��� ���� �־�� �� ��Ȳ�� ������ ���.

		while( NoteAllList[noteArrayNumber][j] != NULL )
		{
			j++;

			if( j >= MAX_HALF_ARRAY )
			{
				// ���� ��������� ���� �� á�� ���, ���� ���.
				cout << "Error! not enough array size 2!" << endl;
				return -1;
			}
		}

		// ���������� arrayAddIndex���� �������� �о��.
		do
		{
			NoteAllList[noteArrayNumber][j] = NoteAllList[noteArrayNumber][j-1];
			j--;
		}
		while( j > arrayAddIndex );

	}

	// arrayAddIndex�� ����Ű�� ������ ��Ʈ Ŭ���� �߰�
	if( makeNewNote(&NoteAllList[noteArrayNumber][arrayAddIndex], noteTimeSec, noteTimeMilSec, noteType, targetMarker) <= 0 )
	{
		// ���� üũ
		cout << "Note making Error!" << endl;
		return -1;
	}


	// ���� ����.
	return 1;
}


// ��Ʈ�� �ϳ� ������ �迭�� �߰��ϴ� �Լ�.
int CNoteFile::addNewNote(const unsigned int noteTimeSec, unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
	const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec)
{
	// ���� ����
	int j=0;
	unsigned int noteArrayNumber = noteTimeSec / SECOND_PER_ARRAY;		// ��� Array�� �־�� �� �� ����.
	int arrayAddIndex = 0;				// �迭 �߰��� � ĭ�� �Է� �� �� �����ϱ� ���� �ʿ� �� �ε���.

	// MilSec�� ����
	noteTimeMilSec %= 1000;


	// ��Ʈ ������ �´��� Ȯ���Ѵ�.
	if( checkNoteFormat(noteTimeSec, noteTimeMilSec, noteType, targetMarker) < 0 )
	{
		cout << "Error! Not Profit for Note Format!" << endl;
		return -1;
	}


	// ���� �Է��Ϸ��� �ð����� �� ���� ���� �̹� �����Ѵٸ�, ������ �����ϴ��� Ȯ��,
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// �� ������ �ƴ��� ���� Ȯ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() < noteTimeSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// ���� ��������� ���� �� á�� ���, ���� ���.
			// -1�� �� ������ ���� �߰� �� �������� ��� �� ���̴�.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}

	// �ʴ� ������ �и��ʰ� �ٸ� �� �����Ƿ� �и��� �˻縦 �ѹ� �� �Ѵ�.
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// �� ������ �ƴ��� ���� Ȯ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() == noteTimeSec	 &&			// �� ������ �ƴ� ���, ũ�� ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeMilSec() < noteTimeMilSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// ���� ��������� ���� �� á�� ���, ���� ���.
			// -1�� �� ������ ���� �߰� �� �������� ��� �� ���̴�.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}


	// �� ū ������ �� ĭ�� �о��.
	// ���� �迭�� ���� �˾� �� ����,
	j=arrayAddIndex;					// ���⼭���� ����.

	if( NoteAllList[noteArrayNumber][j] == NULL )
	{
		// ���� �� ���� �߰��ؾ� �ϴ� ��Ȳ�� ������ ���.


	}
	else
	{
		// �߰��� ���� �־�� �� ��Ȳ�� ������ ���.

		while( NoteAllList[noteArrayNumber][j] != NULL )
		{
			j++;

			if( j >= MAX_HALF_ARRAY )
			{
				// ���� ��������� ���� �� á�� ���, ���� ���.
				cout << "Error! not enough array size 2!" << endl;
				return -1;
			}
		}

		// ���������� arrayAddIndex���� �������� �о��.
		do
		{
			NoteAllList[noteArrayNumber][j] = NoteAllList[noteArrayNumber][j-1];
			j--;
		}
		while( j > arrayAddIndex );

	}

	// arrayAddIndex�� ����Ű�� ������ ��Ʈ Ŭ���� �߰�
	if( makeNewNote(&NoteAllList[noteArrayNumber][arrayAddIndex], noteTimeSec, noteTimeMilSec, noteType, targetMarker, noteEndTimeSec, noteEndTimeMilSec) <= 0 )
	{
		// ���� üũ
		cout << "Note making Error!" << endl;
		return -1;
	}


	// ���� ����.
	return 1;
}

// ��Ʈ�� �ϳ� ������ �迭�� �߰��ϴ� �Լ�.
int CNoteFile::addNewNote(const unsigned int noteTimeSec, unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
	const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec,
	dotData point1, dotData point2, dotData point3, dotData point4 )
{
	// ���� ����
	int j=0;
	unsigned int noteArrayNumber = noteTimeSec / SECOND_PER_ARRAY;		// ��� Array�� �־�� �� �� ����.
	int arrayAddIndex = 0;				// �迭 �߰��� � ĭ�� �Է� �� �� �����ϱ� ���� �ʿ� �� �ε���.

	// MilSec�� ����
	noteTimeMilSec %= 1000;


	// ��Ʈ ������ �´��� Ȯ���Ѵ�.
	if( checkNoteFormat(noteTimeSec, noteTimeMilSec, noteType, targetMarker) < 0 )
	{
		cout << "Error! Not Profit for Note Format!" << endl;
		return -1;
	}


	// ���� �Է��Ϸ��� �ð����� �� ���� ���� �̹� �����Ѵٸ�, ������ �����ϴ��� Ȯ��,
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// �� ������ �ƴ��� ���� Ȯ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() < noteTimeSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// ���� ��������� ���� �� á�� ���, ���� ���.
			// -1�� �� ������ ���� �߰� �� �������� ��� �� ���̴�.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}

	// �ʴ� ������ �и��ʰ� �ٸ� �� �����Ƿ� �и��� �˻縦 �ѹ� �� �Ѵ�.
	while(
		NoteAllList[noteArrayNumber][arrayAddIndex] != NULL	&&									// �� ������ �ƴ��� ���� Ȯ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeSec() == noteTimeSec	 &&			// �� ������ �ƴ� ���, ũ�� ��
		NoteAllList[noteArrayNumber][arrayAddIndex]->getNoteTimeMilSec() < noteTimeMilSec	)
	{
		arrayAddIndex++;

		if( arrayAddIndex >= (MAX_HALF_ARRAY-1) )
		{
			// ���� ��������� ���� �� á�� ���, ���� ���.
			// -1�� �� ������ ���� �߰� �� �������� ��� �� ���̴�.
			cout << "Error! not enough array size!" << endl;
			return -1;
		}
	}


	// �� ū ������ �� ĭ�� �о��.
	// ���� �迭�� ���� �˾� �� ����,
	j=arrayAddIndex;					// ���⼭���� ����.

	if( NoteAllList[noteArrayNumber][j] == NULL )
	{
		// ���� �� ���� �߰��ؾ� �ϴ� ��Ȳ�� ������ ���.


	}
	else
	{
		// �߰��� ���� �־�� �� ��Ȳ�� ������ ���.

		while( NoteAllList[noteArrayNumber][j] != NULL )
		{
			j++;

			if( j >= MAX_HALF_ARRAY )
			{
				// ���� ��������� ���� �� á�� ���, ���� ���.
				cout << "Error! not enough array size 2!" << endl;
				return -1;
			}
		}

		// ���������� arrayAddIndex���� �������� �о��.
		do
		{
			NoteAllList[noteArrayNumber][j] = NoteAllList[noteArrayNumber][j-1];
			j--;
		}
		while( j > arrayAddIndex );

	}

	// arrayAddIndex�� ����Ű�� ������ ��Ʈ Ŭ���� �߰�
	if( makeNewNote(&NoteAllList[noteArrayNumber][arrayAddIndex], noteTimeSec, noteTimeMilSec, noteType, targetMarker, noteEndTimeSec, noteEndTimeMilSec, point1, point2, point3, point4) <= 0 )
	{
		// ���� üũ
		cout << "Note making Error!" << endl;
		return -1;
	}


	// ���� ����.
	return 1;
}



//�Է¹��� ��Ʈ�� �����ϴ� �Լ�.
int CNoteFile::editNote(
	const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker
	)
{
	// �ʿ��� �������� ����
	CNoteData* notePointer = NULL;
	// �и��� ���� ��ȯ
	unsigned int orgNoteTimeMilSec2 = orgNoteTimeMilSec % 1000;
	unsigned int editNoteTimeMilSec2 = editNoteTimeMilSec % 1000;
	int targetArrayNum = -1;

	// ���� �����Ϸ��� �ϴ� ���� �ùٸ� ������ Ȯ�� �� ����, ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾Ƴ���, �ճ�Ʈ->�Ϲݳ�Ʈ, Ȥ�� �� �ݴ�� ���ϴ��� Ȯ���ϴ� �۾�
	int tempChkFlag = checkNoteFormat(orgNoteTimeSec, orgNoteTimeMilSec2, orgNoteType, orgTargetMarker, editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, targetArrayNum, &notePointer);
	if( tempChkFlag < 0 )
	{
		return tempChkFlag;
	}

	// �� ��Ʈ�� ���� ���������� ����.
	if( notePointer->editNoteData(editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker) < 0 )
	{
		// ���� �Ϲݳ�Ʈ->�ճ�Ʈ�� �ٲٷ��� �ߴٸ�,
		cout << "Error! Invalid note Type Change!" << endl;
		return -8;
	}




	// �ϴ� �ش� �迭���� ���� �� ����,
	int i = targetArrayNum+1;
	unsigned int editNoteArrayNumber = editNoteTimeSec / SECOND_PER_ARRAY;		// ���� ��� Array�� �־�� �� �� ����.
	unsigned int orgNoteArrayNumber = orgNoteTimeSec / SECOND_PER_ARRAY;		// ���� � Array�� �־�����,

	// �ش� �迭���� ����
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // ��Ʈ�� �� ���� �־��� �� �ֱ� ������ �ϴ� NULL�� �־� �ش�.

	// �� ������ �迭�� ������ �� ĭ�� ��ܿ´�.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// ������. �������� i++�� �߱� ������ i-1�� �Է��Ѵ�.

	// Ȥ�� �迭�� �ִ�ġ�� �Ѿ���� Ȯ��
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// �ٽ� �߰��Ѵ�.
	if( addNoteObject(notePointer) < 0 )
	{
		cout << "Error! �����ֱ� error" << endl;
		return -4;
	}


	return 0;
}


int CNoteFile::editNote(CNoteData *targetNote,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker)
{
		// �ʿ��� �������� ����
	unsigned int orgNoteArrayNumber = targetNote->getNoteTimeSec() / SECOND_PER_ARRAY;		// ���� � Array�� �־�����,


	//CNoteData* notePointer;
	// �и��� ���� ��ȯ
	unsigned int orgNoteTimeMilSec2 = targetNote->getNoteTimeMilSec() % 1000;
	unsigned int editNoteTimeMilSec2 = editNoteTimeMilSec % 1000;
	int targetArrayNum = -1;


	// ���� �����Ϸ��� �ϴ� ���� �ùٸ� ������ Ȯ�� �� ����, ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾Ƴ���, �ճ�Ʈ->�Ϲݳ�Ʈ, Ȥ�� �� �ݴ�� ���ϴ��� Ȯ���ϴ� �۾�
	int tempChkFlag = checkNoteFormat(targetNote, editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, targetArrayNum);
	if( tempChkFlag < 0 )
	{
		return tempChkFlag;
	}


	// �� ��Ʈ�� ���� ���������� ����.
	if( targetNote->editNoteData(editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker) < 0 )
	{
		// ���� �Ϲݳ�Ʈ -> �ճ�Ʈ �� �ٲٷ��� �ߴٸ�,
		// �� ���� ���� �ʿ� �� �κ��ΰ�???
		AfxMessageBox(_T("Error! Invalid note Type Change!"));
		return -8;
	}


	// �ϴ� �ش� �迭���� ���� �� ����,
	int i = targetArrayNum+1;
	unsigned int editNoteArrayNumber = editNoteTimeSec / SECOND_PER_ARRAY;		// ���� ��� Array�� �־�� �� �� ����.

	// �ش� �迭���� ����
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // ��Ʈ�� �� ���� �־��� �� �ֱ� ������ �ϴ� NULL�� �־� �ش�.

	// �� ������ �迭�� ������ �� ĭ�� ��ܿ´�.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// ������. �������� i++�� �߱� ������ i-1�� �Է��Ѵ�.

	// Ȥ�� �迭�� �ִ�ġ�� �Ѿ���� Ȯ��
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// �ٽ� �߰��Ѵ�.
	if( addNoteObject(targetNote) < 0 )
	{
		cout << "Error! �����ֱ� error" << endl;
		return -4;
	}


	return 0;
}




int CNoteFile::editLongNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	// �ʿ��� �������� ����
	CNoteData* notePointer;
	// �и��� ���� ��ȯ
	unsigned int orgNoteTimeMilSec2 = orgNoteTimeMilSec % 1000;
	unsigned int editNoteTimeMilSec2 = editNoteTimeMilSec % 1000;
	int targetArrayNum = -1;

	/*
	// ���� �����Ϸ��� �ϴ� ���� �ùٸ� ������ Ȯ�� �� ����,
	if( checkNoteFormat(orgNoteTimeSec, orgNoteTimeMilSec2, orgNoteType, orgTargetMarker) < 0 )
	{
		// �ùٸ� ���� �ƴ� ���,
		cout << "Error! not correct edit Value!" << endl;
		return -1;
	}


	// ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾ƿ´�.
	notePointer = findNotePointer(orgNoteTimeSec, orgNoteTimeMilSec2, orgNoteType, orgTargetMarker, targetArrayNum);
	if( notePointer == NULL || targetArrayNum < 0 )
	{
		// �ش� ��Ʈ�� ã�� ������ ���
		cout << "There has no �ش� note!" << endl;
		return -2;
	}


	// �ճ�Ʈ->�Ϲݳ�Ʈ, Ȥ�� �� �ݴ�� ���ϴ��� Ȯ���ϴ� �۾�
	if( classTypeCheck(orgNoteType) != classTypeCheck(editNoteType) )
	{
		cout << "Error! Note Class Type Error!" << endl;
		return -8;
	}
	*/

	// ���� �����Ϸ��� �ϴ� ���� �ùٸ� ������ Ȯ�� �� ����, ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾Ƴ���, �ճ�Ʈ->�Ϲݳ�Ʈ, Ȥ�� �� �ݴ�� ���ϴ��� Ȯ���ϴ� �۾�
	int tempChkFlag = checkNoteFormat(orgNoteTimeSec, orgNoteTimeMilSec2, orgNoteType, orgTargetMarker, editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, targetArrayNum, &notePointer);
	if( tempChkFlag < 0 )
	{
		return tempChkFlag;
	}


	// �� ��Ʈ�� ���� ���������� ����.
	if( notePointer->editNoteData(editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, editNoteEndTimeSec, editNoteEndTimeMilSec) < 0 )
	{
		// ���� �Ϲݳ�Ʈ -> �ճ�Ʈ �� �ٲٷ��� �ߴٸ�,
		// �� ���� ���� �ʿ� �� �κ��ΰ�???
		cout << "Error! Invalid note Type Change!" << endl;
		return -8;
	}


	// �ϴ� �ش� �迭���� ���� �� ����,
	int i = targetArrayNum+1;
	unsigned int editNoteArrayNumber = editNoteTimeSec / SECOND_PER_ARRAY;		// ���� ��� Array�� �־�� �� �� ����.
	unsigned int orgNoteArrayNumber = orgNoteTimeSec / SECOND_PER_ARRAY;		// ���� � Array�� �־�����,

	// �ش� �迭���� ����
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // ��Ʈ�� �� ���� �־��� �� �ֱ� ������ �ϴ� NULL�� �־� �ش�.

	// �� ������ �迭�� ������ �� ĭ�� ��ܿ´�.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// ������. �������� i++�� �߱� ������ i-1�� �Է��Ѵ�.

	// Ȥ�� �迭�� �ִ�ġ�� �Ѿ���� Ȯ��
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// �ٽ� �߰��Ѵ�.
	if( addNoteObject(notePointer) < 0 )
	{
		cout << "Error! �����ֱ� error" << endl;
		return -4;
	}


	return 0;
}


int CNoteFile::editLongNote(CNoteData *targetNote,
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	
	// �ʿ��� �������� ����
	//CNoteData* notePointer;
	// �и��� ���� ��ȯ
	unsigned int orgNoteTimeMilSec2 = targetNote->getNoteTimeMilSec() % 1000;
	unsigned int editNoteTimeMilSec2 = editNoteTimeMilSec % 1000;
	int targetArrayNum = -1;
	
	// ���� �����Ϸ��� �ϴ� ���� �ùٸ� ������ Ȯ�� �� ����, ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾Ƴ���, �ճ�Ʈ->�Ϲݳ�Ʈ, Ȥ�� �� �ݴ�� ���ϴ��� Ȯ���ϴ� �۾�
	int tempChkFlag = checkNoteFormat(targetNote,
		editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, targetArrayNum);
	if( tempChkFlag < 0 )
	{
		return tempChkFlag;
	}


	// �� ��Ʈ�� ���� ���������� ����.
	if( targetNote->editNoteData(editNoteTimeSec, editNoteTimeMilSec2, editNoteType, editTargetMarker, editNoteEndTimeSec, editNoteEndTimeMilSec) < 0 )
	{
		// ���� �Ϲݳ�Ʈ -> �ճ�Ʈ �� �ٲٷ��� �ߴٸ�,
		// �� ���� ���� �ʿ� �� �κ��ΰ�???
		cout << "Error! Invalid note Type Change!" << endl;
		return -8;
	}


	// �ϴ� �ش� �迭���� ���� �� ����,
	int i = targetArrayNum+1;
	unsigned int editNoteArrayNumber = editNoteTimeSec / SECOND_PER_ARRAY;		// ���� ��� Array�� �־�� �� �� ����.
	unsigned int orgNoteArrayNumber = targetNote->getNoteTimeSec() / SECOND_PER_ARRAY;		// ���� � Array�� �־�����,

	// �ش� �迭���� ����
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // ��Ʈ�� �� ���� �־��� �� �ֱ� ������ �ϴ� NULL�� �־� �ش�.

	// �� ������ �迭�� ������ �� ĭ�� ��ܿ´�.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// ������. �������� i++�� �߱� ������ i-1�� �Է��Ѵ�.

	// Ȥ�� �迭�� �ִ�ġ�� �Ѿ���� Ȯ��
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// �ٽ� �߰��Ѵ�.
	if( addNoteObject(targetNote) < 0 )
	{
		cout << "Error! �����ֱ� error" << endl;
		return -4;
	}


	return 0;
}



// �Է¹��� �巡�� ��Ʈ�� �� ��ǥ���� �����ϴ� �Լ�.
int CNoteFile::editDragNotePoint(
	const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
	const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPointY )
{
	// �ʿ��� �������� ����
	CNoteData* notePointer;
	// �и��� ���� ��ȯ
	unsigned int orgNoteTimeMilSec2 = orgNoteTimeMilSec % 1000;
	int targetArrayNum = -1;


	// ���� �����Ϸ��� �ϴ� ���� �ùٸ� ������ Ȯ�� �� ����, ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾Ƴ���, �ճ�Ʈ->�Ϲݳ�Ʈ, Ȥ�� �� �ݴ�� ���ϴ��� Ȯ���ϴ� �۾�
	int tempChkFlag = checkNoteFormat(orgNoteTimeSec, orgNoteTimeMilSec2, newPointX, newPointY, targetArrayNum, &notePointer);
	if( tempChkFlag < 0 )
	{
		return tempChkFlag;
	}


	// �� ��Ʈ�� ���� ���������� ����.
	if( notePointer->editNoteData(pointNumber, newPointX, newPointY) < 0 )
	{
		// ���� �Ϲݳ�Ʈ -> �ճ�Ʈ �� �ٲٷ��� �ߴٸ�,
		// �� ���� ���� �ʿ� �� �κ��ΰ�???
		cout << "Error! Invalid note Type Change!" << endl;
		return -8;
	}

	// �ܼ� �巡�� ��Ʈ�� ���� �����ϴ� ���̱� ������, ���� �����ִ� �۾��� ���� �ʴ´�.


	//// �ϴ� �ش� �迭���� ���� �� ����,
	//int i = targetArrayNum+1;
	//unsigned int editNoteArrayNumber = editNoteTimeSec / SECOND_PER_ARRAY;		// ���� ��� Array�� �־�� �� �� ����.
	//unsigned int orgNoteArrayNumber = orgNoteTimeSec / SECOND_PER_ARRAY;		// ���� � Array�� �־�����,

	//// �ش� �迭���� ����
	//NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // ��Ʈ�� �� ���� �־��� �� �ֱ� ������ �ϴ� NULL�� �־� �ش�.

	//// �� ������ �迭�� ������ �� ĭ�� ��ܿ´�.
	//while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	//{
	//	NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

	//	i++;
	//}
	//NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// ������. �������� i++�� �߱� ������ i-1�� �Է��Ѵ�.

	//// Ȥ�� �迭�� �ִ�ġ�� �Ѿ���� Ȯ��
	//if( i >= MAX_HALF_ARRAY )
	//{
	//	cout << "Error! Out of Array Range!" << endl;
	//	return -10;
	//}


	//// �ٽ� �߰��Ѵ�.
	//if( addNoteObject(notePointer) < 0 )
	//{
	//	cout << "Error! �����ֱ� error" << endl;
	//	return -4;
	//}

	return 0;
}


// �Է¹��� ��Ʈ�� �����ϴ� �Լ�.
int CNoteFile::deleteNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker)
{
	// �ʿ��� �������� ����
	CNoteData* notePointer;
	// �и��� ���� ��ȯ
	unsigned int orgNoteTimeMilSec2 = orgNoteTimeMilSec % 1000;
	int targetArrayNum = -1;


	// ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾ƿ´�.
	notePointer = findNotePointer(orgNoteTimeSec, orgNoteTimeMilSec2, orgNoteType, orgTargetMarker, targetArrayNum);
	if( notePointer == NULL || targetArrayNum < 0 )
	{
		// �ش� ��Ʈ�� ã�� ������ ���
		cout << "There has no �ش� note!" << endl;
		return -2;
	}


	// �ϴ� �ش� �迭���� ���� �� ����,
	int i = targetArrayNum+1;
	unsigned int orgNoteArrayNumber = orgNoteTimeSec / SECOND_PER_ARRAY;		// ���� � Array�� �־�����,

	// �ش� �迭���� ����
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // ��Ʈ�� �� ���� �־��� �� �ֱ� ������ �ϴ� NULL�� �־� �ش�.


	// �� ������ �迭�� ������ �� ĭ�� ��ܿ´�.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// ������. �������� i++�� �߱� ������ i-1�� �Է��Ѵ�.


	// Ȥ�� �迭�� �ִ�ġ�� �Ѿ���� Ȯ��
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// ���Ⱑ�� ������ ���� �Ϸᰡ �� ���̹Ƿ�, ��Ʈ Ŭ���� ����
	delete notePointer;

	return 0;
}

int CNoteFile::deleteNote(CNoteData *targetNote)
{

	// �ʿ��� �������� ����
	//CNoteData* notePointer;
	// �и��� ���� ��ȯ
	unsigned int orgNoteTimeMilSec2 = targetNote->getNoteTimeMilSec() % 1000;
	int targetArrayNum = -1;


	// ���� �����ϴ� ���� ������ �ִ� ��Ʈ�� �ּҸ� �˾ƿ´�.
	findNotePointer(targetNote, targetArrayNum);
	if( targetNote == NULL || targetArrayNum < 0 )
	{
		// �ش� ��Ʈ�� ã�� ������ ���
		cout << "There has no �ش� note!" << endl;
		return -2;
	}


	// �ϴ� �ش� �迭���� ���� �� ����,
	int i = targetArrayNum+1;
	unsigned int orgNoteArrayNumber = targetNote->getNoteTimeSec() / SECOND_PER_ARRAY;		// ���� � Array�� �־�����,

	// �ش� �迭���� ����
	NoteAllList[orgNoteArrayNumber][targetArrayNum] = NULL; // ��Ʈ�� �� ���� �־��� �� �ֱ� ������ �ϴ� NULL�� �־� �ش�.


	// �� ������ �迭�� ������ �� ĭ�� ��ܿ´�.
	while( NoteAllList[orgNoteArrayNumber][i] != NULL && i < MAX_HALF_ARRAY )
	{
		NoteAllList[orgNoteArrayNumber][i-1] = NoteAllList[orgNoteArrayNumber][i];

		i++;
	}
	NoteAllList[orgNoteArrayNumber][i-1] = NULL;				// ������. �������� i++�� �߱� ������ i-1�� �Է��Ѵ�.


	// Ȥ�� �迭�� �ִ�ġ�� �Ѿ���� Ȯ��
	if( i >= MAX_HALF_ARRAY )
	{
		cout << "Error! Out of Array Range!" << endl;
		return -10;
	}


	// ���Ⱑ�� ������ ���� �Ϸᰡ �� ���̹Ƿ�, ��Ʈ Ŭ���� ����
	//delete notePointer;

	return 0;
}

// ������ �ִ� ��� ��Ʈ���� ����
int CNoteFile::clearAllNotes(void)
{
	int i, j;

	// �޸𸮿� �÷����� ��Ʈ���ϵ� ��� ����
	for( i=0 ; i< MAX_NOTE_ARRAY ; i++ )
	{
		for( j=0 ; j < MAX_HALF_ARRAY ; j++ )
		{
			delete NoteAllList[i][j];
			NoteAllList[i][j] = NULL;
		}
	}

	// ��Ÿ ������ �ʱ�ȭ
	this->noteFileVersion = 100;				// Version 1.00

	this->noteFileTitle = "Untitled";			// ����
	this->noteFileArtist = "Unknown";			// ���ǰ�

	this->noteFileLevel = 1;					// �⺻ ���̵��� �׻� 1

	this->mp3FileName = "";						// mp3���� ����
	this->titlePicture = "";					// Ÿ��Ʋ �׸� ���

	// ���� �ð�
	startSongTime.noteTimeSec = 0;
	startSongTime.noteTimeMilSec = 0;

	// ���� �ð� (���Ƿ� 2������ ����)
	endSongTime.noteTimeSec = 120;
	endSongTime.noteTimeMilSec = 0;


	this->noteFileInitBPM = 120;


	return 1;
}






// GET �Լ���
// ��Ʈ �迭���� ��Ʈ�� �ּҰ��� �޾Ƴ��� �Լ�.
CNoteData* CNoteFile::getNoteAddr(const unsigned int i, const unsigned int j)
{
	if( i >= MAX_NOTE_ARRAY || j >= MAX_HALF_ARRAY )
	{
		// ���� ���� �̻��� ���.
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





// SET �Լ���
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


