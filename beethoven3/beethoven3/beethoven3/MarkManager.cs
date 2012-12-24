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
        private static float scale =1.0f;
        //가운데 빈공간 (노트들이 없어질 부분)
        public static Rectangle centerArea;


        //쓸모 없을 수 도 있음
        //public static Vector2 mark0Location;
        //public static Vector2 mark1Location;
        //public static Vector2 mark2Location;
        //public static Vector2 mark3Location;
        //public static Vector2 mark4Location;
        //public static Vector2 mark5Location;
        
        private static StartNoteManager startNoteManager;

        //마커와 노트시작 사이의 거리
        public static float distance = 70.0f;
        #endregion
         
        #region initialization
        //70 이라면. 속도 곱하기 시간은 거리
        // 시간은 = 거리/속도 
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
            StartNoteManager startNoteMana,
            Rectangle area
            )
        {
            markTexture = texture;
            markInitialFrame = initialFrame;
            markFrameCount = frameCount;
            centerArea = area;

            startNoteManager = startNoteMana;
            
            //mark0Location=mark0Loc;
            //mark1Location=mark1Loc;
            //mark2Location=mark2Loc;
            //mark3Location=mark3Loc;
            //mark4Location=mark4Loc;
            //mark5Location=mark5Loc;

            addMark(mark0Loc);
            addMark(mark1Loc);
            addMark(mark2Loc);
            addMark(mark3Loc);
            addMark(mark4Loc);
            addMark(mark5Loc); 
       
            startNoteManager.addStartNote(GetStartNoteLocation(mark0Loc, distance, 0));
            startNoteManager.addStartNote(GetStartNoteLocation(mark1Loc, distance, 1));
            startNoteManager.addStartNote(GetStartNoteLocation(mark2Loc, distance, 2));
            startNoteManager.addStartNote(GetStartNoteLocation(mark3Loc, distance, 3));
            startNoteManager.addStartNote(GetStartNoteLocation(mark4Loc, distance, 4));
            startNoteManager.addStartNote(GetStartNoteLocation(mark5Loc, distance, 5));
        }
        #endregion

        #region method
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
               // System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }
        public static void addMark(Vector2 location)
        {
            Mark thisMark = new Mark(
                markTexture,
                location,
                markInitialFrame,
                markFrameCount,
                scale);
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

        public static void changeMarkPattern(int number)
        {
         //   int i;
            Vector2 mark0Loc = Vector2.Zero;
            Vector2 mark1Loc = Vector2.Zero;
            Vector2 mark2Loc = Vector2.Zero;
            Vector2 mark3Loc = Vector2.Zero;
            Vector2 mark4Loc = Vector2.Zero;
            Vector2 mark5Loc = Vector2.Zero;
          
            //for (i = 0; i < startNoteManager.StartNotes.Count; i++)
            //{
            //    startNoteManager.StartNotes.RemoveAt(i);
            //}


                //배열로 리펙토링

            if (number == 0)
            {
              

                    Marks[0].MarkSprite.Location = new Vector2(500, 170);
                    Marks[1].MarkSprite.Location = new Vector2(600, 270);
                    Marks[2].MarkSprite.Location = new Vector2(600, 370);
                    Marks[3].MarkSprite.Location = new Vector2(500, 470);
                    Marks[4].MarkSprite.Location = new Vector2(400, 370);
                    Marks[5].MarkSprite.Location = new Vector2(400, 270);

                    startNoteManager.StartNotes[0].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(500, 170), distance, 0);
                    startNoteManager.StartNotes[1].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(600, 270), distance, 1);
                    startNoteManager.StartNotes[2].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(600, 370), distance, 2);
                    startNoteManager.StartNotes[3].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(500, 470), distance, 3);
                    startNoteManager.StartNotes[4].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(400, 370), distance, 4);
                    startNoteManager.StartNotes[5].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(400, 270), distance, 5);

            }

            else if (number == 1)
            {


                Marks[0].MarkSprite.Location = new Vector2(300, 270);
                Marks[1].MarkSprite.Location = new Vector2(400, 370);
                Marks[2].MarkSprite.Location = new Vector2(400, 470);
                Marks[3].MarkSprite.Location = new Vector2(300, 570);
                Marks[4].MarkSprite.Location = new Vector2(200, 470);
                Marks[5].MarkSprite.Location = new Vector2(100, 370);

                startNoteManager.StartNotes[0].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(300, 270), distance, 0);
                startNoteManager.StartNotes[1].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(400, 370), distance, 1);
                startNoteManager.StartNotes[2].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(400, 470), distance, 2);
                startNoteManager.StartNotes[3].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(300, 570), distance, 3);
                startNoteManager.StartNotes[4].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(200, 470), distance, 4);
                startNoteManager.StartNotes[5].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(100, 370), distance, 5);

            }
            else if (number == 2)
            {

        

            }
            //mark0Location = mark0Loc;
            //mark1Location = mark1Loc;
            //mark2Location = mark2Loc;
            //mark3Location = mark3Loc;
            //mark4Location = mark4Loc;
            //mark5Location = mark5Loc;

            //addMark(mark0Loc);
            //addMark(mark1Loc);
            //addMark(mark2Loc);
            //addMark(mark3Loc);
            //addMark(mark4Loc);
            //addMark(mark5Loc);

            //startNoteManager.addStartNote(GetStartNoteLocation(Marks[0].MarkSprite.Location, distance, 0));
            //startNoteManager.addStartNote(GetStartNoteLocation(Marks[1].MarkSprite.Location, distance, 1));
            //startNoteManager.addStartNote(GetStartNoteLocation(Marks[2].MarkSprite.Location, distance, 2));
            //startNoteManager.addStartNote(GetStartNoteLocation(Marks[3].MarkSprite.Location, distance, 3));
            //startNoteManager.addStartNote(GetStartNoteLocation(Marks[4].MarkSprite.Location, distance, 4));
            //startNoteManager.addStartNote(GetStartNoteLocation(Marks[5].MarkSprite.Location, distance, 5));
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

