using System;
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

        private double startTime;
        private int markLocation;


        public NoteInfo(double startTime, int markLocation)
        {
            this.startTime = startTime;
            this.markLocation = markLocation;
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
