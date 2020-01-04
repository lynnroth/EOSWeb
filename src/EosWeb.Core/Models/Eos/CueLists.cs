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

        public void SetActive(int cuelistNumber, int cueNumber)
        {
            foreach (var list in this.Values)
            {
                foreach (var cue in list.Cues.Values)
                {
                    if (list.Number == cuelistNumber && cue.Number == cueNumber)
                    {
                        cue.Active = true;
                    }
                    else
                    {
                        cue.Active = false;
                    }
                }
            }
        }


        public void SetPending(int cuelistNumber, int cueNumber)
        {
            foreach (var list in this.Values)
            {
                foreach (var cue in list.Cues.Values)
                {
                    if (list.Number == cuelistNumber && cue.Number == cueNumber)
                    {
                        cue.Pending = true;
                    }
                    else
                    {
                        cue.Pending = false;
                    }
                }
            }
        }
    }
}
