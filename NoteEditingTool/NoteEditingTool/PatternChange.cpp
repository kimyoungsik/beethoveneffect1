// PatternChange.cpp : ���� �����Դϴ�.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "PatternChange.h"
#include "afxdialogex.h"


// CPatternChange ��ȭ �����Դϴ�.

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


// CPatternChange �޽��� ó�����Դϴ�.


void CPatternChange::OnEnChangeEditNoteType()
{
	// TODO:  RICHEDIT ��Ʈ���� ���, �� ��Ʈ����
	// CDialogEx::OnInitDialog() �Լ��� ������ 
	//�ϰ� ����ũ�� OR �����Ͽ� ������ ENM_CHANGE �÷��׸� �����Ͽ� CRichEditCtrl().SetEventMask()�� ȣ������ ������
	// �� �˸� �޽����� ������ �ʽ��ϴ�.

	// TODO:  ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
}


void CPatternChange::OnEnChangeEditNoteType2()
{
	// TODO:  RICHEDIT ��Ʈ���� ���, �� ��Ʈ����
	// CDialogEx::OnInitDialog() �Լ��� ������ 
	//�ϰ� ����ũ�� OR �����Ͽ� ������ ENM_CHANGE �÷��׸� �����Ͽ� CRichEditCtrl().SetEventMask()�� ȣ������ ������
	// �� �˸� �޽����� ������ �ʽ��ϴ�.

	// TODO:  ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
}


void CPatternChange::OnDeltaposSpin1(NMHDR *pNMHDR, LRESULT *pResult)
{
	m_ctrSpin.SetRange(1, 4);			// Spin Control�� �̻��ؼ� ���� ����.
	LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	*pResult = 0;

}


void CPatternChange::OnBnClickedCharIdok()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	OnOK();
}


void CPatternChange::OnBnClickedCharIdcancel()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	OnCancel();
}
