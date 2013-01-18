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
        private double time = 0.0f;
        //경과 시간
        private double changedTime = 0.0f;

        private double dotTime = 0.0f;
        private double dotChangedTime = 0.0f;


        private double startTime = 0.0f;
        private Vector2 currentPosition;
        private int count = 0;

        private bool end = false;

        private LineRenderer lineRenderer;
        private LineRenderer dragLineMarkerRenderer;
        private Vector2 startVector = Vector2.Zero;
        private Vector2 endVector = Vector2.Zero;

       

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
        public Curve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time, LineRenderer lineRenderer, LineRenderer dragLineMarkerRenderer)
        {
            SetLine(p0, p1, p2, p3, time);
            this.lineRenderer = lineRenderer;
            this.dragLineMarkerRenderer = dragLineMarkerRenderer;
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

        private double GetNormalTime()
        {

            double nomalTime = 0.0f;
            if (time <= 1000)
            {
                nomalTime = 60.0f;

            }
            else if (time <= 2000)
            {
                nomalTime = 120.0f;

            }
            else if (time <= 3000)
            {
                nomalTime = 150.0f;

            }
            else if (time <= 4000)
            {
                nomalTime = 210.0f;

            }
            else if (time <= 5000)
            {

                nomalTime = 270.0f;
            }
            else if (time <= 6000)
            {
                nomalTime = 300.0f;

            }
            else if (time <= 7000)
            {
                nomalTime = 330.0f;

            }
            else if (time <= 8000)
            {
                nomalTime = 400.0f;

            }
            else if (time <= 9000)
            {
                nomalTime = 400.0f;

            }
            else if (time <= 10000)
            {
                nomalTime = 400.0f;

            }
            else
            {
                nomalTime = 450.0f;

            }
            return nomalTime;


        }


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
        public void SetLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time)
        {
            Vector2 PlotPoint;
            
            this.changedTime = 0.0;
            this.dotChangedTime = 0.0;

            //총시간
            this.time = time;

            float t;

            //각 포인트를 넣어둔다. 

            ////큐와 배열에 
            //for (t = 0;  t <= 1.0f; t += 0.01f)
            //{
            //    PlotPoint = GetPoint(t, p0, p1, p2, p3);
            //    Points.Add(PlotPoint);

            //    //전부다 넣을 필요가 없고
            //    if (t % 0.1 == 0)
            //    {

            //        PointsQueue.Enqueue(PlotPoint);
            //    }
            //}
            //이것을 초과하면 안됨
            //1초 60
            //2초 120
            //3초 150적당
            //4초 210
            //5초 270
            //6초 330


            //적당한 포인트 수를 가져온다. 


            double nomalTime = GetNormalTime();


            //dragNoteSpeed 가 120보다 얼마나 더 크냐에 따라서 normalTime을 더 낮춘다. 낮출수록 빨라지고 높일수록 느려진다. 

            double speedChagneTime = ChangeSpeed();


            //bpm에 따라서 달라지는 속도, 이것은 2초 이상일때만

            if (time > 2000)
            {
                          nomalTime += speedChagneTime;
            }

            int i;

            //큐와 배열에 
            for (i = 0; i <= nomalTime; i++)
            {
                t = i / (float)nomalTime;
                PlotPoint = GetPoint(t, p0, p1, p2, p3);
                Points.Add(PlotPoint);
                 
               // 전부다 넣을 필요가 없고
             //   if (i % 2 == 0)
             //   {
                    PointsQueue.Enqueue(PlotPoint);
              //  }
            }




            //나누어주는 count에 더하는 수를 크게하면 드래그노트 안을 지나가는 공이 빨라진다. 
            //이것이 드래그 노트 속도에 영향을 줌
            //dotTime = time / (PointsQueue.Count + dragNoteSpeed);
            //총시간을 포인트의 카운트 수만큼 나눈수 즉 하의 큐를 표현하는데 필요한 시간 
                 
            
            dotTime = time / (PointsQueue.Count );
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
        #endregion

        #region update and draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int i, j;
             


            if (Points.Count > 0 && !end)
            {
                startTime += gameTime.ElapsedGameTime.TotalMilliseconds;



                //전체 라인을 그리는 것
                //시간을 앞당겨서 미리 보여주는것도 이것
                for (i = 0; i < Points.Count - 1; i++)
                {
                    j = i + 1;

                    //라인 그리기
                    lineRenderer.DrawLine(Game1.drawLineNote1, new Rectangle(0, 0, 100, 100), spriteBatch.GraphicsDevice, spriteBatch, (Vector2)Points[i], (Vector2)Points[j], Color.White);


                }
                if (startTime > 20 && startTime < 1000)
                {
                    spriteBatch.Draw(Game1.three, new Rectangle(0, 0, 275, 376), Color.White);

                    
                }
                else if (startTime > 1000 && startTime < 2000)
                {
                    spriteBatch.Draw(Game1.two, new Rectangle(0, 0, 275, 376), Color.White);


                }
                else if (startTime > 2000 && startTime < 3000)
                {

                    spriteBatch.Draw(Game1.one, new Rectangle(0, 0, 275, 376), Color.White);

                }
               

                if (startTime > 3000)
                {
                    //게임이 진행하는 전체 시간

                    changedTime += gameTime.ElapsedGameTime.TotalMilliseconds;

                    dotChangedTime += gameTime.ElapsedGameTime.TotalMilliseconds;



                    //하나의 점이 /
                    //     if (dotChangedTime >= dotTime && PointsQueue.Count > 1)
                    if (PointsQueue.Count > 1)
                    {

                        if (count == 0)
                        {
                            currentPosition = (Vector2)PointsQueue.Peek();
                            //판별하는 마크를 만든다.

                            DragNoteManager.MakeDragNote(currentPosition, new Vector2(0, 0));
                        }
                        count++;
                        if (count == 10)
                        {
                            DragNoteManager.DeleteDragNotes();
                        }
                        if (count == 20)
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

                        dragLineMarkerRenderer.DrawLine(DragNoteManager.Texture, DragNoteManager.InitialFrame, spriteBatch.GraphicsDevice, spriteBatch, startVector, endVector, Color.White);

                    }



                    if (changedTime > time)
                    {

                        if (Points.Count > 0)
                        {
                            //지워지기 시작 
                            DeleteAllPoints();

                            //지워지기 시작하면 true -> 화면에서 안보이게 함
                            end = true;

                            //마지막남은 것 지우기

                            //드래그 노트 실패 띄우기 2
                            DragNoteManager.DeleteDragNotes();
                        }
                    }

                }
            }
        }
        #endregion

    }
}

