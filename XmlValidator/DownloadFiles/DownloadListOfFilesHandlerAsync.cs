using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XmlValidator.DownloadFiles
{
    public class DownloadListOfFilesHandlerAsync : ICommandHandlerAsync<DownloadListOfFilesCommand>
    {
        private readonly ICommandHandlerAsync<CalculateDownloadSizeForAllFilesCommand> _calculateDownloadSizeForAllFilesHandler;
        private readonly ICommandHandlerAsync<DownloadFileCommand> _downloadFileHandlerAsync;
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public DownloadListOfFilesHandlerAsync(ICommandHandlerAsync<CalculateDownloadSizeForAllFilesCommand> calculateDownloadSizeForAllFilesHandler, ICommandHandlerAsync<DownloadFileCommand> downloadFileHandlerAsync)
        {
            _calculateDownloadSizeForAllFilesHandler = calculateDownloadSizeForAllFilesHandler;
            _downloadFileHandlerAsync = downloadFileHandlerAsync;
        }

        public async Task Execute(DownloadListOfFilesCommand downloadListOfFilesCommand)
        {
            downloadListOfFilesCommand.ToolStripProgressBar?.GetCurrentParent().Invoke((MethodInvoker)(() =>
                downloadListOfFilesCommand.ToolStripProgressBar.Value = 0));

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "milosev.com");
                downloadListOfFilesCommand.Client = client;
                await GetDownloadSizeOfAllFiles(downloadListOfFilesCommand);
                await DownloadFiles(downloadListOfFilesCommand);
            }
        }

        private async Task GetDownloadSizeOfAllFiles(DownloadListOfFilesCommand downloadListOfFilesCommand)
        {
            if (!(CancellationTokenSource is null))
            {
                CancellationToken cancellationToken = CancellationTokenSource.Token;
                CalculateDownloadSizeForAllFilesCommand calculateDownloadSizeForAllFilesCommand =
                    new CalculateDownloadSizeForAllFilesCommand
                    {
                        Client = downloadListOfFilesCommand.Client,
                        DownloadSizePerUrl = new Dictionary<string, DownloadSizePerUrl>()
                    };
                foreach (KeyValuePair<string, string> saveUrlToFile in downloadListOfFilesCommand.SaveUrlsToFile)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    calculateDownloadSizeForAllFilesCommand.Url = saveUrlToFile.Key;
                    await _calculateDownloadSizeForAllFilesHandler.Execute(calculateDownloadSizeForAllFilesCommand);
                }

                downloadListOfFilesCommand.TotalBytes = calculateDownloadSizeForAllFilesCommand.TotalBytes;
                downloadListOfFilesCommand.ToolStripProgressBar?.GetCurrentParent().Invoke((MethodInvoker)(() =>
                    downloadListOfFilesCommand.ToolStripProgressBar.Maximum =
                        Convert.ToInt32(downloadListOfFilesCommand.TotalBytes)));
                downloadListOfFilesCommand.DownloadSizePerUrl =
                    calculateDownloadSizeForAllFilesCommand.DownloadSizePerUrl;
            }
        }

        private async Task DownloadFiles(DownloadListOfFilesCommand downloadListOfFilesCommand)
        {
            DisplayDownloadProgressCommand displayDownloadProgressCommand =
                new DisplayDownloadProgressCommand
                {
                    ToolStripProgressBar = downloadListOfFilesCommand.ToolStripProgressBar,
                    Maximum = downloadListOfFilesCommand.TotalBytes
                };
            DownloadFileCommand downloadFileCommand = new DownloadFileCommand
            {
                Client = downloadListOfFilesCommand.Client
            };
            foreach (KeyValuePair<string, string> saveUrlToFile in downloadListOfFilesCommand.SaveUrlsToFile)
            {
                Uri uri = new Uri(saveUrlToFile.Key);
                downloadListOfFilesCommand.ToolStripStatusLabel?.GetCurrentParent().Invoke((MethodInvoker)(() => downloadListOfFilesCommand.ToolStripStatusLabel.Text = $"Downloading file: {Path.GetFileName(uri.LocalPath)}"));
                downloadFileCommand.Url = saveUrlToFile.Key;
                downloadFileCommand.OutputPath = saveUrlToFile.Value;
                downloadFileCommand.TotalBytes = downloadListOfFilesCommand.TotalBytes;
                downloadFileCommand.DownloadSizePerUrl =
                    downloadListOfFilesCommand.DownloadSizePerUrl[saveUrlToFile.Key];
                downloadFileCommand.DisplayDownloadProgressCommand = displayDownloadProgressCommand;
                await _downloadFileHandlerAsync.Execute(downloadFileCommand);
            }
            downloadListOfFilesCommand.ToolStripStatusLabel?.GetCurrentParent().Invoke((MethodInvoker)(() => downloadListOfFilesCommand.ToolStripStatusLabel.Text = "OK"));
        }
    }
}