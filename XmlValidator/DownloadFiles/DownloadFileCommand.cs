using System.Net.Http;

namespace XmlValidator.DownloadFiles
{
    public class DownloadFileCommand
    {
        public string OutputPath { get; set; }
        public string Url { get; set; }
        public HttpClient Client { get; set; }
        public long TotalBytes { get; set; }
        public DownloadSizePerUrl DownloadSizePerUrl { get; set; }
        public DisplayDownloadProgressCommand DisplayDownloadProgressCommand { get; set; }
    }
}