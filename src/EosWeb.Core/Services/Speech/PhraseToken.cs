using System;
using System.Collections.Generic;
using System.Linq;

namespace EosWeb.Core.Services.Speech
{
    public class PhraseToken : BaseToken
    {
        string phrase;
        string[] words;
        int numberOfWords = 0;
        public string Value { get; set; }
        string Phrase
        {
            get
            {
                return phrase;
            }
            set
            {
                phrase = value;

                words = phrase.Split(' ');
                numberOfWords = words.Length;
            }
        }
        

        public PhraseToken(string phrase, string value) : base(TokenType.MultiWord)
        {
            Phrase = phrase;
            Value = value;
        }

        public override SpeechResult Process(InputStack command)
        {
            if (command.Count == 0)
            {
                return null;
            }

            string commandText = "";

            //If there aren't enough tokens, we don't have a match
            if (numberOfWords > command.Count)
            {
                return null;
            }

            for (int i = 0; i < numberOfWords; i++)
            {
                commandText += " " + command.ToList()[i].ToString();
            }

            commandText = commandText.Trim();
            if (commandText.Equals(phrase, StringComparison.OrdinalIgnoreCase))
            {
                return new SpeechResult(TokenType.MultiWord, Value, 0, numberOfWords);
            }
        
            return null;
        }
    }
}
