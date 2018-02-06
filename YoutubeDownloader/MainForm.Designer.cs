namespace YoutubeDownloader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txt_url = new System.Windows.Forms.TextBox();
            this.btn_download = new System.Windows.Forms.Button();
            this.chk_audioOnly = new System.Windows.Forms.CheckBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Video URL";
            // 
            // txt_url
            // 
            this.txt_url.Location = new System.Drawing.Point(78, 13);
            this.txt_url.Name = "txt_url";
            this.txt_url.Size = new System.Drawing.Size(274, 20);
            this.txt_url.TabIndex = 1;
            // 
            // btn_download
            // 
            this.btn_download.Location = new System.Drawing.Point(361, 12);
            this.btn_download.Name = "btn_download";
            this.btn_download.Size = new System.Drawing.Size(75, 23);
            this.btn_download.TabIndex = 2;
            this.btn_download.Text = "Download";
            this.btn_download.UseVisualStyleBackColor = true;
            this.btn_download.Click += new System.EventHandler(this.btn_download_Click);
            // 
            // chk_audioOnly
            // 
            this.chk_audioOnly.AutoSize = true;
            this.chk_audioOnly.Location = new System.Drawing.Point(449, 16);
            this.chk_audioOnly.Name = "chk_audioOnly";
            this.chk_audioOnly.Size = new System.Drawing.Size(75, 17);
            this.chk_audioOnly.TabIndex = 4;
            this.chk_audioOnly.Text = "Audio only";
            this.chk_audioOnly.UseVisualStyleBackColor = true;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(14, 46);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(676, 323);
            this.dataGridView.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 381);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.chk_audioOnly);
            this.Controls.Add(this.btn_download);
            this.Controls.Add(this.txt_url);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "YouTube Downloader";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_url;
        private System.Windows.Forms.Button btn_download;
        private System.Windows.Forms.CheckBox chk_audioOnly;
        private System.Windows.Forms.DataGridView dataGridView;
    }
}

