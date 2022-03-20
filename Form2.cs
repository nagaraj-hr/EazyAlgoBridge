using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KiteConnect;
using System.Web;
using System.Net;
using System.IO;

namespace EazyAlgoBridge
{

    public partial class Form2 : Form
    {
        public string folderpath;
        public string sAPIkey;
        public string sAPIsec;
        public string sURL;
        public string sKeyURL;
        public string sOrderType;
        public KK kiteConnect1;
        int iSidFOund;
        
        //public KK kiteConnect1;
        public Form2()
        {
            InitializeComponent();
            iSidFOund = 0;
            sURL = "C:\\Temp\\success.html";
        }
        public Form2(string skey, string ssec, string sfolderpath, string sorder, KK kiteconnect1)
        {
            InitializeComponent();
            iSidFOund = 0;

            sURL = "C:\\Temp\\success.html";
            textBox1.Text = skey;
            textBox2.Text = ssec;
            textBox3.Text = sfolderpath;
            kiteConnect1 = kiteconnect1;
            if (kiteConnect1 == null)
                kiteConnect1 = new KK();
            if (kiteConnect1.MyAccessToken == "")
                button1.Enabled = true;
            else
            {
                button1.Enabled = false;
                webBrowser1.Navigate(sURL);
            }
            comboBox1.Text = sorder;
            this.FormClosing += new FormClosingEventHandler(Form2_FormClosing);
            radioButton1.Checked = false;
            radioButton2.Checked = true;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("Please enter value for API Key");
            else if (textBox2.Text == "")
                MessageBox.Show("Please enter value for API Secret");
            else if (textBox3.Text == "")
                MessageBox.Show("Please Select a folder to process orders");
            else if (!Directory.Exists(textBox3.Text))
            {
                MessageBox.Show("Please select a valid folder");
            }
            else
            {
                //kiteConnect1 = new KK();
                string kurl = kiteConnect1.InitializeKite(textBox1.Text, textBox2.Text);

                // Collect login url to authenticate user. Load this URL in browser or WebView. 
                // After successful authentication this will redirect to your redirect url with request token.
                //string kurl = kite.GetLoginURL();

                //System.Diagnostics.Process.Start(kurl);
                if (kurl == "")
                    MessageBox.Show("Unable to connect to Kite, please recheck the API Key");
                else if (radioButton1.Checked)
                {
                    webBrowser1.Navigate(kurl);
                }
                else if (radioButton2.Checked)
                {
                    button5_Click(sender, e);
                }
                // Collect tokens and user details using the request token
                // User user = kite.GenerateSession(RequestToken, MySecret);

                // Persist these tokens in database or settings
                //string MyAccessToken = user.AccessToken;
                //string MyPublicToken = user.PublicToken;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Order File Path";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string sSelectedPath = fbd.SelectedPath;
                //MessageBox.Show(sSelectedPath);
                textBox3.Text = sSelectedPath;
                folderpath = textBox3.Text;
            }


        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //String access_token = HttpUtility.ParseQueryString(e.Url).Get("access_token");
            //String status = HttpUtility.ParseQueryString(e.Url).Get("status");
            String[] nameValue;
            bool successFound = false;
            bool loginFound = false;
            bool requesttokenFound = false;
            sKeyURL = e.Url.ToString();
            char[] separatingStrings = { '?', '&' };

            if (sKeyURL.Contains("127.0.0.1"))
            {
                string[] words = sKeyURL.Split(separatingStrings);
                globalClass.writetoLogFile("DocumentCOmpleted call: KeyUrl:" + sKeyURL);
                foreach (var word in words)
                {
                    // MessageBox.Show(word);
                    nameValue = word.Split('=');
                    if (string.Equals(nameValue[0], "request_token", StringComparison.OrdinalIgnoreCase))
                    {
                        kiteConnect1.MyRequestToken = nameValue[1];
                        requesttokenFound = true;
                    }
                    if (string.Equals(nameValue[0], "action", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.Equals(nameValue[1], "login", StringComparison.OrdinalIgnoreCase))
                            loginFound = true;
                    }
                    if (string.Equals(nameValue[0], "status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.Equals(nameValue[1], "success", StringComparison.OrdinalIgnoreCase))
                            successFound = true;
                    }

                }
                if (successFound && loginFound && requesttokenFound)
                {
                    //this.webBrowser1.Navigate(sURL);
                    globalClass.writetoLogFile("RequestToken found: " + kiteConnect1.MyRequestToken);
                    if (kiteConnect1.StartKiteSession(kiteConnect1.MyRequestToken, textBox2.Text) == 1)
                    {
                        string htmlText = "<HTML><TITLE> Success </TITLE><BODY>Successfully connected<BR>" + "User: " + kiteConnect1.MyUser.UserName + "<BR>" + "User ID: " + kiteConnect1.MyUser.UserId + "<BR>" + " </BODY></HTML>";
                        System.IO.File.WriteAllText(sURL, htmlText);
                        this.webBrowser1.Navigate(sURL);
                        this.button1.Enabled = false;
                    }


                }
            }
            else if (sKeyURL.Contains("sess_id=") && iSidFOund == 0)
            {
                //this.webBrowser1.Navigate(sKeyURL);
                iSidFOund = 1;
            }

        }


        private void button4_Click(object sender, EventArgs e)
        {

            folderpath = textBox3.Text;
            sAPIkey = textBox1.Text ;
            sAPIsec = textBox2.Text;
            sOrderType = comboBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void Form2_FormClosing(Object sender, FormClosingEventArgs e)
        {
            folderpath = textBox3.Text;
            sAPIkey = textBox1.Text;
            sAPIsec = textBox2.Text;
            sOrderType = comboBox1.Text;
            this.DialogResult = DialogResult.OK;
            //this.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("Please enter value for API Key");
            else if (textBox2.Text == "")
                MessageBox.Show("Please enter value for API Secret");
            else if (textBox3.Text == "")
                MessageBox.Show("Please Select a folder to process orders");
            else if (!Directory.Exists(textBox3.Text))
            {
                MessageBox.Show("Please select a valid folder");
            }
            else
            {
                //kiteConnect1 = new KK();
                string kurl = kiteConnect1.InitializeKite(textBox1.Text, textBox2.Text);

                // Collect login url to authenticate user. Load this URL in browser or WebView. 
                // After successful authentication this will redirect to your redirect url with request token.
                //string kurl = kite.GetLoginURL();

                //System.Diagnostics.Process.Start(kurl);
                if (kurl == "")
                    MessageBox.Show("Unable to connect to Kite, please recheck the API Key");
                else
                    System.Diagnostics.Process.Start(kurl);
                //webBrowser1.Navigate(kurl);
                // Collect tokens and user details using the request token
                // User user = kite.GenerateSession(RequestToken, MySecret);

                // Persist these tokens in database or settings
                //string MyAccessToken = user.AccessToken;
                //string MyPublicToken = user.PublicToken;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            String[] nameValue;
            bool successFound = false;
            bool loginFound = false;
            bool requesttokenFound = false;
            sKeyURL = textBox4.Text;
            char[] separatingStrings = { '?', '&' };

            if (sKeyURL == "")
                MessageBox.Show("Please enter KiteConnect Request token url.");

            else if (sKeyURL.Contains("127.0.0.1"))
            {
                string[] words = sKeyURL.Split(separatingStrings);
                globalClass.writetoLogFile("DocumentCompleted call: KeyUrl:" + sKeyURL);
                foreach (var word in words)
                {
                    // MessageBox.Show(word);
                    nameValue = word.Split('=');
                    if (string.Equals(nameValue[0], "request_token", StringComparison.OrdinalIgnoreCase))
                    {
                        kiteConnect1.MyRequestToken = nameValue[1];
                        requesttokenFound = true;
                    }
                    if (string.Equals(nameValue[0], "action", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.Equals(nameValue[1], "login", StringComparison.OrdinalIgnoreCase))
                            loginFound = true;
                    }
                    if (string.Equals(nameValue[0], "status", StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.Equals(nameValue[1], "success", StringComparison.OrdinalIgnoreCase))
                            successFound = true;
                    }

                }
                if (successFound && loginFound && requesttokenFound)
                {
                    //this.webBrowser1.Navigate(sURL);
                    globalClass.writetoLogFile("RequestToken found: " + kiteConnect1.MyRequestToken);
                    if (kiteConnect1.StartKiteSession(kiteConnect1.MyRequestToken, textBox2.Text) == 1)
                    {
                        string htmlText = "<HTML><TITLE> Success </TITLE><BODY>Successfully connected<BR>" + "User: " + kiteConnect1.MyUser.UserName + "<BR>" + "User ID: " + kiteConnect1.MyUser.UserId + "<BR>" + " </BODY></HTML>";
                        System.IO.File.WriteAllText(sURL, htmlText);
                        this.webBrowser1.Navigate(sURL);
                        this.button1.Enabled = false;
                    }


                }
                else
                {
                    MessageBox.Show("KiteConnect url is not valid.");
                }
            }
            else
            {
                MessageBox.Show("KiteConnect url is not valid.");
            }

            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button3.Enabled = false;
            textBox4.Enabled = false;
            label5.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button3.Enabled = true;
            textBox4.Enabled = true;
            label5.Enabled = true;
        }
    }
}
