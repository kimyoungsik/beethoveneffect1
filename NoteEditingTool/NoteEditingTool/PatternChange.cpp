// PatternChange.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "PatternChange.h"
#include "afxdialogex.h"


// CPatternChange 대화 상자입니다.

IMPLEMENT_DYNAMIC(CPatternChange, CDialogEx)

CPatternChange::CPatternChange(CWnd* pParent /*=NULL*/)
	: CDialogEx(CPatternChange::IDD, pParent)
	, noteTypeString(_T("Pattern Change Note"))
	, notePatternNumber(0)
{

}

CPatternChange::~CPatternChange()
{
}

void CPatternChange::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT_NOTE_TYPE, noteTypeString);
	DDX_Text(pDX, IDC_EDIT_NOTE_TYPE2, notePatternNumber);
	DDV_MinMaxInt(pDX, notePatternNumber, 1, 4);
	DDX_Control(pDX, IDC_SPIN1, m_ctrSpin);
}


BEGIN_MESSAGE_MAP(CPatternChange, CDialogEx)
	ON_EN_CHANGE(IDC_EDIT_NOTE_TYPE, &CPatternChange::OnEnChangeEditNoteType)
	ON_EN_CHANGE(IDC_EDIT_NOTE_TYPE2, &CPatternChange::OnEnChangeEditNoteType2)
	ON_NOTIFY(UDN_DELTAPOS, IDC_SPIN1, &CPatternChange::OnDeltaposSpin1)
	ON_BN_CLICKED(B_CHAR_IDOK, &CPatternChange::OnBnClickedCharIdok)
	ON_BN_CLICKED(B_CHAR_IDCANCEL, &CPatternChange::OnBnClickedCharIdcancel)
END_MESSAGE_MAP()


// CPatternChange 메시지 처리기입니다.


void CPatternChange::OnEnChangeEditNoteType()
{
	// TODO:  RICHEDIT 컨트롤인 경우, 이 컨트롤은
	// CDialogEx::OnInitDialog() 함수를 재지정 
	//하고 마스크에 OR 연산하여 설정된 ENM_CHANGE 플래그를 지정하여 CRichEditCtrl().SetEventMask()를 호출하지 않으면
	// 이 알림 메시지를 보내지 않습니다.

	// TODO:  여기에 컨트롤 알림 처리기 코드를 추가합니다.
}


void CPatternChange::OnEnChangeEditNoteType2()
{
	// TODO:  RICHEDIT 컨트롤인 경우, 이 컨트롤은
	// CDialogEx::OnInitDialog() 함수를 재지정 
	//하고 마스크에 OR 연산하여 설정된 ENM_CHANGE 플래그를 지정하여 CRichEditCtrl().SetEventMask()를 호출하지 않으면
	// 이 알림 메시지를 보내지 않습니다.

	// TODO:  여기에 컨트롤 알림 처리기 코드를 추가합니다.
}


void CPatternChange::OnDeltaposSpin1(NMHDR *pNMHDR, LRESULT *pResult)
{
	m_ctrSpin.SetRange(1, 4);			// Spin Control이 이상해서 따로 설정.
	LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	*pResult = 0;

}


void CPatternChange::OnBnClickedCharIdok()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	OnOK();
}


void CPatternChange::OnBnClickedCharIdcancel()
{
	// TODO: 여기에 컨트롤 알림 처리기 코드를 추가합니다.
	OnCancel();
}
