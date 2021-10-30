using System.Collections.Generic;
using Co_Work.Network;

namespace Co_Work.Core.Project
{
    public class ProjectManager
    {
        public static List<Project> Projects;

        public static Project GetProjectFromGuid(string guid)
        {
            foreach (var project in Projects)
            {
                if (project.GUID==guid)
                {
                    return project;
                }
            }

            return null;
        }
    }
}