using System;
using System.Linq;
using Luma.SmartHub.Audio.Playback;
using Luma.SmartHub.Plugins.Youtube.YoutubeExtractor;

namespace Luma.SmartHub.Plugins.Youtube
{
    public class YoutubePlaybackInfoProvider : IPlaybackInfoProvider
    {
        private readonly DownloadUrlResolver _downloadUrlResolver;

        public YoutubePlaybackInfoProvider()
            : this(new DownloadUrlResolver()) { }
        public YoutubePlaybackInfoProvider(DownloadUrlResolver downloadUrlResolver)
        {
            _downloadUrlResolver = downloadUrlResolver;

            SelectVideoInfoPredicate = c => c.AudioType == AudioType.Aac && c.Resolution == 0;
        }

        public bool IsYoutubeUrl(Uri uri)
        {
            var youtubeHosts = new[] { "m.youtube.com", " youtu.be", "youtube.com", "www.youtube.com" };

            return youtubeHosts.Contains(uri.Host);
        }

        public Func<VideoInfo, bool> SelectVideoInfoPredicate { get; set; }  

        public PlaybackInfo Get(Uri uri)
        {
            if (!IsYoutubeUrl(uri))
                return null;

            var url = uri.ToString();
            var downloadUrls = _downloadUrlResolver.GetDownloadUrls(url);

            var result = downloadUrls
                .OrderByDescending(c => c.AudioBitrate)
                .FirstOrDefault(SelectVideoInfoPredicate);

            if (result == null)
                throw new VideoNotAvailableException($"Audio stream for url {uri} was not found");

            return new PlaybackInfo
            {
                Name = result.Title,
                Uri = new Uri(result.DownloadUrl)
            };
        }
    }
}