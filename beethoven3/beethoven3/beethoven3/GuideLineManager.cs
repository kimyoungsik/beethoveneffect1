using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    static class GuideLineManager
    {
        #region declarations
        public static List<GuideLine> GuideLines = new List<GuideLine>();

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
        public static void AddGuideLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time)
        {
            GuideLine guideLine = new GuideLine(p0, p1, p2, p3, time);
            GuideLines.Add(guideLine);
        }


        //public static void addGuideLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time)
        //{
        //    Curve curve = new Curve(p0, p1, p2, p3, time);
        //    GuideLines.Add(curve);
        //}
        #endregion


        #region update and draw


        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GuideLine guideLine in GuideLines)
            {
                guideLine.Draw(gameTime, spriteBatch);
            }
            //foreach (Curve guideLine in GuideLines)
            //{
            //    guideLine.Draw(gameTime, spriteBatch);
            //}
        }

        #endregion
    }
}


