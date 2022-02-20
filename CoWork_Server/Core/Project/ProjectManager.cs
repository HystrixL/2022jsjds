using System.Collections.Generic;
using System.Linq;

namespace Co_Work.Core.Project
{
    public class ProjectManager
    {
        public static List<Project> Projects;

        public static Project GetProjectFromGuid(string guid)
        {
            return Projects.FirstOrDefault(project => project.GUID == guid);
        }
    }
}