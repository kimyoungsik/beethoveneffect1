// BPMChangeDlg.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "BPMChangeDlg.h"
#include "afxdialogex.h"


// CBPMChangeDlg 대화 상자입니다.

IMPLEMENT_DYNAMIC(CBPMChangeDlg, CDialogEx)

CBPMChangeDlg::CBPMChangeDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CBPMChangeDlg::IDD, pParent)
	, noteBPMC(1)
	, noteBPMString(_T("BPM Change Note"))
{

}

CBPMChangeDlg::~CBPMChangeDlg()
{
}

void CBPMChangeDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT_NOTE_TYPE2, noteBPMC);
	DDV_MinMaxInt(pDX, noteBPMC, 1, 250);
	DDX_Control(pDX, IDC_SPIN1, m_ctrSpin);
	DDX_Text(pDX, IDC_EDIT_NOTE_TYPE, noteBPMString);
}


BEGIN_MESSAGE_MAP(CBPMChangeDlg, CDialogEx)
	ON_NOTIFY(UDN_DELTAPOS, IDC_SPIN1, &CBPMChangeDlg::OnDeltaposSpin1)
	ON_BN_CLICKED(B_CHAR_IDOK, &CBPMChangeDlg::OnBnClickedCharIdok)
	ON_BN_CLICKED(B_CHAR_IDCANCEL, &CBPMChangeDlg::OnBnClickedCharIdcancel)
	ON_EN_CHANGE(IDC_EDIT_NOTE_TYPE, &CBPMChangeDlg::OnEnChangeEditNoteType)
END_MESSAGE_MAP()


// CBPMChangeDlg 메시지 처리기입니다.


void CBPMChangeDlg::OnDeltaposSpin1(NMHDR *pNMHDR, LRESULT *pResult)
{
	m_ctrSpin.SetRange(1, 250);			// Spin Control이 이상해서 따로 설정.
	LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	*pResult = 0;
}


void CBPMChangeDlg::OnBnClickedCharIdok()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	OnOK();
}


void CBPMChangeDlg::OnBnClickedCharIdcancel()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	OnCancel();
}


void CBPMChangeDlg::OnEnChangeEditNoteType()
{
	// TODO:  RICHEDIT 컨트롤인 경우, 이 컨트롤은
	// CDialogEx::OnInitDialog() 함수를 재지정 
	//하고 마스크에 OR 연산하여 설정된 ENM_CHANGE 플래그를 지정하여 CRichEditCtrl().SetEventMask()를 호출하지 않으면
	// 이 알림 메시지를 보내지 않습니다.

	// TODO:  여기에 컨트롤 알림 처리기 코드를 추가합니다.
}
