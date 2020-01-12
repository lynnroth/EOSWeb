using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PubSub;

namespace EosWeb.Blazor.Interop
{
    public static class SpeechInterop
    {
        [JSInvokable]
        public static string Send(string text)
        {
            Hub.Default.Publish<Speech>(new Speech() { Text = text });
            return text;
        }
    }

    public class Speech
    {
        public string Text { get; set; }
    }
}
