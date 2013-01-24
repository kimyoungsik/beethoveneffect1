#define Kinect
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Diagnostics;

namespace beethoven3
{

    class PhotoFrame
    {
        #region declarations
        private Texture2D texture;
        private double startTime;
        private double endTime;

        #endregion

        #region constructor
        public PhotoFrame(double startTime, double endTime)
        {
            //this.texture = texture;
            this.startTime = startTime;
            this.endTime = endTime;
        }




        #endregion

        #region method

        //public Texture2D Texture
        //{
        //    get { return texture; }
        //    set { texture = value; }

        //}

        public double StartTime
        {
            get { return startTime; }
            set { startTime = value; }

        }


        public double EndTime
        {
            get { return endTime; }
            set { endTime = value; }

        }

        #endregion

    }

    class PhotoManager
    {
        #region declarations

        //카리스마 프레임을 가지고 있는 큐
        private Queue photoFrames = new Queue();

        //file에서 가져오는 현재 게임의 흐름
        private TimeSpan currentTime;

    //    private Texture2D cameraTexture;
       
        //카리스마 위치
        private Rectangle picLocation = new Rectangle(100, 100, 150, 150);

     
        //0 false ,가장 초기화 1 false , 현재 실행중 2, true  
        private int isPhotoTime = 0;

        //private bool isJudgeCheck = false;


        #endregion

        #region constructor
        public PhotoManager()
        {


        }

        public void LoadContent(ContentManager cm)
        {

         //   cameraTexture = cm.Load<Texture2D>(@"charisma\charisma1");
           

        }
        #endregion




        #region method



        //public int IsPhotoTime
        //{
        //    get { return isPhotoTime; }
        //    set { isPhotoTime = value; }
        //}

        //public bool IsJudgeCheck
        //{
        //    get { return isJudgeCheck; }
        //    set { isJudgeCheck = value; }
        //}




        public void AddPhotoFrame(double startTime, TimeSpan currentTime)
        {

            PhotoFrame photoFrame = new PhotoFrame( startTime, startTime + 1);

         //   isPhotoTime = 0;
            //isJudgeCheck = false;


            photoFrames.Enqueue(photoFrame);
            this.currentTime = currentTime;
          

        }



        #endregion
        public void Update(GameTime gameTime)
        {
#if Kinect
            if (photoFrames.Count > 0)
            {

              //  currentTime += gameTime.ElapsedGameTime.TotalSeconds;
                PhotoFrame photoFrame = (PhotoFrame)photoFrames.Peek();

                if (currentTime > TimeSpan.FromSeconds(photoFrame.StartTime))
                {

                    Game1.PicFlag = true;
                    photoFrames.Dequeue();

                }

            }
#endif

        }


        #region update and draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        #endregion



    }
}
