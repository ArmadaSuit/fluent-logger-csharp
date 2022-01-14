using System;
using System.Collections.Generic;

namespace Pigeon.EventModes
{
    public class Entry
    {
        public DateTime Time { get; set; }
        public Dictionary<string, object> Record { get; set; }
    }
}
