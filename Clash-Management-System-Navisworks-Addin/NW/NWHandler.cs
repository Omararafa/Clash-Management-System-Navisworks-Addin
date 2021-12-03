﻿using System.Linq;
using System.Windows.Forms;
using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using Autodesk.Navisworks.Api.Clash;
using Autodesk.Navisworks.Api.DocumentParts;
using App = Autodesk.Navisworks.Api.Application;
using Clash_Management_System_Navisworks_Addin.ViewModels;
using System;

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
                List<AClashTest> aClashTests = GetClashTests();
                _nwAClashTests = UpdateAClashTestsResults(aClashTests);


                if (aClashTests != null)
                {
                    if (aClashTests.Count > 0)
                    {
                        IsClashTestsCalled = true;
                    }
                    return _nwAClashTests;
                }

                return null;
            }

            set
            {
                _nwAClashTests = value;
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

        public static bool IsClashTestsCalled { get; set; }

        #endregion

        #region SearchSetMethods

        private static List<ASearchSet> GetSearchSets()
        {
            try
            {


                DocumentSelectionSets selectionSearchSets = Document.SelectionSets;

                if (selectionSearchSets != null && selectionSearchSets.Value.Count > 0)
                {
                    List<SelectionSet> documentSearchSets = GetDocumentSearchSets(selectionSearchSets);

                    if (documentSearchSets != null && documentSearchSets.Count > 0)
                    {
                        List<ASearchSet> aSearchSets = GetASearchSetsList(documentSearchSets);
                        return aSearchSets;
                    }
                    else
                    {
                        MessageBox.Show("Error, No search sets defined in the document!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }

                }

                MessageBox.Show("Error, No search sets defined in the document!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception e)
            {
                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
                return null;
            }
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
            ASearchSet aSearchSet = new ASearchSet(
                selectionSet: searchSet,
                isFromNavis: true);

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

            if (documentClashTests != null && documentClashTests.Count > 0)
            {
                List<AClashTest> aClashTests = GetAClashTestList(documentClashTests);
                return aClashTests;
            }

            return null;
        }

        private static List<ClashTest> GetDocumentClashTests()
        {
            return DocumentClash.TestsData.Tests.Cast<ClashTest>().ToList();
        }

        private static List<AClashTest> GetAClashTestList(List<ClashTest> ClashTests)
        {
            List<AClashTest> aClashTests = new List<AClashTest>();
            List<ASearchSet> nwASearchSets = NWASearchSets;

            foreach (ClashTest clashTest in ClashTests)
            {
                AClashTest aClashTest = GetAClashTest(clashTest, nwASearchSets);
                aClashTests.Add(aClashTest);
            }

            return aClashTests;
        }

        private static AClashTest GetAClashTest(ClashTest clashTest, List<ASearchSet> nwASearchSets)
        {
            try
            {
                if (clashTest == null || nwASearchSets == null)
                {
                    return null;
                }
                if (nwASearchSets.Count > 0)
                {
                    ASearchSet aSearhSetA = nwASearchSets.Find(set =>
                        set.Name == GetSelectionSetFromSelectionSource(clashTest.SelectionA.Selection.SelectionSources.FirstOrDefault()).DisplayName);

                    ASearchSet aSearhSetB = nwASearchSets.Find(set =>
                        set.Name == GetSelectionSetFromSelectionSource(clashTest.SelectionB.Selection.SelectionSources.FirstOrDefault()).DisplayName);

                    AClashTest aClashTest = new AClashTest(
                        name: clashTest.DisplayName,
                        condition: EntityComparisonResult.NotChecked,
                        typeName: GetClashTestType(clashTest.TestType),
                        tolerance: clashTest.Tolerance,
                        isFromNavis: true,
                        clashTest: clashTest,
                        searchSet1: aSearhSetA,
                        searchSet2: aSearhSetB);

                    return aClashTest;
                }

            }
            catch (Exception e)
            {
                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
            }
            return null;
        }

        public static ClashTest CreateNewClashTest(AClashTest aClashTest)
        {
            try
            {
                ClashTest clashTest = new ClashTest();

                clashTest.DisplayName = aClashTest.Name;
                clashTest.CustomTestName = aClashTest.Name;
                clashTest.TestType = GetClashTestType(aClashTest.TypeName);
                clashTest.Tolerance = aClashTest.Tolerance * 3.28084; // to convert to the internal unit (ft).


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
            catch (Exception e)
            {
                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
                return null;
            }
        }

        public static AClashTest ModifyClashTest(AClashTest sourceAClashTest, AClashTest targetAClashTest, List<ASearchSet> nwASearchSets)
        {
            try
            {
                // sourceClashTest: The ClashTest to copy properties from
                // targetClashTest: The ClashTest to edit

                ClashTest tempClashTest = new ClashTest();

                tempClashTest.DisplayName = sourceAClashTest.Name;
                tempClashTest.CustomTestName = sourceAClashTest.Name;
                tempClashTest.TestType = GetClashTestType(sourceAClashTest.TypeName.ToLower());
                tempClashTest.Tolerance = sourceAClashTest.Tolerance * 3.28084; // to convert to the internal unit (ft).

                targetAClashTest.Id = sourceAClashTest.Id;

                SelectionSet searchSetA = nwASearchSets.Where(searchSet => searchSet.Name == sourceAClashTest.SearchSet1.Name).First().SelectionSet;
                SelectionSet searchSetB = nwASearchSets.Where(searchSet => searchSet.Name == sourceAClashTest.SearchSet2.Name).First().SelectionSet;

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
            catch (Exception e)
            {
                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
                return null;
            }
        }

        public static ClashTest RemoveClashTest(AClashTest aClashTest)
        {
            try
            {

                DocumentClashTests documentClashTests = DocumentClash.TestsData;
                ClashTest targetClashTest = documentClashTests.Tests.
                    FirstOrDefault(clashTest => clashTest.DisplayName == aClashTest.Name) as ClashTest;


                DocumentClash.TestsData.TestsRemove(targetClashTest);
                return targetClashTest;

            }
            catch (Exception e)
            {
                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
            }
            return null;
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

        public static List<AClashTest> UpdateAClashTestsResults(List<AClashTest> nwAClashTests)
        {
            try
            {
                if (nwAClashTests == null || nwAClashTests.Count < 1)
                {
                    return null;
                }

                foreach (AClashTest aClashTest in nwAClashTests)
                {
                    if (aClashTest == null || aClashTest.ClashTest == null)
                    {
                        continue;
                    }
                    List<AClashTestResult> aClashTestResults = new List<AClashTestResult>();
                    List<ClashResult> clashResults = GetClashResults(aClashTest.ClashTest);

                    if (clashResults == null || clashResults.Count < 1)
                    {
                        continue;
                    }
                    foreach (ClashResult clashResult in clashResults)
                    {
                        AClashTestResult aClashTestResult = new AClashTestResult(aClashTest, clashResult);
                        aClashTestResults.Add(aClashTestResult);
                    }

                    aClashTest.AClashTestResults = aClashTestResults;
                }
            }
            catch (Exception e)
            {
                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
            }
            return nwAClashTests;
        }

        private static List<ClashResult> GetClashResults(ClashTest clashTest)
        {
            try
            {


                List<ClashResult> clashTestResultsLst = clashTest.Children.Cast<ClashResult>().ToList();

                return clashTestResultsLst;
            }
            catch (Exception e)
            {
                string reportContent = "Method Name: " + System.Reflection.MethodBase.GetCurrentMethod().Name;
                reportContent += e.Message;
                Reporting.ReportHandler.WriteExceptionLog(e.GetType().Name, reportContent);
                return null;
            }
        }

        #endregion
    }
}
