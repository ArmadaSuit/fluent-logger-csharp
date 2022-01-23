using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using MessagePack;
using Pigeon.EventModes;

namespace Pigeon
{
    /// <summary>
    /// simple implementation that Fluentd Forward Protocol client.
    /// </summary>
    public class PigeonClient : IPigeonClient
    {
        private bool _disposed;

        private Stream _stream;
        private readonly TcpClient _client;
        private readonly PigeonConfig _config;

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="config">configuration</param>
        public PigeonClient(PigeonConfig config)
        {
            _config = config;
            _client = new TcpClient();
        }

        /// <summary>
        /// connect client to server.
        /// </summary>
        /// <returns>Task</returns>
        public async Task ConnectAsync()
        {
            await _client.ConnectAsync(_config.Host, _config.Port).ConfigureAwait(false);
        }

        /// <summary>
        /// ensure connection and if not connected then connect client to server.
        /// </summary>
        /// <returns>Task</returns>
        private async Task EnsureConnected()
        {
            if (!_client.Connected)
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

            _stream = _client.GetStream();

            await _stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
            // it is unnecessary
            // await _stream.FlushAsync().ConfigureAwait(false);
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
        /// send a data to will be converted to Message Mode server.
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
        /// send a data will be converted to Message Mode to server.
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
            var bytes = MessagePackSerializer.Serialize(data);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        /// <summary>
        /// send a data as a Forward Mode to server.
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(ForwardMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        /// <summary>
        /// send a data as a PackedForward Mode to server.
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(PackedForwardMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        /// <summary>
        /// send a data as a CompressedPackedForward Mode to server.
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        public async Task SendAsync(CompressedPackedForwardMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data);
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
                    _stream?.Dispose();
                    _client?.Dispose();
                }

                _disposed = true;
            }
        }
    }
}
