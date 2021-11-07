using System.Collections.Generic;

namespace Co_Work.Core.Employee
{
    public static class EmployeeManager
    {
        public static List<Core.Employee.Employee> Employees;
        
        public static Core.Employee.Employee GetEmployeeFromId(string id)
        {
            foreach (var employee in Employees)
            {
                if (employee.ID==id)
                {
                    return employee;
                }
            }

            return null;
        }

        public static Core.Employee.Employee GetEmployeeFromGuid(string guid)
        {
            foreach (var employee in Employees)
            {
                if (employee.GUID==guid)
                {
                    return employee;
                }
            }

            return null;
        }
        
    }
}