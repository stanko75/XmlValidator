using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace XmlValidator.DownloadFiles
{
    public class CalculateDownloadSizeForAllFilesHandler : ICommandHandlerAsync<CalculateDownloadSizeForAllFilesCommand>
    {
        public CancellationTokenSource CancellationTokenSource { get; set; }

        public async Task Execute(CalculateDownloadSizeForAllFilesCommand command)
        {
            DownloadSizePerUrl downloadSizePerUrl = await GetDownloadSize(command.Client, command.Url);
            command.DownloadSizePerUrl.Add(command.Url, downloadSizePerUrl);
            command.TotalBytes += downloadSizePerUrl.Size;
        }

        private async Task<DownloadSizePerUrl> GetDownloadSize(HttpClient client, string url)
        {
            if (!(CancellationTokenSource is null))
            {
                CancellationToken cancellationToken = CancellationTokenSource.Token;

                DownloadSizePerUrl downloadSizePerUrl = new DownloadSizePerUrl();
                HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
                response.EnsureSuccessStatusCode();

                downloadSizePerUrl.Size = response.Content.Headers.ContentLength ?? -1;
                downloadSizePerUrl.Response = response;

                return downloadSizePerUrl;
            }

            return null;
        }
    }
}