#pragma once


#include "bass.h"

#define WIDTH 600	// display width
#define HEIGHT 201	// height (odd number for centre line)




// CBassLibControl ��ȭ �����Դϴ�.

class CBassLibControl : public CDialog
{
	DECLARE_DYNAMIC(CBassLibControl)
public:




	// BASS Lib���� ����ϱ� ���� Ư���Դϴ�.
	HWND win;
	DWORD scanthread;
	BOOL killscan;

	DWORD chan;
	DWORD bpp;			// bytes per pixel
	static QWORD loop[2];		// loop start & end
	HSYNC lsync;		// looping sync

	HDC wavedc;
	HBITMAP wavebmp;
	BYTE *wavebuf;



	void Error(const TCHAR *es);
	static void CALLBACK LoopSyncProc(HSYNC handle, DWORD channel, DWORD data, void *user);					// lib���� static�� �䱸�Ѵ�.
	void SetLoopStart(QWORD pos);
	void SetLoopEnd(QWORD pos);
	static void __cdecl ScanPeaks(void *p);
	BOOL PlayFile();
	void DrawTimeLine(HDC dc, QWORD pos, DWORD col, DWORD y);


	// window procedure
	static long FAR PASCAL SpectrumWindowProc(HWND h, UINT m, WPARAM w, LPARAM l);

	int pseudoMain(HINSTANCE hInstance);

	// ��.

	// ����� �Լ�
	void tempTest(void);

	//


public:
	CBassLibControl(CWnd* pParent = NULL);   // ǥ�� �������Դϴ�.
	virtual ~CBassLibControl();

	virtual void OnFinalRelease();

// ��ȭ ���� �������Դϴ�.
	enum { IDD = IDD_BASSLIBCONTROL };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV �����Դϴ�.

	DECLARE_MESSAGE_MAP()
	DECLARE_DISPATCH_MAP()
	DECLARE_INTERFACE_MAP()
};
