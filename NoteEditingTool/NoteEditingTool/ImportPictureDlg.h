#pragma once


// CImportPictureDlg 대화 상자입니다.

class CImportPictureDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CImportPictureDlg)

public:
	CImportPictureDlg(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CImportPictureDlg();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_DIALOG_IMPORT_PIC };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnPaint();
	CString m_PictureFilename;
	afx_msg void OnBnClickedButtonFindPic();
	afx_msg void OnBnClickedIdok();
	afx_msg void OnBnClickedIdcancel();
	afx_msg void OnEnChangeEdit1();
};
