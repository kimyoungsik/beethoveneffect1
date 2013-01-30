/****************************************************************************


	getMp3FileName, getTitlePicture 구현 필요, (Set도)
	각 Note정보 입력방법 필요

	Charisma 노트의 타겟 마커 바꾸는 메뉴 필요.
	파일명, 타이틀 등 설정하는 창 제작 필요.





****************************************************************************/
// NoteEditingToolDoc.h : CNoteEditingToolDoc 클래스의 인터페이스
//


#pragma once


#include "stdafx.h"
#include "NoteFile.h"				// 미리 만들어 놓은 노트파일을 관리하는 코어 클래스
#include "ImportPictureDlg.h"

//#include "NoteEditingToolView.h"

// 절대 이 부분은 NoteData.h의 상수들과 겹쳐서는 안 된다.
const char EDIT_MODE_WRITE = 'W';
const char EDIT_MODE_MOVE = 'M';
const char EDIT_MODE_ERASE = 'E';
const char EDIT_MODE_CONFG = 'F';

// 어떤 종류의 노트인지를 알려주는 상수, 종류 1은 오른손, 왼손, 롱노트를 가리킨다.
const char NOTE_TYPE_LINE_1 = 'N';
const char NOTE_TYPE_LINE_2 = 'S';			// 종류2는 드래그, 패턴 등을 가리킨다.
const char NOTE_TYPE_LINE_3 = 'B';			// BPM, 캡쳐노트 등을 가리킨다.


const unsigned int NOTE_COMP_RANGE1 = 200;
const unsigned int NOTE_COMP_RANGE2 = NOTE_COMP_RANGE1 - 16;


// 각 프레임들의 크기.
const unsigned int FRAME_1_WIDTH = 800;
const unsigned int FRAME_1_HEIGHT = 260;
const unsigned int FRAME_2_WIDTH = 600;
const unsigned int FRAME_2_HEIGHT = 450;
const unsigned int FRAME_3_WIDTH = FRAME_1_WIDTH - FRAME_2_WIDTH;
const unsigned int FRAME_3_HEIGHT = FRAME_2_HEIGHT;

// 전체 프레임의 크기
const unsigned int BASE_FRAME_WIDTH = FRAME_1_WIDTH + 115;
const unsigned int BASE_FRAME_HEIGHT = FRAME_1_HEIGHT + FRAME_2_HEIGHT + 120;




// 노트 사각형 색 구분을 위한 것
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

// 드래그 노트 위치를 위한 것
const COLORREF DC_START = RGB(0, 94, 187);
const COLORREF DC_CENTER = RGB(255, 0, 128);
const COLORREF DC_END = RGB(0, 162, 232);
const COLORREF DC_LINE = RGB(153, 217, 234);



// 재생 상태를 알려주는 상수들
const int PLAY_STATE_STOP = 0;				// 정지
const int PLAY_STATE_PLAY = 1;				// 재생중
const int PLAY_STATE_PAUSE = 2;				// 일시정지
const int PLAY_STATE_NO_MUSIC = 3;			// 음악이 로드되지 않음.
const int PLAY_STATE_NON_INIT = -1;			// BASS Lib 초기화되지 않음.







// 노트의 기본 정보를 담아두는 클래스.




class CNoteEditingToolView;							// 나중에 사용 할 클래스이므로, 선언만 해 둔다.
class CNotePickingView;								// 이하 동문
class CEditModeSelectView;

class CNoteEditingToolDoc : public CDocument
{
protected: // serialization에서만 만들어집니다.
	CNoteEditingToolDoc();
	DECLARE_DYNCREATE(CNoteEditingToolDoc)

// 특성입니다.
protected:
	CNoteFile newNoteFile;			// 이 에디터가 관리 할 노트 파일 객체
	char noteEditingMode;			// 현재 어떤 모드로 노트를 편집하고 있는지 알려주는 변수.
									// 'W' 그리기 모드, 'M' 이동(수정) 모드, 'E' 삭제 모드
	//int nowPlayingStatus;
	char noteWriteType;				// 어떤 노트를 그릴 것인가를 알려주는 변수.
									// '1' 오른손, '2' 왼손, '4' 롱노트
									// 'D' 드래그, 'C' 카리스마, 'N' 중립자세, 'P' 패턴 변환
									// 'B' bpm change, 'H' photo time

	// Picking View에서 쓸 변수들.
	CNoteData *nowEditingNote;		// 현재 수정중에 있는 노트를 말한다.
	int noteHeadDir;					// 0이면 노트의 시작시간에 포커스가 있는 것. 1이면 노트의 끝에 포커스가 있는 것.
	bool onFocusEditingFlag;

	CNoteData *nowConfigingNote;		// 현재 설정중에 있는 노트를 말한다. CONFG모드 외에는 항상 NULL이어야 한다.
	int noteConfigHeadDir;			// 0이면 노트의 시작시간에 포커스가 있는 것. 1이면 노트의 끝에 포커스가 있는 것.


	CString mnfFilePath;				// 로드 노트 파일의 경로를 저장하는 문자열
	bool isNewFileFlag;				// 새로 만든 파일일 경우 true;

	//CString pictureFileName;


	// View 끼리의 통신을 위해서 사용 할 변수들
	CNoteEditingToolView *noteEditingToolViewPtr;
	CNotePickingView *notePickingViewPtr;
	CEditModeSelectView *editModeSelectViewPtr;

	// 그림파일 읽어오는 대화상자
	CImportPictureDlg importPicDlg;
	
public:




// 작업입니다.
public:


protected:
	// 대략 어떤 종류의 노트인지를 확인한다.
	char chkTypeToBigType(const char noteType);
	// 헤더를 만드는 함수.
	int SaveFileHeader(CArchive &ar);
	// 파일 몸체를 만드는 함수.
	int SaveFileBody(CArchive &ar);
	// 노트 하나에 대해서 기록하는 함수.
	int SaveBodyNote(CArchive &ar, CNoteData *targetNote);

	// 헤더를 읽는 함수.
	int LoadFileHeader(CArchive &ar);
	// 파일 몸체를 읽는 함수.
	int LoadFileBody(CArchive &ar);
	// 노트 하나에 대해서 읽어오는 함수.
	int LoadBodyNote(CArchive &ar, CNoteFile &targetNoteFile);

public:
	// 껍데기 함수들

	// 노트를 하나 적절한 배열에 추가하는 함수.
	int addNewNote(const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char targetMarker);
	// 입력받은 일반 노트를 수정하는 함수.
	int editNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
				const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker);
	int editNote(CNoteData *notePointer,
				const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker);
	// 입력받은 특수 노트 부분을 수정하는 함수.
	int editLongNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker,
				const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
				const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	int editLongNote(CNoteData *targetNote,
		const unsigned int editNoteTimeSec, const unsigned int editNoteTimeMilSec, const char editNoteType, const char editTargetMarker,
		const unsigned int editNoteEndTimeSec, const unsigned int editNoteEndTimeMilSec);
	// 입력받은 드래그 노트의 각 좌표점을 수정하는 함수.
	int editDragNotePoint(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec,
				const unsigned int pointNumber, const unsigned int newPointX, const unsigned int newPointY);
	// 입력받은 노트를 삭제하는 함수.
	int deleteNote(const unsigned int orgNoteTimeSec, const unsigned int orgNoteTimeMilSec, const char orgNoteType, const char orgTargetMarker);
	int deleteNote(CNoteData *targetNote);
	// 노트 배열에서 노트의 주소값을 받아내는 함수.
	CNoteData* getNoteAddr(const unsigned int i, const unsigned int j);
	// 가지고 있는 모든 노트들을 삭제
	int clearAllNotes(void);


public:
	// Get & Set의 껍데기 함수들
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
	// 입력받은 시간 주변에 적당한 노트가 있는 지 확인한다. 있으면 1, 없으면 0 리턴.
	int chkNoteTime(const NoteTime targetTime, const char noteType, const char targetMarker, CNoteData **notePointer);



	// get 함수들
	// 현재 어떤 모드인가를 리턴하는 함수.
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


	// set 함수들
	int setNoteEditingMode(const char editingMode);
	int setNoteWriteType(const char writeType);
	int setNowEditingNote(CNoteData* const nowEditingNote, const int noteHeadDir);
	int setNowConfigingNote(CNoteData* const nowEditingNote, const int noteConfigHeadDir);
	int setOnFocusEditingFlag(const bool onFocusEditingFlag);
	int setNoteEditingToolViewPtr(CNoteEditingToolView* noteEditingToolViewPtr);
	int setNotePickingViewPtr(CNotePickingView *notePickingViewPtr);
	int setEditModeSelectViewPtr(CEditModeSelectView *editModeSelectViewPtr);

// 재정의입니다.
public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
#ifdef SHARED_HANDLERS
	virtual void InitializeSearchContent();
	virtual void OnDrawThumbnail(CDC& dc, LPRECT lprcBounds);
#endif // SHARED_HANDLERS

// 구현입니다.
public:
	virtual ~CNoteEditingToolDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// 생성된 메시지 맵 함수
protected:
	DECLARE_MESSAGE_MAP()

#ifdef SHARED_HANDLERS
	// 검색 처리기에 대한 검색 콘텐츠를 설정하는 도우미 함수
	void SetSearchContent(const CString& value);
#endif // SHARED_HANDLERS
public:
	afx_msg void OnOpenNewPictureFile();
};

