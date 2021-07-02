using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Clash_Management_System_Navisworks_Addin.NW;
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
                if (_projects != null && _projects.Count > 0)
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

        private static List<ASearchSet> _dbASearchSets;
        public static List<ASearchSet> DBASearchSets
        {
            get
            {
                bool isSucceed = SyncSearchSetsWithDB(ViewsHandler.CurrentUser, ViewsHandler.CurrentAClashMatrix, ref _dbASearchSets, NWHandler.NWASearchSets);

                if (isSucceed)
                {
                    return _dbASearchSets;
                }

                return null;
            }
        }

        private static List<AClashTest> _dbAClashTests;
        public static List<AClashTest> DBAClashTests
        {
            get
            {
                bool isSucceed = SyncClashTest(ViewsHandler.CurrentAClashMatrix, ref _dbAClashTests);

                if (isSucceed)
                {
                    return _dbAClashTests;
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

            int currentClashMatrixId = Views.ViewsHandler.CurrentAClashMatrix.Id;

            for (int i = 0; i < n; i++)
            {
                dbASearchSets.RemoveAt(i);
            }


            dbASearchSets.ForEach(aSearhSet => aSearhSet.IsFromNavis = false);

            dbASearchSets.Add(new ASearchSet("M_Ducts", currentClashMatrixId, "Test", EntityComparisonResult.NotChecked, false));
            dbASearchSets.Add(new ASearchSet("FF_Spinklers", currentClashMatrixId, "Test", EntityComparisonResult.NotChecked, false));
            dbASearchSets.Add(new ASearchSet("EL_Conduits", currentClashMatrixId, "Test", EntityComparisonResult.NotChecked, false));

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
            searchSetsFromDB = new List<ASearchSet>();
            string[] SearchSetsNames = searchSetsFromNW.Select(x => x.Name).ToArray();

            WebService.ServiceResponse serviceResponse = new WebService.ClashServiceSoapClient()
            .SyncSearchSets(userTradeAbb, clashMatrixId, SearchSetsNames);

            if (serviceResponse is WebService.Error)
            {
                return false;
            }
            switch (serviceResponse.State)
            {
                case WebService.ResponseState.SUCCESS:

                    WebService.SyncSearchSetsResults syncSearchSetsResults = serviceResponse as WebService.SyncSearchSetsResults;
                    string name = "";
                    int matrixId = -1;
                    string dbMessage = "";
                    EntityComparisonResult status = EntityComparisonResult.NotChecked;

                    foreach (var result in syncSearchSetsResults.Reports)
                    {
                        name = result.Name;
                        matrixId = result.MatrixId;
                        dbMessage = result.Message;

                        switch (result.ReportType)
                        {
                            case WebService.ReportType.ADD:
                                status = EntityComparisonResult.New;
                                break;
                            case WebService.ReportType.REMOVE:
                                status = EntityComparisonResult.Deleted;
                                break;
                            case WebService.ReportType.UPDATE:
                                status = EntityComparisonResult.Edited;
                                break;
                            default:
                                break;
                        }

                        ASearchSet searchSet = new ASearchSet(name, matrixId, dbMessage, status, false);
                        searchSetsFromDB.Add(searchSet);
                    }

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
                                Name = dbClashTest.SearchSet1.Name,
                                TradeId = dbClashTest.SearchSet1.TradeId,
                                Project = new Project(),
                                ModifiedBy = "",
                                IsFromNavis = false
                            },
                            SearchSet2 = new ASearchSet
                            {

                                Pk = -1,
                                Name = dbClashTest.SearchSet2.Name,
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
