﻿using System;
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
        private  int perfect;
        private  int good;
        private  int bad;

        private int gold;
        

        private  int longNoteScore;
        private  int dragNoteScore;
        
        
        private  int combo;

        private int max;

        private  int perfomance;
        
        private  int totalScore;
        
        private  String rank;
        #endregion

        #region constructor
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
            longNoteScore = 0;
            dragNoteScore = 0;
        
            rank = "";
        }
        #endregion

        #region method
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

        //롱노트 
        public int LongNoteScore
        {
            get { return longNoteScore; }
            set { longNoteScore = value; }
        }

        public int DragNoteScore
        {
            get { return dragNoteScore; }
            set { dragNoteScore = value; }
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
            TotalScore = 
            (Perfect * 20)+
            (Good * 10)+
            (Perfomance * 100)+
            (LongNoteScore)+
            (DragNoteScore * 10);
        }

        #endregion
    }
}