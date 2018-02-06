using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace YoutubeDownloader
{
    public partial class MainForm : Form
    {
        static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly IDownloadManager _downloadManager;
        private IList<Download> downloads = new BindingList<Download>();

        public MainForm()
        {
            InitializeComponent();
            Icon = new Icon(typeof(MainForm), "icon.ico");
            Text = $"YouTube Downloader - {typeof(MainForm).Assembly.GetName().Version}";
            dataGridView.AutoGenerateColumns = true;
            dataGridView.DataSource = downloads;
            dataGridView.Columns["Status"].Visible = false;
            dataGridView.Columns["PercentDownloaded"].Visible = false;
            dataGridView.Columns["StatusText"].HeaderText = "Status";
            dataGridView.Columns["Title"].Width = 433;
            _downloadManager = new DownloadManager();
        }

        private async void btn_download_Click(object sender, EventArgs e)
        {
            var url = txt_url.Text;
            txt_url.Text = "";
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Please enter a URL to download");
                return;
            }
            if (downloads.Any(x => x.Url == url && x.Status != Status.Failed))
            {
                MessageBox.Show("This download is already in the queue");
                return;
            }

            var download = new Download { Url = url };
            downloads.Add(download);
            dataGridView.Refresh();

            try
            {
                await _downloadManager.DownloadAsync(new DownloadRequest
                {
                    Url = url,
                    IsAudioOnly = chk_audioOnly.Checked,
                    DestinationFolder = DesktopPath
                }, new Progress<DownloadProgress>(progress =>
                {
                    if (progress.Title != null && download.Title != progress.Title)
                    {
                        download.Title = progress.Title;
                        dataGridView.Refresh();
                    }
                    if (progress.Status != null && download.Status != progress.Status)
                    {
                        download.Status = progress.Status.Value;
                        dataGridView.Refresh();
                    }
                    if (progress.PercentDownloaded != null && download.PercentDownloaded != progress.PercentDownloaded)
                    {
                        download.PercentDownloaded = progress.PercentDownloaded.Value;
                        dataGridView.Refresh();
                    }
                }));
                download.Status = Status.Complete;
                dataGridView.Refresh();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Error downloading {url}. {exception.Message}");
                download.Status = Status.Failed;
                dataGridView.Refresh();
            }
        }
    }

    public class Download
    {
        public string Url { get; set; }
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

    public enum Status
    {
        Queued,
        Downloading,
        Converting,
        Complete,
        Failed,
        Duplicate
    }
}
