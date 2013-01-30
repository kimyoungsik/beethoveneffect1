#pragma once
#include "afxcmn.h"


// CPatternChange 대화 상자입니다.

class CPatternChange : public CDialogEx
{
	DECLARE_DYNAMIC(CPatternChange)

public:
	CPatternChange(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CPatternChange();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOG_PATTERN };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

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
