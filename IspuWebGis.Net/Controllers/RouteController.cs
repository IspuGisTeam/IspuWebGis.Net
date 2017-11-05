using System;
using System.Collections.Generic;
using System.Web.Http;
using IspuWebGis.Models;
using BLL;

namespace IspuWebGis.Controllers
{
    [Route("api/route")]
    public class RouteController : ApiController
    {
        [HttpPost]
        public RouteModel Get([FromBody]RouteModel route)
        {
            int i = 0;
            
            foreach (var p in route.points)
            {
                p.id = i++;
            }


            return route;
        }

        [HttpGet]
        public string Calculate()
        {
            RouteCalculation rc = new RouteCalculation();
            return "";
            //return rc.Calculate();
        }
        public class RouteModel
        { 
            public List<CustomPoint> points { get; set; }
        }
    }
}