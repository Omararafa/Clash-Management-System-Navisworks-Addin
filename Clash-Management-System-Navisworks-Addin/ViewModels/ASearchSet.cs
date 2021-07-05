using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using Clash_Management_System_Navisworks_Addin.Views;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class ASearchSet
    {
        public int Pk { get; set; }
        public int TradeId { get; set; }
        public Project Project { get; set; }
        public bool IsFromNavis { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public string DBMessage { get; set; }
        public int ClashMatrixId { get; set; }
        public AClashMatrix ClashMatrix { get; set; }
        public SelectionSet SelectionSet { get; set; }
        public EntityComparisonResult Status { get; set; }

        public ASearchSet()
        {

        }

        // Database initiation constructor
        public ASearchSet(string name, int clashMatrixId, string dbMessage, EntityComparisonResult status, bool isFromNavis)
        {
            this.Name = name;
            this.ClashMatrixId = clashMatrixId;
            this.DBMessage = dbMessage;
            this.Status = status;
            this.IsFromNavis = isFromNavis;
        }

    // Navisworks objects initiation constructor
    public ASearchSet(SelectionSet selectionSet, bool isFromNavis)
    {
        this.IsFromNavis = isFromNavis;
        this.SelectionSet = selectionSet;
        this.Name = selectionSet.DisplayName;
        this.Status = EntityComparisonResult.NotChecked;
        this.Project = ViewsHandler.CurrentProject;
        this.ClashMatrix = ViewsHandler.CurrentAClashMatrix;
        this.ModifiedBy = ViewsHandler.CurrentUser.Name;
    }

}
}
