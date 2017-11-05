using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using BLL;

namespace IspuWebGis.Net.Models
{
    public class TaskResponse
    {
        public TaskResponse(RouteCalculationResult routeCalcResult)
        {
            routeResult = new Route();
            routeResult.mode = routeCalcResult.Mode.ToString();
            routeResult.totalLength = routeCalcResult.TotalLength;
            routeResult.totalTime = routeCalcResult.TotalTime;
            routeResult.checkpoints = routeCalcResult.Checkpoints.Select(cp => new Checkpoint(cp)).ToList();         
        }
        public long? taskId { get; set; }
        public string name { get; set; }
        public List<Checkpoint> checkpoints { get; set; }
        public DateTime time { get; set; }
        public long UserId { get; set; }
        public bool IsFavourite { get; set; }
        public Route routeResult { get; set; }
    }
}
