#pragma once
#include "notedata.h"





using std::string;
using std::cout;
using std::cin;
using std::endl;



class CNormalNoteData :
	public CNoteData
{
	// ������ ��Ʈ�� ���� �Ϲ����� ��Ʈ�� Ŭ����
private:
	int targetMarker;			// ��Ʈ�� ������ �ϴ� Ÿ�� ��Ŀ (1~6���� �ִ�.)



public:
	CNormalNoteData(unsigned int noteTimeSec, unsigned int noteTimeMilSec, char noteType, int targetMarker);
	~CNormalNoteData(void);


	// GET �Լ���
	int getTargetMarker(void);


	// SET �Լ���
	void setTargetMarker(int targetMarker);

};


