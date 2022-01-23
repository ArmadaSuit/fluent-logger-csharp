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
using MessagePack;
using MessagePack.Formatters;

namespace Pigeon.EventModes
{
    /// <summary>
    /// <see href="https://github.com/fluent/fluentd/wiki/Forward-Protocol-Specification-v1#forward-mode">Forward Mode</see>
    /// </summary>
    [MessagePackFormatter(typeof(ForwardModeFormatter))]
    public class ForwardMode
    {
        /// <summary>
        /// tag name.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// list of Entry.
        /// </summary>
        public List<Entry> Entries { get; set; }

        /// <summary>
        /// option (optional).
        /// </summary>
        public Dictionary<string, object> Option { get; set; }

        class ForwardModeFormatter : IMessagePackFormatter<ForwardMode>
        {
            public void Serialize(ref MessagePackWriter writer, ForwardMode value, MessagePackSerializerOptions options)
            {
                if (value.Tag == null)
                {
                    throw new ArgumentException($"{nameof(value.Tag)} must not be null.");
                }

                if (value.Entries == null || value.Entries.Count == 0)
                {
                    throw new ArgumentException($"{nameof(value.Entries)} must have at least one item.");
                }

                var hasOption = value.Option != null && value.Option.Count != 0;

                writer.WriteArrayHeader(hasOption ? 3 : 2);
                writer.Write(value.Tag);

                var resolver = options.Resolver;
                resolver.GetFormatterWithVerify<List<Entry>>().Serialize(ref writer, value.Entries, options);

                if (hasOption)
                {
                    resolver.GetFormatterWithVerify<Dictionary<string, object>>()
                        .Serialize(ref writer, value.Option, options);
                }
            }

            public ForwardMode Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
