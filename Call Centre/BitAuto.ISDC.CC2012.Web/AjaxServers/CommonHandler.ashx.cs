using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.YPSuperLoginService;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    /// <summary>
    /// CommonHandler 的摘要说明
    /// </summary>
    public class CommonHandler : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        #region 属性定义
        public string Action
        {
            get
            {
                if (Request["Action"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string GetCreaterType
        {
            get
            {
                if (Request["GetCreaterType"] != null)
                {
                    return HttpUtility.UrlDecode(Request["GetCreaterType"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string TableName
        {
            get
            {
                if (Request["TableName"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TableName"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string IDField
        {
            get
            {
                if (Request["IDField"] != null)
                {
                    return HttpUtility.UrlDecode(Request["IDField"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string ShowField
        {
            get
            {
                if (Request["ShowField"] != null)
                {
                    return HttpUtility.UrlDecode(Request["ShowField"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string TableStatus
        {
            get
            {
                if (Request["TableStatus"] != null)
                {
                    return HttpUtility.UrlDecode(Request["TableStatus"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string EnumName
        {
            get
            {
                if (Request["EnumName"] != null)
                {
                    return HttpUtility.UrlDecode(Request["EnumName"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// 需要加密的信息
        /// <summary>
        /// 需要加密的信息
        /// </summary>
        public string EncryptInfo
        {
            get
            {
                if (Request["EncryptInfo"] != null)
                {
                    return HttpUtility.UrlDecode(Request["EncryptInfo"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public int nCsid
        {
            get
            {
                if (Request["csid"] != null)
                {
                    return Convert.ToInt32(HttpUtility.UrlDecode(Request["csid"]));
                }
                else
                {
                    return -1;
                }
            }
        }
        public string BeginTime
        {
            get
            {
                if (Request["BeginTime"] != null)
                {
                    return HttpUtility.UrlDecode(Request["BeginTime"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string EndTime
        {
            get
            {
                if (Request["EndTime"] != null)
                {
                    return HttpUtility.UrlDecode(Request["EndTime"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {
                case "getcreater"://得到某个表的创建人
                    GetCreaterByTable(out msg);
                    break;
                case "getfieldname"://得到某个表的一个字段和ID的json对象
                    GetFieldNameByTable(out msg);
                    break;
                case "getenumname":
                    GetEnumName(out msg);
                    break;
                //case "getyplogin"://前端没有用到，注释掉了。Modify=Masj,Date=2015-10-20
                //    GetYPLogin(out msg);
                //    break;
                case "encryptstring":
                    EncryptString(out msg);
                    break;
                case "checkforselectcallrecordorig":
                    CheckForSelectCallRecordORIG(out msg);
                    break;
            }
            context.Response.Write(msg);
        }

        /// 校验话务查询时间范围
        /// <summary>
        /// 校验话务查询时间范围
        /// </summary>
        /// <param name="msg"></param>
        private void CheckForSelectCallRecordORIG(out string msg)
        {
            msg = "";
            try
            {
                bool r;
                string info;
                DateTime st = CommonFunction.ObjectToDateTime(BeginTime);
                DateTime et = CommonFunction.ObjectToDateTime(EndTime);
                r = BLL.Util.CheckForSelectCallRecordORIG(st, et, out info);
                msg = "{success:" + r.ToString().ToLower() + ",msg:'" + info + "'}";
            }
            catch (Exception ex)
            {
                msg = "{success:false,msg:'" + ex.Message + "'}";
            }
        }

        /// 加密字符串
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="msg"></param>
        private void EncryptString(out string msg)
        {
            msg = BLL.Util.EncryptString(EncryptInfo);
        }

        private void GetYPLogin(out string msg)
        {
            msg = string.Empty;
            int nId = 0;
            try
            {
                SuperLoginServiceClient client = new SuperLoginServiceClient();
                var tokent = client.GetAccessTokenInType(nCsid, 2).m_AccessToken.ToString();
                msg = "{\"number\":\"0\",\"msg\":\"\",\"token\":\"" + tokent + "\"}";
            }
            catch (Exception ex)
            {
                msg = "{\"number\":\"-1\",\"msg\":\"失败\",\"token\":\"\"}";
            }
        }

        private void GetEnumName(out string msg)
        {
            msg = string.Empty;
            OperEnum<Entities.StopCustStopStatus> oe = new OperEnum<Entities.StopCustStopStatus>();
            oe.GetEnumName(out msg);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="msg"></param>
        private void GetCreaterByTable(out string msg)
        {
            msg = string.Empty;
            DataTable dt;
            #region
            if (!string.IsNullOrEmpty(GetCreaterType) && GetCreaterType == "QS")
            {
                int userid = BLL.Util.GetLoginUserID();
                //string groupstr = BitAuto.ISDC.CC2012.BLL.UserGroupDataRigth.Instance.GetGroupStr(userid);

                string NewShowField = string.Empty;
                string NewTableName = string.Empty;
                string strWhere = string.Empty;
                //if (!string.IsNullOrEmpty(groupstr))
                //{
                    NewShowField = "qsr.CreateUserID";
                    NewTableName = "QS_Result AS qsr INNER JOIN dbo.EmployeeAgent AS ea ON qsr.CreateUserID = ea.UserID";
                    //strWhere = "qsr.CreateUserID<>-2 AND (ea.BGID in (" + BLL.Util.SqlFilterByInCondition(groupstr) + ") OR qsr.CreateUserID='" + userid + "')";
                    strWhere = "qsr.CreateUserID<>-2";
                    strWhere += BLL.UserGroupDataRigth.Instance.GetSqlRightstr("ea", "qsr", "BGID", "CreateUserID", userid);
                //}
                //else
                //{
                //    NewShowField = "qsr.CreateUserID";
                //    NewTableName = "QS_Result AS qsr INNER JOIN dbo.EmployeeAgent AS ea ON qsr.CreateUserID = ea.UserID";
                //    strWhere = "qsr.CreateUserID<>-2 AND qsr.CreateUserID='" + userid + "'";
                //}
                dt = BLL.Util.GetAllUserIDByTable(NewTableName, NewShowField, strWhere, TableStatus);
            }
            if (!string.IsNullOrEmpty(GetCreaterType) && GetCreaterType == "QS_IM")
            {
                int userid = BLL.Util.GetLoginUserID();
                //string groupstr = BitAuto.ISDC.CC2012.BLL.UserGroupDataRigth.Instance.GetGroupStr(userid);

                string NewShowField = string.Empty;
                string NewTableName = string.Empty;
                string strWhere = string.Empty;
                //if (!string.IsNullOrEmpty(groupstr))
                //{
                    NewShowField = "qsr.CreateUserID";
                    NewTableName = "QS_IM_Result AS qsr INNER JOIN dbo.EmployeeAgent AS ea ON qsr.CreateUserID = ea.UserID";
                    //strWhere = "qsr.CreateUserID<>-2 AND (ea.BGID in (" + BLL.Util.SqlFilterByInCondition(groupstr) + ") OR qsr.CreateUserID='" + userid + "')";
                    strWhere = "qsr.CreateUserID<>-2";
                    strWhere += BLL.UserGroupDataRigth.Instance.GetSqlRightstr("ea", "qsr", "BGID", "CreateUserID", userid);
                //}
                //else
                //{
                //    NewShowField = "qsr.CreateUserID";
                //    NewTableName = "QS_IM_Result AS qsr INNER JOIN dbo.EmployeeAgent AS ea ON qsr.CreateUserID = ea.UserID";
                //    strWhere = "qsr.CreateUserID<>-2 AND qsr.CreateUserID='" + userid + "'";
                //}
                dt = BLL.Util.GetAllUserIDByTable(NewTableName, NewShowField, strWhere, TableStatus);
            }
            else
            {
                dt = BLL.Util.GetAllUserIDByTable(TableName, ShowField, TableStatus);
            }

            #endregion


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(dr[0].ToString()));

                msg += i == 0 ? "[{UserID:'" + dr[0] + "',Name:'" + userName + "'}" : ",{UserID:'" + dr[0] + "',Name:'" + userName + "'}";

                if (i == dt.Rows.Count - 1)
                {
                    msg += "]";
                }

            }

        }

        /// <summary>
        ///  根据参数表名TableName和显示字段名ShowField、字段ID名IDField、状态TableStatus得到需要的json对象
        /// </summary>
        /// <param name="msg"></param>
        private void GetFieldNameByTable(out string msg)
        {
            msg = string.Empty;
            //DataTable dt = BLL.Util.GetTableInfoByTableName(TableName, TableStatus);
            Entities.QueryQS_RulesTable query = new QueryQS_RulesTable();
            int count = 0;
            query.LoginID = BLL.Util.GetLoginUserID();
            query.RuleTableStatus = TableStatus;
            DataTable dt = BLL.QS_RulesTable.Instance.GetQS_RulesTable(query, "QS_RulesTable.CreateTime DESC", 1, 10000, out count);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                string showname = dr[ShowField].ToString();
                showname = showname.Replace("'", "&prime;");
                msg += i == 0 ? "[{ID:" + dr[IDField] + ",Name:'" + showname + "'}" : ",{ID:'" + dr[IDField] + "',Name:'" + showname + "'}";

                if (i == dt.Rows.Count - 1)
                {
                    msg += "]";
                }

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