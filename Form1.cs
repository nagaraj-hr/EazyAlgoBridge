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

namespace EazyAlgoBridge
{

    public partial class Form1 : Form
    {
        Form2 frm2;
        Start frm3;
        bool firsttime = true;
        EzRegistry objrej;
        //KK kiteConnect1;
        private StringBuilder m_Sb;
        //private bool m_bDirty;
        private System.IO.FileSystemWatcher m_Watcher;
        private bool m_bIsWatching;
        public string OrderFilePath;
        public string sKey;
        public string sSec;
        public string sGlobalOrderType;
        public KK kiteConnect1;
        // private int orderLineNumber;
        Dictionary<string, int> csvFileList; // = new Dictionary<string, int>();
        enum ezOrderTypes {CO, BO, MIS,NRML};
        public Form1()
        {
            InitializeComponent();
            Settings.Image = global::EazyAlgoBridge.Properties.Resources.settings; //Image.FromFile("C:\\Users\\Nagaraj\\Downloads\\settings.png");
            // Align the image and text on the button.
            Settings.ImageAlign = ContentAlignment.MiddleLeft;
            Settings.TextAlign = ContentAlignment.MiddleCenter;

            Start.Image = global::EazyAlgoBridge.Properties.Resources.right; //Image.FromFile("C:\\Users\\Nagaraj\\Downloads\\right.png");
            // Align the image and text on the button.
            Start.ImageAlign = ContentAlignment.MiddleLeft;
            Start.TextAlign = ContentAlignment.MiddleCenter;

            Exit.Image = global::EazyAlgoBridge.Properties.Resources.exit; //Image.FromFile("C:\\Users\\Nagaraj\\Downloads\\exit.png");
            // Align the image and text on the button.
            Exit.ImageAlign = ContentAlignment.MiddleLeft;
            Exit.TextAlign = ContentAlignment.MiddleCenter;

            About.Image = global::EazyAlgoBridge.Properties.Resources.navigation; //Image.FromFile("C:\\Users\\Nagaraj\\Downloads\\exit.png");
            // Align the image and text on the button.
            About.ImageAlign = ContentAlignment.MiddleLeft;
            About.TextAlign = ContentAlignment.MiddleCenter;

            Start.Enabled = false; //default set to false
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            m_Sb = new StringBuilder();
            //m_bDirty = false;
            m_bIsWatching = false;
            //orderLineNumber = 0;
            OrderFilePath = "";
            sGlobalOrderType = "Default";
            //
            //initialize kite
            //
            kiteConnect1 = new KK();
            csvFileList = new Dictionary<string, int>();
            globalClass.writetoLogFile("Started application");
            
        }
        private void ReadFromFile(string fileToRead)
        {
            int counter = 0;
            string line;
            int orderLineNumber = 0;
            //string sExchange; string sSymbol; string sTransactionType; int iQuantity; decimal dPrice; decimal TriggerPrice; string sOrderType; decimal sStoplossValue; decimal sSquareOffValue; decimal TrailSL;
            if (csvFileList.ContainsKey(fileToRead))
                orderLineNumber = csvFileList[fileToRead];

            globalClass.writetoLogFile("processing file: " + fileToRead);
            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(fileToRead);
            while ((line = file.ReadLine()) != null)
            {

                counter++;
                if (counter > orderLineNumber)
                {
                    //process the line read
                    //
                    string[] words = line.Split('$');
                    string sExchange=""; string sSymbol=""; string sTransactionType=""; int iQuantity=0; decimal dPrice=0; decimal TriggerPrice=0; string sOrderType=""; decimal sStoplossValue=0; decimal sSquareOffValue=0; decimal TrailSL=0;
                    int ezOrderType = 0;
                    globalClass.writetoLogFile("Processing line: " +line);
                    for (int i=0; i < words.Count(); i++)
                    {
                        //(Exchange, Symbol, BuyOrSell, Qty,  OrderType,Price, TriggerPrice, Stop, Target, TrailSL)
                        //Place_MIS_Order1(string sExchange, string sSymbol, string sTransactionType, int iQuantity, decimal dPrice, string sProduct, string sOrderType)
                        //Place_BO_Order1(string sExchange, string sSymbol, string sTransactionType, int iQuantity, decimal dPrice, string sProduct, string sOrderType, decimal sStoplossValue, decimal sSquareOffValue)
                        //Place_CO_Order1(string sExchange, string sSymbol, string sTransactionType, int iQuantity, decimal dPrice, string sProduct, string sOrderType, decimal sStoplossValue)
                        decimal decValue = 0;
                        switch (i)
                        {
                            case 0:
                                if (string.Equals(words[0], "EZ_ORDERTYPE_CO", StringComparison.OrdinalIgnoreCase))
                                {
                                    ezOrderType = (int) ezOrderTypes.CO;
                                }
                                else if (string.Equals(words[0], "EZ_ORDERTYPE_BO", StringComparison.OrdinalIgnoreCase))
                                {
                                    ezOrderType = (int) ezOrderTypes.BO;
                                }
                                else if (string.Equals(words[0], "EZ_ORDERTYPE_MIS", StringComparison.OrdinalIgnoreCase))
                                {
                                    ezOrderType = (int) ezOrderTypes.MIS;
                                }
                                else
                                    ezOrderType = (int) ezOrderTypes.NRML;
                                break;
                            case 1:
                                sExchange = words[i];
                                break;
                            case 2:
                                sSymbol = words[i];
                                break;
                            case 3:
                                sTransactionType = words[i];
                                break;
                            case 4:
                                iQuantity = Int32.Parse(words[i]);
                                break;
                            case 5:
                                sOrderType = words[i];
                                break;
                            case 6:
                                decValue = Math.Round(Decimal.Parse(words[i]), 2);
                                dPrice = globalClass.getRound5Value(decValue);
                                break;
                            case 7:
                                decValue = Math.Round(Decimal.Parse(words[i]), 2);
                                TriggerPrice = globalClass.getRound5Value(decValue);
                                break;
        
                            case 8:
                                decValue = Math.Round(Decimal.Parse(words[i]), 2);
                                sStoplossValue = globalClass.getRound5Value(decValue);
                                break;
                            case 9:
                                decValue = Math.Round(Decimal.Parse(words[i]), 2);
                                sSquareOffValue = globalClass.getRound5Value(decValue);
                                break;
                            case 10:
                                decValue = Math.Round(Decimal.Parse(words[i]), 2);
                                TrailSL = globalClass.getRound5Value(decValue);
                                break;
                            default:
                                // code block
                                break;
                        }
       
                    }
                    if (ezOrderType == (int)ezOrderTypes.BO)
                    {
                        if (string.Equals(sGlobalOrderType, "Default", StringComparison.OrdinalIgnoreCase))
                        {
                            globalClass.writetoLogFile("Placing BO Order");
                            kiteConnect1.Place_BO_Order1(sExchange, sSymbol, sTransactionType, iQuantity, dPrice, "MIS", sOrderType, sStoplossValue, sSquareOffValue);
                        }
                        else if (string.Equals(sGlobalOrderType, "CO", StringComparison.OrdinalIgnoreCase))
                        {
                            globalClass.writetoLogFile("Placing CO Order");
                            kiteConnect1.Place_CO_Order1(sExchange, sSymbol, sTransactionType, iQuantity, dPrice, "MIS", sOrderType, TriggerPrice);

                        }
                        else if (string.Equals(sGlobalOrderType, "MIS", StringComparison.OrdinalIgnoreCase))
                        {
                            globalClass.writetoLogFile("Placing MIS Order");
                            kiteConnect1.Place_MIS_Order1(sExchange, sSymbol, sTransactionType, iQuantity, dPrice, "MIS", sOrderType);
                        }
                        else
                        {
                            globalClass.writetoLogFile("Placing NRML Order");
                            kiteConnect1.Place_NRML_Order1(sExchange, sSymbol, sTransactionType, iQuantity, dPrice, "NRML", sOrderType);
                        }
                    }
                    else if (ezOrderType == (int)ezOrderTypes.CO)
                    {
                        if (string.Equals(sGlobalOrderType, "Default", StringComparison.OrdinalIgnoreCase) || string.Equals(sGlobalOrderType, "CO", StringComparison.OrdinalIgnoreCase))
                        {
                            globalClass.writetoLogFile("Placing CO Order");
                            kiteConnect1.Place_CO_Order1(sExchange, sSymbol, sTransactionType, iQuantity, dPrice, "MIS", sOrderType, TriggerPrice);
                        }
                        else if (string.Equals(sGlobalOrderType, "MIS", StringComparison.OrdinalIgnoreCase))
                        {
                            globalClass.writetoLogFile("Placing MIS Order");
                            kiteConnect1.Place_MIS_Order1(sExchange, sSymbol, sTransactionType, iQuantity, dPrice, "MIS", sOrderType);
                        }
                        else
                        {
                            globalClass.writetoLogFile("Placing NRML Order");
                            kiteConnect1.Place_NRML_Order1(sExchange, sSymbol, sTransactionType, iQuantity, dPrice, "NRML", sOrderType);
                        }
                    }
                    else if (ezOrderType == (int)ezOrderTypes.MIS)
                    {
                        if (string.Equals(sGlobalOrderType, "Default", StringComparison.OrdinalIgnoreCase) || string.Equals(sGlobalOrderType, "MIS", StringComparison.OrdinalIgnoreCase) || string.Equals(sGlobalOrderType, "CO", StringComparison.OrdinalIgnoreCase))
                        {
                            globalClass.writetoLogFile("Placing MIS Order");
                            kiteConnect1.Place_MIS_Order1(sExchange, sSymbol, sTransactionType, iQuantity, dPrice, "MIS", sOrderType);
                        }
                        else
                        {
                            globalClass.writetoLogFile("Placing NRML Order");
                            kiteConnect1.Place_NRML_Order1(sExchange, sSymbol, sTransactionType, iQuantity, dPrice, "NRML", sOrderType);
                        }
                    }
                    else
                    {
                        globalClass.writetoLogFile("Placing NRML Order");
                        kiteConnect1.Place_NRML_Order1(sExchange, sSymbol, sTransactionType, iQuantity, dPrice, "NRML", sOrderType);
                    }
                    globalClass.writetoLogFile("Completed processing Order.");
                    this.Invoke((MethodInvoker)delegate
                    {
                        StatusListBox.BeginUpdate();
                        StatusListBox.Items.Add(line);
                        StatusListBox.EndUpdate();
                        dataGridView1.Rows.Add(DateTime.Now, sExchange, sSymbol, sOrderType, iQuantity.ToString(), dPrice.ToString(), "Success");
                        //frm2.kiteConnect1.Place_CO_Order1("NFO", "NiftyFUT", "BUY", 75, 10000, "MIS", "LIMIT", 9950);
                    });

                    orderLineNumber++;

                }
            }
            csvFileList[fileToRead] = orderLineNumber;
            file.Close();
            globalClass.writetoLogFile("Completed processing file.");
        }

        private void StartFileWatcher(string folderPath)
        {
            if (m_bIsWatching)
            {
                m_bIsWatching = false;
                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();
                Start.BackColor = Color.Aqua;
                Start.Text = "Start";

            }
            else
            {
                m_bIsWatching = true;
                Start.BackColor = Color.LightSkyBlue;
                Start.Text = "Stop";
                OrderFilePath = folderPath;

                DeleteCSVFilesInFolder(folderPath);
                m_Watcher = new System.IO.FileSystemWatcher();

                {
                    m_Watcher.Path = OrderFilePath + "\\";
                    m_Watcher.Filter = "*.csv"; //OrderFilePath + "\\Orders.csv"; //OrderFilePath.Substring(OrderFilePath.LastIndexOf('\\') + 1);
                   // m_Watcher.Path = OrderFilePath.Substring(0, OrderFilePath.Length - m_Watcher.Filter.Length);
                }

                {
                    m_Watcher.IncludeSubdirectories = false;
                }

                m_Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                m_Watcher.Changed += new FileSystemEventHandler(OnChanged);
                m_Watcher.Created += new FileSystemEventHandler(OnChanged);
                // m_Watcher.Deleted += new FileSystemEventHandler(OnChanged);
                // m_Watcher.Renamed += new RenamedEventHandler(OnRenamed);
                m_Watcher.EnableRaisingEvents = true;
                //new FileSystemWatcher.KiteConnect1();
                this.Invoke((MethodInvoker)delegate
                {
                    StatusListBox.BeginUpdate();
                    StatusListBox.Items.Add("Started watching for orders");
                    globalClass.writetoLogFile("Started file monitoring...");
                    StatusListBox.EndUpdate();
                });
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            //if (!m_bDirty)
            {
                m_Sb.Remove(0, m_Sb.Length);
                m_Sb.Append(e.FullPath);
                m_Sb.Append(" ");
                m_Sb.Append(e.ChangeType.ToString());
                m_Sb.Append("    ");
                m_Sb.Append(DateTime.Now.ToString());
                //m_bDirty = true;
                // MessageBox.Show(m_Sb.ToString());
                this.Invoke((MethodInvoker)delegate
                {
                    StatusListBox.BeginUpdate();
                    StatusListBox.Items.Add(m_Sb.ToString());
                    StatusListBox.EndUpdate();
                    this.dataGridView1.ScrollBars = ScrollBars.None;
                });


                ReadFromFile(e.FullPath);


                this.Invoke((MethodInvoker)delegate
                {
                    //DataGridview Refreshment
                    dataGridView1.Enabled = true;
                    dataGridView1.ScrollBars = ScrollBars.Both;
                });
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (m_bIsWatching)
            {
                m_bIsWatching = false;
                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();
                Start.BackColor = Color.LightGray;
                Start.Text = "Start";
            }
            else
            {
                if (kiteConnect1.MyAccessToken == "")
                {
                    MessageBox.Show("Please strat the Kite session first");
                    return;
                }
                StartFileWatcher(OrderFilePath);
            }
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            objrej = new EzRegistry();
            objrej.createRegistryKey("SOFTWARE\\EasyAlgo");

            this.sKey = objrej.readFromRegistry("SOFTWARE\\EasyAlgo", "apikey");
            this.sSec = objrej.readFromRegistry("SOFTWARE\\EasyAlgo", "apisec");
            this.OrderFilePath= objrej.readFromRegistry("SOFTWARE\\EasyAlgo", "orderfilepath");
            this.sGlobalOrderType = objrej.readFromRegistry("SOFTWARE\\EasyAlgo", "ordertype");
            if (sGlobalOrderType == "")
                sGlobalOrderType = "Default";
            frm2 = new Form2(this.sKey, this.sSec, this.OrderFilePath, sGlobalOrderType, kiteConnect1);
            //frm2.kiteConnect1 = kiteConnect1;
            DialogResult dr = frm2.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                frm2.Close();
            }
            else if (dr == DialogResult.OK)
            {

                OrderFilePath = frm2.folderpath;
                sKey = frm2.sAPIkey;
                sSec = frm2.sAPIsec;
                sGlobalOrderType = frm2.sOrderType;
                objrej.writeToRegistry("SOFTWARE\\EasyAlgo", "apikey", sKey);
                objrej.writeToRegistry("SOFTWARE\\EasyAlgo", "apisec", sSec);
                objrej.writeToRegistry("SOFTWARE\\EasyAlgo", "orderfilepath", OrderFilePath);
                objrej.writeToRegistry("SOFTWARE\\EasyAlgo", "ordertype", sGlobalOrderType);
                if (kiteConnect1.MyAccessToken != "" && firsttime)
                {
                    Start.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    firsttime = false;
                    this.Invoke((MethodInvoker)delegate
                    {
                        string sMsg = "KiteConnect session started for :" + kiteConnect1.MyUser.UserName + " ID: " + kiteConnect1.MyUser.UserId;
                        StatusListBox.BeginUpdate();
                        StatusListBox.Items.Add(sMsg);
                        StatusListBox.EndUpdate();
                    });
                }
                frm2.Close();
            }
        }

        private void OrderListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
               MessageBox.Show("Clicked");
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dataGridView1.ScrollBars = ScrollBars.Both;
            //MessageBox.Show("Message");
        }
        private void DeleteCSVFilesInFolder (string folderPath)
        {
            string[] fileNames = Directory.GetFiles(folderPath);
            foreach (string fileName in fileNames)
            {
                try
                {
                    string ext = Path.GetExtension(fileName);
                    if (string.Equals(ext, ".csv", StringComparison.OrdinalIgnoreCase))
                        File.Delete(fileName);
                }
                catch (Exception e)
                {
                    globalClass.writetoLogFile("Exception deleting file: " + e.Message);
                }
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            frm3 = new Start();
            DialogResult dr = frm3.ShowDialog(this);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StatusListBox.Items.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //positions
            if (kiteConnect1 == null || kiteConnect1.MyAccessToken == "")
            {
                MessageBox.Show("Please strat the Kite session first");
                return;
            }
            //
            //kiteConnect1.GetPostitions();
            Form3 position1;
            position1 = new Form3(kiteConnect1);
            position1.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //margins
            if (kiteConnect1 == null || kiteConnect1.MyAccessToken == "")
            {
                MessageBox.Show("Please strat the Kite session first");
                return;
            }
            kiteConnect1.GetEquityMargins();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //orders
            if (kiteConnect1 == null || kiteConnect1.MyAccessToken == "")
            {
                MessageBox.Show("Please strat the Kite session first");
                return;
            }
            orders order1;
            order1 = new orders(kiteConnect1);
            order1.ShowDialog();

            //kiteConnect1.GetOrders();
        }
    }
}
