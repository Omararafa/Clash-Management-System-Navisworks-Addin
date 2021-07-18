using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using Autodesk.Navisworks.Api.Plugins;
using Autodesk.Navisworks.Api.DocumentParts;
using Clash_Management_System_Navisworks_Addin.NW;
using Clash_Management_System_Navisworks_Addin.Views;
using Clash_Management_System_Navisworks_Addin.Testing;
using Clash_Management_System_Navisworks_Addin.ViewModels;
using System.Windows.Forms.Integration;

namespace Clash_Management_System_Navisworks_Addin
{
    [Plugin("Clash-Management-System-Navisworks-Addin", "WYHk", DisplayName = "Hello!")]
    [RibbonLayout("Clash-Management-System-Navisworks-Addin.xaml")]
    [RibbonTab("ID_CustomTab_1", DisplayName = "Clash Management")]
    [Command("BE", CanToggle = true, DisplayName = "Button1")]
    [Command("WPFUI", CanToggle = true, DisplayName = "Button2")]


    class NWCmd : CommandHandlerPlugin
    {
        public override int ExecuteCommand(string commandId, params string[] parameters)
        {
            switch (commandId)
            {
                case "BE":
                    SearchSetClassTests.ASearchSetComparisonTest();
                    break;

                case "WPFUI":
                    if (DB.DBHandler.IsServiceAwake)
                    {
                        if (Autodesk.Navisworks.Api.Application.ActiveDocument.Models.Count > 0)
                        {
                            MainWindow mainWindow = new MainWindow();
                            ElementHost.EnableModelessKeyboardInterop(mainWindow);

                            mainWindow.ShowDialog();
                            mainWindow.Topmost = true;
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