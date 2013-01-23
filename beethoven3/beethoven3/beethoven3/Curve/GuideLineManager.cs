using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class GuideLineManager
    {
        #region declarations
        public List<GuideLine> GuideLines = new List<GuideLine>();
        private LineRenderer lineRenderer;
        #endregion


        public GuideLineManager(LineRenderer lineRenderer)
        {
            this.lineRenderer = lineRenderer;

        }


        #region method
        /// <summary>
        /// 커브곡선 추가
        /// </summary>
        /// <param name="p0">시작점</param>
        /// <param name="p1">제어점1</param>
        /// <param name="p2">제어점1</param>
        /// <param name="p3">끝나는점</param>
        /// <param name="time">지속시간</param>
        public void AddGuideLine(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time, bool showGold)
        {
            GuideLine guideLine = new GuideLine(p0, p1, p2, p3, time, showGold, lineRenderer);
            
            GuideLines.Add(guideLine);
        }

        //갑자기 템포가 변했을떄 현재 그려진것드을 지워준다. (지속기간이 다르기 때무에 오래 남게된다)
        public void DeleteAllSecondGuideLine()
        { 
            int i;
            for(i=0; i<GuideLines.Count; i++)
            {
                if (!GuideLines[i].ShowGold)
                {
                    GuideLines.RemoveAt(i);

                }
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
            foreach (GuideLine guideLine in GuideLines)
            {
                guideLine.Draw(gameTime, spriteBatch);
            }   
        }

        #endregion
    }
}


