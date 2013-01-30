#pragma once


#include "bass.h"

#define WIDTH 600	// display width
#define HEIGHT 201	// height (odd number for centre line)




// CBassLibControl 대화 상자입니다.

class CBassLibControl : public CDialog
{
	DECLARE_DYNAMIC(CBassLibControl)
public:




	// BASS Lib에서 사용하기 위한 특성입니다.
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
	static void CALLBACK LoopSyncProc(HSYNC handle, DWORD channel, DWORD data, void *user);					// lib에서 static을 요구한다.
	void SetLoopStart(QWORD pos);
	void SetLoopEnd(QWORD pos);
	static void __cdecl ScanPeaks(void *p);
	BOOL PlayFile();
	void DrawTimeLine(HDC dc, QWORD pos, DWORD col, DWORD y);


	// window procedure
	static long FAR PASCAL SpectrumWindowProc(HWND h, UINT m, WPARAM w, LPARAM l);

	int pseudoMain(HINSTANCE hInstance);

	// 끝.

	// 실험용 함수
	void tempTest(void);

	//


public:
	CBassLibControl(CWnd* pParent = NULL);   // 표준 생성자입니다.
	virtual ~CBassLibControl();

	virtual void OnFinalRelease();

// 대화 상자 데이터입니다.
	enum { IDD = IDD_BASSLIBCONTROL };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 지원입니다.

	DECLARE_MESSAGE_MAP()
	DECLARE_DISPATCH_MAP()
	DECLARE_INTERFACE_MAP()
};
