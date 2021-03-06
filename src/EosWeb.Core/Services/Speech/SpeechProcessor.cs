﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace EosWeb.Core.Services.Speech
{
    public class SpeechResultList : List<SpeechResult>
    {

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var speechResult in this)
            {
                sb.Append(speechResult.Value);
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }

    public class SpeechProcessor
    {
        public List<BaseToken> Commands = new List<BaseToken>();
        IEosService EosService;
        
        public SpeechProcessor(IEosService eosService)
        {
            EosService = eosService;
            
        }

        public List<SpeechResult> Process(string text)
        {
            var command = new InputStack(text);

            var resultList = new SpeechResultList();

            while (command.Count > 0)
            {
                if (string.IsNullOrEmpty(command.Peek()))
                {
                    command.Pop();
                    continue;
                }

                SortedSet<SpeechResult> results = new SortedSet<SpeechResult>();
                foreach (var speechCommand in Commands)
                {
                    var result = speechCommand.Process(command);
                    if (result != null)
                    {
                        results.Add(result);
                    }
                }

                if (results.Count > 0)
                {
                    SpeechResult bestResult = results.First();
                    for (int i = 0; i < bestResult.TokenCount; i++)
                    {
                        command.Pop();
                    }

                    resultList.Add(bestResult);
                }
                else
                {
                    command.Pop();
                }
            }

            resultList.Add(new SpeechResult(TokenType.Text, "Enter", 0, 0));
            resultList.ForEach(x => x.SendCommand());

            return resultList;
        }


    }
}
