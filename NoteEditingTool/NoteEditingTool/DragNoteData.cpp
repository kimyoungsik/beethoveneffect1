
#include "stdafx.h"
#include "DragNoteData.h"


CDragNoteData::CDragNoteData(void)
{	// ������
}


CDragNoteData::CDragNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	// ������.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = (noteTimeMilSec%1000);			// �и����� ������ 0~999�̴�.
	this->noteType = noteType;
	this->targetMarker = targetMarker;

	// ��Ʈ�� ������ �ð��� �⺻�� ���� �ð� �ڷ� ������.
	noteEndTimeSec = noteTimeSec + DEFAULT_DRAG_SEC;
	noteEndTimeMilSec = noteTimeMilSec + DEFAULT_DRAG_MILSEC;
	if (noteTimeMilSec >= 1000)
	{
		noteEndTimeSec += (noteEndTimeMilSec/1000);
		noteEndTimeMilSec -= 1000;
	}

	// ��Ʈ ���� �ʱ�ȭ
	for( int i=0 ; i<MAX_DOT_DATA_NUM ; i++ )
	{
		dotDataArray[i].x = 0;
		dotDataArray[i].y = 0;
	}

	// �⺻ ��Ʈ ���� �Է�
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
	// ������.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = (noteTimeMilSec%1000);			// �и����� ������ 0~999�̴�.
	this->noteType = noteType;
	this->targetMarker = targetMarker;

	this->noteEndTimeSec = noteEndTimeSec;
	this->noteEndTimeMilSec = noteEndTimeMilSec%1000;

	// ��ǥ �Է�
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




// � Ŭ������ ��Ʈ������ �� �� �ְ� �� �ִ� �Լ�.
char CDragNoteData::getClassType(void)
{
	// �巡�� ��Ʈ Ŭ���� ���.
	return 'D';
}



void CDragNoteData::printNoteData(void)
{
	// ��Ʈ ������ �� �ٷ� ����� �� ����ϴ� �Լ�

	printNoteBasicData();									// �⺻ ���� (��/�и���/��ƮŸ��/)
	cout << "Marker No." << targetMarker << ", ";			// Ÿ�� ��Ŀ ���
	
	double noteEndTime = noteEndTimeSec + (noteEndTimeMilSec / (double)1000);
	printf("%.3lfsec\n �� ", noteEndTime);						// ������ �ð� ��� �� �ٹٲ�
	
	// ��Ʈ ���� ���
	int i;
	for( i=0 ; i < MAX_DOT_DATA_NUM-1 ; i++ )
	{
		cout << '(' << dotDataArray[i].x << ", " << dotDataArray[i].y << ")  ";
	}
	cout << '(' << dotDataArray[i].x << ", " << dotDataArray[i].y << ')' << endl;
	//(��� ��)

}




// ����� ��Ʈ ������ ������ �� ����ϴ� �����Լ�.
int CDragNoteData::editNoteData(
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	// ���� ���� �ٸ� Ÿ���� ��Ʈ�� �����Ϸ��� �Ѵٸ�?
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


// �Է¹��� �巡�� ��Ʈ�� �� ��ǥ���� �����ϴ� �Լ�.
int CDragNoteData::editNoteData(
		const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPointY )
{

	if ( pointNumber >= MAX_DOT_DATA_NUM )
	{
		// �߸� �� ��ȣ�� ���� �����Ϸ� �� ���,
		cout << "Error! Drag note has not dot what is number " << pointNumber << endl;
		return -10;
	}

	this->dotDataArray[pointNumber].x = newPointX;
	this->dotDataArray[pointNumber].y = newPointY;

	// ������ ������ ���
	return 1;
}


// ��Ʈ�� ���� �ð��� ����
unsigned int CDragNoteData::getNoteEndTimeSec(void)
{
	return noteEndTimeSec;
}

unsigned int CDragNoteData::getNoteEndTimeMilSec(void)
{
	return noteEndTimeMilSec;
}


// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �����Ѵ�.
int CDragNoteData::getDragPoint(const int pointNum, dotData &point)
{
	if ( pointNum >= MAX_DOT_DATA_NUM || pointNum < 0 )
	{
		// ���� �� �迭 �̻��� ���� ���,
		return -2;
	}

	point.x = dotDataArray[pointNum].x;
	point.y = dotDataArray[pointNum].y;

	return 1;
}



// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �Է��Ѵ�.
int CDragNoteData::setDragPoint(const int pointNum, dotData &point)
{
	if ( pointNum >= MAX_DOT_DATA_NUM || pointNum < 0 )
	{
		// ���� �� �迭 �̻��� ���� ���,
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



