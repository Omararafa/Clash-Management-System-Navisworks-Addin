using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Navisworks.Api.Plugins;

namespace Clash_Management_System_Navisworks_Addin.NW
{
    [Plugin("AddinRibbon", "WYH", DisplayName = "AddinRibbon")]
    [RibbonLayout("AddinRibbon.xaml")]
    [RibbonTab("ID_CustomTab_1")]
    [Command("ID_Button_1")]
    public class NWDCmd : CommandHandlerPlugin
    {
        public override int ExecuteCommand(string name, params string[] parameters)
        {
            // use button 1 to intiate the dock panel
            switch (name)
            {
                case "ID_Button_1":
                    MessageBox.Show("Test");
                    break;
            }

            return 0;
        }
    }
}