// EditModeSelectView.cpp : ���� �����Դϴ�.
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
	// OnCreate�� OnInit �Լ��� �����Ƿ� �׳� �����ڿ� ����.

	//CNoteEditingToolDoc* pDoc = GetDocument();
	//
	//// ���� ��� �˻�
	//char noteEditingMode = pDoc->getNoteEditingMode();
	//char noteWriteType = pDoc->getNoteWriteType();
	//int testFlag = calSelectedMenuCursor( noteWriteType );
	//// ���� ��� �˻�
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

void CEditModeSelectView::OnInitialUpdate()     // ������ �� ó���Դϴ�.
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



// Ŭ���������� ���콺 ��ġ�� ���� ��ǥ�� ����ؼ� ����ϴ� �Լ�.
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
	
	// �ƹ� �ش������ ���� ���
	return -1;
}


// ���� ���� �� ������ ��忡 ���� �������� ��ǥ �����ϴ� �Լ�.
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


	// �ƹ� �ش������ ���� ���
	return -1;
}



// ���� ���콺 ��ġ�� ���� ��ǥ�� ����ؼ� ����ϴ� �Լ�.
char CEditModeSelectView::calMouseSelectArea(const int pointX, const int pointY)
{
	CNoteEditingToolDoc* pDoc = GetDocument();

	// ���� y�������� Ȯ��
	if( pointY >= V_SELECTED_PEN_Y && pointY <= V_SELECTED_9_Y + V_TEXT_AREA_HEIGHT )
	{
		// ������ �������� y�� ������ ���� �� Ȯ��
		if( pointY <= V_SELECTED_PEN_Y + V_MODE_AREA_HEIGHT)
		{
			// ���� �׸��� �޴� �ȿ� ���� ���. ������ x��ǥ�� ��ġ�� �˻��Ѵ�.

			if( pointX >= V_SELECTED_PEN_X && pointX <= V_SELECTED_PEN_X + V_MODE_AREA_WIDTH)
			{
				// ���� ��忡���� �ʿ� ����.
				pDoc->setOnFocusEditingFlag(false);
				pDoc->setNowEditingNote(NULL, 0);

				return EDIT_MODE_WRITE;
			}
			else if( pointX >= V_SELECTED_ERASE_X && pointX <= V_SELECTED_ERASE_X + V_MODE_AREA_WIDTH)
			{
				// ���� ��忡���� �ʱ�ȭ
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
			// ���� ���� ���ȿ� ���� ���.
			if( pointX >= V_SELECTED_1_X && pointX <= (V_SELECTED_1_X + V_TEXT_AREA_WIDTH) )
			{
				// ������ x��ǥ �ȿ� �ִ� �� Ȯ�� �� ����, ������ y��ǥ�� ��ġ�� �˻��Ѵ�.

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
				// �� ���׿��� �ƹ��͵� �ش����� ���� ���, �׳� ����.
			}
		}


	}

	// �ٸ� ������ ������ �̰��� ���
	//onMouseTextFlag = false;
	//onMouseEditModeFlag = false;
	return '!';
}



// ���� ���콺 ��ġ�� ���� ��ǥ�� ����ؼ� ����ϴ� �Լ�.
char CEditModeSelectView::calMouseMoveArea(const int pointX, const int pointY)
{


	return 0;
}



// �÷��� ������ ���¸� Ŭ���������� ���콺 ��ġ�� ���� ��ǥ�� ����ؼ� ����ϴ� �Լ�.
int CEditModeSelectView::calMouseSelectPlayArea(const int pointX, const int pointY)
{
	CNoteEditingToolDoc* pDoc = GetDocument();


	// ���� y�������� Ȯ��

	if( pointY >= V_SELECTED_P_PLAY_Y &&
		pointY <= V_SELECTED_P_PLAY_Y + V_MODE_AREA_HEIGHT)
	{
		// ���� �׸��� �޴� �ȿ� ���� ���. ������ x��ǥ�� ��ġ�� �˻��Ѵ�.

		if( pointX >= V_SELECTED_P_PLAY_X && pointX <= V_SELECTED_P_PLAY_X + V_MODE_AREA_WIDTH)
		{
			// ���
			return PLAY_STATE_PLAY;
		}
		else if( pointX >= V_SELECTED_P_PAUSE_X && pointX <= V_SELECTED_P_PAUSE_X + V_MODE_AREA_WIDTH)
		{
			// ����
			return PLAY_STATE_PAUSE;
		}
		else if( pointX >= V_SELECTED_P_STOP_X && pointX <= V_SELECTED_P_STOP_X + V_MODE_AREA_WIDTH)
		{
			// �Ͻ�����
			return PLAY_STATE_STOP;
		}
	}

	return -99;
}



// ���� ���� �� ������ ��忡 ���� �������� ��ǥ �����ϴ� �Լ�.
int CEditModeSelectView::calSelectedPlayCursor(const int nowPlayingStatus, int &selectedMenuCursorX, int &selectedMenuCursorY)
{

	switch( nowPlayingStatus )
	{
	case PLAY_STATE_NO_MUSIC:
		// ������ ���� ���, �׸��� �ʵ��� �Ѵ�.
		return -1;
		break;

	case PLAY_STATE_NON_INIT:
		// �ʱ�ȭ�� ���� �ʾ��� ���, �׸��� �ʵ��� �Ѵ�.
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


	// �ƹ� �ش������ ���� ���
	return -1;


	return 0;
}






//////////////////////////////////////////////////////////////////////////////
/////////////////////    [CEditModeSelectView �׸���]    /////////////////////
//////////////////////////////////////////////////////////////////////////////

// CEditModeSelectView �׸����Դϴ�.

void CEditModeSelectView::OnDraw(CDC* pDC)
{
	
	CNoteEditingToolDoc* pDoc = GetDocument();
	// TODO: ���⿡ �׸��� �ڵ带 �߰��մϴ�.
	
	// ������۸��� ����
	
	CDC memCDC;
	CBitmap bForMemCDC;
	CRect rect;
	// â ũ�⸦ �޾ƿ´�.
	GetClientRect(&rect);


	bForMemCDC.DeleteObject();
	bForMemCDC.CreateCompatibleBitmap(pDC, rect.Width(), rect.Height());

	memCDC.CreateCompatibleDC(pDC);
	memCDC.SelectObject(&bForMemCDC);

	


	//Graphics graphics(pDC->m_hDC);
	Graphics graphics(memCDC.m_hDC);
		
	char noteEditingMode = pDoc->getNoteEditingMode();
	char noteWriteType = pDoc->getNoteWriteType();


	// ������ �����ܵ��� ��� �� ��ǥ�� �����ϴ� �Լ�.
	int positionX = 0;
	int positionY = 0;


	// ��� �׸���
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



	
	// ���� �������� ��ġ�� ����
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
	
	// ���� ��ġ�� ���¿� �°� ��ȭ��Ų��.
	if ( calSelectedMenuCursor(noteWriteType, positionX, positionY) < 0 )
	{
		AfxMessageBox(_T("����!"));
	}
	else
	{
		graphics.DrawImage(&imagePNGSelOk, positionX, positionY, imagePNGSelOk.GetWidth(), imagePNGSelOk.GetHeight());
	}


	// ���� �������� ���� ����� ����
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

	// ���� ��ġ�� ���¿� �°� ��ȭ��Ų��.
	if ( calSelectedModeCursor(noteEditingMode, positionX, positionY) < 0 )
	{
		AfxMessageBox(_T("����!"));
	}
	else
	{
		graphics.DrawImage(&imagePNGModeSelOk, positionX, positionY, imagePNGModeSelOk.GetWidth(), imagePNGModeSelOk.GetHeight());
	}


	// ���� �������� ��� ����� ����
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


	// ���� ��ġ�� ���¿� �°� ��ȭ��Ų��.
	if ( calSelectedPlayCursor(pDoc->getNotePickingViewPtr()->getNowPlayingStatus(), positionX, positionY) < 0 )
	{
		// �׸��� �ʴ´�.
		//AfxMessageBox(_T("����!"));
	}
	else
	{
		graphics.DrawImage(&imagePNGPlaySelOk, positionX, positionY, imagePNGPlaySelOk.GetWidth(), imagePNGPlaySelOk.GetHeight());
	}




	// ����
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



	// ���� �ֱ�
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
	
	

	// �������� �ѹ��� ȭ�� �׸���
	pDC->BitBlt(0, 0, rect.Width(), rect.Height(), &memCDC, 0, 0, SRCCOPY);


}


// CEditModeSelectView �����Դϴ�.

#ifdef _DEBUG
void CEditModeSelectView::AssertValid() const
{
	CView::AssertValid();
}

CNoteEditingToolDoc* CEditModeSelectView::GetDocument() const // ����׵��� ���� ������ �ζ������� �����˴ϴ�.
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


// CEditModeSelectView �޽��� ó�����Դϴ�.




///////////////////////////////////////////////////////////
// NoteEditingToolDoc�� �����ϱ� ���ؼ� �����ϴ� �κ�.
// ���Ϻ��� ���� ����� ������ ����.



BOOL CEditModeSelectView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: CREATESTRUCT cs�� �����Ͽ� ���⿡��
	//  Window Ŭ���� �Ǵ� ��Ÿ���� �����մϴ�.

	return CView::PreCreateWindow(cs);
}


BOOL CEditModeSelectView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// �⺻���� �غ�
	return DoPreparePrinting(pInfo);
}


void CEditModeSelectView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: �μ��ϱ� ���� �߰� �ʱ�ȭ �۾��� �߰��մϴ�.
}

void CEditModeSelectView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: �μ� �� ���� �۾��� �߰��մϴ�.
}



void CEditModeSelectView::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰� ��/�Ǵ� �⺻���� ȣ���մϴ�.

	CView::OnMouseMove(nFlags, point);
}


void CEditModeSelectView::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰� ��/�Ǵ� �⺻���� ȣ���մϴ�.
	CNoteEditingToolDoc* pDoc = GetDocument();

	// Ÿ�Կ� ���� ����
	char temp = calMouseSelectArea(point.x, point.y);
	pDoc->setNoteEditingMode( temp );
	pDoc->setNoteWriteType(	temp );

	// ��� ���� Ȯ��.
	int tempPlay = calMouseSelectPlayArea(point.x, point.y);
	if ( tempPlay == -99 )
	{
		// �����̹Ƿ� ����
	}
	else if ( pDoc->getNotePickingViewPtr() != NULL )
	{
		// NULL�� �ƴ� ������.
		
		// ���°� �޶����� ������ �޽����� ������.
		// pDoc->getNotePickingViewPtr()->setNowPlayingStatus(tempPlay);
		if ( pDoc->getNotePickingViewPtr()->getNowPlayingStatus() != tempPlay )
		{
			pDoc->getNotePickingViewPtr()->SendMessageW(CM_VK_SPACE2, 1, tempPlay);
		}

		
	}


	// ��� ���̴� ���� �����.
	Invalidate();
	//RedrawWindow();

	// Draw�� ���� �����ش�.
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
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰� ��/�Ǵ� �⺻���� ȣ���մϴ�.
	CNoteEditingToolDoc* pDoc = GetDocument();


	switch( nChar )
	{
	case VK_SPACE:
		if ( pDoc->getNotePickingViewPtr() != NULL )
		{
			pDoc->getNotePickingViewPtr()->SendMessage(CM_VK_SPACE2);
		}
		
		// �ٽ� �׷��ش�.
		Invalidate();

		break;
		
	case '1':
		// ���� ���� ��ȯ.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);
		pDoc->setNoteEditingMode( EDIT_MODE_WRITE );
		pDoc->setNoteWriteType(	EDIT_MODE_WRITE );

		// ��� ���̴� ���� �����.
		Invalidate();
		//RedrawWindow();

		// Draw�� ���� �����ش�.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;

	case '2':
		// ����� ���� ��ȯ.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);
		pDoc->setNoteEditingMode( EDIT_MODE_ERASE );
		pDoc->setNoteWriteType(	EDIT_MODE_ERASE );

		// ��� ���̴� ���� �����.
		Invalidate();
		//RedrawWindow();

		// Draw�� ���� �����ش�.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;

	case '3':
		// ���� ���� ��ȯ.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);
		pDoc->setNoteEditingMode( EDIT_MODE_MOVE );
		pDoc->setNoteWriteType(	EDIT_MODE_MOVE );

		// ��� ���̴� ���� �����.
		Invalidate();
		//RedrawWindow();

		// Draw�� ���� �����ش�.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;


	case '4':
		// ���� ���� ��ȯ.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);
		pDoc->setNoteEditingMode( EDIT_MODE_CONFG );
		pDoc->setNoteWriteType(	EDIT_MODE_CONFG );

		// ��� ���̴� ���� �����.
		Invalidate();
		//RedrawWindow();

		// Draw�� ���� �����ش�.
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
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰� ��/�Ǵ� �⺻���� ȣ���մϴ�.
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰� ��/�Ǵ� �⺻���� ȣ���մϴ�.
	CBrush backBrush(RGB(195, 195, 195));               // �Ķ���. ���ϴ� �÷��� �־��ָ� �ȴ�...

	CBrush* pOldBrush = pDC->SelectObject(&backBrush); 
	CRect rect; pDC->GetClipBox(&rect); 
	pDC->PatBlt(rect.left, rect.top, rect.Width(), rect.Height(), PATCOPY);
	pDC->SelectObject(pOldBrush); 

	return TRUE;                // TRUE �� ���־�� �Ѵ�. ������(return CView::OnEraseBkgnd(pDC);)


	//	��Ҷ� �÷��� �ȹٲٴ��� WM_ERASEBKGND �� �����Ͽ� return TURE �� �صθ�
	//	���� ���۸��� ���� ȭ���� ������ �ٽ� �׸��� ���� ���� �ؼ� �����̳��� ȭ���� �������� �� ���ϼ� �ִ�.
	//return CScrollView::OnEraseBkgnd(pDC);
	//return CView::OnEraseBkgnd(pDC);
}


afx_msg LRESULT CEditModeSelectView::OnCmEditingModeChange(WPARAM wParam, LPARAM lParam)
{
	// �ٸ� �信�� �� �޽����� ������, �׿� �´� ���� �ٲ��ش�.


	CNoteEditingToolDoc* pDoc = GetDocument();

	switch( lParam )
	{

	case EDIT_MODE_WRITE:
		// ���� ���� ��ȯ.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);

		pDoc->setNoteEditingMode( EDIT_MODE_WRITE );
		pDoc->setNoteWriteType(	EDIT_MODE_WRITE );

		// ��� ���̴� ���� �����.
		Invalidate();
		//RedrawWindow();

		// Draw�� ���� �����ش�.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;

	case EDIT_MODE_ERASE:
		// ����� ���� ��ȯ.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);

		pDoc->setNoteEditingMode( EDIT_MODE_ERASE );
		pDoc->setNoteWriteType(	EDIT_MODE_ERASE );

		// ��� ���̴� ���� �����.
		Invalidate();
		//RedrawWindow();

		// Draw�� ���� �����ش�.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;

	case EDIT_MODE_MOVE:
		// ���� ���� ��ȯ.

		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);

		pDoc->setNoteEditingMode( EDIT_MODE_MOVE );
		pDoc->setNoteWriteType(	EDIT_MODE_MOVE );

		// ��� ���̴� ���� �����.
		Invalidate();
		//RedrawWindow();

		// Draw�� ���� �����ش�.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;


	case EDIT_MODE_CONFG:
		// ���� ���� ��ȯ.
		pDoc->setOnFocusEditingFlag(false);
		pDoc->setNowEditingNote(NULL, 0);

		pDoc->setNoteEditingMode( EDIT_MODE_CONFG );
		pDoc->setNoteWriteType(	EDIT_MODE_CONFG );

		// ��� ���̴� ���� �����.
		Invalidate();
		//RedrawWindow();

		// Draw�� ���� �����ش�.
		if ( pDoc->getNoteEditingToolViewPtr() != NULL )
		{
			pDoc->getNoteEditingToolViewPtr()->PostMessageW(CM_REDRAW_DRAG_VIEW3, 0, 0);
		}
		break;


	}


	return 0;
}
