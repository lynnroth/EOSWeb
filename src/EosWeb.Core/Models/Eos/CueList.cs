using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EosWeb.Core.Models.Eos
{
    public class CueList
    {
        public int Index { get; set; }
        public int Number { get; set; }
        public string UID { get; set; }
        public string Label { get; set; }

        public ConcurrentDictionary<int, Cue> Cues { get; } = new ConcurrentDictionary<int, Cue>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Index: {Index}  Number: {Number}  UID: {UID}");
            foreach (var kv in Cues)
            {
                sb.AppendLine($"Cue: {kv.Key}  {kv.Value}");
            }
            return sb.ToString();
        }
    }
}
