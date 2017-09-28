using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSqlserverPackage.Common
{
    class FieldAttribute : Attribute
    {
        public string Field { get; set; }
        public FieldAttribute(string field)
        {
            this.Field = field;
        }
    }
}
