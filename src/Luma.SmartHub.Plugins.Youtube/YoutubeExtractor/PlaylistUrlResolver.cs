using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Luma.SmartHub.Plugins.Youtube.YoutubeExtractor
{
    public class PlaylistUrlResolver
    {
        private readonly IWebClient _webClient;

        //private const string PlaylistEntryPattern = "<a class=\"pl-video-title-link yt-uix-tile-link yt-uix-sessionlink  spf-link \" dir=\"ltr\" href=\"(.*)\" data-sessionlink";

        private const string PlaylistEntryPattern = " data-video-id=\"(.{11})\"";

        public PlaylistUrlResolver()
            : this(new WebClient()) { }
        public PlaylistUrlResolver(IWebClient webClient)
        {
            _webClient = webClient;
        }

        public IEnumerable<Uri> GetPlaylistVideoUrls(string playlistUrl)
        {
            if (playlistUrl == null)
                throw new ArgumentNullException(nameof(playlistUrl));
            
            string pageSource = _webClient.DownloadString(playlistUrl);

            var matches = Regex.Matches(pageSource, PlaylistEntryPattern).Cast<Match>();

            var results = matches.Select(c => new Uri("http://youtube.com/watch?v=" + c.Groups[1].Value));
                
            return results;
        }
    }
}
