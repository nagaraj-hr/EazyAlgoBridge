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
    public partial class Start : Form
    {
        public string folderpath;

        public Start()
        {
            InitializeComponent();

            string user = globalClass.readFromRegistry("SOFTWARE\\EasyAlgo", "username");
            string email = globalClass.readFromRegistry("SOFTWARE\\EasyAlgo", "email");
            string serial = globalClass.readFromRegistry("SOFTWARE\\EasyAlgo", "serial");
            string hashstring = email + globalClass.versionNo;
            string h1 = globalClass.Hash(hashstring.ToLower());

            if (string.Equals(serial, h1, StringComparison.OrdinalIgnoreCase))
            {
                button3.Enabled = false;
                label3.Text = "Registered to: " + user;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            registernow frmregister;
            frmregister = new registernow();
            DialogResult dr = frmregister.ShowDialog(this);
            if (frmregister.registeredto == "")
                return;
            else
            {
                button3.Enabled = false;
                label3.Text = "Registered to: " + frmregister.registeredto;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Start processing
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //close without start processing
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
