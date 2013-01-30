#include "NormalNoteData.h"


CNormalNoteData::CNormalNoteData(unsigned int noteTimeSec, unsigned int noteTimeMilSec, char noteType, int targetMarker)
{
	// 생성자.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = noteTimeMilSec;
	this->noteType = noteType;
	this->targetMarker = targetMarker;

}


CNormalNoteData::~CNormalNoteData(void)
{
}




// GET 함수들
int CNormalNoteData::getTargetMarker(void)
{
	return targetMarker;
}


// SET 함수들
void CNormalNoteData::setTargetMarker(int targetMarker)
{
	this->targetMarker = targetMarker;
}



