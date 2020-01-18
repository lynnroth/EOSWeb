using EosWeb.Core.Services;
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
            IEosService eosService = GetMockService();

            var groupToken = new GroupToken(eosService);

            speechProcessor = new SpeechProcessor(null);
            speechProcessor.Commands.Add(new NumberToken());
            speechProcessor.Commands.Add(groupToken);
            speechProcessor.Commands.Add(new WordToken("GO"));
            speechProcessor.Commands.Add(new WordToken("THRU", "through"));
            speechProcessor.Commands.Add(new WordToken("@", "at"));
            
        }

        private static IEosService GetMockService()
        {
            IEosService eosService = new EosServiceMock();
            eosService.Groups.TryAdd(1, new Models.Eos.Group() { Index = 0, Number = 1, Label = "Front Of House" });
            eosService.Groups.TryAdd(2, new Models.Eos.Group() { Index = 1, Number = 2, Label = "Front of house stage left" });
            eosService.Groups.TryAdd(3, new Models.Eos.Group() { Index = 2, Number = 3, Label = "Cyc" });
            return eosService;
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
            Assert.Equal(6, result.Count);
            Assert.Equal(1, result[0].Value);
            Assert.NotStrictEqual("Thru", result[1].Value);
            Assert.Equal(7, result[2].Value);
            Assert.Equal("@", result[3].Value);
            Assert.Equal(50, result[4].Value);
            Assert.Equal("Enter", result[5].Value);
        }


        [Fact]
        public void PopulateResultListWithGroup()
        {
            var result = speechProcessor.Process("Front of House At 50");
            Assert.Equal(4, result.Count);
            Assert.Equal(TokenType.Group, result[0].Type);
            Assert.Equal(1.ToDecimal(), result[0].Value);
            Assert.Equal("@", result[1].Value);
            Assert.Equal(50, result[2].Value);
            Assert.Equal("Enter", result[3].Value);
        }
               
    }
}
