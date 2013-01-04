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
        private float Scale = 1.0f;
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


        #region properties

        
        public Texture2D TextureChange
        {
            get { return Texture; }
            set { Texture = value; }
        }
        public Rectangle InitFrameChange
        {
            get { return InitialFrame; }
            set { InitialFrame = value; }

        }

        public float ScaleChange
        {
            get { return Scale; }
            set { Scale = value; }

        }
 


        #endregion


        #region method
        ////노트의 littlenotes 의 texture, rect를 전부 변경 
        //public void ChangeImage(Texture2D texture, Rectangle rect)
        //{
        //    int i;
        //    for (i = 0; i < LittleNotes.Count; i++)
        //    {
        //        LittleNotes[i].Texture = texture;
        //        LittleNotes[i].ChangeFrameRect(rect);

        //    }
        //}

        //반환 : 각 노트의 객체

        public RightNoteInfo MakeNote(
            Vector2 location,
            Vector2 velocity,
            /*시작노트의 위치(1베이스)*/
            int markLocation
            )
        {
            Sprite thisNote = new Sprite(
                location,
                Texture,
                InitialFrame,
                velocity,
                this.Scale);

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

            /*시작 노트의 위치를 저장*/
            thisNote.StartNoteLoation = markLocation - 1;
            LittleNotes.Add(thisNote);

            //객체와 index를 넘김
            RightNoteInfo rightNoteInfo = new RightNoteInfo(thisNote, LittleNotes.Count - 1);
            return rightNoteInfo;
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
