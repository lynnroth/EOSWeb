using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EosWeb.Core.Messages
{
    public enum EosUpdateItem
    {
        CueList = 1,
        ActiveCue = 2,
        PendingCue = 3,
        Group = 4,
    }

    public class EosUpdate
    {
        public EosUpdateItem Item { get; set; }
        public int? IntValue1 { get; set; }
        public int? IntValue2 { get; set; }

        public EosUpdate(EosUpdateItem item)
        {
            Item = item;
        }
        public EosUpdate(EosUpdateItem item, int? intValue1, int? intValue2)
        {
            Item = item;
            IntValue1 = intValue1;
            IntValue2 = intValue2;
        }
    }
}
