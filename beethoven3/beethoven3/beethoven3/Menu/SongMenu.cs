using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace beethoven3
{
    class SongMenu
    {
        private NoteFileManager noteFileManager;


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

            background = content.Load<Texture2D>("SongMenu/background");
            leftright[0] = content.Load<Texture2D>("SongMenu/arrow1");
            leftright[1] = content.Load<Texture2D>("SongMenu/arrow2");
            box = content.Load<Texture2D>("Title/box");
            //starox = content.Load<Texture2D>("Title/box");
            
            backtext = content.Load<Texture2D>("Status/backtext");
            starttext = content.Load<Texture2D>("Status/backtext");

            for (int i = 0; i < noteFileManager.noteFiles.Count; i++)
            {
                String a = noteFileManager.noteFiles[i].Mp3;
                FileStream fileStream = new FileStream(@"C:\\beethoven\\" + noteFileManager.noteFiles[i].Picture, FileMode.Open);
                
                    pictures[i] = Texture2D.FromStream(graphicsdevice, fileStream);
                
               
            }
            //for (int i = 0; i < 7; i++)
            //{
            //    top[i] = content.Load<Texture2D>("Tutorial/top" + (i + 1));
            //}
            font = content.Load<SpriteFont>("Fonts/damagefont");
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
        public int Update()
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState key = Keyboard.GetState();



            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            //검은색 버튼 눌렀을 때,
            //if ((mouseRectangle.Intersects(new Rectangle(0, 0, 100, 40)) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released))
            //{
            //    scene_number = 0;
            //}
            //if ((mouseRectangle.Intersects(new Rectangle(100, 0, 100, 40)) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released))
            //{
            //    scene_number = 1;
            //}
            //if ((mouseRectangle.Intersects(new Rectangle(200, 0, 100, 40)) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released))
            //{
            //    scene_number = 3;
            //}
            //if ((mouseRectangle.Intersects(new Rectangle(300, 0, 100, 40)) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released))
            //{
            //    scene_number = 4;
            //}
            //if ((mouseRectangle.Intersects(new Rectangle(400, 0, 100, 40)) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released))
            //{
            //    scene_number = 5;
            //}
            //if ((mouseRectangle.Intersects(new Rectangle(500, 0, 100, 40)) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released))
            //{
            //    scene_number = 6;
            //}


            //left 버튼 누를 때
            if ((mouseRectangle.Intersects(new Rectangle(170, 310, 60, 60)) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released)
                || (key.IsKeyDown(Keys.Left) && !pastkey.IsKeyDown(Keys.Left)) || Game1.drawrec1.Intersects(new Rectangle(170, 310, 60, 60)) && Game1.finalClick && !Game1.pastClick)
            {
                if (scene_number > 0)
                {
                    scene_number--;
                    leftrightmove = -1;
                    frame = 0;
                }
            }
            else if (mouseRectangle.Intersects(new Rectangle(170, 310, 60, 60)) && mouse.LeftButton == ButtonState.Released || Game1.drawrec1.Intersects(new Rectangle(170, 310, 60, 60)))
            {
                arrawframebutton[0] = true;
            }
            else
            {
                arrawframebutton[0] = false;
                arrawframe[0] = 0;
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
            if(isKinectRight)

            {
                 if (scene_number < noteFileManager.noteFiles.Count-1)
                {
                    leftrightmove = 1;
                    scene_number++;
                    frame = 0;
                }

                 isKinectRight = false;


            }




            //right
            if ((mouseRectangle.Intersects(new Rectangle(770, 310, 60, 60)) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released)
                || (key.IsKeyDown(Keys.Right) && !pastkey.IsKeyDown(Keys.Right)) || Game1.drawrec1.Intersects(new Rectangle(770, 310, 60, 60)) && Game1.finalClick && !Game1.pastClick)
            {
                if (scene_number < noteFileManager.noteFiles.Count-1)
                {
                    leftrightmove = 1;
                    scene_number++;
                    frame = 0;
                }
            }
            else if (mouseRectangle.Intersects(new Rectangle(770, 310, 60, 60)) && mouse.LeftButton == ButtonState.Released || Game1.drawrec1.Intersects(new Rectangle(770, 310, 60, 60)))
            {
                arrawframebutton[1] = true;
            }
            else
            {
                arrawframebutton[1] = false;
                arrawframe[1] = 0;
            }

            //if (mouseRectangle.Intersects(backrect) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released)
            //{
            //    return 1;
            //}



            if (mouseRectangle.Intersects(backrect) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released || Game1.drawrec1.Intersects(backrect) && Game1.finalClick && !Game1.pastClick)
            {
                return -1;
            }
            else if (mouseRectangle.Intersects(backrect) && mouse.LeftButton == ButtonState.Released)
            {
                textbutton = true;
            }
            else if (mouseRectangle.Intersects(startrect) && mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released || Game1.drawrec1.Intersects(startrect) && Game1.finalClick && !Game1.pastClick)
            {
                return scene_number;
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
            drawNumber(spriteBatch);
            drawBack(spriteBatch);
            drawStart(spriteBatch);
            TextAnimation1(spriteBatch); 
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
                    if (scene_number != noteFileManager.noteFiles.Count-1)
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
        public void drawNumber(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, (scene_number + 1) + "/3", new Vector2(350, 510), Color.Black);
        }
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
