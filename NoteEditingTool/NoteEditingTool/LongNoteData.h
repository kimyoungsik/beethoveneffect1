#pragma once

#include "NoteData.h"


// ��� ����
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
	// �ճ�Ʈ, ���� ü����, ī������ Ÿ��, ��Ʈ�� ������ Ŭ����
private:
	unsigned int noteEndTimeSec;			// ��Ʈ�� ������ �ð� (���� : ��)
	unsigned int noteEndTimeMilSec;			// ��Ʈ�� ������ �ð� (���� : �и� ��)







public:
	// ������ & �Ҹ���
	CLongNoteData(void);
	CLongNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	CLongNoteData(
		const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec);
	~CLongNoteData(void);


	// �����Լ���
	char getClassType(void);			// � Ŭ������ ��Ʈ������ �� �� �ְ� �� �ִ� �Լ�.
	void printNoteData(void);			// ��Ʈ ������ �� �ٷ� ����� �� ����ϴ� �Լ�
	// ����� ��Ʈ ������ ������ �� ����ϴ� �����Լ�.
	virtual int editNoteData(
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	// ��Ʈ�� ���� �ð��� ����
	virtual unsigned int getNoteEndTimeSec(void);
	virtual unsigned int getNoteEndTimeMilSec(void);
	// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �����Ѵ�.
	virtual int getDragPoint(const int pointNum, dotData &point);
	// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �����Ѵ�.
	virtual int setDragPoint(const int pointNum, dotData &point);



	// ��� �Լ���


};

