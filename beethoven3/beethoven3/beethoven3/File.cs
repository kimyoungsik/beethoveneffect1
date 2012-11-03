﻿using System;
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
        private StreamReader sr;
        private String line;
        private String[] words;
     //   private int type;
     //   private int markNumber;
        private float noteTime;
     //   private int bps;
        private bool played;
        #endregion
        
        #region constructor
        
        public File(String fileName, StartNoteManager startNoteManager) 
        
        {
            this.sr = new StreamReader(fileName);
            this.startNoteManager = startNoteManager;
            this.played = false;
        }
        
        #endregion

        #region method

        public void Update(GameTime gameTime)
        {

            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;



            if (sr.Peek() >= 0)
            {
                if (!played)
                {
                    line = this.sr.ReadLine();
                    played = true;
                }
                words = line.Split(' ');
                noteTime = (float)Convert.ToDouble(words[0]);
                if (noteTime == time)
                {
                    
                    PlayNote(Int32.Parse(words[1]), Int32.Parse(words[2]));
                    played = false;
                }
            }
            else
            {
                sr.Close();
            }

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
        //public void PlayNote(String fileName)
        //{
        //    StreamReader sr = new StreamReader(fileName);
        //    String line;
        //    String[] words;
        //    int type;
        //    int markNumber;
        //    int time;
        //    int bps;
        //    while (sr.Peek() >= 0)
        //    {
        //        line = sr.ReadLine();
        //        words = line.Split(' ');

        //        type = Int32.Parse(words[0]);
        //        markNumber= Int32.Parse(words[1]);
        //        time= Int32.Parse(words[2]);
        //        bps = Int32.Parse(words[3]);

        //        //string to int : null -> error
        //        switch (type)
        //        {
        //            //right
        //            case 0:
        //                //시간에 맞춰서 뿌려줘야 함. 
        //                startNoteManager.MakeRightNote(markNumber);

        //                break;

        //            //left
        //            case 1:
        //                startNoteManager.MakeLeftNote(markNumber);
        //                break;
        //        }
 

                


        //    }
        //    sr.Close();


        //}

        //타입 (r:0,l:1) // 마커번호 // 시간 // bps(속도) 

        #endregion
        
        

        

    }
}
