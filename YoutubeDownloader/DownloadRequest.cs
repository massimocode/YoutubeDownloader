namespace YoutubeDownloader
{
    public class DownloadRequest
    {
        public string Url { get; set; }
        public bool IsAudioOnly { get; set; }
        public string DestinationFolder { get; set; }
    }
}