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
using Clash_Management_System_Navisworks_Addin.Views;
using Clash_Management_System_Navisworks_Addin.ViewModels;

namespace Clash_Management_System_Navisworks_Addin.NW

{

    //All "GET" below means get from NW
    //All "Create" below means create object and may/may not create into NW

    public static class NWHandler
    {
        #region StaticaMembers

        public static Document document = App.ActiveDocument;

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

        public static List<ASearchSet> GetSearchSet(Document document)
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

        static void GetAllAndNestedSearchSets (SavedItem item, List<SelectionSet> documentSearchSets)
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

        static AClashTestResult GetClashTestResult()
        {
            throw new Exception("Method GetClashTestResult: Work in progress");
            return null;
        }



        #endregion

        //TODO:delete or check method below
        static void dummy(List<ASearchSet> NWSS, List<ASearchSet> DBSS)
        {
            List<ASearchSet> combined = new List<ASearchSet>();

            combined.AddRange(NWSS);
            combined.AddRange(DBSS);



        }
    }
}
