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

namespace Clash_Management_System_Navisworks_Addin.Testing
{
    public static class SearchSetClassTests
    {
        public static void SearchSetTest()
        {
            string data = string.Empty;
            Document document = App.ActiveDocument;
            DocumentSelectionSets selectionSets = document.SelectionSets;

            List<SelectionSet> documentSearchSets = NWHandler.GetDocumentSearchSets(selectionSets);

            foreach (var item in documentSearchSets)
            {
                data += item.DisplayName + " .. " + item.HasSearch.ToString();
                data += Environment.NewLine;
            }

            MessageBox.Show(data);
        }
    }
}
