using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Management;
using Splash;
using Splash.IO.PORTS;

using EDSDKLib;
using CCCBSv2;

using System.Runtime.InteropServices;
using System.IO;
using camera;
using System.Threading;

namespace camera
{
    public partial class Form1 : Form
    {
        #region EDSDK
        int cameraCount;
        IntPtr cameraListRef = new IntPtr();       
        IntPtr[] camera = new IntPtr[20];
        EDSDK.EdsDeviceInfo[] DeviceInfo = new EDSDK.EdsDeviceInfo[20];

        string hardDisk, folder, folder2, folder3, midText;
        
        #endregion

        
        public Form1()
        {
            InitializeComponent();
            myConsoleWindow.CreateConsole();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cameraControl.InitCamera();
            hardDisk = "D";
            folder = "pic";
            midText = "";
            textBox_picharddisk.Text = hardDisk;
            textBox_picfolder.Text = folder;
            textBox_picmidtext.Text = midText;
            cameraControl.PicDest = hardDisk + ":\\" + folder + "\\" + midText;
            if (!Directory.Exists("D:\\pic"))//若文件夹不存在则新建文件夹   
            {
                Directory.CreateDirectory("D:\\pic"); //新建文件夹   
            }
            cameraListRef = cameraControl.GetCameras(out cameraCount);
            camera = cameraControl.OpenCameras(cameraListRef, cameraCount);
            for (int i = 0; i < cameraCount; i++)
            {
                cameraControl.LiveView(camera[i]);
            }
           
            cameraControl.ReadCameraTv(camera[0]);
            cameraControl.ReadCameraAv(camera[0]);
            cameraControl.ReadCameraISO_Speed(camera[0]);
            cameraControl.ReadCameraAF_Mode(camera[0]);
            cameraControl.ReadCameraExposure_Comperation(camera[0]);
            cameraControl.ReadCameraFlash_Comperation(camera[0]);
            //cameraControl.ReadCameraSize_Quality(camera[0]);
            cameraControl.ReadCameraWhite_Balance(camera[0]);
            cameraControl.ReadCameraMetering_Mode(camera[0]);
            cameraControl.ReadCameraDrive_Mode(camera[0]);
            cameraControl.ReadCameraPicture_Style(camera[0]);
            Console.WriteLine("Init success");
            
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            hardDisk = textBox_picharddisk.Text;
            folder = textBox_picfolder.Text;
            folder2 = textBox_picfolder2.Text;
            folder3 = textBox_picfolder3.Text;
            midText = textBox_picmidtext.Text;

            string fileDir = "D:\\pic";
            
            if ((folder2 != null)&(folder3==null))
            {
                fileDir = hardDisk + ":\\" + folder + "\\" + folder2;
            }
            if((folder2==null)&(folder3!=null))
            {
                fileDir = hardDisk + ":\\" + folder + "\\" + folder3;
            }
            if((folder2!=null)&(folder3!=null))
            {
                fileDir = hardDisk + ":\\" + folder + "\\" + folder2 
                                                  + "\\" + folder3;
            }
            if((folder2==null)&(folder3==null))
            {
                fileDir = hardDisk + ":\\" + folder;
            }

            cameraControl.PicDest = fileDir + "\\" + midText;
            if (!Directory.Exists(fileDir))//若文件夹不存在则新建文件夹   
            {
                Directory.CreateDirectory(fileDir); //新建文件夹   
            }

        }

        private void button_Init_Click(object sender, EventArgs e)
        {         
            cameraListRef = cameraControl.GetCameras(out cameraCount);
            camera = cameraControl.OpenCameras(cameraListRef, cameraCount);
            DeviceInfo = cameraControl.GetDeviceInfo(camera, cameraCount); 
        }

        private void button_TakePhoto_Click(object sender, EventArgs e)
        {
            Bitmap _bmp = new Bitmap(1050,700);
            cameraControl.TakePhotos(camera, cameraCount);
            _bmp = cameraControl.DownloadPreview(camera[0]);
            ViewBox1.Image = _bmp;
        }

        private unsafe void button_Download_Click(object sender, EventArgs e)
        {
            cameraControl.DownloadImage(camera, cameraCount);
        }

        private void button_closesession_Click(object sender, EventArgs e)
        {
            cameraControl.CloseCameras(camera, cameraCount); 
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            configForm cf = new configForm();
            cf.Show();
        }

        //private void LiveViewButton_Click(object sender, EventArgs e)
        //{

        //    cameraControl.LiveView(camera[0]);
        //    //DownloadThread.Start();//开始线程
        //    relaseTimer.Start();         
        //}

        //private void relaseTimer_Tick_1(object sender, EventArgs e)
        //{
        //    Bitmap _newbmp = cameraControl.DownloadEvf(camera[0]);
        //    LiveViewBox1.Image = _newbmp;
        //}

        private void viewAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LiveViewForm lf = new LiveViewForm();
            lf.Show();
        }

    }
}
