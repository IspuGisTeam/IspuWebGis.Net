using System;
using System.Collections.Generic;
using System.Web.Http;
using BLL;
using IspuWebGis.Net.Models;
using System.Drawing;
using IspuWebGis.Net.Filters;
using DAL.Repos;
using System.Web.Http.Cors;
using System.Web;

namespace IspuWebGis.Controllers
{
    [RoutePrefix("api/tasks")]
    [EnableCors("*", "*", "*")]
    public class TasksController : ApiController
    {
        [HttpGet]
        [Route("")]
        [BasicAuthentication]
        public IEnumerable<TaskResponse> Get()
        {
            // TODO: need to be changed
      
            return makeTasksResponseList(TasksRepo.GetTasksByUserName(HttpContext.Current.User.Identity.Name));
        }
        /* Request sample
         * {
	            "time" : "12-04-2020 23:11",
	            "mode" : "ShortRoute",
	            "name" : "Home - Work",
	            "userId" : "3123123",
	            "isFavourite" : false,
	            "startPoint" : {
		        "x" : 4557725.168,
		        "y" : 7760357.210
	            },
	            "checkpoints" : [{
			        "x" : 4560930.802,
			        "y" : 7760020.759
		        },
		        {
			        "x" : 4560932.802,
			        "y" : 7760029.759
		        }
	        ]   
        }
         */
        [HttpPost]
        [BasicAuthentication]
        [Route("")]
        public Object CreateTask([FromBody]TaskRequest taskRequest)//ClientPoint clientPoint)
        {
            try
            {
                var parsedMode = (RouteCalculationMode)(Enum.Parse(typeof(RouteCalculationMode), taskRequest.mode));
                var checkpoints = taskRequest.checkpoints.ConvertAll(cp => new PointF(cp.x, cp.y));
                var checkpointsForDAL = taskRequest.checkpoints.ConvertAll(cp => new DAL.Models.Point { X = (int)cp.x, Y = (int)cp.y });

                var routeCalculationRes = new RouteCalculation().Calculate(taskRequest.startPoint.toPointF(), checkpoints, parsedMode);

                createTask(taskRequest, routeCalculationRes, checkpointsForDAL);

                var taskResponse = new TaskResponse(routeCalculationRes,taskRequest.name, -1,taskRequest.isFavourite,1,checkpoints,taskRequest.time);

                return taskResponse;
            }catch(Exception e)
            {
                return e.Message;
            }
        }
        [HttpDelete]
        [Route("")]
        [BasicAuthentication]
        public Object Delete(int taskId)
        {
            // TODO: need to be changed
            try
            {
                TasksRepo.DeleteTask(taskId, HttpContext.Current.User.Identity.Name);

                return "Success";
            }catch(Exception e)
            {
                return e.Message;
            }
        }
        [HttpDelete]
        [Route("")]
        [BasicAuthentication]
        public Object Delete()
        {
            // TODO: need to be changed
            try
            {
                TasksRepo.DeleteAllTasks(HttpContext.Current.User.Identity.Name);

                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        [HttpPost]
        [BasicAuthentication]
        [Route("madeFavourite")]
        public bool MadeFavourite([FromBody]MakeTaskFavouriteRequest req)//ClientPoint clientPoint)
        {
            TasksRepo.SetTaskAsFavourite((int)req.taskId);
            return true;
        }

        private static void createTask(TaskRequest taskRequest, RouteCalculationResult routeCalculationRes, List<DAL.Models.Point> checkPoints)
        {
            var newCheckPoints = new List<DAL.Models.Point>();
            float totalLength = 0;
            float totalTime = 0;

            ICollection<RouteCalculationCheckpointResult> points = routeCalculationRes.Checkpoints;
            foreach (var checkPoint in points)
            {
                var newPoints = convertPointsList(checkPoint.WKTPath);

                totalLength += checkPoint.Length;
                totalTime += checkPoint.Time;

                newCheckPoints.AddRange(newPoints);
            }

            TasksRepo.CreateNewTask(taskRequest.name, taskRequest.mode, DateTime.Now,
                   newCheckPoints, taskRequest.isFavourite, HttpContext.Current.User.Identity.Name, checkPoints);
        }

        private static ICollection<DAL.Models.Point> convertPointsList(ICollection<PointF> points)
        {
            var newPoints = new List<DAL.Models.Point>();
            foreach (var point in points)
            {
                var p = new DAL.Models.Point();
                p.X = point.X;
                p.Y = point.Y;
                newPoints.Add(p);
            }

            return newPoints;
        }

        private static List<PointF> convertPointsList(ICollection<DAL.Models.Point> points)
        {
            if (points == null)
                return null;
            var newPoints = new List<PointF>();
            foreach (var point in points)
            {
                var p = new PointF();
                p.X = (float)point.X;
                p.Y = (float)point.Y;
                newPoints.Add(p);
            }

            return newPoints;
        }


        private static IEnumerable<TaskResponse> makeTasksResponseList(IEnumerable<DAL.Models.Task> tasks)
        {
            var newTasks = new List<TaskResponse>();
            foreach (var task in tasks)
            {
                var taskResponse = new TaskResponse();
                taskResponse.taskId = task.Id;
                taskResponse.time = task.Time;
                taskResponse.IsFavourite = task.isFavorite;
                taskResponse.name = task.Name;
                taskResponse.checkpoints = convertPointsList(task.Points);
                taskResponse.routeResult = new Route();
                taskResponse.routeResult.mode = task.Mode;

                taskResponse.routeResult.totalTime = task.Time.Ticks;
                taskResponse.routeResult.checkpoints = new List<Checkpoint>();
                var checkPoint = new Checkpoint();
                checkPoint.length = 0;
                checkPoint.time = task.Time.Ticks;
                checkPoint.WKTPath = convertPointsList(task.Route);

                newTasks.Add(taskResponse);
            }

            return newTasks;
        }
    }
}
