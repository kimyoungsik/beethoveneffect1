// ImportPictureDlg.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "ImportPictureDlg.h"
#include "afxdialogex.h"


// GDI+용 전역변수.
GdiplusStartupInput           g_gidplusStartupInput;
ULONG_PTR                     g_gdiplusToken;
GdiplusStartupOutput         g_gidplusStartupOutput;


// CImportPictureDlg 대화 상자입니다.

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


// CImportPictureDlg 메시지 처리기입니다.


void CImportPictureDlg::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	// TODO: 여기에 메시지 처리기 코드를 추가합니다.
	// 그리기 메시지에 대해서는 CDialogEx::OnPaint()을(를) 호출하지 마십시오.


	//CPaintDC cdc(this);
	//Graphics G(cdc);
	//LPWSTR wchar_strFileame = new WCHAR[UI_MAX_SIZE];
	//int nLen = MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED,filename,-1,wchar_strFileame, UI_MAX_SIZE);
	//wchar_strFileame[nLen] = '\0';         
	//Image pI(wchar_strFileame);
	//G.DrawImage(&pI,x,y,pI.GetWidth(),pI.GetHeight());  //x,y는 그림이 그려질 int형 좌표


}


void CImportPictureDlg::OnBnClickedButtonFindPic()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.

	int i=0;
	int fileNameStartIndex=0;

	CString tempString = _T("");

	// 파일을 연다.
	WCHAR file[MAX_PATH] = _T("");
	// TCHAR로 정의 할 경우 자동으로 WCHAR로 변환 해 주지만, BASS_StreamCreateFile의 BASS_UNICODE Flag를 세워줬기 때문에 명시적으로 정의한다.

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
		AfxMessageBox(_T("그림파일을 선택하지 않았습니다."));
		return;
	}

	// 파일을 파싱해서, ~~~.jpg만 찾아낸다.
	while( file[i] != NULL )
	{
		if ( i >= MAX_PATH )
		{
			AfxMessageBox(_T("파일 경로 파싱에서 에러 발생"));
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

	// 변수에 있는 값이 컨트롤 화면에 나타나도록 한다.
	UpdateData(FALSE);
}


void CImportPictureDlg::OnBnClickedIdok()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	OnOK();
}


void CImportPictureDlg::OnBnClickedIdcancel()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	OnCancel();
}


void CImportPictureDlg::OnEnChangeEdit1()
{
	// TODO:  RICHEDIT 컨트롤인 경우, 이 컨트롤은
	// CDialogEx::OnInitDialog() 함수를 재지정 
	//하고 마스크에 OR 연산하여 설정된 ENM_CHANGE 플래그를 지정하여 CRichEditCtrl().SetEventMask()를 호출하지 않으면
	// 이 알림 메시지를 보내지 않습니다.

	// TODO:  여기에 컨트롤 알림 처리기 코드를 추가합니다.
}
