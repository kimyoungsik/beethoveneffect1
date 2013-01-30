// BPMChangeDlg.cpp : ���� �����Դϴ�.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "BPMChangeDlg.h"
#include "afxdialogex.h"


// CBPMChangeDlg ��ȭ �����Դϴ�.

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


// CBPMChangeDlg �޽��� ó�����Դϴ�.


void CBPMChangeDlg::OnDeltaposSpin1(NMHDR *pNMHDR, LRESULT *pResult)
{
	m_ctrSpin.SetRange(1, 250);			// Spin Control�� �̻��ؼ� ���� ����.
	LPNMUPDOWN pNMUpDown = reinterpret_cast<LPNMUPDOWN>(pNMHDR);
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	*pResult = 0;
}


void CBPMChangeDlg::OnBnClickedCharIdok()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	OnOK();
}


void CBPMChangeDlg::OnBnClickedCharIdcancel()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	OnCancel();
}


void CBPMChangeDlg::OnEnChangeEditNoteType()
{
	// TODO:  RICHEDIT ��Ʈ���� ���, �� ��Ʈ����
	// CDialogEx::OnInitDialog() �Լ��� ������ 
	//�ϰ� ����ũ�� OR �����Ͽ� ������ ENM_CHANGE �÷��׸� �����Ͽ� CRichEditCtrl().SetEventMask()�� ȣ������ ������
	// �� �˸� �޽����� ������ �ʽ��ϴ�.

	// TODO:  ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
}
