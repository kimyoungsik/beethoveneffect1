
// NoteEditingToolView.cpp : CNoteEditingToolView 클래스의 구현
//

#include "stdafx.h"
// SHARED_HANDLERS는 미리 보기, 축소판 그림 및 검색 필터 처리기를 구현하는 ATL 프로젝트에서 정의할 수 있으며
// 해당 프로젝트와 문서 코드를 공유하도록 해 줍니다.
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
	// 표준 인쇄 명령입니다.
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

// CNoteEditingToolView 생성/소멸

CNoteEditingToolView::CNoteEditingToolView()
{
	// TODO: 여기에 생성 코드를 추가합니다.
	dotDataIndex = -1;							// 평소에는 -1이다. 0~3 사이의 값을 가질 경우, 현재 포커싱 되어 있다는 뜻이다.
	OnFocusFlag = false;						// T일 경우, 현재 잡아끌고 있는 중이다.
}

CNoteEditingToolView::~CNoteEditingToolView()
{
}

BOOL CNoteEditingToolView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: CREATESTRUCT cs를 수정하여 여기에서
	//  Window 클래스 또는 스타일을 수정합니다.


	return CView::PreCreateWindow(cs);
}



// 드래그 노트 기본 좌표축을 그리는 함수.
int CNoteEditingToolView::DrawBackXYLine(CDC* pDC)
{

	CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 

	l_my_pen.CreatePen(PS_SOLID,3,RGB(0,0,0));
	old_pen = pDC->SelectObject(&l_my_pen);

	//////////////////////////////////////////////////////////////////////////
	// 여기서부터 l_my_pen의 속성을 적용 받는다.
	//////////////////////////////////////////////////////////////////////////


	// 가로, 세로 그리기
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




	pDC->SelectObject(old_pen);//기존에 사용하던 pen속성으로복구됨.

	return 1;
}





// CNoteEditingToolView 그리기

void CNoteEditingToolView::OnDraw(CDC* pDC)
{
	static int InitCounter = 0;						// Doc에 현재 View의 주소를 넘겨주기 위해 만들어진 1회용 변수.
	
	CNoteData* tempNote = NULL;
	int noteConfigHeadDir = -1;
	dotData tempDotArray[MAX_DOT_DATA_NUM];
	CRect tempRect;
	int i=0;

	// 색 칠하는 용.
	CBrush newBrush(RGB(255, 255, 255));
	CBrush* oldBrush;
	CPen l_my_pen,*old_pen;			 // Pen 객체만 생성한다. old_pen 포인터도 



	CNoteEditingToolDoc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);
	if (!pDoc)
		return;

	if ( InitCounter == 0)
	{
		pDoc->setNoteEditingToolViewPtr(this);
		InitCounter++;
	}


	// TODO: 여기에 원시 데이터에 대한 그리기 코드를 추가합니다.

	DrawBackXYLine(pDC);

	tempNote = pDoc->getNowConfigingNote(noteConfigHeadDir);

	// 원 그리는 부분
	if( tempNote != NULL )
	{
		// 현재 설정중에 있는 노트가 있을 경우,
		if( tempNote->getNoteType() == NOTE_T_DRAG )
		{
			// 만약 드래그 노트일 경우 화면에 원을 출력한다.


			if ( OnFocusFlag == false )
			{
				// 그냥 그릴 때


				// 연결선 먼저 그려준다.

				// 정보 받아 온 다음
				for( i=0 ; i<4 ; i++ )
				{
					tempNote->getDragPoint(i, tempDotArray[i]);
				}

	


				
				// 베지어 곡선 그리기
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


					// 펜의 특성 받아오기 (외곽선)
					l_my_pen.DeleteObject();
					l_my_pen.CreatePen(PS_SOLID, D_CIRCLE_SIZE*2, C_BEZIER_OUTER_LINE_COLOR);
					old_pen = pDC->SelectObject(&l_my_pen);

					// 그리는 함수
					PolyBezier(pDC->m_hDC, pline, 4);


					// 펜의 특성 받아오기 (외솩선)
					l_my_pen.DeleteObject();
					l_my_pen.CreatePen(PS_SOLID, D_CIRCLE_SIZE*2 - D_BEZIER_STROK_SIZE, C_BEZIER_INNER_LINE_COLOR);
					pDC->SelectObject(&l_my_pen);


					// 그리는 함수
					PolyBezier(pDC->m_hDC, pline, 4);



					// 펜 리턴
					pDC->SelectObject(old_pen);
				}	



				// 선 그리기
				pDC->MoveTo((int)(tempDotArray[0].x / D_TOOL_TO_GAME), (int)(tempDotArray[0].y / D_TOOL_TO_GAME));
				pDC->LineTo((int)(tempDotArray[1].x / D_TOOL_TO_GAME), (int)(tempDotArray[1].y / D_TOOL_TO_GAME));
				pDC->LineTo((int)(tempDotArray[2].x / D_TOOL_TO_GAME), (int)(tempDotArray[2].y / D_TOOL_TO_GAME));
				pDC->LineTo((int)(tempDotArray[3].x / D_TOOL_TO_GAME), (int)(tempDotArray[3].y / D_TOOL_TO_GAME));


				// 현재 브러시 내용을 받아온다.
				newBrush.DeleteObject();
				newBrush.CreateSolidBrush(DC_START);
				oldBrush = pDC->SelectObject(&newBrush);


				// 펜의 특성 받아오기 (외곽선용)
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



				// 색 설정
				newBrush.DeleteObject();
				newBrush.CreateSolidBrush(DC_CENTER);
				// 새 브러시 색 적용
				pDC->SelectObject(newBrush);
				// 펜 원래대로
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

				// 색 설정
				newBrush.DeleteObject();
				newBrush.CreateSolidBrush(DC_END);
				// 새 브러시 색 적용
				pDC->SelectObject(newBrush);
				// 펜 두꺼운 걸로
				pDC->SelectObject(&l_my_pen);


				//tempNote->getDragPoint(3, tempDotArray[3]);
				pDC->Ellipse(
					(int)((tempDotArray[3].x / D_TOOL_TO_GAME) - D_CIRCLE_SIZE),
					(int)((tempDotArray[3].y / D_TOOL_TO_GAME) - D_CIRCLE_SIZE),
					(int)((tempDotArray[3].x / D_TOOL_TO_GAME) + D_CIRCLE_SIZE),
					(int)((tempDotArray[3].y / D_TOOL_TO_GAME) + D_CIRCLE_SIZE)
					);


				pDC->SelectObject(oldBrush);
				// 펜 리턴
				pDC->SelectObject(old_pen);

				
			}
			else
			{
				// 그냥 끌고가는 중일 때

				newBrush.DeleteObject();
				newBrush.CreateSolidBrush(DC_START);

				// 현재 브러시 내용을 받아온다.
				oldBrush = pDC->SelectObject(&newBrush);

				// 펜의 특성 받아오기 (외곽선용)
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
						// 색 설정
						newBrush.DeleteObject();
						newBrush.CreateSolidBrush(DC_CENTER);
						// 새 브러시 색 적용
						pDC->SelectObject(newBrush);
						// 펜 원래대로
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
					// 색 설정
					newBrush.DeleteObject();
					newBrush.CreateSolidBrush(DC_END);
					// 새 브러시 색 적용
					pDC->SelectObject(newBrush);
					// 펜 적용
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


// CNoteEditingToolView 인쇄


void CNoteEditingToolView::OnFilePrintPreview()
{
#ifndef SHARED_HANDLERS
	AFXPrintPreview(this);
#endif
}

BOOL CNoteEditingToolView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// 기본적인 준비
	return DoPreparePrinting(pInfo);
}

void CNoteEditingToolView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: 인쇄하기 전에 추가 초기화 작업을 추가합니다.
}

void CNoteEditingToolView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: 인쇄 후 정리 작업을 추가합니다.
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


// CNoteEditingToolView 진단

#ifdef _DEBUG
void CNoteEditingToolView::AssertValid() const
{
	CView::AssertValid();
}

void CNoteEditingToolView::Dump(CDumpContext& dc) const
{
	CView::Dump(dc);
}

CNoteEditingToolDoc* CNoteEditingToolView::GetDocument() const // 디버그되지 않은 버전은 인라인으로 지정됩니다.
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CNoteEditingToolDoc)));
	return (CNoteEditingToolDoc*)m_pDocument;
}
#endif //_DEBUG



afx_msg LRESULT CNoteEditingToolView::OnCmRedrawDragView(WPARAM wParam, LPARAM lParam)
{
	// 이 메시지가 오면 다시 그린다.
	// 초기화들
	dotDataIndex = -1;
	OnFocusFlag = false;



	RedrawWindow();
	//AfxMessageBox(_T("핸들러 작동되었음!"));

	return 0;
}


void CNoteEditingToolView::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CNoteEditingToolDoc* pDoc = GetDocument();

	if( dotDataIndex >= 0 &&
		dotDataIndex <= 3 )
	{
		// 현재 인덱스가 있을 경우,
		OnFocusFlag = true;
	}
	


	CView::OnLButtonDown(nFlags, point);
}


void CNoteEditingToolView::OnLButtonUp(UINT nFlags, CPoint point)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CNoteEditingToolDoc* pDoc = GetDocument();

	CNoteData* tempNote = NULL;
	int nowConfHeadDir = -1;
	dotData tempDotData;

	tempDotData.x = point.x;
	tempDotData.y = point.y;

	// 스케일링한다.

	tempDotData.x = (int)(tempDotData.x * D_TOOL_TO_GAME);
	tempDotData.y = (int)(tempDotData.y * D_TOOL_TO_GAME);

	if( OnFocusFlag == true )
	{
		// 끌고 가다가 마우스를 놓았을 경우,
		if( dotDataIndex >= 0 &&
			dotDataIndex <= 3 )
		{
			// 해당 인덱스의 점을 수정한다.
			tempNote = pDoc->getNowConfigingNote(nowConfHeadDir);
			if ( tempNote == NULL )
			{
				// 역시 심각한 문제
				AfxMessageBox(_T("인덱스가 있으면서 노트 주소가 없다."));
				return;
			}
			
			// 해당 포인트를 수정한다.
			tempNote->setDragPoint(dotDataIndex, tempDotData);

			OnFocusFlag = false;



			RedrawWindow();
		}
		else
		{
			// 포커스가 있으면서 인덱스가 범위 밖인 것은 문제가 있는 것
			AfxMessageBox(_T("포커스 플래그와 인덱스의 불일치!"));
		}
	}



	CView::OnLButtonUp(nFlags, point);
}


void CNoteEditingToolView::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CNoteEditingToolDoc* pDoc = GetDocument();
	CDC* pDC = GetDC();							// 다큐멘트를 받아오는 것?

	CNoteData* tempNote = NULL;
	int noteConfigHeadDir = -1;
	dotData tempDotArray[MAX_DOT_DATA_NUM];
	int i=0;

	tempNote = pDoc->getNowConfigingNote(noteConfigHeadDir);

	// 원 그리는 부분
	if( tempNote != NULL )
	{
		// 현재 설정중에 있는 노트가 있을 경우,
		if( tempNote->getNoteType() == NOTE_T_DRAG )
		{
			// 계속 보이던 것을 지우고.
			RedrawWindow();




			if ( OnFocusFlag == false )
			{
				// 그냥 마우스를 움직이고 있는 중일 경우.

				// 각 점들의 좌표값을 얻는다.
				for( i=0 ; i<4 ; i++ )
				{
					tempNote->getDragPoint(i, tempDotArray[i]);
				}

				// 각 점들의 좌표값을 적절하게 바꾼다..
				for( i=0 ; i<4 ; i++ )
				{
					tempDotArray[i].x = (int)(tempDotArray[i].x / D_TOOL_TO_GAME);
					tempDotArray[i].y = (int)(tempDotArray[i].y / D_TOOL_TO_GAME);
				}

				// 각 점들의 좌표값과 일치하는 점이 있는지 확인한다.
				for( i=3 ; i>=0 ; i-- )
				{
					if ( point.x >= (int)(tempDotArray[i].x - D_CIRCLE_SIZE) &&
						point.x <= (int)(tempDotArray[i].x + D_CIRCLE_SIZE))
					{
						if( point.y >= (int)(tempDotArray[i].y - D_CIRCLE_SIZE) &&
							point.y <= (int)(tempDotArray[i].y + D_CIRCLE_SIZE))
						{
							CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
							l_my_pen.CreatePen(PS_SOLID, 3, RGB(100, 240, 120));
							old_pen = pDC->SelectObject(&l_my_pen);

							pDC->SelectObject(&l_my_pen);

							if ( i == 0 || i == 3 )
							{						// 해당 범위 안에 있을 경우, 원을 그려준다.
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



							pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
							l_my_pen.DeleteObject();


							// 현재 Index를 저장하고, 
							dotDataIndex = i;

							// 루프를 나간다.
							break;
						}
					}
					else
					{
						dotDataIndex = -1;
					}
				}


				// 베지어 곡선


			}
			else
			{
				// 끌고 가고 있는 중일 경우.

				CPen l_my_pen,*old_pen; // Pen 객체만 생성한다. old_pen 포인터도 
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




				pDC->SelectObject(old_pen);		//기존에 사용하던 pen속성으로복구됨.
				l_my_pen.DeleteObject();


			}


		
		}

	}


	CView::OnMouseMove(nFlags, point);
}




BOOL CNoteEditingToolView::OnEraseBkgnd(CDC* pDC)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
	CBrush backBrush(RGB(136, 136, 136));               // 파랑색. 원하는 컬러를 넣어주면 된다...

	CBrush* pOldBrush = pDC->SelectObject(&backBrush); 
	CRect rect; pDC->GetClipBox(&rect); 
	pDC->PatBlt(rect.left, rect.top, rect.Width(), rect.Height(), PATCOPY);
	pDC->SelectObject(pOldBrush); 

	return TRUE;                // TRUE 로 해주어야 한다. 기존것(return CView::OnEraseBkgnd(pDC);)


	//	평소때 컬러를 안바꾸더라도 WM_ERASEBKGND 를 정의하여 return TURE 로 해두면
	//	더블 버퍼링을 쓸때 화면의 바탕을 다시 그리는 일이 없게 해서 조금이나마 화면의 깜박임을 더 줄일수 있다.
	//return CScrollView::OnEraseBkgnd(pDC);


	return CView::OnEraseBkgnd(pDC);
}


void CNoteEditingToolView::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	// TODO: 여기에 메시지 처리기 코드를 추가 및/또는 기본값을 호출합니다.
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
			// 초기화 먼저
			pDoc->setOnFocusEditingFlag(false);
			pDoc->setNowEditingNote(NULL, 0);

			// 쓰기 모드로 전환
			pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE3, 0, EDIT_MODE_WRITE);
		}
		break;

	case '2':
		// 삭제 모드로 전환
		// 초기화들
		OnFocusFlag = false;
		RedrawWindow();

		// 메시지 전송
		if ( pDoc->getEditModeSelectViewPtr() != NULL )
		{
			// 초기화 먼저
			pDoc->setOnFocusEditingFlag(false);
			pDoc->setNowEditingNote(NULL, 0);

			pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE3, 0, EDIT_MODE_ERASE);
		}
		break;

	case '3':
		// 이동 모드로 전환
		// 초기화들
		//dotDataIndex = -1;
		OnFocusFlag = false;
		RedrawWindow();

		// 메시지 전송
		if ( pDoc->getEditModeSelectViewPtr() != NULL )
		{
			// 초기화 먼저
			pDoc->setOnFocusEditingFlag(false);
			pDoc->setNowEditingNote(NULL, 0);

			pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE3, 0, EDIT_MODE_MOVE);
		}
		break;

	case '4':
		// 설정 모드로 전환
		pDoc->getEditModeSelectViewPtr()->SendMessage(CM_EDITING_MODE_CHANGE3, 0, EDIT_MODE_CONFG);
		break;
	}


	CView::OnKeyDown(nChar, nRepCnt, nFlags);
}
