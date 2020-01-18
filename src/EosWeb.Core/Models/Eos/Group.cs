using EosWeb.Core.Services.Speech;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EosWeb.Core.Models.Eos
{
    public enum GroupMatchType
    {
        None = 0,
        Exact = 1,
        StartsWith = 2,

    }

    public class Group
    {
        public int Index { get; set; }
        public decimal Number { get; set; }
        public string UID { get; set; }
        public string Label { get; set; }
        public ConcurrentDictionary<int, Cue> Channels { get; } = new ConcurrentDictionary<int, Cue>();

        public override string ToString()
        {
            return $"{Number}:{Label}";
        }

        public GroupMatchType IsMatch(string text)
        {
            if (Label.Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                return GroupMatchType.Exact;
            }
            if (Label.StartsWith(text))
            {
                return GroupMatchType.StartsWith;
            }

            return GroupMatchType.None;
        }

        public GroupMatchType IsMatch(InputStack command, int workingIndex = 0, string currentText = "")
        {
            currentText += command.ToList()[workingIndex];

            if (Label.Equals(currentText, StringComparison.OrdinalIgnoreCase))
            {
                //Check to see if the next token also matches, if not we have an exact match.
                if (IsMatch(command, workingIndex++, currentText) == GroupMatchType.None)
                {
                    return GroupMatchType.Exact;
                }
                
            }
            if (Label.StartsWith(currentText))
            {
                return IsMatch(command, workingIndex++, currentText);
            }

            return GroupMatchType.None;
        }
    }
}
