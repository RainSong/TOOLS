﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryDBObject.Models
{
    public class DBColumn
    {
        public int ObjectID { get; set; }
        public int ColumnID { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public int MaxLength { get; set; }
        public bool Nullable { get; set; }
        public string Identity { get; set; }
        public bool IsPrimary { get; set; }
        public string Default { get; set; }
        public string Description { get; set; }
    }
}
