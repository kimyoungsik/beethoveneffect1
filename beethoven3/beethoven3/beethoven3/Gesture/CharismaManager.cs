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
        private Queue charismaFrames = new Queue();
       
        //file에서 가져오는 현재 게임의 흐름
        private double currentTime;
       
        private Texture2D charisma1;
        private Texture2D charisma2;

        //카리스마 위치
        private Rectangle picLocation = new Rectangle(100,100,150,150);

        //현재 카리스마 타입.
        private int type;


        private bool isCharismaTime = false;

       
        #endregion

        #region constructor
        public CharismaManager()
        {
        
        
        }

         public void LoadContent(ContentManager cm)
        {
          
            charisma1 = cm.Load<Texture2D>(@"charisma\charisma1");
            charisma2 = cm.Load<Texture2D>(@"charisma\charisma2");
     

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



        public void AddCharismaFrame(double startTime, double endTime, int type, double currentTime)
        {
            Texture2D texture  = GetTexture(type);

            CharisimaFrame charismaFrame = new CharisimaFrame(texture,startTime,endTime);


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
            }

            return texture;

        }
        #endregion

        #region update and draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

          if(charismaFrames.Count > 0)
          {

              currentTime += gameTime.ElapsedGameTime.TotalSeconds;
              CharisimaFrame charismaFrame  = (CharisimaFrame)charismaFrames.Peek();

              if (currentTime > charismaFrame.StartTime)
              {
                  isCharismaTime = true;
                  spriteBatch.Draw(charismaFrame.Texture, picLocation, Color.White);
              }
              if (currentTime > charismaFrame.EndTime)
              {
                  isCharismaTime = false;
                  charismaFrames.Dequeue();  
              }
          }
        }
        #endregion



    }
}
