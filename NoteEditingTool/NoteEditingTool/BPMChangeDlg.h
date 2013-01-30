#pragma once
#include "afxcmn.h"


// CBPMChangeDlg 대화 상자입니다.

class CBPMChangeDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CBPMChangeDlg)

public:
	CBPMChangeDlg(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CBPMChangeDlg();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOG_BPMC };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
public:
	int noteBPMC;
	afx_msg void OnDeltaposSpin1(NMHDR *pNMHDR, LRESULT *pResult);
	CSpinButtonCtrl m_ctrSpin;
	afx_msg void OnBnClickedCharIdok();
	afx_msg void OnBnClickedCharIdcancel();
	afx_msg void OnEnChangeEditNoteType();
	CString noteBPMString;
};
