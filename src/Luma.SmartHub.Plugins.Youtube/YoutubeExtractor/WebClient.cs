using System.IO;
using System.Net;

namespace Luma.SmartHub.Plugins.Youtube.YoutubeExtractor
{
    public interface IWebClient
    {
        string DownloadString(string url);
    }

    public class WebClient : IWebClient
    {
        public string DownloadString(string url)
        {
            var request = WebRequest.Create(url);
            request.Method = "GET";
            request.Headers["Accept-Language"] = "en-US";

            System.Threading.Tasks.Task<WebResponse> task = System.Threading.Tasks.Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result)).Result;
        }

        private string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                using (var sr = new StreamReader(responseStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
