#include "NormalNoteData.h"


CNormalNoteData::CNormalNoteData(unsigned int noteTimeSec, unsigned int noteTimeMilSec, char noteType, int targetMarker)
{
	// ������.
	this->noteTimeSec = noteTimeSec;
	this->noteTimeMilSec = noteTimeMilSec;
	this->noteType = noteType;
	this->targetMarker = targetMarker;

}


CNormalNoteData::~CNormalNoteData(void)
{
}




// GET �Լ���
int CNormalNoteData::getTargetMarker(void)
{
	return targetMarker;
}


// SET �Լ���
void CNormalNoteData::setTargetMarker(int targetMarker)
{
	this->targetMarker = targetMarker;
}



