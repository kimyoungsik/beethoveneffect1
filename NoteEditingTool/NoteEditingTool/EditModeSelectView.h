#pragma once



#include "NoteEditingToolDoc.h"




// ���� �����

const int V_PENCIL_OFFSET_X = 0;			// ���� ����� ������ ������
const int V_PENCIL_OFFSET_Y = 0;
const int V_PENCIL_OFFSET_Y2 = 40;

const int V_TEXT_OFFSET_X = 0;				// �ؽ�Ʈ ������
const int V_TEXT_OFFSET_Y = 0;


// ���� ������ ���� ���콺 ��ǥ�� �����µ� ���.
const int V_MODE_AREA_WIDTH = 61;				// ���콺 ���� ������ �ʺ� ���.
const int V_MODE_AREA_HEIGHT = 46;				// ���콺 ���� ������ ���� ���.
const int V_SELECTED_PEN_X = 30;			// ���� ��� ���� Ŀ�� ������
const int V_SELECTED_PEN_Y = 22;
const int V_SELECTED_ERASE_X = V_SELECTED_PEN_X + V_MODE_AREA_WIDTH;
const int V_SELECTED_ERASE_Y = V_SELECTED_PEN_Y;
const int V_SELECTED_MOD_X = V_SELECTED_ERASE_X + V_MODE_AREA_WIDTH;
const int V_SELECTED_MOD_Y = V_SELECTED_PEN_Y;
const int V_SELECTED_CONFG_X = V_SELECTED_MOD_X + V_MODE_AREA_WIDTH;
const int V_SELECTED_CONFG_Y = V_SELECTED_PEN_Y;

// ��������� �����µ� ���
const int V_SELECTED_P_PLAY_X = 61;
const int V_SELECTED_P_PLAY_Y = 81;
const int V_SELECTED_P_PAUSE_X = V_SELECTED_P_PLAY_X + V_MODE_AREA_WIDTH;
const int V_SELECTED_P_PAUSE_Y = V_SELECTED_P_PLAY_Y;
const int V_SELECTED_P_STOP_X = V_SELECTED_P_PAUSE_X + V_MODE_AREA_WIDTH;
const int V_SELECTED_P_STOP_Y = V_SELECTED_P_PLAY_Y;




const int V_TEXT_AREA_WIDTH = 241;						// ���콺 ���� ������ �ʺ� ���.
const int V_TEXT_AREA_HEIGHT = 27;						// ���콺 ���� ������ ���� ���.
const int V_TEXT_AREA_INTERVAL = 13;					// �� �ؽ�Ʈ ���� �� ���� ���.

const int V_SELECTED_1_X = 30;				// ���콺 Ŭ�� �޴� 1 ������
const int V_SELECTED_1_Y = 158;
const int V_SELECTED_2_X = V_SELECTED_1_X;				// ���콺 Ŭ�� �޴� 2 ������
const int V_SELECTED_2_Y = V_SELECTED_1_Y + V_TEXT_AREA_HEIGHT;
const int V_SELECTED_3_X = V_SELECTED_1_X;				// ���콺 Ŭ�� �޴� 3 ������
const int V_SELECTED_3_Y = V_SELECTED_2_Y + V_TEXT_AREA_HEIGHT;
const int V_SELECTED_4_X = V_SELECTED_1_X;				// ���콺 Ŭ�� �޴� 4 ������
const int V_SELECTED_4_Y = V_SELECTED_3_Y + V_TEXT_AREA_HEIGHT + V_TEXT_AREA_INTERVAL;
const int V_SELECTED_5_X = V_SELECTED_1_X;				// ���콺 Ŭ�� �޴� 5 ������
const int V_SELECTED_5_Y = V_SELECTED_4_Y + V_TEXT_AREA_HEIGHT;
const int V_SELECTED_6_X = V_SELECTED_1_X;				// ���콺 Ŭ�� �޴� 6 ������
const int V_SELECTED_6_Y = V_SELECTED_5_Y + V_TEXT_AREA_HEIGHT;
const int V_SELECTED_7_X = V_SELECTED_1_X;				// ���콺 Ŭ�� �޴� 7 ������
const int V_SELECTED_7_Y = V_SELECTED_6_Y + V_TEXT_AREA_HEIGHT + 1;
const int V_SELECTED_8_X = V_SELECTED_1_X;				// ���콺 Ŭ�� �޴� 8 ������
const int V_SELECTED_8_Y = V_SELECTED_7_Y + V_TEXT_AREA_HEIGHT + V_TEXT_AREA_INTERVAL - 1;
const int V_SELECTED_9_X = V_SELECTED_1_X;				// ���콺 Ŭ�� �޴� 9 ������
const int V_SELECTED_9_Y = V_SELECTED_8_Y + V_TEXT_AREA_HEIGHT + 1;
// ���� ������ ���� ���콺 ��ǥ�� �����µ� ���.

const int V_AREA_X1 = V_SELECTED_1_X;
const int V_AREA_X2 = V_TEXT_AREA_WIDTH - V_SELECTED_1_X;














// CEditModeSelectView ���Դϴ�.

class CEditModeSelectView : public CView
{
// �Ӽ��Դϴ�.
protected:
	//CImage m_bmpBitmap;
	bool onMouseTextFlag;
	bool onMouseEditModeFlag;








	DECLARE_DYNCREATE(CEditModeSelectView)



// Ư���Դϴ�.
protected:
	// ���� ���� �� ���� ����� ���� �������� ��ǥ �����ϴ� �Լ�.
	int calSelectedMenuCursor(const char noteWriteType, int &selectedMenuCursorX, int &selectedMenuCursorY);
	// ���� ���� �� ������ ��忡 ���� �������� ��ǥ �����ϴ� �Լ�.
	int calSelectedModeCursor(const char editModeType, int &selectedMenuCursorX, int &selectedMenuCursorY);
	//// ���� ���õǾ� �� ���� ����� �ǹٸ����� Ȯ���ϴ� �Լ�.
	//int chkSelectedMenuCursor(const char noteWriteType);
	// ���� � ������������� ���� Ŀ���� ��ǥ�� ����ϴ� �Լ�.
//	int calSelected
	// Ŭ���������� ���콺 ��ġ�� ���� ��ǥ�� ����ؼ� ����ϴ� �Լ�.
	char calMouseSelectArea(const int pointX, const int pointY);
	// ���� ���콺 ��ġ�� ���� ��ǥ�� ����ؼ� ����ϴ� �Լ�.
	char calMouseMoveArea(const int pointX, const int pointY);


	// �÷��� ������ ���¸� Ŭ���������� ���콺 ��ġ�� ���� ��ǥ�� ����ؼ� ����ϴ� �Լ�.
	int calMouseSelectPlayArea(const int pointX, const int pointY);
	// ���� ���� �� ������ ��忡 ���� �������� ��ǥ �����ϴ� �Լ�.
	int calSelectedPlayCursor(const int nowPlayingStatus, int &selectedMenuCursorX, int &selectedMenuCursorY);



protected:
	CEditModeSelectView();           // ���� ����⿡ ���Ǵ� protected �������Դϴ�.
	virtual ~CEditModeSelectView();
	virtual void OnInitialUpdate();     // ������ �� ó���Դϴ�.

public:
	virtual void OnDraw(CDC* pDC);      // �� �並 �׸��� ���� �����ǵǾ����ϴ�.
#ifdef _DEBUG
	virtual void AssertValid() const;
//#ifndef _WIN32_WCE
	virtual void Dump(CDumpContext& dc) const;
//#endif
#endif

protected:
	DECLARE_MESSAGE_MAP()


///////////////////////////////////////////////////////////
// NoteEditingToolDoc�� �����ϱ� ���ؼ� �����ϴ� �κ�.
// ���Ϻ��� ���� ����� ������ ����.
public:
	
	CNoteEditingToolDoc* GetDocument() const;			// Picking View�� ���� document�� ���.
//	virtual void OnInitialUpdate();     // ������ �� ó���Դϴ�.

	
// �������Դϴ�.
public:
//	virtual void OnDraw(CDC* pDC);  // �� �並 �׸��� ���� �����ǵǾ����ϴ�.
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






#ifndef _DEBUG  // NotePickingView.cpp�� ����� ����
inline CNoteEditingToolDoc* CEditModeSelectView::GetDocument() const
   { return reinterpret_cast<CNoteEditingToolDoc*>(m_pDocument); }
#endif
