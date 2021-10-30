using System.Collections.Generic;
using Co_Work.Network;

namespace Co_Work.Core
{
    public static class EmployeeManager
    {
        public static List<Employee> Employees = new List<Employee>();
        
        public static Employee GetEmployeeFromId(string id)
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

        public static Employee GetEmployeeFromGuid(string guid)
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