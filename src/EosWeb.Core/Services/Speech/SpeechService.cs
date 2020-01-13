using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using PubSub;
using EosWeb.Core.Messages;

namespace EosWeb.Core.Services.Speech
{
    public class SpeechService 
    {
        IEosService EosService;
        
        SpeechProcessor SpeechProcessor;

        Hub Hub = Hub.Default;

        public SpeechService(IEosService eosService) 
        {
            EosService = eosService;
            SpeechProcessor = new SpeechProcessor(eosService);
            SetUpCommands();

            Hub.Subscribe<SpeechAvailable>((message) =>
            {
                Process(message.Text);
            });
        }

        public void Process(string text)
        {
            if (SpeechProcessor != null)
            {
                SpeechProcessor.Process(text);
            }
        }

        private void SetUpCommands()
        {
            if (SpeechProcessor == null)
            {
                return;
            }
            SpeechProcessor.Commands.Add(new NumberToken());
            SpeechProcessor.Commands.Add(new GroupToken(EosService));
            SpeechProcessor.Commands.Add(new PhraseToken("go_to_cue", "Go To Cue"));
            SpeechProcessor.Commands.Add(new PhraseToken("+%", "Up"));
            SpeechProcessor.Commands.Add(new PhraseToken("-%", "Down"));

            SpeechProcessor.Commands.Add(new WordToken("Go0", "Go"));
            SpeechProcessor.Commands.Add(new WordToken("@", "at"));
            SpeechProcessor.Commands.Add(new WordToken("Full"));
            SpeechProcessor.Commands.Add(new WordToken("Out"));
            SpeechProcessor.Commands.Add(new WordToken("Clear"));
            SpeechProcessor.Commands.Add(new WordToken("/", "slash"));
            SpeechProcessor.Commands.Add(new WordToken("Thru", "through"));
            
        }

    }
}
