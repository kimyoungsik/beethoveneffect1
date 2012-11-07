using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    static class LineRenderer
    {
        #region declarations
        static Color m_LIneColor = Color.White;
        static Texture2D m_LineTexture = null;
        #endregion 

        #region methods
        // 라인을 그리기 위해 1x1 짜리 하얀색 pixel 텍스쳐를 만듬.
        static private void CreateLineTexture(GraphicsDevice device)
        {
            m_LineTexture = new Texture2D(device, 1, 1, true, SurfaceFormat.Color);
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            m_LineTexture.SetData<Color>(pixels);
        }

        static public Color LineColor
        {
            set { m_LIneColor = value; }
        }
        

        
        static public void DrawLine(GraphicsDevice device, SpriteBatch spriteBatch, Vector2 vStart, Vector2 vEnd, Color color)
        {
            m_LIneColor = color;
            Draw(device, spriteBatch, vStart, vEnd);
        }
        #endregion

        #region draw
        // 선을 그리기 위해, 두 점간의 거리와 각도를 구한다.
        // 그리고 SpriteBatch의 angle과 scale 특징을 이용해서 늘리고 회전한다.
        
        static public void Draw(GraphicsDevice device, SpriteBatch spriteBatch, Vector2 vStart, Vector2 vEnd)
        {
            if (m_LineTexture == null)
                CreateLineTexture(device);

            float distance = Vector2.Distance(vStart, vEnd);
            float angle = (float)Math.Atan2((double)(vEnd.Y - vStart.Y), (double)(vEnd.X - vStart.X));

            spriteBatch.Draw(m_LineTexture, vStart, null, m_LIneColor, angle, Vector2.Zero, new Vector2(distance, 1), SpriteEffects.None, 1.0f);
        }
        #endregion
    }
}
