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
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using Pigeon.EventModes;
using Pigeon.Resolvers;

namespace Pigeon
{
    /// <summary>
    /// simple implementation that Fluentd Forward Protocol client.
    /// </summary>
    public class PigeonClient : IPigeonClient
    {
        private static readonly IFormatterResolver DefaultResolver = CompositeResolver.Create(
            BuiltinResolver.Instance,
            AttributeFormatterResolver.Instance,
            DynamicEnumResolver.Instance,
            DynamicGenericResolver.Instance,
            DynamicUnionResolver.Instance,
            DynamicObjectResolver.Instance,
            PigeonPrimitiveObjectResolver.Instance,
            DynamicContractlessObjectResolver.Instance
        );

        private static readonly MessagePackSerializerOptions DefaultOptions =
            MessagePackSerializerOptions.Standard.WithResolver(DefaultResolver);

        private bool _disposed;

        private Socket? _socket;
        private readonly PigeonConfig _config;
        private readonly MessagePackSerializerOptions _options;

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="config">configuration</param>
        public PigeonClient(PigeonConfig config)
        {
            _config = config;

            _options = config.SerializerOptions ?? DefaultOptions;
        }

        /// <summary>
        /// connect client to server.
        /// </summary>
        /// <returns>Task</returns>
        public async Task ConnectAsync()
        {
            _socket?.Dispose();
            _socket = null;

            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            if (_config.SendTimeout.HasValue)
            {
                socket.SendTimeout = _config.SendTimeout.Value;
            }

            if (_config.ReceiveTimeout.HasValue)
            {
                socket.ReceiveTimeout = _config.ReceiveTimeout.Value;
            }

            try
            {
                if (_config.ConnectTimeout.HasValue)
                {
                    using var cts = new CancellationTokenSource(_config.ConnectTimeout.Value);
                    try
                    {
                        await socket.ConnectAsync(_config.Host, _config.Port, cts.Token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        throw new SocketException((int)SocketError.TimedOut);
                    }
                }
                else
                {
                    await socket.ConnectAsync(_config.Host, _config.Port, CancellationToken.None).ConfigureAwait(false);
                }

                _socket = socket;
            }
            catch
            {
                socket.Dispose();
                throw;
            }
        }

        /// <summary>
        /// ensure connection and if not connected then connect client to server.
        /// </summary>
        /// <returns>Task</returns>
        private async Task EnsureConnected()
        {
            if (_socket is not { Connected: true })
            {
                await ConnectAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// send a data that will not be changed to server.
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        private async Task SendAsync(byte[] data)
        {
            await EnsureConnected();

            var bytesSent = 0;
            while (bytesSent < data.Length)
            {
                var sent = await _socket!.SendAsync(
                    data.AsMemory(bytesSent),
                    SocketFlags.None,
                    CancellationToken.None).ConfigureAwait(false);

                if (sent == 0)
                {
                    throw new SocketException((int)SocketError.ConnectionReset);
                }

                bytesSent += sent;
            }
        }

        /// <summary>
        /// send a data that will be converted to Message Mode to server.
        /// </summary>
        /// <param name="tag">tag</param>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(string tag, Dictionary<string, object> data)
        {
            var message = new MessageMode { Tag = tag, Time = new EventTime(), Record = data };
            await SendAsync(message).ConfigureAwait(false);
        }

        /// <summary>
        /// send a data that will be converted to Message Mode to server.
        /// </summary>
        /// <param name="tag">tag</param>
        /// <param name="time">event time</param>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(string tag, DateTime time, Dictionary<string, object> data)
        {
            var message = new MessageMode { Tag = tag, Time = new EventTime(time), Record = data };
            await SendAsync(message).ConfigureAwait(false);
        }

        /// <summary>
        /// send a data that will be converted to Message Mode to server.
        /// </summary>
        /// <param name="tag">tag</param>
        /// <param name="time">event time</param>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(string tag, DateTimeOffset time, Dictionary<string, object> data)
        {
            var message = new MessageMode { Tag = tag, Time = new EventTime(time), Record = data };
            await SendAsync(message).ConfigureAwait(false);
        }

        /// <summary>
        /// send a data will be converted to PackedForward Mode to server.
        /// </summary>
        /// <param name="tag">tag</param>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(string tag, List<Entry> data)
        {
            var message = new PackedForwardMode { Tag = tag, Entries = data };
            await SendAsync(message).ConfigureAwait(false);
        }

        /// <summary>
        /// send a data as a Message Mode to server.
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(MessageMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data, _options);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        /// <summary>
        /// send a data as a Forward Mode to server.
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(ForwardMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data, _options);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        /// <summary>
        /// send a data as a PackedForward Mode to server.
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(PackedForwardMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data, _options);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        /// <summary>
        /// send a data as a CompressedPackedForward Mode to server.
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(CompressedPackedForwardMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data, _options);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        /// <summary>
        /// destructor.
        /// </summary>
        ~PigeonClient()
        {
            Dispose(false);
        }

        /// <summary>
        /// dispose managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// dispose managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> if this method is being invoked by the <see cref="Dispose()"/> method, otherwise <c>false</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _socket?.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
