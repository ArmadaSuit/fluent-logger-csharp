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
