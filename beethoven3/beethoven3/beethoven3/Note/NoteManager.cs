using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace beethoven3
{
    //이게 하나의 노트 
    class NoteManager
    {
        #region declarations
        public List<Sprite> LittleNotes = new List<Sprite>();

        private static Texture2D Texture;
        private Rectangle InitialFrame;
        private static int FrameCount;
        public float noteSpeed;
        private static int CollisionRadius;
   
        #endregion


        #region constructor
        public NoteManager(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            int collisionRadius,
            float noteSpeed
        
            )
       {
            Texture = texture;
            InitialFrame = initialFrame;
            FrameCount = frameCount;
            CollisionRadius = collisionRadius;
            this.noteSpeed = noteSpeed;
          //  this.noteType = noteType;
       }

        #endregion

        #region method
        public void MakeNote(
            Vector2 location,
            Vector2 velocity
            )
        {
            Sprite thisNote = new Sprite(
                location,
                Texture,
                InitialFrame,
                velocity);

            thisNote.Velocity *= noteSpeed;

            for (int x = 1; x < FrameCount; x++)
            {
                thisNote.AddFrame(new Rectangle(
                    InitialFrame.X + (InitialFrame.Width * x),
                    InitialFrame.Y,
                    InitialFrame.Width,
                    InitialFrame.Height));
            }
            thisNote.CollisionRadius = CollisionRadius;
            LittleNotes.Add(thisNote);
        }
        #endregion

        #region update and draw



        public void Update(GameTime gameTime)
        {
            for (int x = LittleNotes.Count - 1; x >= 0; x--)
            {
                LittleNotes[x].Update(gameTime);
               //마커를 지난후에 지워야 한다.
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite littleNote in LittleNotes)
            {
                littleNote.Draw(spriteBatch);
            }
        }



        #endregion
    }
}
