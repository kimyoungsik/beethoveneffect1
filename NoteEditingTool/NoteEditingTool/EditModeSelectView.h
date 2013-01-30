#pragma once



#include "NoteEditingToolDoc.h"




// 관련 상수들

const int V_PENCIL_OFFSET_X = 0;			// 연필 모양의 아이콘 오프셋
const int V_PENCIL_OFFSET_Y = 0;
const int V_PENCIL_OFFSET_Y2 = 40;

const int V_TEXT_OFFSET_X = 0;				// 텍스트 오프셋
const int V_TEXT_OFFSET_Y = 0;


// 쓰기 종류에 따른 마우스 좌표의 오프셋들 계산.
const int V_MODE_AREA_WIDTH = 61;				// 마우스 선택 영역의 너비 계산.
const int V_MODE_AREA_HEIGHT = 46;				// 마우스 선택 영역의 높이 계산.
const int V_SELECTED_PEN_X = 30;			// 쓰기 모드 선택 커서 오프셋
const int V_SELECTED_PEN_Y = 22;
const int V_SELECTED_ERASE_X = V_SELECTED_PEN_X + V_MODE_AREA_WIDTH;
const int V_SELECTED_ERASE_Y = V_SELECTED_PEN_Y;
const int V_SELECTED_MOD_X = V_SELECTED_ERASE_X + V_MODE_AREA_WIDTH;
const int V_SELECTED_MOD_Y = V_SELECTED_PEN_Y;
const int V_SELECTED_CONFG_X = V_SELECTED_MOD_X + V_MODE_AREA_WIDTH;
const int V_SELECTED_CONFG_Y = V_SELECTED_PEN_Y;

// 재생아이콘 오프셋들 계산
const int V_SELECTED_P_PLAY_X = 61;
const int V_SELECTED_P_PLAY_Y = 81;
const int V_SELECTED_P_PAUSE_X = V_SELECTED_P_PLAY_X + V_MODE_AREA_WIDTH;
const int V_SELECTED_P_PAUSE_Y = V_SELECTED_P_PLAY_Y;
const int V_SELECTED_P_STOP_X = V_SELECTED_P_PAUSE_X + V_MODE_AREA_WIDTH;
const int V_SELECTED_P_STOP_Y = V_SELECTED_P_PLAY_Y;




const int V_TEXT_AREA_WIDTH = 241;						// 마우스 선택 영역의 너비 계산.
const int V_TEXT_AREA_HEIGHT = 27;						// 마우스 선택 영역의 높이 계산.
const int V_TEXT_AREA_INTERVAL = 13;					// 각 텍스트 종류 별 간격 계산.

const int V_SELECTED_1_X = 30;				// 마우스 클릭 메뉴 1 오프셋
const int V_SELECTED_1_Y = 158;
const int V_SELECTED_2_X = V_SELECTED_1_X;				// 마우스 클릭 메뉴 2 오프셋
const int V_SELECTED_2_Y = V_SELECTED_1_Y + V_TEXT_AREA_HEIGHT;
const int V_SELECTED_3_X = V_SELECTED_1_X;				// 마우스 클릭 메뉴 3 오프셋
const int V_SELECTED_3_Y = V_SELECTED_2_Y + V_TEXT_AREA_HEIGHT;
const int V_SELECTED_4_X = V_SELECTED_1_X;				// 마우스 클릭 메뉴 4 오프셋
const int V_SELECTED_4_Y = V_SELECTED_3_Y + V_TEXT_AREA_HEIGHT + V_TEXT_AREA_INTERVAL;
const int V_SELECTED_5_X = V_SELECTED_1_X;				// 마우스 클릭 메뉴 5 오프셋
const int V_SELECTED_5_Y = V_SELECTED_4_Y + V_TEXT_AREA_HEIGHT;
const int V_SELECTED_6_X = V_SELECTED_1_X;				// 마우스 클릭 메뉴 6 오프셋
const int V_SELECTED_6_Y = V_SELECTED_5_Y + V_TEXT_AREA_HEIGHT;
const int V_SELECTED_7_X = V_SELECTED_1_X;				// 마우스 클릭 메뉴 7 오프셋
const int V_SELECTED_7_Y = V_SELECTED_6_Y + V_TEXT_AREA_HEIGHT + 1;
const int V_SELECTED_8_X = V_SELECTED_1_X;				// 마우스 클릭 메뉴 8 오프셋
const int V_SELECTED_8_Y = V_SELECTED_7_Y + V_TEXT_AREA_HEIGHT + V_TEXT_AREA_INTERVAL - 1;
const int V_SELECTED_9_X = V_SELECTED_1_X;				// 마우스 클릭 메뉴 9 오프셋
const int V_SELECTED_9_Y = V_SELECTED_8_Y + V_TEXT_AREA_HEIGHT + 1;
// 쓰기 종류에 따른 마우스 좌표의 오프셋들 계산.

const int V_AREA_X1 = V_SELECTED_1_X;
const int V_AREA_X2 = V_TEXT_AREA_WIDTH - V_SELECTED_1_X;














// CEditModeSelectView 뷰입니다.

class CEditModeSelectView : public CView
{
// 속성입니다.
protected:
	//CImage m_bmpBitmap;
	bool onMouseTextFlag;
	bool onMouseEditModeFlag;








	DECLARE_DYNCREATE(CEditModeSelectView)



// 특성입니다.
protected:
	// 현재 선택 한 쓰기 방법에 따른 변수들의 좌표 설정하는 함수.
	int calSelectedMenuCursor(const char noteWriteType, int &selectedMenuCursorX, int &selectedMenuCursorY);
	// 현재 선택 한 에디팅 모드에 따른 변수들의 좌표 설정하는 함수.
	int calSelectedModeCursor(const char editModeType, int &selectedMenuCursorX, int &selectedMenuCursorY);
	//// 현재 선택되어 진 쓰기 방법이 옳바른가를 확인하는 함수.
	//int chkSelectedMenuCursor(const char noteWriteType);
	// 현재 어떤 편집모드인지에 따른 커서의 좌표를 출력하는 함수.
//	int calSelected
	// 클릭했을때의 마우스 위치의 따른 좌표를 계산해서 출력하는 함수.
	char calMouseSelectArea(const int pointX, const int pointY);
	// 현재 마우스 위치의 따른 좌표를 계산해서 출력하는 함수.
	char calMouseMoveArea(const int pointX, const int pointY);


	// 플레이 상태의 형태를 클릭했을때의 마우스 위치의 따른 좌표를 계산해서 출력하는 함수.
	int calMouseSelectPlayArea(const int pointX, const int pointY);
	// 현재 선택 한 에디팅 모드에 따른 변수들의 좌표 설정하는 함수.
	int calSelectedPlayCursor(const int nowPlayingStatus, int &selectedMenuCursorX, int &selectedMenuCursorY);



protected:
	CEditModeSelectView();           // 동적 만들기에 사용되는 protected 생성자입니다.
	virtual ~CEditModeSelectView();
	virtual void OnInitialUpdate();     // 생성된 후 처음입니다.

public:
	virtual void OnDraw(CDC* pDC);      // 이 뷰를 그리기 위해 재정의되었습니다.
#ifdef _DEBUG
	virtual void AssertValid() const;
//#ifndef _WIN32_WCE
	virtual void Dump(CDumpContext& dc) const;
//#endif
#endif

protected:
	DECLARE_MESSAGE_MAP()


///////////////////////////////////////////////////////////
// NoteEditingToolDoc와 연동하기 위해서 설정하는 부분.
// 이하부터 밖의 디버그 모드까지 포함.
public:
	
	CNoteEditingToolDoc* GetDocument() const;			// Picking View는 같은 document를 사용.
//	virtual void OnInitialUpdate();     // 생성된 후 처음입니다.

	
// 재정의입니다.
public:
//	virtual void OnDraw(CDC* pDC);  // 이 뷰를 그리기 위해 재정의되었습니다.
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
protected:
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);





public:
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
protected:
	afx_msg LRESULT OnCmRedrawView(WPARAM wParam, LPARAM lParam);
public:
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
protected:
	afx_msg LRESULT OnCmEditingModeChange(WPARAM wParam, LPARAM lParam);
};






#ifndef _DEBUG  // NotePickingView.cpp의 디버그 버전
inline CNoteEditingToolDoc* CEditModeSelectView::GetDocument() const
   { return reinterpret_cast<CNoteEditingToolDoc*>(m_pDocument); }
#endif
