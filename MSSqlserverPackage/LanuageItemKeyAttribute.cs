using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSqlserverPackage
{
    public class LanuageItemKeyAttribute:Attribute
    {
        public string Key { get; set; }
        public LanuageItemKeyAttribute(string key)
        {
            this.Key = key;
        }
    }
}
