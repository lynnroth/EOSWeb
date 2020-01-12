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
            var command = new PhraseToken("Test One");
            var result = command.Process(new InputStack());
            Assert.Null(result);
            
        }

    }
}
