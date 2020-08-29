using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    public static class Log
    {
        public static void InsertLogStopCustApply(Entities.StopCustApply oldModel, Entities.StopCustApply newModel, int userID)
        {
            string userLogStr = string.Empty;
            string logStr = string.Empty;

            Hashtable ht_FieldName = new Hashtable();

            ht_FieldName.Add("ApplyerID", "申请人");
            ht_FieldName.Add("ApplyTime", "申请时间");
            ht_FieldName.Add("AuditTime", "审核时间");
            ht_FieldName.Add("StopTime", "停用时间");
            ht_FieldName.Add("StopStatus", "停用申请状态");
            ht_FieldName.Add("RejectReason", "驳回理由");
            ht_FieldName.Add("AuditOpinion", "审核意见");
            ht_FieldName.Add("Remark", "备注");
            ht_FieldName.Add("CreateUserID", "操作人");

            BLL.GetLogDesc.ht_FieldName = ht_FieldName;

            Hashtable ht_FieldType = new Hashtable();

            ht_FieldType.Add("ApplyerID", "UserID");
            ht_FieldType.Add("StopStatus", GetStopStatus());
            ht_FieldType.Add("CreateUserID", "UserID");

            BLL.GetLogDesc.ht_FieldType = ht_FieldType;

            if (oldModel == null)//为空，则是新增
            {
                //插入日志
                BLL.GetLogDesc.getAddLogInfo(newModel, out userLogStr);

                logStr = "CRM停用客户申请表新增：" + userLogStr;
            }
            else //不为空，则是编辑
            {
                //插入日志 
                BLL.GetLogDesc.getCompareLogInfo(oldModel, newModel, out userLogStr);

                logStr = "CRM停用客户申请表编辑：" + userLogStr;
            }

            if (userLogStr != string.Empty)
            {
                InsertUserLog(logStr, userID);
            }
        }

        //public static void InsertLogOrderCRMStopCustTask(Entities.OrderCRMStopCustTask oldModel, Entities.OrderCRMStopCustTask newModel, int userID)
        //{
        //    string userLogStr = string.Empty;
        //    string logStr = string.Empty;

        //    Hashtable ht_FieldName = new Hashtable();

        //    ht_FieldName.Add("RelationID", "关联ID（CRM系统停用客户申请表主键ID）");
        //    ht_FieldName.Add("AssignUserID", "分配坐席");
        //    ht_FieldName.Add("AssignTime", "分配时间");
        //    ht_FieldName.Add("SubmitTime", "提交任务时间");
        //    ht_FieldName.Add("CreateUserID", "操作人");

        //    BLL.GetLogDesc.ht_FieldName = ht_FieldName;

        //    Hashtable ht_FieldType = new Hashtable();

        //    ht_FieldType.Add("AssignUserID", "UserID");
        //    ht_FieldType.Add("CreateUserID", "UserID");

        //    BLL.GetLogDesc.ht_FieldType = ht_FieldType;

        //    if (oldModel == null)//为空，则是新增
        //    {
        //        //插入日志
        //        BLL.GetLogDesc.getAddLogInfo(newModel, out userLogStr);

        //        logStr = "CRM客户停用任务信息表新增：" + userLogStr;
        //    }
        //    else //不为空，则是编辑
        //    {
        //        //插入日志 
        //        BLL.GetLogDesc.getCompareLogInfo(oldModel, newModel, out userLogStr);

        //        logStr = "CRM客户停用任务信息表编辑：" + userLogStr;
        //    }

        //    if (userLogStr != string.Empty)
        //    {
        //        InsertUserLog(logStr, userID);
        //    }
        //}

        //public static void InsertLogOrderCRMStopCustTaskOperationLog(Entities.OrderCRMStopCustTaskOperationLog oldModel, Entities.OrderCRMStopCustTaskOperationLog newModel, int userID)
        //{
        //    string userLogStr = string.Empty;
        //    string logStr = string.Empty;

        //    Hashtable ht_FieldName = new Hashtable();

        //    ht_FieldName.Add("OperationStatus", "操作状态");
        //    ht_FieldName.Add("TaskStatus", "任务状态");
        //    ht_FieldName.Add("CreateUserID", "操作人");

        //    BLL.GetLogDesc.ht_FieldName = ht_FieldName;

        //    Hashtable ht_FieldType = new Hashtable();

        //    ht_FieldType.Add("OperationStatus", GetOperStatus());
        //    ht_FieldType.Add("TaskStatus", GetTaskStatus());
        //    ht_FieldType.Add("CreateUserID", "UserID");

        //    BLL.GetLogDesc.ht_FieldType = ht_FieldType;

        //    if (oldModel == null)//为空，则是新增
        //    {
        //        //插入日志
        //        BLL.GetLogDesc.getAddLogInfo(newModel, out userLogStr);

        //        logStr = "CRM客户停用任务信息表新增：" + userLogStr;
        //    }
        //    else //不为空，则是编辑
        //    {
        //        //插入日志 
        //        BLL.GetLogDesc.getCompareLogInfo(oldModel, newModel, out userLogStr);

        //        logStr = "CRM客户停用任务信息表编辑：" + userLogStr;
        //    }

        //    if (userLogStr != string.Empty)
        //    {
        //        InsertUserLog(logStr, userID);
        //    }
        //}


        //停用申请状态（枚举）

        private static Hashtable GetStopStatus()
        {
            Hashtable ht_msg = new Hashtable();
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.StopCustStopStatus));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ht_msg.Add(dr["value"].ToString(), dr["name"].ToString());
            }
            return ht_msg;
        }

        //操作状态（枚举）
        private static Hashtable GetOperStatus()
        {
            Hashtable ht_msg = new Hashtable();

            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.StopCustTaskOperStatus));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ht_msg.Add(dr["value"].ToString(), dr["name"].ToString());
            }

            return ht_msg;
        }

        //任务状态（枚举）
        private static Hashtable GetTaskStatus()
        {
            Hashtable ht_msg = new Hashtable();

            DataTable dt = BLL.Util.GetEnumDataTable(typeof(Entities.StopCustTaskStatus));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                ht_msg.Add(dr["value"].ToString(), dr["name"].ToString());
            }

            return ht_msg;
        }

        public static int InsertUserLog(string loginInfo, int userID)
        {
            Entities.UserActionLog model = new Entities.UserActionLog();
            model.UserEID = userID;
            model.TrueName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(model.UserEID);
            model.Loginfo = loginInfo;
            model.IP = GetIP();
            model.CreateTime = DateTime.Now;
            return BLL.UserActionLog.Instance.Insert(model);
        }

        /// <summary>
        /// 获取ip地址
        /// </summary>
        /// <returns></returns>
        private static string GetIP()
        {
            string ip = "";
            try
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["remote_addr"];
            }
            catch { }
            return ip;
        }

    }
}