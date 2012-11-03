using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//for test
using Microsoft.Xna.Framework.Input;
//

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
        
        #endregion


        #region constructor

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
               15,
               3f,
                //notetype
               0);

            rightNoteManager = new NoteManager(
                //노크와 시작마커가 같은 sprite
                texture,
                 new Rectangle(0, 300, 5, 5),
                1,
                15,
                1f,
                //notetype
                1);
            //longnote


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
            for (int i = 0; i < StartNotes.Count; i++)
            {
                StartNotes.RemoveAt(i);
            }
        }

        //테스트
        private void HandleKeyboardInput(KeyboardState keyState)
        {
        

            if (keyState.IsKeyDown(Keys.NumPad0))
            {
                
                MakeRightNote(0);
            }

            if (keyState.IsKeyDown(Keys.NumPad1))
            {
                MakeRightNote(1);
            }

            if (keyState.IsKeyDown(Keys.NumPad2))
            {
                MakeLeftNote(2);
            }
        }

        //오른손 노트
        private void MakeRightNote(int markNumber)
        {
            Vector2 location = StartNotes[markNumber].StartNoteSprite.Center;

            Vector2 direction =
                            MarkManager.Marks[markNumber].MarkSprite.Center -
                            location;
            //속도 1로 맞추기 
            //direction.Normalize();
            rightNoteManager.MakeNote(location, direction);
        }

        //왼손노트
        private void MakeLeftNote(int markNumber)
        {
            Vector2 location = StartNotes[markNumber].StartNoteSprite.Center;

            Vector2 direction =
                            MarkManager.Marks[markNumber].MarkSprite.Center -
                            location;

            leftNoteManager.MakeNote(location, direction);
        }
        #endregion

        #region update and draw
        public void Update(GameTime gameTime)
        {
            rightNoteManager.Update(gameTime);
            leftNoteManager.Update(gameTime);
            foreach (StartNote startNote in StartNotes)
            {
                startNote.Update(gameTime);
            }
            HandleKeyboardInput(Keyboard.GetState());
            //마커가 변환할 때는 노트가 나오지 않도록 bool active쓰는것도 괜찮을것 같음
            //각 startnote에서 마커로 노트를 발사
            //신호를 줄때마다, 노트 타입 별로
            
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            rightNoteManager.Draw(spriteBatch);
            leftNoteManager.Draw(spriteBatch);
            foreach (StartNote startNote in StartNotes)
            {
                startNote.Draw(spriteBatch);
            }
        }
        #endregion

    }
}
