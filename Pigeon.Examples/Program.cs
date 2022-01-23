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

namespace Pigeon.Examples
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            using var pigeonClient = new PigeonClient(new PigeonConfig("127.0.0.1", 24224));
            const string tag = "pigeon.example";
            // send single entry data
            await pigeonClient.SendAsync(tag, new Dictionary<string, object>
            {
                { "mode", "data will be converted to MessageMode" }
            });

            var entries = new List<Entry>
            {
                new Entry
                {
                    Time = new EventTime(DateTime.Now),
                    Record = new Dictionary<string, object>
                        { { "mode", "data will be converted to PackedForwardMode" } }
                }
            };
            // send multiple entry data
            await pigeonClient.SendAsync(tag, entries);

            // if specify mode
            await pigeonClient.SendAsync(new MessageMode
            {
                Tag = tag,
                Time = new EventTime(DateTime.Now),
                Record = new Dictionary<string, object>
                {
                    { "mode", "MessageMode" }
                }
            });

            await pigeonClient.SendAsync(new ForwardMode
            {
                Tag = tag,
                Entries = new List<Entry>
                {
                    new Entry
                    {
                        Time = new EventTime(DateTime.Now),
                        Record = new Dictionary<string, object> { { "mode", "ForwardMode" } }
                    }
                }
            });

            await pigeonClient.SendAsync(new PackedForwardMode
            {
                Tag = tag,
                Entries = new List<Entry>
                {
                    new Entry
                    {
                        Time = new EventTime(DateTime.Now),
                        Record = new Dictionary<string, object>
                        {
                            { "mode", "PackedForwardMode" }
                        }
                    }
                }
            });

            await pigeonClient.SendAsync(new CompressedPackedForwardMode
            {
                Tag = tag,
                Entries = new List<Entry>
                {
                    new Entry
                    {
                        Time = new EventTime(DateTime.Now),
                        Record = new Dictionary<string, object> { { "mode", "CompressedPackedForwardMode" } }
                    }
                }
            });
        }
    }
}
