
// MainFrm.h : CMainFrame Ŭ������ �������̽�
//

#pragma once


//#include "NoteEditingToolView.h"

// ���� â�� ������ ���ؼ� ������ ���� ��� Ŭ���� ����
class CSplitterX : public CSplitterWnd
{
public:
	CSplitterX() {};
	~CSplitterX() {};

protected:
public:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg LRESULT OnNcHitTest(CPoint point);
	// �� �κ��� �����ε��Ѵ�.
};




//class CNoteEditingToolView;					// Ŭ���� �̸��̶�� �ν����� ���ؼ� ��Ÿ���� ������ ���� ����.

class CMainFrame : public CFrameWndEx
{
	
protected: // serialization������ ��������ϴ�.
	CMainFrame();
	DECLARE_DYNCREATE(CMainFrame)

// Ư���Դϴ�.
public:
	//CSplitterWnd m_wndSplitter1;	// ���� �����츦 ���� �����ϴ� ��ü
	//CSplitterWnd m_wndSplitter2;	// ���� �����츦 ���� �����ϴ� ��ü
	CSplitterX m_wndSplitter1;	// ���� �����츦 ���� �����ϴ� ��ü
	CSplitterX m_wndSplitter2;	// ���� �����츦 ���� �����ϴ� ��ü


// �۾��Դϴ�.
public:
	BOOL OnCreateClient(LPCREATESTRUCT lpcs, CCreateContext* pContext);				// ȭ�� ������ ���� �Լ� ������.

// �������Դϴ�.
public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual BOOL LoadFrame(UINT nIDResource, DWORD dwDefaultStyle = WS_OVERLAPPEDWINDOW | FWS_ADDTOTITLE, CWnd* pParentWnd = NULL, CCreateContext* pContext = NULL);

// �����Դϴ�.
public:
	virtual ~CMainFrame();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:  // ��Ʈ�� ������ ���Ե� ����Դϴ�.
	CMFCMenuBar       m_wndMenuBar;
	CMFCToolBar       m_wndToolBar;
	CMFCStatusBar     m_wndStatusBar;
	CMFCToolBarImages m_UserImages;

// ������ �޽��� �� �Լ�
protected:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnViewCustomize();
	afx_msg LRESULT OnToolbarCreateNew(WPARAM wp, LPARAM lp);
	afx_msg void OnApplicationLook(UINT id);
	afx_msg void OnUpdateApplicationLook(CCmdUI* pCmdUI);
	DECLARE_MESSAGE_MAP()

public:
	afx_msg void OnGetMinMaxInfo(MINMAXINFO* lpMMI);
	afx_msg void OnSize(UINT nType, int cx, int cy);
};


