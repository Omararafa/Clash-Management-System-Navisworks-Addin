using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using System.Collections.Generic;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class ASearchSet
    {
        public int Pk { get; set; }
        public int TradeId { get; set; }
        public Project Project { get; set; }
        public bool IsFromNavis { get; set; }
        public string ModifiedBy { get; set; }
        public string SearchSetName { get; set; }
        public AClashMatrix clashMatrix { get; set; }
        public SelectionSet selectionSet { get; set; }
        public EntityComparisonResult Status { get; set; }

        // Database intiation constructor
        public ASearchSet(int pk, string searchSet, int tradeId, Project project, string modefiedBy, bool isFromNavis)
        {
            this.Pk = pk;
            this.TradeId = tradeId;
            this.Project = project;
            this.ModifiedBy = modefiedBy;
            this.IsFromNavis = isFromNavis;
            this.SearchSetName = searchSet;
        }

        // Navisworks objects intiation constructor
        public ASearchSet(SelectionSet selectionSet, Project project, string modefiedBy, AClashMatrix aClashMatrix, bool isFromNavis)
        {
            this.Project = project;
            this.ModifiedBy = modefiedBy;
            this.IsFromNavis = isFromNavis;
            this.clashMatrix = aClashMatrix;
            this.selectionSet = selectionSet;
            this.SearchSetName = selectionSet.DisplayName;
            this.Status = EntityComparisonResult.NotChecked;
        }

    }
}
