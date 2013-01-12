using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace beethoven3
{
    internal class Skeleton2DdataCoordEventArgs
    {

        private readonly PointT[] _points;

        public Skeleton2DdataCoordEventArgs(PointT[] points)
        {
            _points = (PointT[])points.Clone();
        }

        public PointT GetPoint(int index)
        {
            return _points[index];
        }

        internal double[] GetCoords()
        {
            var tmp = new double[_points.Length * 2];
            for (int i = 0; i < _points.Length; i++)
            {
                tmp[2 * i] = _points[i].X;
                tmp[(2 * i) + 1] = _points[i].Y;
            }

            return tmp;
        }
    }
}
