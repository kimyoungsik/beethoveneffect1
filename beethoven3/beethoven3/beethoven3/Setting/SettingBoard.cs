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
    class SettingBoard
    {

        private Texture2D background;
        private Texture2D nextButton;
        private Texture2D hoverNextButton;
        private bool clickNextButton;
        private Rectangle recBackground;
        private Rectangle recNextButton;

        public SettingBoard()
        {
            clickNextButton = false;
        }

        public void LoadContent(ContentManager cm)
        {
            //   background = cm.Load<Texture2D>(@"result\background");
            nextButton = cm.Load<Texture2D>(@"result\nextButton");
            hoverNextButton = cm.Load<Texture2D>(@"result\hoverNextButton");
        }


        public void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            //   recBackground = new Rectangle(0, 0, width, height);
            //    spriteBatch.Draw(background, recBackground, Color.White);

            recNextButton = new Rectangle(width - 400, height - 200, 356, 215);
            spriteBatch.Draw(nextButton, recNextButton, Color.White);


            if (clickNextButton)
            {
                spriteBatch.Draw(hoverNextButton, recNextButton, Color.White);
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
