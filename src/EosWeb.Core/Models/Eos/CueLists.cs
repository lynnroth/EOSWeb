using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EosWeb.Core.Models.Eos
{
    public class CueLists : ConcurrentDictionary<int, CueList>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var cuelist in this.Values)
            {
                sb.AppendLine(cuelist.ToString());
            }
            
            return sb.ToString();
        }
    }
}
