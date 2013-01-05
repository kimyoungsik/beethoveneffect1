using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
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
       //  private String[] noteContents;
        private List<NoteInfo> arrayNotes = new List<NoteInfo>();

        private double noteTime;
        private bool newNote = true;
        private bool drawLine = false;
        private double drawLineTime;
        private int startNoteNumber;

     //   private String[] noteLine;
    //    private NoteInfo[] rightNoteMarks;
        private int currentRightNoteIndex;
        private double time;

        private ExplosionManager badManager;

        private ScoreManager scoreManager;

        private bool endFile;

        private Double startTime;

        private Double endTime;

        private int bpm;
        //패턴이 바뀌는 중인지 체크
        private bool patternChanging = false;

        public Queue drawGuideLineQueue = new Queue();

        //////////////////////////////////FOR PATTERN CHAGNE//////////////////////////////
        //패턴 바뀌기전의 마커위치들
        private Vector2[] initMarkersLocation = null;

        //패턴 바뀌기 전의 마커위치 뽑아낼 수 있도록 하는 bool
        //  bool isFirstGettingMarker = false;

        //실시간으로 바뀐 마커위치
        private Vector2[] changedMarks = new Vector2[6];

        //최종적으로 이동될 마커 위치
        private Vector2[] Endlocations = new Vector2[6];


        //패턴 끝나는 시간.
        private double endPatternChangeTime = 0;
        //패턴 시작 시간.
        private double startPatternChangeTime = 0;
        //지속시간
        private double lastingTime = 0;


        /// <summary>
        /// ///////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="startNoteManager"></param>
        /// <param name="noteFileManager"></param>
        /// <param name="badManager"></param>
        /// <param name="scoreManager"></param>
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
        public List<NoteInfo> GetArrayNotes()
        {
            return arrayNotes;
        }

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
            //String[] files = Directory.GetFiles(dir, file, SearchOption.AllDirectories);
            //byte[] buffer;
            //int i;
            //for (i = 0; i < files.Length; i++)
            //{
            //    IFormatter formatter = new BinaryFormatter();
            //    //streamReader sr = new StreamReader(files[i]);
            //    FileStream fileStream = new FileStream(files[i], FileMode.Open, FileAccess.Read);
            //    ()formatter.Deserialize(); 
            // //  FileInfo fileStream = new FileInfo(files[i]);

            //    BinaryReader br = new BinaryReader(fileStream);

            //    char[]  buf0;
            //    buf0 = br.ReadChars(128);
            //    int a;
            //    Console.WriteLine(buf0);

            //    //int length = (int)fileStream.Length;
            //    //buffer = new byte[length];
            //    a = br.ReadInt32();
                

            //    //while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
            //    //{
            //    //    sum += count;
            //    //}


            //   // String line = sr.ReadLine();
            //  //  String[] info = line.Split(' ');

            //    //0: version , 1:name , 2: artist, 3: mp3, 4: picture
            //   // noteFileManager.Add(info[0], info[1], info[2], info[3], info[4]);

            String[] files = Directory.GetFiles(dir, file, SearchOption.AllDirectories);

            int i;
            for (i = 0; i < files.Length; i++)
            {

                StreamReader sr = new StreamReader(files[i],Encoding.Unicode);

                String line = sr.ReadLine();
                //첫줄은 MNF FILE임을 확인
                //Check Wheater first line is MNF FILE or not
                if (line == "MNF FILE")
                {
                    
                    line = sr.ReadLine();
                    
                    //두번째줄은 공백
                    //Second line is empty

                    if (line == "")
                    {
                        //헤더임을 표시하는 라인
                        
                        line = sr.ReadLine();
                        
                        if (line == "#################### [HEADER] ####################")
                        {

                            String name = sr.ReadLine();

                            String artist = sr.ReadLine();

                            String mp3 = sr.ReadLine();

                            String picture = sr.ReadLine();

                            double startTime = Convert.ToDouble(sr.ReadLine());

                            double endTime = Convert.ToDouble(sr.ReadLine());

                            //0: version , 1:name , 2: artist, 3: mp3, 4: picture

                            //노트정보관리에 다음 사항을 넣는다. //*** 버전 추가 
                            noteFileManager.Add("1", name, artist, mp3, picture, startTime, endTime);

                            

                            bpm = Int32.Parse(sr.ReadLine());
                        }
                        else
                        {

                            Trace.WriteLine("can not read file");
                        }
                    }
                    else
                    {
                        Trace.WriteLine("can not read file");
                    }
                }
                else
                {
                    Trace.WriteLine("can not read file");
                }
            }
            
        }


        /// <summary>
        /// 파일의 내용을 읽어 allNotes 큐에 넣는다.
        /// </summary>
        /// <param name="fileName"></param>

        //노래 시작하기 바로 직전 //***로딩 여기서?
        public void Loading(int noteNumber)
        {
            String name = noteFileManager.noteFiles[noteNumber].Name;
            
            //시작 시간 설정
            startTime = noteFileManager.noteFiles[noteNumber].StartTime;
            
            //끝나는 시간 설정
            endTime = noteFileManager.noteFiles[noteNumber].EndTime;

            StreamReader sr = new StreamReader("C:\\beethoven\\" + name, Encoding.Unicode);
            scoreManager.SongName = name;
         
            //첫줄은 헤더
            String stringLine = sr.ReadLine();
            while (stringLine != "##################### [BODY] #####################")
            {
                stringLine = sr.ReadLine();

            }
            //END가 나올 때 까지
            //Routine until meeting End
            while(stringLine != "END")
            {
                stringLine = sr.ReadLine();
                
                //END가 아니어야 들어갈 수 있다.
                //END is not allowed
                if (stringLine != "END")
                {
                    String[] lines = ((String)stringLine).Split('/', ',');
                    bool isright = false;

                    try
                    {
                        //오른손노트 왼손노트
                        //rightNote and LeftNote
                        if (lines[1] == "1" || lines[1] == "2")
                        {

                            if (lines[1] == "1")
                            {
                                isright = true;
                            }

                            //lines =>2:오른손이면 0 , 1: 마크위치 0: 시작시간 
                            arrayNotes.Add(new NoteInfo(isright, Convert.ToDouble(lines[0]), Int32.Parse(lines[2]),  /*type*/lines[1],/*lastTime*/ 0, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero));

                        }
                        //롱노트
                        else if (lines[1] == "4")
                        {

                            arrayNotes.Add(new NoteInfo(isright,/*startTime*/Convert.ToDouble(lines[0]), /*markLocation*/Int32.Parse(lines[2]), /*type*/lines[1], /*lastTime*/Convert.ToDouble(lines[3]), Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero));


                        }
                        //드래그노트
                        else if (lines[1] == "D")
                        {

                            arrayNotes.Add(new NoteInfo(isright, /*startTime*/Convert.ToDouble(lines[0]), /*markLocation*/ -1, /*type*/lines[1], /*lastTime*/ Convert.ToDouble(lines[2]), new Vector2(Int32.Parse(lines[3]), Int32.Parse(lines[4])), new Vector2(Int32.Parse(lines[5]), Int32.Parse(lines[6])), new Vector2(Int32.Parse(lines[7]), Int32.Parse(lines[8])), new Vector2(Int32.Parse(lines[9]), Int32.Parse(lines[10]))));


                        }

                        //PATTERN CHANGE
                        //패턴 체인지
                        else if (lines[1] == "C")
                        {
                            arrayNotes.Add(new NoteInfo(isright,/*startTime*/Convert.ToDouble(lines[0]), /*not markLocation but change type */Int32.Parse(lines[2]), /*type*/lines[1], /*not lastTime but Lasting time(지속시간)*/Convert.ToDouble(lines[3]), Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero));

                        }


                        //BPM CHANGE
                        else if (lines[1] == "B")
                        {

                        }
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Trace.WriteLine(ex);
                    }
                }
            
            }
            sr.Close();
        }

        /// <summary>
        /// 가이드라인을 추가하여 그릴 수 있도록 함
        /// </summary>
        /// <param name="startMarkLocation"></param>
        /// <param name="endMarkLocation"></param>
        /// <param name="gold"></param>

        public void DrawGuidLine(int startMarkLocation, int endMarkLocation, bool gold, double firstStartTime, double secondStartTime)
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

        //템포로 나누어 시간 변경
        public void ChangeArrayNoteTempo(double changedTempo)
        {
            int i;
            for (i = 0; i < this.arrayNotes.Count; i++)
            {

                arrayNotes[i].StartTime = arrayNotes[i].StartTime / changedTempo;

            }

        }


        //템포 다시 원상복귀
        public void ChangeArrayNoteTempoBack(double changedTempo)
        {
            int i;
            for (i = 0; i < this.arrayNotes.Count; i++)
            {

                arrayNotes[i].StartTime = arrayNotes[i].StartTime * changedTempo;

            }

        }


        public void OptionalArrayNote(double optionalTime)
        {
            int i;
            for (i = 0; i < this.arrayNotes.Count; i++)
            {

                arrayNotes[i].StartTime +=  optionalTime;
            }

        }


        public double ChangeTimeOfNote(double inputTime, double changedTempo)
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
            int i;

            if (patternChanging)
            {
                //진행시간이 끝나는 시간보다 적을 때
                if (processTime < endPatternChangeTime && processTime > startPatternChangeTime)
                {
                    patternChanging = true;


                    //현재 진행 상황, 진행이 많이 될수록 값이 적게 나온다.
                    double diffrence = endPatternChangeTime - processTime;

                    for (i = 0; i < 6; i++)
                    {
                        changedMarks[i].X = GetLocation(initMarkersLocation[i].X, Endlocations[i].X, lastingTime - diffrence, lastingTime);
                        changedMarks[i].Y = GetLocation(initMarkersLocation[i].Y, Endlocations[i].Y,  lastingTime - diffrence, lastingTime);
                    }


                    MarkManager.changeMarkPattern(changedMarks[0], changedMarks[1], changedMarks[2], changedMarks[3], changedMarks[4], changedMarks[5]);

                }
                else if (processTime <= startPatternChangeTime)
                {
                    patternChanging = true;
                }
                else
                {
                    patternChanging = false;
                    //isFirstGettingMarker = false;
                }
            }
            //////////////////////////////////////////////////////////////////
            if (processTime < endTime)
            {
                //노트가 아무것도 없으면 실행되지 않는다.
                //it doesn't work if there is no note.
                if (drawGuideLineQueue.Count > 0)
                {
                    DrawGuideLineInfo guideline = (DrawGuideLineInfo)drawGuideLineQueue.Peek();
                    

                    if (guideline.FirstStartTime <= processTime)
                    {
                        DrawGuideLineInfo guideline2 = (DrawGuideLineInfo)drawGuideLineQueue.Dequeue();
                     //   Trace.WriteLine(guideline.StartMarkLocation +"-"+ guideline.EndMarkLocation);
                        DrawGuidLine(guideline2.StartMarkLocation, guideline2.EndMarkLocation, guideline2.Gold, guideline2.FirstStartTime, guideline2.SecondStartTime);
                    }
                }
                if (arrayNotes.Count != 0)
                {

                    //시간에 맞추어서 노트가 날아갈 수 있게 생성 시간을 정한다. 
                    noteTime = GetNoteStartTime(arrayNotes[0].StartTime);
                 

                    if (noteTime <= processTime )
                    {
                        //PlayNote(타입,날아가는 마커 위치)
                        //타입 0-오른손 1-왼손 2-양손 3-롱노트 4-드래그노트 
                        //Trace.WriteLine(processTime);

                      
                        //오른손 노트

                        if (arrayNotes[0].Type == "1")
                        {
                            //시간에 맞춰서 뿌려줘야 함. 
                            //notecontent[2] => 마커위치
                            RightNoteInfo rightNote = startNoteManager.MakeRightNote(arrayNotes[0].MarkLocation);

                            try
                            {
                                //현재오른손노트와 다음 노트와 연결, 그리고 그 다음 노트와 연결
                                //시작점,제어점1,제어점2,끝점,지속시간

                                //outof range로 문제 될 수 있음

                                //현재노트가  오른손 노트이고, 그 다음 노트가 오른손노트일 때
                                
                                
                                //적어도 1개 이상의 오른손 노트가 있을 때
                                if (arrayNotes.Count > 1)
                                {
                                    //현재 노트로 오른손노트이고 다음 노트도 오른손 노트일때
                                    if (arrayNotes[0].IsRight && arrayNotes[1].IsRight)
                                    {
                                        DrawGuideLineInfo drawGuideLineInfo = new DrawGuideLineInfo(arrayNotes[0].MarkLocation - 1, arrayNotes[1].MarkLocation - 1, true, arrayNotes[0].StartTime, arrayNotes[1].StartTime);
                                    
                                        drawGuideLineQueue.Enqueue(drawGuideLineInfo);
                                      
                                        //골드라인
                                        //  DrawGuidLine(rightNoteMarks[currentRightNoteIndex].MarkLocation, rightNoteMarks[currentRightNoteIndex + 1].MarkLocation, true);
                                        // if 마커에 맞추었을 때 
                                        // 스타트로 날아간 후에 어느정도 시간이 지났을 때
                                      //  DrawGuidLine(arrayNotes[0].MarkLocation - 1, arrayNotes[1].MarkLocation - 1, true, arrayNotes[0].StartTime, arrayNotes[1].StartTime);
                                    }
                                }

                                //적어도 노트가 2개 이상
                                if (arrayNotes.Count > 2)
                                {
                                    if (arrayNotes[1].IsRight && arrayNotes[2].IsRight)
                                    // if (rightNoteMarks[currentRightNoteIndex + 1].IsRight && rightNoteMarks[currentRightNoteIndex + 2].IsRight)
                                    {
                                        DrawGuideLineInfo drawGuideLineInfo = new DrawGuideLineInfo(arrayNotes[1].MarkLocation - 1, arrayNotes[2].MarkLocation - 1, false, arrayNotes[0].StartTime, arrayNotes[1].StartTime);
                                        drawGuideLineQueue.Enqueue(drawGuideLineInfo);
                                       
                                        //일반 가이드라인
                                        // DrawGuidLine(rightNoteMarks[currentRightNoteIndex + 1].MarkLocation, rightNoteMarks[currentRightNoteIndex + 2].MarkLocation, false);               
                                     //   DrawGuidLine(arrayNotes[1].MarkLocation - 1, arrayNotes[2].MarkLocation - 1, false, arrayNotes[0].StartTime, arrayNotes[1].StartTime);
                                    }
                                }
                            }
                            catch (IndexOutOfRangeException)
                            {

                            }
                            catch (NullReferenceException)
                            {

                            }
                        }

                     //왼손노트 
                        else if (arrayNotes[0].Type == "2")
                        {
                            startNoteManager.MakeLeftNote(arrayNotes[0].MarkLocation);
                        }

                        //양손노트

                        //롱노트
                        else if (arrayNotes[0].Type == "4")
                        {
                            /* 다른것도 마찬가지이지만 롱노트가 여러개가 동시에 만들어질 경우
                             하나 밖에 나오지 않는다.
                             이것을 해결하려면. 바로 이곳에서 noteManager class의 객체를 만들어야 한다. 
                             하지만 이곳은 그냥 만들어진 객체에 add 하는것일 뿐이다. 
                             물론 객체를 계속 만들고 지우는것도 가능하다.
                             필요하다면 만들 수 도 있다.
                             */
                            startNoteManager.MakeLongNote(arrayNotes[0].MarkLocation);
                            startNoteNumber = arrayNotes[0].MarkLocation;

                            //롱노트 //드래그노트는 끝나는 시간 까지, 포함 이건 noteInfo에포함해야 됨 
                            //끝나는시간
                            //  drawLineTime = arrayNotes[0].LastTime + arrayNotes[0].StartTime;
                            drawLineTime = arrayNotes[0].LastTime;
                            drawLine = true;
                            //   break;
                        }
                        //드래그 노트
                        else if (arrayNotes[0].Type == "D")
                        {
                            //case 4:
                            //시작점,제어점1,제어점2,끝점,지속시간
                            CurveManager.addCurve(arrayNotes[0].StartPoint, arrayNotes[0].FirstOperatorPoint, arrayNotes[0].SecondOperatorPoint, arrayNotes[0].EndPoint, (arrayNotes[0].LastTime - arrayNotes[0].StartTime) * 1000);
                        }
                        //패턴 변환
                        //pattern change
                        else if (arrayNotes[0].Type == "C" )
                        {
                            //marklocation이란 attribute에는 몇번 패턴으로 변할 것인가.
                            //marklocation means which pattern the note will be changed

                            //이미 저장되어 있는 인덱스에 따른 패튼 가져오기
                            //Get some patterns which is alread stored according to the index

                            Endlocations = MarkManager.GetPattern(arrayNotes[0].MarkLocation);

                           
                            //패턴이 변하는 중이 아니라면 처음 마커위치를 가져온다. 
                           // if(!isFirstGettingMarker)
                           // {
                                initMarkersLocation =  MarkManager.GetCurrentMarkerLocation();
                            //    isFirstGettingMarker = true;
                           // }
                            
                            
                            //끝나는 시간이라고 가정
                            //if Lasttime were endingTime
                            endPatternChangeTime = arrayNotes[0].LastTime;
                            startPatternChangeTime = arrayNotes[0].StartTime;
                            //지속시간
                            //lastingtime
                             lastingTime = endPatternChangeTime - arrayNotes[0].StartTime;
                             patternChanging = true;
//                            //진행시간이 끝나는 시간보다 적을 때
//                            if (processTime < endPatternChangeTime && processTime > arrayNotes[0].StartTime)
//                            {
//                                patternChanging = true;
                                

//                                //현재 진행 상황, 진행이 많이 될수록 값이 적게 나온다.
//                                double diffrence = endPatternChangeTime - processTime;


//                                for (i = 0; i < 6; i++)
//                                {
//                                    changedMarks[i].X = GetLocation(Endlocations[i].X, initMarkersLocation[i].X, lastingTime - diffrence, lastingTime);
//                                    changedMarks[i].Y = GetLocation(Endlocations[i].Y, initMarkersLocation[i].Y, lastingTime - diffrence, lastingTime);
//                                }


//                                MarkManager.changeMarkPattern(changedMarks[0], changedMarks[1], changedMarks[2], changedMarks[3], changedMarks[4], changedMarks[5]);

//                            }
//                            else if (processTime <= arrayNotes[0].StartTime)
//                            {
//                                patternChanging = true;
//                            }
//                            else
//                            {
//                                patternChanging = false;
////isFirstGettingMarker = false;
//                            }

                            //명령하는 위치로 패턴 변환
                            //locatinos make the pattern location change
                           // MarkManager.changeMarkPattern(locations[0], locations[1], locations[2], locations[3], locations[4], locations[5]);
                        }



                        arrayNotes.RemoveAt(0);

                        newNote = true;

                        //오른손노트를 true로 되어있는 array의 index를 하나씩 증가시시키는 값.
                        //      currentRightNoteIndex++;
                    }

                    //파일의 끝
                    //End of note file
                    //if (allNotes.Count == 0)
                    //if (arrayNotes.Count == 0)

                    //{
                    //    endFile = true;
                    //}
                }
            }
            else
            {
                endFile = true;
            }
                

        }

        //전체 진행에 따른 현재 마커 위치
        //Get current marker locaiton in accordance to the process
        public float GetLocation(float startPoint, float endPoint, double currentTime, double lastingTime)
        {
            return  startPoint + ((endPoint - startPoint) * ((float)currentTime / (float)lastingTime));  
 //         시작점 + (      (끝점 - 시작점)* (lasttime - currentTime/lasttime))
        }
   


        
        public Vector2 GetMarkerLocation(int markerNumber)
        {
            Vector2 location = new Vector2(0,0);
            switch (markerNumber)
            {
                case 0:
                    
                    location = MarkManager.Marks[0].MarkSprite.Location;
                    break;

                case 1:
                    location = MarkManager.Marks[1].MarkSprite.Location;
                    break;
                case 2:
                    location = MarkManager.Marks[2].MarkSprite.Location;
                    break;
                case 3:
                    location = MarkManager.Marks[3].MarkSprite.Location;
                    break;
                case 4:
                    location = MarkManager.Marks[4].MarkSprite.Location;
                    break;
                case 5:
                    location = MarkManager.Marks[5].MarkSprite.Location;
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

                    //여기에서 손이 이곳에  있으면 되는것으로 
                      
                    if (checkLongNoteInCenterArea(startNoteNumber))
                    {
                        
                  //?      badManager.AddExplosion(StartNoteManager.longNoteManager.LittleNotes[0].Center, Vector2.Zero);
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
                        //Trace.WriteLine("outofrange in longnote");
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


                  //  badManager.AddExplosion(littleNote.Center, Vector2.Zero);
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
                //    badManager.AddExplosion(littleNote.Center, Vector2.Zero);
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
                    littleNote.Center
                    );

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
