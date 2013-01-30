// ImportPictureDlg.cpp : ���� �����Դϴ�.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "ImportPictureDlg.h"
#include "afxdialogex.h"


// GDI+�� ��������.
GdiplusStartupInput           g_gidplusStartupInput;
ULONG_PTR                     g_gdiplusToken;
GdiplusStartupOutput         g_gidplusStartupOutput;


// CImportPictureDlg ��ȭ �����Դϴ�.

IMPLEMENT_DYNAMIC(CImportPictureDlg, CDialogEx)

CImportPictureDlg::CImportPictureDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CImportPictureDlg::IDD, pParent)
	, m_PictureFilename(_T(""))
{

}

CImportPictureDlg::~CImportPictureDlg()
{
}

void CImportPictureDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT1, m_PictureFilename);
	DDV_MaxChars(pDX, m_PictureFilename, 250);
}


BEGIN_MESSAGE_MAP(CImportPictureDlg, CDialogEx)
	ON_WM_PAINT()
	ON_BN_CLICKED(IDC_BUTTON_FIND_PIC, &CImportPictureDlg::OnBnClickedButtonFindPic)
	ON_BN_CLICKED(IMPORT_IDOK, &CImportPictureDlg::OnBnClickedIdok)
	ON_BN_CLICKED(IMPORT_IDCANCEL, &CImportPictureDlg::OnBnClickedIdcancel)
	ON_EN_CHANGE(IDC_EDIT1, &CImportPictureDlg::OnEnChangeEdit1)
END_MESSAGE_MAP()


// CImportPictureDlg �޽��� ó�����Դϴ�.


void CImportPictureDlg::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰��մϴ�.
	// �׸��� �޽����� ���ؼ��� CDialogEx::OnPaint()��(��) ȣ������ ���ʽÿ�.


	//CPaintDC cdc(this);
	//Graphics G(cdc);
	//LPWSTR wchar_strFileame = new WCHAR[UI_MAX_SIZE];
	//int nLen = MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED,filename,-1,wchar_strFileame, UI_MAX_SIZE);
	//wchar_strFileame[nLen] = '\0';         
	//Image pI(wchar_strFileame);
	//G.DrawImage(&pI,x,y,pI.GetWidth(),pI.GetHeight());  //x,y�� �׸��� �׷��� int�� ��ǥ


}


void CImportPictureDlg::OnBnClickedButtonFindPic()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.

	int i=0;
	int fileNameStartIndex=0;

	CString tempString = _T("");

	// ������ ����.
	WCHAR file[MAX_PATH] = _T("");
	// TCHAR�� ���� �� ��� �ڵ����� WCHAR�� ��ȯ �� ������, BASS_StreamCreateFile�� BASS_UNICODE Flag�� ������� ������ ��������� �����Ѵ�.

	OPENFILENAME ofn={0};
	ofn.lStructSize=sizeof(ofn);
	//ofn.hwndOwner=win;
	ofn.nMaxFile = MAX_PATH;
	ofn.lpstrFile = file;
	//swprintf(ofn.lpstrFile, _T("%s"), file);
	ofn.Flags=OFN_FILEMUSTEXIST|OFN_HIDEREADONLY|OFN_EXPLORER;
	ofn.lpstrTitle = _T("Select a file to Title picture");
	ofn.lpstrFilter= _T("JPEG File (*.jpg)\0 *.jpg\0 PNG File (*.png)\0 *.png\0 GIF File (*.gif)\0 *.gif\0");
	if (!GetOpenFileName(&ofn))
	{
		AfxMessageBox(_T("�׸������� �������� �ʾҽ��ϴ�."));
		return;
	}

	// ������ �Ľ��ؼ�, ~~~.jpg�� ã�Ƴ���.
	while( file[i] != NULL )
	{
		if ( i >= MAX_PATH )
		{
			AfxMessageBox(_T("���� ��� �Ľ̿��� ���� �߻�"));
			return;
		}

		if ( file[i] == '\\' )
		{
			fileNameStartIndex = i;
		}
		
		i++;
	}

	
	for ( i = (fileNameStartIndex+1) ; (file[i] != NULL) &&  ( i < MAX_PATH )  ; i++ )
	{
		tempString += file[i];
	}

	m_PictureFilename = tempString;

	// ������ �ִ� ���� ��Ʈ�� ȭ�鿡 ��Ÿ������ �Ѵ�.
	UpdateData(FALSE);
}


void CImportPictureDlg::OnBnClickedIdok()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	OnOK();
}


void CImportPictureDlg::OnBnClickedIdcancel()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	OnCancel();
}


void CImportPictureDlg::OnEnChangeEdit1()
{
	// TODO:  RICHEDIT ��Ʈ���� ���, �� ��Ʈ����
	// CDialogEx::OnInitDialog() �Լ��� ������ 
	//�ϰ� ����ũ�� OR �����Ͽ� ������ ENM_CHANGE �÷��׸� �����Ͽ� CRichEditCtrl().SetEventMask()�� ȣ������ ������
	// �� �˸� �޽����� ������ �ʽ��ϴ�.

	// TODO:  ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
}
