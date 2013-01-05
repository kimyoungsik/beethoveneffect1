using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace beethoven3
{

    class StartNoteManager
    {
        
        #region declarations
        
        private Texture2D texture;
        private Rectangle initialFrame;
        
        private int frameCount;
        
        public List<StartNote> StartNotes = new List<StartNote>();
        
        public static NoteManager rightNoteManager;
        public static NoteManager leftNoteManager;
        public static NoteManager longNoteManager;
        //BPS에 따라 달라진다.
        public static float noteSpeed = 70.0f;
        #endregion
         

        #region constructor

        //각 노트를 만든다. 노트의 텍스쳐는 아래 texture
        public StartNoteManager(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount
         
            )
        {
            this.texture = texture;
            this.initialFrame = initialFrame;
            this.frameCount = frameCount;
       

            leftNoteManager = new NoteManager(
                //노크와 시작마커가 같은 sprite
               texture,
               new Rectangle(0, 200, 50, 50),
               1,
               /*반지름 for 충돌*/
               initialFrame.Width/2,
               //notespeed 영향주는것 1
               noteSpeed
                //notetype
               );

            rightNoteManager = new NoteManager(
                //노크와 시작마커가 같은 sprite
                texture,
                 new Rectangle(0, 200, 50, 50),
                1,
                initialFrame.Width / 2,
                noteSpeed
                //notetype
                );


            //doublenote
            longNoteManager = new NoteManager(
                //노크와 시작마커가 같은 sprite
                 texture,
                  new Rectangle(0, 0, 50, 50),
                 1,
                initialFrame.Width / 2,
                 noteSpeed
                //notetype
                 );

        }
        #endregion
        
        #region properties
        public Rectangle InitialFrame
        {
            get { return initialFrame; }
            set { initialFrame = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public int FrameCount
        {
            get { return frameCount; }
            set { frameCount = value; }
        }

        #endregion

        #region method




        //각 노트의 모양을 바꾸다.
        //노래 선택하고 시작하기전에 설정한다.
        public void changeRightNoteImage(Texture2D texture, Rectangle rect,float scale)
        {
            rightNoteManager.TextureChange = texture;
            rightNoteManager.InitFrameChange = rect;
            rightNoteManager.ScaleChange = scale;
        }



        public void changeLeftNoteImage(Texture2D texture, Rectangle rect, float scale)
        {
            leftNoteManager.TextureChange = texture;
            leftNoteManager.InitFrameChange = rect;
            leftNoteManager.ScaleChange = scale;
        }


        public void changeLongNoteImage(Texture2D texture, Rectangle rect, float scale)
        {
            longNoteManager.TextureChange = texture;
            longNoteManager.InitFrameChange = rect;
            longNoteManager.ScaleChange = scale;
        }

       



        public void addStartNote(Vector2 location)
        {
            StartNote thisStartNote = new StartNote(
                texture,
                location,
                initialFrame,
                frameCount);
            StartNotes.Add(thisStartNote);
        }
        public void deleteAllMarks()
        {
            // for (int i = 0; i < 6; i++)
            for (int i = 0; i < 6; i++)
            {
                StartNotes.RemoveAt(0);
            }
        }
        
        //오른손 노트
        //매개변수로 들어오는 markNumber가 (1베이스) 시작 노트의 위치이다. 

        //반환 : 노트의 객체
        public RightNoteInfo MakeRightNote(int markNumber)
        {
            
            Vector2 location = StartNotes[markNumber-1].StartNoteSprite.Location;

            Vector2 direction =
                            MarkManager.Marks[markNumber-1].MarkSprite.Location -
                            location;
            //속도 1로 맞추기 
            direction.Normalize();

            RightNoteInfo rightNoteInfo = rightNoteManager.MakeNote(location, direction,/*시작 노트의 위치*/markNumber);

            return rightNoteInfo;

        }

        //왼손노트
        public void MakeLeftNote(int markNumber)
        {
            //노트시작점의 위치
            Vector2 location = StartNotes[markNumber-1].StartNoteSprite.Location;

            //노트시작점에서 마크의 방향
            Vector2 direction =
                            MarkManager.Marks[markNumber-1].MarkSprite.Location -
                            location;
            direction.Normalize();
            leftNoteManager.MakeNote(location, direction,/*시작 노트의 위치*/markNumber);
        }

        //public void MakeDoubleNote(int markNumber)
        //{
        //    //노트시작점의 위치
        //    Vector2 location = StartNotes[markNumber].StartNoteSprite.Location;

        //    //노트시작점에서 마크의 방향
        //    Vector2 direction =
        //                    MarkManager.Marks[markNumber].MarkSprite.Location -
        //                    location;
        //    direction.Normalize();
        //    doubleNoteManager.MakeNote(location, direction);
        //}

        public void MakeLongNote(int markNumber)
        {
            //노트시작점의 위치
            Vector2 location = StartNotes[markNumber-1].StartNoteSprite.Location;

            //노트시작점에서 마크의 방향
            Vector2 direction =
                            MarkManager.Marks[markNumber-1].MarkSprite.Location -
                            location;
            direction.Normalize();
            longNoteManager.MakeNote(location, direction,/*시작 노트의 위치*/markNumber);
        }
        

        #endregion

        #region update and draw
        public void Update(GameTime gameTime)
        {
            rightNoteManager.Update(gameTime);
            leftNoteManager.Update(gameTime);
         //   doubleNoteManager.Update(gameTime);
            longNoteManager.Update(gameTime);
            foreach (StartNote startNote in StartNotes)
            {
                startNote.Update(gameTime);
            }
            //마커가 변환할 때는 노트가 나오지 않도록 bool active쓰는것도 괜찮을것 같음
            //각 startnote에서 마커로 노트를 발사
            //신호를 줄때마다, 노트 타입 별로
            
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            rightNoteManager.Draw(spriteBatch);
            leftNoteManager.Draw(spriteBatch);
         //   doubleNoteManager.Draw(spriteBatch);
            longNoteManager.Draw(spriteBatch);
            //스타트 표시점 보이지 않게 하려면 주석을 달아야 한다.
            foreach (StartNote startNote in StartNotes)
            {
                startNote.Draw(spriteBatch);
            }
        }
        #endregion

    }
}
