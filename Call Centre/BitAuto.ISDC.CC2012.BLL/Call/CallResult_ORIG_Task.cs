using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CallResult_ORIG_Task
    {
        public static CallResult_ORIG_Task Instance = new CallResult_ORIG_Task();

        /// 插入或者更新一条记录
        /// <summary>
        /// 插入或者更新一条记录
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="source"></param>
        /// <param name="IsEstablish"></param>
        /// <param name="NotEstablishReason"></param>
        /// <param name="IsSuccess"></param>
        /// <param name="NotSuccessReason"></param>
        /// <returns></returns>
        public bool InseretOrUpdateOneData(string BusinessID, long ProjectID, ProjectSource source, bool? IsEstablish, NotEstablishReason? NotEstablishReason, bool? IsSuccess, NotSuccessReason? NotSuccessReason)
        {
            CallResult_ORIG_TaskInfo info = new CallResult_ORIG_TaskInfo();
            info.BusinessID = BusinessID;
            info.ProjectID = ProjectID;
            info.Source = (int)source;

            info.IsEstablish = CommonFunction.BoolToInt(IsEstablish);
            info.NotEstablishReason = NotEstablishReason.HasValue ? (int?)NotEstablishReason.Value : null;
            info.IsSuccess = CommonFunction.BoolToInt(IsSuccess);
            info.NotSuccessReason = NotSuccessReason.HasValue ? (int?)NotSuccessReason.Value : null;

            info.Status = 0;

            CallResult_ORIG_TaskInfo old = GetCallResult_ORIG_TaskInfoByBusinessID(BusinessID);
            if (old == null)
            {
                //新增
                info.CreateUserID = CommonFunction.ObjectToInteger(BLL.Util.GetLoginUserIDNotCheck(), -1);
                info.CreateTime = DateTime.Now;
                return CommonBll.Instance.InsertComAdoInfo(info);
            }
            else
            {
                //更新
                info.RecID = old.RecID;
                info.LastUpdateUserID = CommonFunction.ObjectToInteger(BLL.Util.GetLoginUserIDNotCheck(), -1);
                info.LastUpdateTime = DateTime.Now;
                return CommonBll.Instance.UpdateComAdoInfo(info);
            }
        }
        /// 插入或者更新一条记录
        /// <summary>
        /// 插入或者更新一条记录
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="source"></param>
        /// <param name="IsEstablish"></param>
        /// <param name="NotEstablishReason"></param>
        /// <param name="IsSuccess"></param>
        /// <param name="NotSuccessReason"></param>
        /// <returns></returns>
        public bool InseretOrUpdateOneData(string BusinessID, long ProjectID, ProjectSource source, int? IsEstablish, int? NotEstablishReason, int? IsSuccess, int? NotSuccessReason)
        {
            CallResult_ORIG_TaskInfo info = new CallResult_ORIG_TaskInfo();
            info.BusinessID = BusinessID;
            info.ProjectID = ProjectID;
            info.Source = (int)source;

            info.IsEstablish = IsEstablish;
            info.NotEstablishReason = NotEstablishReason;
            info.IsSuccess = IsSuccess;
            info.NotSuccessReason = NotSuccessReason;

            info.Status = 0;

            CallResult_ORIG_TaskInfo old = GetCallResult_ORIG_TaskInfoByBusinessID(BusinessID);
            if (old == null)
            {
                //新增
                info.CreateUserID = CommonFunction.ObjectToInteger(BLL.Util.GetLoginUserIDNotCheck(), -1);
                info.CreateTime = DateTime.Now;
                return CommonBll.Instance.InsertComAdoInfo(info);
            }
            else
            {
                //更新
                info.RecID = old.RecID;
                info.LastUpdateUserID = CommonFunction.ObjectToInteger(BLL.Util.GetLoginUserIDNotCheck(), -1);
                info.LastUpdateTime = DateTime.Now;
                return CommonBll.Instance.UpdateComAdoInfo(info);
            }
        }

        /// 根据业务ID获取实体类型
        /// <summary>
        /// 根据业务ID获取实体类型
        /// </summary>
        /// <param name="BusinessID"></param>
        /// <returns></returns>
        public CallResult_ORIG_TaskInfo GetCallResult_ORIG_TaskInfoByBusinessID(string BusinessID)
        {
            return Dal.CallResult_ORIG_Task.Instance.GetCallResult_ORIG_TaskInfoByBusinessID(BusinessID);
        }

        /// 获取未接通选项--1|选项一;2|选项二
        /// <summary>
        /// 获取未接通选项--1|选项一;2|选项二
        /// </summary>
        /// <returns></returns>
        public static string GetNotEstablishReasonStr()
        {
            string str = "";
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(NotEstablishReason));
            foreach (DataRow dr in dt.Rows)
            {
                str += CommonFunction.ObjectToInteger(dr["value"]).ToString() + "|" + dr["name"].ToString() + ";";
            }
            return str.TrimEnd(';');
        }
        /// 获取失败选项--1|选项一;2|选项二
        /// <summary>
        /// 获取失败选项--1|选项一;2|选项二
        /// </summary>
        /// <returns></returns>
        public static string GetNotSuccessReasonStr()
        {
            string str = "";
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(NotSuccessReason));
            foreach (DataRow dr in dt.Rows)
            {
                str += CommonFunction.ObjectToInteger(dr["value"]).ToString() + "|" + dr["name"].ToString() + ";";
            }
            return str.TrimEnd(';');
        }

        /// 获取失败选项--1|选项一;2|选项二
        /// <summary>
        /// 获取失败选项--1|选项一;2|选项二
        /// </summary>
        /// <returns></returns>
        public static string GetNotSuccessReasonStr(List<int> filterList)
        {
            string str = "";
            DataTable dt = BLL.Util.GetEnumDataTable(typeof(NotSuccessReason));

            for (int i = 0; i < filterList.Count; i++)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int val = CommonFunction.ObjectToInteger(dr["value"]);
                    if (val == filterList[i])
                    {
                        str += CommonFunction.ObjectToInteger(dr["value"]).ToString() + "|" + dr["name"].ToString() + ";";
                    }
                }
            }
            return str.TrimEnd(';');
        }
    }
}
