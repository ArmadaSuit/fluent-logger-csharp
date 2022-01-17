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
