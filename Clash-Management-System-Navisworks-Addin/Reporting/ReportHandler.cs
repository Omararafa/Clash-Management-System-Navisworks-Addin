using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Clash_Management_System_Navisworks_Addin.ViewModels;

namespace Clash_Management_System_Navisworks_Addin.Reporting
{
    public static class ReportHandler
    {
        #region Static Members
        private static string _path;

        public static string Path
        {
            get { return _path; }
            set { _path = value; }
        }


        #endregion

        #region Reporting Methods


        public static bool WriteReport(List<AClashTest> aClashTests)
        {
            string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            //TODO Deploy: change report folder directory
            string reportFolder = appFolder + @"\Navisworks Reports";

            if (!Directory.Exists(reportFolder))
            {
                Directory.CreateDirectory(reportFolder);
            }

            //TODO Deploy: change file naming convention
            string reportPath = reportFolder + @"\Report-" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".csv";
            List<string> metaData = GetMetaData();
            string reportHeader = AClashTest.GetReportHeaders();
            List<string> reportData = GetClashTestReportData(aClashTests);
            List<string> failedClashTestsData = GetFailedCreatedClashTestsData(DB.DBHandler.DBFailedClashTests);

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(reportPath, false))
                {
                    foreach (string line in metaData)
                    {
                        streamWriter.WriteLine(line);
                    }

                    streamWriter.WriteLine(reportHeader);

                    foreach (string line in reportData)
                    {
                        streamWriter.WriteLine(line);
                    }

                    foreach (string line in failedClashTestsData)
                    {
                        streamWriter.WriteLine(line);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured, please retry!" + Environment.NewLine + ex.Message);
                return false;
            }
        }

        private static List<string> GetClashTestReportData(List<AClashTest> AClashTests)
        {
            List<string> data = new List<string>();

            foreach (AClashTest aClashTest in AClashTests)
            {
                data.Add(aClashTest.Name + "," + aClashTest.Condition.ToString() + "," +
                         aClashTest.SearchSet1.Name + "," + aClashTest.SearchSet2.Name);
            }

            return data;
        }

        private static List<string> GetFailedCreatedClashTestsData(List<WebService.ClashTest> ClashTests)
        {
            List<string> data = new List<string>();

            data.Add(string.Empty);
            data.Add("The following clash tests had not been created as their search sets is missing in the navisworks document");
            data.Add(string.Empty);

            foreach (WebService.ClashTest ClashTest in ClashTests)
            {
                string searchSet1Name = ClashTest.SearchSet1.TradeAbbr + "-" + ClashTest.SearchSet1.Name;
                string searchSet2Name = ClashTest.SearchSet2.TradeAbbr + "-" + ClashTest.SearchSet2.Name;

                data.Add(ClashTest.Name + "," + "NOT CREATED" + "," +
                         searchSet1Name + "," + searchSet2Name);
            }

            return data;
        }

        private static List<string> GetMetaData()
        {
            List<string> metaData = new List<string>();

            metaData.Add(string.Format("User Domain:,{0}", Views.ViewsHandler.CurrentUser.Domain));
            metaData.Add(string.Format("Username:,{0}", Views.ViewsHandler.CurrentUser.Name));
            metaData.Add(string.Format("Project:,{0}", Views.ViewsHandler.CurrentProject.Name));
            metaData.Add(string.Format("Clash Matrix:,{0}", Views.ViewsHandler.CurrentAClashMatrix.Name));
            metaData.Add(string.Format("Time:,{0}", DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")));

            metaData.Add(string.Empty);
            metaData.Add("Sync Clash Tests Report");
            metaData.Add(string.Empty);

            return metaData;
        }

        #endregion
    }
}
