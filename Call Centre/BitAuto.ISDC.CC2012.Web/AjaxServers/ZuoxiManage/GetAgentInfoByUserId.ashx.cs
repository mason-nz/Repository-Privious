using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using BitAuto.Utils.Config;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage
{
    /// <summary>
    /// GetAgentInfoByUserId 的摘要说明
    /// </summary>
    public class GetAgentInfoByUserId : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// 用户ID 列表
        /// </summary>
        public string UserID
        {
            get
            {
                return HttpContext.Current.Request["userID"] == null ? string.Empty : HttpContext.Current.Request["userID"].ToString();
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";

            StringBuilder sbStr = new StringBuilder();
            sbStr.Append("[{");

            sbStr.Append("'AgentNum':'");
            sbStr.Append(GetAgentNum());
            sbStr.Append("',");

            sbStr.Append("'DataRight':'");
            sbStr.Append(GetDataRight());
            sbStr.Append("',");

            sbStr.Append("'RoleIDs':'");
            sbStr.Append(GetRoleIdsByUserID());
            sbStr.Append("'");

            sbStr.Append("}]");
            context.Response.Write(sbStr.ToString());
        }

        private string GetDataRight()
        {
            string retStr = "";
            Entities.UserDataRigth model = BLL.UserDataRigth.Instance.GetUserDataRigth(int.Parse(UserID));
            if (model != null)
            {
                retStr = model.RightType.ToString();
            }

            return retStr;
        }

        private string GetAgentNum()
        {
            string retStr = "";
            Entities.QueryEmployeeAgent query = new Entities.QueryEmployeeAgent();
            query.UserID =int.Parse(UserID);
            int totalCount = 0;
            DataTable dt = BLL.EmployeeAgent.Instance.GetEmployeeAgent(query, "", 1, 10, out totalCount);

            if (dt != null&&dt.Rows.Count>0)
            {
                retStr = dt.Rows[0]["AgentNum"].ToString();
            }

            return retStr;
        }

        private string GetRoleIdsByUserID()
        {
            string ids = "";
            DataTable dt = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(int.Parse(UserID), ConfigurationUtil.GetAppSettingValue("ThisSysID"));

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ids += dr["RoleID"].ToString() + ",";
                }
            }
            if (ids.Length > 0)
            {
                ids = ids.Substring(0, ids.Length - 1);
            }
            return ids;
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