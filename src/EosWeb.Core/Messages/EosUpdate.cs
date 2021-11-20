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
        public decimal? Value1 { get; set; }
        public decimal? Value2 { get; set; }

        public EosUpdate(EosUpdateItem item)
        {
            Item = item;
        }
        public EosUpdate(EosUpdateItem item, decimal? intValue1, decimal? intValue2)
        {
            Item = item;
            Value1 = intValue1;
            Value2 = intValue2;
        }
    }
}
