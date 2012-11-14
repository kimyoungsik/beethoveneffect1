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

        #endregion
        
        #region constructor
        
        public File( StartNoteManager startNoteManager)       
        {
             this.startNoteManager = startNoteManager;
  
        }
        
        #endregion

        #region method

        public void FileLoading(String dir, String file)
        {
            //폴더를 검사해서 
            //파일을 읽어서 
            //NOTEFILEMANAGER 
            String[] files = Directory.GetFiles(dir, file, SearchOption.AllDirectories);

            int i = 1;




        }


        /// <summary>
        /// 파일의 내용을 읽어 allNotes 큐에 넣는다.
        /// </summary>
        /// <param name="fileName"></param>
        public void Loading(String fileName)
        {
            StreamReader sr = new StreamReader(fileName);
           
            while (sr.Peek() >= 0)
            {            
                String line = sr.ReadLine();
                allNotes.Enqueue(line);
            }
            sr.Close();
        }


        public void FindNote(double processTime)
        {
            //처음 실행하거나 de큐를 거치지 않은 새로운 ㄳ만
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

           double time = gameTime.TotalGameTime.TotalSeconds;

           FindNote(time);
       //    DrawLineInDragNote(spriteBatch, time);

        }



        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            double time = gameTime.TotalGameTime.TotalSeconds;

          //  FindNote(time);
            DrawLineInLongNote(spriteBatch, time);

        }
     
        #endregion
         

    }
}
