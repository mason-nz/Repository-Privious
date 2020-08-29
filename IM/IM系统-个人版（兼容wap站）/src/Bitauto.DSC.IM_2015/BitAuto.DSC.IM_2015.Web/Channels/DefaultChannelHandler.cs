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

        static DefaultChannelHandler()
        {
            stateManager = new CometStateManager(new InProcCometStateProvider());
        }

        //public static List<CometClient> GetAllCometClients()
        //{
        //   //return stateManager.GetAllCometClients();
        //} 


        #region IHttpAsyncHandler Members

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return stateManager.BeginSubscribe(context, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
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
            //CometClient client = null;
            string msg;
            try
            {
                return stateManager.IsExists(clientPrivateToken, out msg);
                //client = stateManager.GetCometClient(clientPrivateToken);

            }
            catch (Exception ex)
            {
            }
            //if (client != null)
            //    return true;

            return false;
        }
        #endregion
    }
}