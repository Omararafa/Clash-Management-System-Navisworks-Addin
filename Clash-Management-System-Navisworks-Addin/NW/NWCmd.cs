using System.Windows.Forms;
using Autodesk.Navisworks.Api.Plugins;
using System.Windows.Forms.Integration;
using Clash_Management_System_Navisworks_Addin.Views;
using System;

namespace Clash_Management_System_Navisworks_Addin
{
    [Plugin("Clash-Management-System-Navisworks-Addin", "AAW", DisplayName = "Clash Management")]
    [RibbonLayout("Clash-Management-System-Navisworks-Addin.xaml")]
    [RibbonTab("ID_CustomTab_1", DisplayName = "Clash Management")]
    [Command("ClashManagement", Icon = "16x16.png", LargeIcon = "32x32.png",
              CanToggle = true, DisplayName = "Clash Management",
              ToolTip = "Sync search sets, clash tests and clash resutls")]

    class NWCmd : CommandHandlerPlugin
    {
        public override int ExecuteCommand(string commandId, params string[] parameters)
        {
            switch (commandId)
            {
                case "ClashManagement":

                    DB.DBHandler.endPointAddress = DB.AddressHandler.GetConfigAddress();

                    DB.DBHandler.address = new System.ServiceModel.EndpointAddress(DB.DBHandler.endPointAddress);

                    if (DB.DBHandler.IsServiceAwake)
                    {
                        if (Autodesk.Navisworks.Api.Application.ActiveDocument.Models.Count > 0)
                        {
                            MainWindow mainWindow = new MainWindow();
                            ElementHost.EnableModelessKeyboardInterop(mainWindow);
                            try
                            {
                                mainWindow.ShowDialog();
                                mainWindow.Topmost = true;
                            }
                            catch (Exception e)
                            {
                                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                                reportContent += e.Message;
                                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
                            }
                        }
                        else
                        {
                            MessageBox.Show("The document does not contain any models!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot connect to the Database!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    break;
            }

            return 0;
        }
    }
}