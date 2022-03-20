using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace EazyAlgoBridge
{
    public static class globalClass
    {
        public static string logfilePath;
        public static string versionNo = "10";
        public static void writetoLogFile(string logstring)
        {
            string logFileName;
            string linetoWrite;
            logfilePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            logFileName = logfilePath + "\\Ezlog.txt";
            DateTime aDate = DateTime.Now;
            linetoWrite = aDate.ToString("dd/MM/yyyy HH:mm:ss") + " " + logstring;
            if (File.Exists(logFileName))
            {
                using (StreamWriter sw = File.AppendText(logFileName))
                {
                    sw.WriteLine(linetoWrite);
                }
            }
            else
            //File.WriteAllText(logFileName, logstring);
            {
                using (StreamWriter sw = File.CreateText(logFileName))
                {
                    sw.WriteLine(linetoWrite);
                }
            }

        }



        public static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString().Substring(0, 16);
            }
        }
        public static int writeToRegistry(string regKey, string name, string value)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(regKey);

            if (key != null)
            {
                //storing the values  
                key.SetValue(name, value);

                key.Close();
            }
            return 1;
        }

        public static string readFromRegistry(string regKey, string name)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(regKey);
            string readVal = "";
            //if it does exist, retrieve the stored values  
            if (key != null)
            {
                object obj = key.GetValue(name);
                if (obj != null)
                {
                    readVal = obj.ToString();
                }

                key.Close();

            }
            return readVal;
        }

        public static bool createRegistryKey(string regKey)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(regKey);

            if (key != null)
            {

                key.Close();
                return true;

            }
            return false;
        }
        public static decimal getRound5Value (decimal dVal)
        {
            decimal dtempVal = Math.Round((dVal * 100) / 5);
            return ((dtempVal / 100) * 5);
        }
    }
}
