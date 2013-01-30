
#include "stdafx.h"
#include "LongNoteData.h"




CLongNoteData::CLongNoteData(void)
{
	// 생성자
}

CLongNoteData::CLongNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	// 생성자.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = (noteTimeMilSec%1000);			// 밀리초의 범위는 0~999이다.
	this->noteType = noteType;
	this->targetMarker = targetMarker;

	// 노트의 끝나는 시간의 기본은 적정 시간 뒤로 보낸다.
	noteEndTimeSec = noteTimeSec + DEFAULT_SEC_RANGE;
	noteEndTimeMilSec = noteTimeMilSec + DEFAULT_MILSEC_RANGE;
	if (noteTimeMilSec >= 1000)
	{
		noteEndTimeSec += (noteEndTimeMilSec/1000);
		noteEndTimeMilSec %= 1000;
	}
	
	//if (noteTimeMilSec >= 500)
	//{
	//	noteEndTimeSec = (this->noteTimeSec) + 1;
	//	noteEndTimeMilSec = (this->noteTimeMilSec) + 500 - 1000;
	//}
	//else
	//{
	//	noteEndTimeSec = (this->noteTimeSec);
	//	noteEndTimeMilSec = (this->noteTimeMilSec) + 500;
	//}
}


CLongNoteData::CLongNoteData(
	const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
	const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec)
{
	// 생성자.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = (noteTimeMilSec%1000);			// 밀리초의 범위는 0~999이다.
	this->noteType = noteType;
	this->targetMarker = targetMarker;

	this->noteEndTimeSec = noteEndTimeSec;
	this->noteEndTimeMilSec = noteEndTimeMilSec;
};



CLongNoteData::~CLongNoteData(void)
{
}


//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [Private]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////






//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [Virtual]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////


// 어떤 클래스의 노트인지를 알 수 있게 해 주는 함수.
char CLongNoteData::getClassType(void)
{
	// 롱노트 클래스 사용.
	return 'L';
}



void CLongNoteData::printNoteData(void)
{
	// 노트 정보를 한 줄로 출력할 때 사용하는 함수

	printNoteBasicData();									// 기본 정보 (초/밀리초/노트타입/)
	cout << "Marker No." << targetMarker << ", ";			// 타겟 마커 출력
	
	double noteEndTime = noteEndTimeSec + (noteEndTimeMilSec / (double)1000);
	printf("%.3lfsec\n", noteEndTime);						// 끝나는 시간 출력 후 줄바꿈. (출력 끝)

}





// 스페셜 노트 정보를 수정할 때 사용하는 가상함수.
int CLongNoteData::editNoteData(
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	// 만약 서로 다른 타입의 노트로 수정하려고 한다면?
	if( editNoteType != NOTE_T_LONG &&
		editNoteType != NOTE_T_PATTERN &&
		editNoteType != NOTE_T_CHARISMA &&
		editNoteType != NOTE_T_NEUTRAL	)
	{
		cout << "Error! Invalid note Type Change!" << endl;
		return -2;
	}

	this->noteTimeSec = editNoteTimeSec;
	this->noteTimeMilSec = editNoteTimeMilSec;
	this->noteType = editNoteType;
	this->targetMarker = editTargetMarker;

	this->noteEndTimeSec = editNoteEndTimeSec;
	this->noteEndTimeMilSec = editNoteEndTimeMilSec;
	
	return 0;

}


// 노트의 종료 시간을 리턴
unsigned int CLongNoteData::getNoteEndTimeSec(void)
{
	return noteEndTimeSec;
}

unsigned int CLongNoteData::getNoteEndTimeMilSec(void)
{
	return noteEndTimeMilSec;
}



// 해당 번호의 드래그 노트의 좌표를 리턴한다.
int CLongNoteData::getDragPoint(const int pointNum, dotData &point)
{
	// 롱 노트에서는 쓰일 일이 없다.
	return -99;
}

// 해당 번호의 드래그 노트의 좌표를 리턴한다.
int CLongNoteData::setDragPoint(const int pointNum, dotData &point)
{
	// 롱 노트에서는 쓰일 일이 없다.
	return -99;
}