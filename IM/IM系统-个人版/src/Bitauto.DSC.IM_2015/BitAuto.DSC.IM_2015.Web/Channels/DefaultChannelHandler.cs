using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
using BitAuto.DSC.IM_2015.Core.Messages;
using BitAuto.DSC.IM_2015.Core;
using BitAuto.DSC.IM_2015.Entities;
namespace BitAuto.DSC.IM_2015.Web.Channels
{
    /// <summary>
    /// This is our handler for the comet subscription mechanism
    /// </summary>
    public class DefaultChannelHandler : IHttpAsyncHandler
    {
        /// <summary>
        /// This is our state manager that manages the state of the client
        /// </summary>
        private static CometStateManager stateManager;

       // private Dictionary<string, DateTime> idcTest = new Dictionary<string, DateTime>();
        static DefaultChannelHandler()
        {
            //
            //  Initialize 
            stateManager = new CometStateManager(
                new InProcCometStateProvider());


            stateManager.ClientInitialized += new CometClientEventHandler(stateManager_ClientInitialized);
            stateManager.ClientSubscribed += new CometClientEventHandler(stateManager_ClientSubscribed);
            stateManager.IdleClientKilled += new CometClientEventHandler(stateManager_IdleClientKilled);
            stateManager.AgentStateOnChanged += new AgentStateEventHandler(stateManager_AgentStateOnChanged);
        }

        public static List<CometClient> GetAllCometClients()
        {
            return stateManager.GetAllCometClients();
        } 

        static void stateManager_AgentStateOnChanged(object sender, AgentStateEventArgs args)
        {
            //Debug.WriteLine("Client AgentStateOnChanged: " + args.AgentState.UserID + "," + args.AgentState.Status);
        }

        static void stateManager_IdleClientKilled(object sender, CometClientEventArgs args)
        {
        }

        static void stateManager_ClientSubscribed(object sender, CometClientEventArgs args)
        {      
        }

        static void stateManager_ClientInitialized(object sender, CometClientEventArgs args)
        {
        }

        #region IHttpAsyncHandler Members

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            //if (idcTest.ContainsKey(context.Request["privateToken"]))
            //{
            //    idcTest[context.Request["privateToken"]] = DateTime.Now;
            //}
            //else
            //{
            //    idcTest.Add(context.Request["privateToken"], DateTime.Now);
            //}
            return stateManager.BeginSubscribe(context, cb, extraData);

        }

        public void EndProcessRequest(IAsyncResult result)
        {
            /*
            CometAsyncResult cometAsyncResult = result as CometAsyncResult;
            var tn = cometAsyncResult.Context.Request["privateToken"];
            if (idcTest.ContainsKey(tn))
            {
                BLL.Loger.Log4Net.Info(string.Format("PriveToken:{0}; dt:{1}", tn, DateTime.Now - idcTest[tn]));
            }
            */
            stateManager.EndSubscribe(result);
        }

        #endregion

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public static CometStateManager StateManager
        {
            get { return stateManager; }
        }


        public static bool isExistClient(string clientPrivateToken)
        {
            CometClient client = null;
            try
            {
                client = stateManager.GetCometClient(clientPrivateToken);

            }
            catch (Exception ex)
            {
            }
            if (client != null)
                return true;

            return false;
        }
        #endregion
    }
}