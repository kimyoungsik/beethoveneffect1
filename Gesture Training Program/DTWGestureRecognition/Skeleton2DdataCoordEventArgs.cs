namespace DTWGestureRecognition
{
    using System.Windows;


    internal class Skeleton2DdataCoordEventArgs
    {
        private readonly Point[] _points;

        public Skeleton2DdataCoordEventArgs(Point[] points)
        {
            _points = (Point[]) points.Clone();
        }

        public Point GetPoint(int index)
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