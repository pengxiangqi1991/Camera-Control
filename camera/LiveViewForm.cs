using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace camera
{
    public partial class LiveViewForm : Form
    {

        IntPtr cameraListRef = new IntPtr();
        IntPtr[] camera = new IntPtr[20];
        int cameraCount;
        PictureBox [] liveBox = new PictureBox[20];

        public LiveViewForm()
        {
            InitializeComponent();
        }       

        private void LiveViewForm_Load(object sender, EventArgs e)
        {
            cameraListRef = cameraControl.GetCameras(out cameraCount);
            camera = cameraControl.OpenCameras(cameraListRef, cameraCount);
            liveViewBoxList();
            LiveRefreshTimer.Start();    
        }


        private void LiveRefreshTimer_Tick(object sender, EventArgs e)
        {
            Bitmap[] _bmp = new Bitmap[20];
            for (int i = 0; i < cameraCount; i++)
            {
                _bmp[i] = cameraControl.DownloadEvf(camera[i]);
                liveBox[i].Image = _bmp[i];
            }

        }

        public void liveViewBoxList()
        {
            int j;
            string liveBoxName = "LiveBox";
            for (int i = 0; i < cameraCount; i++)
            {
                j = i+1;
                liveBox[i] = (this.Controls.Find(liveBoxName + j, true))[0] as PictureBox;
            }
        }

    }
}
