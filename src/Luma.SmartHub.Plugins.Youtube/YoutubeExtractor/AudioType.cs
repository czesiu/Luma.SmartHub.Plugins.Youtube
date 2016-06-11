namespace Luma.SmartHub.Plugins.Youtube.YoutubeExtractor
{
    public enum AudioType
    {
        Aac,
        Mp3,
        Vorbis,

        /// <summary>
        /// The audio type is unknown. This can occur if YoutubeExtractor is not up-to-date.
        /// </summary>
        Unknown
    }
}