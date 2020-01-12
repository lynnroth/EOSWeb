using System;
using System.Linq;

namespace EosWeb.Core.Services.Speech
{
    public class PhraseToken : BaseToken
    {
        string phrase;

        string Phrase
        {
            get
            {
                return phrase;
            }
            set
            {
                phrase = value;
                numberOfWords = phrase.Split(' ').Length;
            }
        }
        int numberOfWords = 0;

        public PhraseToken(string phrase) : base(TokenType.MultiWord)
        {
            Phrase = phrase;
        }

        public override SpeechResult Process(InputStack command)
        {
            if (command.Count == 0)
            {
                return null;
            }
            //foreach (var alias in Aliases)
            //{
            //    if (command.Peek().Equals(alias, StringComparison.OrdinalIgnoreCase))
            //    {
            //        command.Pop();
            //        return new SpeechResult(Word, 0);
            //    }
            //}

            return null;
        }
    }
}
