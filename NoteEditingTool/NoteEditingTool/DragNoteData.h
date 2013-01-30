#pragma once

#include "notedata.h"

// ��� ����
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
// �Ӽ��Դϴ�.
// �巡�� ��Ʈ�� ���� Ŭ����
private:
	unsigned int noteEndTimeSec;			// ��Ʈ�� ������ �ð� (���� : ��)
	unsigned int noteEndTimeMilSec;			// ��Ʈ�� ������ �ð� (���� : �и� ��)

	dotData dotDataArray[MAX_DOT_DATA_NUM];			// ������ � �������� �����ϴ� �迭




public:
	// ������ & �Ҹ���
	CDragNoteData(void);
	CDragNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	CDragNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker,
		const unsigned int noteEndTimeSec, const unsigned int noteEndTimeMilSec, const dotData point1, const dotData point2, const dotData point3, const dotData point4);
	~CDragNoteData(void);

// Ư���Դϴ�.
public:
	
	// �����Լ���
	char getClassType(void);			// � Ŭ������ ��Ʈ������ �� �� �ְ� �� �ִ� �Լ�.
	void printNoteData(void);			// ��Ʈ ������ �� �ٷ� ����� �� ����ϴ� �Լ�
	// ����� ��Ʈ ������ ������ �� ����ϴ� �����Լ�.
	int editNoteData(
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	// �Է¹��� �巡�� ��Ʈ�� �� ��ǥ���� �����ϴ� �Լ�.
	int editNoteData(
		const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPoinY);
	// ��Ʈ�� ���� �ð��� ����
	virtual unsigned int getNoteEndTimeSec(void);
	virtual unsigned int getNoteEndTimeMilSec(void);
	// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �����Ѵ�.
	virtual int getDragPoint(const int pointNum, dotData &point);
	// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �Է��Ѵ�.
	virtual int setDragPoint(const int pointNum, dotData &point);

};

