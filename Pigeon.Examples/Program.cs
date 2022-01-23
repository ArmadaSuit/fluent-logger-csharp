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
