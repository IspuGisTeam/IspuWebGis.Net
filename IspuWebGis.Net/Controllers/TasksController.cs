using System;
using System.Collections.Generic;
using System.Web.Http;
using BLL;
using IspuWebGis.Net.Models;
using System.Drawing;
using IspuWebGis.Net.Filters;
using DAL.Repos;
using System.Web.Http.Cors;

namespace IspuWebGis.Controllers
{
    [RoutePrefix("api/tasks")]
    [EnableCors("*", "*", "*")]
    public class TasksController : ApiController
    {
        [HttpGet]
        public Object Get()
        {
            // TODO: need to be changed
            TasksRepo.GetTasksByUserId(-1);
      
            return null;
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
        public TaskResponse CreateTask([FromBody]TaskRequest taskRequest)//ClientPoint clientPoint)
        {
            try
            {
                var parsedMode = (RouteCalculationMode)(Enum.Parse(typeof(RouteCalculationMode), taskRequest.mode));
                var checkpoints = taskRequest.checkpoints.ConvertAll<PointF>(cp => new PointF(cp.x, cp.y));

                var routeCalculationRes = new RouteCalculation().Calculate(taskRequest.startPoint.toPointF(), checkpoints, parsedMode);

              /*  TasksRepo.CreateNewTask(taskRequest.name, taskRequest.mode,
                    taskRequest.startPoint,taskRequest.isFavourite,)*/
                var taskResponse = new TaskResponse(routeCalculationRes);

                return taskResponse;
            }catch(Exception e)
            {
                return null;
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
    }
}
