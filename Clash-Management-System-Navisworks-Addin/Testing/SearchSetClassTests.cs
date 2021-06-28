using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using Autodesk.Navisworks.Api.DocumentParts;
using App = Autodesk.Navisworks.Api.Application;
using Clash_Management_System_Navisworks_Addin.NW;
using Clash_Management_System_Navisworks_Addin.ViewModels;

namespace Clash_Management_System_Navisworks_Addin.Testing
{
    public static class SearchSetClassTests
    {
        // >> the mothod has been tested and set private in the NWHandler
        //public static void SearchSetTest()
        //{
        //    return;

        //    string data = string.Empty;
        //    Document document = App.ActiveDocument;
        //    DocumentSelectionSets selectionSets = document.SelectionSets;

        //    List<SelectionSet> documentSearchSets = NWHandler.GetDocumentSearchSets(selectionSets);

        //    foreach (var item in documentSearchSets)
        //    {
        //        data += item.DisplayName + " .. " + item.HasSearch.ToString();
        //        data += Environment.NewLine;
        //    }

        //    MessageBox.Show(data);
        //}

        public static void ASearchSetComparisonTest()
        {
            List<ASearchSet> combinedASearchSets = NWHandler.CompareNWDBASearchSet();

            string data = string.Empty;
            foreach (var searchSet in combinedASearchSets)
            {
                data += "Search set name: " + searchSet.SearchSetName + " Status: " + searchSet.Status + Environment.NewLine;
            }

            MessageBox.Show(data);
        }
    }
}
