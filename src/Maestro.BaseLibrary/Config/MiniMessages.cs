using System.Configuration;

namespace Maestro.BaseLibrary.Config
{
    public class MiniMessages : ConfigurationElementCollection
    {
        public MiniMessage this[int index]
        {
            get { return (MiniMessage)base.BaseGet(index); }
            set
            {
                if ((((base.BaseGet(index)) != null)))
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MiniMessage();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MiniMessage)element).Message;
        }

        public MiniMessage Add(string message)
        {
            MiniMessage newMessage = new MiniMessage();
            newMessage.Message = message;
            this.BaseAdd(newMessage);
            return newMessage;
        }
    }
}
