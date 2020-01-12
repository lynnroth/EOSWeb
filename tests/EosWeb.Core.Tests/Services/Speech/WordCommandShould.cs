using EosWeb.Core.Services.Speech;
using System;
using Xunit;

namespace EosWeb.Core.Tests
{
    public class WordCommandShould  : IDisposable
    {
        public WordCommandShould()
        {
     
        }

        public void Dispose()
        {
            
        }

        [Fact]
        public void NotFailWithEmptyCommand()
        {
            var command = new WordToken("Test");
            var result = command.Process(new InputStack());
            Assert.Null(result);
        }

    }
}
