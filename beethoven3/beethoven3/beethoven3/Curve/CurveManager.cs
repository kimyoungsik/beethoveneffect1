using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class CurveManager
    {
        #region declarations
        private List<Curve> Curves = new List<Curve>();
        private LineRenderer lineRenderer;
        private LineRenderer dragLineMarkerRenderer;
        private DragNoteManager dragNoteManager;

        public CurveManager(LineRenderer lineRenderer, LineRenderer dragLineMarkerRenderer, DragNoteManager dragNoteManager )
        {
            this.lineRenderer = lineRenderer;
            this.dragLineMarkerRenderer = dragLineMarkerRenderer;
            this.dragNoteManager = dragNoteManager;
        }
      //  public static List<Curve> GuideLines = new List<Curve>();
        #endregion
          
        #region method
        /// <summary>
        /// 커브곡선 추가
        /// </summary>
        /// <param name="p0">시작점</param>
        /// <param name="p1">제어점1</param>
        /// <param name="p2">제어점1</param>
        /// <param name="p3">끝나는점</param>
        /// <param name="time">지속시간</param>
        public void addCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time)
        {

            Curve curve = new Curve(p0, p1, p2, p3, time, lineRenderer, dragLineMarkerRenderer, dragNoteManager);
            Curves.Add(curve);    
        }

        public void DeleteAllCurve()
        {
            foreach (Curve curve in Curves)
            {
                curve.DeleteAllPoints();
                curve.End = true;
            }

        }


    //public static void addGuideLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time)
    //{
    //    Curve curve = new Curve(p0, p1, p2, p3, time);
    //    GuideLines.Add(curve);
    //}
        #endregion


        #region update and draw


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Curve curve in Curves)
            {
                curve.Draw(gameTime,  spriteBatch);
            }
            //foreach (Curve guideLine in GuideLines)
            //{
            //    guideLine.Draw(gameTime, spriteBatch);
            //}
        }

        #endregion
    }
}


