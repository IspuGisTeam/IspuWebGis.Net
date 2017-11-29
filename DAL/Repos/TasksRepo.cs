using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Spatial;

namespace DAL.Repos
{
    public class TasksRepo
    {

        public static IEnumerable<Task> GetTasksByUserId(int userId)
        {
            IEnumerable<Task> tasks = new GisContext().Tasks.ToList();
            foreach (var task in tasks)
            {
                if (task.UserId == userId)
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

        public static void CreateNewTask(string name, string mode, DateTime time, DbGeometry route, bool IsFavorite, User user, ICollection<Point> points, int userId = -1)
        {
            var dbContext = new GisContext();
            dbContext.Tasks.Add(new Task
            {
                Name = name,
                Mode = mode,
                Time = time,
                Route = route,
                isFavorite = IsFavorite,
                User = user,
                Points = points,
                UserId = userId
            });
            dbContext.SaveChanges();
        }
    }
}