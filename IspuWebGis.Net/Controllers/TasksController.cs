using System;
using System.Collections.Generic;
using System.Web.Http;
using BLL;
using IspuWebGis.Net.Models;
using System.Drawing;

namespace IspuWebGis.Controllers
{
    [Route("api/tasks")]
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
        [HttpPost]
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
    }
}