﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beethoven3
{
    /// <summary>
    /// 
    /// </summary>
    class NoteInfo
    {
        private bool isRight;
        private double startTime;
        private int markLocation;


        public NoteInfo(bool isRight, double startTime, int markLocation)
        {
            this.isRight = isRight;
            this.startTime = startTime;
            this.markLocation = markLocation;
        }


        public bool IsRight
        {
            get { return isRight; }
            set { isRight = value; }
        }

        public double StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public int MarkLocation
        {
            get { return markLocation; }
            set { markLocation = value; }
        }


    }
}
