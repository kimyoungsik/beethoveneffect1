using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace beethoven3
{
    class Curve
    {

        #region declarations
        // public static List<Vector2> points = new List<Vector2>();
        //public Queue Points = new Queue();
        private List<Vector2> Points = new List<Vector2>();
        private double time = 0.0;
        private double changedTime = 0.0;
        #endregion



        #region constructor
        public Curve()
        {
        }
        #endregion



        #region method

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

        public void SetLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time)
        {
            Vector2 PlotPoint;
            float t;

            this.time = time;
            for (t = 0;  t <= 1.0f; t += 0.01f)
            {
                PlotPoint = GetPoint(t, p0, p1, p2, p3);
               // points.Add(PlotPoint);
                Points.Add(PlotPoint);
            }

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
          
            if (Points.Count > 0)
            {
                changedTime += gameTime.ElapsedGameTime.TotalSeconds;

                for (i = 0; i < Points.Count - 1; i++)
                {
                    j = i + 1;

                    LineRenderer.DrawLine(spriteBatch.GraphicsDevice, spriteBatch, (Vector2)Points[i], (Vector2)Points[j], Color.Red);
                }
             
                if (changedTime > time)
                {
                    if (Points.Count > 0)
                    {
                        DeleteAllPoints();
                    
                    }

                }
            }
        }

        #endregion

    }
}
