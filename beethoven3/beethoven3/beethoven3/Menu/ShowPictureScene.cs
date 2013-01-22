using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.GamerServices;


//사진 보여주는것
namespace beethoven3
{
    class ShowPictureScene
    {

        private Texture2D background;
        private bool clickNextButton;
        MouseState mouseStatePrevious;//

        private Rectangle recBackground = new Rectangle(0, 0, 1024, 769);
        private Rectangle recNextButton = new Rectangle(784, 5, 220, 170);

        //private Rectangle recPreviousButton = new Rectangle(36, 35, 220, 170);

        //private bool clickPreviousButton;

        public ShowPictureScene()
        {
            clickNextButton = false;
            //clickPreviousButton = false;
        }

        public void LoadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>(@"Textures\photoTime");

          
        }

        public virtual void Update(GameTime gameTime, Rectangle rightHandPosition)
        {
            MouseState mouseStateCurrent = Mouse.GetState();
            mouseStatePrevious = Game1.mouseStatePrevious;

            Rectangle mouseRect = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);

            //if (mouseRect.Intersects(recPreviousButton) || rightHandPosition.Intersects(recPreviousButton))
            //{

            //    Game1.nearButton = true;
            //    Game1.GetCenterOfButton(recPreviousButton);

            //    clickPreviousButton = true;
            //    //click the right hand item section
            //    if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
            //    {
            //        Game1.gameState = Game1.GameStates.ResultManager;

            //        //가운데로 고정된거 풀기 
            //        Game1.nearButton = false;
            //    }
            //}
            //else
            //{
            //    clickPreviousButton = false;
            //}



            if (mouseRect.Intersects(getRectNextButton()) || rightHandPosition.Intersects(getRectNextButton()))
            {

                Game1.nearButton = true;
                Game1.GetCenterOfButton(getRectNextButton());

                setClickNextButton(true);
                //click the right hand item section
                if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {
                    Game1.nearButton = false;
                    Game1.gameState = Game1.GameStates.RecordBoard;
                }
            }
            else
            {
                Game1.nearButton = false;

                setClickNextButton(false);
            }

            //if (
            //!(mouseRect.Intersects(getRectNextButton()) || rightHandPosition.Intersects(getRectNextButton()))

            //&& !(mouseRect.Intersects(recPreviousButton) || rightHandPosition.Intersects(recPreviousButton))
            //)
            //{
            //    Game1.nearButton = false;
            //}


        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(background, recBackground, Color.White);

       
            spriteBatch.Draw(Game1.nextButton, new Vector2(recNextButton.X, recNextButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);
            //spriteBatch.Draw(Game1.previousButton, new Vector2(recPreviousButton.X, recPreviousButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);


            if (clickNextButton)
            {
                spriteBatch.Draw(Game1.hoverNextButton, new Vector2(recNextButton.X, recNextButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);


            }

            //if (clickPreviousButton)
            //{

            //    spriteBatch.Draw(Game1.hoverPreviousButton, new Vector2(recPreviousButton.X, recPreviousButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);
            //}

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
