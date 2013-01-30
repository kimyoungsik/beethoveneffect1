// BassLibControl.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "BassLibControl.h"
#include "afxdialogex.h"


// CBassLibControl 대화 상자입니다.

IMPLEMENT_DYNAMIC(CBassLibControl, CDialog)



static CBassLibControl* useByLoopSyncProc;						// static 함수를 사용하기 위해 this포인터를 받아오기 위한 전역변수??
QWORD CBassLibControl::loop[2] = {0};


CBassLibControl::CBassLibControl(CWnd* pParent /*=NULL*/)
	: CDialog(CBassLibControl::IDD, pParent)
{

	// BASS Lib에서 사용하기 위한 특성입니다.
	this->win = NULL;
	this->scanthread = 0;
	this->killscan = FALSE;


	//	this->loop[0] = 0;	// loop start & end
	//	this->loop[1] = 0;

	this->wavedc = 0;
	this->wavebmp = 0;

	useByLoopSyncProc = this;			// 자신의 포인터를 등록


	// 끝.




	EnableAutomation();

}

CBassLibControl::~CBassLibControl()
{
}







//////////////////////////////////////////////////////////////////////////////
//////////////////////////////   [BASS START]   //////////////////////////////
//////////////////////////////////////////////////////////////////////////////







void CBassLibControl::Error(const TCHAR *es)
{
	TCHAR tempBuff[100];
	_stprintf(tempBuff, _T("%s\n(error code: %d)\n"),es,BASS_ErrorGetCode());

	MessageBox(tempBuff);
}

void CALLBACK CBassLibControl::LoopSyncProc(HSYNC handle, DWORD channel, DWORD data, void *user)
{
	// Static함수 이므로, 이 함수 안에서 Member Function을 사용하려면,
	// useByLoopSyncProc->함수명() 과 같은 형태로 사용해야만 한다.

	//if (  !BASS_ChannelSetPosition(channel, loop[0], BASS_POS_BYTE)   ) // try seeking to loop start
	CBassLibControl *tempThisPointer = useByLoopSyncProc;

	if (  !BASS_ChannelSetPosition(channel, tempThisPointer->loop[0], BASS_POS_BYTE)   ) // try seeking to loop start
	{
		BASS_ChannelSetPosition(channel,0,BASS_POS_BYTE); // failed, go to start of file instead
	}

}


void CBassLibControl::SetLoopStart(QWORD pos)
{
	loop[0]=pos;
}

void CBassLibControl::SetLoopEnd(QWORD pos)
{
	loop[1]=pos;
	BASS_ChannelRemoveSync(chan,lsync); // remove old sync
	lsync = BASS_ChannelSetSync(chan,BASS_SYNC_POS|BASS_SYNC_MIXTIME,loop[1],LoopSyncProc,0); // set new sync
}


void __cdecl CBassLibControl::ScanPeaks(void *p)
{
	CBassLibControl *tempThisPointer = useByLoopSyncProc;
	// Static 함수에 대해서 강제적으로 얻은 Pointer.




	DWORD decoder=(DWORD)p;
	DWORD cpos=0,peak[2]={0};
	while ( ! useByLoopSyncProc->killscan )
	{
		DWORD level=BASS_ChannelGetLevel(decoder); // scan peaks
		DWORD pos;
		if (peak[0]<LOWORD(level)) peak[0]=LOWORD(level); // set left peak
		if (peak[1]<HIWORD(level)) peak[1]=HIWORD(level); // set right peak
		if (!BASS_ChannelIsActive(decoder)) pos=-1; // reached the end
		else pos = (DWORD)( BASS_ChannelGetPosition(decoder,BASS_POS_BYTE) / (useByLoopSyncProc->bpp));
		if (pos>cpos) {
			DWORD a;
			for (a=0;a<peak[0]*(HEIGHT/2)/32768;a++)
			{
				(useByLoopSyncProc->wavebuf)[(HEIGHT/2-1-a)*WIDTH+cpos]= (BYTE)(1+a); // draw left peak
			}
			for (a=0;a<peak[1]*(HEIGHT/2)/32768;a++)
			{
				(useByLoopSyncProc->wavebuf)[(HEIGHT/2+1+a)*WIDTH+cpos] = (BYTE)(1+a); // draw right peak
			}
			if (pos>=WIDTH) break; // gone off end of display
			cpos=pos;
			peak[0]=peak[1]=0;
		}
	}
	BASS_StreamFree(decoder); // free the decoder
	useByLoopSyncProc->scanthread=0;
}



// select a file to play, and start scanning it
BOOL CBassLibControl::PlayFile()
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
		for (a=1;a<=HEIGHT/2;a++)
		{
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


void CBassLibControl::DrawTimeLine(HDC dc, QWORD pos, DWORD col, DWORD y)
{
	HPEN pen = CreatePen(PS_SOLID,0,col),oldpen;
	DWORD wpos = (DWORD)(pos/bpp);
	DWORD time = (DWORD)(BASS_ChannelBytes2Seconds(chan,pos));
	char text[10];
	sprintf(text,"%u:%02u",time/60,time%60);
	oldpen = (HPEN)SelectObject(dc,pen);
	MoveToEx(dc,wpos,0,NULL);
	LineTo(dc,wpos,HEIGHT);
	SetTextColor(dc,col);
	SetBkMode(dc,TRANSPARENT);
	SetTextAlign(dc,wpos>=WIDTH/2?TA_RIGHT:TA_LEFT);
	//	 TextOut(dc,wpos,y,text,strlen(text));
	SelectObject(dc,oldpen);
	DeleteObject(pen);
}


// window procedure
long FAR PASCAL CBassLibControl::SpectrumWindowProc(HWND h, UINT m, WPARAM w, LPARAM l)
{
	CBassLibControl *tempThisPointer = useByLoopSyncProc;
	// Static 함수에 대해서 강제적으로 얻은 Pointer.



	switch (m) {
	case WM_LBUTTONDOWN: // set loop start
		tempThisPointer->SetLoopStart(LOWORD(l)* (tempThisPointer->bpp));
		return 0;
	case WM_RBUTTONDOWN: // set loop end
		tempThisPointer->SetLoopEnd(LOWORD(l)* (tempThisPointer->bpp));
		return 0;
	case WM_MOUSEMOVE:
		if (w&MK_LBUTTON) tempThisPointer->SetLoopStart(LOWORD(l)* (tempThisPointer->bpp));
		if (w&MK_RBUTTON) tempThisPointer->SetLoopEnd(LOWORD(l)* (tempThisPointer->bpp));
		return 0;

	case WM_TIMER:
		::InvalidateRect(h,0,0); // refresh window
		return 0;
	case WM_PAINT:
		if (::GetUpdateRect(h,0,0)) {
			PAINTSTRUCT p;
			HDC dc;
			if (!(dc = ::BeginPaint(h,&p))) return 0;
			BitBlt(dc, 0, 0, WIDTH,HEIGHT, tempThisPointer->wavedc, 0, 0, SRCCOPY); // draw peak waveform
			tempThisPointer->DrawTimeLine(dc,loop[0],0xffff00,12); // loop start
			tempThisPointer->DrawTimeLine(dc,loop[1],0x00ffff,24); // loop end
			tempThisPointer->DrawTimeLine(dc,BASS_ChannelGetPosition( tempThisPointer->chan, BASS_POS_BYTE ), 0xffffff,0); // current pos
			::EndPaint(h, &p);
		}
		return 0;

	case WM_CREATE:
		tempThisPointer->win = h;
		// initialize output
		if (!BASS_Init(-1,44100,0,tempThisPointer->win,NULL)) {
			tempThisPointer->Error(_T("Can't initialize device"));
			return -1;
		}
		if (!(tempThisPointer->PlayFile())) { // start a file playing
			BASS_Free();
			return -1;
		}
		::SetTimer(h,0,100,0); // set update timer (10hz)
		break;

	case WM_DESTROY:
		::KillTimer(h,0);
		if (tempThisPointer->scanthread) { // still scanning
			tempThisPointer->killscan = TRUE;
			WaitForSingleObject((HANDLE)(tempThisPointer->scanthread),1000); // wait for the thread
		}
		BASS_Free();
		if (tempThisPointer->wavedc)
		{
			DeleteDC(tempThisPointer->wavedc);
		}
		if (tempThisPointer->wavebmp)
		{
			DeleteObject(tempThisPointer->wavebmp);
		}
		PostQuitMessage(0);
		break;
	}

	return ::DefWindowProc(h, m, w, l);
}



int CBassLibControl::pseudoMain(HINSTANCE hInstance)
{
	WNDCLASS wc;
	MSG msg;

	// check the correct BASS was loaded
	if (HIWORD(BASS_GetVersion())!=BASSVERSION) {
		::MessageBox(0, _T("An incorrect version of BASS.DLL was loaded"),0,MB_ICONERROR);
		return 0;
	}

	// register window class and create the window
	memset(&wc,0,sizeof(wc));
	wc.lpfnWndProc = SpectrumWindowProc;
	wc.hInstance = AfxGetInstanceHandle();//hInstance;
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.lpszClassName = _T("BASS-CustLoop");

	if (!RegisterClass(&wc))
	{
		Error(_T("Can't create window"));
		return 0;
	}

	if( !CreateWindowW(_T("BASS-CustLoop"),
		_T("BASS custom looping example (left-click to set loop start, right-click to set end)"),
		WS_POPUPWINDOW|WS_CAPTION|WS_VISIBLE, 100, 100,
		WIDTH+2*GetSystemMetrics(SM_CXDLGFRAME),
		HEIGHT+GetSystemMetrics(SM_CYCAPTION)+2*GetSystemMetrics(SM_CYDLGFRAME),
		NULL, NULL, hInstance, NULL))
	{
		Error(_T("Can't create window"));
		return 0;
	}
	::ShowWindow(win, SW_SHOWNORMAL);

	while (GetMessage(&msg,NULL,0,0)>0) {
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}


	return 0;
}



void CBassLibControl::tempTest()
{




	if (!BASS_Init(-1,44100,0,win,NULL))
	{
		::MessageBox(0, _T("Bass 라이브러리 초기화 실패"),_T("에러"),16);
	}

	if (!(chan=BASS_StreamCreateFile(FALSE,"Deep Purple - Hush.mp3",0,0,0))
		&& !(chan=BASS_MusicLoad(FALSE,"Deep Purple - Hush.mp3",0,0,BASS_MUSIC_RAMPS|BASS_MUSIC_POSRESET|BASS_MUSIC_PRESCAN,1))) 
	{
		::MessageBox(0, _T("C3.mp3 로드 실패"), _T("에러"), 16);
	}


	BASS_ChannelSetSync(chan, BASS_SYNC_END | BASS_SYNC_MIXTIME, 0, LoopSyncProc,0); // set sync to loop at end
	BASS_ChannelPlay(chan, FALSE); // start playing
}





//////////////////////////////////////////////////////////////////////////////
//////////////////////////////    [BASS END]    //////////////////////////////
//////////////////////////////////////////////////////////////////////////////




void CBassLibControl::OnFinalRelease()
{
	// 자동화 개체에 대한 마지막 참조가 해제되면
	// OnFinalRelease가 호출됩니다. 기본 클래스에서 자동으로 개체를 삭제합니다.
	// 기본 클래스를 호출하기 전에 개체에 필요한 추가 정리 작업을
	// 추가하십시오.

	CDialog::OnFinalRelease();
}

void CBassLibControl::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CBassLibControl, CDialog)
END_MESSAGE_MAP()

BEGIN_DISPATCH_MAP(CBassLibControl, CDialog)
END_DISPATCH_MAP()

// 참고: IID_IBassLibControl에 대한 지원을 추가하여
//  VBA에서 형식 안전 바인딩을 지원합니다. 
//  이 IID는 .IDL 파일에 있는 dispinterface의 GUID와 일치해야 합니다.

// {2C7ACB60-4AF1-43D5-A423-C1082666FAAB}
static const IID IID_IBassLibControl =
{ 0x2C7ACB60, 0x4AF1, 0x43D5, { 0xA4, 0x23, 0xC1, 0x8, 0x26, 0x66, 0xFA, 0xAB } };

BEGIN_INTERFACE_MAP(CBassLibControl, CDialog)
	INTERFACE_PART(CBassLibControl, IID_IBassLibControl, Dispatch)
END_INTERFACE_MAP()


// CBassLibControl 메시지 처리기입니다.
