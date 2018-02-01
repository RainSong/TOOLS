﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryTable.Models
{
    public class DBObject
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime create_date { get; set; }
        public DateTime modify_date { get; set; }
        public string script { get; set; }
    }
}
