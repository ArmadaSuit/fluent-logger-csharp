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

using System.Collections.Generic;
using MessagePack;

namespace Pigeon.EventModes
{
    /// <summary>
    /// <see href="https://github.com/fluent/fluentd/wiki/Forward-Protocol-Specification-v1#entry">Entry</see>
    /// </summary>
    [MessagePackObject]
    public class Entry
    {
        /// <summary>
        /// EventTime.
        /// </summary>
        [Key(0)]
        public EventTime Time { get; set; }

        /// <summary>
        /// Record.
        /// </summary>
        [Key(1)]
        public Dictionary<string, object> Record { get; set; }
    }
}
