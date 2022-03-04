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
using MessagePack;
using MessagePack.Formatters;

namespace Pigeon.Formatters
{
    public static class DateAndTimeFormatters
    {
        public sealed class DateTimeUnixTimeFormatter : IMessagePackFormatter<DateTime>
        {
            public static readonly IMessagePackFormatter<DateTime> Instance = new DateTimeUnixTimeFormatter();

            public void Serialize(ref MessagePackWriter writer, DateTime value, MessagePackSerializerOptions options)
            {
                if (value.Kind == DateTimeKind.Local)
                {
                    value = value.ToUniversalTime();
                }

                var seconds = (value.Ticks - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerSecond;
                writer.Write(seconds);
            }

            public DateTime Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        public sealed class DateTimeOffsetUnixTimeFormatter : IMessagePackFormatter<DateTimeOffset>
        {
            public static readonly IMessagePackFormatter<DateTimeOffset> Instance =
                new DateTimeOffsetUnixTimeFormatter();

            public void Serialize(ref MessagePackWriter writer, DateTimeOffset value,
                MessagePackSerializerOptions options)
            {
                var seconds = (value.UtcTicks - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerSecond;
                writer.Write(seconds);
            }

            public DateTimeOffset Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        public sealed class DateTimeStringFormatter : IMessagePackFormatter<DateTime>
        {
            public string format { get; set; } = "o";

            public static readonly IMessagePackFormatter<DateTime> Instance = new DateTimeStringFormatter();

            public void Serialize(ref MessagePackWriter writer, DateTime value, MessagePackSerializerOptions options)
            {
                writer.Write(value.ToString(format));
            }

            public DateTime Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        public sealed class DateTimeOffsetStringFormatter : IMessagePackFormatter<DateTimeOffset>
        {
            public string format { get; set; } = "o";

            public static readonly IMessagePackFormatter<DateTimeOffset> Instance =
                new DateTimeOffsetStringFormatter();

            public void Serialize(ref MessagePackWriter writer, DateTimeOffset value,
                MessagePackSerializerOptions options)
            {
                writer.Write(value.ToString(format));
            }

            public DateTimeOffset Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

    }
}
