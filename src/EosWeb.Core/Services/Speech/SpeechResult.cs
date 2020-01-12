using EosWeb.Core.Messages;
using PubSub;
using System;
using System.Linq;

namespace EosWeb.Core.Services.Speech
{
    public class SpeechResult : IComparable
    {
        Hub Hub = Hub.Default;
        public object Value { get; set; } = null;
        public int Score { get; private set; } = -1;
        public int TokenCount { get; set; }

        public SpeechResult(object value, int score, int tokenCount)
        {
            Value = value;
            Score = score;
            TokenCount = tokenCount;
        }

        public SpeechResult()
        {
        }

        public void SendCommand()
        {
            Hub.Publish<EosKey>(new EosKey(Value.ToString()));
        }

        public static SpeechResult Empty
        {
            get
            {
                return new SpeechResult();
            }
        }


        public int Compare(SpeechResult x, SpeechResult y)
        {
            return x.Score.CompareTo(y.Score);
        }

        public int CompareTo(object obj)
        {
            return this.Score.CompareTo(((SpeechResult)obj).Score);
        }
    }
}
