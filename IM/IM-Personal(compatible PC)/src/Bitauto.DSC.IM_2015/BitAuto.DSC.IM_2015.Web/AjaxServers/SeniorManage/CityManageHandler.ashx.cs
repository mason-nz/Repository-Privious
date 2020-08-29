using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage
{
    /// <summary>
    /// CityManageHandler 的摘要说明
    /// </summary>
    public class CityManageHandler : IHttpHandler, IRequiresSessionState
    {
        #region 属性
        /// 操作方式
        /// <summary>
        /// 操作方式
        /// </summary>
        public string Action { get { return BLL.Util.GetCurrentRequestFormStr("Action"); } }
        /// ID
        /// <summary>
        /// ID
        /// </summary>
        public int RecID { get { return BLL.Util.GetCurrentRequestFormInt("RecID"); } }
        /// DistrictID
        /// <summary>
        /// DistrictID
        /// </summary>
        public string DistrictID { get { return BLL.Util.GetCurrentRequestFormStr("DistrictID"); } }

        /// 城市群
        /// <summary>
        /// 城市群
        /// </summary>
        public string CityGroups { get { return BLL.Util.GetCurrentRequestFormStr("CityGroups"); } }
        /// 客服
        /// <summary>
        /// 客服
        /// </summary>
        public string UserIds { get { return BLL.Util.GetCurrentRequestFormStr("UserIds"); } }

        /// 开始时间
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get { return BLL.Util.GetCurrentRequestFormStr("StartTime"); } }
        /// 结束时间
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get { return BLL.Util.GetCurrentRequestFormStr("EndTime"); } }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            bool success = false;
            string msg = "";
            string data = "[]";

            switch (Action.ToLower())
            {
               
                case "assign":
                    success = Assignation(out msg);
                    break;
                case "recover":
                    success = Recover(out msg);
                    break;
                case "savetime":
                    success = SaveTime(out msg);
                    break;
            }

            if (success)
            {
                context.Response.Write("{'result':'success','msg':'" + msg + "','data':" + data + "}");
            }
            else
            {
                context.Response.Write("{'result':'error','msg':'" + msg + "'}");
            }

        }

        
        /// 分配客服
        /// <summary>
        /// 分配客服
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool Assignation(out string msg)
        {
            msg = "";
            try
            {
                List<string> groups = CommonFunc.StringToList(CityGroups);
                List<string> agents = CommonFunc.StringToList(UserIds);
                if (groups.Count * agents.Count == 0) return false;
                int userid = BLL.Util.GetLoginUserID();
                return BLL.CityGroupAgent.Instance.AddCityGroupAgent(groups, agents, userid);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        /// 回收客服
        /// <summary>
        /// 回收客服
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool Recover(out string msg)
        {
            msg = "";
            try
            {
                List<string> groups = CommonFunc.StringToList(CityGroups);
                List<string> agents = CommonFunc.StringToList(UserIds);
                if (groups.Count * agents.Count == 0) return false;

                return BLL.CityGroupAgent.Instance.DeleteCityGroupAgent(groups, agents);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        /// 保存时间
        /// <summary>
        /// 保存时间
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool SaveTime(out string msg)
        {
            msg = "";
            try
            {
                //BLL.BaseData.Instance.SaveTime(new ServeTime(StartTime), new ServeTime(EndTime));
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}