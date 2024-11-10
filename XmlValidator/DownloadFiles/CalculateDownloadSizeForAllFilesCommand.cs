using System.Collections.Generic;
using System.Net.Http;

namespace XmlValidator.DownloadFiles
{
    public class CalculateDownloadSizeForAllFilesCommand
    {
        public string Url { get; set; }
        public HttpClient Client { get; set; }
        public long TotalBytes { get; set; } = 0;
        public Dictionary<string, DownloadSizePerUrl> DownloadSizePerUrl { get; set; }
    }
}