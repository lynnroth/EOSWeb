using System;
using System.Collections.Generic;
using System.Linq;

namespace EosWeb.Core.Services.Speech
{
    public class BaseToken
    {

        public TokenType Type { get; set; }


        public BaseToken(TokenType type)
        {
        }

        public virtual SpeechResult Process(InputStack commandStack)
        {
            return SpeechResult.Empty;
        }

    }
}
