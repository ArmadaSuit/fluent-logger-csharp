// Pigeon
//
// Copyright 2022 ArmadaSuit and contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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
        /// the connect timeout value in milliseconds.
        /// </summary>
        public int? ConnectTimeout { get; set; }

        /// <summary>
        /// the send timeout value of the connection in milliseconds.
        /// </summary>
        public int? SendTimeout { get; set; }

        /// <summary>
        /// the receive timeout value of the connection in milliseconds.
        /// </summary>
        public int? ReceiveTimeout { get; set; }

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
