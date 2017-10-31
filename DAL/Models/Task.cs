using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Task
    {
       
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Mode { get; set; }
        public DateTime Time { get; set; }
        public DbGeometry Route { get; set; }
        public bool isFavorite { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Point> Points { get; set; }
    }
}
