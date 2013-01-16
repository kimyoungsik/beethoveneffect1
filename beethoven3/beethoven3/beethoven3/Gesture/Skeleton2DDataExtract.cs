using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework;
using Microsoft.Kinect;

namespace beethoven3
{
    internal class Skeleton2DDataExtract
    {

        public delegate void Skeleton2DdataCoordEventHandler(object sender, Skeleton2DdataCoordEventArgs a);
        
        public static event Skeleton2DdataCoordEventHandler Skeleton2DdataCoordReady;

        public static void ProcessData(Skeleton data)
        {
            var p = new PointT[6];
            PointT shoulderRight = new PointT(), shoulderLeft = new PointT();

            foreach (Joint j in data.Joints)
            {
                switch (j.JointType)
                {
                    case JointType.HandLeft:
                        p[0] = new PointT(j.Position.X, j.Position.Y);

                        break;
                    case JointType.WristLeft:
                        p[1] = new PointT(j.Position.X, j.Position.Y);
                        break;
                    case JointType.ElbowLeft:
                        p[2] = new PointT(j.Position.X, j.Position.Y);
                        break;
                    case JointType.ElbowRight:
                        p[3] = new PointT(j.Position.X, j.Position.Y);
                        break;
                    case JointType.WristRight:
                        p[4] = new PointT(j.Position.X, j.Position.Y);
                        break;
                    case JointType.HandRight:
                        p[5] = new PointT(j.Position.X, j.Position.Y);
                        break;
                    case JointType.ShoulderLeft:
                        shoulderLeft = new PointT(j.Position.X, j.Position.Y);
                        break;
                    case JointType.ShoulderRight:
                        shoulderRight = new PointT(j.Position.X, j.Position.Y);
                        break;
                }


            }

            var center = new PointT((shoulderLeft.X + shoulderRight.X) / 2, (shoulderLeft.Y + shoulderRight.Y) / 2);
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
