using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Windows.Controls;

namespace Clash_Management_System_Navisworks_Addin.DB
{
    public class AddressHandler
    {
        public static string GetConfigAddress()
        {
            try
            {
                string assemblyAdrs = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string ConfigAddress = string.Empty;

                string ConfigFilePath = assemblyAdrs + ".config";

                XmlDocument document = new XmlDocument();
                document.Load(ConfigFilePath);

                XmlNodeList elemList = document.GetElementsByTagName("endpoint");
                for (int i = 0; i < elemList.Count; i++)
                {
                    ConfigAddress = elemList[i].Attributes["address"].Value;
                    if (ConfigAddress != null && ConfigAddress != string.Empty)
                    {
                        break;
                    }
                }
                return ConfigAddress;
            }
            catch (Exception e)
            {
                string reportContent = "Method Name: Read Confing file: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
            }
            return null;


        }

    }
}
