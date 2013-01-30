
#include "stdafx.h"
#include "LongNoteData.h"




CLongNoteData::CLongNoteData(void)
{
	// ������
}

CLongNoteData::CLongNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker)
{
	// ������.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = (noteTimeMilSec%1000);			// �и����� ������ 0~999�̴�.
	this->noteType = noteType;
	this->targetMarker = targetMarker;

	// ��Ʈ�� ������ �ð��� �⺻�� ���� �ð� �ڷ� ������.
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
	// ������.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = (noteTimeMilSec%1000);			// �и����� ������ 0~999�̴�.
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


// � Ŭ������ ��Ʈ������ �� �� �ְ� �� �ִ� �Լ�.
char CLongNoteData::getClassType(void)
{
	// �ճ�Ʈ Ŭ���� ���.
	return 'L';
}



void CLongNoteData::printNoteData(void)
{
	// ��Ʈ ������ �� �ٷ� ����� �� ����ϴ� �Լ�

	printNoteBasicData();									// �⺻ ���� (��/�и���/��ƮŸ��/)
	cout << "Marker No." << targetMarker << ", ";			// Ÿ�� ��Ŀ ���
	
	double noteEndTime = noteEndTimeSec + (noteEndTimeMilSec / (double)1000);
	printf("%.3lfsec\n", noteEndTime);						// ������ �ð� ��� �� �ٹٲ�. (��� ��)

}





// ����� ��Ʈ ������ ������ �� ����ϴ� �����Լ�.
int CLongNoteData::editNoteData(
	const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
	const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec)
{
	// ���� ���� �ٸ� Ÿ���� ��Ʈ�� �����Ϸ��� �Ѵٸ�?
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


// ��Ʈ�� ���� �ð��� ����
unsigned int CLongNoteData::getNoteEndTimeSec(void)
{
	return noteEndTimeSec;
}

unsigned int CLongNoteData::getNoteEndTimeMilSec(void)
{
	return noteEndTimeMilSec;
}



// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �����Ѵ�.
int CLongNoteData::getDragPoint(const int pointNum, dotData &point)
{
	// �� ��Ʈ������ ���� ���� ����.
	return -99;
}

// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �����Ѵ�.
int CLongNoteData::setDragPoint(const int pointNum, dotData &point)
{
	// �� ��Ʈ������ ���� ���� ����.
	return -99;
}