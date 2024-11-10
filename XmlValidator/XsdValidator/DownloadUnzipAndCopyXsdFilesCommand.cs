using System.Collections.Generic;
using System.Windows.Forms;

namespace XmlValidator.XsdValidator
{
    public class DownloadUnzipAndCopyXsdFilesCommand
    {
        public string FileName { get; set; }
        public string RootFolder { get; set; }
        public Dictionary<string, string> SaveUrlsToFile { get; set; }
        public string SecondZipFileNameWithPath { get; set; }
        public string CopyFromFolder { get; set; }
        public string CopyToFolder { get; set; }
        public ToolStripProgressBar ToolStripProgressBar { get; set; }
        public ToolStripStatusLabel ToolStripStatusLabel { get; set; }
    }
}