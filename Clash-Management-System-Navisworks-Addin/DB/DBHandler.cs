using System;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Clash_Management_System_Navisworks_Addin.NW;
using Clash_Management_System_Navisworks_Addin.Views;
using Clash_Management_System_Navisworks_Addin.ViewModels;

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
                bool isSucceed = SyncClashTests(ViewsHandler.CurrentAClashMatrix, ref _dbAClashTests, ref _dbFailedClashTests);

                if (isSucceed)
                {
                    return _dbAClashTests;
                }

                return null;
            }
        }



        private static List<WebService.ClashTest> _dbFailedClashTests;
        public static List<WebService.ClashTest> DBFailedClashTests
        {
            get
            {
                bool isSucceed = SyncClashTests(ViewsHandler.CurrentAClashMatrix, ref _dbAClashTests, ref _dbFailedClashTests);

                if (isSucceed)
                {
                    return _dbFailedClashTests;
                }

                return null;
            }
        }

        //TODO:Remap TradeAbb
        public static string TradeAbb
        {
            get
            {
                return "SC";
            }
        }

        static EndpointAddress address = new EndpointAddress("http://localhost:9090/ClashService.asmx");
        static WebService.ClashServiceSoapClient service = new WebService.ClashServiceSoapClient(new BasicHttpBinding
        {
            Name = "ClashServiceSoap",
            Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.None },
        },
        address);

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
            try
            {
                string userDomain = user.Domain;
                string userName = user.Name;

                userProjects = new List<Project>();

                var serviceResponse = service.GetProjects(userDomain, userName);

                switch (serviceResponse.State)
                {
                    case WebService.ResponseState.SUCCESS:

                        var projectsResults = serviceResponse as WebService.ProjectsResults;


                        string projectName;
                        string projectCode;
                        List<AClashMatrix> projectClashMatrcies;


                        foreach (var dbProject in projectsResults.Projects)
                        {
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
                            project.Name = projectName;
                            project.Code = projectCode;
                            project.ClashMatrices = projectClashMatrcies;
                            userProjects.Add(project);
                        }

                        return true;

                    case WebService.ResponseState.FAILD:
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return false;
            }
        }


        #endregion  

        #region SearchSetsHandlers
        public static bool SyncSearchSetsWithDB(User user, AClashMatrix clashMatrix, ref List<ASearchSet> searchSetsFromDB,
            List<ASearchSet> searchSetsFromNW)
        {
            string userTradeAbb = DBHandler.TradeAbb;
            int clashMatrixId = clashMatrix.Id;
            searchSetsFromDB = new List<ASearchSet>();
            string[] SearchSetsNames = searchSetsFromNW.Select(x => x.Name).ToArray();

            WebService.ServiceResponse serviceResponse = service
            .SyncSearchSets(userTradeAbb, clashMatrixId, SearchSetsNames);


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
                    WebService.Error error = serviceResponse as WebService.Error;
                    System.Windows.Forms.MessageBox.Show(error.Meesage);
                    return false;
                default:
                    return false;
            }
        }
        #endregion

        #region ClashTestsHandlers

        private static WebService.ClashTest ConvertToDBClashTest(AClashTest nwClashTest)
        {
            return new WebService.ClashTest();
        }
        public static bool SyncClashTests(AClashMatrix clashMatrix, ref List<AClashTest> clashTests, ref List<WebService.ClashTest> dbFailedClashTests)
        {
            if (clashTests == null)
            {
                clashTests = new List<AClashTest>();
            }

            if (dbFailedClashTests == null)
            {
                dbFailedClashTests = new List<WebService.ClashTest>();
            }

            int clashMatrixId = clashMatrix.Id;
            WebService.ServiceResponse serviceResponse = service.GetClashTests(clashMatrixId);

            switch (serviceResponse.State)
            {
                case WebService.ResponseState.SUCCESS:

                    WebService.ClashTestsResults clashTestsResults = serviceResponse as WebService.ClashTestsResults;

                    List<AClashTest> clashTestsFromDB = new List<AClashTest>();
                    List<ASearchSet> nwSearchSets = NWHandler.NWASearchSets;

                    foreach (var dbClashTest in clashTestsResults.ClashTests)
                    {
                        if (IsSearchSetExistOnDB(dbClashTest, nwSearchSets))
                        {
                            AClashTest clashTest = new AClashTest
                            {
                                Name = dbClashTest.Name,
                                Condition = EntityComparisonResult.NotChecked,
                                Id = dbClashTest.Id,
                                UniqueName = dbClashTest.UniqueName,
                                Type = dbClashTest.Type,
                                TypeName = dbClashTest.TypeName,
                                Tolerance = dbClashTest.Tolerance,
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
                        else
                        {
                            dbFailedClashTests.Add(dbClashTest);
                        }
                    }

                    return true;
                case WebService.ResponseState.FAILD:
                    return false;
                default:
                    return false;
            }

        }

        private static bool IsSearchSetExistOnDB(WebService.ClashTest dbClashTest, List<ASearchSet> nwASearchSets)
        {
            if (nwASearchSets.Exists(searchSet => searchSet.Name == dbClashTest.SearchSet1.Name) &&
                nwASearchSets.Exists(searchSet => searchSet.Name == dbClashTest.SearchSet2.Name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ClashResultHandlers

        private static WebService.ClashResult GetDBClashResultFromNWClashResult(AClashTestResult nwResult)
        {
            WebService.ClashResult dbResult = new WebService.ClashResult()
            {
                Guid = nwResult.Guid,
                Name = nwResult.Name,
                State = nwResult.State,
                ApprovedBy = nwResult.ApprovedBy,
                ApprovedTime = nwResult.ApprovedTime.ToString(),
                AssignedTo = nwResult.AssignedTo,
                CreatedTime = nwResult.CreatedTime.ToString(),
                Description = nwResult.Description,
                Comments = nwResult.Comments,
                Distance = nwResult.Distance.ToString(),
                ClashPoint = new WebService.ClashPoint()
                {
                    X = nwResult.ClashPoint.X,
                    Y = nwResult.ClashPoint.Y,
                    Z = nwResult.ClashPoint.Z
                },
                Item1Name = nwResult.Item1Name,
                Item1SourceFile = nwResult.Item1SourceFile,
                Item2Name = nwResult.Item2Name,
                Item2SourceFile = nwResult.Item2SourceFile
            };
            return dbResult;
        }

        //TODO: Method below needs update after ClashResult class update in ViewModels
        public static bool SetClashResultToDB(AClashMatrix clashMatrix, List<AClashTest> clashTestsFromNW)
        {
            int clashMatrixId = clashMatrix.Id;
            int clashTestsCount = clashTestsFromNW.Count;
            WebService.ClashResultSyncRequest[] clashResultSyncRequests = new WebService.ClashResultSyncRequest[clashTestsCount];

            int j = 0;

            foreach (AClashTest nwClashTest in clashTestsFromNW)
            {
                WebService.ClashResultSyncRequest clashResultSyncRequest = new WebService.ClashResultSyncRequest();

                //Build Clash Test DB Object
                WebService.ClashTest dbClashTest = new WebService.ClashTest();

                dbClashTest.AddDate = nwClashTest.AddedDate;
                dbClashTest.AddedBy = nwClashTest.AddedBy;
                dbClashTest.Id = nwClashTest.Id;
                dbClashTest.LastRunDate = nwClashTest.LastRunDate;
                dbClashTest.MatrixId = nwClashTest.ClashMatrixId;
                dbClashTest.Name = nwClashTest.Name;
                dbClashTest.ProjectCode = nwClashTest.ProjectCode;
                dbClashTest.SearchSet1 = new WebService.SearchSetInfo
                {
                    //TODO: Check Id property source from searchSet Id
                    Id = nwClashTest.SearchSet1.TradeId,
                    Name = nwClashTest.SearchSet1.Name,
                    TradeId = nwClashTest.SearchSet1.TradeId,
                    TradeAbbr = TradeAbb
                };
                dbClashTest.SearchSet2 = new WebService.SearchSetInfo
                {
                    //TODO: Check Id property source from searchSet Id
                    Id = nwClashTest.SearchSet1.TradeId,
                    Name = nwClashTest.SearchSet1.Name,
                    TradeId = nwClashTest.SearchSet1.TradeId,
                    TradeAbbr = TradeAbb
                };
                dbClashTest.Tolerance = nwClashTest.Tolerance;
                dbClashTest.TradeId = nwClashTest.TradeId;
                dbClashTest.TradeCode = nwClashTest.TradeCode;
                dbClashTest.Type = nwClashTest.Type;
                dbClashTest.TypeName = nwClashTest.TypeName;
                dbClashTest.UniqueName = nwClashTest.UniqueName;

                clashResultSyncRequest.ClashTest = dbClashTest;

                //TODO: Create, populate ClashResult classes after update of ClashResult class in View Models.
                int ClashTestResultsCount = nwClashTest.ClashTestResults.Count;
                WebService.ClashResult[] dbClashTestResults = new WebService.ClashResult[ClashTestResultsCount];

                int i = 0;
                foreach (var nwResult in nwClashTest.AClashTestResults)
                {
                    WebService.ClashResult dbResult = GetDBClashResultFromNWClashResult(nwResult);
                    dbClashTestResults[i] = dbResult;
                    i++;
                }

                clashResultSyncRequest.NewResults = dbClashTestResults;

                clashResultSyncRequests[i] = clashResultSyncRequest;


                j++;
            }
            WebService.ServiceResponse serviceResponse = service
                .SyncClashResults(clashMatrixId, clashResultSyncRequests.ToArray());

            switch (serviceResponse.State)
            {
                case WebService.ResponseState.SUCCESS:
                    return true;
                case WebService.ResponseState.FAILD:
                default:
                    return false;
            }

        }


        #endregion

    }
}
