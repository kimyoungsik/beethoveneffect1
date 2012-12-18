using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beethoven3
{
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

        //5개 안에 점수 가 있을 때
        public bool HighScore(ScoreInfo scoreInfo)
        {
            bool ret = false;
            int i;
            int max;

            //높은 순으로 정렬 해야 한다. 
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
                if (scoreInfos[i] == scoreInfo)
                {
                    ret = true;
                    i = max;
                }
            }
            return ret;
        }

        public void Order()
        {
              scoreInfos = scoreInfos.OrderByDescending(x => x.Score).ToList();
        }

        //5개 점수 가져오기
        public List<int> GetFiveScore()
        {
            
            int i;
            int max;
            List<int> retListInt = new List<int>();
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
                retListInt.Add(scoreInfos[i].Score);

            }


            return retListInt;
        }




    }
}
