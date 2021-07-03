using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Api.DocumentParts;
using App = Autodesk.Navisworks.Api.Application;
using Clash_Management_System_Navisworks_Addin.DB;
using Clash_Management_System_Navisworks_Addin.Views;
using Clash_Management_System_Navisworks_Addin.ViewModels;

namespace Clash_Management_System_Navisworks_Addin.NW

{
    public static class NWHandler
    {
        #region Static Members

        private static Document _document;
        public static Document Document
        {
            get
            {
                _document = App.ActiveDocument;
                return _document;
            }
        }

        private static List<ASearchSet> _nwASearchSets;
        public static List<ASearchSet> NWASearchSets
        {
            get
            {
                _nwASearchSets = GetSearchSets(Document);
                return _nwASearchSets;
            }
        }

        private static List<AClashTest> _nwAClashTests;
        public static List<AClashTest> NWAClashTests
        {
            get
            {
                _nwAClashTests = GetClashTests(NWHandler.Document);
                return _nwAClashTests;
            }
        }

        private static List<AClashTestResult> _nwAClashResults;
        public static List<AClashTestResult> NWAClashResults
        {
            get
            {
                _nwAClashResults = GetClashTestResults();
                return _nwAClashResults;
            }
        }

        private static DocumentClash _documentClash;

        public static DocumentClash DocumentClash
        {
            get
            {
                _documentClash = Document.Clash as DocumentClash;
                return _documentClash;
            }

        }

        #endregion


        #region ClashMatrixMethods

        static AClashMatrix CreateClashMatrix(List<object> clashMatrixData)
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


        static List<AClashTest> GetClashTests(Document currentDocument)
        {
            throw new Exception("Method GetClashTest: Work in progress");
            return null;
        }

        // ---------> THIS METHOD IS STILL IN PROGRESS (MISSIGN TOLERANCE FROM DB) <---------
        private static ClashTest CreateNewClashTest(AClashTest aClashTest)
        {
            ClashTest clashTest = new ClashTest();

            clashTest.DisplayName = aClashTest.Name;
            clashTest.CustomTestName = aClashTest.Name;
            clashTest.TestType = GetClashTestType(aClashTest.TypeName);
            

            SelectionSet searchSetA = aClashTest.SearchSet1.SelectionSet;
            SelectionSet searchSetB = aClashTest.SearchSet2.SelectionSet;

            SelectionSource selectionSourceA = Document.SelectionSets.CreateSelectionSource(searchSetA);
            SelectionSource selectionSourceB = Document.SelectionSets.CreateSelectionSource(searchSetB);
            SelectionSourceCollection selectionSourceCollectionA = new SelectionSourceCollection();
            SelectionSourceCollection selectionSourceCollectionB = new SelectionSourceCollection();
            selectionSourceCollectionA.Add(selectionSourceA);
            selectionSourceCollectionB.Add(selectionSourceB);

            clashTest.SelectionA.Selection.CopyFrom(selectionSourceCollectionA);
            clashTest.SelectionB.Selection.CopyFrom(selectionSourceCollectionB);

            DocumentClash.TestsData.TestsAddCopy(clashTest);
            return clashTest;
        }

        private static ClashTest CreateNewClashTest(AClashTest aClashTest, ClashTest oldClashTest)
        {
            ClashTest clashTest = new ClashTest();

            clashTest.DisplayName = oldClashTest.DisplayName;
            clashTest.CustomTestName = oldClashTest.CustomTestName;
            clashTest.TestType = GetClashTestType(aClashTest.TypeName);


            SelectionSet searchSetA = aClashTest.SearchSet1.SelectionSet;
            SelectionSet searchSetB = aClashTest.SearchSet2.SelectionSet;

            SelectionSource selectionSourceA = Document.SelectionSets.CreateSelectionSource(searchSetA);
            SelectionSource selectionSourceB = Document.SelectionSets.CreateSelectionSource(searchSetB);
            SelectionSourceCollection selectionSourceCollectionA = new SelectionSourceCollection();
            SelectionSourceCollection selectionSourceCollectionB = new SelectionSourceCollection();
            selectionSourceCollectionA.Add(selectionSourceA);
            selectionSourceCollectionB.Add(selectionSourceB);

            clashTest.SelectionA.Selection.CopyFrom(selectionSourceCollectionA);
            clashTest.SelectionB.Selection.CopyFrom(selectionSourceCollectionB);

            DocumentClash.TestsData.TestsAddCopy(clashTest);
            return clashTest;
        }

        // ---------> THIS MOTTHOD STILL IN PROGRESS <---------
        private static ClashTestType GetClashTestType(string typeName)
        {
            switch (typeName.Trim().ToLower())
            {
                case "hard":
                    return ClashTestType.Hard;
                case "duplicate":
                    return ClashTestType.Duplicate;
                case "clearance":
                    return ClashTestType.Clearance;
                case "hardconservative":
                    return ClashTestType.HardConservative;
                default:
                    return ClashTestType.Custom;
            }
        }

        #endregion


        #region SearchSetMethods

        public static List<ASearchSet> GetSearchSets(Document document)
        {
            DocumentSelectionSets selectionSearchSets = document.SelectionSets;
            List<SelectionSet> documentSearchSets = GetDocumentSearchSets(selectionSearchSets);
            List<ASearchSet> AsearchSets = GetASearchSetsList(documentSearchSets);

            return AsearchSets;
        }


        static List<SelectionSet> GetDocumentSearchSets(DocumentSelectionSets documentSelectionSets)
        {
            SavedItemCollection searchSelectionSets = documentSelectionSets.Value;
            List<SelectionSet> documentSearchSets = new List<SelectionSet>();


            foreach (SavedItem item in searchSelectionSets)
            {
                GetAllAndNestedSearchSets(item, documentSearchSets);
            }

            return documentSearchSets;
        }

        static List<ASearchSet> GetASearchSetsList(List<SelectionSet> searchSets)
        {
            List<ASearchSet> AsearchSets = new List<ASearchSet>();

            foreach (SelectionSet set in searchSets)
            {
                ASearchSet aSearchSet = GetASearchSet(set);
                AsearchSets.Add(aSearchSet);
            }

            return AsearchSets;
        }

        static ASearchSet GetASearchSet(SelectionSet searchSet)
        {
            ASearchSet aSearchSet = new ASearchSet(searchSet, ViewsHandler.CurrentProject,
                                                   ViewsHandler.CurrentUser.Name, ViewsHandler.CurrentAClashMatrix, true);

            return aSearchSet;
        }

        static void GetAllAndNestedSearchSets(SavedItem item, List<SelectionSet> documentSearchSets)
        {
            if (item.IsGroup)
            {
                foreach (SavedItem childItem in ((GroupItem)item).Children)
                {
                    GetAllAndNestedSearchSets(childItem, documentSearchSets);
                }
            }
            else
            {
                SelectionSet searchItem = item as SelectionSet;
                if (searchItem != null && searchItem.HasSearch)
                {
                    documentSearchSets.Add(searchItem);
                }
            }
        }

        #endregion

        #region ClashResultMethods

        static List<AClashTestResult> GetClashTestResults()
        {
            throw new Exception("Method GetClashTestResults: Work in progress");
            return null;
        }


        #endregion
    }
}
