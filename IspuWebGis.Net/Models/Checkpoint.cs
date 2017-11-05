using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using BLL;

namespace IspuWebGis.Net.Models
{
    public class Checkpoint
    {
        public Checkpoint(RouteCalculationCheckpointResult checkpoint)
        {
            startPoint = new ClientPoint(checkpoint.StartPoint);
            finishPoint = new ClientPoint(checkpoint.FinishPoint);
            WKTPath = checkpoint.WKTPath;
            time = checkpoint.Time;
            length = checkpoint.Length;
        }

        public ClientPoint startPoint;
        public ClientPoint finishPoint;
        public List<PointF> WKTPath;
        public float length;
        public float time;
    }
}