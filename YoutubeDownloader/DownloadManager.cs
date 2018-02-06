using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;

namespace YoutubeDownloader
{
    public class DownloadManager : IDownloadManager
    {
        public async Task DownloadAsync(DownloadRequest downloadRequest, IProgress<DownloadProgress> progress)
        {
            if (downloadRequest.IsAudioOnly)
            {
                await DownloadAudio(downloadRequest, progress);
            }
            else
            {
                await DownloadVideo(downloadRequest, progress);
            }
        }

        private async Task DownloadVideo(DownloadRequest downloadRequest, IProgress<DownloadProgress> progress)
        {
            var videos = (await YouTube.Default.GetAllVideosAsync(downloadRequest.Url)).ToList();
            var video = videos.FirstOrDefault(x => x.Format == VideoFormat.Mp4);
            if (video == null)
            {
                video = videos.First();
            }

            progress.Report(new DownloadProgress { Title = video.Title });

            var outputPath = Path.Combine(downloadRequest.DestinationFolder, video.FullName);
            if (File.Exists(outputPath))
            {
                progress.Report(new DownloadProgress { Status = Status.Duplicate });
                return;
            }

            progress.Report(new DownloadProgress { Status = Status.Downloading });

            await DownloadToPath(video, outputPath, progress);
        }

        private async Task DownloadToPath(YouTubeVideo video, string outputPath, IProgress<DownloadProgress> progress)
        {
            var uri = await video.GetUriAsync();
            using (var httpClient = new HttpClient())
            using (var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead))
            using (var downloadStream = await response.Content.ReadAsStreamAsync())
            using (var fileStream = File.OpenWrite(outputPath))
            {
                var contentSize = int.Parse(response.Content.Headers.GetValues("Content-Length").First());

                var buffer = new byte[1024 * 1024];
                int read;
                while ((read = await downloadStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await fileStream.WriteAsync(buffer, 0, read);
                    progress.Report(new DownloadProgress { PercentDownloaded = (int)(fileStream.Position * 100 / contentSize) });
                }
            }
        }

        private async Task DownloadAudio(DownloadRequest downloadRequest, IProgress<DownloadProgress> progress)
        {
            var videos = (await YouTube.Default.GetAllVideosAsync(downloadRequest.Url)).ToList();
            var audio = videos.FirstOrDefault(x => x.AdaptiveKind == AdaptiveKind.Audio);
            if (audio != null)
            {
                progress.Report(new DownloadProgress { Title = audio.Title });

                var outputPath = Path.Combine(downloadRequest.DestinationFolder, audio.FullName);
                if (audio.Format == VideoFormat.Mp4)
                {
                    outputPath = Path.ChangeExtension(outputPath, ".m4a");
                }

                if (File.Exists(outputPath))
                {
                    progress.Report(new DownloadProgress { Status = Status.Duplicate });
                    return;
                }

                progress.Report(new DownloadProgress { Status = Status.Downloading });
                await DownloadToPath(audio, outputPath, progress);
            }
            else
            {
                var video = videos.First();
                var temporaryPath = Path.ChangeExtension(Path.Combine(downloadRequest.DestinationFolder, video.FullName), ".temp");
                var finalPath = Path.ChangeExtension(temporaryPath, ".mp3");
                if (File.Exists(temporaryPath) || File.Exists(finalPath))
                {
                    progress.Report(new DownloadProgress { Status = Status.Duplicate });
                    return;
                }

                progress.Report(new DownloadProgress { Status = Status.Downloading });
                await DownloadToPath(video, temporaryPath, progress);

                progress.Report(new DownloadProgress { Status = Status.Converting });
                await Task.Run(() =>
                {
                    var inputFile = new MediaFile { Filename = temporaryPath };
                    var outputFile = new MediaFile { Filename = finalPath };
                    using (var engine = new Engine())
                    {
                        engine.Convert(inputFile, outputFile);
                    }
                    File.Delete(temporaryPath);
                });
            }
        }
    }
}
