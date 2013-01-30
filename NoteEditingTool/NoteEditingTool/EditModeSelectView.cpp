// EditModeSelectView.cpp : 구현 파일입니다.
//

#include "stdafx.h"
#include "NoteEditingTool.h"
#include "EditModeSelectView.h"


#include "NoteEditingToolDoc.h"
#include "NoteData.h"
#include "NoteEditingToolView.h"
#include "NotePickingView.h"



// CEditModeSelectView

UINT CM_REDRAW_DRAG_VIEW3 = RegisterWindowMessage(_T("CM_REDRAW_DRAG_VIEW"));

IMPLEMENT_DYNCREATE(CEditModeSelectView, CView)

CEditModeSelectView::CEditModeSelectView()
{
	// OnCreate나 OnInit 함수가 없으므로 그냥 생성자에 넣음.

	//CNoteEditingToolDoc* pDoc = GetDocument();
	//
	//// 편집 모드 검사
	//char noteEditingMode = pDoc->getNoteEditingMode();
	//char noteWriteType = pDoc->getNoteWriteType();
	//int testFlag = calSelectedMenuCursor( noteWriteType );
	//// 쓰기 모드 검사
	//if( testFlag < 0 )
	//{
	//	return;
	//}

	onMouseTextFlag = false;
	onMouseEditModeFlag = false;

}

CEditModeSelectView::~CEditModeSelectView()
{
}

void CEditModeSelectView::OnInitialUpdate()     // 생성된 후 처음입니다.
{
	CView::OnInitialUpdate();


	CNoteEditingToolDoc* pDoc = GetDocument();

	pDoc->setEditModeSelectViewPtr(this);
}


UINT CM_VK_SPACE2 = RegisterWindowMessage(_T("CM_VK_SPACE"));
UINT CM_REDRAW_VIEW = RegisterWindowMessage(_T("CM_REDRAW_VIEW"));
UINT CM_EDITING_MODE_CHANGE = RegisterWindowMessage(_T("CM_EDITING_MODE_CHANGE"));


BEGIN_MESSAGE_MAP(CEditModeSelectView, CView)
	ON_WM_MOUSEMOVE()
	ON_WM_LBUTTONDOWN()
	ON_REGISTERED_MESSAGE(CM_REDRAW_VIEW, &CEditModeSelectView::OnCmRedrawView)
	ON_WM_KEYDOWN()
	ON_WM_ERASEBKGND()
	ON_REGISTERED_MESSAGE(CM_EDITING_MODE_CHANGE, &CEditModeSelectView::OnCmEditingModeChange)
END_MESSAGE_MAP()









//////////////////////////////////////////////////////////////////////////////
//////////////////////////////   [protected]    //////////////////////////////
//////////////////////////////////////////////////////////////////////////////



// 클릭했을때의 마우스 위치의 따른 좌표를 계산해서 출력하는 함수.
int CEditModeSelectView::calSelectedMenuCursor(const char noteWriteType, int &selectedMenuCursorX, int &selectedMenuCursorY)
{
	switch( noteWriteType )
	{
	case NOTE_T_RIGHT:
		selectedMenuCursorX = V_SELECTED_1_X;
		selectedMenuCursorY = V_SELECTED_1_Y;
		return 1;
		break;

	case NOTE_T_LEFT:
		selectedMenuCursorX = V_SELECTED_2_X;
		selectedMenuCursorY = V_SELECTED_2_Y;
		return 1;
		break;

	case NOTE_T_LONG:
		selectedMenuCursorX = V_SELECTED_3_X;
		selectedMenuCursorY = V_SELECTED_3_Y;
		return 1;
		break;


	case NOTE_T_DRAG:
		selectedMenuCursorX = V_SELECTED_4_X;
		selectedMenuCursorY = V_SELECTED_4_Y;
		return 1;
		break;

	case NOTE_T_CHARISMA:
		selectedMenuCursorX = V_SELECTED_5_X;
		selectedMenuCursorY = V_SELECTED_5_Y;
		return 1;
		break;

	case NOTE_T_NEUTRAL:
		selectedMenuCursorX = V_SELECTED_6_X;
		selectedMenuCursorY = V_SELECTED_6_Y;
		return 1;
		break;

	case NOTE_T_PATTERN:
		selectedMenuCursorX = V_SELECTED_7_X;
		selectedMenuCursorY = V_SELECTED_7_Y;
		return 1;
		break;


	case NOTE_T_BPM:
		selectedMenuCursorX = V_SELECTED_8_X;
		selectedMenuCursorY = V_SELECTED_8_Y;
		return 1;
		break;

	case NOTE_T_PHOTO:
		selectedMenuCursorX = V_SELECTED_9_X;
		selectedMenuCursorY = V_SELECTED_9_Y;
		return 1;
		break;


	}
	
	// 아무 해당사항이 없을 경우
	return -1;
}


// 현재 선택 한 에디팅 모드에 따른 변수들의 좌표 설정하는 함수.
int CEditModeSelectView::calSelectedModeCursor(const char editModeType, int &selectedMenuCursorX, int &selectedMenuCursorY)
{

	switch( editModeType )
	{
	case EDIT_MODE_WRITE:
		selectedMenuCursorX = V_SELECTED_PEN_X;
		selectedMenuCursorY = V_SELECTED_PEN_Y;
		return 1;
		break;

	case EDIT_MODE_ERASE:
		selectedMenuCursorX = V_SELECTED_ERASE_X;
		selectedMenuCursorY = V_SELECTED_ERASE_Y;
		return 1;
		break;

	case EDIT_MODE_MOVE:
		selectedMenuCursorX = V_SELECTED_MOD_X;
		selectedMenuCursorY = V_SELECTED_MOD_Y;
		return 1;
		break;

	case EDIT_MODE_CONFG:
		selectedMenuCursorX = V_SELECTED_CONFG_X;
		selectedMenuCursorY = V_SELECTED_CONFG_Y;
		return 1;
		break;
	}


	// 아무 해당사항이 없을 경우
	return -1;
}



// 현재 마우스 위치의 따른 좌표를 계산해서 출력하는 함수.
char CEditModeSelectView::calMouseSelectArea(const int pointX, const int pointY)
{
	CNoteEditingToolDoc* pDoc = GetDocument();

	// 먼저 y영역부터 확인
	if( pointY >= V_SELECTED_PEN_Y && pointY <= V_SELECTED_9_Y + V_TEXT_AREA_HEIGHT )
	{
		// 각각의 세부적인 y의 영역에 들어가는 지 확인
		if( pointY <= V_SELECTED_PEN_Y + V_MODE_AREA_HEIGHT)
		{
			// 연필 그리기 메뉴 안에 있을 경우. 각각의 x좌표의 위치를 검사한다.

			if( pointX >= V_SELECTED_PEN_X && pointX <= V_SELECTED_PEN_X + V_MODE_AREA_WIDTH)
			{
				// 쓰기 모드에서는 필요 없다.
				pDoc->setOnFocusEditingFlag(false);
				pDoc->setNowEditingNote(NULL, 0);

				return EDIT_MODE_WRITE;
			}
			else if( pointX >= V_SELECTED_ERASE_X && pointX <= V_SELECTED_ERASE_X + V_MODE_AREA_WIDTH)
			{
				// 삭제 모드에서는 초기화
				pDoc->setOnFocusEditingFlag(false);
				pDoc->setNowEditingNote(NULL, 0);

				return EDIT_MODE_ERASE;
			}
			else if( pointX >= V_SELECTED_MOD_X && pointX <= V_SELECTED_MOD_X + V_MODE_AREA_WIDTH)
			{
				pDoc->setOnFocusEditingFlag(false);
				pDoc->setNowEditingNote(NULL, 0);

				return EDIT_MODE_MOVE;
			}
			else if( pointX >= V_SELECTED_CONFG_X && pointX <= V_SELECTED_CONFG_X + V_MODE_AREA_WIDTH)
			{
				pDoc->setOnFocusEditingFlag(false);
				pDoc->setNowEditingNote(NULL, 0);

				return EDIT_MODE_CONFG;
			}

		}
		else if( pointY <= (V_SELECTED_9_Y + V_TEXT_AREA_HEIGHT) )
		{
			// 쓰기 선택 모드안에 있을 경우.
			if( pointX >= V_SELECTED_1_X && pointX <= (V_SELECTED_1_X + V_TEXT_AREA_WIDTH) )
			{
				// 적절한 x좌표 안에 있는 지 확인 한 다음, 각각의 y좌표의 위치를 검사한다.

				if( pointY >= V_SELECTED_1_Y && pointY <= V_SELECTED_1_Y+V_TEXT_AREA_HEIGHT)
				{
					return NOTE_T_RIGHT;
				}
				else if( pointY >= V_SELECTED_2_Y && pointY <= (V_SELECTED_2_Y + V_TEXT_AREA_HEIGHT) )
				{
					return NOTE_T_LEFT;
				}
				else if( pointY >= V_SELECTED_3_Y && pointY <= (V_SELECTED_3_Y + V_TEXT_AREA_HEIGHT) )
				{
					return NOTE_T_LONG;
				}
				else if( pointY >= V_SELECTED_4_Y && pointY <= (V_SELECTED_4_Y + V_TEXT_AREA_HEIGHT) )
				{
					return NOTE_T_DRAG;
				}
				else if( pointY >= V_SELECTED_5_Y && pointY <= (V_SELECTED_5_Y + V_TEXT_AREA_HEIGHT) )
				{
					return NOTE_T_CHARISMA;
				}
				else if( pointY >= V_SELECTED_6_Y && pointY <= (V_SELECTED_6_Y + V_TEXT_AREA_HEIGHT) )
				{
					return NOTE_T_NEUTRAL;
				}
				else if( pointY >= V_SELECTED_7_Y && pointY <= (V_SELECTED_7_Y + V_TEXT_AREA_HEIGHT) )
				{
					return NOTE_T_PATTERN;
				}
				else if( pointY >= V_SELECTED_8_Y && pointY <= (V_SELECTED_8_Y + V_TEXT_AREA_HEIGHT) )
				{
					return NOTE_T_BPM;
				}
				else if( pointY >= V_SELECTED_9_Y && pointY <= (V_SELECTED_9_Y + V_TEXT_AREA_HEIGHT) )
				{
					return NOTE_T_PHOTO;
				}
				// 위 사항에서 아무것도 해당하지 않을 경우, 그냥 종료.
			}
		}


	}

	// 다른 영역일 때에는 이것을 출력
	//onMouseTextFlag = false;
	//onMouseEditModeFlag = false;
	return '!';
}



// 현재 마우스 위치의 따른 좌표를 계산해서 출력하는 함수.
char CEditModeSelectView::calMouseMoveArea(const int pointX, const int pointY)
{


	return 0;
}



// 플레이 상태의 형태를 클릭했을때의 마우스 위치의 따른 좌표를 계산해서 출력하는 함수.
int CEditModeSelectView::calMouseSelectPlayArea(const int pointX, const int pointY)
{
	CNoteEditingToolDoc* pDoc = GetDocument();


	// 먼저 y영역부터 확인

	if( pointY >= V_SELECTED_P_PLAY_Y &&
		pointY <= V_SELECTED_P_PLAY_Y + V_MODE_AREA_HEIGHT)
	{
		// 연필 그리기 메뉴 안에 있을 경우. 각각의 x좌표의 위치를 검사한다.

		if( pointX >= V_SELECTED_P_PLAY_X && pointX <= V_SELECTED_P_PLAY_X + V_MODE_AREA_WIDTH)
		{
			// 재생
			return PLAY_STATE_PLAY;
		}
		else if( pointX >= V_SELECTED_P_PAUSE_X && pointX <= V_SELECTED_P_PAUSE_X + V_MODE_AREA_WIDTH)
		{
			// 멈춤
			return PLAY_STATE_PAUSE;
		}
		else if( pointX >= V_SELECTED_P_STOP_X && pointX <= V_SELECTED_P_STOP_X + V_MODE_AREA_WIDTH)
		{
			// 일시정지
			return PLAY_STATE_STOP;
		}
	}

	return -99;
}



// 현재 선택 한 에디팅 모드에 따른 변수들의 좌표 설정하는 함수.
int CEditModeSelectView::calSelectedPlayCursor(const int nowPlayingStatus, int &selectedMenuCursorX, int &selectedMenuCursorY)
{

	switch( nowPlayingStatus )
	{
	case PLAY_STATE_NO_MUSIC:
		// 음악이 없을 경우, 그리지 않도록 한다.
		return -1;
		break;

	case PLAY_STATE_NON_INIT:
		// 초기화가 되지 않았을 경우, 그리지 않도록 한다.
		return -1;
		break;

	case PLAY_STATE_PLAY:
		selectedMenuCursorX = V_SELECTED_P_PLAY_X;
		selectedMenuCursorY = V_SELECTED_P_PLAY_Y;
		return 1;
		break;

	case PLAY_STATE_STOP:
		selectedMenuCursorX = V_SELECTED_P_STOP_X;
		selectedMenuCursorY = V_SELECTED_P_STOP_Y;
		return 1;
		break;

	case PLAY_STATE_PAUSE:
		selectedMenuCursorX = V_SELECTED_P_PAUSE_X;
		selectedMenuCursorY = V_SELECTED_P_PAUSE_Y;
		return 1;
		break;

	}


	// 아무 해당사항이 없을 경우
	return -1;


	return 0;
}






//////////////////////////////////////////////////////////////////////////////
/////////////////////    [CEditModeSelectView 그리기]    /////////////////////
//////////////////////////////////////////////////////////////////////////////

// CEditModeSelectView 그리기입니다.

void CEditModeSelectView::OnDraw(CDC* pDC)
{
	
	CNoteEditingToolDoc* pDoc = GetDocument();
	// TODO: 여기에 그리기 코드를 추가합니다.
	
	// 더블버퍼링을 위해
	
	CDC memCDC;
	CBitmap bForMemCDC;
	CRect rect;
	// 창 크기를 받아온다.
	GetClientRect(&rect);


	bForMemCDC.DeleteObject();
	bForMemCDC.CreateCompatibleBitmap(pDC, rect.Width(), rect.Height());

	memCDC.CreateCompatibleDC(pDC);
	memCDC.SelectObject(&bForMemCDC);

	


	//Graphics graphics(pDC->m_hDC);
	Graphics graphics(memCDC.m_hDC);
		
	char noteEditingMode = pDoc->getNoteEditingMode();
	char noteWriteType = pDoc->getNoteWriteType();


	// 각각의 아이콘들을 출력 할 좌표를 저장하는 함수.
	int positionX = 0;
	int positionY = 0;


	// 배경 그리기
	HRSRC hResource = FindResource(AfxGetApp()->m_hInstance, MAKEINTRESOURCE(IDB_PNG_BACK), _T("PNG"));
	if(!hResource) return;

	DWORD imageSize = SizeofResource(AfxGetApp()->m_hInstance, hResource);
	HGLOBAL hGlobal = LoadResource(AfxGetApp()->m_hInstance, hResource);
	LPVOID pData = LockResource(hGlobal);

	HGLOBAL hBuffer = GlobalAlloc(GMEM_MOVEABLE, imageSize);
	LPVOID pBuffer = GlobalLock(hBuffer);

	CopyMemory(pBuffer,pData,imageSize);
	GlobalUnlock(hBuffer);

	IStream *pStream;
	HRESULT hr = CreateStreamOnHGlobal(hBuffer, TRUE, &pStream);

	Image imagePNG(pStream);


	pStream->Release();
	if (imagePNG.GetLastStatus() != Ok) return;

	graphics.DrawImage(&imagePNG, 0, 0, imagePNG.GetWidth(), imagePNG.GetHeight());



	
	// 현재 선택중인 위치의 음영
	hResource = FindResource(AfxGetApp()->m_hInstance, MAKEINTRESOURCE(IDB_PNG_SEL_OK), _T("PNG"));
	if(!hResource) return;
	imageSize = SizeofResource(AfxGetApp()->m_hInstance, hResource);
	hGlobal = LoadResource(AfxGetApp()->m_hInstance, hResource);
	pData = LockResource(hGlobal);
	hBuffer = GlobalAlloc(GMEM_MOVEABLE, imageSize);
	pBuffer = GlobalLock(hBuffer);
	CopyMemory(pBuffer,pData,imageSize);
	GlobalUnlock(hBuffer);
	hr = CreateStreamOnHGlobal(hBuffer, TRUE, &pStream);
	
	Image imagePNGSelOk(pStream);
	pStream->Release();
	if (imagePNGSelOk.GetLastStatus() != Ok) return;
	
	// 음영 위치를 상태에 맞게 변화시킨다.
	if ( calSelectedMenuCursor(noteWriteType, positionX, positionY) < 0 )
	{
		AfxMessageBox(_T("에러!"));
	}
	else
	{
		graphics.DrawImage(&imagePNGSelOk, positionX, positionY, imagePNGSelOk.GetWidth(), imagePNGSelOk.GetHeight());
	}


	// 현재 선택중인 편집 모드의 음영
	hResource = FindResource(AfxGetApp()->m_hInstance, MAKEINTRESOURCE(IDB_PNG_MODE_SEL), _T("PNG"));
	if(!hResource) return;
	imageSize = SizeofResource(AfxGetApp()->m_hInstance, hResource);
	hGlobal = LoadResource(AfxGetApp()->m_hInstance, hResource);
	pData = LockResource(hGlobal);
	hBuffer = GlobalAlloc(GMEM_MOVEABLE, imageSize);
	pBuffer = GlobalLock(hBuffer);
	CopyMemory(pBuffer,pData,imageSize);
	GlobalUnlock(hBuffer);
	hr = CreateStreamOnHGlobal(hBuffer, TRUE, &pStream);
	
	Image imagePNGModeSelOk(pStream);
	pStream->Release();
	if (imagePNGModeSelOk.GetLastStatus() != Ok) return;

	// 음영 위치를 상태에 맞게 변화시킨다.
	if ( calSelectedModeCursor(noteEditingMode, positionX, positionY) < 0 )
	{
		AfxMessageBox(_T("에러!"));
	}
	else
	{
		graphics.DrawImage(&imagePNGModeSelOk, positionX, positionY, imagePNGModeSelOk.GetWidth(), imagePNGModeSelOk.GetHeight());
	}


	// 현재 선택중인 재생 모드의 음영
	hResource = FindResource(AfxGetApp()->m_hInstance, MAKEINTRESOURCE(IDB_PNG_MODE_SEL), _T("PNG"));
	if(!hResource) return;
	imageSize = SizeofResource(AfxGetApp()->m_hInstance, hResource);
	hGlobal = LoadResource(AfxGetApp()->m_hInstance, hResource);
	pData = LockResource(hGlobal);
	hBuffer = GlobalAlloc(GMEM_MOVEABLE, imageSize);
	pBuffer = GlobalLock(hBuffer);
	CopyMemory(pBuffer,pData,imageSize);
	GlobalUnlock(hBuffer);
	hr = CreateStreamOnHGlobal(hBuffer, TRUE, &pStream);

	Image imagePNGPlaySelOk(pStream);
	pStream->Release();
	if (imagePNGPlaySelOk.GetLastStatus() != Ok) return;


	// 음영 위치를 상태에 맞게 변화시킨다.
	if ( calSelectedPlayCursor(pDoc->getNotePickingViewPtr()->getNowPlayingStatus(), positionX, positionY) < 0 )
	{
		// 그리지 않는다.
		//AfxMessageBox(_T("에러!"));
	}
	else
	{
		graphics.DrawImage(&imagePNGPlaySelOk, positionX, positionY, imagePNGPlaySelOk.GetWidth(), imagePNGPlaySelOk.GetHeight());
	}




	// 연필
	hResource = FindResource(AfxGetApp()->m_hInstance, MAKEINTRESOURCE(IDB_PNG_PENCIL), _T("PNG"));
	if(!hResource) return;
	imageSize = SizeofResource(AfxGetApp()->m_hInstance, hResource);
	hGlobal = LoadResource(AfxGetApp()->m_hInstance, hResource);
	pData = LockResource(hGlobal);
	hBuffer = GlobalAlloc(GMEM_MOVEABLE, imageSize);
	pBuffer = GlobalLock(hBuffer);
	CopyMemory(pBuffer,pData,imageSize);
	GlobalUnlock(hBuffer);
	hr = CreateStreamOnHGlobal(hBuffer, TRUE, &pStream);

	Image imagePNGPencil(pStream);
	pStream->Release();
	if (imagePNGPencil.GetLastStatus() != Ok) return;

	graphics.DrawImage(&imagePNGPencil, V_PENCIL_OFFSET_X, V_PENCIL_OFFSET_Y, imagePNGPencil.GetWidth(), imagePNGPencil.GetHeight());



	// 글자 넣기
	hResource = FindResource(AfxGetApp()->m_hInstance, MAKEINTRESOURCE(IDB_PNG_TEXT), _T("PNG"));
	if(!hResource) return;
	imageSize = SizeofResource(AfxGetApp()->m_hInstance, hResource);
	hGlobal = LoadResource(AfxGetApp()->m_hInstance, hResource);
	pData = LockResource(hGlobal);
	hBuffer = GlobalAlloc(GMEM_MOVEABLE, imageSize);
	pBuffer = GlobalLock(hBuffer);
	CopyMemory(pBuffer,pData,imageSize);
	GlobalUnlock(hBuffer);
	*pStream;
	hr = CreateStreamOnHGlobal(hBuffer, TRUE, &pStream);
	Image imagePNGText(pStream);

	pStream->Release();
	if (imagePNGText.GetLastStatus() != Ok) return;
	graphics.DrawImage(&imagePNGText, V_TEXT_OFFSET_X, V_TEXT_OFFSET_Y, imagePNGText.GetWidth(), imagePNGText.GetHeight());
	
	

	// 마지막에 한번에 화면 그리기
	pDC->BitBlt(0, 0, rect.Width(), rect.Height(), &memCDC, 0, 0, SRCCOPY);


}


// CEditModeSelectView 진단입니다.

#ifdef _DEBUG
void CEditModeSelectView::AssertValid() const
{
	CView::AssertValid();
}

CNoteEditingToolDoc* CEditModeSelectView::GetDocument() const // 디버그되지 않은 버전은 인라인으로 지정됩니다.
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CNoteEditingToolDoc)));
	return (CNoteEditingToolDoc*)m_pDocument;
}

//#ifndef _WIN32_WCE
void CEditModeSelectView::Dump(CDumpContext& dc) const
{
	CView::Dump(dc);
}
//#endif
#endif //_DEBUG


// CEditModeSelectView 메시지 처리기입니다.




///////////////////////////////////////////////////////////
// NoteEditingToolDoc와 연동하기 위해서 설정하는 부분.
// 이하부터 밖의 디버그 모드까지 포함.



BOOL CEditModeSelectView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: CREATESTRUCT cs를 수정하여 여기에서
	//  Window 클래스 또는 스타일을 수정합니다.

	return CView::PreCreateWindow(cs);
}


BOOL CEditModeSelectView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// 기본적인 준비
	return DoPreparePrinting(pInfo);
}


void CEditModeSelectView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: 인쇄하기 전에 추가 초기화 작업을 추가합니다.
}

void CEditModeSelectView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: 인쇄 후 정리 작업을 추가합니다.
}



void CEditModeSelectView::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.

	CView::OnMouseMove(nFlags, point);
}


void CEditModeSelectView::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CNoteEditingToolDoc* pDoc = GetDocument();

	// 타입에 따른 설정
	char temp = calMouseSelectArea(point.x, point.y);
	pDoc->setNoteEditingMode( temp );
	pDoc->setNoteWriteType(	temp );

	// 재생 상태 확인.
	int tempPlay = calMouseSelectPlayArea(point.x, point.y);
	if ( tempPlay == -99 )
	{
		// 에러이므로 무시
	}
	else if ( pDoc->getNotePickingViewPtr() != NULL )
	{
		// NULL이 아닐 때에만.
		
		// 상태가 달라졌을 때에만 메시지를 보낸다.
		// pDoc->getNotePickingViewPtr()->setNowPlayingStatus(tempPlay);
		if ( pDoc->getNotePickingViewPtr()->getNowPlayingStatus() != tempPlay )
		{
			pDoc->getNotePickingViewPtr()->SendMessageW(CM_VK_SPACE2, 1, tempPlay);
		}

		
	}


	// 계속 보이던 것을 지우고.
	Invalidate();
	//RedrawWindow();

	// Draw도 같이 지워준다.
	if(	pDoc->getNoteEditingToolViewPtr() != NULL )
	{
		pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
	}

	CView::OnLButtonDown(nFlags, point);
}


afx_msg LRESULT CEditModeSelectView::OnCmRedrawView(WPARAM wParam, LPARAM lParam)
{
	Invalidate();

	return 0;
}


void CEditModeSelectView::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CNoteEditingToolDoc* pDoc = GetDocument();


	switch( nChar )
	{
	case VK_SPACE:
		if ( pDoc->getNotePickingViewPtr() != NULL )
		{
			pDoc->getNotePickingViewPtr()->SendMessage(CM_VK_SPACE2);
		}
		
		// 다시 그려준다.
		Invalidate();

		break;
		
	case '1':
		// 쓰기 모드로 변환.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);
		pDoc->setNoteEditingMode( EDIT_MODE_WRITE );
		pDoc->setNoteWriteType(	EDIT_MODE_WRITE );

		// 계속 보이던 것을 지우고.
		Invalidate();
		//RedrawWindow();

		// Draw도 같이 지워준다.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;

	case '2':
		// 지우기 모드로 변환.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);
		pDoc->setNoteEditingMode( EDIT_MODE_ERASE );
		pDoc->setNoteWriteType(	EDIT_MODE_ERASE );

		// 계속 보이던 것을 지우고.
		Invalidate();
		//RedrawWindow();

		// Draw도 같이 지워준다.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;

	case '3':
		// 수정 모드로 변환.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);
		pDoc->setNoteEditingMode( EDIT_MODE_MOVE );
		pDoc->setNoteWriteType(	EDIT_MODE_MOVE );

		// 계속 보이던 것을 지우고.
		Invalidate();
		//RedrawWindow();

		// Draw도 같이 지워준다.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;


	case '4':
		// 쓰기 모드로 변환.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);
		pDoc->setNoteEditingMode( EDIT_MODE_CONFG );
		pDoc->setNoteWriteType(	EDIT_MODE_CONFG );

		// 계속 보이던 것을 지우고.
		Invalidate();
		//RedrawWindow();

		// Draw도 같이 지워준다.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;


	}



	CView::OnKeyDown(nChar, nRepCnt, nFlags);
}


BOOL CEditModeSelectView::OnEraseBkgnd(CDC* pDC)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CBrush backBrush(RGB(195, 195, 195));               // 파랑색. 원하는 컬러를 넣어주면 된다...

	CBrush* pOldBrush = pDC->SelectObject(&backBrush); 
	CRect rect; pDC->GetClipBox(&rect); 
	pDC->PatBlt(rect.left, rect.top, rect.Width(), rect.Height(), PATCOPY);
	pDC->SelectObject(pOldBrush); 

	return TRUE;                // TRUE 로 해주어야 한다. 기존것(return CView::OnEraseBkgnd(pDC);)


	//	평소때 컬러를 안바꾸더라도 WM_ERASEBKGND 를 정의하여 return TURE 로 해두면
	//	더블 버퍼링을 쓸때 화면의 바탕을 다시 그리는 일이 없게 해서 조금이나마 화면의 깜박임을 더 줄일수 있다.
	//return CScrollView::OnEraseBkgnd(pDC);
	//return CView::OnEraseBkgnd(pDC);
}


afx_msg LRESULT CEditModeSelectView::OnCmEditingModeChange(WPARAM wParam, LPARAM lParam)
{
	// 다른 뷰에서 이 메시지를 받으면, 그에 맞는 모드로 바꿔준다.


	CNoteEditingToolDoc* pDoc = GetDocument();

	switch( lParam )
	{

	case EDIT_MODE_WRITE:
		// 쓰기 모드로 변환.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);

		pDoc->setNoteEditingMode( EDIT_MODE_WRITE );
		pDoc->setNoteWriteType(	EDIT_MODE_WRITE );

		// 계속 보이던 것을 지우고.
		Invalidate();
		//RedrawWindow();

		// Draw도 같이 지워준다.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;

	case EDIT_MODE_ERASE:
		// 지우기 모드로 변환.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);

		pDoc->setNoteEditingMode( EDIT_MODE_ERASE );
		pDoc->setNoteWriteType(	EDIT_MODE_ERASE );

		// 계속 보이던 것을 지우고.
		Invalidate();
		//RedrawWindow();

		// Draw도 같이 지워준다.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;

	case EDIT_MODE_MOVE:
		// 수정 모드로 변환.

		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);

		pDoc->setNoteEditingMode( EDIT_MODE_MOVE );
		pDoc->setNoteWriteType(	EDIT_MODE_MOVE );

		// 계속 보이던 것을 지우고.
		Invalidate();
		//RedrawWindow();

		// Draw도 같이 지워준다.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;


	case EDIT_MODE_CONFG:
		// 쓰기 모드로 변환.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);

		pDoc->setNoteEditingMode( EDIT_MODE_CONFG );
		pDoc->setNoteWriteType(	EDIT_MODE_CONFG );

		// 계속 보이던 것을 지우고.
		Invalidate();
		//RedrawWindow();

		// Draw도 같이 지워준다.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;


	}


	return 0;
}
