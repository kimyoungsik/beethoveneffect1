﻿using System;
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

        private static float distance = 70.0f;
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

        /// <summary>
        /// 마커의 위치를 주면 노트가 시작되는 위치를 리턴  
        /// 중복되는 부분 refactor 필요 
        /// </summary>
        public static Vector2 GetStartNoteLocation(Vector2 center, float distance, int type)
        {
            Vector2 otherCenter = center;
            int x = 70;
            switch (type)
            {
                case 0:

                    bool find0 = false;
                    while (!find0)
                    {
                        otherCenter.Y -= x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.Y < 0)
                        {
                            find0 = true;
                        }
                    }
                    break;

                case 1:

                    bool find1 = false;
                    while (!find1)
                    {
                        otherCenter.X += x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X > 2000)
                        {
                            find1 = true;
                        }

                    }
                    break;
                case 2:

                    bool find2 = false;
                    while (!find2)
                    {
                        otherCenter.X += x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X > 2000)
                        {
                            find2 = true;
                        }
                    }
                    break;
                case 3:

                    bool find3 = false;
                    while (!find3)
                    {
                        otherCenter.Y += x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.Y < 1500)
                        {
                            find3 = true;
                        }
                    }
                    break;
                case 4:
                    bool find4 = false;
                    while (!find4)
                    {
                        otherCenter.X -= x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X < 0)
                        {
                            find4 = true;
                        }

                    }
                    break;
                case 5:

                    bool find5 = false;
                    while (!find5)
                    {
                        otherCenter.X -= x;

                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X < 0)
                        {
                            find5 = true;
                        }

                    }
                    break;
            }
            return otherCenter;

        }
        #endregion


        //public static Vector2 GetStartNoteLocation(Vector2 center, float distance, int type)
        //{
        //    Vector2 otherCenter = center;
        //    int x = 1;
        //    switch (type)
        //    {
        //        case 0:

                    
        //            bool find0 = false;
        //            while (!find0)
        //            {
        //                otherCenter.Y -= x;
        //                if (Vector2.Distance(center, otherCenter) == distance || otherCenter.Y < 0)
        //                {
        //                    find0 = true;
        //                }
                      
        //            }
        //            break;

        //        case 1:
                   
        //            bool find1 = false;
        //            while (!find1)
        //            {
        //                otherCenter.X += x;
        //                otherCenter.Y -= x;
        //                if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X > 2000)
        //                {
        //                    find1 = true;
        //                }
                        
        //            }
        //            break;
        //        case 2:
                   
        //            bool find2 = false;
        //            while (!find2)
        //            {
        //                otherCenter.X += x;
        //                otherCenter.Y += x;
        //                if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X > 2000)
        //                {
        //                    find2 = true;
        //                }
                       
        //            }
        //            break;
        //        case 3:
                   
        //            bool find3 = false;
        //            while (!find3)
        //            {
        //                otherCenter.Y += x;
        //                if (Vector2.Distance(center, otherCenter) == distance || otherCenter.Y > 1500)
        //                {
        //                    find3 = true;
        //                }
                     
        //            }
        //            break;
        //        case 4:
                  
        //            bool find4 = false;
        //            while (!find4)
        //            {
        //                otherCenter.X -= x;
        //                otherCenter.Y += x;
        //                if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X < 0)
        //                {
        //                    find4 = true;
        //                }
                         
        //            }
        //            break;
        //        case 5:

        //            bool find5 = false;
        //            while (!find5)
        //            {
        //                otherCenter.X -= x;
        //                otherCenter.Y -= x;
        //                if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X < 0)
        //                {
        //                    find5 = true;
        //                }
                          
        //            }
        //            break;
        //    }
        //    return otherCenter;

        //}
        
       
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
