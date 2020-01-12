using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EosWeb.Core.Messages
{
    public class EosKey
    {
        public string Key { get; set; }
        public EosKey(string key)
        {
            Key = key;
        }
    }
}
