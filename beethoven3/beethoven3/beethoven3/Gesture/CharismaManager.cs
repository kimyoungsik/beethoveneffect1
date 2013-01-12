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
        // public static List<Vector2> points = new List<Vector2>();
        
        
        private Queue charismaFrames = new Queue();
       //기준시간
        private double time = 0.0f;
        //경과 시간
        private double changedTime = 0.0f;
        private double currentTime;
       
        private Texture2D charisma1;
        private Texture2D charisma2;

        private Rectangle picLocation = new Rectangle(100,100,150,150);

        private bool end = false;

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
        public void AddCharismaFrame(double startTime, double endTime, int type, double currentTime)
        {
            Texture2D texture  = GetTexture(type);

            CharisimaFrame charismaFrame = new CharisimaFrame(texture,startTime,endTime);


            charismaFrames.Enqueue(charismaFrame);
            this.currentTime = currentTime;

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

         //   Trace.WriteLine(charismaFrames.Count);
          if(charismaFrames.Count > 0)

          {

        //      changedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
              currentTime += gameTime.ElapsedGameTime.TotalSeconds;
              CharisimaFrame charismaFrame  = (CharisimaFrame)charismaFrames.Peek();

              if (currentTime > charismaFrame.StartTime)
              {   
                  spriteBatch.Draw(charismaFrame.Texture, picLocation, Color.White);
              }
              if (currentTime > charismaFrame.EndTime)
              { 
                  
                  charismaFrames.Dequeue();
                  
              }

          }

              
        }
        #endregion



    }
}
