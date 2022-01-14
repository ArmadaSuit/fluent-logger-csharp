using System;

namespace Pigeon
{
    public class PigeonConfig
    {
        public string Host { get; }

        public int Port { get; }

        public PigeonConfig(string host, int port)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host));
            }

            Host = host;
            Port = port;
        }
    }
}
