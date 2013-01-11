using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class ResultNumber
    {

        #region declarations
        public Sprite ResultNumberSprite;
        
        //변환시 사라짐
        public bool Disappear = false;

        private int numberRadius = 15;

        private Vector2 location;
        #endregion

        #region constructor
        public ResultNumber(
            Texture2D texture,
            Vector2 location,
            Rectangle initialFrame,
            int frameCount,
            float scale
            )
        {
            ResultNumberSprite = new Sprite(
                location,
                texture,
                initialFrame,
                Vector2.Zero,
                scale);

            for (int x = 1; x < frameCount; x++)
            {
                ResultNumberSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X = (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            this.location = location;
            ResultNumberSprite.CollisionRadius = numberRadius;
        }
        #endregion


        #region update and draw
        public void Update(GameTime gameTime)
        {
            ResultNumberSprite.Update(gameTime);


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            ResultNumberSprite.Draw(spriteBatch);

        }

        #endregion

    }
}
