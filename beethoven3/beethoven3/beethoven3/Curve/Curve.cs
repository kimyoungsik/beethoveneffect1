using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using System.Diagnostics;


namespace beethoven3
{
    /// <summary>
    /// 드래그 노트 커브를 나타냄
    /// </summary>
    class Curve
    {

        #region declarations
         private Queue PointsQueue = new Queue();
        private List<Vector2> Points = new List<Vector2>();
           //경과 시간
      //  private double changedTime = 0.0f;

//        private double dotTime = 0.0f;
      //  private double dotChangedTime = 0.0f;


        private double startTime = 0.0f;
        private double endTime = 0.0f;

       private  Color color = Color.White;
                   

        private Vector2 currentPosition;
        private int count = 0;

        private bool end = false;

        private LineRenderer lineRenderer;
        private LineRenderer dragLineMarkerRenderer;
        private Vector2 startVector = Vector2.Zero;
        private Vector2 endVector = Vector2.Zero;
        private DragNoteManager dragNoteManager;
        private ItemManager itemManager;

        private double distance; 

        public static int dragNoteSpeed= 120;

        #endregion

        #region constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p0">시작점</param>
        /// <param name="p1">제어점1</param>
        /// <param name="p2">제어점1</param>
        /// <param name="p3">끝나는점</param>
        /// <param name="time">지속시간</param>
        public Curve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double startTime, double endTime, LineRenderer lineRenderer, LineRenderer dragLineMarkerRenderer, DragNoteManager dragNoteManager, ItemManager itemManager)
        {
            this.dragNoteManager = dragNoteManager;
            this.distance = Vector2.Distance(p0, p1) + Vector2.Distance(p1, p2) + Vector2.Distance(p2, p3);
            SetLine(p0, p1, p2, p3, startTime, endTime);
            this.lineRenderer = lineRenderer;
            this.dragLineMarkerRenderer = dragLineMarkerRenderer;
            this.itemManager = itemManager;
            this.startTime = startTime;
            this.endTime = endTime;

        }
        #endregion

        #region method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p0">시작점</param>
        /// <param name="p1">제어점1</param>
        /// <param name="p2">제어점1</param>
        /// <param name="p3">끝나는점</param>
        
        private Vector2 GetPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float cx = 3 * (p1.X - p0.X);
            float cy = 3 * (p1.Y - p0.Y);

            float bx = 3 * (p2.X - p1.X) - cx;
            float by = 3 * (p2.Y - p1.Y) - cy;

            float ax = p3.X - p0.X - cx - bx;
            float ay = p3.Y - p0.Y - cy - by;

            float Cube = t * t * t;
            float Square = t * t;

            float resX = (ax * Cube) + (bx * Square) + (cx * t) + p0.X;
            float resY = (ay * Cube) + (by * Square) + (cy * t) + p0.Y;

            return new Vector2(resX, resY);
        }

      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p0">시작점</param>
        /// <param name="p1">제어점1</param>
        /// <param name="p2">제어점1</param>
        /// <param name="p3">끝나는점</param>
        /// <param name="time">지속시간</param>
        public void SetLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double startTime, double endTime)
        {
            Vector2 PlotPoint;
            float t;
            int j;
            int count = (int)((endTime - startTime) * Game1._fps);
           
            //큐와 배열에 
            for (j = 0; j <= count; j++)
            {
                t = j / (float)count;
                
                
                PlotPoint = GetPoint(t, p0, p1, p2, p3);
                Points.Add(PlotPoint);
                 
                    PointsQueue.Enqueue(PlotPoint);
            }

             end = false;
        }

        public void DeleteAllPoints()
        {
            //int i;
            //int count = Points.Count();
            //for(i=0; i<count; i++)
            //{
                Points.Clear();
            //    Points.RemoveAt(i);
            //}
            dragNoteManager.Delete();
        }


        public bool End
        {
            get { return end; }
            set { end = value; }
        }

        #endregion

        #region update and draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, TimeSpan processTime)
        {
            int i, j;
             

              // 4        15
            if (Points.Count > 0 && processTime <= TimeSpan.FromSeconds(endTime))
            {
              //  startTime += gameTime.ElapsedGameTime.TotalMilliseconds;


                //3초전이면 나타나기            7 -3 = 4                            
                if (processTime >= TimeSpan.FromSeconds(startTime - 3))
                {
                    ////전체 라인을 그리는 것
                    ////시간을 앞당겨서 미리 보여주는것도 이것
                    for (i = 0; i < Points.Count - 1; i++)
                    {
                        j = i + 1;

                        //라인 그리기//&&&
                        
                        lineRenderer.DrawLine(dragNoteManager.Background, new Rectangle(0, 0, 100, 100), spriteBatch.GraphicsDevice, spriteBatch, (Vector2)Points[i], (Vector2)Points[j], Color.White);


                    }
                   
                }

                //10초
                if (processTime >= TimeSpan.FromSeconds(startTime))
                {
               
                    if (PointsQueue.Count > 1)
                    {

                        if (count == 0)
                        {
                            currentPosition = (Vector2)PointsQueue.Peek();
                            //판별하는 마크를 만든다.

                            dragNoteManager.MakeDragNote(currentPosition, new Vector2(0, 0));
                        }
                        count++; 
                        if (count == 10)
                        {
                            dragNoteManager.DeleteDragNotes();
                        }
                        if (count == 30)
                        {
                            count = 0;
                            //드래그 노트 실패 띄우기 1
                            //맨 앞의 것만 지우게 되어있는데 문제 있으면 전부다 지우는것으로 .
                        }

                        //따라다니면서 마크 찍는 것
                        //공굴러가거나 움직이는 스프라이트로 너무 깜빡인다 싶으면 20을 좀 줄이면 됨

                        startVector = (Vector2)PointsQueue.Dequeue();
                        endVector = (Vector2)PointsQueue.Peek();


                        //dotChangedTime = 0.0f;

                    }

                    if (startVector != Vector2.Zero && endVector != Vector2.Zero)
                    {
                        //spriteBatch.Draw(DragNoteManager.Texture, startVector, Color.White);

                        dragLineMarkerRenderer.DrawLine(dragNoteManager.Texture, dragNoteManager.InitialFrame, spriteBatch.GraphicsDevice, spriteBatch, startVector, endVector, Color.White);

                    }

            

                }

                    ///@@@카운트 
                else if (processTime >= TimeSpan.FromSeconds(startTime - 1))//9초
                {
                    Texture2D[] dragNoteTexture = itemManager.GetDragNoteStartTexters();
                    Rectangle[] initFrame = itemManager.GetDragNoteStartInitFrame();
        

                    spriteBatch.Draw(dragNoteTexture[itemManager.getNoteIndex()], new Rectangle((int)Points[0].X, (int)Points[0].Y, initFrame[itemManager.getNoteIndex()].Width, initFrame[itemManager.getNoteIndex()].Height), color);
                    spriteBatch.Draw(Game1.one, new Rectangle(10, 50, 150, 150), Color.White);

                }
                else if (processTime >= TimeSpan.FromSeconds(startTime - 2))//8초
                {
                    Texture2D[] dragNoteTexture = itemManager.GetDragNoteStartTexters();
                    Rectangle[] initFrame = itemManager.GetDragNoteStartInitFrame();
                          spriteBatch.Draw(dragNoteTexture[itemManager.getNoteIndex()], new Rectangle((int)Points[0].X, (int)Points[0].Y, initFrame[itemManager.getNoteIndex()].Width, initFrame[itemManager.getNoteIndex()].Height), color);
                    spriteBatch.Draw(Game1.two, new Rectangle(10, 50, 150, 150), Color.White);


                }
                //start 10초가 가정
                else if (processTime >= TimeSpan.FromSeconds(startTime - 3))//7초        
                {
                    Texture2D[] dragNoteTexture = itemManager.GetDragNoteStartTexters();
                    Rectangle[] initFrame = itemManager.GetDragNoteStartInitFrame();
                    

                    spriteBatch.Draw(dragNoteTexture[itemManager.getNoteIndex()], new Rectangle((int)Points[0].X, (int)Points[0].Y, initFrame[itemManager.getNoteIndex()].Width, initFrame[itemManager.getNoteIndex()].Height), color);
                    spriteBatch.Draw(Game1.three, new Rectangle(10, 50, 150, 150), Color.White);

                }
               
                //
            }
            else
            {

                if (Points.Count > 0)
                {
                    //지워지기 시작 
                    DeleteAllPoints();

                    //지워지기 시작하면 true -> 화면에서 안보이게 함
                    end = true;

                    //마지막남은 것 지우기

                    //드래그 노트 실패 띄우기 2
                    dragNoteManager.DeleteDragNotes();
                }
            }
        }
        #endregion

    }
}

