// NotePickingView.cpp : 구현 파일입니다.
//



// BASS에서 사용하는 헤더들

#include <windows.h>
#include <iostream>
#include <process.h>
#include <string>
#include <time.h>
#include "tchar.h"


// BASS 끝


#include "stdafx.h"
#include "NoteEditingTool.h"
#include "NotePickingView.h"


#include "NoteEditingToolDoc.h"
#include "NoteEditingToolView.h"
#include "EditModeSelectView.h"

// 다이얼로그들
#include "MnfSettingDlg.h"
#include "CharismaDlg.h"
#include "PatternChange.h"
#include "BPMChangeDlg.h"

// CNotePickingView


IMPLEMENT_DYNCREATE(CNotePickingView, CScrollView)


// 완전 임시 전역변수.
//	QWORD tempLength = 0;




static CNotePickingView* useByLoopSyncProc;						// static 함수를 사용하기 위해 this포인터를 받아오기 위한 전역변수??
QWORD CNotePickingView::loop[2] = {0};

CNotePickingView::CNotePickingView()
{


	// 텍스트용 설정
	m_bTransparent = TRUE;
	m_colorText = RGB(240, 240, 240);
	m_logFont.lfHeight  = 20;
	m_logFont.lfWidth  = 0;
	m_logFont.lfEscapement = 0;
	m_logFont.lfOrientation = 0;
	m_logFont.lfWeight  = FW_NORMAL;
	m_logFont.lfItalic  = FALSE;
	m_logFont.lfUnderline = FALSE;
	m_logFont.lfStrikeOut = FALSE;
	m_logFont.lfCharSet  = DEFAULT_CHARSET;
	m_logFont.lfOutPrecision = OUT_CHARACTER_PRECIS;
	m_logFont.lfClipPrecision = CLIP_CHARACTER_PRECIS;
	m_logFont.lfQuality  = DEFAULT_QUALITY;
	m_logFont.lfPitchAndFamily = DEFAULT_PITCH|FF_DONTCARE;
	//strcpy(m_logFont.lfFaceName, _T("Arial Black"));
	_tcscpy_s(m_logFont.lfFaceName, _T("Arial Black"));
	//m_logFont.lfFaceName = _T("Arial Black");


	//// BASS Lib에서 사용하기 위한 특성입니다.
	//this->win = NULL;
	//this->scanthread = 0;
	//this->killscan = FALSE;

	//this->wavedc = 0;
	//this->wavebmp = 0;

	//useByLoopSyncProc = this;			// 자신의 포인터를 등록

	//this->bpp = 5000;					// 임시 초기값

	chan = 0;



	//// BASS 관련 플래그들
	//nowPlayingStatus = -1;
	bassInitFlag = FALSE;


	//WIDTH = 3000;			// display width
	//// 끝.


	// 더블버퍼링 관련
	bmpOfTimeLineMask = NULL;

	// Doc에서 사용할 포인터




	// 타이머를 위한 것
	timeGetDevCaps(&tc, sizeof(TIMECAPS));
	m_uResolution = min(max(tc.wPeriodMin, 0), tc.wPeriodMax);
	timeBeginPeriod(m_uResolution);
	m_idEvent = NULL;

	mutexFlag = true;


	// 타이머에서 사용 할 변수 초기화
	/*onTimerMousePoint.x = 0;
	onTimerMousePoint.y = 0;
	chkMouseHandleState = TIMER_1_FLAG_NULL;*/
	m_timerRedrawFlag = false;
}

CNotePickingView::~CNotePickingView()
{
	DWORD channelActiveStatus;
	channelActiveStatus = BASS_ChannelIsActive(chan);


	// 타이머 종료
	if ( m_idEvent )
	{
		//KillTimer(TIMER_1_ID);
		// 계속해서 여기에서 에러가 나서 지움. (아마도 메인프레임이 먼저 없어지기 때문에 문제인 듯)
		// destroy the timer
		timeKillEvent(m_idEvent);
		// reset the timer
		timeEndPeriod (m_uResolution);

	}




	if ( nowPlayingStatus == 1 )
	{
		// 재생중이라면, 타이머를 죽인다.
		BASS_ChannelStop(chan);
	}

	if (scanthread)
	{
		// still scanning
		killscan = TRUE;
		WaitForSingleObject((HANDLE)(scanthread), 1000); // wait for the thread
	}

	// Free 해 준다.
	BASS_Free();

	// 사용했던 그림을 삭제
	if (wavedc)
	{
		DeleteDC(wavedc);
	}
	if (wavebmp)
	{
		DeleteObject(wavebmp);
	}
}


UINT CM_SCAN_END = RegisterWindowMessage(_T("CM_SCAN_END"));
UINT CM_REDRAW_DC = RegisterWindowMessage(_T("CM_REDRAW_DC"));
UINT CM_REDRAW_DRAG_VIEW2 = RegisterWindowMessage(_T("CM_REDRAW_DRAG_VIEW"));
UINT CM_VK_SPACE = RegisterWindowMessage(_T("CM_VK_SPACE"));
UINT CM_REDRAW_VIEW2 = RegisterWindowMessage(_T("CM_REDRAW_VIEW"));
UINT CM_EDITING_MODE_CHANGE2 = RegisterWindowMessage(_T("CM_EDITING_MODE_CHANGE"));

BEGIN_MESSAGE_MAP(CNotePickingView, CScrollView)
	ON_WM_MOUSEMOVE()
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	//ON_MESSAGE(CM_REDRAW_DRAG_VIEW, &CNotePickingView::OnCmRedrawDragView)
	ON_WM_KEYDOWN()
	ON_WM_ERASEBKGND()
	ON_COMMAND(ID_OPEN_NEW_MUSIC_FILE, &CNotePickingView::OnOpenNewMusicFile)
	ON_COMMAND(ID_MNF_SETTING, &CNotePickingView::OnMnfSetting)
	ON_WM_RBUTTONDOWN()
	ON_REGISTERED_MESSAGE(CM_SCAN_END, &CNotePickingView::OnCmScanEnd)
	ON_WM_TIMER()
	ON_REGISTERED_MESSAGE(CM_REDRAW_DC, &CNotePickingView::OnCmRedrawDc)
	ON_REGISTERED_MESSAGE(CM_VK_SPACE, &CNotePickingView::OnCmVkSpace)
END_MESSAGE_MAP()







//////////////////////////////////////////////////////////////////////////////
//////////////////////////////   [BASS START]   //////////////////////////////
//////////////////////////////////////////////////////////////////////////////







void CNotePickingView::Error(const TCHAR *es)
{
	TCHAR tempBuff[100];
	_stprintf_s(tempBuff, _T("%s\n(error code: %d)\n"),es,BASS_ErrorGetCode());
	
	MessageBox(tempBuff);
}

void CALLBACK CNotePickingView::LoopSyncProc(HSYNC handle, DWORD channel, DWORD data, void *user)
{
	// Static함수 이므로, 이 함수 안에서 Member Function을 사용하려면,
	// useByLoopSyncProc->함수명() 과 같은 형태로 사용해야만 한다.

	// if ( !BASS_ChannelSetPosition(channel, loop[0], BASS_POS_BYTE) ) // try seeking to loop start
	CNotePickingView *tempThisPointer = useByLoopSyncProc;

	if (  !BASS_ChannelSetPosition(channel, tempThisPointer->loop[0], BASS_POS_BYTE)   ) // try seeking to loop start
	{
		BASS_ChannelSetPosition(channel,0,BASS_POS_BYTE); // failed, go to start of file instead
	}

}


void CNotePickingView::SetLoopStart(QWORD pos)
{
	loop[0]=pos;
}

void CNotePickingView::SetLoopEnd(QWORD pos)
{
	loop[1]=pos;
	BASS_ChannelRemoveSync(chan,lsync); // remove old sync
	lsync = BASS_ChannelSetSync(chan,BASS_SYNC_POS|BASS_SYNC_MIXTIME,loop[1],LoopSyncProc,0); // set new sync
}


void __cdecl CNotePickingView::ScanPeaks(void *p)
{
	// 스레드를 하나 생성해서 실시간으로 음악의 파형을 분석하기 위해 만들어진 함수이다.
	// p는 스캔용으로 만들어진 Channel 2라는 QWORD가 들어온다.


	CNotePickingView *tempThisPointer = useByLoopSyncProc;
	// Static 함수에 대해서 강제적으로 얻은 Pointer.


	DWORD decoder = (DWORD)p;
	DWORD cpos=0, peak[2]={0};


	int tempInt = 0;					// 임시로 저장하는 함수
	int maxIndex = (int)(HEIGHT * useByLoopSyncProc->WIDTH * BSS_WAVE_FORM_RATE_REAL);


	while ( ! useByLoopSyncProc->killscan )
	{
		// 스캔을 그만 하라는 명령이 내려 올 때까지 스캔.
		
		DWORD level=BASS_ChannelGetLevel(decoder); // scan peaks
		DWORD pos;
		if (peak[0]<LOWORD(level))
		{
			peak[0]=LOWORD(level); // set left peak
		}
		if (peak[1]<HIWORD(level))
		{
			peak[1]=HIWORD(level); // set right peak
		}
		
		if (!BASS_ChannelIsActive(decoder))
		{
			pos = -1; // reached the end
		}
		else
		{
			pos = (DWORD)( BASS_ChannelGetPosition(decoder,BASS_POS_BYTE) / (useByLoopSyncProc->bpp));
			//pos = (DWORD)( BASS_ChannelGetPosition(decoder,BASS_POS_BYTE) / 40000 );
		}

		if (pos>cpos)
		{
			DWORD a;
			
			for ( a=0 ; a<peak[0]*(HEIGHT/2)/BSS_UNKNOWN_WAVE_WIDTH ; a++ )
			{
				// 가끔 Peak값에서 OverFlow가 나기 때문에 이와 같이 이중으로 일을 하게 만들었다
				//tempInt = (HEIGHT/2-1-a)*(tempLength)+cpos;
				//(useByLoopSyncProc->wavebuf)[tempInt] = (BYTE)(1+a); // draw left peak
				tempInt = (int)((HEIGHT/2-1-a)*(useByLoopSyncProc->WIDTH * BSS_WAVE_FORM_RATE_REAL)+cpos);
				if( tempInt <= maxIndex )
				{
					(useByLoopSyncProc->wavebuf)[tempInt] = (BYTE)(1+a); // draw left peak
				}
			}
			
			for ( a=0 ; a<peak[1]*(HEIGHT/2)/BSS_UNKNOWN_WAVE_WIDTH ; a++ )
			{
				// 가끔 Peak값에서 OverFlow가 나기 때문에 이와 같이 이중으로 일을 하게 만들었다
				tempInt = (int)((HEIGHT/2+1+a)*(useByLoopSyncProc->WIDTH * BSS_WAVE_FORM_RATE_REAL)+cpos);
				if( tempInt <= maxIndex )
				{
					(useByLoopSyncProc->wavebuf)[tempInt] = (BYTE)(1+a); // draw right peak
				}
			}
			
			if ( pos >= (DWORD)((useByLoopSyncProc->WIDTH * BSS_WAVE_FORM_RATE_REAL)) )
			{
				break; // gone off end of display
			}
			cpos=pos;
			peak[0]=peak[1]=0;
		}
	}


	BASS_StreamFree(decoder); // free the decoder
	
 	useByLoopSyncProc->PostMessageW(CM_SCAN_END, 0, 0);
	//PostMessageW(CM_REDRAW_DRAG_VIEW2, 0, 0);
	useByLoopSyncProc->scanthread = 0;

	return;
}



// select a file to play, and start scanning it
BOOL CNotePickingView::PlayFile()
{
	TCHAR file[MAX_PATH] = _T("");
	OPENFILENAME ofn={0};
	ofn.lStructSize=sizeof(ofn);
	ofn.hwndOwner=win;
	ofn.nMaxFile=MAX_PATH;
	ofn.lpstrFile = file;
	ofn.Flags=OFN_FILEMUSTEXIST|OFN_HIDEREADONLY|OFN_EXPLORER;
	ofn.lpstrTitle = _T("Select a file to play");
	ofn.lpstrFilter= _T("Playable files\0*.mp3;*.mp2;*.mp1;*.ogg;*.wav;*.aif;*.mo3;*.it;*.xm;*.s3m;*.mtm;*.mod;*.umx\0All files\0*.*\0\0");
	if (!GetOpenFileName(&ofn)) return FALSE;

	if (!(chan=BASS_StreamCreateFile(FALSE,file,0,0,0)) )
	{
		Error(_T("Can't play file"));
		if( !(chan=BASS_MusicLoad(FALSE,file,0,0,BASS_MUSIC_RAMPS|BASS_MUSIC_POSRESET|BASS_MUSIC_PRESCAN,1))) 
		{
			Error(_T("Can't play file"));
			return FALSE; // Can't load the file
		}
	}
	{
		BYTE data[2000]={0};
		BITMAPINFOHEADER *bh=(BITMAPINFOHEADER*)data;
		RGBQUAD *pal=(RGBQUAD*)(data+sizeof(*bh));
		int a;
		bh->biSize=sizeof(*bh);
		bh->biWidth=WIDTH;
		bh->biHeight=-HEIGHT;
		bh->biPlanes=1;
		bh->biBitCount=8;
		bh->biClrUsed=bh->biClrImportant=HEIGHT/2+1;
		// setup palette
		for (a=1;a<=HEIGHT/2;a++) {
			pal[a].rgbRed=(255*a)/(HEIGHT/2);
			pal[a].rgbGreen=255-pal[a].rgbRed;
		}
		// create the bitmap
		wavebmp=CreateDIBSection(0,(BITMAPINFO*)bh,DIB_RGB_COLORS,(void**)&wavebuf,NULL,0);
		wavedc=CreateCompatibleDC(0);
		SelectObject(wavedc,wavebmp);
	}
	bpp = (DWORD)BASS_ChannelGetLength(chan,BASS_POS_BYTE)/WIDTH; // bytes per pixel
	if (bpp<BASS_ChannelSeconds2Bytes(chan,0.02)) // minimum 20ms per pixel (BASS_ChannelGetLevel scans 20ms)
		bpp = (DWORD)(BASS_ChannelSeconds2Bytes(chan,0.02));
	BASS_ChannelSetSync(chan,BASS_SYNC_END|BASS_SYNC_MIXTIME,0,LoopSyncProc,0); // set sync to loop at end
	BASS_ChannelPlay(chan,FALSE); // start playing
	{ // start scanning peaks in a new thread
		DWORD chan2=BASS_StreamCreateFile(FALSE,file,0,0,BASS_STREAM_DECODE);
		if (!chan2) chan2=BASS_MusicLoad(FALSE,file,0,0,BASS_MUSIC_DECODE,1);
		scanthread=_beginthread(ScanPeaks,0,(void*)chan2);
	}
	return TRUE;
}


void CNotePickingView::DrawTimeLine(HDC dc, QWORD pos, DWORD col, DWORD y)
{
	HPEN pen = CreatePen(PS_SOLID,0,col),oldpen;
	DWORD wpos = (DWORD)(pos/bpp);
	DWORD time = (DWORD)(BASS_ChannelBytes2Seconds(chan,pos));
	//char text[10];
	//TCHAR timeText[10];
	TCHAR timeText[30];									// 초기화 필요.

	_stprintf_s(timeText, _T("%u:%02u"),time/60,time%60);
	//sprintf(text,"%u:%02u",time/60,time%60);
	oldpen = (HPEN)SelectObject(dc,pen);
	MoveToEx(dc,wpos + V_NOTE_SET_OFF_X,0,NULL);
	LineTo(dc,wpos + V_NOTE_SET_OFF_X,HEIGHT);
	SetTextColor(dc,col);
	SetBkMode(dc,TRANSPARENT);
	SetTextAlign(dc,wpos>=WIDTH/2?TA_RIGHT:TA_LEFT);
	::TextOut( dc, wpos + V_NOTE_SET_OFF_X, y, timeText, _tcslen(timeText) );
	SelectObject(dc,oldpen);
	DeleteObject(pen);

}


void CNotePickingView::DrawTimeLineEx(HDC dc, QWORD pos, DWORD col, DWORD y, CPoint scrollPos)
{
	HPEN pen = CreatePen(PS_SOLID,0,col),oldpen;
	DWORD wpos = (DWORD)(pos/bpp);
	DWORD time = (DWORD)(BASS_ChannelBytes2Seconds(chan,pos));


	//double temp = (BASS_ChannelBytes2Seconds(chan,bpp));
	//char text[10];
	TCHAR timeText[30];									// 초기화 필요.


	_stprintf_s(timeText, _T("%u:%02u"),time/60,time%60);
	//sprintf(text,"%u:%02u",time/60,time%60);
	
	// 실질적으로 선을 그리는 부분
	oldpen = (HPEN)SelectObject(dc,pen);
	MoveToEx(dc, wpos + V_NOTE_SET_OFF_X - scrollPos.x, 0, NULL);
	LineTo(dc, wpos + V_NOTE_SET_OFF_X - scrollPos.x, HEIGHT);

	// 실질적으로 시간 숫자를 그리는 부분.
	SetTextColor(dc,col);
	SetBkMode(dc,TRANSPARENT);
	SetTextAlign( dc, wpos>=WIDTH/2 ? TA_RIGHT : TA_LEFT );
	::TextOut( dc, wpos + V_NOTE_SET_OFF_X - scrollPos.x, y, timeText, _tcslen(timeText) );
	
	// 쓰고 난 펜은 지운다.
	SelectObject(dc,oldpen);
	DeleteObject(pen);

}

//// window procedure
//long FAR PASCAL CNotePickingView::SpectrumWindowProc(HWND h, UINT m, WPARAM w, LPARAM l)
//{
//	CNotePickingView *tempThisPointer = useByLoopSyncProc;
//	// Static 함수에 대해서 강제적으로 얻은 Pointer.
//
//
//
//	switch (m) {
//	case WM_LBUTTONDOWN: // set loop start
//		tempThisPointer->SetLoopStart(LOWORD(l)* (tempThisPointer->bpp));
//		return 0;
//	case WM_RBUTTONDOWN: // set loop end
//		tempThisPointer->SetLoopEnd(LOWORD(l)* (tempThisPointer->bpp));
//		return 0;
//	case WM_MOUSEMOVE:
//		if (w&MK_LBUTTON) tempThisPointer->SetLoopStart(LOWORD(l)* (tempThisPointer->bpp));
//		if (w&MK_RBUTTON) tempThisPointer->SetLoopEnd(LOWORD(l)* (tempThisPointer->bpp));
//		return 0;
//
//	case WM_TIMER:
//		::InvalidateRect(h,0,0); // refresh window
//		return 0;
//	case WM_PAINT:
//		if (::GetUpdateRect(h,0,0)) {
//			PAINTSTRUCT p;
//			HDC dc;
//			if (!(dc = ::BeginPaint(h,&p)))
//			{
//				return 0;
//			}
//			BitBlt(dc, 0, 0, (useByLoopSyncProc->WIDTH), HEIGHT, tempThisPointer->wavedc, 0, 0, SRCCOPY); // draw peak waveform
//			tempThisPointer->DrawTimeLine(dc,loop[0],0xffff00,12); // loop start
//			tempThisPointer->DrawTimeLine(dc,loop[1],0x00ffff,24); // loop end
//			tempThisPointer->DrawTimeLine(dc,BASS_ChannelGetPosition( tempThisPointer->chan, BASS_POS_BYTE ), 0xffffff,0); // current pos
//			::EndPaint(h, &p);
//		}
//		return 0;
//
//	case WM_CREATE:
//		tempThisPointer->win = h;
//		// initialize output
//		if (!BASS_Init(-1,44100,0,tempThisPointer->win,NULL)) {
//			tempThisPointer->Error(_T("Can't initialize device"));
//			return -1;
//		}
//		if (!(tempThisPointer->PlayFile())) { // start a file playing
//			BASS_Free();
//			return -1;
//		}
//		::SetTimer(h,0,100,0); // set update timer (10hz)
//		break;
//
//	case WM_DESTROY:
//		::KillTimer(h,0);
//		if (tempThisPointer->scanthread) { // still scanning
//			tempThisPointer->killscan = TRUE;
//			WaitForSingleObject((HANDLE)(tempThisPointer->scanthread),1000); // wait for the thread
//		}
//		BASS_Free();
//		if (tempThisPointer->wavedc)
//		{
//			DeleteDC(tempThisPointer->wavedc);
//		}
//		if (tempThisPointer->wavebmp)
//		{
//			DeleteObject(tempThisPointer->wavebmp);
//		}
//		PostQuitMessage(0);
//		break;
//	}
//
//	return ::DefWindowProc(h, m, w, l);
//}



int CNotePickingView::pseudoMain(HINSTANCE hInstance)
{
	WNDCLASS wc;
	MSG msg;

	// check the correct BASS was loaded
	if (HIWORD(BASS_GetVersion())!=BASSVERSION) {
		::MessageBox(0, _T("An incorrect version of BASS.DLL was loaded"),0,MB_ICONERROR);
		return -3;
	}

	// register window class and create the window
	memset(&wc,0,sizeof(wc));
	//wc.lpfnWndProc = SpectrumWindowProc;
	wc.hInstance = AfxGetInstanceHandle();//hInstance;
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.lpszClassName = _T("BASS-CustLoop");

	if (!RegisterClass(&wc))
	{
		Error(_T("Can't create window"));
		return -2;
	}

	if( !CreateWindowW(_T("BASS-CustLoop"),
		_T("BASS custom looping example (left-click to set loop start, right-click to set end)"),
		WS_POPUPWINDOW|WS_CAPTION|WS_VISIBLE, 100, 100,
		WIDTH+2*GetSystemMetrics(SM_CXDLGFRAME),
		HEIGHT+GetSystemMetrics(SM_CYCAPTION)+2*GetSystemMetrics(SM_CYDLGFRAME),
		NULL, NULL, hInstance, NULL))
	{
			Error(_T("Can't create window"));
			return -2;
	}
	::ShowWindow(win, SW_SHOWNORMAL);

	while (GetMessage(&msg,NULL,0,0)>0) {
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}


	return 0;
}




// BASS Lib 초기화 함수
int CNotePickingView::BInit(void)
{
	if ( bassInitFlag == false )
	{
		// BASS 라이브러리 초기화
		if ( !BASS_Init(-1,44100,0,win,NULL) )
		{
			AfxMessageBox(_T("BASS 라이브러리 초기화에 실패했습니다. \n사운드 카드 상태를 확인하거나, 스피커를 연결 해 주세요. \n다시 초기화하기 위해, 프로그램을 재시작 해 주세요."));
			//::MessageBox(0, _T("Bass 라이브러리 초기화 실패"),_T("에러"),16);
			// 재 초기화를 위해, 라이브러리 프리?
			//BASS_Free();
			
			return -1;
		}
		bassInitFlag = true;
	}

	nowPlayingStatus = 3;

	return 1;	
}

// mp3파일 불러오는 함수 (mnf파일정보 갱신)
int CNotePickingView::BopenMusicFile(void)
{	
	// 혹시 현재 재생중인 파일이 있는지 확인
	if( chan )
	{
		// 상태 확인 후
		if (BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
		{
			// 재생중이면, 정지
			tempStop();
		}
		
		// 스트림을 놓아주지 않는다.
		// 아직 파일을 새로 여는 것을 확정하지 않았기 때문.
	}




	// 열기 시작
	CNoteEditingToolDoc* pDoc = GetDocument();

	WCHAR file[MAX_PATH] = _T("");
	// TCHAR로 정의 할 경우 자동으로 WCHAR로 변환 해 주지만, BASS_StreamCreateFile의 BASS_UNICODE Flag를 세워줬기 때문에 명시적으로 정의한다.

	OPENFILENAME ofn={0};
	ofn.lStructSize=sizeof(ofn);
	ofn.hwndOwner=win;
	ofn.nMaxFile = MAX_PATH;
	ofn.lpstrFile = file;
	//swprintf(ofn.lpstrFile, _T("%s"), file);
	ofn.Flags=OFN_FILEMUSTEXIST|OFN_HIDEREADONLY|OFN_EXPLORER;
	ofn.lpstrTitle = _T("Select a file to play");
	ofn.lpstrFilter= _T("Playable files\0*.mp3;*.mp2;*.mp1;*.ogg;*.wav;*.aif;*.mo3;*.it;*.xm;*.s3m;*.mtm;*.mod;*.umx\0All files\0*.*\0\0");
	if (!GetOpenFileName(&ofn))
	{
		return -1;
	}


	// 스트림을 놓아준다.
	BASS_StreamFree(chan);
	if (!(chan=BASS_StreamCreateFile(FALSE,file,0,0,BASS_UNICODE)) )
	{
		// 파일을 오픈 할 때, 유니코드로 읽도록 플래그를 설정 해 줘야 한다.
		Error(_T("Can't play file"));
		if( !(chan=BASS_MusicLoad(FALSE,file,0,0,BASS_MUSIC_RAMPS|BASS_MUSIC_POSRESET|BASS_MUSIC_PRESCAN,1))) 
		{
			Error(_T("Can't play file"));
			nowPlayingStatus = 3;				// 음악이 로드되지 않은 상태
			return -2; // Can't load the file
		}
	}

	{
		//tempLength = (DWORD)BASS_ChannelGetLength(chan, BASS_POS_BYTE) / 20000;
 		WIDTH  = (DWORD)( BASS_ChannelGetLength(chan, BASS_POS_BYTE) / BSS_WAVE_FORM_RATE * BSS_WAVE_FORM_RATE_REAL );
		//WIDTH = 300;

		BYTE data[2000]={0};
		BITMAPINFOHEADER *bh=(BITMAPINFOHEADER*)data;
		RGBQUAD *pal=(RGBQUAD*)(data+sizeof(*bh));
		int a;
		bh->biSize=sizeof(*bh);
		bh->biWidth= (LONG)(WIDTH * BSS_WAVE_FORM_RATE_REAL);
		bh->biHeight=-HEIGHT;
		bh->biPlanes=1;
		bh->biBitCount=8;
		bh->biClrUsed = bh->biClrImportant = HEIGHT/2+1;
		// setup palette
		for (a=1;a<=HEIGHT/2;a++)
		{
			pal[a].rgbRed=(255*a)/(HEIGHT/2);
			pal[a].rgbGreen=255-pal[a].rgbRed;
		}
		// create the bitmap
		wavebmp=CreateDIBSection( 0,(BITMAPINFO*)bh,DIB_RGB_COLORS, (void**)&wavebuf, NULL, 0 );
		wavedc=CreateCompatibleDC(0);
		SelectObject(wavedc,wavebmp);
	}
	 

	bpp = (DWORD)( BASS_ChannelGetLength(chan,BASS_POS_BYTE) / WIDTH / BSS_WAVE_FORM_RATE_REAL  ); // bytes per pixel
	if (bpp<BASS_ChannelSeconds2Bytes(chan,PIXEL_TO_BASS_MILTIME)) // minimum 20ms per pixel (BASS_ChannelGetLevel scans 20ms)
		bpp = (DWORD)(BASS_ChannelSeconds2Bytes(chan,PIXEL_TO_BASS_MILTIME));
	//BASS_ChannelSetSync(chan,BASS_SYNC_END|BASS_SYNC_MIXTIME,0,LoopSyncProc,0); // set sync to loop at end
	//BASS_ChannelPlay(chan,FALSE); // start playing

	{ // start scanning peaks in a new thread
		DWORD chan2 = BASS_StreamCreateFile(FALSE, file, 0, 0, BASS_STREAM_DECODE | BASS_UNICODE);
		// 파일을 오픈 할 때, 유니코드로 읽도록 플래그를 설정 해 줘야 한다.
		if (!chan2)
		{
			chan2=BASS_MusicLoad( FALSE, file, 0, 0, BASS_MUSIC_DECODE | BASS_UNICODE, 1 );
		}

		// 스레드 발싸
		scanthread = _beginthread(ScanPeaks, 0, (void*)chan2);
	}

	nowPlayingStatus = 0;
	//	BASS_ChannelPlay(chan, FALSE); // start playing

	// 여기까지 왔으면, 파일 이름을 읽어와 저장한다.
	saveMp3FileName(file);


	// 끝나는 시간 기본을 음악의 종료시간으로 설정
	QWORD tempLenForSecond = BASS_ChannelGetLength(chan, BASS_POS_BYTE); // the length in bytes
	double tempTimeInSecond = BASS_ChannelBytes2Seconds(chan, tempLenForSecond); // the length in seconds

	NoteTime EndMusicTime;
	EndMusicTime.noteTimeSec = (unsigned int)(tempTimeInSecond);
	EndMusicTime.noteTimeMilSec = (unsigned int)( (tempTimeInSecond - EndMusicTime.noteTimeSec) * 1000 );

	pDoc->setEndSongTime(EndMusicTime);
	EndMusicTime.noteTimeSec = 0;
	EndMusicTime.noteTimeMilSec = 0;
	pDoc->setStartSongTime(EndMusicTime);


	// 스크롤 창 크기 조정
	CSize sizeTotal;
	// TODO: 이 뷰의 전체 크기를 계산합니다.
	sizeTotal.cx = (LONG)( (WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X );
	sizeTotal.cy = FRAME_1_HEIGHT;
	SetScrollSizes(MM_TEXT, sizeTotal);


	// 화면의 새로고침
	DrawDoubleBuffering();

	// 창모드 화면의 새로고침
	if ( pDoc->getEditModeSelectViewPtr() != NULL )
	{
		pDoc->getEditModeSelectViewPtr()->SendMessage(CM_REDRAW_VIEW2);
	}

	return 1;
}

// MNF파일에 명시되어 있는 mp3파일 불러오는 함수
int CNotePickingView::BLoadMusicFileAuto(void)
{
	CNoteEditingToolDoc* pDoc = GetDocument();

	WCHAR file[MAX_PATH] = _T("");
	//WCHAR tempFileName2[MAX_PATH] = _T("");
	//WCHAR *tempFileName2 = _T("");
	CString tempFileName1;
	// TCHAR로 정의 할 경우 자동으로 WCHAR로 변환 해 주지만, BASS_StreamCreateFile의 BASS_UNICODE Flag를 세워줬기 때문에 명시적으로 정의한다.

	//OPENFILENAME ofn={0};
	//ofn.lStructSize=sizeof(ofn);
	//ofn.hwndOwner=win;
	//ofn.nMaxFile = MAX_PATH;
	//ofn.lpstrFile = file;
	////swprintf(ofn.lpstrFile, _T("%s"), file);
	//ofn.Flags=OFN_FILEMUSTEXIST|OFN_HIDEREADONLY|OFN_EXPLORER;
	//ofn.lpstrTitle = _T("Select a file to play");
	//ofn.lpstrFilter= _T("Playable files\0*.mp3;*.mp2;*.mp1;*.ogg;*.wav;*.aif;*.mo3;*.it;*.xm;*.s3m;*.mtm;*.mod;*.umx\0All files\0*.*\0\0");
	//if (!GetOpenFileName(&ofn))
	//{
	//	return -1;
	//}

	//if ( pDoc->getIsNewFileFlag() == true )
	if( pDoc->getMp3FileName() == "" )
	{
		// 해당하는 파일 이름이 없을 경우.
		// 현재 로드 된 채널을 삭제한다.




	}
	else
	{
		// 어떤 파일을 로드하는 경우, 그 파일의 음악을 불러온다.
		if( chan )
		{
			// 상태 확인 후
			if (BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
			{
				// 재생중이면, 정지
				tempStop();
			}

			// 스트림을 놓아주지 않는다.
			// 아직 파일을 새로 여는 것을 확정하지 않았기 때문.
		}



		tempFileName1 = pDoc->getMnfFilePath();			// CString to wchar_t
		//file = tempFileName.GetBuffer(0);
		_CrtSetDebugFillThreshold(0);					// wcscpy를 썼을 때, file의 남은 영역을 NULL로 채우지 않는 문제 해결용.
		wcscpy_s(file, tempFileName1.GetBuffer(0));
		// std::string에서 WCHAR로 바꾸는 방법.
		CA2W tempFileName2( pDoc->getMp3FileName().c_str() );

		// 만약 파일 이름을 아직 설정하지 않은 경우, 바로 종료한다.
		if ( tempFileName2 == _T("") )
		{
			AfxMessageBox(_T("현재 설정 된 음악 파일이 없습니다."));
			return 0;
		}

		// 파일 이름 문자열 추가.
		wcscat_s(file, tempFileName2);
		//tempFileName2 = pDoc->getMp3FileName().c_str();


		// 스트림을 놓아준다.
		BASS_StreamFree(chan);
		if (!(chan=BASS_StreamCreateFile(FALSE,file,0,0,BASS_UNICODE)) )
		{
			// 파일을 오픈 할 때, 유니코드로 읽도록 플래그를 설정 해 줘야 한다.
			Error(_T("mp3 파일이 존재하지 않습니다."));
			if( !(chan=BASS_MusicLoad(FALSE,file,0,0,BASS_MUSIC_RAMPS|BASS_MUSIC_POSRESET|BASS_MUSIC_PRESCAN,1))) 
			{
				Error(_T("mp3 파일이 존재하지 않습니다."));
				nowPlayingStatus = 3;				// 음악이 로드되지 않은 상태
				return -2; // Can't load the file
			}
		}

		{
			//tempLength = (DWORD)BASS_ChannelGetLength(chan, BASS_POS_BYTE) / 20000;
			WIDTH  = (DWORD)( BASS_ChannelGetLength(chan, BASS_POS_BYTE) / BSS_WAVE_FORM_RATE * BSS_WAVE_FORM_RATE_REAL );
			//WIDTH = 300;

			BYTE data[2000]={0};
			BITMAPINFOHEADER *bh=(BITMAPINFOHEADER*)data;
			RGBQUAD *pal=(RGBQUAD*)(data+sizeof(*bh));
			int a;
			bh->biSize=sizeof(*bh);
			bh->biWidth= (LONG)(WIDTH * BSS_WAVE_FORM_RATE_REAL);
			bh->biHeight=-HEIGHT;
			bh->biPlanes=1;
			bh->biBitCount=8;
			bh->biClrUsed = bh->biClrImportant = HEIGHT/2+1;
			// setup palette
			for (a=1;a<=HEIGHT/2;a++)
			{
				pal[a].rgbRed=(255*a)/(HEIGHT/2);
				pal[a].rgbGreen=255-pal[a].rgbRed;
			}
			// create the bitmap
			wavebmp=CreateDIBSection( 0,(BITMAPINFO*)bh,DIB_RGB_COLORS, (void**)&wavebuf, NULL, 0 );
			wavedc=CreateCompatibleDC(0);
			SelectObject(wavedc,wavebmp);
		}


		bpp = (DWORD)( BASS_ChannelGetLength(chan,BASS_POS_BYTE) / WIDTH / BSS_WAVE_FORM_RATE_REAL  ); // bytes per pixel
		if (bpp<BASS_ChannelSeconds2Bytes(chan,PIXEL_TO_BASS_MILTIME)) // minimum 20ms per pixel (BASS_ChannelGetLevel scans 20ms)
			bpp = (DWORD)(BASS_ChannelSeconds2Bytes(chan,PIXEL_TO_BASS_MILTIME));
		//BASS_ChannelSetSync(chan,BASS_SYNC_END|BASS_SYNC_MIXTIME,0,LoopSyncProc,0); // set sync to loop at end
		//BASS_ChannelPlay(chan,FALSE); // start playing

		// 새로운 스레드를 위한 namespace로 보인다. (chan2의 변위 설정)
		{ // start scanning peaks in a new thread
			DWORD chan2 = BASS_StreamCreateFile(FALSE, file, 0, 0, BASS_STREAM_DECODE | BASS_UNICODE);
			// 파일을 오픈 할 때, 유니코드로 읽도록 플래그를 설정 해 줘야 한다.
			if (!chan2)
			{
				chan2=BASS_MusicLoad( FALSE, file, 0, 0, BASS_MUSIC_DECODE | BASS_UNICODE, 1 );
			}

			// 스레드 발싸
			scanthread = _beginthread(ScanPeaks, 0, (void*)chan2);
		}

		nowPlayingStatus = 0;
		//	BASS_ChannelPlay(chan, FALSE); // start playing

		// 여기까지 왔으면, 파일 이름을 읽어와 저장한다.
		//saveMp3FileName(file);


		// 끝나는 시간 기본을 음악의 종료시간으로 설정
		//QWORD tempLenForSecond = BASS_ChannelGetLength(chan, BASS_POS_BYTE); // the length in bytes
		//double tempTimeInSecond = BASS_ChannelBytes2Seconds(chan, tempLenForSecond); // the length in seconds

		//NoteTime EndMusicTime;
		//EndMusicTime.noteTimeSec = (unsigned int)(tempTimeInSecond);
		//EndMusicTime.noteTimeMilSec = (unsigned int)( (tempTimeInSecond - EndMusicTime.noteTimeSec) * 1000 );

		//pDoc->setEndSongTime(EndMusicTime);
		//EndMusicTime.noteTimeSec = 0;
		//EndMusicTime.noteTimeMilSec = 0;
		//pDoc->setStartSongTime(EndMusicTime);


		// 스크롤 창 크기 조정
		CSize sizeTotal;
		// TODO: 이 뷰의 전체 크기를 계산합니다.
		sizeTotal.cx = (LONG)( (WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X );
		sizeTotal.cy = FRAME_1_HEIGHT;
		SetScrollSizes(MM_TEXT, sizeTotal);
	}


	// 화면의 새로고침
	DrawDoubleBuffering();


	return 1;

}


// 문자열을 받아 오면 "파일명.mp3"만 빼 내서 노트파일에 저장하는 함수.
int CNotePickingView::saveMp3FileName(WCHAR *file)
{
	CNoteEditingToolDoc* pDoc = GetDocument();		// 임시 도큐먼트 포인터

	int tailIndex = MAX_PATH-1;				// 파일명의 맨 마지막 인덱스
	int headIndex = 0;						// 파일명의 맨 처음 인덱스
	WCHAR tempFileName[MAX_PATH];

	while( file[tailIndex] != NULL )
	{
		// 맨 마지막 글을 찾아낸다.
		tailIndex--;
	}
	if ( tailIndex < 3 )
	{
		// 3보다 더 작으면 확장자도 없는 것이므로, 에러 출력
		MessageBox(_T("CNotePickingView::saveMp3FileName 에서의 에러"));
		return -1;
	}

	// 여기서부터 '\'가 나오거나, 끝에 다다를 때까지 탐색
	headIndex = tailIndex;
	while( headIndex >= 0 &&
		file[headIndex] != '\\')
	{
		headIndex--;
	}
	headIndex++;

	// 파일명과 확장자 부분만 따로 저장.
	for( int i=0 ; i< tailIndex - headIndex ; i++ )
	{
		tempFileName[i] = file[i + headIndex];
	}


	// wchar_t를 std::string으로 바꾸는 작업.
	int  wLen;
	char *cArr;

	wLen = wcslen(tempFileName);
	cArr = (char*)calloc( 1, wLen*2+1);
	WideCharToMultiByte(CP_ACP,0, tempFileName, wLen, cArr, wLen*2,0,0);

	string str = cArr;

	//	pDoc->setMp3FileName(tempFileName);
	pDoc->setMp3FileName(cArr);



	return 1;
}

// 비트맵 이미지를 받아 와, 입력받은 색을 자른다.
//-----------------------------------------------------------------------------------------------------
// 함수명 : 마스크 이미지 만들기
// 인자명 : 1.  in_bitmap          : 원본 비트맵
//          2.  in_transColor      : 투명색
//-----------------------------------------------------------------------------------------------------
CBitmap* CNotePickingView::Create_MaskBitmap( CBitmap* in_bitmap, COLORREF in_transColor)
{
	//Data
	CDC memDC_org;					// DC for Original 
	CDC memDC_mask;					// DC for Mask

	CBitmap* bitmap_mask;			// bitmap for mask
	BITMAP bitmap_info;				// bitmap info

	// Init
	bitmap_mask =new CBitmap();
	in_bitmap->GetBitmap( &bitmap_info);
	bitmap_mask->CreateBitmap( bitmap_info.bmWidth, bitmap_info.bmHeight, 1, 1, NULL);
	memDC_org.CreateCompatibleDC( NULL );
	memDC_mask.CreateCompatibleDC( NULL );

	memDC_org.SelectObject( *in_bitmap );
	memDC_mask.SelectObject( *bitmap_mask );

	// Exec
	memDC_org.SetBkColor( in_transColor);
	memDC_mask.BitBlt( 0, 0, bitmap_info.bmWidth, bitmap_info.bmHeight, &memDC_org, 0, 0, SRCCOPY);
	memDC_org.BitBlt( 0, 0, bitmap_info.bmWidth, bitmap_info.bmHeight, &memDC_mask, 0, 0, SRCINVERT);

	return bitmap_mask;
}



void CNotePickingView::tempTest()
{
	CNoteEditingToolDoc* pDoc = GetDocument();

	// BASS 라이브러리 초기화
 	if (!BASS_Init(-1,44100,0,win,NULL))
	{
		::MessageBox(0, _T("Bass 라이브러리 초기화 실패"),_T("에러"),16);
		return;
	}

	/*
	if (!(chan=BASS_StreamCreateFile(FALSE,"RHCP - Universally Speaking.mp3",0,0,0))
		&& !(chan=BASS_MusicLoad(FALSE,"RHCP - Universally Speaking.mp3",0,0,BASS_MUSIC_RAMPS|BASS_MUSIC_POSRESET|BASS_MUSIC_PRESCAN,1))) 
	{
		::MessageBox(0, _T("C3.mp3 로드 실패"), _T("에러"), 16);
	}


	BASS_ChannelSetSync(chan, BASS_SYNC_END | BASS_SYNC_MIXTIME, 0, LoopSyncProc,0); // set sync to loop at end
	
	nowPlayingStatus = 0;

	*/

	//char file[MAX_PATH] = "";//"C:\\Users\\TALON\\Music\\Luther Vandross - Superstar.mp3";
	WCHAR file[MAX_PATH] = _T("");
	// TCHAR로 정의 할 경우 자동으로 WCHAR로 변환 해 주지만, BASS_StreamCreateFile의 BASS_UNICODE Flag를 세워줬기 때문에 명시적으로 정의한다.

	OPENFILENAME ofn={0};
	ofn.lStructSize=sizeof(ofn);
	ofn.hwndOwner=win;
	ofn.nMaxFile = MAX_PATH;
	ofn.lpstrFile = file;
	//swprintf(ofn.lpstrFile, _T("%s"), file);
	ofn.Flags=OFN_FILEMUSTEXIST|OFN_HIDEREADONLY|OFN_EXPLORER;
	ofn.lpstrTitle = _T("Select a file to play");
	ofn.lpstrFilter= _T("Playable files\0*.mp3;*.mp2;*.mp1;*.ogg;*.wav;*.aif;*.mo3;*.it;*.xm;*.s3m;*.mtm;*.mod;*.umx\0All files\0*.*\0\0");
	if (!GetOpenFileName(&ofn))
	{
		return;
	}



	if (!(chan=BASS_StreamCreateFile(FALSE,file,0,0,BASS_UNICODE)) )
	{
		// 파일을 오픈 할 때, 유니코드로 읽도록 플래그를 설정 해 줘야 한다.
		Error(_T("Can't play file"));
		if( !(chan=BASS_MusicLoad(FALSE,file,0,0,BASS_MUSIC_RAMPS|BASS_MUSIC_POSRESET|BASS_MUSIC_PRESCAN,1))) 
		{
			Error(_T("Can't play file"));
			return; // Can't load the file
		}
	}

	{
		//tempLength = (DWORD)BASS_ChannelGetLength(chan, BASS_POS_BYTE) / 20000;
		WIDTH  = (DWORD)( BASS_ChannelGetLength(chan, BASS_POS_BYTE) / BSS_WAVE_FORM_RATE * BSS_WAVE_FORM_RATE_REAL );
		//WIDTH = 300;

		BYTE data[2000]={0};
		BITMAPINFOHEADER *bh=(BITMAPINFOHEADER*)data;
		RGBQUAD *pal=(RGBQUAD*)(data+sizeof(*bh));
		int a;
		bh->biSize=sizeof(*bh);
		bh->biWidth= (LONG)(WIDTH * BSS_WAVE_FORM_RATE_REAL);
		bh->biHeight=-HEIGHT;
		bh->biPlanes=1;
		bh->biBitCount=8;
		bh->biClrUsed = bh->biClrImportant = HEIGHT/2+1;
		// setup palette
		for (a=1;a<=HEIGHT/2;a++)
		{
			pal[a].rgbRed=(255*a)/(HEIGHT/2);
			pal[a].rgbGreen=255-pal[a].rgbRed;
		}
		// create the bitmap
		wavebmp=CreateDIBSection( 0,(BITMAPINFO*)bh,DIB_RGB_COLORS, (void**)&wavebuf, NULL, 0 );
		wavedc=CreateCompatibleDC(0);
		SelectObject(wavedc,wavebmp);
	}

	
	bpp = (DWORD)( BASS_ChannelGetLength(chan,BASS_POS_BYTE) / WIDTH / BSS_WAVE_FORM_RATE_REAL  ); // bytes per pixel
	if (bpp<BASS_ChannelSeconds2Bytes(chan,PIXEL_TO_BASS_MILTIME)) // minimum 20ms per pixel (BASS_ChannelGetLevel scans 20ms)
		bpp = (DWORD)(BASS_ChannelSeconds2Bytes(chan,PIXEL_TO_BASS_MILTIME));
	//BASS_ChannelSetSync(chan,BASS_SYNC_END|BASS_SYNC_MIXTIME,0,LoopSyncProc,0); // set sync to loop at end
	//BASS_ChannelPlay(chan,FALSE); // start playing
	
	{ // start scanning peaks in a new thread
		DWORD chan2 = BASS_StreamCreateFile(FALSE, file, 0, 0, BASS_STREAM_DECODE | BASS_UNICODE);
		// 파일을 오픈 할 때, 유니코드로 읽도록 플래그를 설정 해 줘야 한다.
		if (!chan2)
		{
			chan2=BASS_MusicLoad( FALSE, file, 0, 0, BASS_MUSIC_DECODE | BASS_UNICODE, 1 );
		}

		// 스레드 발싸
		scanthread = _beginthread(ScanPeaks, 0, (void*)chan2);
		//scanthread =:_beginthreadx(ScanPeaks, 0, (void*)chan2);
	}

	nowPlayingStatus = 0;
//	BASS_ChannelPlay(chan, FALSE); // start playing

	// 여기까지 왔으면, 파일 이름을 읽어와 저장한다.
	saveMp3FileName(file);


	// 끝나는 시간 기본을 음악의 종료시간으로 설정
	QWORD tempLenForSecond = BASS_ChannelGetLength(chan, BASS_POS_BYTE); // the length in bytes
	double tempTimeInSecond = BASS_ChannelBytes2Seconds(chan, tempLenForSecond); // the length in seconds

	NoteTime EndMusicTime;
	EndMusicTime.noteTimeSec = (unsigned int)(tempTimeInSecond);
	EndMusicTime.noteTimeMilSec = (unsigned int)( (tempTimeInSecond - EndMusicTime.noteTimeSec) * 1000 );

	 pDoc->setEndSongTime(EndMusicTime);
	 EndMusicTime.noteTimeSec = 0;
	 EndMusicTime.noteTimeMilSec = 0;
	 pDoc->setStartSongTime(EndMusicTime);



	// 스크롤 창 크기 조정
	CSize sizeTotal;
	// TODO: 이 뷰의 전체 크기를 계산합니다.
	sizeTotal.cx = (LONG)( (WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X );
	sizeTotal.cy = FRAME_1_HEIGHT;
	SetScrollSizes(MM_TEXT, sizeTotal);

}




void CNotePickingView::tempPause()
{ 
	CNoteEditingToolDoc *pDoc = GetDocument();


	// destroy the timer
	timeKillEvent(m_idEvent);
	// reset the timer
	//timeEndPeriod (m_uResolution);
	m_idEvent = NULL;

	//	KillTimer(TIMER_1_ID);
	mutexFlag = true;

	if ( !BASS_ChannelPause(chan) )
	{
		MessageBox(_T("일시정지 에러!"));
	}
	else
	{
		if( pDoc->getEditModeSelectViewPtr() != NULL )
		{
			// 메시지를 보낸다.
			pDoc->getEditModeSelectViewPtr()->SendMessage(CM_REDRAW_VIEW2);
		}

		// 다시 그린다.
		DrawDoubleBuffering();
		Invalidate();

		nowPlayingStatus = 2;
	}
}

void CNotePickingView::tempStop()
{
	CNoteEditingToolDoc *pDoc = GetDocument();


	// destroy the timer
	timeKillEvent(m_idEvent);
	// reset the timer
	//timeEndPeriod (m_uResolution);
	m_idEvent = NULL;

	mutexFlag = true;


	if ( !BASS_ChannelStop(chan) )
	{
		MessageBox(_T("멈춤 에러!"));
	}
	else
	{
		nowPlayingStatus = PLAY_STATE_STOP;
		if( pDoc->getEditModeSelectViewPtr() != NULL )
		{
			// 메시지를 보낸다.
			pDoc->getEditModeSelectViewPtr()->SendMessage(CM_REDRAW_VIEW2);
		}

		BASS_ChannelSetPosition(chan,0,BASS_POS_BYTE);

		// 다시 그린다.
		DrawDoubleBuffering();
		Invalidate();
	}
}

void CNotePickingView::tempPlay()
{
	//KillTimer(TIMER_1_ID);


	CNoteEditingToolDoc *pDoc = GetDocument();


	if ( scanthread )
	{
		// 아직 음원이 분석중인 경우,
		AfxMessageBox(_T("아직 음원을 분석중에 있습니다."));
	}
	else
	{
		if( !BASS_ChannelPlay(chan, FALSE) )
		{
			MessageBox(_T("재시작 에러!"));
		}
		else
		{	
			nowPlayingStatus = 1;

			if( pDoc->getEditModeSelectViewPtr() != NULL )
			{
				// 메시지를 보낸다.
				pDoc->getEditModeSelectViewPtr()->SendMessage(CM_REDRAW_VIEW2);
			}


			// 타이머 설정 20hz

			/*
			m_uTimer = SetTimer(TIMER_1_ID, TIMER_1_MSEC, NULL);
			if( m_uTimer == 0 )
			{
			AfxMessageBox(_T("타이머 동작 실패!"));
			}
			*/
			m_idEvent = timeSetEvent(TIMER_1_MSEC, m_uResolution, this->TimerFunction, (DWORD)this, TIME_PERIODIC);
			if ( m_idEvent == NULL )
			{
				// 타이머 설정에 실패
				AfxMessageBox(_T("타이머 동작 실패!"));
			}
			mutexFlag = true;
		}
	}
}


//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [BASS END]    //////////////////////////////
//////////////////////////////////////////////////////////////////////////////





// CNotePickingView 그리기입니다.

void CNotePickingView::OnInitialUpdate()
{
	CScrollView::OnInitialUpdate();
	
	CNoteEditingToolDoc* pDoc = GetDocument();


	// BASS Lib에서 사용하기 위한 특성입니다.
	this->win = NULL;
	this->scanthread = 0;
	this->killscan = FALSE;

	this->wavedc = 0;
	this->wavebmp = 0;

	useByLoopSyncProc = this;			// 자신의 포인터를 등록

	this->bpp = 5000;					// 임시 초기값




	// BASS 관련 플래그들
	nowPlayingStatus = -1;


	WIDTH = 3000;			// display width
	// 끝.

	// BASS Lib 초기화
	//	tempTest();
	// Doc에서 사용 할 포인터
	pDoc->setNotePickingViewPtr(this);



	CSize sizeTotal;
	// TODO: 이 뷰의 전체 크기를 계산합니다.
	sizeTotal.cx = WIDTH;
	sizeTotal.cy = FRAME_1_HEIGHT;
	SetScrollSizes(MM_TEXT, sizeTotal);

	// 만약 잡혀있는 채널이 있다면, 그 채널을 풀어준다.
	if( chan )
	{
		// 스트림을 풀어 준 다음,
		BASS_StreamFree(chan);
		chan = 0;

		// 그림의 리셋
		// 사용했던 그림을 삭제
		if (wavedc)
		{
			DeleteDC(wavedc);
		}
		if (wavebmp)
		{
			DeleteObject(wavebmp);
		}
	}

	// 만약 아직도 스캐닝 중일 경우,
	if (scanthread)
	{
		// still scanning
		killscan = TRUE;
		WaitForSingleObject((HANDLE)(scanthread), 1000); // wait for the thread
	}

	// 만약 파일을 불러 온 거라면, 그음악을 찾는다.
	if ( bassInitFlag == true )
	{

		// 초기화를 한 적이 있다면, 파일을 로드했을 때거나, 새 파일을 생성했을 때이므로
		// 음악 파일이 있는지 확인 한 다음 일을 한다.
		if ( BLoadMusicFileAuto() < 0 )
		{
			// 음악 파일 로드 실패.
			//AfxMessageBox(_T("Error in BLoadMusicFileAuto()"));
			nowPlayingStatus = PLAY_STATE_NO_MUSIC;
		}
	}
	else
	{
		// 초기화를 한 적이 없다면, Bass Lib 초기화
		if( BInit() < 0 )
		{
			// 에러 난 것.
			return;
		}
		
		//AfxMessageBox(_T("여기까지 실행"));
		
		// 혹시나 외부에서 바로 파일을 열어 실행했을 수도 있으므로,
		if ( BLoadMusicFileAuto() < 0 )
		{
			//AfxMessageBox(_T("Error in BLoadMusicFileAuto()"));
		}

	}
	
 	//// 만약 타이머가 있다면 버린다.
 	//if ( m_uTimer )
 	//{
 	//	KillTimer(TIMER_1_ID);
 	//}
 
 	//// 타이머 변수 초기화
 	//m_uTimer = 0;
	// 타이머 종료
	if ( m_idEvent )
	{
		// 계속해서 여기에서 에러가 나서 지움. (아마도 메인프레임이 먼저 없어지기 때문에 문제인 듯)
		// destroy the timer
		timeKillEvent(m_idEvent);
		// reset the timer
		//timeEndPeriod (m_uResolution);
		m_idEvent = NULL;
	}

	// 타이머 변수 초기화
	m_uTimer = 0;
	mutexFlag = true;


	// 타이머에서 사용 할 변수 초기화
	//onTimerMousePoint.x = 0;
	//onTimerMousePoint.y = 0;
	//chkMouseHandleState = TIMER_1_FLAG_NULL;
	m_timerRedrawFlag = false;


	// 화면을 그릴 준비를 한다.	
	DrawDoubleBuffering();

}
// 더블 버퍼링의 이미지를 그린다.
void CNotePickingView::DrawDoubleBuffering(void)
{
	CNoteEditingToolDoc	* pDoc = GetDocument();
	ASSERT_VALID(pDoc);
	if (!pDoc)
		return;
	// TODO: 여기에 그리기 코드를 추가합니다.

//	CDC* pDC = GetDC();						// 다큐멘트를 받아오는 것?

	//COLORREF m_FramColor = ARGB();

	// 찍는 화면이 스크롤 뷰이기 때문에, 스크롤의 위치에 따른 보정이 필요하다.
	int nVertScroll = 0;//GetScrollPos(SB_VERT);	// 세로
	int nHorzScroll = GetScrollPos(SB_HORZ);	// 가로
	CPoint scrollPos = CPoint(nHorzScroll, nVertScroll);

	unsigned int i=0;
	unsigned int j=0;
	CNoteData* tempNote = pDoc->getNoteAddr(0, 0);

	CDC* pDC = GetDC();							// 다큐멘트를 받아오는 것?
	CPoint newPoint;							// 사각형을 그릴 좌표.

	char drawOkFlag = 0;						// 사각형을 그려도 되는 것인지 확인하는 플래그.
	NoteTime noteTime;
	char editMode = pDoc->getNoteEditingMode();

	// 색 칠하는 용.
	// 만약 GDI+를 하고 싶다면, SolidBrush로 만들어서,
	//Graphics graphics(*pDC);
	//graphics.SetSmoothingMode(SmoothingModeHighQuality);
	//Pen pen(BACK_PEN_COLOR, 1);
	SolidBrush brush(BACK_COLOR_FOR_DRAG);

	//pen.SetLineJoin(LineJoinRound);			// 모서리가 둥근 사각형
	//graphics.DrawRectangle(&pen, x, y, 너비, 높이);
	//graphics.FillRectangle(&brush, x, y, 너비, 높이);
	// 와 같이 한다.
	
	CBrush newBrush(RGB(255, 255, 255));
	CBrush* oldBrush;
	CRect rect;
	GetClientRect(&rect);

	// 더블 버퍼링을 위한 객체 생성
	CDC memDC;					// 처리 CDC을 지정 한다.
	CDC timeLineDC;				// 타임라인용 CDC

	// 빈공간을 새롭게 만든다.
	CDC mdcOffScreen;			// 더블버퍼링을 위한 메모리 그림버퍼
	// memDC에 그림을 그려서 mdcOffScreen에 올린 다음, 최종적으로 mdc를 pDC에 올린다.
	CBitmap *oldbitmap;


	//HDC hdc = ::GetDC(this->m_hWnd);
	//HDC hdc = ::GetDC(mdcOffScreen.)


	// 뮤텍스 건다.
	mutexFlag = false;



	// 도화지를 현재 스크린 DC와 일치시킨다.
	bmpOffScreen.DeleteObject();
	bmpOffScreen.CreateCompatibleBitmap(pDC, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X, FRAME_1_HEIGHT);
	bmpOfTimeLine.DeleteObject();
	bmpOfTimeLine.CreateCompatibleBitmap(pDC, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X, FRAME_1_HEIGHT);
	bMaskedTimeLine.DeleteObject();
	//bMaskedTimeLine.CreateCompatibleBitmap(pDC, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X, FRAME_1_HEIGHT);
	if ( bmpOfTimeLineMask != NULL )
	{
		bmpOfTimeLineMask->DeleteObject();
	}



	// DC와 같은 메모리 DC를 만들어 주는 함수 (가상 화면 DC를 현재 스크린 DC와 일치시킨다.)
	memDC.CreateCompatibleDC(pDC);
	mdcOffScreen.CreateCompatibleDC(pDC);
	timeLineDC.CreateCompatibleDC(pDC);

	// 가상 화면 DC에 도화지를 깐다.
	oldbitmap = mdcOffScreen.SelectObject(&bmpOffScreen);
	memDC.SelectObject(&bmpOfTimeLine);
	timeLineDC.SelectObject(&bMaskedTimeLine);


	// GDI+를 위해
	Graphics graphics(mdcOffScreen);
	graphics.SetSmoothingMode(SmoothingModeHighQuality);


	// 배경 그리기
	//BitBlt(hdc, - scrollPos.x + V_NOTE_SET_OFF_X, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), HEIGHT, wavedc, 0, 0, SRCCOPY); // draw peak waveform
	mdcOffScreen.BitBlt(V_NOTE_SET_OFF_X, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X, HEIGHT, CDC::FromHandle(wavedc), 0, 0, SRCCOPY);
	//pDC->BitBlt(0, 0, rect.Width(), rect.Height(), &mdcOffScreen, 0, 0, SRCCOPY);
	//pDC->BitBlt(0, 0, WIDTH, FRAME_1_HEIGHT, &mdcOffScreen, 0, 0, SRCCOPY);

	// 타임라인 그리기
	DrawTimeLineEx(memDC.m_hDC, BASS_ChannelGetPosition(chan, BASS_POS_BYTE ), 0xffffff, 0, scrollPos); // current pos
	// 검은색을 배경색으로 설정	
	//pDC->BitBlt(0, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), FRAME_1_HEIGHT, &memDC, 0, 0, SRCCOPY);

	bmpOfTimeLineMask = Create_MaskBitmap(&bmpOfTimeLine, RGB(0, 0, 0));


	memDC.SelectObject(bmpOfTimeLineMask);
	timeLineDC.BitBlt(0, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), FRAME_1_HEIGHT, &memDC, 0, 0, SRCAND);
	//pDC->BitBlt(-scrollPos.x, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X, FRAME_1_HEIGHT, &memDC, 0, 0, SRCCOPY);

	memDC.SelectObject(bmpOfTimeLine);
	timeLineDC.BitBlt(0, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), FRAME_1_HEIGHT, &memDC, 0, 0, SRCPAINT);


	//memDC.SetBkColor(RGB(0, 0, 0));
	//pDC->BitBlt(0, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), FRAME_1_HEIGHT, &timeLineDC, 0, 0, SRCCOPY);

	//DrawTimeLineEx(mdcOffScreen.m_hDC, BASS_ChannelGetPosition(chan, BASS_POS_BYTE ), 0xffffff, 0, scrollPos); // current pos
	//mdcOffScreen.BitBlt(0, 0, rect.Width(), rect.Height(), CDC::FromHandle(hdc), 0, 0, SRCCOPY);


	//pDC->BitBlt(0, 0, rect.Width(), rect.Height(), &mdcOffScreen, 0, 0, SRCCOPY);




	// 배경 선 그리기
	if( drawBackgroundLines(&mdcOffScreen) < 0 )
	{
		// 만약 배경 그리기에 문제가 있을 경우.
		AfxMessageBox(_T("배경 그리기에 문제가 있습니다."));
	}

	//pDC->BitBlt(0, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), FRAME_1_HEIGHT, &memDC, 0, 0, SRCCOPY);
	//pDC->BitBlt(0, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), FRAME_1_HEIGHT, &mdcOffScreen, 0, 0, SRCCOPY);
	





	// 노트들 그리기.

	if( editMode == EDIT_MODE_WRITE ||
		editMode == EDIT_MODE_ERASE)
	{
		//////////// 쓰기 모드일 경우.

		for( i=0 ; i < MAX_NOTE_ARRAY ; i++ )
		{
			for( j=0 ; j < MAX_HALF_ARRAY ; j++ )
			{
				// 하나 받아 온다.
				tempNote = pDoc->getNoteAddr(i, j);

				if( tempNote == NULL )
				{	// 탐색이 끝나면 다음 루프로 간다.
					break;
				}


				// 노트의 타입에 따른 색 결정
				switch( tempNote->getNoteType() )
				{
				case NOTE_T_RIGHT:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_RIGHT);
					break;

				case NOTE_T_LEFT:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_LEFT);
					break;

				case NOTE_T_LONG:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_LONG);
					break;

				case NOTE_T_PATTERN:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_PATTERN);
					break;

				case NOTE_T_CHARISMA:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_CHARISMA);
					break;

				case NOTE_T_NEUTRAL:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_NEUTRAL);
					break;

				case NOTE_T_DRAG:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_DRAG);
					break;

				case NOTE_T_BPM:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_BPM);
					break;

				case NOTE_T_PHOTO:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_PHOTO);
					break;

				default:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_UNKNOWN);
				}

				// 현재 브러시 내용을 받아온다.
				oldBrush = mdcOffScreen.SelectObject(&newBrush);
				//oldBrush = pDC->SelectObject(&newBrush);


				// 노트의 시간을 받아 온 다음,
				noteTime.noteTimeSec = tempNote->getNoteTimeSec();
				noteTime.noteTimeMilSec = tempNote->getNoteTimeMilSec();

				// 그 시간을 적절한 사각형의 좌표로 변경한다.
				newPoint.x = ConvertTimeToPixX(noteTime);	
				newPoint.y = ConvertTypeToPixY(tempNote->getNoteType(), tempNote->getTargetMarker());

				if(
					tempNote->getNoteType() == NOTE_T_CHARISMA ||
					tempNote->getNoteType() == NOTE_T_NEUTRAL
					)
				{
					// 다른 노트와 겹칠 수 없는 장거리 노트의 경우,
					NoteTime noteEndTime;
					CPoint newPoint2;

					// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
					noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
					noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

					newPoint2.x = ConvertTimeToPixX(noteEndTime);
					newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.


					// 그릴 수 없는부분 출력
					brush.SetColor(BACK_COLOR_FOR_DRAG);
					graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_AFTER_CHAR / (double)1000)), V_LINE_TARG6_Y - V_NOTE_SET_OFF_Y);


					// 그림을 출력한다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

					// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
					// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					// 					pDC->Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);
				}
				else if( tempNote->getNoteType() == NOTE_T_DRAG ||
					tempNote->getNoteType() == NOTE_T_PATTERN
					)
				{
					// 다른 노트와 겹칠 수 없는 장거리 노트의 경우,
					NoteTime noteEndTime;
					CPoint newPoint2;

					// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
					noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
					noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

					newPoint2.x = ConvertTimeToPixX(noteEndTime);
					newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.


					// 그릴 수 없는부분 출력
					brush.SetColor(BACK_COLOR_FOR_DRAG);
					graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_AFTER_DRAG / (double)1000)), V_LINE_TARG6_Y - V_NOTE_SET_OFF_Y);


					// 그림을 출력한다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

					// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
					// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					// 					pDC->Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);
				}
				else if( tempNote->getNoteType() == NOTE_T_LONG )
				{
					// 롱노트의 경우.
					NoteTime noteEndTime;
					CPoint newPoint2;

					// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
					noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
					noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

					newPoint2.x = ConvertTimeToPixX(noteEndTime);
					newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.

					// 그릴 수 없는부분 출력
					brush.SetColor(BACK_COLOR_FOR_RIGHT);
					graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);

					// 그림을 출력한다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

				}	
				else if( tempNote->getNoteType() == NOTE_T_RIGHT )
				{
					// 오른손 노트의 경우
					// 그릴 수 없는부분 출력
					brush.SetColor(BACK_COLOR_FOR_RIGHT);
					graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);


					// 실제 사각형을 그린다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
				}	
				else if( tempNote->getNoteType() == NOTE_T_LEFT )
				{
					// 왼손 노트의 경우
					// 그릴 수 없는부분 출력
					brush.SetColor(BACK_COLOR_FOR_LEFT);
					graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);

					// 실제 사각형을 그린다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
				}
				else
				{
					// 실제 사각형을 그린다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
				}




				// 브러시 반환.
				mdcOffScreen.SelectObject(oldBrush);
				//pDC->SelectObject(oldBrush);


			}
		}



	}
	else if ( editMode == EDIT_MODE_MOVE )
	{
		int noteHeadDir;


		//////////// 편집 모드일 경우.
		if( pDoc->getNowEditingNote(noteHeadDir) == NULL )
		{
			for( i=0 ; i < MAX_NOTE_ARRAY ; i++ )
			{
				for( j=0 ; j < MAX_HALF_ARRAY ; j++ )
				{
					// 하나 받아 온다.
					tempNote = pDoc->getNoteAddr(i, j);

					if( tempNote == NULL )
					{	// 탐색이 끝나면 다음 루프로 간다.
						break;
					}

					// 노트의 타입에 따른 색 결정
					switch( tempNote->getNoteType() )
					{
					case NOTE_T_RIGHT:
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(NC_RIGHT);
						break;

					case NOTE_T_LEFT:
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(NC_LEFT);
						break;

					case NOTE_T_LONG:
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(NC_LONG);
						break;

					case NOTE_T_PATTERN:
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(NC_PATTERN);
						break;

					case NOTE_T_CHARISMA:
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(NC_CHARISMA);
						break;

					case NOTE_T_NEUTRAL:
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(NC_NEUTRAL);
						break;

					case NOTE_T_DRAG:
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(NC_DRAG);
						break;

					case NOTE_T_BPM:
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(NC_BPM);
						break;

					case NOTE_T_PHOTO:
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(NC_PHOTO);
						break;

					default:
						newBrush.CreateSolidBrush(NC_UNKNOWN);
					}

					// 현재 브러시 내용을 받아온다.
					//oldBrush = pDC->SelectObject(&newBrush);
					oldBrush = mdcOffScreen.SelectObject(&newBrush);


					// 노트의 시간을 받아 온 다음,
					noteTime.noteTimeSec = tempNote->getNoteTimeSec();
					noteTime.noteTimeMilSec = tempNote->getNoteTimeMilSec();

					// 그 시간을 적절한 사각형의 좌표로 변경한다.
					newPoint.x = ConvertTimeToPixX(noteTime);
					newPoint.y = ConvertTypeToPixY(tempNote->getNoteType(), tempNote->getTargetMarker());

					if(
						tempNote->getNoteType() == NOTE_T_CHARISMA ||
						tempNote->getNoteType() == NOTE_T_NEUTRAL
						)
					{
						// 다른 노트와 겹칠 수 없는 장거리 노트의 경우,
						NoteTime noteEndTime;
						CPoint newPoint2;

						// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
						noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
						noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

						newPoint2.x = ConvertTimeToPixX(noteEndTime);
						newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.


						// 그릴 수 없는부분 출력
						brush.SetColor(BACK_COLOR_FOR_DRAG);
						graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_AFTER_CHAR / (double)1000)), V_LINE_TARG6_Y - V_NOTE_SET_OFF_Y);


						// 그림을 출력한다.
						mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
						mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
						mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

						// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
						// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
						// 					pDC->Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);
					}
					else if( tempNote->getNoteType() == NOTE_T_DRAG ||
						tempNote->getNoteType() == NOTE_T_PATTERN
						)
					{
						// 다른 노트와 겹칠 수 없는 장거리 노트의 경우,
						NoteTime noteEndTime;
						CPoint newPoint2;

						// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
						noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
						noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

						newPoint2.x = ConvertTimeToPixX(noteEndTime);
						newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.


						// 그릴 수 없는부분 출력
						brush.SetColor(BACK_COLOR_FOR_DRAG);
						graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_AFTER_DRAG / (double)1000)), V_LINE_TARG6_Y - V_NOTE_SET_OFF_Y);


						// 그림을 출력한다.
						mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
						mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
						mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

						// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
						// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
						// 					pDC->Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);
					}
					else if( tempNote->getNoteType() == NOTE_T_LONG )
					{
						// 롱노트의 경우.
						NoteTime noteEndTime;
						CPoint newPoint2;

						// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
						noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
						noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

						newPoint2.x = ConvertTimeToPixX(noteEndTime);
						newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.

						// 그릴 수 없는부분 출력
						brush.SetColor(BACK_COLOR_FOR_RIGHT);
						graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);

						// 그림을 출력한다.
						mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
						mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
						mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

					}	
					else if( tempNote->getNoteType() == NOTE_T_RIGHT )
					{
						// 오른손 노트의 경우
						// 그릴 수 없는부분 출력
						brush.SetColor(BACK_COLOR_FOR_RIGHT);
						graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);


						// 실제 사각형을 그린다.
						mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					}	
					else if( tempNote->getNoteType() == NOTE_T_LEFT )
					{
						// 왼손 노트의 경우
						// 그릴 수 없는부분 출력
						brush.SetColor(BACK_COLOR_FOR_LEFT);
						graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);

						// 실제 사각형을 그린다.
						mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					}
					else
					{
						// 실제 사각형을 그린다.
						mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
						// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					}



					// 브러시 반환.
					pDC->SelectObject(oldBrush);
					//pDC->SelectObject(oldBrush);

				}
			}
		}
		else
		{
			// 뭔가 수정중에 있는 노트가 있을 경우
			if( pDoc->getOnFocusEditingFlag() == true )
			{
				// 현재 그 노트를 클릭중에 있을 때
				int iisisisi;

				// 사각형들을 그린다.
				for( i=0 ; i < MAX_NOTE_ARRAY ; i++ )
				{
					for( j=0 ; j < MAX_HALF_ARRAY ; j++ )
					{
						// 하나 받아 온다.
						tempNote = pDoc->getNoteAddr(i, j);

						if( tempNote == NULL )
						{	// 탐색이 끝나면 다음 루프로 간다.
							break;
						}

						if( pDoc->getNowEditingNote(iisisisi) == tempNote)
						{
							// 만약 현재 잡아끌고 있는 노트일 경우,

						}
						else
						{
							// 노트의 타입에 따른 색 결정
							switch( tempNote->getNoteType() )
							{
							case NOTE_T_RIGHT:
								newBrush.DeleteObject();
								newBrush.CreateSolidBrush(NC_RIGHT);
								break;

							case NOTE_T_LEFT:
								newBrush.DeleteObject();
								newBrush.CreateSolidBrush(NC_LEFT);
								break;

							case NOTE_T_LONG:
								newBrush.DeleteObject();
								newBrush.CreateSolidBrush(NC_LONG);
								break;

							case NOTE_T_PATTERN:
								newBrush.DeleteObject();
								newBrush.CreateSolidBrush(NC_PATTERN);
								break;

							case NOTE_T_CHARISMA:
								newBrush.DeleteObject();
								newBrush.CreateSolidBrush(NC_CHARISMA);
								break;

							case NOTE_T_NEUTRAL:
								newBrush.DeleteObject();
								newBrush.CreateSolidBrush(NC_NEUTRAL);
								break;

							case NOTE_T_DRAG:
								newBrush.DeleteObject();
								newBrush.CreateSolidBrush(NC_DRAG);
								break;

							case NOTE_T_BPM:
								newBrush.DeleteObject();
								newBrush.CreateSolidBrush(NC_BPM);
								break;

							case NOTE_T_PHOTO:
								newBrush.DeleteObject();
								newBrush.CreateSolidBrush(NC_PHOTO);
								break;

							default:
								newBrush.CreateSolidBrush(NC_UNKNOWN);
							}

							// 현재 브러시 내용을 받아온다.
							oldBrush = mdcOffScreen.SelectObject(&newBrush);
							//oldBrush = pDC->SelectObject(&newBrush);





							// 잡아끌고 있는 노트가 아닐 경우,

							// 노트의 시간을 받아 온 다음,
							noteTime.noteTimeSec = tempNote->getNoteTimeSec();
							noteTime.noteTimeMilSec = tempNote->getNoteTimeMilSec();

							// 그 시간을 적절한 사각형의 좌표로 변경한다.
							newPoint.x = ConvertTimeToPixX(noteTime);
							newPoint.y = ConvertTypeToPixY(tempNote->getNoteType(), tempNote->getTargetMarker());
							if(
								tempNote->getNoteType() == NOTE_T_CHARISMA ||
								tempNote->getNoteType() == NOTE_T_NEUTRAL
								)
							{
								// 다른 노트와 겹칠 수 없는 장거리 노트의 경우,
								NoteTime noteEndTime;
								CPoint newPoint2;

								// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
								noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
								noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

								newPoint2.x = ConvertTimeToPixX(noteEndTime);
								newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.


								// 그릴 수 없는부분 출력
								brush.SetColor(BACK_COLOR_FOR_DRAG);
								graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_AFTER_CHAR / (double)1000)), V_LINE_TARG6_Y - V_NOTE_SET_OFF_Y);


								// 그림을 출력한다.
								mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
								mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
								mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

								// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
								// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
								// 					pDC->Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);
							}
							else if( tempNote->getNoteType() == NOTE_T_DRAG ||
								tempNote->getNoteType() == NOTE_T_PATTERN
								)
							{
								// 다른 노트와 겹칠 수 없는 장거리 노트의 경우,
								NoteTime noteEndTime;
								CPoint newPoint2;

								// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
								noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
								noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

								newPoint2.x = ConvertTimeToPixX(noteEndTime);
								newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.


								// 그릴 수 없는부분 출력
								brush.SetColor(BACK_COLOR_FOR_DRAG);
								graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_AFTER_DRAG / (double)1000)), V_LINE_TARG6_Y - V_NOTE_SET_OFF_Y);


								// 그림을 출력한다.
								mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
								mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
								mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

								// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
								// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
								// 					pDC->Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);
							}
							else if( tempNote->getNoteType() == NOTE_T_LONG )
							{
								// 롱노트의 경우.
								NoteTime noteEndTime;
								CPoint newPoint2;

								// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
								noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
								noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

								newPoint2.x = ConvertTimeToPixX(noteEndTime);
								newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.

								// 그릴 수 없는부분 출력
								brush.SetColor(BACK_COLOR_FOR_RIGHT);
								graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);

								// 그림을 출력한다.
								mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
								mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
								mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

							}	
							else if( tempNote->getNoteType() == NOTE_T_RIGHT )
							{
								// 오른손 노트의 경우
								// 그릴 수 없는부분 출력
								brush.SetColor(BACK_COLOR_FOR_RIGHT);
								graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);


								// 실제 사각형을 그린다.
								mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
							}	
							else if( tempNote->getNoteType() == NOTE_T_LEFT )
							{
								// 왼손 노트의 경우
								// 그릴 수 없는부분 출력
								brush.SetColor(BACK_COLOR_FOR_LEFT);
								graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);

								// 실제 사각형을 그린다.
								mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
							}
							else
							{
								// 실제 사각형을 그린다.
								mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
								// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
							}


							// 브러시 반환.
							mdcOffScreen.SelectObject(oldBrush);
							//							pDC->SelectObject(oldBrush);
						}
					}
				}
			}
			// 외곽선을 그린다.
			else
			{
				// 그냥 마우스만 올려놓고 있는 상황일 때.

				// 사각형들을 그린다.
				for( i=0 ; i < MAX_NOTE_ARRAY ; i++ )
				{
					for( j=0 ; j < MAX_HALF_ARRAY ; j++ )
					{
						// 하나 받아 온다.
						tempNote = pDoc->getNoteAddr(i, j);

						if( tempNote == NULL )
						{	// 탐색이 끝나면 다음 루프로 간다.
							break;
						}


						// 노트의 타입에 따른 색 결정
						switch( tempNote->getNoteType() )
						{
						case NOTE_T_RIGHT:
							newBrush.DeleteObject();
							newBrush.CreateSolidBrush(NC_RIGHT);
							break;

						case NOTE_T_LEFT:
							newBrush.DeleteObject();
							newBrush.CreateSolidBrush(NC_LEFT);
							break;

						case NOTE_T_LONG:
							newBrush.DeleteObject();
							newBrush.CreateSolidBrush(NC_LONG);
							break;

						case NOTE_T_PATTERN:
							newBrush.DeleteObject();
							newBrush.CreateSolidBrush(NC_PATTERN);
							break;

						case NOTE_T_CHARISMA:
							newBrush.DeleteObject();
							newBrush.CreateSolidBrush(NC_CHARISMA);
							break;

						case NOTE_T_NEUTRAL:
							newBrush.DeleteObject();
							newBrush.CreateSolidBrush(NC_NEUTRAL);
							break;

						case NOTE_T_DRAG:
							newBrush.DeleteObject();
							newBrush.CreateSolidBrush(NC_DRAG);
							break;

						case NOTE_T_BPM:
							newBrush.DeleteObject();
							newBrush.CreateSolidBrush(NC_BPM);
							break;

						case NOTE_T_PHOTO:
							newBrush.DeleteObject();
							newBrush.CreateSolidBrush(NC_PHOTO);
							break;

						default:
							newBrush.CreateSolidBrush(NC_UNKNOWN);
						}

						// 현재 브러시 내용을 받아온다.
						oldBrush = mdcOffScreen.SelectObject(&newBrush);
						// 						oldBrush = pDC->SelectObject(&newBrush);


						// 노트의 시간을 받아 온 다음,
						noteTime.noteTimeSec = tempNote->getNoteTimeSec();
						noteTime.noteTimeMilSec = tempNote->getNoteTimeMilSec();

						// 그 시간을 적절한 사각형의 좌표로 변경한다.
						newPoint.x = ConvertTimeToPixX(noteTime);
						newPoint.y = ConvertTypeToPixY(tempNote->getNoteType(), tempNote->getTargetMarker());

						if(
							tempNote->getNoteType() == NOTE_T_CHARISMA ||
							tempNote->getNoteType() == NOTE_T_NEUTRAL
							)
						{
							// 다른 노트와 겹칠 수 없는 장거리 노트의 경우,
							NoteTime noteEndTime;
							CPoint newPoint2;

							// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
							noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
							noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

							newPoint2.x = ConvertTimeToPixX(noteEndTime);
							newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.


							// 그릴 수 없는부분 출력
							brush.SetColor(BACK_COLOR_FOR_DRAG);
							graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_AFTER_CHAR / (double)1000)), V_LINE_TARG6_Y - V_NOTE_SET_OFF_Y);


							// 그림을 출력한다.
							mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
							mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
							mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

							// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
							// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
							// 					pDC->Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);
						}
						else if( tempNote->getNoteType() == NOTE_T_DRAG ||
							tempNote->getNoteType() == NOTE_T_PATTERN
							)
						{
							// 다른 노트와 겹칠 수 없는 장거리 노트의 경우,
							NoteTime noteEndTime;
							CPoint newPoint2;

							// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
							noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
							noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

							newPoint2.x = ConvertTimeToPixX(noteEndTime);
							newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.


							// 그릴 수 없는부분 출력
							brush.SetColor(BACK_COLOR_FOR_DRAG);
							graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_AFTER_DRAG / (double)1000)), V_LINE_TARG6_Y - V_NOTE_SET_OFF_Y);


							// 그림을 출력한다.
							mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
							mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
							mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

							// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
							// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
							// 					pDC->Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);
						}
						else if( tempNote->getNoteType() == NOTE_T_LONG )
						{
							// 롱노트의 경우.
							NoteTime noteEndTime;
							CPoint newPoint2;

							// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
							noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
							noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

							newPoint2.x = ConvertTimeToPixX(noteEndTime);
							newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.

							// 그릴 수 없는부분 출력
							brush.SetColor(BACK_COLOR_FOR_RIGHT);
							graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);

							// 그림을 출력한다.
							mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
							mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
							mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

						}	
						else if( tempNote->getNoteType() == NOTE_T_RIGHT )
						{
							// 오른손 노트의 경우
							// 그릴 수 없는부분 출력
							brush.SetColor(BACK_COLOR_FOR_RIGHT);
							graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);


							// 실제 사각형을 그린다.
							mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
						}	
						else if( tempNote->getNoteType() == NOTE_T_LEFT )
						{
							// 왼손 노트의 경우
							// 그릴 수 없는부분 출력
							brush.SetColor(BACK_COLOR_FOR_LEFT);
							graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);

							// 실제 사각형을 그린다.
							mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
						}
						else
						{
							// 실제 사각형을 그린다.
							mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
							// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
						}



						// 브러시 반환.
						mdcOffScreen.SelectObject(oldBrush);
						//pDC->SelectObject(oldBrush);
					}
				}
			}
			// 외곽선을 그린다.
		}
	}
	else if( editMode == EDIT_MODE_CONFG )
	{
		//////////// 상세 편집 모드일 경우

		// 선택된 노트가 있을 경우, 현재 선택중인 노트의 테두리를 그려준다.
		int noteConfigHeadDir = -1;
		bool onFocusFlag = false;
		CNoteData *tempNote = NULL;
		CNoteData *nowConfgNote = NULL;
		NoteTime tempTime;



		// 다른 노트들을 그려준다.
		for( i=0 ; i < MAX_NOTE_ARRAY ; i++ )
		{
			for( j=0 ; j < MAX_HALF_ARRAY ; j++ )
			{
				// 하나 받아 온다.
				tempNote = pDoc->getNoteAddr(i, j);

				if( tempNote == NULL )
				{	// 탐색이 끝나면 다음 루프로 간다.
					break;
				}


				// 노트의 타입에 따른 색 결정
				switch( tempNote->getNoteType() )
				{
				case NOTE_T_RIGHT:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_RIGHT);
					break;

				case NOTE_T_LEFT:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_LEFT);
					break;

				case NOTE_T_LONG:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_LONG);
					break;

				case NOTE_T_PATTERN:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_PATTERN);
					break;

				case NOTE_T_CHARISMA:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_CHARISMA);
					break;

				case NOTE_T_NEUTRAL:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_NEUTRAL);
					break;

				case NOTE_T_DRAG:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_DRAG);
					break;

				case NOTE_T_BPM:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_BPM);
					break;

				case NOTE_T_PHOTO:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_PHOTO);
					break;

				default:
					newBrush.CreateSolidBrush(NC_UNKNOWN);
				}

				// 현재 브러시 내용을 받아온다.
				oldBrush = mdcOffScreen.SelectObject(&newBrush);
				//oldBrush = pDC->SelectObject(&newBrush);


				// 노트의 시간을 받아 온 다음,
				noteTime.noteTimeSec = tempNote->getNoteTimeSec();
				noteTime.noteTimeMilSec = tempNote->getNoteTimeMilSec();

				// 그 시간을 적절한 사각형의 좌표로 변경한다.
				newPoint.x = ConvertTimeToPixX(noteTime);	
				newPoint.y = ConvertTypeToPixY(tempNote->getNoteType(), tempNote->getTargetMarker());

				if(
					tempNote->getNoteType() == NOTE_T_CHARISMA ||
					tempNote->getNoteType() == NOTE_T_NEUTRAL
					)
				{
					// 다른 노트와 겹칠 수 없는 장거리 노트의 경우,
					NoteTime noteEndTime;
					CPoint newPoint2;

					// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
					noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
					noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

					newPoint2.x = ConvertTimeToPixX(noteEndTime);
					newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.


					// 그릴 수 없는부분 출력
					brush.SetColor(BACK_COLOR_FOR_DRAG);
					graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_AFTER_CHAR / (double)1000)), V_LINE_TARG6_Y - V_NOTE_SET_OFF_Y);


					// 그림을 출력한다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

					// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
					// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					// 					pDC->Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);
				}
				else if( tempNote->getNoteType() == NOTE_T_DRAG ||
					tempNote->getNoteType() == NOTE_T_PATTERN
					)
				{
					// 다른 노트와 겹칠 수 없는 장거리 노트의 경우,
					NoteTime noteEndTime;
					CPoint newPoint2;

					// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
					noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
					noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

					newPoint2.x = ConvertTimeToPixX(noteEndTime);
					newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.


					// 그릴 수 없는부분 출력
					brush.SetColor(BACK_COLOR_FOR_DRAG);
					graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_AFTER_DRAG / (double)1000)), V_LINE_TARG6_Y - V_NOTE_SET_OFF_Y);


					// 그림을 출력한다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

					// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
					// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					// 					pDC->Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);
				}
				else if( tempNote->getNoteType() == NOTE_T_LONG )
				{
					// 롱노트의 경우.
					NoteTime noteEndTime;
					CPoint newPoint2;

					// 끝나는 지점의 시간을 적절한 좌표값으로 받아온다.
					noteEndTime.noteTimeSec = tempNote->getNoteEndTimeSec();
					noteEndTime.noteTimeMilSec = tempNote->getNoteEndTimeMilSec();

					newPoint2.x = ConvertTimeToPixX(noteEndTime);
					newPoint2.y = newPoint.y;							// y 축은 항상 같기 때문.

					// 그릴 수 없는부분 출력
					brush.SetColor(BACK_COLOR_FOR_RIGHT);
					graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, newPoint2.x - newPoint.x + (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);

					// 그림을 출력한다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint2.x+1, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					mdcOffScreen.Rectangle(newPoint2.x, newPoint2.y, newPoint2.x + V_NOTE_WIDTH, newPoint2.y + V_NOTE_HEIGHT);

				}	
				else if( tempNote->getNoteType() == NOTE_T_RIGHT )
				{
					// 오른손 노트의 경우
					// 그릴 수 없는부분 출력
					brush.SetColor(BACK_COLOR_FOR_RIGHT);
					graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);


					// 실제 사각형을 그린다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
				}	
				else if( tempNote->getNoteType() == NOTE_T_LEFT )
				{
					// 왼손 노트의 경우
					// 그릴 수 없는부분 출력
					brush.SetColor(BACK_COLOR_FOR_LEFT);
					graphics.FillRectangle(&brush, newPoint.x, V_NOTE_SET_OFF_Y, (int)(V_1SEC_PIXEL_X * (MIN_NOTE_END_INTERVAL_SAME_R / (double)1000)), V_LINE_SPEC_Y - V_NOTE_SET_OFF_Y);

					// 실제 사각형을 그린다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
				}
				else
				{
					// 실제 사각형을 그린다.
					mdcOffScreen.Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					// 					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
				}

				mdcOffScreen.SelectObject(oldBrush);
				//pDC->SelectObject(oldBrush);

			}

			nowConfgNote = pDoc->getNowConfigingNote(noteConfigHeadDir);
			onFocusFlag = pDoc->getOnFocusEditingFlag();



			if( nowConfgNote != NULL && onFocusFlag == true )
			{
				if ( noteConfigHeadDir == 0 )
				{
					// 시간 받아 온 다음
					tempTime.noteTimeSec = nowConfgNote->getNoteTimeSec();
					tempTime.noteTimeMilSec = nowConfgNote->getNoteTimeMilSec();

					// 적절한 좌표로 변환
					newPoint.x = ConvertTimeToPixX(tempTime);
					newPoint.y = ConvertTypeToPixY(nowConfgNote->getNoteType(), nowConfgNote->getTargetMarker());

					// 스크롤된 좌표로 보정 (더블 버퍼링 사용으로 인해 사용하지 않는다.)
					// newPoint -= scrollPos;

					// 실제 사각형 그리기.
					CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
					l_my_pen.CreatePen(PS_SOLID, 2, RGB(163, 73, 164));
					old_pen = mdcOffScreen.SelectObject(&l_my_pen);
					//old_pen = pDC->SelectObject(&l_my_pen);

					mdcOffScreen.SelectObject(&l_my_pen);

					mdcOffScreen.MoveTo( newPoint.x-2, newPoint.y-1 );
					mdcOffScreen.LineTo( newPoint.x + V_NOTE_WIDTH + 1, newPoint.y-1 );
					mdcOffScreen.LineTo( newPoint.x + V_NOTE_WIDTH + 1, newPoint.y + V_NOTE_HEIGHT + 1 );
					mdcOffScreen.LineTo( newPoint.x-1, newPoint.y + V_NOTE_HEIGHT + 1 );
					mdcOffScreen.LineTo( newPoint.x-1, newPoint.y - 2 );

					mdcOffScreen.SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.

					// 					pDC->SelectObject(&l_my_pen);
					// 
					// 					pDC->MoveTo( newPoint.x-2, newPoint.y-1 );
					// 					pDC->LineTo( newPoint.x + V_NOTE_WIDTH + 1, newPoint.y-1 );
					// 					pDC->LineTo( newPoint.x + V_NOTE_WIDTH + 1, newPoint.y + V_NOTE_HEIGHT + 1 );
					// 					pDC->LineTo( newPoint.x-1, newPoint.y + V_NOTE_HEIGHT + 1 );
					// 					pDC->LineTo( newPoint.x-1, newPoint.y - 2 );
					// 
					// 					pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
					l_my_pen.DeleteObject();

				}
				else if ( noteConfigHeadDir == 1 )
				{
					// 시간 받아 온 다음
					tempTime.noteTimeSec = nowConfgNote->getNoteEndTimeSec();
					tempTime.noteTimeMilSec = nowConfgNote->getNoteEndTimeMilSec();

					// 적절한 좌표로 변환
					newPoint.x = ConvertTimeToPixX(tempTime);
					newPoint.y = ConvertTypeToPixY(nowConfgNote->getNoteType(), nowConfgNote->getTargetMarker());

					// 스크롤된 좌표로 보정 (더블 버퍼링 사용으로 인해 사용하지 않는다.)
					//newPoint -= scrollPos;

					// 실제 사각형 그리기.
					CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
					l_my_pen.CreatePen(PS_SOLID, 2, RGB(163, 73, 164));
					old_pen = mdcOffScreen.SelectObject(&l_my_pen);

					mdcOffScreen.SelectObject(&l_my_pen);

					mdcOffScreen.MoveTo( newPoint.x-2, newPoint.y-1 );
					mdcOffScreen.LineTo( newPoint.x + V_NOTE_WIDTH + 1, newPoint.y-1 );
					mdcOffScreen.LineTo( newPoint.x + V_NOTE_WIDTH + 1, newPoint.y + V_NOTE_HEIGHT + 1 );
					mdcOffScreen.LineTo( newPoint.x-1, newPoint.y + V_NOTE_HEIGHT + 1 );
					mdcOffScreen.LineTo( newPoint.x-1, newPoint.y - 2 );

					mdcOffScreen.SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.

					//old_pen = pDC->SelectObject(&l_my_pen);

					//pDC->SelectObject(&l_my_pen);

					//pDC->MoveTo( newPoint.x-2, newPoint.y-1 );
					//pDC->LineTo( newPoint.x + V_NOTE_WIDTH + 1, newPoint.y-1 );
					//pDC->LineTo( newPoint.x + V_NOTE_WIDTH + 1, newPoint.y + V_NOTE_HEIGHT + 1 );
					//pDC->LineTo( newPoint.x-1, newPoint.y + V_NOTE_HEIGHT + 1 );
					//pDC->LineTo( newPoint.x-1, newPoint.y - 2 );

					//pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
					l_my_pen.DeleteObject();

				}
			}



		}

	}
	else
	{
		MessageBox(_T("Draw에서 Mode의 치명적 예외"));
	}
	
	

	mdcOffScreen.BitBlt(0, 0, WIDTH, FRAME_1_HEIGHT, &timeLineDC, 0, 0, SRCCOPY);


	// 뮤텍스 푼다.
	mutexFlag = true;

}



void CNotePickingView::OnDraw(CDC* pDC)
{
	CNoteEditingToolDoc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);
	if (!pDoc)
		return;
	// TODO: 여기에 그리기 코드를 추가합니다.

	CDC mdcOffScreen;			// 더블버퍼링을 위한 메모리 그림버퍼
	int nVertScroll = GetScrollPos(SB_VERT);	// 세로
	int nHorzScroll = GetScrollPos(SB_HORZ);	// 가로
	CPoint scrollPos = CPoint(nHorzScroll, nVertScroll);


	CRect rect;
	GetClientRect(&rect);

	// 비트맵 출력
	mdcOffScreen.CreateCompatibleDC(pDC);
	mdcOffScreen.SelectObject(&bmpOffScreen);


	// 뮤텍스 건다.
	mutexFlag = false;
	// 마지막에 그린다.
	pDC->BitBlt(0, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X, FRAME_1_HEIGHT, &mdcOffScreen, 0, 0, SRCCOPY);

	// 뮤텍스 푼다.
	mutexFlag = true;
	
	//DrawTimeLineEx(pDC->m_hDC, BASS_ChannelGetPosition(chan, BASS_POS_BYTE ), 0xffffff, 0, scrollPos); // current pos
	DrawTimeLine(pDC->m_hDC, BASS_ChannelGetPosition(chan, BASS_POS_BYTE ), 0xffffff, 0); // current pos

	ReleaseDC(pDC);

}


//////////////////////////////////////////////////////////////////////////////
//////////////////////////////   [Protected]    //////////////////////////////
//////////////////////////////////////////////////////////////////////////////


// 현재 마우스의 위치에 맞는 노트의 적절한 위치를 리턴하는 함수.
CPoint CNotePickingView::getNoteReviPos(CPoint point, char &drawOkFlag)
{
	CNoteEditingToolDoc *pDoc = GetDocument();


	if( point.x <= V_NOTE_SET_OFF_X )
	{
		drawOkFlag = 0;
	}
	else
	{
		long drawLevelY = (point.y - (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL)) / (V_NOTE_HEIGHT + V_NOTE_INTVAL);		// 사각형을 그릴 단계, (위에서부터 0, 1, 2, ....)


		switch( drawLevelY )
		{
			// 일반 노트의 위치에 그렸을 경우.
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
			// 현재 그리려는 노트 타입에 맞는지 확인.
			if( pDoc->getNoteWriteType() == NOTE_T_RIGHT ||
				pDoc->getNoteWriteType() == NOTE_T_LEFT ||
				pDoc->getNoteWriteType() == NOTE_T_LONG
				)
			{
				point.y = drawLevelY * (V_NOTE_HEIGHT + V_NOTE_INTVAL) + (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL);
				drawOkFlag = 'N';
			}
			break;

			// 특수 노트 위치에 그렸을 경우.
		case 6:
			if( pDoc->getNoteWriteType() == NOTE_T_PATTERN ||
				pDoc->getNoteWriteType() == NOTE_T_CHARISMA ||
				pDoc->getNoteWriteType() == NOTE_T_NEUTRAL ||
				pDoc->getNoteWriteType() == NOTE_T_DRAG 
				)
			{
				point.y = drawLevelY * (V_NOTE_HEIGHT + V_NOTE_INTVAL) + (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL);
				drawOkFlag = 'S';
			}
			break;

			// BPM 변환 노트에 그렸을 경우.
		case 7:
			if( pDoc->getNoteWriteType() == NOTE_T_BPM ||
				pDoc->getNoteWriteType() == NOTE_T_PHOTO
				)
			{
				point.y = drawLevelY * (V_NOTE_HEIGHT + V_NOTE_INTVAL) + (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL);
				drawOkFlag = 'B';
			}
			break;

		default:
			drawOkFlag = 0;
		}

	}

	return point;
}




CPoint CNotePickingView::getNoteReviPos(CPoint point, char &drawOkFlag, const CPoint scrollPoint)
{
	CNoteEditingToolDoc *pDoc = GetDocument();


	//drawOkFlag = 0;

	// 먼저 가로 방향 여백에 마우스가 있는지 확인.
	if( (point.x - scrollPoint.x) <= V_NOTE_SET_OFF_X )
	{
		drawOkFlag = 0;
	}
	else
	{
		long drawLevelY = (point.y - scrollPoint.y - (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL)) / (V_NOTE_HEIGHT + V_NOTE_INTVAL);		// 사각형을 그릴 단계, (위에서부터 0, 1, 2, ....)

		switch( drawLevelY )
		{
			// 일반 노트의 위치에 그렸을 경우.
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
			// 현재 그리려는 노트 타입에 맞는지 확인.
			if( pDoc->getNoteWriteType() == NOTE_T_RIGHT ||
				pDoc->getNoteWriteType() == NOTE_T_LEFT ||
				pDoc->getNoteWriteType() == NOTE_T_LONG
				)
			{
				point.y = drawLevelY * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
				drawOkFlag = 'N';
			}
			else
			{
				drawOkFlag = 0;
			}
			break;


		case 6:
			// 특수 노트 위치에 그렸을 경우.
			if( pDoc->getNoteWriteType() == NOTE_T_PATTERN ||
				pDoc->getNoteWriteType() == NOTE_T_CHARISMA ||
				pDoc->getNoteWriteType() == NOTE_T_NEUTRAL ||
				pDoc->getNoteWriteType() == NOTE_T_DRAG 
				)
			{
				point.y = drawLevelY * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
				drawOkFlag = 'S';
			}
			break;

			// BPM 변환 노트에 그렸을 경우.
		case 7:
			if( pDoc->getNoteWriteType() == NOTE_T_BPM ||
				pDoc->getNoteWriteType() == NOTE_T_PHOTO
				)
			{
				point.y = drawLevelY * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
				drawOkFlag = 'B';
			}
			break;

		default:
			drawOkFlag = 0;
		}

	}




	return point;
}



CPoint CNotePickingView::getNoteReviPosRanType(CPoint point, char &drawOkFlag)
{

	CNoteEditingToolDoc *pDoc = GetDocument();


	if( point.x <= (LONG)(V_NOTE_SET_OFF_X) )
	{
		drawOkFlag = 0;
	}
	else
	{
		long drawLevelY = (point.y - (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL)) / (V_NOTE_HEIGHT + V_NOTE_INTVAL);		// 사각형을 그릴 단계, (위에서부터 0, 1, 2, ....)

		switch( drawLevelY )
		{
			// 일반 노트의 위치에 그렸을 경우.
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
			// 현재 그리려는 노트 타입에 맞는지 확인.
			//if( pDoc->getNoteWriteType() == NOTE_T_RIGHT ||
			//	pDoc->getNoteWriteType() == NOTE_T_LEFT ||
			//	pDoc->getNoteWriteType() == NOTE_T_LONG
			//	)
			//{
			point.y = drawLevelY * (V_NOTE_HEIGHT + V_NOTE_INTVAL) + (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL);
			drawOkFlag = 'N';
			//}
			break;

			// 특수 노트 위치에 그렸을 경우.
		case 6:
			//if( pDoc->getNoteWriteType() == NOTE_T_PATTERN ||
			//	pDoc->getNoteWriteType() == NOTE_T_CHARISMA ||
			//	pDoc->getNoteWriteType() == NOTE_T_NEUTRAL ||
			//	pDoc->getNoteWriteType() == NOTE_T_DRAG 
			//	)
			//{
			point.y = drawLevelY * (V_NOTE_HEIGHT + V_NOTE_INTVAL) + (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL);
			drawOkFlag = 'S';
			//}
			break;

			// BPM 변환 노트에 그렸을 경우.
		case 7:
			//if( pDoc->getNoteWriteType() == NOTE_T_BPM ||
			//	pDoc->getNoteWriteType() == NOTE_T_PHOTO
			//	)
			//{
			point.y = drawLevelY * (V_NOTE_HEIGHT + V_NOTE_INTVAL) + (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL);
			drawOkFlag = 'B';
			//}
			break;

		default:
			drawOkFlag = 0;
		}

	}

	return point;
}


// 현재 마우스의 x축 값을 적절한 시간 값으로 바꿔주는 함수.
NoteTime CNotePickingView::ConvertPixToTime(CPoint point)
{
	NoteTime retTime;
	double timePos;
	
	
	if ( chan )
	{
		// 현재 음악이 로드되어 있는 상태라면,
		timePos = BASS_ChannelBytes2Seconds(chan, (QWORD)((point.x - V_NOTE_SET_OFF_X) * bpp) );

		retTime.noteTimeSec = (unsigned int)timePos;
		retTime.noteTimeMilSec = (unsigned int)( timePos * 1000 ) % 1000 ;
	}
	else
	{
		// 음악이 아직 로드되지 않았다면,
		unsigned long ConvrTime = (unsigned long)((point.x - V_NOTE_SET_OFF_X) * 1000 / V_1SEC_PIXEL_X);
		// 1000을 곱하는 이유는 V_1SEC_PIXEL_X가 1초를 기준으로 되어 있기 때문이다.

		retTime.noteTimeSec = ConvrTime / 1000;
		retTime.noteTimeMilSec = ConvrTime % 1000;
	}
	
	
	
//	QWORD tempPixelPerSec = BASS_ChannelSeconds2Bytes(chan, timePos) / bpp;



	return retTime;
}


// 시간 값을 적절한 x축 픽셀값으로 바꿔주는 함수
unsigned long CNotePickingView::ConvertTimeToPixX(NoteTime nTime)
{
	double timePos = (double)(nTime.noteTimeSec) + (nTime.noteTimeMilSec / (double)(1000));
	unsigned long retPixel;

	if ( chan )
	{
		// 만약 음악이 로드되어 있다면,
		QWORD tempPixelPerSec = BASS_ChannelSeconds2Bytes(chan, timePos) / bpp;
		retPixel = (unsigned long)(tempPixelPerSec);
		retPixel += V_NOTE_SET_OFF_X;
	}
	else
	{
		// 만약 음악이 로드되어 있지않다면,
		retPixel = (nTime.noteTimeSec * 1000) + (nTime.noteTimeMilSec);
		retPixel = (unsigned int)(retPixel * V_1SEC_PIXEL_X / 1000 );
		retPixel += V_NOTE_SET_OFF_X;

	}
	


	return retPixel;
}

// 각 타입에 맞는 y축 픽셀값으로 바꿔주는 함수.
unsigned long CNotePickingView::ConvertTypeToPixY(const char noteType, const char targetMarker)
{
	unsigned long retYPixel=0;

	switch( noteType )
	{
		// 1~6 일반 타입.
	case NOTE_T_RIGHT:
	case NOTE_T_LEFT:
	case NOTE_T_LONG:
		switch( targetMarker )
		{
		case MARKER_NUM1:
			retYPixel = 0 * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
			break;
		case MARKER_NUM2:
			retYPixel = 1 * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
			break;
		case MARKER_NUM3:
			retYPixel = 2 * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
			break;
		case MARKER_NUM4:
			retYPixel = 3 * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
			break;
		case MARKER_NUM5:
			retYPixel = 4 * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
			break;
		case MARKER_NUM6:
			retYPixel = 5 * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
			break;

		default:
			retYPixel = 0;
		}
		break;

	case NOTE_T_PATTERN:
	case NOTE_T_CHARISMA:
	case NOTE_T_NEUTRAL:
	case NOTE_T_DRAG:
		retYPixel = 6 * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
		break;

	case NOTE_T_BPM:
	case NOTE_T_PHOTO:
		retYPixel = 7 * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
		break;
	}

	return retYPixel;
}



// 픽셀 값을 적절한 노트 기본 정보로 바꾸는 함수.
int CNotePickingView::ConvertPixToNoteData(const CPoint point, NoteTime &time, char &noteType, char &targetMarker)
{
	CNoteEditingToolDoc *pDoc = GetDocument();

	CPoint tempPoint;
	char drawOkFlag = 0;

	// 보정된 노트위치.
	tempPoint = getNoteReviPos(point, drawOkFlag);

	if( drawOkFlag == 0 )
	{
		// 그리면 안된다는 결과가 나왔을 때,
		return -1;
	}

	// 시간값 Get.
	// 연산자 오버로딩에 문제가 생겨 일을 2번 함.
	time.noteTimeSec = ConvertPixToTime(tempPoint).noteTimeSec;
	time.noteTimeMilSec = ConvertPixToTime(tempPoint).noteTimeMilSec;

	// 노트 타입은 Doc에서 따로 함수를 두어서 관리
	//noteType = NOTE_T_RIGHT;		// 임시로 지정.
	noteType = pDoc->getNoteWriteType();


	// 노트 타입에 맞는 y축 위치인지 검사.
	switch( noteType )
	{
	case NOTE_T_RIGHT:
	case NOTE_T_LEFT:
	case NOTE_T_LONG:
		// 타겟 마커를 획득한다.
		switch( (tempPoint.y - (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL) ) / (V_NOTE_HEIGHT + V_NOTE_INTVAL) )
		{
		case 0:
			targetMarker = MARKER_NUM1;
			break;
		case 1:
			targetMarker = MARKER_NUM2;
			break;
		case 2:
			targetMarker = MARKER_NUM3;
			break;
		case 3:
			targetMarker = MARKER_NUM4;
			break;
		case 4:
			targetMarker = MARKER_NUM5;
			break;
		case 5:
			targetMarker = MARKER_NUM6;
			break;

		default:
			// 혹시나 이상한 값이 들어왔을 경우.
			return -1;
		}
		break;

	case NOTE_T_PATTERN:
	case NOTE_T_CHARISMA:
	case NOTE_T_NEUTRAL:
	case NOTE_T_DRAG:
		if( tempPoint.y < V_SPEC_NOTE_Y || tempPoint.y > V_SPEC_NOTE_Y_END)
		{
			return -1;
		}
		targetMarker = '1';
		break;

	case NOTE_T_BPM:
		targetMarker = 120;
		break;

	case NOTE_T_PHOTO:
		//	retYPixel = 7 * (V_NOTE_HEIGHT+V_NOTE_INTVAL) + V_NOTE_SET_OFF_Y + V_NOTE_INTVAL;
		targetMarker = '1';
		break;
	}

	return 1;

}



// 경우에 따라서 타입을 지정 해 주는 방식으로 픽셀 값을 적절한 노트 기본 정보로 바꾸는 함수.
int CNotePickingView::ConvertPixToNoteDataRanType(const CPoint point, NoteTime &time, char &noteBigType, char &targetMarker)
{
	CNoteEditingToolDoc *pDoc = GetDocument();

	CPoint tempPoint;
	char drawOkFlag = 0;

	// 보정된 노트위치.
	tempPoint = getNoteReviPosRanType(point, drawOkFlag);

	if( drawOkFlag == 0 )
	{
		// 그리면 안된다는 결과가 나왔을 때,
		return -1;
	}

	// 시간값 Get.
	// 연산자 오버로딩에 문제가 생겨 일을 2번 함.
	time.noteTimeSec = ConvertPixToTime(tempPoint).noteTimeSec;
	time.noteTimeMilSec = ConvertPixToTime(tempPoint).noteTimeMilSec;

	// 노트 타입은 Doc에서 따로 함수를 두어서 관리
	//noteType = NOTE_T_RIGHT;		// 임시로 지정.
	//noteType = pDoc->getNoteWriteType();


	// 노트 타입에 맞는 y축 위치인지 검사.
	switch( drawOkFlag )
	{
	case NOTE_TYPE_LINE_1:
		// 타겟 마커를 획득한다.
		switch( (tempPoint.y - (V_NOTE_SET_OFF_Y + V_NOTE_INTVAL) ) / (V_NOTE_HEIGHT + V_NOTE_INTVAL) )
		{
		case 0:
			noteBigType = NOTE_TYPE_LINE_1;
			targetMarker = MARKER_NUM1;
			break;
		case 1:
			noteBigType = NOTE_TYPE_LINE_1;
			targetMarker = MARKER_NUM2;
			break;
		case 2:
			noteBigType = NOTE_TYPE_LINE_1;
			targetMarker = MARKER_NUM3;
			break;
		case 3:
			noteBigType = NOTE_TYPE_LINE_1;
			targetMarker = MARKER_NUM4;
			break;
		case 4:
			noteBigType = NOTE_TYPE_LINE_1;
			targetMarker = MARKER_NUM5;
			break;
		case 5:
			noteBigType = NOTE_TYPE_LINE_1;
			targetMarker = MARKER_NUM6;
			break;

		default:
			// 혹시나 이상한 값이 들어왔을 경우.
			return -1;
		}
		break;

	case NOTE_TYPE_LINE_2:
		if( tempPoint.y < V_LINE_TARG6_Y || tempPoint.y > V_SPEC_NOTE_Y)
		{
			return -1;
		}
		noteBigType = NOTE_TYPE_LINE_2;
		targetMarker = 0;
		break;

	case NOTE_TYPE_LINE_3:
		if( tempPoint.y < V_SPEC_NOTE_Y_END || tempPoint.y > V_LINE_Y_END)
		{
			return -1;
		}
		noteBigType = NOTE_TYPE_LINE_3;
		targetMarker = 0;
		break;

	default:
		// 그 외의 상황일 경우,
		return -1;
	}

	return 1;

}


// 배경 선 그리기
int CNotePickingView::drawBackgroundLines(CDC *pDC)
{
	// CDC 객체를 HDC 객체로 변환
	HDC hdc = ::GetDC(this->m_hWnd);

	CString tempCString;			// 숫자 출력용.

	//int endOfX = 5000;

	QWORD tempPixelPerSec;
	//QWORD tempByte = BASS_ChannelSeconds2Bytes(chan, 100);
	//tempByte /= bpp;
	//tempByte -= V_NOTE_SET_OFF_X;


	//CClientDC dc(this); // 그림을 그리기 위해 CDC 계열의 DC 객체를 생성한다.
	CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 

	l_my_pen.CreatePen(PS_SOLID,3,RGB(0,0,0));
	old_pen = pDC->SelectObject(&l_my_pen);

	//////////////////////////////////////////////////////////////////////////
	// 여기서부터 l_my_pen의 속성을 적용 받는다.
	//////////////////////////////////////////////////////////////////////////


	// 제일 가는 가로선 그리기.
	l_my_pen.DeleteObject();
	l_my_pen.CreatePen(PS_SOLID, 1, RGB(205,215,208));
	pDC->SelectObject(&l_my_pen);

	//pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG1_Y );
	//pDC->LineTo(V_NOTE_SET_OFF_X + endOfX, V_LINE_TARG1_Y );
	//pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG2_Y );
	//pDC->LineTo(V_NOTE_SET_OFF_X+endOfX, V_LINE_TARG2_Y );
	//pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG3_Y );
	//pDC->LineTo(V_NOTE_SET_OFF_X+endOfX, V_LINE_TARG3_Y );
	//pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG4_Y );
	//pDC->LineTo(V_NOTE_SET_OFF_X+endOfX, V_LINE_TARG4_Y );
	//pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG5_Y );
	//pDC->LineTo(V_NOTE_SET_OFF_X+endOfX, V_LINE_TARG5_Y );
	//pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_SPEC_Y );
	//pDC->LineTo(V_NOTE_SET_OFF_X+endOfX, V_LINE_SPEC_Y );

	pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG1_Y );
	pDC->LineTo(V_NOTE_SET_OFF_X + (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), V_LINE_TARG1_Y );
	pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG2_Y );
	pDC->LineTo(V_NOTE_SET_OFF_X + (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), V_LINE_TARG2_Y );
	pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG3_Y );
	pDC->LineTo(V_NOTE_SET_OFF_X + (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), V_LINE_TARG3_Y );
	pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG4_Y );
	pDC->LineTo(V_NOTE_SET_OFF_X + (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), V_LINE_TARG4_Y );
	pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG5_Y );
	pDC->LineTo(V_NOTE_SET_OFF_X + (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), V_LINE_TARG5_Y );
	pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_SPEC_Y );
	pDC->LineTo(V_NOTE_SET_OFF_X + (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), V_LINE_SPEC_Y );


	// 제일 가는 세로선 (1초, 5초 마다?) 그리기.
	l_my_pen.DeleteObject();
	l_my_pen.CreatePen(PS_SOLID, 1, RGB(105,125,115));
	pDC->SelectObject(&l_my_pen);

	if ( chan )
	{
		// 정의 된 채널이 있을 경우
		for( int i=0 ; i<=(int)(WIDTH * BSS_WAVE_FORM_RATE_REAL / V_1SEC_PIXEL_X) ; i++ )
		{
			// 계속해서 47Pixel/Sec로 조정하다 보면, 나중에 오차가 발생하기 때문에, 이와 같이 계속적으로 보정한다.
			tempPixelPerSec = BASS_ChannelSeconds2Bytes(chan, i) / bpp;
			pDC->MoveTo((int)tempPixelPerSec + V_NOTE_SET_OFF_X, V_NOTE_SET_OFF_Y + V_LINE_INTERVAL );
			pDC->LineTo((int)tempPixelPerSec + V_NOTE_SET_OFF_X, V_LINE_Y_END );
		}
	}
	else
	{
		// 채널에 아무것도 없을 경우,
		for( int i=0 ; i<=(int)(WIDTH * BSS_WAVE_FORM_RATE_REAL / V_1SEC_PIXEL_X) ; i++ )
		{
			pDC->MoveTo(V_1SEC_PIXEL_X*i + V_NOTE_SET_OFF_X, V_NOTE_SET_OFF_Y + V_LINE_INTERVAL );
			pDC->LineTo(V_1SEC_PIXEL_X*i + V_NOTE_SET_OFF_X, V_LINE_Y_END );
		}
	}

	// 5초마다 그리는 선
	
	// LOGFONT로부터 글꼴을 생성
	CFont newFont, *pOldFont;
	newFont.CreateFontIndirect(&m_logFont);
	// 생성된 글꼴을 DC에 선택
	pOldFont = (CFont *)pDC->SelectObject(&newFont);
	// 텍스트의 전경색과 배경색 설정
	pDC->SetTextColor(m_colorText);

	// 배경 모드를 설정
	if(m_bTransparent)
	{
		pDC->SetBkMode(TRANSPARENT);
	}
	else
	{
		pDC->SetBkMode(OPAQUE);
	}



	l_my_pen.DeleteObject();
	l_my_pen.CreatePen(PS_SOLID, 1, RGB(205,215,208));
	pDC->SelectObject(&l_my_pen);


	if ( chan )
	{
		// 정의 된 채널이 있을 경우
		for( int i=0 ; i<=(int)(WIDTH * BSS_WAVE_FORM_RATE_REAL / V_1SEC_PIXEL_X) ; i++ )
		{
			// 계속해서 47Pixel/Sec로 조정하다 보면, 나중에 오차가 발생하기 때문에, 이와 같이 계속적으로 보정한다.
			tempPixelPerSec = BASS_ChannelSeconds2Bytes(chan, i*5) / bpp;
			pDC->MoveTo((int)tempPixelPerSec + V_NOTE_SET_OFF_X, V_NOTE_SET_OFF_Y + V_LINE_INTERVAL );
			pDC->LineTo((int)tempPixelPerSec + V_NOTE_SET_OFF_X, V_LINE_Y_END );

			// 텍스트
			//SetTextColor(hdc, RGB(240, 240, 240));
			tempCString.Format(_T("%2u:%02u"), (i*5/60), (i*5)%60);
			pDC->TextOutW((int)tempPixelPerSec + V_NOTE_SET_OFF_X, V_LINE_Y_END, tempCString);
		}
	}
	else
	{
		for( int i=1 ; i<=(int)(WIDTH * BSS_WAVE_FORM_RATE_REAL / (V_1SEC_PIXEL_X*5)) ; i++ )
		{
			pDC->MoveTo(V_1SEC_PIXEL_X*5*i + V_NOTE_SET_OFF_X, V_NOTE_SET_OFF_Y );
			pDC->LineTo(V_1SEC_PIXEL_X*5*i + V_NOTE_SET_OFF_X, V_LINE_Y_END );

			// 텍스트
			//SetTextColor(hdc, RGB(240, 240, 240));
			tempCString.Format(_T("%2u:%02u"), (i*5/60), (i*5)%60);
			pDC->TextOutW(V_1SEC_PIXEL_X*5*i + V_NOTE_SET_OFF_X, V_LINE_Y_END, tempCString);
			//SetBkMode(hdc,TRANSPARENT);
			//SetTextAlign( hdc, V_1SEC_PIXEL_X*5*i>=WIDTH/2 ? TA_RIGHT : TA_LEFT );
			//::TextOut( hdc, V_1SEC_PIXEL_X*5*i + V_NOTE_SET_OFF_X - scrollPos.x, y, timeText, _tcslen(timeText) );
		}
	}

	pDC->SelectObject(pOldFont);
	newFont.DeleteObject();




	// 제일 굵은 외곽선 그리기.
	l_my_pen.DeleteObject();
	l_my_pen.CreatePen(PS_SOLID,3,RGB(255,255,255));
	pDC->SelectObject(&l_my_pen);

	pDC->MoveTo(V_NOTE_SET_OFF_X, 0);
	pDC->LineTo(V_NOTE_SET_OFF_X, V_LINE_Y_END);

	pDC->MoveTo(V_NOTE_SET_OFF_X, V_NOTE_SET_OFF_Y+V_LINE_INTERVAL );
	pDC->LineTo(V_NOTE_SET_OFF_X + (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), V_NOTE_SET_OFF_Y+V_LINE_INTERVAL );

	pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_Y_END );
	pDC->LineTo(V_NOTE_SET_OFF_X + (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), V_LINE_Y_END );



	// 조금 얇은 선 그리기.
	l_my_pen.DeleteObject();
	l_my_pen.CreatePen(PS_SOLID,2,RGB(255,255,255));
	pDC->SelectObject(&l_my_pen);

	pDC->MoveTo(V_NOTE_SET_OFF_X, V_LINE_TARG6_Y );
	pDC->LineTo(V_NOTE_SET_OFF_X + (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL), V_LINE_TARG6_Y );



	//////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////
	pDC->SelectObject(old_pen);//기존에 사용하던 pen속성으로복구됨.

	return 1;
}



// 대략 어떤 종류의 노트인지를 확인한다.
char CNotePickingView::chkTypeToBigType(const char noteType)
{
	switch( noteType )
	{	
	case NOTE_T_RIGHT:
	case NOTE_T_LEFT:
	case NOTE_T_LONG:
		return NOTE_TYPE_LINE_1;
		break;

	case NOTE_T_PATTERN:
	case NOTE_T_CHARISMA:
	case NOTE_T_NEUTRAL:
	case NOTE_T_DRAG:
		return NOTE_TYPE_LINE_2;
		break;

	case NOTE_T_BPM:
	case NOTE_T_PHOTO:
		return NOTE_TYPE_LINE_3;
		break;
	}

	return '/';
}

// 현재 그리려는 노트와 중첩되면 안 되는 노트가 있는 지 확인한다.
bool CNotePickingView::chkIsNoteDrawOk(CNoteData *oldNotePtr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, const char noteType, const char TargetMarker)
{
	CNoteEditingToolDoc *pDoc = GetDocument();

	// 생성 할 노트의 시간
	unsigned long newNoteTimeLong = (unsigned long)((noteTimeSec * 1000) + noteTimeMilSec);
	unsigned long newNoteEndTimeLong = newNoteTimeLong;
	// 비교 할 노트의 시작과 끝 시간
	unsigned long compNoteStartTimeLong = 0;
	unsigned long compNoteEndTimeLong = 0;

	unsigned long newNoteTimeInterval = 0;			// 노트 타입에 따라서 어느 정도 인터벌을 줄 것이지 설정.

	bool isNewNoteRight = true;						// 새 노트가 오른손일 경우 T
	bool isNewNoteEx = false;						// 새 노트가 Drag노트 등의 특수 노트일 경우 T
	bool isCompNoteRight = true;						// 비교 할 노트가 오른손일 경우 T
	bool isCompNoteEx = false;						// 비교 할 노트가 Drag노트 등의 특수 노트일 경우 T

	int i=0, j=0;
	CNoteData *tempNote = NULL;

	// 먼저 그리려는 노트의 타입부터 확인
	switch( noteType )
	{
	// 단일 노트의 경우에는 똑같이 그대로 간다.
	case NOTE_T_RIGHT:
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
		break;


	case NOTE_T_LEFT:
		// 적절한 인터벌을 준다.
		isNewNoteRight = false;
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
		break;

	case NOTE_T_BPM:
	case NOTE_T_PHOTO:
		// 검사 할 필요가 없는 노트이므로 통과
		return true;
		break;

	case NOTE_T_LONG:
		if ( oldNotePtr == NULL )
		{
			// 새로 노트를 만드는 것일 때
			newNoteEndTimeLong += (unsigned long)( (DEFAULT_SEC_RANGE * 1000) + DEFAULT_MILSEC_RANGE );
		}
		else
		{
			// 수정하는 중일 때
			newNoteEndTimeLong = (unsigned long)( (oldNotePtr->getNoteEndTimeSec() * 1000) + oldNotePtr->getNoteEndTimeMilSec() );
		}
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
		break;


	case NOTE_T_CHARISMA:
		isNewNoteEx = true;
		if ( oldNotePtr == NULL )
		{
			// 새로 노트를 만드는 것일 때
			newNoteEndTimeLong += (unsigned long)( (DEFAULT_SEC_RANGE * 1000) + DEFAULT_MILSEC_RANGE );
		}
		else
		{
			// 수정하는 중일 때
			newNoteEndTimeLong = (unsigned long)( (oldNotePtr->getNoteEndTimeSec() * 1000) + oldNotePtr->getNoteEndTimeMilSec() );
		}
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_CHAR;
		break;


	case NOTE_T_NEUTRAL:
		isNewNoteEx = true;
		if ( oldNotePtr == NULL )
		{
			// 새로 노트를 만드는 것일 때
			newNoteEndTimeLong += (unsigned long)( (DEFAULT_SEC_RANGE * 1000) + DEFAULT_MILSEC_RANGE );
		}
		else
		{
			// 수정하는 중일 때
			newNoteEndTimeLong = (unsigned long)( (oldNotePtr->getNoteEndTimeSec() * 1000) + oldNotePtr->getNoteEndTimeMilSec() );
		}
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_NEUTRAL;
		break;


	case NOTE_T_PATTERN:
		isNewNoteEx = true;
		if ( oldNotePtr == NULL )
		{
			// 새로 노트를 만드는 것일 때
			newNoteEndTimeLong += (unsigned long)( (DEFAULT_SEC_RANGE * 1000) + DEFAULT_MILSEC_RANGE );
		}
		else
		{
			// 수정하는 중일 때
			newNoteEndTimeLong = (unsigned long)( (oldNotePtr->getNoteEndTimeSec() * 1000) + oldNotePtr->getNoteEndTimeMilSec() );
		}
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_PATTERN;
		break;


	case NOTE_T_DRAG:
		isNewNoteEx = true;
		if ( oldNotePtr == NULL )
		{
			// 새로 노트를 만드는 것일 때
			newNoteEndTimeLong += (unsigned long)( (DEFAULT_DRAG_SEC * 1000) + DEFAULT_DRAG_MILSEC );
		}
		else
		{
			// 수정하는 중일 때
			newNoteEndTimeLong = (unsigned long)( (oldNotePtr->getNoteEndTimeSec() * 1000) + oldNotePtr->getNoteEndTimeMilSec() );
		}
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_DRAG;
		break;


	default:
		// 정체불명의 타입일 경우, 일단 false
		return false;
		break;
	}



	for ( i=0 ; i < MAX_NOTE_ARRAY ; i++ )
	{
		for ( j=0 ; j < MAX_HALF_ARRAY ; j++ )
		{
			// 노트 정보를 받아 온 다음,
			tempNote = pDoc->getNoteAddr(i, j);

			// 만약 비었다면 넘어간다.
			if ( tempNote == NULL )
			{
				// 루프문 탈출
				break;
			}

			// 같은 노트인지 확인
			if ( oldNotePtr != NULL && oldNotePtr == tempNote )
			{
				// 같은 노트일 경우 스킵
			}
			else
			{


				// 각 노트 타입에 따른 확인
				switch( tempNote->getNoteType() )
				{
					// 단일 노트의 경우에는 똑같이 그대로 간다.
				case NOTE_T_RIGHT:
					isCompNoteEx = false;
					isCompNoteRight = true;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					//compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong = compNoteStartTimeLong;
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
					break;


				case NOTE_T_LEFT:
					// 왼손노트의 경우 사실상 유일하게 겹쳐도 되는 노트이므로 조금 다르게 처리.
					isCompNoteEx = false;
					isCompNoteRight = false;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					//compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong = compNoteStartTimeLong;
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
					break;

				case NOTE_T_BPM:
				case NOTE_T_PHOTO:
					// 여기 예외처리
					isCompNoteEx = false;
				 	isCompNoteRight = !isNewNoteRight;
					break;

				case NOTE_T_LONG:
					isCompNoteEx = false;
					isCompNoteRight = true;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
					break;


				case NOTE_T_CHARISMA:
					isCompNoteEx = true;
					isCompNoteRight = isNewNoteRight;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_CHAR;
					break;


				case NOTE_T_NEUTRAL:
					isCompNoteEx = true;
					isCompNoteRight = isNewNoteRight;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_NEUTRAL;
					break;


				case NOTE_T_PATTERN:
					isCompNoteEx = true;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_PATTERN;
					break;


				case NOTE_T_DRAG:
					isCompNoteEx = true;
					isCompNoteRight = isNewNoteRight;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_DRAG;
					break;


				default:
					// 정체불명의 타입일 경우, 일단 false
					return false;
					break;
				}


				// 겹치는 경우의 수는 총 4가지 이므로, 2가지의 검사를 시행한다.
				if (
					(
					(newNoteTimeLong <= compNoteStartTimeLong) &&
					(compNoteStartTimeLong <= newNoteEndTimeLong)
					)
					||
					(
					(compNoteStartTimeLong <= newNoteTimeLong) &&
					(newNoteTimeLong <= compNoteEndTimeLong)
					)
 					)
				{
					// 만약 시간이 현재 노트에 해당 할 경우, false 리턴.

					// 시간이 겹친다고 판정났을 때, 왼손 & 오른손 문제가 있는지 확인한다.
					if ( isNewNoteRight == isCompNoteRight )
					{
						// 두 노트가 같은 노트일 경우
						return false;
					}
					else
					{
						// 손은 서로 다르지만, 둘 중 하나라도 완전배재 노트일 경우
						if ( isNewNoteEx == true ||
							isCompNoteEx == true )
						{
							return false;
						}

					}
				}
			}
		}
	}


	// 별 일 없이 검사를 모두 마쳤다면 true;
	return true;
}

// (꼬리를 수정하는 용 오버로딩) 현재 그리려는 노트와 중첩되면 안 되는 노트가 있는 지 확인한다.
bool CNotePickingView::chkIsNoteDrawOk(CNoteData *oldNotePtr, const unsigned int noteTimeSec, const unsigned int noteTimeMilSec, NoteTime noteEndTime, const char noteType, const char TargetMarker)
{

	CNoteEditingToolDoc *pDoc = GetDocument();

	// 생성 할 노트의 시간
	unsigned long newNoteTimeLong = (unsigned long)((noteTimeSec * 1000) + noteTimeMilSec);
	unsigned long newNoteEndTimeLong = (unsigned long)( (noteEndTime.noteTimeSec * 1000) + noteEndTime.noteTimeMilSec );
	// 비교 할 노트의 시작과 끝 시간
	unsigned long compNoteStartTimeLong = 0;
	unsigned long compNoteEndTimeLong = 0;

	unsigned long newNoteTimeInterval = 0;			// 노트 타입에 따라서 어느 정도 인터벌을 줄 것이지 설정.

	bool isNewNoteRight = true;						// 새 노트가 오른손일 경우 T
	bool isNewNoteEx = false;						// 새 노트가 Drag노트 등의 특수 노트일 경우 T
	bool isCompNoteRight = true;						// 비교 할 노트가 오른손일 경우 T
	bool isCompNoteEx = false;						// 비교 할 노트가 Drag노트 등의 특수 노트일 경우 T

	int i=0, j=0;
	CNoteData *tempNote = NULL;

	// 먼저 그리려는 노트의 타입부터 확인
	switch( noteType )
	{
		// 단일 노트의 경우에는 똑같이 그대로 간다.
	case NOTE_T_RIGHT:
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
		break;


	case NOTE_T_LEFT:
		// 적절한 인터벌을 준다.
		isNewNoteRight = false;
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
		break;

	case NOTE_T_BPM:
	case NOTE_T_PHOTO:
		// 검사 할 필요가 없는 노트이므로 통과
		return true;
		break;

	case NOTE_T_LONG:
// 		if ( oldNotePtr == NULL )
// 		{
// 			// 새로 노트를 만드는 것일 때
// 			newNoteEndTimeLong += (unsigned long)( (DEFAULT_SEC_RANGE * 1000) + DEFAULT_MILSEC_RANGE );
// 		}
// 		else
// 		{
// 			// 수정하는 중일 때
// 			newNoteEndTimeLong = (unsigned long)( (oldNotePtr->getNoteEndTimeSec() * 1000) + oldNotePtr->getNoteEndTimeMilSec() );
// 		}
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
		break;


	case NOTE_T_CHARISMA:
		isNewNoteEx = true;
// 		if ( oldNotePtr == NULL )
// 		{
// 			// 새로 노트를 만드는 것일 때
// 			newNoteEndTimeLong += (unsigned long)( (DEFAULT_SEC_RANGE * 1000) + DEFAULT_MILSEC_RANGE );
// 		}
// 		else
// 		{
// 			// 수정하는 중일 때
// 			newNoteEndTimeLong = (unsigned long)( (oldNotePtr->getNoteEndTimeSec() * 1000) + oldNotePtr->getNoteEndTimeMilSec() );
// 		}
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_CHAR;
		break;


	case NOTE_T_NEUTRAL:
		isNewNoteEx = true;
// 		if ( oldNotePtr == NULL )
// 		{
// 			// 새로 노트를 만드는 것일 때
// 			newNoteEndTimeLong += (unsigned long)( (DEFAULT_SEC_RANGE * 1000) + DEFAULT_MILSEC_RANGE );
// 		}
// 		else
// 		{
// 			// 수정하는 중일 때
// 			newNoteEndTimeLong = (unsigned long)( (oldNotePtr->getNoteEndTimeSec() * 1000) + oldNotePtr->getNoteEndTimeMilSec() );
// 		}
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_NEUTRAL;
		break;


	case NOTE_T_PATTERN:
		isNewNoteEx = true;
// 		if ( oldNotePtr == NULL )
// 		{
// 			// 새로 노트를 만드는 것일 때
// 			newNoteEndTimeLong += (unsigned long)( (DEFAULT_SEC_RANGE * 1000) + DEFAULT_MILSEC_RANGE );
// 		}
// 		else
// 		{
// 			// 수정하는 중일 때
// 			newNoteEndTimeLong = (unsigned long)( (oldNotePtr->getNoteEndTimeSec() * 1000) + oldNotePtr->getNoteEndTimeMilSec() );
// 		}
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_PATTERN;
		break;


	case NOTE_T_DRAG:
		isNewNoteEx = true;
// 		if ( oldNotePtr == NULL )
// 		{
// 			// 새로 노트를 만드는 것일 때
// 			newNoteEndTimeLong += (unsigned long)( (DEFAULT_DRAG_SEC * 1000) + DEFAULT_DRAG_MILSEC );
// 		}
// 		else
// 		{
// 			// 수정하는 중일 때
// 			newNoteEndTimeLong = (unsigned long)( (oldNotePtr->getNoteEndTimeSec() * 1000) + oldNotePtr->getNoteEndTimeMilSec() );
// 		}
		// 적절한 인터벌을 준다.
		newNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_DRAG;
		break;


	default:
		// 정체불명의 타입일 경우, 일단 false
		return false;
		break;
	}



	for ( i=0 ; i < MAX_NOTE_ARRAY ; i++ )
	{
		for ( j=0 ; j < MAX_HALF_ARRAY ; j++ )
		{
			// 노트 정보를 받아 온 다음,
			tempNote = pDoc->getNoteAddr(i, j);

			// 만약 비었다면 넘어간다.
			if ( tempNote == NULL )
			{
				// 루프문 탈출
				break;
			}

			// 같은 노트인지 확인
			if ( oldNotePtr != NULL && oldNotePtr == tempNote )
			{
				// 같은 노트일 경우 스킵
			}
			else
			{


				// 각 노트 타입에 따른 확인
				switch( tempNote->getNoteType() )
				{
					// 단일 노트의 경우에는 똑같이 그대로 간다.
				case NOTE_T_RIGHT:
					isCompNoteEx = false;
					isCompNoteRight = true;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					//compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong = compNoteStartTimeLong;
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
					break;


				case NOTE_T_LEFT:
					// 왼손노트의 경우 사실상 유일하게 겹쳐도 되는 노트이므로 조금 다르게 처리.
					isCompNoteEx = false;
					isCompNoteRight = false;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					//compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong = compNoteStartTimeLong;
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
					break;

				case NOTE_T_BPM:
				case NOTE_T_PHOTO:
					// 여기 예외처리
					isCompNoteEx = false;
					isCompNoteRight = !isNewNoteRight;
					break;

				case NOTE_T_LONG:
					isCompNoteEx = false;
					isCompNoteRight = true;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_SAME_R;
					break;


				case NOTE_T_CHARISMA:
					isCompNoteEx = true;
					isCompNoteRight = isNewNoteRight;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_CHAR;
					break;


				case NOTE_T_NEUTRAL:
					isCompNoteEx = true;
					isCompNoteRight = isNewNoteRight;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_NEUTRAL;
					break;


				case NOTE_T_PATTERN:
					isCompNoteEx = true;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_PATTERN;
					break;


				case NOTE_T_DRAG:
					isCompNoteEx = true;
					isCompNoteRight = isNewNoteRight;
					compNoteStartTimeLong = (unsigned long)( (tempNote->getNoteTimeSec() * 1000) + tempNote->getNoteTimeMilSec() );
					compNoteEndTimeLong = (unsigned long)( (tempNote->getNoteEndTimeSec() * 1000) + tempNote->getNoteEndTimeMilSec() );
					compNoteEndTimeLong += MIN_NOTE_END_INTERVAL_AFTER_DRAG;
					break;


				default:
					// 정체불명의 타입일 경우, 일단 false
					return false;
					break;
				}


				// 겹치는 경우의 수는 총 4가지 이므로, 2가지의 검사를 시행한다.
				if (
					(
					(newNoteTimeLong <= compNoteStartTimeLong) &&
					(compNoteStartTimeLong <= newNoteEndTimeLong)
					)
					||
					(
					(compNoteStartTimeLong <= newNoteTimeLong) &&
					(newNoteTimeLong <= compNoteEndTimeLong)
					)
					)
				{
					// 만약 시간이 현재 노트에 해당 할 경우, false 리턴.

					// 시간이 겹친다고 판정났을 때, 왼손 & 오른손 문제가 있는지 확인한다.
					if ( isNewNoteRight == isCompNoteRight )
					{
						// 두 노트가 같은 노트일 경우
						return false;
					}
					else
					{
						// 손은 서로 다르지만, 둘 중 하나라도 완전배재 노트일 경우
						if ( isNewNoteEx == true ||
							isCompNoteEx == true )
						{
							return false;
						}

					}
				}
			}
		}
	}


	// 별 일 없이 검사를 모두 마쳤다면 true;
	return true;
}



//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [GET & SET]   //////////////////////////////
//////////////////////////////////////////////////////////////////////////////

int CNotePickingView::getNowPlayingStatus(void)
{
	return this->nowPlayingStatus;
}


void CNotePickingView::setNowPlayingStatus(const int nowPlayingStatus)
{
	this->nowPlayingStatus = nowPlayingStatus;
}




//////////////////////////////////////////////////////////////////////////////
//////////////////////////////     [Public]     //////////////////////////////
//////////////////////////////////////////////////////////////////////////////



// CNotePickingView 진단입니다.

#ifdef _DEBUG
void CNotePickingView::AssertValid() const
{
	CScrollView::AssertValid();
}

#ifndef _WIN32_WCE
void CNotePickingView::Dump(CDumpContext& dc) const
{
	CScrollView::Dump(dc);
}
#endif
#endif //_DEBUG




BOOL CNotePickingView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: CREATESTRUCT cs를 수정하여 여기에서
	//  Window 클래스 또는 스타일을 수정합니다.

	//cs.lpszClass = AfxRegisterWndClass(
	//	CS_VREDRAW | CS_HREDRAW,
	//	LoadCursor(NULL, IDC_ARROW),
	//	(HBRUSH)GetStockObject(DKGRAY_BRUSH),         // 정의되어 있는 BRUSH중 원하는 것을 넣으면 된다. 
	//	LoadIcon(NULL, IDI_APPLICATION));


	return CView::PreCreateWindow(cs);
}




BOOL CNotePickingView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// 기본적인 준비
	return DoPreparePrinting(pInfo);
}

void CNotePickingView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: 인쇄하기 전에 추가 초기화 작업을 추가합니다.
}

void CNotePickingView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: 인쇄 후 정리 작업을 추가합니다.
}










// CNotePickingView 메시지 처리기입니다.


void CNotePickingView::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CNoteEditingToolDoc *pDoc = GetDocument();


	// 찍는 화면이 스크롤 뷰이기 때문에, 스크롤의 위치에 따른 보정이 필요하다.
	int nVertScroll = GetScrollPos(SB_VERT);	// 세로
	int nHorzScroll = GetScrollPos(SB_HORZ);	// 가로
	CPoint scrollPos = CPoint(nHorzScroll, nVertScroll);	


	// 마우스를 움직일 때마다 찍을 노트의 위치를 보여주는 부분.
	CDC* pDC = GetDC();							// 다큐멘트를 받아오는 것?

	char noteEditingMode = pDoc->getNoteEditingMode();

	CPoint newPoint;							// 사각형을 그릴 좌표.
	char drawOkFlag = 0;						// 사각형을 그려도 되는 것인지 확인하는 플래그.
	// 만약 스크롤이 일정량 이상 이동 된 상태일 경우에 대한 보정 필요하다.


	// 색 칠하는 용.
	CBrush newBrush(RGB(255, 255, 255));
	CBrush* oldBrush;



	// 더블 버퍼링 지원용
	CDC mdcOffScreen;			// 더블버퍼링을 위한 메모리 그림버퍼



	// 만약 재생중에 있을 경우
	//if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
	//{
	//	if( noteEditingMode == EDIT_MODE_WRITE )
	//	{
	//		//point
	//	}



	//}
	//else
	{





		// 계속 보이던 것을 지우고.

		//RedrawWindow();
		// 배경을 그린 다음
		mdcOffScreen.CreateCompatibleDC(pDC);
		mdcOffScreen.SelectObject(bmpOffScreen);	

		// 화면에 출력
		pDC->BitBlt(-scrollPos.x, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X, FRAME_1_HEIGHT, &mdcOffScreen, 0, 0, SRCCOPY);

		// 현재의 타임라인을 그린다.
		DrawTimeLineEx(pDC->m_hDC, BASS_ChannelGetPosition(chan, BASS_POS_BYTE ), 0xffffff, 0, scrollPos); // current pos



		if( noteEditingMode == EDIT_MODE_WRITE )
		{
			// 함수 오버로딩 필요.
			newPoint = getNoteReviPos(point, drawOkFlag, scrollPos);

			// 쓸 수 없는 영역 안에 있는지 확인
			if( point.x + nHorzScroll <= (int)(V_NOTE_SET_OFF_X + V_UNUSED_TIMELINE) )
			{
				drawOkFlag = false;
			}

			if( drawOkFlag != 0 )
			{
				// 노트의 타입에 따른 색 결정
				switch( pDoc->getNoteWriteType() )
				{
				case NOTE_T_RIGHT:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_RIGHT);
					break;

				case NOTE_T_LEFT:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_LEFT);
					break;

				case NOTE_T_LONG:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_LONG);
					break;

				case NOTE_T_PATTERN:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_PATTERN);
					break;

				case NOTE_T_CHARISMA:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_CHARISMA);
					break;

				case NOTE_T_NEUTRAL:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_NEUTRAL);
					break;

				case NOTE_T_DRAG:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_DRAG);
					break;

				case NOTE_T_BPM:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_BPM);
					break;

				case NOTE_T_PHOTO:
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(NC_PHOTO);
					break;

				default:
					newBrush.CreateSolidBrush(NC_UNKNOWN);
				}

				// 현재 브러시 내용을 받아온다.
				oldBrush = pDC->SelectObject(&newBrush);




				if( pDoc->getNoteWriteType() == NOTE_T_LONG ||
					pDoc->getNoteWriteType() == NOTE_T_DRAG ||
					pDoc->getNoteWriteType() == NOTE_T_CHARISMA ||
					pDoc->getNoteWriteType() == NOTE_T_NEUTRAL ||
					pDoc->getNoteWriteType() == NOTE_T_PATTERN
					)
				{
					// 실제 사각형을 그린다.
					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_LNOTE_LENGTH + 1, newPoint.y + V_NOTE_HEIGHT);
					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
					pDC->Rectangle(newPoint.x + V_LNOTE_LENGTH, newPoint.y, newPoint.x + V_LNOTE_LENGTH + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
				}
				else
				{
					// 실제 사각형을 그린다.
					pDC->Rectangle(newPoint.x, newPoint.y, newPoint.x + V_NOTE_WIDTH, newPoint.y + V_NOTE_HEIGHT);
				}




				pDC->SelectObject(oldBrush);
			}

		}
		else if( noteEditingMode == EDIT_MODE_MOVE )
		{
			NoteTime time;
			char noteType;
			char targetMarker;
			CNoteData* targetNote = NULL;
			int chkNoteTimeResult = -10;
			int noteHeadDir = -1;

			// 시간에 맞게 보정 해 준다.
			point += scrollPos;					// 스크롤 보정

			// 수정 모드일 경우.
			if( pDoc->getOnFocusEditingFlag() == true )
			{
				newPoint = getNoteReviPosRanType(point, drawOkFlag);

				ConvertPixToNoteDataRanType(point, time, noteType, targetMarker);			// 사실 수행할 필요는 없지만, Drag에서 1~6마커 자리로 커서가 올라가는 버그를 위해.


				// 현재 잡아끌고 있는 중일 때.
				targetNote = pDoc->getNowEditingNote(noteHeadDir);	

				if ( noteType == chkTypeToBigType(targetNote->getNoteType()) )
				{
					if( targetNote != NULL)
					{
						if ( drawOkFlag != 0 )
						{
							// 그려도 되는 상황일 경우,
							// 노트가 존재 할 경우, 사각형을 그려준다.
							//pDC->Rectangle(drawingPoint.x-2, drawingPoint.y-2, drawingPoint.x + V_NOTE_WIDTH + 2, drawingPoint.y + V_NOTE_HEIGHT);

							newPoint -= scrollPos;

							CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
							l_my_pen.CreatePen(PS_SOLID, 2, NCL_EDIT);
							old_pen = pDC->SelectObject(&l_my_pen);

							pDC->SelectObject(&l_my_pen);

							pDC->MoveTo( newPoint.x-2, newPoint.y-1 );
							pDC->LineTo( newPoint.x + V_NOTE_WIDTH + 1, newPoint.y-1 );
							pDC->LineTo( newPoint.x + V_NOTE_WIDTH + 1, newPoint.y + V_NOTE_HEIGHT + 1 );
							pDC->LineTo( newPoint.x-1, newPoint.y + V_NOTE_HEIGHT + 1 );
							pDC->LineTo( newPoint.x-1, newPoint.y - 2 );

							pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
							l_my_pen.DeleteObject();
						}	
					}
					else
					{
						// 무언가 문제가 있을 때,
						AfxMessageBox(_T("NULL일 때, Flag가 True가 되었음!"));
					}
				}
			}
			else
			{
				// 그냥 마우스만 올려 놓고 있는 상태일 때,

				// 만약 적절한 위치에 올려 놨다면,
				if( ConvertPixToNoteDataRanType(point, time, noteType, targetMarker) > 0 )
				{
					// 충돌처리를 해 준다.

					//chkNoteTimeResult = pDoc->chkNoteTime(time, noteType, targetMarker, &targetNote);
					chkNoteTimeResult = pDoc->chkNoteTime(time, noteType, targetMarker, &targetNote);
					if( chkNoteTimeResult >= 1 )
					{
						// 포인터를 잡아준다.
						if( targetNote != NULL )
						{
							//pDoc->setNowEditingNote(targetNote, noteHeadDir);
							noteHeadDir = pDoc->getNoteHeadDir();
							if( noteHeadDir == 0 )
							{
								time.noteTimeSec = targetNote->getNoteTimeSec();
								time.noteTimeMilSec = targetNote->getNoteTimeMilSec();

								CPoint drawingPoint;
								drawingPoint.x = ConvertTimeToPixX(time);
								drawingPoint.y = ConvertTypeToPixY(targetNote->getNoteType(), targetNote->getTargetMarker());
								drawingPoint -= scrollPos;

								// 노트가 존재 할 경우, 사각형을 그려준다.
								//pDC->Rectangle(drawingPoint.x-2, drawingPoint.y-2, drawingPoint.x + V_NOTE_WIDTH + 2, drawingPoint.y + V_NOTE_HEIGHT);

								CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
								l_my_pen.CreatePen(PS_SOLID, 2, NCL_EDIT);
								old_pen = pDC->SelectObject(&l_my_pen);

								pDC->SelectObject(&l_my_pen);

								pDC->MoveTo( drawingPoint.x-2, drawingPoint.y-1 );
								pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y-1 );
								pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
								pDC->LineTo( drawingPoint.x-1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
								pDC->LineTo( drawingPoint.x-1, drawingPoint.y - 2 );

								pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
								l_my_pen.DeleteObject();
							}
							else if( noteHeadDir == 1 )
							{
								time.noteTimeSec = targetNote->getNoteEndTimeSec();
								time.noteTimeMilSec = targetNote->getNoteEndTimeMilSec();

								CPoint drawingPoint;
								drawingPoint.x = ConvertTimeToPixX(time);
								drawingPoint.y = ConvertTypeToPixY(targetNote->getNoteType(), targetNote->getTargetMarker());
								drawingPoint -= scrollPos;

								// 노트가 존재 할 경우, 사각형을 그려준다.
								//pDC->Rectangle(drawingPoint.x-2, drawingPoint.y-2, drawingPoint.x + V_NOTE_WIDTH + 2, drawingPoint.y + V_NOTE_HEIGHT);

								CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
								l_my_pen.CreatePen(PS_SOLID, 2, NCL_EDIT);
								old_pen = pDC->SelectObject(&l_my_pen);

								pDC->SelectObject(&l_my_pen);

								pDC->MoveTo( drawingPoint.x-2, drawingPoint.y-1 );
								pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y-1 );
								pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
								pDC->LineTo( drawingPoint.x-1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
								pDC->LineTo( drawingPoint.x-1, drawingPoint.y - 2 );

								pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
								l_my_pen.DeleteObject();
							}
						}
						else
						{
							AfxMessageBox(_T("치명적인 문제 발생!"));
						}
					}
				}
			}
		}
		else if( noteEditingMode == EDIT_MODE_ERASE )
		{
			NoteTime time;
			char noteType;
			char targetMarker;
			CNoteData* targetNote = NULL;
			int chkNoteTimeResult = -10;
			int noteHeadDir = -1;

			// 시간에 맞게 보정 해 준다.
			point += scrollPos;					// 스크롤 보정

			newPoint = getNoteReviPosRanType(point, drawOkFlag);


			// 그냥 마우스만 올려 놓고 있는 상태일 때,

			// 만약 적절한 위치에 올려 놨다면,
			if( ConvertPixToNoteDataRanType(point, time, noteType, targetMarker) > 0 )
			{
				// 충돌처리를 해 준다.
				chkNoteTimeResult = pDoc->chkNoteTime(time, drawOkFlag, targetMarker, &targetNote);
				if( chkNoteTimeResult >= 1 )
				{
					// 포인터를 잡아준다.
					if( targetNote != NULL )
					{
						//pDoc->setNowEditingNote(targetNote, noteHeadDir);
						noteHeadDir = pDoc->getNoteHeadDir();
						if( noteHeadDir == 0 )
						{
							time.noteTimeSec = targetNote->getNoteTimeSec();
							time.noteTimeMilSec = targetNote->getNoteTimeMilSec();

							CPoint drawingPoint;
							drawingPoint.x = ConvertTimeToPixX(time);
							drawingPoint.y = newPoint.y;
							drawingPoint -= scrollPos;

							// 노트가 존재 할 경우, 사각형을 그려준다.
							//pDC->Rectangle(drawingPoint.x-2, drawingPoint.y-2, drawingPoint.x + V_NOTE_WIDTH + 2, drawingPoint.y + V_NOTE_HEIGHT);

							CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
							l_my_pen.CreatePen(PS_SOLID, 2, RGB(255, 2, 128));
							old_pen = pDC->SelectObject(&l_my_pen);

							pDC->SelectObject(&l_my_pen);

							pDC->MoveTo( drawingPoint.x-2, drawingPoint.y-1 );
							pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y-1 );
							pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
							pDC->LineTo( drawingPoint.x-1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
							pDC->LineTo( drawingPoint.x-1, drawingPoint.y - 2 );

							pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
							l_my_pen.DeleteObject();
						}
						else if( noteHeadDir == 1 )
						{
							time.noteTimeSec = targetNote->getNoteEndTimeSec();
							time.noteTimeMilSec = targetNote->getNoteEndTimeMilSec();

							CPoint drawingPoint;
							drawingPoint.x = ConvertTimeToPixX(time);
							drawingPoint.y = newPoint.y;
							drawingPoint -= scrollPos;

							// 노트가 존재 할 경우, 사각형을 그려준다.
							//pDC->Rectangle(drawingPoint.x-2, drawingPoint.y-2, drawingPoint.x + V_NOTE_WIDTH + 2, drawingPoint.y + V_NOTE_HEIGHT);

							CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
							l_my_pen.CreatePen(PS_SOLID, 2, RGB(255, 2, 128));
							old_pen = pDC->SelectObject(&l_my_pen);

							pDC->SelectObject(&l_my_pen);

							pDC->MoveTo( drawingPoint.x-2, drawingPoint.y-1 );
							pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y-1 );
							pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
							pDC->LineTo( drawingPoint.x-1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
							pDC->LineTo( drawingPoint.x-1, drawingPoint.y - 2 );

							pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
							l_my_pen.DeleteObject();
						}
					}
					else
					{
						AfxMessageBox(_T("치명적인 문제 발생!"));
					}
				}
			}
		}
		else if( noteEditingMode == EDIT_MODE_CONFG )
		{
			CNoteData *teaslkejflskNote = pDoc->getNowEditingNote(nHorzScroll);
			if( teaslkejflskNote == NULL )
			{
				int sj = 0;
			}

			NoteTime time;
			char noteType;
			char targetMarker;
			CNoteData* targetNote = NULL;
			int chkNoteTimeResult = -10;
			int noteHeadDir = -1;

			// 시간에 맞게 보정 해 준다.
			point += scrollPos;					// 스크롤 보정

			newPoint = getNoteReviPosRanType(point, drawOkFlag);


			// 그냥 마우스만 올려 놓고 있는 상태일 때,

			// 만약 적절한 위치에 올려 놨다면,
			if( ConvertPixToNoteDataRanType(point, time, noteType, targetMarker) > 0 )
			{
				// 충돌처리를 해 준다.
				chkNoteTimeResult = pDoc->chkNoteTime(time, drawOkFlag, targetMarker, &targetNote);
				if( chkNoteTimeResult >= 1 )
				{
					// 포인터를 잡아준다.
					if( targetNote != NULL )
					{
						//pDoc->setNowEditingNote(targetNote, noteHeadDir);
						noteHeadDir = pDoc->getNoteHeadDir();
						if( noteHeadDir == 0 )
						{

							time.noteTimeSec = targetNote->getNoteTimeSec();
							time.noteTimeMilSec = targetNote->getNoteTimeMilSec();

							CPoint drawingPoint;
							drawingPoint.x = ConvertTimeToPixX(time);
							drawingPoint.y = newPoint.y;
							drawingPoint -= scrollPos;

							// 노트가 존재 할 경우, 사각형을 그려준다.
							//pDC->Rectangle(drawingPoint.x-2, drawingPoint.y-2, drawingPoint.x + V_NOTE_WIDTH + 2, drawingPoint.y + V_NOTE_HEIGHT);

							CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
							l_my_pen.CreatePen(PS_SOLID, 2, RGB(200, 191, 231));
							old_pen = pDC->SelectObject(&l_my_pen);

							pDC->SelectObject(&l_my_pen);

							pDC->MoveTo( drawingPoint.x-2, drawingPoint.y-1 );
							pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y-1 );
							pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
							pDC->LineTo( drawingPoint.x-1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
							pDC->LineTo( drawingPoint.x-1, drawingPoint.y - 2 );

							pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
							l_my_pen.DeleteObject();
						}
						else if( noteHeadDir == 1 )
						{
							time.noteTimeSec = targetNote->getNoteEndTimeSec();
							time.noteTimeMilSec = targetNote->getNoteEndTimeMilSec();

							CPoint drawingPoint;
							drawingPoint.x = ConvertTimeToPixX(time);
							drawingPoint.y = newPoint.y;
							drawingPoint -= scrollPos;

							// 노트가 존재 할 경우, 사각형을 그려준다.
							//pDC->Rectangle(drawingPoint.x-2, drawingPoint.y-2, drawingPoint.x + V_NOTE_WIDTH + 2, drawingPoint.y + V_NOTE_HEIGHT);

							CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
							l_my_pen.CreatePen(PS_SOLID, 2, RGB(200, 191, 231));
							old_pen = pDC->SelectObject(&l_my_pen);

							pDC->SelectObject(&l_my_pen);

							pDC->MoveTo( drawingPoint.x-2, drawingPoint.y-1 );
							pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y-1 );
							pDC->LineTo( drawingPoint.x + V_NOTE_WIDTH + 1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
							pDC->LineTo( drawingPoint.x-1, drawingPoint.y + V_NOTE_HEIGHT + 1 );
							pDC->LineTo( drawingPoint.x-1, drawingPoint.y - 2 );

							pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
							l_my_pen.DeleteObject();
						}
					}
					else
					{
						AfxMessageBox(_T("치명적인 문제 발생!"));
					}
				}
				else
				{
					// 만약 포커스가 없을 경우, 노트를 NULL로 만들어 준다.
					//pDoc->setNowEditingNote(NULL, 0);
				}
			}
		}


	}



	ReleaseDC(pDC);



	CScrollView::OnMouseMove(nFlags, point);
}


void CNotePickingView::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.

	CNoteEditingToolDoc* pDoc = GetDocument();

	CNoteData* notePointer = NULL;
	int noteHeadDir = -10;

	// 찍는 화면이 스크롤 뷰이기 때문에, 스크롤의 위치에 따른 보정이 필요하다.
	int nVertScroll = GetScrollPos(SB_VERT);	// 세로
	int nHorzScroll = GetScrollPos(SB_HORZ);	// 가로
	point.x += nHorzScroll;
	point.y += nVertScroll;



	// 음악 재생 포커스를 넣는 부분.
	if ( chan )
	{
		// 현재 채널에 포커스가 있을 경우에만 해당.
		if ( point.x >= V_NOTE_SET_OFF_X )
		{
			if ( point.y <= V_NOTE_SET_OFF_Y && point.y >= 0 )
			{
				// 클릭 한 위치로 재생 포커스 이동.
				BASS_ChannelSetPosition(chan, ((point.x - V_NOTE_SET_OFF_X) * bpp), BASS_POS_BYTE);


				if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PAUSED ||
					BASS_ChannelIsActive(chan) == BASS_ACTIVE_STOPPED )
				{
					// 화면 다시 그리기
					DrawDoubleBuffering();
					Invalidate();
				}
			}
		}
	}


	if( pDoc->getNoteEditingMode() == EDIT_MODE_MOVE && 
		pDoc->getOnFocusEditingFlag() == false  &&
		pDoc->getNowEditingNote(noteHeadDir) != NULL
		)
	{
		// 특정 포커스가 있는 상태에서 클릭했을 경우
		pDoc->setOnFocusEditingFlag(true);
		//	AfxMessageBox(_T("!"));
	}




	CScrollView::OnLButtonDown(nFlags, point);
}


void CNotePickingView::OnLButtonUp(UINT nFlags, CPoint point)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	// Document 정보를 받아온다.
	CNoteEditingToolDoc* pDoc = GetDocument();

	//	CPoint point;
	NoteTime time;
	char noteType, targetMarker;


	// 찍는 화면이 스크롤 뷰이기 때문에, 스크롤의 위치에 따른 보정이 필요하다.
	int nVertScroll = GetScrollPos(SB_VERT);	// 세로
	int nHorzScroll = GetScrollPos(SB_HORZ);	// 가로
	point.x += nHorzScroll;
	point.y += nVertScroll;


	CDC* pDC = GetDC();						// 다큐멘트를 받아오는 것?


	//if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
	//{

	//	if( pDoc->getNoteEditingMode() == EDIT_MODE_WRITE )
	//	{
	//		// 쓰기 모드일 때
	//		onTimerMousePoint = point;
	//		chkMouseHandleState = TIMER_1_FLAG_UP;
	//	}
	//	else if ( pDoc->getNoteEditingMode() == EDIT_MODE_MOVE )
	//	{
	//	}
	//	else if( pDoc->getNoteEditingMode() == EDIT_MODE_ERASE )
	//	{

	//	}
	//	else if( pDoc->getNoteEditingMode() == EDIT_MODE_CONFG )
	//	{

	//	}


	//}
	//else
	{

		if( pDoc->getNoteEditingMode() == EDIT_MODE_WRITE )
		{
			if ( point.x >= (int)(V_NOTE_SET_OFF_X + V_UNUSED_TIMELINE) )
			{
				// ↑ 이미 nHorzScroll이 더해져 있는 상태이므로 또 더하지 않는다.

				// 마우스의 좌표를 적절히 분석 한 다음,
				if( ConvertPixToNoteData(point, time, noteType, targetMarker) > 0 )
				{
					// 만약 그려도 좋은 노트일 경우,
					if ( chkIsNoteDrawOk(NULL, time.noteTimeSec, time.noteTimeMilSec, noteType, targetMarker) )
					{
						// 알맞은 값으로 노트를 생성한다.
						pDoc->addNewNote(time.noteTimeSec, time.noteTimeMilSec, noteType, targetMarker);
						

						// 직접 그리거나, 그리라는 신호를 주는 함수
						//if ( chan )
						//{
						//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
						//	{
						//		m_timerRedrawFlag = true;
						//	}
						//	else
						//	{
						//		DrawDoubleBuffering();
						//		Invalidate();
						//	}
						//}
						//else
						//{
						//	DrawDoubleBuffering();
						//	Invalidate();
						//}

						DrawDoubleBuffering();
						Invalidate(FALSE);
					}
					else
					{
						// 겹치는 노트가 있을 경우
						MessageBox(_T("겹치게 되는 노트가 있어 그릴 수 없습니다."));

					}
				}
			}
		}
		else if( pDoc->getNoteEditingMode() == EDIT_MODE_MOVE )
		{
			if( pDoc->getOnFocusEditingFlag() == true )
			{
				if ( point.x >= (int)(V_NOTE_SET_OFF_X + V_UNUSED_TIMELINE) )
				{
					// ↑ 이미 nHorzScroll이 더해져 있는 상태이므로 또 더하지 않는다.

					// 만약 드래그 한 다음에 클릭을 놓았을 경우, 
					CNoteData *tmpNoteData = NULL;
					int noteHeadDir = -10;
					CString tmpMessage = _T("");						// 에러 내용을 표시하기 위한 메시지

					// 노트의 정보를 받아온다.
					tmpNoteData = pDoc->getNowEditingNote(noteHeadDir);
					if( noteHeadDir == 0 )
					{
						if( ConvertPixToNoteDataRanType(point, time, noteType, targetMarker) > 0 )
						{
							if ( noteType == chkTypeToBigType(tmpNoteData->getNoteType()) )
							{
								// 노트의 타입이 같다는 가정 하에서,

								if ( tmpNoteData->getNoteType() == NOTE_T_LONG ||
									tmpNoteData->getNoteType() == NOTE_T_DRAG ||
									tmpNoteData->getNoteType() == NOTE_T_CHARISMA ||
									tmpNoteData->getNoteType() == NOTE_T_NEUTRAL ||
									tmpNoteData->getNoteType() == NOTE_T_PATTERN
									)
								{
									// 앞 뒤 시간이 꼬일 경우, 종료하게 한다.
									long tmpNoteTime = (time.noteTimeSec * 1000) + time.noteTimeMilSec;
									long tmpNoteEndTime = (tmpNoteData->getNoteEndTimeSec() * 1000) + tmpNoteData->getNoteEndTimeMilSec();

									if ( tmpNoteTime >= tmpNoteEndTime )
									{
										AfxMessageBox(_T("노트의 종료시간은 시작 시간보다 앞이여야 합니다."));

										// 플래그를 바꿔준다.
										pDoc->setOnFocusEditingFlag(false);
										pDoc->setNowEditingNote(NULL, 0);

										// 윈도우를 다시 그려준다.
										//RedrawWindow();
										// 직접 그리거나, 그리라는 신호를 주는 함수
										//if ( chan )
										//{
										//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
										//	{
										//		m_timerRedrawFlag = true;
										//	}
										//	else
										//	{
										//		DrawDoubleBuffering();
										//		Invalidate();
										//	}
										//}
										//else
										//{
										//	DrawDoubleBuffering();
										//	Invalidate();
										//}

										DrawDoubleBuffering();
										Invalidate(FALSE);

										return;
									}
									else if ( tmpNoteTime + MAX_NOTE_INTERVAL_ALL <= tmpNoteEndTime )
									{
										AfxMessageBox(_T("노트의 길이가 너무 깁니다."));

										// 플래그를 바꿔준다.
										pDoc->setOnFocusEditingFlag(false);
										pDoc->setNowEditingNote(NULL, 0);

										// 윈도우를 다시 그려준다.
										//RedrawWindow();
										// 직접 그리거나, 그리라는 신호를 주는 함수
										//if ( chan )
										//{
										//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
										//	{
										//		m_timerRedrawFlag = true;
										//	}
										//	else
										//	{
										//		DrawDoubleBuffering();
										//		Invalidate();
										//	}
										//}
										//else
										//{
										//	DrawDoubleBuffering();
										//	Invalidate();
										//}

										DrawDoubleBuffering();
										Invalidate(FALSE);

										return;
									}
									else
									{
										// 각각의 타입 별 최대시간보다 더 클 경우, 메시지를 보내고 수정이 안 되게 한다.
										switch( tmpNoteData->getNoteType() )
										{
										case NOTE_T_DRAG:
											// 드래그 노트일 경우,
											if( tmpNoteTime >= tmpNoteEndTime - MIN_NOTE_INTERVAL_DRAG )
											{
												tmpMessage.Format(_T("드래그 노트는 최소 %.3lf초 이상 길어야 합니다."), MIN_NOTE_INTERVAL_DRAG/(double)1000);
												AfxMessageBox(tmpMessage);

												// 플래그를 바꿔준다.
												pDoc->setOnFocusEditingFlag(false);
												pDoc->setNowEditingNote(NULL, 0);

												// 윈도우를 다시 그려준다.
												//RedrawWindow();						// 직접 그리거나, 그리라는 신호를 주는 함수
												//if ( chan )
												//{
												//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
												//	{
												//		m_timerRedrawFlag = true;
												//	}
												//	else
												//	{
												//		DrawDoubleBuffering();
												//		Invalidate();
												//	}
												//}
												//else
												//{
												//	DrawDoubleBuffering();
												//	Invalidate();
												//}

												DrawDoubleBuffering();
												Invalidate(FALSE);

												return;
											}
											break;

										case NOTE_T_CHARISMA:
											// 카리스마 노트의 경우,
											if( tmpNoteTime >= tmpNoteEndTime - MIN_NOTE_INTERVAL_CHARISMA )
											{
												tmpMessage.Format(_T("카리스마 노트는 최소 %.3lf초 이상 길어야 합니다."), MIN_NOTE_INTERVAL_CHARISMA/(double)1000);
												AfxMessageBox(tmpMessage);

												// 플래그를 바꿔준다.
												pDoc->setOnFocusEditingFlag(false);
												pDoc->setNowEditingNote(NULL, 0);

												// 윈도우를 다시 그려준다.
												//RedrawWindow();

												// 직접 그리거나, 그리라는 신호를 주는 함수
												//if ( chan )
												//{
												//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
												//	{
												//		m_timerRedrawFlag = true;
												//	}
												//	else
												//	{
												//		DrawDoubleBuffering();
												//		Invalidate();
												//	}
												//}
												//else
												//{
												//	DrawDoubleBuffering();
												//	Invalidate();
												//}

												DrawDoubleBuffering();
												Invalidate(FALSE);

												return;
											}
											break;

										case NOTE_T_PATTERN:
											// 패턴 변환 노트일 경우,
											if( tmpNoteTime >= tmpNoteEndTime - MIN_NOTE_INTERVAL_PATTERN )
											{
												tmpMessage.Format(_T("패턴 변환 노트는 최소 %.3lf초 이상 길어야 합니다."), MIN_NOTE_INTERVAL_PATTERN/(double)1000);
												AfxMessageBox(tmpMessage);

												// 플래그를 바꿔준다.
												pDoc->setOnFocusEditingFlag(false);
												pDoc->setNowEditingNote(NULL, 0);

												// 윈도우를 다시 그려준다.
												//RedrawWindow();						// 직접 그리거나, 그리라는 신호를 주는 함수
												//if ( chan )
												//{
												//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
												//	{
												//		m_timerRedrawFlag = true;
												//	}
												//	else
												//	{
												//		DrawDoubleBuffering();
												//		Invalidate();
												//	}
												//}
												//else
												//{
												//	DrawDoubleBuffering();
												//	Invalidate();
												//}

												DrawDoubleBuffering();
 												Invalidate(FALSE);

												return;
											}
											break;

										case NOTE_T_NEUTRAL:
											// 중립자세 노트일 경우,
											if( tmpNoteTime >= tmpNoteEndTime - MIN_NOTE_INTERVAL_NEUTRAL )
											{
												tmpMessage.Format(_T("드래그 노트는 최소 %.3lf초 이상 길어야 합니다."), MIN_NOTE_INTERVAL_NEUTRAL/(double)1000);
												AfxMessageBox(tmpMessage);

												// 플래그를 바꿔준다.
												pDoc->setOnFocusEditingFlag(false);
												pDoc->setNowEditingNote(NULL, 0);

												// 윈도우를 다시 그려준다.
												//RedrawWindow();
												// 직접 그리거나, 그리라는 신호를 주는 함수
												//if ( chan )
												//{
												//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
												//	{
												//		m_timerRedrawFlag = true;
												//	}
												//	else
												//	{
												//		DrawDoubleBuffering();
												//		Invalidate();
												//	}
												//}
												//else
												//{
												//	DrawDoubleBuffering();
												//	Invalidate();
												//}

 												DrawDoubleBuffering();
 												Invalidate(FALSE);

												return;
											}
											break;
										}
									}
								}


								// 알맞은 값으로 노트를 생성한다.
								if ( chkTypeToBigType(tmpNoteData->getNoteType()) == NOTE_TYPE_LINE_1 )
								{
									// 라인 1일 경우.
									// 만약 그려도 좋은 노트일 경우,
									if ( chkIsNoteDrawOk(tmpNoteData, time.noteTimeSec, time.noteTimeMilSec, tmpNoteData->getNoteType(), targetMarker) )
									{
										if( pDoc->editNote(tmpNoteData, time.noteTimeSec, time.noteTimeMilSec, tmpNoteData->getNoteType(), targetMarker) < 0 )
										{
											AfxMessageBox(_T("Editing Error!"));
										}
									}
									else
									{
										// 겹치는 노트가 있을 경우
										MessageBox(_T("겹치게 되는 노트가 있어 그릴 수 없습니다."));
									}
								}
								else
								{	
									if ( chkIsNoteDrawOk(tmpNoteData, time.noteTimeSec, time.noteTimeMilSec, tmpNoteData->getNoteType(), targetMarker) )
									{
										// 라일 2와 3일 경우에는, 타겟 마커를 그대로 유지 해 준다.
										if( pDoc->editNote(tmpNoteData, time.noteTimeSec, time.noteTimeMilSec, tmpNoteData->getNoteType(), tmpNoteData->getTargetMarker()) < 0 )
										{
											AfxMessageBox(_T("Editing Error!"));
										}
									}
									else
									{
										// 겹치는 노트가 있을 경우
										MessageBox(_T("겹치게 되는 노트가 있어 그릴 수 없습니다."));
									}

								}
								//tmpNoteData->editNoteData(time.noteTimeSec, time.noteTimeMilSec, tmpNoteData->getNoteType(), targetMarker);
							}

						}
					}
					else if( noteHeadDir == 1 )
					{
						if( ConvertPixToNoteDataRanType(point, time, noteType, targetMarker) > 0 )
						{
							if ( noteType == chkTypeToBigType(tmpNoteData->getNoteType()) )
							{
								// 노트의 타입이 같다는 가정 하에서,

								if ( tmpNoteData->getNoteType() == NOTE_T_LONG ||
									tmpNoteData->getNoteType() == NOTE_T_DRAG ||
									tmpNoteData->getNoteType() == NOTE_T_CHARISMA ||
									tmpNoteData->getNoteType() == NOTE_T_NEUTRAL ||
									tmpNoteData->getNoteType() == NOTE_T_PATTERN
									)
								{
									// 앞 뒤 시간이 꼬일 경우, 종료하게 한다.
									long tmpNoteTime = (tmpNoteData->getNoteTimeSec() * 1000) + tmpNoteData->getNoteTimeMilSec();
									long tmpNoteEndTime = (time.noteTimeSec * 1000) + time.noteTimeMilSec;

									if ( tmpNoteTime >= tmpNoteEndTime )
									{
										AfxMessageBox(_T("노트의 종료시간은 시작 시간보다 뒤여야 합니다."));

										// 플래그를 바꿔준다.
										pDoc->setOnFocusEditingFlag(false);
										pDoc->setNowEditingNote(NULL, 0);

										// 윈도우를 다시 그려준다.
										// 직접 그리거나, 그리라는 신호를 주는 함수
										//if ( chan )
										//{
										//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
										//	{
										//		m_timerRedrawFlag = true;
										//	}
										//	else
										//	{
										//		DrawDoubleBuffering();
										//		Invalidate();
										//	}
										//}
										//else
										//{
										//	DrawDoubleBuffering();
										//	Invalidate();
										//}

										DrawDoubleBuffering();
										Invalidate(FALSE);
										return;
									}
									else
									{

										// 각각의 타입 별 최대시간보다 더 클 경우, 메시지를 보내고 수정이 안 되게 한다.
										switch( tmpNoteData->getNoteType() )
										{
										case NOTE_T_DRAG:
											// 드래그 노트일 경우,
											if( tmpNoteTime >= tmpNoteEndTime - MIN_NOTE_INTERVAL_DRAG )
											{
												tmpMessage.Format(_T("드래그 노트는 최소 %.3lf초 이상 길어야 합니다."), MIN_NOTE_INTERVAL_DRAG/(double)1000);
												AfxMessageBox(tmpMessage);

												// 플래그를 바꿔준다.
												pDoc->setOnFocusEditingFlag(false);
												pDoc->setNowEditingNote(NULL, 0);

												// 윈도우를 다시 그려준다.
												//RedrawWindow();
												// 직접 그리거나, 그리라는 신호를 주는 함수
												//if ( chan )
												//{
												//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
												//	{
												//		m_timerRedrawFlag = true;
												//	}
												//	else
												//	{
												//		DrawDoubleBuffering();
												//		Invalidate();
												//	}
												//}
												//else
												//{
												//	DrawDoubleBuffering();
												//	Invalidate();
												//}

												DrawDoubleBuffering();
												Invalidate(FALSE);

												return;
											}
											break;

										case NOTE_T_CHARISMA:
											// 카리스마 노트의 경우,
											if( tmpNoteTime >= tmpNoteEndTime - MIN_NOTE_INTERVAL_CHARISMA )
											{
												tmpMessage.Format(_T("카리스마 노트는 최소 %.3lf초 이상 길어야 합니다."), MIN_NOTE_INTERVAL_CHARISMA/(double)1000);
												AfxMessageBox(tmpMessage);

												// 플래그를 바꿔준다.
												pDoc->setOnFocusEditingFlag(false);
												pDoc->setNowEditingNote(NULL, 0);

												// 윈도우를 다시 그려준다.
												//RedrawWindow();						// 직접 그리거나, 그리라는 신호를 주는 함수
												//if ( chan )
												//{
												//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
												//	{
												//		m_timerRedrawFlag = true;
												//	}
												//	else
												//	{
												//		DrawDoubleBuffering();
												//		Invalidate();
												//	}
												//}
												//else
												//{
												//	DrawDoubleBuffering();
												//	Invalidate();
												//}

												DrawDoubleBuffering();
												Invalidate(FALSE);

												return;
											}
											break;

										case NOTE_T_PATTERN:
											// 패턴 변환 노트일 경우,
											if( tmpNoteTime >= tmpNoteEndTime - MIN_NOTE_INTERVAL_PATTERN )
											{
												tmpMessage.Format(_T("패턴 변환 노트는 최소 %.3lf초 이상 길어야 합니다."), MIN_NOTE_INTERVAL_PATTERN/(double)1000);
												AfxMessageBox(tmpMessage);

												// 플래그를 바꿔준다.
												pDoc->setOnFocusEditingFlag(false);
												pDoc->setNowEditingNote(NULL, 0);

												// 윈도우를 다시 그려준다.
												//RedrawWindow();
												// 직접 그리거나, 그리라는 신호를 주는 함수
												//if ( chan )
												//{
												//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
												//	{
												//		m_timerRedrawFlag = true;
												//	}
												//	else
												//	{
												//		DrawDoubleBuffering();
												//		Invalidate();
												//	}
												//}
												//else
												//{
												//	DrawDoubleBuffering();
												//	Invalidate();
												//}

												DrawDoubleBuffering();
												Invalidate(FALSE);

												return;
											}
											break;

										case NOTE_T_NEUTRAL:
											// 중립자세 노트일 경우,
											if( tmpNoteTime >= tmpNoteEndTime - MIN_NOTE_INTERVAL_NEUTRAL )
											{
												tmpMessage.Format(_T("드래그 노트는 최소 %.3lf초 이상 길어야 합니다."), MIN_NOTE_INTERVAL_NEUTRAL/(double)1000);
												AfxMessageBox(tmpMessage);

												// 플래그를 바꿔준다.
												pDoc->setOnFocusEditingFlag(false);
												pDoc->setNowEditingNote(NULL, 0);

												// 윈도우를 다시 그려준다.
												//RedrawWindow();
												// 직접 그리거나, 그리라는 신호를 주는 함수
												//if ( chan )
												//{
												//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
												//	{
												//		m_timerRedrawFlag = true;
												//	}
												//	else
												//	{
												//		DrawDoubleBuffering();
												//		Invalidate();
												//	}
												//}
												//else
												//{
												//	DrawDoubleBuffering();
												//	Invalidate();
												//}

												DrawDoubleBuffering();
												Invalidate(FALSE);

												return;
											}
											break;
										}
									}
								}

								if ( chkIsNoteDrawOk(tmpNoteData, tmpNoteData->getNoteTimeSec(), tmpNoteData->getNoteTimeMilSec(), time, tmpNoteData->getNoteType(), targetMarker) )
								{
									// 라일 2와 3일 경우에는, 타겟 마커를 그대로 유지 해 준다.
									if( pDoc->editLongNote(tmpNoteData, tmpNoteData->getNoteTimeSec(), tmpNoteData->getNoteTimeMilSec(), tmpNoteData->getNoteType(), tmpNoteData->getTargetMarker(), time.noteTimeSec, time.noteTimeMilSec) )
									{
										AfxMessageBox(_T("Editing Error!"));
									}
								}
								else
								{
									// 겹치는 노트가 있을 경우
									MessageBox(_T("겹치게 되는 노트가 있어 그릴 수 없습니다."));
								}

								//tmpNoteData->editNoteData(tmpNoteData->getNoteTimeSec(), tmpNoteData->getNoteTimeMilSec(), tmpNoteData->getNoteType(), tmpNoteData->getTargetMarker(), time.noteTimeSec, time.noteTimeMilSec);


							}
						}
					}

				}

				// 플래그를 바꿔준다.
				pDoc->setOnFocusEditingFlag(false);
				pDoc->setNowEditingNote(NULL, 0);

				// 윈도우를 다시 그려준다.
				// 직접 그리거나, 그리라는 신호를 주는 함수
				//if ( chan )
				//{
				//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
				//	{
				//		m_timerRedrawFlag = true;
				//	}
				//	else
				//	{
				//		DrawDoubleBuffering();
				//		Invalidate();
				//	}
				//}
				//else
				//{
				//	DrawDoubleBuffering();
				//	Invalidate();
				//}

				DrawDoubleBuffering();
				Invalidate(FALSE);
			}
		}
		else if( pDoc->getNoteEditingMode() == EDIT_MODE_ERASE )
		{
			// 만약 드래그 한 다음에 클릭을 놓았을 경우, 
			CNoteData *tmpNoteData = NULL;
			int noteHeadDir = -10;

			// 노트의 정보를 받아온다.
			tmpNoteData = pDoc->getNowEditingNote(noteHeadDir);
			if( tmpNoteData == NULL )
			{


			}
			else
			{
				// 해당 노트의 삭제.
				//	if( pDoc->editNote(tmpNoteData, time.noteTimeSec, time.noteTimeMilSec, tmpNoteData->getNoteType(), targetMarker) < 0 )
				if ( pDoc->deleteNote(tmpNoteData) < 0 )
				{
					AfxMessageBox(_T("Erasing Error!"));
				}

				// 플래그를 바꿔준다.
				pDoc->setNowEditingNote(NULL, 0);
			}

			// 윈도우를 다시 그려준다.
			// 직접 그리거나, 그리라는 신호를 주는 함수
			//if ( chan )
			//{
			//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
			//	{
			//		m_timerRedrawFlag = true;
			//	}
			//	else
			//	{
			//		DrawDoubleBuffering();
			//		Invalidate();
			//	}
			//}
			//else
			//{
			//	DrawDoubleBuffering();
			//	Invalidate();
			//}

			DrawDoubleBuffering();
			Invalidate(FALSE);
		}
		else if( pDoc->getNoteEditingMode() == EDIT_MODE_CONFG )
		{
			// 수정 모드일 경우,

			bool onFucusFlag = false;
			int noteHeadDir = -1;
			CNoteData *tempNote = pDoc->getNowEditingNote(noteHeadDir);
			onFucusFlag = pDoc->getOnFocusEditingFlag();

			if( tempNote != NULL )
			{
				//if( onFucusFlag == false )
				{
					// 포커스 플래그가 올라와 있지 않고, 현재 수정중인 노트가 있을 경우.
					pDoc->setNowConfigingNote(tempNote, noteHeadDir);
					pDoc->setOnFocusEditingFlag(true);

					// 드래그 노트 뷰를 다시 그리도록 한다.
					if( pDoc->getNoteEditingToolViewPtr() != NULL )
					{
						pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW2, 0, 0);
					}
					else
					{
						AfxMessageBox(_T("피킹뷰에서 NULL Ptr 에러!"));
					}

					// 직접 그리거나, 그리라는 신호를 주는 함수
					//if ( chan )
					//{
					//	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
					//	{
					//		m_timerRedrawFlag = true;
					//	}
					//	else
					//	{
					//		DrawDoubleBuffering();
					//		Invalidate();
					//	}
					//}
					//else
					//{
					//	DrawDoubleBuffering();
					//	Invalidate();
					//}

					DrawDoubleBuffering();
					Invalidate(FALSE);
				}
			}

		}

	}

	ReleaseDC(pDC);

	CScrollView::OnLButtonUp(nFlags, point);
}





// CNotePickingView 진단

#ifdef _DEBUG
/*
void CNotePickingView::AssertValid() const
{
CView::AssertValid();
}

void CNotePickingView::Dump(CDumpContext& dc) const
{
CView::Dump(dc);
}
*/
CNoteEditingToolDoc* CNotePickingView::GetDocument() const // 디버그되지 않은 버전은 인라인으로 지정됩니다.
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CNoteEditingToolDoc)));
	return (CNoteEditingToolDoc*)m_pDocument;
}
#endif //_DEBUG


// CNoteEditingToolView 메시지 처리기


//afx_msg LRESULT CNotePickingView::OnCmRedrawDragView(WPARAM wParam, LPARAM lParam)
//{
//	return 0;
//}


void CNotePickingView::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CNoteEditingToolDoc* pDoc = GetDocument();
	HDC hdc = ::GetDC(this->m_hWnd);
	DWORD channelStatus = 0;

	switch( nChar )
	{
	case VK_SPACE:
		if ( chan == 0 )
		{
			// 음악을 로드 했다가, 새 파일을 연 상태이므로, 음악이 로드되지 않았음을 알린다.
			AfxMessageBox(_T("음악이 로드되지 않았습니다."));
		}
		else
		{
			channelStatus = BASS_ChannelIsActive(chan);
			switch( channelStatus )
			{
			case BASS_ACTIVE_STOPPED:
			case BASS_ACTIVE_PAUSED:
				// 정지 중이거나 일시정지 중일 경우, 재생
				tempPlay();
				break;

			case BASS_ACTIVE_PLAYING:
				// 재생주일 경우, 일시중지
				tempPause();
				break;

			case BASS_ACTIVE_STALLED:
				// Playback of the stream has been stalled due to a lack of sample data. The playback will automatically resume once there is sufficient data to do so.
				AfxMessageBox(_T("재생을 위한 BASS Lib에서 나올수 없는 인자 리턴됨"));
				break;

			default:
				AfxMessageBox(_T("BASS Lib 오류!"));
			}
		}

// 		if ( nowPlayingStatus == PLAY_STATE_STOP || nowPlayingStatus == PLAY_STATE_PAUSE )
// 		{
// 			// 정지 중이거나 일시정지 중일 경우, 재생
// 			tempPlay();
// 		}
// 		else if( nowPlayingStatus == PLAY_STATE_PLAY )
// 		{
// 			// 재생주일 경우, 일시중지
// 			tempPause();
// 		}
// 		else if ( nowPlayingStatus == PLAY_STATE_NO_MUSIC )
// 		{
// 			MessageBox(_T("음악이 로드되지 않았습니다."));
// 		}
// 		else
// 		{
// 			// 그 외의 경우 에러
// 			MessageBox(_T("아직 초기화가 안 되었거나 정체불명의 모드입니다.")); 
// 		}
// 		
// 		// 다시 그리라는 명령 내린다.
// 		if( pDoc->getEditModeSelectViewPtr() != NULL )
// 		{
// 			pDoc->getEditModeSelectViewPtr()->SendMessage(CM_REDRAW_VIEW2);
// 		}

		break;

	case '1':
		// 쓰기 모드로 전환
		pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE2, 0, EDIT_MODE_WRITE);
		break;

	case '2':
		// 삭제 모드로 전환
		pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE2, 0, EDIT_MODE_ERASE);
		break;

	case '3':
		// 이동 모드로 전환
		pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE2, 0, EDIT_MODE_MOVE);
		break;

	case '4':
		// 설정 모드로 전환
		pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE2, 0, EDIT_MODE_CONFG);
		break;

	case 'a':
	case 'A':
		//CDC *pDC = GetDC();

		//tempTest();

		break;



	case 's':
	case 'S':
		//PlayFile();
		break;

	case 'd':
	case 'D':
		//BitBlt(hdc, 0, 0, WIDTH,HEIGHT, wavedc, 0, 0, SRCCOPY); // draw peak waveform
		break;
	}


	CScrollView::OnKeyDown(nChar, nRepCnt, nFlags);
}


BOOL CNotePickingView::OnEraseBkgnd(CDC* pDC)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	// TODO: Add your message handler code here and/or call default
	CBrush backBrush(RGB(58, 58, 58));               // 파랑색. 원하는 컬러를 넣어주면 된다...

	CBrush* pOldBrush = pDC->SelectObject(&backBrush); 
	CRect rect; pDC->GetClipBox(&rect); 
	pDC->PatBlt(rect.left, rect.top, rect.Width(), rect.Height(), PATCOPY);
	pDC->SelectObject(pOldBrush); 

	return TRUE;                // TRUE 로 해주어야 한다. 기존것(return CView::OnEraseBkgnd(pDC);)


	//	평소때 컬러를 안바꾸더라도 WM_ERASEBKGND 를 정의하여 return TURE 로 해두면
	//	더블 버퍼링을 쓸때 화면의 바탕을 다시 그리는 일이 없게 해서 조금이나마 화면의 깜박임을 더 줄일수 있다.
	//return CScrollView::OnEraseBkgnd(pDC);
}


void CNotePickingView::OnOpenNewMusicFile()
{
	// TODO: 여기에 명령 처리기 코드를 추가합니다.
	if( BopenMusicFile() < 0 )
	{
		AfxMessageBox(_T("음악 파일을 선택하지 않았습니다."));
	}
}


void CNotePickingView::OnMnfSetting()
{
	// TODO: 여기에 명령 처리기 코드를 추가합니다.
	CNoteEditingToolDoc* pDoc = GetDocument();

	NoteTime tempTime;

	
	// MNF파일의 값을 다이얼로그 클래스에도 저장한다.
	settingDlg.mnfTitle = CString::CStringT(CA2CT(pDoc->getNoteFileTitle().c_str()));
	settingDlg.mnfArtist = CString::CStringT(CA2CT(pDoc->getNoteFileArtist().c_str()));
	settingDlg.mnfLevel = pDoc->getNoteFileLevel();
	settingDlg.mnfInitBPM =  pDoc->getNoteFileInitBPM();
	
	// 시작 시간 설정
	tempTime = pDoc->getStartSongTime();
	settingDlg.startTimeMin = (tempTime.noteTimeSec / 60);
	settingDlg.startTimeSec = (tempTime.noteTimeSec % 60);
	settingDlg.startTimeMilSec = (tempTime.noteTimeMilSec);

	// 종료 시간 설정
	tempTime = pDoc->getEndSongTime();
	settingDlg.endTimeMin = (tempTime.noteTimeSec / 60);
	settingDlg.endTimeSec = (tempTime.noteTimeSec % 60);
	settingDlg.endTimeMilSec = (tempTime.noteTimeMilSec);


	if( settingDlg.DoModal() == IDOK )
	{
		// OK를 눌렀을 경우, 정보 업데이트

		// 시작 시간 설정
		tempTime.noteTimeSec = settingDlg.startTimeMin * 60;
		tempTime.noteTimeSec += settingDlg.startTimeSec;
		tempTime.noteTimeMilSec = settingDlg.startTimeMilSec;
		pDoc->setStartSongTime(tempTime);

		// 종료 시간 설정
		tempTime.noteTimeSec = settingDlg.endTimeMin * 60;
		tempTime.noteTimeSec += settingDlg.endTimeSec;
		tempTime.noteTimeMilSec = settingDlg.endTimeMilSec;
		pDoc->setEndSongTime(tempTime);

		// 기타 정보 업데이트
		pDoc->setNoteFileTitle( std::string(CT2CA(settingDlg.mnfTitle.operator LPCWSTR())) );
		pDoc->setNoteFileArtist( std::string(CT2CA(settingDlg.mnfArtist.operator LPCWSTR())) );
		pDoc->setNoteFileLevel(settingDlg.mnfLevel);
		pDoc->setNoteFileInitBPM(settingDlg.mnfInitBPM);
	}


}


void CNotePickingView::OnRButtonDown(UINT nFlags, CPoint point)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CNoteEditingToolDoc* pDoc = GetDocument();
	
	int noteHeadDir = 0;
	CNoteData *tempNote = NULL;
	
	
	if ( pDoc->getNoteEditingMode() == EDIT_MODE_CONFG)
	{
		// 수정 모드일 때, 수정하려고 하는 포커스가 있다면,
		tempNote = pDoc->getNowEditingNote(noteHeadDir);
		if ( tempNote != NULL )
		{
			// 만약 현재 마우스를 올리고 있는 노트가 있다면,
			switch( tempNote->getNoteType() )
			{
			case NOTE_T_CHARISMA:
				// 카리스마 노트일 경우,
				charismaDlg.noteCharismaPoseNum = (int)(tempNote->getTargetMarker()) - '0';
				if( charismaDlg.DoModal() == IDOK )
				{
					tempNote->setTargetMarker((char)(charismaDlg.noteCharismaPoseNum) + '0');
				}
				break;


			case NOTE_T_PATTERN:
				// 패턴 변화일 경우.
				patternDlg.notePatternNumber = (int)(tempNote->getTargetMarker()) - '0';
				if( patternDlg.DoModal() == IDOK )
				{
					tempNote->setTargetMarker((char)(patternDlg.notePatternNumber) + '0');
				}
				break;

			
			case NOTE_T_BPM:
				// BPM 변화일 경우
				bpmCDlg.noteBPMC = (int)(0x000000ff & tempNote->getTargetMarker());
				if( bpmCDlg.DoModal() == IDOK )
				{
					tempNote->setTargetMarker((char)(bpmCDlg.noteBPMC));
				}
				break;



			}
		}
	}

	CScrollView::OnRButtonDown(nFlags, point);
}


afx_msg LRESULT CNotePickingView::OnCmScanEnd(WPARAM wParam, LPARAM lParam)
{
	// 다른 스레드로부터 메시지가 올 경우, 화면을 다시 그린다.

	DrawDoubleBuffering();
	Invalidate();


	return 0;
}


void CNotePickingView::OnTimer(UINT_PTR nIDEvent)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
//
//	KillTimer(TIMER_1_ID);
//	m_uTimer = 0;
//	CNoteEditingToolDoc* pDoc = GetDocument();
//
//
//	// 더블 버퍼링 지원용
//	CDC* pDC = GetDC();
////	CDC memDC;					// 더블버퍼링을 위한 메모리 그림버퍼
//	CDC mdcOffScreen;			// 더블버퍼링을 위한 메모리 그림버퍼
////	CDC mdcOfTimeLine;			// 더블버퍼링을 위한 메모리 그림버퍼
//
//	// 찍는 화면이 스크롤 뷰이기 때문에, 스크롤의 위치에 따른 보정이 필요하다.
//	int nVertScroll = GetScrollPos(SB_VERT);	// 세로
//	int nHorzScroll = GetScrollPos(SB_HORZ);	// 가로
//	CPoint scrollPos = CPoint(nHorzScroll, nVertScroll);	
//
//	DWORD channelActiveStatus = 0;
//	QWORD channelGetPos = 0;
//
//	switch( nIDEvent )
//	{
//	case TIMER_1_ID:
//
//
//		// 파형 그림 준비
//		mdcOffScreen.CreateCompatibleDC(pDC);
//		mdcOffScreen.SelectObject(&bmpOffScreen);
//
//
//		// 현재 채널의 상태 확인 (재생중인지, 멈췄는지)
//		channelActiveStatus = BASS_ChannelIsActive(chan);
//		if ( channelActiveStatus == BASS_ACTIVE_STOPPED )
//		{
//			// 정지중일 경우, 정지라고 정함.
//			//if ( m_uTimer )
//			//{
//			//	KillTimer(TIMER_1_ID);
//			//}
//			nowPlayingStatus = 0;
//
//
//			// 다시 그리라는 명령 내린다.
//			if( pDoc->getEditModeSelectViewPtr() != NULL )
//			{
//				pDoc->getEditModeSelectViewPtr()->SendMessage(CM_REDRAW_VIEW2);
//			}
//		}
//		else if ( channelActiveStatus == BASS_ACTIVE_PAUSED )
//		{
//			// 일시정지의 경우, 일시정지
//			//if ( m_uTimer )
//			//{
//			//	KillTimer(TIMER_1_ID);
//			//}
//			nowPlayingStatus = 2;
//
//			// 다시 그리라는 명령 내린다.
//			if( pDoc->getEditModeSelectViewPtr() != NULL )
//			{
//				pDoc->getEditModeSelectViewPtr()->SendMessage(CM_REDRAW_VIEW2);
//			}
//		}
//
//
//		// 배경을 먼저 그린 다음,
//		pDC->BitBlt(-scrollPos.x, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X, FRAME_1_HEIGHT, &mdcOffScreen, 0, 0, SRCCOPY);
//		
//		// 선을 그린다.
//		//DrawTimeLineEx(pDC->m_hDC, BASS_ChannelGetPosition(chan, BASS_POS_BYTE ), 0xffffff, 0, scrollPos); // current pos
//		channelGetPos = BASS_ChannelGetPosition(chan, BASS_POS_BYTE);
//		if ( channelGetPos == -1 )
//		{
//			// 잘못된 값을 줬을 때
//			int i = BASS_ErrorGetCode();
//			CString tempstr;
//			tempstr.Format(_T("BASS_ChannelGetPosition Error! \n Number %d"), i);
//			AfxMessageBox(tempstr);
//		}
//		DrawTimeLineEx(pDC->m_hDC, channelGetPos, 0xffffff, 0, scrollPos); // current pos
//
//		//// 다시 타이머 설정
//
//		m_uTimer = SetTimer(TIMER_1_ID, TIMER_1_MSEC, NULL);					// 타이머 설정 20hz
//		if ( !m_uTimer )
//		{
//			AfxMessageBox(_T("타이머를 설정할 수 없습니다."));
//		}
//
//		break;
//	}

	CScrollView::OnTimer(nIDEvent);
}


afx_msg LRESULT CNotePickingView::OnCmRedrawDc(WPARAM wParam, LPARAM lParam)
{
	DrawDoubleBuffering();
	Invalidate();

	return 0;
}


afx_msg LRESULT CNotePickingView::OnCmVkSpace(WPARAM wParam, LPARAM lParam)
{
	// Space Bar를 누른 것과 같은 행동을 하는 함수.
	if ( wParam != 0 )
	{
		// 기본값이 아닐 경우,
		// 모드를 수동으로 선택했을 때 lParam에 따라서 다르다.
		/*
		if ( nowPlayingStatus == PLAY_STATE_STOP || nowPlayingStatus == PLAY_STATE_PAUSE )
		{
			// 정지 중이거나 일시정지 중일 경우, 재생
			tempPlay();
		}
		else if( nowPlayingStatus == PLAY_STATE_PLAY )
		{
			// 재생주일 경우, 일시중지
			tempPause();
		}
		else if ( nowPlayingStatus == PLAY_STATE_NO_MUSIC )
		{
			MessageBox(_T("음악이 로드되지 않았습니다."));
		}
		else
		{
			// 그 외의 경우 에러
			MessageBox(_T("아직 초기화가 안 되었거나 정체불명의 모드입니다.")); 
		}
		*/

		
		switch( lParam )
		{
		case PLAY_STATE_PLAY:
			if ( nowPlayingStatus == PLAY_STATE_STOP || nowPlayingStatus == PLAY_STATE_PAUSE )
			{
				tempPlay();
			}
			else if( nowPlayingStatus == PLAY_STATE_PLAY )
			{
				// 재생주일 경우,
				MessageBox(_T("이미 재생중입니다. (예외처리 되지 않은 신호)"));
			}
			else if ( nowPlayingStatus == PLAY_STATE_NO_MUSIC )
			{
				MessageBox(_T("음악이 로드되지 않았습니다."));
			}
			else
			{
				// 그 외의 경우 에러
				MessageBox(_T("아직 초기화가 안 되었거나 정체불명의 모드입니다.")); 
			}
			break;

		case PLAY_STATE_PAUSE:
			if ( nowPlayingStatus == PLAY_STATE_STOP || nowPlayingStatus == PLAY_STATE_PAUSE )
			{
				// 정지 중이거나 일시정지 중일 경우, 아무런 작동하지 않는다.
				// 정지->일시정지 때에도 먹통으로 처리.
			}
			else if( nowPlayingStatus == PLAY_STATE_PLAY )
			{
				// 재생주일 경우, 일시중지
				tempPause();
			}
			else if ( nowPlayingStatus == PLAY_STATE_NO_MUSIC )
			{
				MessageBox(_T("음악이 로드되지 않았습니다."));
			}
			else
			{
				// 그 외의 경우 에러
				MessageBox(_T("아직 초기화가 안 되었거나 정체불명의 모드입니다.")); 
			}
			break;

		case PLAY_STATE_STOP:
			if ( nowPlayingStatus == PLAY_STATE_STOP )
			{
				// 정지 중이거나 일시정지 중일 경우, 아무런 작동하지 않는다.
			}
			else if( nowPlayingStatus == PLAY_STATE_PLAY || 
				nowPlayingStatus == PLAY_STATE_PAUSE  )
			{
				// 일시정지->정지 때에는 반응.
				// 재생주일 경우, 정지
				tempStop();
			}
			else if ( nowPlayingStatus == PLAY_STATE_NO_MUSIC )
			{
				MessageBox(_T("음악이 로드되지 않았습니다."));
			}
			else
			{
				// 그 외의 경우 에러
				MessageBox(_T("아직 초기화가 안 되었거나 정체불명의 모드입니다.")); 
			}
			break;

		default:
			// 정체불명의 값이 들어왔을 경우, 무시.
			AfxMessageBox(_T("CNotePickingView::OnCmVkSpace에서 영 좋지 않은 상태1"));
			return 0;
		}
		
	}
	else
	{
		if ( nowPlayingStatus == PLAY_STATE_STOP || nowPlayingStatus == PLAY_STATE_PAUSE )
		{
			// 정지 중이거나 일시정지 중일 경우, 재생
			tempPlay();
		}
		else if( nowPlayingStatus == PLAY_STATE_PLAY )
		{
			// 재생주일 경우, 일시중지
			tempPause();
		}
		else if ( nowPlayingStatus == PLAY_STATE_NO_MUSIC )
		{
			MessageBox(_T("음악이 로드되지 않았습니다."));
		}
		else
		{
			// 그 외의 경우 에러
			MessageBox(_T("아직 초기화가 안 되었거나 정체불명의 모드입니다.")); 
		}
	}


	return 0;
}


void CNotePickingView::MMTimerHandler(UINT nIDEvent) // called every elTime milliseconds
{
	// do what you want to do, but quickly

	//destroy the timer
	timeKillEvent(m_idEvent);
	//reset the timer
	//timeEndPeriod (m_uResolution);
	m_idEvent = NULL;

	/*
	CNoteEditingToolDoc* pDoc = GetDocument();

	
	// 더블 버퍼링 지원용
	CDC* pDC = GetDC();
	//	CDC memDC;					// 더블버퍼링을 위한 메모리 그림버퍼
	CDC mdcOffScreen;			// 더블버퍼링을 위한 메모리 그림버퍼
	//	CDC mdcOfTimeLine;			// 더블버퍼링을 위한 메모리 그림버퍼

	// 찍는 화면이 스크롤 뷰이기 때문에, 스크롤의 위치에 따른 보정이 필요하다.
	//int nHorzScroll = 0;	// 가로
	//int nVertScroll = GetScrollPos(SB_VERT);	// 세로
	//int nVertScroll = 0;	// 세로
	//int nHorzScroll = GetScrollPos(SB_HORZ);	// 가로
	
	//CPoint scrollPos = CPoint(nHorzScroll, nVertScroll);	
	CPoint scrollPos = GetScrollPosition();

	DWORD channelActiveStatus = 0;
	QWORD channelGetPos = 0;


	if ( m_timerRedrawFlag == true )
	{
		DrawDoubleBuffering();
		m_timerRedrawFlag = false;
	}
	*/


	//mdcOffScreen.CreateCompatibleDC(pDC);
	//mdcOffScreen.SelectObject(&bmpOffScreen);



	// 색 칠하는 용.
	//CBrush newBrush(RGB(255, 255, 255));
	//CBrush* oldBrush;

	
	// 배경을 먼저 그린 다음,
	//pDC->BitBlt(-scrollPos.x, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X, FRAME_1_HEIGHT, &mdcOffScreen, 0, 0, SRCCOPY);
	//pDC->BitBlt(0, 0, (int)(WIDTH * BSS_WAVE_FORM_RATE_REAL) + V_NOTE_SET_OFF_X, FRAME_1_HEIGHT, &mdcOffScreen, 0, 0, SRCCOPY);

	//// 선을 그린다.
	////DrawTimeLineEx(pDC->m_hDC, BASS_ChannelGetPosition(chan, BASS_POS_BYTE ), 0xffffff, 0, scrollPos); // current pos
	//channelGetPos = BASS_ChannelGetPosition(chan, BASS_POS_BYTE);
	//if ( channelGetPos == -1 )
	//{
	//	// 잘못된 값을 줬을 때
	//	int i = BASS_ErrorGetCode();
	//	CString tempstr;
	//	tempstr.Format(_T("BASS_ChannelGetPosition Error! \n Number %d"), i);
	//	AfxMessageBox(tempstr);
	//}
	//DrawTimeLineEx(pDC->m_hDC, channelGetPos, 0xffffff, 0, scrollPos); // current pos

	mutexFlag = true;


	/////////// 마우스 핸들 확인하는 부분.
	/*
	switch( chkMouseHandleState )
	{
	case TIMER_1_FLAG_NULL:
		break;

	case TIMER_1_FLAG_UP:
		// 마우스가 올라가는 핸들러가 왔을 경우.	 

		if( pDoc->getNoteEditingMode() == EDIT_MODE_WRITE )
		{
			if ( point.x >= (int)(V_NOTE_SET_OFF_X + V_UNUSED_TIMELINE) )
			{
				// ↑ 이미 nHorzScroll이 더해져 있는 상태이므로 또 더하지 않는다.

				// 마우스의 좌표를 적절히 분석 한 다음,
				if( ConvertPixToNoteData(point, time, noteType, targetMarker) > 0 )
				{
					// 만약 그려도 좋은 노트일 경우,
					if ( chkIsNoteDrawOk(NULL, time.noteTimeSec, time.noteTimeMilSec, noteType, targetMarker) )
					{
						// 알맞은 값으로 노트를 생성한다.
						pDoc->addNewNote(time.noteTimeSec, time.noteTimeMilSec, noteType, targetMarker);
						DrawDoubleBuffering();
						Invalidate();
					}
					else
					{
						// 겹치는 노트가 있을 경우
						MessageBox(_T("겹치게 되는 노트가 있어 그릴 수 없습니다."));

					}
				}
			}
		}


		break;


	case TIMER_1_FLAG_MOVE:


		break;

	}
	
	// 다음 타이머 사용을 위한 초기화
	onTimerMousePoint.x = 0;
	onTimerMousePoint.y = 0;
	chkMouseHandleState = TIMER_1_FLAG_NULL;

	*/













	Invalidate(FALSE);


	if ( BASS_ChannelIsActive(chan) == BASS_ACTIVE_PLAYING )
	{
		// 다시 타이머 설정
		m_idEvent = timeSetEvent(TIMER_1_MSEC, m_uResolution, this->TimerFunction, (DWORD)this, TIME_PERIODIC);
		if ( m_idEvent == NULL )
		{
			// 타이머 설정에 실패
			AfxMessageBox(_T("타이머 동작 실패! 2"));
		}
	}
	else if (  BASS_ChannelIsActive(chan) == BASS_ACTIVE_STOPPED )
	{
		tempStop();
	}
}

void CALLBACK CNotePickingView::TimerFunction(UINT wTimerID, UINT msg,
	DWORD dwUser, DWORD dw1, DWORD dw2)
{
	// This is used only to call MMTimerHandler

	// Typically, this function is static member of CTimersDlg

	if( useByLoopSyncProc->mutexFlag )
	{
		CNotePickingView* obj = (CNotePickingView*) dwUser;
		obj->MMTimerHandler(wTimerID);
	}
}



