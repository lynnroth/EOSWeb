using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EosWeb.Core.OSC
{
    public class OscAddress
    {
        public List<string> Parts { get; }
        public string Address { get; }
        public OscAddress(string address)
        {
            Address = address;
            Parts = address.TrimStart('/').Split('/').ToList();
        }

        public override string ToString()
        {
            return Address;
        }
    }
}
