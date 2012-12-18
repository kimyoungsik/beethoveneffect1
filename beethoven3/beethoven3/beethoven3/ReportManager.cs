﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace beethoven3
{
    class ReportManager
    {
        private List<ScoreInfoManager> scoreInfoManagers = new List<ScoreInfoManager>();

        public ReportManager()
        {
            
        }
        
        public void SaveReport()
        {
            TextWriter tw = new StreamWriter("c:\\beethovenRecord\\record.txt");
            int i, j;
            for (i = 0; i < scoreInfoManagers.Count; i++)
            {
                tw.WriteLine("**");
                tw.WriteLine(scoreInfoManagers[i].SongName);
                List<ScoreInfo> scoreInfos = scoreInfoManagers[i].GetScoreInfos();
                for (j = 0; j < scoreInfos.Count; j++)
                {
                    tw.WriteLine(scoreInfos[j].UserPicture + "$$" + scoreInfos[j].Score);
                }           
            }
            tw.Close();
        }


        public void Loading()
        {

            StreamReader sr = new StreamReader("c:\\beethovenRecord\\record.txt");
           String line;

           while (sr.Peek() >= 0)
           {
               line = sr.ReadLine();
               if (line == "**")//처음
               {
                   sr.ReadLine();
                   

               
               
               }


           }

            noteLine = ((String)line).Split(' ');
                        
            sr.Close();
        }






        //곡 이름에 따라서 점수 가져오기

        public List<int> GetHighScore(String songName)
        {
            List<int> highScores = new List<int>();
            int i;
            for (i = 0; i < scoreInfoManagers.Count; i++)
            {
                if (scoreInfoManagers[i].SongName == songName)
                {
                    highScores = scoreInfoManagers[i].GetFiveScore();
                }
            }
            return highScores;
        }
        //곡이름에 따라 새로 등록된 것인지 체크
        public bool IsHighScore(String songName, int score)
        {
            bool ret = false;

            int i;

          
                for (i = 0; i < scoreInfoManagers.Count; i++)
                {
                    if (scoreInfoManagers[i].SongName == songName)
                    {
                        ret = scoreInfoManagers[i].IsHighScore(score);
                        i = scoreInfoManagers.Count;
                    }

                }
          
            return ret;


        }
        // 곡 이름에 따라 찾아서 더하기

        public void AddSongInfoManager(String songName, int score, String userPicture)
        {
            int j = -1;
            int i;

            for (i = 0; i < scoreInfoManagers.Count; i++)
            {
                if (scoreInfoManagers[i].SongName == songName)
                {
                    j = i;
                    i = scoreInfoManagers.Count;
                }
            }
            //곡이름이 같은것이 있으면
            if (j != -1)
            {
                scoreInfoManagers[j].AddScoreInfo(new ScoreInfo(userPicture, score));
            }
            //없으면
            else
            {
                ScoreInfoManager newScoreInfoManager = new ScoreInfoManager();
                newScoreInfoManager.SongName = songName;
                newScoreInfoManager.AddScoreInfo(new ScoreInfo(userPicture,score));

                scoreInfoManagers.Add(newScoreInfoManager);
            }
        }
    }
}
