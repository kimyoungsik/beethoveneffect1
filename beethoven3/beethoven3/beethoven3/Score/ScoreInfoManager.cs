using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beethoven3
{
    class ScoreInfo
    {
        private String userPicture;
        private int score;

        public ScoreInfo(String userPicture, int score)
        {
            this.userPicture = userPicture;
            this.score = score;
        }

        public String UserPicture
        {
            get { return userPicture; }
            set { userPicture = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }

        }
    }
    class ScoreInfoManager
    {
        private String songName;
        private List<ScoreInfo> scoreInfos = new List<ScoreInfo>();

        public ScoreInfoManager()
        {

        }

        public String SongName
        {
            get { return songName; }
            set { songName = value; }
        }

        public void AddScoreInfo(ScoreInfo scoreInfo)
        {
            scoreInfos.Add(scoreInfo);
        }

        public List<ScoreInfo> GetScoreInfos()
        {
            Order();
            return scoreInfos;

        }

        //5개 안에 점수 가 있을 때
        public bool IsHighScore(int score)
        {
            bool ret = false;
            
            int last= 4;

            //높은 순으로 정렬 해야 한다. 
            Order();

            if (scoreInfos.Count < 5)
            {
                ret = true;
            }
            else
            {
                if (score >= scoreInfos[last].Score)
                {
                    ret = true;

                }
            }
          
            return ret;
        }


        public void Order()
        {
              scoreInfos = scoreInfos.OrderByDescending(x => x.Score).ToList();
        }

        //5개 점수 가져오기
        public List<ScoreInfo> GetFiveScore()
        {
            Order();
            int i;
            int max;
            List<ScoreInfo> retList = new List<ScoreInfo>();
            //max = scoreInfos.Count > 5 ? 5 : scoreInfos.Count;
            
            if (scoreInfos.Count > 5)
            {
                max = 5;
            }
            else
            {
                max = scoreInfos.Count;
            }

            for (i = 0; i < max; i++)
            {
                retList.Add(scoreInfos[i]);

            }


            return retList;
        }




    }
}
