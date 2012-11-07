using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    static class CurveManager
    {
        #region declarations
        public static List<Curve> Curves = new List<Curve>();

        #endregion


        #region method
        public static void addCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, double time)
        
        {
            Curve curve = new Curve(p0, p1, p2, p3, time);
            Curves.Add(curve);    
        }
        #endregion


        #region update and draw


        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Curve curve in Curves)
            {
                curve.Draw(gameTime,  spriteBatch);
            }
        }

        #endregion
    }
}


