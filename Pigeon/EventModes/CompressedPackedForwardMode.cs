using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using MessagePack;
using MessagePack.Formatters;

namespace Pigeon.EventModes
{
    /// <summary>
    /// <see href="https://github.com/fluent/fluentd/wiki/Forward-Protocol-Specification-v1#compressedpackedforward-mode">CompressedPackedForwardMode Mode</see>
    /// </summary>
    [MessagePackFormatter(typeof(CompressedPackedForwardModeFormatter))]
    public class CompressedPackedForwardMode
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

        class CompressedPackedForwardModeFormatter : IMessagePackFormatter<CompressedPackedForwardMode>
        {
            public void Serialize(ref MessagePackWriter writer, CompressedPackedForwardMode value,
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

                using var memoryStream = new MemoryStream();
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    messagePackWriter.Flush();

                    gZipStream.Write(bufferWriter.WrittenSpan);
                }

                writer.Write(memoryStream.ToArray());

                value.Option ??= new Dictionary<string, object>();
                value.Option["compressed"] = "gzip";
                var resolver = options.Resolver;
                resolver.GetFormatterWithVerify<Dictionary<string, object>>()
                    .Serialize(ref writer, value.Option, options);
            }

            public CompressedPackedForwardMode Deserialize(ref MessagePackReader reader,
                MessagePackSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

    }
}
