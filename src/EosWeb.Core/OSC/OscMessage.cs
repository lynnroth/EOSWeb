using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EosWeb.Core.OSC
{
    public class OscMessage
    {

        public List<string> AddressParts { get; }
        public string Address { get; }
        public List<object> Data { get; }

        public OscMessage()
        {
        }

        public OscMessage(string address, List<object> data)
        {
            Address = address;
            AddressParts = address.TrimStart('/').Split('/').ToList();
            Data = data;
        }

        public override string ToString()
        {
            StringBuilder outString = new StringBuilder();
            outString.Append(Address + ": ");

            foreach (var item in Data)
            {
                outString.Append(item.ToString());
            }
            return outString.ToString();
        }
    }
}
