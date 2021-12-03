using System.Linq;
using System.Collections.Generic;
using Clash_Management_System_Navisworks_Addin.DB;
using Clash_Management_System_Navisworks_Addin.NW;
using Clash_Management_System_Navisworks_Addin.ViewModels;
using System;

namespace Clash_Management_System_Navisworks_Addin.DBNWHandler
{
    public static class DBNWComparison
    {
        #region Static Members

        private static List<ASearchSet> _aSearchSetsComparisonList;
        public static List<ASearchSet> ASearchSetsComparisonList
        {
            get
            {
                _aSearchSetsComparisonList = CompareNWDBASearchSets();
                return _aSearchSetsComparisonList;
            }
        }

        private static List<AClashTest> _aClashTestComparisonList;
        public static List<AClashTest> AClashTestComparisonList
        {
            get
            {
                _aClashTestComparisonList = CompareNWDBAClashTests();
                return _aClashTestComparisonList;
            }
        }

        #endregion

        #region Naniswork & Database Comparison Methods
        private static List<ASearchSet> CompareNWDBASearchSets()
        {
            // 1. Create a dictionary for the DB search set
            // 2. Iterate over the NW search set list
            // 3. Check if the item exist in the DB dict:
            //       >> true: Status = NotModified    
            //                Delete this item from the dictionary                         
            //       >> false: Status = New
            // 4. Iterate over the dbDic and set Status property to Deleted
            // 5. Create combined list = dbDic + nwLst

            try
            {
                List<ASearchSet> dbASearchSets = DBHandler.DBASearchSets;
                List<ASearchSet> combinedASearchSets = new List<ASearchSet>();
                Dictionary<string, ASearchSet> dbASearchSetsDic = new Dictionary<string, ASearchSet>();

                foreach (ASearchSet aDBSearchSet in dbASearchSets)
                {
                    if (dbASearchSetsDic.ContainsKey(aDBSearchSet.Name) != false)
                    {
                        dbASearchSetsDic.Add(aDBSearchSet.Name, aDBSearchSet);
                    }
                }

                foreach (ASearchSet nwSearchSet in NWHandler.NWASearchSets)
                {
                    if (dbASearchSetsDic.ContainsKey(nwSearchSet.Name))
                    {
                        nwSearchSet.Conditon = EntityComparisonResult.NotEdited;
                        dbASearchSetsDic.Remove(nwSearchSet.Name);
                    }
                    else
                    {
                        nwSearchSet.Conditon = EntityComparisonResult.New;
                    }
                }

                foreach (string searchSetName in dbASearchSetsDic.Keys)
                {
                    dbASearchSetsDic[searchSetName].Conditon = EntityComparisonResult.Deleted;
                }

                combinedASearchSets.AddRange(NWHandler.NWASearchSets);
                combinedASearchSets.AddRange(dbASearchSetsDic.Values.ToList());
                return combinedASearchSets;
            }
            catch (Exception e)
            {
                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
            }
            return null;
        }

        public static List<AClashTest> CompareNWDBAClashTests()
        {
            // 1. Create a dictionary for the NW clash Tests
            // 2. Iterate over the DB clash tests list
            // 3. Check if the item exist in the NW dict (comparing by the name of the clash test):
            //       >> true:   IF NW == DB (comparing all other preperties):
            //                      - Status = NotModified
            //                      - Delete this item from the dictionary                         
            //                  IF NW != DB (comparing all other preperties):
            //                      - Status = Edited
            //                      - Make NW == DB
            //                      - Delete this item from the dictionary                         
            //       >> false:  Status = New
            //                  Create the clash test in NW
            // 4. Iterate over the NWDic and set Status property to Deleted
            // 5. Create combined list = dbLst + nwLst
            try
            {


                List<ASearchSet> nwASearchSet = NWHandler.NWASearchSets;
                List<AClashTest> nwClashTests = NWHandler.NWAClashTests;
                List<AClashTest> dbAClashTests = DBHandler.DBAClashTests;
                List<AClashTest> combinedAClashTests = new List<AClashTest>();

                if (nwClashTests == null || nwClashTests.Count == 0)
                {
                    foreach (var aClashTest in dbAClashTests)
                    {
                        aClashTest.Condition = EntityComparisonResult.New;
                        combinedAClashTests.Add(aClashTest);
                        return combinedAClashTests;
                    }
                }

                Dictionary<string, AClashTest> nwAClashTestsDic = nwClashTests.ToDictionary(x => x.Name);

                foreach (AClashTest dbAClashTest in dbAClashTests)
                {
                    if (nwAClashTestsDic.ContainsKey(dbAClashTest.Name))
                    {
                        if (IsAClashTestsEqual(nwAClashTestsDic[dbAClashTest.Name], dbAClashTest))
                        {
                            UpdateDBAClashTest(dbAClashTest, nwAClashTestsDic[dbAClashTest.Name]);
                            dbAClashTest.Condition = EntityComparisonResult.NotEdited;

                            nwAClashTestsDic.Remove(dbAClashTest.Name);
                        }
                        else
                        {
                            dbAClashTest.Condition = EntityComparisonResult.Edited;
                            NWHandler.ModifyClashTest(dbAClashTest, nwAClashTestsDic[dbAClashTest.Name], nwASearchSet);
                            UpdateDBAClashTest(dbAClashTest, nwAClashTestsDic[dbAClashTest.Name]);

                            nwAClashTestsDic.Remove(dbAClashTest.Name);
                        }
                    }
                    else
                    {
                        try
                        {

                            NWHandler.CreateNewClashTest(dbAClashTest);
                            dbAClashTest.Condition = EntityComparisonResult.New;

                        }
                        catch (Exception e)
                        {
                            string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                            reportContent += e.Message;
                            Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
                        }
                    }
                }

                foreach (string clashTestName in nwAClashTestsDic.Keys)
                {
                    try
                    {

                        NWHandler.RemoveClashTest(nwAClashTestsDic[clashTestName]);
                        nwAClashTestsDic[clashTestName].Condition = EntityComparisonResult.Deleted;
                    }
                    catch (Exception e)
                    {
                        string reportContent = "Method Name: Login: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                        reportContent += e.Message;
                        Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
                    }
                }

                combinedAClashTests.AddRange(dbAClashTests);
                combinedAClashTests.AddRange(nwAClashTestsDic.Values.ToList());

                return combinedAClashTests;
            }
            catch (Exception e)
            {
                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
            }
            return null;
        }

        private static bool IsAClashTestsEqual(AClashTest nwAClashTest, AClashTest dbAClashTest)
        {
            if (Math.Abs(nwAClashTest.Tolerance - dbAClashTest.Tolerance * 3.28084) < 0.00328084 && // to convert to internal unit (ft).
                nwAClashTest.TypeName == dbAClashTest.TypeName &&
                nwAClashTest.SearchSet1.Name == dbAClashTest.SearchSet1.Name &&
                nwAClashTest.SearchSet2.Name == dbAClashTest.SearchSet2.Name)
            {
                return true;
            }

            return false;
        }

        private static AClashTest UpdateDBAClashTest(AClashTest dbAClashTest, AClashTest nwAClashTesy)
        {
            dbAClashTest.ClashTest = nwAClashTesy.ClashTest;
            dbAClashTest.SearchSet1 = nwAClashTesy.SearchSet1;
            dbAClashTest.SearchSet2 = nwAClashTesy.SearchSet2;
            dbAClashTest.AddedBy = nwAClashTesy.AddedBy;
            dbAClashTest.AddedDate = nwAClashTesy.AddedDate;
            dbAClashTest.LastRunDate = nwAClashTesy.LastRunDate;

            return dbAClashTest;
        }
        #endregion
    }
}
