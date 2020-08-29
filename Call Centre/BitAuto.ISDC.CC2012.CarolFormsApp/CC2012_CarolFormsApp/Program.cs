using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RECONCOMLibrary;

namespace CC2012_CarolFormsApp
{
    static class Program
    {
        static System.Threading.Mutex mutex;  //这个静态类型的Mutex是必需的  
        public static ReconCOM rc;
        public static int CallID;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool IsFirstRun;
            string mutexName = Application.ProductName;
            mutex = new System.Threading.Mutex(true, mutexName, out IsFirstRun);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (IsFirstRun)
            {
                int iRet;
                try
                {
                    rc = new ReconCOM();

                }
                catch (Exception ex)
                {
                    string msg = "组件实例化错误："+ex.Message;
                    MessageBox.Show(msg);
                    return;
                }
                
                //rc.OnConnectionEvent += new RECONCOMLibrary._IReconCOMEvents_OnConnectionEventEventHandler(recon_OnConnectionEvent);
                //rc.OnCTIEvent += new RECONCOMLibrary._IReconCOMEvents_OnCTIEventEventHandler(recon_OnCTIEvent);
                //rc.OnAgentEvent += new RECONCOMLibrary._IReconCOMEvents_OnAgentEventEventHandler(recon_OnAgentEvent);

                //Connecting to server specified in recon.cfg
                if ((iRet = rc.R_Initialize("RCConsole")) == 0)
                    Trace("R_Initialize succeded");
                else
                    Trace("R_Initialize failed: " + (enErrorCode)iRet);
                Application.Run(new Login());
                GC.SuppressFinalize(mutex);
            }
            else
            {
                MessageBox.Show(Application.ProductName + "程序已经启动！");
            }
        }

        public static void Trace(string s)
        {
            System.Console.WriteLine(s);
            rc.R_Trace(3, s);
        }

        //public static void recon_OnAgentEvent(AgentEvent ae)
        //{
        //    System.Console.WriteLine("EventName:" + ae.EventName);
        //    System.Console.WriteLine("AgentUsername:" + ae.AgentUsername);
        //    System.Console.WriteLine("AgentLoginID:" + ae.AgentLoginID);
        //    //System.Console.WriteLine("AgentState:" + (enAgentState)ae.AgentState);
        //    //System.Console.WriteLine("AgentAuxState:" + ae.AgentAuxState);
        //    //System.Console.WriteLine("MediaType:" + ae.MediaType);
        //    //System.Console.WriteLine("Timestamp:" + ae.Timestamp);
        //}

        //public static void recon_OnConnectionEvent(ConnectionEvent e)
        //{
        //    Trace("recon_OnConnectionEvent: " + e.ComponentName +
        //        " (type: " + e.ComponentType +
        //        ") " + (e.Connected > 0 ? "is connected." : "is disconnected."));
        //}

        //public static void recon_OnCTIEvent(CTIEvent e)
        //{
        //    CallID = e.CallID;
        //    Trace("recon_OnCTIEvent: " + e.EventName + " event");
        //    Trace(String.Format(" CallID: {0,8:X8} ({1})", e.CallID, e.CallID));
        //    Trace(String.Format(" CallType:{0}  Media:{1}", e.CallType, e.MediaType));
        //    Trace(String.Format(" State:{0}  Cause:{1}", e.State, e.EventCause));
        //    Trace(String.Format(" dwParam1:0x{0,2:x2}  dwParam2:0x{1,2:x2}", e.LongParam1, e.LongParam2));
        //    Trace(String.Format(" sParam1:{0}  sParam2:{1}", e.StringParam1, e.StringParam2));
        //    Trace(String.Format(" PartyA  DN:{0}  {1}", e.PartyA_Number, e.PartyA_Descr));
        //    Trace(String.Format(" PartyB  DN:{0}  {1}", e.PartyB_Number, e.PartyB_Descr));
        //    //AgentEvent ae=new AgentEvent();
        //    //int i = rc.T_AgentGetState("1040", 2, out ae);
        //    //Console.WriteLine((enAgentState)i);
        //    // Trace(String.Format(" AttachedData:{0}", e.GetAttachedDataList().ToString()));  
        //    KeyValueList kvl = new KeyValueList();
        //    int k = rc.T_GetAttachedDataList(e.CallID, out kvl);//获取通话随路数据
        //    //其中UserChoice:Personal
        //    //sys_ANI:1000  呼入分机号码
        //    //sys_DNIS:2448 呼入测试号码
        //    //e.PartyB_Number 呼出号码
        //    if (kvl.Size() > 0)
        //    {
        //        for (int i = 0; i < kvl.Size(); i++)
        //        {
        //            string skey = "";
        //            kvl.GetPair(i, out skey);
        //            Console.WriteLine("GetAttachedData:");
        //            Trace(skey + ":" + kvl.GetValue(skey));
        //            Console.WriteLine();
        //        }
        //    }
        //    //Trace((enErrorCode)k + "|  KeyValueListCount:" + kvl.Size());
        //}
    }

}
