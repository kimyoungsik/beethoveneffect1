using System;
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
                tw.WriteLine("%%");
                tw.WriteLine(scoreInfoManagers[i].SongName);
                List<ScoreInfo> scoreInfos = scoreInfoManagers[i].GetScoreInfos();
                for (j = 0; j < scoreInfos.Count; j++)
                {
                    tw.WriteLine(scoreInfos[j].UserPicture + '$' + scoreInfos[j].Score);
                }   
            }
            tw.WriteLine("**");
            tw.Close();
        }


        public void LoadReport()
        {

            StreamReader sr = new StreamReader("c:\\beethovenRecord\\record.txt");
            String line;
            String songTitle=null;
            String[] contents;
            line = sr.ReadLine();
            while (line != "**")//처음
               {
                   if (line == "%%")
                   {
                       //노래 제목
                       songTitle = sr.ReadLine();
                   }
                   else
                   {
                       contents = ((String)line).Split('$');
                       this.AddSongInfoManager(songTitle, Int32.Parse(contents[1]), contents[0]); // contents[0] 그림 , contents[1]  점수
                   }
                   line = sr.ReadLine();          
            }     
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
