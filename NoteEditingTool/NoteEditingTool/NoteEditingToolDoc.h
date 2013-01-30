/****************************************************************************


	getMp3FileName, getTitlePicture ���� �ʿ�, (Set��)
	�� Note���� �Է¹�� �ʿ�

	Charisma ��Ʈ�� Ÿ�� ��Ŀ �ٲٴ� �޴� �ʿ�.
	���ϸ�, Ÿ��Ʋ �� �����ϴ� â ���� �ʿ�.





****************************************************************************/
// NoteEditingToolDoc.h : CNoteEditingToolDoc Ŭ������ �������̽�
//


#pragma once


#include "stdafx.h"
#include "NoteFile.h"				// �̸� ����� ���� ��Ʈ������ �����ϴ� �ھ� Ŭ����
#include "ImportPictureDlg.h"

//#include "NoteEditingToolView.h"

// ���� �� �κ��� NoteData.h�� ������ ���ļ��� �� �ȴ�.
const char EDIT_MODE_WRITE = 'W';
const char EDIT_MODE_MOVE = 'M';
const char EDIT_MODE_ERASE = 'E';
const char EDIT_MODE_CONFG = 'F';

// � ������ ��Ʈ������ �˷��ִ� ���, ���� 1�� ������, �޼�, �ճ�Ʈ�� ����Ų��.
const char NOTE_TYPE_LINE_1 = 'N';
const char NOTE_TYPE_LINE_2 = 'S';			// ����2�� �巡��, ���� ���� ����Ų��.
const char NOTE_TYPE_LINE_3 = 'B';			// BPM, ĸ�ĳ�Ʈ ���� ����Ų��.


const unsigned int NOTE_COMP_RANGE1 = 200;
const unsigned int NOTE_COMP_RANGE2 = NOTE_COMP_RANGE1 - 16;


// �� �����ӵ��� ũ��.
const unsigned int FRAME_1_WIDTH = 800;
const unsigned int FRAME_1_HEIGHT = 260;
const unsigned int FRAME_2_WIDTH = 600;
const unsigned int FRAME_2_HEIGHT = 450;
const unsigned int FRAME_3_WIDTH = FRAME_1_WIDTH - FRAME_2_WIDTH;
const unsigned int FRAME_3_HEIGHT = FRAME_2_HEIGHT;

// ��ü �������� ũ��
const unsigned int BASE_FRAME_WIDTH = FRAME_1_WIDTH + 115;
const unsigned int BASE_FRAME_HEIGHT = FRAME_1_HEIGHT + FRAME_2_HEIGHT + 120;




// ��Ʈ �簢�� �� ������ ���� ��
const COLORREF NC_RIGHT = RGB(192, 80, 77);
const COLORREF NC_LEFT =  RGB(79, 129, 189);
const COLORREF NC_LONG = RGB(247, 150, 70);

const COLORREF NC_DRAG = RGB(204, 193, 218);
const COLORREF NC_CHARISMA = RGB(155, 187, 89);
const COLORREF NC_NEUTRAL = RGB(128, 100, 162);
const COLORREF NC_PATTERN = RGB(74, 69, 42);

const COLORREF NC_BPM = RGB(255, 153, 204);
const COLORREF NC_PHOTO = RGB(147, 205, 221);

const COLORREF NC_UNKNOWN = RGB(218, 71, 229);

// �巡�� ��Ʈ ��ġ�� ���� ��
const COLORREF DC_START = RGB(0, 94, 187);
const COLORREF DC_CENTER = RGB(255, 0, 128);
const COLORREF DC_END = RGB(0, 162, 232);
const COLORREF DC_LINE = RGB(153, 217, 234);



// ��� ���¸� �˷��ִ� �����
const int PLAY_STATE_STOP = 0;				// ����
const int PLAY_STATE_PLAY = 1;				// �����
const int PLAY_STATE_PAUSE = 2;				// �Ͻ�����
const int PLAY_STATE_NO_MUSIC = 3;			// ������ �ε���� ����.
const int PLAY_STATE_NON_INIT = -1;			// BASS Lib �ʱ�ȭ���� ����.







// ��Ʈ�� �⺻ ������ ��Ƶδ� Ŭ����.




class CNoteEditingToolView;							// ���߿� ��� �� Ŭ�����̹Ƿ�, ���� �� �д�.
class CNotePickingView;								// ���� ����
class CEditModeSelectView;

class CNoteEditingToolDoc : public CDocument
{
protected: // serialization������ ��������ϴ�.
	CNoteEditingToolDoc();
	DECLARE_DYNCREATE(CNoteEditingToolDoc)

// Ư���Դϴ�.
protected:
	CNoteFile newNoteFile;			// �� �����Ͱ� ���� �� ��Ʈ ���� ��ü
	char noteEditingMode;			// ���� � ���� ��Ʈ�� �����ϰ� �ִ��� �˷��ִ� ����.
									// 'W' �׸��� ���, 'M' �̵�(����) ���, 'E' ���� ���
	//int nowPlayingStatus;
	char noteWriteType;				// � ��Ʈ�� �׸� ���ΰ��� �˷��ִ� ����.
									// '1' ������, '2' �޼�, '4' �ճ�Ʈ
									// 'D' �巡��, 'C' ī������, 'N' �߸��ڼ�, 'P' ���� ��ȯ
									// 'B' bpm change, 'H' photo time

	// Picking View���� �� ������.
	CNoteData *nowEditingNote;		// ���� �����߿� �ִ� ��Ʈ�� ���Ѵ�.
	int noteHeadDir;					// 0�̸� ��Ʈ�� ���۽ð��� ��Ŀ���� �ִ� ��. 1�̸� ��Ʈ�� ���� ��Ŀ���� �ִ� ��.
	bool onFocusEditingFlag;

	CNoteData *nowConfigingNote;		// ���� �����߿� �ִ� ��Ʈ�� ���Ѵ�. CONFG��� �ܿ��� �׻� NULL�̾�� �Ѵ�.
	int noteConfigHeadDir;			// 0�̸� ��Ʈ�� ���۽ð��� ��Ŀ���� �ִ� ��. 1�̸� ��Ʈ�� ���� ��Ŀ���� �ִ� ��.


	CString mnfFilePath;				// �ε� ��Ʈ ������ ��θ� �����ϴ� ���ڿ�
	bool isNewFileFlag;				// ���� ���� ������ ��� true;

	//CString pictureFileName;


	// View ������ ����� ���ؼ� ��� �� ������
	CNoteEditingToolView *noteEditingToolViewPtr;
	CNotePickingView *notePickingViewPtr;
	CEditModeSelectView *editModeSelectViewPtr;

	// �׸����� �о���� ��ȭ����
	CImportPictureDlg importPicDlg;
	
public:




// �۾��Դϴ�.
public:


protected:
	// �뷫 � ������ ��Ʈ������ Ȯ���Ѵ�.
	char chkTypeToBigType(const char noteType);
	// ����� ����� �Լ�.
	int SaveFileHeader(CArchive &ar);
	// ���� ��ü�� ����� �Լ�.
	int SaveFileBody(CArchive &ar);
	// ��Ʈ �ϳ��� ���ؼ� ����ϴ� �Լ�.
	int SaveBodyNote(CArchive &ar, CNoteData *targetNote);

	// ����� �д� �Լ�.
	int LoadFileHeader(CArchive &ar);
	// ���� ��ü�� �д� �Լ�.
	int LoadFileBody(CArchive &ar);
	// ��Ʈ �ϳ��� ���ؼ� �о���� �Լ�.
	int LoadBodyNote(CArchive &ar, CNoteFile &targetNoteFile);

public:
	// ������ �Լ���

	// ��Ʈ�� �ϳ� ������ �迭�� �߰��ϴ� �Լ�.
	int addNewNote(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	// �Է¹��� �Ϲ� ��Ʈ�� �����ϴ� �Լ�.
	int editNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
				const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker);
	int editNote(CNoteData *notePointer,
				const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker);
	// �Է¹��� Ư�� ��Ʈ �κ��� �����ϴ� �Լ�.
	int editLongNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
				const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
				const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	int editLongNote(CNoteData *targetNote,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	// �Է¹��� �巡�� ��Ʈ�� �� ��ǥ���� �����ϴ� �Լ�.
	int editDragNotePoint(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
				const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPointY);
	// �Է¹��� ��Ʈ�� �����ϴ� �Լ�.
	int deleteNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker);
	int deleteNote(CNoteData *targetNote);
	// ��Ʈ �迭���� ��Ʈ�� �ּҰ��� �޾Ƴ��� �Լ�.
	CNoteData* getNoteAddr(const unsigned int i, const unsigned int j);
	// ������ �ִ� ��� ��Ʈ���� ����
	int clearAllNotes(void);


public:
	// Get & Set�� ������ �Լ���
	string getMp3FileName(void);
	string getTitlePicture(void);
	string getNoteFileTitle(void);
	string getNoteFileArtist(void);
	unsigned int getNoteFileLevel(void);
	unsigned int getNoteFileInitBPM(void);
	NoteTime getStartSongTime(void);
	NoteTime getEndSongTime(void);

	void setNoteFileTitle(string noteFileTitle);
	void setNoteFileArtist(string noteFileArtist);
	void setNoteFileLevel(const unsigned int noteFileLevel);
	void setMp3FileName(string mp3FileName);
	void setTitlePicture(string titlePicture);
	void setStartSongTime(NoteTime startSongTime);
	void setEndSongTime(NoteTime endSongTime);
	void setNoteFileInitBPM(unsigned int noteFileInitBPM);



public:
	// �Է¹��� �ð� �ֺ��� ������ ��Ʈ�� �ִ� �� Ȯ���Ѵ�. ������ 1, ������ 0 ����.
	int chkNoteTime(const NoteTime targetTime, const char noteType, const char targetMarker, CNoteData **notePointer);



	// get �Լ���
	// ���� � ����ΰ��� �����ϴ� �Լ�.
	char getNoteEditingMode(void);		
	char getNoteWriteType(void);
	CNoteData* getNowEditingNote(int &noteHeadDir);
	CNoteData* getNowConfigingNote(int &noteConfigHeadDir);
	bool getOnFocusEditingFlag(void);
	int getNoteHeadDir(void);
	int getNoteConfigHeadDir(void);
	CNoteEditingToolView *getNoteEditingToolViewPtr(void);
	CNotePickingView *getNotePickingViewPtr(void);
	CEditModeSelectView *getEditModeSelectViewPtr(void);
	CString getMnfFilePath(void);
	bool getIsNewFileFlag(void);


	// set �Լ���
	int setNoteEditingMode(const char editingMode);
	int setNoteWriteType(const char writeType);
	int setNowEditingNote(CNoteData* const nowEditingNote, const int noteHeadDir);
	int setNowConfigingNote(CNoteData* const nowEditingNote, const int noteConfigHeadDir);
	int setOnFocusEditingFlag(const bool onFocusEditingFlag);
	int setNoteEditingToolViewPtr(CNoteEditingToolView* noteEditingToolViewPtr);
	int setNotePickingViewPtr(CNotePickingView *notePickingViewPtr);
	int setEditModeSelectViewPtr(CEditModeSelectView *editModeSelectViewPtr);

// �������Դϴ�.
public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
#ifdef SHARED_HANDLERS
	virtual void InitializeSearchContent();
	virtual void OnDrawThumbnail(CDC& dc, LPRECT lprcBounds);
#endif // SHARED_HANDLERS

// �����Դϴ�.
public:
	virtual ~CNoteEditingToolDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// ������ �޽��� �� �Լ�
protected:
	DECLARE_MESSAGE_MAP()

#ifdef SHARED_HANDLERS
	// �˻� ó���⿡ ���� �˻� �������� �����ϴ� ����� �Լ�
	void SetSearchContent(const CString& value);
#endif // SHARED_HANDLERS
public:
	afx_msg void OnOpenNewPictureFile();
};

