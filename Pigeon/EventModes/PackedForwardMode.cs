using System.Collections.Generic;

namespace Pigeon.EventModes
{
    public class PackedForwardMode
    {
        public string Tag { get; set; }
        public List<Entry> Entries { get; set; }
        public Dictionary<string, object> Option { get; set; }
    }
}
