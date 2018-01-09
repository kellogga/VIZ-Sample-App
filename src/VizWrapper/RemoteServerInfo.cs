namespace VizWrapper
{
    public class RemoteServerInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public override string ToString()
        {
            return Host + ":" + Port;
        }
    }
}
