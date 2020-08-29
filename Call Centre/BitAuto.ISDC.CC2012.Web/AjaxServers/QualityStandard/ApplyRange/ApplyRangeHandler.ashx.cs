using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;
using System.Collections;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ApplyRange
{
    /// <summary>
    /// ApplyRangeHandler 的摘要说明
    /// </summary>
    public class ApplyRangeHandler : IHttpHandler, IRequiresSessionState
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
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
        public string BGID
        {
            get
            {
                if (Request["BGID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["BGID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string QS_RTID
        {
            get
            {
                if (Request["QS_RTID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["QS_RTID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string QS_IM_RTID
        {
            get
            {
                if (Request["QS_IM_RTID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["QS_IM_RTID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {
                case "rangemanage": 
                    int userId = BLL.Util.GetLoginUserID();
                    if (!BLL.Util.CheckRight(userId, "SYS024MOD6201"))
                    {
                        msg = "{result:'error',msg:'您没有此操作的权限'}";
                    }
                    else
                    {
                        RangeManage(out msg);
                    };
                    break;
            }
            context.Response.Write(msg);
        }

        /// 编辑应用范围
        /// <summary>
        /// 编辑应用范围
        /// </summary>
        /// <param name="msg"></param>
        private void RangeManage(out string msg)
        {
            msg = string.Empty;
            try
            {
                int bgid = CommonFunction.ObjectToInteger(BGID, -1);
                int qs_rtid = CommonFunction.ObjectToInteger(QS_RTID, -1);
                int qs_im_rtid = CommonFunction.ObjectToInteger(QS_IM_RTID, -1);

                Entities.QS_RulesRange old_Model = BLL.QS_RulesRange.Instance.getModelByBGID(bgid);

                BLL.QS_RulesRange.Instance.RangeManage(bgid, qs_rtid, qs_im_rtid);

                Entities.QS_RulesRange new_Model = BLL.QS_RulesRange.Instance.getModelByBGID(bgid);

                insertLog(old_Model, new_Model);

                msg = "{result:'ok',msg:''}";
            }
            catch (Exception ex)
            {
                msg = "{result:'error',msg:'" + ex.Message + "'}";
            }
        }

        //插入日志
        private void insertLog(QS_RulesRange old_Model, QS_RulesRange new_Model)
        {
            string userLogStr = string.Empty;
            string logStr = string.Empty;

            Hashtable ht_FieldName = new Hashtable();

            ht_FieldName.Add("RecID", "主键");
            ht_FieldName.Add("QS_RTID", "录音质检评分表");
            ht_FieldName.Add("QS_IM_RTID", "会话质检评分表");
            ht_FieldName.Add("BGID", "业务组");
            ht_FieldName.Add("Status", "状态");
            ht_FieldName.Add("CreateTime", "时间");
            ht_FieldName.Add("CreateUserID", "编辑者");

            BLL.GetLogDesc.ht_FieldName = ht_FieldName;

            Hashtable ht_FieldType = new Hashtable();

            Hashtable ht1 = GetScoreTable();
            Hashtable ht2 = GetGroup();

            ht_FieldType.Add("QS_RTID", ht1);
            ht_FieldType.Add("QS_IM_RTID", ht1);
            ht_FieldType.Add("BGID", ht2);
            ht_FieldType.Add("CreateUserID", "UserID");

            BLL.GetLogDesc.ht_FieldType = ht_FieldType;

            if (old_Model == null)//为空，则是新增
            {
                //插入日志
                BLL.GetLogDesc.getAddLogInfo(new_Model, out userLogStr);

                logStr = "应用范围新增：" + userLogStr;
            }
            else //不为空，则是编辑
            {
                //插入日志 
                BLL.GetLogDesc.getCompareLogInfo(old_Model, new_Model, out userLogStr);

                logStr = "应用范围编辑：" + userLogStr;
            }

            if (userLogStr != string.Empty)
            {
                BLL.Util.InsertUserLog(logStr);
            }
        }
        //评分表的ID和名称
        private Hashtable GetScoreTable()
        {
            Hashtable ht_msg = new Hashtable();

            DataTable dt = BLL.Util.GetTableInfoByTableName("QS_RulesTable", "10003");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ht_msg.Add(dr["QS_RTID"].ToString(), dr["Name"].ToString());
            }

            return ht_msg;
        }
        //业务组的ID和名称
        private Hashtable GetGroup()
        {
            Hashtable ht_msg = new Hashtable();

            DataTable dt = BLL.BusinessGroup.Instance.GetAllBusinessGroup();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ht_msg.Add(dr["BGID"].ToString(), dr["Name"].ToString());
            }

            return ht_msg;
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