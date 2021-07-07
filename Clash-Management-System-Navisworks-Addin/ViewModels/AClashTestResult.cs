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
        public EntityComparisonResult Condition { get; set; }

        /* The data type of the clash result representing in the api
            public Guid Guid { get; set; }
            public string Name { get; set; }
            public ClashResultStatus State { get; set; }
            public string ApprovedBy { get; set; }
            public DateTime? ApprovedTime { get; set; }
            public string AssignedTo { get; set; }
            public DateTime? CreatedTime { get; set; }
            public string Description { get; set; }
            public CommentCollection Comments { get; set; }
            public double Distance { get; set; }
            public Point3D ClashPoint { get; set; }
            public ModelItem Item1 { get; set; }
            public string Item1SourceFile { get; set; }
            public ModelItem Item2 { get; set; }
            public string Item2SourceFile { get; set; }
        */

        /* The data type of the clash result required for the db
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
            public string Item1 { get; set; }
            public string Item1SourceFile { get; set; }
            public string Item2 { get; set; }
            public string Item2SourceFile { get; set; }
        */

    }
}
