#pragma once
#include "notedata.h"





using std::string;
using std::cout;
using std::cin;
using std::endl;



class CNormalNoteData :
	public CNoteData
{
	// 오른손 노트와 같은 일반적은 노트의 클래스
private:
	int targetMarker;			// 노트가 가려고 하는 타겟 마커 (1~6까지 있다.)



public:
	CNormalNoteData(unsigned int noteTimeSec, unsigned int noteTimeMilSec, char noteType, int targetMarker);
	~CNormalNoteData(void);


	// GET 함수들
	int getTargetMarker(void);


	// SET 함수들
	void setTargetMarker(int targetMarker);

};


