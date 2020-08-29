using System;
using System.Collections.Generic;
using System.Text;
using RECONCOMLibrary;
using RCConsole;
using System.ServiceModel;
using RCConsole.VCLogService;

class Commands
{
    protected static Commands _instance = new Commands();
    protected Dictionary<String, Command> commands = new Dictionary<String, Command>();

    private Commands()
    {
        commands["login"] = new LoginCommand();//坐席CFG登录
        commands["logout"] = new LogoutCommand();//坐席CFG登出
        commands["ms"] = new MonitorStartCommand();//坐席登录CFG后，监听事件
        commands["mc"] = new MakeCallCommand();//外呼电话
        commands["download"] = new DownloadCommand();
        commands["alon"] = new AgentLogOnCommand();//坐席ACD登录
        commands["aloff"] = new AgentLogOffCommand();//坐席ACD登出
        commands["ags"] = new AgentGetStateCommand();//获取坐席当前状态（就绪3、置忙4、事后处理5）
        commands["ass"] = new AgentSetStateCommand();//设置坐席当前状态（就绪3、置忙4、事后处理5）
        commands["rc"] = new ReleaseCallCommand();//挂断电话
        commands["cc"] = new ConsultCallCommand();//电话转接
        commands["rnc"] = new ReconnectCallCommand();//电话恢复转接

        commands["ac"] = new AnswerCallCommand();//接电话  需要硬件接通
        commands["hc"] = new HoldCallCommand();//电话保持
        commands["rtc"] = new RetrieveCallCommand();//电话恢复


    }

    public static Commands getInstance() { return _instance; }

    public Command getCommand(String cmd)
    {
        try
        {
            return commands[cmd];
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    public void writeShortHelp()
    {
        foreach (Command cmd in commands.Values)
        {
            System.Console.WriteLine(cmd.getShortHelp());
        }
    }

}

interface Command
{
    String getHelp();

    String getShortHelp();

    bool execute(ReconCOM rc, String[] argv);
}

class LoginCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String username = argv.Length > 1 ? argv[1] : "System";
            String password = argv.Length > 2 ? argv[2] : "carols";
            iRet = rc.CS_Login(username, password);
            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                System.Console.WriteLine("MyComponentID:" + rc.CM_GetMyComponentID());
                return true;
            }
            System.Console.WriteLine();
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: login [user] [password]";
    }

    public String getShortHelp()
    {
        return "login [user] [password] - makes CFG login";
    }
}

class LogoutCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            iRet = rc.CS_Logout();
            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                return true;
            }
            System.Console.WriteLine();
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: logout";
    }

    public String getShortHelp()
    {
        return "logout - makes CFG logout";
    }
}

class MonitorStartCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        int iRet;
        int type = 1;
        if (argv.Length < 2)
        {
            System.Console.WriteLine();
            System.Console.WriteLine(getHelp());
            return false;
        }
        if (argv.Length > 2 && argv[2].Equals("rp", StringComparison.OrdinalIgnoreCase))
            type = 2;
        try
        {
            iRet = rc.T_StartMonitor(argv[1], type);
            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                //AgentEvent ae = new AgentEvent();
                //int i = rc.T_AgentGetState("1040", 1, out ae);
                //Console.WriteLine((enAgentState)i);

                //int b = rc.T_AgentLogOn("1040", "1040", "test1", 0);
                //Console.WriteLine(b);
                //int i = rc.T_AgentSetState("1040", "1040", 3, 0);
                //Console.WriteLine((enAgentState)i);
                //Console.WriteLine((enErrorCode)i);

                //i = rc.T_AgentLogOff("1040", "1040");
                //Console.WriteLine((enAgentState)i);
                //Console.WriteLine((enErrorCode)i);
                return true;
            }
            System.Console.WriteLine();
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: ms <dialnr> [ext|rp]";
    }

    public String getShortHelp()
    {
        return "ms <dialnr> [ext|rp] - monitor start";
    }
}

class DownloadCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        if (argv.Length < 3)
        {
            System.Console.WriteLine();
            System.Console.WriteLine(getHelp());
            return false;
        }

        try
        {
            int iRet;
            iRet = rc.R_FileDownload(argv[1], argv[2], argv[3]);
            if (iRet == (int)enErrorCode.E_SUCCESS)
                return true;
            System.Console.WriteLine();
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: download <compname> <localfile> <remotefile>";
    }

    public String getShortHelp()
    {
        return "download <compname> <localfile> <remotefile> - starts filetransfer download.";
    }
}

class MakeCallCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        int iRet;
        if (argv.Length < 3)
        {
            System.Console.WriteLine();
            System.Console.WriteLine(getHelp());
            return false;
        }
        try
        {
            iRet = rc.T_MakeCall(argv[1], argv[2], null);
            if (iRet == (int)enErrorCode.E_SUCCESS)
                return true;
            System.Console.WriteLine();
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: mc <caller> <called>";
    }

    public String getShortHelp()
    {
        return "mc <caller> <called> - make call";
    }
}

class AgentLogOnCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String user = argv.Length > 1 ? argv[1] : "System";
            //String username = argv.Length > 2 ? argv[2] : "carols";
            iRet = rc.T_AgentLogOn(user, user, user, 0);
            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                System.Console.WriteLine("MyComponentID:" + rc.CM_GetMyComponentID());
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(System.Configuration.ConfigurationSettings.AppSettings["VCLogServiceURL"].ToString(), false)));

                Console.WriteLine("录音接口登录返回值：" + client.AgentLogin(user, user));

                client.Close();
                return true;
            }
            System.Console.WriteLine();
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: alon [user]";
    }

    public String getShortHelp()
    {
        return "alon [user]";
    }
}

class AgentLogOffCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String user = argv.Length > 1 ? argv[1] : "System";
            //String username = argv.Length > 2 ? argv[2] : "carols";
            iRet = rc.T_AgentLogOff(user, user);
            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(System.Configuration.ConfigurationSettings.AppSettings["VCLogServiceURL"].ToString(), false)));

                Console.WriteLine("录音接口登出返回值：" + client.Agentlogout(user));

                client.Close();
                return true;
            }
            System.Console.WriteLine();
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: aloff [user]";
    }

    public String getShortHelp()
    {
        return "aloff [user]";
    }
}

class AgentGetStateCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String user = argv.Length > 1 ? argv[1] : "System";
            //String username = argv.Length > 2 ? argv[2] : "carols";
            AgentEvent ae = new AgentEvent();
            iRet = rc.T_AgentGetState(user, 2, out ae);
            System.Console.WriteLine();
            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                System.Console.WriteLine("AgentState:" + (enAgentState)ae.AgentState);
                //System.Console.WriteLine("EventName:" + ae.EventName);
                return true;
            }
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: ags [user]";
    }

    public String getShortHelp()
    {
        return "ags [user]";
    }
}

class AgentSetStateCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String user = argv.Length > 1 ? argv[1] : "System";
            int status = argv.Length > 2 ? int.Parse(argv[2]) : 0;

            iRet = rc.T_AgentSetState(user, user, status, 0);
            System.Console.WriteLine();
            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                return true;
            }
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: ass [user]";
    }

    public String getShortHelp()
    {
        return "ass [user]";
    }
}

class AnswerCallCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String user = argv.Length > 1 ? argv[1] : "System";
            //int status = argv.Length > 2 ? int.Parse(argv[2]) : 0;
            System.Console.WriteLine();
            System.Console.WriteLine(Program.CallID);
            iRet = rc.T_AnswerCall(user, Program.CallID);

            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                return true;
            }
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: ac [user]";
    }

    public String getShortHelp()
    {
        return "ac [user]";
    }
}

class ReleaseCallCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String user = argv.Length > 1 ? argv[1] : "System";
            //int status = argv.Length > 2 ? int.Parse(argv[2]) : 0;
            System.Console.WriteLine();
            System.Console.WriteLine(Program.CallID);
            iRet = rc.T_ReleaseCall(user, Program.CallID);

            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                return true;
            }
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: rc [user]";
    }

    public String getShortHelp()
    {
        return "rc [user]";
    }
}

class HoldCallCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String user = argv.Length > 1 ? argv[1] : "System";
            //int status = argv.Length > 2 ? int.Parse(argv[2]) : 0;
            System.Console.WriteLine();
            System.Console.WriteLine(Program.CallID);
            iRet = rc.T_HoldCall(user, Program.CallID);

            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                return true;
            }
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: hc [user]";
    }

    public String getShortHelp()
    {
        return "hc [user]";
    }
}

class RetrieveCallCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String user = argv.Length > 1 ? argv[1] : "System";
            //int status = argv.Length > 2 ? int.Parse(argv[2]) : 0;

            iRet = rc.T_RetrieveCall(user, Program.CallID);
            System.Console.WriteLine();
            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                return true;
            }
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: rtc [user]";
    }

    public String getShortHelp()
    {
        return "rtc [user]";
    }
}

class ConsultCallCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String user = argv.Length > 1 ? argv[1] : "System";
            String sDestDN = argv.Length > 2 ? argv[2] : "0";

            KeyValueList kvl = new KeyValueList();
            rc.T_GetAttachedDataList(Program.CallID, out kvl);
            iRet = rc.T_ConsultCall(user, sDestDN, Program.CallID, kvl);
            System.Console.WriteLine();
            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                return true;
            }
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: cc [user] [sDestDN]";
    }

    public String getShortHelp()
    {
        return "cc [user] [sDestDN]";
    }
}

class ReconnectCallCommand : Command
{
    public bool execute(ReconCOM rc, String[] argv)
    {
        try
        {
            int iRet;
            String user = argv.Length > 1 ? argv[1] : "System";
            String sDestDN = argv.Length > 2 ? argv[2] : "0";

            //KeyValueList kvl = new KeyValueList();
            //rc.T_GetAttachedDataList(Program.CallID, out kvl);
            iRet = rc.T_ReconnectCall(user, Program.CallID);
            System.Console.WriteLine();
            if (iRet == (int)enErrorCode.E_SUCCESS)
            {
                return true;
            }
            System.Console.WriteLine((enErrorCode)iRet);
            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
            return false;
        }
    }

    public String getHelp()
    {
        return "usage: rnc [user]";
    }

    public String getShortHelp()
    {
        return "rnc [user]";
    }
}