namespace DTWGestureRecognition
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Linq;
    using Microsoft.Kinect;
    
    public partial class MainWindow
    {
        private const int RedIdx = 2;
        private const int GreenIdx = 1;
        private const int BlueIdx = 0;
        private const int Ignore = 2;
        private const int BufferSize = 32;
        private const int MinimumFrames = 6;
        private const int CaptureCountdownSeconds = 3;
        private const string GestureSaveFileLocation = @"C:\Users\ssm\Desktop\DTWUpdated1.5\DTWGestureRecognition\";
        private const string GestureSaveFileNamePrefix = @"RecordedGestures";
        private readonly Dictionary<JointType, Brush> _jointColors = new Dictionary<JointType, Brush>
        { 
            {JointType.HipCenter, new SolidColorBrush(Color.FromRgb(169, 176, 155))},
            {JointType.Spine, new SolidColorBrush(Color.FromRgb(169, 176, 155))},
            {JointType.ShoulderCenter, new SolidColorBrush(Color.FromRgb(168, 230, 29))},
            {JointType.Head, new SolidColorBrush(Color.FromRgb(200, 0, 0))},
            {JointType.ShoulderLeft, new SolidColorBrush(Color.FromRgb(79, 84, 33))},
            {JointType.ElbowLeft, new SolidColorBrush(Color.FromRgb(84, 33, 42))},
            {JointType.WristLeft, new SolidColorBrush(Color.FromRgb(255, 126, 0))},
            {JointType.HandLeft, new SolidColorBrush(Color.FromRgb(215, 86, 0))},
            {JointType.ShoulderRight, new SolidColorBrush(Color.FromRgb(33, 79,  84))},
            {JointType.ElbowRight, new SolidColorBrush(Color.FromRgb(33, 33, 84))},
            {JointType.WristRight, new SolidColorBrush(Color.FromRgb(77, 109, 243))},
            {JointType.HandRight, new SolidColorBrush(Color.FromRgb(37,  69, 243))},
            {JointType.HipLeft, new SolidColorBrush(Color.FromRgb(77, 109, 243))},
            {JointType.KneeLeft, new SolidColorBrush(Color.FromRgb(69, 33, 84))},
            {JointType.AnkleLeft, new SolidColorBrush(Color.FromRgb(229, 170, 122))},
            {JointType.FootLeft, new SolidColorBrush(Color.FromRgb(255, 126, 0))},
            {JointType.HipRight, new SolidColorBrush(Color.FromRgb(181, 165, 213))},
            {JointType.KneeRight, new SolidColorBrush(Color.FromRgb(71, 222, 76))},
            {JointType.AnkleRight, new SolidColorBrush(Color.FromRgb(245, 228, 156))},
            {JointType.FootRight, new SolidColorBrush(Color.FromRgb(77, 109, 243))}
        };
        private readonly short[] _depthFrame32 = new short[320 * 240 * 4];
        private bool _capturing;
        private DtwGestureRecognizer _dtw;
        private int _lastFrames;
        private DateTime _lastTime = DateTime.MaxValue;
        private KinectSensor  _nui;
        private int _totalFrames;
        private int _flipFlop;
        private ArrayList _video;
        private DateTime _captureCountdown = DateTime.Now;
        private Timer _captureCountdownTimer;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void LoadGesturesFromFile(string fileLocation)
        {
            int itemCount = 0;
            string line;
            string gestureName = String.Empty;

            
            ArrayList frames = new ArrayList();
            double[] items = new double[12];


            System.IO.StreamReader file = new System.IO.StreamReader(fileLocation);
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("@"))
                {
                    gestureName = line;
                    continue;
                }

                if (line.StartsWith("~"))
                {
                    frames.Add(items);
                    itemCount = 0;
                    items = new double[12];
                    continue;
                }

                if (!line.StartsWith("----"))
                {
                    items[itemCount] = Double.Parse(line);
                }

                itemCount++;

                if (line.StartsWith("----"))
                {
                    _dtw.AddOrUpdate(frames, gestureName);
                    frames = new ArrayList();
                    gestureName = String.Empty;
                    itemCount = 0;
                }
            }

            file.Close();
        }

        private static void SkeletonExtractSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null) return; // sometimes frame image comes null, so skip it.
                var skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                skeletonFrame.CopySkeletonDataTo(skeletons);

                foreach (Skeleton data in skeletons)
                {
                    Skeleton2DDataExtract.ProcessData(data);
                }
            }
        }


        #region NoNeed
        private short[] ConvertDepthFrame(short[] depthFrame16)
        {
            for (int i16 = 0, i32 = 0; i16 < depthFrame16.Length && i32 < _depthFrame32.Length; i16 += 2, i32 += 4)
            {
                int player = depthFrame16[i16] & 0x07;
                int realDepth = (depthFrame16[i16 + 1] << 5) | (depthFrame16[i16] >> 3);
                
                var intensity = (short)(255 - (255 * realDepth / 0x0fff));

                _depthFrame32[i32 + RedIdx] = 0;
                _depthFrame32[i32 + GreenIdx] = 0;
                _depthFrame32[i32 + BlueIdx] = 0;

                switch (player)
                {
                    case 0:
                        _depthFrame32[i32 + RedIdx] = (byte)(intensity / 2);
                        _depthFrame32[i32 + GreenIdx] = (byte)(intensity / 2);
                        _depthFrame32[i32 + BlueIdx] = (byte)(intensity / 2);
                        break;
                    case 1:
                        _depthFrame32[i32 + RedIdx] = intensity;
                        break;
                    case 2:
                        _depthFrame32[i32 + GreenIdx] = intensity;
                        break;
                    case 3:
                        _depthFrame32[i32 + RedIdx] = (byte)(intensity / 4);
                        _depthFrame32[i32 + GreenIdx] = intensity;
                        _depthFrame32[i32 + BlueIdx] = intensity;
                        break;
                    case 4:
                        _depthFrame32[i32 + RedIdx] = intensity;
                        _depthFrame32[i32 + GreenIdx] = intensity;
                        _depthFrame32[i32 + BlueIdx] = (byte)(intensity / 4);
                        break;
                    case 5:
                        _depthFrame32[i32 + RedIdx] = intensity;
                        _depthFrame32[i32 + GreenIdx] = (byte)(intensity / 4);
                        _depthFrame32[i32 + BlueIdx] = intensity;
                        break;
                    case 6:
                        _depthFrame32[i32 + RedIdx] = (byte)(intensity / 2);
                        _depthFrame32[i32 + GreenIdx] = (byte)(intensity / 2);
                        _depthFrame32[i32 + BlueIdx] = intensity;
                        break;
                    case 7:
                        _depthFrame32[i32 + RedIdx] = (byte)(255 - intensity);
                        _depthFrame32[i32 + GreenIdx] = (byte)(255 - intensity);
                        _depthFrame32[i32 + BlueIdx] = (byte)(255 - intensity);
                        break;
                }
            }

            return _depthFrame32;
        }
        #endregion

        private void NuiDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (var image = e.OpenDepthImageFrame())
            {
                if (image == null) return; // sometimes frame image comes null, so skip it.

                depthImage.Source = image.ToBitmapSource();
            }
            ++_totalFrames;

            DateTime cur = DateTime.Now;
            if (cur.Subtract(_lastTime) > TimeSpan.FromSeconds(1))
            {
                int frameDiff = _totalFrames - _lastFrames;
                _lastFrames = _totalFrames;
                _lastTime = cur;
                //frameRate.Text = frameDiff + " fps";
            }
        }


        #region NoNeed
        //private Point GetDisplayPosition(Joint joint)
        //{
        //    float depthX, depthY;
        //    var pos = _nui.MapSkeletonPointToDepth(joint.Position, DepthImageFormat.Resolution320x240Fps30);

        //    depthX = pos.X;
        //    depthY = pos.Y;

        //    int colorX, colorY;

        //    var pos2 = _nui.MapSkeletonPointToColor(joint.Position, ColorImageFormat.RgbResolution640x480Fps30);
        //    colorX = pos2.X;
        //    colorY = pos2.Y;

        //    return new Point((int)(skeletonCanvas.Width * colorX / 640.0), (int)(skeletonCanvas.Height * colorY / 480));
        //}

        //private Polyline GetBodySegment(JointCollection joints, Brush brush, params JointType[] ids)
        //{

        //    //var points = new PointCollection(ids.Length);
        //    //foreach (JointType t in ids)
        //    //{
        //    //    points.Add(GetDisplayPosition(joints[t]));
        //    //}

        //    var polyline = new Polyline();
        //    polyline.Points = points;
        //    polyline.Stroke = brush;
        //    polyline.StrokeThickness = 5;
        //    return polyline;
        //}

        private void NuiSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons;
            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame == null) return;
                skeletons = new Skeleton[frame.SkeletonArrayLength];
                frame.CopySkeletonDataTo(skeletons);
            }

            int iSkeleton = 0;
            var brushes = new Brush[6];
            brushes[0] = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            brushes[1] = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            brushes[2] = new SolidColorBrush(Color.FromRgb(64, 255, 255));
            brushes[3] = new SolidColorBrush(Color.FromRgb(255, 255, 64));
            brushes[4] = new SolidColorBrush(Color.FromRgb(255, 64, 255));
            brushes[5] = new SolidColorBrush(Color.FromRgb(128, 128, 255));

            //skeletonCanvas.Children.Clear();
            //foreach (var data in skeletons)
            //{
            //    if (SkeletonTrackingState.Tracked == data.TrackingState)
            //    {
            //        Brush brush = brushes[iSkeleton % brushes.Length];
            //        skeletonCanvas.Children.Add(GetBodySegment(data.Joints, brush, JointType.HipCenter, JointType.Spine, JointType.ShoulderCenter, JointType.Head));
            //        skeletonCanvas.Children.Add(GetBodySegment(data.Joints, brush, JointType.ShoulderCenter, JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, JointType.HandLeft));
            //        skeletonCanvas.Children.Add(GetBodySegment(data.Joints, brush, JointType.ShoulderCenter, JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, JointType.HandRight));
            //        skeletonCanvas.Children.Add(GetBodySegment(data.Joints, brush, JointType.HipCenter, JointType.HipLeft, JointType.KneeLeft, JointType.AnkleLeft, JointType.FootLeft));
            //        skeletonCanvas.Children.Add(GetBodySegment(data.Joints, brush, JointType.HipCenter, JointType.HipRight, JointType.KneeRight, JointType.AnkleRight, JointType.FootRight));

            //        foreach (Joint joint in data.Joints)
            //        {
            //            Point jointPos = GetDisplayPosition(joint);
            //            var jointLine = new Line();
            //            jointLine.X1 = jointPos.X - 3;
            //            jointLine.X2 = jointLine.X1 + 6;
            //            jointLine.Y1 = jointLine.Y2 = jointPos.Y;
            //            jointLine.Stroke = _jointColors[joint.JointType];
            //            jointLine.StrokeThickness = 6;
            //            skeletonCanvas.Children.Add(jointLine);
            //        }
            //    }

            //    iSkeleton++;
            //} 
        }

        private void NuiColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var image = e.OpenColorImageFrame())
            {
                if (image == null) return;

                videoImage.Source = image.ToBitmapSource();
            }
        }

        #endregion

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            _nui = (from i in KinectSensor.KinectSensors
                    where i.Status == KinectStatus.Connected
                    select i).FirstOrDefault();

            if (_nui == null)
                throw new NotSupportedException("No kinectes connected!");

            try
            {
                _nui.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                _nui.SkeletonStream.Enable();
                _nui.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                _nui.Start();
            }
            catch (InvalidOperationException)
            {
                System.Windows.MessageBox.Show("Runtime initialization failed. Please make sure Kinect device is plugged in.");
                return;
            }

            _lastTime = DateTime.Now;

            //_dtw = new DtwGestureRecognizer(12, 0.6, 2, 2, 10);
            _dtw = new DtwGestureRecognizer(12, 0.6, 2, 2, 10);
            _video = new ArrayList();

            _nui.DepthFrameReady += NuiDepthFrameReady;

            _nui.SkeletonFrameReady += NuiSkeletonFrameReady;
            _nui.SkeletonFrameReady += SkeletonExtractSkeletonFrameReady;

            _nui.ColorFrameReady += NuiColorFrameReady;

            Skeleton2DDataExtract.Skeleton2DdataCoordReady += NuiSkeleton2DdataCoordReady;

            dtwTextOutput.Text = _dtw.RetrieveText();

            Debug.WriteLine("Finished Window Loading");
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            Debug.WriteLine("Stopping NUI");
            _nui.Stop();
            Debug.WriteLine("NUI stopped");
            Environment.Exit(0);
        }

        private void NuiSkeleton2DdataCoordReady(object sender, Skeleton2DdataCoordEventArgs a)
        {
            currentBufferFrame.Text = _video.Count.ToString();

            if (_video.Count > MinimumFrames && _capturing == false)
            {
                ////Debug.WriteLine("Reading and video.Count=" + video.Count);
                string s = _dtw.Recognize(_video);
                results.Text = "Recognize: " + s;
                if (!s.Contains("__UNKNOWN"))
                {
                    _video = new ArrayList();
                }
            }

            if (_video.Count > BufferSize)
            {
                
                if (_capturing)
                {
                    DtwStoreClick(null, null);
                }
                else
                {
                    _video.RemoveAt(0);
                }
            }

            if (!double.IsNaN(a.GetPoint(0).X))
            {

                _flipFlop = (_flipFlop + 1) % Ignore;
                if (_flipFlop == 0)
                {
                    _video.Add(a.GetCoords());
                }
            }

            //dtwTextOutput.Text = _dtw.RetrieveText();
        }

        private void DtwReadClick(object sender, RoutedEventArgs e)
        {
            //dtwRead.IsEnabled = false;
            dtwCapture.IsEnabled = true;
            //dtwStore.IsEnabled = false;
            _capturing = false;
            status.Text = "Reading";
        }

        private void DtwCaptureClick(object sender, RoutedEventArgs e)
        {
            _captureCountdown = DateTime.Now.AddSeconds(CaptureCountdownSeconds);

            _captureCountdownTimer = new Timer();
            _captureCountdownTimer.Interval = 50;
            _captureCountdownTimer.Start();
            _captureCountdownTimer.Tick += CaptureCountdown;
            //dtwTextOutput.Text = _dtw.RetrieveText();
        }

        private void CaptureCountdown(object sender, EventArgs e)
        {
            if (sender == _captureCountdownTimer)
            {
                if (DateTime.Now < _captureCountdown)
                {
                    status.Text = "Wait " + ((_captureCountdown - DateTime.Now).Seconds + 1) + " seconds";
                }
                else
                {
                    _captureCountdownTimer.Stop();
                    status.Text = "Training Gesture";
                    StartCapture();
                }
            }
        } 


        private void StartCapture()
        {

            //dtwRead.IsEnabled = false;
            dtwCapture.IsEnabled = false;
            //dtwStore.IsEnabled = true;

            _capturing = true;

            //_captureCountdownTimer.Dispose();

            status.Text = "Training Gesture" + gestureList.Text;

            _video = new ArrayList();
        }


        private void DtwStoreClick(object sender, RoutedEventArgs e)
        {

            //dtwRead.IsEnabled = false;
            dtwCapture.IsEnabled = true;
            //dtwStore.IsEnabled = false;
            _capturing = false;

            status.Text = "Remembering " + gestureList.Text;

            _dtw.AddOrUpdate(_video, gestureList.Text);
            results.Text = "Gesture " + gestureList.Text + "added";

            _video = new ArrayList();

            DtwReadClick(null, null);
        }


        private void DtwSaveToFile(object sender, RoutedEventArgs e)
        {
            string fileName = "Gesture " + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".txt";
            System.IO.File.WriteAllText(fileName, _dtw.RetrieveText());
            status.Text = "Save " + fileName;
        }


        private void DtwLoadFile(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            dlg.InitialDirectory = GestureSaveFileLocation;

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                LoadGesturesFromFile(dlg.FileName);
                dtwTextOutput.Text = _dtw.RetrieveText();
                status.Text = "Gesture load";
            } 
        }

        private void DtwShowGestureText(object sender, RoutedEventArgs e)
        {
            dtwTextOutput.Text = _dtw.RetrieveText();
        }

        private void dtwAngleUp_Click(object sender, RoutedEventArgs e)
        {
            if (_nui.ElevationAngle < _nui.MaxElevationAngle - 3)
            {
                status.Text = "Angle Up";
                while (true)
                {
                    try
                    {
                        _nui.ElevationAngle += 3;
                        break;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                status.Text = "Max Angle";
            }
        }

        private void dtwAngleDown_Click(object sender, RoutedEventArgs e)
        {
            if (_nui.ElevationAngle > _nui.MinElevationAngle + 3)
            {
                status.Text = "Angle Down";
                while (true)
                {
                    try
                    {
                        _nui.ElevationAngle -= 3;
                        break;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                status.Text = "Min Angle";
            }
        }
    }
}