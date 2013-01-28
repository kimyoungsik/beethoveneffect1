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

        private Texture2D rankA;
        private Texture2D rankB;
        private Texture2D rankC;
        private Texture2D rankD;
        private Texture2D rankE;
        private Texture2D rankF;

        private Texture2D newRecord;

        private Rectangle recZero= new Rectangle(0,0,1024,769);
        private Rectangle recNextButton =  new Rectangle(784,5 , 220, 170);



        private Rectangle recNewRecord;
        
         private ScoreManager scoreManager;

      
        private bool clickNextButton;

        public ResultManager(ScoreManager scoreManager)
        {
             clickNextButton = false;
            this.scoreManager = scoreManager;
        }

        public void LoadContent(ContentManager cm)
        {
             background = cm.Load<Texture2D>(@"result\background");
             newRecord = cm.Load<Texture2D>(@"result\newRecord");
             rankA = cm.Load<Texture2D>(@"result\rankA");
             rankB = cm.Load<Texture2D>(@"result\rankB");
             rankC = cm.Load<Texture2D>(@"result\rankC");
             rankD = cm.Load<Texture2D>(@"result\rankD");
             rankE = cm.Load<Texture2D>(@"result\rankE");
             rankF = cm.Load<Texture2D>(@"result\rankF");
         }


        public void Draw(SpriteBatch spriteBatch,bool isNewRecord)
        {
            spriteBatch.Draw(background, recZero, Color.White);

            spriteBatch.Draw(Game1.nextButton, new Vector2(recNextButton.X, recNextButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);

          

           
            if (isNewRecord)
            {

                spriteBatch.Draw(newRecord, recZero, Color.White);
            }


            if (clickNextButton)
            {
                spriteBatch.Draw(Game1.hoverNextButton, new Vector2(recNextButton.X, recNextButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);

              
            }


            int totalPerfect = scoreManager.OneHandPerfect + (int)(scoreManager.LongPerfect*0.1)/*롱노트는 한개당 0.1개*/ + (int)(scoreManager.DragPerfect*0.5)/* 드래그노트는 한개당 0.5개*/ + scoreManager.PosturePerfect + scoreManager.JesturePerfect;
            int totalGood = scoreManager.OneHandGood + (int)(scoreManager.DragGood*0.2)/*드래그노트는 한개당 0.2개*/ + scoreManager.JestureGood;
            int totalMiss = scoreManager.OneHandMiss + scoreManager.LongMiss + scoreManager.DragMiss + scoreManager.PostureMiss + scoreManager.JestureMiss;


            double rankRate = ((totalPerfect + totalGood) * 1.0f / (totalPerfect + totalGood + totalMiss + scoreManager.OneHandBad));
           
            if(rankRate >= 0.9)
            {
                scoreManager.Rank = "A";
                spriteBatch.Draw(rankA, recZero, Color.White);
            }
            else if (rankRate >= 0.8)
            {
                scoreManager.Rank = "B";
                spriteBatch.Draw(rankB, recZero, Color.White);
            }
            else if (rankRate >=0.7)
            {
                scoreManager.Rank = "C";
                spriteBatch.Draw(rankC, recZero, Color.White);
            }
            else if (rankRate >= 0.6)
            {
                scoreManager.Rank = "D";
                spriteBatch.Draw(rankD, recZero, Color.White);
            }
            else if (rankRate >= 0.5)
            {
                scoreManager.Rank = "E";
                spriteBatch.Draw(rankE, recZero, Color.White);
            }
            else 
            {
                scoreManager.Rank = "F";
                spriteBatch.Draw(rankF, recZero, Color.White);
            }
             //

           
            //int intMax = (int)scoreManager.Max;

            spriteBatch.DrawString(Game1.georgia, totalPerfect.ToString(), new Vector2(330, 370), Color.Gray);
            spriteBatch.DrawString(Game1.georgia, totalGood.ToString(), new Vector2(330, 430), Color.Gray);
            spriteBatch.DrawString(Game1.georgia, scoreManager.OneHandBad.ToString(), new Vector2(330, 490), Color.Gray);
            spriteBatch.DrawString(Game1.georgia, totalMiss.ToString(), new Vector2(330, 550), Color.Gray);

            spriteBatch.DrawString(Game1.georgia, scoreManager.Perfomance.ToString(), new Vector2(800, 370), Color.Gray);
            spriteBatch.DrawString(Game1.georgia, scoreManager.Max.ToString("#####"), new Vector2(800, 430), Color.Gray);



            spriteBatch.DrawString(Game1.georgia, scoreManager.Gold.ToString(), new Vector2(780, 570), Color.Gray);
            
            
            spriteBatch.DrawString(Game1.georgia, scoreManager.TotalScore.ToString(), new Vector2(800, 640), Color.Gray);


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
