using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class Number
    {

        #region declarations
        public ExplosionSprite NumberSprite;
        //   private float speed = 120f;

        //변환시 사라짐
        public bool Disappear = false;

        private int numberRadius = 15;

        private Vector2 location;
        #endregion

        #region constructor
        public Number(
            Texture2D texture,
            Vector2 location,
            Rectangle initialFrame,
            int duration,
            int frameCount,

            float scale
            )
        {
            NumberSprite = new ExplosionSprite(
                location,
                texture,
                initialFrame,
                Vector2.Zero,
                duration,
                scale);

            for (int x = 1; x < frameCount; x++)
            {
                NumberSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X = (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            this.location = location;
            NumberSprite.CollisionRadius = numberRadius;
        }
        #endregion


        #region update and draw
        public void Update(GameTime gameTime)
        {
            NumberSprite.Update(gameTime);


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            NumberSprite.Draw(spriteBatch);

        }

        #endregion

    }
}
