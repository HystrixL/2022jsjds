using System.Collections.Generic;
using Co_Work.Core;
using Co_Work.Core.Project;
using Co_Work.Local;

namespace Co_Work.Network
{
    public class Response
    {
        public class Login
        {
            public enum LoginResultEnum
            {
                Succeed,
                UnknownAccount,
                WrongPassword,
                InvalidValues
            }

            public Login(LoginResultEnum loginResult, Employee employee, Ftp ftp)
            {
                LoginResult = loginResult;
                Employee = employee;
                Ftp = ftp;
            }

            public LoginResultEnum LoginResult { get; set; }
            public Employee Employee { get; set; }

            public Ftp Ftp { get; set; }
        }

        public class Register
        {
            public enum RegisterResultEnum
            {
                Succeed,
                IdAlreadyExists,
                InvalidValues
            }

            public Register(RegisterResultEnum registerResult, Employee employee)
            {
                RegisterResult = registerResult;
                Employee = employee;
            }

            public RegisterResultEnum RegisterResult { get; set; }
            public Employee Employee { get; set; }
        }

        public class GetFileInfo
        {
            public enum GetFileInfoResultEnum
            {
                Succeed,
                UnknownProject,
                InvalidValues,
            }

            public GetFileInfo(GetFileInfoResultEnum creatProjectResult, List<ProjectFile> projectFiles)
            {
                CreatProjectResult = creatProjectResult;
                ProjectFiles = projectFiles;
            }

            public GetFileInfoResultEnum CreatProjectResult { get; set; }
            public List<ProjectFile> ProjectFiles { get; set; }
        }

        public class CreatProject
        {
            public enum CreatProjectResultEnum
            {
                Succeed,
                PermissionDenied,
                InvalidValues,
            }

            public CreatProject(CreatProjectResultEnum creatProjectResult, Project project)
            {
                CreatProjectResult = creatProjectResult;
                Project = project;
            }

            public CreatProjectResultEnum CreatProjectResult { get; set; }
            public Project Project { get; set; }
        }

        public class DeleteProject
        {
            public enum DeleteProjectResultEnum
            {
                Succeed,
                PermissionDenied,
                UnknownProject,
                InvalidValues
            }

            public DeleteProject(DeleteProjectResultEnum deleteProjectResult)
            {
                DeleteProjectResult = deleteProjectResult;
            }

            public DeleteProjectResultEnum DeleteProjectResult { get; set; }
        }

        public class UpdateProject
        {
            public enum UpdateProjectResultEnum
            {
                Succeed,
                PermissionDenied,
                UnknownProject,
                InvalidValues
            }

            public UpdateProject(UpdateProjectResultEnum deleteProjectResult, Project project)
            {
                DeleteProjectResult = deleteProjectResult;
                Project = project;
            }

            public UpdateProjectResultEnum DeleteProjectResult { get; set; }
            public Project Project { get; set; }
        }

        public class GetEmployeesInfo
        {
            public enum GetEmployeesInfoResultEnum
            {
                Succeed,
                PermissionDenied,
            }

            public GetEmployeesInfo(GetEmployeesInfoResultEnum getEmployeesInfoResult, List<Employee> employees)
            {
                GetEmployeesInfoResult = getEmployeesInfoResult;
                Employees = employees;
            }

            public GetEmployeesInfoResultEnum GetEmployeesInfoResult { get; set; }
            public List<Employee> Employees { get; set; }
        }

        public class GetEmployeeInfoFromGuid
        {
            public enum GetEmployeeInfoFromGuidResultEnum
            {
                Succeed,
                UnknownEmployee,
            }

            public GetEmployeeInfoFromGuid(GetEmployeeInfoFromGuidResultEnum getEmployeeInfoFromGuidResult, Employee employee)
            {
                GetEmployeeInfoFromGuidResult = getEmployeeInfoFromGuidResult;
                Employee = employee;
            }

            public GetEmployeeInfoFromGuidResultEnum GetEmployeeInfoFromGuidResult { get; set; }
            public Employee Employee { get; set; }
        }

        public class GetEmployeeInfoFromId
        {
            public enum GetEmployeeInfoFromIdEnum
            {
                Succeed,
                UnknownEmployee,
            }

            public GetEmployeeInfoFromId(GetEmployeeInfoFromIdEnum getEmployeeInfoFromIdEnumResult, Employee employee)
            {
                GetEmployeeInfoFromIdEnumResult = getEmployeeInfoFromIdEnumResult;
                Employee = employee;
            }

            public GetEmployeeInfoFromIdEnum GetEmployeeInfoFromIdEnumResult { get; set; }
            public Employee Employee { get; set; }
        }

        public class GetProjectsInfoFromEmployee
        {
            public enum GetProjectsInfoFromEmployeeResultEnum
            {
                Succeed,
                UnknownEmployee,
            }

            public GetProjectsInfoFromEmployee(GetProjectsInfoFromEmployeeResultEnum getProjectsInfoFromEmployeeResult, List<Project> projects)
            {
                GetProjectsInfoFromEmployeeResult = getProjectsInfoFromEmployeeResult;
                Projects = projects;
            }

            public GetProjectsInfoFromEmployeeResultEnum GetProjectsInfoFromEmployeeResult { get; set; }
            public List<Project> Projects { get; set; }
        }

        public class GetProjectsInfo
        {
            public enum GetProjectsInfoResultEnum
            {
                Succeed,
                PermissionDenied,
            }

            public GetProjectsInfo(GetProjectsInfoResultEnum getProjectsInfoResult, List<Project> projects)
            {
                GetProjectsInfoResult = getProjectsInfoResult;
                Projects = projects;
            }

            public GetProjectsInfoResultEnum GetProjectsInfoResult { get; set; }
            public List<Project> Projects { get; set; }
        }

        public class GetProjectInfo
        {
            public enum GetProjectInfoResultEnum
            {
                Succeed,
                UnknownProject,
            }

            public GetProjectInfo(GetProjectInfoResultEnum getProjectInfoResult, Project project)
            {
                GetProjectInfoResult = getProjectInfoResult;
                Project = project;
            }

            public GetProjectInfoResultEnum GetProjectInfoResult { get; set; }
            public Project Project { get; set; }
        }
    }
}