using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using System.Diagnostics;


namespace beethoven3
{
    /// <summary>
    /// 드래그 노트 커브를 나타냄
    /// </summary>
    class Curve
    {

        #region declarations
        // public static List<Vector2> points = new List<Vector2>();
        private Queue PointsQueue = new Queue();
        private List<Vector2> Points = new List<Vector2>();
        //곡선의 기준시간
       // private double time = 0.0f;
        //경과 시간
        private double changedTime = 0.0f;

        private double dotTime = 0.0f;
        private double dotChangedTime = 0.0f;


        private double startTime = 0.0f;
        private double endTime = 0.0f;


        private Vector2 currentPosition;
        private int count = 0;

        private bool end = false;

        private LineRenderer lineRenderer;
        private LineRenderer dragLineMarkerRenderer;
        private Vector2 startVector = Vector2.Zero;
        private Vector2 endVector = Vector2.Zero;
        private DragNoteManager dragNoteManager;


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
        public Curve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double startTime, double endTime, LineRenderer lineRenderer, LineRenderer dragLineMarkerRenderer, DragNoteManager dragNoteManager)
        {

            this.distance = Vector2.Distance(p0, p1) + Vector2.Distance(p1, p2) + Vector2.Distance(p2, p3);
            SetLine(p0, p1, p2, p3, startTime, endTime);
            this.lineRenderer = lineRenderer;
            this.dragLineMarkerRenderer = dragLineMarkerRenderer;
            this.dragNoteManager = dragNoteManager;
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

        //private double GetNormalTime()
        //{

        //    double nomalTime = 0.0f;
        //    if (time <= 1000)
        //    {
        //        nomalTime = 60.0f;

        //    }
        //    else if (time <= 2000)
        //    {
        //        nomalTime = 120.0f;

        //    }
        //    else if (time <= 3000)
        //    {
        //        nomalTime = 150.0f;

        //    }
        //    else if (time <= 4000)
        //    {
        //        nomalTime = 210.0f;

        //    }
        //    else if (time <= 5000)
        //    {

        //        nomalTime = 270.0f;
        //    }
        //    else if (time <= 6000)
        //    {
        //        nomalTime = 300.0f;

        //    }
        //    else if (time <= 7000)
        //    {
        //        nomalTime = 330.0f;

        //    }
        //    else if (time <= 8000)
        //    {
        //        nomalTime = 400.0f;

        //    }
        //    else if (time <= 9000)
        //    {
        //        nomalTime = 400.0f;

        //    }
        //    else if (time <= 10000)
        //    {
        //        nomalTime = 400.0f;

        //    }
        //    else
        //    {
        //        nomalTime = 450.0f;

        //    }
        //    return nomalTime;


        //}


        public double ChangeSpeed()
        {

            double minB = 60.0f;
            //double minV = 85.0f;
            double minV = 60;


            double midB = 120.0f;
            double midV = 0;

            double maxB = 240.0f;
            double maxV = -60f;


            double speed = 0.0f;

            

            //위 보간
            if (dragNoteSpeed > midB && dragNoteSpeed < maxB)
            {


                speed = ((((dragNoteSpeed - midB) / (maxB - midB)) * (maxV - midV)) + midV);



            }

                //아래 보간
            else if (dragNoteSpeed < midB && dragNoteSpeed >= minB)
            {

                speed = ((((dragNoteSpeed - minB) / (midB - minB)) * (midV - minV)) + minV);


            }
            else if (dragNoteSpeed == midB)
            {
                speed = midV;
            }
            else if (dragNoteSpeed == maxB)
            {
                speed = maxV;
            }

            else
            {
                speed = midV;
                //bpm 오류
            }

            return speed;



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
            
        //    this.changedTime = 0.0;
         //   this.dotChangedTime = 0.0;

            //총시간
            double time = endTime - startTime;

            float t;

           
            int count = (int)(time * 60);


            //큐와 배열에 
            for (int j = 0; j <= count; j++)
            {
                t = j / (float)count;
                
                
                PlotPoint = GetPoint(t, p0, p1, p2, p3);
                Points.Add(PlotPoint);
                 
               // 전부다 넣을 필요가 없고
             //   if (i % 2 == 0)
             //   {
                    PointsQueue.Enqueue(PlotPoint);
              //  }
            }

            //200개의 포인트를 저장해 둔다.
            //100개의 포인트를 찍을 곳은 중간위치 정가운데 시간이다.
                        
            //나누어주는 count에 더하는 수를 크게하면 드래그노트 안을 지나가는 공이 빨라진다. 
            //이것이 드래그 노트 속도에 영향을 줌
            //dotTime = time / (PointsQueue.Count + dragNoteSpeed);
            //총시간을 포인트의 카운트 수만큼 나눈수 즉 하의 큐를 표현하는데 필요한 시간 
            
          //  dotTime = time / (PointsQueue.Count );
            end = false;
        }

        public void DeleteAllPoints()
        {
            int i;
            for(i=0; i<Points.Count(); i++)
            {
                Points.RemoveAt(i);
            }
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

                        //라인 그리기
                        lineRenderer.DrawLine(Game1.drawLineNote1, new Rectangle(0, 0, 100, 100), spriteBatch.GraphicsDevice, spriteBatch, (Vector2)Points[i], (Vector2)Points[j], Color.White);


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


                        dotChangedTime = 0.0f;

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

                    spriteBatch.Draw(Game1.one, new Rectangle(0, 0, 275, 376), Color.White);

                }
                else if (processTime >= TimeSpan.FromSeconds(startTime - 2))//8초
                {
                    spriteBatch.Draw(Game1.two, new Rectangle(0, 0, 275, 376), Color.White);


                }
                //start 10초가 가정
                else if (processTime >= TimeSpan.FromSeconds(startTime - 3))//7초        
                {

                    spriteBatch.Draw(Game1.three, new Rectangle(0, 0, 275, 376), Color.White);

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

