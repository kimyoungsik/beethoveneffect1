
// NoteEditingToolView.cpp : CNoteEditingToolView Ŭ������ ����
//

#include "stdafx.h"
// SHARED_HANDLERS�� �̸� ����, ����� �׸� �� �˻� ���� ó���⸦ �����ϴ� ATL ������Ʈ���� ������ �� ������
// �ش� ������Ʈ�� ���� �ڵ带 �����ϵ��� �� �ݴϴ�.
#ifndef SHARED_HANDLERS
#include "NoteEditingTool.h"
#endif

#include "NoteEditingToolDoc.h"
#include "NoteEditingToolView.h"
#include "NotePickingView.h"
#include "EditModeSelectView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

UINT CM_REDRAW_DRAG_VIEW = RegisterWindowMessage(_T("CM_REDRAW_DRAG_VIEW"));
UINT CM_EDITING_MODE_CHANGE3 = RegisterWindowMessage(_T("CM_EDITING_MODE_CHANGE"));
UINT CM_VK_SPACE3 = RegisterWindowMessage(_T("CM_VK_SPACE"));

// CNoteEditingToolView

IMPLEMENT_DYNCREATE(CNoteEditingToolView, CView)


BEGIN_MESSAGE_MAP(CNoteEditingToolView, CView)
	// ǥ�� �μ� ����Դϴ�.
	ON_COMMAND(ID_FILE_PRINT, &CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, &CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, &CNoteEditingToolView::OnFilePrintPreview)
	ON_WM_CONTEXTMENU()
	ON_WM_RBUTTONUP()
//	ON_MESSAGE(WM_TEST, &CNoteEditingToolView::TEST)
//	ON_MESSAGE(CM_REDRAW_DRAG_VIEW, &CNoteEditingToolView::OnCmRedrawDragView)
	ON_REGISTERED_MESSAGE(CM_REDRAW_DRAG_VIEW, &CNoteEditingToolView::OnCmRedrawDragView)
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_ERASEBKGND()
	ON_WM_KEYDOWN()
END_MESSAGE_MAP()

// CNoteEditingToolView ����/�Ҹ�

CNoteEditingToolView::CNoteEditingToolView()
{
	// TODO: ���⿡ ���� �ڵ带 �߰��մϴ�.
	dotDataIndex = -1;							// ��ҿ��� -1�̴�. 0~3 ������ ���� ���� ���, ���� ��Ŀ�� �Ǿ� �ִٴ� ���̴�.
	OnFocusFlag = false;						// T�� ���, ���� ��Ʋ��� �ִ� ���̴�.
}

CNoteEditingToolView::~CNoteEditingToolView()
{
}

BOOL CNoteEditingToolView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: CREATESTRUCT cs�� �����Ͽ� ���⿡��
	//  Window Ŭ���� �Ǵ� ��Ÿ���� �����մϴ�.


	return CView::PreCreateWindow(cs);
}



// �巡�� ��Ʈ �⺻ ��ǥ���� �׸��� �Լ�.
int CNoteEditingToolView::DrawBackXYLine(CDC* pDC)
{

	CPen l_my_pen,*old_pen; // Pen ��ü�� �����Ѵ�. old_pen �����͵� 

	l_my_pen.CreatePen(PS_SOLID,3,RGB(0,0,0));
	old_pen = pDC->SelectObject(&l_my_pen);

	//////////////////////////////////////////////////////////////////////////
	// ���⼭���� l_my_pen�� �Ӽ��� ���� �޴´�.
	//////////////////////////////////////////////////////////////////////////


	// ����, ���� �׸���
	l_my_pen.DeleteObject();
	l_my_pen.CreatePen(PS_SOLID, 3, RGB(50,45,47));
	pDC->SelectObject(&l_my_pen);

	pDC->MoveTo(0, FRAME_2_HEIGHT / 2);
	pDC->LineTo(FRAME_2_WIDTH, FRAME_2_HEIGHT / 2);

	pDC->MoveTo(FRAME_2_WIDTH / 2, 0);
	pDC->LineTo(FRAME_2_WIDTH / 2, FRAME_2_HEIGHT);
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




	pDC->SelectObject(old_pen);//������ ����ϴ� pen�Ӽ����κ�����.

	return 1;
}





// CNoteEditingToolView �׸���

void CNoteEditingToolView::OnDraw(CDC* pDC)
{
	static int InitCounter = 0;						// Doc�� ���� View�� �ּҸ� �Ѱ��ֱ� ���� ������� 1ȸ�� ����.
	
	CNoteData* tempNote = NULL;
	int noteConfigHeadDir = -1;
	dotData tempDotArray[MAX_DOT_DATA_NUM];
	CRect tempRect;
	int i=0;

	// �� ĥ�ϴ� ��.
	CBrush newBrush(RGB(255, 255, 255));
	CBrush* oldBrush;
	CPen l_my_pen,*old_pen;			 // Pen ��ü�� �����Ѵ�. old_pen �����͵� 



	CNoteEditingToolDoc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);
	if (!pDoc)
		return;

	if ( InitCounter == 0)
	{
		pDoc->setNoteEditingToolViewPtr(this);
		InitCounter++;
	}


	// TODO: ���⿡ ���� �����Ϳ� ���� �׸��� �ڵ带 �߰��մϴ�.

	DrawBackXYLine(pDC);

	tempNote = pDoc->getNowConfigingNote(noteConfigHeadDir);

	// �� �׸��� �κ�
	if( tempNote != NULL )
	{
		// ���� �����߿� �ִ� ��Ʈ�� ���� ���,
		if( tempNote->getNoteType() == NOTE_T_DRAG )
		{
			// ���� �巡�� ��Ʈ�� ��� ȭ�鿡 ���� ����Ѵ�.


			if ( OnFocusFlag == false )
			{
				// �׳� �׸� ��


				// ���ἱ ���� �׷��ش�.

				// ���� �޾� �� ����
				for( i=0 ; i<4 ; i++ )
				{
					tempNote->getDragPoint(i, tempDotArray[i]);
				}

	


				
				// ������ � �׸���
				{
					//tempDotArray
					POINT pline[4];

					pline[0].x = (int)(tempDotArray[0].x / D_TOOL_TO_GAME);
					pline[0].y = (int)(tempDotArray[0].y / D_TOOL_TO_GAME);
					pline[1].x = (int)(tempDotArray[1].x / D_TOOL_TO_GAME);
					pline[1].y = (int)(tempDotArray[1].y / D_TOOL_TO_GAME);
					pline[2].x = (int)(tempDotArray[2].x / D_TOOL_TO_GAME);
					pline[2].y = (int)(tempDotArray[2].y / D_TOOL_TO_GAME);
					pline[3].x = (int)(tempDotArray[3].x / D_TOOL_TO_GAME);
					pline[3].y = (int)(tempDotArray[3].y / D_TOOL_TO_GAME);


					// ���� Ư�� �޾ƿ��� (�ܰ���)
					l_my_pen.DeleteObject();
					l_my_pen.CreatePen(PS_SOLID, D_CIRCLE_SIZE*2, C_BEZIER_OUTER_LINE_COLOR);
					old_pen = pDC->SelectObject(&l_my_pen);

					// �׸��� �Լ�
					PolyBezier(pDC->m_hDC, pline, 4);


					// ���� Ư�� �޾ƿ��� (�ܼ޼�)
					l_my_pen.DeleteObject();
					l_my_pen.CreatePen(PS_SOLID, D_CIRCLE_SIZE*2 - D_BEZIER_STROK_SIZE, C_BEZIER_INNER_LINE_COLOR);
					pDC->SelectObject(&l_my_pen);


					// �׸��� �Լ�
					PolyBezier(pDC->m_hDC, pline, 4);



					// �� ����
					pDC->SelectObject(old_pen);
				}	



				// �� �׸���
				pDC->MoveTo((int)(tempDotArray[0].x / D_TOOL_TO_GAME), (int)(tempDotArray[0].y / D_TOOL_TO_GAME));
				pDC->LineTo((int)(tempDotArray[1].x / D_TOOL_TO_GAME), (int)(tempDotArray[1].y / D_TOOL_TO_GAME));
				pDC->LineTo((int)(tempDotArray[2].x / D_TOOL_TO_GAME), (int)(tempDotArray[2].y / D_TOOL_TO_GAME));
				pDC->LineTo((int)(tempDotArray[3].x / D_TOOL_TO_GAME), (int)(tempDotArray[3].y / D_TOOL_TO_GAME));


				// ���� �귯�� ������ �޾ƿ´�.
				newBrush.DeleteObject();
				newBrush.CreateSolidBrush(DC_START);
				oldBrush = pDC->SelectObject(&newBrush);


				// ���� Ư�� �޾ƿ��� (�ܰ�����)
				l_my_pen.DeleteObject();
				l_my_pen.CreatePen(PS_SOLID, D_BEZIER_STROK_SIZE/2, C_BEZIER_OUTER_LINE_COLOR);
				old_pen = pDC->SelectObject(&l_my_pen);



				//tempNote->getDragPoint(0, tempDotArray[0]);
				pDC->Ellipse(
					(int)((tempDotArray[0].x / D_TOOL_TO_GAME) - D_CIRCLE_SIZE),
					(int)((tempDotArray[0].y / D_TOOL_TO_GAME) - D_CIRCLE_SIZE),
					(int)((tempDotArray[0].x / D_TOOL_TO_GAME) + D_CIRCLE_SIZE),
					(int)((tempDotArray[0].y / D_TOOL_TO_GAME) + D_CIRCLE_SIZE)
					);



				// �� ����
				newBrush.DeleteObject();
				newBrush.CreateSolidBrush(DC_CENTER);
				// �� �귯�� �� ����
				pDC->SelectObject(newBrush);
				// �� �������
				pDC->SelectObject(old_pen);



				for( i=1 ; i < 3 ; i++ )
				{
					//tempNote->getDragPoint(i, tempDotArray[i]);
					pDC->Ellipse(
						(int)((tempDotArray[i].x / D_TOOL_TO_GAME) - D_CIRCLE_DOT_SIZE),
						(int)((tempDotArray[i].y / D_TOOL_TO_GAME) - D_CIRCLE_DOT_SIZE),
						(int)((tempDotArray[i].x / D_TOOL_TO_GAME) + D_CIRCLE_DOT_SIZE),
						(int)((tempDotArray[i].y / D_TOOL_TO_GAME) + D_CIRCLE_DOT_SIZE)
						);
				}

				// �� ����
				newBrush.DeleteObject();
				newBrush.CreateSolidBrush(DC_END);
				// �� �귯�� �� ����
				pDC->SelectObject(newBrush);
				// �� �β��� �ɷ�
				pDC->SelectObject(&l_my_pen);


				//tempNote->getDragPoint(3, tempDotArray[3]);
				pDC->Ellipse(
					(int)((tempDotArray[3].x / D_TOOL_TO_GAME) - D_CIRCLE_SIZE),
					(int)((tempDotArray[3].y / D_TOOL_TO_GAME) - D_CIRCLE_SIZE),
					(int)((tempDotArray[3].x / D_TOOL_TO_GAME) + D_CIRCLE_SIZE),
					(int)((tempDotArray[3].y / D_TOOL_TO_GAME) + D_CIRCLE_SIZE)
					);


				pDC->SelectObject(oldBrush);
				// �� ����
				pDC->SelectObject(old_pen);

				
			}
			else
			{
				// �׳� ������ ���� ��

				newBrush.DeleteObject();
				newBrush.CreateSolidBrush(DC_START);

				// ���� �귯�� ������ �޾ƿ´�.
				oldBrush = pDC->SelectObject(&newBrush);

				// ���� Ư�� �޾ƿ��� (�ܰ�����)
				l_my_pen.DeleteObject();
				l_my_pen.CreatePen(PS_SOLID, D_BEZIER_STROK_SIZE/2, C_BEZIER_OUTER_LINE_COLOR);
				old_pen = pDC->SelectObject(&l_my_pen);


				if ( dotDataIndex != 0 )
				{
					tempNote->getDragPoint(0, tempDotArray[0]);
					pDC->Ellipse(
						(int)((tempDotArray[0].x / D_TOOL_TO_GAME) - D_CIRCLE_SIZE),
						(int)((tempDotArray[0].y / D_TOOL_TO_GAME) - D_CIRCLE_SIZE),
						(int)((tempDotArray[0].x / D_TOOL_TO_GAME) + D_CIRCLE_SIZE),
						(int)((tempDotArray[0].y / D_TOOL_TO_GAME) + D_CIRCLE_SIZE)
						);
				}

				
				for( i=1 ; i < 3 ; i++ )
				{
					if ( dotDataIndex != i )
					{
						// �� ����
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(DC_CENTER);
						// �� �귯�� �� ����
						pDC->SelectObject(newBrush);
						// �� �������
						pDC->SelectObject(old_pen);

						tempNote->getDragPoint(i, tempDotArray[i]);
						pDC->Ellipse(
							(int)((tempDotArray[i].x / D_TOOL_TO_GAME) - D_CIRCLE_DOT_SIZE),
							(int)((tempDotArray[i].y / D_TOOL_TO_GAME) - D_CIRCLE_DOT_SIZE),
							(int)((tempDotArray[i].x / D_TOOL_TO_GAME) + D_CIRCLE_DOT_SIZE),
							(int)((tempDotArray[i].y / D_TOOL_TO_GAME) + D_CIRCLE_DOT_SIZE)
							);
					}
				}


				if ( dotDataIndex != 3 )
				{
					// �� ����
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(DC_END);
					// �� �귯�� �� ����
					pDC->SelectObject(newBrush);
					// �� ����
					pDC->SelectObject(&l_my_pen);

					tempNote->getDragPoint(3, tempDotArray[3]);
					pDC->Ellipse(
						(int)((tempDotArray[3].x / D_TOOL_TO_GAME) - D_CIRCLE_SIZE),
						(int)((tempDotArray[3].y / D_TOOL_TO_GAME) - D_CIRCLE_SIZE),
						(int)((tempDotArray[3].x / D_TOOL_TO_GAME) + D_CIRCLE_SIZE),
						(int)((tempDotArray[3].y / D_TOOL_TO_GAME) + D_CIRCLE_SIZE)
						);
				}

				pDC->SelectObject(oldBrush);
				pDC->SelectObject(old_pen);
			}
		}



	}


	ReleaseDC(pDC);
}


// CNoteEditingToolView �μ�


void CNoteEditingToolView::OnFilePrintPreview()
{
#ifndef SHARED_HANDLERS
	AFXPrintPreview(this);
#endif
}

BOOL CNoteEditingToolView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// �⺻���� �غ�
	return DoPreparePrinting(pInfo);
}

void CNoteEditingToolView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: �μ��ϱ� ���� �߰� �ʱ�ȭ �۾��� �߰��մϴ�.
}

void CNoteEditingToolView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: �μ� �� ���� �۾��� �߰��մϴ�.
}

void CNoteEditingToolView::OnRButtonUp(UINT /* nFlags */, CPoint point)
{
	ClientToScreen(&point);
	OnContextMenu(this, point);
}

void CNoteEditingToolView::OnContextMenu(CWnd* /* pWnd */, CPoint point)
{
#ifndef SHARED_HANDLERS
	theApp.GetContextMenuManager()->ShowPopupMenu(IDR_POPUP_EDIT, point.x, point.y, this, TRUE);
#endif
}


// CNoteEditingToolView ����

#ifdef _DEBUG
void CNoteEditingToolView::AssertValid() const
{
	CView::AssertValid();
}

void CNoteEditingToolView::Dump(CDumpContext& dc) const
{
	CView::Dump(dc);
}

CNoteEditingToolDoc* CNoteEditingToolView::GetDocument() const // ����׵��� ���� ������ �ζ������� �����˴ϴ�.
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CNoteEditingToolDoc)));
	return (CNoteEditingToolDoc*)m_pDocument;
}
#endif //_DEBUG



afx_msg LRESULT CNoteEditingToolView::OnCmRedrawDragView(WPARAM wParam, LPARAM lParam)
{
	// �� �޽����� ���� �ٽ� �׸���.
	// �ʱ�ȭ��
	dotDataIndex = -1;
	OnFocusFlag = false;



	RedrawWindow();
	//AfxMessageBox(_T("�ڵ鷯 �۵��Ǿ���!"));

	return 0;
}


void CNoteEditingToolView::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰� ��/�Ǵ� �⺻���� ȣ���մϴ�.
	CNoteEditingToolDoc* pDoc = GetDocument();

	if( dotDataIndex >= 0 &&
		dotDataIndex <= 3 )
	{
		// ���� �ε����� ���� ���,
		OnFocusFlag = true;
	}
	


	CView::OnLButtonDown(nFlags, point);
}


void CNoteEditingToolView::OnLButtonUp(UINT nFlags, CPoint point)
{
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰� ��/�Ǵ� �⺻���� ȣ���մϴ�.
	CNoteEditingToolDoc* pDoc = GetDocument();

	CNoteData* tempNote = NULL;
	int nowConfHeadDir = -1;
	dotData tempDotData;

	tempDotData.x = point.x;
	tempDotData.y = point.y;

	// �����ϸ��Ѵ�.

	tempDotData.x = (int)(tempDotData.x * D_TOOL_TO_GAME);
	tempDotData.y = (int)(tempDotData.y * D_TOOL_TO_GAME);

	if( OnFocusFlag == true )
	{
		// ���� ���ٰ� ���콺�� ������ ���,
		if( dotDataIndex >= 0 &&
			dotDataIndex <= 3 )
		{
			// �ش� �ε����� ���� �����Ѵ�.
			tempNote = pDoc->getNowConfigingNote(nowConfHeadDir);
			if ( tempNote == NULL )
			{
				// ���� �ɰ��� ����
				AfxMessageBox(_T("�ε����� �����鼭 ��Ʈ �ּҰ� ����."));
				return;
			}
			
			// �ش� ����Ʈ�� �����Ѵ�.
			tempNote->setDragPoint(dotDataIndex, tempDotData);

			OnFocusFlag = false;



			RedrawWindow();
		}
		else
		{
			// ��Ŀ���� �����鼭 �ε����� ���� ���� ���� ������ �ִ� ��
			AfxMessageBox(_T("��Ŀ�� �÷��׿� �ε����� ����ġ!"));
		}
	}



	CView::OnLButtonUp(nFlags, point);
}


void CNoteEditingToolView::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰� ��/�Ǵ� �⺻���� ȣ���մϴ�.
	CNoteEditingToolDoc* pDoc = GetDocument();
	CDC* pDC = GetDC();							// ��ť��Ʈ�� �޾ƿ��� ��?

	CNoteData* tempNote = NULL;
	int noteConfigHeadDir = -1;
	dotData tempDotArray[MAX_DOT_DATA_NUM];
	int i=0;

	tempNote = pDoc->getNowConfigingNote(noteConfigHeadDir);

	// �� �׸��� �κ�
	if( tempNote != NULL )
	{
		// ���� �����߿� �ִ� ��Ʈ�� ���� ���,
		if( tempNote->getNoteType() == NOTE_T_DRAG )
		{
			// ��� ���̴� ���� �����.
			RedrawWindow();




			if ( OnFocusFlag == false )
			{
				// �׳� ���콺�� �����̰� �ִ� ���� ���.

				// �� ������ ��ǥ���� ��´�.
				for( i=0 ; i<4 ; i++ )
				{
					tempNote->getDragPoint(i, tempDotArray[i]);
				}

				// �� ������ ��ǥ���� �����ϰ� �ٲ۴�..
				for( i=0 ; i<4 ; i++ )
				{
					tempDotArray[i].x = (int)(tempDotArray[i].x / D_TOOL_TO_GAME);
					tempDotArray[i].y = (int)(tempDotArray[i].y / D_TOOL_TO_GAME);
				}

				// �� ������ ��ǥ���� ��ġ�ϴ� ���� �ִ��� Ȯ���Ѵ�.
				for( i=3 ; i>=0 ; i-- )
				{
					if ( point.x >= (int)(tempDotArray[i].x - D_CIRCLE_SIZE) &&
						point.x <= (int)(tempDotArray[i].x + D_CIRCLE_SIZE))
					{
						if( point.y >= (int)(tempDotArray[i].y - D_CIRCLE_SIZE) &&
							point.y <= (int)(tempDotArray[i].y + D_CIRCLE_SIZE))
						{
							CPen l_my_pen,*old_pen; // Pen ��ü�� �����Ѵ�. old_pen �����͵� 
							l_my_pen.CreatePen(PS_SOLID, 3, RGB(100, 240, 120));
							old_pen = pDC->SelectObject(&l_my_pen);

							pDC->SelectObject(&l_my_pen);

							if ( i == 0 || i == 3 )
							{						// �ش� ���� �ȿ� ���� ���, ���� �׷��ش�.
								pDC->Ellipse(
									tempDotArray[i].x - D_CIRCLE_SIZE - D_CIRCLE_STROK_SIZE,
									tempDotArray[i].y - D_CIRCLE_SIZE - D_CIRCLE_STROK_SIZE,
									tempDotArray[i].x + D_CIRCLE_SIZE + D_CIRCLE_STROK_SIZE,
									tempDotArray[i].y + D_CIRCLE_SIZE + D_CIRCLE_STROK_SIZE
									);
							}
							else
							{
								pDC->Ellipse(
									tempDotArray[i].x - D_CIRCLE_DOT_SIZE - D_CIRCLE_STROK_SIZE,
									tempDotArray[i].y - D_CIRCLE_DOT_SIZE - D_CIRCLE_STROK_SIZE,
									tempDotArray[i].x + D_CIRCLE_DOT_SIZE + D_CIRCLE_STROK_SIZE,
									tempDotArray[i].y + D_CIRCLE_DOT_SIZE + D_CIRCLE_STROK_SIZE
									);
							}



							pDC->SelectObject(old_pen);		//������ ����ϴ� pen�Ӽ����κ�����.
							l_my_pen.DeleteObject();


							// ���� Index�� �����ϰ�, 
							dotDataIndex = i;

							// ������ ������.
							break;
						}
					}
					else
					{
						dotDataIndex = -1;
					}
				}


				// ������ �


			}
			else
			{
				// ���� ���� �ִ� ���� ���.

				CPen l_my_pen,*old_pen; // Pen ��ü�� �����Ѵ�. old_pen �����͵� 
				l_my_pen.CreatePen(PS_SOLID, 3, RGB(100, 240, 120));
				old_pen = pDC->SelectObject(&l_my_pen);

				pDC->SelectObject(&l_my_pen);


				if ( dotDataIndex == 0 || dotDataIndex == 3 )
				{
					pDC->Ellipse(
						(int)(point.x - D_CIRCLE_SIZE - D_CIRCLE_STROK_SIZE),
						(int)(point.y - D_CIRCLE_SIZE - D_CIRCLE_STROK_SIZE),
						(int)(point.x + D_CIRCLE_SIZE + D_CIRCLE_STROK_SIZE),
						(int)(point.y + D_CIRCLE_SIZE + D_CIRCLE_STROK_SIZE)
						);
				}
				else
				{
					pDC->Ellipse(
						(int)(point.x - D_CIRCLE_DOT_SIZE - D_CIRCLE_STROK_SIZE),
						(int)(point.y - D_CIRCLE_DOT_SIZE - D_CIRCLE_STROK_SIZE),
						(int)(point.x + D_CIRCLE_DOT_SIZE + D_CIRCLE_STROK_SIZE),
						(int)(point.y + D_CIRCLE_DOT_SIZE + D_CIRCLE_STROK_SIZE)
						);
				}




				pDC->SelectObject(old_pen);		//������ ����ϴ� pen�Ӽ����κ�����.
				l_my_pen.DeleteObject();


			}


		
		}

	}


	CView::OnMouseMove(nFlags, point);
}




BOOL CNoteEditingToolView::OnEraseBkgnd(CDC* pDC)
{
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰� ��/�Ǵ� �⺻���� ȣ���մϴ�.
	CBrush backBrush(RGB(136, 136, 136));               // �Ķ���. ���ϴ� �÷��� �־��ָ� �ȴ�...

	CBrush* pOldBrush = pDC->SelectObject(&backBrush); 
	CRect rect; pDC->GetClipBox(&rect); 
	pDC->PatBlt(rect.left, rect.top, rect.Width(), rect.Height(), PATCOPY);
	pDC->SelectObject(pOldBrush); 

	return TRUE;                // TRUE �� ���־�� �Ѵ�. ������(return CView::OnEraseBkgnd(pDC);)


	//	��Ҷ� �÷��� �ȹٲٴ��� WM_ERASEBKGND �� �����Ͽ� return TURE �� �صθ�
	//	���� ���۸��� ���� ȭ���� ������ �ٽ� �׸��� ���� ���� �ؼ� �����̳��� ȭ���� �������� �� ���ϼ� �ִ�.
	//return CScrollView::OnEraseBkgnd(pDC);


	return CView::OnEraseBkgnd(pDC);
}


void CNoteEditingToolView::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	// TODO: ���⿡ �޽��� ó���� �ڵ带 �߰� ��/�Ǵ� �⺻���� ȣ���մϴ�.
	CNoteEditingToolDoc *pDoc = GetDocument();

	switch( nChar )
	{
	case VK_SPACE:
		if ( pDoc->getNotePickingViewPtr() != NULL )
		{
			pDoc->getNotePickingViewPtr()->SendMessage(CM_VK_SPACE3);
		}
		break;

	case '1':
		if ( pDoc->getEditModeSelectViewPtr() != NULL )
		{
			// �ʱ�ȭ ����
			pDoc->setOnFocusEditingFlag(false);
			pDoc->setNowEditingNote(NULL, 0);

			// ���� ���� ��ȯ
			pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE3, 0, EDIT_MODE_WRITE);
		}
		break;

	case '2':
		// ���� ���� ��ȯ
		// �ʱ�ȭ��
		OnFocusFlag = false;
		RedrawWindow();

		// �޽��� ����
		if ( pDoc->getEditModeSelectViewPtr() != NULL )
		{
			// �ʱ�ȭ ����
			pDoc->setOnFocusEditingFlag(false);
			pDoc->setNowEditingNote(NULL, 0);

			pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE3, 0, EDIT_MODE_ERASE);
		}
		break;

	case '3':
		// �̵� ���� ��ȯ
		// �ʱ�ȭ��
		//dotDataIndex = -1;
		OnFocusFlag = false;
		RedrawWindow();

		// �޽��� ����
		if ( pDoc->getEditModeSelectViewPtr() != NULL )
		{
			// �ʱ�ȭ ����
			pDoc->setOnFocusEditingFlag(false);
			pDoc->setNowEditingNote(NULL, 0);

			pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE3, 0, EDIT_MODE_MOVE);
		}
		break;

	case '4':
		// ���� ���� ��ȯ
		pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE3, 0, EDIT_MODE_CONFG);
		break;
	}


	CView::OnKeyDown(nChar, nRepCnt, nFlags);
}
