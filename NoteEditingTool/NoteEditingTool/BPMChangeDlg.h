#pragma once
#include "afxcmn.h"


// CBPMChangeDlg ��ȭ �����Դϴ�.

class CBPMChangeDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CBPMChangeDlg)

public:
	CBPMChangeDlg(CWnd* pParent = NULL);   // ǥ�� �������Դϴ�.
	virtual ~CBPMChangeDlg();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_DIALOG_BPMC };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

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
