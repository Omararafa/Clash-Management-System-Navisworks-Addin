using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Clash;
using Clash_Management_System_Navisworks_Addin.Views;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class AClashTestResult
    {
        public AClashTest ClashTest { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedTime { get; set; }
        public string AssignedTo { get; set; }
        public string CreatedTime { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public string Distance { get; set; }
        public ClashPoint ClashPoint { get; set; }
        public string Item1Name { get; set; }
        public string Item1SourceFile { get; set; }
        public string Item2Name { get; set; }
        public string Item2SourceFile { get; set; }

        public AClashTestResult(ClashResult clashResult, AClashTest aClashTest)
        {
            this.ClashTest = aClashTest;
            this.Guid = clashResult.Guid;
            this.Name = clashResult.DisplayName;
            this.State = GetState(clashResult.Status);
            this.ApprovedBy = clashResult.ApprovedBy;
            this.ApprovedTime = clashResult.ApprovedTime.ToString();
            this.AssignedTo = clashResult.AssignedTo;
            this.CreatedTime = clashResult.CreatedTime.ToString();
            this.Description = clashResult.Description;
            this.Comments = GetClashResultComments(clashResult.Comments);
            this.Distance = clashResult.Distance.ToString();
            this.ClashPoint = new ClashPoint(clashResult.Center);
            this.Item1Name = clashResult.Item1.DisplayName;
            this.Item1SourceFile = GetItemSourceFile(clashResult.Item1);
            this.Item2Name = clashResult.Item2.DisplayName;
            this.Item2SourceFile = GetItemSourceFile(clashResult.Item2);
        }

        private static string GetState(ClashResultStatus clashResultStatus)
        {
            switch (clashResultStatus)
            {
                case ClashResultStatus.New:
                    return "New";
                case ClashResultStatus.Active:
                    return "Active";
                case ClashResultStatus.Reviewed:
                    return "Reviewed";
                case ClashResultStatus.Approved:
                    return "Approved";
                default:
                    return "Resolved";
            }
        }

        private static string GetClashResultComments(CommentCollection commentsCollection)
        {
            string comments = string.Empty;

            foreach (Comment comment in commentsCollection)
            {
                comments += comment.Body + ", ";
            }

            return comments;
        }

        private static string GetItemSourceFile(ModelItem item)
        {
            DataProperty property = item.PropertyCategories.FindCategoryByDisplayName("Item").Properties.FindPropertyByDisplayName("Source File");
            string sourceFile = property.Value.IsDisplayString ? property.Value.ToDisplayString() : property.Value.ToString();

            return sourceFile;
        }
    }
}
