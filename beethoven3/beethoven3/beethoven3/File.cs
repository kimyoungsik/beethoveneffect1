using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace beethoven3
{
    class File
    {
        #region declarations

        private StartNoteManager startNoteManager;
        private Queue allNotes = new Queue();      

        #endregion
        
        #region constructor
        
        public File( StartNoteManager startNoteManager)       
        {
             this.startNoteManager = startNoteManager;
        }
        
        #endregion

        #region method

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

        public void FindNote(double gameTime)
        {
            String[] noteContents;
            double noteTime;
            
            String note = (String)allNotes.Peek();

            noteContents = note.Split(' ');

            noteTime = Convert.ToDouble(noteContents[0]);

            noteTime = GetNoteStartTime(noteTime);

            if(noteTime <= gameTime )
             {

               PlayNote(Int32.Parse(noteContents[1]), Int32.Parse(noteContents[2]));
               allNotes.Dequeue();    
             }
            
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

        public void Update(GameTime gameTime)
        {

           double time = gameTime.TotalGameTime.TotalSeconds;

           FindNote(time);

        }


         public void PlayNote(int type, int markNumber)
        {
                switch (type)
                {
                    //right
                    case 0:
                        //시간에 맞춰서 뿌려줘야 함. 
                        startNoteManager.MakeRightNote(markNumber);

                        break;

                    //left
                    case 1:
                        startNoteManager.MakeLeftNote(markNumber);
                        break;
                }
 
       }

        #endregion
         

    }
}
