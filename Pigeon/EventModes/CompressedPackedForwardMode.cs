using System.Collections.Generic;

namespace Pigeon.EventModes
{
    public class CompressedPackedForwardMode
    {
        public string Tag { get; set; }
        public List<Entry> Entries { get; set; }
        public Dictionary<string, object> Option { get; set; }
    }
}
