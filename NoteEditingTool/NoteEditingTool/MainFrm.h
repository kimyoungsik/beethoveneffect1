
// MainFrm.h : CMainFrame 클래스의 인터페이스
//

#pragma once


//#include "NoteEditingToolView.h"

// 분할 창의 고정을 위해서 다음과 같은 상속 클래스 생성
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
	// 이 부분을 오버로딩한다.
};




//class CNoteEditingToolView;					// 클래스 이름이라고 인식하지 못해서 나타나는 에러를 위한 선언.

class CMainFrame : public CFrameWndEx
{
	
protected: // serialization에서만 만들어집니다.
	CMainFrame();
	DECLARE_DYNCREATE(CMainFrame)

// 특성입니다.
public:
	//CSplitterWnd m_wndSplitter1;	// 분할 윈도우를 위해 선언하는 객체
	//CSplitterWnd m_wndSplitter2;	// 분할 윈도우를 위해 선언하는 객체
	CSplitterX m_wndSplitter1;	// 분할 윈도우를 위해 선언하는 객체
	CSplitterX m_wndSplitter2;	// 분할 윈도우를 위해 선언하는 객체


// 작업입니다.
public:
	BOOL OnCreateClient(LPCREATESTRUCT lpcs, CCreateContext* pContext);				// 화면 분할을 위한 함수 재정의.

// 재정의입니다.
public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual BOOL LoadFrame(UINT nIDResource, DWORD dwDefaultStyle = WS_OVERLAPPEDWINDOW | FWS_ADDTOTITLE, CWnd* pParentWnd = NULL, CCreateContext* pContext = NULL);

// 구현입니다.
public:
	virtual ~CMainFrame();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:  // 컨트롤 모음이 포함된 멤버입니다.
	CMFCMenuBar       m_wndMenuBar;
	CMFCToolBar       m_wndToolBar;
	CMFCStatusBar     m_wndStatusBar;
	CMFCToolBarImages m_UserImages;

// 생성된 메시지 맵 함수
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


