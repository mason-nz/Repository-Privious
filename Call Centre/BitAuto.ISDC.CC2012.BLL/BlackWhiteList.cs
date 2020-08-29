using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Data.SqlClient;
using System.Configuration;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class BlackWhiteList
    {
        public static readonly BlackWhiteList Instance = new BlackWhiteList();

        #region CC数据库
        /// <summary>
        /// 获取需要同步的新增的黑、白名单数据（黑名单和白名单的数据一起同步）
        /// </summary>
        /// <returns></returns>
        public DataTable GetSynchrodata_BlackWhiteData_Insert()
        {
            return Dal.BlackWhiteList.Instance.GetSynchrodata_BlackWhiteData_Insert();
        }
        /// <summary>
        /// 获取需要同步的更新了的黑、白名单数据（黑名单和白名单的数据一起同步）
        /// </summary>
        /// <returns></returns>
        public DataTable GetSynchrodata_BlackWhiteData_Update()
        {
            return Dal.BlackWhiteList.Instance.GetSynchrodata_BlackWhiteData_Update();
        }
        /// <summary>
        /// 将指定的RecIDS(多个RecID之间用“，”隔开)的SynchrodataStatus值改为2
        /// </summary>
        /// <param name="RecIDS"></param>
        /// <returns></returns>
        public bool UpdateSuccessSynchrodateStatus(string RecIDS)
        {
            int retVal = Dal.BlackWhiteList.Instance.UpdateSuccessSynchrodateStatus(RecIDS);
            if (retVal > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 逻辑删除数据
        /// </summary>
        /// <param name="RecIDS"></param>
        /// <returns></returns>
        public bool DeleteByChangeStatus(int RecID, int UserId)
        {
            int retVal = Dal.BlackWhiteList.Instance.DeleteByChangeStatus(RecID, UserId);
            if (retVal > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据指定条件查询黑、白名单数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetBlackWhiteData(QueryBlackWhite query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.BlackWhiteList.Instance.GetBlackWhiteData(query, order, currentPage, pageSize, out totalCount);
        }

        public bool IsPhoneNumberCDIDExist(string PhoneNumber, int CSID)
        {
            if (Dal.BlackWhiteList.Instance.IsPhoneNumExist(PhoneNumber, CSID) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsPhoneNumExist(string PhoneNumber, int CSID, int RecID)
        {
            if (Dal.BlackWhiteList.Instance.IsPhoneNumExist(PhoneNumber, CSID, RecID) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.BlackWhiteList GetModel(int RecId)
        {
            return Dal.BlackWhiteList.Instance.GetModel(RecId);
        }
        public int Add(Entities.BlackWhiteList model)
        {
            return Dal.BlackWhiteList.Instance.Add(model);
        }
        public bool Update(Entities.BlackWhiteList model)
        {
            return Dal.BlackWhiteList.Instance.Update(model);
        }
        public string ImportData(DataTable dt)
        {
            //映射关系
            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("Type", "Type"));
            list.Add(new SqlBulkCopyColumnMapping("PhoneNum", "PhoneNum"));
            list.Add(new SqlBulkCopyColumnMapping("EffectiveDate", "EffectiveDate"));
            list.Add(new SqlBulkCopyColumnMapping("ExpiryDate", "ExpiryDate"));
            list.Add(new SqlBulkCopyColumnMapping("CallType", "CallType"));
            list.Add(new SqlBulkCopyColumnMapping("CDIDS", "CDIDS"));
            list.Add(new SqlBulkCopyColumnMapping("Reason", "Reason"));
            list.Add(new SqlBulkCopyColumnMapping("SynchrodataStatus", "SynchrodataStatus"));
            list.Add(new SqlBulkCopyColumnMapping("CreateUserId", "CreateUserId"));
            list.Add(new SqlBulkCopyColumnMapping("CreateDate", "CreateDate"));
            list.Add(new SqlBulkCopyColumnMapping("UpdateUserId", "UpdateUserId"));
            list.Add(new SqlBulkCopyColumnMapping("UpdateDate", "UpdateDate"));
            list.Add(new SqlBulkCopyColumnMapping("Status", "Status"));

            //批量新增
            string msg = "";
            Util.BulkCopyToDB(dt, Dal.BlackWhiteList.Instance.Conn, "BlackWhiteList", 10000, list, out msg);
            return msg;
        }
        #endregion

        #region Holly数据库
        /// 判断电话号码是什么类型 0：黑名单；1：白名单；2：一般用户
        /// <summary>
        /// 判断电话号码是什么类型 0：黑名单；1：白名单；2：一般用户
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <param name="CallDisplayId"></param>
        /// <returns></returns>
        public int GetPhoneNumberType(string ZhuJiaoPhoneNumber, string BeiJiaoPhoneNum, out string msg)
        {
            string MutilID = Dal.BlackWhiteList.Instance.GetCallDisplayMutilIDHolly(BeiJiaoPhoneNum);
            if (string.IsNullOrEmpty(MutilID))
            {
                msg = "查询不到热线数据：" + BeiJiaoPhoneNum;
                return 2;
            }
            else
            {
                msg = "查询多选ID：" + MutilID;
                return Dal.BlackWhiteList.Instance.GetPhoneNumberType(ZhuJiaoPhoneNumber, CommonFunction.ObjectToInteger(MutilID));
            }
        }
        /// 新增或者更新数据
        /// <summary>
        /// 新增或者更新数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int UpdateDataForSynch(DataTable dt)
        {
            int successCoun = 0;
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Entities.BlackWhiteList model = new Entities.BlackWhiteList();
                    model.RecId = Convert.ToInt32(dt.Rows[i]["RecId"]);
                    model.Type = Convert.ToInt32(dt.Rows[i]["Type"]);
                    model.PhoneNum = dt.Rows[i]["PhoneNum"].ToString();
                    model.EffectiveDate = (dt.Rows[i]["EffectiveDate"] == null ? Constant.DATE_INVALID_VALUE : Convert.ToDateTime(dt.Rows[i]["EffectiveDate"]));
                    model.ExpiryDate = (dt.Rows[i]["ExpiryDate"] == null ? Constant.DATE_INVALID_VALUE : Convert.ToDateTime(dt.Rows[i]["ExpiryDate"]));
                    model.CallType = Convert.ToInt32(dt.Rows[i]["CallType"]);
                    model.CDIDS = Convert.ToInt32(dt.Rows[i]["CDIDS"]);
                    model.Reason = dt.Rows[i]["Reason"].ToString();
                    model.SynchrodataStatus = 2;
                    model.CreateUserId = (dt.Rows[i]["CreateUserId"] == null ? Constant.INT_INVALID_VALUE : Convert.ToInt32(dt.Rows[i]["CreateUserId"]));
                    model.CreateDate = (dt.Rows[i]["CreateDate"] == null ? Constant.DATE_INVALID_VALUE : Convert.ToDateTime(dt.Rows[i]["CreateDate"]));
                    model.UpdateDate = (dt.Rows[i]["UpdateDate"] == null ? Constant.DATE_INVALID_VALUE : Convert.ToDateTime(dt.Rows[i]["UpdateDate"]));
                    model.UpdateUserId = (dt.Rows[i]["UpdateUserId"] == null ? Constant.INT_INVALID_VALUE : Convert.ToInt32(dt.Rows[i]["UpdateUserId"]));
                    model.Status = Convert.ToInt32(dt.Rows[i]["Status"]);

                    if (Dal.BlackWhiteList.Instance.IsRecIdExistForSynch(Convert.ToInt32(dt.Rows[i]["RecId"])) > 0)
                    {
                        if (Dal.BlackWhiteList.Instance.UpdateForSynch(model))
                        {
                            successCoun++;
                        }
                    }
                    else
                    {
                        if (Dal.BlackWhiteList.Instance.AddForSynch(model) > 0)
                        {
                            successCoun++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("同步修改的黑白名单数据时出现异常：" + ex.Message);
            }
            return successCoun;
        }
        /// 批量删除数据
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteListForSynch(string RecIdlist)
        {
            return Dal.BlackWhiteList.Instance.DeleteListForSynch(RecIdlist);
        }
        /// 同步黑白名单数据
        /// <summary>
        /// 同步黑白名单数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string SynchBlackWhiteData(DataTable dt)
        {
            //映射关系
            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("RecId", "RecId"));
            list.Add(new SqlBulkCopyColumnMapping("Type", "Type"));
            list.Add(new SqlBulkCopyColumnMapping("PhoneNum", "PhoneNum"));
            list.Add(new SqlBulkCopyColumnMapping("EffectiveDate", "EffectiveDate"));
            list.Add(new SqlBulkCopyColumnMapping("ExpiryDate", "ExpiryDate"));
            list.Add(new SqlBulkCopyColumnMapping("CallType", "CallType"));
            list.Add(new SqlBulkCopyColumnMapping("CDIDS", "CDIDS"));
            list.Add(new SqlBulkCopyColumnMapping("Reason", "Reason"));
            list.Add(new SqlBulkCopyColumnMapping("SynchrodataStatus", "SynchrodataStatus"));
            list.Add(new SqlBulkCopyColumnMapping("CreateUserId", "CreateUserId"));
            list.Add(new SqlBulkCopyColumnMapping("CreateDate", "CreateDate"));
            list.Add(new SqlBulkCopyColumnMapping("UpdateUserId", "UpdateUserId"));
            list.Add(new SqlBulkCopyColumnMapping("UpdateDate", "UpdateDate"));
            list.Add(new SqlBulkCopyColumnMapping("Status", "Status"));

            //批量新增
            string msg = "";
            Util.BulkCopyToDB(dt, Dal.BlackWhiteList.Instance.ConnCCBlackWhiteSynch, "BlackWhiteList", 10000, list, out msg);
            return msg;
        }
        #endregion

        #region 黑名单验证
        /// 验证是否黑名单
        /// <summary>
        /// 验证是否黑名单
        /// </summary>
        /// <param name="blackListCheckType">验证类型</param>
        /// <param name="phone">号码</param>
        /// <returns>true 是黑名单 false 不是</returns>
        public bool CheckPhoneAndTelIsInBlackList(BlackListCheckType blackListCheckType, string phone)
        {
            return Dal.BlackWhiteList.Instance.CheckPhoneAndTelIsInBlackList(blackListCheckType, phone);
        }
        /// 验证是否黑名单
        /// <summary>
        /// 验证是否黑名单
        /// </summary>
        /// <param name="projectID">项目ID</param>
        /// <param name="phone">号码</param>
        /// <returns>true 是黑名单 false 不是</returns>
        public bool CheckPhoneAndTelIsInBlackList(long projectID, string phone)
        {
            Entities.ProjectInfo pinfo = BLL.ProjectInfo.Instance.GetProjectInfo(projectID);
            //是否启用黑名单校验 1启用 0禁用
            if (pinfo.IsBlacklistCheck.GetValueOrDefault(-2) == 1)
            {
                if (pinfo.BlacklistCheckType.HasValue)
                {
                    BlackListCheckType blackListCheckType = (BlackListCheckType)pinfo.BlacklistCheckType.Value;
                    return CheckPhoneAndTelIsInBlackList(blackListCheckType, phone);
                }
            }
            //不进行验证，不是黑名单
            return false;
        }

        /// 判断此号码是否为免打扰号码：-1：是已删除的免打扰号码；0：是免打扰号码；1：是已过期的免打扰号码；2：不是免打扰号码
        /// <summary>
        /// 判断此号码是否为免打扰号码：-1：是已删除的免打扰号码；0：是免打扰号码；1：是已过期的免打扰号码；2：不是免打扰号码
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public int PhoneNumIsNoDisturb(string PhoneNumber)
        {
            return Dal.BlackWhiteList.Instance.PhoneNumIsNoDisturb(PhoneNumber);
        }
        /// 更新一条免打扰数据
        /// <summary>
        /// 更新一条免打扰数据
        /// </summary>
        public bool UpdateNoDisturbData(Entities.BlackWhiteList model)
        {
            return Dal.BlackWhiteList.Instance.UpdateNoDisturbData(model);
        }
        /// 增加一条免打扰数据
        /// <summary>
        /// 增加一条免打扰数据
        /// </summary>
        public int AddNoDisturbData(Entities.BlackWhiteList model)
        {
            return Dal.BlackWhiteList.Instance.AddNoDisturbData(model);
        }
        /// 根据电话号码和类型，获取RecID
        /// <summary>
        /// 根据电话号码和类型，获取RecID
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetRecIDByPhoneNumberAndType(string phoneNumber, int type)
        {
            return Dal.BlackWhiteList.Instance.GetRecIDByPhoneNumberAndType(phoneNumber, type);
        }
        /// 获取有效和失效的黑名单
        /// <summary>
        /// 获取有效和失效的黑名单
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="maxRow"></param>
        /// <returns></returns>
        public DataTable GetBlackListDataForOutCall(long timestamp, int maxRow)
        {
            return Dal.BlackWhiteList.Instance.GetBlackListDataForOutCall(timestamp, maxRow);
        }
        #endregion
    }
}
