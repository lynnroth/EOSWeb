using System;
using System.Collections.Generic;
using System.Linq;

namespace EosWeb.Core.Services.Speech
{
    public class InputStack : Stack<string>
    {
        public InputStack()
        {
        }

        public InputStack(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            List<string> items = text.Split(' ').ToList();
            items.Reverse();

            
            foreach (var item in items)
            {
                Push(item);
            }
            
        }
    }
}
