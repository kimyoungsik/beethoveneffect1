using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;


namespace beethoven3
{
    class MenuScene
    {
        MouseState pastmouse;
        KeyboardState pastkey;


        private Texture2D menuBackground;
        private Rectangle recMenuBackground = new Rectangle(0, 0, 1024, 769);

        private Texture2D startButton;
        private Texture2D hoverStartButton;
        private Rectangle recStartButton = new Rectangle(138, 291, 202, 88);

        private Texture2D shopButton;
        private Texture2D hoverShopButton;
        private Rectangle recShopButton = new Rectangle(698, 279, 198, 110);

        private Texture2D tutorialButton;
        private Texture2D hoverTutorialButton;
        private Rectangle recTutorialButton = new Rectangle(51, 531, 290, 97);


        private Texture2D settingButton;
        private Texture2D hoverSettingButton;
        private Rectangle recSettingButton = new Rectangle(698, 524, 263, 110);

        private bool clickStartButton;
        private bool clickShopButton;
        private bool clickTutorialButton;
        private bool clickSettingButton;
        


        
        public MenuScene()
        {
           clickStartButton = false;
           clickShopButton = false;
           clickTutorialButton = false;
           clickSettingButton = false;
        
        }

        public void LoadContent(ContentManager cm)
        {
            menuBackground = cm.Load<Texture2D>(@"Menu\firstMenuBackground");
   
            startButton = cm.Load<Texture2D>(@"Menu\start");
            hoverStartButton = cm.Load<Texture2D>(@"Menu\hoverStart");

            shopButton = cm.Load<Texture2D>(@"Menu\shop");
            hoverShopButton = cm.Load<Texture2D>(@"Menu\hoverShop");

            tutorialButton = cm.Load<Texture2D>(@"Menu\tutorial");
            hoverTutorialButton = cm.Load<Texture2D>(@"Menu\hoverTutorial");

            settingButton = cm.Load<Texture2D>(@"Menu\setting");
            hoverSettingButton = cm.Load<Texture2D>(@"Menu\hoverSetting");
        }


        public Rectangle RecStartButton
        {
            get { return recStartButton; }
          
        }
        public Rectangle RecSettingButton
        {
            get { return recSettingButton; }

        }
        public Rectangle RecShopButton
        {
            get { return recShopButton; }

        }
        public Rectangle RecTutorialButton
        {
            get { return recTutorialButton; }

        }

        public bool ClickStartButton
        {
            get { return clickStartButton; }
            set { clickStartButton = value; }
        }

        
        public bool ClickShopButton
        {
            get { return clickShopButton; }
            set { clickShopButton = value; }
        }


        
        public bool ClickTutorialButton
        {
            get { return clickTutorialButton; }
            set { clickTutorialButton = value; }
        }

        
        public bool ClickSettingButton
        {
            get { return clickSettingButton; }
            set { clickSettingButton = value; }
        }
        
        

        public void Update(GameTime gameTime , Rectangle rightHandPosition)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState key = Keyboard.GetState();
            pastmouse = Game1.mouseStatePrevious;
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 5, 5);


              //시작
            if (mouseRectangle.Intersects(recStartButton) || rightHandPosition.Intersects(recStartButton))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(recStartButton);
               
                clickStartButton = true;
                //click the right hand item section

               
                if ((mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {
                  //  SoundFmod.PlaySound(Game1.mouseClickSound);
                    Game1.nearButton = false;
                    
                    Game1.gameState = Game1.GameStates.SongMenu;
                    SongMenu.opening = true;

                }
            }
            else
            {
              //  SoundFmod.PlaySound2(Game1.mouseHoverSound);
                clickStartButton = false;
            }


            //상점
            if (mouseRectangle.Intersects(recShopButton) || rightHandPosition.Intersects(recShopButton))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(recShopButton);

                clickShopButton = true;
                //click the right hand item section
                if ((mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {

                    Game1.nearButton = false;
                    Game1.gameState = Game1.GameStates.ShopDoor;
                }
            }
            else
            {
                clickShopButton = false;
            }


            //튜토리얼
            if (mouseRectangle.Intersects(recTutorialButton) || rightHandPosition.Intersects(recTutorialButton))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(recTutorialButton);

                clickTutorialButton = true;
                //click the right hand item section
                if ((mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {
                    Game1.nearButton = false;
                    Game1.gameState = Game1.GameStates.TutorialScene;
                }
            }
            else
            {
                clickTutorialButton = false;
            }


            //세팅
            if (mouseRectangle.Intersects(recSettingButton) || rightHandPosition.Intersects(recSettingButton))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(recSettingButton);

                clickSettingButton = true;
                //click the right hand item section
                if ((mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {

                    Game1.nearButton = false;
                    Game1.gameState = Game1.GameStates.SettingBoard;


                }
            }
            else
            {
                clickSettingButton = false;
            }

            if (
            !(mouseRectangle.Intersects(recSettingButton) || rightHandPosition.Intersects(recSettingButton))
            && !(mouseRectangle.Intersects(recTutorialButton) || rightHandPosition.Intersects(recTutorialButton))
            && !(mouseRectangle.Intersects(recShopButton) || rightHandPosition.Intersects(recShopButton))
            && !(mouseRectangle.Intersects(recStartButton) || rightHandPosition.Intersects(recStartButton))
                )
            {

                Game1.nearButton = false;
            }

            pastmouse = mouse;
            pastkey = key;
         
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(menuBackground, recMenuBackground, Color.White);
            
        //    spriteBatch.Draw(title,recMainPicture , Color.White);

        //    spriteBatch.Draw(mainPicture, recTitle, Color.White);


            spriteBatch.Draw(startButton, recStartButton, Color.White);


            spriteBatch.Draw(shopButton, recShopButton, Color.White);


            spriteBatch.Draw(tutorialButton, recTutorialButton, Color.White);


            spriteBatch.Draw(settingButton, recSettingButton, Color.White);


            if(clickStartButton)
            {
                spriteBatch.Draw(hoverStartButton, recStartButton, Color.White);
             //   spriteBatch.DrawString(Game1.georgia, "START",new Vector2(recStartButton.X,recStartButton.Y-30), Color.Yellow);
            }

            if(clickShopButton)
            {

                spriteBatch.Draw(hoverShopButton, recShopButton, Color.White);

                
            }

            if (clickTutorialButton)
            {
                spriteBatch.Draw(hoverTutorialButton, recTutorialButton, Color.White);
            }

            if (clickSettingButton)
            {
                spriteBatch.Draw(hoverSettingButton, recSettingButton, Color.White);
            }


        }

        //public void setButton1()
        //{
        //    this.button1 = true;
        //}
    }
}
