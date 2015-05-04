using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Diagnostics;







namespace PKS_projekt1
{
    

    public partial class Form1 : Form
    {
       private string file = "";
       Help help;
       ReadFromFile rdF = null;
       
     

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // zistenie adresy suboru
                file = openFileDialog1.FileName;
                label1.Text = "File:   " + file;
                rdF = null;
            }
            else MessageBox.Show("Nevybrali ste ziadny subor");
        }


        

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Autor: Tomas Cicman" + Environment.NewLine + "AIS cislo: 60327" + Environment.NewLine + "Odbor: PKSS 4" + Environment.NewLine + "Rocnik: 2014/2015");
             
        }


        // metoda pre zakazanie otvarania viacerych rovnakych okien
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (help == null)
            {
                help = new Help();
                help.Disposed += new EventHandler(help_set_null);
                help.Show();
                help.MaximizeBox = false;
                help.MinimizeBox = false;
                
            }
            else
            {
                help.Activate();
            }
        }
        private void help_set_null(object sender, EventArgs e)
        {
            help = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch myStopwatch = new Stopwatch();

            if (File.Exists(file))
            {
                myStopwatch.Start();

                if (rdF == null)
                {
                    rdF = new ReadFromFile(file, comboBox1.SelectedIndex);
                    rdF.Append = true;
                }
                richTextBox1.Clear();
                new Print_Text(rdF, comboBox1.SelectedIndex, richTextBox1);
                //append = false;
                myStopwatch.Stop();
                // vypis casu vykonavania programu + casu spracovania suboru
                long elapsedTime = myStopwatch.ElapsedMilliseconds;
                label2.Text = elapsedTime.ToString() + " ns";
                label3.Text = rdF.stopping_time.ToString() + " ns";
                
            }
            else
            {
                MessageBox.Show("Subor neexistuje. \nProsím vyberte subor: Menu->Open file.");
            }
            
        }

        

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit_form exit = new Exit_form();
            if (exit.ShowDialog() == DialogResult.Yes)
            {
                Close();
            }
        }
                   
   

    }
}
