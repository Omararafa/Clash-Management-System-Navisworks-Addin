using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Clash_Management_System_Navisworks_Addin.DB;
using Clash_Management_System_Navisworks_Addin.NW;
using Clash_Management_System_Navisworks_Addin.ViewModels;

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


            List<ASearchSet> dbASearchSets = DBHandler.GenerateASearchSet(NWHandler.NWASearchSets);
            List<ASearchSet> combinedASearchSets = new List<ASearchSet>();

            Dictionary<string, ASearchSet> dbASearchSetsDic = dbASearchSets.ToDictionary(x => x.Name);

            foreach (ASearchSet nwSearchSet in NWHandler.NWASearchSets)
            {
                if (dbASearchSetsDic.ContainsKey(nwSearchSet.Name))
                {
                    nwSearchSet.Status = EntityComparisonResult.NotEdited;
                    dbASearchSetsDic.Remove(nwSearchSet.Name);
                }
                else
                {
                    nwSearchSet.Status = EntityComparisonResult.New;
                }
            }

            foreach (string searchSetName in dbASearchSetsDic.Keys)
            {
                dbASearchSetsDic[searchSetName].Status = EntityComparisonResult.Deleted;
            }

            combinedASearchSets.AddRange(NWHandler.NWASearchSets);
            combinedASearchSets.AddRange(dbASearchSetsDic.Values.ToList());

            return combinedASearchSets;
        }


        private static List<AClashTest> CompareNWDBAClashTests()
        {
            throw new NotImplementedException("Method CompareNWDBAClashTests: Work in progress");

            return null;
        }
        #endregion
    }
}
