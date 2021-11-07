using System;
using System.Linq;
using Co_Work.Core;
using Co_Work.Core.Employee;
using Co_Work.Core.Project;

namespace Co_Work.Local
{
    public static class DataBaseManager
    {
        public static DataContext _dataContext;
        
        public static void Init(string dbPath = "CoWork.db")
        {
            _dataContext = new DataContext(dbPath);
            _dataContext.Database.EnsureCreated();
            AccountManager.Accounts = _dataContext.AccountDataBase.ToList();
            EmployeeManager.Employees = _dataContext.EmployeeDataBase.ToList();
            ProjectManager.Projects = _dataContext.ProjectDataBase.ToList();
            Console.WriteLine("数据库初始化已完成...");
            SaveChange();
        }

        public static void SaveChange()
        {
            _dataContext.SaveChanges();
        }
    }
}