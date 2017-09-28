using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MSSqlserverPackage
{
    class LanguageHelper
    {
    }

    public class LanguageLoader
    {
        public static List<dynamic> GetALL()
        {
            return null;
        }

        public static LanuageContent GetLanguage(string fileName)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);

            var type = typeof(LanuageContent);
            var config = Activator.CreateInstance<LanuageContent>();
            var nl = xmlDoc.SelectNodes("//@name");
            if (nl!= null && nl.Count > 0)
            {
                config.Name = nl[0].Value;
            }
            nl = xmlDoc.SelectNodes("//@display");
            if (nl != null && nl.Count > 0)
            {
                config.Display = nl[0].Value;
            }
            var properties = type.GetProperties();
            foreach (var p in properties)
            {
                var attrs = p.GetCustomAttributes<LanuageItemKeyAttribute>();
                if (attrs == null || !attrs.Any()) continue;
                var firstAttr = attrs.First();
                var xpath = string.Format("//item[@key =\"{0}\"]", firstAttr.Key);
                var nodes = xmlDoc.SelectNodes(xpath);
                if (nodes != null && nodes.Count > 0)
                {
                    p.SetValue(config, nodes[0].InnerText);
                }
            }
            return config;
        }
    }
}
