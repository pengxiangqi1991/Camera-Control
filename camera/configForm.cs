using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDSDKLib;

namespace camera
{
    public partial class configForm : Form
    {
        public configForm()
        {
            InitializeComponent();
            iniFilePaths = AppDomain.CurrentDomain.BaseDirectory.ToString()+"11.ini";
          
        }

        public static string iniFilePaths;                             //"d:\\pic\\11.ini";

        private void button_applyconfig_Click(object sender, EventArgs e)
        {
            IniFile iniFile = new IniFile(iniFilePaths);
            string session = "CameraID";
            string idboxname = "textBox_id";
            string nameboxname = "textBox_name";
            for (int i = 0; i < cameraControl.MAX_CAMERA_NUM; i++)
            {
                int j = i + 1;
                TextBox cameraIDbox = (this.Controls.Find(idboxname + j, true))[0] as TextBox;
                TextBox camerNamebox = (this.Controls.Find(nameboxname + j, true))[0] as TextBox;
                string cameraid = cameraIDbox.Text;
                string nameofID = camerNamebox.Text;
                iniFile.IniWriteValue(session, cameraid, nameofID);
            }      
        }

        private void button_updatelist_Click(object sender, EventArgs e)
        {
            IntPtr cameraListRef = new IntPtr();
            IntPtr[] camera = new IntPtr[20];
            EDSDK.EdsDeviceInfo[] deviceInfo;
            int cameraCount;
            cameraListRef = cameraControl.GetCameras(out cameraCount);
            camera = cameraControl.OpenCameras(cameraListRef, cameraCount);
            deviceInfo = cameraControl.GetDeviceInfo(camera, cameraCount);

            string session = "CameraID";
            IniFile iniFile = new IniFile(iniFilePaths);
            string idboxname = "textBox_id";
            string nameboxname = "textBox_name";
            for (int i = 0; i < cameraCount; i++)
            {
                int j = i + 1;
                TextBox cameraIDbox = (this.Controls.Find(idboxname + j, true))[0] as TextBox;
                cameraIDbox.Text = deviceInfo[i].szPortName.ToString();
                string nameofID = iniFile.IniReadValue(session, cameraIDbox.Text);
                TextBox camerNamebox = (this.Controls.Find(nameboxname + j, true))[0] as TextBox;
                camerNamebox.Text = nameofID;
            }
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
