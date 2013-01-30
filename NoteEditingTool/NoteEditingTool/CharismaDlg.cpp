// CharismaDlg.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "CharismaDlg.h"
#include "afxdialogex.h"


// CCharismaDlg 대화 상자입니다.

IMPLEMENT_DYNAMIC(CCharismaDlg, CDialogEx)

CCharismaDlg::CCharismaDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CCharismaDlg::IDD, pParent)
	, noteCharismaPoseNum(0)
	, noteCharismaNote(_T("Charisma Time Note"))
{

}

CCharismaDlg::~CCharismaDlg()
{
}

void CCharismaDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT_NOTE_TYPE2, noteCharismaPoseNum);
	DDV_MinMaxUInt(pDX, noteCharismaPoseNum, 1, 4);
	DDX_Text(pDX, IDC_EDIT_NOTE_TYPE, noteCharismaNote);
	DDX_Control(pDX, IDC_SPIN1, m_ctrSpin);
}


BEGIN_MESSAGE_MAP(CCharismaDlg, CDialogEx)
	ON_NOTIFY(UDN_DELTAPOS, IDC_SPIN1, &CCharismaDlg::OnDeltaposSpin1)
	ON_WM_INITMENU()
	ON_BN_CLICKED(B_CHAR_IDOK, &CCharismaDlg::OnBnClickedCharIdok)
	ON_BN_CLICKED(B_CHAR_IDCANCEL, &CCharismaDlg::OnBnClickedCharIdcancel)
	ON_EN_CHANGE(IDC_EDIT_NOTE_TYPE2, &CCharismaDlg::OnEnChangeEditNoteType2)
END_MESSAGE_MAP()


// CCharismaDlg 메시지 처리기입니다.


void CCharismaDlg::OnDeltaposSpin1(NMHDR *pNMHDR, LRESULT *pResult)
{
	m_ctrSpin.SetRange(1, 4);			// Spin Control이 이상해서 따로 설정.
	LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.



	*pResult = 0;
}


void CCharismaDlg::OnInitMenu(CMenu* pMenu)
{
	CDialogEx::OnInitMenu(pMenu);

	// TODO: 여기에 메시지 처리기 코드를 추가합니다.


	m_ctrSpin.SetRange(1, 4);			// Spin Control이 이상해서 따로 설정.
}


void CCharismaDlg::OnBnClickedCharIdok()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	OnOK();
}


void CCharismaDlg::OnBnClickedCharIdcancel()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.'
	OnCancel();
}


void CCharismaDlg::OnEnChangeEditNoteType2()
{
	// TODO:  RICHEDIT 컨트롤인 경우, 이 컨트롤은
	// CDialogEx::OnInitDialog() 함수를 재지정 
	//하고 마스크에 OR 연산하여 설정된 ENM_CHANGE 플래그를 지정하여 CRichEditCtrl().SetEventMask()를 호출하지 않으면
	// 이 알림 메시지를 보내지 않습니다.

	// TODO:  여기에 컨트롤 알림 처리기 코드를 추가합니다.
}
