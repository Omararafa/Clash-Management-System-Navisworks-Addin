using System;
using System.Collections.Generic;

namespace Clash_Management_System_Navisworks_Addin.ViewModels
{
    public class User
    {
        public string Name { get; set; }

        public string Domain { get; set; }
        public string EmployeeId { get; set; }

        public int ProjectCode { get; set; }

        public int RoleId { get; set; }

        public string TradeAbb
        {
            get
            {
                return DB.DBHandler.TradeAbb;
            }
        }
        private List<Project> _projects;
        public List<Project> Projects
        {
            get
            {
                if (this._projects == null || this._projects.Count <= 0)
                {
                    try
                    {
                        return _projects = DB.DBHandler.Projects;

                    }
                    catch (Exception)
                    {

                        return null;
                    }

                }
                return this._projects;
            }
        }
        public User(string employeeId, int projectCode = -1, int roleId = -1)
        {
            this.EmployeeId = employeeId;
        }
        public User(string name, string domain)
        {
            this.Name = name;
            this.Domain = domain;
        }
    }
}
