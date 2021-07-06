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
        private List<Project> _projects;
        public List<Project> Projects
        {
            get
            {
                if (this._projects == null||this._projects.Count<=0)
                {
                    return _projects = DB.DBHandler.Projects;
                    
                }
                return this._projects;
            }
        }

        public User(string name, string domain)
        {
            this.Name = name;
            this.Domain = domain;
        }
    }
}
