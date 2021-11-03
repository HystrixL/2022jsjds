using System;
using System.Collections.Generic;
using System.IO;
using Co_Work.Core;

namespace Co_Work.Local
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

        public List<ProjectFile> GetProjectFilesList(string path = "")
        {
            if (path == "") path = _projectFilesRelativeDirectory;
            var fileList = new List<ProjectFile>();

            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    var projectFile = new ProjectFile();
                    projectFile.ProjectGuid = _projectGuid;
                    projectFile.FileName = fileInfo.Name;
                    projectFile.FileSize = fileInfo.Length;
                    projectFile.UploadDate = fileInfo.CreationTime;
                    projectFile.Path = GetRelativePath(fileInfo.FullName).Replace(fileInfo.Name,"");
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

    public class ProjectFile
    {
        public string ProjectGuid { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadDate { get; set; }
        public string Uploader { get; set; }
        public string Path { get; set; } = @"\";
    }
}