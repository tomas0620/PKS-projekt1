using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PcapDotNet.Base;
using PcapDotNet.Core;
using PcapDotNet.Core.Extensions;
using PcapDotNet.Packets;

using System.Diagnostics;

namespace PKS_projekt1
{

    class ReadFromFile
    {

        private int frameN = 0;
        private int packet_length = 0;
        private GetInfo info;
        
        private StringBuilder allFrames = new StringBuilder();
        private StringBuilder IPaddress = new StringBuilder();

        

        private Dictionary<string, int> listOfIP = new Dictionary<string, int>();
        private Dictionary<Tuple<int, string, string,string,string>, string> dictionaryFrames = new Dictionary<Tuple<int, string, string, string, string>, string>();
       
      //  private Dictionary<int, string> basic_frames = new Dictionary<int, string>();

        public int dicnum = 0; // rozdielovy kluc pri "slovniku" jednotlivych ramcov
        private int comboIndex_rdF;
        

        public long stopping_time;

        private bool append = true;

        private string packet_string;

        public ReadFromFile(string file, int comboIndex)
        {
            OfflinePacketDevice selectedfile = new OfflinePacketDevice(file);

            allFrames.Clear();
            frameN = 0;
            comboIndex_rdF = comboIndex;

            using (PacketCommunicator communicator = selectedfile.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000)) //prevzane z https://pcapdotnet.codeplex.com/wikipage?title=Pcap.Net%20Tutorial%20-%20Handling%20offline%20dump%20files
            {

                Stopwatch stopping = new Stopwatch();
                stopping.Start();
                // nacitava ramce do konca suboru
                communicator.ReceivePackets(0, DispatcherHandler);
                stopping.Stop();
                stopping_time = stopping.ElapsedMilliseconds;
            }
            
        }

        private void DispatcherHandler(Packet packet)
        {
            frameN++;
            byte[] packet_bytes = new byte[packet.Length];
            packet_bytes = packet.Buffer;
            packet_string = BitConverter.ToString(packet_bytes);
            string[] asdasd= packet_string.Split('-');
            packet_string = string.Join("",asdasd);
          
            packet_length = packet.Length;
           // basic_frames.Add(frameN, packet_string);
            info = new GetInfo(packet_string, this, comboIndex_rdF);
            packet_length = 0;
        }
        


        //----------------------- Getters / Setters -----------------------
        public StringBuilder AllFrames
        {
            get { return allFrames; }
        }

        public int FrameN
        {
            get { return frameN; }
        }

        public int Packet_length
        {
            get { return packet_length; }
        }

        public StringBuilder IPaddress1
        {
            get { return IPaddress; }
        }

        public Dictionary<string, int> ListOfIP
        {
            get { return listOfIP; }
            set { listOfIP = value; }
        }

        public Dictionary<Tuple<int, string, string, string, string>, string> DictionaryFrames
        {
            get { return dictionaryFrames; }
            set { dictionaryFrames = value; }
        }

        public bool Append
        {
            get { return append; }
            set { append = value; }
        }

        public string Packet_string
        {
            get { return packet_string; }
            set { packet_string = value; }
        }
    }
}
