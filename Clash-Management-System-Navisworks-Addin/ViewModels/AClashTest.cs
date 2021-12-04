﻿using System;
using System.Collections.Generic;
using Autodesk.Navisworks.Api.Clash;
using Clash_Management_System_Navisworks_Addin.Views;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class AClashTest
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public EntityComparisonResult Condition { get; set; }
        public int Id { get; set; }
        public string UniqueName { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public double Tolerance { get; set; }
        public int ClashMatrixId { get; set; }
        public int TradeId { get; set; }
        public string TradeCode { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? LastRunDate { get; set; }
        public string AddedBy { get; set; }
        public string ProjectCode { get; set; }
        public string Priority { get; set; }
        public ASearchSet SearchSet1 { get; set; }
        public ASearchSet SearchSet2 { get; set; }
        public ClashTest ClashTest { get; set; }
        public bool IsFromNavis { get; set; }
        public List<AClashTestResult> AClashTestResults { get; set; }
        public AClashTest()
        {

        }

        public AClashTest(string name,
         EntityComparisonResult condition,
         int id,
         string uniqueName,
         int type,
         string typeName,
         double tolerance,
         int clashMatrixId,
         int tradeId,
         string tradeCode,
         DateTime? addedDate,
         DateTime? lastRunDate,
         string addedBy,
         string projectCode,
         ASearchSet searchSet1,
         ASearchSet searchSet2)
        {
            this.Name = name;
            this.Condition = condition;
            this.Id = id;
            this.UniqueName = uniqueName;
            this.Type = type;
            this.TypeName = typeName;
            this.Tolerance = tolerance;
            this.ClashMatrixId = clashMatrixId;
            this.TradeId = tradeId;
            this.TradeCode = tradeCode;
            this.AddedDate = addedDate;
            this.LastRunDate = lastRunDate;
            this.AddedBy = addedBy;
            this.ProjectCode = projectCode;
            this.SearchSet1 = searchSet1;
            this.SearchSet2 = searchSet2;
        }
        public AClashTest(string name,
         EntityComparisonResult condition,
         string typeName,
         double tolerance,
         bool isFromNavis,
         ClashTest clashTest,
         ASearchSet searchSet1,
         ASearchSet searchSet2)
        {
            this.Name = name;
            this.Condition = condition;
            this.TypeName = typeName;
            this.Tolerance = tolerance;
            this.ClashMatrixId = ViewsHandler.CurrentAClashMatrix.Id;
            this.ProjectCode = ViewsHandler.CurrentProject.Code;
            this.ClashTest = clashTest;
            this.SearchSet1 = searchSet1;
            this.SearchSet2 = searchSet2;
            this.IsFromNavis = isFromNavis;
            this.TypeName = clashTest.TestType.ToString();
        }


        //TODO Deploy: change report headers as per company requirements
        public static string GetReportHeaders()
        {
            string[] headers = { "Name", "Status", "Search Set 1", "Search Set 2", "Clash Type", "Tolerance", "Priority" };

            return string.Join(",", headers);
        }
    }
}
