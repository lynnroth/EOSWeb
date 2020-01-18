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

            string c = command.Peek();

            switch (c)
            {
                case "one":
                    c = "1";
                    break;
                case "two":
                    c = "2";
                    break;
                case "three":
                    c = "3";
                    break;
                case "four":
                    c = "4";
                    break;
                case "five":
                    c = "5";
                    break;
                case "six":
                    c = "6";
                    break;
                case "seven":
                    c = "7";
                    break;
                case "eight":
                    c = "8";
                    break;
                case "nine":
                    c = "9";
                    break;
                case "zero":
                    c = "0";
                    break;
            }


            if (int.TryParse(c, out int number))
            {
                return new SpeechResult(TokenType.Number, number, 0, 1);
            }
            return null;
        }
    }
}
