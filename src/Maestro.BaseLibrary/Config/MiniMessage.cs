using System;
using System.Configuration;

namespace Maestro.BaseLibrary.Config
{
    public class MiniMessage : ConfigurationElement
    {
        [ConfigurationProperty("Message", IsRequired = true)]
        public string Message
        {
            get { return Convert.ToString(this["Message"]); }
            set { this["Message"] = value; }
        }
    }
}

