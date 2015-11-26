using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.IO;
using System.Collections;


namespace camera
{
    class IniFile
    {
        public string inipath;
        //声明读写INI文件的API函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section">指向包含 Section 名称的字符串</param>
        /// <param name="key">指向包含 Key 名称的字符串</param>
        /// <param name="val">要写的字符串</param>
        /// <param name="filePath">ini 文件的文件名</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        /// <summary>
        /// 从 ini 文件的某个 Section 取得一个 key 的字符串
        /// </summary>
        /// <param name="section">指向包含 Section 名称的字符串</param>
        /// <param name="key">指向包含 Key 名称的字符串</param>
        /// <param name="def">如果 Key 值没有找到，则返回缺省的字符串</param>
        /// <param name="retVal">返回字符串的缓冲区</param>
        /// <param name="size">缓冲区的长度</param>
        /// <param name="filePath">ini 文件的文件名</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>   
        /// 构造方法   
        /// </summary>   
        /// <param name="INIPath">文件路径</param>   
        public IniFile(string INIPath)
        {
            inipath = INIPath;
        }

        /// <summary>   
        /// 写入INI文件   
        /// </summary>   
        /// <param name="Section">项目名称(如 [TypeName] )</param>   
        /// <param name="Key">键</param>   
        /// <param name="Value">值</param>   
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }

        /// <summary>   
        /// 读出INI文件   
        /// </summary>   
        /// <param name="Section">项目名称(如 [TypeName] )</param>   
        /// <param name="Key">键</param>   
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        /// <summary>  
        /// 获取Section下所有的Keys  
        /// </summary>  
        /// <param name="sectionName"></param>  
        /// <returns></returns>  
        public ArrayList ReadKeys(string sectionName)
        {
            byte[] buffer = new byte[5120];
            int rel = GetPrivateProfileString(sectionName, null, "", buffer, buffer.GetUpperBound(0), inipath);
            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }

        /// <summary>   
        /// 验证文件是否存在   
        /// </summary>   
        /// <returns>布尔值</returns>   
        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }

    }
}
