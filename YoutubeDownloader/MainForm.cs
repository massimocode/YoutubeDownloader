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
            dataGridView.RowHeadersVisible = false;
            dataGridView.DataSource = downloads;
            dataGridView.Columns["Url"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridView.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView.Columns["Status"].Visible = false;
            dataGridView.Columns["PercentDownloaded"].Visible = false;
            dataGridView.Columns["StatusText"].HeaderText = "Status";
            dataGridView.Columns["StatusText"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
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
            var isAudioOnly = chk_audioOnly.Checked;
            var downloadType = isAudioOnly ? DownloadType.Audio : DownloadType.Video;
            if (downloads.Any(x => x.Url == url && x.Type == downloadType && x.Status != Status.Failed))
            {
                MessageBox.Show("This download is already in the queue");
                return;
            }

            var download = new Download { Url = url, Type = downloadType };
            downloads.Add(download);
            dataGridView.Refresh();

            try
            {
                await _downloadManager.DownloadAsync(new DownloadRequest
                {
                    Url = url,
                    IsAudioOnly = isAudioOnly,
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
}
