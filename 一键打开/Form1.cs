using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Allods
{
    public partial class Form1 : Form
    {
        public static IniFiles ini;
        private string url;
        private int Login_X;
        private int Login_Y;

        private int display_X;
        private int display_Y;

        private int delaytime;

        [System.Runtime.InteropServices.DllImport("user32")]
        public extern static void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);
        [System.Runtime.InteropServices.DllImport("user32")]
        static extern bool SetCursorPos(int X, int Y);

        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        const int MOUSEEVENTF_LEFTUP = 0x0004;

        const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        public Form1()
        {
            InitializeComponent();
            CheckIniConfig();
            InitConfig();
            this.Load += Form1_Load;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            Thread.Sleep(delaytime*1000);
            //调用系统默认的浏览器 
            System.Diagnostics.Process.Start("explorer.exe", url);
            Point selectpoint = new Point();
            selectpoint.X = Login_X;
            selectpoint.Y = Login_Y;
            Thread.Sleep(3000);
            mouse_click(selectpoint);
            Thread.Sleep(3000);
            SendKeys.SendWait("{F11}");
            Thread.Sleep(3000);
            selectpoint.X = display_X;
            selectpoint.Y = display_Y;
            mouse_click(selectpoint);
            Application.Exit();  
        }


        private bool CheckIniConfig()
        {
            try
            {
                string iniPath = Path.Combine(Application.StartupPath, "Config.ini");
                ini = new IniFiles(iniPath);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private void InitConfig()
        {
            try
            {
                Login_X = Convert.ToInt32(ini.ReadValue("Coordinate", "LoginX"));
                Login_Y = Convert.ToInt32(ini.ReadValue("Coordinate", "LoginY"));
                display_X = Convert.ToInt32(ini.ReadValue("Coordinate", "displayX"));
                display_Y = Convert.ToInt32(ini.ReadValue("Coordinate", "displayY"));
                url = ini.ReadValue("URL", "mainurl");
                delaytime= Convert.ToInt32(ini.ReadValue("DelayTime", "delay"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //鼠标点击位置
        private void mouse_click(Point p1)
        {
            SetCursorPos(p1.X, p1.Y);
            mouse_event((int)(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_ABSOLUTE), p1.X, p1.Y, 0, IntPtr.Zero);
            mouse_event((int)(MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE), p1.X, p1.Y, 0, IntPtr.Zero);
        }



    }
}
