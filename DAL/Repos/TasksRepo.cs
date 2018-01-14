using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Spatial;
using System.Drawing;
using Point = DAL.Models.Point;

namespace DAL.Repos
{
    public class TasksRepo
    {
        public static IEnumerable<Task> GetTasksByUserName(string userName)
        {
            IEnumerable<Task> tasks = new GisContext().Tasks.ToList();
            foreach (var task in tasks)
            {
                if (task.User.UserName == userName)
                {
                    yield return task;
                }
            }
        }

        public static void SetTaskAsFavourite(int taskId)
        {
            var task = new GisContext().Tasks.FirstOrDefault<Task>(t => t.Id == taskId);
            if (task != null)
            {
                task.isFavorite = true;
            }
        }

        public static int CreateNewTask(string name, string mode, DateTime time, List<Point> route, bool IsFavorite, string userName, ICollection<Point> points)
        {
            var dbContext = new GisContext();

            var userRes = dbContext.Users.Where(u => u.UserName == userName);
            
            User user = userRes.First();
                
            Task task = dbContext.Tasks.Add(new Task
            {
                Name = name,
                Mode = mode,
                Time = time,
                Route = route,
                isFavorite = IsFavorite,
                User = user,
                Points = points,
                UserId = ((user != null) ? user.Id : 0)
            });
            
            dbContext.SaveChanges();

            return task.Id;
        }

        public static void DeleteTask(int taskId,string userName)
        {
            var dbContext = new GisContext();
            
            Task task = dbContext.Tasks.Where(t => t.Id== taskId).First();
            
            dbContext.Tasks.Remove(task);
            dbContext.SaveChanges();
        }

        public static void DeleteAllTasks(string userName)
        {
            var dbContext = new GisContext();

            dbContext.Tasks.RemoveRange(dbContext.Tasks.Where(t => t.User.UserName == userName));

            dbContext.SaveChanges();
        }

        private static string CreateWKTByPoints(IEnumerable<PointF> list)
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