using DAL;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BLL
{
    internal class RoadGraph
    {
        //Singleton
        private static RoadGraph instance;

        private RoadGraph()
        {
            GraphInit();
        }

        public static RoadGraph getInstance()
        {
            if (instance == null)
                instance = new RoadGraph();
            return instance;
        }

        private List<GraphVertex> vertices;

        public void GraphInit()
        {
            //Init vertices
            vertices = RouteUtility.GetPointsFromRoadDB(true, false).Select(point => new GraphVertex { Coordinates = point }).ToList();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].Id = i;
            }

            //init roads
            var roads = new GisContext().Roads.ToList();
            for (int i = 0; i < roads.Count; i++)
            {
                var edge = roads[i];
                var point1 = RouteUtility.GetPointFromDbGeometry(edge.Shape.StartPoint);
                var point2 = RouteUtility.GetPointFromDbGeometry(edge.Shape.EndPoint);
                var vert1 = vertices.First(v => v.Coordinates.X == point1.X && v.Coordinates.Y == point1.Y);
                var vert2 = vertices.First(v => v.Coordinates.X == point2.X && v.Coordinates.Y == point2.Y);
                var points = RouteUtility.GetPointsByRoad(edge);
                //TO-DO: учитывать односторонние дороги
                var newEdge1 = new GraphEdge
                {
                    Length = Convert.ToSingle(edge.Length),
                    Time = Convert.ToSingle(edge.Length) / edge.Speed,
                    Vert1 = vert1,
                    Vert2 = vert2,
                    Shape = points.ToList()
                };
                var newEdge2 = new GraphEdge
                {
                    Length = Convert.ToSingle(edge.Length),
                    Time = Convert.ToSingle(edge.Length) / edge.Speed,
                    Vert1 = vert2,
                    Vert2 = vert1,
                    Shape = points.Reverse().ToList()
                };

                vert1.Edges.Add(newEdge1);
                vert2.Edges.Add(newEdge2);
            }
        }

        public RouteCalculationCheckpointResult CalculatePathBetweenPoints(PointF startPoint, PointF finishPoint, RouteCalculationMode mode)
        {
            //TO-DO: Искать не ближайшую точку дороги, а наиболее удобную.
            var nearStart = this.GetNearVertex(startPoint);
            var nearFinish = this.GetNearVertex(finishPoint);

            var calcResult = CalculatePathBetweenVertices(nearStart, nearFinish, mode);

            if (calcResult.Item1.First() != startPoint)
            {
                calcResult.Item1.Insert(0, startPoint);
            }

            if (calcResult.Item1.Last() != finishPoint)
            {
                calcResult.Item1.Add(finishPoint);
            }

            // Length и Time маршрута учитывают только движение по дорогам
            // и не учитывают время и длину маршрута до ближайшей точки дороги
            var result = new RouteCalculationCheckpointResult
            {
                StartPoint = startPoint,
                FinishPoint = finishPoint,
                WKTPath = calcResult.Item1,
                Length = calcResult.Item2,
                Time = calcResult.Item3
            };

            return result;
        }

        public Tuple<List<PointF>, float, float> CalculatePathBetweenVertices(GraphVertex vertex1, GraphVertex vertex2, RouteCalculationMode mode)
        {
            //Реализация алгоритма Дейкстры на основе очереди с приоритетом (Heap)
            int n = vertices.Count;
            float[] d = new float[n];
            int[] p = new int[n];
            for (int i = 0; i < n; i++)
            {
                d[i] = float.MaxValue;
                p[i] = -1;
            }

            d[vertex1.Id] = 0;

            var queue = new SimplePriorityQueue<int, float>();
            queue.Enqueue(vertex1.Id, 0);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();

                for (int i = 0; i < vertices[v].Edges.Count; i++)
                {
                    int to = vertices[v].Edges[i].Vert2.Id;
                    //если нужен самый короткий путь - минимизируем длину пути.
                    //если нужен самый быстрый - минимизируем время (длина/скорость).
                    float len = (mode == RouteCalculationMode.ShortRoute) ? (vertices[v].Edges[i].Length) : (vertices[v].Edges[i].Time);
                    if (d[v] + len < d[to])
                    {
                        d[to] = d[v] + len;
                        p[to] = v;
                        if (!queue.TryUpdatePriority(to, d[to]))
                        {
                            queue.Enqueue(to, d[to]);
                        }
                    }
                }
            }

            if (p[vertex2.Id] == -1)
            {
                //не нашли путь
                throw new Exception();
            }

            // восстанавливаем путь
            int currentPoint = vertex2.Id;

            var vertPath = new List<int>();

            while (currentPoint != -1)
            {
                vertPath.Add(currentPoint);
                currentPoint = p[currentPoint];
            }

            vertPath.Reverse();

            var edgePath = new List<GraphEdge>();
            for (int i = 0; i < vertPath.Count - 1; i++)
            {
                var vert1 = vertices[vertPath[i]];
                var vert2 = vertices[vertPath[i + 1]];
                var edge = vert1.Edges.First(e => e.Vert2.Id == vert2.Id);
                edgePath.Add(edge);
            }

            List<PointF> wktPath = edgePath.SelectMany(e => e.Shape).ToList();
            float length = edgePath.Sum(edge => edge.Length);
            float time = edgePath.Sum(edge => edge.Time);

            return new Tuple<List<PointF>, float, float>(wktPath, length, time);
        }

        private GraphVertex GetNearVertex(PointF point)
        {
            float minLen = 0.0f;
            int? minIndex = null;
            for (int i = 0; i < vertices.Count; i++)
            {
                float len = (vertices[i].Coordinates.X - point.X) * (vertices[i].Coordinates.X - point.X) + (vertices[i].Coordinates.Y - point.Y) * (vertices[i].Coordinates.Y - point.Y);
                if (minIndex == null || len < minLen)
                {
                    minIndex = i;
                    minLen = len;
                }
            }

            return vertices[minIndex.Value];
        }
    }

    internal class GraphVertex
    {
        public int Id { get; set; }
        public PointF Coordinates { get; set; }
        public List<GraphEdge> Edges { get; set; }

        public GraphVertex()
        {
            Edges = new List<GraphEdge>();
        }
    }

    internal class GraphEdge
    {
        public float Length { get; set; }
        public float Time { get; set; }
        public GraphVertex Vert1 { get; set; }
        public GraphVertex Vert2 { get; set; }
        public List<PointF> Shape { get; set; }
    }
}
