using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Forms;

namespace XmlValidator.DownloadFiles
{
    public class DownloadListOfFilesCommand
    {
        public Dictionary<string, string> SaveUrlsToFile { get; set; }
        public Dictionary<string, DownloadSizePerUrl> DownloadSizePerUrl { get; set; }
        public long TotalBytes { get; set; }
        public ToolStripProgressBar ToolStripProgressBar { get; set; }
        public HttpClient Client { get; set; }
        public ToolStripStatusLabel ToolStripStatusLabel { get; set; }
    }
}