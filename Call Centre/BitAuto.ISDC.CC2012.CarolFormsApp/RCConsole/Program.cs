using System;
using System.Collections.Generic;
using System.Text;
using RECONCOMLibrary;
using System.Management;

namespace RCConsole
{
    class Program
    {
        protected ReconCOM rc;
        public static int CallID;

        public Program()
        {
            int iRet;

            rc = new ReconCOM();
            rc.OnConnectionEvent += new RECONCOMLibrary._IReconCOMEvents_OnConnectionEventEventHandler(recon_OnConnectionEvent);
            rc.OnCTIEvent += new RECONCOMLibrary._IReconCOMEvents_OnCTIEventEventHandler(recon_OnCTIEvent);
            rc.OnAgentEvent += new RECONCOMLibrary._IReconCOMEvents_OnAgentEventEventHandler(recon_OnAgentEvent);


            //Connecting to server specified in recon.cfg
            if ((iRet = rc.R_Initialize("RCConsole")) == 0)
                Trace("R_Initialize succeded");
            else
                Trace("R_Initialize failed: " + (enErrorCode)iRet);
        }

        protected void Trace(string s)
        {
            System.Console.WriteLine(s);
            rc.R_Trace(3, s);
        }


        private void recon_OnAgentEvent(AgentEvent ae)
        {
            System.Console.WriteLine("EventName:" + ae.EventName);
            System.Console.WriteLine("AgentUsername:" + ae.AgentUsername);
            System.Console.WriteLine("AgentLoginID:" + ae.AgentLoginID);
            System.Console.WriteLine("Party_Number:" + ae.Party_Number);
            System.Console.WriteLine("HardwareLogin:" + ae.HardwareLogin);
            //rc.T_AgentGetState("1059",2,out
            //System.Console.WriteLine("AgentState:" + (enAgentState)ae.AgentState);
            //System.Console.WriteLine("AgentAuxState:" + ae.AgentAuxState);
            //System.Console.WriteLine("MediaType:" + ae.MediaType);
            //System.Console.WriteLine("Timestamp:" + ae.Timestamp);
            //System.Console.WriteLine("T_GetSysDataKey:" + rc.T_GetSysDataKey((int)enSysDataKeys.voiceMsgCallDataKey_AgentLoginID));
        }

        private void recon_OnConnectionEvent(ConnectionEvent e)
        {
            Trace("recon_OnConnectionEvent: " + e.ComponentName +
                " (type: " + e.ComponentType +
                ") " + (e.Connected > 0 ? "is connected." : "is disconnected."));
        }

        private void recon_OnCTIEvent(CTIEvent e)
        {
            CallID = e.CallID;
            Trace("recon_OnCTIEvent: " + e.EventName + " event");
            Trace(String.Format(" CallID: {0,8:X8} ({1})", e.CallID, e.CallID));
            Trace(String.Format(" CallType:{0}  Media:{1}", e.CallType, e.MediaType));
            Trace(String.Format(" State:{0}  Cause:{1}", e.State, e.EventCause));
            Trace(String.Format(" dwParam1:0x{0,2:x2}  dwParam2:0x{1,2:x2}", e.LongParam1, e.LongParam2));
            Trace(String.Format(" sParam1:{0}  sParam2:{1}", e.StringParam1, e.StringParam2));
            Trace(String.Format(" PartyA  DN:{0}  {1}", e.PartyA_Number, e.PartyA_Descr));
            Trace(String.Format(" PartyB  DN:{0}  {1}", e.PartyB_Number, e.PartyB_Descr));
            //AgentEvent ae=new AgentEvent();
            //int i = rc.T_AgentGetState("1040", 2, out ae);
            //Console.WriteLine((enAgentState)i);
            // Trace(String.Format(" AttachedData:{0}", e.GetAttachedDataList().ToString()));  
            
            KeyValueList kvl = new KeyValueList();
            int k = rc.T_GetAttachedDataList(e.CallID, out kvl);//获取通话随路数据
            //其中UserChoice:Personal
            //sys_ANI:1000  呼入分机号码
            //sys_DNIS:2448 呼入测试号码
            //e.PartyB_Number 呼出号码
            if (kvl.Size() > 0)
            {
                for (int i = 0; i < kvl.Size(); i++)
                {
                    string skey = "";
                    kvl.GetPair(i, out skey);
                    Console.WriteLine("GetAttachedData:");
                    Trace(skey + ":" + kvl.GetValue(skey));
                    Console.WriteLine();
                }
            }
            //else
            //{
            //    KeyValueList kvl2 = new KeyValueList();
            //    kvl2.AddPair("CurrentMacAddress", GetMacAddress());
            //    kvl2.AddPair("CurrentUserID", e.PartyA_Number);

            //    rc.T_AttachDataList(e.CallID, kvl2);
            //}
            System.Console.WriteLine("MyComponentID:" + rc.CM_GetMyComponentID());
            //Trace((enErrorCode)k + "|  KeyValueListCount:" + kvl.Size());
        }

        public String[] ParseCommand(String cmd)
        {
            return cmd.Split(' ');
        }

        public void Run()
        {
            byte[] b = new byte[500];
            String s;
            while (true)
            {
                System.Console.Write(">");
                try
                {
                    s = System.Console.ReadLine();
                    if (s.Length == 0)
                        continue;
                    String[] cmd = ParseCommand(s);
                    if (cmd[0].Equals("exit", StringComparison.OrdinalIgnoreCase))
                        break;
                    if (cmd[0].Equals("help", StringComparison.OrdinalIgnoreCase))
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("exit - exits from the application");
                        Commands.getInstance().writeShortHelp();
                        System.Console.WriteLine();
                        continue;
                    }

                    Command command = Commands.getInstance().getCommand(cmd[0]);
                    if (command != null)
                    {
                        if (command.execute(rc, cmd))
                            System.Console.WriteLine("OK");
                        else
                            System.Console.WriteLine("NOK");
                    }
                    else
                        System.Console.WriteLine("Unknown command: " + cmd[0]);
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine("Exception: " + ex + ex.Message);
                }
            }

            rc.R_Close();
            Trace("R_Close succeeded");
        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("Welcome to ReconCOM console!");
            new Program().Run();
            System.Console.WriteLine("Bye!");
        }

        public static string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac += mo["MacAddress"].ToString();
                    }
                }
                //return strMac.Replace(":", "");
                return strMac;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
