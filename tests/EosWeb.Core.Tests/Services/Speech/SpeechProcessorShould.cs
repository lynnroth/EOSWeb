using EosWeb.Core.Services.Speech;
using System;
using Xunit;

namespace EosWeb.Core.Tests
{
    public class SpeechProcessorShould  : IDisposable
    {
        SpeechProcessor speechProcessor;

        public SpeechProcessorShould()
        {
            speechProcessor = new SpeechProcessor(null);
            speechProcessor.Commands.Add(new NumberToken());
            speechProcessor.Commands.Add(new WordToken("GO"));
            speechProcessor.Commands.Add(new WordToken("THRU", "through"));
            speechProcessor.Commands.Add(new WordToken("@", "at"));
        }

        public void Dispose()
        {
            
        }

        [Fact]
        public void NotFailWhenEmpty()
        {
            speechProcessor.Process("");

        }

        [Fact]
        public void NotFailWhenNotEmpty()
        {
            speechProcessor.Process("1 2 3");

        }


        [Fact]
        public void PopulateResultList()
        {
            var result = speechProcessor.Process("1 Thru 7 At 50");
            Assert.Equal(5, result.Count);
            Assert.Equal(1, result[0].Value);
            Assert.NotStrictEqual("Thru", result[1].Value);
            Assert.Equal(7, result[2].Value);
            Assert.Equal("@", result[3].Value);
            Assert.Equal(50, result[4].Value);
        }

    }
}
