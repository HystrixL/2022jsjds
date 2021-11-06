using System;

namespace Co_Work.Local.ProjectFile
{
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