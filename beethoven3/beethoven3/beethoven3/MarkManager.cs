using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    static class MarkManager
    {
        #region declarations
        private static Texture2D markTexture;
        private static Rectangle markInitialFrame;
        private static int markFrameCount;
        public static List<Mark> Marks = new List<Mark>();

        //쓸모 없을 수 도 있음
        private static Vector2 mark0Location;
        private static Vector2 mark1Location;
        private static Vector2 mark2Location;
        private static Vector2 mark3Location;
        private static Vector2 mark4Location;
        private static Vector2 mark5Location;
        //

        private static StartNoteManager startNoteManager;

        private static float distance = 10.0f;
        #endregion


        #region initialization

        public static void initialize(
            Texture2D texture,
            Rectangle initialFrame,
            int frameCount,
            Vector2 mark0Loc,
            Vector2 mark1Loc,
            Vector2 mark2Loc,
            Vector2 mark3Loc,
            Vector2 mark4Loc,
            Vector2 mark5Loc,
            StartNoteManager startNoteMana
            )
        {
            markTexture = texture;
            markInitialFrame = initialFrame;
            markFrameCount = frameCount;

            startNoteManager = startNoteMana;
            
            mark0Location=mark0Loc;
            mark1Location=mark1Loc;
            mark2Location=mark2Loc;
            mark3Location=mark3Loc;
            mark4Location=mark4Loc;
            mark5Location=mark5Loc;



            addMark(mark0Loc);
            addMark(mark1Loc);
            addMark(mark2Loc);
            addMark(mark3Loc);
            addMark(mark4Loc);
            addMark(mark5Loc);
            
            //startNoteManager = new StartNoteManager(texture, initialFrame, frameCount);
            //Vector2.Distance(Center, otherCenter)

            //startNoteManager.addStartNote(new Vector2(mark0Loc.X, mark0Loc.Y - distance));
            //startNoteManager.addStartNote(new Vector2(mark1Loc.X + distance, mark1Loc.Y - distance));
            //startNoteManager.addStartNote(new Vector2(mark2Loc.X + distance, mark2Loc.Y + distance));
            //startNoteManager.addStartNote(new Vector2(mark3Loc.X, mark3Loc.Y + distance));
            //startNoteManager.addStartNote(new Vector2(mark4Loc.X - distance, mark4Loc.Y + distance));
            //startNoteManager.addStartNote(new Vector2(mark5Loc.X - distance, mark5Loc.Y - distance));

            startNoteManager.addStartNote(GetStartNoteLocation(mark0Loc, distance, 0));
            startNoteManager.addStartNote(GetStartNoteLocation(mark1Loc, distance, 1));
            startNoteManager.addStartNote(GetStartNoteLocation(mark2Loc, distance, 2));
            startNoteManager.addStartNote(GetStartNoteLocation(mark3Loc, distance, 3));
            startNoteManager.addStartNote(GetStartNoteLocation(mark4Loc, distance, 4));
            startNoteManager.addStartNote(GetStartNoteLocation(mark5Loc, distance, 5));
        }

        #endregion

        
        
        #region method
        public static void addMark(Vector2 location)
        {
            Mark thisMark = new Mark(
                markTexture,
                location,
                markInitialFrame,
                markFrameCount);
            Marks.Add(thisMark);
        }
        public static void deleteAllMarks()
        {
           // for (int i = 0; i < 6; i++)
            for (int i = 0; i < Marks.Count; i++)
            {
                Marks.RemoveAt(i);
            }
        }


        public static Vector2 GetStartNoteLocation(Vector2 center, float distance, int type)
        {
            Vector2 otherCenter = center;
         
            switch (type)
            {
                case 0:

                    int y0 = 0;
                    bool find0 = false;
                    while (!find0)
                    {
                        otherCenter.Y -= y0;
                        if(Vector2.Distance(center,otherCenter) == distance || otherCenter.Y < 0)
                        {

                            find0 = true;
                        }

                        y0++;   
                    }
                    

                    break;

                case 1:
                    int x1 = 0;
                    int y1 = 0;
                    bool find1 = false;
                    while (!find1)
                    {
                        otherCenter.X += x1;
                        otherCenter.Y -= y1;
                        if(Vector2.Distance(center,otherCenter) == distance || otherCenter.Y < 0)
                        {

                            find1 = true;
                        }
                        x1++;
                        y1++;   
                    }
                    
                    break;

                case 2:
                    int x2 = 0;
                    int y2 = 0;
                    bool find2 = false;
                    while (!find2)
                    {
                        otherCenter.X += x2;
                        otherCenter.Y += y2;
                        if(Vector2.Distance(center,otherCenter) == distance || otherCenter.Y < 1500)
                        {

                            find1 = true;
                        }
                        x2++;
                        y2++;   
                    }
                    
                   
                    break;


                case 3:
                      int x3 = 0;
                    int y3 = 0;
                    bool find3 = false;
                    while (!find3)
                    {
                        
                        otherCenter.Y += y3;
                        if(Vector2.Distance(center,otherCenter) == distance || otherCenter.Y < 1500)
                        {

                            find3 = true;
                        }
                        x3++;
                        y3++;   
                    }
                  

                    break;


                case 4:
                      int x4 = 0;
                    int y4 = 0;
                    bool find4 = false;
                    while (!find4)
                    {
                        otherCenter.X -= x4;
                        otherCenter.Y += y4;
                        if(Vector2.Distance(center,otherCenter) == distance || otherCenter.X < 0)
                        {

                            find4 = true;
                        }
                        x4++;
                        y4++;   
                    }
                    
                    
                    break;


                case 5:
                        int x5 = 0;
                    int y5 = 0;
                    bool find5 = false;
                    while (!find5)
                    {
                        otherCenter.X -= x5;
                        otherCenter.Y -= y5;
                        if(Vector2.Distance(center,otherCenter) == distance || otherCenter.Y < 0)
                        {

                            find4 = true;
                        }
                        x5++;
                        y5++;   
                    }
                    break;


                  

            }

            return otherCenter;
            
        }

        #endregion
       
        #region update and draw
        public static void Update(GameTime gameTime)
        {
      
            foreach (Mark mark in Marks)
            {
            
                mark.Update(gameTime);
            }

        }

        public static void Draw(SpriteBatch spriteBatch)
        {

            foreach (Mark mark in Marks)
            {
                mark.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
