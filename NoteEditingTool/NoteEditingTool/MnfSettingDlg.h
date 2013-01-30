#pragma once


// CMnfSettingDlg 대화 상자입니다.

class CMnfSettingDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CMnfSettingDlg)

public:
	CMnfSettingDlg(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CMnfSettingDlg();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOG_SETTING };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnEnChangeEdit1();
	afx_msg void OnEnChangeEditTitle();
	CString mnfTitle;
	CString mnfArtist;

	afx_msg void OnBnClickedIdOk();
	afx_msg void OnBnClickedIdCancel();
	unsigned int mnfLevel;
	afx_msg void OnEnChangeEditLevel();
	unsigned char mnfInitBPM;
	int startTimeMin;
	int endTimeMin;
	int startTimeSec;
	int endTimeSec;
	afx_msg void OnEnChangeEditStartTimeSec();
	int startTimeMilSec;
	int endTimeMilSec;
};
