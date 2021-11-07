using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Co_Work.Core;
using Co_Work.Core.Employee;
using Co_Work.Core.Project;
using Co_Work.Local;
using Co_Work.Local.ProjectFile;
using Co_Work.Network.TCP;
using Microsoft.EntityFrameworkCore.Query;

namespace Co_Work.Network
{
    partial class Request
    {
        public partial class Login
        {
            public static void Parse(TransData<Request.Login> transData, Client client)
            {
                var loginResult =
                    AccountManager.TryLogin(transData.Content.Id, transData.Content.Password);
                Employee employee;
                if (loginResult == Response.Login.LoginResultEnum.Succeed)
                {
                    employee = EmployeeManager.GetEmployeeFromId(transData.Content.Id);
                }
                else
                {
                    employee = null;
                }

                var loginResponse = new Response.Login(loginResult, employee);
                var loginTransData =
                    new TransData<Response.Login>(loginResponse, client.ClientGuid, transData.Guid);
                client.SendResponse(loginTransData.ToString());
            }
        }
    }

    partial class Request
    {
        public partial class Register
        {
            public static void Parse(TransData<Request.Register> transData, Client client)
            {
                var registerResult =
                    AccountManager.TryRegister(transData.Content.Id, transData.Content.Password, transData.Content.Name,
                        transData.Content.Age, transData.Content.EntryTime);
                Employee employee;
                if (registerResult == Response.Register.RegisterResultEnum.Succeed)
                {
                    employee = EmployeeManager.GetEmployeeFromId(transData.Content.Id);
                }
                else
                {
                    employee = null;
                }

                DataBaseManager.SaveChange();
                var registerResponse = new Response.Register(registerResult, employee);
                var registerTransData =
                    new TransData<Response.Register>(registerResponse, client.ClientGuid, transData.Guid);
                client.SendResponse(registerTransData.ToString());
            }
        }
    }

    partial class Request
    {
        public partial class GetFileInfo
        {
            public static void Parse(TransData<Request.GetFileInfo> transData, Client client)
            {
                var getFileInfoResult =
                    Response.GetFileInfo.GetFileInfoResultEnum.UnknownProject;
                List<ProjectFile> projectFiles = null;
                if (ProjectManager.GetProjectFromGuid(transData.Content.ProjectGuid) != null)
                {
                    projectFiles = new FileManager(transData.Content.ProjectGuid).GetProjectFilesList();
                    getFileInfoResult = Response.GetFileInfo.GetFileInfoResultEnum.Succeed;
                }

                var getFileInfoResponse = new Response.GetFileInfo(getFileInfoResult, projectFiles);
                var getFileInfoTransData =
                    new TransData<Response.GetFileInfo>(getFileInfoResponse, client.ClientGuid, transData.Guid);
                client.SendResponse(getFileInfoTransData.ToString());
            }
        }
    }

    partial class Request
    {
        public partial class CreatProject
        {
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

                    var members = transData.Content.Members.Select(member => EmployeeManager.GetEmployeeFromGuid(member)).ToList();

                    creatProjectResult = Response.CreatProject.CreatProjectResultEnum.Succeed;
                    project = new Project
                    {
                        Name = transData.Content.ProjectName,
                        StartDate = DateTime.ParseExact(transData.Content.StartDate, "yyyy-MM-dd",
                            CultureInfo.CurrentCulture),
                        
                        Creator = creator,
                        Members = members,
                        Note = transData.Content.ProjectNote,
                        ProgressRate = transData.Content.ProjectProcess
                    };
                    if (transData.Content.EndDate != "")
                    {
                        project.EndDate = DateTime.ParseExact(transData.Content.EndDate, "yyyy-MM-dd",
                            CultureInfo.CurrentCulture);
                    }

                    ProjectManager.Projects.Add(project);
                    DataBaseManager._dataContext.Add(project);
                }

                DataBaseManager.SaveChange();
                var creatProjectResponse = new Response.CreatProject(creatProjectResult, project);

                var creatProjectTransData =
                    new TransData<Response.CreatProject>(creatProjectResponse, client.ClientGuid, transData.Guid);

                client.SendResponse(creatProjectTransData.ToString());
            }
        }
    }

    partial class Request
    {
        public partial class DeleteProject
        {
            public static void Parse(TransData<Request.DeleteProject> transData, Client client)
            {
                Response.DeleteProject.DeleteProjectResultEnum deleteProjectResult;
                var project = ProjectManager.GetProjectFromGuid(transData.Content.ProjectGuid);
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
                        DataBaseManager._dataContext.Remove(project);
                        deleteProjectResult = Response.DeleteProject.DeleteProjectResultEnum.Succeed;
                    }
                    else
                    {
                        deleteProjectResult = Response.DeleteProject.DeleteProjectResultEnum.PermissionDenied;
                    }
                }

                DataBaseManager.SaveChange();
                var deleteProjectResponse = new Response.DeleteProject(deleteProjectResult);
                var deleteProjectTransData =
                    new TransData<Response.DeleteProject>(deleteProjectResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(deleteProjectTransData.ToString());
            }
        }
    }

    partial class Request
    {
        public partial class UpdateProject
        {
            public static void Parse(TransData<Request.UpdateProject> transData, Client client)
            {
                Response.UpdateProject.UpdateProjectResultEnum updateProjectResult;
                var project = ProjectManager.GetProjectFromGuid(transData.Content.ProjectGuid);
                var updater = EmployeeManager.GetEmployeeFromGuid(transData.Content.UpdaterGuid);
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
                        project.StartDate = DateTime.ParseExact(transData.Content.StartDate, "yyyy-MM-dd",
                            CultureInfo.CurrentCulture);
                        if (transData.Content.EndDate != "")
                        {
                            project.EndDate = DateTime.ParseExact(transData.Content.EndDate, "yyyy-MM-dd",
                                CultureInfo.CurrentCulture);
                        }

                        if (!transData.Content.Members.Contains(updater.GUID))
                        {
                            transData.Content.Members.Add(updater.GUID);
                        }

                        var members = transData.Content.Members.Select(member => EmployeeManager.GetEmployeeFromGuid(member)).ToList();

                        project.Members = members;
                        updateProjectResult = Response.UpdateProject.UpdateProjectResultEnum.Succeed;
                    }
                    else
                    {
                        updateProjectResult = Response.UpdateProject.UpdateProjectResultEnum.PermissionDenied;
                    }
                }

                DataBaseManager.SaveChange();
                var updateProjectResponse = new Response.UpdateProject(updateProjectResult, project);
                var updateProjectTransData =
                    new TransData<Response.UpdateProject>(updateProjectResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(updateProjectTransData.ToString());
            }
        }
    }

    partial class Request
    {
        public partial class GetEmployeesInfo
        {
            public static void Parse(TransData<Request.GetEmployeesInfo> transData, Client client)
            {
                var getEmployeesInfoResult =
                    Response.GetEmployeesInfo.GetEmployeesInfoResultEnum.Succeed;
                var employees = EmployeeManager.Employees;

                var getEmployeesInfoResponse =
                    new Response.GetEmployeesInfo(getEmployeesInfoResult, employees);
                var getEmployeesInfoTransData =
                    new TransData<Response.GetEmployeesInfo>(getEmployeesInfoResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getEmployeesInfoTransData.ToString());
            }
        }
    }

    partial class Request
    {
        public partial class GetEmployeeInfoFromGuid
        {
            public static void Parse(TransData<Request.GetEmployeeInfoFromGuid> transData, Client client)
            {
                Response.GetEmployeeInfoFromGuid.GetEmployeeInfoFromGuidResultEnum getEmployeeInfoFromGuidResult;
                var employee = EmployeeManager.GetEmployeeFromGuid(transData.Content.EmployeeGuid);
                if (employee == null)
                {
                    getEmployeeInfoFromGuidResult = Response.GetEmployeeInfoFromGuid.GetEmployeeInfoFromGuidResultEnum.UnknownEmployee;
                }
                else
                {
                    getEmployeeInfoFromGuidResult = Response.GetEmployeeInfoFromGuid.GetEmployeeInfoFromGuidResultEnum.Succeed;
                }

                var getEmployeeInfoFromGuidResponse =
                    new Response.GetEmployeeInfoFromGuid(getEmployeeInfoFromGuidResult, employee);
                var getEmployeeInfoFromGuidTransData =
                    new TransData<Response.GetEmployeeInfoFromGuid>(getEmployeeInfoFromGuidResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getEmployeeInfoFromGuidTransData.ToString());
            }
        }
    }

    partial class Request
    {
        public partial class GetEmployeeInfoFromId
        {
            public static void Parse(TransData<Request.GetEmployeeInfoFromId> transData, Client client)
            {
                Response.GetEmployeeInfoFromId.GetEmployeeInfoFromIdEnum getEmployeeInfoFromIdResult;
                var employee = EmployeeManager.GetEmployeeFromId(transData.Content.EmployeeId);
                if (employee == null)
                {
                    getEmployeeInfoFromIdResult = Response.GetEmployeeInfoFromId.GetEmployeeInfoFromIdEnum.UnknownEmployee;
                }
                else
                {
                    getEmployeeInfoFromIdResult = Response.GetEmployeeInfoFromId.GetEmployeeInfoFromIdEnum.Succeed;
                }

                var getEmployeeInfoFromIdResponse =
                    new Response.GetEmployeeInfoFromId(getEmployeeInfoFromIdResult, employee);
                var getEmployeeInfoFromIdTransData =
                    new TransData<Response.GetEmployeeInfoFromId>(getEmployeeInfoFromIdResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getEmployeeInfoFromIdTransData.ToString());
            }
        }

}
    
    
    
    partial class Request
    {
        public partial class GetProjectsInfoFromEmployee
        {
            public static void Parse(TransData<Request.GetProjectsInfoFromEmployee> transData, Client client)
            {
                Response.GetProjectsInfoFromEmployee.GetProjectsInfoFromEmployeeResultEnum
                    getProjectsInfoFromEmployeeResult;
                var employee = EmployeeManager.GetEmployeeFromGuid(transData.Content.EmployeeGuid);
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

                var getProjectsInfoFromEmployeeResponse =
                    new Response.GetProjectsInfoFromEmployee(getProjectsInfoFromEmployeeResult, projects);
                var getProjectsInfoFromEmployeeTransData =
                    new TransData<Response.GetProjectsInfoFromEmployee>(getProjectsInfoFromEmployeeResponse,
                        client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getProjectsInfoFromEmployeeTransData.ToString());
            }
        }
    }

    partial class Request
    {
        public partial class GetProjectsInfo
        {
            public static void Parse(TransData<Request.GetProjectsInfo> transData, Client client)
            {
                var getProjectsInfoResult =
                    Response.GetProjectsInfo.GetProjectsInfoResultEnum.Succeed;
                var projects = ProjectManager.Projects;

                var getProjectsInfoResponse =
                    new Response.GetProjectsInfo(getProjectsInfoResult, projects);
                var getProjectsInfoTransData =
                    new TransData<Response.GetProjectsInfo>(getProjectsInfoResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getProjectsInfoTransData.ToString());
            }
        }
    }

    partial class Request
    {
        public partial class GetProjectInfo
        {
            public static void Parse(TransData<Request.GetProjectInfo> transData, Client client)
            {
                Response.GetProjectInfo.GetProjectInfoResultEnum getProjectInfoResult;
                var project = ProjectManager.GetProjectFromGuid(transData.Content.ProjectGuid);
                if (project != null)
                {
                    getProjectInfoResult = Response.GetProjectInfo.GetProjectInfoResultEnum.UnknownProject;
                }
                else
                {
                    getProjectInfoResult = Response.GetProjectInfo.GetProjectInfoResultEnum.Succeed;
                }

                var getProjectInfoResponse =
                    new Response.GetProjectInfo(getProjectInfoResult, project);
                var getProjectInfoTransData =
                    new TransData<Response.GetProjectInfo>(getProjectInfoResponse, client.ClientGuid,
                        transData.Guid);

                client.SendResponse(getProjectInfoTransData.ToString());
            }
        }
    }
}