using System;
using System.IO;
using FluentAssertions;
using Luma.SmartHub.Plugins.Youtube.YoutubeExtractor;
using Moq;
using Xunit;

namespace Luma.SmartHub.Plugins.Youtube.Tests
{
    public class YoutubePlaylistProviderTests
    {
        [Fact]
        public void Should_Create_Playlist_From_Watch_Url()
        {
            var exampleVideoWithPlaylistUrl = "http://www.youtube.com/watch?v=BVfZ6GSee3E&list=PLzjFbaFzsmMQk8613XlrMO20m0FPs5Z1d&index=1";
            var expectedResultsCount = 200;
            var fixture = new YoutubePlaylistProviderTestsFixture()
                .ReturnHtmlForUrl(exampleVideoWithPlaylistUrl, "example-video-page-with-playlist.html");

            var results = fixture.Sut.CreatePlaylist(new Uri(exampleVideoWithPlaylistUrl));

            results.Should().HaveCount(expectedResultsCount);
        }

        [Fact]
        public void Should_Create_Playlist_From_Playlist_Url()
        {
            var examplePlaylistUrl = "http://www.youtube.com/playlist?list=PLzjFbaFzsmMQk8613XlrMO20m0FPs5Z1d";
            var expectedResultsCount = 100;
            var fixture = new YoutubePlaylistProviderTestsFixture()
                .ReturnHtmlForUrl(examplePlaylistUrl, "example-playlist-page.html");

            var results = fixture.Sut.CreatePlaylist(new Uri(examplePlaylistUrl));

            results.Should().HaveCount(expectedResultsCount);
        }

        private class YoutubePlaylistProviderTestsFixture
        {
            public YoutubePlaylistProvider Sut { get; }

            public Mock<IWebClient> WebClient { get; }

            public YoutubePlaylistProviderTestsFixture()
            {
                WebClient = new Mock<IWebClient>();
                Sut = new YoutubePlaylistProvider(new PlaylistUrlResolver(WebClient.Object));
            }

            public YoutubePlaylistProviderTestsFixture ReturnHtmlForUrl(string url, string htmlPath)
            {
                var html = File.ReadAllText(htmlPath);

                WebClient.Setup(c => c.DownloadString(url)).Returns(html);

                return this;
            }
        }
    }
}
