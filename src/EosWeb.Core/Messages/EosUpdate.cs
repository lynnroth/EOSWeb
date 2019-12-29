using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EosWeb.Core.Messages
{
    public enum EosUpdateItem
    {
        CueList = 1,

    }


    public class EosUpdate
    {
        public EosUpdateItem Item { get; set; }
        public EosUpdate(EosUpdateItem item)
        {
            Item = item;
        }
    }
}
