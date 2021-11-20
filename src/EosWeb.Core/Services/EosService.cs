using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using PubSub;

using EosWeb.Core.Models.Eos;
using EosWeb.Core.OSC;
using EosWeb.Core.Messages;
using System.Collections.Concurrent;

namespace EosWeb.Core.Services
{
    public interface IEosService
    {
        public CueLists CueLists { get; }
        public ConcurrentDictionary<decimal, Group> Groups { get; }
        public string Version { get; set; }
        public void Load();
        public void FireCue(decimal listNumber, decimal cueNumber, int partNumber = 0);
    }

    public class EosServiceMock : IEosService
    {
        public CueLists CueLists { get; } = new CueLists();
        public ConcurrentDictionary<decimal, Group> Groups { get; } = new ConcurrentDictionary<decimal, Group>();
        public string Version { get; set; }

        public void Load()
        { 
        }

        public void FireCue(decimal listNumber, decimal cueNumber, int partNumber = 0)
        { 
        }
    }

    public class EosService : IEosService
    {
        readonly Hub Hub = Hub.Default;
        readonly IOscClient OscClient;

        public CueLists CueLists { get; } = new CueLists();
        public ConcurrentDictionary<decimal, Group> Groups { get; } = new ConcurrentDictionary<decimal, Group>();
        public string Version { get; set; }

        public EosService(IOscClient oscClient)
        {
            OscClient = oscClient;

            Hub.Subscribe<OscMessage>((message) =>
            {
                ProcessPacket(message);
            });

            Hub.Subscribe<EosKey>((message) =>
            {
                SendKey(message.Key);
            });

            Hub.Subscribe<EosMacro>((message) =>
            {
                FireMacro(message.Macro);
            });

            Load();
            
        }

        public void Load()
        {
            OscClient.SendAsync("/eos/get/cuelist/count");
            OscClient.SendAsync("/eos/get/group/count");

        }

        private void FireMacro(string macro)
        {
            OscClient.SendAsync($"/eos/user/macro/{macro}/fire");
        }

        private void SendKey(string key)
        {
            OscClient.SendAsync($"/eos/key/{key}");
        }

        public void FireCue(decimal listNumber, decimal cueNumber, int partNumber = 0)
        {
            OscClient.SendAsync($"/eos/cue/{listNumber}/{cueNumber}/{partNumber}/fire");
        }


        private void ProcessPacket(OscMessage message)
        {
            if (message.AddressParts[0] == "eos" && message.AddressParts[1] == "out")
            {
                if (message.AddressParts[2] == "active")
                {
                    HandleActiveCue(message);
                }
                else if (message.AddressParts[2] == "pending")
                {
                    HandlePendingCue(message);
                }
                else if (message.AddressParts[2] == "previous" && message.AddressParts.Count > 4)
                {
                    //if (message.AddressParts[3] == "cue")
                    //{
                    //    int cueList = message.AddressParts[4].ToInt32();
                    //    int cue = message.AddressParts[5].ToInt32();
                    //    try
                    //    {
                    //        CueLists[cueList].Cues[cue].Active = false;
                    //        Hub.Default.Publish<EosUpdate>(new EosUpdate(EosUpdateItem.CueList));
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Load();
                    //    }
                       
                    //}
                }
                else if (message.AddressParts[2] == "get")
                {
                    if (message.AddressParts[3] == "cuelist")
                    {
                        ProcessCueLists(message);
                    }
                    else if (message.AddressParts[3] == "cue")
                    {
                        ProcessCues(message);
                    }
                    else if (message.AddressParts[3] == "group")
                    {
                        ProcessGroups(message);
                    }
                    else if (message.AddressParts[3] == "version")
                    {
                        Version = message.Data[0].ToString();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Not handling /eos/out/get/" + message.AddressParts[3]);
                    }
                }
            }
        }

        private void HandlePendingCue(OscMessage message)
        {
            if (message.AddressParts[3] == "cue" && message.AddressParts.Count > 4)
            {
                if (message.AddressParts[4] == "text")
                {

                }
                else
                {
                    try
                    {
                        decimal cueList = message.AddressParts[4].ToDecimal();
                        decimal cue = message.AddressParts[5].ToDecimal();
                        CueLists.SetPending(cueList, cue);
                        Hub.Default.Publish<EosUpdate>(new EosUpdate(EosUpdateItem.CueList));
                        Hub.Default.Publish<EosUpdate>(new EosUpdate(EosUpdateItem.PendingCue, cueList, cue));
                    }
                    catch
                    {
                        Load();
                    }
                }
            }
        }

        private void HandleActiveCue(OscMessage message)
        {
            if (message.AddressParts[3] == "cue" && message.AddressParts.Count > 4)
            {
                if (message.AddressParts[4] == "text")
                {

                }
                else
                {
                    try
                    {
                        decimal cueList = message.AddressParts[4].ToDecimal();
                        decimal cue = message.AddressParts[5].ToDecimal();
                        CueLists.SetActive(cueList, cue);
                        Hub.Default.Publish<EosUpdate>(new EosUpdate(EosUpdateItem.CueList));
                        Hub.Default.Publish<EosUpdate>(new EosUpdate(EosUpdateItem.ActiveCue, cueList, cue));
                    }
                    catch
                    {
                        Load();
                    }
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
                    Number = message.AddressParts[5].ToDecimal(),
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
                Hub.Default.Publish<EosUpdate>(new EosUpdate(EosUpdateItem.CueList));
            }
        }



        private void ProcessGroups(OscMessage message)
        {
            if (message.AddressParts[4] == "count")
            {
                var count = message.Data[0].ToInt32();

                for (int i = 0; i < count; i++)
                {
                    OscClient.SendAsync($"/eos/get/group/index/{i}");
                }

            }
            else if (message.AddressParts[6] == "list")
            {
                if (message.AddressParts[5] == "channels")
                {
                    //OscClient.SendAsync($"/eos/get/group/{message.AddressParts[4]}/channels/list/{message.AddressParts[7]}/{message.AddressParts[8]}");
                }
                else
                {

                }
            }
            else if (message.AddressParts[5] == "list")
            {
                var g = new Group()
                {
                    Number = message.AddressParts[4].ToDecimal(),
                    Index = message.Data[0].ToInt32(),
                    UID = message.Data[1].ToString(),
                    Label = message.Data[2].ToString(),

                };
                System.Diagnostics.Debug.WriteLine(g);
                Groups.AddOrUpdate(g.Number, g, (k, v) => v);

                Hub.Default.Publish<EosUpdate>(new EosUpdate(EosUpdateItem.Group));
                //OscClient.SendAsync($"/eos/get/group/{g.Number}");

            }
            else
            {
                
            }

        }
        
    }


}
