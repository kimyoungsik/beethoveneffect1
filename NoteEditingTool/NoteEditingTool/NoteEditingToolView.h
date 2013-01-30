
// NoteEditingToolView.h : CNoteEditingToolView 클래스의 인터페이스
//

#pragma once

//#ifndef __CM_REDRAW_DRAG_VIEW
//#define __CM_REDRAW_DRAG_VIEW
//UINT CM_REDRAW_DRAG_VIEW = RegisterWindowMessage(_T("CM_REDRAW_DRAG_VIEW"));
//#endif

const unsigned int D_CIRCLE_SIZE = 30;				// 그릴 원의 반지름의 크기
const unsigned int D_CIRCLE_STROK_SIZE = 3;			// 원 테두리의 두께
const unsigned int D_CIRCLE_DOT_SIZE = 8;			// 그릴 보조점의 반지름의 크기
const unsigned int D_GAME_CENTER_POINT_X = 512;		// 게임 상에서의 중점의 좌표.
const unsigned int D_GAME_CENTER_POINT_Y = 420;

const unsigned int D_GAME_CENTER_IDEAL_POINT_X = 1024/2;		// 게임 해상도 상에서의 중점의 좌표.
const unsigned int D_GAME_CENTER_IDEAL_POINT_Y = 768/2;

const double D_TOOL_TO_GAME = (double)D_GAME_CENTER_IDEAL_POINT_X / ((double)FRAME_2_WIDTH / 2);
	// (편집프로그램 에서의 좌표) * (D_TOOL_TO_GAME) = (게임상에서의 좌표)


// 사용 할 색
const COLORREF C_BEZIER_INNER_LINE_COLOR = RGB(159, 170, 232);	// 베지어 곡선용 색
const COLORREF C_BEZIER_OUTER_LINE_COLOR = RGB(222, 222, 222);		// 베지어 곡선 테두리 색
const unsigned int D_BEZIER_STROK_SIZE = 10;						// 베지어 곡선 테두리 두께




class CNoteEditingToolView : public CView
{
protected:
	int dotDataIndex;						// 평소에는 -1이다. 0~3 사이의 값을 가질 경우, 현재 포커싱 되어 있다는 뜻이다.
	bool OnFocusFlag;						// T일 경우, 현재 잡아끌고 있는 중이다.



protected: // serialization에서만 만들어집니다.
	CNoteEditingToolView();
	DECLARE_DYNCREATE(CNoteEditingToolView)

// 특성입니다.
public:
	CNoteEditingToolDoc* GetDocument() const;

// 작업입니다.
protected:
	// 드래그 노트 기본 좌표축을 그리는 함수.
	int DrawBackXYLine(CDC* pDC);

public:



// 재정의입니다.
public:
	virtual void OnDraw(CDC* pDC);  // 이 뷰를 그리기 위해 재정의되었습니다.
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
protected:
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);

// 구현입니다.
public:
	virtual ~CNoteEditingToolView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// 생성된 메시지 맵 함수
protected:
	afx_msg void OnFilePrintPreview();
	afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnContextMenu(CWnd* pWnd, CPoint point);
	afx_msg LRESULT OnCmRedrawDragView(WPARAM wParam, LPARAM lParam);
//	afx_msg LRESULT TEST();
	DECLARE_MESSAGE_MAP()


public:
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
};

#ifndef _DEBUG  // NoteEditingToolView.cpp의 디버그 버전
inline CNoteEditingToolDoc* CNoteEditingToolView::GetDocument() const
   { return reinterpret_cast<CNoteEditingToolDoc*>(m_pDocument); }
#endif

