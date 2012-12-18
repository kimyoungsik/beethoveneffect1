using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beethoven3
{
    class ReportManager
    {
        private List<ScoreInfoManager> scoreInfoManagers = new List<ScoreInfoManager>();

        public ReportManager()
        {
            
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

        //  







    }
}
