using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace beethoven3
{
    //이게 하나의 노트 
    class BackJestureManager
    {
        #region declarations
        public Sprite juestureMark;

        private Texture2D Texture;
        private Rectangle InitialFrame;
        private int FrameCount;
        private int CollisionRadius =15;
        private float Scale = 1.0f;

        private bool showJestureMark ;

        #endregion


        #region constructor
        public BackJestureManager(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,

            //int collisionRadius,
            //float noteSpeed
            Vector2 location
            )
        {
            Texture = texture;
            InitialFrame = initialFrame;
            FrameCount = frameCount;
            //CollisionRadius = collisionRadius;
            //this.noteSpeed = noteSpeed;

            //  this.noteType = noteType;

            juestureMark = new Sprite(
               location,
               Texture,
               InitialFrame,
               Vector2.Zero
               );

            for (int x = 1; x < FrameCount; x++)
            {
                juestureMark.AddFrame(new Rectangle(
                    InitialFrame.X + (InitialFrame.Width * x),
                    InitialFrame.Y,
                    InitialFrame.Width,
                    InitialFrame.Height));
            }
            juestureMark.CollisionRadius = CollisionRadius;

            showJestureMark = false;

        }

        #endregion



        #region method
        public bool ShowJestureMark
        {
            get { return showJestureMark; }
            set { showJestureMark = value; }
        }


        public void Delete()
        {
            //juestureMark.
            //BackJestureManager.
            //LittleNotes.Clear();
            //int i;
            //for (i = 0; i < LittleNotes.Count; i++)
            //{
            //    LittleNotes[i].IsShow = false;
            //    LittleNotes.RemoveAt(i);
            //}



        }

        public void MakeJuesture(
            
            )
        {
            //juestureMark = new Sprite(
            //    location,
            //    Texture,
            //    InitialFrame,
            //    Vector2.Zero
            //    );
           
            //for (int x = 1; x < FrameCount; x++)
            //{
            //    juestureMark.AddFrame(new Rectangle(
            //        InitialFrame.X + (InitialFrame.Width * x),
            //        InitialFrame.Y,
            //        InitialFrame.Width,
            //        InitialFrame.Height));
            //}
            //juestureMark.CollisionRadius = CollisionRadius;

        
            //객체와 index를 넘김
            //    RightNoteInfo rightNoteInfo = new RightNoteInfo(thisNote, LittleNotes.Count - 1);
            //  return rightNoteInfo;
        }
        #endregion


        #region update and draw
        public void Update(GameTime gameTime)
        {
            if (showJestureMark)
            {
                juestureMark.Update(gameTime);
            }
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            if (showJestureMark)
            {
                juestureMark.Draw(spriteBatch);
            }

        }
        #endregion
    }
}
