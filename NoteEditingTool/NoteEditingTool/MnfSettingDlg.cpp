// MnfSettingDlg.cpp : ���� �����Դϴ�.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "MnfSettingDlg.h"
#include "afxdialogex.h"


// CMnfSettingDlg ��ȭ �����Դϴ�.

IMPLEMENT_DYNAMIC(CMnfSettingDlg, CDialogEx)

CMnfSettingDlg::CMnfSettingDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CMnfSettingDlg::IDD, pParent)
	, mnfLevel(0)
	, mnfInitBPM(0)
	, startTimeMin(0)
	, endTimeMin(0)
	, startTimeSec(0)
	, endTimeSec(0)
	, startTimeMilSec(0)
	, endTimeMilSec(0)
{

}

CMnfSettingDlg::~CMnfSettingDlg()
{
}

void CMnfSettingDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT_TITLE, mnfTitle);
	DDX_Text(pDX, IDC_EDIT_ARTIST, mnfArtist);
	DDX_Text(pDX, IDC_EDIT_LEVEL, mnfLevel);
	DDV_MinMaxUInt(pDX, mnfLevel, 0, 8);
	DDV_MaxChars(pDX, mnfTitle, 100);
	DDV_MaxChars(pDX, mnfArtist, 100);
	DDX_Text(pDX, IDC_EDIT_INIT_BPM, mnfInitBPM);
	DDX_Text(pDX, IDC_EDIT_START_TIME_MIN, startTimeMin);
	DDV_MinMaxInt(pDX, startTimeMin, 0, 200);
	DDX_Text(pDX, IDC_EDIT_END_TIME_MIN, endTimeMin);
	DDV_MinMaxInt(pDX, endTimeMin, 0, 200);
	DDX_Text(pDX, IDC_EDIT_START_TIME_SEC, startTimeSec);
	DDV_MinMaxInt(pDX, startTimeSec, 0, 59);
	DDX_Text(pDX, IDC_EDIT_END_TIME_SEC, endTimeSec);
	DDV_MinMaxInt(pDX, endTimeSec, 0, 59);
	DDX_Text(pDX, IDC_EDIT_START_TIME_MIL_SEC, startTimeMilSec);
	DDV_MinMaxInt(pDX, startTimeMilSec, 0, 999);
	DDX_Text(pDX, IDC_EDIT_END_TIME_MIL_SEC, endTimeMilSec);
	DDV_MinMaxInt(pDX, endTimeMilSec, 0, 999);
}


BEGIN_MESSAGE_MAP(CMnfSettingDlg, CDialogEx)
	ON_EN_CHANGE(IDC_EDIT_TITLE, &CMnfSettingDlg::OnEnChangeEditTitle)
	ON_BN_CLICKED(BUTTON_ID_OK, &CMnfSettingDlg::OnBnClickedIdOk)
	ON_BN_CLICKED(BUTTON_ID_CANCEL, &CMnfSettingDlg::OnBnClickedIdCancel)
	ON_EN_CHANGE(IDC_EDIT_LEVEL, &CMnfSettingDlg::OnEnChangeEditLevel)
	ON_EN_CHANGE(IDC_EDIT_START_TIME_SEC, &CMnfSettingDlg::OnEnChangeEditStartTimeSec)
END_MESSAGE_MAP()


// CMnfSettingDlg �޽��� ó�����Դϴ�.


void CMnfSettingDlg::OnEnChangeEditTitle()
{
	// TODO:  RICHEDIT ��Ʈ���� ���, �� ��Ʈ����
	// CDialogEx::OnInitDialog() �Լ��� ������ 
	//�ϰ� ����ũ�� OR �����Ͽ� ������ ENM_CHANGE �÷��׸� �����Ͽ� CRichEditCtrl().SetEventMask()�� ȣ������ ������
	// �� �˸� �޽����� ������ �ʽ��ϴ�.

	// TODO:  ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
}


void CMnfSettingDlg::OnBnClickedIdOk()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	OnOK();
}


void CMnfSettingDlg::OnBnClickedIdCancel()
{
	// TODO: ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
	OnCancel();
}


void CMnfSettingDlg::OnEnChangeEditLevel()
{
	// TODO:  RICHEDIT ��Ʈ���� ���, �� ��Ʈ����
	// CDialogEx::OnInitDialog() �Լ��� ������ 
	//�ϰ� ����ũ�� OR �����Ͽ� ������ ENM_CHANGE �÷��׸� �����Ͽ� CRichEditCtrl().SetEventMask()�� ȣ������ ������
	// �� �˸� �޽����� ������ �ʽ��ϴ�.

	// TODO:  ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
}


void CMnfSettingDlg::OnEnChangeEditStartTimeSec()
{
	// TODO:  RICHEDIT ��Ʈ���� ���, �� ��Ʈ����
	// CDialogEx::OnInitDialog() �Լ��� ������ 
	//�ϰ� ����ũ�� OR �����Ͽ� ������ ENM_CHANGE �÷��׸� �����Ͽ� CRichEditCtrl().SetEventMask()�� ȣ������ ������
	// �� �˸� �޽����� ������ �ʽ��ϴ�.

	// TODO:  ���⿡ ��Ʈ�� �˸� ó���� �ڵ带 �߰��մϴ�.
}
