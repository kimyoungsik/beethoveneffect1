using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class Mark
    {
        #region declarations
        
        public Sprite MarkSprite;
        // private float speed = 120f;
        
        //변환시 사라짐
        public bool Disappear = false;

        //충돌 판정시에 
        private int markRadius = 15;
        private Vector2 location;
        #endregion

        #region constructor

        public Mark(
            Texture2D texture,
            Vector2 location,
            Rectangle initialFrame,
            int frameCount,
            float scale)
        {
            MarkSprite = new Sprite(
                location,
                texture,
                initialFrame,
                Vector2.Zero,
                scale);

            for (int x = 1; x < frameCount; x++)
            {
                MarkSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X = (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            this.location = location;
            MarkSprite.CollisionRadius = initialFrame.Width/2;
        }
        #endregion
        
        #region update and draw
        public void Update(GameTime gameTime)
        {
            MarkSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MarkSprite.Draw(spriteBatch);
        }
        #endregion 
    }
}
