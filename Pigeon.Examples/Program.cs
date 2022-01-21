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
            await pigeonClient.SendAsync(new MessageMode
            {
                Tag = "pigeon.example",
                Time = new EventTime(DateTime.Now),
                Record = new Dictionary<string, object>
                {
                    { "mode", "MessageMode" }
                }
            });

            await pigeonClient.SendAsync(new ForwardMode
            {
                Tag = "pigeon.example",
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
                Tag = "pigeon.example",
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
                Tag = "pigeon.example",
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
