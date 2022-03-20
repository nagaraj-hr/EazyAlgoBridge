using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace EazyAlgoBridge
{
    public partial class EzRegistry
    {
        public int writeToRegistry(string regKey, string name, string value)
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

        public string readFromRegistry(string regKey, string name)
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

        public bool createRegistryKey(string regKey)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(regKey);
 
            if (key != null)
            {

                key.Close();
                return true;

            }
            return false;
        }
    }
}
