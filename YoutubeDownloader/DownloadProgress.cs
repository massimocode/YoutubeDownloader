namespace YoutubeDownloader
{
    public class DownloadProgress
    {
        public string Title { get; set; }
        public Status? Status { get; set; }
        public int? PercentDownloaded { get; set; }
    }
}