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
            this.userPicture = userPicture; ;
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
}