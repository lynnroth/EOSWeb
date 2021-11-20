using System;

namespace EosWeb.Core.Models.Eos
{
    public class Cue
    {
        public bool Active { get; set; }
        public bool Pending { get; set; }
        public string RowClass
        {
            get
            {
                return Active ? "cue-active border-light" : "";
            }
        }
        public decimal Index { get; set; }
        public decimal Number { get; set; }
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
