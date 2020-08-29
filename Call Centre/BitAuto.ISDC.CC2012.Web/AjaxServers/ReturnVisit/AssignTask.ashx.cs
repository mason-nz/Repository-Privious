using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BitAuto.YanFa.Crm2009.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    /// <summary>
    /// AssignTask 的摘要说明
    /// </summary>
    public class AssignTask : IHttpHandler, IRequiresSessionState
    {

        private string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }

        private string CustIDs
        {
            get
            {
                return HttpContext.Current.Request["CustIDs"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CustIDs"].ToString());
            }
        }

        private string IDsJSON
        {
            get
            {
                return HttpContext.Current.Request["IDs"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["IDs"].ToString());
            }
        }

        private string AssignUserID
        {
            get
            {
                return HttpContext.Current.Request["AssignUserID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["AssignUserID"].ToString());
            }
        }


        public int userId = 0;

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string msg = "";

            CheckPar(out msg);

            if (msg == "")
            {
                userId = BLL.Util.GetLoginUserID();

                switch (Action)
                {
                    case "AssignTask":
                        AssignUserMapping(AssignUserID, CustIDs, out msg);
                        break;
                    case "RecedeTask":
                        RecedeUserMapping(AssignUserID, IDsJSON, out msg);
                        break;
                    case "RecedeTaskOne":
                        RecedeUserMappingOne(AssignUserID, CustIDs, out msg);
                        break;
                    case "ClearNextCallTime":
                        ClearNextCallTime(out msg);
                        break;
                    case "isYYKF":
                        IsYYKF(AssignUserID, out msg);
                        break;
                    case "IsExistsCust":
                        IsExistsCust(AssignUserID, CustIDs, out msg);
                        break;
                    case "AssignTaskNew":
                        AssignTaskNew(AssignUserID, CustIDs, out msg);
                        break;
                }
            }

            if (msg == "")
            {
                msg = "success";
            }

            context.Response.Write(msg);
        }
        private void CheckPar(out string msg)
        {
            msg = "";

            if (Action == "")
            {
                msg += "参数不正确";
            }


            if (Action == "AssignTask")
            {
                if (CustIDs == "")
                {
                    msg += "请选择客户";
                }

                if (AssignUserID == "")
                {
                    msg += "用户ID参数不正确";
                }
            }
        }


        private void AssignUserMapping(string userIds, string custIds, out string msg)
        {
            string[] userIdArry = userIds.Split(',');
            try
            {
                foreach (string userIdStr in userIdArry)
                {
                    int userId = -1;
                    if (int.TryParse(userIdStr, out userId))
                    {
                        BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.InsertBatch(custIds, userId);
                        BLL.Util.InsertUserLog("分配客户ID是" + custIds + "的客户的负责员工" + userId);
                    }
                }
                msg = "success";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
        }

        private void RecedeUserMapping(string userIds, string custIds, out string msg)
        {
            //string[] custIdsArry = custIds.Split(',');
            try
            {
                List<JsonIDs> ids = new List<JsonIDs>();
                ids = (List<JsonIDs>)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(IDsJSON, typeof(List<JsonIDs>));

                foreach (JsonIDs item in ids)
                {
                    string custids = HttpContext.Current.Server.UrlDecode(item.CustID);
                    string userid = HttpContext.Current.Server.UrlDecode(item.UserID);

                    #region  修改前：调用CC方法
                    //string[] custidsArray = custids.Split(',');

                    //foreach (string custid in custidsArray)
                    //{
                    //BLL.ProjectTask_ReturnVisit.Instance.DeleteCustUserMappingForCC(custid, userid);
                    //    BLL.Util.InsertUserLog("回收客户ID是" + custid + "的客户的负责员工" + userid);
                    //}
                    #endregion

                    #region  修改后：调用CRM方法
                    bool r = YanFa.Crm2009.BLL.CustUserMapping.Instance.Delete(custids, int.Parse(userid));
                    if (r)
                    {
                        BLL.Util.InsertUserLog("回收客户ID是(" + custids + ")的客户的负责员工" + userid);
                    }
                    else
                    {
                        BLL.Util.InsertUserLog("回收客户ID是(" + custids + ")的客户的负责员工" + userid + "时未能成功执行");
                        msg = "部分回收操作未能成功执行，请稍后再次尝试回收！";
                        return;
                    }
                    #endregion


                }
                msg = "success";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
        }


        private void RecedeUserMappingOne(string userIds, string custIds, out string msg)
        {
            try
            {
                DataTable dt = BLL.ProjectTask_ReturnVisit.Instance.GetCustUserForCCOne(CustIDs);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string custid = row["custid"].ToString();
                        string userid = row["userid"].ToString();

                        #region 修改前：调用CC方法
                        //BLL.ProjectTask_ReturnVisit.Instance.DeleteCustUserMappingForCC(custid, userid);
                        //BLL.Util.InsertUserLog("回收客户ID是" + custid + "的客户的负责员工" + userid);
                        #endregion

                        #region  修改后：调用CRM方法
                        bool r = YanFa.Crm2009.BLL.CustUserMapping.Instance.Delete(custid, int.Parse(userid));
                        if (r)
                        {
                            BLL.Util.InsertUserLog("回收客户ID是" + custid + "的客户的负责员工" + userid);
                        }
                        else
                        {
                            BLL.Util.InsertUserLog("回收客户ID是(" + custid + ")的客户的负责员工" + userid + "时未能成功执行");
                            msg = "部分回收操作未能成功执行，请稍后再次尝试回收！";
                            return;
                        }
                        #endregion
                    }
                }

                msg = "success";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
        }

        //清除客户下，非负责坐席的数据
        private void ClearNextCallTime(out string msg)
        {
            try
            {
                string[] custidsArray = CustIDs.Split(',');
                BLL.CRMCustForNextVisit.Instance.ClearErrorDataByCust();
                msg = "success";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
        }
        /// <summary>
        /// 是否是“运营客服”
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="msg"></param>
        private void IsYYKF(string userID, out string msg)
        {
            try
            {
                int assignUserId = Int32.Parse(userID);
                bool result = BLL.ProjectTask_ReturnVisit.Instance.IsYYKF(assignUserId);
                if (result)
                {
                    msg = "success";
                }
                else
                {
                    msg = "fail";
                }
            }
            catch
            {
                msg = "fail";
            }
        }
        /// <summary>
        /// 分配运营客服
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="custIds"></param>
        /// <param name="msg"></param>
        private void AssignTaskNew(string userIds, string custIds, out string msg)
        {
            string memberIdList = custIds;
            string[] userIdArry = userIds.Split(',');
            try
            {
                foreach (string userIdStr in userIdArry)
                {
                    int userId = -1;
                    if (int.TryParse(userIdStr, out userId))
                    {
                        //删除该客户在该业务线上的运营客服
                     //   BLL.ProjectTask_ReturnVisit.Instance.DeleteCustMemberOfBL(userId, memberIdList);
                        memberIdList = System.Text.RegularExpressions.Regex.Replace(memberIdList, @"\'", "");
                        BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.DeleteCustMemberByNewCar(userId, memberIdList);

                        //给CRM日志记录中添加数据
                        BitAuto.YanFa.Crm2009.BLL.UserActionLog.Instance.InsertLog("删除原来所分配的该业务线的运营客服;客户ID:" + memberIdList + ";负责运营坐席：" + userId, "", EnumActionType.Delete);
                        
                        BLL.Util.InsertUserLog("删除分配客户ID是" + memberIdList + "的客户的负责员工" + userId);
                        custIds = custIds.Replace("'","");
                        BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.InsertBatch(custIds, userId);

                        BLL.Util.InsertUserLog("分配客户ID是" + custIds + "的客户的负责员工" + userId);
                    }
                }
                msg = "success";
            }
            catch (Exception ex)
            {
                msg = "分配客户出现异常";
                BLL.Util.InsertUserLog("分配客户出现异常，异常原因："+ex.Message);
            }
        }

        private void IsExistsCust(string userIds, string custIds, out string msg)
        {
            string memberIdList = custIds;
            string[] userIdArry = userIds.Split(',');
            try
            {
                foreach (string userIdStr in userIdArry)
                {
                    int userId = -1;
                    if (int.TryParse(userIdStr, out userId))
                    {
                        bool isExists = BLL.ProjectTask_ReturnVisit.Instance.IsExistsCustMember(userId, memberIdList);

                        if (isExists)
                        {
                            msg = "existsCust";
                            return;
                        }
                    }
                }
                msg = "success";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
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

    public class JsonIDs
    {
        public string CustID { get; set; }
        public string UserID { get; set; }
    }
}