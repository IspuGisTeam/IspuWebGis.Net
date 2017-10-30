using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Model
{
    public class Road
    {
        public int Id { get; set; }
        public short Forward { get; set; }
        public short Backward { get; set; }
        public short Speed { get; set; }
        public SqlGeometry Shape { get; set; }
        public int ObjectId { get; set; }
        public decimal Length { get; set; }
    }
}
