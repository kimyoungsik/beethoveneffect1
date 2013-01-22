using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.GamerServices;

// 기록판
namespace beethoven3
{
    class RecordBoard
    {

        private Texture2D background;
   //     private Texture2D nextButton;
        private bool clickNextButton;
        private Rectangle recBackground = new Rectangle(0, 0, 1024, 769);
        private Rectangle recNextButton = new Rectangle(784, 5, 220, 170);
        private Rectangle recPreviousButton = new Rectangle(36, 5, 220, 170);
        MouseState mouseStatePrevious;//



        private bool clickPreviousButton;

        public RecordBoard()
        {
             clickNextButton = false;
             clickPreviousButton = false;
        }

        public void LoadContent(ContentManager cm)
        {
             background = cm.Load<Texture2D>(@"recordBoard\background");
   
        }

        public virtual void Update(GameTime gameTime, Rectangle rightHandPosition)
        {
            MouseState mouseStateCurrent = Mouse.GetState();
            mouseStatePrevious = Game1.mouseStatePrevious;

            Rectangle rectMouseRecordBoard = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);


            if (rectMouseRecordBoard.Intersects(getRectNextButton()) || rightHandPosition.Intersects(getRectNextButton()))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(getRectNextButton());

                setClickNextButton(true);
                //click the right hand item section
                if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {
                    Game1.nearButton = false;

                    Game1.gameState = Game1.GameStates.SongMenu;
                }
            }
            else
            {
               

                setClickNextButton(false);
            }


            if (rectMouseRecordBoard.Intersects(recPreviousButton) || rightHandPosition.Intersects(recPreviousButton))
            {

                Game1.nearButton = true;
                Game1.GetCenterOfButton(recPreviousButton);

                clickPreviousButton = true;
                //click the right hand item section
                if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {
                    Game1.gameState = Game1.GameStates.ShowPictures;

                    //가운데로 고정된거 풀기 
                    Game1.nearButton = false;
                }
            }
            else
            {
                clickPreviousButton = false;
            }


            if (
            !(rectMouseRecordBoard.Intersects(getRectNextButton()) || rightHandPosition.Intersects(getRectNextButton()))

            && !(rectMouseRecordBoard.Intersects(recPreviousButton) || rightHandPosition.Intersects(recPreviousButton))
            )
            {
                Game1.nearButton = false;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(background, recBackground, Color.White);


            spriteBatch.Draw(Game1.nextButton, new Vector2(recNextButton.X, recNextButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);


            spriteBatch.Draw(Game1.previousButton, new Vector2(recPreviousButton.X, recPreviousButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);

            if (clickNextButton)
            {
                spriteBatch.Draw(Game1.hoverNextButton, new Vector2(recNextButton.X, recNextButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);

                
            }
            if (clickPreviousButton)
            {

                spriteBatch.Draw(Game1.hoverPreviousButton, new Vector2(recPreviousButton.X, recPreviousButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);
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
