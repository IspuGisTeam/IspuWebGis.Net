using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Drawing;
using System.Linq;

namespace BLL
{
    /*
     * Utility class with function that helps to build routes
     * */

    public static class RouteUtility
    {
        public const int SRID = 4326;

        public static IEnumerable<Tuple<PointF, PointF>> GetLinesFromRoadDB()
        {
            var roads = new GisContext().Roads.ToList();
            foreach (var road in roads)
            {
                for (int i = 1; i < road.Shape.PointCount; i++)
                {
                    var point1 = road.Shape.PointAt(i);
                    var point2 = road.Shape.PointAt(i + 1);
                    yield return new Tuple<PointF, PointF>(GetPointFromDbGeometry(point1), GetPointFromDbGeometry(point2));
                }
            }
        }

        public static IEnumerable<PointF> GetPointsByRoad(Road road)
        {
            for(int i=1;i<=road.Shape.PointCount;i++)
            {
                var p = road.Shape.PointAt(i);
                yield return GetPointFromDbGeometry(p);
            }
        }

        public static IEnumerable<PointF> GetPointsFromRoadDB(bool IncludeBoundary = true, bool IncludeSubpoints = true)
        {
            var roads = new GisContext().Roads.ToList();
            foreach (var road in roads)
            {
                if (IncludeBoundary)
                {
                    yield return GetPointFromDbGeometry(road.Shape.PointAt(1));
                }

                if (IncludeSubpoints)
                {
                    for (int i = 2; i < road.Shape.PointCount; i++)
                    {
                        var point1 = road.Shape.PointAt(i);
                        yield return GetPointFromDbGeometry(point1);
                    }
                }

                if (IncludeBoundary)
                {
                    yield return GetPointFromDbGeometry(road.Shape.PointAt(road.Shape.PointCount.Value));
                }
            }
        }

        public static PointF GetPointFromDbGeometry(DbGeometry geom)
        {
            return new PointF((float)geom.XCoordinate.Value, (float)geom.YCoordinate.Value);
        }

        public static void DrawMapToBmp(string outputPath, bool drawPoints = false, int reduceRate = 4)
        {
            float xMin = GetPointsFromRoadDB().Min(p => p.X);
            float xMax = GetPointsFromRoadDB().Max(p => p.X);
            float yMin = GetPointsFromRoadDB().Min(p => p.Y);
            float yMax = GetPointsFromRoadDB().Max(p => p.Y);

            xMax -= xMin;
            yMax -= yMin;

            Bitmap bmp = new Bitmap((int)xMax / reduceRate, (int)yMax / reduceRate);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.Clear(Color.White);
                Pen pen = new Pen(Color.Black, 3.0f);
                foreach (var line in GetLinesFromRoadDB())
                {
                    var p1 = new PointF((line.Item1.X - xMin) / reduceRate, (line.Item1.Y - yMin) / reduceRate);
                    var p2 = new PointF((line.Item2.X - xMin) / reduceRate, (line.Item2.Y - yMin) / reduceRate);
                    graphics.DrawLine(pen, p1, p2);
                }

                if (drawPoints)
                {
                    foreach (var point in GetPointsFromRoadDB(true, false))
                    {
                        const int radius = 3;
                        graphics.FillEllipse(Brushes.Blue, (point.X - xMin) / reduceRate - radius, (point.Y - yMin) / reduceRate - radius, radius * 2, radius * 2);
                    }

                    foreach (var point in GetPointsFromRoadDB(false, true))
                    {
                        const int radius = 3;
                        graphics.FillEllipse(Brushes.Red, (point.X - xMin) / reduceRate - radius, (point.Y - yMin) / reduceRate - radius, radius * 2, radius * 2);
                    }
                }
            }

            bmp.Save(outputPath);
        }

        public static void DrawMapToBmpWithPath(string outputPath, RouteCalculationResult res, bool drawPoint = false, int reduceRate = 6)
        {
            Color[] colors = { Color.Red, Color.Green, Color.Blue, Color.Orange, Color.DarkViolet, Color.Chocolate, Color.DarkGray };

            float xMin = GetPointsFromRoadDB().Min(p => p.X);
            float xMax = GetPointsFromRoadDB().Max(p => p.X);
            float yMin = GetPointsFromRoadDB().Min(p => p.Y);
            float yMax = GetPointsFromRoadDB().Max(p => p.Y);

            xMax -= xMin;
            yMax -= yMin;

            reduceRate = 2;
            while (xMax / reduceRate > 3000 || yMax / reduceRate > 3000)
            {
                reduceRate++;
            }

            Bitmap bmp = new Bitmap((int)xMax / reduceRate, (int)yMax / reduceRate);
            using (var graphics = Graphics.FromImage(bmp))
            {
                graphics.Clear(Color.White);
                Pen pen1 = new Pen(Color.Black, 3.0f);
                foreach (var line in GetLinesFromRoadDB())
                {
                    var p1 = new PointF((line.Item1.X - xMin) / reduceRate, (line.Item1.Y - yMin) / reduceRate);
                    var p2 = new PointF((line.Item2.X - xMin) / reduceRate, (line.Item2.Y - yMin) / reduceRate);
                    graphics.DrawLine(pen1, p1, p2);
                }
                var list = res.Checkpoints;
                for (int i = 0; i < list.Count; i++)
                {
                    Pen pen2 = new Pen(colors[i], 10.0f);
                    pen2.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    for (int j = 1; j < list[i].WKTPath.Count; j++)
                    {
                        var p1 = list[i].WKTPath[j - 1];
                        var p2 = list[i].WKTPath[j];
                        p1 = new PointF((p1.X - xMin) / reduceRate, (p1.Y - yMin) / reduceRate);
                        p2 = new PointF((p2.X - xMin) / reduceRate, (p2.Y - yMin) / reduceRate);
                        graphics.DrawLine(pen2, p1, p2);
                    }
                }
                var font = new Font("Arial", 40, FontStyle.Bold);
                graphics.DrawString("Length: " + res.TotalLength.ToString(), font, Brushes.Black, 0 , bmp.Height / 6);
                graphics.DrawString("Time: " + res.TotalTime.ToString(), font, Brushes.Black, 0, bmp.Height / 6 + 60);
            }
            bmp.Save(outputPath);
        }

        public static string CreateWKTByPoints(IEnumerable<PointF> list)
        {
            List<PointF> arr = new List<PointF>();
            foreach (var el in list)
            {
                if (arr.Count == 0 || arr.Last() != el)
                {
                    arr.Add(el);
                }
            }

            //LINESTRING (4556941.7027999982 7758109.5407000035, 4556570.3013999984 7757926.8623000011)

            string pointsStr = string.Join(",", arr.Select(p => string.Format("{0} {1}", p.X, p.Y)));
            return string.Format("LINESTRING ({0})", pointsStr);
        }

    }
}
