using System.Collections.Generic;
using System.Drawing;

namespace BLL
{
    public class RouteCalculationCheckpointResult
    {
        public PointF StartPoint { get; set; }
        public PointF FinishPoint { get; set; }
        public List<PointF> WKTPath { get; set; }
        public float Length { get; set; }
        public float Time { get; set; }
    }
}
