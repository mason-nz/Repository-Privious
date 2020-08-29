using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BitAuto.DSC.IM_2015.MainWindowsService;
using BitAuto.DSC.IM_2015.Messages;

namespace ServiceHostTest
{
    class Program
    {
        private static readonly object _locker = new object();
        private static AutoResetEvent autoResetEvent = new AutoResetEvent(true);
        private static CountdownEvent e = new CountdownEvent(1);
        private static Timer syncMessageTimer = new Timer(obj =>
        {
            //Console.WriteLine(DateTime.Now.ToString("O"));
            //syncMessageTimer.Change(1000, Timeout.Infinite);
        }, null, 1000, Timeout.Infinite);

        static void Main(string[] args)
        {
            //AllocAgentMessage cmforuser = new AllocAgentMessage()
            //{
            //    UserId = "1",
            //    VisitID = "1",
            //    contractphone = "1",
            //    AgentID = 23,
            //    CsID = 232,
            //    WYName = "1",
            //    Converstime = DateTime.Now,
            //    AgentNum = "2323",
            //    AgentToken = "2332"

            //};

            //var s = BitAuto.DSC.IM_2015.BLL.Util.DataContractObject2Json(cmforuser, typeof (AllocAgentMessage));
            //var ss = 2;
            //var ips= BitAuto.DSC.IM_2015.BLL.Util.GetLocalIp();

            // var slong = BitAuto.DSC.IM_2015.BLL.Util.IpToLong(ips);
            // var longIPStr = BitAuto.DSC.IM_2015.BLL.Util.LongToIp(slong);
            //var slong = BitAuto.DSC.IM_2015.BLL.Util.IpStrToLong(ips);
            //var longIPStr = BitAuto.DSC.IM_2015.BLL.Util.IplongToIp(slong);

            MainIMService ser = new MainIMService();
            ser.StartService();
            Console.WriteLine("Service started..........");
            //ThreadPool.QueueUserWorkItem((obj) => { Th1(); }, null);
            //ThreadPool.QueueUserWorkItem((obj) => { Th2(); }, null);
            //while (true)
            //{
            //    var c = Console.ReadKey().Key;
            //    if (c == ConsoleKey.A)
            //    {
            //        //MessageSync("测试");
            //        //Th1();
            //        syncMessageTimer.Change(Timeout.Infinite, Timeout.Infinite);
            //    }
            //    else if (c == ConsoleKey.B)
            //    {
            //        syncMessageTimer.Change(2000, Timeout.Infinite);
            //    }
            //}
            Console.Read();
        }
        private static long ni = 0;

        private static void Th1()
        {
            Console.WriteLine("Th1             Start:   " + DateTime.Now.ToString("O"));

            bool isRuning = (Interlocked.Read(ref ni) == 1);
            Monitor.Enter(_locker);


            Monitor.Exit(_locker);

            if (!isRuning)
            {
                MessageSync("测试");
            }

            Console.WriteLine("Th1             End:     " + DateTime.Now.ToString("O"));

            //autoResetEvent.Set();
            //lock (_locker)
            //{
            //autoResetEvent.WaitOne();
            //Console.WriteLine(" TH1     Start: " + DateTime.Now.ToString("O"));
            //Thread.Sleep(2000);
            //Console.WriteLine(" TH1     End: " + DateTime.Now.ToString("O"));
            //autoResetEvent.Set();
            //CountdownEvent e=new CountdownEvent(1);
            //e.AddCount();

            //}
        }

        private static void DoQueueWork()
        {
            while (true)
            {
                MessageSync();
                Thread.Sleep(5000);
            }

        }


        public static void MessageSync(string str = "")
        {
            //autoResetEvent.WaitOne();
            Monitor.Enter(_locker);
            Interlocked.Increment(ref ni);
            Console.WriteLine(" MessageSync     Start:  " + DateTime.Now.ToString("o") + "     " + str);
            Thread.Sleep(3000);
            Console.WriteLine(" MessageSync     End:    " + DateTime.Now.ToString("O") + "     " + str);
            Interlocked.Decrement(ref ni);
            Monitor.Exit(_locker);
            //autoResetEvent.Set();
        }

        private static void Th2()
        {
            DoQueueWork();
            // MessageSync();
            ////lock (_locker)
            ////{
            //autoResetEvent.WaitOne();
            //Console.WriteLine(" TH2     Start: " + DateTime.Now.ToString("O"));
            //Thread.Sleep(2000);
            //Console.WriteLine(" TH2     End: " + DateTime.Now.ToString("O"));
            //autoResetEvent.Set();
            ////}
        }
    }
}
