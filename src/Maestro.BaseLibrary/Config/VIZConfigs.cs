using System;
using System.Configuration;

namespace Maestro.BaseLibrary.Config
{
    public class VIZConfigs : ConfigurationElementCollection
    {

        public new VIZConfig this[string index]
        {
            get { return (VIZConfig)base.BaseGet(index); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new VIZConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((VIZConfig)element).Name;
        }

    }
}