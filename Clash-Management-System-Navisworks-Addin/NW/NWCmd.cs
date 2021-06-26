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


    [Plugin("Clash-Management-System-Navisworks-Addin", "WYHk", DisplayName = "Hello!")]
    [RibbonLayout("Clash-Management-System-Navisworks-Addin.xaml")]
    [RibbonTab("ID_CustomTab_1", DisplayName = "Clash Management")]
    [Command("Clash-Management-System-Navisworks-Addin", CanToggle = true, DisplayName = "Clash Manager")]


    class Clash_Management_System_Navisworks_Addin : CommandHandlerPlugin
    {
        bool m_toEnableButton = true; // to enable button or not
        public override int ExecuteCommand(string commandId, params string[] parameters)
        {
            

            switch (commandId)
            {

                case "Clash-Management-System-Navisworks-Addin":
                    {
                        MessageBox.Show("Hello, Wael!", "Holla, it worked!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        break;

                    }
            }

            return 0;
        }

    }
}