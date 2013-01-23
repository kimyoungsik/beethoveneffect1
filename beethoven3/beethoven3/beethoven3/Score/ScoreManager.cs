using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace beethoven3
{
    /// <summary>
    /// 마지막에 max꼭 체크!!!!!!
    /// 
    /// </summary>
    class ScoreManager
    {
        #region declarations
        
        //곡제목
        private String songName;

        //기본노트 카운트
        private int oneHandPerfect;
        private int oneHandGood;
        private int oneHandBad;
        private int oneHandMiss;

        //롱노트 카운트
        private int longPerfect;
        private int longMiss;

        //드래그

        private int dragPerfect;
        private int dragGood;
        private int dragMiss;


        //드래그 카운트
        private int posturePerfect;
        private int postureMiss;

        //제스쳐

        private int jesturePerfect;
        private int jestureGood;
        private int jestureMiss;



        private int gold;

        private int combo;
        
        private int max;


        private  int perfomance;
        
        private  int totalScore;

        private int gage;

        private int totalGold;

        private  String rank;

        //기본노트 카운트
        private double oneHandPerfectPoint = 10;
        private double oneHandGoodPoint = 5;
        //private double oneHandBadPoint;
        //private double oneHandMissPoint;

        //롱노트 카운트
        private double longPerfectPoint= 0.5f;
        //private double longMissPoint;

        private double dragPerfectPoint = 5;
        private double dragGoodPoint = 2;
        //private double dragMissPoint ;


        //드래그 카운트
        private double posturePerfectPoint = 10;
        private double postureMissPoint = 1;

        //제스쳐

        private double jesturePerfectPoint = 50;
        private double jestureGoodPoint = 25;
        //private double jestureMissPoint =;

        private float standardGage = 100.0f;

        #endregion

        #region constructor
        public  ScoreManager()
        {
            songName = "";

            //기본노트 카운트

            oneHandPerfect=0;
            oneHandGood=0;
            oneHandBad=0;
            oneHandMiss=0;

            //롱노트 카운트
            longPerfect=0;
            longMiss=0;

            //드래그 카운트
            dragGood = 0;
            dragPerfect = 0;
            dragMiss = 0;
            //포스쳐
            posturePerfect=0;
            postureMiss=0;

            //제스쳐

            jesturePerfect=0;
            jestureGood=0;
            jestureMiss=0;


            max = 0;
            combo = 0;
            perfomance = 0;
            totalScore = 0;
            gold = 0;
           
            gage = 0;
          

            rank = "";
        }
        #endregion

        #region method


        public void init()
        {
            songName = "";

            //기본노트 카운트

            oneHandPerfect = 0;
            oneHandGood = 0;
            oneHandBad = 0;
            oneHandMiss = 0;

            //롱노트 카운트
            longPerfect = 0;
            longMiss = 0;

            //드래그 카운트
            dragGood = 0;
            dragPerfect = 0;
            dragMiss = 0;
            //포스쳐
            posturePerfect = 0;
            postureMiss = 0;

            //제스쳐

            jesturePerfect = 0;
            jestureGood = 0;
            jestureMiss = 0;


            max = 0;
            combo = 0;
            perfomance = 0;
            totalScore = 0;
            gold = 0;

            gage = 0;


            rank = "";

        }


        public int OneHandPerfect
        {
            get { return oneHandPerfect; }
            set { oneHandPerfect = value; }
        }

        public int OneHandGood
        {
            get { return oneHandGood; }
            set { oneHandGood = value; }
        }

        public int OneHandBad
        {
            get { return oneHandBad; }
            set { oneHandBad = value; }


        }

        public int OneHandMiss
        {
            get { return oneHandMiss; }
            set { oneHandMiss = value; }
        }

        public int LongPerfect
        {
            get { return longPerfect; }
            set { longPerfect = value; }
        }

        public int LongMiss
        {
            get { return longMiss; }
            set { longMiss = value; }
        }



        public int DragPerfect
        {
            get { return dragPerfect; }
            set { dragPerfect = value; }
        }


        public int DragGood
        {
            get { return dragGood; }
            set { dragGood = value; }
        }



        public int DragMiss
        {
            get { return dragMiss; }
            set { dragMiss = value; }
        }




        
        public int PosturePerfect
        {
            get { return posturePerfect; }
            set { posturePerfect = value; }
        }


        public int PostureMiss
        {
            get { return postureMiss; }
            set { postureMiss = value; }
        }

        public int JesturePerfect
        {
            get { return jesturePerfect; }
            set { jesturePerfect = value; }
        }

        public int JestureGood
        {
            get { return jestureGood; }
            set { jestureGood = value; }
        }


        public int JestureMiss
        {
            get { return jestureMiss; }
            set { jestureMiss = value; }
        }

        public int Gage
        {
            get { return gage; }
            set {
                if (value > 100)
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    value = 0;
                }
                
                gage = value; }
        }

        public String SongName
        {
            get { return songName; }
            set { songName = value; }
        }

       


        public int TotalGold
        {
            get { return totalGold; }
            set { totalGold = value; }
        }
     
        public int Max
        {
            get { return max; }
            set { max = value; }
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

       
        #endregion


        #region update
        //totalScore구하기
        public  void Update(GameTime gameTime)
        {
            TotalScore =(int)(
            (oneHandPerfect * oneHandPerfectPoint) +
            (oneHandGood * oneHandGoodPoint)+
            (longPerfect * longPerfectPoint) +
            (dragPerfect* dragPerfectPoint)+
            (dragGood * dragGoodPoint)+
            (posturePerfect * posturePerfectPoint)+
            (jesturePerfect * jesturePerfectPoint)+
            (jestureGood * jestureGoodPoint)
            );
        }

        #endregion
    }
}