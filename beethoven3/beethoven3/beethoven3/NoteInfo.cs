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

        #region declarations
        private bool isRight;
        private double startTime;
        private int markLocation;
        #endregion

        #region constructor
        public NoteInfo(bool isRight, double startTime, int markLocation)
        {
            this.isRight = isRight;
            this.startTime = startTime;
            this.markLocation = markLocation;
        }
        #endregion

        #region method
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
        #endregion

    }
}
