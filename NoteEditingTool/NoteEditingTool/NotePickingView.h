#pragma once


//#include <iostream>

//#include <process.h>

//#include <windows.h>

#include "bass.h"
#include "NoteEditingToolDoc.h"
//#include "BassLibControl.h"


#include "MnfSettingDlg.h"
#include "CharismaDlg.h"
#include "PatternChange.h"
#include "BPMChangeDlg.h"



// 타이머를 위한 헤더
#include <MMSystem.h>
#pragma comment(lib, "winmm.lib")




const int HEIGHT = 261;			// height (odd number for centre line)
////////////////////////////////////////////////////////////////////////// BASS Lib




// 각종 필요한 상수들

const unsigned int V_NOTE_SET_OFF_Y = 23;				// 이 높이 정도를 띄우고 시작.
const unsigned int V_NOTE_SET_OFF_X = 50;				// 이 정도의 왼쪽 여백을 띄우고 시작.
const unsigned int V_NOTE_HEIGHT = 20;					// 노트의 높이를 나타낸다.
const unsigned int V_NOTE_WIDTH = 8;						// 노트의 너비를 나타낸다.
const unsigned int V_NOTE_INTVAL = 6;					// 일반 노트들 사이의 간격 상수
//const unsigned int V_NOTE_INTVAL2 = 6;					// 일반 노트와 특수 노트 사이의 간격 상수
//const unsigned int V_NOTE_INTVAL3 = 4;					// 특수 노트들 사이의 간격 상수

const long double PIXEL_TO_TIME = 20.83333333334;		
														// [120114 별 의미없는 값이 되었다.]
														// x축 픽셀 값 * 이 값 = (Sec) 로 정한다.
														// Pixel per (PIXEL_TO_TIME)ms
														// 클릭 한 값이 지침선의 값보다 크면, 값을 올린다.
const double PIXEL_TO_MILTIME = PIXEL_TO_TIME / (double)1000;
const double PIXEL_TO_BASS_MILTIME = PIXEL_TO_MILTIME / 10000;





// 특수 노트가 시작되는 위치
const unsigned int V_SPEC_NOTE_Y = V_NOTE_SET_OFF_Y + V_NOTE_INTVAL + (V_NOTE_INTVAL + V_NOTE_HEIGHT)*6;
const unsigned int V_SPEC_NOTE_Y_END = V_NOTE_SET_OFF_Y + V_NOTE_INTVAL + (V_NOTE_INTVAL + V_NOTE_HEIGHT)*6 + V_NOTE_HEIGHT;


const unsigned int V_LNOTE_LENGTH = (unsigned int)( ( (DEFAULT_SEC_RANGE * 1000) + DEFAULT_MILSEC_RANGE ) / PIXEL_TO_TIME);
const unsigned int V_30SEC_PIXEL_X = (unsigned int)((( (30 * 1000) + 0 ) / PIXEL_TO_TIME ));
const unsigned int V_5SEC_PIXEL_X = (unsigned int)((( (5 * 1000) + 0 ) / PIXEL_TO_TIME ));
const unsigned int V_1SEC_PIXEL_X = (unsigned int)((( (1 * 1000) + 0 ) / PIXEL_TO_TIME ));



// 선 그리는 데 필요한 상수들.
const unsigned int V_LINE_Y_END = V_NOTE_SET_OFF_Y + V_NOTE_INTVAL + (V_NOTE_HEIGHT + V_NOTE_INTVAL ) * 8;			// 여기까지가 y축 맨 아래 선이다.
const unsigned int V_LINE_INTERVAL = V_NOTE_INTVAL/2;

const unsigned int V_LINE_TARG1_Y = V_NOTE_SET_OFF_Y + V_LINE_INTERVAL + (V_NOTE_HEIGHT + V_NOTE_INTVAL );
const unsigned int V_LINE_TARG2_Y = V_NOTE_SET_OFF_Y + V_LINE_INTERVAL + (V_NOTE_HEIGHT + V_NOTE_INTVAL ) * 2;
const unsigned int V_LINE_TARG3_Y = V_NOTE_SET_OFF_Y + V_LINE_INTERVAL + (V_NOTE_HEIGHT + V_NOTE_INTVAL ) * 3;
const unsigned int V_LINE_TARG4_Y = V_NOTE_SET_OFF_Y + V_LINE_INTERVAL + (V_NOTE_HEIGHT + V_NOTE_INTVAL ) * 4;
const unsigned int V_LINE_TARG5_Y = V_NOTE_SET_OFF_Y + V_LINE_INTERVAL + (V_NOTE_HEIGHT + V_NOTE_INTVAL ) * 5;
const unsigned int V_LINE_TARG6_Y = V_NOTE_SET_OFF_Y + V_LINE_INTERVAL + (V_NOTE_HEIGHT + V_NOTE_INTVAL ) * 6;

const unsigned int V_LINE_SPEC_Y = V_NOTE_SET_OFF_Y + V_LINE_INTERVAL + (V_NOTE_HEIGHT + V_NOTE_INTVAL ) * 7;


// 처음 몇 초간은 노트를 그릴 수 없다.
const unsigned int V_UNUSED_TIME_SEC = 5;
const unsigned int V_UNUSED_TIMELINE = (unsigned int)(V_UNUSED_TIME_SEC * V_1SEC_PIXEL_X);

// 같은 오른손 노트는 일정 간격을 가져야 한다. (300ms 정도)
const unsigned long MIN_NOTE_END_INTERVAL_SAME_R = 300;
const unsigned long MIN_NOTE_END_INTERVAL_AFTER_CHAR = 500;
const unsigned long MIN_NOTE_END_INTERVAL_AFTER_NEUTRAL = MIN_NOTE_END_INTERVAL_AFTER_CHAR;
const unsigned long MIN_NOTE_END_INTERVAL_AFTER_DRAG = 300;
const unsigned long MIN_NOTE_END_INTERVAL_AFTER_PATTERN = 300;

// BASS 에서 사용 할 상수들
const unsigned int BSS_WAVE_FORM_RATE = 15000;

const double BSS_WAVE_FORM_RATE_REAL = 2;
const int BSS_UNKNOWN_WAVE_WIDTH = 32768;


// 노트 선택시 나오는 색
const COLORREF NCL_EDIT = RGB(102, 255, 204);
const COLORREF BACK_COLOR_FOR_DRAG = __ARGB(180, 0, 0, 0);
const COLORREF BACK_COLOR_FOR_RIGHT = __ARGB(70, 237, 28, 36);
const COLORREF BACK_COLOR_FOR_LEFT = __ARGB(70, 63, 72, 204);
const COLORREF BACK_PEN_COLOR = __ARGB(0, 0, 0, 0);


// 각 노트 타입 별 최소 요구시간
const long MIN_NOTE_INTERVAL_DRAG = 5000;
const long MIN_NOTE_INTERVAL_CHARISMA = 3000;
const long MIN_NOTE_INTERVAL_PATTERN = 3000;
const long MIN_NOTE_INTERVAL_NEUTRAL = 3000;

const long MAX_NOTE_INTERVAL_ALL = 20000;			// 20초 이상 지속되는 노트는 제한하기로 한다.

// 타이머 설정
const int TIMER_1_ID = 5230;
const int TIMER_1_MSEC = 25;

// 마우스 핸들 설정
const int TIMER_1_FLAG_NULL = 0;
const int TIMER_1_FLAG_UP = 1;
const int TIMER_1_FLAG_MOVE = 3;






// CNotePickingView 뷰입니다.


class CNotePickingView : public CScrollView
{
protected:
	// 특성입니다.



	DECLARE_DYNCREATE(CNotePickingView)

protected:
	CNotePickingView();           // 동적 만들기에 사용되는 protected 생성자입니다.
	virtual ~CNotePickingView();


// 특성입니다.
protected:
	CNoteEditingToolDoc* GetDocument() const;			// Picking View는 같은 document를 사용.
	CRect movingRect;				// 마우스를 움직 일 때마다 보일 사각형의 모습.

	// 텍스트 출력을 위한 변수들
	BOOL m_bTransparent;				// 텍스트의 배경을 투명하게 할 것인지
	COLORREF m_colorText;			// 텍스트 전경색
	LOGFONT m_logFont;				// 텍스트를 출력할 논리적 글꼴
	//CBassLibControl BassLibCont;

protected:
	// 대화상자들
	CMnfSettingDlg settingDlg;		// 제목, 작곡가, 난이도 조절용 대화상자
	CCharismaDlg charismaDlg;		// 카리스마 노트 용 대화상자.
	CPatternChange patternDlg;		// 패턴 변화 용 대화상자.
	CBPMChangeDlg bpmCDlg;			// Bpm 변화 용 대화상자.


protected:
// BASS Lib에서 사용하기 위한 특성입니다.
	HWND win;
	DWORD scanthread;
	BOOL killscan;

	DWORD chan;
	DWORD bpp;					// bytes per pixel
	static QWORD loop[2];		// loop start & end
	HSYNC lsync;				// looping sync

	HDC wavedc;
	HBITMAP wavebmp;
	BYTE *wavebuf;
	

	// 상수를 변수로 가져 온 것
	unsigned int WIDTH;				// display width


	// 더블버퍼링을 위한 변수
	CBitmap bmpOffScreen;			// 더블버퍼링을 위한 비트맵 객체를 만든다.
	CBitmap bmpOfTimeLine;			// 타임라인을 위한 비트맵 객체를 만든다.
	CBitmap* bmpOfTimeLineMask;		// 타임라인 마스크
	CBitmap bMaskedTimeLine;

	// 현재 재생중에 있는지 알기 위한 변수 (0이면 정지, 1이면 재생, 2이면 일시정지, 초기값 -1)
	// 3이면 음악이 로드되지 않은 상태.
	int nowPlayingStatus;
	bool bassInitFlag;				// false이면, 초기화가 되지 않은 것.


	// 타이머를 위한 변수
	UINT m_uTimer;
	TIMECAPS tc;
	UINT m_uResolution;
	MMRESULT m_idEvent;
	//UINT resolution;

	bool mutexFlag;					// 뮤텍스와 같은 역할을 하는 임시 변수.

protected:
	// 타이머에서 마우스의 입력을 받아들이기 위한 변수
	//CPoint onTimerMousePoint;
	//int chkMouseHandleState;				// 마우스 메시지가 있었는가를 확인하는 변수,


	bool m_timerRedrawFlag;


protected:

	// 에러가 났을 경우, 그 에러코드를 알아내서 출력하기 위한 함수.
	void Error(const TCHAR *es);
	static void CALLBACK LoopSyncProc(HSYNC handle, DWORD channel, DWORD data, void *user);					// lib에서 static을 요구한다.
	void SetLoopStart(QWORD pos);
	void SetLoopEnd(QWORD pos);
	// 스래드를 새로 돌려서 파형을 그린다.
	static void __cdecl ScanPeaks(void *p);
	BOOL PlayFile();
	void DrawTimeLine(HDC dc, QWORD pos, DWORD col, DWORD y);
	// 스크롤과 오프셋 양 만큼의 차를 주기 위해 만든 함수
	void DrawTimeLineEx(HDC dc, QWORD pos, DWORD col, DWORD y, CPoint scrollPos);


	// window procedure
	//static long FAR PASCAL SpectrumWindowProc(HWND h, UINT m, WPARAM w, LPARAM l);

	int pseudoMain(HINSTANCE hInstance);
	
	// BASS Lib 초기화 함수
	int BInit(void);

	// mp3파일 불러오는 함수 (mnf파일정보 갱신)
	int BopenMusicFile(void);

	// MNF파일에 명시되어 있는 mp3파일 불러오는 함수
	int BLoadMusicFileAuto(void);

	// mp3파일 새로고침하는 함수 (mnf파일에 있는 이름대로 불러온다.)
	int BrefreshMusicFile(void);

	// 문자열을 받아 오면 "파일명.mp3"만 빼 내서 노트파일에 저장하는 함수.
	int saveMp3FileName(WCHAR *file);
	
	// 비트맵 이미지를 받아 와, 입력받은 색을 자른다.
	//-----------------------------------------------------------------------------------------------------
	// 함수명 : 마스크 이미지 만들기
	// 인자명 : 1.  in_bitmap          : 원본 비트맵
	//          2.  in_transColor      : 투명색
	//-----------------------------------------------------------------------------------------------------
	CBitmap* Create_MaskBitmap( CBitmap* in_bitmap, COLORREF in_transColor);
	
	// 시험용 함수들
	void tempTest();
	void tempPause();
	void tempStop();
	void tempPlay();
	// 끝.


// 기능입니다.
protected:
	// 현재 마우스의 위치에 맞는 노트의 적절한 위치를 리턴하는 함수.
	CPoint getNoteReviPos(CPoint point, char &drawOkFlag);
	CPoint getNoteReviPos(CPoint point, char &drawOkFlag, const CPoint scrollPoint);
	CPoint getNoteReviPosRanType(CPoint point, char &drawOkFlag);
	// 현재 마우스의 위치에 맞는 노트의 적절한 위치를 y값만 리턴하는 함수.
	CPoint getNoteReviPos2(CPoint point, char &drawOkFlag, const CPoint scrollPoint);
	// 현재 마우스의 x축 값을 적절한 시간 값으로 바꿔주는 함수.
	NoteTime ConvertPixToTime(CPoint point);
	// 시간 값을 적절한 x축 픽셀값으로 바꿔주는 함수
	unsigned long ConvertTimeToPixX(NoteTime nTime);
	// 각 타입에 맞는 y축 픽셀값으로 바꿔주는 함수.
	unsigned long ConvertTypeToPixY(const char noteType, const char targetMarker);
	// 픽셀 값을 적절한 노트 기본 정보로 바꾸는 함수.
	int ConvertPixToNoteData(const CPoint point, NoteTime &time, char &noteType, char &targetMarker);
	// 경우에 따라서 타입을 지정 해 주는 방식으로 픽셀 값을 적절한 노트 기본 정보로 바꾸는 함수.
	int ConvertPixToNoteDataRanType(const CPoint point, NoteTime &time, char &noteBigType, char &targetMarker);
	// 배경 선 그리기
	int drawBackgroundLines(CDC *pDC);
	// 대략 어떤 종류의 노트인지를 확인한다.
	char chkTypeToBigType(const char noteType);
	// 현재 그리려는 노트와 중첩되면 안 되는 노트가 있는 지 확인한다.
	bool chkIsNoteDrawOk(CNoteData *oldNotePtr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char TargetMarker);
	// (꼬리를 수정하는 용 오버로딩) 현재 그리려는 노트와 중첩되면 안 되는 노트가 있는 지 확인한다.
	bool chkIsNoteDrawOk(CNoteData *oldNotePtr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, NoteTime noteEndTime, const char noteType, const char TargetMarker);


public:
	// get & set 함수들
	int getNowPlayingStatus(void);


	void setNowPlayingStatus(const int nowPlayingStatus);






public:
#ifdef _DEBUG
	virtual void AssertValid() const;
//#ifndef _WIN32_WCE
	virtual void Dump(CDumpContext& dc) const;
//#endif
#endif

protected:
	void DrawDoubleBuffering(void);		// 더블 버퍼링의 이미지를 그린다.
	virtual void OnDraw(CDC* pDC);      // 이 뷰를 그리기 위해 재정의되었습니다.
	virtual void OnInitialUpdate();     // 생성된 후 처음입니다.

	
// 재정의입니다.
public:
//	virtual void OnDraw(CDC* pDC);  // 이 뷰를 그리기 위해 재정의되었습니다.
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
protected:
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);


	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);



//protected:
//	afx_msg LRESULT OnCmRedrawDragView(WPARAM wParam, LPARAM lParam);
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnOpenNewMusicFile();
	afx_msg void OnMnfSetting();
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
protected:
	afx_msg LRESULT OnCmScanEnd(WPARAM wParam, LPARAM lParam);
public:
	afx_msg void OnTimer(UINT_PTR nIDEvent);
protected:
	afx_msg LRESULT OnCmRedrawDc(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnCmVkSpace(WPARAM wParam, LPARAM lParam);


public:
	// 타이머 함수들
	void MMTimerHandler(UINT nIDEvent);
	static void CALLBACK TimerFunction(UINT wTimerID, UINT msg, 	DWORD dwUser, DWORD dw1, DWORD dw2);

};


#ifndef _DEBUG  // NotePickingView.cpp의 디버그 버전
inline CNoteEditingToolDoc* CNotePickingView::GetDocument() const
   { return reinterpret_cast<CNoteEditingToolDoc*>(m_pDocument); }
#endif

