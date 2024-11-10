using System.Net.Http;

namespace XmlValidator.DownloadFiles
{
    public class DownloadSizePerUrl
    {
        public long Size { get; set; }
        public HttpResponseMessage Response { get; set; }
    }
}