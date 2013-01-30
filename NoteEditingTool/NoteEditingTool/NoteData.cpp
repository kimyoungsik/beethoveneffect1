
#include "stdafx.h"
#include "NoteData.h"


CNoteData::CNoteData(void)
{
}

CNoteData::CNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	// ������.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = (noteTimeMilSec%1000);			// �и����� ������ 0~999�̴�.
	this->noteType = noteType;
	this->targetMarker = targetMarker;

	// �и��� Ÿ�� üũ


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


// � Ŭ������ ��Ʈ������ �� �� �ְ� �� �ִ� �Լ�.
char CNoteData::getClassType(void)
{
	// �Ϲ����� ��Ʈ Ŭ���� ���.
	return 'N';
}


void CNoteData::printNoteData(void)
{
	// ��Ʈ ������ �� �ٷ� ����� �� ����ϴ� �Լ�

	printNoteBasicData();				// �⺻ ���� (��/�и���/��ƮŸ��/)
	cout << "Marker No." <<  targetMarker << endl;		// Ÿ�� ��Ŀ ��� �� �ٹٲ�. (��� ��)

}

// ��Ʈ ������ ������ �� ����ϴ� �����Լ�
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


// ����� ��Ʈ ������ ������ �� ����ϴ� �����Լ�.
int CNoteData::editNoteData(
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	cout << "Error! Worng Class Virtual Func!" << endl;
	return -99;
}

// �Է¹��� �巡�� ��Ʈ�� �� ��ǥ���� �����ϴ� �Լ�.
int CNoteData::editNoteData(
		const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPoinY
		)
{
	cout << "Error! Worng Class Virtual Func!" << endl;
	return -99;
}

// ��Ʈ�� ���� �ð��� ����
unsigned int CNoteData::getNoteEndTimeSec(void)
{
	return 0;
}


unsigned int CNoteData::getNoteEndTimeMilSec(void)
{
	return 0;
}



// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �����Ѵ�.
int CNoteData::getDragPoint(const int pointNum, dotData &point)
{
	// �Ϲ� ��Ʈ������ ���� ���� ����.
	return -99;
}


// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �����Ѵ�.
int CNoteData::setDragPoint(const int pointNum, dotData &point)
{
	// �Ϲ� ��Ʈ������ ���� ���� ����.
	return -99;
}



//////////////////////////////////////////////////////////////////////////////
//////////////////////////////     [Public]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////




void CNoteData::printNoteBasicData(void)
{	
	// ���� ��� �κ�
	double noteTime = noteTimeSec + (noteTimeMilSec / (double)1000);
	//cout << noteTimeSec << '.' << noteTimeMilSec << "sec, " << noteType << " type, ";
	//cout << noteTime << "sec, " << noteType << " type, ";
	printf("%.3lf", noteTime);
	cout << "sec, " << noteType << " type, ";
}



// ��Ʈ�� ���� ��ġ�Ѵٸ� True�� ����ϴ� �Լ�
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

// ���� ��Ʈ�� ���� ��ġ�Ѵٸ� True�� ����ϴ� �Լ�
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










// GET �Լ���.


// ��Ʈ ������ ���Ϲ޴� �Լ�.
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



// SET �Լ���.
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
