namespace YoutubeDownloader
{
    public class Download
    {
        public string Url { get; set; }
        public DownloadType Type { get; set; }
        public string Title { get; set; }
        public Status Status { get; set; }
        public int PercentDownloaded { get; set; }
        public string StatusText
        {
            get
            {
                if (Status == Status.Downloading)
                {
                    return $"Downloading - {PercentDownloaded}%";
                }
                else
                {
                    return Status.ToString();
                }
            }
        }

    }
}
