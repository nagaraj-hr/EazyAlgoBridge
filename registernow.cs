using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EazyAlgoBridge
{
    public partial class registernow : Form
    {
        public string registeredto = "";
        public registernow()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("Please enter Name");
            else if (textBox2.Text == "")
                MessageBox.Show("Please enter Email address");
            else if (textBox3.Text == "")
                MessageBox.Show("Please enter serial number");
            else
            {
                string hashstring = textBox2.Text + globalClass.versionNo;
                string h1 = globalClass.Hash(hashstring.ToLower());

                if (string.Equals(textBox3.Text, h1, StringComparison.OrdinalIgnoreCase))
                {
                    globalClass.createRegistryKey("SOFTWARE\\EasyAlgo");
                    globalClass.writeToRegistry("SOFTWARE\\EasyAlgo", "username", textBox1.Text);
                    globalClass.writeToRegistry("SOFTWARE\\EasyAlgo", "email", textBox2.Text);
                    globalClass.writeToRegistry("SOFTWARE\\EasyAlgo", "serial", textBox3.Text);
                    MessageBox.Show("Registration successful");
                    button1.Enabled = false;
                    registeredto = textBox1.Text;
                }
                else
                    MessageBox.Show("Unable to register, please check the values");
            }
        }
    }
}
