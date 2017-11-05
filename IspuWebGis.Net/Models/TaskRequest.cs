using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace IspuWebGis.Net.Models
{
    public class TaskRequest
    {
        public TaskRequest()
        {

        }
        public string name { get; set; }
        public ClientPoint startPoint { get; set; }
        public List<ClientPoint> checkpoints { get; set; }
        public DateTime time { get; set; }
        public String mode { get; set; }
        public long userId { get; set; }
        public bool isFavourite { get; set; }
    }
}