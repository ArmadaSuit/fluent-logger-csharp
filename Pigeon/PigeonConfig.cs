using System;

namespace Pigeon
{
    /// <summary>
    /// Fluentd Forward Protocol client configuration.
    /// </summary>
    public class PigeonConfig
    {
        /// <summary>
        /// host name.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// port number.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="host">host name</param>
        /// <param name="port">port number</param>
        /// <exception cref="ArgumentNullException">host is null or empty</exception>
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
