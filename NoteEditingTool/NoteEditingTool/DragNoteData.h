#pragma once

#include "notedata.h"

// 상수 정의
const unsigned int MAX_DOT_DATA_NUM = 5;
const unsigned int DEFAULT_DRAG_SEC = 5;
const unsigned int DEFAULT_DRAG_MILSEC = 0;


// std Use
using std::string;
using std::cout;
using std::cin;
using std::endl;






class CDragNoteData :
	public CNoteData
{
// 속성입니다.
// 드래그 노트를 위한 클래스
private:
	unsigned int noteEndTimeSec;			// 노트가 끝나는 시간 (단위 : 초)
	unsigned int noteEndTimeMilSec;			// 노트가 끝나는 시간 (단위 : 밀리 초)

	dotData dotDataArray[MAX_DOT_DATA_NUM];			// 베지에 곡선 유도점을 저장하는 배열




public:
	// 생성자 & 소멸자
	CDragNoteData(void);
	CDragNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	CDragNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec, const dotData point1, const dotData point2, const dotData point3, const dotData point4);
	~CDragNoteData(void);

// 특성입니다.
public:
	
	// 가상함수들
	char getClassType(void);			// 어떤 클래스의 노트인지를 알 수 있게 해 주는 함수.
	void printNoteData(void);			// 노트 정보를 한 줄로 출력할 때 사용하는 함수
	// 스페셜 노트 정보를 수정할 때 사용하는 가상함수.
	int editNoteData(
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	// 입력받은 드래그 노트의 각 좌표점을 수정하는 함수.
	int editNoteData(
		const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPoinY);
	// 노트의 종료 시간을 리턴
	virtual unsigned int getNoteEndTimeSec(void);
	virtual unsigned int getNoteEndTimeMilSec(void);
	// 해당 번호의 드래그 노트의 좌표를 리턴한다.
	virtual int getDragPoint(const int pointNum, dotData &point);
	// 해당 번호의 드래그 노트의 좌표를 입력한다.
	virtual int setDragPoint(const int pointNum, dotData &point);

};

