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
        public static float distance = 80.0f;
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
            
            addMark(mark0Loc);
            addMark(mark1Loc);
            addMark(mark2Loc);
            addMark(mark3Loc);
            addMark(mark4Loc);
            addMark(mark5Loc); 
       

            //GetStartNoteLocatin => 마커 위치와 거리를 주면 그에 따라서 스타트 노트 위치를 반환한다.
            // 그 값을 가지고 스타트 노트를 만든다.
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


        //public static void changeMarkPattern(int number)
        //{
        //    Vector2 mark0Loc = Vector2.Zero;
        //    Vector2 mark1Loc = Vector2.Zero;
        //    Vector2 mark2Loc = Vector2.Zero;
        //    Vector2 mark3Loc = Vector2.Zero;
        //    Vector2 mark4Loc = Vector2.Zero;
        //    Vector2 mark5Loc = Vector2.Zero;
        
        //    //배열로 리펙토링

        //    if (number == 0)
        //    {
              
        //            Marks[0].MarkSprite.Location = new Vector2(500, 170);
        //            Marks[1].MarkSprite.Location = new Vector2(600, 270);
        //            Marks[2].MarkSprite.Location = new Vector2(600, 370);
        //            Marks[3].MarkSprite.Location = new Vector2(500, 470);
        //            Marks[4].MarkSprite.Location = new Vector2(400, 370);
        //            Marks[5].MarkSprite.Location = new Vector2(400, 270);

        //            startNoteManager.StartNotes[0].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(500, 170), distance, 0);
        //            startNoteManager.StartNotes[1].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(600, 270), distance, 1);
        //            startNoteManager.StartNotes[2].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(600, 370), distance, 2);
        //            startNoteManager.StartNotes[3].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(500, 470), distance, 3);
        //            startNoteManager.StartNotes[4].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(400, 370), distance, 4);
        //            startNoteManager.StartNotes[5].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(400, 270), distance, 5);

        //    }

        //    else if (number == 1)
        //    {


        //        Marks[0].MarkSprite.Location = new Vector2(300, 270);
        //        Marks[1].MarkSprite.Location = new Vector2(400, 370);
        //        Marks[2].MarkSprite.Location = new Vector2(400, 470);
        //        Marks[3].MarkSprite.Location = new Vector2(300, 570);
        //        Marks[4].MarkSprite.Location = new Vector2(200, 470);
        //        Marks[5].MarkSprite.Location = new Vector2(100, 370);

        //        startNoteManager.StartNotes[0].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(300, 270), distance, 0);
        //        startNoteManager.StartNotes[1].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(400, 370), distance, 1);
        //        startNoteManager.StartNotes[2].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(400, 470), distance, 2);
        //        startNoteManager.StartNotes[3].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(300, 570), distance, 3);
        //        startNoteManager.StartNotes[4].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(200, 470), distance, 4);
        //        startNoteManager.StartNotes[5].StartNoteSprite.Location = GetStartNoteLocation(new Vector2(100, 370), distance, 5);

        //    }
        //    else if (number == 2)
        //    {

        

        //    }
          
        //}


        //현재 저장되어 있는 마커의 위치를 가져온다. 
        public static Vector2[] GetCurrentMarkerLocation()
        {

            Vector2[] vectorarray = new Vector2[6];

            int i;
            for (i = 0; i < 6; i++)
            {
                vectorarray[i] = Marks[i].MarkSprite.Location;
            }
             return vectorarray;

        }

        public static Vector2[] GetPattern(int index)
        {
            Vector2[] vectorarray = new Vector2[6];

            Vector2 mark0Loc = Vector2.Zero;
            Vector2 mark1Loc = Vector2.Zero;
            Vector2 mark2Loc = Vector2.Zero;
            Vector2 mark3Loc = Vector2.Zero;
            Vector2 mark4Loc = Vector2.Zero;
            Vector2 mark5Loc = Vector2.Zero;

            
            if (index == 0)
            {
                mark0Loc = new Vector2(450, 170);
                mark1Loc = new Vector2(650, 370);
                mark2Loc = new Vector2(650, 470);
                mark3Loc = new Vector2(450, 670);
                mark4Loc = new Vector2(250, 470);
                mark5Loc = new Vector2(250, 370);

            }
            else if (index == 1)
            {

                mark0Loc = new Vector2(450, 270);
                mark1Loc = new Vector2(550, 370);
                mark2Loc = new Vector2(550, 470);
                mark3Loc = new Vector2(450, 570);
                mark4Loc = new Vector2(350, 470);
                mark5Loc = new Vector2(350, 370);
            }
            vectorarray[0] = mark0Loc;
            vectorarray[1] = mark1Loc;
            vectorarray[2] = mark2Loc;
            vectorarray[3] = mark3Loc;
            vectorarray[4] = mark4Loc;
            vectorarray[5] = mark5Loc;

            return vectorarray;
        }


        //각 마터의 위치를 주면 바로 패턴이 변함
        public static void changeMarkPattern(Vector2 mark0, Vector2 mark1, Vector2 mark2, Vector2 mark3, Vector2 mark4, Vector2 mark5)
        {
            Marks[0].MarkSprite.Location = mark0;
            Marks[1].MarkSprite.Location = mark1;
            Marks[2].MarkSprite.Location = mark2;
            Marks[3].MarkSprite.Location = mark3;
            Marks[4].MarkSprite.Location = mark4;
            Marks[5].MarkSprite.Location = mark5;

            startNoteManager.StartNotes[0].StartNoteSprite.Location = GetStartNoteLocation(mark0, distance, 0);
            startNoteManager.StartNotes[1].StartNoteSprite.Location = GetStartNoteLocation(mark1, distance, 1);
            startNoteManager.StartNotes[2].StartNoteSprite.Location = GetStartNoteLocation(mark2, distance, 2);
            startNoteManager.StartNotes[3].StartNoteSprite.Location = GetStartNoteLocation(mark3, distance, 3);
            startNoteManager.StartNotes[4].StartNoteSprite.Location = GetStartNoteLocation(mark4, distance, 4);
            startNoteManager.StartNotes[5].StartNoteSprite.Location = GetStartNoteLocation(mark5, distance, 5);
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
                        //거리만큼 뺀다.
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

