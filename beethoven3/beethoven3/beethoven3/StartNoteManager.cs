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

            foreach (StartNote startNote in StartNotes)
            {

                startNote.Update(gameTime);
            }

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
