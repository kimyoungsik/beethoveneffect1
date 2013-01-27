using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Kinect;
using Microsoft.Xna.Framework.Input;
using System.IO;
// 기록판
namespace beethoven3
{
    class SettingBoard
    {

        MouseState mouseStatePrevious;
        private Texture2D background;

        //ok 버튼
        private Texture2D nextButton;
        private Texture2D hoverNextButton;
        
        private Texture2D UpButton;
        private Texture2D hoverUpButton;
      
        private Texture2D DownButton;
        private Texture2D hoverDownButton;

        private Texture2D check;

       
        private Rectangle recScaleUpButton;
        private Rectangle recScaleDownButton;
        private Rectangle recAngleUpButton;
        private Rectangle recAngleDownButton;

        private bool clickNextButton;
        private bool clickScaleUpButton;
        private bool clickScaleDownButton;
        private bool clickAngleUpButton;
        private bool clickAngleDownButton;
        private bool clickPreviousButton;

        private Rectangle recPreviousButton = new Rectangle(36, 35, 220, 170);

        private Rectangle recBackground = new Rectangle(0, 0, 1024, 769);


        private Rectangle recCheck = new Rectangle(655, 484, 113, 132);
        private bool checkFaceDetact= true;


        private Rectangle recNextButton;


        private Game1 game1;
        public SettingBoard(Game1 game1)
        {
            clickNextButton = false;
            clickScaleUpButton = false;
            clickScaleDownButton = false;
            clickAngleUpButton = false;
            clickAngleDownButton = false;
            clickPreviousButton = false;
            this.game1 = game1;
            //페이셜디텍트 체크 
             

        }

        public void LoadContent(ContentManager cm)
        {
            background = cm.Load<Texture2D>(@"settingBoard\setBackground");
            nextButton = cm.Load<Texture2D>(@"settingBoard\ok");
            hoverNextButton = cm.Load<Texture2D>(@"settingBoard\okHover");
            
            UpButton = cm.Load<Texture2D>(@"settingBoard\up");
            hoverUpButton = cm.Load<Texture2D>(@"settingBoard\upHover");
            
            DownButton = cm.Load<Texture2D>(@"settingBoard\down");
            hoverDownButton = cm.Load<Texture2D>(@"settingBoard\downHover");

            check = cm.Load<Texture2D>(@"settingBoard\check");
                       
        }


        public void SaveCheckFile()
        {
            String dir = System.Environment.CurrentDirectory + "\\beethovenRecord\\checkKinect.txt";

            if (!System.IO.File.Exists(dir))
            {
                var myFile = System.IO.File.Create(dir);
                myFile.Close();
            }


            TextWriter tw = new StreamWriter(dir);
            tw.WriteLine("auto");
            if (checkFaceDetact)
            {
                tw.WriteLine("1");
            }
            else
            {
                tw.WriteLine("0");
            }
            //각도
            tw.WriteLine("angle");
            tw.WriteLine(game1.nui.ElevationAngle);

            //스켈링
            tw.WriteLine("scale");
            tw.WriteLine(game1.userParam);


           



            tw.WriteLine("!!");
                tw.Close();
        }


        public void LoadCheckFile()
        {
            String dir = System.Environment.CurrentDirectory + "\\beethovenRecord\\checkKinect.txt";

            if (!System.IO.File.Exists(dir))
            {
                var myFile = System.IO.File.Create(dir);
                myFile.Close();
            }

            StreamReader sr = new StreamReader(dir);
            String line;
            line = sr.ReadLine();

            //세이브도 하나도 안되어있는 상태에서 처음 열었을때
            if (line != null)
            {
                while (line != "!!")//처음
                {
                    if (line == "auto")
                    {
                        line = sr.ReadLine();
                        if (line == "1")
                        {
                            checkFaceDetact = true;
                        }
                        else if (line == "0")
                        {
                            checkFaceDetact = false;
                        }
                    }

                    else if (line == "angle")
                    {

                        line = sr.ReadLine();
                        if (!checkFaceDetact)
                        {
                            game1.nui.ElevationAngle = Convert.ToInt32(line);

                        }


                    }


                    else if( line == "scale")
                    {
                        line = sr.ReadLine();
                        game1.userParam = (float)Convert.ToDouble(line);


                    }
                    line = sr.ReadLine();
                }
            }
            sr.Close();
        }

        public void Update(GameTime gameTime, Rectangle rightHandPosition)
        {
            MouseState mouseStateCurrent = Mouse.GetState();
            mouseStatePrevious = Game1.mouseStatePrevious;
            //    KeyboardState key = Keyboard.GetState();

            Rectangle rectMouseSettingBoard = new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, 5, 5);
            //nextButton 위에 마우스를 올려놨을 때
            //mousecursor on nextButton item section



             //스케일 증가 
            if (rectMouseSettingBoard.Intersects(recCheck) || rightHandPosition.Intersects(recCheck))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(recCheck);

              //  ClickScaleUpButton = true;
                //click the right hand item section
                if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {

                    Game1.nearButton = false;
                    if(checkFaceDetact)
                    {
                        checkFaceDetact = false;

                    }
                    else
                    {
                        checkFaceDetact = true;
                    }

                }
            }
            else
            {
               // ClickScaleUpButton = false;
            }


            //스케일 증가 
            if (rectMouseSettingBoard.Intersects(RecScaleUpButton) || rightHandPosition.Intersects(RecScaleUpButton))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(RecScaleUpButton);

                ClickScaleUpButton = true;
                //click the right hand item section
                if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {

                    Game1.nearButton = false;
                    if (game1.userParam < 1)
                    {
                        game1.userParam += 0.05f;
                    }
                    else
                    {
                        game1.userParam = 1;
                    }


                }
            }
            else
            {
                ClickScaleUpButton = false;
            }



            //스케일 감소
            if (rectMouseSettingBoard.Intersects(RecScaleDownButton) || rightHandPosition.Intersects(RecScaleDownButton))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(RecScaleDownButton);

                ClickScaleDownButton = true;
                //click the right hand item section
                if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {
                    Game1.nearButton = false;
                    if (game1.userParam > 0.05)
                    {
                        game1.userParam -= 0.05f;
                    }
                    else
                    {
                        game1.userParam = 0.05f;
                    }

                }
            }
            else
            {
                ClickScaleDownButton = false;
            }




            //각도 증가 
            if (rectMouseSettingBoard.Intersects(RecAngleUpButton) || rightHandPosition.Intersects(RecAngleUpButton))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(RecAngleUpButton);

                ClickAngleUpButton = true;
                //click the right hand item section
                if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {

                    Game1.nearButton = false;
                    if (game1.nui.ElevationAngle < game1.nui.MaxElevationAngle - 3)
                    {
                        while (true)
                        {
                            try
                            {
                                game1.nui.ElevationAngle += 3;
                                break;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }

                }
            }
            else
            {
                ClickAngleUpButton = false;
            }



            //각도 감소
            if (rectMouseSettingBoard.Intersects(RecAngleDownButton) || rightHandPosition.Intersects(RecAngleDownButton))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(RecAngleDownButton);

                ClickAngleDownButton = true;
                //click the right hand item section
                if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {

                    Game1.nearButton = false;
                    if (game1.nui.ElevationAngle > game1.nui.MinElevationAngle + 3)
                    {
                        while (true)
                        {
                            try
                            {
                                game1.nui.ElevationAngle -= 3;
                                break;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }
            else
            {
                ClickAngleDownButton = false;
            }



            if (rectMouseSettingBoard.Intersects(RectNextButton) || rightHandPosition.Intersects(RectNextButton))
            {

                Game1.nearButton = true;
                Game1.GetCenterOfButton(RectNextButton);

                ClickNextButton = true;
                //click the right hand item section
                if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {
                    Game1.nearButton = false;
                    Game1.gameState = Game1.GameStates.Menu;
                    SaveCheckFile();
                }
            }
            else
            {
                ClickNextButton = false;
            }


            if (rectMouseSettingBoard.Intersects(recPreviousButton) || rightHandPosition.Intersects(recPreviousButton))
            {

                Game1.nearButton = true;
                Game1.GetCenterOfButton(recPreviousButton);

                clickPreviousButton =true ;
                //click the right hand item section
                if ((mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrevious.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                {
                    Game1.gameState = Game1.GameStates.Menu;
                   
                    //가운데로 고정된거 풀기 
                    Game1.nearButton = false;
                }
            }
            else
            {
                clickPreviousButton = false;
            }


            if (
            !(rectMouseSettingBoard.Intersects(recNextButton) || rightHandPosition.Intersects(recNextButton))
            &&!(rectMouseSettingBoard.Intersects(recCheck) || rightHandPosition.Intersects(recCheck))
            && !(rectMouseSettingBoard.Intersects(RecScaleUpButton) || rightHandPosition.Intersects(RecScaleUpButton))
            && !(rectMouseSettingBoard.Intersects(RecScaleDownButton) || rightHandPosition.Intersects(RecScaleDownButton))
            && !(rectMouseSettingBoard.Intersects(RecAngleUpButton) || rightHandPosition.Intersects(RecAngleUpButton))
            && !(rectMouseSettingBoard.Intersects(RecAngleDownButton) || rightHandPosition.Intersects(RecAngleDownButton))
            && !(rectMouseSettingBoard.Intersects(recPreviousButton) || rightHandPosition.Intersects(recPreviousButton))
            )
            {

                Game1.nearButton = false;
            }





            mouseStatePrevious = mouseStateCurrent;
           

        }

        public void Draw(SpriteBatch spriteBatch)
        {
           
            spriteBatch.Draw(background, recBackground, Color.White);

            recNextButton = new Rectangle(800, 67, 123, 81);
            spriteBatch.Draw(nextButton, recNextButton, Color.White);

            recScaleUpButton = new Rectangle(768, 172, 102, 70);
            spriteBatch.Draw(UpButton, recScaleUpButton, Color.White);

            recScaleDownButton = new Rectangle(768, 249, 102, 70);
            spriteBatch.Draw(DownButton, recScaleDownButton, Color.White);

            recAngleUpButton = new Rectangle(768, 331, 102, 70);
            spriteBatch.Draw(UpButton, recAngleUpButton, Color.White);

            recAngleDownButton = new Rectangle(768, 407, 102, 70);
            spriteBatch.Draw(DownButton, recAngleDownButton, Color.White);

            

            float userParam = game1.UserParam;

            KinectSensor nui = game1.Nui;
            int elevationAngle  = nui.ElevationAngle;

          //  spriteBatch.DrawString(Game1.georgia, "scaling", new Vector2(100, 100), Color.Black);

            spriteBatch.DrawString(Game1.georgia, userParam.ToString(), new Vector2(570, 220), Color.Yellow);

          //  spriteBatch.DrawString(Game1.georgia, "angle", new Vector2(100, 300), Color.Black);

            spriteBatch.DrawString(Game1.georgia, elevationAngle.ToString(), new Vector2(600, 370), Color.Yellow);


            spriteBatch.Draw(Game1.previousButton, new Vector2(recPreviousButton.X, recPreviousButton.Y), null, Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 1f);



            if (clickNextButton)
            {
                spriteBatch.Draw(hoverNextButton, recNextButton, Color.White);
            }

            if (checkFaceDetact)
            {
                spriteBatch.Draw(check, recCheck, Color.White);
            }
            if (clickScaleUpButton)
            {
                spriteBatch.Draw(hoverUpButton, recScaleUpButton, Color.White);
            }


            if (clickScaleDownButton)
            {
                spriteBatch.Draw(hoverDownButton, recScaleDownButton, Color.White);
            }

            if (clickAngleUpButton)
            {
                spriteBatch.Draw(hoverUpButton, recAngleUpButton, Color.White);
            }


            if (clickAngleDownButton)
            {
                spriteBatch.Draw(hoverDownButton, recAngleDownButton, Color.White);
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



        public Rectangle RectNextButton
        {
            get { return recNextButton; }
        }


        public bool CheckFaceDetact
        {
            get { return checkFaceDetact; }
            set { checkFaceDetact = value; }
        }

        public Rectangle RecScaleUpButton
        {
            get { return recScaleUpButton; }
        }

        public Rectangle RecScaleDownButton
        {
            get { return recScaleDownButton; }
        }

        public Rectangle RecAngleUpButton
        {
            get { return recAngleUpButton; }
        }

        public Rectangle RecAngleDownButton
        {
            get { return recAngleDownButton; }
        }

        public bool ClickNextButton
        {
            set { clickNextButton = value; }
        }


        public bool ClickScaleUpButton
        {
            set { clickScaleUpButton = value; }
        }


        public bool ClickScaleDownButton
        {
            set { clickScaleDownButton = value; }
        }


        public bool ClickAngleUpButton
        {
            set { clickAngleUpButton = value; }
        }


        public bool ClickAngleDownButton
        {
            set { clickAngleDownButton = value; }
        }

     
    }
}
