using EosWeb.Core.Models.Eos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EosWeb.Core.Services.Speech
{
    public class GroupToken : BaseToken
    {
        IEosService EosService;
        public GroupToken(IEosService eosService) : base(TokenType.Group)
        {
            EosService = eosService;
        }
        
        public override SpeechResult Process(InputStack command)
        {
            if (command.Count == 0)
            {
                return null;
            }

            SortedSet<SpeechResult> results = new SortedSet<SpeechResult>();

            foreach (var group in EosService.Groups.Values)
            {
                var label = group.Label;
                var labelParts = label.Split(' ');

                string commandText = "";

                //If there aren't enough tokens, we don't have a match
                if (labelParts.Length > command.Count)
                {
                    continue;
                }

                for (int i = 0; i < labelParts.Length; i++)
                {
                    commandText += " " + command.ToList()[i].ToString();       
                }
                commandText = commandText.Trim();
                if (commandText.Equals(label, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(new SpeechResult(TokenType.Group, group.Number, 100 - labelParts.Length, labelParts.Length));
                }
            }

            if (results.Count > 0)
            {
                return results.First();
            }
            return null;
        }
    }
}
