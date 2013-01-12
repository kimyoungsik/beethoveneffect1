using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace beethoven3
{
    
    class PointT
    {
        public double X;
        public double Y;
        public PointT()
        {
        }
        public PointT(double _x, double _y)
        {
            X = _x;
            Y = _y;
        }
        public PointT(float _x, float _y)
        {
            X =(double) _x;
            Y = (double)_y;
        }

    }
}
