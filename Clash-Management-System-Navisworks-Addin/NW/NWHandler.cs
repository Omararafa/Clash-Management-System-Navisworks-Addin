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
                _nwASearchSets = GetSearchSets();
                return _nwASearchSets;
            }
        }

        private static List<AClashTest> _nwAClashTests;
        public static List<AClashTest> NWAClashTests
        {
            get
            {
                _nwAClashTests = GetClashTests();
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

        #region SearchSetMethods

        public static List<ASearchSet> GetSearchSets()
        {
            DocumentSelectionSets selectionSearchSets = Document.SelectionSets;
            List<SelectionSet> documentSearchSets = GetDocumentSearchSets(selectionSearchSets);
            List<ASearchSet> aSearchSets = GetASearchSetsList(documentSearchSets);

            return aSearchSets;
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

        #region ClashTestMethods
        private static List<AClashTest> GetClashTests()
        {
            List<ClashTest> documentClashTests = GetDocumentClashTests();
            List<AClashTest> aClashTests = GetAClashTestList(documentClashTests);

            return aClashTests;
        }

        private static List<AClashTest> GetAClashTestList(List<ClashTest> ClashTests)
        {
            List<AClashTest> aClashTests = new List<AClashTest>();

            foreach (ClashTest clashTest in ClashTests)
            {
                AClashTest aClashTest = GetAClashTest(clashTest);
                aClashTests.Add(aClashTest);
            }

            return aClashTests;
        }

        private static AClashTest GetAClashTest(ClashTest clashTest)
        {
            ASearchSet aSearhSetA = NWHandler.NWASearchSets.Find(set =>
                set.Name == GetSelectionSetFromSelectionSource(clashTest.SelectionA.Selection.SelectionSources.FirstOrDefault()).DisplayName);

            ASearchSet aSearhSetB = NWHandler.NWASearchSets.Find(set =>
                set.Name == GetSelectionSetFromSelectionSource(clashTest.SelectionB.Selection.SelectionSources.FirstOrDefault()).DisplayName);

            AClashTest aClashTest = new AClashTest
            {
                Name = clashTest.DisplayName,
                Status = EntityComparisonResult.NotChecked,
                TypeName = GetClashTestType(clashTest.TestType),
                Tolerance = clashTest.Tolerance,
                ClashMatrixId = ViewsHandler.CurrentAClashMatrix.Id,
                ProjectCode = ViewsHandler.CurrentProject.Code,
                IsFromNavis = true,
                ClashTest = clashTest,
                SearchSet1 = aSearhSetA,
                SearchSet2 = aSearhSetB,
            };

            return aClashTest;
        }

        private static List<ClashTest> GetDocumentClashTests()
        {
            return DocumentClash.TestsData.Tests.Cast<ClashTest>().ToList();
        }

        private static ClashTest CreateNewClashTest(AClashTest aClashTest)
        {
            ClashTest clashTest = new ClashTest();

            clashTest.DisplayName = aClashTest.Name;
            clashTest.CustomTestName = aClashTest.Name;
            clashTest.TestType = GetClashTestType(aClashTest.TypeName);
            clashTest.Tolerance = aClashTest.Tolerance;


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

        public static AClashTest ModifyClashTest(AClashTest sourceAClashTest, AClashTest targetAClashTest)
        {
            // sourceClashTest: The ClashTest to copy properties from
            // targetClashTest: The ClashTest to edit

            ClashTest tempClashTest = new ClashTest();

            tempClashTest.DisplayName = sourceAClashTest.ClashTest.DisplayName;
            tempClashTest.CustomTestName = sourceAClashTest.ClashTest.CustomTestName;
            tempClashTest.TestType = sourceAClashTest.ClashTest.TestType;
            tempClashTest.Tolerance = sourceAClashTest.ClashTest.Tolerance;


            SelectionSet searchSetA = sourceAClashTest.SearchSet1.SelectionSet;
            SelectionSet searchSetB = sourceAClashTest.SearchSet2.SelectionSet;

            SelectionSource selectionSourceA = Document.SelectionSets.CreateSelectionSource(searchSetA);
            SelectionSource selectionSourceB = Document.SelectionSets.CreateSelectionSource(searchSetB);
            SelectionSourceCollection selectionSourceCollectionA = new SelectionSourceCollection();
            SelectionSourceCollection selectionSourceCollectionB = new SelectionSourceCollection();
            selectionSourceCollectionA.Add(selectionSourceA);
            selectionSourceCollectionB.Add(selectionSourceB);

            tempClashTest.SelectionA.Selection.CopyFrom(selectionSourceCollectionA);
            tempClashTest.SelectionB.Selection.CopyFrom(selectionSourceCollectionB);

            DocumentClash.TestsData.TestsEditTestFromCopy(targetAClashTest.ClashTest, tempClashTest);
            return targetAClashTest;
        }

        private static ClashTest RemoveClashTest(AClashTest aClashTest)
        {
            DocumentClashTests documentClashTests = DocumentClash.TestsData;
            ClashTest targetClashTest = documentClashTests.Tests.
                FirstOrDefault(clashTest => clashTest.DisplayName == aClashTest.Name) as ClashTest;


            DocumentClash.TestsData.TestsRemove(targetClashTest);
            return targetClashTest;
        }

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

        private static string GetClashTestType(ClashTestType type)
        {
            switch (type)
            {
                case ClashTestType.Hard:
                    return "Hard";
                case ClashTestType.Duplicate:
                    return "Duplicate";
                case ClashTestType.Clearance:
                    return "Clearance";
                case ClashTestType.HardConservative:
                    return "HardConservative";
                default:
                    return "Custom";
            }
        }

        private static SelectionSet GetSelectionSetFromSelectionSource(SelectionSource selectionSource)
        {
            return Document.SelectionSets.ResolveSelectionSource(selectionSource) as SelectionSet;
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
