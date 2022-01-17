using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using MessagePack;
using Pigeon.EventModes;

namespace Pigeon
{
    public class PigeonClient : IDisposable
    {
        private NetworkStream _stream;
        private readonly TcpClient _client;
        private readonly PigeonConfig _config;

        private bool _disposed;

        public PigeonClient(PigeonConfig config)
        {
            _config = config;
            _client = new TcpClient();
        }

        public async Task ConnectAsync()
        {
            await _client.ConnectAsync(_config.Host, _config.Port).ConfigureAwait(false);
        }

        public async Task SendAsync(byte[] data)
        {
            if (!_client.Connected)
            {
                await ConnectAsync().ConfigureAwait(false);
            }

            _stream = _client.GetStream();

            await _stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
            // it is unnecessary
            // await _stream.FlushAsync().ConfigureAwait(false);
        }

        public async Task SendAsync(string tag, Dictionary<string, object> data)
        {
            var message = new MessageMode { Tag = tag, Time = new EventTime(), Record = data };
            await SendAsync(message).ConfigureAwait(false);
        }

        public async Task SendAsync(string tag, DateTime time, Dictionary<string, object> data)
        {
            var message = new MessageMode { Tag = tag, Time = new EventTime(time), Record = data };
            await SendAsync(message).ConfigureAwait(false);
        }

        public async Task SendAsync(string tag, DateTimeOffset time, Dictionary<string, object> data)
        {
            var message = new MessageMode { Tag = tag, Time = new EventTime(time), Record = data };
            await SendAsync(message).ConfigureAwait(false);
        }

        public async Task SendAsync(string tag, List<Entry> data)
        {
            var message = new PackedForwardMode { Tag = tag, Entries = data };
            await SendAsync(message).ConfigureAwait(false);
        }

        public async Task SendAsync<T>(string tag, T data)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(MessageMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        public async Task SendAsync(ForwardMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        public async Task SendAsync(PackedForwardMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        public async Task SendAsync(CompressedPackedForwardMode data)
        {
            var bytes = MessagePackSerializer.Serialize(data);
            await SendAsync(bytes).ConfigureAwait(false);
        }

        ~PigeonClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }

                _stream?.Dispose();
                _client?.Dispose();

                _disposed = true;
            }
        }
    }
}
