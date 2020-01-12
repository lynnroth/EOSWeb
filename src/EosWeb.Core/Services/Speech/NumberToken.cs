using System;
using System.Linq;

namespace EosWeb.Core.Services.Speech
{
    public class NumberToken : BaseToken
    {
        public NumberToken() : base(TokenType.Number)
        {
        }

        public override SpeechResult Process(InputStack command)
        {
            if (command.Count == 0)
            {
                return null;
            }

            if (int.TryParse(command.Peek(), out int number))
            {
                return new SpeechResult(number, 0, 1);
            }
            return null;
        }
    }
}
