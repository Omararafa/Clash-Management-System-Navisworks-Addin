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
        public static Project CurrentProject { get; set; }
        public static AClashMatrix CurrentAClashMatrix { get; set; }
        public static List<Project> CurrentUserProjects { get; set; }
        public static List<AClashMatrix> CurrentProjectClashMatrix { get; set; }
        #endregion




        #region Static Methods
        static bool WriteCredentialsToAssembly(string name, string password)
        {
            throw new Exception("Method WriteCredentialsToAssembly: Work in progress");
            return false;
        }

        static bool WriteCurrentProjectNameToAssembly(string currentProjectName)
        {
            throw new Exception("Method WriteCurrentProjectNameToAssembly: Work in progress");
            return false;
        }

        static bool WriteCurrentClashMatrixtNameToAssembly(string clashMatrixName)
        {
            throw new Exception("Method WriteCurrentClashMatrixtNameToAssembly: Work in progress");
            return false;
        }

        #endregion

    }
}
