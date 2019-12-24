using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EosWeb.Blazor.Messages
{
    public class UIToggle
    {
        public string Key { get; set; }
        public bool Hidden { get; set; }

        public UIToggle(string key, bool hidden)
        {
            Key = key;
            Hidden = hidden;
        }
    }
}
