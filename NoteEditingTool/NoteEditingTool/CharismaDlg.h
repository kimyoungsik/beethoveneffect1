#pragma once
#include "afxcmn.h"


// CCharismaDlg 대화 상자입니다.

class CCharismaDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CCharismaDlg)

public:
	CCharismaDlg(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CCharismaDlg();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOG_CHARISMA };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()

public:
	int noteCharismaPoseNum;
	CString noteCharismaNote;
	afx_msg void OnDeltaposSpin1(NMHDR *pNMHDR, LRESULT *pResult);
	CSpinButtonCtrl m_ctrSpin;
	afx_msg void OnInitMenu(CMenu* pMenu);
	afx_msg void OnBnClickedCharIdok();
	afx_msg void OnBnClickedCharIdcancel();
	afx_msg void OnEnChangeEditNoteType2();
};
