using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace IspuWebGis.Net.Models
{
    public class ClientPoint
    {
        public ClientPoint(PointF point)
        {
            x = point.X;
            y = point.Y;
        }
        public float x { get; set; }
        public float y { get; set; }

        public PointF toPointF()
        {
            return new PointF(x, y);
        }
    }
}