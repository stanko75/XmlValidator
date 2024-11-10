using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace XmlValidator.DownloadFiles
{

    public class DownloadFileHandlerAsync : ICommandHandlerAsync<DownloadFileCommand>
    {
        private readonly ICommandHandler<DisplayDownloadProgressCommand> _displayDownloadProgressHandler;
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public DownloadFileHandlerAsync(ICommandHandler<DisplayDownloadProgressCommand> displayDownloadProgressHandler)
        {
            _displayDownloadProgressHandler = displayDownloadProgressHandler;
        }

        public async Task Execute(DownloadFileCommand command)
        {
            await DownloadFile(command.OutputPath, command.DownloadSizePerUrl,
                command.DisplayDownloadProgressCommand);
        }

        private async Task DownloadFile(string outputPath,
            DownloadSizePerUrl downloadSizePerUrl, DisplayDownloadProgressCommand commandDisplayDownloadProgressCommand)
        {
            if (!(CancellationTokenSource is null))
            {
                CancellationToken cancellationToken = CancellationTokenSource.Token;

                HttpResponseMessage response = downloadSizePerUrl.Response;

                string savePath = Path.GetDirectoryName(outputPath) ?? string.Empty;
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                       fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192,
                           true))
                {
                    byte[] buffer = new byte[8192];
                    int bytesRead;

                    while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);

                        commandDisplayDownloadProgressCommand.Value = bytesRead;

                        _displayDownloadProgressHandler.Execute(commandDisplayDownloadProgressCommand);
                    }
                }
            }
        }
    }
}