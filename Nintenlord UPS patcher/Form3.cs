using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Nintenlord.Hacking.Core;
using System.Threading.Tasks;
using System.Linq;

namespace Nintenlord.UPSpatcher
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            textBox2.Text = firstLine;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Select a patch";
            open.Filter = "UPS files|*.ups";
            open.Multiselect = false;
            open.ShowDialog();
            if (open.FileNames.Length > 0)
            {
                textBox1.Text = open.FileName;
            }
        }

        string firstLine = "UPS Patch details:";

        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show("Patch doesn't exist.");
                return;
            }

            textBox2.Text = "loading";

            UPSfile UPSFile = new UPSfile(textBox1.Text);
            int[,] details = UPSFile.GetData();

            // get required amount for padding
            var largestValue = details.Cast<int>().Max();
            var numberOfDigits = (int)Math.Max(1, Math.Floor(Math.Log10(largestValue)));

            List<string> lines = new List<string>(details.Length + 1);
            lines.Add(firstLine);
            //var lenTxt = "Lenghts";
            lines.Add("Offsets" + "\t" + "Lenghts");

            // make format string for the lines
            var linefmt = "{0:X" + numberOfDigits + "}\t" + "{1}";

            for (int i = 0; i < details.GetLength(0); i++)
            {
                var line = string.Format(linefmt, details[i, 0], details[i, 1]);
                lines.Add(line);
            }

            textBox2.Text += "...";

            textBox2.Lines = lines.ToArray();
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
