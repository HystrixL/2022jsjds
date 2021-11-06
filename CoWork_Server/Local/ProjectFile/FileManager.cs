using System.Collections.Generic;
using System.IO;

namespace Co_Work.Local.ProjectFile
{
    public class FileManager
    {
        private string _projectFilesPath = @"CoWorkData\ProjectFiles";
        private string _projectGuid;
        private string _projectFilesRelativeDirectory;

        public FileManager(string projectGuid)
        {
            _projectGuid = projectGuid;
            var serverRootPath = Directory.GetCurrentDirectory();
            _projectFilesRelativeDirectory = Path.Combine(serverRootPath, _projectFilesPath, _projectGuid);
        }

        public List<Local.ProjectFile.ProjectFile> GetProjectFilesList(string path = "")
        {
            if (path == "") path = _projectFilesRelativeDirectory;
            var fileList = new List<Local.ProjectFile.ProjectFile>();

            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    var projectFile = new Local.ProjectFile.ProjectFile
                    {
                        ProjectGuid = _projectGuid,
                        FileName = fileInfo.Name,
                        FileSize = fileInfo.Length,
                        UploadDate = fileInfo.CreationTime,
                        Path = GetRelativePath(fileInfo.FullName).Replace(fileInfo.Name,"")
                    };
                    fileList.Add(projectFile);
                }

                foreach (string directory in Directory.GetDirectories(path))
                {
                    fileList.AddRange(GetProjectFilesList(directory));
                }
            }

            return fileList;
        }

        private string GetRelativePath(string absolutePath)
        {
            return absolutePath.Replace(_projectFilesRelativeDirectory, "");
        }
    }
}