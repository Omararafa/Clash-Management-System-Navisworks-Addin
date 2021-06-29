using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clash_Management_System_Navisworks_Addin.ViewModels;


namespace Clash_Management_System_Navisworks_Addin.DB
{
    public static class DBHandler
    {
        #region Static Members


        static bool GetProjects(User user, ref List<Project> userProjects)
        {
            string userDomain = user.Domain;

            string userName = user.Name;

            userProjects = new List<Project>();

            WebService.ServiceResponse serviceResponse = new WebService.ClashServiceSoapClient()
            .GetProjects(userDomain, userName);

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
