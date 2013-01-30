namespace DTWGestureRecognition
{
    using System;
    using System.Windows;
    using Microsoft.Kinect;

    internal class Skeleton2DDataExtract
    {
        public delegate void Skeleton2DdataCoordEventHandler(object sender, Skeleton2DdataCoordEventArgs a);
        public static event Skeleton2DdataCoordEventHandler Skeleton2DdataCoordReady;

        public static void ProcessData(Skeleton data)
        {
            var p = new Point[6];
            Point shoulderRight = new Point(), shoulderLeft = new Point();
            
            foreach (Joint j in data.Joints)
            {
                switch (j.JointType)
                {
                    case JointType.HandLeft:
                        p[0] = new Point(j.Position.X, j.Position.Y);
                        break;
                    case JointType.WristLeft:
                        p[1] = new Point(j.Position.X, j.Position.Y);
                        break;
                    case JointType.ElbowLeft:
                        p[2] = new Point(j.Position.X, j.Position.Y);
                        break;
                    case JointType.ElbowRight:
                        p[3] = new Point(j.Position.X, j.Position.Y);
                        break;
                    case JointType.WristRight:
                        p[4] = new Point(j.Position.X, j.Position.Y);
                        break;
                    case JointType.HandRight:
                        p[5] = new Point(j.Position.X, j.Position.Y);
                        break;
                    case JointType.ShoulderLeft:
                        shoulderLeft = new Point(j.Position.X, j.Position.Y);
                        break;
                    case JointType.ShoulderRight:
                        shoulderRight = new Point(j.Position.X, j.Position.Y);
                        break;
                }
            }

            var center = new Point((shoulderLeft.X + shoulderRight.X) / 2, (shoulderLeft.Y + shoulderRight.Y) / 2);
            for (int i = 0; i < 6; i++)
            {
                p[i].X -= center.X;
                p[i].Y -= center.Y;
            }

            double shoulderDist =
                Math.Sqrt(Math.Pow((shoulderLeft.X - shoulderRight.X), 2) +
                          Math.Pow((shoulderLeft.Y - shoulderRight.Y), 2));
            for (int i = 0; i < 6; i++)
            {
                p[i].X /= shoulderDist;
                p[i].Y /= shoulderDist;
            }

            Skeleton2DdataCoordReady(null, new Skeleton2DdataCoordEventArgs(p));
        }
    }
}