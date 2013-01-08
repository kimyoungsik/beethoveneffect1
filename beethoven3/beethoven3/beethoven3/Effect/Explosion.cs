using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class Explosion
    {
        #region declarations
        public ExplosionSprite ExplosionSprite;
     //   private float speed = 120f;

        //변환시 사라짐
        public bool Disappear = false;

        private int explosionRadius = 15;

        private Vector2 location;
        #endregion

        #region constructor
        public Explosion(
            Texture2D texture,
            Vector2 location,
            Rectangle initialFrame,
            int duration,
            int frameCount,
            
            float scale
            )
        {
            ExplosionSprite = new ExplosionSprite(
                location,
                texture,
                initialFrame,
                Vector2.Zero,
                duration,
                scale);

            for (int x = 1; x < frameCount; x++)
            {
                ExplosionSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X = (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            this.location = location;
            ExplosionSprite.CollisionRadius = explosionRadius;
        }
        #endregion


        #region update and draw
        public void Update(GameTime gameTime)
        {
            ExplosionSprite.Update(gameTime);


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            ExplosionSprite.Draw(spriteBatch);

        }

        #endregion

    }
}
