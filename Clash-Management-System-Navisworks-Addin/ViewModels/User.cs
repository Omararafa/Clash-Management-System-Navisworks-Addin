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

        public string TradeAbb 
        { get
            {
                return DB.DBHandler.TradeAbb;
            }
        }

        public List<Project> Projects 
        {
            get
            {
                return DB.DBHandler.Projects;
            }
        }

        public User(string name, string domain)
        {
            this.Name = name;
            this.Domain = domain;
        }
    }
}
