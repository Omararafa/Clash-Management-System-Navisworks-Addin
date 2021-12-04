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
        public static string Path { get; set; }



        #endregion

        #region Reporting Methods
        public static bool WriteReport(List<AClashTest> aClashTests)
        {
            string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            //TODO Deploy: change file naming convention
            List<string> metaData = GetMetaData("Sync Clash Tests Report");
            string reportHeader = AClashTest.GetReportHeaders();
            List<string> reportData = GetClashTestReportData(aClashTests);
            List<string> failedClashTestsData = GetFailedCreatedClashTestsData(DB.DBHandler.DBFailedClashTests);

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(Path, false))
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

        public static bool WriteExceptionLog(string title, string reportContent)
        {
            string appFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            //TODO Deploy: change report folder directory
            string reportFolder = appFolder + @"\Navisworks Reports\Exception";

            if (!Directory.Exists(reportFolder))
            {
                Directory.CreateDirectory(reportFolder);
            }

            //TODO Deploy: change file naming convention
            string exeptionPath = reportFolder + @"\ExceptionReport-" + DateTime.Now.ToString("yyyy-dd-M--HH-mm") + ".txt";
            List<string> metaDataComma = GetMetaData(title);
            List<string> metaData = new List<string>();
            for (int i = 0; i < metaDataComma.Count; i++)
            {
                string item = metaDataComma[i];
                item = item.Replace(",", " ");
                metaData.Add(item);
            }

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(exeptionPath, true))
                {
                    foreach (string line in metaData)
                    {
                        streamWriter.WriteLine(line);
                    }

                    streamWriter.WriteLine(title);

                    streamWriter.WriteLine(reportContent);

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while writing exception report: " + System.IO.Path.GetFileNameWithoutExtension(exeptionPath) + Environment.NewLine + ex.Message);
                return false;
            }
        }

        private static List<string> GetClashTestReportData(List<AClashTest> AClashTests)
        {
            List<string> data = new List<string>();

            foreach (AClashTest aClashTest in AClashTests)
            {
                data.Add(aClashTest.Name + "," + aClashTest.Condition.ToString() + "," +
                         aClashTest.SearchSet1.Name + "," + aClashTest.SearchSet2.Name + "," +
                         aClashTest.TypeName + "," + aClashTest.Tolerance + "," + aClashTest.Priority);
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
                         searchSet1Name + "," + searchSet2Name + "," + ClashTest.TypeName + "," +
                         ClashTest.Tolerance + "," + ClashTest.Priority.Name + " - " + ClashTest.Priority.Description);
            }

            return data;
        }

        private static List<string> GetMetaData(string title)
        {
            List<string> metaData = new List<string>();

            metaData.Add(string.Format("User Domain:,{0}", Views.ViewsHandler.CurrentUser.Domain));
            metaData.Add(string.Format("Username:,{0}", Views.ViewsHandler.CurrentUser.Name));
            metaData.Add(string.Format("Project:,{0}", Views.ViewsHandler.CurrentProject.Name));
            metaData.Add(string.Format("Clash Matrix:,{0}", Views.ViewsHandler.CurrentAClashMatrix.Name));
            metaData.Add(string.Format("Time:,{0}", DateTime.Now.ToString("hh:mm:ss tt")));
            metaData.Add(string.Format("Date:,{0}", DateTime.Now.ToString("yyyy-M-dd")));

            metaData.Add(string.Empty);
            metaData.Add(title);
            metaData.Add(string.Empty);

            return metaData;
        }

        #endregion
    }
}
