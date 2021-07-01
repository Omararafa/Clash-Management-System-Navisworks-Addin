using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clash_Management_System_Navisworks_Addin.ViewModels;
using Clash_Management_System_Navisworks_Addin.Views;


namespace Clash_Management_System_Navisworks_Addin.DB
{
    public static class DBHandler
    {
        #region Static Members
        private static List<Project> _projects;
        public static List<Project> Projects
        {
            get
            {
                if (_projects!=null && _projects.Count>0)
                {
                    return _projects;
                }

                _projects = new List<Project>();

                if (GetProjects(ViewsHandler.CurrentUser, ref _projects))
                {
                    return _projects;
                }

                return null;
            }
        }

        public static string TradeAbb;


        #endregion


        #region Database Handler Methods

        public static List<ASearchSet> GenerateASearchSet(List<ASearchSet> nwASearchSets)
        {
            // this method generate a random search set to compare with the one on the navis
            // --------------> || THIS IS A TEST FUNCTION || <--------------

            List<ASearchSet> dbASearchSets = nwASearchSets.ToList();

            int n = dbASearchSets.Count / 3;

            for (int i = 0; i < n; i++)
            {
                dbASearchSets.RemoveAt(i);
            }

            dbASearchSets.ForEach(aSearhSet => aSearhSet.IsFromNavis = false);

            dbASearchSets.Add(new ASearchSet(12, "M_Ducts", 33, new Project(), "WYH", false));
            dbASearchSets.Add(new ASearchSet(13, "FF_Spinklers", 34, new Project(), "WYH", false));
            dbASearchSets.Add(new ASearchSet(14, "EL_Conduits", 32, new Project(), "WYH", false));

            return dbASearchSets;
        }

        #endregion

        #region User Introduction  Methods

        public static bool GetLoginAuthentication(Credentials userCredentials)
        {
            throw new Exception("Method UserAutorized: Work in progress");

            return false;
        }

        static bool GetProjects(User user, ref List<Project> userProjects)
        {
            string userDomain = user.Domain;

            string userName = user.Name;

            userProjects = new List<Project>();

            WebService.ServiceResponse serviceResponse = new WebService.ClashServiceSoapClient()
            .GetProjects(userDomain, userName);

            if (serviceResponse is WebService.Error)
            {
                return false;
            }

            switch (serviceResponse.State)
            {
                case WebService.ResponseState.SUCCESS:

                    WebService.ProjectsResults projectsResults = new WebService.ProjectsResults();

                    List<Project> projects = new List<Project>();

                    string projectName;
                    string projectCode;
                    List<AClashMatrix> projectClashMatrcies;


                    foreach (var dbProject in projectsResults.Projects)
                    {
                        projectName = "";
                        projectCode = "";
                        projectClashMatrcies = new List<AClashMatrix>();
                        Project project = new Project();

                        projectName = dbProject.Name;
                        projectCode = dbProject.Code;

                        foreach (var dbClashMatrix in dbProject.Matrices)
                        {
                            AClashMatrix clashMatrix = new AClashMatrix
                            {
                                Name = dbClashMatrix.Name,
                                Id = dbClashMatrix.Id,
                                Project = project
                            };
                            projectClashMatrcies.Add(clashMatrix);
                        }
                        project.ClashMatrices = projectClashMatrcies;
                    }

                    userProjects = projects;
                    return true;
                case WebService.ResponseState.FAILD:
                    return false;
                default:
                    return false;
            }
        }


        #endregion

        #region SearchSetsHandlers
        static bool SyncSearchSetsWithDB(User user, AClashMatrix clashMatrix, ref List<ASearchSet> searchSetsFromDB,
            List<ASearchSet> searchSetsFromNW)
        {
            string userTradeAbb = user.TradeAbb;
            int clashMatrixId = clashMatrix.Id;
            string[] SearchSetsNames = searchSetsFromNW.Select(x => x.SearchSetName).ToArray();

            WebService.ServiceResponse serviceResponse = new WebService.ClashServiceSoapClient()
            .SyncSearchSets(userTradeAbb, clashMatrixId, SearchSetsNames);

            if (serviceResponse is WebService.Error)
            {
                return false;
            }
            switch (serviceResponse.State)
            {
                case WebService.ResponseState.SUCCESS:

                    //TODO: add a loop to map the SearchSetsResult

                    return true;
                case WebService.ResponseState.FAILD:
                    return false;
                default:
                    return false;
            }
        }
        #endregion

        #region ClashTestsHandlers
        static bool SyncClashTest(AClashMatrix clashMatrix, ref List<AClashTest> clashTests)
        {
            int clashMatrixId = clashMatrix.Id;
            WebService.ServiceResponse serviceResponse = new WebService.ClashServiceSoapClient()

            .GetClashTests(clashMatrixId);

            switch (serviceResponse.State)
            {
                case WebService.ResponseState.SUCCESS:

                    WebService.ClashTestsResults clashTestsResults = new WebService.ClashTestsResults();

                    List<AClashTest> clashTestsFromDB = new List<AClashTest>();


                    foreach (var dbClashTest in clashTestsResults.ClashTests)
                    {

                        AClashTest clashTest = new AClashTest
                        {

                            Name = dbClashTest.Name,
                            Status = EntityComparisonResult.NotChecked,
                            Id = dbClashTest.Id,
                            UniqueName = dbClashTest.UniqueName,
                            Type = dbClashTest.Type,
                            TypeName = dbClashTest.TypeName,
                            ClashMatrixId = dbClashTest.MatrixId,
                            TradeId = dbClashTest.TradeId,
                            TradeCode = dbClashTest.TradeCode,
                            AddedDate = dbClashTest.AddDate,
                            LastRunDate = dbClashTest.LastRunDate,
                            AddedBy = dbClashTest.AddedBy,
                            ProjectCode = dbClashTest.ProjectCode,
                            SearchSet1 = new ASearchSet
                            {

                                Pk = -1,
                                SearchSetName = dbClashTest.SearchSet1.Name,
                                TradeId = dbClashTest.SearchSet1.TradeId,
                                Project = new Project(),
                                ModifiedBy = "",
                                IsFromNavis = false
                            },
                            SearchSet2 = new ASearchSet
                            {

                                Pk = -1,
                                SearchSetName = dbClashTest.SearchSet2.Name,
                                TradeId = dbClashTest.SearchSet2.TradeId,
                                Project = new Project(),
                                ModifiedBy = "",
                                IsFromNavis = false
                            }

                        };

                        clashTestsFromDB.Add(clashTest);

                        clashTests = clashTestsFromDB;


                    }

                    return true;
                case WebService.ResponseState.FAILD:
                    return false;
                default:
                    return false;
            }

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
