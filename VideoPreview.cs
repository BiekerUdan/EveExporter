using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;


namespace EveExporter
{
    public partial class VideoPreview : UserControl
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

        public VideoPreview()
        {
            InitializeComponent();

            Core.Initialize();

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

        }

        public void SetVideo(String videopath)
        {
            // Play video
            using (var media = new Media(_libVLC, new Uri(videopath)))
            {
                var videoView = new VideoView { MediaPlayer = _mediaPlayer };
                videoView.Dock = DockStyle.Fill;
                Controls.Add(videoView);
                videoView.MediaPlayer.Play(media);
            }
        }
    }
}
