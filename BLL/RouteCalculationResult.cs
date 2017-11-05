using System.Collections.Generic;

namespace BLL
{
    public class RouteCalculationResult
    {
        public RouteCalculationMode Mode { get; set; }
        public float TotalLength { get; set; }
        public float TotalTime { get; set; }
        public List<RouteCalculationCheckpointResult> Checkpoints { get; set; } = new List<RouteCalculationCheckpointResult>();

    }
}
