using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Co_Work.Network
{
    class Request
    {
        public class Login
        {
            public Login(string id, string password)
            {
                ID = id;
                Password = password;
            }

            public string ID { get; set; }
            public string Password { get; set; }
        }

        public class Register
        {
            public Register(string id, string password, string name, int age, string entryTime)
            {
                ID = id;
                Password = password;
                Name = name;
                Age = age;
                EntryTime = entryTime;
            }

            public string ID { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }

            public int Age { get; set; }
            public string EntryTime { get; set; }
        }

        public class GetFileInfo
        {
            public GetFileInfo(string projectGuid)
            {
                ProjectGuid = projectGuid;
            }

            public string ProjectGuid { get; set; }
        }

        public class CreatProject
        {
            public CreatProject(string projectName, string projectNote, double projectProcess, string startDate,
                string creatorGuid, List<string> members)
            {
                ProjectName = projectName;
                ProjectNote = projectNote;
                ProjectProcess = projectProcess;
                StartDate = startDate;
                CreatorGuid = creatorGuid;
                Members = members;
            }

            public string ProjectName { get; set; }
            public string ProjectNote { get; set; }
            public double ProjectProcess { get; set; }
            public string StartDate { get; set; }
            public string CreatorGuid { get; set; }
            public List<string> Members { get; set; } //Guid
            
        }

        public class DeleteProject
        {
            public DeleteProject(string projectGuid, string deleterGuid)
            {
                ProjectGuid = projectGuid;
                DeleterGuid = deleterGuid;
            }

            public string ProjectGuid { get; set; }

            public string DeleterGuid { get; set; }
        }

        public class UpdateProject
        {
            public UpdateProject(string projectGuid, string projectName, string projectNote, double projectProcess,
                string startDate,
                string updaterGuid, List<string> members)
            {
                ProjectGuid = projectGuid;
                ProjectName = projectName;
                ProjectNote = projectNote;
                ProjectProcess = projectProcess;
                StartDate = startDate;
                UpdaterGuid = updaterGuid;
                Members = members;
            }

            public string ProjectGuid { get; set; }

            public string ProjectName { get; set; }
            public string ProjectNote { get; set; }
            public double ProjectProcess { get; set; }
            public string StartDate { get; set; }
            public string UpdaterGuid { get; set; }
            public List<string> Members { get; set; } //Guid
        }

        public class GetEmployeesInfo
        {
            public GetEmployeesInfo(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }
        }

        public class GetEmployeeInfo
        {
            public GetEmployeeInfo(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }
        }

        public class GetProjectsInfoFromEmployee
        {
            public GetProjectsInfoFromEmployee(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }
        }

        public class GetProjectsInfo
        {
            public GetProjectsInfo(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }
        }


        public class GetProjectInfo
        {
            public GetProjectInfo(string projectGuid)
            {
                ProjectGuid = projectGuid;
            }

            public string ProjectGuid { get; set; }
        }
    }
}