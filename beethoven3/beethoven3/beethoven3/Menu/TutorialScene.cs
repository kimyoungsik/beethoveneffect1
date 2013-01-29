using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
namespace beethoven3
{
    class TutorialScene
    {
        MouseState pastmouse;
       
        Texture2D background;
        Texture2D[] pictures;
        Texture2D[] leftright;
        public bool isKinectLeft = false;
        public bool isKinectRight = false;
     
        private Rectangle backrect = new Rectangle(36, 35, 110, 85);
        private bool clickPreviousButton;

        private Vector2 currentPageLocation = new Vector2(850, 700);
    //    private Vector2 wholePageLocation = new Vector2(800, 600);

      //  Texture2D songBackground;

        //  private Texture2D levelTexture;
        private int count = 20;

       
        int scene_number;
        int fadeinout = 0;
        int[] arrawframe;
        bool[] arrawframebutton;
        int frame;
        int margin;
        int leftrightmove;
        // bool textbutton;
        // int textanitime;
        // tutorial
        public TutorialScene()
        {

         
            pictures = new Texture2D[30];
            leftright = new Texture2D[2];
            arrawframe = new int[2];
            arrawframebutton = new bool[2];
            scene_number = 0;

            clickPreviousButton = false;
            
        }


        //컨텐츠 로드
        public void Load(ContentManager content)
        {
            background = content.Load<Texture2D>(@"tutorial\tutorialBackground");
            //    levelTexture = content.Load<Texture2D>(@"ui\heart");


            //background = content.Load<Texture2D>("SongMenu/background");
            leftright[0] = content.Load<Texture2D>("SongMenu/leftArrow");
            leftright[1] = content.Load<Texture2D>("SongMenu/rightArrow");
            String tutorialPage = "tutorial/tutorial";

            for (int i = 0; i < count; i++)
            {

                String name = tutorialPage + Convert.ToString(i+2);
                pictures[i] = content.Load<Texture2D>(name);

            }

        }



        //업데이트
        public int Scene_number
        {
            get { return scene_number; }
            set { scene_number = value; }
        }



        public int Update()
        {

#if Kinect

            Rectangle rightHandPosition = new Rectangle((int)Game1.j1r.Position.X, (int)Game1.j1r.Position.Y, 5, 5);
#else

            Rectangle rightHandPosition = new Rectangle(0, 0, 5, 5);
#endif

            MouseState mouse = Mouse.GetState();
            KeyboardState key = Keyboard.GetState();
            pastmouse = Game1.mouseStatePrevious;


            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            Rectangle recRightArrow = new Rectangle(900, 310, 120, 120);
            Rectangle recLeftArrow = new Rectangle(10, 310, 120, 120);


            if (scene_number > 0)
            {
                //left
                if (mouseRectangle.Intersects(recLeftArrow) || rightHandPosition.Intersects(recLeftArrow))
                {
                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(recLeftArrow);


                    arrawframebutton[0] = true;


                    if ((mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick) || ((key.IsKeyDown(Keys.Left))))
                    {

                        if (scene_number > 0)
                        {
                            Game1.nearButton = false;
                            scene_number--;
                            leftrightmove = -1;
                            frame = 0;

                        }
                    }
                }
                else
                {

                    arrawframebutton[0] = false;
                    arrawframe[0] = 0;
                }

            }
            if (scene_number < count - 1)
            {
                if (mouseRectangle.Intersects(recRightArrow) || rightHandPosition.Intersects(recRightArrow))
                {



                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(recRightArrow);

                    arrawframebutton[1] = true;

                    if ((mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick))
                    {

                        if (scene_number < count - 1)
                        {

                            Game1.nearButton = false;
                            leftrightmove = 1;
                            scene_number++;
                            frame = 0;



                        }
                    }



                }
                else
                {



                    arrawframebutton[1] = false;
                    arrawframe[1] = 0;
                }
            }


            if (!(mouseRectangle.Intersects(recRightArrow) || rightHandPosition.Intersects(recRightArrow))
                && !(mouseRectangle.Intersects(recLeftArrow) || rightHandPosition.Intersects(recLeftArrow))
                && !(mouseRectangle.Intersects(backrect) || rightHandPosition.Intersects(backrect))
               
                )
            {

                Game1.nearButton = false;

            }





            if (mouseRectangle.Intersects(backrect) || rightHandPosition.Intersects(backrect))
            {
                //버튼 변하는 부분
                Game1.nearButton = true;
                Game1.GetCenterOfButton(backrect);
                clickPreviousButton = true;

                if ((Game1.finalClick && !Game1.pastClick) || (mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released))
                {
                    clickPreviousButton = false;
                    Game1.nearButton = false;

                    //  SoundFmod.StopSound();
                    return -1;
                }
            }
            else
            {
                clickPreviousButton = false;
            }

           
            //키넥트로 왼=> 오 왼쪽 이동
            if (isKinectLeft)
            {
                if (scene_number > 0)
                {
                    scene_number--;
                    leftrightmove = -1;
                    frame = 0;
                   
                }
                isKinectLeft = false;


            }


            //키넥트로 오->왼 => 오른쪽 이동 
            if (isKinectRight)
            {
                if (scene_number < count - 1)
                {
                    leftrightmove = 1;
                    scene_number++;
                    frame = 0;
                  
                }

                isKinectRight = false;


            }
            

            fadeinout += 5;
            fadeinout = Math.Min(fadeinout, 255);

            pastmouse = mouse;
        
            Game1.pastClick = Game1.finalClick;




            return -2;

        }

        /// <summary>
        /// 그린다 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="fireing"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(background, Vector2.Zero, new Color(fadeinout, fadeinout, fadeinout));
            //  spriteBatch.Draw(songBackground, new Vector2(270,100), new Color(fadeinout, fadeinout, fadeinout));
           spriteBatch.Draw(background, new Rectangle(0, 0, 1024, 769), Color.White);
            //사진

            drawText(spriteBatch);


            drawArrow(spriteBatch);

            //노래가 하나라도 있어야한다. 

            spriteBatch.Draw(Game1.previousButton, new Vector2(backrect.X, backrect.Y), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            if (clickPreviousButton)
            {

                spriteBatch.Draw(Game1.hoverPreviousButton, new Vector2(backrect.X, backrect.Y), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
            }





            spriteBatch.DrawString(Game1.georgia, (scene_number+1).ToString(), currentPageLocation, Color.White);


            spriteBatch.DrawString(Game1.georgia, " / ", new Vector2(currentPageLocation.X + 45, currentPageLocation.Y), Color.White);



            spriteBatch.DrawString(Game1.georgia, count.ToString(), new Vector2(currentPageLocation.X + 100, currentPageLocation.Y), Color.White);
           
            //  drawTop(spriteBatch);
        }
        public void drawArrow(SpriteBatch spriteBatch)
        {
            Color color = Color.White;
            color.A = 50;
            if (fadeinout == 255)
            {
                if (arrawframebutton[0])
                {
                    arrawframe[0] += 4;
                    arrawframe[0] = Math.Min(arrawframe[0], 45);

                    //이거변함
                    float arrawddiyong1 = 80 * (float)Math.Tan(MathHelper.ToRadians(arrawframe[0]));
                    if (scene_number != 0)
                    {

                        //크기조정

                       
                        spriteBatch.Draw(leftright[0], new Rectangle(100 - (int)arrawddiyong1, 310, 30 + (int)arrawddiyong1, 120), color);

                        // spriteBatch.Draw(leftright[0], new Vector2(200 - (int)arrawddiyong1, 310), new Rectangle(0, 0, 30 + (int)arrawddiyong1, 60), new Color(fadeinout, fadeinout, fadeinout), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
                    }
                }
                else
                {
                    if (scene_number != 0)
                    {
                        // spriteBatch.Draw(leftright[0], new Rectangle(170, 310, 60, 60), new Color(fadeinout, fadeinout, fadeinout));
                        spriteBatch.Draw(leftright[0], new Rectangle(10, 310, 120, 120), color);

                    }

                }
                if (arrawframebutton[1])
                {
                    arrawframe[1] += 4;
                    arrawframe[1] = Math.Min(arrawframe[1], 45);
                    float arrawddiyong2 = 80 * (float)Math.Tan(MathHelper.ToRadians(arrawframe[1]));
                    if (scene_number != count-1)
                    {
                        spriteBatch.Draw(leftright[1], new Rectangle(900, 310, 30 + (int)arrawddiyong2, 120), color);
                    }
                }
                else
                {
                    //  if (scene_number != 0 && scene_number != noteFileManager.noteFiles.Count-1)
                    if (17 > 0 && scene_number != count - 1)

                        spriteBatch.Draw(leftright[1], new Rectangle(900, 310, 120, 120), color);
                }
            }
        }
        public void drawText(SpriteBatch spriteBatch)
        {
            int sizew = 1024;
            int size = 769;
            //int backgroundSizeWidth = 475;
            //int backgroundSizeHeight = 600;
            //int backgroundHeight = 100;
            //int backgroundWidth = 20;
            int height = 0; //높이 위치
            int width = -250;
            if (leftrightmove == 1)
            {
                frame += 3;
                frame = Math.Min(90, frame);
                margin = 800 - (int)(800 * Math.Sin(MathHelper.ToRadians(frame)));

                //곡 바꿀시 노래재생
              
                if (frame == 0)
                    leftrightmove = 0;
            }
            else if (leftrightmove == -1)
            {
                frame += 3;
                frame = Math.Min(90, frame);
                margin = (int)(800 * Math.Sin(MathHelper.ToRadians(frame))) - 800;

                //곡 바꿀시 노래재생
               
                if (frame == 0)
                    leftrightmove = 0;
            }

            if (scene_number - 1 >= 0 && leftrightmove == 1)
            {
                if (scene_number - 1 == 0)
                {
               //     spriteBatch.Draw(songBackground, new Rectangle(margin - 1024 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));

                    //  spriteBatch.Draw(, new Vector2(270, 100), new Color(fadeinout, fadeinout, fadeinout));
                    spriteBatch.Draw(pictures[scene_number - 1], new Rectangle(margin - 1024 + width, height, sizew, size), new Color(fadeinout, fadeinout, fadeinout));
                }
                else if (scene_number - 1 == 8)
                {
               //     spriteBatch.Draw(songBackground, new Rectangle(margin - 1024 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));

                    spriteBatch.Draw(pictures[scene_number - 1], new Rectangle(margin - 1024 + width, height, sizew, size), new Color(fadeinout, fadeinout, fadeinout));
                }
                else
                {
                //    spriteBatch.Draw(songBackground, new Rectangle(margin - 1024 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));

                    spriteBatch.Draw(pictures[scene_number - 1], new Rectangle(margin - 1024 + width, height, sizew, size), new Color(fadeinout, fadeinout, fadeinout));
                }
            }
            if (scene_number < count)
            {
                if (scene_number == 0)
                {
              //      spriteBatch.Draw(songBackground, new Rectangle(margin + 250 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));

                    spriteBatch.Draw(pictures[scene_number], new Rectangle(margin + 250 + width, height, sizew, size), new Color(fadeinout, fadeinout, fadeinout));

                }
                else if (scene_number == count)
                {
                  //  spriteBatch.Draw(songBackground, new Rectangle(margin + 250 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));

                    spriteBatch.Draw(pictures[scene_number], new Rectangle(margin + 250 + width, height, sizew, size), new Color(fadeinout, fadeinout, fadeinout));

                }
                else
                {
                //    spriteBatch.Draw(songBackground, new Rectangle(margin + 250 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));

                    spriteBatch.Draw(pictures[scene_number], new Rectangle(margin + 250 + width, height, sizew, size), new Color(fadeinout, fadeinout, fadeinout));

                }
            }
            if (scene_number + 1 < count && leftrightmove == -1)
            {
                if (scene_number + 1 == 0)
                {
                 //   spriteBatch.Draw(songBackground, new Rectangle(margin + 1024 + backgroundWidth, 0, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));

                    spriteBatch.Draw(pictures[scene_number + 1], new Rectangle(margin + 1024 , 0, sizew, size), new Color(fadeinout, fadeinout, fadeinout));

                }
                else if (scene_number + 1 == 8)
                {
                 //   spriteBatch.Draw(songBackground, new Rectangle(margin + 1024 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));

                    spriteBatch.Draw(pictures[scene_number + 1], new Rectangle(margin + 1024  , height, sizew, size), new Color(fadeinout, fadeinout, fadeinout));

                }
                else
                {
                  //  spriteBatch.Draw(songBackground, new Rectangle(margin + 1024 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));

                    spriteBatch.Draw(pictures[scene_number + 1], new Rectangle(margin + 1024 , height, sizew, size), new Color(fadeinout, fadeinout, fadeinout));


                }
            }
        }


        public void choGiWha()
        {
            fadeinout = 0;
            scene_number = 0;
        }
    }
}
