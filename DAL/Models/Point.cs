﻿using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Point
     {
        public double X { get; set; }
        public double Y { get; set; }
        public int Id { get; set; }
        public int? TaskId { get; set; }

        public virtual  Task Task { get; set; }
    }
}
