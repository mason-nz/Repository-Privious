using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//添加用于Socket的类
using System.Net;
using System.Net.Sockets;
using System.Threading;
//添加win管理员判断类
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace CC2012_CarolFormsApp
{
    public class SYSTimeHelper
    {
        private static byte[] result = new byte[1024];
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;

            public void FromDateTime(DateTime dateTime)
            {
                wYear = (ushort)dateTime.Year;
                wMonth = (ushort)dateTime.Month;
                wDayOfWeek = (ushort)dateTime.DayOfWeek;
                wDay = (ushort)dateTime.Day;
                wHour = (ushort)dateTime.Hour;
                wMinute = (ushort)dateTime.Minute;
                wSecond = (ushort)dateTime.Second;
                wMilliseconds = (ushort)dateTime.Millisecond;
            }

            public DateTime ToDateTime()
            {
                return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond);
            }
        }
        //设定，获取系统时间,SetSystemTime()默认设置的为UTC时间，比北京时间少了8个小时。  
        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SYSTEMTIME time);
        public static void ClientB()
        {
            //设定服务器IP地址  
            IPAddress ip = IPAddress.Parse("192.168.8.221");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 8885)); //配置服务器IP与端口  
                Loger.Log4Net.Info("[SYSTimeHelper]连接服务器成功");
            }
            catch
            {
                Loger.Log4Net.Info("[SYSTimeHelper]连接服务器失败");
                return;
            }
            //通过clientSocket接收数据  
            int receiveLength = clientSocket.Receive(result);
            Loger.Log4Net.Info("[SYSTimeHelper]接收服务器消息：" + Encoding.ASCII.GetString(result, 0, receiveLength));
            #region 设置系统时间
            try
            {
                string receivemsg = Encoding.ASCII.GetString(result, 0, receiveLength);
                string[] arraymsg = receivemsg.Split('!');
                Loger.Log4Net.Info("[SYSTimeHelper]解析服务器时间：" + arraymsg[1]);
                string sendMessage = "...!";
                if (SetSystemTime(arraymsg[1]))
                {
                    sendMessage = "set time successful!";
                }
                else
                {
                    sendMessage = "set time failed!";
                }
                clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                Loger.Log4Net.Info("[SYSTimeHelper]向服务器发送消息：" + sendMessage + "时间：" + DateTime.Now.ToString());
            }
            catch (Exception e)
            {
                Loger.Log4Net.Info("[SYSTimeHelper]设置本机时间失败，errorMessage:" + e.Message);
                Loger.Log4Net.Info("[SYSTimeHelper]设置本机时间失败，errorStackTrace:" + e.StackTrace);
            }
            #endregion

            Loger.Log4Net.Info("[SYSTimeHelper]发送完毕");
        }

        static bool SetSystemTime(string ttime)
        {
            //用户
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            //用户组
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            if (principal.IsInRole(WindowsBuiltInRole.Administrator) == true)
            {
                Loger.Log4Net.Info("[SYSTimeHelper]当前程序处于管理员身份运行下！");
            }
            else
            {
                Loger.Log4Net.Info("[SYSTimeHelper]当前程序不处于管理员身份运行下");
            }


            DateTime dt = new DateTime();

            if (DateTime.TryParse(ttime, out dt))
            {
                Loger.Log4Net.Info("[SYSTimeHelper]开始设置本机时间：" + dt.ToString());
                SYSTimeHelper.SYSTEMTIME st = new SYSTimeHelper.SYSTEMTIME();
                st.FromDateTime(dt);

                if (SYSTimeHelper.SetLocalTime(ref st))
                {
                    Loger.Log4Net.Info("[SYSTimeHelper]设置本机时间成功：" + ttime);
                    return true;
                }
                else
                {
                    Loger.Log4Net.Info("[SYSTimeHelper]设置本机时间失败：" + ttime);
                    return false;
                }
            }
            else
            {
                Loger.Log4Net.Info("[SYSTimeHelper]格式化时间字符串失败");
            }

            return false;
        }
    }
}
