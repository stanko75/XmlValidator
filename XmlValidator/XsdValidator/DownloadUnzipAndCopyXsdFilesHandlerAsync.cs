using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XmlValidator.DownloadFiles;

namespace XmlValidator.XsdValidator
{
    public class DownloadUnzipAndCopyXsdFilesHandlerAsync : ICommandHandlerAsync<DownloadUnzipAndCopyXsdFilesCommand>
    {
        private readonly ICommandHandlerAsync<DownloadListOfFilesCommand> _downloadListOfFilesHandlerAsync;
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public DownloadUnzipAndCopyXsdFilesHandlerAsync(
            CancellationDecorator<DownloadListOfFilesCommand> downloadListOfFilesHandlerAsync)
        {
            _downloadListOfFilesHandlerAsync = downloadListOfFilesHandlerAsync;
        }

        public async Task Execute(DownloadUnzipAndCopyXsdFilesCommand command)
        {
            string fullFileName = Path.Combine(command.RootFolder, command.FileName);
            if (!File.Exists(fullFileName))
            {
                await DoDownload(command.SaveUrlsToFile, command.ToolStripProgressBar, command.ToolStripStatusLabel);
            }

            command.ToolStripStatusLabel?.GetCurrentParent().Invoke((MethodInvoker)(() => command.ToolStripStatusLabel.Text = "Unzipping"));
            await Task.Run(async () =>
            {
                try
                {
                    UnZip(command.RootFolder, command.FileName, command.SecondZipFileNameWithPath, command.CopyFromFolder,
                        command.CopyToFolder);
                }
                catch //UnZip failed, delete try again
                {
                    File.Delete(fullFileName);
                    await DoDownload(command.SaveUrlsToFile, command.ToolStripProgressBar, command.ToolStripStatusLabel);
                }

                command.ToolStripStatusLabel?.GetCurrentParent().Invoke((MethodInvoker)(() => command.ToolStripStatusLabel.Text = "OK"));
            });
        }

        private async Task DoDownload(Dictionary<string, string> saveUrlsToFile,
            ToolStripProgressBar tssDownloadProgress, ToolStripStatusLabel tssStatus)
        {
            try
            {
                DownloadListOfFilesCommand downloadListOfFilesCommand = new DownloadListOfFilesCommand
                {
                    SaveUrlsToFile = saveUrlsToFile
                    , ToolStripProgressBar = tssDownloadProgress
                    , ToolStripStatusLabel = tssStatus
                };
                await _downloadListOfFilesHandlerAsync.Execute(downloadListOfFilesCommand);
            }
            catch (IOException ioEx)
            {
                MessageBox.Show(ioEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UnZip(string rootFolder, string fileName, string secondZipFileNameWithPath, string copyFromFolder,
            string copyToFolder)
        {
            string fullFileName = Path.Combine(rootFolder, fileName);

            //unzip downloaded
            string temp = Path.Combine(rootFolder, "temp");
            if (Directory.Exists(temp))
            {
                Directory.Delete(temp, true);
            }

            ZipFile.ExtractToDirectory(fullFileName, temp);

            //unzip second file with coupled xsds

            if (!string.IsNullOrEmpty(secondZipFileNameWithPath))
            {
                string secondZipFullFileName =
                    Path.Combine(temp, secondZipFileNameWithPath);
                if (File.Exists(secondZipFullFileName))
                {
                    ZipFile.ExtractToDirectory(secondZipFullFileName, temp);
                }
            }

            string sourceDir = Path.Combine(temp, copyFromFolder);
            string[] allFilesInSourceDir = Directory.GetFiles(sourceDir, "*.xsd", SearchOption.AllDirectories);

            foreach (string sourceFileName in allFilesInSourceDir)
            {
                string destinationRoot = Path.Combine(rootFolder, copyToFolder);
                //string sourceRoot = Path.Combine(temp, copyFromRootFolder);
                string sourceRoot = temp;
                string destFilename = sourceFileName.Replace(sourceRoot, destinationRoot);
                string destFileDir = Path.GetDirectoryName(destFilename);

                if (!(destFileDir is null) && !Directory.Exists(destFileDir))
                {
                    Directory.CreateDirectory(destFileDir);
                }

                File.Copy(sourceFileName, destFilename, true);
            }
        }
    }
}