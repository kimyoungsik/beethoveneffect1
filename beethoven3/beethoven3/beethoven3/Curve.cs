﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;


namespace beethoven3
{
    class Curve
    {

        #region declarations
        // public static List<Vector2> points = new List<Vector2>();
        public Queue PointsQueue = new Queue();
        private List<Vector2> Points = new List<Vector2>();
        //곡선의 기준시간
        private double time = 0.0f;
        //경과 시간
        private double changedTime = 0.0f;

        private double dotTime = 0.0f;
        private double dotChangedTime = 0.0f;
        
        private Vector2 currentPosition;
        private int count = 0;

        private bool end = false;


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
        public Curve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time)
        {
            SetLine(p0, p1, p2, p3, time);
       
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
        public void SetLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time)
        {
            Vector2 PlotPoint;
            
            this.changedTime = 0.0;
            this.dotChangedTime = 0.0;
            this.time = time;

            float t;
            for (t = 0;  t <= 1.0f; t += 0.01f)
            {
                PlotPoint = GetPoint(t, p0, p1, p2, p3);
                Points.Add(PlotPoint);
                PointsQueue.Enqueue(PlotPoint);
            }
            dotTime = time / PointsQueue.Count;
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


                changedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                
                
                dotChangedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                for (i = 0; i < Points.Count - 1; i++)
                {
                    j = i + 1;

                    //라인 그리기
                    LineRenderer.DrawLine(Game1.spriteSheet, new Rectangle(0, 0, 50, 50), spriteBatch.GraphicsDevice, spriteBatch, (Vector2)Points[i], (Vector2)Points[j], Color.White);

                    
                }

                if (dotChangedTime >= dotTime && PointsQueue.Count > 1)
                {
                    
                    if (count == 0)
                    {
                        currentPosition = (Vector2)PointsQueue.Peek();
                        //판별하는 마크
                        DragNoteManager.MakeDragNote(currentPosition, new Vector2(0,0));
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
                    LineRenderer.DrawLine(Game1.spriteSheet, new Rectangle(0, 100, 50, 50), spriteBatch.GraphicsDevice, spriteBatch, (Vector2)PointsQueue.Dequeue(), (Vector2)PointsQueue.Peek(), Color.White);
                    dotChangedTime = 0.0f;

                }
                if (changedTime > time)
                {
                    Trace.WriteLine("time:" + time + "changedTime:" + changedTime);

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
        #endregion

    }
}

