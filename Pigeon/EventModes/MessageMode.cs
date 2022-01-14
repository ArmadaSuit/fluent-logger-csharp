using System;
using System.Collections.Generic;

namespace Pigeon.EventModes
{
    public class MessageMode
    {
        public string Tag { get; set; }
        public DateTime Time { get; set; }
        public Dictionary<string, object> Record { get; set; }
        public Dictionary<string, object> Option { get; set; }
    }
}
