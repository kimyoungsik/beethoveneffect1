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

    class CharisimaFrame
   
    {  
        #region declarations
        private Texture2D texture;
        private double startTime;
        private double endTime;

        #endregion

        #region constructor
        public CharisimaFrame(Texture2D texture, double startTime,double endTime )
        {
            this.texture = texture;
            this.startTime = startTime;
            this.endTime = endTime;
        }

      


        #endregion

        #region method

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }

        }

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

    class CharismaManager
    {
        #region declarations
    
        //카리스마 프레임을 가지고 있는 큐
        public Queue charismaFrames;
       
        //file에서 가져오는 현재 게임의 흐름
        public double currentTime;
       
        private Texture2D charisma1;
        private Texture2D charisma2;
        private Texture2D charisma3;
        private Texture2D charisma4;
        private Texture2D charisma5;
        private Texture2D charismaMessage;
        //카리스마 위치
        public Rectangle picLocation = new Rectangle(0, 0, 1024, 769);

        //현재 카리스마 타입.
        private int type;

        //0 false ,가장 초기화 1 false , 현재 실행중 2, true  
        private bool isCharismaTime = false;

        //private bool isJudgeCheck = false;

        //카리스마 중에 게이지 감소 안되게 하기 위해서 
        private bool playCharisma = false;
       
        #endregion

        #region constructor
        public CharismaManager()
        {
            charismaFrames = new Queue();
        
        }

         public void LoadContent(ContentManager cm)
        {
            charismaMessage = cm.Load<Texture2D>(@"charisma\Sitt_Message");
            charisma1 = cm.Load<Texture2D>(@"charisma\Sitt_1");
            charisma2 = cm.Load<Texture2D>(@"charisma\Sitt_2");
            charisma3 = cm.Load<Texture2D>(@"charisma\Sitt_3");
            charisma4 = cm.Load<Texture2D>(@"charisma\Sitt_4");
            charisma5 = cm.Load<Texture2D>(@"charisma\Sitt_1");

        }
        #endregion
        


        
        #region method

        public int Type
        {
            get { return type; }
            set { type = value; }
        }


        public bool IsCharismaTime
        {
            get { return isCharismaTime; }
            set { isCharismaTime = value; }
        }


        public bool PlayCharisma
        {
            get { return playCharisma; }
            set { playCharisma = value; }
        }

        //public bool IsJudgeCheck
        //{
        //    get { return isJudgeCheck; }
        //    set { isJudgeCheck = value; }
        //}




        public void AddCharismaFrame(double startTime, double endTime, int type, double currentTime)
        {
            Texture2D texture  = GetTexture(type);

            CharisimaFrame charismaFrame = new CharisimaFrame(texture,startTime,endTime);

            isCharismaTime = true;
            //isJudgeCheck = false;


            charismaFrames.Enqueue(charismaFrame);
            this.currentTime = currentTime;
            this.type = type;

        }



        public Texture2D GetTexture(int type)
        {
            Texture2D texture = null;

            switch(type)
            {
                case 1:
                    texture = charisma1;
                    break;

                case 2:
                     texture = charisma2;
                    break;
                case 3:
                    texture = charisma3;
                    break;

                case 4:
                    texture = charisma4;
                    break;
                case 5:
                    texture = charisma5;
                    break;

          
            }

            return texture;

        }
        #endregion




        #region update and draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {



#if Kinect
          if(charismaFrames.Count > 0)
          {

              currentTime += gameTime.ElapsedGameTime.TotalSeconds;
              CharisimaFrame charismaFrame  = (CharisimaFrame)charismaFrames.Peek();

              if (currentTime >= charismaFrame.StartTime )
              {
                  spriteBatch.Draw(charismaFrame.Texture, picLocation, Color.White);
                  spriteBatch.Draw(charismaMessage, new Rectangle(0, 0, 1024, 769), Color.White);

              }
              //if (currentTime >= charismaFrame.EndTime)
              //{
              //    charismaFrames.Dequeue();
              //    Game1.isGesture = false;
              //    PlayCharisma = false;
              //}
          }
#endif
        }
  
        #endregion



    }
}
