using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Co_Work.Core;
using Co_Work.Core.Project;
using Co_Work.Local;
using Co_Work.Network.TCP;

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

            public static void Parse(TransData<Request.Login> transData, Client client)
            {
                Response.Login.LoginResultEnum loginResult =
                    AccountManager.TryLogin(transData.Content.ID, transData.Content.Password);
                Employee employee;
                if (loginResult == Response.Login.LoginResultEnum.Succeed)
                {
                    employee = EmployeeManager.GetEmployeeFromId(transData.Content.ID);
                }
                else
                {
                    employee = null;
                }

                Response.Login loginResponse = new Response.Login(loginResult, employee);
                TransData<Response.Login> loginTransData =
                    new TransData<Response.Login>(loginResponse, client.ClientGuid, transData.Guid);
                client.SendResponse(loginTransData.ToString());
            }
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

            public static void Parse(TransData<Request.Register> transData, Client client)
            {
                Response.Register.RegisterResultEnum registerResult =
                    AccountManager.TryRegister(transData.Content.ID, transData.Content.Password, transData.Content.Name,
                        transData.Content.Age, transData.Content.EntryTime);
                Employee employee;
                if (registerResult == Response.Register.RegisterResultEnum.Succeed)
                {
                    employee = EmployeeManager.GetEmployeeFromId(transData.Content.ID);
                }
                else
                {
                    employee = null;
                }

                DataBaseManager.SaveChange();
                Response.Register registerResponse = new Response.Register(registerResult, employee);
                TransData<Response.Register> registerTransData =
                    new TransData<Response.Register>(registerResponse, client.ClientGuid, transData.Guid);
                client.SendResponse(registerTransData.ToString());
            }
        }

        public class GetFileInfo
        {
            public GetFileInfo(string projectGuid)
            {
                ProjectGuid = projectGuid;
            }

            public string ProjectGuid { get; set; }

            public static void Parse(TransData<Request.GetFileInfo> transData, Client client)
            {
                Response.GetFileInfo.GetFileInfoResultEnum getFileInfoResult =
                    Response.GetFileInfo.GetFileInfoResultEnum.UnknownProject;
                List<ProjectFile> projectFiles = null;
                if (ProjectManager.GetProjectFromGuid(transData.Content.ProjectGuid) != null)
                {
                    projectFiles = new FileManager(transData.Content.ProjectGuid).GetProjectFilesList();
                    getFileInfoResult = Response.GetFileInfo.GetFileInfoResultEnum.Succeed;
                }

                Response.GetFileInfo getFileInfoResponse = new Response.GetFileInfo(getFileInfoResult, projectFiles);
                TransData<Response.GetFileInfo> getFileInfoTransData =
                    new TransData<Response.GetFileInfo>(getFileInfoResponse, client.ClientGuid, transData.Guid);
                client.SendResponse(getFileInfoTransData.ToString());
            }
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

            public static void Parse(TransData<Request.CreatProject> transData, Client client)
            {
                Response.CreatProject.CreatProjectResultEnum creatProjectResult;
                Project project;
                var creator = EmployeeManager.GetEmployeeFromGuid(transData.Content.CreatorGuid);
                if (creator.Level < Level.Admin && transData.Content.Members.Count > 1)
                {
                    creatProjectResult = Response.CreatProject.CreatProjectResultEnum.PermissionDenied;
                    project = null;
                }
                else
                {
                    if (!transData.Content.Members.Contains(creator.GUID))
                    {
                        transData.Content.Members.Add(creator.GUID);
                    }

                    List<Employee> members = new List<Employee>();
                    foreach (var member in transData.Content.Members)
                    {
                        members.Add(EmployeeManager.GetEmployeeFromGuid(member));
                    }

                    creatProjectResult = Response.CreatProject.CreatProjectResultEnum.Succeed;
                    project = new Project()
                    {
                        Name = transData.Content.ProjectName,
                        StartTime = DateTime.ParseExact(transData.Content.StartDate, "yyyy-MM-dd", CultureInfo.CurrentCulture),
                        Creator = creator,
                        Members = members
                    };
                    project.Note = transData.Content.ProjectNote;
                    project.ProgressRate = transData.Content.ProjectProcess;
                    
                    ProjectManager.Projects.Add(project);
                    DataBaseManager._dataContext.Add(project);
                }

                DataBaseManager.SaveChange();
                Response.CreatProject creatProjectResponse = new Response.CreatProject(creatProjectResult, project);

                TransData<Response.CreatProject> creatProjectTransData =
                    new TransData<Response.CreatProject>(creatProjectResponse, client.ClientGuid, transData.Guid);

                client.SendResponse(creatProjectTransData.ToString());
            }
        }

        public class DeleteProject
        {
            public DeleteProject(string projectGuid)
            {
                ProjectGuid = projectGuid;
            }

            public string ProjectGuid { get; set; }

            public string DeleterGuid { get; set; }

            public static void Parse(TransData<Request.DeleteProject> transData, Client client)
            {
                Response.DeleteProject.DeleteProjectResultEnum deleteProjectResult;
                Project project = ProjectManager.GetProjectFromGuid(transData.Content.ProjectGuid);
                if (project == null)
                {
                    deleteProjectResult = Response.DeleteProject.DeleteProjectResultEnum.UnknownProject;
                }
                else
                {
                    if (project.Creator.GUID == transData.Content.DeleterGuid ||
                        EmployeeManager.GetEmployeeFromGuid(transData.Content.DeleterGuid).Level > Level.Staff)
                    {
                        ProjectManager.Projects.Remove(project);
                        DataBaseManager._dataContext.Remove(transData.Content.ProjectGuid);
                        deleteProjectResult = Response.DeleteProject.DeleteProjectResultEnum.Succeed;
                    }
                    else
                    {
                        deleteProjectResult = Response.DeleteProject.DeleteProjectResultEnum.PermissionDenied;
                    }
                }

                DataBaseManager.SaveChange();
                Response.DeleteProject deleteProjectResponse = new Response.DeleteProject(deleteProjectResult);
                TransData<Response.DeleteProject> deleteProjectTransData =
                    new TransData<Response.DeleteProject>(deleteProjectResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(deleteProjectTransData.ToString());
            }
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

            public static void Parse(TransData<Request.UpdateProject> transData, Client client)
            {
                Response.UpdateProject.UpdateProjectResultEnum updateProjectResult;
                Project project = ProjectManager.GetProjectFromGuid(transData.Content.ProjectGuid);
                Employee updater = EmployeeManager.GetEmployeeFromGuid(transData.Content.UpdaterGuid);
                if (project == null)
                {
                    updateProjectResult = Response.UpdateProject.UpdateProjectResultEnum.UnknownProject;
                }
                else
                {
                    if (project.Creator.GUID == transData.Content.UpdaterGuid ||
                        EmployeeManager.GetEmployeeFromGuid(transData.Content.UpdaterGuid).Level > Level.Staff)
                    {
                        project.Name = transData.Content.ProjectName;
                        project.Note = transData.Content.ProjectNote;
                        project.ProgressRate = transData.Content.ProjectProcess;
                        project.StartTime = DateTime.ParseExact(transData.Content.StartDate, "yyyy-MM-dd", CultureInfo.CurrentCulture);

                        if (!transData.Content.Members.Contains(updater.GUID))
                        {
                            transData.Content.Members.Add(updater.GUID);
                        }

                        List<Employee> members = new List<Employee>();
                        foreach (var member in transData.Content.Members)
                        {
                            members.Add(EmployeeManager.GetEmployeeFromGuid(member));
                        }

                        project.Members = members;
                        updateProjectResult = Response.UpdateProject.UpdateProjectResultEnum.Succeed;
                    }
                    else
                    {
                        updateProjectResult = Response.UpdateProject.UpdateProjectResultEnum.PermissionDenied;
                    }
                }

                DataBaseManager.SaveChange();
                Response.UpdateProject updateProjectResponse = new Response.UpdateProject(updateProjectResult, project);
                TransData<Response.UpdateProject> updateProjectTransData =
                    new TransData<Response.UpdateProject>(updateProjectResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(updateProjectTransData.ToString());
            }
        }

        public class GetEmployeesInfo
        {
            public GetEmployeesInfo(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }

            public static void Parse(TransData<Request.GetEmployeesInfo> transData, Client client)
            {
                Response.GetEmployeesInfo.GetEmployeesInfoResultEnum getEmployeesInfoResult =
                    Response.GetEmployeesInfo.GetEmployeesInfoResultEnum.Succeed;
                List<Employee> employees = EmployeeManager.Employees;

                Response.GetEmployeesInfo getEmployeesInfoResponse =
                    new Response.GetEmployeesInfo(getEmployeesInfoResult, employees);
                TransData<Response.GetEmployeesInfo> getEmployeesInfoTransData =
                    new TransData<Response.GetEmployeesInfo>(getEmployeesInfoResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getEmployeesInfoTransData.ToString());
            }
        }

        public class GetEmployeeInfo
        {
            public GetEmployeeInfo(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }

            public static void Parse(TransData<Request.GetEmployeeInfo> transData, Client client)
            {
                Response.GetEmployeeInfo.GetEmployeeInfoResultEnum getEmployeeInfoResult;
                Employee employee = EmployeeManager.GetEmployeeFromGuid(transData.Content.EmployeeGuid);
                if (employee != null)
                {
                    getEmployeeInfoResult = Response.GetEmployeeInfo.GetEmployeeInfoResultEnum.UnknownEmployee;
                }
                else
                {
                    getEmployeeInfoResult = Response.GetEmployeeInfo.GetEmployeeInfoResultEnum.Succeed;
                }

                Response.GetEmployeeInfo getEmployeeInfoResponse =
                    new Response.GetEmployeeInfo(getEmployeeInfoResult, employee);
                TransData<Response.GetEmployeeInfo> getEmployeeInfoTransData =
                    new TransData<Response.GetEmployeeInfo>(getEmployeeInfoResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getEmployeeInfoTransData.ToString());
            }
        }

        public class GetProjectsInfoFromEmployee
        {
            public GetProjectsInfoFromEmployee(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }

            public static void Parse(TransData<Request.GetProjectsInfoFromEmployee> transData, Client client)
            {
                Response.GetProjectsInfoFromEmployee.GetProjectsInfoFromEmployeeResultEnum
                    getProjectsInfoFromEmployeeResult;
                Employee employee = EmployeeManager.GetEmployeeFromGuid(transData.Content.EmployeeGuid);
                List<Project> projects;
                if (employee == null)
                {
                    projects = null;
                    getProjectsInfoFromEmployeeResult = Response.GetProjectsInfoFromEmployee
                        .GetProjectsInfoFromEmployeeResultEnum.UnknownEmployee;
                }
                else
                {
                    projects = ProjectManager.Projects.Where(project =>
                            project.Creator.GUID == employee.GUID || project.Members.Any(e => e.GUID == employee.GUID))
                        .ToList();

                    getProjectsInfoFromEmployeeResult = Response.GetProjectsInfoFromEmployee
                        .GetProjectsInfoFromEmployeeResultEnum.Succeed;
                }

                Response.GetProjectsInfoFromEmployee getProjectsInfoFromEmployeeResponse =
                    new Response.GetProjectsInfoFromEmployee(getProjectsInfoFromEmployeeResult, projects);
                TransData<Response.GetProjectsInfoFromEmployee> getProjectsInfoFromEmployeeTransData =
                    new TransData<Response.GetProjectsInfoFromEmployee>(getProjectsInfoFromEmployeeResponse,
                        client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getProjectsInfoFromEmployeeTransData.ToString());
            }
        }

        public class GetProjectsInfo
        {
            public GetProjectsInfo(string employeeGuid)
            {
                EmployeeGuid = employeeGuid;
            }

            public string EmployeeGuid { get; set; }

            public static void Parse(TransData<Request.GetProjectsInfo> transData, Client client)
            {
                Response.GetProjectsInfo.GetProjectsInfoResultEnum getProjectsInfoResult =
                    Response.GetProjectsInfo.GetProjectsInfoResultEnum.Succeed;
                List<Project> projects = ProjectManager.Projects;

                Response.GetProjectsInfo getProjectsInfoResponse =
                    new Response.GetProjectsInfo(getProjectsInfoResult, projects);
                TransData<Response.GetProjectsInfo> getProjectsInfoTransData =
                    new TransData<Response.GetProjectsInfo>(getProjectsInfoResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getProjectsInfoTransData.ToString());
            }
        }


        public class GetProjectInfo
        {
            public GetProjectInfo(string projectGuid)
            {
                ProjectGuid = projectGuid;
            }

            public string ProjectGuid { get; set; }

            public static void Parse(TransData<Request.GetProjectInfo> transData, Client client)
            {
                Response.GetProjectInfo.GetProjectInfoResultEnum getProjectInfoResult;
                Project project = ProjectManager.GetProjectFromGuid(transData.Content.ProjectGuid);
                if (project != null)
                {
                    getProjectInfoResult = Response.GetProjectInfo.GetProjectInfoResultEnum.UnknownProject;
                }
                else
                {
                    getProjectInfoResult = Response.GetProjectInfo.GetProjectInfoResultEnum.Succeed;
                }

                Response.GetProjectInfo getProjectInfoResponse =
                    new Response.GetProjectInfo(getProjectInfoResult, project);
                TransData<Response.GetProjectInfo> getProjectInfoTransData =
                    new TransData<Response.GetProjectInfo>(getProjectInfoResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getProjectInfoTransData.ToString());
            }
        }
    }
}