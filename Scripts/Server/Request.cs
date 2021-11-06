using System.Collections.Generic;


namespace Co_Work.Network
{
    partial class Request
    {
        public partial class Login
        {
            public Login(string id, string password)
            {
                Id = id;
                Password = password;
            }

            public string Id { get; set; }
            public string Password { get; set; }
        }

        public partial class Register
        {
            public Register(string id, string password, string name, int age, string entryTime)
            {
                Id = id;
                Password = password;
                Name = name;
                Age = age;
                EntryTime = entryTime;
            }

            public string Id { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }

            public int Age { get; set; }
            public string EntryTime { get; set; }
        }

        public partial class GetFileInfo
        {
            public GetFileInfo(string projectGuid)
            {
                ProjectGuid = projectGuid;
            }

            public string ProjectGuid { get; set; }
        }

        public partial class CreatProject
        {
            public CreatProject(string projectName, string projectNote, double projectProcess, string startDate, string endDate,
                string creatorGuid, List<string> members)
            {
                ProjectName = projectName;
                ProjectNote = projectNote;
                ProjectProcess = projectProcess;
                StartDate = startDate;
                EndDate = endDate;
                CreatorGuid = creatorGuid;
                Members = members;
            }

            public string ProjectName { get; set; }
            public string ProjectNote { get; set; }
            public double ProjectProcess { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string CreatorGuid { get; set; }
            public List<string> Members { get; set; } //Guid
        }

        public partial class DeleteProject
        {
            public DeleteProject(string projectGuid, string deleterGuid)
            {
                ProjectGuid = projectGuid;
                DeleterGuid = deleterGuid;
            }

            public string ProjectGuid { get; set; }

            public string DeleterGuid { get; set; }
        }

        public partial class UpdateProject
        {
            public UpdateProject(string projectGuid, string projectName, string projectNote, double projectProcess,
                string startDate, string endDate,
                string updaterGuid, List<string> members)
            {
                ProjectGuid = projectGuid;
                ProjectName = projectName;
                ProjectNote = projectNote;
                ProjectProcess = projectProcess;
                StartDate = startDate;
                EndDate = endDate;
                UpdaterGuid = updaterGuid;
                Members = members;
            }

            public string ProjectGuid { get; set; }

            public string ProjectName { get; set; }
            public string ProjectNote { get; set; }
            public double ProjectProcess { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string UpdaterGuid { get; set; }
            public List<string> Members { get; set; } //Guid
        }

        public partial class GetEmployeesInfo
        {
            public GetEmployeesInfo(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }
        }

        public partial class GetEmployeeInfoFromGuid
        {
            public GetEmployeeInfoFromGuid(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }
        }

        public partial class GetEmployeeInfoFromId
        {
            public GetEmployeeInfoFromId(string employeeId)
            {
                EmployeeId = employeeId;
            }

            public string EmployeeId { get; set; }
        }

        public partial class GetProjectsInfoFromEmployee
        {
            public GetProjectsInfoFromEmployee(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }
        }

        public partial class GetProjectsInfo
        {
            public GetProjectsInfo(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }
        }


        public partial class GetProjectInfo
        {
            public GetProjectInfo(string projectGuid)
            {
                ProjectGuid = projectGuid;
            }

            public string ProjectGuid { get; set; }
        }
    }
}