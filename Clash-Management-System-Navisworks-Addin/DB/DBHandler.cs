using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clash_Management_System_Navisworks_Addin.ViewModels;

using Project = Clash_Management_System_Navisworks_Addin.ViewModels.Project;

namespace Clash_Management_System_Navisworks_Addin.DB
{
    public static class DBHandler
    {
        #region Static Members


        static bool GetProjects(User user)
        {
            string userDomain = user.Domain;
            
            string userName = user.Name;

            WebService.ServiceResponse serviceResponse = new WebService.ClashServiceSoapClient()
            .GetProjects(userDomain, userName);

            switch (serviceResponse.State)
            {
                case WebService.ResponseState.SUCCESS:
                    return true;
                case WebService.ResponseState.FAILD:
                    return false;
                default:
                    return false;
            }
        }



        static bool SyncSearchSetsWithDB(User user, AClashMatrix clashMatrix, List<ASearchSet> searchSets)
        {
            string userTradeAbb = user.TradeAbb;
            int clashMatrixId = clashMatrix.Id;
            string[] SearchSetsNames = searchSets.Select(x => x.SearchSetName).ToArray();

            WebService.ServiceResponse serviceResponse = new WebService.ClashServiceSoapClient()
            .SyncSearchSets(userTradeAbb, clashMatrixId, SearchSetsNames);

            switch (serviceResponse.State)
            {
                case WebService.ResponseState.SUCCESS:
                    return true;
                case WebService.ResponseState.FAILD:
                    return false;
                default:
                    return false;
            }
        }



        #endregion

        #region Database Handler Methods



        #endregion



        #region User Introduction  Methods

        public static bool GetLoginAuthentication(Credentials userCredentials)
        {
            throw new Exception("Method UserAutorized: Work in progress");

            return false;
        }

        public static List<Project> GetUserProjects(User user)
        {
            throw new Exception("Method GetUserProjects: Work in progress");

            return null;
        }

        public static List<AClashMatrix> GetProjectClashMatrices(Project project)
        {
            throw new Exception("Method GetProjectClashMatrices: Work in progress");

            return null;
        }
        #endregion

        #region SearchSetsHandlers
        public static List<string> GetSearchSetsFromDB(AClashMatrix clashMatrix)
        {
            throw new Exception("Method GetSearchSetsFromDB: Work in progress");

            return null;
        }

        public static List<List<string>> SettSearchSetsToDB()
        {
            throw new Exception("Method SetSearchSetsToDB: Work in progress");

            return null;
        }
        #endregion

        #region ClashTestsHandlers
        public static List<string> GetClashTestsFromDB()
        {
            throw new Exception("Method GetClashTestsFromDB: Work in progress");

            return null;
        }

        public static List<List<string>> SetClashTestsToDB()
        {
            throw new Exception("Method SetClashTestsToDB: Work in progress");

            return null;
        }
        #endregion

        #region ClashResultHandlers


        public static List<List<string>> SetClashResultToDB()
        {
            throw new Exception("Method SetClashResultToDB: Work in progress");

            return null;
        }
        #endregion




    }
}
