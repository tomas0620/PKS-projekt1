using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS_projekt1
{
    class Comunications_with_flags
    {
        private StringBuilder comunications_help_str = new StringBuilder();
        private Dictionary<Tuple<int, string>, string> comunications = new Dictionary<Tuple<int, string>, string>();
        private Dictionary<Tuple<int, int>, int> lengths_pom = new Dictionary<Tuple<int, int>, int>();
        private Dictionary<int, string> lengths = new Dictionary<int, string>();


        public Comunications_with_flags(string selected, ReadFromFile rdF)
        {
            bool synack = false, ack = false, fin_k1 = false, fin_s = false, fin_k2 = false, fin_ack = false, full = false;
            int s_port=0 , d_port=0;

            int comunication_num = 1;
            string client_IP = "" , server_IP = "";
            int from = 0;
            var key = Tuple.Create(0,"");
            int number_of_frames = 1;
            int framelength = 0 ;

            inicialize();

            foreach (var item in rdF.DictionaryFrames)
            {
                if (item.Key.Item2 == selected)
                {
                    long pom = Convert.ToInt64(item.Key.Item5.Substring(8,2),16);
                    string binary = Convert.ToString(pom,2).PadLeft(5,'0');

                    if ((s_port == 0) && (d_port == 0) && binary == "00010")
                    {
                        number_of_frames = 0;
                        s_port = Convert.ToInt32(item.Key.Item5.Substring(0, 4), 16);
                        d_port = Convert.ToInt32(item.Key.Item5.Substring(4, 4), 16);
                        client_IP = item.Key.Item3;
                        server_IP = item.Key.Item4;
                        string pom_Str = "Klient: " + client_IP + " : " + +Convert.ToInt32(item.Key.Item5.Substring(0, 4), 16) + "  \t Server: " + server_IP + " : " + item.Key.Item2 + "(" + Convert.ToInt32(item.Key.Item5.Substring(4, 4), 16) + ")" + "\r\n" + item.Value.ToString();
                        comunications_help_str.Clear();
                        comunications_help_str.Append(pom_Str);
                        comunications.Add(Tuple.Create(comunication_num, "nekompletna"), comunications_help_str.ToString());
                        from++;
                        full = false;
                        int sub_number = 3;
                        framelength = Convert.ToInt32(item.Key.Item5.Substring(10));
                        length_counter(framelength);


                        foreach (var item2_pom in rdF.DictionaryFrames.Skip(from))
                        {
                            if (item2_pom.Key.Item2 == selected)
                            {
                                if (((client_IP == item2_pom.Key.Item3 && server_IP == item2_pom.Key.Item4) || (client_IP == item2_pom.Key.Item4 && server_IP == item2_pom.Key.Item3)) && (Convert.ToInt32(item2_pom.Key.Item5.Substring(0, 4), 16) == s_port && Convert.ToInt32(item2_pom.Key.Item5.Substring(4, 4), 16) == d_port) || (Convert.ToInt32(item2_pom.Key.Item5.Substring(0, 4), 16) == d_port && Convert.ToInt32(item2_pom.Key.Item5.Substring(4, 4), 16) == s_port))
                                {
                                    number_of_frames++;
                                }
                            }

                        }
                        foreach (var item2 in rdF.DictionaryFrames.Skip(from))
                        {
                            if (item2.Key.Item2 == selected)
                            {

                                pom = Convert.ToInt64(item2.Key.Item5.Substring(8, 2), 16);
                                binary = Convert.ToString(pom, 2).PadLeft(5, '0');

                                if (((s_port != 0) && (d_port != 0) && ((client_IP == item2.Key.Item3 && server_IP == item2.Key.Item4) || (client_IP == item2.Key.Item4 && server_IP == item2.Key.Item3)) && (Convert.ToInt32(item2.Key.Item5.Substring(0, 4), 16) == s_port && Convert.ToInt32(item2.Key.Item5.Substring(4, 4), 16) == d_port) || (Convert.ToInt32(item2.Key.Item5.Substring(0, 4), 16) == d_port && Convert.ToInt32(item2.Key.Item5.Substring(4, 4), 16) == s_port)) && binary.Substring(2,1)=="1")
                                {
                                    key = Tuple.Create(comunication_num, "nekompletna");
                                    comunications_help_str.Clear();
                                    comunications_help_str.Append(comunications[key]);
                                    comunications_help_str.Append(item2.Value);
                                    comunications.Remove(key);
                                    comunications[Tuple.Create(comunication_num, "kompletna")] = comunications_help_str.ToString();
                                    framelength = Convert.ToInt32(item2.Key.Item5.Substring(10));
                                    length_counter(framelength);
                                    set_lengths(comunication_num);
                                    s_port = 0;
                                    d_port = 0;
                                    synack = false; ack = false; fin_k1 = false; fin_s = false; fin_k2 = false;
                                    full = true;
                                    comunication_num++;
                                    set_lengths(comunication_num);
                                    
                                    continue;
                                }

                                if (((client_IP == item2.Key.Item3 && server_IP == item2.Key.Item4) || (client_IP == item2.Key.Item4 && server_IP == item2.Key.Item3)) && ((Convert.ToInt32(item2.Key.Item5.Substring(0, 4), 16) == s_port && Convert.ToInt32(item2.Key.Item5.Substring(4, 4), 16) == d_port) || (Convert.ToInt32(item2.Key.Item5.Substring(0, 4), 16) == d_port && Convert.ToInt32(item2.Key.Item5.Substring(4, 4), 16) == s_port)))
                                {
                                    key = Tuple.Create(comunication_num, "nekompletna");
                                    if (!synack && binary.Substring(3, 1) == "1" && binary.Substring(0, 1) == "1")
                                    {
                                        synack = true;
                                        comunications_help_str.Clear();
                                        comunications_help_str.Append(comunications[key]);
                                        comunications_help_str.Append(item2.Value);
                                        comunications[key] = comunications_help_str.ToString();
                                        framelength = Convert.ToInt32(item2.Key.Item5.Substring(10));
                                        length_counter(framelength);

                                    }
                                    else
                                    {
                                        if (synack && !ack && binary.Substring(0, 1) == "1")
                                        {
                                            comunications_help_str.Clear();
                                            comunications_help_str.Append(comunications[key]);
                                            comunications_help_str.Append(item2.Value);
                                            comunications[key] = comunications_help_str.ToString();
                                            ack = true;
                                            framelength = Convert.ToInt32(item2.Key.Item5.Substring(10));
                                            length_counter(framelength);
                                        }
                                        else
                                        {
                                            if (ack && !fin_k1 && binary.Substring(4, 1) == "1" && binary.Substring(0, 1) == "1")
                                            {
                                                comunications_help_str.Clear();
                                                comunications_help_str.Append(comunications[key]);
                                                comunications_help_str.Append(item2.Value);
                                                comunications[key] = comunications_help_str.ToString();
                                                fin_k1 = true;
                                                framelength = Convert.ToInt32(item2.Key.Item5.Substring(10));
                                                length_counter(framelength);
                                                continue;
                                            }
                                            if (fin_k1 && !fin_ack && binary.Substring(0, 1) == "1")
                                            {
                                                comunications_help_str.Clear();
                                                comunications_help_str.Append(comunications[key]);
                                                comunications_help_str.Append(item2.Value);
                                                comunications[key] = comunications_help_str.ToString();
                                                fin_ack = true;
                                                framelength = Convert.ToInt32(item2.Key.Item5.Substring(10));
                                                length_counter(framelength);
                                                continue;
                                            }
                                            if (fin_ack && !fin_s && binary.Substring(4, 1) == "1" && binary.Substring(0, 1) == "1")
                                            {
                                                comunications_help_str.Clear();
                                                comunications_help_str.Append(comunications[key]);
                                                comunications_help_str.Append(item2.Value);
                                                comunications[key] = comunications_help_str.ToString();
                                                fin_s = true;
                                                framelength = Convert.ToInt32(item2.Key.Item5.Substring(10));
                                                length_counter(framelength);
                                            }
                                            else
                                                if (fin_s && !fin_k2 && binary.Substring(0, 1) == "1")
                                                {
                                                    comunications_help_str.Clear();
                                                    comunications_help_str.Append(comunications[key]);
                                                    comunications_help_str.Append(item2.Value);
                                                    comunications.Remove(key);
                                                    comunications[Tuple.Create(comunication_num, "kompletna")] = comunications_help_str.ToString();
                                                    fin_k2 = true;
                                                    framelength = Convert.ToInt32(item2.Key.Item5.Substring(10));
                                                    length_counter(framelength);
                                                    set_lengths(comunication_num);
                                                    s_port = 0;
                                                    d_port = 0;
                                                    synack = false; ack = false; fin_k1 = false; fin_s = false; fin_k2 = false;
                                                    full = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    if (number_of_frames > 19)
                                                    {
                                                        if (sub_number < 10 || sub_number > (number_of_frames - 10))
                                                        {
                                                            comunications_help_str.Clear();
                                                            comunications_help_str.Append(comunications[key]);
                                                            comunications_help_str.Append(item2.Value);
                                                            comunications[key] = comunications_help_str.ToString();
                                                            sub_number++;
                                                            framelength = Convert.ToInt32(item2.Key.Item5.Substring(10));
                                                            length_counter(framelength);
                                                        }
                                                        else
                                                        {
                                                            sub_number++;
                                                            framelength = Convert.ToInt32(item2.Key.Item5.Substring(10));
                                                            length_counter(framelength);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        comunications_help_str.Clear();
                                                        comunications_help_str.Append(comunications[key]);
                                                        comunications_help_str.Append(item2.Value);
                                                        comunications[key] = comunications_help_str.ToString();
                                                        framelength = Convert.ToInt32(item2.Key.Item5.Substring(10));
                                                        length_counter(framelength);
                                                    }
                                                }
                                        }
                                    }
                                }
                            }
                        }
                        comunication_num++;
                        if (!full)
                        {
                            s_port = 0;
                            d_port = 0;
                            synack = false; ack = false; fin_k1 = false; fin_s = false; fin_k2 = false;
                            full = true;
                            set_lengths(comunication_num-1);
                        }
                    }
                    else
                        from++;
                    }
                else
                from++;
            }
        }

         private void length_counter (int framelength)
        {
             foreach(var item in lengths_pom)
             {
                    if ( framelength > item.Key.Item1 && framelength < item.Key.Item2)
                    {
                        var key = Tuple.Create(item.Key.Item1,item.Key.Item2);
                        int l = item.Value;
                        l++;
                        lengths_pom[Tuple.Create(item.Key.Item1, item.Key.Item2)] = l;
                        break;
                    }
             }
        }
         private void inicialize()
         {
             lengths_pom.Add(Tuple.Create(0, 19), 0);
             lengths_pom.Add(Tuple.Create(20, 39), 0);
             lengths_pom.Add(Tuple.Create(40, 79), 0);
             lengths_pom.Add(Tuple.Create(80, 159), 0);
             lengths_pom.Add(Tuple.Create(160, 319), 0);
             lengths_pom.Add(Tuple.Create(320, 639), 0);
             lengths_pom.Add(Tuple.Create(640, 1279), 0);
             lengths_pom.Add(Tuple.Create(1280, 2559), 0);
         }

         private void set_lengths(int comunication_num)
         {
             comunications_help_str.Clear();
             comunications_help_str.Append("Statistika dlzky ramcov v bajtoch: \r\n");
             foreach (var l in lengths_pom)
             {
                 comunications_help_str.Append(l.Key.Item1 + " ..... " + l.Key.Item2 + "     = " + l.Value +"\r\n");
             }
             comunications_help_str.Append("\r\n\r\n");
             lengths.Add(comunication_num--, comunications_help_str.ToString());
             comunications_help_str.Clear();
             lengths_pom.Clear();
             inicialize();
         }



        //----------------------- Getters / Setters -----------------------
        public Dictionary<Tuple<int, string>, string> Comunications
        {
            get { return comunications; }
            set { comunications = value; }
        }

        public Dictionary<Tuple<int, int>, int> Lengths_pom
        {
            get { return lengths_pom; }
            set { lengths_pom = value; }
        }

        public Dictionary<int, string> Lengths
        {
            get { return lengths; }
            set { lengths = value; }
        }


}
}