﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class AClashTestResult
    {
        public AClashTest ClashTest { get; set; }
        public EntityComparisonResult Status { get; set; }
    }
}
