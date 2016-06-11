using System;
using System.Linq;
using Luma.SmartHub.Audio.Playback;
using Luma.SmartHub.Plugins.Youtube.YoutubeExtractor;

namespace Luma.SmartHub.Plugins.Youtube
{
    public class YoutubePlaybackUriTransformer : IPlaybackUriTransformer
    {
        private readonly DownloadUrlResolver _downloadUrlResolver;

        public YoutubePlaybackUriTransformer()
            : this(new DownloadUrlResolver()) { }
        public YoutubePlaybackUriTransformer(DownloadUrlResolver downloadUrlResolver)
        {
            _downloadUrlResolver = downloadUrlResolver;
        }

        public bool IsYoutubeUrl(Uri uri)
        {
            var youtubeHosts = new[] { "m.youtube.com", " youtu.be", "youtube.com", "www.youtube.com" };

            return youtubeHosts.Contains(uri.Host);
        }

        public Uri Transform(Uri uri)
        {
            if (!IsYoutubeUrl(uri))
                return null;

            var url = uri.ToString();
            var downloadUrls = _downloadUrlResolver.GetDownloadUrls(url);

            var result = downloadUrls
                .OrderByDescending(c => c.AudioBitrate)
                .FirstOrDefault(c => c.AudioType == AudioType.Aac && c.Resolution == 0);

            if (result == null)
                throw new VideoNotAvailableException($"Audio stream for url {uri} was not found");

            // TODO: Return Title too
            return new Uri(result.DownloadUrl);
        }
    }
}