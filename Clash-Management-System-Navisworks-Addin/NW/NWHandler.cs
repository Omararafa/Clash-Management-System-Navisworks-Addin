using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Clash_Management_System_Navisworks_Addin.ViewModels;
using Autodesk.Navisworks.Api.DocumentParts;

namespace Clash_Management_System_Navisworks_Addin.NW
{

    //All "GET" below means get from NW
    //All "Create" below means create object and may/may not create into NW

    public static class NWHandler
    {
        #region ClashMatrixMethods

        static AClashMatrix CreateClashMatrix(List<object>clashMatrixData)
        {
            throw new Exception("Method CreateClashMatrix: Work in progress");
            return null;
        }


        #endregion



        #region ClashTestMethods

        static AClashTest CreateClashTest(List<object> clashTestData)
        {
            throw new Exception("Method CreateClashTest: Work in progress");
            return null;
        }

        static List<AClashTest> CreateClashTest(List<List<object>> clashTestDatas)
        {
            foreach (var clashTestData in clashTestDatas)
            {
                CreateClashTest(clashTestData);
            }
            throw new Exception("Method CreateClashTest: Work in progress");
            return null;
        }


        static AClashTest GetClashTest(Document currentDocument)
        {
            throw new Exception("Method GetClashTest: Work in progress");
            return null;
        }


        #endregion


        #region SearchSetMethods

        static ASearchSet CreateSearchSet()
        {
            throw new Exception("Method CreateSearchSet: Work in progress");
            return null;
        }

        static ASearchSet GetSearchSet(Document document)
        {
            DocumentSelectionSets selectionSets = Autodesk.Navisworks.Api.Application.ActiveDocument.SelectionSets;
            //selectionSets

            throw new Exception("Method GetSearchSet: Work in progress");
            return null;
        }

        private SavedItemCollection GetSearchSets(DocumentSelectionSets documentSelectionSets)
        {
            SavedItemCollection searchSelectionSets = documentSelectionSets.Value;
            foreach (SavedItem item in searchSelectionSets)
            {
                
            }
        }

        #endregion

        #region ClashResultMethods

        static AClashTestResult GetClashTestResult()
        {
            throw new Exception("Method GetClashTestResult: Work in progress");
            return null;
        }



        #endregion
    }
}
