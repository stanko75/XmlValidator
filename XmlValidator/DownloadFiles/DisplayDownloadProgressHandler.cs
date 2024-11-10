using System.Windows.Forms;

namespace XmlValidator.DownloadFiles
{
    public class DisplayDownloadProgressHandler : ICommandHandler<DisplayDownloadProgressCommand>
    {
        public void Execute(DisplayDownloadProgressCommand displayDownloadProgressCommand)
        {
            if (!(displayDownloadProgressCommand.ToolStripProgressBar is null))
            {
                displayDownloadProgressCommand.ToolStripProgressBar.GetCurrentParent().Invoke((MethodInvoker)(() =>
                {
                    displayDownloadProgressCommand.ToolStripProgressBar.Value +=
                        (int)displayDownloadProgressCommand.Value;
                }));
            }
        }
    }
}