using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace beethoven3
{
    class File
    {
        #region declarations

        private StartNoteManager startNoteManager;

        private NoteFileManager noteFileManager;


        private Queue allNotes = new Queue();
        private String[] noteContents;
        

        private double noteTime;
        private bool newNote = true;
        private bool drawLine = false;
        private double drawLineTime;
        private int startNoteNumber;

        private String[] noteLine;
        private NoteInfo[] rightNoteMarks;
        private int currentRightNoteIndex;
        private double time;


        #endregion
        
        #region constructor

        public File(StartNoteManager startNoteManager, NoteFileManager noteFileManager)       
        {
             this.startNoteManager = startNoteManager;
             this.noteFileManager = noteFileManager;
             rightNoteMarks = new NoteInfo[100];
             currentRightNoteIndex = 0;
             time = 0;
        }
        
        #endregion

        #region method

        public void FileLoading(String dir, String file)
        {
            //폴더를 검사해서 
            //파일을 읽어서 
            //NOTEFILEMANAGER 
            String[] files = Directory.GetFiles(dir, file, SearchOption.AllDirectories);

            int i;
            for (i = 0; i < files.Length; i++)
            {

                StreamReader sr = new StreamReader(files[i]);
                
                String line = sr.ReadLine();
                String[] info =line.Split(' ');
                    
                    //0: version , 1:name , 2: artist, 3: mp3, 4: picture
                noteFileManager.Add(info[0], info[1], info[2], info[3], info[4]);

                
            }



        }


        /// <summary>
        /// 파일의 내용을 읽어 allNotes 큐에 넣는다.
        /// </summary>
        /// <param name="fileName"></param>
        //public void Loading(String fileName)
        //{
        //    StreamReader sr = new StreamReader(fileName);
        //    //첫줄은 헤더
        //    //sr.ReadLine();
        //    while (sr.Peek() >= 0)
        //    {            
        //        String line = sr.ReadLine();
        //        allNotes.Enqueue(line);
        //    }
        //    sr.Close();
        //}
        public void Loading(int noteNumber)
        {
            String name = noteFileManager.noteFiles[noteNumber].Name;

            StreamReader sr = new StreamReader("C:\\beethoven\\"+name);

            int index = 0;
            //첫줄은 헤더
            sr.ReadLine();
            while (sr.Peek() >= 0)
            {
                String line = sr.ReadLine();
                allNotes.Enqueue(line);
                noteLine = ((String)line).Split(' ');
                try
                {
                    if (Int32.Parse(noteLine[1]) == 0)
                    {
                        //가이드라인을 긋기위해 오른손 노트만 모아둠
                        rightNoteMarks[index] = new NoteInfo(Convert.ToDouble(noteLine[0]), Int32.Parse(noteLine[2]));
                        index++;
                    }
                }
                catch (IndexOutOfRangeException)
                {

                }
            }
            sr.Close();
        }

        public void FindNote(double processTime)
        {
            //처음 실행하거나 de큐를 거치지 않은 새로운
            if (newNote)
            {
                noteContents = ((String)allNotes.Peek()).Split(' ');

                noteTime = Convert.ToDouble(noteContents[0]);
                
                noteTime = GetNoteStartTime(noteTime);

                newNote = false;
            }
            if (noteTime <= processTime)
            {
                //PlayNote(타입,날아가는 마커 위치)
                //타입 0-오른손 1-왼손 2-양손 3-롱노트 4-드래그노트 
                int type = Int32.Parse(noteContents[1]);
                
                switch (type)
                {
                    //오른손 노트
                    case 0:
                        //시간에 맞춰서 뿌려줘야 함. 
                        //notecontent[2] => 마커위치
                        startNoteManager.MakeRightNote(Int32.Parse(noteContents[2]));
                        

                        try
                        {
                            //현재오른손노트와 다음 노트와 연결, 그리고 그 다음 노트와 연결
                            //LineRenderer.DrawLine(Game1.spriteSheet, new Rectangle(0, 0, 50, 50), spriteBatch.GraphicsDevice, spriteBatch, (Vector2)Points[i], (Vector2)Points[j], Color.White);
                            //시작점,제어점1,제어점2,끝점,지속시간

                            double length1 = 0.6;
                            double startlength = 0.2;
                            float angle11 = 30;
                            float angle12 = 90;

                            int startMarkLocation = rightNoteMarks[currentRightNoteIndex].MarkLocation;
                            int endMarkLocation = rightNoteMarks[currentRightNoteIndex + 1].MarkLocation;
                            Vector2 normal = MarkManager.Marks[startMarkLocation].MarkSprite.Location - MarkManager.Marks[endMarkLocation].MarkSprite.Location;
                            normal.Normalize();
                            

                            Vector2 start = GetMarkerLocation(startMarkLocation);
                            Vector2 end = GetMarkerLocation(endMarkLocation);
                        //    Vector2 ex = Vector2.Multiply(MarkManager.Marks[startMarkLocation].MarkSprite.Location, normal);
                            double length = Vector2.Distance(start, end);
                            double x = normal.X * (length * startlength);
                            double y = normal.Y * (length * startlength);
                            Vector2 min1;
                            min1.X = start.X + (float)x;
                            min1.Y = start.Y +(float)y;

                            Vector2 angle = new Vector2((float)Math.Cos((float)Math.Atan2(min1.Y, min1.X) - MathHelper.ToRadians(angle11)), (float)Math.Sin((float)Math.Atan2(min1.Y, min1.X) - MathHelper.ToRadians(angle11)));
                            Vector2 min4;
                            double anx = angle.X * (length * length1);
                            double any = angle.Y * (length * length1);
                            min4.X = start.X + (float)anx;
                            min4.Y = start.Y + (float)any;



                            Vector2 angle2 = new Vector2((float)Math.Cos((float)Math.Atan2(min1.Y, min1.X) - MathHelper.ToRadians(angle12)), (float)Math.Sin((float)Math.Atan2(min1.Y, min1.X) - MathHelper.ToRadians(angle12)));
                            Vector2 min5;
                            double anx2 = angle2.X * (length * length1);
                            double any2 = angle2.Y * (length * length1);
                            min5.X = start.X + (float)anx2;
                            min5.Y = start.Y + (float)any2;
                           // double length = Math.Sqrt((end.X - start.X) * (end.X - start.X) + (end.Y - start.Y) * (end.Y - start.Y));
                          //  Vector2 min1 = Vector2.Reflect(MarkManager.Marks[startMarkLocation].MarkSprite.Location, normal);
                         //   min1.Normalize();
                         //   min1.X *= (float)(length * 0.3);
                          //  min1.Y *= (float)(length * 0.3);
                          //  min2.Normalize();
                          //  min2.X *= (float)(length * 0.3);
                          //  min2.Y *= (float)(length * 0.3);
                            GuideLineManager.AddGuideLine(start, min4, min5, end, (rightNoteMarks[currentRightNoteIndex + 1].StartTime - rightNoteMarks[currentRightNoteIndex].StartTime) * 1000, true);

                            //Vector2 a = MarkManager.Marks[1].MarkSprite.Location - MarkManager.Marks[0].MarkSprite.Location;
                            //a.Normalize();
                           // Vector2 normal = Vector2.Zero;
                            //normal = Vector2.Reflect(MarkManager.Marks[0].MarkSprite.Location, a);
                            //(nvert.ToDouble(noteContents[2])
                                                      
                            startMarkLocation = rightNoteMarks[currentRightNoteIndex + 1].MarkLocation;
                            endMarkLocation = rightNoteMarks[currentRightNoteIndex + 2].MarkLocation;
                            normal = MarkManager.Marks[startMarkLocation].MarkSprite.Location - MarkManager.Marks[endMarkLocation].MarkSprite.Location;
                            normal.Normalize();




                             start = GetMarkerLocation(startMarkLocation);
                             end = GetMarkerLocation(endMarkLocation);
                             length = Vector2.Distance(start, end);
                             x = normal.X * (length * length1);
                             y = normal.Y * (length * length1);
                             min1.X = start.X + (float)x;
                            min1.Y = start.Y + (float)y;

                            angle = new Vector2((float)Math.Cos((float)Math.Atan2(min1.Y, min1.X) - MathHelper.ToRadians(angle11)), (float)Math.Sin((float)Math.Atan2(min1.Y, min1.X) - MathHelper.ToRadians(angle11)));

                             anx = angle.X * (length * length1);
                             any = angle.Y * (length * length1);
                            min4.X = start.X + (float)anx;
                            min4.Y = start.Y + (float)any;



                             angle2 = new Vector2((float)Math.Cos((float)Math.Atan2(min1.Y, min1.X) - MathHelper.ToRadians(angle12)), (float)Math.Sin((float)Math.Atan2(min1.Y, min1.X) - MathHelper.ToRadians(angle12)));

                             anx2 = angle2.X * (length * length1);
                             any2 = angle2.Y * (length * length1);
                            min5.X = start.X + (float)anx2;
                            min5.Y = start.Y + (float)any2;
                            // double length = Math.Sqrt((end.X - start.X) * (end.X - start.X) + (end.Y - start.Y) * (end.Y - start.Y));
                            //  Vector2 min1 = Vector2.Reflect(MarkManager.Marks[startMarkLocation].MarkSprite.Location, normal);
                            //   min1.Normalize();
                            //   min1.X *= (float)(length * 0.3);
                            //  min1.Y *= (float)(length * 0.3);
                            //  min2.Y *= (float)(length * 0.3);
                            GuideLineManager.AddGuideLine(start, min4, min5, end, (rightNoteMarks[currentRightNoteIndex + 1].StartTime - rightNoteMarks[currentRightNoteIndex].StartTime) * 1000, false);

                        //    normal = MarkManager.Marks[endMarkLocation].MarkSprite.Location - MarkManager.Marks[startMarkLocation].MarkSprite.Location;
                        //    normal.Normalize();

                        //    Vector2 start2 = GetMarkerLocation(startMarkLocation);
                        //    Vector2 end2 = GetMarkerLocation(endMarkLocation);

                        //    length = Math.Sqrt((end2.X - start2.X) * (end2.X - start2.X) + (end2.Y - start2.Y) * (end2.Y - start2.Y));

                        //    Vector2 mid1 = Vector2.Reflect(MarkManager.Marks[startMarkLocation].MarkSprite.Location, normal);
                        //    //mid1.Normalize();

                        //    //mid1.X *= (float)(length * 0.3);
                        //    //mid1.Y *= (float)(length * 0.3);
                        //    Vector2 mid2 = Vector2.Reflect(MarkManager.Marks[startMarkLocation].MarkSprite.Location, normal);
                        //    //mid2.Normalize();
                        //    //mid2.X *= (float)(length * 0.3);
                        //    //mid2.Y *= (float)(length * 0.3);
                        ////    GuideLineManager.AddGuideLine(start2, mid1, mid2, end2, (rightNoteMarks[currentRightNoteIndex + 1].StartTime - rightNoteMarks[currentRightNoteIndex].StartTime) * 1000, false);

                        }
                        catch (IndexOutOfRangeException)
                        {

                        }

                        currentRightNoteIndex++;

                        break;

                    //왼손노트 
                    case 1:
                        startNoteManager.MakeLeftNote(Int32.Parse(noteContents[2]));
                        break;

                    //양손노트
                    case 2:
                        
                        startNoteManager.MakeDoubleNote(Int32.Parse(noteContents[2]));
                        break;

                    //롱노트
                    case 3:


                        startNoteManager.MakeLongNote(Int32.Parse(noteContents[2]));
                        startNoteNumber = Int32.Parse(noteContents[2]);
                        drawLineTime = Convert.ToDouble(noteContents[3]);
                        drawLine = true;
                        break;

                    //드래그 노트
                    case 4:
                        //시작점,제어점1,제어점2,끝점,지속시간
                        CurveManager.addCurve(new Vector2(Int32.Parse(noteContents[3]), Int32.Parse(noteContents[4])), new Vector2(Int32.Parse(noteContents[5]), Int32.Parse(noteContents[6])), new Vector2(Int32.Parse(noteContents[7]), Int32.Parse(noteContents[8])), new Vector2(Int32.Parse(noteContents[9]), Int32.Parse(noteContents[10])), Convert.ToDouble(noteContents[2]));
                        
                                  
                        break;
                }
                allNotes.Dequeue();
                newNote = true;
            }
          
        }


        public Vector2 GetMarkerLocation(int markerNumber)
        {
            Vector2 location = new Vector2(0,0);
            switch (markerNumber)
            {
                case 0:
                    location = MarkManager.mark0Location;
                    break;

                case 1:
                    location = MarkManager.mark1Location;
                    break;
                case 2:
                    location = MarkManager.mark2Location;
                    break;
                case 3:
                    location = MarkManager.mark3Location;
                    break;
                case 4:
                    location = MarkManager.mark4Location;
                    break;
                case 5:
                    location = MarkManager.mark5Location;
                    break;
         
            }


            return location;

        }
        public void DrawLineInLongNote(SpriteBatch spriteBatch, double processTime)
        {
            if (drawLine)
            {

                if (drawLineTime >= processTime)
                {
                    startNoteManager.MakeLongNote(startNoteNumber);
                   // LineRenderer.DrawLine(Game1.spriteSheet, new Rectangle(200, 100, 50, 55), spriteBatch.GraphicsDevice, spriteBatch, StartNoteManager.longNoteManager.LittleNotes[0].Location, startNoteManager.StartNotes[this.startNoteNumber].StartNoteSprite.Location, Color.White);
                    if( (checkLongNoteToMarker(startNoteNumber)) == 2)
                    {
                        //롱노트 시간 안움직임
                       // StartNoteManager.longNoteManager.LittleNotes[0].Velocity = new Vector2(0,0);
                        StartNoteManager.longNoteManager.LittleNotes.RemoveAt(0);
                    }

                }
                else//시간 지난후에 다시 들어오지 않게 
                {

                    try
                    {
                        StartNoteManager.longNoteManager.LittleNotes.RemoveAt(0);
                    }
                    catch (ArgumentOutOfRangeException)
                    {

                    }
                        //drawLine = false;
                    //if (StartNoteManager.longNoteManager.LittleNotes.Count >= 1)
                    //{
                    //    drawLine = false;
                    //    StartNoteManager.longNoteManager.LittleNotes.RemoveAt(0);
                    //}
                }
                
            }

        }


        public int checkLongNoteToMarker(int number)
        {

                Sprite littleNote = StartNoteManager.longNoteManager.LittleNotes[0];

              
                //마커의 반지름으로
                int judgment = MarkManager.Marks[number].MarkSprite.JudgedNote(
                    littleNote.Center,
                    littleNote.CollisionRadius);

                return judgment;
        }


        /// <summary>
        /// 마커에 노트가 닿는 시간을 정확히 맞추기 위해서
        /// </summary>
        /// <param name="noteTime"></param>
        /// <returns></returns>

        public double GetNoteStartTime(double noteTime)
        {

            double startTime= 0.0f;

            //거리/속력 

            double time = (MarkManager.distance) / (StartNoteManager.noteSpeed);

            startTime = noteTime - time;

            return startTime;


            
        }


        public void Update(SpriteBatch spriteBatch, GameTime gameTime)
        {
        //   double time = gameTime.TotalGameTime.TotalSeconds;
            this.time += gameTime.ElapsedGameTime.TotalSeconds;
            FindNote(this.time);
        }

         

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            //double time = gameTime.TotalGameTime.TotalSeconds;
            DrawLineInLongNote(spriteBatch, this.time);

        }
     
        #endregion
         

    }
}
