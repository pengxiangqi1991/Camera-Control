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

using System.Runtime.InteropServices;
using System.IO;
using EDSDKLib;
using camera;

namespace camera
{
    class cameraControl
    {
        
        public static int MAX_CAMERA_NUM = 2;
        public static string PicDest = "D:\\pic\\";
        public static string[] err_str = new string[100];
        public static int err_str_count = 4;

        public static uint InitCamera()
        {
            uint success ;
            success = EDSDK.EdsInitializeSDK();
            if (success != EDSDK.EDS_ERR_OK)
            {
                Console.WriteLine("InitializeSDK failed");
            }
            else
            {
                Console.WriteLine("InitializeSDK success");
            }
            return success;
        }

        public static IntPtr GetCameras(out int Count)
        {

            uint success;
            IntPtr CameraListRef = new IntPtr();

            success = EDSDK.EdsGetCameraList(out CameraListRef);
            if (success != EDSDK.EDS_ERR_OK)
            {
                Console.WriteLine("GetCameraList failed");
            }
            else
            {
                Console.WriteLine("GetCameraList success");
            }

            success = EDSDK.EdsGetChildCount(CameraListRef, out Count);
            if (success != EDSDK.EDS_ERR_OK)
            {
                Console.WriteLine("GetChildCount failed");
            }
            else
            {
                Console.WriteLine("GetChildCount success\t"+"child num is:"+Count.ToString());
            }
            return CameraListRef;
        }

        public static IntPtr[] OpenCameras(IntPtr CameraListRef, int Count)
        {
            int i;
            uint success;

            IntPtr[] Camera = new IntPtr[20];
            IntPtr _camera = new IntPtr();
            for (i = 0; i < Count; i++)
            {
                EDSDK.EdsGetChildAtIndex(CameraListRef,i,out _camera);
                Camera[i] = _camera;
                success = EDSDK.EdsOpenSession(_camera);
                if (success != EDSDK.EDS_ERR_OK)
                {
                    Console.WriteLine("OpenCamera failed\t" + "failed camera is:" + i.ToString() + "\t" + success.ToString());
                }
                else
                {
                    Console.WriteLine("OpenCamera successs\t" + i.ToString());
                }

            }
            return Camera;
            
        }

        public static EDSDK.EdsDeviceInfo[] GetDeviceInfo(IntPtr[] camera, int Count)
        {
            EDSDK.EdsDeviceInfo[] DeviceInfo = new EDSDK.EdsDeviceInfo[Count];
            EDSDK.EdsDeviceInfo deviceInfo;
            for (int i = 0; i < Count; i++)
            {
                EDSDK.EdsGetDeviceInfo(camera[i],out deviceInfo);
                DeviceInfo[i] = deviceInfo;
  
            }
            return DeviceInfo;
        }

        public static uint TakePhotos(IntPtr[] camera, int Count)
        {
            
            uint err = 0;
            uint success;
            for (int i = 0; i < Count; i++)
            {
                success = EDSDK.EdsSendCommand(camera[i], EDSDK.CameraCommand_TakePicture, 0);  //Take Picture
                if (success != EDSDK.EDS_ERR_OK)
                {
                    Console.WriteLine( "Fail to takePic,camera num is:" + i.ToString()
                                                         + "\t" + success.ToString()); 
                    err = (uint)i;
                }
            }
            return err;
        }

        public static unsafe void DownloadImage(IntPtr[] camera,int Count)
        {
            IntPtr sdRef = IntPtr.Zero;
            IntPtr dicmRef = IntPtr.Zero;
            IntPtr canonRef = IntPtr.Zero;
            int nFiles;
            IniFile iniFile = new IniFile(configForm.iniFilePaths);

            for (int i = 0; i < Count; i++)
            {
                EDSDK.EdsGetChildAtIndex(camera[i], 0, out sdRef);
                EDSDK.EdsGetChildAtIndex(sdRef, 0, out dicmRef);
                EDSDK.EdsGetChildAtIndex(dicmRef, 0, out canonRef);
                EDSDK.EdsGetChildCount(canonRef, out nFiles);

                string dest = PicDest;

                //for (int i = 0; i < 5; i++)
                {
                    IntPtr fileRef = IntPtr.Zero;
                    EDSDK.EdsDirectoryItemInfo fileInfo;
                    #region Get Directory and download
                    try
                    {
                        EDSDK.EdsGetChildAtIndex(canonRef, (nFiles - 1), out fileRef);
                        EDSDK.EdsGetDirectoryItemInfo(fileRef, out fileInfo);

                        IntPtr fStream = IntPtr.Zero;  //it's a cannon sdk file stream, not a managed stream
                        uint fSize = fileInfo.Size;
                        #region DownLoad
                        try
                        {

                            EDSDK.EdsCreateMemoryStream(fSize, out fStream);
                            EDSDK.EdsDownload(fileRef, fSize, fStream);
                            EDSDK.EdsDownloadComplete(fileRef);
                            byte[] buffer = new byte[fSize];
                            IntPtr imgLocation;
                            //uint _imageLen;

                            if (EDSDK.EdsGetPointer(fStream, out imgLocation) == EDSDK.EDS_ERR_OK)
                            {
                                #region save
                                Marshal.Copy(imgLocation, buffer, 0, (int)fSize - 1);
                                EDSDK.EdsDeviceInfo deviceInfo;
                                EDSDK.EdsGetDeviceInfo(camera[i],out deviceInfo);
                                string deviceName;
                                deviceName = iniFile.IniReadValue("CameraID", deviceInfo.szPortName);    
                                File.WriteAllBytes(dest + deviceName + "__" + fileInfo.szFileName, buffer);
                                //EDSDK.EdsGetLength(fStream, out _imageLen);
                                //UnmanagedMemoryStream ums = new UnmanagedMemoryStream((byte*)imgLocation.ToPointer(), _imageLen, _imageLen, FileAccess.Read);
                                //Bitmap _bmp = new Bitmap(ums, true);
                                //pictureBox1.Image = _bmp;
                                //_bmp.Save("new1", System.Drawing.Imaging.ImageFormat.Jpeg);
                                #endregion

                            }
                        }
                        finally
                        {
                            EDSDK.EdsRelease(fStream);
                            Console.WriteLine("DownloadSuccess\t" + i.ToString());
                        }

                        #endregion
                    }
                    finally
                    {
                        EDSDK.EdsRelease(fileRef);
                    }
                    #endregion
                }
            }
        }

        public static void CloseCameras(IntPtr[] Camera, int Count)
        {
            int i;
            uint success;
            IntPtr _camera = new IntPtr();
            for (i = 0; i < Count; i++)
            {

                _camera = Camera[i];
                success = EDSDK.EdsCloseSession(_camera);
                if (success != EDSDK.EDS_ERR_OK)
                {
                    Console.WriteLine("CloseCamera failed\t\t" + success.ToString());
                }
                else
                {
                    Console.WriteLine("CloseCamera successs");
                }

            }
        }


        public static void LiveView(IntPtr Camera)
        {
            uint success;
            uint _data;
            //获取EVF输出设备信息
            success = EDSDK.EdsGetPropertyData(Camera,EDSDK.PropID_Evf_OutputDevice,0, out _data);
            IntPtr _device = new IntPtr();
            if (success == EDSDK.EDS_ERR_OK)
            {
                _device = new IntPtr(_data | EDSDK.EvfOutputDevice_PC);
                success = EDSDK.EdsSetPropertyData(Camera,EDSDK.PropID_Evf_OutputDevice,0,Marshal.SizeOf(_device),_device);
            }   
        }

        public unsafe static Bitmap DownloadPreview(IntPtr Camera)
        {
            uint success;
            Bitmap _bmp, _newbmp;
            UnmanagedMemoryStream ums;
            //while (true)
            {
                IntPtr _stream = IntPtr.Zero;
                IntPtr _image = new IntPtr();
                IntPtr _imageData = new IntPtr();
                uint _imageLen;
                int _width = 1050, _height = 700;
                _newbmp = new Bitmap(_width, _height);

                success = EDSDK.EdsCreateMemoryStream(0, out _stream);
                if (success == EDSDK.EDS_ERR_OK)
                {
                    success = EDSDK.EdsCreateEvfImageRef(_stream, out _image);
                }
                if (success == EDSDK.EDS_ERR_OK)
                {
                    success = EDSDK.EdsDownloadEvfImage(Camera, _image);
                }
                if (success == EDSDK.EDS_ERR_OK)
                {
                    EDSDK.EdsGetPointer(_stream, out _imageData);
                    EDSDK.EdsGetLength(_stream, out _imageLen);

                    ums = new UnmanagedMemoryStream((byte*)_imageData.ToPointer(), _imageLen, _imageLen, FileAccess.Read);

                    _bmp = new Bitmap(ums, true);
                    _newbmp = new Bitmap(_bmp, _width, _height);
                    Console.WriteLine("bmp is download");

                }

                if (_stream != IntPtr.Zero)
                {
                    EDSDK.EdsRelease(_stream);
                    _stream = IntPtr.Zero;
                    Console.WriteLine("relase stream");
                }
                if (_image != IntPtr.Zero)
                {
                    EDSDK.EdsRelease(_image);
                    _image = IntPtr.Zero;
                    Console.WriteLine("relase image");
                }
            }
            return _newbmp;
        }

        public unsafe static Bitmap DownloadEvf(IntPtr Camera)
        {
            uint success;
            Bitmap _bmp, _newbmp;

            UnmanagedMemoryStream ums;
            //            while (true)
            {
                IntPtr _stream = IntPtr.Zero;
                IntPtr _image = new IntPtr();
                IntPtr _imageData = new IntPtr();
                uint _imageLen;
                int _width = 500, _height = 330;
                _newbmp = new Bitmap(_width, _height);


                success = EDSDK.EdsCreateMemoryStream(0, out _stream);
                if (success == EDSDK.EDS_ERR_OK)
                {
                    success = EDSDK.EdsCreateEvfImageRef(_stream, out _image);
                }
                if (success == EDSDK.EDS_ERR_OK)
                {
                    success = EDSDK.EdsDownloadEvfImage(Camera, _image);
                }
                if (success == EDSDK.EDS_ERR_OK)
                {
                    EDSDK.EdsGetPointer(_stream, out _imageData);
                    EDSDK.EdsGetLength(_stream, out _imageLen);

                    ums = new UnmanagedMemoryStream((byte*)_imageData.ToPointer(), _imageLen, _imageLen, FileAccess.Read);

                    _bmp = new Bitmap(ums, true);
                    _newbmp = new Bitmap(_bmp, _width, _height);

                    //LiveViewBox是定义的一个pictureBox 控件
                    //LiveViewBox1.Image = _newbmp;
                }

                if (_stream != IntPtr.Zero)
                {
                    EDSDK.EdsRelease(_stream);
                    _stream = IntPtr.Zero;
                }
                if (_image != IntPtr.Zero)
                {
                    EDSDK.EdsRelease(_image);
                    _image = IntPtr.Zero;
                }
            }
            return _newbmp;
        }


        public static void ReadCameraTv(IntPtr Camera)
        {
            uint success;
            uint Tv;
            string Tv_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_Tv, 0, out Tv);
            #region switch Tv
            switch (Tv)
            {
                case 0x0c:
                    Tv_value = "Bulb";
                    break;
                case 0x10:
                    Tv_value = "30\"";
                    break;
                case 0x13:
                    Tv_value = "25\"";
                    break;
                case 0x14:
                    Tv_value = "20\"";
                    break;
                case 0x15:
                    Tv_value = "20\"(1/3)";
                    break;
                case 0x18:
                    Tv_value = "15\"";
                    break;
                case 0x1b:
                    Tv_value = "13\"";
                    break;
                case 0x1c:
                    Tv_value = "10\"";
                    break;
                case 0x1d:
                    Tv_value = "10\"(1/3)";
                    break;
                case 0x20:
                    Tv_value = "8\"";
                    break;
                case 0x23:
                    Tv_value = "6\"(1/3)";
                    break;
                case 0x24:
                    Tv_value = "6\"";
                    break;
                case 0x25:
                    Tv_value = "5\"";
                    break;
                case 0x28:
                    Tv_value = "4\"";
                    break;
                case 0x2b:
                    Tv_value = "3\"2";
                    break;
                case 0x2c:
                    Tv_value = "3\"";
                    break;
                case 0x2d:
                    Tv_value = "2\"5";
                    break;
                case 0x30:
                    Tv_value = "2\"";
                    break;
                case 0x33:
                    Tv_value = "1\"6";
                    break;
                case 0x34:
                    Tv_value = "1\"5";
                    break;
                case 0x35:
                    Tv_value = "1\"3";
                    break;
                case 0x38:
                    Tv_value = "1\"";
                    break;
                case 0x3b:
                    Tv_value = "0\"8";
                    break;
                case 0x3c:
                    Tv_value = "0\"7";
                    break;
                case 0x3d:
                    Tv_value = "0\"6";
                    break;
                case 0x40:
                    Tv_value = "0\"5";
                    break;
                case 0x43:
                    Tv_value = "0\"4";
                    break;
                case 0x44:
                    Tv_value = "0\"3";
                    break;
                case 0x45:
                    Tv_value = "0\"3(1/3)";
                    break;
                case 0x48:
                    Tv_value = "1/4";
                    break;
                case 0x4b:
                    Tv_value = "1/5";
                    break;
                case 0x4c:
                    Tv_value = "1/6";
                    break;
                case 0x4d:
                    Tv_value = "1/6(1/3)";
                    break;
                case 0x50:
                    Tv_value = "1/8";
                    break;
                case 0x53:
                    Tv_value = "1/10(1/3)";
                    break;
                case 0x54:
                    Tv_value = "1/10";
                    break;
                case 0x55:
                    Tv_value = "1/13";
                    break;
                case 0x58:
                    Tv_value = "1/15";
                    break;
                case 0x5b:
                    Tv_value = "1/20(1/3)";
                    break;
                case 0x5c:
                    Tv_value = "1/20";
                    break;
                case 0x5d:
                    Tv_value = "1/25";
                    break;
                case 0x60:
                    Tv_value = "1/30";
                    break;
                case 0x63:
                    Tv_value = "1/40";
                    break;
                case 0x64:
                    Tv_value = "1/45";
                    break;
                case 0x65:
                    Tv_value = "1/50";
                    break;
                case 0x68:
                    Tv_value = "1/60";
                    break;
                case 0x6b:
                    Tv_value = "1/80";
                    break;
                case 0x6c:
                    Tv_value = "1/90";
                    break;
                case 0x6d:
                    Tv_value = "1/100";
                    break;
                case 0x70:
                    Tv_value = "1/125";
                    break;
                case 0x73:
                    Tv_value = "1/160";
                    break;
                case 0x74:
                    Tv_value = "1/180";
                    break;
                case 0x75:
                    Tv_value = "1/200";
                    break;
                case 0x78:
                    Tv_value = "1/250";
                    break;
                case 0x7b:
                    Tv_value = "1/320";
                    break;
                case 0x7c:
                    Tv_value = "1/350";
                    break;
                case 0x7d:
                    Tv_value = "1/400";
                    break;
                case 0x80:
                    Tv_value = "1/500";
                    break;
                case 0x83:
                    Tv_value = "1/640";
                    break;
                case 0x84:
                    Tv_value = "1/750";
                    break;
                case 0x85:
                    Tv_value = "1/800";
                    break;
                case 0x88:
                    Tv_value = "1/1000";
                    break;
                case 0x8b:
                    Tv_value = "1/1250";
                    break;
                case 0x8c:
                    Tv_value = "1/1500";
                    break;
                case 0x8d:
                    Tv_value = "1/1600";
                    break;
                case 0x90:
                    Tv_value = "1/2000";
                    break;
                case 0x93:
                    Tv_value = "1/2500";
                    break;
                case 0x94:
                    Tv_value = "1/3000";
                    break;
                case 0x95:
                    Tv_value = "1/3200";
                    break;
                case 0x98:
                    Tv_value = "1/4000";
                    break;
                case 0x9b:
                    Tv_value = "1/5000";
                    break;
                case 0x9c:
                    Tv_value = "1/6000";
                    break;
                case 0x9d:
                    Tv_value = "1/6400";
                    break;
                case 0xa0:
                    Tv_value = "1/8000";
                    break;
                case 0xffffffff:
                    Tv_value = "Not vaild";
                    break;
                default:
                    Tv_value = "err";
                    break;
            }
            #endregion
            //Console.WriteLine("GetPropertyData {0}",success);
            Console.WriteLine("Tv is \t\t\t{0}",Tv_value);
        }

        public static void ReadCameraAv(IntPtr Camera)
        {
            uint success;
            uint Av;
            string Av_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_Av, 0, out Av);
            #region switch Av
            switch (Av)
            {
                case 0x08:
                    Av_value = "1";
                    break;
                case 0x0b:
                    Av_value = "1.1";
                    break;
                case 0x0c:
                    Av_value = "1.2";
                    break;
                case 0x0d:
                    Av_value = "1.2(1/3)";
                    break;
                case 0x10:
                    Av_value = "1.4";
                    break;
                case 0x13:
                    Av_value = "1.6";
                    break;
                case 0x14:
                    Av_value = "1.8";
                    break;
                case 0x15:
                    Av_value = "1.8(1/3)";
                    break;
                case 0x18:
                    Av_value = "2";
                    break;
                case 0x1b:
                    Av_value = "2.2";
                    break;
                case 0x1c:
                    Av_value = "2.5";
                    break;
                case 0x1d:
                    Av_value = "2.5(1/3)";
                    break;
                case 0x20:
                    Av_value = "2.8";
                    break;
                case 0x23:
                    Av_value = "3.2";
                    break;
                case 0x24:
                    Av_value = "3.5";
                    break;
                case 0x25:
                    Av_value = "3.5(1/3)";
                    break;
                case 0x28:
                    Av_value = "4";
                    break;
                case 0x2b:
                    Av_value = "4.5";
                    break;
                case 0x2c:
                    Av_value = "4.5";
                    break;
                case 0x2d:
                    Av_value = "5.0";
                    break;
                case 0x30:
                    Av_value = "5.6";
                    break;
                case 0x33:
                    Av_value = "6.3";
                    break;
                case 0x34:
                    Av_value = "6.7";
                    break;
                case 0x35:
                    Av_value = "7.1";
                    break;
                case 0x38:
                    Av_value = "8";
                    break;
                case 0x3b:
                    Av_value = "9";
                    break;
                case 0x3c:
                    Av_value = "9.5";
                    break;
                case 0x3d:
                    Av_value = "10";
                    break;
                case 0x40:
                    Av_value = "11";
                    break;
                case 0x43:
                    Av_value = "13(1/3)";
                    break;
                case 0x44:
                    Av_value = "13";
                    break;
                case 0x45:
                    Av_value = "14";
                    break;
                case 0x48:
                    Av_value = "16";
                    break;
                case 0x4b:
                    Av_value = "18";
                    break;
                case 0x4c:
                    Av_value = "19";
                    break;
                case 0x4d:
                    Av_value = "20";
                    break;
                case 0x50:
                    Av_value = "22";
                    break;
                case 0x53:
                    Av_value = "25";
                    break;
                case 0x54:
                    Av_value = "27";
                    break;
                case 0x55:
                    Av_value = "29";
                    break;
                case 0x58:
                    Av_value = "32";
                    break;
                case 0x5b:
                    Av_value = "36";
                    break;
                case 0x5c:
                    Av_value = "38";
                    break;
                case 0x5d:
                    Av_value = "40";
                    break;
                case 0x60:
                    Av_value = "45";
                    break;
                case 0x63:
                    Av_value = "51";
                    break;
                case 0x64:
                    Av_value = "54";
                    break;
                case 0x65:
                    Av_value = "57";
                    break;
                case 0x68:
                    Av_value = "64";
                    break;
                case 0x6b:
                    Av_value = "72";
                    break;
                case 0x6c:
                    Av_value = "76";
                    break;
                case 0x6d:
                    Av_value = "80";
                    break;
                case 0x70:
                    Av_value = "91";
                    break;
                case 0xffffffff:
                    Av_value = "Not valid";
                    break;
                default:
                    Av_value = "err";
                    break;
            }
            #endregion
            //Console.WriteLine("GetPropertyData {0}", success);
            Console.WriteLine("Av is \t\t\t{0}", Av_value);
        }

        public static void ReadCameraISO_Speed(IntPtr Camera)
        {
            uint success;
            uint iso_speed;
            string iso_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_ISOSpeed, 0, out iso_speed);
            #region switch ISO
            switch(iso_speed)
            {
                case 0x28:
                    iso_value = "6";
                    break;
                case 0x30:
                    iso_value = "12";
                    break;
                case 0x38:
                    iso_value = "25";
                    break;
                case 0x40:
                    iso_value = "50";
                    break;
                case 0x48:
                    iso_value = "100";
                    break;
                case 0x4b:
                    iso_value = "125";
                    break;
                case 0x4d:
                    iso_value = "160";
                    break;
                case 0x50:
                    iso_value = "200";
                    break;
                case 0x53:
                    iso_value = "250";
                    break;
                case 0x55:
                    iso_value = "320";
                    break;
                case 0x58:
                    iso_value = "400";
                    break;
                case 0x5b:
                    iso_value = "500";
                    break;
                case 0x5d:
                    iso_value = "640";
                    break;
                case 0x60:
                    iso_value = "800";
                    break;
                case 0x63:
                    iso_value = "1000";
                    break;
                case 0x65:
                    iso_value = "1250";
                    break;
                case 0x68:
                    iso_value = "1600";
                    break;
                case 0x70:
                    iso_value = "3200";
                    break;
                case 0x78:
                    iso_value = "6400";
                    break;
                case 0x80:
                    iso_value = "12800";
                    break;
                case 0x88:
                    iso_value = "25600";
                    break;
                case 0x90:
                    iso_value = "52100";
                    break;
                case 0x98:
                    iso_value = "102400";
                    break;
                case 0xffffffff:
                    iso_value = "Not valid";
                    break;
                default:
                    iso_value = "err";
                    break;

            }
            #endregion

            //Console.WriteLine("GetPropertyData {0}", success);
            Console.WriteLine("iso is \t\t\t{0}", iso_value);
        }

        public static void ReadCameraAF_Mode(IntPtr Camera)
        {
            uint success;
            uint af;
            string af_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_AFMode, 0, out af);
            #region switch AF Mode
            switch (af)
            {
                case 0:
                    af_value = "One-Shot AF";
                    break;
                case 1:
                    af_value = "AI Servo AF";
                    break;
                case 2:
                    af_value = "AI Focus AF";
                    break;
                case 3:
                    af_value = "Manual AF";
                    break;
                case 0xffffffff:
                    af_value = "Not valid";
                    break;
                default:
                    af_value = "err";
                    break;
            }
            #endregion  
            //Console.WriteLine("GetPropertyData {0}", success);
            Console.WriteLine("AF Mode is \t\t{0}", af_value);
        }

        public static void ReadCameraExposure_Comperation(IntPtr Camera)
        {
            uint success;
            uint excom;
            string excom_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_ExposureCompensation, 0, out excom);
            #region switch excom
            switch (excom)
            {
                case 0x18:
                    excom_value = "+3";
                    break;
                case 0x15:
                    excom_value = "+2 2/3";
                    break;
                case 0x14:
                    excom_value = "+2 1/2";
                    break;
                case 0x13:
                    excom_value = "+2 1/3";
                    break;
                case 0x10:
                    excom_value = "+2";
                    break;
                case 0x0d:
                    excom_value = "+1 2/3";
                    break;
                case 0x0c:
                    excom_value = "+1 1/2";
                    break;
                case 0x0b:
                    excom_value = "+1 1/3";
                    break;
                case 0x08:
                    excom_value = "+1";
                    break;
                case 0x05:
                    excom_value = "+2/3";
                    break;
                case 0x04:
                    excom_value = "+1/2";
                    break;
                case 0x03:
                    excom_value = "+1/3";
                    break;
                case 0x00:
                    excom_value = "+0";
                    break;
                case 0xfd:
                    excom_value = "-1/3";
                    break;
                case 0xfc:
                    excom_value = "-1/2";
                    break;
                case 0xfb:
                    excom_value = "-2/3";
                    break;
                case 0xf8:
                    excom_value = "-1";
                    break;
                case 0xf5:
                    excom_value = "-1 1/3";
                    break;
                case 0xf4:
                    excom_value = "-1 1/2";
                    break;
                case 0xf3:
                    excom_value = "-1 2/3";
                    break;
                case 0xf0:
                    excom_value = "-2";
                    break;
                case 0xed:
                    excom_value = "-2 1/3";
                    break;
                case 0xec:
                    excom_value = "-2 1/2";
                    break;
                case 0xeb:
                    excom_value = "-2 2/3";
                    break;
                case 0xe8:
                    excom_value = "-3";
                    break;
                case 0xffffffff:
                    excom_value = "Not valid";
                    break;
                default:
                    excom_value = "err";
                    break;
            }
            #endregion


            //Console.WriteLine("GetPropertyData {0}", success);
            Console.WriteLine("ExposureCompensation is {0}", excom_value);
        }

        public static void ReadCameraFlash_Comperation(IntPtr Camera)
        {
            uint success;
            uint flcom;
            string flcom_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_FlashCompensation, 0, out flcom);
            #region switch Flash Compensation
            switch(flcom)
            {
                case 0x18:
                    flcom_value = "+3";
                    break;
                case 0x15:
                    flcom_value = "+2 2/3";
                    break;
                case 0x14:
                    flcom_value = "+2 1/2";
                    break;
                case 0x13:
                    flcom_value = "+2 1/3";
                    break;
                case 0x10:
                    flcom_value = "+2";
                    break;
                case 0x0d:
                    flcom_value = "+1 2/3";
                    break;
                case 0x0c:
                    flcom_value = "+1 1/2";
                    break;
                case 0x0b:
                    flcom_value = "+1 1/3";
                    break;
                case 0x08:
                    flcom_value = "+1";
                    break;
                case 0x05:
                    flcom_value = "+2/3";
                    break;
                case 0x04:
                    flcom_value = "+1/2";
                    break;
                case 0x03:
                    flcom_value = "+1/3";
                    break;
                case 0x00:
                    flcom_value = "+0";
                    break;
                case 0xfd:
                    flcom_value = "-1/3";
                    break;
                case 0xfc:
                    flcom_value = "-1/2";
                    break;
                case 0xfb:
                    flcom_value = "-2/3";
                    break;
                case 0xf8:
                    flcom_value = "-1";
                    break;
                case 0xf5:
                    flcom_value = "-1 1/3";
                    break;
                case 0xf4:
                    flcom_value = "-1 1/2";
                    break;
                case 0xf3:
                    flcom_value = "-1 2/3";
                    break;
                case 0xf0:
                    flcom_value = "-2";
                    break;
                case 0xed:
                    flcom_value = "-2 1/3";
                    break;
                case 0xec:
                    flcom_value = "-2 1/2";
                    break;
                case 0xeb:
                    flcom_value = "-2 2/3";
                    break;
                case 0xe8:
                    flcom_value = "-3";
                    break;
                case 0xffffffff:
                    flcom_value = "Not valid";
                    break;
                default:
                    flcom_value = "err";
                    break;
            }
            #endregion

            //Console.WriteLine("GetPropertyData {0}", success);
            Console.WriteLine("Flash Compensation is \t{0}", flcom_value);
        }

        //----------------unfinish---------------------------//
        public static void ReadCameraSize_Quality(IntPtr Camera)
        {
            uint success;
            uint sq;
            string sq_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_ImageQuality, 0, out sq);
            #region switch ImageQuality
            switch(sq)
            {

                default:
                    sq_value = "err";
                    break;
            }
            #endregion

            //Console.WriteLine("GetPropertyData {0}", success);
            Console.WriteLine("Size/Quality is \t{0}", sq_value);
        }

        public static void ReadCameraWhite_Balance(IntPtr Camera)
        {
            uint success;
            uint wb;
            string wb_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_WhiteBalance, 0, out wb);
            #region switch White Balance
            switch(wb)
            {
                case 0:
                    wb_value = "Auto";
                    break;
                case 1:
                    wb_value = "Daylight";
                    break;
                case 2:
                    wb_value = "Cloudy";
                    break;
                case 3:
                    wb_value = "Tungsten";
                    break;
                case 4:
                    wb_value = "Fluorescent";
                    break;
                case 5:
                    wb_value = "Flash";
                    break;
                case 6:
                    wb_value = "Manual";
                    break;
                case 8:
                    wb_value = "Shade ";
                    break;
                case 9:
                    wb_value = "Color temperature";
                    break;
                case 10:
                    wb_value = "Custom white balance: PC-1";
                    break;
                case 11:
                    wb_value = "Custom white balance: PC-2";
                    break;
                case 12:
                    wb_value = "Custom white balance: PC-3";
                    break;
                case 15:
                    wb_value = "Manual 2";
                    break;
                case 16:
                    wb_value = "Manual 3";
                    break;
                case 18:
                    wb_value = "Manual 4";
                    break;
                case 19:
                    wb_value = "Manual 5";
                    break;
                case 20:
                    wb_value = "Custom white balance: PC-4";
                    break;
                case 21:
                    wb_value = "Custom white balance: PC-5";
                    break;
                default:
                    wb_value = "err";
                    break;
            }
            #endregion  

            //Console.WriteLine("GetPropertyData {0}", success);
            Console.WriteLine("WhiteBalance Mode is \t{0}", wb_value);
        }

        public static void ReadCameraMetering_Mode(IntPtr Camera)
        {
            uint success;
            uint mm;
            string mm_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_MeteringMode, 0, out mm);
            #region switch Metering Mode
            switch(mm)
            {
                case 1:
                    mm_value = "Spot metering";
                    break;
                case 3:
                    mm_value = "Evaluative metering";
                    break;
                case 4:
                    mm_value = "Partial metering";
                    break;
                case 5:
                    mm_value = "Center-weighted averaging metering";
                    break;
                case 0xffffffff:
                    mm_value = "Not valid";
                    break;

                default:
                    mm_value = "err";
                    break;
            }
            #endregion

            //Console.WriteLine("GetPropertyData {0}", success);
            Console.WriteLine("Metering Mode is \t{0}", mm_value);
        }

        public static void ReadCameraDrive_Mode(IntPtr Camera)
        {
            uint success;
            uint dm;
            string dm_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_DriveMode, 0, out dm);
            #region switch Driver Mode
            switch(dm)
            {   
                case 0x00:
                    dm_value = "Single-Frame Shooting";
                    break;
                case 0x01:
                    dm_value = "Continuous Shooting";
                    break;
                case 0x02:
                    dm_value = "Video";
                    break;
                case 0x03:
                    dm_value = "Not used";
                    break;
                case 0x04:
                    dm_value = "High-Speed Continuous Shooting";
                    break;                
                case 0x05:
                    dm_value = "Low-Speed Continuous Shooting";
                    break;
                case 0x06:
                    dm_value = "Silent single shooting";
                    break;
                case 0x07:
                    dm_value = "10-Sec Self-Time r plus continuous shots";
                    break;
                case 0x10:
                    dm_value = "10-Sec Self-Timer";
                    break;
                case 0x11:
                    dm_value = "2-Sec Sel f-Timer";
                    break;
                default:
                    dm_value = "err";
                    break;
            }
            #endregion
            //Console.WriteLine("GetPropertyData {0}", success);
            Console.WriteLine("Driver Mode is \t\t{0}", dm_value);
        }

        public static void ReadCameraPicture_Style(IntPtr Camera)
        {
            uint success;
            uint ps;
            string ps_value;
            success = EDSDK.EdsGetPropertyData(Camera, EDSDK.PropID_PictureStyle, 0, out ps);
            #region switch Picture Style
            switch(ps)
            {
                case 0x81:
                    ps_value = "Standard";
                    break;
                case 0x82:
                    ps_value = "Portrait";
                    break;
                case 0x83:
                    ps_value = "Landscape";
                    break;
                case 0x84:
                    ps_value = "Neutral";
                    break;
                case 0x85:
                    ps_value = "Faithful";
                    break;
                case 0x86:
                    ps_value = "Monochrome";
                    break;
                case 0x87:
                    ps_value = "Auto";
                    break;
                case 0x41:
                    ps_value = "Computer Setting 1";
                    break;
                case 0x42:
                    ps_value = "Computer Setting 2";
                    break;
                case 0x43:
                    ps_value = "Computer Setting 3";
                    break;
                default:
                    ps_value = "err";
                    break;
            }
            #endregion
            //Console.WriteLine("GetPropertyData {0}", success);
            Console.WriteLine("Picture Style is \t{0}", ps_value);
        }
    }
}
