using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

using Splash.IO.PORTS;

namespace Splash
{
    public class Environment
    {
        [DllImport("kernel32.dll")]
        private static extern UInt16 GetUserDefaultUILanguage();

        [DllImport("kernel32.dll")]
        private static extern UInt16 GetSystemDefaultLangID();

        [DllImport("kernel32.dll")]
        private static extern UInt16 GetUserDefaultLangID();

        /// <summary>  
        /// 用户缺省界面语言  
        /// </summary>  
        public static UInt16 UserDefaultUILanguage
        {
            get
            {
                return GetUserDefaultUILanguage();
            }
        }

        /// <summary>  
        /// 用户缺省语言  
        /// </summary>  
        public static UInt16 UserDefaultLangID
        {
            get
            {
                return GetUserDefaultLangID();
            }
        }

        /// <summary>  
        /// 系统缺省语言  
        /// </summary>  
        public static UInt16 SystemDefaultLangID
        {
            get
            {
                return GetSystemDefaultLangID();
            }
        }
    }
} 
