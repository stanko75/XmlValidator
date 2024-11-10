using System.Windows.Forms;

namespace XmlValidator.DownloadFiles
{
    public class DisplayDownloadProgressCommand
    {
        public ToolStripProgressBar ToolStripProgressBar { get; set; }
        public long Maximum { get; set; }
        public double Value { get; set; }
    }
}