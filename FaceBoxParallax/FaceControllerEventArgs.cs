using System;
using System.Collections.Generic;

namespace FaceBoxParallax
{
    public class FaceControllerEventArgs : EventArgs
    {
        public Tuple<double, double> Coords { get; set; }
    }
}