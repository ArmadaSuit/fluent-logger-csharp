using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Pigeon.EventModes;

namespace Pigeon
{
    public class PigeonClient : IDisposable
    {
        private NetworkStream _stream;
        private readonly TcpClient _client;
        private readonly PigeonConfig _config;

        public PigeonClient(PigeonConfig config)
        {
            _config = config;
            _client = new TcpClient();
        }

        private async Task ConnectAsync()
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
            await _stream.FlushAsync().ConfigureAwait(false);
        }

        public async Task SendAsync(MessageMode data)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(ForwardMode data)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(PackedForwardMode data)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(CompressedPackedForwardMode data)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _client?.Dispose();
        }
    }
}
