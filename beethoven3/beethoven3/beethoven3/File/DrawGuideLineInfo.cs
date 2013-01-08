using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beethoven3
{
    class DrawGuideLineInfo
    {
        private int startMarkLocation;
        private int endMarkLocation;
        private bool gold;
        private double firstStartTime;
        private double secondStartTime;

        public DrawGuideLineInfo(int startMarkLocation, int endMarkLocation, bool gold, double firstStartTime, double secondStartTime)
        {
            this.startMarkLocation = startMarkLocation;
            this.endMarkLocation = endMarkLocation;
            this.gold = gold;
            this.firstStartTime = firstStartTime;
            this.secondStartTime = secondStartTime;
        }


        public int StartMarkLocation
        {
            get { return startMarkLocation; }
            set { startMarkLocation = value; }
        }

        public int EndMarkLocation
        {
            get { return endMarkLocation; }
            set { endMarkLocation = value; }
        }


        public bool Gold
        {
            get { return gold; }
            set { gold = value; }
        }

        public double FirstStartTime
        {
            get { return firstStartTime; }
            set { firstStartTime = value; }
        }

        public double SecondStartTime
        {
            get { return secondStartTime; }
            set { secondStartTime = value; }
        }

    }
}
