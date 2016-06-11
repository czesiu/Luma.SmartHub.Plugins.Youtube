using System;
using Luma.SmartHub.Plugins.Youtube.YoutubeExtractor;
using Xunit;

namespace Luma.SmartHub.Plugins.Youtube.Tests.YoutubeExtractor
{
    /// <summary>
    /// Small series of unit tests for DownloadUrlResolver.
    /// </summary>
    public class DownloadUrlResolverTest
    {
        [Fact]
        public void TryNormalizedUrlForStandardYouTubeUrlShouldReturnSame()
        {
            var urlResolver = new DownloadUrlResolver();
            string url = "http://youtube.com/watch?v=12345";

            string normalizedUrl = String.Empty;

            Assert.True(urlResolver.TryNormalizeYoutubeUrl(url, out normalizedUrl));
            Assert.Equal(url, normalizedUrl);
        }

        [Fact]
        public void TryNormalizedrlForYouTuDotBeUrlShouldReturnNormalizedUrl()
        {
            var urlResolver = new DownloadUrlResolver();
            string url = "http://youtu.be/12345";

            string normalizedUrl = String.Empty;
            Assert.True(urlResolver.TryNormalizeYoutubeUrl(url, out normalizedUrl));
            Assert.Equal("http://youtube.com/watch?v=12345", normalizedUrl);
        }

        [Fact]
        public void TryNormalizedUrlForMobileLinkShouldReturnNormalizedUrl()
        {
            var urlResolver = new DownloadUrlResolver();
            string url = "http://m.youtube.com/?v=12345";

            string normalizedUrl = String.Empty;
            Assert.True(urlResolver.TryNormalizeYoutubeUrl(url, out normalizedUrl));
            Assert.Equal("http://youtube.com/watch?v=12345", normalizedUrl);
        }

        [Fact]
        public void GetNormalizedYouTubeUrlForBadLinkShouldReturnNull()
        {
            var urlResolver = new DownloadUrlResolver();
            string url = "http://notAYouTubeUrl.com";

            string normalizedUrl = String.Empty;
            Assert.False(urlResolver.TryNormalizeYoutubeUrl(url, out normalizedUrl));
            Assert.Null(normalizedUrl);
        }
    }
}
