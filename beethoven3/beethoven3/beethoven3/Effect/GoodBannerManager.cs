using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class GoodBannerManager
    {

        #region declarations

        private Texture2D texture;
        private Rectangle initialFrame;
        private float scale;
        private int frameCount;
        private int duration;
        //public => private // 삭제 영역에 들어가면 삭제 되는 곳에서 발견 // 내 생각엔 특별히 public으로 할 이유가 없다. 
        private List<Banner> Banners = new List<Banner>();

        #endregion


        #region constructor

        public void BannerInit(Texture2D texture, Rectangle initialFrame, int frameCount, float scale, int duration)
        {
            this.texture = texture;
            this.initialFrame = initialFrame;
            this.frameCount = frameCount;
            this.scale = scale;
            this.duration = duration;

        }
        public void AddBanners(Vector2 location, float scale)
        {
            Banner thisBanner = new Banner(
                texture,
                location,
                initialFrame,
                duration,
                frameCount,
                scale);
            Banners.Add(thisBanner);
        }
        public void deleteAllMarks()
        {

            for (int i = 0; i < Banners.Count; i++)
            {
                if (Banners[i].BannerSprite.remainingDuration < 0)
                {
                    Banners.RemoveAt(i);
                }
            }
        }

        public void DisappearAllMarks()
        {
            int i;


            for (i = Banners.Count - 1; i > -1; i--)
            {
                Banners[i].BannerSprite.remainingDuration = -1;
            }
        }


        #endregion

        #region properties
        public Rectangle InitialFrame
        {
            get { return initialFrame; }
            set { initialFrame = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public int FrameCount
        {
            get { return frameCount; }
            set { frameCount = value; }
        }

        #endregion

        #region method



        #endregion

        #region update and draw
        public void Update(GameTime gameTime)
        {

            int i;
            for (i = 0; i < Banners.Count; i++)
            {
                Banners[i].Update(gameTime);

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            int i;
            for (i = 0; i < Banners.Count; i++)
            {
                Banners[i].Draw(spriteBatch);

            }

        }


        #endregion

    }

}
