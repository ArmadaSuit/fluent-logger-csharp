using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pigeon.EventModes;

namespace Pigeon
{
    public interface IPigeonClient : IDisposable
    {
        Task ConnectAsync();
        Task SendAsync(string tag, Dictionary<string, object> data);
        Task SendAsync(string tag, DateTime time, Dictionary<string, object> data);
        Task SendAsync(string tag, DateTimeOffset time, Dictionary<string, object> data);
        Task SendAsync(string tag, List<Entry> data);
        Task SendAsync(MessageMode data);
        Task SendAsync(ForwardMode data);
        Task SendAsync(PackedForwardMode data);
        Task SendAsync(CompressedPackedForwardMode data);
    }
}
