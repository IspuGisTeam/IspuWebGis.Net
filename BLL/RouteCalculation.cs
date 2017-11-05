using System;
using System.Collections.Generic;
using System.Drawing;

namespace BLL
{
    public class RouteCalculation
    {
        public RouteCalculationResult Calculate(PointF startPoint, List<PointF> checkpoints, RouteCalculationMode mode = RouteCalculationMode.ShortRoute)
        {
            var result = new RouteCalculationResult
            {
                Mode = mode
            };

            var graph = RoadGraph.getInstance();
            var list = new List<List<Tuple<PointF, PointF>>>();

            foreach (var checkpoint in checkpoints)
            {
                var localResult = graph.CalculatePathBetweenPoints(startPoint, checkpoint, mode);
                result.Checkpoints.Add(localResult);
                result.TotalLength += localResult.Length;
                result.TotalTime += localResult.Time;
                startPoint = checkpoint;
            }

            return result;
        }

    }
}