using System.Collections.Generic;
using MessagePack;

namespace Pigeon.EventModes
{
    /// <summary>
    /// <see href="https://github.com/fluent/fluentd/wiki/Forward-Protocol-Specification-v1#entry">Entry</see>
    /// </summary>
    [MessagePackObject]
    public class Entry
    {
        /// <summary>
        /// EventTime.
        /// </summary>
        [Key(0)]
        public EventTime Time { get; set; }

        /// <summary>
        /// Record.
        /// </summary>
        [Key(1)]
        public Dictionary<string, object> Record { get; set; }
    }
}
