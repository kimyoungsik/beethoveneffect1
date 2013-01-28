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
        private ReportManager reportManager;
        public static bool songChanged;
        public static bool opening;
     //   private SpriteFont pericles36Font;
      //  bool pastClick;
        MouseState pastmouse;
        KeyboardState pastkey;

        Texture2D background;
        Texture2D[] pictures;
        Texture2D[] leftright;

       //시작
        Texture2D starttext;//back
        Texture2D hoverStarttext;


        private Texture2D noSongBackground;
        private Rectangle backrect = new Rectangle(36, 35, 110, 85);
        private bool clickPreviousButton;

        private Texture2D basicSongPicture;

        private bool clickStartButton;

        private Rectangle startrect = new Rectangle(812, 34, 193, 79);

        private Texture2D songBackground;

        
        private Vector2 currentSongLocation = new Vector2(850, 700);
      //  private Texture2D levelTexture;


        public bool isKinectRight = false;
        public bool isKinectLeft = false;
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
        public SongMenu(NoteFileManager noteFileManager,ReportManager reportManager)
        {

            this.noteFileManager = noteFileManager;
            this.reportManager = reportManager;

            pictures = new Texture2D[50];
            leftright = new Texture2D[2];
            arrawframe = new int[2];
            arrawframebutton = new bool[2];
            scene_number = 0;
       
            clickPreviousButton = false;
            clickStartButton = false;
        }
        

        //컨텐츠 로드
        public void Load(ContentManager content,GraphicsDevice graphicsdevice)
        {
            songBackground = content.Load<Texture2D>(@"SongMenu\songBackground");
        //    levelTexture = content.Load<Texture2D>(@"ui\heart");


            background = content.Load<Texture2D>("SongMenu/background");
            leftright[0] = content.Load<Texture2D>("SongMenu/leftArrow");
            leftright[1] = content.Load<Texture2D>("SongMenu/rightArrow");

         
            starttext = content.Load<Texture2D>("SongMenu/startButton");

            hoverStarttext = content.Load<Texture2D>("SongMenu/hoverStartButton");

            basicSongPicture = content.Load<Texture2D>("SongMenu/No_Image");

            noSongBackground = content.Load<Texture2D>("SongMenu/noSongBackground");


            for (int i = 0; i < noteFileManager.noteFiles.Count; i++)
            {
                //String a = noteFileManager.noteFiles[i].Mp3;

                int dupicateIndex = FindDuplicatePicture(i, noteFileManager.noteFiles[i].Picture);

                if (dupicateIndex == -1)
                {

                    //그림파일이 있을경우 
                    if (noteFileManager.noteFiles[i].Picture != "")
                    {
                        FileStream fileStream = new FileStream(System.Environment.CurrentDirectory + "\\beethovenSong\\" + noteFileManager.noteFiles[i].Picture, FileMode.Open);
                        pictures[i] = Texture2D.FromStream(graphicsdevice, fileStream);
                    }
                    //없을경우 기본으로 설정
                    else
                    {
                        pictures[i] = basicSongPicture;
                    }
                    
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
         //   font = content.Load<SpriteFont>("Fonts/damagefont");
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
            pastmouse = Game1.mouseStatePrevious;


            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            Rectangle recRightArrow = new Rectangle(770, 310, 120, 120);
            Rectangle recLeftArrow = new Rectangle(110, 310, 120, 120);

            

            //left

            if (scene_number > 0)
            {
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
                            songChanged = true;
                        }
                    }
                }
                else
                {

                    arrawframebutton[0] = false;
                    arrawframe[0] = 0;
                }

            }

            if (scene_number < noteFileManager.noteFiles.Count - 1)
            {
                if (mouseRectangle.Intersects(recRightArrow) || rightHandPosition.Intersects(recRightArrow))
                {



                    Game1.nearButton = true;
                    Game1.GetCenterOfButton(recRightArrow);

                    arrawframebutton[1] = true;

                    if ((mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released) || (key.IsKeyDown(Keys.Right) && !pastkey.IsKeyDown(Keys.Right)) || (Game1.finalClick && !Game1.pastClick))
                    {

                        if (scene_number < noteFileManager.noteFiles.Count - 1)
                        {

                            Game1.nearButton = false;
                            leftrightmove = 1;
                            scene_number++;
                            frame = 0;
                            songChanged = true;


                        }
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
                    songChanged = true;
                   
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
                    songChanged = true;
                
                }

                isKinectRight = false;


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


            if (Game1.soundRecogStartIndex != -1)
            {
                int ret  = Game1.soundRecogStartIndex;
                Game1.soundRecogStartIndex = -1;
                Game1.nearButton = false;


                return ret;

            }

            if (mouseRectangle.Intersects(startrect) || rightHandPosition.Intersects(startrect))
            {

                Game1.nearButton = true;
                Game1.GetCenterOfButton(startrect);
                clickStartButton = true;

               

                if ((Game1.finalClick && !Game1.pastClick) || (mouse.LeftButton == ButtonState.Pressed && pastmouse.LeftButton == ButtonState.Released))
                {
                    Game1.nearButton = false;
                    clickStartButton = false;
                    int ret;
                    //if (Game1.soundRecogStartIndex != -1)
                    //{
                    //    ret = Game1.soundRecogStartIndex;
                    //}

                    ////보통때는 -1 이다가 start가 인식하면 곡의 인덱스 값을 받음
                    //Game1.soundRecogStartIndex = -1;

                    ret = scene_number;
                    return ret;
                }
            }
            else
            {
                clickStartButton = false;
            }


            if (opening)
            {
                bool isPlay = false;
                SoundFmod.sndChannel.isPlaying(ref isPlay);
                if (isPlay)
                {
                    SoundFmod.StopSound();
                }

                if (noteFileManager.noteFiles.Count > 0)
                {
                    SoundFmod.PlaySound(Game1.songsDir + noteFileManager.noteFiles[scene_number].Mp3);
                    // songChanged = false;
                }
                opening = false;
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

            if (noteFileManager.noteFiles.Count > 0)
            {

                spriteBatch.Draw(background, Vector2.Zero, new Color(fadeinout, fadeinout, fadeinout));
                //  spriteBatch.Draw(songBackground, new Vector2(270,100), new Color(fadeinout, fadeinout, fadeinout));

            }
            else
            {

                spriteBatch.Draw(noSongBackground, Vector2.Zero, new Color(fadeinout, fadeinout, fadeinout));

            }//사진

            drawText(spriteBatch);
            
            
            drawArrow(spriteBatch);
       
            //노래가 하나라도 있어야한다. 

            spriteBatch.Draw(Game1.previousButton, new Vector2(backrect.X, backrect.Y), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

            if (clickPreviousButton)
            {

                spriteBatch.Draw(Game1.hoverPreviousButton, new Vector2(backrect.X, backrect.Y), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
            }

            if (noteFileManager.noteFiles.Count > 0)
            {

                spriteBatch.Draw(starttext, new Vector2(startrect.X, startrect.Y), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);

                if (clickStartButton)
                {

                    spriteBatch.Draw(hoverStarttext, new Vector2(startrect.X, startrect.Y), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 1f);
                }
            }
            if (noteFileManager.noteFiles.Count > 0)
            {
                String name = noteFileManager.noteFiles[scene_number].Name;
                String shortName = name;
                if (name.Length > 15)
                {

                    shortName = name.Remove(15);
                    shortName = shortName + "...";
                }
            //    spriteBatch.DrawString(Game1.georgia, name, new Vector2(512, 420), Color.White);
                spriteBatch.DrawString(Game1.georgia, shortName, new Vector2(422, 590), Color.DimGray, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

                String artist = noteFileManager.noteFiles[scene_number].Artist;
                String shortArtist = artist;
                if (artist.Length > 15)
                {

                    shortArtist = artist.Remove(15);
                    shortArtist = shortArtist + "...";
                }
                spriteBatch.DrawString(Game1.georgia, shortArtist, new Vector2(422, 560), Color.DimGray, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);


                int bestScore = reportManager.GetHighestScore(name);
                spriteBatch.DrawString(Game1.georgia, bestScore.ToString(), new Vector2(552, 50), Color.Brown, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);


                //     spriteBatch.Draw(uiBackground, new Vector2(0, 0), Color.White);

                int level = noteFileManager.noteFiles[scene_number].Level;
                //0이하이거나 넘어가지 않게 

                Rectangle rec = new Rectangle(0, 0, level * 26/*하나의 그림의 width*/, 22);
                //하트. gage양 만큼 하트가 나타남.
                spriteBatch.Draw(Game1.levelTexture, new Vector2(380, 630), rec, Color.White);
            }

            if (noteFileManager.noteFiles.Count > 0)
            {

                spriteBatch.DrawString(Game1.georgia, (scene_number + 1).ToString(), currentSongLocation, Color.White);


                spriteBatch.DrawString(Game1.georgia, " / ", new Vector2(currentSongLocation.X + 45, currentSongLocation.Y), Color.White);



                spriteBatch.DrawString(Game1.georgia, noteFileManager.noteFiles.Count.ToString(), new Vector2(currentSongLocation.X + 100, currentSongLocation.Y), Color.White);
            }

            else
            {

                spriteBatch.DrawString(Game1.georgia, "Please Add New Song", new Vector2(280,384), Color.White);



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

                    //이거변함
                    float arrawddiyong1 = 80 * (float)Math.Tan(MathHelper.ToRadians(arrawframe[0]));
                    if (scene_number != 0)
                      {

                        //크기조정
                          spriteBatch.Draw(leftright[0], new Rectangle(200 - (int)arrawddiyong1, 310, 30 + (int)arrawddiyong1, 120), new Color(fadeinout, fadeinout, fadeinout));
                  
                       // spriteBatch.Draw(leftright[0], new Vector2(200 - (int)arrawddiyong1, 310), new Rectangle(0, 0, 30 + (int)arrawddiyong1, 60), new Color(fadeinout, fadeinout, fadeinout), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
                    }
                }
                else
                {
                    if (scene_number != 0)
                    {
                        // spriteBatch.Draw(leftright[0], new Rectangle(170, 310, 60, 60), new Color(fadeinout, fadeinout, fadeinout));
                        spriteBatch.Draw(leftright[0], new Rectangle(110, 310, 120, 120), new Color(fadeinout, fadeinout, fadeinout));
       
                    }
                      
                }
                if (arrawframebutton[1])
                {
                    arrawframe[1] += 4;
                    arrawframe[1] = Math.Min(arrawframe[1], 45);
                    float arrawddiyong2 = 80 * (float)Math.Tan(MathHelper.ToRadians(arrawframe[1]));
                    if (scene_number != noteFileManager.noteFiles.Count - 1)
                    {
                        spriteBatch.Draw(leftright[1], new Rectangle(770, 310, 30 + (int)arrawddiyong2, 120), new Color(fadeinout, fadeinout, fadeinout));
                    }
                }
                else
                {
                  //  if (scene_number != 0 && scene_number != noteFileManager.noteFiles.Count-1)
                    if (noteFileManager.noteFiles.Count > 0 && scene_number != noteFileManager.noteFiles.Count - 1)
                  
                      spriteBatch.Draw(leftright[1], new Rectangle(770, 310, 120, 120), new Color(fadeinout, fadeinout, fadeinout));
                }
            }
        }
        public void drawText(SpriteBatch spriteBatch)
        {
            int size = 430;
            int backgroundSizeWidth = 475;
            int backgroundSizeHeight = 600;
            int backgroundHeight = 100;
            int backgroundWidth = 20;
            int height = 120;
            int width = 40;
            if (leftrightmove == 1)
            {
                frame += 3;
                frame = Math.Min(90, frame);
                margin = 800 - (int)(800 * Math.Sin(MathHelper.ToRadians(frame)));

                //곡 바꿀시 노래재생
                if (frame == 90 && songChanged)
                {
                    bool isPlay = false;
                    SoundFmod.sndChannel.isPlaying(ref isPlay);
                    if (isPlay)
                    {
                        SoundFmod.StopSound();
                    }
                    SoundFmod.PlaySound(Game1.songsDir + noteFileManager.noteFiles[scene_number].Mp3);
                    songChanged = false;
                }
                if (frame == 0)
                    leftrightmove = 0;
            }
            else if (leftrightmove == -1)
            {
                frame += 3;
                frame = Math.Min(90, frame);
                margin = (int)(800 * Math.Sin(MathHelper.ToRadians(frame))) - 800;

                //곡 바꿀시 노래재생
                if (frame == 90 && songChanged)
                {
                    bool isPlay = false;
                    SoundFmod.sndChannel.isPlaying(ref isPlay);
                    if (isPlay)
                    {
                        SoundFmod.StopSound();
                    }
                    SoundFmod.PlaySound(Game1.songsDir + noteFileManager.noteFiles[scene_number].Mp3);
                    songChanged = false;


                }
                if (frame == 0)
                    leftrightmove = 0;
            }

            if (scene_number - 1 >= 0 && leftrightmove == 1)
            {
                if (scene_number - 1 == 0)
                {
                    spriteBatch.Draw(songBackground, new Rectangle(margin - 1024 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));
            
                  //  spriteBatch.Draw(, new Vector2(270, 100), new Color(fadeinout, fadeinout, fadeinout));
                    spriteBatch.Draw(pictures[scene_number - 1], new Rectangle(margin - 1024 + width, height, size, size), new Color(fadeinout, fadeinout, fadeinout));
                }
                else if (scene_number - 1 == 8)
                {
                    spriteBatch.Draw(songBackground, new Rectangle(margin - 1024 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));
              
                    spriteBatch.Draw(pictures[scene_number - 1], new Rectangle(margin - 1024 + width, height, size, size), new Color(fadeinout, fadeinout, fadeinout));
                 }
                else
                {
                    spriteBatch.Draw(songBackground, new Rectangle(margin - 1024 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));

                    spriteBatch.Draw(pictures[scene_number - 1], new Rectangle(margin - 1024 + width, height, size, size), new Color(fadeinout, fadeinout, fadeinout));
                }   
            }
            if (scene_number < noteFileManager.noteFiles.Count)
            {
                if (scene_number == 0)
                {
                    spriteBatch.Draw(songBackground, new Rectangle(margin + 250 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));
             
                    spriteBatch.Draw(pictures[scene_number], new Rectangle(margin + 250 + width, height, size, size), new Color(fadeinout, fadeinout, fadeinout));
                  
                }
                else if (scene_number == noteFileManager.noteFiles.Count)
                {
                    spriteBatch.Draw(songBackground, new Rectangle(margin + 250 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));
            
                    spriteBatch.Draw(pictures[scene_number], new Rectangle(margin + 250 + width, height, size, size), new Color(fadeinout, fadeinout, fadeinout));
                   
                }
                else
                {
                    spriteBatch.Draw(songBackground, new Rectangle(margin + 250 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));
           
                    spriteBatch.Draw(pictures[scene_number], new Rectangle(margin + 250 + width, height, size, size), new Color(fadeinout, fadeinout, fadeinout));
                  
                }
            }
            if (scene_number + 1 < noteFileManager.noteFiles.Count && leftrightmove == -1)
            {
                if (scene_number + 1 == 0)
                {
                    spriteBatch.Draw(songBackground, new Rectangle(margin + 1024 + backgroundWidth, 0, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));
                
                    spriteBatch.Draw(pictures[scene_number + 1], new Rectangle(margin + 1024 + width, 0, size, size), new Color(fadeinout, fadeinout, fadeinout));
                  
                }
                else if (scene_number + 1 == 8)
                {
                    spriteBatch.Draw(songBackground, new Rectangle(margin + 1024 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));
              
                    spriteBatch.Draw(pictures[scene_number + 1], new Rectangle(margin + 1024 + width, height, size, size), new Color(fadeinout, fadeinout, fadeinout));
                   
                }
                else
                {
                    spriteBatch.Draw(songBackground, new Rectangle(margin + 1024 + backgroundWidth, backgroundHeight, backgroundSizeWidth, backgroundSizeHeight), new Color(fadeinout, fadeinout, fadeinout));
                   
                    spriteBatch.Draw(pictures[scene_number + 1], new Rectangle(margin + 1024 + width, height, size, size), new Color(fadeinout, fadeinout, fadeinout));
                   
                
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
