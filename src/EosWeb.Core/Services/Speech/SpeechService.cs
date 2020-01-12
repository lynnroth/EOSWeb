using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;

namespace EosWeb.Core.Services.Speech
{
    public class SpeechService 
    {
        IConfiguration Configuration;
        EosService EosService;
        OscClientService OscClientService;

        SpeechProcessor SpeechProcessor;
        

        public SpeechService()
        {
            SetUpCommands();
        }

        public SpeechService(IConfiguration configuration, EosService eosService, OscClientService oscClientService) : this()
        {
            Configuration = configuration;
            EosService = eosService;
            OscClientService = oscClientService;

            SpeechProcessor = new SpeechProcessor(eosService);
        }

        public void Process(string text)
        {
            SpeechProcessor.Process(text);
        }

        private void SetUpCommands()
        {
            if (SpeechProcessor == null)
            {
                return;
            }
            SpeechProcessor.Commands.Add(new NumberToken());
            SpeechProcessor.Commands.Add(new WordToken("Go"));
            SpeechProcessor.Commands.Add(new PhraseToken("Go To Cue"));
        }

    }
}
