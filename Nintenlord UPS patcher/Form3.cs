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
        const string firstLine = "UPS Patch details:";


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

        private async void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text))
            {
                MessageBox.Show("Patch doesn't exist.");
                return;
            }

            textBox2.Text = "loading...";
            toggleBtns(false);

            // these actions take a long time and would block the UI thread
            UPSfile UPSFile = new UPSfile(textBox1.Text);
            int[,] details = UPSFile.GetData();
            var largestValue = details.Cast<int>().Max();

            // get required amount for padding
            var numberOfDigits = (int)Math.Max(1, Math.Floor(Math.Log10(largestValue)));
            var spacing = new string(' ', 3);

            List<string> lines = new List<string>(details.Length + 1);
            lines.Add(firstLine);
            lines.Add("Offsets" + spacing + "Lenghts");

            // make format string for the lines
            var linefmt = "{0:X" + numberOfDigits + "}" + spacing + "{1}";

            for (int i = 0; i < details.GetLength(0); i++)
            {
                var line = string.Format(linefmt, details[i, 0], details[i, 1]);
                lines.Add(line);
            }

            textBox2.Lines = lines.ToArray();
            toggleBtns(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        void toggleBtns(bool b)
        {
            foreach (var btn in Controls.OfType<Button>())
            {
                btn.Enabled = b;
            }
        }
    }
}
