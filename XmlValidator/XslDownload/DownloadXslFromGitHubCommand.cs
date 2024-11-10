using System.Collections.Generic;
using System.Windows.Forms;

namespace XmlValidator.XslDownload
{
    public class DownloadXslFromGitHubCommand
    {
        public List<string> Repositories { get; set; }
        public string RooFolderWhereToSaveDownloadedFiles { get; set; }
        public ToolStripStatusLabel TssUblStatus { get; set; }
        public ToolStripProgressBar TsslUblDownloadProgress { get; set; }
    }
}