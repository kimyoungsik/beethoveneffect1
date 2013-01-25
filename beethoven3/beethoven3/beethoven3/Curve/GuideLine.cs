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
    class GuideLine
    {

        #region declarations
        // public static List<Vector2> points = new List<Vector2>();
     //   public Queue PointsQueue = new Queue();
        private List<Vector2> Points = new List<Vector2>();
        //곡선의 기준시간
        private double time = 0.0f;
        //경과 시간
        private double changedTime = 0.0f;

    //    private double dotTime = 0.0f;
        private double dotChangedTime = 0.0f;

     //   private Vector2 currentPosition;
     //   private int count = 0;

        private bool end;
        private bool goldEnd = false;
        private bool showGold;

        private LineRenderer lineRenderer;

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
        public GuideLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time, bool showGold, LineRenderer lineRenderer)
        {
            this.lineRenderer = lineRenderer;
            //다른 골드라인을 만들기 전에 , 지워주기
            if (showGold)
            {
                GoldManager.DeleteAll();
            } 
            this.showGold = showGold;
            SetLine(p0, p1, p2, p3, time);
            
        }
        #endregion

        #region method
        public bool ShowGold
        {
            get { return showGold; }
            set { showGold = value; }

        }
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
        /// 

        public void SetLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time)
        {
            Vector2 PlotPoint;

            this.changedTime = 0.0;
            this.dotChangedTime = 0.0;
            this.time = time;

            float t;
            for (t = 0; t <= 1.0f; t += 0.01f)
            {
                PlotPoint = GetPoint(t, p0, p1, p2, p3);
                Points.Add(PlotPoint);
               // PointsQueue.Enqueue(PlotPoint);
            }
         //   dotTime = time / PointsQueue.Count;
            if (this.showGold)
            {
                int i = 0;
                int j = 0;
                while (i < Points.Count)
                {
                    //골드의 갯수
                    if (j == Points.Count / 10)
                    {
                        GoldManager.MakeGold(Points[i], new Vector2(0, 0));
                        j = 0;
                    }

                    i++;
                    j++;

                }

               
                goldEnd = false;
            }
            end = false;
        }

        public void DeleteAllPoints()
        {
            int i;
            for (i = 0; i < Points.Count(); i++)
            {
                Points.RemoveAt(i);
            }

        }
        #endregion

        #region update and draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //int i, j;

            if (Points.Count > 0 && !end)
            {
                changedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
               // dotChangedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                

                //동전라인 아닌 가이드라인 
                //if (!showGold)
                //{
                //    for (i = 0; i < Points.Count - 1; i++)
                //    {
                //        j = i + 1;
                //        Color color = Color.White;
                //        //color.A = 50;
                //        //라인 그리기
                //        lineRenderer.DrawLine(Game1.spriteSheet, new Rectangle(0, 0, 20, 20), spriteBatch.GraphicsDevice, spriteBatch, (Vector2)Points[i], (Vector2)Points[j], color);
                //    }
                //}
              
              
                if (changedTime > time)
                {
                   
                    if (Points.Count > 0)
                    {
                        //지워지기 시작 
                        DeleteAllPoints();

                        //이걸 하면
                        //두번째 동전만 안나오기 시작함..
                        //if (this.showGold)
                        //{
                        //    //동전삭제
                        //    GoldManager.DeleteAll();
                        //}
                            //지워지기 시작하면 true -> 화면에서 안보이게 함
                        end = true;
                    }
                }
            }
        }
        #endregion

    }
}

