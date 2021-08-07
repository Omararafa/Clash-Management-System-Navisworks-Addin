using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clash_Management_System_Navisworks_Addin.ViewModels;

namespace Clash_Management_System_Navisworks_Addin.Views
{
    public static class ViewsHandler
    {
        #region Static Properties
        public static Credentials CurrentCredentials { get; set; }
        public static User CurrentUser { get; set; }
        public static List<Project> CurrentUserProjects
        {
            get
            {
                return CurrentUser.Projects;
            }
        }
        public static Project CurrentProject { get; set; }
        public static AClashMatrix CurrentAClashMatrix { get; set; }
        public static List<AClashMatrix> CurrentProjectClashMatrices
        {
            get
            {
                return ViewsHandler.CurrentProject.ClashMatrices;
            }
        }
        public static List<AClashTest> SelectedClashTests { get; set; }


        #endregion




        #region Static Methods
        


        #endregion

    }
}
