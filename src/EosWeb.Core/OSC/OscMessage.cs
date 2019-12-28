using System;
using System.Collections.Generic;
using System.Text;

namespace EosWeb.Core.OSC
{
    public class OscMessage
    {
        public OscAddress Address { get; }
        public List<object> Data { get; }

        public OscMessage()
        {
        }

        public OscMessage(string address, List<object> data)
        {
            Address = new OscAddress(address);
            Data = data;
        }

        public override string ToString()
        {
            StringBuilder outString = new StringBuilder();
            outString.Append(Address.Address + ": ");

            foreach (var item in Data)
            {
                outString.Append(item.ToString());
            }
            return outString.ToString();
        }
    }
}
