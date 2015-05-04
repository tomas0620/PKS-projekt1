using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PKS_projekt1
{
    class Print_Text
    {
        public Print_Text(ReadFromFile rdF, int comboboxInex, RichTextBox richTextBox1)
        {
            string request_pom = "";
            string request_pom_IP = "";
            int comunicationNumber = 1;
            int pocet1 = 0;
            int pocet2 = 0;
            Comunications_with_flags cwf;

            switch (comboboxInex)
            {
                case 0:
                    if (rdF.Append == true)
                    {
                        rdF.Append = false;
                        // pripojenie IP adries k celkovemu stringu
                        rdF.AllFrames.Append(rdF.IPaddress1.ToString() + "\r\n\n");

                        // pripojenie IP adresy uzla s najvacsim poctom odvys. bajtov + ich pocet 
                        rdF.AllFrames.Append("Adresa uzla s najväčším počtom odvysielaných bajtov: \r\n");

                        // vyhladanie ip adresy a poctu
                        // Funkcia Aggregate zlúči všetky prvky zoznamu do jedného prvku, pričom sa využije funkcia, ktorá zlúči dva prvky do jedného.
                        string dictionary_ip = (rdF.ListOfIP.Aggregate((l, r) => l.Value > r.Value ? l : r).Key).ToString();
                        rdF.AllFrames.Append(Convert.ToInt32(dictionary_ip.Substring(0, 2), 16).ToString() + "." + Convert.ToInt32(dictionary_ip.Substring(2, 2), 16).ToString() + "." + Convert.ToInt32(dictionary_ip.Substring(4, 2), 16).ToString() + "." + Convert.ToInt32(dictionary_ip.Substring(6, 2), 16).ToString() + "  ........  " + rdF.ListOfIP[dictionary_ip].ToString() + "\r\n\n");
                    }
                    // vypis celeho stringu na richtexbox
                    richTextBox1.Text = rdF.AllFrames.ToString();
                   
                    break;
                case 1:
                    foreach (var item in rdF.DictionaryFrames)
                    {
                        if (item.Key.Item2.Equals("ARP"))
                        {
                            pocet1++;
                        }
                    }
                    foreach (var item in rdF.DictionaryFrames)
                    {
                            if(item.Key.Item2.Equals("ARP") && ( pocet1>20) ){
                                if (((pocet2 < 10) || (pocet2 >= (pocet1 - 10)))) {
                                    pocet2++;
                                   /* if ((item.Key.Item4 == "request"))
                                    {
                                        request_pom = item.Value;
                                        request_pom_IP = item.Key.Item3;
                                    }
                                    else
                                    {*/
                                        if (item.Key.Item4 == "reply")
                                        {
                                            foreach (var item2 in rdF.DictionaryFrames)
                                            {
                                                if (item2.Key.Item4 == "request"  && (item2.Key.Item1<item.Key.Item1) && item2.Key.Item3.Contains(item.Key.Item3.Substring(0,16)+item.Key.Item3.Substring(28,12)))
                                                {
                                                    string IP_from = Convert.ToInt32(request_pom_IP.Substring(0, 2), 16) + "." + Convert.ToInt32(request_pom_IP.Substring(2, 2), 16) + "." + Convert.ToInt32(request_pom_IP.Substring(4, 2), 16) + "." + Convert.ToInt32(request_pom_IP.Substring(6, 2), 16);
                                                    string IP_to = Convert.ToInt32(request_pom_IP.Substring(8, 2), 16) + "." + Convert.ToInt32(request_pom_IP.Substring(10, 2), 16) + "." + Convert.ToInt32(request_pom_IP.Substring(12, 2), 16) + "." + Convert.ToInt32(request_pom_IP.Substring(14, 2), 16);
                                                    richTextBox1.AppendText("Komunikácia č." + comunicationNumber + "\r\n");
                                                    richTextBox1.AppendText("ARP-Request, IP adresa: " + IP_to + " MAC adresa: ???\r\n");
                                                    richTextBox1.AppendText("Zdrojová IP:" + IP_from + ",\tCieľová adresa: " + IP_to + "\r\n");
                                                    richTextBox1.AppendText(request_pom + "\r\n");


                                                    IP_from = Convert.ToInt32(item.Key.Item3.Substring(0, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(2, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(4, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(6, 2), 16);
                                                    IP_to = Convert.ToInt32(item.Key.Item3.Substring(8, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(10, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(12, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(14, 2), 16);
                                                    richTextBox1.AppendText("Komunikácia č." + comunicationNumber + "\r\n");
                                                    richTextBox1.AppendText("ARP-Reply, IP adresa: " + IP_to + " MAC adresa: " + item.Key.Item3.Substring(16, 2) + " " + item.Key.Item3.Substring(18, 2) + " " + item.Key.Item3.Substring(20, 2) + " " + item.Key.Item3.Substring(22, 2) + " " + item.Key.Item3.Substring(24, 2) + " " + item.Key.Item3.Substring(26, 2) + " " + "\r\n");
                                                    richTextBox1.AppendText("Zdrojová IP:" + IP_to + ",\tCieľová adresa: " + IP_from + "\r\n");
                                                    richTextBox1.AppendText(item.Value + "\r\n");
                                                    comunicationNumber++;
                                                }
                                            }
                                        }
                                    //}
                                }
                                else
                                {
                                    pocet2++;
                                }
                            }
                            else
                            {
                                if (item.Key.Item2.Equals("ARP"))
                                {
                                    /*if (item.Key.Item4 == "request")
                                    {
                                        request_pom = item.Value;
                                        request_pom_IP = item.Key.Item3;
                                    }
                                    else
                                    {*/
                                        if (item.Key.Item4 == "reply")
                                        {
                                            foreach (var item2 in rdF.DictionaryFrames)
                                            {
                                                if (item2.Key.Item3.Contains(item.Key.Item3.Substring(0, 16) + item.Key.Item3.Substring(28, 12)))
                                                {
                                                    string IP_from = Convert.ToInt32(item2.Key.Item3.Substring(0, 2), 16) + "." + Convert.ToInt32(item2.Key.Item3.Substring(2, 2), 16) + "." + Convert.ToInt32(item2.Key.Item3.Substring(4, 2), 16) + "." + Convert.ToInt32(item2.Key.Item3.Substring(6, 2), 16);
                                                    string IP_to = Convert.ToInt32(item2.Key.Item3.Substring(8, 2), 16) + "." + Convert.ToInt32(item2.Key.Item3.Substring(10, 2), 16) + "." + Convert.ToInt32(item2.Key.Item3.Substring(12, 2), 16) + "." + Convert.ToInt32(item2.Key.Item3.Substring(14, 2), 16);
                                                    richTextBox1.AppendText("Komunikácia č." + comunicationNumber + "\r\n");
                                                    richTextBox1.AppendText("ARP-request, IP adresa: " + IP_to + " MAC adresa: ???\r\n");
                                                    richTextBox1.AppendText("Zdrojová IP:" + IP_from + ",\tCieľová adresa: " + IP_to + "\r\n");
                                                    richTextBox1.AppendText(item2.Value + "\r\n");


                                                    IP_from = Convert.ToInt32(item.Key.Item3.Substring(0, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(2, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(4, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(6, 2), 16);
                                                    IP_to = Convert.ToInt32(item.Key.Item3.Substring(8, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(10, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(12, 2), 16) + "." + Convert.ToInt32(item.Key.Item3.Substring(14, 2), 16);
                                                    richTextBox1.AppendText("Komunikácia č." + comunicationNumber + "\r\n");
                                                    richTextBox1.AppendText("ARP-Reply, IP adresa: " + IP_to + " MAC adresa: " + item.Key.Item3.Substring(16, 2) + " " + item.Key.Item3.Substring(18, 2) + " " + item.Key.Item3.Substring(20, 2) + " " + item.Key.Item3.Substring(22, 2) + " " + item.Key.Item3.Substring(24, 2) + " " + item.Key.Item3.Substring(26, 2) + " " + "\r\n");
                                                    richTextBox1.AppendText("Zdrojová IP:" + IP_to + ",\tCieľová adresa: " + IP_from + "\r\n");
                                                    richTextBox1.AppendText(item.Value + "\r\n");
                                                    comunicationNumber++;
                                                    break;
                                                }
                                            }
                                            
                                        }
                                   // }
                                }
                            }

                        }
                    break;
                case 2:
                    cwf = new Comunications_with_flags("ftp-control",rdF);
                    foreach( var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "kompletna")
                        {
                            richTextBox1.AppendText("Kompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            richTextBox1.AppendText(cwf.Lengths[item.Key.Item1] + "\r\n");
                            break;
                        }
                    }
                    foreach (var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "nekompletna")
                        {
                            richTextBox1.AppendText("Nekompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            break;
                        }
                    }
                    break;
                case 3:
                    cwf = new Comunications_with_flags("ftp-data",rdF);
                    foreach( var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "kompletna")
                        {
                            richTextBox1.AppendText("Kompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            richTextBox1.AppendText(cwf.Lengths[item.Key.Item1] + "\r\n");
                            break;
                        }
                    }
                    foreach (var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "nekompletna")
                        {
                            richTextBox1.AppendText("Nekompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            break;
                        }
                    }
                    break;
                case 4:
                    cwf = new Comunications_with_flags("http",rdF);
                    foreach( var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "kompletna")
                        {
                            richTextBox1.AppendText("Kompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            richTextBox1.AppendText(cwf.Lengths[item.Key.Item1] + "\r\n");
                            break;
                        }
                    }
                    foreach (var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "nekompletna")
                        {
                            richTextBox1.AppendText("Nekompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            break;
                        }
                    }
                    break;
                case 5:
                    cwf = new Comunications_with_flags("sttps(ssl)", rdF);
                    foreach( var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "kompletna")
                        {
                            richTextBox1.AppendText("Kompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            richTextBox1.AppendText(cwf.Lengths[item.Key.Item1] + "\r\n");
                            break;
                        }
                    }
                    foreach (var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "nekompletna")
                        {
                            richTextBox1.AppendText("Nekompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            break;
                        }
                    }
                    break;
                case 6:
                    foreach (var item in rdF.DictionaryFrames)
                    {
                        if (item.Key.Item2.Equals("ICMP - all"))
                        {
                            pocet1++;
                        }
                    }

                    foreach (var item in rdF.DictionaryFrames)
                    {
                        if (item.Key.Item2.Equals("ICMP - all") && (pocet1 > 20))
                        {
                            /*if (((pocet2 < 10) || (pocet2 >= (pocet1 - 10))))
                            {*/
                                pocet2++;
                                richTextBox1.AppendText(item.Key.Item4 + "\r\n");
                                richTextBox1.AppendText(item.Value + "\r\n");
                            //}
                            /*else
                            {
                                pocet2++;
                            }*/
                         }
                       /* else
                        {
                            if (item.Key.Item2.Equals("ICMP - all"))
                            {
                                richTextBox1.AppendText(item.Key.Item4 + "\r\n");
                                richTextBox1.AppendText(item.Value + "\r\n");
                            }

                        }*/
                    }
                    break;
                case 7:
                    cwf = new Comunications_with_flags("ssh", rdF);
                    foreach( var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "kompletna")
                        {
                            richTextBox1.AppendText("Kompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            richTextBox1.AppendText(cwf.Lengths[item.Key.Item1] + "\r\n");
                            break;
                        }
                    }
                    foreach (var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "nekompletna")
                        {
                            richTextBox1.AppendText("Nekompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            break;
                        }
                    }
                    break;
                case 8:
                    cwf = new Comunications_with_flags("telnet", rdF);
                    foreach( var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "kompletna")
                        {
                            richTextBox1.AppendText("Kompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            richTextBox1.AppendText(cwf.Lengths[item.Key.Item1] + "\r\n");
                            break;
                        }
                    }
                    foreach (var item in cwf.Comunications)
                    {
                        if (item.Key.Item2 == "nekompletna")
                        {
                            richTextBox1.AppendText("Nekompletna komunikacia\r\n");
                            richTextBox1.AppendText(item.Value + "\r\n");
                            break;
                        }
                    }
                    break;
                case 9:

                    break;
                case 10:
                    Doimplementacia d = new Doimplementacia("ftp-data", rdF);
                    richTextBox1.AppendText("Pocet vsetkych ramcov s apl. protokolom ftp-data: " + d.Number_of_ftpdata + "\r\n\r\n");
                    richTextBox1.AppendText(d.Ftp_data_frames.ToString());
                    break;
            }
        }
    }
}
