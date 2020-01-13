using EosWeb.Core.Services.Speech;
using System;
using Xunit;

namespace EosWeb.Core.Tests
{
    public class PhraseCommandShould  : IDisposable
    {
        public PhraseCommandShould()
        {
     
        }

        public void Dispose()
        {
            
        }

        [Fact]
        public void NotFail()
        {
            var command = new PhraseToken("Test One", "TestOne");
            var result = command.Process(new InputStack());
            Assert.Null(result);
            
        }

        [Theory]
        [InlineData("Test", "Test")]
        [InlineData("Test One", "TestOne")]
        [InlineData("Test Command Two", "TestCommandTwo")]
        public void ReturnGroup(string text, string value)
        {
            
            var stack = new InputStack(text);
            var command = new PhraseToken(text, value);
            var result = command.Process(stack);
            Assert.NotNull(result);
            Assert.Equal(value, result.Value);
            
        }

    }
}
