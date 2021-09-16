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


        public static bool IsServiceAwake
        {
            get
            {
                try
                {
                    return service.IsServiceAwake();
                }
                catch (EndpointNotFoundException)
                {
                    return false;
                }
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
            MaxReceivedMessageSize = 10000000
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

            dbASearchSets.Add(new ASearchSet("M_Ducts", currentClashMatrixId, "Test", "AR", EntityComparisonResult.NotChecked, false));
            dbASearchSets.Add(new ASearchSet("FF_Spinklers", currentClashMatrixId, "AR", "Test", EntityComparisonResult.NotChecked, false));
            dbASearchSets.Add(new ASearchSet("EL_Conduits", currentClashMatrixId, "AR", "Test", EntityComparisonResult.NotChecked, false));

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
                        WebService.Error error = serviceResponse as WebService.Error;
                        System.Windows.Forms.MessageBox.Show("Database Exception: " + error.Meesage);
                        return false;
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Database Exception: " + e.Message);
                return false;
            }
        }


        #endregion  

        #region SearchSetsHandlers
        public static bool SyncSearchSetsWithDB(User user, AClashMatrix clashMatrix, ref List<ASearchSet> searchSetsFromDB,
            List<ASearchSet> searchSetsFromNW)
        {

            try
            {
                int clashMatrixId = clashMatrix.Id;
                searchSetsFromDB = new List<ASearchSet>();
                string[] searchSetsNames = searchSetsFromNW.Select(x => x.Name).ToArray();

                Dictionary<string, string[]> groupedSearchSetNames = GroupSearchSetsByTradeAbb(searchSetsNames);
                bool success = false;
                //We shall create a request for each dictionary key [trade abbreviation]
                foreach (var group in groupedSearchSetNames)
                {
                    //var item = processedSearchSetNames.ElementAt(0);
                    string tradeAbb = group.Key;
                    string[] modifiedSearchSetNames = group.Value;

                    WebService.ServiceResponse serviceResponse = service
                    .SyncSearchSets(tradeAbb, clashMatrixId, modifiedSearchSetNames);


                    switch (serviceResponse.State)
                    {
                        case WebService.ResponseState.SUCCESS:

                            WebService.SyncSearchSetsResults syncSearchSetsResults = serviceResponse as WebService.SyncSearchSetsResults;
                            string name = "";
                            int matrixId = -1;
                            string dbMessage = "";
                            //string tradeAbb = "";
                            EntityComparisonResult status = EntityComparisonResult.NotChecked;

                            foreach (var result in syncSearchSetsResults.Reports)
                            {
                                name = tradeAbb + "-" + result.Name;
                                matrixId = result.MatrixId;
                                dbMessage = result.Message;
                                //tradeAbb = result.TradeAbb;

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

                                ASearchSet searchSet = new ASearchSet(name, matrixId, dbMessage, tradeAbb, status, false);
                                searchSetsFromDB.Add(searchSet);
                            }

                            success = true;
                            break;
                        case WebService.ResponseState.FAILD:
                            WebService.Error error = serviceResponse as WebService.Error;
                            System.Windows.Forms.MessageBox.Show(error.Meesage);
                            success = false;
                            break;
                        default:
                            success = false;
                            break;
                    }

                }
                return success;
                //TODO: Add lines below to the foreach looping above DB debugging

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Database Exception: " + e.Message);
                return false;
            }
        }
        #endregion

        #region ClashTestsHandlers

        public static bool SyncClashTests(AClashMatrix clashMatrix, ref List<AClashTest> clashTestsFromDB, ref List<WebService.ClashTest> dbFailedClashTests)
        {
            try
            {
                clashTestsFromDB = new List<AClashTest>();
                dbFailedClashTests = new List<WebService.ClashTest>();

                int clashMatrixId = clashMatrix.Id;
                WebService.ServiceResponse serviceResponse = service.GetClashTests(clashMatrixId);

                switch (serviceResponse.State)
                {
                    case WebService.ResponseState.SUCCESS:

                        WebService.ClashTestsResults clashTestsResults = serviceResponse as WebService.ClashTestsResults;

                        List<ASearchSet> nwASearchSets = NWHandler.NWASearchSets;

                        foreach (var dbClashTest in clashTestsResults.ClashTests)
                        {
                            AClashTest clashTest = new AClashTest
                            {
                                Name = dbClashTest.Name,
                                Condition = EntityComparisonResult.NotChecked,
                                Id = dbClashTest.Id,
                                UniqueName = dbClashTest.UniqueName,
                                Type = dbClashTest.Type,
                                TypeName = dbClashTest.TypeName,
                                //Tolerance = dbClashTest.Tolerance,
                                ClashMatrixId = dbClashTest.MatrixId,
                                TradeId = dbClashTest.TradeId,
                                TradeCode = dbClashTest.TradeCode,
                                AddedDate = dbClashTest.AddDate,
                                LastRunDate = dbClashTest.LastRunDate,
                                AddedBy = dbClashTest.AddedBy,
                                ProjectCode = dbClashTest.ProjectCode,
                                SearchSet1 = nwASearchSets.Where(searchSet => searchSet.Name == ModifyDBSearchSetName(dbClashTest.SearchSet1)).FirstOrDefault(),
                                SearchSet2 = nwASearchSets.Where(searchSet => searchSet.Name == ModifyDBSearchSetName(dbClashTest.SearchSet2)).FirstOrDefault()
                            };

                            if (clashTest.SearchSet1 == null || clashTest.SearchSet2 == null)
                            {
                                dbFailedClashTests.Add(dbClashTest);
                                continue;
                            }

                            clashTest.SearchSet1.Pk = -1;
                            clashTest.SearchSet1.TradeId = dbClashTest.SearchSet1.TradeId;
                            clashTest.SearchSet1.Project = ViewsHandler.CurrentProject;
                            clashTest.SearchSet1.ModifiedBy = "";
                            clashTest.SearchSet1.IsFromNavis = true;

                            clashTest.SearchSet2.Pk = -1;
                            clashTest.SearchSet2.TradeId = dbClashTest.SearchSet2.TradeId;
                            clashTest.SearchSet2.Project = ViewsHandler.CurrentProject;
                            clashTest.SearchSet2.ModifiedBy = "";
                            clashTest.SearchSet2.IsFromNavis = true;

                            clashTestsFromDB.Add(clashTest);
                        }

                        return true;
                    case WebService.ResponseState.FAILD:
                        return false;
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Database Exception: " + e.Message);
                return false;
            }

        }

        private static bool IsSearchSetExistOnDB(WebService.ClashTest dbClashTest, List<ASearchSet> nwASearchSets)
        {
            if (nwASearchSets.Exists(searchSet => searchSet.Name == ModifyDBSearchSetName(dbClashTest.SearchSet1)) &&
                nwASearchSets.Exists(searchSet => searchSet.Name == ModifyDBSearchSetName(dbClashTest.SearchSet2)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string ModifyDBSearchSetName(WebService.SearchSetInfo searchSet)
        {
            return searchSet.TradeAbbr + "-" + searchSet.Name;
        }

        private static Dictionary<string, string[]> GroupSearchSetsByTradeAbb(string[] namesWithTradeAbb)
        {
            Dictionary<string, string[]> processedNames = new Dictionary<string, string[]>();

            List<string[]> splittedNames = new List<string[]>();

            foreach (var nameWithAbb in namesWithTradeAbb)
            {
                string[] splitedName = nameWithAbb.Split(new char[] { '-' }, 2);
                splittedNames.Add(splitedName);
            }

            var groupedSplittedNames = splittedNames.GroupBy(x => x[0]).ToList();
            foreach (var group in groupedSplittedNames)
            {
                string tradeAbb = group.Key;
                List<string> value = new List<string>();
                foreach (var name in group)
                {
                    value.Add(name[1]);
                }
                processedNames.Add(tradeAbb, value.ToArray());
            }

            return processedNames;
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
            if (dbResult.Item1Name == string.Empty)
            {
                dbResult.Item1Name = "Not Identified";
            }

            if (dbResult.Item2Name == string.Empty)
            {
                dbResult.Item2Name = "Not Identified";
            }
            return dbResult;
        }

        //TODO: Method below needs update after ClashResult class update in ViewModels
        public static bool SyncClashResultToDB(AClashMatrix clashMatrix, List<AClashTest> clashTestsFromNW)
        {
            int clashMatrixId = clashMatrix.Id;
            int clashTestsCount = clashTestsFromNW.Count;
            List<WebService.ClashResultSyncRequest> clashResultSyncRequests = new List<WebService.ClashResultSyncRequest>();

            foreach (AClashTest nwClashTest in clashTestsFromNW)
            {
                //if (nwClashTest.AClashTestResults.Count < 1)
                //{
                //    continue;
                //}

                //Build Clash Test DB Object
                WebService.ClashTest dbClashTest = new WebService.ClashTest();
                //Get corresponding DB ClashTest [to be used for id extraction]
                AClashTest dbAClashTest = DBAClashTests.Where(clashTest => clashTest.Name == nwClashTest.Name).FirstOrDefault();

                if (dbAClashTest == null)
                {
                    continue;
                }


                #region Legacy code

                /*
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
                //TODO: report tolerance issue to DB
                //dbClashTest.Tolerance = nwClashTest.Tolerance;
                dbClashTest.TradeId = nwClashTest.TradeId;
                dbClashTest.TradeCode = nwClashTest.TradeCode;
                dbClashTest.Type = nwClashTest.Type;
                dbClashTest.TypeName = nwClashTest.TypeName;
                dbClashTest.UniqueName = nwClashTest.UniqueName;

                clashResultSyncRequest.ClashTest = dbClashTest;
                */
                #endregion


                int ClashTestResultsCount = nwClashTest.AClashTestResults.Count;
                //Build DB ClashResultSyncRequest object
                WebService.ClashResultSyncRequest clashResultSyncRequest = new WebService.ClashResultSyncRequest();
                //[Extractor id]
                clashResultSyncRequest.ClashTestId = dbAClashTest.Id;

                //Build DB ClashResult[] object
                List<WebService.ClashResult> dbClashTestResults = new List<WebService.ClashResult>();
                //Populate ClashTestResult into the list
                foreach (var nwResult in nwClashTest.AClashTestResults)
                {
                    WebService.ClashResult dbResult = GetDBClashResultFromNWClashResult(nwResult);
                    dbClashTestResults.Add(dbResult);
                }

                clashResultSyncRequest.NewResults = dbClashTestResults.ToArray();

                clashResultSyncRequests.Add(clashResultSyncRequest);

            }

            WebService.ServiceResponse serviceResponse = service
                        .SyncClashResults(clashMatrixId, clashResultSyncRequests.ToArray());

            switch (serviceResponse.State)
            {
                case WebService.ResponseState.SUCCESS:
                    return true;
                case WebService.ResponseState.FAILD:
                    System.Windows.Forms.MessageBox.Show("DB Error:" + (serviceResponse as WebService.Error).Meesage);
                    return false;
                default:
                    return false;
            }


        }


        #endregion

    }
}
