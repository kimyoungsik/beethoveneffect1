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
    class SongMenu
    {
        private NoteFileManager noteFileManager;

        private SpriteFont pericles36Font;
      //  bool pastClick;
        MouseState pastmouse;
        KeyboardState pastkey;

        Texture2D background;
        Texture2D[] pictures;
        Texture2D[] leftright;
        Texture2D box;//
        Texture2D backtext;//back
        Texture2D starttext;//back

        SpriteFont font;
        Rectangle backrect;
        Rectangle backtextrect;
        Rectangle startrect;
        Rectangle starttextrect;
        Texture2D[] top;

        private Texture2D levelTexture;


        public bool isKinectRight = false;
        public bool isKinectLeft = false;
        int scene_number;
        int fadeinout = 0;
        int[] arrawframe;
        bool[] arrawframebutton;
        int frame;
        int margin;
        int leftrightmove;
        bool textbutton;
        int textanitime;
        // tutorial
        public SongMenu(NoteFileManager noteFileManager)
        {

            this.noteFileManager = noteFileManager;


            pictures = new Texture2D[10];
            leftright = new Texture2D[2];
            arrawframe = new int[2];
            arrawframebutton = new bool[2];
            backrect = new Rectangle(50,  655, 180, 45);
            backtextrect = new Rectangle(110, 660, 60, 30);

            startrect = new Rectangle(800, 655, 180, 45);
            starttextrect = new Rectangle(860, 660, 60, 30);
            top = new Texture2D[7];


        }
        

        //컨텐츠 로드
        public void Load(ContentManager content,GraphicsDevice graphicsdevice)
        {
            pericles36Font = content.Load<SpriteFont>(@"Fonts\Pericles36");
            levelTexture = content.Load<Texture2D>(@"ui\heart");



            background = content.Load<Texture2D>("SongMenu/background");
            leftright[0] = content.Load<Texture2D>("SongMenu/arrow1");
            leftright[1] = content.Load<Texture2D>("SongMenu/arrow2");
            box = content.Load<Texture2D>("Title/box");
            //starox = content.Load<Texture2D>("Title/box");
            
            backtext = content.Load<Texture2D>("Status/backtext");
            starttext = content.Load<Texture2D>("Status/backtext");



            for (int i = 0; i < noteFileManager.noteFiles.Count; i++)
            {
                //String a = noteFileManager.noteFiles[i].Mp3;

                int dupicateIndex = FindDuplicatePicture(i, noteFileManager.noteFiles[i].Picture);

                if (dupicateIndex == -1)
                {
                    FileStream fileStream = new FileStream(System.Environment.CurrentDirectory + "\\beethovenSong\\" + noteFileManager.noteFiles[i].Picture, FileMode.Open);

                    pictures[i] = Texture2D.FromStream(graphicsdevice, fileStream);
                } 
                else
                {
                    pictures[i] = pictures[dupicateIndex];

                }

               
            }
            //for (int i = 0; i < 7; i++)
            //{
            //    top[i] = content.Load<Texture2D>("Tutorial/top" + (i + 1));
            //}
            font = content.Load<SpriteFont>("Fonts/damagefont");
        }


        //중복된 사진이 있을 경우에
        public int FindDuplicatePicture(int currentIndex, String PicName)
        {
            int ret = -1;
            int i;

            for (i = 0; i < currentIndex; i++)
            {
                if (PicName == noteFileManager.noteFiles[i].Picture)
                {

                    ret = i;
                    i = currentIndex;

                }

            }
                
            
            return ret;


        }


        //결과 화면에서 쓰일 그림
        public Texture2D FindPicture(NoteFile noteFile)
        {
            Texture2D picture = null;
            int i ;
            for (i = 0; i < noteFileManager.noteFiles.Count; i++)
            {
                if (noteFileManager.noteFiles[i] == noteFile)
                {
                    picture = pictures[i];
                    i = noteFileManager.noteFiles.Count;
                }
            }
            return picture;
        }

        //업데이트
        public int Scene_number
        {
            get { return scene_number; }
            set { scene_number = value; }
        }



        public int Update()
        {
            Rectangle rightHandPosition = new Rectangle((int)Game1.j1r.Position.X, (int)Game1.j1r.Position.Y, 5, 5);


            MouseState mouse = Mouse.GetState();
            KeyboardState key = Keyboard.GetState();



            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            Rectangle recRightArrow = new Rectangle(770, 310, 60, 60);
            Rectangle recLeftArrow = new Rectangle(170, 310, 60, 60);

            

            //left
            if (mouseRectangle.Intersects(recLeftArrow) || rightHandPosition.Intersects(recLeftArrow))
            {
                Game1.nearButton = true;
                Game1.GetCenterOfButton(recLeftArrow);
                 

                arrawframebutton[0] = true;


                if ((mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || (Game1.finalClick && !Game1.pastClick) || ((key.IsKeyDown(Keys.Left) && !pastkey.IsKeyDown(Keys.Left))))
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



            if (mouseRectangle.Intersects(recRightArrow) || rightHandPosition.Intersects(recRightArrow))
            {



                Game1.nearButton = true;
                Game1.GetCenterOfButton(recRightArrow);
                           
                arrawframebutton[1] = true;

                if ( (mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released)|| ( key.IsKeyDown(Keys.Right) && !pastkey.IsKeyDown(Keys.Right) ) || ( Game1.finalClick && !Game1.pastClick) )
                
                {

                    if (scene_number < noteFileManager.noteFiles.Count - 1)
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



            if (!(mouseRectangle.Intersects(recRightArrow) || rightHandPosition.Intersects(recRightArrow))
                &&!(mouseRectangle.Intersects(recLeftArrow) || rightHandPosition.Intersects(recLeftArrow))
                &&!(mouseRectangle.Intersects(backrect) || rightHandPosition.Intersects(backrect))
                &&!(  mouseRectangle.Intersects(startrect) || rightHandPosition.Intersects(startrect)))

            
            {

                Game1.nearButton = false;

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
                if (scene_number < noteFileManager.noteFiles.Count - 1)
                {
                    leftrightmove = 1;
                    scene_number++;
                    frame = 0;
                }

                isKinectRight = false;


            }
            //if (mouseRectangle.Intersects(backrect) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released)
            //{
            //    return 1;
            //}



            if (mouseRectangle.Intersects(backrect) || rightHandPosition.Intersects(backrect))
            {
                //버튼 변하는 부분
                Game1.nearButton = true;
                Game1.GetCenterOfButton(backrect);
                if( (Game1.finalClick && !Game1.pastClick) || (mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released))
                {
                    Game1.nearButton = false;
                    return -1;
                }
            }
           


           if(  mouseRectangle.Intersects(startrect) || rightHandPosition.Intersects(startrect))
           {
      
            //else if ((mouseRectangle.Intersects(startrect) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || (Game1.drawrec1.Intersects(startrect) && Game1.finalClick && !Game1.pastClick) || Game1.soundRecogStartIndex != -1)
            //{
               //버튼 변하는 부분
               Game1.nearButton = true;
               Game1.GetCenterOfButton(startrect);
               if ((Game1.finalClick && !Game1.pastClick) || (mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released))
               {
                   Game1.nearButton = false;
                    int ret;
                    if (Game1.soundRecogStartIndex != -1)
                    {
                        ret = Game1.soundRecogStartIndex;
                    }

                    //보통때는 -1 이다가 start가 인식하면 곡의 인덱스 값을 받음
                    Game1.soundRecogStartIndex = -1;

                    ret = scene_number;
                    return ret;
               }
            }


            else
            {
                textbutton = false;
                textanitime = 30;
            }


            fadeinout += 5;
            fadeinout = Math.Min(fadeinout, 255);

            pastmouse = mouse;
            pastkey = key;
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
            spriteBatch.Draw(background, Vector2.Zero, new Color(fadeinout, fadeinout, fadeinout));
            drawText(spriteBatch);
            drawArrow(spriteBatch);
           // drawNumber(spriteBatch);
            drawBack(spriteBatch);
            drawStart(spriteBatch);
            TextAnimation1(spriteBatch);

            //노래가 하나라도 있어야한다. 


            if (noteFileManager.noteFiles.Count > 0)
            {
                String name = noteFileManager.noteFiles[scene_number].Name;
                spriteBatch.DrawString(pericles36Font, name, new Vector2(512, 420), Color.White);


                //     spriteBatch.Draw(uiBackground, new Vector2(0, 0), Color.White);

                int level = noteFileManager.noteFiles[scene_number].Level;
                //0이하이거나 넘어가지 않게 

                Rectangle rec = new Rectangle(0, 0, level * 40/*하나의 그림의 width*/, 50);
                //하트. gage양 만큼 하트가 나타남.
                spriteBatch.Draw(levelTexture, new Vector2(300, 700), rec, Color.White);
            }

          //  drawTop(spriteBatch);
        }
        public void drawArrow(SpriteBatch spriteBatch)
        {
            if (fadeinout == 255)
            {
                if (arrawframebutton[0])
                {
                    arrawframe[0] += 4;
                    arrawframe[0] = Math.Min(arrawframe[0], 45);
                    float arrawddiyong1 = 30 * (float)Math.Tan(MathHelper.ToRadians(arrawframe[0]));
                    if (scene_number != 0)
                        spriteBatch.Draw(leftright[0], new Rectangle(200 - (int)arrawddiyong1, 310, 30 + (int)arrawddiyong1, 60), new Color(fadeinout, fadeinout, fadeinout));
                }
                else
                {
                    if (scene_number != 0)
                        spriteBatch.Draw(leftright[0], new Rectangle(170, 310, 60, 60), new Color(fadeinout, fadeinout, fadeinout));
                }
                if (arrawframebutton[1])
                {
                    arrawframe[1] += 4;
                    arrawframe[1] = Math.Min(arrawframe[1], 45);
                    float arrawddiyong2 = 30 * (float)Math.Tan(MathHelper.ToRadians(arrawframe[1]));
                    if (scene_number != noteFileManager.noteFiles.Count-1)
                        spriteBatch.Draw(leftright[1], new Rectangle(770, 310, 30 + (int)arrawddiyong2, 60), new Color(fadeinout, fadeinout, fadeinout));
                }
                else
                {
                  //  if (scene_number != 0 && scene_number != noteFileManager.noteFiles.Count-1)
                    if (noteFileManager.noteFiles.Count > 0 && scene_number != noteFileManager.noteFiles.Count - 1)
                  
                      spriteBatch.Draw(leftright[1], new Rectangle(770, 310, 60, 60), new Color(fadeinout, fadeinout, fadeinout));
                }
            }
        }
        public void drawText(SpriteBatch spriteBatch)
        {
            if (leftrightmove == 1)
            {
                frame += 3;
                frame = Math.Min(90, frame);
                margin = 800 - (int)(800 * Math.Sin(MathHelper.ToRadians(frame)));
                if (frame == 0)
                    leftrightmove = 0;
            }
            else if (leftrightmove == -1)
            {
                frame += 3;
                frame = Math.Min(90, frame);
                margin = (int)(800 * Math.Sin(MathHelper.ToRadians(frame))) - 800;
                if (frame == 0)
                    leftrightmove = 0;
            }

            if (scene_number - 1 >= 0 && leftrightmove == 1)
            {
                if (scene_number - 1 == 0)
                    spriteBatch.Draw(pictures[scene_number - 1], new Rectangle(margin - 1024, 100, 500, 500), new Color(fadeinout, fadeinout, fadeinout));
                else if (scene_number - 1 == 8)
                    spriteBatch.Draw(pictures[scene_number - 1], new Rectangle(margin - 1024, 100, 500, 500), new Color(fadeinout, fadeinout, fadeinout));
                else
                    spriteBatch.Draw(pictures[scene_number - 1], new Rectangle(margin - 1024, 100, 500, 500), new Color(fadeinout, fadeinout, fadeinout));
            }
            if (scene_number < noteFileManager.noteFiles.Count)
            {
                if (scene_number == 0)
                    spriteBatch.Draw(pictures[scene_number], new Rectangle(margin + 250, 100, 500, 500), new Color(fadeinout, fadeinout, fadeinout));
                else if (scene_number == noteFileManager.noteFiles.Count)
                    spriteBatch.Draw(pictures[scene_number], new Rectangle(margin + 250, 100, 500, 500), new Color(fadeinout, fadeinout, fadeinout));
                else
                    spriteBatch.Draw(pictures[scene_number], new Rectangle(margin + 250, 100, 500, 500), new Color(fadeinout, fadeinout, fadeinout));
            }
            if (scene_number + 1 < noteFileManager.noteFiles.Count && leftrightmove == -1)
            {
                if (scene_number + 1 == 0)
                    spriteBatch.Draw(pictures[scene_number + 1], new Rectangle(margin + 1024, 0, 500, 500), new Color(fadeinout, fadeinout, fadeinout));
                else if (scene_number + 1 == 8)
                    spriteBatch.Draw(pictures[scene_number + 1], new Rectangle(margin + 1024, 100, 500, 500), new Color(fadeinout, fadeinout, fadeinout));
                else
                    spriteBatch.Draw(pictures[scene_number + 1], new Rectangle(margin + 1024, 100, 500, 500), new Color(fadeinout, fadeinout, fadeinout));
            }
        }
        //public void drawNumber(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.DrawString(font, (scene_number + 1) + "/3", new Vector2(350, 510), Color.Black);
        //}
        public void drawBack(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(box, backrect, new Color(fadeinout, fadeinout, fadeinout));
            spriteBatch.Draw(backtext, backtextrect, new Color(fadeinout, fadeinout, fadeinout));
        }

        public void drawStart(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(box, startrect, new Color(fadeinout, fadeinout, fadeinout));
          //  spriteBatch.Draw(starttext, starttextrect, new Color(fadeinout, fadeinout, fadeinout));
        }
        public void drawTop(SpriteBatch spriteBatch)
        {
            if (scene_number == 0)
                spriteBatch.Draw(top[1], new Rectangle(0, 0, 600, 40), Color.White);
            if (scene_number >= 1 && scene_number < 3)
                spriteBatch.Draw(top[2], new Rectangle(0, 0, 600, 40), Color.White);
            if (scene_number == 3)
                spriteBatch.Draw(top[3], new Rectangle(0, 0, 600, 40), Color.White);
            if (scene_number == 4)
                spriteBatch.Draw(top[4], new Rectangle(0, 0, 600, 40), Color.White);
            if (scene_number == 5)
                spriteBatch.Draw(top[5], new Rectangle(0, 0, 600, 40), Color.White);
            if (scene_number > 5)
                spriteBatch.Draw(top[6], new Rectangle(0, 0, 600, 40), Color.White);
        }

        public void TextAnimation1(SpriteBatch spriteBatch)
        {
            if (textbutton)
            {
                int x, width;
                textanitime--;
                textanitime = Math.Max(0, textanitime);
                if (textanitime != 0)
                {
                    x = backtextrect.X - 65 - backtextrect.Width * (30 - textanitime) / 130;
                    width = backrect.Width + 20 + backtextrect.Width * (30 - textanitime) / 65;
                    spriteBatch.Draw(backtext, new Rectangle(x, backtextrect.Y, width, backtextrect.Height), new Color(200, 200, 200, 180 - 6 * (30 - textanitime)));
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
