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

        // Запуск алгоритма коммивояжера
        public RouteCalculationResult CalculateTSP(PointF startPoint, List<PointF> checkpoints, RouteCalculationMode mode = RouteCalculationMode.ShortRoute)
        {
            var graph = RoadGraph.getInstance();

            var TSPTable = graph.CalculateTSPTable(startPoint, checkpoints, mode);
            var result = graph.CalculateTSPOptimalWay(TSPTable, 0, mode);
            if (result == null)
            {
                throw new Exception("Path was not found :(");
            }

            result.Mode = mode;
            return result;
        }

        

    }
}