using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class Project
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int Id { get; set; }

        public List<AClashMatrix> ClashMatrices { get; set; }
    }
}
