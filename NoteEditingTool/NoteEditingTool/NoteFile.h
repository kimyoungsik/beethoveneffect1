/*********************************************************************************************************************************************
				�� ���亥 ����Ʈ ������Ʈ ��ȭ�� ��
				~ ��Ʈ ���� ������ �ھ� ���ۺκ� ~

	121113 - ��Ʈ ���� �Ϸ�,
	121123 - ��Ʈ ���� �κп��� Long->Normal�� �� �ٲٵ��� ���� �κ��� ������ �� �ؾ� �� �� ����.
	121124 - �� �Ϸ�.
	121124 - �巡�� ��Ʈ ���� ��ǥ ���� �籸�� ���.
	121129 - �� �Ϸ�.




*********************************************************************************************************************************************/
#pragma once

//#include <iostream>
#include <string>
#include "LongNoteData.h"
#include "DragNoteData.h"

// ��� ����
const unsigned int MAX_NOTE_ARRAY = 30;			// �� 30�ʸ����� ��Ʈ�� ���� �� �迭�� �����ϴ� ����Ʈ�� �ִ� ����.
const unsigned int MAX_HALF_ARRAY = 100;			// �� 30�ʸ����� ��Ʈ�� ���� �� �迭�� �ִ� ����.	// ������ ���� ����.
const unsigned int SECOND_PER_ARRAY = 30;		// �� �� �������� ������ ARRAY���� ���� ������ �����ϴ� ���.






// standard string �迭 ���
using std::string;
using std::cout;
using std::cin;
using std::endl;



// �ð� ����ü
struct NoteTime
{
	unsigned int noteTimeSec;			// ��Ʈ�� �ľ� �ϴ� �ð� (���� : ��)
	unsigned int noteTimeMilSec;		// ��Ʈ�� �ľ� �ϴ� �ð� (���� : �и� ��)

	NoteTime& operator = (const NoteTime& newTime)
	{
		this->noteTimeSec = newTime.noteTimeSec;
		this->noteTimeMilSec = newTime.noteTimeMilSec;
		return *this;
	}
};



class CNoteFile
{
// ��ü���� ��Ʈ ������ ������ �����ϴ� Ŭ����.

// Ư���Դϴ�.
private:

	unsigned int noteFileVersion;	// ��Ʈ ���� ����

	string noteFileTitle;			// ����
	string noteFileArtist;			// ���ǰ�
	
	unsigned int noteFileLevel;		// ���̵�


	string mp3FileName;				// mp3���� ����
	string titlePicture;			// Ÿ��Ʋ �׸� ���

	NoteTime startSongTime;			// ���� �ð�
	NoteTime endSongTime;			// ���� �ð�

	unsigned int noteFileInitBPM;	// ���� bpm



	// ��Ʈ�� �����ϴ� ������. 
	CNoteData *NoteAllList[MAX_NOTE_ARRAY][MAX_HALF_ARRAY];				// ��Ʈ ������ �����ϴ� ����Ʈ�� �����ϴ� ��ü ����Ʈ.
	//CNoteData *NoteHalfList[MAX_HALF_ARRAY];				// ��Ʈ�� �����ϰ� �ִ� ����Ʈ



// ����Դϴ�.
private:
	// Ư�� �ּҿ� ������ ��Ʈ�� �����ؼ� �迭�� ��ũ ��Ų��.
	int makeNewNote(CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	int makeNewNote(
		CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec );
	int makeNewNote(
		CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec,
		dotData point1, dotData point2, dotData point3, dotData point4 );
	// ���� ���� ��Ʈ ������ �ùٸ� �����ΰ��� üũ�ϴ� �Լ�.
	int checkNoteFormat(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	int checkNoteFormat(
		CNoteData *notePointer,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker, int &targetArrayNum);
	// ���� �� ��Ʈ ������ �ùٸ� �����ΰ��� üũ�ϴ� �Լ�.
	int checkNoteFormat(
		const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		int &targetArrayNum, CNoteData** notePointer);
	// �������� �� ��Ʈ ������ �ùٸ� �����ΰ��� üũ�ϴ� �Լ�.
	int checkNoteFormat(
		const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
		const unsigned int editPointX, const unsigned int editPointY,
		int &targetArrayNum, CNoteData** notePointer);

	// Ư�� ���� ��ġ�ϴ� ��Ʈ�� �ִ��� ã�� �Լ�
	CNoteData* findNotePointer(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker, int &targetArrayNum);
	// ���� Ư�� ���� ��ġ�ϴ� ��Ʈ�� �ִ��� ã�� �Լ�
	CNoteData* findNotePointer(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, int &targetArrayNum);
	CNoteData* findNotePointer(CNoteData *targetNote, int &targetArrayNum);

	// �ش��ϴ� ��Ʈ Ŭ������ ������ ��ġ�� �����ִ� �Լ�. (���������� �̹� �����ϴ� ��Ʈ ��ü��� ����)
	int addNoteObject(CNoteData* const targetNote);
	// Ÿ���� �Է��ϸ� � ��Ʈ Ŭ������ ����ϴ� �� �����ϴ� �Լ�
	char classTypeCheck(const char noteType);



public:
	// ������ & �Ҹ���
	CNoteFile(void);
	~CNoteFile(void);



	// �Լ���
	int printAllNotesData(const int noteArrayNumber);		// ������ �ִ� ��� ��Ʈ�� �����͸� ����ϴ� �Լ�. (-1�� ���� ���)
	int addNewNoteBrute(const int noteArrayNumber, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);			// ��Ʈ�� �ϳ� �迭�� �������� �׳� �߰��ϴ� �Լ�.
	int addNewNote(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);					// ��Ʈ�� �ϳ� ������ �迭�� �߰��ϴ� �Լ�.
	int addNewNote(const unsigned int noteTimeSec, unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec	);					// ��Ʈ�� �ϳ� ������ �迭�� �߰��ϴ� �Լ�.
	int addNewNote(const unsigned int noteTimeSec, unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec,
		dotData point1, dotData point2, dotData point3, dotData point4 );
	// �Է¹��� �Ϲ� ��Ʈ�� �����ϴ� �Լ�.
	int editNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker);
	int editNote(CNoteData *targetNote,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker);
	// �Է¹��� Ư�� ��Ʈ �κ��� �����ϴ� �Լ�.
	int editLongNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	int editLongNote(CNoteData *targetNote,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	// �Է¹��� �巡�� ��Ʈ�� �� ��ǥ���� �����ϴ� �Լ�.
	int editDragNotePoint(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
				const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPointY);
	// �Է¹��� ��Ʈ�� �����ϴ� �Լ�.
	int deleteNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker);
	// �Է¹��� ��Ʈ�� �����ϴ� �Լ�.
	int deleteNote(CNoteData *targetNote);
	// ������ �ִ� ��� ��Ʈ���� ����
	int clearAllNotes(void);




	// GET �Լ���
	// ��Ʈ �迭���� ��Ʈ�� �ּҰ��� �޾Ƴ��� �Լ�.
	CNoteData* getNoteAddr(const unsigned int i, const unsigned int j);
	unsigned int getNoteFileVersion(void);
	string getNoteFileTitle(void);
	string getNoteFileArtist(void);
	unsigned int getNoteFileLevel(void);
	string getMp3FileName(void);
	string getTitlePicture(void);
	NoteTime getStartSongTime(void);
	NoteTime getEndSongTime(void);
	unsigned int getNoteFileInitBPM(void);

	// SET �Լ���
	void setNoteFileVersion(const unsigned int noteFileVersion);
	void setNoteFileTitle(string noteFileTitle);
	void setNoteFileArtist(string noteFileArtist);
	void setNoteFileLevel(const unsigned int noteFileLevel);
	void setMp3FileName(string mp3FileName);
	void setTitlePicture(string titlePicture);
	void setStartSongTime(NoteTime startSongTime);
	void setEndSongTime(NoteTime endSongTime);
	void setNoteFileInitBPM(unsigned int noteFileInitBPM);

};

