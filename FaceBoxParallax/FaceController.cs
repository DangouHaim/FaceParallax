using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Threading;

namespace FaceBoxParallax
{
    public class FaceController
    {
        private VideoCapture _capture;
        private CascadeClassifier _cascadeClassifier;
        private FaceControllerEventArgs _eventArgs;
        private static double _lX = 0;
        private static double _lY = 0;
        private static double _deltaX = 0;
        private static double _deltaY = 0;
        private static int _outCount = 0;

        public event EventHandler<FaceControllerEventArgs> OnFaceMoving = (s, e) => 
        {
            if(Math.Abs(_deltaX - (_lX - e.Coords.Item1)) < 50)
            {
                _deltaX = _lX - e.Coords.Item1;
            }
            else
            {
                _deltaX = 0;
            }
            if (Math.Abs(_deltaY - (_lY - e.Coords.Item2)) < 50)
            {
                _deltaY = _lY - e.Coords.Item2;
            }
            else
            {
                _deltaY = 0;
            }


            _lX = e.Coords.Item1;
            _lY = e.Coords.Item2;

            _deltaY = (int)_deltaY / 2;
            _deltaY *= 2;
            _deltaX = (int)_deltaX / 2;
            _deltaX *= 2;
        };

        public event EventHandler<FaceControllerEventArgs> OnShift = (s, e) => { };

        public event EventHandler<EventArgs> OnReset = (s, e) => 
        {
            _lX = 0;
            _lY = 0;
            _deltaX = 0;
            _deltaY = 0;
        };

        public FaceController()
        {

            _capture = new VideoCapture();

            _cascadeClassifier = new CascadeClassifier(@"haarcascades\cascade.xml");
            _eventArgs = new FaceControllerEventArgs();
        }

        public void Start()
        {
            while(true)
            {
                var imageFrame = _capture.QueryFrame().ToImage<Bgr, Byte>();




                if (imageFrame != null)
                {
                    var grayframe = imageFrame.Convert<Gray, byte>();
                    //var gaus = grayframe.ThresholdAdaptive(new Gray(255), AdaptiveThresholdType.GaussianC, ThresholdType.Binary, 115, new Gray(1));
                    var faces = _cascadeClassifier.DetectMultiScale(grayframe, 1.5, 1, Size.Empty);


                    if (faces.Any())
                    {
                        var face = faces.First();
                        _eventArgs.Coords = new Tuple<double, double>(face.X, face.Y);
                        OnFaceMoving(this, _eventArgs);
                        OnShift(this, new FaceControllerEventArgs()
                        {
                            Coords = new Tuple<double, double>(_deltaX, _deltaY)
                        });
                        _outCount = 0;
                    }
                    else
                    {
                        _outCount++;
                        if(_outCount > 20)
                        {
                            OnReset(this, new EventArgs());
                        }
                    }
                }
                else
                {
                    OnReset(this, new EventArgs());
                }
                GC.Collect();
                Thread.Sleep(10);
            }
        }
    }
}
