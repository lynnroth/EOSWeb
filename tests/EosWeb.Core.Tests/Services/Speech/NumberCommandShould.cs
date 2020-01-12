using EosWeb.Core.Services.Speech;
using System;
using Xunit;

namespace EosWeb.Core.Tests
{
    public class NumberCommandShould  : IDisposable
    {
        public NumberCommandShould()
        {
     
        }

        public void Dispose()
        {
            
        }

        [Fact]
        public void NotFail()
        {
            NumberToken command = new NumberToken();
            var result = command.Process(new InputStack());
            Assert.Null(result);
        }


        [Theory]
        [InlineData("1", 1)]
        [InlineData("12", 12)]
        [InlineData("134", 134)]
        [InlineData("1233", 1233)]
        public void ReturnNumberWithDigitInput(string text, int expectedResultValue)
        {
            NumberToken command = new NumberToken();
            var stack = new InputStack(text);
            var result = command.Process(stack);
            Assert.NotNull(result);
            Assert.Equal(expectedResultValue, result.Value);
            Assert.Equal(0, result.Score);
        }

        [Theory]
        [InlineData("1 2 3 4 5", 1,2,3,4,5)]
        [InlineData("11 12 13 14", 11, 12, 13, 14)]
        public void ReturnMultipleResultsNumberWithDigitInput(string text, params int[] expectedResultValues)
        {
            NumberToken command = new NumberToken();
            var stack = new InputStack(text);

            Assert.Equal(expectedResultValues.Length, stack.Count);
            for (int i = 0; i < stack.Count; i++)
            {
                var result = command.Process(stack);
                Assert.NotNull(result);
                Assert.Equal(expectedResultValues[i], result.Value);
                Assert.Equal(0, result.Score);
            }
        }

    }
}
