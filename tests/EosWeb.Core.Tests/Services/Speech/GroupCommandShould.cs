using EosWeb.Core.Services;
using EosWeb.Core.Services.Speech;
using System;
using Xunit;

namespace EosWeb.Core.Tests
{
    public class GroupCommandShould : IDisposable
    {
        public GroupCommandShould()
        {
     
        }

        public void Dispose()
        {
            
        }

        [Fact]
        public void NotFailWithEmptyCommand()
        {
            IEosService eosService = GetMockService();

            var command = new GroupToken(eosService);
            var result = command.Process(new InputStack());
            Assert.Null(result);
        }


        [Theory]
        [InlineData("Test", 1)]
        [InlineData("Test One", 2)]
        [InlineData("Test Group Two", 3)]
        public void ReturnGroup(string text, decimal groupNumber)
        {
            IEosService eosService = GetMockService();
            var stack = new InputStack(text);
            
            var command = new GroupToken(eosService);
            var result = command.Process(stack);
            Assert.NotNull(result);
            Assert.Equal(groupNumber, result.Value);
        }

        private static IEosService GetMockService()
        {
            IEosService eosService = new EosServiceMock();
            eosService.Groups.TryAdd(1, new Models.Eos.Group() { Index = 0, Number = 1, Label = "Test" });
            eosService.Groups.TryAdd(2, new Models.Eos.Group() { Index = 1, Number = 2, Label = "Test One" });
            eosService.Groups.TryAdd(3, new Models.Eos.Group() { Index = 2, Number = 3, Label = "Test Group Two" });
            return eosService;
        }
    }
}
