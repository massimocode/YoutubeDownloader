using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;

namespace YoutubeDownloader
{
    public partial class MainForm : Form
    {
        static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public MainForm()
        {
            InitializeComponent();
            Icon = new Icon(typeof(MainForm), "icon.ico");
            Text = $"YouTube Downloader - {typeof(MainForm).Assembly.GetName().Version}";
        }

        private async void btn_download_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_url.Text))
            {
                MessageBox.Show("Please enter a URL to download");
                return;
            }
            var url = txt_url.Text;
            txt_url.Text = "";
            try
            {
                Log($"Resolving {url}");
                if (chk_audioOnly.Checked)
                {
                    await DownloadAudio(url);
                }
                else
                {
                    await DownloadVideo(url);
                }
            }
            catch (Exception ex)
            {
                Log($"An error occurred downloading {url}");
                Log(ex.Message);
            }
        }

        private async Task DownloadVideo(string url)
        {
            var video = await YouTube.Default.GetVideoAsync(url);
            Log($"Found video for {video.Title} - Downloading...");
            File.WriteAllBytes(Path.Combine(DesktopPath, video.FullName), await video.GetBytesAsync());
            Log($"Successfully downloaded video for {video.Title}");
        }

        private async Task DownloadAudio(string url)
        {
            var videos = (await YouTube.Default.GetAllVideosAsync(url)).ToList();
            var audio = videos.FirstOrDefault(x => x.AdaptiveKind == AdaptiveKind.Audio);
            if (audio != null)
            {
                Log($"Found audio for {audio.Title} - Downloading...");
                var outputPath = audio.FullName;
                if (audio.Format == VideoFormat.Mp4)
                {
                    outputPath = Path.ChangeExtension(outputPath, ".m4a");
                }
                File.WriteAllBytes(Path.Combine(DesktopPath, outputPath), await audio.GetBytesAsync());
                Log($"Successfully downloaded audio for {audio.Title}");
            }
            else
            {
                var video = videos.First();
                Log($"Couldn't find any audio streams for {video.Title}. Downloading {video.Format} format for conversion...");
                var temporaryFilePath = Path.ChangeExtension(Path.Combine(DesktopPath, video.FullName), ".temp");
                File.WriteAllBytes(temporaryFilePath, await video.GetBytesAsync());
                Log($"Converting {video.Title} to MP3");
                await Task.Run(() =>
                {
                    var inputFile = new MediaFile { Filename = temporaryFilePath };
                    var outputFile = new MediaFile { Filename = Path.ChangeExtension(temporaryFilePath, ".mp3") };
                    using (var engine = new Engine())
                    {
                        engine.Convert(inputFile, outputFile);
                    }
                    File.Delete(temporaryFilePath);
                });
                Log($"Successfully downloaded and converted {video.Title} to MP3");
            }
        }

        private void Log(string text)
        {
            txt_progress.AppendText(text + Environment.NewLine);
        }
    }
}
