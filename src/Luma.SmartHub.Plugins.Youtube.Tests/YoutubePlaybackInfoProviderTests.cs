using System;
using System.IO;
using FluentAssertions;
using Luma.SmartHub.Plugins.Youtube.YoutubeExtractor;
using Moq;
using Xunit;

namespace Luma.SmartHub.Plugins.Youtube.Tests
{
    public class YoutubePlaybackInfoProviderTests
    {
        [Fact]
        public void Should_Return_Proper_Audio_Uri_And_Name()
        {
            var exampleVideoUrl = "http://youtube.com/watch?v=BVfZ6GSee3E";
            var expectedAudioUri = "https://r4---sn-2pvopxg-2v1l.googlevideo.com/videoplayback?sparams=clen,dur,gir,id,initcwndbps,ip,ipbits,itag,keepalive,lmt,mime,mm,mn,ms,mv,pl,requiressl,source,upn,expire&clen=4994824&initcwndbps=1497500&ipbits=0&dur=314.444&source=youtube&lmt=1464406177685666&requiressl=yes&fexp=9416126,9416891,9422596,9424092,9428398,9431012,9432060,9432565,9432683,9433096,9433223,9433946,9434605,9435527,9435876,9436026,9436097,9436606,9436879,9436923,9437066,9437285,9437403,9437553,9437602,9437987,9438011,9438245,9438661,9438957&signature=C2D0E49E60B41A745394189ED86F5756EAD6CF78.74FFD8969724CC4152405495553710C2C9E248FC&key=yt6&gir=yes&itag=140&expire=1465669346&upn=_xgEzsohfto&sver=3&mime=audio/mp4&mv=m&mt=1465647402&ms=au&ip=213.192.79.68&pl=24&keepalive=yes&mn=sn-2pvopxg-2v1l&mm=31&id=o-AEbXN7xC4f1q7lKEjFm5jNaMQIOiJEzjAxvjDdEJmBmf&ratebypass=yes";
            var expectedTitle = "Zootopia - Funny moments [HD]";
            var fixture = new YoutubePlaybackInfoProviderTestsFixture()
                .ReturnHtmlForUrl(exampleVideoUrl, "example-video-page.html");
            
            var result = fixture.Sut.Get(new Uri(exampleVideoUrl));

            result.Uri.Should().Be(expectedAudioUri);
            result.Name.Should().Be(expectedTitle);
        }

        [Fact]
        public void For_Unavailable_Video_Should_Throw_VideoNotAvailableException()
        {
            var exampleVideoUrl = "http://youtube.com/watch?v=fogOM_O9EtA";
            var fixture = new YoutubePlaybackInfoProviderTestsFixture()
                .ReturnHtmlForUrl(exampleVideoUrl, "example-unavailable-video-page.html");
            
            var act = new Action(() => fixture.Sut.Get(new Uri(exampleVideoUrl)));

            Assert.Throws<VideoNotAvailableException>(act);
        }

        private class YoutubePlaybackInfoProviderTestsFixture
        {
            public YoutubePlaybackInfoProvider Sut { get; }

            public Mock<IWebClient> WebClient { get; }

            public YoutubePlaybackInfoProviderTestsFixture()
            {
                WebClient = new Mock<IWebClient>();
                Sut = new YoutubePlaybackInfoProvider(new DownloadUrlResolver(WebClient.Object));
            }

            public YoutubePlaybackInfoProviderTestsFixture ReturnHtmlForUrl(string url, string htmlPath)
            {
                var html = File.ReadAllText(htmlPath);

                WebClient.Setup(c => c.DownloadString(url)).Returns(html);

                return this;
            }
        }
    }
}
