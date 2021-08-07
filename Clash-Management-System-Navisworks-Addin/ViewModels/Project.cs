using System.Collections.Generic;

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
