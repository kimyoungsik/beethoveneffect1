using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beethoven3
{
    class ScoreManager
    {

        private  int perfect;
        private  int good;
        private  int bad;
        private  int max;
        private  int otherScore;
        private  int combo;
        private  int perfomance;
        private  int totalScore;
        private  String rank;
        private  int gold; 

        public  ScoreManager()
        {
            perfect = 0;
            good = 0;
            bad = 0;
            max = 0;
            combo = 0;
            perfomance = 0;
            totalScore = 0;
            gold = 0;
            otherScore = 0;
            rank = "c";
        }

        public int Perfect
        {
            get { return perfect; }
            set { perfect = value; }
        }

        public int Good
        {
            get { return good; }
            set { good = value; }
        }


        public int Bad
        {
            get { return bad; }
            set { bad = value; }
        }

        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        //롱노트 드래그노트
        public int OtherScore
        {
            get { return otherScore; }
            set { otherScore = value; }
        }

        public int Combo
        {
            get { return combo; }
            set { combo = value; }
        }

        public int Perfomance
        {
            get { return perfomance; }
            set { perfomance = value; }
        }

        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }

        public String Rank
        {
            get { return rank; }
            set { rank = value; }
        }


        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }



    }
}