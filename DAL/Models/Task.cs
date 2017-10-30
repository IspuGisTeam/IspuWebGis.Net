using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Task
    {
       
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Mode { get; set; }
        public DateTime Time { get; set; }
        public SqlGeometry Route { get; set; }
        public bool isFavorite { get; set; }
        public ICollection<Point> Points { get; set; }
    }
}
