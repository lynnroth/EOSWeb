using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using PubSub;

using EosWeb.Core.Models.Eos;
using EosWeb.Core.OSC;
using EosWeb.Core.Messages;

namespace EosWeb.Core.Services
{
    public class EosService
    {
        readonly Hub Hub = Hub.Default;
        readonly IOscClient OscClient;

        public CueLists CueLists { get; } = new CueLists();

        public EosService(IOscClient oscClient)
        {
            OscClient = oscClient;

            Hub.Subscribe<OscMessage>((message) =>
            {
                ProcessPacket(message);
            });

            OscClient.SendAsync("/eos/get/cuelist/count");

        }

        public void Load()
        {
            OscClient.SendAsync("/eos/get/cuelist/count"); 
        }
        private void ProcessPacket(OscMessage message)
        {
            if (message.AddressParts[0] == "eos" && message.AddressParts[1] == "out" && message.AddressParts[2] == "get")
            {
                if (message.AddressParts[3] == "cuelist")
                {
                    ProcessCueLists(message);
                }
                else if (message.AddressParts[3] == "cue")
                {
                    ProcessCues(message);
                }
            }
        }

        private void ProcessCueLists(OscMessage message)
        {
            if (message.AddressParts[4] == "count") // /eos/out/get/cuelist/count
            {
                foreach (var d in message.Data)
                {
                    if (int.TryParse(d.ToString(), out int count))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            OscClient.SendAsync($"/eos/get/cuelist/index/{i}");
                            //OscClient.SendAsync($"/eos/get/cue/index/1/count");
                            //OscClient.SendAsync($"/eos/get/cue/1/index/{i}");
                        }
                    }
                }
            }
            else if (message.AddressParts[5] == "list") // /eos/out/get/cuelist/1/list/0/13
            {
                var cl = new CueList()
                {
                    Index = message.AddressParts[6].ToInt32(),
                    Number = message.AddressParts[4].ToInt32(),
                    UID = message.Data[1].ToString()
                };

                CueLists.AddOrUpdate(cl.Number, cl, (k, v) => v);
                Hub.Default.Publish<EosUpdate>(new EosUpdate(EosUpdateItem.CueList));
                OscClient.SendAsync($"/eos/get/cue/{cl.Number}/count");
            }
        }


        private void ProcessCues(OscMessage message)
        {
            if (message.AddressParts[5] == "count") // /eos/out/get/cue/{cuelistnumber}/count
            {

                var count = message.Data[0].ToInt32();
                var clNumber = message.AddressParts[4].ToInt32();

                for (int i = 0; i < count; i++)
                {
                    OscClient.SendAsync($"/eos/get/cue/{clNumber}/index/{i}");
                }

            }
            else if (message.AddressParts[7] == "list") // /eos/out/get/cuelist/1/list/0/13
            {
                //Cue
                //  ListNumber a4
                //  CueNumber a5
                //  CuePartNumber a6
                //  Cue UID D1    
                // Cue Label D2
                var cl = CueLists[message.AddressParts[4].ToInt32()];
                var c = new Cue()
                {
                    Number = message.AddressParts[5].ToInt32(),
                    PartNumber = message.AddressParts[6].ToInt32(),
                    Index = message.Data[0].ToInt32(),
                    UID = message.Data[1].ToString(),
                    Label = message.Data[2].ToString(),
                    UpIntensityDuration = TimeSpan.FromMilliseconds(message.Data[3].ToInt32()),
                    DownIntensityDuration = TimeSpan.FromMilliseconds(message.Data[5].ToInt32()),
                    Note = message.Data[27].ToString()
                };

                cl.Cues.AddOrUpdate(c.Number, c, (k, v) => v);
                //foreach (var d in message.Data)
                //{
                //    if (int.TryParse(d.ToString(), out int count))
                //    {
                //        for (int i = 0; i < count; i++)
                //        {
                //            //OscClient.SendAsync($"/eos/get/cuelist/index/{i}");
                //        }
                //    }
                //}
                //CueLists.AddOrUpdate(cl.Number, cl, (k, v) => v);
                //Hub.Default.Publish<EosUpdate>(new EosUpdate(EosUpdateItem.CueList));
                //OscClient.SendAsync($"/eos/get/cue/{cl.Number}/count");
            }
        }
    }


}
