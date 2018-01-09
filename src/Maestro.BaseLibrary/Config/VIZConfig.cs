using System;
using System.Configuration;
using System.Data;

namespace Maestro.BaseLibrary.Config
{
    public class VIZConfig : ConfigurationElement
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public override string ToString()
        {
            return Name + " (" + Host + ":" + Port.ToString() + ")";
        }
    }
}