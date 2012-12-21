using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
namespace beethoven3
{
    class File
    {
        #region declarations

        private StartNoteManager startNoteManager;

        private NoteFileManager noteFileManager;


     //   private Queue allNotes = new Queue();
        private String[] noteContents;
        private ArrayList arrayNotes = new ArrayList();


        private double noteTime;
        private bool newNote = true;
        private bool drawLine = false;
        private double drawLineTime;
        private int startNoteNumber;

        private String[] noteLine;
    //    private NoteInfo[] rightNoteMarks;
        private int currentRightNoteIndex;
        private double time;

        private ExplosionManager badManager;

        private ScoreManager scoreManager;

        private bool endFile;
        #endregion
        
        #region constructor

        public File(StartNoteManager startNoteManager, NoteFileManager noteFileManager, ExplosionManager badManager, ScoreManager scoreManager)       
        {

             this.startNoteManager = startNoteManager;
             this.noteFileManager = noteFileManager;
             //rightNoteMarks = new NoteInfo[500];
             currentRightNoteIndex = 0;
             time = 0;
            
             this.badManager = badManager;

             this.scoreManager = scoreManager;
             this.endFile = false;

             
             newNote = true;
        }
        
        #endregion

        #region method

        public void SetCurrentRightNoteIndex(int value)
        {
            this.currentRightNoteIndex = value;

        }
        public void SetNewNote(bool value)
        {
            this.newNote = value;
        }
        
        public bool GetEndFile()
        {
            return this.endFile;
        }
        
        public void SetEndFile(bool value)
        {
            this.endFile = value;
        }
        public void SetTime(double value)
        {
            this.time = value;
        }
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
                String[] info = line.Split(' ');

                //0: version , 1:name , 2: artist, 3: mp3, 4: picture
                noteFileManager.Add(info[0], info[1], info[2], info[3], info[4]);
            }
        }


        /// <summary>
        /// 파일의 내용을 읽어 allNotes 큐에 넣는다.
        /// </summary>
        /// <param name="fileName"></param>

        public void Loading(int noteNumber)
        {
            String name = noteFileManager.noteFiles[noteNumber].Name;

            StreamReader sr = new StreamReader("C:\\beethoven\\"+name);
            scoreManager.SongName = name;
          //  int index = 0;
            //첫줄은 헤더
            sr.ReadLine();
            while (sr.Peek() >= 0)
            {
                String line = sr.ReadLine();
                //allNotes.Enqueue(line);
                arrayNotes.Add(line);

                //noteLine = ((String)line).Split(' ');
                //try
                //{
                //    if (Int32.Parse(noteLine[1]) == 0)
                //    {
                //        //가이드라인을 긋기위해 오른손 노트만 모아둠
                //        rightNoteMarks[index] = new NoteInfo(true, Convert.ToDouble(noteLine[0]), Int32.Parse(noteLine[2]));
                //        index++;
                //    }
                //    else
                //    {
                //        rightNoteMarks[index] = new NoteInfo(false, Convert.ToDouble(noteLine[0]), Int32.Parse(noteLine[2]));
                //        index++;
                //    }
                //}
                //catch (IndexOutOfRangeException)
                //{

                //}
            }
            sr.Close();
        }

        /// <summary>
        /// 가이드라인을 추가하여 그릴 수 있도록 함
        /// </summary>
        /// <param name="startMarkLocation"></param>
        /// <param name="endMarkLocation"></param>
        /// <param name="gold"></param>
        
        public void DrawGuidLine(int startMarkLocation, int endMarkLocation, bool gold,double firstStartTime, double secondStartTime)
        {
            double ratio = 0.6;
            //double startRatio = 0.2;
            float firstAngle = 30;
            float secondAngle = 90;
            Vector2 line;
            Vector2 start;
            Vector2 end;
            Vector2 angle;
            Vector2 angle2;
            Vector2 firstMid;
            Vector2 secondMid;
            double length;
           
            line = MarkManager.Marks[startMarkLocation].MarkSprite.Location - MarkManager.Marks[endMarkLocation].MarkSprite.Location;
            line.Normalize();

            start = GetMarkerLocation(startMarkLocation);
            end = GetMarkerLocation(endMarkLocation);
            //사이의 거리
            length = Vector2.Distance(start, end);
         
            //첫번째각
            angle = new Vector2((float)Math.Cos((float)Math.Atan2(start.Y, start.X) - MathHelper.ToRadians(firstAngle)), (float)Math.Sin((float)Math.Atan2(start.Y, start.X) - MathHelper.ToRadians(firstAngle)));
            //첫번째 제어점
            firstMid.X = start.X + (float)((angle.X * (length * ratio)));
            firstMid.Y = start.Y + (float)(angle.Y * (length * ratio));

            //두번쨰 각
            angle2 = new Vector2((float)Math.Cos((float)Math.Atan2(start.Y, start.X) - MathHelper.ToRadians(secondAngle)), (float)Math.Sin((float)Math.Atan2(start.Y, start.X) - MathHelper.ToRadians(secondAngle)));
            //두번째 제어점
            secondMid.X = start.X + (float)(angle2.X * (length * ratio));
            secondMid.Y = start.Y + (float)(angle2.Y * (length * ratio));

            GuideLineManager.AddGuideLine(start, firstMid, secondMid, end, (secondStartTime - firstStartTime) * 1000, gold);

        }
        public double ChangeTimeOfNote( double inputTime, double changedTempo)
        {
            double outputTime = 0.0f;

            outputTime = inputTime / changedTempo;
            
            return outputTime;
            
            //템포가 2배가 된상태에서 4초동안 지속이 된다면 모두 4-4/2 2초씩 줄여야 한다ㅣ
            //템포가 2배가 된상태에서 y초동안 지속이 된다면 모두 y-y/2초씩 줄여야 한다.
            //템포가 2배가 된상태에서 1초동안 지속이 된다면 모두 1-1/2  0.5초씩 줄여야 한다ㅣ
            //템포가 4배가 된상태에서 1초동안 지속이 된다면 모두 1-  1/4   0.75초씩 줄여야 한다ㅣ
            
            //템파고 x배가 된상태에서 y초 동안 지속이 된다면 모두 y-y/x초씩 줄여야 한다.
            //템포가 1.2배가 된상태에서 2초동안 지속이 된다면 모두 2-2/1.2 0.3333초
            //템포가 1.5배가 된상태에서 2초동안 지속이 된다면 모두 2-2/1.5 0.6666초
            
        }
        public void FindNote(double processTime, double changedTempo, double optionalTime)
        {


            if (!endFile)
            {
                    //noteContents = ((String)allNotes.Peek()).Split(' ');
                    noteContents = ((String)arrayNotes[0]).Split(' ');
                    noteTime = Convert.ToDouble(noteContents[0]);


                    if (changedTempo != 0)
                    {
                        noteTime = ChangeTimeOfNote( noteTime, changedTempo);
                        
                    }

                    noteTime += optionalTime;
                   // noteTime = Convert.ToDouble(noteContents[0]);
                    //시간에 맞추어서 노트가 날아갈 수 있게 생성 시간을 정한다. 
                    noteTime = GetNoteStartTime(noteTime);
                    newNote = false;
             
                    Trace.WriteLine(optionalTime.ToString());
                    //Trace.WriteLine(noteTime.ToString());

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
                                //시작점,제어점1,제어점2,끝점,지속시간

                                //outof range로 문제 될 수 있음
                                
                                //현재노트가  오른손 노트이고, 그 다음 노트가 오른손노트일 때
                              //  if (rightNoteMarks[currentRightNoteIndex].IsRight && rightNoteMarks[currentRightNoteIndex + 1].IsRight)


                                //오른손 노트여부 : [0]-> 0 ,, 마커 위치 [1]
                                double[] firstRightNote = GetNote(0);
                                double[] secondRightNote = null;
                                double[] thirdRightNote = null;
                                if (arrayNotes.Count > 1)
                                {
                                    secondRightNote = GetNote(1);
                                }
                                
                                if (arrayNotes.Count > 2)
                                {
                                    thirdRightNote = GetNote(2);
                                }

                                if (arrayNotes.Count > 1)
                                {
                                    if ((int)firstRightNote[0] == 0 && (int)secondRightNote[0] == 0)
                                    {
                                        //골드라인
                                        //  DrawGuidLine(rightNoteMarks[currentRightNoteIndex].MarkLocation, rightNoteMarks[currentRightNoteIndex + 1].MarkLocation, true);
                                        DrawGuidLine((int)firstRightNote[1], (int)secondRightNote[1], true,firstRightNote[2],secondRightNote[2]);

                                    }
                                }
                                if (arrayNotes.Count > 2)
                                {
                                    if ((int)secondRightNote[0] == 0 && (int)thirdRightNote[0] == 0)
                                    // if (rightNoteMarks[currentRightNoteIndex + 1].IsRight && rightNoteMarks[currentRightNoteIndex + 2].IsRight)
                                    {
                                        //일반 가이드라인
                                        // DrawGuidLine(rightNoteMarks[currentRightNoteIndex + 1].MarkLocation, rightNoteMarks[currentRightNoteIndex + 2].MarkLocation, false);
                                        DrawGuidLine((int)secondRightNote[1], (int)thirdRightNote[1], false, firstRightNote[2], secondRightNote[2]);
                                    }
                                }
                            }
                            catch (IndexOutOfRangeException)
                            {

                            }
                            catch (NullReferenceException)
                            {

                            }

                            break;



                        //왼손노트 
                        case 1:
                            startNoteManager.MakeLeftNote(Int32.Parse(noteContents[2]));
                            break;

                        //양손노트
                        case 2:

                            //      startNoteManager.MakeDoubleNote(Int32.Parse(noteContents[2]));
                            break;

                        //롱노트
                        case 3:
                            /* 다른것도 마찬가지이지만 롱노트가 여러개가 동시에 만들어질 경우
                             하나 밖에 나오지 않는다.
                             이것을 해결하려면. 바로 이곳에서 noteManager class의 객체를 만들어야 한다. 
                             하지만 이곳은 그냥 만들어진 객체에 add 하는것일 뿐이다. 
                             물론 객체를 계속 만들고 지우는것도 가능하다.
                             필요하다면 만들 수 도 있다.
                             */
                            startNoteManager.MakeLongNote(Int32.Parse(noteContents[2]));
                            startNoteNumber = Int32.Parse(noteContents[2]);
                            drawLineTime = Convert.ToDouble(noteContents[3]) + Convert.ToDouble(noteContents[0]);
                            drawLine = true;
                            break;

                        //드래그 노트
                        case 4:
                            //시작점,제어점1,제어점2,끝점,지속시간
                            CurveManager.addCurve(new Vector2(Int32.Parse(noteContents[3]), Int32.Parse(noteContents[4])), new Vector2(Int32.Parse(noteContents[5]), Int32.Parse(noteContents[6])), new Vector2(Int32.Parse(noteContents[7]), Int32.Parse(noteContents[8])), new Vector2(Int32.Parse(noteContents[9]), Int32.Parse(noteContents[10])), Convert.ToDouble(noteContents[2]));

                            break;
                    }
                    //allNotes.Dequeue();
                    arrayNotes.RemoveAt(0);

                    newNote = true;

                    //오른손노트를 true로 되어있는 array의 index를 하나씩 증가시시키는 값.
                    currentRightNoteIndex++;
                }

                //파일의 끝
                //End of note file
                //if (allNotes.Count == 0)
                if (arrayNotes.Count == 0)
                
                {
                    endFile = true;
                }
            }
        }

        //0:오른손여부(0이면 오른손), 1:마크위치 2: 노트시작 시간 
        public  double[] GetNote(int index)
        {
            
            
            String line = (String)arrayNotes[index];
            String[] lines = ((String)line).Split(' ');
            double[] values = new double[3];
            values[0] = (double)Int32.Parse(lines[1]);
            values[1] = (double)Int32.Parse(lines[2]);
            values[2] = Convert.ToDouble(lines[0]);
            return values;

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
           //drawLine필요성 여부 검토
            //필요하긴 하다. startNoteNumber이 null이 안되도록 한다.
            if (drawLine)
            {
                //drawLineTime => 그려지는 총 시간
                //
                if (drawLineTime >= processTime)
                {
                    startNoteManager.MakeLongNote(startNoteNumber);

                    //여기에서 손이 이곳에 있으면 되는것으로 
                      
                    if (checkLongNoteInCenterArea(startNoteNumber))
                    {
                        
                        badManager.AddExplosion(StartNoteManager.longNoteManager.LittleNotes[0].Center, Vector2.Zero);
                        StartNoteManager.longNoteManager.LittleNotes.RemoveAt(0);
                        scoreManager.Bad = scoreManager.Bad + 1;
                        if (scoreManager.Combo > scoreManager.Max)
                        {
                            scoreManager.Max = scoreManager.Combo;
                        }
    
                        scoreManager.Combo = 0;
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
                       // drawLine = false;
                        //이게 어디엔가에 있어야 할 듯 . 

                    
                    //if (StartNoteManager.longNoteManager.LittleNotes.Count >= 1)
                    //{
                    //    drawLine = false;
                    //    StartNoteManager.longNoteManager.LittleNotes.RemoveAt(0);
                    //}
                }
                
            }

        }
        //public bool checkCollinsion(int number)
        //{

        //    Sprite littleNote = StartNoteManager.longNoteManager.LittleNotes[0];

          
        //    //마커의 반지름으로
        //    bool judgment = MarkManager.Marks[number].MarkSprite.JudgedEdge(
        //        littleNote.Location, littleNote.CollisionRadius
        //        );

        //    return judgment;
        //}
        /// <summary>
        /// 롱노트 사각형 만나면 사라지게끔
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool checkLongNoteInCenterArea(int number)
        {
            Sprite littleNote = StartNoteManager.longNoteManager.LittleNotes[0];


            bool judgment = littleNote.IsBoxColliding(MarkManager.centerArea);

            return judgment;

        }


        /// <summary>
        /// 오른 손 노트 사각형 범위 들어가면 삭제 , 반복문 돌 필요가 없는지 다시 검토
        /// </summary>
        public void CheckRightNoteInCenterArea()
        {
            int i;
            for (i=0; i<StartNoteManager.rightNoteManager.LittleNotes.Count; i++ )
            {
                 Sprite littleNote = StartNoteManager.rightNoteManager.LittleNotes[i];

                
                if (littleNote.IsBoxColliding(MarkManager.centerArea))
                {


                    badManager.AddExplosion(littleNote.Center, Vector2.Zero);
                    StartNoteManager.rightNoteManager.LittleNotes.RemoveAt(i);
                    scoreManager.Bad = scoreManager.Bad + 1;
                    if (scoreManager.Combo > scoreManager.Max)
                    {
                        scoreManager.Max = scoreManager.Combo;
                    }


                    scoreManager.Combo = 0;
                    scoreManager.Gage = scoreManager.Gage - 1;
                }
            
            }
            

        }

        public void CheckLeftNoteInCenterArea()
        {
            int i;
            for (i = 0; i < StartNoteManager.leftNoteManager.LittleNotes.Count; i++)
            {
                Sprite littleNote = StartNoteManager.leftNoteManager.LittleNotes[i];


                if (littleNote.IsBoxColliding(MarkManager.centerArea))
                {
                    badManager.AddExplosion(littleNote.Center, Vector2.Zero);
                    StartNoteManager.leftNoteManager.LittleNotes.RemoveAt(i);
                    scoreManager.Bad = scoreManager.Bad + 1;
                    if (scoreManager.Combo > scoreManager.Max)
                    {
                        scoreManager.Max = scoreManager.Combo;
                    }
                    scoreManager.Combo = 0;

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
       


        public void Update(SpriteBatch spriteBatch, GameTime gameTime, double changedTempo, double optionalTime)
        {
            //오른노트가 사각형 범위로 가면 지워지도록
            CheckRightNoteInCenterArea();
            CheckLeftNoteInCenterArea();
            this.time += gameTime.ElapsedGameTime.TotalSeconds;
            FindNote(this.time, changedTempo, optionalTime);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            //double time = gameTime.TotalGameTime.TotalSeconds;
            DrawLineInLongNote(spriteBatch, this.time);

        }
     
        #endregion
    }
}
