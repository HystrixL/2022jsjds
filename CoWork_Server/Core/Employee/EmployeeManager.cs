using System.Collections.Generic;
using System.Linq;

namespace Co_Work.Core.Employee
{
    public static class EmployeeManager
    {
        public static List<Core.Employee.Employee> Employees;
        
        public static Core.Employee.Employee GetEmployeeFromId(string id)
        {
            return Employees.FirstOrDefault(employee => employee.ID == id);
        }

        public static Core.Employee.Employee GetEmployeeFromGuid(string guid)
        {
            return Employees.FirstOrDefault(employee => employee.GUID == guid);
        }
        
    }
}