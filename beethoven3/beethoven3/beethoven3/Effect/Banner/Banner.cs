using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class Banner
    {

        #region declarations
        public ExplosionSprite BannerSprite;
        //   private float speed = 120f;

        //변환시 사라짐
        public bool Disappear = false;

        private int bannerRadius = 15;

        private Vector2 location;
        #endregion

        #region constructor
        public Banner(
            Texture2D texture,
            Vector2 location,
            Rectangle initialFrame,
            int duration,
            int frameCount,

            float scale
            )
        {
            BannerSprite = new ExplosionSprite(
                location,
                texture,
                initialFrame,
                Vector2.Zero,
                duration,
                scale);

            for (int x = 1; x < frameCount; x++)
            {
                BannerSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X = (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            this.location = location;
            BannerSprite.CollisionRadius = bannerRadius;
        }
        #endregion


        #region update and draw
        public void Update(GameTime gameTime)
        {
            BannerSprite.Update(gameTime);


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            BannerSprite.Draw(spriteBatch);

        }

        #endregion

    }
}
