using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;
using XYAuto.ITSC.Chitunion2017.LuceneMedia.LuceneManage;
using XYAuto.ITSC.Chitunion2017.LuceneMediaConsole.Common;

namespace XYAuto.ITSC.Chitunion2017.LuceneMediaConsole
{
    public class ServiceManage
    {

        readonly static string time = ConfigurationManager.AppSettings["TimeInterval"];

        static System.Timers.Timer timer;
        readonly static Task[] tasks = new Task[2];
        public ServiceManage()
        {
            try
            {
                ExecutionCode(null, null);
                int interval = !string.IsNullOrEmpty(time) ? Convert.ToInt32(time) : 60;
                //间隔1小时 3600000
                //timer = new System.Timers.Timer(interval * 3600000) { AutoReset = true };
                timer = new System.Timers.Timer(interval * 60000) { AutoReset = true };
                //timer = new System.Timers.Timer(10000) { AutoReset = true };
                timer.Elapsed += new ElapsedEventHandler(ExecutionCode);
            }
            catch (Exception ex)
            {

                Log4NetHelper.Error("服务运行异常：", ex);
            }
        }
        public void Stop()
        {
            //释放所有task所占资源
            foreach (var task in tasks)
            {
                if (task != null)
                {

                    task.Dispose();
                }
            }
            timer.Stop();
        }
        public void Start()
        {

            timer.Start();

        }
        private void ExecutionCode(object source, System.Timers.ElapsedEventArgs e)
        {

            if (tasks[0] == null || tasks[0].IsCompleted)
            {
                tasks[0] = Task.Factory.StartNew(() =>
                {

                    WeiBoMediaLucene.Instance.AddWeiBoIndex();
                });
            }

            if (tasks[1] == null || tasks[1].IsCompleted)
            {
                tasks[1] = Task.Factory.StartNew(() =>
                {

                    WeiXinMediaLucene.Instance.AddIndexInfo();
                });
            }

            //Task.WaitAll(tasks);
        }
    }
}
