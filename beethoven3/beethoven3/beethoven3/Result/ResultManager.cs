using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;


namespace beethoven3
{
    class ResultManager
    {

        private Texture2D background;
        private Texture2D nextButton;
        private Texture2D hoverNextButton;
        private Texture2D rankA;
        private Texture2D newRecord;

        private Rectangle recBackground;
        private Rectangle recNextButton;
        private Rectangle recRankA;
        private Rectangle recNewRecord;


        private bool clickNextButton;

        public ResultManager()
        {
             clickNextButton = false;
        }

        public void LoadContent(ContentManager cm)
        {
             background = cm.Load<Texture2D>(@"result\background");
             nextButton = cm.Load<Texture2D>(@"result\nextButton");
             newRecord = cm.Load<Texture2D>(@"result\newRecord");
             rankA = cm.Load<Texture2D>(@"result\rankA");
             hoverNextButton = cm.Load<Texture2D>(@"result\hoverNextButton");
        }


        public void Draw(SpriteBatch spriteBatch,int width,int height,bool isNewRecord)
        {
            recBackground = new Rectangle(0,0,width,height);
            spriteBatch.Draw(background, recBackground, Color.White);

            recNextButton = new Rectangle(width - 400, height - 200, 356, 215);
            spriteBatch.Draw(nextButton, recNextButton, Color.White);

            recRankA = new Rectangle(100, height - 320, 169, 185);
            spriteBatch.Draw(rankA, recRankA, Color.White);


            if (isNewRecord)
            {
                recNewRecord = new Rectangle(50, height - 150, 445, 162);
                spriteBatch.Draw(newRecord, recNewRecord, Color.White);
            }


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
