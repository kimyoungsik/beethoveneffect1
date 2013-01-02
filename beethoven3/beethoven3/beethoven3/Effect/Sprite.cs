using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace beethoven3
{
    class Sprite
    {
        #region declarations

        public Texture2D Texture;

        //스프라이트에 정의된 각 애니메이션 프레임의 rectangle 객체
        //텍스쳐를 바꾸면 frams의 rec를 바꿔야 한다.

        protected List<Rectangle> frames = new List<Rectangle>();

        protected int frameWidth = 0;
        protected int frameHeight = 0;
        
        //주어진 시간에 표시되는 프레임을 저장
        private int currentFrame;

        //frametime에 정해놓은 시간동안 표시 (timeforcurrentframe과 비교)
        private float frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;
        //

        //효과
        protected Color tintColor = Color.White;
        protected float rotation;
        //


        //make collinsion
        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;  
        public int BoundingYPadding = 0;

        //find sprite location
        protected Vector2 location = Vector2.Zero;

        //direction and speed
        protected Vector2 velocity = Vector2.Zero;

        protected float scale = 1.0f;
        
        //각 노트의 시작 노트의 위치를 저장해둔다.(0베이스)
        private int startNoteLoation = -1;


        #endregion

        #region constructor
        public Sprite(
            Vector2 location,
            Texture2D texture,
            Rectangle initialFrame,
            Vector2 velocity,
            //기본적으로 scale은 1, 값이 들어가면 그 값을 따른다.
            float scale = 1.0f
             )
        {
            this.location = location;
            Texture = texture;
            this.velocity = velocity;
            this.scale = scale;
            frames.Add(initialFrame);
            frameWidth = initialFrame.Width;
            frameHeight = initialFrame.Height;
        }
        #endregion


        #region properties

        public int StartNoteLoation
        {
            get { return startNoteLoation; }
            set { startNoteLoation = value; }


        }
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        //프레임 지정, 그러나 프레임 안에서 해당하는 수만큼만 가능+
        public int Frame
        {
            get { return currentFrame; }
            set
            {
                currentFrame = (int)MathHelper.Clamp(value, 0,
                frames.Count - 1);
            }
        }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        // currentframe rectangle
        
        public Rectangle Source
        {
            get { return frames[currentFrame]; }
            set { Source = value; }
        }

        // sprite's frame location rectangle
        public Rectangle Destination
        {
            get
            {
                return new Rectangle(
                    (int)location.X,
                    (int)location.Y,
                    frameWidth,
                    frameHeight);
            }
        }

        public Vector2 Center
        {
            get
            {
                return location +
                    new Vector2(frameWidth * scale / 2, frameHeight * scale / 2);
            }
        }

        #endregion

        #region collision

        public Rectangle BoundingBoxRect
        {
            get
            {
                return new Rectangle(
                    (int)location.X + BoundingXPadding,
                    (int)location.Y + BoundingYPadding,
                    frameWidth - (BoundingXPadding * 2),
                    frameHeight - (BoundingYPadding * 2));
            }
        }

        public bool IsBoxColliding(Rectangle OtherBox)
        {
            return BoundingBoxRect.Intersects(OtherBox);
        }

        
        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            //중앙을 이은 선이 나의 충돌거리(반지름)과 다른것의 충돌거리의 합보다 작으면 
            if (Vector2.Distance(Center, otherCenter) <
                (CollisionRadius + otherRadius))
                return true;
            else
                return false;
        }


        //거리 판단
        //오른손 노트와 마커 판단의 경우 : otherCenter => note's center, otherRadius => note's radius
        //otherradius는 안쓰임 일단.
        public int JudgedNote(Vector2 otherCenter)
        {
            //bad
            int ret = 0;

            //반/2 보다 가까울때  , perfect

          //  Trace.WriteLine(Vector2.Distance(Center, otherCenter));
            //마커 센터에서 노트의 센터 사이의 거리가  마커의 radius/2 보다 작을 떄  
            if (Vector2.Distance(Center, otherCenter) <
                (CollisionRadius/2))
            {
                ret = 2;
            }
            //마커 센터에서 노트의 센터 사이의 거리가  마커의 radius 보다 작을 떄  
            //반들어왔을때 . good
            else if (Vector2.Distance(Center, otherCenter) <
                (CollisionRadius))
            {
                ret = 1;
            }
            return ret;
        }

        #endregion

        #region method
        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        //프레임의 각 RECT값을 변경
        public void ChangeFrameRect(Rectangle rect)
        {
            int i;
            for (i = 0; i < frames.Count; i++)
            {
                frames[i] = rect;
            }

            frameHeight = rect.Height;
            frameWidth = rect.Width;
        }

        #endregion

        #region update and draw
        public virtual void Update(GameTime gameTime) 
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

           
            timeForCurrentFrame += (elapsed);
          
            //여기에 얼마나 자주 들어가는것에 따라서 프레임이 빨리 바뀌거나 느리게 바뀐다. 
            //frameTime을 바꾼다.

            if (timeForCurrentFrame >= FrameTime)
            {
                currentFrame = (currentFrame + 1) % (frames.Count);
                timeForCurrentFrame = 0.0f;
            }

            location += (velocity * elapsed);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                //위치: Center-> location 으로 바꿈 (마커와 노트 매칭 떄문에 )
                location,
                Source,
                tintColor,
                rotation,
                //origin ->  new Vector2(frameWidth / 2, frameHeight / 2) ->  new Vector2(0,0) 으로 바꿈 (마커와 노트 매칭 떄문에 )
                new Vector2(0,0),
                scale,
                SpriteEffects.None,
                0.0f);   
        }
        #endregion
    }
}
