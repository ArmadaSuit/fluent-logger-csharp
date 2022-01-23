using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pigeon.EventModes;

namespace Pigeon
{
    /// <summary>
    /// interface that Fluentd Forward Protocol client.
    /// </summary>
    public interface IPigeonClient : IDisposable
    {
        /// <summary>
        /// connect to server.
        /// </summary>
        /// <returns>Task</returns>
        Task ConnectAsync();

        /// <summary>
        /// send a data to server.
        /// </summary>
        /// <param name="tag">tag</param>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        Task SendAsync(string tag, Dictionary<string, object> data);

        /// <summary>
        /// send a data to server.
        /// </summary>
        /// <param name="tag">tag</param>
        /// <param name="time">event time</param>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        Task SendAsync(string tag, DateTime time, Dictionary<string, object> data);

        /// <summary>
        /// send a data to server.
        /// </summary>
        /// <param name="tag">tag</param>
        /// <param name="time">event time</param>
        /// <param name="data">data</param>
        /// <returns>Task</returns>
        Task SendAsync(string tag, DateTimeOffset time, Dictionary<string, object> data);

        /// <summary>
        /// send a data to server.
        /// </summary>
        /// <param name="tag">tag</param>
        /// <param name="data">Entry list</param>
        /// <returns>Task</returns>
        Task SendAsync(string tag, List<Entry> data);

        /// <summary>
        /// send a data to server.
        /// </summary>
        /// <param name="data">MessageMode data</param>
        /// <returns>Task</returns>
        Task SendAsync(MessageMode data);

        /// <summary>
        /// send a data to server.
        /// </summary>
        /// <param name="data">ForwardMode data</param>
        /// <returns>Task</returns>
        Task SendAsync(ForwardMode data);

        /// <summary>
        /// send a data to server.
        /// </summary>
        /// <param name="data">PackedForwardMode data</param>
        /// <returns>Task</returns>
        Task SendAsync(PackedForwardMode data);

        /// <summary>
        /// send a data to server.
        /// </summary>
        /// <param name="data">CompressedPackedForwardMode data</param>
        /// <returns>Task</returns>
        Task SendAsync(CompressedPackedForwardMode data);
    }
}
