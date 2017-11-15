using System;
using System.Collections.Generic;
using System.Web.Http;
using BLL;
using IspuWebGis.Net.Models;
using System.Drawing;

namespace IspuWebGis.Controllers
{
    [RoutePrefix("api/tasks")]
    public class TasksController : ApiController
    {
        [HttpGet]
        public Object Get()
        {
            /*var points = new List<CustomPoint>();
            points.Add(new CustomPoint { id = 2, address = "address_text", latitude = 50, longitude = 45 });
            var task = new Task { id = 5, name = "task_name", timeOfCreation = DateTime.Now, points = points};
            return new {tasks=new Task[] { task } };*/
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
        [Route("")]
        public TaskResponse CreateTask([FromBody]TaskRequest taskRequest)//ClientPoint clientPoint)
        {
            try
            {
                var parsedMode = (RouteCalculationMode)(Enum.Parse(typeof(RouteCalculationMode), taskRequest.mode));
                var checkpoints = taskRequest.checkpoints.ConvertAll<PointF>(cp => new PointF(cp.x, cp.y));

                var routeCalculationRes = new RouteCalculation().Calculate(taskRequest.startPoint.toPointF(), checkpoints, parsedMode);
                var taskResponse = new TaskResponse(routeCalculationRes);

                return taskResponse;
            }catch(Exception e)
            {
                return null;
            }
        }
        [HttpPost]
        [Route("madeFavourite")]
        public bool MadeFavourite([FromBody]int taskId)//ClientPoint clientPoint)
        {
            return true;
        }
    }
}
