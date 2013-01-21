using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;
namespace beethoven3
{
    //충돌로 판별하는 마크르 만드는 것
    class DragNoteManager
    {
        #region declarations
        public List<Sprite> dragNotes = new List<Sprite>();

        private Texture2D texture;
        private Rectangle initialFrame;
        private int frameCount;

        //노트 스피드로서 역할을 제대로 하지 않는다.
        private float noteSpeed;
        private int collisionRadius;
      
        private ScoreManager scoreManager;
        private MissBannerManager missBanner;
        #endregion


        #region initialize

        public DragNoteManager(
        
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            int collisionRadius,
            float noteSpeed,
            MissBannerManager missBanner,
            ScoreManager scoreManager
            )
        {
            this.texture = texture;
            this.initialFrame = initialFrame;
            this.frameCount = frameCount;
            this.collisionRadius = collisionRadius;
            this.noteSpeed = noteSpeed;
            this.missBanner = missBanner;
            this.scoreManager = scoreManager;
        }
        #endregion

        #region method
        public void MakeDragNote(
            Vector2 location,
            Vector2 velocity
            )
        {
            Sprite thisNote = new Sprite(
                location,
                texture,
                initialFrame,
                velocity);

            thisNote.Velocity *= noteSpeed;

            for (int x = 1; x < frameCount; x++)
            {
                thisNote.AddFrame(new Rectangle(
                    initialFrame.X + (initialFrame.Width * x),
                    initialFrame.Y,
                    initialFrame.Width,
                    initialFrame.Height));
            }
            thisNote.CollisionRadius = collisionRadius;
            dragNotes.Add(thisNote);
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Rectangle InitialFrame
        {
            get { return initialFrame; }
            set { initialFrame = value; }
        }

        public void DeleteDragNoteFromFront()
        {
            if (dragNotes.Count > 0)
            {
                try
                {

                    dragNotes.RemoveAt(0);
                }
                catch (ArgumentOutOfRangeException)
                {
                    //Trace.WriteLine("out of range in dragnote");
                }
            }
        }
        public void DeleteDragNotes()
        {
            int i;
            for (i = 0; i < dragNotes.Count; i++)
            {
             
                //미스 띄워주기 
                missBanner.AddBanners(new Vector2(1024 / 2 - 975 / 4, 769 / 2 - 412 / 4));

                dragNotes.RemoveAt(i);
                //***이거 잘못된듯 
                //드래그 노트실패하면 깍아야 할듯  
               // ScoreManager.DragNoteScore = ScoreManager.DragNoteScore + 1;


                if (scoreManager.Combo > scoreManager.Max)
                {
                    scoreManager.Max = scoreManager.Combo;
                }
                scoreManager.Combo = 0;
                scoreManager.Gage = scoreManager.Gage - 1;
   
            }
        }

        #endregion


        #region update and draw
        
        public void Update(GameTime gameTime)
        {
            for (int x = dragNotes.Count - 1; x >= 0; x--)
            {
                dragNotes[x].Update(gameTime);
                
            }
        }


        //도중 마커점이니깐 안보여도 된다. 안쓰임
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite dragNote in dragNotes)
            {
                dragNote.Draw(spriteBatch);
            }
        }

        #endregion
      
    }
}
