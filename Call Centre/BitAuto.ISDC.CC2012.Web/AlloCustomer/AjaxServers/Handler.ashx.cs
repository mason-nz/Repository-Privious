using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Web.AlloCustomer.AjaxServers
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string Action { get { return (Request["Action"] + "").Trim(); } }
        public string UserID { get { return (Request["UserID"] + "").Trim(); } }
        public string CustID { get { return (Request["CustIDs"] + "").Trim(); } }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action)
            {
                case "assignTask": AssignTask(out msg);
                    break;
                case "recedeTask": RecedeTask(out msg);
                    break;
            }
            context.Response.Write("{ " + msg + "}");
        }

        /// 收回操作
        /// <summary>
        /// 收回操作
        /// </summary>
        /// <param name="msg"></param>
        private void RecedeTask(out string msg)
        {
            msg = string.Empty;
            //增加“需求管理--客户分配”回收功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024BUT10110102"))
            {
                msg = "result:'false',msg:'没有收回权限！'";
                return;
            }
            string[] arr = CustID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //删除
            deleteCustUserMapping(arr);
            for (int i = 0; i < arr.Length; i++)
            {
                //通知易集客
                BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.UpdateCustUserId(arr[i].ToString(), -1, BLL.Util.GetLoginUserID());
            }
            msg = "result:'true'";
        }
        /// 删除客户下的客服（规则：部门，业务线，用户分类）
        /// <summary>
        /// 删除客户下的客服（规则：部门，业务线，用户分类）
        /// </summary>
        /// <param name="custid"></param>
        private void deleteCustUserMapping(string[] custids)
        {
            //查询客户下的满足规则客服
            string where = " AND v_userinfo.userclass=7";
            where += " AND v_userinfo.BusinessLine &1=1";
            where += " AND v_userinfo.DepartID in (select ID from SysRightsManager.dbo.f_Cid('DP00805')) ";
            where += " AND CustUserMapping.CustID in ('" + string.Join("','", custids) + "')";
            int totalCount = 0;
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.GetCustUserMapping(where, "", 1, 999999, out totalCount);
            if (dt == null) return;

            //循环删除
            foreach (DataRow dr in dt.Rows)
            {
                int userid = CommonFunction.ObjectToInteger(dr["userid"]);
                string custid = CommonFunction.ObjectToString(dr["custid"]);
                bool r = YanFa.Crm2009.BLL.CustUserMapping.Instance.Delete(custid, userid);
            }
        }
        /// 分配操作
        /// <summary>
        /// 分配操作
        /// </summary>
        /// <param name="msg"></param>
        private void AssignTask(out string msg)
        {
            msg = string.Empty;
            //增加“需求管理--客户分配”分配功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024BUT10110101"))
            {
                msg = "result:'false',msg:'没有分配权限！'";
                return;
            }
            //验证
            int _userID = 0;
            if (!int.TryParse(UserID, out _userID) || CustID == "")
            {
                msg = "result:'false',msg:'参数错误，分配失败！'";
                return;
            }
            string[] arr = CustID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //删除
            deleteCustUserMapping(arr);
            for (int i = 0; i < arr.Length; i++)
            {
                //新增
                YanFa.Crm2009.Entities.CustUserMapping model = new YanFa.Crm2009.Entities.CustUserMapping();
                model.CreateTime = DateTime.Now.ToString();
                model.CustID = arr[i].ToString();
                model.UserID = _userID;
                int r = YanFa.Crm2009.BLL.CustUserMapping.Instance.InsertCustUserMapping(model);
                //通知易集客
                BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.UpdateCustUserId(arr[i].ToString(), _userID, BLL.Util.GetLoginUserID());
            }
            msg = "result:'true'";
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