using Autodesk.Navisworks.Api;
using Clash_Management_System_Navisworks_Addin.Views;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class ASearchSet
    {
        public int Pk { get; set; }
        public int TradeId { get; set; }
        public string TradeAbb { get; set; }
        public Project Project { get; set; }
        public bool IsFromNavis { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public string DBMessage { get; set; }
        public int ClashMatrixId { get; set; }
        public AClashMatrix ClashMatrix { get; set; }
        public SelectionSet SelectionSet { get; set; }
        public EntityComparisonResult Conditon { get; set; }

        public ASearchSet()
        {

        }

        // Database initiation constructor
        public ASearchSet(string name, int clashMatrixId, string dbMessage,string tradeAbb, EntityComparisonResult condition, bool isFromNavis)
        {
            this.Name = name;
            this.ClashMatrixId = clashMatrixId;
            this.DBMessage = dbMessage;
            this.Conditon = condition;
            this.IsFromNavis = isFromNavis;
            this.TradeAbb=tradeAbb;
        }

        // Navisworks objects initiation constructor
        public ASearchSet(SelectionSet selectionSet, bool isFromNavis)
        {
            this.IsFromNavis = isFromNavis;
            this.SelectionSet = selectionSet;
            this.Name = selectionSet.DisplayName;
            this.Conditon = EntityComparisonResult.NotChecked;
            this.Project = ViewsHandler.CurrentProject;
            this.ClashMatrix = ViewsHandler.CurrentAClashMatrix;
            this.ModifiedBy = ViewsHandler.CurrentUser.Name;
        }

        public static string GetHeaders()
        {
            string[] headers = {"Name", "Project", "Clash Matrix", "Trade Id", "Modified By",  "Condition"};

            return string.Join(",", headers);
        }

    }
}
