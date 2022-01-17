using System;
using System.Collections.Generic;
using Pigeon.EventModes;

namespace Pigeon.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var pigeonClient = new PigeonClient(new PigeonConfig("127.0.0.1", 24224));
            pigeonClient.SendAsync(new MessageMode
            {
                Tag = "pigeon.example",
                Time = new EventTime(DateTime.Now),
                Record = new Dictionary<string, object>
                {
                    { "mode", "MessageMode" }
                }
            }).Wait();

            pigeonClient.SendAsync(new ForwardMode
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
            }).Wait();

            pigeonClient.SendAsync(new PackedForwardMode
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
            }).Wait();

            pigeonClient.SendAsync(new CompressedPackedForwardMode
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
            }).Wait();
        }
    }
}
