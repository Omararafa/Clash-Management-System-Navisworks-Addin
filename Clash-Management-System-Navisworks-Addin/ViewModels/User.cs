using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class User
    {
        public string Name { get; set; }

        public string Domain { get; set; }

        public string TradeAbb { get; set; }

        public List<Project> Projects { get; set; }

        public User(string name, string domain)
        {
            this.Name = name;
            this.Domain = domain;
            this.Projects = DB.DBHandler.Projects;
            this.TradeAbb = DB.DBHandler.TradeAbb;
        }
    }
}
