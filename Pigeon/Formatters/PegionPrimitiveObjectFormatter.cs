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
using System.Collections;
using System.Linq;
using System.Reflection;
using MessagePack;
using MessagePack.Formatters;

namespace Pigeon.Formatters
{
    public sealed class PigeonPrimitiveObjectFormatter : IMessagePackFormatter<object>
    {
        public static readonly PigeonPrimitiveObjectFormatter Instance = new PigeonPrimitiveObjectFormatter();

        private PigeonPrimitiveObjectFormatter() { }

        private static readonly ArrayFormatter<DateTime> DateTimeArrayFormatter = new ArrayFormatter<DateTime>();

        private static readonly ArrayFormatter<DateTimeOffset> DateTimeOffsetArrayFormatter =
            new ArrayFormatter<DateTimeOffset>();

        private readonly IMessagePackFormatter<DateTime> _dateTimeFormatter;

        private readonly IMessagePackFormatter<DateTimeOffset> _dateTimeOffsetFormatter;

        public PigeonPrimitiveObjectFormatter(IMessagePackFormatter<DateTime> dateTimeFormatter,
            IMessagePackFormatter<DateTimeOffset> dateTimeOffsetFormatter)
        {
            _dateTimeFormatter = dateTimeFormatter;
            _dateTimeOffsetFormatter = dateTimeOffsetFormatter;
        }

        public void Serialize(ref MessagePackWriter writer, object value, MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            switch (value)
            {
                case DateTime time:
                    _dateTimeFormatter.Serialize(ref writer, time, options);
                    return;
                case DateTimeOffset dateTimeOffset:
                    _dateTimeOffsetFormatter.Serialize(ref writer, dateTimeOffset, options);
                    return;
                case DateTime[] dateTimes:
                    DateTimeArrayFormatter.Serialize(ref writer, dateTimes, options);
                    return;
                case DateTimeOffset[] dateTimeOffsets:
                    DateTimeOffsetArrayFormatter.Serialize(ref writer, dateTimeOffsets, options);
                    return;
                case IDictionary dictionary:
                {
                    // check IDictionary first
                    writer.WriteMapHeader(dictionary.Count);
                    foreach (DictionaryEntry item in dictionary)
                    {
                        Serialize(ref writer, item.Key, options);
                        Serialize(ref writer, item.Value, options);
                    }

                    return;
                }
                case ICollection collection:
                {
                    writer.WriteArrayHeader(collection.Count);
                    foreach (var item in collection)
                    {
                        Serialize(ref writer, item, options);
                    }

                    return;
                }
                default:
                {
                    var type = value.GetType();
                    var typeInfo = type.GetTypeInfo();
                    if (PrimitiveObjectFormatter.IsSupportedType(type, typeInfo, value))
                    {
                        PrimitiveObjectFormatter.Instance.Serialize(ref writer, value, options);
                        return;
                    }

                    // References
                    // https://github.com/ttakahari/FluentdClient.Sharp/blob/1.1.0/src/FluentdClient.Sharp.MessagePack/Payload.cs#L171-L184
                    if (type.GetTypeInfo().IsAnonymous())
                    {
                        var properties = type.GetProperties();

                        var dictionary = properties.ToDictionary(x => x.Name, x => x.GetValue(value));

                        writer.WriteMapHeader(dictionary.Count);

                        foreach (var item in dictionary)
                        {
                            Serialize(ref writer, item.Key, options);
                            Serialize(ref writer, item.Value, options);
                        }

                        return;
                    }

                    DynamicObjectTypeFallbackFormatter.Instance.Serialize(ref writer, value, options);
                    return;
                }
            }
        }

        public object Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
