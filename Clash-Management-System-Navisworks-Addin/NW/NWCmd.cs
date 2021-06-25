using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Navisworks.Api.Plugins;

namespace Clash_Management_System_Navisworks_Addin
{
    #region Ribbon
    //[Plugin("Clash-Management-System-Navisworks-Addin", "WYH", DisplayName = "Clash-Management-System-Navisworks-Addin")]
    //[RibbonLayout("Clash-Management-System-Navisworks-Addin.xaml")]
    //[RibbonTab("ID_CustomTab_1", DisplayName ="Hellooooooooo")]
    //[Command("ID_Button_1")]
    //public class NWDCmd : CommandHandlerPlugin
    //{
    //    public override int ExecuteCommand(string name, params string[] parameters)
    //    {
    //        // use button 1 to intiate the dock panel
    //        switch (name)
    //        {
    //            case "ID_Button_1":
    //                MessageBox.Show("Test");
    //                break;
    //        }

    //        return 0;
    //    }
    //} 
    #endregion


    [Plugin("Test", "WYHk", DisplayName = "Hello!")]
    public class NWCmd : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            MessageBox.Show("Hello, World!", "Excute", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return 0;
        }
    }
}