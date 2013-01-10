//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace beethoven3
//{
//    class Numbers
//    {
//        #region declarations

//        public StaticSprite NumberSprite;
//        // private float speed = 120f;

//        //충돌 판정시에 
//        private int markRadius = 15;
//        private Vector2 location;
//        #endregion

//        #region constructor

//        public Numbers(
//            Texture2D texture,
//            Vector2 location,
//            Rectangle initialFrame,
//            int frameCount,
//            float scale)
//        {
//            NumberSprite = new StaticSprite(
//                location,
//                texture,
//                initialFrame,
//                Vector2.Zero,
//                scale);

//            for (int x = 1; x < frameCount; x++)
//            {
//                NumberSprite.AddFrame(
//                    new Rectangle(
//                        initialFrame.X = (initialFrame.Width * x),
//                        initialFrame.Y,
//                        initialFrame.Width,
//                        initialFrame.Height));
//            }
//            this.location = location;
//            NumberSprite.CollisionRadius = initialFrame.Width / 2;
//        }
//        #endregion

//        #region update and draw
//        public void Update(GameTime gameTime)
//        {
//            NumberSprite.Update(gameTime);
//        }

//        public void Draw(SpriteBatch spriteBatch)
//        {
//            NumberSprite.Draw(spriteBatch);
//        }
//        #endregion
//    }
//}
