using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS_projekt1
{
    class Doimplementacia
    {
        private int number_of_ftpdata = 0;
        private StringBuilder ftp_data_frames = new StringBuilder();


        public Doimplementacia(string selected, ReadFromFile rdF)
        {
            foreach (var item in rdF.DictionaryFrames)
            {
                if (item.Key.Item2 == selected)
                {
                    number_of_ftpdata++;
                    ftp_data_frames.Append(item.Value);
                }
            }

        }


        public StringBuilder Ftp_data_frames
        {
            get { return ftp_data_frames; }
            set { ftp_data_frames = value; }
        }
        
        public int Number_of_ftpdata
        {
            get { return number_of_ftpdata; }
            set { number_of_ftpdata = value; }
        }
    }
}
