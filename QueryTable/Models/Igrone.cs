using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryTable.Models
{
    public class Igrone
    {
        public string ID { get; set; }
        public string Content { get; set; }
        public string IgroneType { get; set; }
        public Igrone()
        {
            this.ID = Guid.NewGuid().ToString();
        }
        public Igrone(string content)
        {
            this.ID = Guid.NewGuid().ToString();
            this.IgroneType = "KeyWord";
            this.Content = content;
        }
        public Igrone(string content, string igronType)
        {
            this.ID = Guid.NewGuid().ToString();
            this.Content = content;
            this.IgroneType = igronType;
        }
    }
}
