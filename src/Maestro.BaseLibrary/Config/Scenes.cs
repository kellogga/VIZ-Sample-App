using System.Configuration;

namespace Maestro.BaseLibrary.Config
{
    public class Scenes: ConfigurationElementCollection
    {
            public new Scene this[string index]
            {
                get { return (Scene)base.BaseGet(index.ToLower()); }
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return new Scene();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((Scene)element).Name;
            }
        }
    }
