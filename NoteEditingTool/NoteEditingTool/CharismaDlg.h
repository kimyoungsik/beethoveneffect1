#pragma once
#include "afxcmn.h"


// CCharismaDlg ��ȭ �����Դϴ�.

class CCharismaDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CCharismaDlg)

public:
	CCharismaDlg(CWnd* pParent = NULL);   // ǥ�� �������Դϴ�.
	virtual ~CCharismaDlg();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_DIALOG_CHARISMA };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

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
