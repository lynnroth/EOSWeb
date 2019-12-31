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

    public class Cue
    {
        public bool Active { get; set; }
        
        public string RowClass
        {
            get
            {
                return Active ? "cue-active border-light" : "";
            }
        }
        public int Index { get; set; }
        public int Number { get; set; }
        public string NumberWithPart
        {
            get { return Number + (PartNumber == 0 ? "" : "." + PartNumber); }
        }
        public int PartNumber { get; set; }
        public string UID { get; set; }
        public string Label { get; set; }
        public TimeSpan UpIntensityDuration { get; set; }
        public TimeSpan DownIntensityDuration { get; set; }
        public string Note { get; set; }

        public override string ToString()
        {
            return $"Index: {Index}  Number: {Number}  Label: {Label}  Up: {UpIntensityDuration}  Down: {DownIntensityDuration}";
        }

    }
}
