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
