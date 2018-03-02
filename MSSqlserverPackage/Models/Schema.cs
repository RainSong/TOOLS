using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSSqlserverPackage.Common;
using RainSong.Common;

namespace MSSqlserverPackage.Models
{
    public class Schema
    {
        [Field("schema_id")]
        public int ID { get; set; }
        [Field("name")]
        public string Name { get; set; }
    }
}
