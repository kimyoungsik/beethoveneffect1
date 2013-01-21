using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;


//사진 보여주는것
namespace beethoven3
{
    class ShowPictureScene
    {

        private Texture2D background;
        private bool clickNextButton;


        private Rectangle recBackground = new Rectangle(0, 0, 1024, 769);
        private Rectangle recNextButton = new Rectangle(784, 5, 220, 170);

      

        public ShowPictureScene()
        {
            clickNextButton = false;
        }

        public void LoadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>(@"Textures\photoTime");

          
        }


        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(background, recBackground, Color.White);

       
            spriteBatch.Draw(Game1.nextButton, new Vector2(recNextButton.X, recNextButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);


            if (clickNextButton)
            {
                spriteBatch.Draw(Game1.hoverNextButton, new Vector2(recNextButton.X, recNextButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);


            }


        }

        public void setClickNextButton(bool value)
        {
            this.clickNextButton = value;
        }

        public Rectangle getRectNextButton()
        {
            return this.recNextButton;
        }
    }
}
