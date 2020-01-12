using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EosWeb.Core.Messages
{
    public class EosMacro
    {
        public string Macro { get; set; }

        public EosMacro(string macro)
        {
            Macro = macro;
        }
    }
}
