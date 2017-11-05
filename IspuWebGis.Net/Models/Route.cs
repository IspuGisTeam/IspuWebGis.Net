using IspuWebGis.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace IspuWebGis.Net.Models
{
    public class Route
    {
        public String mode { get; set; }
        public float totalLength { get; set; }
        public float totalTime { get; set; }
        public List<Checkpoint> checkpoints { get; set; }
    }
}
