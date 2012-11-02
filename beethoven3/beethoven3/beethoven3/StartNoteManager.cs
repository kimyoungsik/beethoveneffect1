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
       // public List<Mark> Marks = new List<Mark>();

        public List<StartNote> StartNotes = new List<StartNote>();

        private MarkManager markManager;
        public NoteManager noteManager;
        #endregion


        #region constructor

        public StartNoteManager(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            MarkManager markManager
            )
        {
            this.texture = texture;
            this.initialFrame = initialFrame;
            this.frameCount = frameCount;
            this.markManager = markManager;


            noteManager = new NoteManager(
                //노크와 시작마커가 같은 sprite
                texture,
                new Rectangle(0, 300, 5, 5),
                4,
                2,
                50f,
                //notetype
                0);

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
        #endregion

        #region update and draw
        public void Update(GameTime gameTime)
        {
            noteManager.Update(gameTime);
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
            foreach (StartNote startNote in StartNotes)
            {
                startNote.Draw(spriteBatch);
            }
        }
        #endregion

    }
}
