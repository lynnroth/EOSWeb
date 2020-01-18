using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PubSub;
using EosWeb.Core.Messages;

namespace EosWeb.Blazor.Interop
{
    public static class SpeechInterop
    {
        [JSInvokable]
        public static string Send(string text)
        {
            Hub.Default.Publish<SpeechAvailable>(new SpeechAvailable() { Text = text });
            return text;
        }
    }

}
