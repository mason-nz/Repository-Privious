using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;


namespace BitAuto.DSC.IM_2015.MainInterface
{
    [ServiceContract(CallbackContract = typeof(IIsMsgCallBackServices))]
    public interface IIMServices
    {
        [OperationContract]
        ProxyAgentClient[] GetAllAgents();

        [OperationContract]
        ProxyNetFriend[] GetAllNetFriends();

        [OperationContract]
        ProxyAgentClient GetAgentByToken(int agentId);

        [OperationContract]
        bool IsAgentExists(int agentId);

        [OperationContract]
        int AddAgent(ProxyAgentClient agent);

        [OperationContract]
        void RemoveAgent(int agentId,int closeType);

        [OperationContract]
        CometMessage[] SwitchMessage(string ip, CometMessage[] messages);

        [OperationContract]
        int SetAgentStatus(int agentid, int status);

        [OperationContract]
        string CheckStates(int agentid, string tokens);

        [OperationContract]
        int AddNetFriendWaitList(ProxyNetFriend netFriend);

        [OperationContract]
        void RemoveNetFriendFromWaitList(string strBusinessLine, string token);

        [OperationContract]
        void RemoveNetFriend(string token, bool sendNetFrinedMsg, bool sendAgentMsg,int CloseType);

        [OperationContract]
        int TransferNetFriend(int fromAgent, int toAgent, string netFriendToken);

        [OperationContract]
        void ClearAllObjectByIp(string strIISIP);

        [OperationContract]
        List<ProxyNetFriend> GetCometClientsByBusinessLines(string businessLines);

        [OperationContract]
        void ForceAgentLeave(int agentId,int closeType);

        [OperationContract]
        void SyncObj(int[] agents, string[] netFriends, string iisIP, out int[] agentsDelete, out string[] netFndDelete,out bool CallbackExist);

        [OperationContract]
        int RegisterIIS(string strIISIP);

        [OperationContract]
        void WcfTest(int id);

        //更新网友实体，坐席回复时间
        [OperationContract]
        void UpdateNetFriendAgentReplayTime(string NetFriendKey);
        

    }

    public interface IIsMsgCallBackServices
    {
        /// <summary>
        /// 服务像客户端发送信息(异步)
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(CometMessage[] messages);
    }


}
