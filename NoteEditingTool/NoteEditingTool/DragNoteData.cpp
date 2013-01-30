
#include "stdafx.h"
#include "DragNoteData.h"


CDragNoteData::CDragNoteData(void)
{	// 생성자
}


CDragNoteData::CDragNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	// 생성자.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = (noteTimeMilSec%1000);			// 밀리초의 범위는 0~999이다.
	this->noteType = noteType;
	this->targetMarker = targetMarker;

	// 노트의 끝나는 시간의 기본은 적정 시간 뒤로 보낸다.
	noteEndTimeSec = noteTimeSec + DEFAULT_DRAG_SEC;
	noteEndTimeMilSec = noteTimeMilSec + DEFAULT_DRAG_MILSEC;
	if (noteTimeMilSec >= 1000)
	{
		noteEndTimeSec += (noteEndTimeMilSec/1000);
		noteEndTimeMilSec -= 1000;
	}

	// 도트 정보 초기화
	for( int i=0 ; i<MAX_DOT_DATA_NUM ; i++ )
	{
		dotDataArray[i].x = 0;
		dotDataArray[i].y = 0;
	}

	// 기본 도트 정보 입력
	dotDataArray[0].x = 269;
	dotDataArray[0].y = 390;

	dotDataArray[1].x = 407;
	dotDataArray[1].y = 220;

	dotDataArray[2].x = 605;
	dotDataArray[2].y = 551;

	dotDataArray[3].x = 750;
	dotDataArray[3].y = 389;


}


CDragNoteData::CDragNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
	const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec, const dotData point1, const dotData point2, const dotData point3, const dotData point4)
{
	// 생성자.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = (noteTimeMilSec%1000);			// 밀리초의 범위는 0~999이다.
	this->noteType = noteType;
	this->targetMarker = targetMarker;

	this->noteEndTimeSec = noteEndTimeSec;
	this->noteEndTimeMilSec = noteEndTimeMilSec%1000;

	// 좌표 입력
	this->dotDataArray[0].x = point1.x;
	this->dotDataArray[0].y = point1.y;
	this->dotDataArray[1].x = point2.x;
	this->dotDataArray[1].y = point2.y;
	this->dotDataArray[2].x = point3.x;
	this->dotDataArray[2].y = point3.y;
	this->dotDataArray[3].x = point4.x;
	this->dotDataArray[3].y = point4.y;

	this->dotDataArray[4].x = 0;
	this->dotDataArray[4].y = 0;

}




CDragNoteData::~CDragNoteData(void)
{
}





//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [Private]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////









//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [Virtual]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////




// 어떤 클래스의 노트인지를 알 수 있게 해 주는 함수.
char CDragNoteData::getClassType(void)
{
	// 드래그 노트 클래스 사용.
	return 'D';
}



void CDragNoteData::printNoteData(void)
{
	// 노트 정보를 한 줄로 출력할 때 사용하는 함수

	printNoteBasicData();									// 기본 정보 (초/밀리초/노트타입/)
	cout << "Marker No." << targetMarker << ", ";			// 타겟 마커 출력
	
	double noteEndTime = noteEndTimeSec + (noteEndTimeMilSec / (double)1000);
	printf("%.3lfsec\n ┖ ", noteEndTime);						// 끝나는 시간 출력 후 줄바꿈
	
	// 도트 내용 출력
	int i;
	for( i=0 ; i < MAX_DOT_DATA_NUM-1 ; i++ )
	{
		cout << '(' << dotDataArray[i].x << ", " << dotDataArray[i].y << ")  ";
	}
	cout << '(' << dotDataArray[i].x << ", " << dotDataArray[i].y << ')' << endl;
	//(출력 끝)

}




// 스페셜 노트 정보를 수정할 때 사용하는 가상함수.
int CDragNoteData::editNoteData(
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	// 만약 서로 다른 타입의 노트로 수정하려고 한다면?
	if( editNoteType != NOTE_T_DRAG	)
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


// 입력받은 드래그 노트의 각 좌표점을 수정하는 함수.
int CDragNoteData::editNoteData(
		const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPointY )
{

	if ( pointNumber >= MAX_DOT_DATA_NUM )
	{
		// 잘못 된 번호의 점을 수정하려 할 경우,
		cout << "Error! Drag note has not dot what is number " << pointNumber << endl;
		return -10;
	}

	this->dotDataArray[pointNumber].x = newPointX;
	this->dotDataArray[pointNumber].y = newPointY;

	// 무사히 끝났을 경우
	return 1;
}


// 노트의 종료 시간을 리턴
unsigned int CDragNoteData::getNoteEndTimeSec(void)
{
	return noteEndTimeSec;
}

unsigned int CDragNoteData::getNoteEndTimeMilSec(void)
{
	return noteEndTimeMilSec;
}


// 해당 번호의 드래그 노트의 좌표를 리턴한다.
int CDragNoteData::getDragPoint(const int pointNum, dotData &point)
{
	if ( pointNum >= MAX_DOT_DATA_NUM || pointNum < 0 )
	{
		// 지정 된 배열 이상의 값일 경우,
		return -2;
	}

	point.x = dotDataArray[pointNum].x;
	point.y = dotDataArray[pointNum].y;

	return 1;
}



// 해당 번호의 드래그 노트의 좌표를 입력한다.
int CDragNoteData::setDragPoint(const int pointNum, dotData &point)
{
	if ( pointNum >= MAX_DOT_DATA_NUM || pointNum < 0 )
	{
		// 지정 된 배열 이상의 값일 경우,
		return -2;
	}

	this->dotDataArray[pointNum].x = point.x;
	this->dotDataArray[pointNum].y = point.y;
	//point.x = dotDataArray[pointNum].x;
	//point.y = dotDataArray[pointNum].y;

	return 1;
}



//////////////////////////////////////////////////////////////////////////////
//////////////////////////////     [Public]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////



