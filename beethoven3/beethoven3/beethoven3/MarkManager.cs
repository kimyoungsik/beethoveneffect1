using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class MarkManager
    {
        #region declarations
        private Texture2D texture;
        private Rectangle initialFrame;
        private int frameCount;
        public List<Mark> Marks = new List<Mark>();

        private Vector2 mark1Location;
        private Vector2 mark2Location;
        private Vector2 mark3Location;
        private Vector2 mark4Location;
        private Vector2 mark5Location;
        private Vector2 mark6Location;

        private StartNoteManager startNoteManager;

        private int distance = 100;
        #endregion


        #region constructor

        public MarkManager(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            Vector2 mark1Location,
            Vector2 mark2Location,
            Vector2 mark3Location,
            Vector2 mark4Location,
            Vector2 mark5Location,
            Vector2 mark6Location,
            StartNoteManager startNoteManager
            )
        {
            this.texture = texture;
            this.initialFrame = initialFrame;
            this.frameCount = frameCount;

            this.startNoteManager = startNoteManager;
            
            this.mark1Location=mark1Location;
            this.mark2Location=mark2Location;
            this.mark3Location=mark3Location;
            this.mark4Location=mark4Location;
            this.mark5Location=mark5Location;
            this.mark6Location=mark6Location;



            addMark(mark1Location);
            addMark(mark2Location);
            addMark(mark3Location);
            addMark(mark4Location);
            addMark(mark5Location);
            addMark(mark6Location);
            
            //startNoteManager = new StartNoteManager(texture, initialFrame, frameCount);

            startNoteManager.addStartNote(new Vector2(mark1Location.X, mark1Location.Y - distance));
            startNoteManager.addStartNote(new Vector2(mark2Location.X + distance, mark2Location.Y - distance));
            startNoteManager.addStartNote(new Vector2(mark3Location.X + distance, mark3Location.Y + distance));
            startNoteManager.addStartNote(new Vector2(mark4Location.X, mark4Location.Y + distance));
            startNoteManager.addStartNote(new Vector2(mark5Location.X - distance, mark5Location.Y + distance));
            startNoteManager.addStartNote(new Vector2(mark6Location.X - distance, mark6Location.Y - distance));

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
        public void addMark(Vector2 location)
        {
            Mark thisMark = new Mark(
                texture,
                location,
                initialFrame,
                frameCount);
            Marks.Add(thisMark);
        }
        public void deleteAllMarks()
        {
           // for (int i = 0; i < 6; i++)
            for (int i = 0; i < Marks.Count; i++)
            {
                Marks.RemoveAt(i);
            }
        }
        #endregion
       
        #region update and draw
        public void Update(GameTime gameTime)
        {
      
            foreach (Mark mark in Marks)
            {
            
                mark.Update(gameTime);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (Mark mark in Marks)
            {
                mark.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
