using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Collections;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoreTableManage
{
    /// <summary>
    /// ApplicationHandler 的摘要说明
    /// </summary>
    public class ApplicationHandler : IHttpHandler, IRequiresSessionState
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
        public string RangeStr
        {
            get
            {
                if (Request["RangeStr"] != null)
                {
                    return HttpUtility.UrlDecode(Request["RangeStr"].ToString());
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
                //case "rangemanage": RangeManage(out msg);
                //    break;
                case "deleterulestable":
                     int userId = BLL.Util.GetLoginUserID();
                     if (!BLL.Util.CheckRight(userId, "SYS024BUT600203"))  
                     {
                         msg = "您没有访问该页面的权限，无法删除";
                     }
                     else
                     {
                         deleteRulesTable(out msg);
                     };
                    break;
            }
            context.Response.Write("{msg:'" + msg + "'}");
        }

        //废弃 强斐 2015-4-15
        //private void RangeManage(out string msg)
        //{
        //    //判断该分组是否存在，存在则修改；不存在则插入新纪录
        //    msg = string.Empty;

        //    string[] rangeStr = RangeStr.Split('|');
        //    for (int i = 0; i < rangeStr.Length; i++)
        //    {
        //        string[] itemStr = rangeStr[i].Split('$');
        //        if (itemStr.Length == 2)
        //        {
        //            int _bgid;
        //            if (!int.TryParse(itemStr[0], out _bgid))
        //            {
        //                msg = "分组ID有误";
        //                return;
        //            }
        //            int _qs_rtid;
        //            if (!int.TryParse(itemStr[1], out _qs_rtid))
        //            {
        //                msg = "评分表ID有误";
        //                return;
        //            }
        //            Entities.QS_RulesRange old_Model = BLL.QS_RulesRange.Instance.getModelByBGID(_bgid);

        //            int result = BLL.QS_RulesRange.Instance.RangeManage(_bgid, _qs_rtid);

        //            Entities.QS_RulesRange new_Model = BLL.QS_RulesRange.Instance.getModelByBGID(_bgid);

        //            insertLog(old_Model, new_Model);

        //            msg = "操作成功";
        //        }
        //    }
        //}

        /// 删除评分表
        /// <summary>
        /// 删除评分表
        /// </summary>
        /// <param name="msg"></param>
        private void deleteRulesTable(out string msg)
        {
            msg = string.Empty;
            int _QS_RTID;
            if (!int.TryParse(QS_RTID, out _QS_RTID))
            {
                msg = "评分表ID有误";
                return;
            }

            Entities.QS_RulesTable oldModel = BLL.QS_RulesTable.Instance.GetQS_RulesTable(_QS_RTID);

            BLL.QS_RulesTable.Instance.Delete(_QS_RTID);

            rulesTableinsertLog(oldModel);

            msg = "操作成功";
        }

        //评分表删除 插入日志
        private void rulesTableinsertLog(Entities.QS_RulesTable oldModel)
        {
            string userLogStr = string.Empty;
            string logStr = string.Empty;

            Hashtable ht_FieldName = new Hashtable();

            ht_FieldName.Add("QS_RTID", "主键");
            ht_FieldName.Add("Name", "评分表名称");
            ht_FieldName.Add("ScoreType", "评分表类型");
            ht_FieldName.Add("Description", "评分表描述");
            ht_FieldName.Add("Status", "评分表状态");
            ht_FieldName.Add("DeadItemNum", "致命项数");
            ht_FieldName.Add("NoDeadItemNum", "非致命项数");
            ht_FieldName.Add("CreateTime", "创建时间");
            ht_FieldName.Add("CreateUserID", "创建人");
            ht_FieldName.Add("LastModifyTime", "最后修改时间");
            ht_FieldName.Add("LastModifyUserID", "最后修改人");
            ht_FieldName.Add("HaveQAppraisal", "是否有质检评价");

            BLL.GetLogDesc.ht_FieldName = ht_FieldName;

            Hashtable ht_FieldType = new Hashtable();

            Hashtable ht1 = new Hashtable();
            ht1.Add("1", "评分型");
            ht1.Add("2", "合格型");
            Hashtable ht2 = new Hashtable();
            ht2.Add("10001", "未完成");
            ht2.Add("10002", "未审核");
            ht2.Add("10003", "已完成");
            Hashtable ht3 = new Hashtable();
            ht3.Add("0", "没有");
            ht3.Add("1", "有");

            ht_FieldType.Add("ScoreType", ht1);
            ht_FieldType.Add("Status", ht2);
            ht_FieldType.Add("CreateUserID", "UserID");
            ht_FieldType.Add("LastModifyUserID", "UserID");
            ht_FieldType.Add("HaveQAppraisal", ht3);

            BLL.GetLogDesc.ht_FieldType = ht_FieldType;

            //插入日志
            BLL.GetLogDesc.getDeleteLogInfo(oldModel, out userLogStr);

            logStr = "评分表信息删除：" + userLogStr;

            BLL.Util.InsertUserLog(logStr);
        }

        //废弃 强斐 2015-4-15
        ////应用范围表 插入日志
        //private void insertLog(Entities.QS_RulesRange oldModel, Entities.QS_RulesRange newModel)
        //{
        //    string userLogStr = string.Empty;
        //    string logStr = string.Empty;

        //    Hashtable ht_FieldName = new Hashtable();

        //    ht_FieldName.Add("RecID", "主键");
        //    ht_FieldName.Add("QS_RTID", "质检评分表");
        //    ht_FieldName.Add("BGID", "业务组");
        //    ht_FieldName.Add("Status", "状态");
        //    ht_FieldName.Add("CreateTime", "时间");
        //    ht_FieldName.Add("CreateUserID", "编辑者");

        //    BLL.GetLogDesc.ht_FieldName = ht_FieldName;

        //    Hashtable ht_FieldType = new Hashtable();

        //    ht_FieldType.Add("QS_RTID", GetScoreTable());
        //    ht_FieldType.Add("BGID", GetGroup());
        //    ht_FieldType.Add("CreateUserID", "UserID");

        //    BLL.GetLogDesc.ht_FieldType = ht_FieldType;

        //    if (oldModel == null)//为空，则是新增
        //    {
        //        //插入日志
        //        BLL.GetLogDesc.getAddLogInfo(newModel, out userLogStr);

        //        logStr = "应用范围新增：" + userLogStr;
        //    }
        //    else //不为空，则是编辑
        //    {
        //        //插入日志 
        //        BLL.GetLogDesc.getCompareLogInfo(oldModel, newModel, out userLogStr);

        //        logStr = "应用范围编辑：" + userLogStr;
        //    }

        //    if (userLogStr != string.Empty)
        //    {
        //        BLL.Util.InsertUserLog(logStr);
        //    }
        //}

        ////评分表的ID和名称
        //private Hashtable GetScoreTable()
        //{
        //    Hashtable ht_msg = new Hashtable();

        //    DataTable dt = BLL.Util.GetTableInfoByTableName("QS_RulesTable", "10003");

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        DataRow dr = dt.Rows[i];
        //        ht_msg.Add(dr["QS_RTID"].ToString(), dr["Name"].ToString());
        //    }

        //    return ht_msg;
        //}

        ////业务组的ID和名称
        //private Hashtable GetGroup()
        //{
        //    Hashtable ht_msg = new Hashtable();

        //    DataTable dt = BLL.BusinessGroup.Instance.GetAllBusinessGroup();

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        DataRow dr = dt.Rows[i];
        //        ht_msg.Add(dr["BGID"].ToString(), dr["Name"].ToString());
        //    }

        //    return ht_msg;
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}