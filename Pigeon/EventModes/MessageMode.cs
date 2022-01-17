using System;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;

namespace Pigeon.EventModes
{
    /// <summary>
    /// <see href="https://github.com/fluent/fluentd/wiki/Forward-Protocol-Specification-v1#message-modes">Message Mode</see>
    /// </summary>
    [MessagePackFormatter(typeof(MessageModeFormatter))]
    public class MessageMode
    {
        /// <summary>
        /// tag name.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// EventTime.
        /// </summary>
        public EventTime Time { get; set; }

        /// <summary>
        /// event record.
        /// </summary>
        public Dictionary<string, object> Record { get; set; }

        /// <summary>
        /// option (optional).
        /// </summary>
        public Dictionary<string, object> Option { get; set; }

        class MessageModeFormatter : IMessagePackFormatter<MessageMode>
        {
            public void Serialize(ref MessagePackWriter writer, MessageMode value, MessagePackSerializerOptions options)
            {
                if (value.Tag == null)
                {
                    throw new ArgumentException($"{nameof(value.Tag)} must not be null.");
                }

                if (value.Record == null)
                {
                    throw new ArgumentException($"{nameof(value.Record)} must not be null.");
                }

                var hasOption = value.Option != null && value.Option.Count != 0;

                writer.WriteArrayHeader(hasOption ? 4 : 3);
                writer.Write(value.Tag);

                var resolver = options.Resolver;
                resolver.GetFormatterWithVerify<EventTime>().Serialize(ref writer, value.Time, options);
                resolver.GetFormatterWithVerify<Dictionary<string, object>>()
                    .Serialize(ref writer, value.Record, options);

                if (hasOption)
                {
                    resolver.GetFormatterWithVerify<Dictionary<string, object>>()
                        .Serialize(ref writer, value.Option, options);
                }
            }

            public MessageMode Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
