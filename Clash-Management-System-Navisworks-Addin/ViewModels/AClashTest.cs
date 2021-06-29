﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class AClashTest
    {
        public string Name { get; set; }
        public EntityComparisonResult Status { get; set; }

        public int Id { get; set; }

        public string UniqueName { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public int ClashMatrixId { get; set; }

        public int TradeId { get; set; }

        public string TradeCode { get; set; }

        public DateTime? AddedDate { get; set; }

        public DateTime? LastRunDate { get; set; }

        public string AddedBy { get; set; }

        public string ProjectCode { get; set; }

        public ASearchSet SearchSet1 { get; set; }

        public ASearchSet SearchSet2 { get; set; }


        public AClashTest(string name,
         EntityComparisonResult status,
         int id,
         string uniqueName,
         int type,
         string typeName,
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
            this.Status = status;
            this.Id = id;
            this.UniqueName = uniqueName;
            this.Type = type;
            this.TypeName = typeName;
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
    }
}
