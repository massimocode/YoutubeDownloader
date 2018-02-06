using System;
using System.Threading.Tasks;

namespace YoutubeDownloader
{
    public interface IDownloadManager
    {
        Task DownloadAsync(DownloadRequest downloadRequest, IProgress<DownloadProgress> progress);
    }
}