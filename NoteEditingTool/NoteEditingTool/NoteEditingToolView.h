
// NoteEditingToolView.h : CNoteEditingToolView Ŭ������ �������̽�
//

#pragma once

//#ifndef __CM_REDRAW_DRAG_VIEW
//#define __CM_REDRAW_DRAG_VIEW
//UINT CM_REDRAW_DRAG_VIEW = RegisterWindowMessage(_T("CM_REDRAW_DRAG_VIEW"));
//#endif

const unsigned int D_CIRCLE_SIZE = 30;				// �׸� ���� �������� ũ��
const unsigned int D_CIRCLE_STROK_SIZE = 3;			// �� �׵θ��� �β�
const unsigned int D_CIRCLE_DOT_SIZE = 8;			// �׸� �������� �������� ũ��
const unsigned int D_GAME_CENTER_POINT_X = 512;		// ���� �󿡼��� ������ ��ǥ.
const unsigned int D_GAME_CENTER_POINT_Y = 420;

const unsigned int D_GAME_CENTER_IDEAL_POINT_X = 1024/2;		// ���� �ػ� �󿡼��� ������ ��ǥ.
const unsigned int D_GAME_CENTER_IDEAL_POINT_Y = 768/2;

const double D_TOOL_TO_GAME = (double)D_GAME_CENTER_IDEAL_POINT_X / ((double)FRAME_2_WIDTH / 2);
	// (�������α׷� ������ ��ǥ) * (D_TOOL_TO_GAME) = (���ӻ󿡼��� ��ǥ)


// ��� �� ��
const COLORREF C_BEZIER_INNER_LINE_COLOR = RGB(159, 170, 232);	// ������ ��� ��
const COLORREF C_BEZIER_OUTER_LINE_COLOR = RGB(222, 222, 222);		// ������ � �׵θ� ��
const unsigned int D_BEZIER_STROK_SIZE = 10;						// ������ � �׵θ� �β�




class CNoteEditingToolView : public CView
{
protected:
	int dotDataIndex;						// ��ҿ��� -1�̴�. 0~3 ������ ���� ���� ���, ���� ��Ŀ�� �Ǿ� �ִٴ� ���̴�.
	bool OnFocusFlag;						// T�� ���, ���� ��Ʋ��� �ִ� ���̴�.



protected: // serialization������ ��������ϴ�.
	CNoteEditingToolView();
	DECLARE_DYNCREATE(CNoteEditingToolView)

// Ư���Դϴ�.
public:
	CNoteEditingToolDoc* GetDocument() const;

// �۾��Դϴ�.
protected:
	// �巡�� ��Ʈ �⺻ ��ǥ���� �׸��� �Լ�.
	int DrawBackXYLine(CDC* pDC);

public:



// �������Դϴ�.
public:
	virtual void OnDraw(CDC* pDC);  // �� �並 �׸��� ���� �����ǵǾ����ϴ�.
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
protected:
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);

// �����Դϴ�.
public:
	virtual ~CNoteEditingToolView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// ������ �޽��� �� �Լ�
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

#ifndef _DEBUG  // NoteEditingToolView.cpp�� ����� ����
inline CNoteEditingToolDoc* CNoteEditingToolView::GetDocument() const
   { return reinterpret_cast<CNoteEditingToolDoc*>(m_pDocument); }
#endif

