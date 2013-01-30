#pragma once

#include <iostream>



// ��Ʈ Ÿ�� ����. ���⼭ ���� ���� ���Ͽ� ��� �� �� ���ȴ�.
const char NOTE_T_RIGHT = '1';
const char NOTE_T_LEFT = '2';
const char NOTE_T_LONG = '4';
const char NOTE_T_DRAG = 'D';					// Drag Note
const char NOTE_T_CHARISMA = 'C';				// Charisma Time
const char NOTE_T_NEUTRAL = 'N';				// Neutral Position
const char NOTE_T_PATTERN = 'P';				// Pattern Change
const char NOTE_T_BPM = 'B';					// BPM Change
const char NOTE_T_PHOTO = 'H';					// Photo Time



// ��Ŀ �̸� ����. ���⼭ ���� ���� ���Ͽ� ��� �� �� ���ȴ�.
const char MARKER_NUM1 = '1';
const char MARKER_NUM2 = '2';
const char MARKER_NUM3 = '3';
const char MARKER_NUM4 = '4';
const char MARKER_NUM5 = '5';
const char MARKER_NUM6 = '6';






using std::string;
using std::cout;
using std::cin;
using std::endl;


// ������ � ������ ����ϴ� ����ü
struct dotData{
	int x;
	int y;
};


class CNoteData
{
// ���� ��Ʈ�� �����ϴ� �θ� Ŭ����.
// ������, �޼�, BPM Change�� �����Ѵ�.
protected:
	unsigned int noteTimeSec;			// ��Ʈ�� �ľ� �ϴ� �ð� (���� : ��)
	unsigned int noteTimeMilSec;		// ��Ʈ�� �ľ� �ϴ� �ð� (���� : �и� ��)

	char noteType;						// ��Ʈ�� ����.
	char targetMarker;					// Ÿ�� ��Ŀ (1~6������ ���� ������. 'n'�� ��� Ÿ���� ������ �ʴ� ��Ʈ���� �ǹ��Ѵ�.)
	// BPM Change�� ��� (unsigned int)noteType * (unsigned int)targetMarker �� ������ �����Ѵ�.





// ����Դϴ�.
private:


public:
	// ������ & �Ҹ���
	CNoteData(void);
	CNoteData(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	~CNoteData(void);


	///////// �������̽��� ���� �Լ���
	virtual char getClassType(void);			// � Ŭ������ ��Ʈ������ �� �� �ְ� �� �ִ� �Լ�.
	virtual void printNoteData(void);			// ��Ʈ ������ �� �ٷ� ����� �� ����ϴ� �Լ�
	// ��Ʈ ������ ������ �� ����ϴ� �����Լ�
	virtual int editNoteData(
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker);
	// ����� ��Ʈ ������ ������ �� ����ϴ� �����Լ�.
	virtual int editNoteData(
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	// �Է¹��� �巡�� ��Ʈ�� �� ��ǥ���� �����ϴ� �Լ�.
	virtual int editNoteData(
		const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPoinY);
	// ��Ʈ�� ���� �ð��� ����
	virtual unsigned int getNoteEndTimeSec(void);
	virtual unsigned int getNoteEndTimeMilSec(void);
	// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �����Ѵ�.
	virtual int getDragPoint(const int pointNum, dotData &point);
	// �ش� ��ȣ�� �巡�� ��Ʈ�� ��ǥ�� �����Ѵ�.
	virtual int setDragPoint(const int pointNum, dotData &point);


	//////// �Ϲ� �Լ���
	void printNoteBasicData(void);				// ��Ʈ�� �⺻ �������� ����ϴ� �Լ�.

	// ��Ʈ�� ���� ��ġ�Ѵٸ� True�� ����ϴ� �Լ�
	bool compareNoteData(const unsigned int cNoteTimeSec, const unsigned int cNoteTimeMilSec, const char cNoteType, const char cTargetMarker);
	// ���� ��Ʈ�� ���� ��ġ�Ѵٸ� True�� ����ϴ� �Լ�
	bool compareNoteData(const unsigned int cNoteTimeSec, const unsigned int cNoteTimeMilSec, const char cNoteType);



	// GET �Լ���.
	CNoteData getBasicNoteData(void);				// ��Ʈ ������ ���Ϲ޴� �Լ�.
	unsigned int getNoteTimeSec(void);
	unsigned int getNoteTimeMilSec(void);
	char getNoteType(void);
	char getTargetMarker(void);



	// SET �Լ���.
	void setNoteTimeSec(const unsigned int second);
	void setNoteTimeMilSec(const unsigned int miliSecond);
	void setNoteType(const char m_noteType);
	void setTargetMarker(const char m_targetMarker);
};

