// CharismaDlg.cpp : ���� �����Դϴ�.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "CharismaDlg.h"
#include "afxdialogex.h"


// CCharismaDlg ��ȭ �����Դϴ�.

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


// CCharismaDlg �޽��� ó�����Դϴ�.


void CCharismaDlg::OnDeltaposSpin1(NMHDR *pNMHDR, LRESULT *pResult)
{
	m_ctrSpin.SetRange(1, 4);			// Spin Control�� �̻��ؼ� ���� ����.
	LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.



	*pResult = 0;
}


void CCharismaDlg::OnInitMenu(CMenu* pMenu)
{
	CDialogEx::OnInitMenu(pMenu);

	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰��մϴ�.


	m_ctrSpin.SetRange(1, 4);			// Spin Control�� �̻��ؼ� ���� ����.
}


void CCharismaDlg::OnBnClickedCharIdok()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	OnOK();
}


void CCharismaDlg::OnBnClickedCharIdcancel()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.'
	OnCancel();
}


void CCharismaDlg::OnEnChangeEditNoteType2()
{
	// TODO:  RICHEDIT ��Ʈ���� ���, �� ��Ʈ����
	// CDialogEx::OnInitDialog() �Լ��� ������ 
	//�ϰ� ����ũ�� OR �����Ͽ� ������ ENM_CHANGE �÷��׸� �����Ͽ� CRichEditCtrl().SetEventMask()�� ȣ������ ������
	// �� �˸� �޽����� ������ �ʽ��ϴ�.

	// TODO:  ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
}
