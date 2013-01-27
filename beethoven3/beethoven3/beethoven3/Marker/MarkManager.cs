//#define Debug

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace beethoven3
{
    static class MarkManager
    {
        #region declarations
        private static Texture2D markTexture;
        private static Rectangle markInitialFrame;
        private static int markFrameCount;

        

        //마크 리스트
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
        public static float distance = 300.0f;


       
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
            float Scale = 1.0f
            /*지워지는 영역*/
        //    Rectangle area
            //GETPATTEN 함수 실행시에 지정되도록 한다,. 
            )
        {
            markTexture = texture;
            markInitialFrame = initialFrame;
            markFrameCount = frameCount;
        //    centerArea = area;
            scale = Scale;
            startNoteManager = startNoteMana;
            //새로 넣기 전에 지워주기 
            if (Marks.Count > 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    Marks.RemoveAt(0);
                }
            }
            addMark(mark0Loc);
            addMark(mark1Loc);
            addMark(mark2Loc);
            addMark(mark3Loc);
            addMark(mark4Loc);
            addMark(mark5Loc);

            //새로 넣기 전에 지워주기 
            if(startNoteManager.StartNotes.Count > 0)
            {
                startNoteManager.deleteAllMarks();
            }
              
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
        //*****
        public static void chageMarksImages(Texture2D Texture, Rectangle Rect, float Scale)
        {

            markTexture = Texture;
            markInitialFrame = Rect;
            scale = Scale;

            for (int i = 0; i < Marks.Count; i++)
            {
                Marks[i].MarkSprite.Texture = Texture;

                //각 크기 값을 정해놓은 frame을 바꾼다.
                Marks[i].MarkSprite.ChangeFrameRect(Rect);
               // markInitialFrame = rect;
                Marks[i].MarkSprite.Scale = scale;

                //반지름
                int radius = (int)((Rect.Width * Scale) / 2);
                Marks[i].MarkSprite.CollisionRadius = radius;
            }

        }


        public static Vector2 GetLocationMarker(int index)
        {

            return Marks[index].MarkSprite.Location;

        }


        //마커의 정확한 가로 세로 길이를 반환한다.
        //노트를 지우는 영역을 만드는데 필요하다.

        public static Vector2 GetMarkerSize()
        {
            Vector2 size;


            size.X = markTexture.Width * scale;
            size.Y = markTexture.Height * scale;

            return size;
        }




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

        //0번과 5번 마커 위치 필요
        public static void SetRemoveArea(Vector2 mark0Loc, Vector2 mark5Loc, int MarkWidth, int MarkHeight)
        {
          //  centerArea = new Rectangle((int)mark0Loc.X + (MarkWidth + MarkWidth / 2), (int)mark0Loc.Y + MarkHeight / 2, (int)mark5Loc.X - ((int)mark0Loc.X + (MarkWidth + MarkWidth)), (int)mark5Loc.Y - ((int)mark0Loc.Y));
            centerArea = new Rectangle(((int)mark5Loc.X - ((int)mark0Loc.X + MarkWidth)) / 2 + (int)mark0Loc.X + MarkWidth - 2, (int)mark0Loc.Y, 4, ((int)mark5Loc.Y + MarkHeight) - (int)mark0Loc.Y);

        
        }

        //INDEX필요 
        public static void SetRemoveArea(int patternIndex, int MarkWidth, int MarkHeight)
        {
            Vector2[] markLoc = GetPattern(patternIndex);
         //   centerArea = new Rectangle((int)markLoc[0].X + (MarkWidth *2 ), (int)markLoc[0].Y + MarkHeight / 2, (int)markLoc[5].X - ((int)markLoc[0].X + (MarkWidth *3)), (int)markLoc[5].Y - ((int)markLoc[0].Y));
            centerArea = new Rectangle(((int)markLoc[5].X - ((int)markLoc[0].X + MarkWidth)) / 2 + (int)markLoc[0].X + MarkWidth - 25, (int)markLoc[0].Y, 50, ((int)markLoc[5].Y + MarkHeight) - (int)markLoc[0].Y);

        
        }

        public static Vector2[] GetPattern(int index)
        {
            //마커 가로길이 , 마크 없어지는 영역 지정을 위해
            
            ////초기에는 마커가 없다. 
            //if (Marks != null)
            //{
            //    markWidth = Marks[0].MarkSprite.Texture.Width;
            //}
           
            Vector2[] vectorarray = new Vector2[6];

            Vector2 mark0Loc = Vector2.Zero;
            Vector2 mark1Loc = Vector2.Zero;
            Vector2 mark2Loc = Vector2.Zero;
            Vector2 mark3Loc = Vector2.Zero;
            Vector2 mark4Loc = Vector2.Zero;
            Vector2 mark5Loc = Vector2.Zero;

            switch (index)
            {
                case 0:
                    //mark0Loc = new Vector2(250, 260);
                    //mark1Loc = new Vector2(210, 420);
                    //mark2Loc = new Vector2(250, 580);
                    //mark3Loc = new Vector2(620, 260);
                    //mark4Loc = new Vector2(660, 420);
                    //mark5Loc = new Vector2(620, 580);

                    ////큰거
                    mark0Loc = new Vector2(200, 240);
                    mark1Loc = new Vector2(140, 420);
                    mark2Loc = new Vector2(200, 620);
                    mark3Loc = new Vector2(680, 240);
                    mark4Loc = new Vector2(740, 420);
                    mark5Loc = new Vector2(680, 620);
                  
                    break;

                case 1:
                    mark0Loc = new Vector2(250, 260);
                    mark1Loc = new Vector2(210, 420);
                    mark2Loc = new Vector2(250, 580);
                    mark3Loc = new Vector2(620, 260);
                    mark4Loc = new Vector2(660, 420);
                    mark5Loc = new Vector2(620, 580);

                    //mark0Loc = new Vector2(200, 240);
                    //mark1Loc = new Vector2(140, 420);
                    //mark2Loc = new Vector2(200, 620);
                    //mark3Loc = new Vector2(680, 240);
                    //mark4Loc = new Vector2(740, 420);
                    //mark5Loc = new Vector2(680, 620);
                 //   centerArea = new Rectangle(250, 260, 220, 320);
                    break;

                case 2:

                    mark0Loc = new Vector2(300, 280);
                    mark1Loc = new Vector2(260, 420);
                    mark2Loc = new Vector2(300, 560);
                    mark3Loc = new Vector2(570, 280);
                    mark4Loc = new Vector2(610, 420);
                    mark5Loc = new Vector2(570, 560);
                    break;

                case 3:

                    mark0Loc = new Vector2(140, 220);
                    mark1Loc = new Vector2(100, 400);
                    mark2Loc = new Vector2(140, 620);
                    mark3Loc = new Vector2(740, 220);
                    mark4Loc = new Vector2(780, 400);
                    mark5Loc = new Vector2(740, 620);
              //      centerArea = new Rectangle(250, 260, 220, 320);
                    break;

                    

            }
           
            vectorarray[0] = mark0Loc;
            vectorarray[1] = mark1Loc;
            vectorarray[2] = mark2Loc;
            vectorarray[3] = mark3Loc;
            vectorarray[4] = mark4Loc;
            vectorarray[5] = mark5Loc;


            //마커 없어지는 영역
               //마크 가장 왼쪽 위치 + 마크 가로길이 , 마크 가장 오른쪽 위치 + 마크가로길이 
          //  centerArea = new Rectangle((int)mark0Loc.X , (int)mark0Loc.Y, (int)mark5Loc.X - (int)mark0Loc.X, (int)mark5Loc.Y - (int)mark0Loc.Y);
  

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
            float x = distance;
            switch (type)
            {
                case 0:

                    bool find0 = false;
                    while (!find0)
                    {
                        //거리만큼 뺀다.
                        otherCenter.X -= x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X < 0)
                        {
                            find0 = true;
                        }
                    }
                    break;

                case 1:

                    bool find1 = false;
                    while (!find1)
                    {
                        otherCenter.X -= x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X < 0)
                        {
                            find1 = true;
                        }

                    }
                    break;
                case 2:

                    bool find2 = false;
                    while (!find2)
                    {
                        otherCenter.X -= x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X < 0)
                        {
                            find2 = true;
                        }
                    }
                    break;

                case 3:

                    bool find3 = false;
                    while (!find3)
                    {
                        otherCenter.X += x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X > 2000)
                        {
                            find3 = true;
                        }
                    }
                    break;

                case 4:

                    bool find4 = false;
                    while (!find4)
                    {
                        otherCenter.X += x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X > 2000)
                        {
                            find4 = true;
                        }
                    }
                    break;

                case 5:

                    bool find5 = false;
                    while (!find5)
                    {
                        otherCenter.X += x;
                        if (Vector2.Distance(center, otherCenter) == distance || otherCenter.X > 2000)
                        {
                            find5 = true;
                        }

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

#if Debug
            //노트 없어지는 영역 표시
            spriteBatch.Draw(Game1.blackRect, centerArea, Color.Red);
#endif

        }
        #endregion
    }
}

