using System;
using System.Linq;
using Luma.SmartHub.Audio.Playback;
using Luma.SmartHub.Plugins.Youtube.YoutubeExtractor;

namespace Luma.SmartHub.Plugins.Youtube
{
    public class YoutubePlaylistProvider : IPlaylistProvider
    {
        private readonly PlaylistUrlResolver _playlistUrlResolver;

        public YoutubePlaylistProvider()
            : this(new PlaylistUrlResolver()) { }
        public YoutubePlaylistProvider(PlaylistUrlResolver playlistUrlResolver)
        {
            _playlistUrlResolver = playlistUrlResolver;
        }

        public bool IsYoutubeUrl(Uri uri)
        {
            var youtubeHosts = new[] { "m.youtube.com", " youtu.be", "youtube.com", "www.youtube.com" };

            return youtubeHosts.Contains(uri.Host);
        }

        public Uri[] CreatePlaylist(Uri playlistUrl)
        {
            if (!IsYoutubeUrl(playlistUrl))
                return null;

            var query = HttpHelper.ParseQueryString(playlistUrl);
            var isYoutubePlaylist = query.ContainsKey("list");
            if (!isYoutubePlaylist)
                throw new InvalidOperationException("This isn't a Youtube playlist url");

            return _playlistUrlResolver
                .GetPlaylistVideoUrls(playlistUrl.ToString())
                .ToArray();
        }
    }
}
