
// NoteEditingTool.h : NoteEditingTool ���� ���α׷��� ���� �� ��� ����
//
#pragma once

#ifndef __AFXWIN_H__
	#error "PCH�� ���� �� ������ �����ϱ� ���� 'stdafx.h'�� �����մϴ�."
#endif

#include "resource.h"       // �� ��ȣ�Դϴ�.


// CNoteEditingToolApp:
// �� Ŭ������ ������ ���ؼ��� NoteEditingTool.cpp�� �����Ͻʽÿ�.
//

class CNoteEditingToolApp : public CWinAppEx
{
protected:
	ULONG_PTR gdiplusToken;				// GDI+�� ���� ��ū

public:
	CNoteEditingToolApp();


// �������Դϴ�.
public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();

// �����Դϴ�.
	UINT  m_nAppLook;
	BOOL  m_bHiColorIcons;

	virtual void PreLoadState();
	virtual void LoadCustomState();
	virtual void SaveCustomState();

	afx_msg void OnAppAbout();
	DECLARE_MESSAGE_MAP()
};

extern CNoteEditingToolApp theApp;
