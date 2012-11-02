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
       
       // private int frameCount;

       // Mark[] Marks = new Mark[6];

        public List<Mark> Marks = new List<Mark>();

        #endregion


        #region constructor

        public MarkManager(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount
            )
        {
            this.texture = texture;
            this.initialFrame = initialFrame;
            this.frameCount = frameCount;

            addMark(new Vector2(100, 100));
            addMark(new Vector2(150, 150));
            addMark(new Vector2(200, 200));
            addMark(new Vector2(250, 250));
            addMark(new Vector2(300, 300));
            addMark(new Vector2(350, 350));
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
