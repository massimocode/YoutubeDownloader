using System;
using System.Drawing;
using System.IO;
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
            var audioOnly = chk_audioOnly.Checked;
            var url = txt_url.Text;
            txt_url.Text = "";
            Log($"Resolving {url}...");
            try
            {
                var video = await Task.Run(() => YouTube.Default.GetVideo(url));
                Log($"Found video {video.FullName} - Downloading...");
                var savedFilePath = Path.Combine(DesktopPath, video.FullName);
                if (audioOnly)
                {
                    savedFilePath = Path.ChangeExtension(savedFilePath, ".temp");
                }
                File.WriteAllBytes(savedFilePath, await video.GetBytesAsync());
                Log($"Successfully downloaded {video.FullName}");
                if (audioOnly)
                {
                    Log($"Converting {video.FullName} to MP3");
                    await ConvertToMp3Async(savedFilePath);
                    Log($"Successfully converted {video.FullName} to MP3");
                }
            }
            catch (Exception error)
            {
                Log("An error occurred: " + error.Message);
            }
        }

        private void Log(string text)
        {
            txt_progress.AppendText(text + Environment.NewLine);
        }

        private Task ConvertToMp3Async(string inputFileName)
        {
            return Task.Run(() =>
            {
                var inputFile = new MediaFile { Filename = inputFileName };
                var outputFile = new MediaFile { Filename = Path.ChangeExtension(inputFileName, ".mp3") };
                using (var engine = new Engine())
                {
                    engine.Convert(inputFile, outputFile);
                }
                File.Delete(inputFileName);
            });
        }
    }
}
