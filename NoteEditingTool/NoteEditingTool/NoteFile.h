/*********************************************************************************************************************************************
				『 베토벤 이펙트 프로젝트 김화민 』
				~ 노트 파일 에디터 코어 제작부분 ~

	121113 - 노트 삽입 완료,
	121123 - 노트 수정 부분에서 Long->Normal로 못 바꾸도록 막는 부분의 구현을 더 해야 할 것 같다.
	121124 - ┖ 완료.
	121124 - 드래그 노트 내부 좌표 수정 재구현 요망.
	121129 - ┖ 완료.




*********************************************************************************************************************************************/
#pragma once

//#include <iostream>
#include <string>
#include "LongNoteData.h"
#include "DragNoteData.h"

// 상수 정의
const unsigned int MAX_NOTE_ARRAY = 30;			// 매 30초마다의 노트를 저장 할 배열을 저장하는 리스트의 최대 개수.
const unsigned int MAX_HALF_ARRAY = 100;			// 매 30초마다의 노트를 저장 할 배열의 최대 개수.	// 성능을 위해 구현.
const unsigned int SECOND_PER_ARRAY = 30;		// 몇 초 기준으로 각각의 ARRAY들을 만들 것인지 결정하는 상수.






// standard string 배열 사용
using std::string;
using std::cout;
using std::cin;
using std::endl;



// 시간 구조체
struct NoteTime
{
	unsigned int noteTimeSec;			// 노트를 쳐야 하는 시간 (단위 : 초)
	unsigned int noteTimeMilSec;		// 노트를 쳐야 하는 시간 (단위 : 밀리 초)

	NoteTime& operator = (const NoteTime& newTime)
	{
		this->noteTimeSec = newTime.noteTimeSec;
		this->noteTimeMilSec = newTime.noteTimeMilSec;
		return *this;
	}
};



class CNoteFile
{
// 전체적인 노트 파일의 정보를 저장하는 클래스.

// 특성입니다.
private:

	unsigned int noteFileVersion;	// 노트 파일 버전

	string noteFileTitle;			// 제목
	string noteFileArtist;			// 음악가
	
	unsigned int noteFileLevel;		// 난이도


	string mp3FileName;				// mp3파일 제목
	string titlePicture;			// 타이틀 그림 경로

	NoteTime startSongTime;			// 시작 시간
	NoteTime endSongTime;			// 종료 시간

	unsigned int noteFileInitBPM;	// 최초 bpm



	// 노트를 저장하는 변수들. 
	CNoteData *NoteAllList[MAX_NOTE_ARRAY][MAX_HALF_ARRAY];				// 노트 정보를 저장하는 리스트를 저장하는 전체 리스트.
	//CNoteData *NoteHalfList[MAX_HALF_ARRAY];				// 노트를 저장하고 있는 리스트



// 기능입니다.
private:
	// 특정 주소에 적절한 노트를 생성해서 배열에 링크 시킨다.
	int makeNewNote(CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	int makeNewNote(
		CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec );
	int makeNewNote(
		CNoteData** targetAddr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec,
		dotData point1, dotData point2, dotData point3, dotData point4 );
	// 새로 만들 노트 형식이 올바른 형식인가를 체크하는 함수.
	int checkNoteFormat(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	int checkNoteFormat(
		CNoteData *notePointer,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker, int &targetArrayNum);
	// 수정 할 노트 형식이 올바른 형식인가를 체크하는 함수.
	int checkNoteFormat(
		const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		int &targetArrayNum, CNoteData** notePointer);
	// ┗━수정 할 노트 형식이 올바른 형식인가를 체크하는 함수.
	int checkNoteFormat(
		const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
		const unsigned int editPointX, const unsigned int editPointY,
		int &targetArrayNum, CNoteData** notePointer);

	// 특정 값이 일치하는 노트가 있는지 찾는 함수
	CNoteData* findNotePointer(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker, int &targetArrayNum);
	// ┗━ 특정 값이 일치하는 노트가 있는지 찾는 함수
	CNoteData* findNotePointer(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, int &targetArrayNum);
	CNoteData* findNotePointer(CNoteData *targetNote, int &targetArrayNum);

	// 해당하는 노트 클래스를 적절한 위치에 끼워넣는 함수. (정상적으로 이미 존재하는 노트 객체라는 가정)
	int addNoteObject(CNoteData* const targetNote);
	// 타입을 입력하면 어떤 노트 클래스를 사용하는 지 리턴하는 함수
	char classTypeCheck(const char noteType);



public:
	// 생성자 & 소멸자
	CNoteFile(void);
	~CNoteFile(void);



	// 함수들
	int printAllNotesData(const int noteArrayNumber);		// 가지고 있는 모든 노트의 데이터를 출력하는 함수. (-1은 전부 출력)
	int addNewNoteBrute(const int noteArrayNumber, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);			// 노트를 하나 배열의 마지막에 그냥 추가하는 함수.
	int addNewNote(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);					// 노트를 하나 적절한 배열에 추가하는 함수.
	int addNewNote(const unsigned int noteTimeSec, unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec	);					// 노트를 하나 적절한 배열에 추가하는 함수.
	int addNewNote(const unsigned int noteTimeSec, unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec,
		dotData point1, dotData point2, dotData point3, dotData point4 );
	// 입력받은 일반 노트를 수정하는 함수.
	int editNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker);
	int editNote(CNoteData *targetNote,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker);
	// 입력받은 특수 노트 부분을 수정하는 함수.
	int editLongNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	int editLongNote(CNoteData *targetNote,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	// 입력받은 드래그 노트의 각 좌표점을 수정하는 함수.
	int editDragNotePoint(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
				const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPointY);
	// 입력받은 노트를 삭제하는 함수.
	int deleteNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker);
	// 입력받은 노트를 삭제하는 함수.
	int deleteNote(CNoteData *targetNote);
	// 가지고 있는 모든 노트들을 삭제
	int clearAllNotes(void);




	// GET 함수들
	// 노트 배열에서 노트의 주소값을 받아내는 함수.
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

	// SET 함수들
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

