using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PKS_projekt1
{
    class GetInfo
    {
        private string sourceMAC = "";
        private string destinationMAC = "";

        private string frame3_type = "";
        private string frame4_type = "";
        private string sender_IP = "";
        private string target_IP = "";
        private string frame = "";
        private int length_medium = 0;
        private int pozition_SP;
        private int pozition_DP;
        private int source_port;                  // premenna pre ulozenie zdrojoveho portu v TCP/UDP 
        private int destination_port;             // premenna pre ulozenie cieloveho portu v TCP/UDP 
        private string flags = "";
        private string tuple5;                   // premenna pre ulozenie zdrojoveho, cieloveho portu a flags
        private string find_value = "";           // premenna pre ulozenie hodnoty, na zaklade ktorej sa bude vyhladavat
        private string status_or_rrIP_sIP = "";
        private string arpOper_icmpType_or_tIP;
        private string external_file;             // premenna pre ulozenie nazvu ext. suboru
        private int icmp_type_num;
        private string icmp_type;
        private int IPport;

        private int arp_operation_num;


        private StringBuilder actualFrame = new StringBuilder();


        public GetInfo(string myFrame, ReadFromFile rdF_GI, int comboIndex_GI)
        {

            //frame = myFrame.ToString();
            frame = myFrame;

            //zistenie MAC adries, nacitanie 3 a 4 pola ramca (pre zistenie typu ramca)
            destinationMAC = frame.Substring(0, 2) + " " + frame.Substring(2, 2) + " " + frame.Substring(4, 2) + " " + frame.Substring(6, 2) + " " + frame.Substring(8, 2) + " " + frame.Substring(10, 2);
            sourceMAC = frame.Substring(12, 2) + " " + frame.Substring(14, 2) + " " + frame.Substring(16, 2) + " " + frame.Substring(18, 2) + " " + frame.Substring(20, 2) + " " + frame.Substring(22, 2);
            frame3_type = frame.Substring(24, 4);
            frame4_type = frame.Substring(28, 4);

            // zistenie dlzka ramca prenasaneho po mediu 
            if ((length_medium = rdF_GI.Packet_length - 14) < 46)
            {
                length_medium = (46 - length_medium) + 4 + 14 + length_medium;
            }
            else
            {
                length_medium = rdF_GI.Packet_length + 4;
            }


            switch (frame3_type)
            {
                case "0806":
                             sender_IP = Convert.ToInt32(frame.Substring(56, 2), 16).ToString() + "." + Convert.ToInt32(frame.Substring(58, 2), 16).ToString() + "." + Convert.ToInt32(frame.Substring(60, 2), 16).ToString() + "." + Convert.ToInt32(frame.Substring(62, 2), 16).ToString();
                             arp_operation_num = Convert.ToInt32(frame.Substring(40, 4), 16);
                             if (arp_operation_num == 1)
                             {
                                 arpOper_icmpType_or_tIP = "request";
                                 status_or_rrIP_sIP = frame.Substring(56, 8) + frame.Substring(76,8) + frame.Substring(44, 12);
                             }
                             else
                             {
                                 arpOper_icmpType_or_tIP = "reply";
                                 status_or_rrIP_sIP = frame.Substring(76, 8) + frame.Substring(56, 8) + frame.Substring(44, 12) + frame.Substring(64,12);
                             }
                             find_value = "ARP";
                             break;
                case "0800":
                             sender_IP= Convert.ToInt32(frame.Substring(52, 2), 16).ToString() + "." + Convert.ToInt32(frame.Substring(54, 2), 16).ToString() + "." + Convert.ToInt32(frame.Substring(56, 2), 16).ToString() + "." + Convert.ToInt32(frame.Substring(58, 2), 16).ToString();
                             target_IP = Convert.ToInt32(frame.Substring(60, 2), 16).ToString() + "." + Convert.ToInt32(frame.Substring(62, 2), 16).ToString() + "." + Convert.ToInt32(frame.Substring(64, 2), 16).ToString() + "." + Convert.ToInt32(frame.Substring(66, 2), 16).ToString();

                             // IP s najvacsim poctom odoslanych bytov
                             if (! rdF_GI.IPaddress1.ToString().Contains(sender_IP))
                             {
                                 rdF_GI.IPaddress1.Append(sender_IP + "\r\n");
                             }
                             string actualip =frame.Substring(52, 8).ToString();
                             if (rdF_GI.ListOfIP.ContainsKey(actualip))
                             {
                                 rdF_GI.ListOfIP[actualip] += rdF_GI.Packet_length;
                             }
                             else
                             {
                                 rdF_GI.ListOfIP.Add(actualip, rdF_GI.Packet_length);
                             }

                             IPport = Convert.ToInt32(frame.Substring(46,2),16);
                                                 
                             switch(IPport){
                                 case 1:
                                     external_file = @"port_names_ICMP.txt";
                                     icmp_type_num = Convert.ToInt32(frame.Substring(28 + (Convert.ToInt32(frame4_type[0].ToString(), 16)) * (Convert.ToInt32(frame4_type[1].ToString(), 16))*2, 2), 16);
                                     find_value = "ICMP - all";
                                     icmp_type = find_portName(icmp_type_num, external_file);
                                     break;
                                 case 6:
                                     external_file = @"port_names_TCP.txt";
                                     break;
                                 case 17:
                                     external_file = @"port_names_UDP.txt";
                                     break;
                             }

                             
                             //urcenie zdrojoveho a cielove portu pre urcenie vnoreneho protokolu
                             if (IPport != 1)
                             {
                                 pozition_SP = 28 + (Convert.ToInt32(frame4_type[0].ToString(), 16)) * (Convert.ToInt32(frame4_type[1].ToString(), 16)*2);
                                 pozition_DP = pozition_SP + 4;
                                 source_port = Convert.ToInt32(frame.Substring(pozition_SP, 4), 16);
                                 destination_port = Convert.ToInt32(frame.Substring(pozition_DP, 4), 16);
                                 if (pozition_DP + 22+2<frame.Length)
                                 flags=frame.Substring(pozition_DP+22,2);
                                 tuple5 = (frame.Substring(pozition_SP,8)+flags)+rdF_GI.Packet_length;
                                 status_or_rrIP_sIP = sender_IP;
                                 arpOper_icmpType_or_tIP = target_IP;

                                 //vyhadanie mena protokolu podla portu (zo suboru)
                                 if (source_port < destination_port)
                                 {
                                     find_value = find_portName(source_port, external_file);
                                 }
                                 else
                                 {
                                     find_value = find_portName(destination_port, external_file);
                                 }
                             }
                             break;
            }

            //if (comboIndex_GI == 0)
            //{
                rdF_GI.AllFrames.Append("Ramec: " + rdF_GI.FrameN + "\r\n");
                rdF_GI.AllFrames.Append("Dlzka ramca poskytnuta paketovym drajverom: " + rdF_GI.Packet_length + "\r\n");
                rdF_GI.AllFrames.Append("Dlzka ramca prenasaneho po mediu: " + length_medium + "\r\n");
                rdF_GI.AllFrames.Append(convert_f_type());
                rdF_GI.AllFrames.Append("Zdrojova MAC adresa: " + sourceMAC + "\r\n");
                rdF_GI.AllFrames.Append("Cielova MAC adresa: " + destinationMAC + "\r\n\n");

                frame_to_string(myFrame, rdF_GI.AllFrames);

                if (IPport == 1)
                {
                    actualFrame.Append("ICMP TYPE: "+icmp_type+ "\r\n");
                }

            
                actualFrame.Append("Ramec: " + rdF_GI.FrameN + "\r\n");
                actualFrame.Append("Dlzka ramca poskytnuta paketovym drajverom: " + rdF_GI.Packet_length + "\r\n");
                actualFrame.Append("Dlzka ramca prenasaneho po mediu: " + length_medium + "\r\n");
                actualFrame.Append(convert_f_type());
                actualFrame.Append("Zdrojova MAC adresa: " + sourceMAC + "\r\n");
                actualFrame.Append("Cielova MAC adresa: " + destinationMAC + "\r\n\n");

                frame_to_string(myFrame, actualFrame);
                rdF_GI.DictionaryFrames.Add(Tuple.Create(++rdF_GI.dicnum, find_value, status_or_rrIP_sIP, arpOper_icmpType_or_tIP, tuple5), actualFrame.ToString());
            //}
            sourceMAC = "";
            destinationMAC = "";
            frame3_type = "";
            frame4_type = "";
            sender_IP = "";
            target_IP = "";
            frame = "";
            
        }

        // Convert FRAME TYPE from HEXA to STRING
        private string convert_f_type()
        {
            if (String.Compare(frame3_type, "05DC") == 1)
            {
                return "Ethernet II \r\n";
            }
            else
            {
                switch (frame4_type)
                {
                    case "AAAA":
                        return "802.3 - SNAP \r\n";
                    case "FFFF":
                        return "raw \r\n";
                    default: 
                        return "802.3 - LLC \r\n";

                }
            }
            return "BAD TYPE \r\n";
        }


        // "vypis" celkoveho ramca v upravenom tvare  
        private void frame_to_string(string frame, StringBuilder tostring )
        {
            int j = 0;

            for (int i = 0; i < frame.Length; i++)
            {
                j++;
                if (i % 2 == 0)
                {
                    tostring.Append(" ");
                }

                tostring.Append(frame[i]);
                if (j == 16)
                {
                    tostring.Append("\t");
                }
                if (j == 32)
                {
                    tostring.AppendLine();
                    j = 0;
                }

            }

            tostring.AppendLine();
            tostring.AppendLine();
        }

        private string find_portName(int port_number, string port_names_file)
        {
            string name;
            using (StreamReader sr = new StreamReader(port_names_file))
            {
                while ((name = sr.ReadLine()) != null)
                {
                    if (port_number == Convert.ToInt32(name))
                    {
                        return sr.ReadLine();
                    }
                    else
                    {
                        sr.ReadLine();
                    }
                }
            }
            return "???";
        }

        //----------------------- Getters / Setters -----------------------
        
        public string DestinationMAC
        {
            get { return destinationMAC; }
            set { destinationMAC = value; }
        }

        public string SourceMAC
        {
            get { return sourceMAC; }
            set { sourceMAC = value; }
        }




    }
}
