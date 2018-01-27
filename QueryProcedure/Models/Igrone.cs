using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryProcedure
{
    public class Igrone
    {
        public string Content { get; set; }
        public string IgroneType { get; set; }
        public Igrone(string content)
        {
            this.IgroneType = "KeyWord";
            this.Content = content;
        }
        public Igrone(string content, string igronType)
        {
            this.Content = content;
            this.IgroneType = igronType;
        }
    }
}
