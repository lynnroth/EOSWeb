using System;
using System.Collections.Generic;
using System.Linq;

namespace EosWeb.Core.Services.Speech
{
    public class WordToken : BaseToken
    {
        string Word;
        List<string> Aliases = new List<string>();

        public WordToken(string word, params string[] aliases ) : base(TokenType.Text)
        {
            Word = word;

            Aliases.Add(Word); 
            foreach (var alias in aliases)
            {
                Aliases.Add(alias);
            }
            
        }

        public override SpeechResult Process(InputStack command)
        {

            if (command.Count == 0)
            {
                return null;
            }
            
            foreach (var alias in Aliases)
            {
                if (command.Peek().Equals(alias, StringComparison.OrdinalIgnoreCase))
                {
                    return new SpeechResult(Word, 0, 1);
                }
            }
            
            return null;
        }
    }

}
