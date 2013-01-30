
#include "stdafx.h"
#include "NoteData.h"


CNoteData::CNoteData(void)
{
}

CNoteData::CNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	// 생성자.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = (noteTimeMilSec%1000);			// 밀리초의 범위는 0~999이다.
	this->noteType = noteType;
	this->targetMarker = targetMarker;

	// 밀리초 타입 체크


}


CNoteData::~CNoteData(void)
{
}



//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [Private]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////






//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [Virtual]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////


// 어떤 클래스의 노트인지를 알 수 있게 해 주는 함수.
char CNoteData::getClassType(void)
{
	// 일반적인 노트 클래스 사용.
	return 'N';
}


void CNoteData::printNoteData(void)
{
	// 노트 정보를 한 줄로 출력할 때 사용하는 함수

	printNoteBasicData();				// 기본 정보 (초/밀리초/노트타입/)
	cout << "Marker No." <<  targetMarker << endl;		// 타겟 마커 출력 후 줄바꿈. (출력 끝)

}

// 노트 정보를 수정할 때 사용하는 가상함수
int CNoteData::editNoteData(
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker
	)
{
	this->noteTimeSec = editNoteTimeSec;
	this->noteTimeMilSec = editNoteTimeMilSec;
	this->noteType = editNoteType;
	this->targetMarker = editTargetMarker;
	
	return 0;
}


// 스페셜 노트 정보를 수정할 때 사용하는 가상함수.
int CNoteData::editNoteData(
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	cout << "Error! Worng Class Virtual Func!" << endl;
	return -99;
}

// 입력받은 드래그 노트의 각 좌표점을 수정하는 함수.
int CNoteData::editNoteData(
		const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPoinY
		)
{
	cout << "Error! Worng Class Virtual Func!" << endl;
	return -99;
}

// 노트의 종료 시간을 리턴
unsigned int CNoteData::getNoteEndTimeSec(void)
{
	return 0;
}


unsigned int CNoteData::getNoteEndTimeMilSec(void)
{
	return 0;
}



// 해당 번호의 드래그 노트의 좌표를 리턴한다.
int CNoteData::getDragPoint(const int pointNum, dotData &point)
{
	// 일반 노트에서는 쓰일 일이 없다.
	return -99;
}


// 해당 번호의 드래그 노트의 좌표를 수정한다.
int CNoteData::setDragPoint(const int pointNum, dotData &point)
{
	// 일반 노트에서는 쓰일 일이 없다.
	return -99;
}



//////////////////////////////////////////////////////////////////////////////
//////////////////////////////     [Public]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////




void CNoteData::printNoteBasicData(void)
{	
	// 공통 출력 부분
	double noteTime = noteTimeSec + (noteTimeMilSec / (double)1000);
	//cout << noteTimeSec << '.' << noteTimeMilSec << "sec, " << noteType << " type, ";
	//cout << noteTime << "sec, " << noteType << " type, ";
	printf("%.3lf", noteTime);
	cout << "sec, " << noteType << " type, ";
}



// 노트의 값과 일치한다면 True를 출력하는 함수
bool CNoteData::compareNoteData(const unsigned int cNoteTimeSec, const unsigned int cNoteTimeMilSec, const char cNoteType, const char cTargetMarker)
{
	if(
		noteTimeSec == cNoteTimeSec	&&
		noteTimeMilSec == cNoteTimeMilSec	&&
		noteType == cNoteType	&&
		targetMarker == cTargetMarker
		)
	{
		return true;
	}

	return false;
}

// ┗━ 노트의 값과 일치한다면 True를 출력하는 함수
bool CNoteData::compareNoteData(const unsigned int cNoteTimeSec, const unsigned int cNoteTimeMilSec, const char cNoteType)
{
	
	if(
		noteTimeSec == cNoteTimeSec	&&
		noteTimeMilSec == cNoteTimeMilSec	&&
		noteType == cNoteType
		)
	{
		return true;
	}

	return false;
}










// GET 함수들.


// 노트 정보를 리턴받는 함수.
CNoteData CNoteData::getBasicNoteData(void)
{
	CNoteData tmpData;

	tmpData.setNoteTimeSec(noteTimeSec);
	tmpData.setNoteTimeMilSec(noteTimeMilSec);
	tmpData.setNoteType(noteType);
	tmpData.setTargetMarker(targetMarker);

	return tmpData;
}


unsigned int CNoteData::getNoteTimeSec(void)
{
	return noteTimeSec;
}

unsigned int CNoteData::getNoteTimeMilSec(void)
{
	return noteTimeMilSec;
}

char CNoteData::getNoteType(void)
{
	return noteType;
}

char CNoteData::getTargetMarker(void)
{
	return targetMarker;
}



// SET 함수들.
void CNoteData::setNoteTimeSec(const unsigned int second)
{
	noteTimeSec = second;
}


void CNoteData::setNoteTimeMilSec(const unsigned int miliSecond)
{
	noteTimeMilSec = miliSecond;
}

void CNoteData::setNoteType(const char m_noteType)
{
	noteType = m_noteType;
}

void CNoteData::setTargetMarker(const char m_targetMarker)
{
	targetMarker = m_targetMarker;
}
