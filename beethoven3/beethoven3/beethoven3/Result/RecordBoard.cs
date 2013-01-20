using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;

// 기록판
namespace beethoven3
{
    class RecordBoard
    {

        private Texture2D background;
        private Texture2D nextButton;
        private bool clickNextButton;
        private Rectangle recBackground = new Rectangle(0, 0, 1024, 769);
        private Rectangle recNextButton = new Rectangle(784, 5, 220, 170);

        public RecordBoard()
        {
             clickNextButton = false;
        }

        public void LoadContent(ContentManager cm)
        {
             background = cm.Load<Texture2D>(@"recordBoard\background");
   
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
