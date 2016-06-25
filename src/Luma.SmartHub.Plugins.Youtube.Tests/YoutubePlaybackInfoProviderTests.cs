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
            var exampleVideoUrl = "http://youtube.com/watch?v=maw2OoL15J4";
            var expectedAudioUri = "https://r3---sn-f5f7ln7y.googlevideo.com/videoplayback?nh=IgpwcjAyLndhdzAyKg4yMTMuNDYuMTc4LjEwOQ&clen=796953&gir=yes&sver=3&lmt=1463113091862123&dur=50.131&source=youtube&fexp=9416126,9416891,9419452,9422596,9427378,9428398,9429854,9431012,9433096,9433221,9433705,9433946,9435526,9435692,9435876,9437066,9437088,9437103,9437553,9438256,9438326,9438902,9439652,9440179,9440309&sparams=clen,dur,gir,id,initcwndbps,ip,ipbits,itag,keepalive,lmt,mime,mm,mn,ms,mv,nh,pl,requiressl,source,upn,expire&requiressl=yes&ipbits=0&itag=140&mm=31&ip=89.67.30.237&mn=sn-f5f7ln7y&mt=1466852949&mv=m&ms=au&pl=13&keepalive=yes&id=o-AHzsykHMS8p-STV4WNjL1IefVH_3QaxPSrG0PSBV7kL7&upn=FCU8wnQWjls&expire=1466874780&mime=audio/mp4&key=yt6&initcwndbps=2640000&signature=BD1476CD500CFD1B623E3166A2758C33B54D0249.0634456CBAA7DC29BB6A3BD5B9C01A11A1C7F927&ratebypass=yes";
            var expectedTitle = "Despicable Me: Minion Rush - Field Sports - Update Trailer";
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
            private string _url;
            private string _htmlPath;

            public YoutubePlaybackInfoProvider Sut { get; }

            public Mock<IWebClient> WebClient { get; }

            public YoutubePlaybackInfoProviderTestsFixture()
            {
                WebClient = new Mock<IWebClient>();
                Sut = new YoutubePlaybackInfoProvider(new DownloadUrlResolver(WebClient.Object));
            }

            public YoutubePlaybackInfoProviderTestsFixture ReturnHtmlForUrl(string url, string htmlPath)
            {
                _url = url;
                _htmlPath = htmlPath;

                var html = File.ReadAllText(htmlPath);

                WebClient.Setup(c => c.DownloadString(url)).Returns(html);

                return this;
            }

            public YoutubePlaybackInfoProviderTestsFixture Update()
            {
                var webClient = new WebClient();

                var html = webClient.DownloadString(_url);

                File.WriteAllText(_htmlPath, html);

                return this;
            }
        }
    }
}
