#pragma once


// CMnfSettingDlg ��ȭ �����Դϴ�.

class CMnfSettingDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CMnfSettingDlg)

public:
	CMnfSettingDlg(CWnd* pParent = NULL);   // ǥ�� �������Դϴ�.
	virtual ~CMnfSettingDlg();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_DIALOG_SETTING };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

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
