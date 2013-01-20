using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
namespace beethoven3
{
    class ShopDoor
    {
        MouseState pastmouse;
        KeyboardState pastkey;

        private Texture2D shopDoorBackground;
        private Rectangle recMenuBackground = new Rectangle(0, 0, 1024, 769);

        
        private Texture2D mainPicture;
        private Rectangle recMainPicture = new Rectangle(280, 200, 568, 413);



        private Texture2D rightHand;
        private Texture2D leftHand;
        private Texture2D note;
        private Texture2D effect;
        private Texture2D background;
      //  private Texture2D previousButton;

        private Texture2D hoverRightHand;
        private Texture2D hoverLeftHand;
        private Texture2D hoverNote;
        private Texture2D hoverEffect;
        private Texture2D hoverBackground;
     //   private Texture2D hoverPreviousButton;

        private Rectangle recRightHand = new Rectangle(750,360,191,58);
        private Rectangle recLeftHand = new Rectangle(50, 360, 287, 72);
        private Rectangle recNote = new Rectangle(274, 166, 147, 50);
        private Rectangle recEffect = new Rectangle(615, 166, 179, 72);
        private Rectangle recBackground = new Rectangle(326, 584, 395, 72);
    //    private Rectangle recPreviousButton;


        private bool clickRightHand;
        private bool clickLeftHand;
        private bool clickNote;
        private bool clickEffect;
        private bool clickBackground;
        private bool clickPreviousButton;



        private SpriteFont pericles36Font;

        public ShopDoor()
        {
             clickRightHand=false;
             clickLeftHand = false;
             clickNote = false;
             clickEffect = false;
             clickBackground = false;
             clickPreviousButton = false;

        }

        public void LoadContent(ContentManager cm)
        {
            shopDoorBackground = cm.Load<Texture2D>(@"shopDoorItems\background");


            mainPicture = cm.Load<Texture2D>(@"shopDoorItems\mainPic");


             rightHand= cm.Load<Texture2D>(@"shopDoorItems\rightBaton");
             leftHand = cm.Load<Texture2D>(@"shopDoorItems\leftHand");
             note = cm.Load<Texture2D>(@"shopDoorItems\note");
             effect = cm.Load<Texture2D>(@"shopDoorItems\effect");
             background = cm.Load<Texture2D>(@"shopDoorItems\background");



          //   previousButton = cm.Load<Texture2D>(@"result\nextButton");

             hoverRightHand = cm.Load<Texture2D>(@"shopDoorItems\rightBaton");
             hoverLeftHand = cm.Load<Texture2D>(@"shopDoorItems\leftHand");
             hoverNote = cm.Load<Texture2D>(@"shopDoorItems\note");
             hoverEffect = cm.Load<Texture2D>(@"shopDoorItems\effect");
             hoverBackground = cm.Load<Texture2D>(@"shopDoorItems\background");



            // hoverPreviousButton = cm.Load<Texture2D>(@"result\\hoverNextButton");

          //   pericles36Font = cm.Load<SpriteFont>(@"Fonts\Pericles36");
           
        }

        public void Update(GameTime gameTime, Rectangle rightHandPosition)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState key = Keyboard.GetState();

            Rectangle rect = new Rectangle(mouse.X, mouse.Y, 5, 5);


            //mousecursor on right hand item section
            //오른쪽지휘봉 아이템
            if (rect.Intersects(recRightHand) || rightHandPosition.Intersects(recRightHand))
            {
                //hover on
                setClickRightHand(true);
                Game1.nearButton = true;
                Game1.GetCenterOfButton(getRectRightHand());
                //click the right hand item section
                if ((mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || Game1.finalClick && !Game1.pastClick)
                {
                    Game1.gameState = Game1.GameStates.RightItemShop;
                    //가운데로 고정된거 풀기 
                    Game1.nearButton = false;
                }
            }
            else
            {
                //hover off
                //    nearButton = false;
                setClickRightHand(false);
            }


            //mouse cursor on left hadn item section
            if (rect.Intersects(getRectLeftHand()) || rightHandPosition.Intersects(getRectLeftHand()))
            {
                setClickLeftHand(true);
                Game1.nearButton = true;
                Game1.GetCenterOfButton(getRectLeftHand());
                if (mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released || Game1.finalClick && !Game1.pastClick)
                {
                    Game1.gameState = Game1.GameStates.LeftItemShop;
                    //가운데로 고정된거 풀기 
                    Game1.nearButton = false;
                }
            }
            else
            {
                //    nearButton = false;
                setClickLeftHand(false);
            }


            //note
            if (rect.Intersects(getRectNote()) || rightHandPosition.Intersects(getRectNote()))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(getRectNote());

                setClickNote(true);
                if (mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released || Game1.finalClick && !Game1.pastClick)
                {
                    Game1.gameState = Game1.GameStates.NoteItemShop;
                    //가운데로 고정된거 풀기 
                    Game1.nearButton = false;
                }

            }
            else
            {
                setClickNote(false);
            }

            if (rect.Intersects(getRectEffect()) || rightHandPosition.Intersects(getRectEffect()))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(getRectEffect());

                setClickEffect(true);
                if (mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released || Game1.finalClick && !Game1.pastClick)
                {
                    Game1.gameState = Game1.GameStates.EffectItemShop;
                    //가운데로 고정된거 풀기 
                    Game1.nearButton = false;
                }

            }
            else
            {
                setClickEffect(false);
            }

            if (rect.Intersects(getRectBackground()) || rightHandPosition.Intersects(getRectBackground()))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(getRectBackground());

                setClickBackground(true);
                if (mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released || Game1.finalClick && !Game1.pastClick)
                {
                    Game1.gameState = Game1.GameStates.BackgroundItemShop;
                    //가운데로 고정된거 풀기 
                    Game1.nearButton = false;
                }

            }
            else
            {
                setClickBackground(false);
            }

            //if (rect.Intersects(getRectPreviousButton()) || rightHandPosition.Intersects(getRectPreviousButton()))
            //{

            //    Game1.nearButton = true;
            //    Game1.GetCenterOfButton(getRectPreviousButton());

            //    setClickPreviousButton(true);
            //    //click the right hand item section
            //    if ((mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
            //    {
            //        Game1.gameState = Game1.GameStates.Menu;

            //        //가운데로 고정된거 풀기 
            //        Game1.nearButton = false;
            //    }
            //}
            //else
            //{
            //    setClickPreviousButton(false);
            //}


            if (
            !(rect.Intersects(getRectRightHand()) || rightHandPosition.Intersects(getRectRightHand()))
            && !(rect.Intersects(getRectLeftHand()) || rightHandPosition.Intersects(getRectLeftHand()))
            && !(rect.Intersects(getRectNote()) || rightHandPosition.Intersects(getRectNote()))
            && !(rect.Intersects(getRectEffect()) || rightHandPosition.Intersects(getRectEffect()))
            && !(rect.Intersects(getRectBackground()) || rightHandPosition.Intersects(getRectBackground()))
                /*   && !(rect.Intersects(getRectPreviousButton()) || rightHandPosition.Intersects(getRectPreviousButton()) )*/
            )
            {
                Game1.nearButton = false;
            }

            pastmouse = mouse;
            pastkey = key;
         
            
        }

        public void Draw(SpriteBatch spriteBatch,int width,int height)
        {

            spriteBatch.Draw(shopDoorBackground, recMenuBackground, Color.White);

       
            spriteBatch.Draw(mainPicture, recMainPicture, Color.White);


            spriteBatch.Draw(rightHand, recRightHand, Color.White);
               
            spriteBatch.Draw(leftHand, recLeftHand, Color.White);

            spriteBatch.Draw(note, recNote, Color.White);

            spriteBatch.Draw(effect,
                 recEffect,
                     Color.White);
            spriteBatch.Draw(background,
                 recBackground,
                     Color.White);

            // recPreviousButton = new Rectangle(width - 400, height - 200, 356, 215);
            //spriteBatch.Draw(previousButton, recPreviousButton, Color.White);




            if (clickPreviousButton)
            {

                //버튼이 주변에 있다고 감지
               // Game1.nearButton = true;
              //  Game1.GetCenterOfButton(recPreviousButton);
                //spriteBatch.Draw(hoverPreviousButton, recPreviousButton, Color.White);
            }
            else
            {
                //다른곳으로 이동
               // Game1.nearButton = false;
            }


            if (clickRightHand)
            {

                Rectangle rectangleRightHand = new Rectangle(width / 2 - (hoverRightHand.Width / 2) - 100, height / 2 - (hoverRightHand.Height / 2) - 100, 208, 233);

                //Game1.nearButton = true;
                //Game1.GetCenterOfButton(rectangleRightHand);

                spriteBatch.Draw(hoverRightHand, rectangleRightHand, Color.White);
            }
            //else
            //{
            //      Game1.nearButton = false;
            
            //}

            if (clickLeftHand)
            {

                Rectangle rectangleLeftHand = new Rectangle(width / 2 - (hoverLeftHand.Width / 2) + 100, height / 2 - (hoverLeftHand.Height / 2) - 150, 273, 172);

                //Game1.nearButton = true;
                //Game1.GetCenterOfButton(rectangleLeftHand);

                spriteBatch.Draw(hoverLeftHand, rectangleLeftHand, Color.White);


            }
            //else
            //{
            //    Game1.nearButton = false;

            //}



            if (clickNote)
            {
                spriteBatch.Draw(hoverNote,
                     new Rectangle(width / 2 - (hoverNote.Width / 2) + 220, height / 2 - (hoverNote.Height / 2) + 25, 172,
                         344),
                         Color.White);
            }
            if (clickEffect)
            {
                spriteBatch.Draw(hoverEffect,
                     new Rectangle(width / 2 - (hoverEffect.Width / 2) + 90, height / 2 - (hoverEffect.Height / 2) + 185, 237,
                         161),
                         Color.White);
            }
            if (clickBackground)
            {
                spriteBatch.Draw(hoverBackground,
                 new Rectangle(width / 2 - (hoverBackground.Width / 2) - 100, height / 2 - (hoverBackground.Height / 2) + 140, 213,
                     229),
                     Color.White);
            }

            //if (button1)
            //{
            //    spriteBatch.Draw(heart,
            //    new Rectangle(0, 0, 100,s
            //        100),
            //        Color.White);
            //}

        }

        //public void setButton1()
        //{
        //    this.button1 = true;
        //}

        public void setClickLeftHand(bool value)
        {
            this.clickLeftHand = value;
        }

        public void setClickNote(bool value)
        {
            this.clickNote = value;
        }

        public void setClickEffect(bool value)
        {
            this.clickEffect = value;
        }

        public void setClickBackground(bool value)
        {
            this.clickBackground = value;
        }

        public void setClickRightHand(bool value)
        {
            this.clickRightHand = value;
        }


        public Rectangle getRectRightHand()
        {
            return this.recRightHand;
        }

        public Rectangle getRectLeftHand()
        {
            return this.recLeftHand;
        }

        public Rectangle getRectNote()
        {
            return this.recNote;
        }

        public Rectangle getRectEffect()
        {
            return this.recEffect;
        }

        public Rectangle getRectBackground()
        {
            return this.recBackground;
        }


          public void setClickPreviousButton(bool value)
        {
            this.clickPreviousButton = value;
        }

        //public Rectangle getRectPreviousButton()
        //{
        //    return this.recPreviousButton;
        //}
       
    }
}
