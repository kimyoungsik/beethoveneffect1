#pragma once


// CImportPictureDlg ��ȭ �����Դϴ�.

class CImportPictureDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CImportPictureDlg)

public:
	CImportPictureDlg(CWnd* pParent = NULL);   // ǥ�� �������Դϴ�.
	virtual ~CImportPictureDlg();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_DIALOG_IMPORT_PIC };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnPaint();
	CString m_PictureFilename;
	afx_msg void OnBnClickedButtonFindPic();
	afx_msg void OnBnClickedIdok();
	afx_msg void OnBnClickedIdcancel();
	afx_msg void OnEnChangeEdit1();
};
