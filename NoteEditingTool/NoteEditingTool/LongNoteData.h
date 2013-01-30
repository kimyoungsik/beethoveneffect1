#pragma once

#include "NoteData.h"


// 상수 정의
const unsigned int DEFAULT_SEC_RANGE = 3;
const unsigned int DEFAULT_MILSEC_RANGE = 500;




// std Use
using std::string;
using std::cout;
using std::cin;
using std::endl;





class CLongNoteData :
	public CNoteData
{
	// 롱노트, 패턴 체인지, 카리스마 타임, 뉴트럴 포지션 클래스
private:
	unsigned int noteEndTimeSec;			// 노트가 끝나는 시간 (단위 : 초)
	unsigned int noteEndTimeMilSec;			// 노트가 끝나는 시간 (단위 : 밀리 초)







public:
	// 생성자 & 소멸자
	CLongNoteData(void);
	CLongNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	CLongNoteData(
		const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec);
	~CLongNoteData(void);


	// 가상함수들
	char getClassType(void);			// 어떤 클래스의 노트인지를 알 수 있게 해 주는 함수.
	void printNoteData(void);			// 노트 정보를 한 줄로 출력할 때 사용하는 함수
	// 스페셜 노트 정보를 수정할 때 사용하는 가상함수.
	virtual int editNoteData(
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	// 노트의 종료 시간을 리턴
	virtual unsigned int getNoteEndTimeSec(void);
	virtual unsigned int getNoteEndTimeMilSec(void);
	// 해당 번호의 드래그 노트의 좌표를 리턴한다.
	virtual int getDragPoint(const int pointNum, dotData &point);
	// 해당 번호의 드래그 노트의 좌표를 리턴한다.
	virtual int setDragPoint(const int pointNum, dotData &point);



	// 사용 함수들


};

