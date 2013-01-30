#pragma once
#include "afxcmn.h"


// CPatternChange ��ȭ �����Դϴ�.

class CPatternChange : public CDialogEx
{
	DECLARE_DYNAMIC(CPatternChange)

public:
	CPatternChange(CWnd* pParent = NULL);   // ǥ�� �������Դϴ�.
	virtual ~CPatternChange();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_DIALOG_PATTERN };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnEnChangeEditNoteType();
	CString noteTypeString;
	afx_msg void OnEnChangeEditNoteType2();
	int notePatternNumber;
	afx_msg void OnDeltaposSpin1(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnBnClickedCharIdok();
	afx_msg void OnBnClickedCharIdcancel();
	CSpinButtonCtrl m_ctrSpin;
};
