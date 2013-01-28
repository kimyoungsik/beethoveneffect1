using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


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
        private Texture2D background;
        //노트 스피드로서 역할을 제대로 하지 않는다.
        private float noteSpeed;
        private int collisionRadius;
      
        private ScoreManager scoreManager;
        private MissBannerManager missBanner;
        private Vector2 missSize = new Vector2(975.0f, 412.0f);
       // public ItemManager itemManager;
        private int index;
        #endregion


        #region initialize

        public DragNoteManager(
        
            Texture2D texture,
            Texture2D background,
            Rectangle initialFrame,
            int frameCount,
            int collisionRadius,
            float noteSpeed,
            MissBannerManager missBanner,
            ScoreManager scoreManager
         //   ItemManager ItemManager
            )
        {
            this.texture = texture;
            this.background = background;
            this.initialFrame = initialFrame;
            this.frameCount = frameCount;
            this.collisionRadius = collisionRadius;
            this.noteSpeed = noteSpeed;
            this.missBanner = missBanner;
            this.scoreManager = scoreManager;
         //   this.itemManager = itemManager;
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


        

         public int Index
        {
            get { return index; }
            set { index = value; }
        }


        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Texture2D Background
        {
            get { return background; }
            set { background = value; }
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
                   
                }
            }
        }

        public void Delete()
        {
            dragNotes.Clear();

        }
        public void DeleteDragNotes()
        {
            int i;
            for (i = 0; i < dragNotes.Count; i++)
            {
             
                //미스 띄워주기 
                missBanner.AddBanners(new Vector2(1024 / 2 - (missSize.X * 0.2f) / 2, 769 / 2 - (missSize.Y * 0.2f) / 2), 0.2f);
               
                dragNotes.RemoveAt(i);
                
                if (scoreManager.Combo > scoreManager.Max)
                {
                    scoreManager.Max = scoreManager.Combo;
                }
                scoreManager.Combo = 0;
                scoreManager.ComboChanged = true;

                scoreManager.Gage -= 0.5f;
   
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
        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    //foreach (Sprite dragNote in dragNotes)
        //    //{
        //    //    dragNote.Draw(spriteBatch);
        //    //}
        //}

        #endregion
      
    }
}
