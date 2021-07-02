using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        // --------------> THIS METHOD IS STIL IN PROGRESS <--------------
        public static void WriteReport(List<Object> records, string reportPath)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(reportPath, false))
                {
                    streamWriter.WriteLine();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("An error occurs, please retry!" + Environment.NewLine + ex.Message);
            }
        }
        #endregion
    }
}
