using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            int ret = BitAuto.YanFa.SysRightManager.Common.UserInfo.Login("masj");
            Console.Write(ret);
            Console.ReadLine();
        }
    }
}
