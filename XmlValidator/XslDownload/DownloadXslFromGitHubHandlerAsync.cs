using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XmlValidator.DownloadFiles;

namespace XmlValidator.XslDownload
{
    public class DownloadXslFromGitHubHandlerAsync : ICommandHandlerAsync<DownloadXslFromGitHubCommand>
    {
        private readonly CancellationDecorator<DownloadListOfFilesCommand> _downloadListOfFilesCancellationDecorator;
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public DownloadXslFromGitHubHandlerAsync(
            CancellationDecorator<DownloadListOfFilesCommand> downloadListOfFilesCancellationDecorator)
        {
            _downloadListOfFilesCancellationDecorator = downloadListOfFilesCancellationDecorator;
        }

        public async Task Execute(DownloadXslFromGitHubCommand command)
        {
            await DownloadAndUnzip(command.Repositories, command.TsslUblDownloadProgress, command.TssUblStatus, command.RooFolderWhereToSaveDownloadedFiles);
        }

        private async Task DownloadAndUnzip(List<string> repositories, ToolStripProgressBar tsslUblDownloadProgress,
            ToolStripStatusLabel tssUblStatus, string rooFolderWhereToSaveDownloadedFiles)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "milosev.com");

                foreach (string repository in repositories)
                {
                    HttpResponseMessage response = await client.GetAsync(repository);
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    //string jsonResponse = File.ReadAllText(@"C:\thrash\xrechnung-schematron.json");

                    List<Release> releases = JsonConvert.DeserializeObject<List<Release>>(jsonResponse);
                    Release release = releases.FirstOrDefault();

                    if (!(release is null))
                    {
                        foreach (Asset asset in release.Assets)
                        {
                            DownloadListOfFilesCommand downloadListOfFilesCommand =
                                new DownloadListOfFilesCommand
                                {
                                    ToolStripProgressBar = tsslUblDownloadProgress,
                                    ToolStripStatusLabel = tssUblStatus
                                };

                            string[] extractSaveToFolderName = repository.Split('/');
                            string saveToFolderName = extractSaveToFolderName[extractSaveToFolderName.Length - 2];

                            string saveDownloadedFileTo = Path.Combine(rooFolderWhereToSaveDownloadedFiles, saveToFolderName);
                            saveDownloadedFileTo = Path.Combine(saveDownloadedFileTo, asset.Name);

                            Dictionary<string, string> saveUrlsToFile = new Dictionary<string, string>
                            {
                                {
                                    asset.BrowserDownloadUrl,
                                    saveDownloadedFileTo
                                }
                            };
                            downloadListOfFilesCommand.SaveUrlsToFile = saveUrlsToFile;

                            if (!File.Exists(saveDownloadedFileTo))
                            {
                                await _downloadListOfFilesCancellationDecorator.Execute(downloadListOfFilesCommand);
                            }

                            string extractTo = $@"{Path.GetDirectoryName(saveDownloadedFileTo)}\zip";
                            if (Directory.Exists(extractTo))
                            {
                                Directory.Delete(extractTo, true);
                            }

                            ZipFile.ExtractToDirectory(saveDownloadedFileTo, extractTo);
                        }
                    }
                }
            }
        }
    }
}