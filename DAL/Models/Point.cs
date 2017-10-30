﻿using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Point
     {
        
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int Rank { get; set; }
        public string Address { get; set; }
        public SqlGeometry Shape { get; set; }
    }
}
