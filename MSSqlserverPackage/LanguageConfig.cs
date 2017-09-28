using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSqlserverPackage
{
    public class LanguagesConfig : ConfigurationSection
    {
        [ConfigurationProperty("languages")]
        public LanguagesConfig Languages
        {
            get
            {
                return (this["languages"] as LanguagesConfig);
            }
        }
    }
    public class LanguageConfig : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return (this["name"] ?? string.Empty).ToString();
            }
            set
            {
                this["name"] = value;
            }
        }
        [ConfigurationProperty("display")]
        public string Display
        {
            get
            {
                return (this["display"] ?? string.Empty).ToString();
            }
            set
            {
                this["display"] = value;
            }
        }
        [ConfigurationProperty("path")]
        public string Path
        {
            get { return (this["path"] ?? string.Empty).ToString(); }
            set { this["path"] = value; }
        }

        [ConfigurationProperty("isDefault")]
        public string IsDefault
        {
            get { return (this["path"] ?? string.Empty).ToString(); }
            set { this["path"] = value; }
        }
    }
}
