using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace beethoven3
{
    class Item
    {
        #region declarations
        public Sprite ItemSprite;
        // private float speed = 120f;

        //변환시 사라짐
        
        //충돌 판정시에 
        private int itemRadius = 15;

        private Vector2 location;
        private bool sold;

        
        #endregion

        #region constructor
        public Item(
            Texture2D texture,
            Vector2 location,
            Rectangle initialFrame,
            int frameCount)
        {
            ItemSprite = new Sprite(
                location,
                texture,
                initialFrame,
                Vector2.Zero);

            for (int x = 1; x < frameCount; x++)
            {
                ItemSprite.AddFrame(
                    new Rectangle(
                        initialFrame.X = (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            this.location = location;
            this.sold = false;
            ItemSprite.CollisionRadius = itemRadius;
        }
        #endregion

        public void buyItem()
        {
            this.sold = true;
        }


        #region update and draw
        public void Update(GameTime gameTime)
        {
            ItemSprite.Update(gameTime);


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            ItemSprite.Draw(spriteBatch);

        }

        #endregion

    }
}
