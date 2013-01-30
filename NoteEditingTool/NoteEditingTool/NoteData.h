#pragma once

#include <iostream>



// 노트 타입 정의. 여기서 정한 것이 파일에 출력 될 때 사용된다.
const char NOTE_T_RIGHT = '1';
const char NOTE_T_LEFT = '2';
const char NOTE_T_LONG = '4';
const char NOTE_T_DRAG = 'D';					// Drag Note
const char NOTE_T_CHARISMA = 'C';				// Charisma Time
const char NOTE_T_NEUTRAL = 'N';				// Neutral Position
const char NOTE_T_PATTERN = 'P';				// Pattern Change
const char NOTE_T_BPM = 'B';					// BPM Change
const char NOTE_T_PHOTO = 'H';					// Photo Time



// 마커 이름 정의. 여기서 정한 것이 파일에 출력 될 때 사용된다.
const char MARKER_NUM1 = '1';
const char MARKER_NUM2 = '2';
const char MARKER_NUM3 = '3';
const char MARKER_NUM4 = '4';
const char MARKER_NUM5 = '5';
const char MARKER_NUM6 = '6';






using std::string;
using std::cout;
using std::cin;
using std::endl;


// 베지에 곡선 점들을 기록하는 구조체
struct dotData{
	int x;
	int y;
};


class CNoteData
{
// 개별 노트를 관리하는 부모 클래스.
// 오른손, 왼손, BPM Change를 관리한다.
protected:
	unsigned int noteTimeSec;			// 노트를 쳐야 하는 시간 (단위 : 초)
	unsigned int noteTimeMilSec;		// 노트를 쳐야 하는 시간 (단위 : 밀리 초)

	char noteType;						// 노트의 종류.
	char targetMarker;					// 타겟 마커 (1~6까지의 값을 가진다. 'n'의 경우 타겟을 가지지 않는 노트임을 의미한다.)
	// BPM Change의 경우 (unsigned int)noteType * (unsigned int)targetMarker 의 값으로 변경한다.





// 기능입니다.
private:


public:
	// 생성자 & 소멸자
	CNoteData(void);
	CNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	~CNoteData(void);


	///////// 인터페이스용 가상 함수들
	virtual char getClassType(void);			// 어떤 클래스의 노트인지를 알 수 있게 해 주는 함수.
	virtual void printNoteData(void);			// 노트 정보를 한 줄로 출력할 때 사용하는 함수
	// 노트 정보를 수정할 때 사용하는 가상함수
	virtual int editNoteData(
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker);
	// 스페셜 노트 정보를 수정할 때 사용하는 가상함수.
	virtual int editNoteData(
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	// 입력받은 드래그 노트의 각 좌표점을 수정하는 함수.
	virtual int editNoteData(
		const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPoinY);
	// 노트의 종료 시간을 리턴
	virtual unsigned int getNoteEndTimeSec(void);
	virtual unsigned int getNoteEndTimeMilSec(void);
	// 해당 번호의 드래그 노트의 좌표를 리턴한다.
	virtual int getDragPoint(const int pointNum, dotData &point);
	// 해당 번호의 드래그 노트의 좌표를 수정한다.
	virtual int setDragPoint(const int pointNum, dotData &point);


	//////// 일반 함수들
	void printNoteBasicData(void);				// 노트의 기본 정보들을 출력하는 함수.

	// 노트의 값과 일치한다면 True를 출력하는 함수
	bool compareNoteData(const unsigned int cNoteTimeSec, const unsigned int cNoteTimeMilSec, const char cNoteType, const char cTargetMarker);
	// ┗━ 노트의 값과 일치한다면 True를 출력하는 함수
	bool compareNoteData(const unsigned int cNoteTimeSec, const unsigned int cNoteTimeMilSec, const char cNoteType);



	// GET 함수들.
	CNoteData getBasicNoteData(void);				// 노트 정보를 리턴받는 함수.
	unsigned int getNoteTimeSec(void);
	unsigned int getNoteTimeMilSec(void);
	char getNoteType(void);
	char getTargetMarker(void);



	// SET 함수들.
	void setNoteTimeSec(const unsigned int second);
	void setNoteTimeMilSec(const unsigned int miliSecond);
	void setNoteType(const char m_noteType);
	void setTargetMarker(const char m_targetMarker);
};

