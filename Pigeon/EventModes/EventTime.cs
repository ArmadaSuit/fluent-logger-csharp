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

namespace Pigeon.EventModes
{
    /// <summary>
    /// time from Unix epoch in nanosecond precision.<br/>
    /// serialized to fixext 8.
    /// <see href="https://github.com/fluent/fluentd/wiki/Forward-Protocol-Specification-v1#eventtime-ext-format">EventTime Ext Format</see>
    /// </summary>
    [MessagePackFormatter(typeof(EventTimeFormatter))]
    public class EventTime
    {
        /// <summary>
        /// second part of time from Unix epoch.
        /// </summary>
        private long Seconds;

        /// <summary>
        /// nanosecond part of time from Unix epoch.
        /// </summary>
        private long NanoSeconds;

        /// <summary>
        /// constructor.
        /// </summary>
        public EventTime() : this(DateTime.Now)
        {
        }

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="seconds">seconds</param>
        /// <param name="nanoSeconds">nanoseconds</param>
        public EventTime(long seconds, long nanoSeconds)
        {
            Seconds = seconds;
            NanoSeconds = nanoSeconds;
        }

        /// <summary>
        /// constructor.<br/>
        /// in 100 nanosecond precision.
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        public EventTime(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Local)
            {
                dateTime = dateTime.ToUniversalTime();
            }

            Seconds = (dateTime.Ticks - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerSecond;
            NanoSeconds = (dateTime.Ticks % TimeSpan.TicksPerSecond) * 100;
        }

        /// <summary>
        /// constructor.<br/>
        /// in 100 nanosecond precision.
        /// </summary>
        /// <param name="dateTimeOffset">DateTimeOffset</param>
        public EventTime(DateTimeOffset dateTimeOffset)
        {
            Seconds = (dateTimeOffset.UtcTicks - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerSecond;
            NanoSeconds = (dateTimeOffset.UtcTicks % TimeSpan.TicksPerSecond) * 100;
        }

        internal class EventTimeFormatter : IMessagePackFormatter<EventTime>
        {
            public void Serialize(ref MessagePackWriter writer, EventTime value, MessagePackSerializerOptions options)
            {
                var span = writer.GetSpan(10);
                span[0] = MessagePackCode.FixExt8;
                span[1] = 0x00;
                unchecked
                {
                    span[2] = (byte) (value.Seconds >> 24);
                    span[3] = (byte) (value.Seconds >> 16);
                    span[4] = (byte) (value.Seconds >> 8);
                    span[5] = (byte) value.Seconds;
                    span[6] = (byte) (value.NanoSeconds >> 24);
                    span[7] = (byte) (value.NanoSeconds >> 16);
                    span[8] = (byte) (value.NanoSeconds >> 8);
                    span[9] = (byte) value.NanoSeconds;
                }

                writer.Advance(10);
            }

            public EventTime Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}
