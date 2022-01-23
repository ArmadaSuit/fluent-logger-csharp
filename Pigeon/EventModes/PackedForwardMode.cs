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
using System.Buffers;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;

namespace Pigeon.EventModes
{
    /// <summary>
    /// <see href="https://github.com/fluent/fluentd/wiki/Forward-Protocol-Specification-v1#packedforward-mode">PackedForwardMode Mode</see>
    /// </summary>
    [MessagePackFormatter(typeof(PackedForwardModeFormatter))]
    public class PackedForwardMode
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

        class PackedForwardModeFormatter : IMessagePackFormatter<PackedForwardMode>
        {
            public void Serialize(ref MessagePackWriter writer, PackedForwardMode value,
                MessagePackSerializerOptions options)
            {
                if (value.Tag == null)
                {
                    throw new ArgumentException($"{nameof(value.Tag)} must not be null.");
                }

                if (value.Entries == null || value.Entries.Count == 0)
                {
                    throw new ArgumentException($"{nameof(value.Entries)} must have at least one item.");
                }

                writer.WriteArrayHeader(3);
                writer.Write(value.Tag);

                var bufferWriter = new ArrayBufferWriter<byte>();
                var messagePackWriter = writer.Clone(bufferWriter);
                foreach (var entry in value.Entries)
                {
                    MessagePackSerializer.Serialize(ref messagePackWriter, entry);
                }

                messagePackWriter.Flush();

                writer.Write(bufferWriter.WrittenSpan);

                value.Option ??= new Dictionary<string, object>();
                value.Option["compressed"] = "text";
                var resolver = options.Resolver;
                resolver.GetFormatterWithVerify<Dictionary<string, object>>()
                    .Serialize(ref writer, value.Option, options);
            }

            public PackedForwardMode Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
