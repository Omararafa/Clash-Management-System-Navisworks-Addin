using System.Collections.Generic;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class AClashMatrix
    {
        public string Name { get; set; }
        public List<AClashTest> ClashTests { get; set; }
        public int Id { get; set; }
        public Project Project { get; set; }
    }
}
