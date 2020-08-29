using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.YanFa.Crm2009.Entities;
using System.Data;
using System.Collections;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustAudit
{
    /// <summary>
    /// CustAuditManager 的摘要说明
    /// </summary>
    public class CustAuditManager : IHttpHandler, IRequiresSessionState
    {
        public HttpContext currentContext;

        public int aduitSuccessCount = 0;//审核成功数量
        public int aduitFaileCount = 0;//审核失败数量
        public int verifyCount = 0;//未审核数量
        public int otherAduitFaileCount = 0;//其他未审核数量
        public string aduitFaileDesc = string.Empty;//审核失败描述
        public string verifyDesc = string.Empty;//未审核验证描述
        public string otherAduitFaileDesc = string.Empty;//其他未审核描述

        #region 属性
        private string RequestAuditType
        {
            get { return currentContext.Request.Form["AuditType"] == null ? string.Empty : currentContext.Request.Form["AuditType"].Trim(); }
        }
        private string RequestAudit
        {
            get { return currentContext.Request.Form["Audit"] == null ? string.Empty : currentContext.Request.Form["Audit"].Trim(); }
        }
        private string RequestDescription
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("Description")); }
        }
        private string RequestTID
        {
            get { return currentContext.Request.Form["TID"] == null ? string.Empty : currentContext.Request.Form["TID"].Trim(); }
        }

        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            currentContext = context;
            string re = "";

            if (RequestAudit.Trim() == "yes" && !string.IsNullOrEmpty(RequestTID))
            {
                string[] ids = RequestTID.Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    if (ids[i] != "")
                    {
                        string s = Audit(ids[i]);
                        if (s != "ok")
                        {
                            re += s + "<br/>";
                        }
                        else
                        {
                            aduitSuccessCount++;
                        }
                    }
                }
                string info = string.Format(@"aduitSuccessCount:{0},
                              aduitFaileCount:{1},verifyCount:{2},aduitFaileDesc:'{3}',
                              verifyDesc:'{4}',otherAduitFaileCount:'{5}',otherAduitFaileDesc:'{6}'", aduitSuccessCount, aduitFaileCount,
                              verifyCount, BLL.Util.EscapeString(aduitFaileDesc),
                              BLL.Util.EscapeString(verifyDesc), otherAduitFaileCount,
                              BLL.Util.EscapeString(otherAduitFaileDesc));
                if (re != "")
                {
                    context.Response.Write("{Update:\"" + BLL.Util.EscapeString(re) + "\"," + info + "}");
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    context.Response.Write("{Update:\"yes\"," + info + "}");
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }
        }
        /// <summary>
        /// 审核
        /// </summary> 
        /// <param name="tid"></param>
        private string Audit(string tid)
        {
            string re = "";
            //可以审核的状态有：提交完成180003，删除完成180004
            //审核通过：返回处理成功，处理失败 删除成功，删除失败
            //审核拒绝：拒绝后，可以再次 提交完成180003，删除完成180004
            //提交处理失败之后：可以操作：驳回180009
            Entities.ProjectTaskInfo ProjectTaskInfo = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(tid);
            Entities.ProjectTask_Cust ProjectTask_Cust = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(tid);
            if (ProjectTaskInfo != null && ProjectTask_Cust != null)
            {
                //提交完成
                if (ProjectTaskInfo.TaskStatus == (int)Entities.EnumProjectTaskStatus.SubmitFinsih)
                {
                    if (RequestAuditType == "AuditPass")
                    {
                        int isChange = 0;
                        string custChangeName = ProjectTask_Cust.CustName;
                        //通过
                        if (string.IsNullOrEmpty(ProjectTask_Cust.OriginalCustID))
                        {
                            //excel 
                            re = SubmitAuditPassExcel(ProjectTask_Cust, ProjectTaskInfo);
                        }
                        else
                        {
                            //crm数据库数据修改
                            //修改日期=2011-07-06,修改人=masj
                            re = SubmitAuditPassCRM(ProjectTask_Cust, ProjectTaskInfo, out isChange);
                        }
                        if (re == "ok")
                        {
                            //更新 CRMCustID 字段
                            ProjectTaskInfo.CrmCustID = ProjectTask_Cust.OriginalCustID;
                            BLL.ProjectTaskInfo.Instance.UpdateCrmCustID(ProjectTaskInfo.PTID, ProjectTask_Cust.OriginalCustID);
                            if (ProjectTaskInfo.Source == 1)//数据来源1===Excel导入，2===CRM
                            {
                                BLL.ProjectTask_DelCustRelation.Instance.UpdateCustIDByTID(ProjectTaskInfo.PTID, ProjectTask_Cust.OriginalCustID);
                            }
                            if (BLL.ProjectTask_AuditContrastInfo.Instance.InsertByDelCustRelation(ProjectTaskInfo.PTID, ProjectTask_Cust.CustName, custChangeName) > 0 || isChange == 1)
                            {
                                DateTime dtime = DateTime.Now;
                                //成功
                                BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(ProjectTaskInfo.PTID, EnumProjectTaskStatus.StayReview, EnumProjectTaskOperationStatus.TaskAuditSuccess, dtime);
                            }
                            else
                            {
                                DateTime dtime = DateTime.Now;
                                BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(ProjectTaskInfo.PTID, EnumProjectTaskStatus.Finshed, EnumProjectTaskOperationStatus.TaskAuditSuccess, dtime);
                            }
                        }
                        else if (!re.StartsWith("VerifyLogic"))//不是验证逻辑，便可以记录到日志中去
                        {
                        }
                    }
                    if (RequestAuditType == "CallBack")
                    {
                        DateTime dtime = DateTime.Now;
                        //打回,更改任务状态为处理中
                        BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(ProjectTaskInfo.PTID, EnumProjectTaskStatus.Assigning, EnumProjectTaskOperationStatus.TaskCallBack, RequestDescription, dtime);
                        BLL.ProjectTaskInfo.Instance.InsertOrUpdateTaskAdditionalStatus(ProjectTaskInfo.PTID, "AS_K", "");
                        re = "ok";
                    }
                    if (RequestAuditType == "AuditRefuse")
                    {
                        DateTime dtime = DateTime.Now;
                        //拒绝,更改任务状态为已完成
                        BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(ProjectTaskInfo.PTID, EnumProjectTaskStatus.Finshed, EnumProjectTaskOperationStatus.TaskAuditReject, RequestDescription, dtime);
                        re = "ok";
                    }
                }
                else if (ProjectTaskInfo.TaskStatus == (int)EnumProjectTaskStatus.DelFinsh)//删除完成
                {
                    if (RequestAuditType == "AuditPass")
                    {
                        //通过  

                        if (string.IsNullOrEmpty(ProjectTask_Cust.OriginalCustID))
                        {
                            //excel 
                            re = "ok";
                        }
                        else
                        {
                            //crm数据库数据修改
                            re = DelAuditPassCRM(ProjectTask_Cust);
                        }
                        if (re == "ok")
                        {
                            DateTime dtime = DateTime.Now;
                            //成功
                            BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(ProjectTaskInfo.PTID, EnumProjectTaskStatus.Finshed, EnumProjectTaskOperationStatus.TaskAuditSuccess, dtime);
                        }
                        else
                        {
                        }

                    }
                    else if (RequestAuditType == "AuditRefuse")
                    {
                        //拒绝
                        BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(ProjectTaskInfo.PTID, EnumProjectTaskStatus.Finshed, EnumProjectTaskOperationStatus.TaskAuditReject, RequestDescription, DateTime.Now);
                        re = "ok";
                    }
                    else if (RequestAuditType == "CallBack")
                    {
                        DateTime dtime = DateTime.Now;
                        //打回
                        BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(ProjectTaskInfo.PTID, EnumProjectTaskStatus.Assigning, EnumProjectTaskOperationStatus.TaskCallBack, RequestDescription, dtime);
                        BLL.ProjectTaskInfo.Instance.InsertOrUpdateTaskAdditionalStatus(ProjectTaskInfo.PTID, "AS_K", "");
                        re = "ok";
                    }
                }
                else if (ProjectTaskInfo.TaskStatus == (int)EnumProjectTaskStatus.Stop)
                {
                    DateTime dtime = DateTime.Now;
                    //通过
                    if (RequestAuditType == "AuditPass")
                    {
                        DataTable dt = BLL.ProjectTaskLog.Instance.GetProjectTaskLog(ProjectTaskInfo.PTID);
                        string stopReason = string.Empty;
                        if (dt != null)
                        {
                            if (int.Parse(dt.Rows[0]["TaskStatus"].ToString()) == (int)EnumProjectTaskStatus.Stop)
                            {
                                stopReason = dt.Rows[0]["Description"].ToString();
                            }
                        }
                        Entities.ProjectTask_AuditContrastInfo auditInfo = new Entities.ProjectTask_AuditContrastInfo();
                        auditInfo.ContrastType = 5;
                        auditInfo.CreateTime = dtime;
                        auditInfo.CreateUserID = BLL.Util.GetLoginUserID();
                        auditInfo.CustID = ProjectTaskInfo.CrmCustID;
                        auditInfo.DisposeStatus = 0;
                        auditInfo.ExportStatus = 0;
                        auditInfo.PTID = ProjectTaskInfo.PTID;
                        auditInfo.ContrastInfo = stopReason;
                        auditInfo.Remark = "客户信息停用";
                        BLL.ProjectTask_AuditContrastInfo.Instance.Insert(auditInfo);
                        //客户停用，任务改为待复核状态
                        BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(ProjectTaskInfo.PTID, EnumProjectTaskStatus.StayReview, EnumProjectTaskOperationStatus.TaskAuditSuccess, dtime);
                        re = "ok";
                    }
                    else if (RequestAuditType == "AuditRefuse")
                    {
                        BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(ProjectTaskInfo.PTID, EnumProjectTaskStatus.Finshed, EnumProjectTaskOperationStatus.TaskAuditReject, dtime);
                        re = "ok";
                    }
                    else if (RequestAuditType == "CallBack")
                    {
                        //打回
                        BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(ProjectTaskInfo.PTID, EnumProjectTaskStatus.Assigning, EnumProjectTaskOperationStatus.TaskCallBack, RequestDescription, dtime);
                        BLL.ProjectTaskInfo.Instance.InsertOrUpdateTaskAdditionalStatus(ProjectTaskInfo.PTID, "AS_K", "");
                        re = "ok";
                    }
                }
            }
            return re;
        }

        /// <summary>
        /// 提交 审核通过 excel 数据源
        /// </summary>
        /// <param name="ProjectTask_Cust"></param>
        /// <param name="ProjectTaskInfo"></param>
        /// <returns>成功返回true，失败返回false</returns>
        private string SubmitAuditPassExcel(BitAuto.ISDC.CC2012.Entities.ProjectTask_Cust ProjectTask_Cust, BitAuto.ISDC.CC2012.Entities.ProjectTaskInfo ProjectTaskInfo)
        {
            string verifyStr = "";
            try
            {
                BitAuto.YanFa.Crm2009.Entities.QueryCustInfo queryCustInfo = new QueryCustInfo();
                queryCustInfo.ExistCustName = ProjectTask_Cust.CustName;
                int o;

                #region 客户信息验证
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(queryCustInfo, "", 1, 1, out o);
                if (dt != null && dt.Rows.Count > 0)
                {
                    verifyStr = "提交审核失败，与CRM系统中的客户名称重复";
                    aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"" + verifyStr + ",<br/>";
                    aduitFaileCount++;
                    //客户名称 存在，处理失败
                    return verifyStr;
                }
                #endregion


                List<Entities.ProjectTask_CSTMember> ccCstMembers = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(ProjectTaskInfo.PTID);
                List<Entities.ProjectTask_DMSMember> ccMembers = BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMemberByTID(ProjectTaskInfo.PTID);
                //20004 特许经销商、20005综合店 、20009展厅时，可以没有会员
                if ((ProjectTask_Cust.CarType != 2 && ProjectTask_Cust.TypeID != "20004" && ProjectTask_Cust.TypeID != "20005" && ProjectTask_Cust.TypeID != "20009" && ccMembers.Count == 0))
                {
                    //verifyStr = "\"" + ProjectTask_Cust.CustName + "\"客户无易湃会员";
                    verifyStr = "\"" + ProjectTask_Cust.CustName + "\"客户无易湃会员";
                    verifyCount++;
                    verifyDesc += verifyStr + ",<br/>";
                    return "VerifyLogic," + verifyStr + "，可点击“打回”打回相应座席";
                }

                #region 会员信息验证
                if (ccMembers.Count > 0 && ProjectTask_Cust.CarType != 2)
                {
                    Hashtable memberNames = new Hashtable();
                    for (int i = 0; i < ccMembers.Count; i++)
                    {
                        //QueryDMSMember queryDMSMember = new QueryDMSMember();
                        ////////会员之单位有没有重名-------Modify=Masj,Date=20110516///////
                        if (memberNames.ContainsKey(ccMembers[i].Abbr))
                        {
                            verifyStr = "会员简称不可重复";
                            aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的会员简称：\"" + ccMembers[i].Abbr + "\"" + verifyStr + ",<br/>";
                            return verifyStr;
                        }
                        else { memberNames.Add(ccMembers[i].Abbr, ccMembers[i].Abbr); }
                        //////////////////////////////////////
                        //queryDMSMember.Abbr = ccMembers[i].Abbr;
                        //DataTable dttemp = Crm2009.BLL.DMSMember.Instance.GetDMSMember(queryDMSMember, "", 1, 1, out o);
                        //if (dttemp != null && dttemp.Rows.Count > 0)
                        if (BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.IsExistsByAbbrName(ccMembers[i].Abbr, 1))
                        {
                            verifyStr = "提交审核失败，与CRM系统中的会员简称重复";
                            aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的会员简称：\"" + ccMembers[i].Abbr + "\"" + verifyStr + ",<br/>";
                            aduitFaileCount++;
                            //会员简称 存在，处理失败
                            return verifyStr;
                        }
                    }
                }
                #endregion

                #region 车商通会员简称验证不能重复

                if (ccCstMembers.Count > 0 && ProjectTask_Cust.CarType != 1)
                {
                    Hashtable memberNames = new Hashtable();
                    for (int i = 0; i < ccCstMembers.Count; i++)
                    {
                        //QueryDMSMember queryDMSMember = new QueryDMSMember();
                        ////////会员之单位有没有重名-------Modify=Masj,Date=20110516///////
                        if (memberNames.ContainsKey(ccCstMembers[i].ShortName))
                        {
                            verifyStr = "车商通会员简称不可重复";
                            aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员简称：\"" + ccCstMembers[i].ShortName + "\"" + verifyStr + ",<br/>";
                            return verifyStr;
                        }
                        else { memberNames.Add(ccCstMembers[i].ShortName, ccCstMembers[i].ShortName); }

                        #region 比对Crm查找是否有重复的简称
                        //是否存在重名的会员名称
                        Entities.ProjectTask_CSTMember memberInfo = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(ccCstMembers[i].ID);
                        if (!string.IsNullOrEmpty(memberInfo.OriginalCSTRecID))
                        {
                            string where = " ShortName='" + Utils.StringHelper.SqlFilter(ccCstMembers[i].ShortName) + "' and CSTRecID!='" + memberInfo.OriginalCSTRecID + "'";
                            if (BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                            {
                                verifyStr = "提交审核失败，与CRM系统中的车商通会员简称重复";
                                aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员简称：\"" + ccCstMembers[i].ShortName + "\"" + verifyStr + ",<br/>";
                                aduitFaileCount++;
                                //会员简称 存在，处理失败
                                return verifyStr;
                            }
                        }
                        else
                        {
                            string where = " ShortName='" + Utils.StringHelper.SqlFilter(ccCstMembers[i].ShortName) + "'";
                            if (BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                            {
                                verifyStr = "提交审核失败，与CRM系统中的车商通会员简称重复";
                                aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员简称：\"" + ccCstMembers[i].ShortName + "\"" + verifyStr + ",<br/>";
                                aduitFaileCount++;
                                //会员简称 存在，处理失败
                                return verifyStr;
                            }
                        }
                        #endregion
                    }
                }
                //else if (ccMembers.Count == 0 && ProjectTask_Cust.CarType != 2)
                //{
                //    verifyStr = "\"" + ProjectTask_Cust.CustName + "\"客户无会员";
                //    verifyCount++;
                //    verifyDesc += verifyStr + ",<br/>";
                //    return "VerifyLogic," + verifyStr + "，可点击“审核拒绝”打回相应座席";
                //}

                #endregion

                #region 车商通会员全称可以重复 lxw 12.11.7修改


                #region 车商通会员全称验证不能重复
                //if (ccCstMembers.Count > 0 && ProjectTask_Cust.CarType != 1)
                //{
                //    Hashtable memberFullNames = new Hashtable();
                //    for (int i = 0; i < ccCstMembers.Count; i++)
                //    {
                //        //QueryDMSMember queryDMSMember = new QueryDMSMember();
                //        ////////会员之单位有没有重名-------Modify=Masj,Date=20110516///////
                //        if (memberFullNames.ContainsKey(ccCstMembers[i].FullName))
                //        {
                //            verifyStr = "车商通会员全称不可重复";
                //            aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员全称：\"" + ccCstMembers[i].FullName + "\"" + verifyStr + ",<br/>";
                //            return verifyStr;
                //        }
                //        else { memberFullNames.Add(ccCstMembers[i].FullName, ccCstMembers[i].FullName); }

                #region 比对Crm查找是否有重复的简称
                //是否存在重名的会员名称
                //Entities.ProjectTask_CSTMember memberInfo = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(ccCstMembers[i].ID);
                //if (!string.IsNullOrEmpty(memberInfo.OriginalCSTRecID))
                //{
                //    string where = " FullName='" + Utils.StringHelper.SqlFilter(ccCstMembers[i].FullName) + "' and CSTRecID!='" + memberInfo.OriginalCSTRecID + "'";
                //    if (Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                //    {
                //        verifyStr = "提交审核失败，与CRM系统中的车商通会员全称重复";
                //        aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员全称：\"" + ccCstMembers[i].FullName + "\"" + verifyStr + ",<br/>";
                //        aduitFaileCount++;
                //        //会员简称 存在，处理失败
                //        return verifyStr;
                //    }
                //}
                //else
                //{
                //    string where = " FullName='" + Utils.StringHelper.SqlFilter(ccCstMembers[i].FullName) + "'";
                //    if (Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                //    {
                //        verifyStr = "提交审核失败，与CRM系统中的车商通会员全称重复";
                //        aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员全称：\"" + ccCstMembers[i].FullName + "\"" + verifyStr + ",<br/>";
                //        aduitFaileCount++;
                //        //会员简称 存在，处理失败
                //        return verifyStr;
                //    }
                //}
                #endregion
                //    }
                //}

                #endregion


                #endregion

                //else if (ccMembers.Count == 0 && ProjectTask_Cust.CarType != 2)
                //{
                //    verifyStr = "\"" + ProjectTask_Cust.CustName + "\"客户无会员";
                //    verifyCount++;
                //    verifyDesc += verifyStr + ",<br/>";
                //    return "VerifyLogic," + verifyStr + "，可点击“审核拒绝”打回相应座席";
                //}

                #region 车商通会员编码不需要 12.11.7

                #region 车商通会员编码验证不能重复
                //if (ccCstMembers.Count > 0 && ProjectTask_Cust.CarType != 1)
                //{
                //    Hashtable memberVendorCodes = new Hashtable();
                //    for (int i = 0; i < ccCstMembers.Count; i++)
                //    {
                //        //QueryDMSMember queryDMSMember = new QueryDMSMember();
                //        ////////会员之单位有没有重名-------Modify=Masj,Date=20110516///////
                //        if (memberVendorCodes.ContainsKey(ccCstMembers[i].VendorCode))
                //        {
                //            verifyStr = "车商通会员编码不可重复";
                //            aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员编码：\"" + ccCstMembers[i].VendorCode + "\"" + verifyStr + ",<br/>";
                //            return verifyStr;
                //        }
                //        else { memberVendorCodes.Add(ccCstMembers[i].VendorCode, ccCstMembers[i].VendorCode); }

                #region 比对Crm查找是否有重复的简称
                //是否存在重名的会员名称
                //Entities.ProjectTask_CSTMember memberInfo = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(ccCstMembers[i].ID);
                //if (!string.IsNullOrEmpty(memberInfo.OriginalCSTRecID))
                //{
                //    string where = " VendorCode='" + Utils.StringHelper.SqlFilter(ccCstMembers[i].VendorCode) + "' and CSTRecID!='" + memberInfo.OriginalCSTRecID + "'";
                //    if (Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                //    {
                //        verifyStr = "提交审核失败，与CRM系统中的车商通会员编码重复";
                //        aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员编码：\"" + ccCstMembers[i].VendorCode + "\"" + verifyStr + ",<br/>";
                //        aduitFaileCount++;
                //        //会员简称 存在，处理失败
                //        return verifyStr;
                //    }
                //}
                //else
                //{
                //    string where = " VendorCode='" + Utils.StringHelper.SqlFilter(ccCstMembers[i].VendorCode) + "'";
                //    if (Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                //    {
                //        verifyStr = "提交审核失败，与CRM系统中的车商通会员编码重复";
                //        aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员编码：\"" + ccCstMembers[i].VendorCode + "\"" + verifyStr + ",<br/>";
                //        aduitFaileCount++;
                //        //会员简称 存在，处理失败
                //        return verifyStr;
                //    }
                //}
                #endregion

                //    }
                //} 
                #endregion
                #endregion

                //else if (ccMembers.Count == 0 && ProjectTask_Cust.CarType != 2)
                //{
                //    verifyStr = "\"" + ProjectTask_Cust.CustName + "\"客户无会员";
                //    verifyCount++;
                //    verifyDesc += verifyStr + ",<br/>";
                //    return "VerifyLogic," + verifyStr + "，可点击“审核拒绝”打回相应座席";
                //}

                //"客户名称"、"会员名称"不存在，插入一条Custs数据，相关表数据也插入（见上）（会员为申请状态） 
                BitAuto.YanFa.Crm2009.Entities.CustInfo model = InsertCustInfo(ProjectTask_Cust, ProjectTaskInfo.PTID, false);
                if (model == null) { throw new Exception("无法插入客户信息"); }
                InsertContactInfo(ProjectTask_Cust, model.CustID);
                int isDMSMemberChange = 0;
                InsertDMSMember(ProjectTask_Cust, model.CustID, out isDMSMemberChange);

                //插入车商通会员
                if (ProjectTask_Cust.CarType != 1)
                {
                    InsertCstMember(ProjectTask_Cust, model.CustID);
                }

            }
            catch (Exception e)
            {
                string s = " 审核异常：" + e.Message;
                BLL.Loger.Log4Net.Info(" 审核异常：" + e.Message);// "其他异常 ：" +; ;
                otherAduitFaileDesc += s + "<br/>";
                otherAduitFaileCount++;
                return s;
            }
            // 记录日志
            return "ok";
        }

        /// <summary>
        /// 提交 审核通过 crm 数据源
        /// </summary>
        /// <param name="ProjectTask_Cust"></param>
        /// <param name="ProjectTaskInfo"></param>
        /// <returns>成功返回true，失败返回false</returns>
        private string SubmitAuditPassCRM(BitAuto.ISDC.CC2012.Entities.ProjectTask_Cust ProjectTask_Cust, BitAuto.ISDC.CC2012.Entities.ProjectTaskInfo ProjectTaskInfo, out int isChange)
        {
            string verifyStr = "";
            isChange = 0;
            //更新 ProjectTask_Cust 到 CustInfo 数据  需要做同样的判断，如果名称修改， 需要判断是否有重名的，有处理失败，没有，更新
            //ProjectTask_Cust_Contact 到 ContactInfo  一条一条处理，有更新，没有插入
            //CC_Cust_Brand 到 Cust_Brand     删除后插入
            //ProjectTask_DMSMember 到 DMSMember 和 DMSMember_MapCoordinate  一条一条处理，有更新，没有插入 同事同步客户
            //ProjectTask_DMSMember_Brand 到 DMSMember_Brand        删除后插入    
            try
            {
                int o;
                BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ProjectTask_Cust.OriginalCustID);
                if (custInfo != null && custInfo.CustName != ProjectTask_Cust.CustName)
                {
                    string temp = string.Empty;
                    if (!BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.IsExistCustName(ProjectTask_Cust.CustName, out temp))//客户名称，在CRM库中不存在时
                    {
                        isChange = 1;
                        BLL.ProjectTask_AuditContrastInfo.Instance.InsertByCustNameChange(ProjectTask_Cust, custInfo);
                    }
                    ProjectTask_Cust.CustName = custInfo.CustName;//审核时，客户名称不更新
                }

                List<Entities.ProjectTask_CSTMember> ccCstMembers = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(ProjectTaskInfo.PTID);
                List<Entities.ProjectTask_DMSMember> ccMembers = BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMemberByTID(ProjectTaskInfo.PTID);
                if ((ProjectTask_Cust.CarType != 2 && ccMembers.Count == 0) && ProjectTask_Cust.TypeID == "20004" && ProjectTask_Cust.TypeID == "20005" && ProjectTask_Cust.TypeID == "20009")
                {
                    verifyStr = "\"" + ProjectTask_Cust.CustName + "\"客户无会员";
                    verifyCount++;
                    verifyDesc += verifyStr + ",<br/>";
                    return "VerifyLogic," + verifyStr + "，可点击“打回”打回相应座席";
                }

                //会员信息验证
                if (ccMembers.Count > 0 && ProjectTask_Cust.CarType != 2)
                {
                    for (int i = 0; i < ccMembers.Count; i++)
                    {
                        if (string.IsNullOrEmpty(ccMembers[i].OriginalDMSMemberID))//新增
                        {

                            if (BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.IsExistsByAbbrName(ccMembers[i].Abbr, 1))
                            {
                                verifyStr = "提交审核失败，与CRM系统中的会员简称重复";
                                aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的会员简称：\"" + ccMembers[i].Abbr + "\"" + verifyStr + ",<br/>";
                                aduitFaileCount++;
                                return verifyStr;
                            }
                        }
                        else//更新
                        {
                            if (BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.IsExistsByAbbrName(ccMembers[i].OriginalDMSMemberID, ccMembers[i].Abbr, 1))
                            {
                                verifyStr = "提交审核失败，与CRM系统中的会员简称重复";
                                aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的会员简称：\"" + ccMembers[i].Abbr + "\"" + verifyStr + ",<br/>";
                                aduitFaileCount++;
                                return verifyStr;
                            }
                        }
                    }
                }
                #region 车商通会员简称验证不能重复
                if (ccCstMembers.Count > 0 && ProjectTask_Cust.CarType != 1)
                {
                    Hashtable memberNames = new Hashtable();
                    for (int i = 0; i < ccCstMembers.Count; i++)
                    {
                        //QueryDMSMember queryDMSMember = new QueryDMSMember();
                        ////////会员之单位有没有重名-------Modify=Masj,Date=20110516///////
                        //如果是新增车商通会员
                        if (string.IsNullOrEmpty(ccCstMembers[i].OriginalCSTRecID))
                        {
                            if (memberNames.ContainsKey(ccCstMembers[i].ShortName))
                            {
                                verifyStr = "车商通会员简称不可重复";
                                aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员简称：\"" + ccCstMembers[i].ShortName + "\"" + verifyStr + ",<br/>";
                                return verifyStr;
                            }
                            else { memberNames.Add(ccCstMembers[i].ShortName, ccCstMembers[i].ShortName); }

                            #region 比对Crm查找是否有重复的简称
                            //是否存在重名的会员名称
                            Entities.ProjectTask_CSTMember memberInfo = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(ccCstMembers[i].ID);
                            if (!string.IsNullOrEmpty(memberInfo.OriginalCSTRecID))
                            {
                                string where = " ShortName='" + Utils.StringHelper.SqlFilter(ccCstMembers[i].ShortName) + "' and CSTRecID!='" + memberInfo.OriginalCSTRecID + "'";
                                if (BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                                {
                                    verifyStr = "提交审核失败，与CRM系统中的车商通会员简称重复";
                                    aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员简称：\"" + ccCstMembers[i].ShortName + "\"" + verifyStr + ",<br/>";
                                    aduitFaileCount++;
                                    //会员简称 存在，处理失败
                                    return verifyStr;
                                }
                            }
                            else
                            {
                                string where = " ShortName='" + Utils.StringHelper.SqlFilter(ccCstMembers[i].ShortName) + "'";
                                if (BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                                {
                                    verifyStr = "提交审核失败，与CRM系统中的车商通会员简称重复";
                                    aduitFaileDesc += "\"" + ProjectTask_Cust.CustName + "\"的车商通会员简称：\"" + ccCstMembers[i].ShortName + "\"" + verifyStr + ",<br/>";
                                    aduitFaileCount++;
                                    //会员简称 存在，处理失败
                                    return verifyStr;
                                }
                            }
                            #endregion
                        }
                    }
                }

                #endregion

                //"客户名称"、"会员名称"不存在，插入一条Custs数据，相关表数据也插入（见上）（会员为申请状态） 
                BitAuto.YanFa.Crm2009.Entities.CustInfo model = InsertCustInfo(ProjectTask_Cust, ProjectTaskInfo.PTID, true);
                if (model == null) { throw new Exception("无法插入客户信息"); }
                InsertContactInfo(ProjectTask_Cust, model.CustID);// 一条一条处理，有更新，没有插入 同事同步客户
                int isDMSMemberChange = 0;
                InsertDMSMember(ProjectTask_Cust, model.CustID, out isDMSMemberChange);// 一条一条处理，有更新，没有插入 同事同步客户
                if (isDMSMemberChange == 1)
                {
                    isChange = 1;
                }
                //插入车商通会员
                if (ProjectTask_Cust.CarType != 1)
                {
                    InsertCstMember(ProjectTask_Cust, model.CustID);
                }
            }
            catch (Exception e)
            {
                string s = "其它异常 ：" + e.Message;
                BLL.Loger.Log4Net.Info(" 审核异常：" + e.Message);
                otherAduitFaileDesc += s + "<br/>";
                otherAduitFaileCount++;
                return s;

            }
            // 记录日志
            return "ok";
        }

        /// <summary>
        /// 删除 审核通过 crm 数据源
        /// </summary>
        /// <param name="ProjectTask_Cust"></param> 
        /// <returns>成功返回true，失败返回false</returns>
        private string DelAuditPassCRM(BitAuto.ISDC.CC2012.Entities.ProjectTask_Cust ProjectTask_Cust)
        {
            try
            {

                List<BitAuto.YanFa.Crm2009.Entities.DMSMember> dmsMembers = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(ProjectTask_Cust.OriginalCustID);
                if (dmsMembers.Count > 0)
                {
                    return "此客户有会员，删除审核失败";
                }
                //先删除 品牌 ，在删除客户
                if (BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.UpdateStatus(ProjectTask_Cust.OriginalCustID, -1) > 0)
                {
                    return "ok";
                }
                //crm数据库数据修改
                //crm客户有会员信息，不可删除，处理失败;
                //crm客户可删除，直接删除
                //修改任务状态，记录日志
            }
            catch (Exception e)
            {
                return "其它异常 ：" + e.Message; ;
            }
            // 记录日志
            return "删除客户以及品牌失败";
        }

        /// <summary>
        /// 插入客户信息
        /// </summary>
        /// <param name="ProjectTask_Cust"></param>
        /// <param name="isUpdata">true 更新 false 添加</param>
        /// <returns></returns>
        private BitAuto.YanFa.Crm2009.Entities.CustInfo InsertCustInfo(BitAuto.ISDC.CC2012.Entities.ProjectTask_Cust ProjectTask_Cust, string tId, bool isUpdata)
        {
            BitAuto.YanFa.Crm2009.Entities.CustInfo model = null;
            if (isUpdata)
            {
                model = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(ProjectTask_Cust.OriginalCustID);
                if (model != null)
                {
                    if (model.Lock <= 0)//未锁定才能修改客户名称
                    {
                        model.CustName = ProjectTask_Cust.CustName;
                    }
                    model.AbbrName = ProjectTask_Cust.AbbrName;
                    model.TypeID = ProjectTask_Cust.TypeID;
                    model.IndustryID = ProjectTask_Cust.IndustryID;
                    model.LevelID = ProjectTask_Cust.LevelID;
                    //model.LicenseNumber = ProjectTask_Cust.l;
                    // 集团 厂商 没有 所属集团 所属厂商
                    if (model.TypeID == ((int)EnumCustomType.Company).ToString() || model.TypeID == ((int)EnumCustomType.Bloc).ToString())
                    {
                        model.Pid = "";
                        model.CustPid = ProjectTask_Cust.CustPid;
                    }
                    else
                    {
                        if (model.TypeID == ((int)EnumCustomType.SynthesizedShop).ToString())
                        {
                            //综合店 没有厂商
                            model.CustPid = "";
                        }
                        else
                        {
                            model.CustPid = ProjectTask_Cust.CustPid;
                        }
                        model.Pid = ProjectTask_Cust.Pid;
                    }
                    model.ShopLevel = ProjectTask_Cust.ShopLevel;
                    model.ProvinceID = ProjectTask_Cust.ProvinceID;
                    model.CityID = ProjectTask_Cust.CityID;
                    model.CountyID = ProjectTask_Cust.CountyID;
                    model.Address = ProjectTask_Cust.Address;
                    model.contactName = ProjectTask_Cust.ContactName;
                    model.zipcode = ProjectTask_Cust.Zipcode;
                    model.Officetel = ProjectTask_Cust.OfficeTel;
                    model.Fax = ProjectTask_Cust.Fax;
                    model.Notes = ProjectTask_Cust.Notes;
                    model.LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    model.LastUpdateUserID = ProjectTask_Cust.LastUpdateUserID;//Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
                    //添加所属4S字段 Modify---yangyh
                    model.FoursPid = ProjectTask_Cust.FoursPid;
                    //二手车经营类型
                    if (!string.IsNullOrEmpty(ProjectTask_Cust.UsedCarBusinessType))
                    {
                        model.UsedCarBusinessType = ProjectTask_Cust.UsedCarBusinessType.ToString();
                    }
                    //所属交易市场
                    if (!string.IsNullOrEmpty(ProjectTask_Cust.TradeMarketID))
                    {
                        model.TradeMarketID = ProjectTask_Cust.TradeMarketID.ToString();
                    }
                    model.CarType = ProjectTask_Cust.CarType;


                    string content = string.Format("修改客户【{0}(ID:{1})】：{2}。", model.CustName, model.CustID, BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetUpdateContent(model, "{0}由【{1}】修改为【{2}】", '，'));
                    BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("CustLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update, content);
                    if (BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.UpdateCustInfo(model) > 0)
                    {
                        //新增车商通会员ID

                        #region 添加,如果原custinfo表中存在则将原车商通会员ID提取出来进行比对---Modify=masj，Date=2012-04-13

                        //BitAuto.YanFa.Crm2009.Entities.CustInfo model1 = new Crm2009.Entities.CustInfo();
                        //model1.CustID = model.CustID;

                        //string cstMemberNewIDsStr = "";
                        //cstMemberNewIDsStr = ProjectTask_Cust.CstMemberID;
                        //string[] cstMembernewIDArray = null;
                        //if (!string.IsNullOrEmpty(cstMemberNewIDsStr))
                        //{
                        //    cstMemberNewIDsStr = cstMemberNewIDsStr.Replace("，", ",");
                        //    cstMemberNewIDsStr = cstMemberNewIDsStr.Trim(',');

                        //    if (cstMemberNewIDsStr.IndexOf(',') != -1)
                        //    {
                        //        cstMembernewIDArray = cstMemberNewIDsStr.Split(',');
                        //    }
                        //    else
                        //    {
                        //        cstMembernewIDArray = new string[1];
                        //        cstMembernewIDArray[0] = cstMemberNewIDsStr;
                        //    }
                        //}
                        ////根据custid去车商通会员ID表中查找出之前的车商通会员ID字符串
                        //Crm2009.Entities.QueryCstMember model_cstMember = new Crm2009.Entities.QueryCstMember();
                        //model_cstMember.CustID = model1.CustID;
                        //model_cstMember.Status = 0;
                        //string[] cstMemberOldIDArray = null;
                        //if (model_cstMember != null)
                        //{
                        //    DataTable dt_cstMember= Crm2009.BLL.CstMember.Instance.GetTable(model_cstMember);
                        //    cstMemberOldIDArray = new string[dt_cstMember.Rows.Count];
                        //    for (int k = 0; k < dt_cstMember.Rows.Count; k++)
                        //    {
                        //        cstMemberOldIDArray[k] = dt_cstMember.Rows[k]["CstMemberID"].ToString();
                        //    }
                        //    if (cstMemberOldIDArray != null)
                        //    {
                        //        Crm2009.BLL.CstMember.Instance.CompareAndDelete(cstMembernewIDArray, cstMemberOldIDArray, model1);
                        //    }
                        //}
                        #endregion

                        //如果是二手车
                        if (ProjectTask_Cust.CarType != 1)
                        {
                            int tradeMarketID = -1;
                            //更新所属交易市场
                            if (int.TryParse(model.TradeMarketID, out tradeMarketID))
                            {
                                BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.UpdateTradingMarket(model.CustID, tradeMarketID.ToString(), string.Empty);
                            }
                            int usedCarBusinessType = -1;
                            //更新二手车经营类型
                            if (int.TryParse(model.UsedCarBusinessType, out usedCarBusinessType))
                            {
                                BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.UpdateCustInfo_UsedCarBusinessType(model);
                            }
                            //插入二手车规模
                            IList<Entities.ProjectTask_BusinessScale> scaleList = BLL.ProjectTask_BusinessScale.Instance.GetAllProjectTask_BusinessScaleByTID(tId);
                            foreach (Entities.ProjectTask_BusinessScale scaleInfo in scaleList)
                            {
                                if (scaleInfo.OriginalRecID > 0)
                                {
                                    if (scaleInfo.Status == 0)
                                    {

                                        BitAuto.YanFa.Crm2009.Entities.BusinessScaleInfo scaleModel = BitAuto.YanFa.Crm2009.BLL.BusinessScaleInfo.Instance.GetScaleInfoByID((int)scaleInfo.OriginalRecID);
                                        if (scaleModel != null)
                                        {
                                            if (scaleInfo.MonthSales > 0)
                                            {
                                                scaleModel.MonthSales = (int)scaleInfo.MonthSales;
                                            }
                                            if (scaleInfo.MonthStock > 0)
                                            {
                                                scaleModel.MonthStock = (int)scaleInfo.MonthStock;
                                            }
                                            if (scaleInfo.MonthTrade > 0)
                                            {
                                                scaleModel.MonthTrade = (int)scaleInfo.MonthTrade;
                                            }
                                            BitAuto.YanFa.Crm2009.BLL.BusinessScaleInfo.Instance.UpdateBusinessScaleInfo(scaleModel);
                                        }
                                    }
                                    else
                                    {
                                        BitAuto.YanFa.Crm2009.BLL.BusinessScaleInfo.Instance.Delete((int)scaleInfo.OriginalRecID);
                                    }
                                }
                                else
                                {
                                    BitAuto.YanFa.Crm2009.Entities.BusinessScaleInfo scaleModel = new BitAuto.YanFa.Crm2009.Entities.BusinessScaleInfo();
                                    scaleModel.CustID = model.CustID;
                                    scaleModel.Status = 0;
                                    if (scaleInfo.MonthSales > 0)
                                    {
                                        scaleModel.MonthSales = (int)scaleInfo.MonthSales;
                                    }
                                    if (scaleInfo.MonthStock > 0)
                                    {
                                        scaleModel.MonthStock = (int)scaleInfo.MonthStock;
                                    }
                                    if (scaleInfo.MonthTrade > 0)
                                    {
                                        scaleModel.MonthTrade = (int)scaleInfo.MonthTrade;
                                    }
                                    //Crm2009.BLL.BusinessScaleInfo.Instance.UpdateBusinessScaleInfo(scaleModel);

                                    scaleModel.CreateTime = DateTime.Now;
                                    scaleModel.CreateUserID = BLL.Util.GetLoginUserID();
                                    BitAuto.YanFa.Crm2009.BLL.BusinessScaleInfo.Instance.InsertBusinessScaleInfo(scaleModel);
                                }
                            }
                        }


                        //更新成功后 更新主营品牌RequesthidBrand
                        BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.InsertCustBrand(model.CustID, BLL.Util.List2String(ProjectTask_Cust.BrandIDs, ",", "", ""));
                    }
                }
                else
                {
                    return model;
                }
            }
            else
            {
                model = new BitAuto.YanFa.Crm2009.Entities.CustInfo();
                model.CarType = 1;//默认为新车
                model.TypeID = ProjectTask_Cust.TypeID;
                model.CustName = ProjectTask_Cust.CustName;
                model.AbbrName = ProjectTask_Cust.AbbrName;
                model.IndustryID = ProjectTask_Cust.IndustryID;
                //model.LicenseNumber = ProjectTask_Cust.; 
                model.LevelID = ProjectTask_Cust.LevelID;
                model.CustPid = ProjectTask_Cust.CustPid;
                model.Pid = ProjectTask_Cust.Pid;
                // 集团 厂商 没有 所属集团 所属厂商
                if (model.TypeID == ((int)EnumCustomType.Company).ToString() || model.TypeID == ((int)EnumCustomType.Bloc).ToString())
                {
                    model.Pid = "";
                    model.CustPid = ProjectTask_Cust.CustPid;
                }
                else
                {
                    if (model.TypeID == ((int)EnumCustomType.SynthesizedShop).ToString())
                    {
                        //综合店 没有厂商
                        model.CustPid = "";
                    }
                    else
                    {
                        model.CustPid = ProjectTask_Cust.CustPid;
                    }
                    model.Pid = ProjectTask_Cust.Pid;
                }
                model.ShopLevel = ProjectTask_Cust.ShopLevel;
                model.ProvinceID = ProjectTask_Cust.ProvinceID;
                model.CityID = ProjectTask_Cust.CityID;
                model.CountyID = ProjectTask_Cust.CountyID;
                model.Address = ProjectTask_Cust.Address;
                model.contactName = ProjectTask_Cust.ContactName;
                model.zipcode = ProjectTask_Cust.Zipcode;
                model.Officetel = ProjectTask_Cust.OfficeTel;
                model.Fax = ProjectTask_Cust.Fax;
                model.Notes = ProjectTask_Cust.Notes;
                model.Status = 0;
                model.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                model.CreateUserID = ProjectTask_Cust.LastUpdateUserID;//Convert.ToInt32(HttpContext.Current.Session["userid"].ToString());
                model.CreateSource = 1;
                //二手车经营类型
                if (!string.IsNullOrEmpty(ProjectTask_Cust.UsedCarBusinessType))
                {
                    model.UsedCarBusinessType = ProjectTask_Cust.UsedCarBusinessType.ToString();
                }
                //所属交易市场
                if (!string.IsNullOrEmpty(ProjectTask_Cust.TradeMarketID))
                {
                    model.TradeMarketID = ProjectTask_Cust.TradeMarketID.ToString();
                }
                model.CarType = ProjectTask_Cust.CarType;

                model.CustID = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.InsertCustInfo(model);
                //添加所属4S字段 Modify---yangyh
                model.FoursPid = ProjectTask_Cust.FoursPid;

                if (model.CustID == "-1")
                {
                    return model;
                }
                else
                {
                    int tradeMarketID = -1;
                    if (model.CarType != 1)
                    {
                        //更新所属交易市场
                        if (int.TryParse(model.TradeMarketID, out tradeMarketID))
                        {
                            BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.UpdateTradingMarket(model.CustID, tradeMarketID.ToString(), string.Empty);
                        }
                        int usedCarBusinessType = -1;
                        if (int.TryParse(model.UsedCarBusinessType, out usedCarBusinessType))
                        {
                            BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.UpdateCustInfo_UsedCarBusinessType(model);
                        }
                        //插入二手车规模
                        IList<Entities.ProjectTask_BusinessScale> scaleList = BLL.ProjectTask_BusinessScale.Instance.GetAllProjectTask_BusinessScaleByTID(tId);
                        foreach (Entities.ProjectTask_BusinessScale scaleInfo in scaleList)
                        {
                            if (scaleInfo.OriginalRecID > 0)
                            {
                                if (scaleInfo.Status == 0)
                                {

                                    BitAuto.YanFa.Crm2009.Entities.BusinessScaleInfo scaleModel = BitAuto.YanFa.Crm2009.BLL.BusinessScaleInfo.Instance.GetScaleInfoByID((int)scaleInfo.OriginalRecID);
                                    if (scaleModel != null)
                                    {
                                        if (scaleInfo.MonthSales > 0)
                                        {
                                            scaleModel.MonthSales = (int)scaleInfo.MonthSales;
                                        }
                                        if (scaleInfo.MonthStock > 0)
                                        {
                                            scaleModel.MonthStock = (int)scaleInfo.MonthStock;
                                        }
                                        if (scaleInfo.MonthTrade > 0)
                                        {
                                            scaleModel.MonthTrade = (int)scaleInfo.MonthTrade;
                                        }
                                        BitAuto.YanFa.Crm2009.BLL.BusinessScaleInfo.Instance.UpdateBusinessScaleInfo(scaleModel);
                                    }
                                }
                                else
                                {
                                    BitAuto.YanFa.Crm2009.BLL.BusinessScaleInfo.Instance.Delete((int)scaleInfo.OriginalRecID);
                                }
                            }
                            else
                            {
                                BitAuto.YanFa.Crm2009.Entities.BusinessScaleInfo scaleModel = new BitAuto.YanFa.Crm2009.Entities.BusinessScaleInfo();
                                scaleModel.CustID = model.CustID;
                                scaleModel.Status = 0;
                                if (scaleInfo.MonthSales > 0)
                                {
                                    scaleModel.MonthSales = (int)scaleInfo.MonthSales;
                                }
                                if (scaleInfo.MonthStock > 0)
                                {
                                    scaleModel.MonthStock = (int)scaleInfo.MonthStock;
                                }
                                if (scaleInfo.MonthTrade > 0)
                                {
                                    scaleModel.MonthTrade = (int)scaleInfo.MonthTrade;
                                }
                                //Crm2009.BLL.BusinessScaleInfo.Instance.UpdateBusinessScaleInfo(scaleModel);

                                scaleModel.CreateTime = DateTime.Now;
                                scaleModel.CreateUserID = BLL.Util.GetLoginUserID();
                                BitAuto.YanFa.Crm2009.BLL.BusinessScaleInfo.Instance.InsertBusinessScaleInfo(scaleModel);
                            }
                        }
                    }

                    BLL.CrmCustInfo.Instance.InitCustDepartMapping(model.CustID);
                    ProjectTask_Cust.OriginalCustID = model.CustID;
                    Entities.ProjectTaskInfo ctModel = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(ProjectTask_Cust.PTID);
                    if (ctModel != null)
                    {
                        BLL.CallRecordInfo.Instance.UpdateCRMCustIDByID(ctModel.RelationID, model.CustID.ToString());
                    }

                    //主营品牌 添加成功 插入关系表 
                    BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.InsertCustBrand(model.CustID, BLL.Util.List2String(ProjectTask_Cust.BrandIDs, ",", "", ""));

                    //添加客户日志，需要CustID，应在数据库操作完成以后调用：
                    string content = string.Format("添加客户【{0}(ID:{1})】。", model.CustName, model.CustID);
                    BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("CustLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Add, content);
                }
            }


            return model;
        }

        /// <summary>
        /// 插入联系人
        /// </summary>
        /// <param name="ProjectTask_Cust"></param> 
        /// <returns></returns>
        private int InsertContactInfo(BitAuto.ISDC.CC2012.Entities.ProjectTask_Cust ProjectTask_Cust, string custID)
        {
            int o;
            QueryProjectTask_Cust_Contact queryProjectTask_Cust_Contact = new QueryProjectTask_Cust_Contact();
            queryProjectTask_Cust_Contact.PTID = ProjectTask_Cust.PTID;
            DataTable dt = BLL.ProjectTask_Cust_Contact.Instance.GetContactInfo(queryProjectTask_Cust_Contact, "", 1, 10000, out o);
            if (dt != null && dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //若真实姓名和移动电话，在CRM库中存在，则不会插入
                    if (BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.IsExistByCNameAndPhone(custID, dt.Rows[i]["CName"] + "", dt.Rows[i]["Phone"] + "") && (dt.Rows[i]["OriginalContactID"].ToString() == "-2" || dt.Rows[i]["OriginalContactID"].ToString() == ""))
                    {
                        continue;
                    }
                    ContactInfo model;
                    int OriginalContactID;
                    if (int.TryParse(dt.Rows[i]["OriginalContactID"] + "", out OriginalContactID) && OriginalContactID > 0)
                    {
                        model = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfoByUserID(OriginalContactID);
                    }
                    else
                    {
                        OriginalContactID = -1;
                        model = new ContactInfo();
                    }
                    //有更新，没有插入 先判断id
                    if (model != null)
                    {
                        int pid;
                        if (int.TryParse(dt.Rows[i]["pid"] + "", out pid))
                        {
                            model.PID = pid;
                        }
                        else
                        {
                            model.PID = -1;
                        }
                        model.CustID = custID;//dt.Rows[i]["CustID"] + "";
                        model.CName = dt.Rows[i]["CName"] + "";
                        model.EName = dt.Rows[i]["EName"] + "";

                        model.Sex = dt.Rows[i]["Sex"] + "";
                        model.DepartMent = dt.Rows[i]["DepartMent"] + "";

                        int OfficeTypeCode;
                        if (int.TryParse(dt.Rows[i]["OfficeTypeCode"] + "", out OfficeTypeCode))
                        {
                            model.OfficeTypeCode = OfficeTypeCode;
                        }
                        else
                        {
                            model.OfficeTypeCode = -1;
                        }

                        model.Title = dt.Rows[i]["Title"] + "";
                        model.OfficeTel = dt.Rows[i]["OfficeTel"] + "";
                        model.Phone = dt.Rows[i]["Phone"] + "";
                        model.Email = dt.Rows[i]["Email"] + "";
                        model.Fax = dt.Rows[i]["Fax"] + "";
                        model.Remark = dt.Rows[i]["Remarks"] + "";
                        model.CreateTime = DateTime.Now;
                        model.Status = 0;
                        model.Address = dt.Rows[i]["Address"] + "";
                        model.ZipCode = dt.Rows[i]["ZipCode"] + "";
                        model.MSN = dt.Rows[i]["MSN"] + "";
                        model.Birthday = dt.Rows[i]["Birthday"] + "";
                        model.Hobby = dt.Rows[i]["Hobby"] + "";
                        model.Responsible = dt.Rows[i]["Responsible"] + "";
                    }

                    //有更新，没有插入 先判断id
                    int ContactID = 0;
                    if (OriginalContactID > 0)
                    {
                        BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.UpdateContactInfo(model);
                        ContactID = OriginalContactID;
                        //add lxw 先删除MemberContactMapping之前车易通联系人关联表. 12.12.12
                        BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.DeleteByContactID(ContactID);
                    }
                    else
                    {
                        ContactID = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.InsertContactInfo(model);
                    }

                    //add lxw 插入MemberContactMapping车易通联系人关联表. 12.12.12 
                    DataTable dt_CC_MCM = null;
                    dt_CC_MCM = BLL.ProjectTask_MemberContactMapping.Instance.GetList("CC_MCM.ContactID=" + dt.Rows[i]["ID"].ToString());
                    if (dt_CC_MCM != null && dt_CC_MCM.Rows.Count > 0)
                    {
                        for (int p = 0; p < dt_CC_MCM.Rows.Count; p++)
                        {
                            MemberContactMapping model_MCM = new MemberContactMapping();
                            model_MCM.ContactID = ContactID;
                            model_MCM.IsMain = int.Parse(dt_CC_MCM.Rows[p]["IsMain"].ToString());
                            model_MCM.MemberID = new Guid(dt_CC_MCM.Rows[p]["MemberID"].ToString());
                            model_MCM.CreateTime = DateTime.Now;
                            BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.AddMemberContactMapping(model_MCM);
                        }
                    }

                }
            }
            return 1;
        }

        /// <summary>
        /// 插入会员相关信息
        /// </summary>
        /// <param name="ProjectTask_Cust"></param>
        /// <returns></returns>
        private int InsertDMSMember(BitAuto.ISDC.CC2012.Entities.ProjectTask_Cust ProjectTask_Cust, string custID, out int isChange)
        {
            //ProjectTask_DMSMember 到 DMSMember 和 DMSMember_MapCoordinate  一条一条处理，有更新，没有插入 同事同步客户
            isChange = 0;
            List<BitAuto.ISDC.CC2012.Entities.ProjectTask_DMSMember> ProjectTask_DMSMember = BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMemberByTID(ProjectTask_Cust.PTID);
            for (int i = 0; i < ProjectTask_DMSMember.Count; i++)
            {
                DMSMember member;
                DMSMember DMSMember = null;//CRM系统中，会员信息实体
                if (!string.IsNullOrEmpty(ProjectTask_DMSMember[i].OriginalDMSMemberID))
                {
                    member = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(new Guid(ProjectTask_DMSMember[i].OriginalDMSMemberID));
                    DMSMember = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(new Guid(ProjectTask_DMSMember[i].OriginalDMSMemberID));
                }
                else
                {
                    member = new DMSMember();
                }
                if (member != null)
                {
                    member.CustID = custID;//ProjectTask_Cust.OriginalCustID;
                    member.Name = ProjectTask_DMSMember[i].Name;
                    member.MemberType = ProjectTask_DMSMember[i].MemberType;
                    member.Abbr = ProjectTask_DMSMember[i].Abbr;
                    member.Phone = ProjectTask_DMSMember[i].Phone;
                    member.Fax = ProjectTask_DMSMember[i].Fax;
                    member.CompanyWebSite = ProjectTask_DMSMember[i].CompanyWebSite;
                    member.Email = ProjectTask_DMSMember[i].Email;
                    member.Postcode = ProjectTask_DMSMember[i].Postcode;
                    member.ProvinceID = ProjectTask_DMSMember[i].ProvinceID;
                    member.CityID = ProjectTask_DMSMember[i].CityID;
                    member.CountyID = ProjectTask_DMSMember[i].CountyID;
                    member.ContactAddress = ProjectTask_DMSMember[i].ContactAddress;
                    member.TrafficInfo = ProjectTask_DMSMember[i].TrafficInfo;
                    member.EnterpriseBrief = ProjectTask_DMSMember[i].EnterpriseBrief;
                    member.Remarks = ProjectTask_DMSMember[i].Remarks;
                    member.ModifyTime = DateTime.Now;
                    member.ModifyUserID = ProjectTask_Cust.LastUpdateUserID;//Crm2009.BLL.Util.GetUserID(); ;
                    //member.MapCoordinateList = ProjectTask_DMSMember[i].;地图怎么弄？
                    if (member.MapCoordinateList.Count > 0)
                    {
                        foreach (BitAuto.YanFa.Crm2009.Entities.DMSMapCoordinate map in member.MapCoordinateList)
                        {
                            if (map.MapProviderName.ToLower() == BitAuto.YanFa.Crm2009.Entities.Constants.Constant.MapProviderName.ToLower())
                            {
                                map.Latitude = ProjectTask_DMSMember[i].Lantitude;
                                map.Longitude = ProjectTask_DMSMember[i].Longitude;
                                break;
                            }
                        }
                    }
                    else
                    {
                        member.MapCoordinateList.Add(new DMSMapCoordinate(
                            member.ID, BitAuto.YanFa.Crm2009.Entities.Constants.Constant.MapProviderName,
                            ProjectTask_DMSMember[i].Longitude, ProjectTask_DMSMember[i].Lantitude)
                        );
                    }

                    member.BrandIds = ProjectTask_DMSMember[i].BrandIDs;
                    member.SerialIds = ProjectTask_DMSMember[i].SerialIds;
                    if (!string.IsNullOrEmpty(ProjectTask_DMSMember[i].OriginalDMSMemberID))
                    {
                        BitAuto.YanFa.Crm2009.Entities.DMSMember memberModel = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(new Guid(ProjectTask_DMSMember[i].OriginalDMSMemberID));
                        //modify by qizhiqiang2012-4-25为了插入已开通有排期车易通信息变更
                        if (memberModel != null)
                        {
                            //没有开通直接更新会员信息
                            if (string.IsNullOrEmpty(memberModel.MemberCode))
                            {
                                //memberModel.BrandIds = ProjectTask_DMSMember[i].BrandIDs;
                                try
                                {
                                    //生成更新日志
                                    string content = string.Format("为【{0}(ID:{1})】修改会员【{2}(ID:{3})】：{4}。",
                                        member.CustName, member.CustID, member.Name, member.MemberCode,
                                        BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetUpdateContent(member, "{0}由【{1}】修改为【{2}】", '，'));
                                    BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(BitAuto.YanFa.Crm2009.BLL.LogModule.DMSMember, (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update, content);
                                }
                                catch
                                { }
                                BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.Update(member);
                                int cid = BLL.ProjectTask_AuditContrastInfo.Instance.InsertByDMSMemberBrandChange(ProjectTask_DMSMember[i], DMSMember);
                                bool isDMSMemberChange = false;
                                int recid = BLL.ProjectTask_AuditContrastInfo.Instance.InsertByDMSMemberChange(ProjectTask_DMSMember[i], DMSMember, out isDMSMemberChange);
                                if (isDMSMemberChange || cid > 0)
                                {
                                    isChange = 1;
                                }
                                WebServices.DealerInfoService.Instance.UpdateMemberInfo(recid, ProjectTask_DMSMember[i], DMSMember);
                            }
                            else
                            {
                                //已开通易湃会员，并且易湃会员有排期，如果会员核实信息有更改生成已开通有排期车易通信息变更记录
                                if (BLL.ProjectTask_AuditContrastInfo.Instance.HavePeiQiByDMSMemberCode(memberModel.MemberCode))
                                {
                                    BLL.ProjectTask_AuditContrastInfo.Instance.InsertByOpenedDMSMemberChange(ProjectTask_DMSMember[i], DMSMember);
                                }
                                else
                                {
                                    int cid = BLL.ProjectTask_AuditContrastInfo.Instance.InsertByDMSMemberBrandChange(ProjectTask_DMSMember[i], DMSMember);
                                    bool isDMSMemberChange = false;
                                    int recid = BLL.ProjectTask_AuditContrastInfo.Instance.InsertByDMSMemberChange(ProjectTask_DMSMember[i], DMSMember, out isDMSMemberChange);
                                    if (isDMSMemberChange || cid > 0)
                                    {
                                        isChange = 1;
                                    }
                                    WebServices.DealerInfoService.Instance.UpdateMemberInfo(recid, ProjectTask_DMSMember[i], DMSMember);
                                }
                            }
                        }
                        //

                    }
                    else
                    {
                        member.CreateSource = 1;
                        BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.Insert(member);

                        //申请开通
                        member.SyncStatus = (int)EnumDMSSyncStatus.ApplyFor;
                        member.ApplyUserID = ProjectTask_Cust.LastUpdateUserID;
                        member.ApplyTime = DateTime.Now;
                        BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.Update(member);
                        BLL.ProjectTask_DMSMember.Instance.UpdateOriginalDMSMemberIDByID(ProjectTask_DMSMember[i].MemberID, member.ID.ToString());
                        BLL.CallRecordInfo.Instance.UpdateOriginalDMSMemberIDByID(ProjectTask_DMSMember[i].MemberID, member.ID.ToString());
                        //插入日志
                        BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.InsertSyncLog(member.ID, member.SyncStatus, "申请成功(来自呼叫中心)", member.ApplyUserID.Value, member.ApplyTime.Value);
                        //添加会员日志，需要member.ID，应在数据库操作完成以后调用：
                        string content = string.Format("添加DMSMember会员【{0}(ID:{1})】。", member.Name, member.ID.ToString());
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(ConfigurationUtil.GetAppSettingValue("MemberLogModuleID"), (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Add, content);
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// 插入车商通会员信息
        /// </summary>
        /// <param name="ProjectTask_Cust"></param>
        /// <param name="custID"></param>
        private void InsertCstMember(BitAuto.ISDC.CC2012.Entities.ProjectTask_Cust ProjectTask_Cust, string custID)
        {
            List<Entities.ProjectTask_CSTMember> ProjectTask_CSTMember = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(ProjectTask_Cust.PTID);
            for (int i = 0; i < ProjectTask_CSTMember.Count; i++)
            {
                //如果是新增的车商通会员，则插入crm库
                if (string.IsNullOrEmpty(ProjectTask_CSTMember[i].OriginalCSTRecID))
                {
                    CstMember member = new CstMember();
                    member.Address = ProjectTask_CSTMember[i].Address;
                    member.CityID = ProjectTask_CSTMember[i].CityID;
                    member.CountyID = ProjectTask_CSTMember[i].CountyID;
                    member.FullName = ProjectTask_CSTMember[i].FullName;
                    member.PostCode = ProjectTask_CSTMember[i].PostCode;
                    member.ShortName = ProjectTask_CSTMember[i].ShortName;
                    member.SuperId = (int)ProjectTask_CSTMember[i].SuperId;
                    member.TrafficInfo = ProjectTask_CSTMember[i].TrafficInfo;
                    member.VendorClass = (int)ProjectTask_CSTMember[i].VendorClass;
                    //member.VendorCode = ProjectTask_CSTMember[i].VendorCode;
                    member.ProvinceID = ProjectTask_CSTMember[i].ProvinceID;
                    member.CustID = custID;
                    member.Status = 0;
                    member.ApplyTime = DateTime.Now;
                    member.SyncStatus = (int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.ApplyFor;
                    member.CreateTime = DateTime.Now;
                    member.CreateUserID = BLL.Util.GetLoginUserID();
                    member.CreateSource = 1;
                    string cstRecId = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.InsertCstMember(member);

                    //插入开通日志
                    BitAuto.YanFa.Crm2009.BLL.CSTMemberSyncLog.Instance.AddSynclog(cstRecId, member.SyncStatus, "申请成功(来自呼叫中心)", member.CreateUserID, member.CreateTime);

                    Entities.ProjectTask_CSTMember ccCstLinkMan = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(ProjectTask_CSTMember[i].ID);
                    ccCstLinkMan.OriginalCSTRecID = cstRecId;
                    //回写crm车商通会员ID
                    BLL.ProjectTask_CSTMember.Instance.Update(ccCstLinkMan);

                    //插入公司联系人
                    Entities.ProjectTask_CSTLinkMan linkManInfo = BLL.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkManModel(ProjectTask_CSTMember[i].ID);
                    if (linkManInfo != null)
                    {
                        BitAuto.YanFa.Crm2009.Entities.CSTLinkMan cstLinkManInfo = new CSTLinkMan();
                        cstLinkManInfo.CreateTime = DateTime.Now;
                        cstLinkManInfo.CSTRecID = cstRecId;
                        cstLinkManInfo.Department = linkManInfo.Department;
                        cstLinkManInfo.Email = linkManInfo.Email;
                        cstLinkManInfo.Mobile = linkManInfo.Mobile;
                        cstLinkManInfo.Name = linkManInfo.Name;
                        cstLinkManInfo.Position = linkManInfo.Position;
                        int linkManCstRecID = BitAuto.YanFa.Crm2009.BLL.CSTLinkMan.Instance.Add(cstLinkManInfo);

                        //回写crm联系人ID
                        linkManInfo.CSTRecID = cstRecId;
                        linkManInfo.OriginalCSTLinkManID = linkManCstRecID;
                        BLL.ProjectTask_CSTLinkMan.Instance.Update(linkManInfo);
                    }

                    //插入主营品牌
                    if (ProjectTask_Cust.TypeID == ((int)EnumCustomType.FourS).ToString())
                    {
                        List<Entities.ProjectTask_CSTMember_Brand> ccBrandInfos = BLL.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_Brand(ProjectTask_CSTMember[i].ID);

                        foreach (Entities.ProjectTask_CSTMember_Brand info in ccBrandInfos)
                        {
                            BitAuto.YanFa.Crm2009.Entities.CSTMember_Brand cstMemberBrand = new CSTMember_Brand();
                            cstMemberBrand.CreateTime = DateTime.Now;
                            cstMemberBrand.CSTRecID = cstRecId;
                            cstMemberBrand.BrandID = info.BrandID;

                            BitAuto.YanFa.Crm2009.BLL.CSTMember_Brand.Instance.Add(cstMemberBrand);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 插入车商通会员信息
        /// </summary>
        /// <param name="ProjectTask_Cust"></param>
        /// <param name="custID"></param>
        /// <returns></returns>
        //private int InsertCstMember(ProjectTask_Cust ProjectTask_Cust, string custID)
        //{
        //    List<Entities.ProjectTask_CSTMember> ProjectTask_CSTMember = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(ProjectTask_Cust.TID);
        //    for (int i = 0; i < ProjectTask_CSTMember.Count; i++)
        //    {
        //        CstMember member=null;
        //        //CRM系统中，会员信息实体
        //        if (!string.IsNullOrEmpty(ProjectTask_CSTMember[i].OriginalCSTRecID))
        //        {
        //            #region 在crm存在的会员信息
        //            member = Crm2009.BLL.CstMember.Instance.GetCstMemberModel(ProjectTask_CSTMember[i].OriginalCSTRecID);
        //            //如果是开通会员
        //            if (BLL.ProjectTask_CSTMember.Instance.IsOpenSuccessMember(ProjectTask_CSTMember[i].ID))
        //            {
        //                #region 记录变更信息
        //                Entities.ProjectTask_AuditContrastInfo auditInfo = new Entities.ProjectTask_AuditContrastInfo();
        //                auditInfo.CreateTime = DateTime.Now;
        //                auditInfo.CreateUserID = BLL.Util.GetLoginUserID();
        //                auditInfo.CustID = member.CustID;
        //                auditInfo.DisposeStatus = 0;
        //                auditInfo.DisposeTime = null;
        //                auditInfo.DMSMemberID = member.CSTRecID;
        //                auditInfo.ExportStatus = 0;//0-未导出，1-已导出
        //                auditInfo.TID = ProjectTask_CSTMember[i].TID;
        //                int changeCount = 0;

        //                #region 三个字段
        //                if (member.Address != ProjectTask_CSTMember[i].Address && (!string.IsNullOrEmpty(member.Address)||!string.IsNullOrEmpty(ProjectTask_CSTMember[i].Address)))
        //                {
        //                    auditInfo.ContrastInfo = "车商通地址由：" + member.Address +
        //                        "，改为：" + ProjectTask_CSTMember[i].Address;
        //                    auditInfo.ContrastInfoInside = "Address:('" + BLL.Util.EscapeString(member.Address) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].Address) + "')";
        //                    member.Address = ProjectTask_CSTMember[i].Address;
        //                    changeCount++;
        //                }
        //                if (member.PostCode != ProjectTask_CSTMember[i].PostCode)
        //                {
        //                    if (changeCount > 0)
        //                    {
        //                        auditInfo.ContrastInfo += ",";
        //                        auditInfo.ContrastInfoInside += ",";
        //                    }
        //                    auditInfo.ContrastInfo += "车商通邮编由：" + member.PostCode +
        //                        "，改为：" + ProjectTask_CSTMember[i].PostCode;
        //                    auditInfo.ContrastInfoInside += "PostCode:('" + BLL.Util.EscapeString(member.PostCode) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].PostCode) + "')";
        //                    member.PostCode = ProjectTask_CSTMember[i].PostCode;
        //                    changeCount++;
        //                }
        //                if (member.TrafficInfo != ProjectTask_CSTMember[i].TrafficInfo && (!string.IsNullOrEmpty(member.TrafficInfo) || !string.IsNullOrEmpty(ProjectTask_CSTMember[i].TrafficInfo)))
        //                {
        //                    if (changeCount > 0)
        //                    {
        //                        auditInfo.ContrastInfo += ",";
        //                        auditInfo.ContrastInfoInside += ",";
        //                    }
        //                    auditInfo.ContrastInfo += "车商通交通信息由：" + member.TrafficInfo +
        //                        "，改为：" + ProjectTask_CSTMember[i].TrafficInfo;
        //                    auditInfo.ContrastInfoInside += "TrafficInfo:('" + BLL.Util.EscapeString(member.TrafficInfo) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].TrafficInfo) + "')";
        //                    member.TrafficInfo = ProjectTask_CSTMember[i].TrafficInfo;
        //                    changeCount++;
        //                }
        //                auditInfo.ContrastType = 8;
        //                auditInfo.DisposeStatus = 0;
        //                auditInfo.DisposeTime = DateTime.Now;


        //                //是否有改变
        //                if (changeCount > 0)
        //                {
        //                    //插入变更记录
        //                    int auditRecID = BLL.ProjectTask_AuditContrastInfo.Instance.Insert(auditInfo);
        //                    try
        //                    {
        //                        DataSet ds = Crm2009.BLL.CSTMember_Brand.Instance.GetList("CSTRecID='" + member.CSTRecID + "'");
        //                        int[] brandIds = new int[ds.Tables[0].Rows.Count];
        //                        int j = 0;
        //                        foreach (DataRow dr in ds.Tables[0].Rows)
        //                        {
        //                            brandIds[j] = int.Parse(dr["BrandID"].ToString());
        //                            j++;
        //                        }
        //                        string msg = string.Empty;
        //                        //更新车商通信息
        //                        BitAuto.YanFa.DMSInterface.CstMemberServiceHandler.UpdateTranstarUserInfo(member.CstMemberID, member.FullName, member.ShortName, member.VendorClass, member.SuperId, member.ProvinceID, member.CityID, out msg);
        //                        //更新crm车商通信息
        //                        Crm2009.BLL.CstMember.Instance.UpdateCstMember(member);

        //                        Entities.ProjectTask_AuditContrastInfo auditModel = BLL.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(auditRecID);
        //                        auditModel.DisposeStatus = 1;
        //                        auditModel.DisposeTime = DateTime.Now;
        //                        BLL.ProjectTask_AuditContrastInfo.Instance.Update(auditModel);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Entities.ProjectTask_AuditContrastInfo auditModel = BLL.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(auditRecID);
        //                        auditModel.Remark = ex.ToString();
        //                        BLL.ProjectTask_AuditContrastInfo.Instance.Update(auditModel);
        //                    }
        //                }
        //                #endregion

        //                changeCount = 0;
        //                auditInfo.ContrastInfo = string.Empty;
        //                auditInfo.ContrastInfoInside = string.Empty;

        //                #region 四个字段
        //                if (member.FullName != ProjectTask_CSTMember[i].FullName)
        //                {
        //                    auditInfo.ContrastInfo = "车商通会员全称由：" + member.FullName +
        //                        "，改为：" + ProjectTask_CSTMember[i].FullName;
        //                    auditInfo.ContrastInfoInside = "FullName:('" + BLL.Util.EscapeString(member.FullName) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].FullName) + "')";
        //                    changeCount++;
        //                }
        //                if (member.ShortName!= ProjectTask_CSTMember[i].ShortName)
        //                {
        //                    if (changeCount > 0)
        //                    {
        //                        auditInfo.ContrastInfo += ",";
        //                        auditInfo.ContrastInfoInside += ",";
        //                    }
        //                    auditInfo.ContrastInfo += "车商通会员简称由：" + member.ShortName +
        //                        "，改为：" + ProjectTask_CSTMember[i].ShortName;
        //                    auditInfo.ContrastInfoInside += "ShortName:('" + BLL.Util.EscapeString(member.ShortName) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].ShortName) + "')";
        //                    changeCount++;
        //                }
        //                if (member.VendorClass != ProjectTask_CSTMember[i].VendorClass)
        //                {
        //                    if (changeCount > 0)
        //                    {
        //                        auditInfo.ContrastInfo += ",";
        //                        auditInfo.ContrastInfoInside += ",";
        //                    }
        //                    auditInfo.ContrastInfo += "车商通会员类型由：" + BLL.Util.GetCstMemberVendorClass(member.VendorClass) +
        //                        "，改为：" + BLL.Util.GetCstMemberVendorClass((int)ProjectTask_CSTMember[i].VendorClass);
        //                    auditInfo.ContrastInfoInside += "VendorClass:('" + BLL.Util.EscapeString(member.VendorClass.ToString()) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].VendorClass.ToString()) + "')";
        //                    changeCount++;
        //                }
        //                if (member.ProvinceID != ProjectTask_CSTMember[i].ProvinceID || member.CountyID != ProjectTask_CSTMember[i].CountyID || member.CityID != ProjectTask_CSTMember[i].CityID)
        //                {
        //                    if (changeCount > 0)
        //                    {
        //                        auditInfo.ContrastInfo += ",";
        //                        auditInfo.ContrastInfoInside += ",";
        //                    }
        //                    auditInfo.ContrastInfo += "车商通地区 省由：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(member.ProvinceID) +
        //                        "，改为：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(ProjectTask_CSTMember[i].ProvinceID) + ",市由：" +Crm2009.BLL.AreaInfo.Instance.GetAreaName(member.CityID) +
        //                        "，改为：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(ProjectTask_CSTMember[i].CityID) + ",区由：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(member.CountyID) +
        //                        "，改为：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(ProjectTask_CSTMember[i].CountyID);
        //                    auditInfo.ContrastInfoInside += "ProvinceID:('" + BLL.Util.EscapeString(member.ProvinceID) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].ProvinceID) + "')," +
        //                        "CityID:('" + BLL.Util.EscapeString(member.CityID) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].CityID) + "')," +
        //                        "CountyID:('" + BLL.Util.EscapeString(member.CountyID) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].CountyID) + "')";
        //                    changeCount++;
        //                }
        //                if (ProjectTask_Cust.TypeID == ((int)EnumCustomType.FourS).ToString())
        //                {
        //                    DataSet ds = Crm2009.BLL.CSTMember_Brand.Instance.GetList(" CSTRecID='" + member.CSTRecID+"'");
        //                    int num = 0;
        //                    string crmBrandIDs = string.Empty;
        //                    foreach (DataRow dr in ds.Tables[0].Rows)
        //                    {
        //                        crmBrandIDs += dr["BrandID"].ToString();
        //                        if (num < ds.Tables[0].Rows.Count-1)
        //                        {
        //                            crmBrandIDs += ",";
        //                        }
        //                        num++;
        //                    }
        //                    string ccBrandIDs = BLL.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_BrandIDs(ProjectTask_CSTMember[i].ID);
        //                    if (crmBrandIDs.Trim() != ccBrandIDs.Trim())
        //                    {
        //                        if (changeCount > 0)
        //                        {
        //                            auditInfo.ContrastInfo += ",";
        //                            auditInfo.ContrastInfoInside += ",";
        //                        }
        //                        auditInfo.ContrastInfo += "车商通主营品牌由：" + Crm2009.BLL.CarBrand.Instance.GetBrandNames(crmBrandIDs) +
        //                            "，改为：" + Crm2009.BLL.CarBrand.Instance.GetBrandNames(ccBrandIDs);
        //                        auditInfo.ContrastInfoInside += "BrandIDS:('" + BLL.Util.EscapeString(crmBrandIDs) + "','" + BLL.Util.EscapeString(ccBrandIDs) + "')";
        //                        changeCount++;
        //                    }
        //                }
        //                auditInfo.ContrastType = 7;
        //                auditInfo.DisposeStatus = 0;
        //                auditInfo.DisposeTime = null;
        //                BLL.ProjectTask_AuditContrastInfo.Instance.Insert(auditInfo);
        //                #endregion

        //                #endregion

        //            }
        //            else //如果是没有开通的会员，并且在Crm已经存在
        //            {
        //                Entities.ProjectTask_AuditContrastInfo auditInfo = new Entities.ProjectTask_AuditContrastInfo();
        //                auditInfo.CreateTime = DateTime.Now;
        //                auditInfo.CreateUserID = BLL.Util.GetLoginUserID();
        //                auditInfo.CustID = member.CustID;
        //                auditInfo.DisposeTime = DateTime.Now;
        //                auditInfo.DMSMemberID = member.CSTRecID;
        //                auditInfo.ExportStatus = 0;//0-未导出，1-已导出
        //                auditInfo.TID = ProjectTask_CSTMember[i].TID;
        //                auditInfo.DisposeStatus = 1;
        //                int changeCount = 0;

        //                #region 三个字段
        //                if (member.Address != ProjectTask_CSTMember[i].Address&&(!string.IsNullOrEmpty(member.Address) || !string.IsNullOrEmpty(ProjectTask_CSTMember[i].Address)))
        //                {
        //                    auditInfo.ContrastInfo = "车商通地址由：" + member.Address +
        //                        "，改为：" + ProjectTask_CSTMember[i].Address;
        //                    auditInfo.ContrastInfoInside += "Address:('" + BLL.Util.EscapeString(member.Address) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].Address) + "')";
        //                    member.Address = ProjectTask_CSTMember[i].Address;
        //                    changeCount++;
        //                }
        //                if (member.PostCode != ProjectTask_CSTMember[i].PostCode)
        //                {
        //                    if (changeCount > 0)
        //                    {
        //                        auditInfo.ContrastInfo += ",";
        //                        auditInfo.ContrastInfoInside += ",";
        //                    }
        //                    auditInfo.ContrastInfo += "车商通邮编由：" + member.PostCode +
        //                        "，改为：" + ProjectTask_CSTMember[i].PostCode;
        //                    auditInfo.ContrastInfoInside += "PostCode:('" + BLL.Util.EscapeString(member.PostCode) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].PostCode) + "')";
        //                    member.PostCode = ProjectTask_CSTMember[i].PostCode;
        //                    changeCount++;
        //                }
        //                if (member.TrafficInfo != ProjectTask_CSTMember[i].TrafficInfo && (!string.IsNullOrEmpty(member.TrafficInfo) || !string.IsNullOrEmpty(ProjectTask_CSTMember[i].TrafficInfo)))
        //                {
        //                    if (changeCount > 0)
        //                    {
        //                        auditInfo.ContrastInfo += ",";
        //                        auditInfo.ContrastInfoInside += ",";
        //                    }
        //                    auditInfo.ContrastInfo += "车商通交通信息由：" + member.TrafficInfo +
        //                        "，改为：" + ProjectTask_CSTMember[i].TrafficInfo;
        //                    auditInfo.ContrastInfoInside += "TrafficInfo:('" + BLL.Util.EscapeString(member.TrafficInfo) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].TrafficInfo) + "')";
        //                    member.TrafficInfo = ProjectTask_CSTMember[i].TrafficInfo;
        //                    changeCount++;
        //                }
        //                auditInfo.ContrastType = 8;


        //                //是否有改变
        //                if (changeCount > 0)
        //                {
        //                    //插入变更记录
        //                    BLL.ProjectTask_AuditContrastInfo.Instance.Insert(auditInfo);
        //                }
        //                #endregion

        //                auditInfo.ContrastInfo = string.Empty;
        //                auditInfo.ContrastInfoInside = string.Empty;
        //                changeCount = 0;

        //                #region 四个字段
        //                if (member.FullName != ProjectTask_CSTMember[i].FullName)
        //                {
        //                    auditInfo.ContrastInfo = "车商通会员全称由：" + member.FullName +
        //                        "，改为：" + ProjectTask_CSTMember[i].FullName;
        //                    auditInfo.ContrastInfoInside = "FullName:('" + BLL.Util.EscapeString(member.FullName) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].FullName) + "')";
        //                    changeCount++;
        //                }
        //                if (member.ShortName != ProjectTask_CSTMember[i].ShortName)
        //                {
        //                    if (changeCount > 0)
        //                    {
        //                        auditInfo.ContrastInfo += ",";
        //                        auditInfo.ContrastInfoInside += ",";
        //                    }
        //                    auditInfo.ContrastInfo += "车商通会员简称由：" + member.ShortName +
        //                        "，改为：" + ProjectTask_CSTMember[i].ShortName;
        //                    auditInfo.ContrastInfoInside += "ShortName:('" + BLL.Util.EscapeString(member.ShortName) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].ShortName) + "')";
        //                    changeCount++;
        //                }
        //                if (member.VendorClass != ProjectTask_CSTMember[i].VendorClass)
        //                {
        //                    if (changeCount > 0)
        //                    {
        //                        auditInfo.ContrastInfo += ",";
        //                        auditInfo.ContrastInfoInside += ",";
        //                    }
        //                    auditInfo.ContrastInfo += "车商通会员类型由：" + BLL.Util.GetCstMemberVendorClass(member.VendorClass) +
        //                        "，改为：" + BLL.Util.GetCstMemberVendorClass((int)ProjectTask_CSTMember[i].VendorClass);
        //                    auditInfo.ContrastInfoInside += "VendorClass:('" + BLL.Util.EscapeString(member.VendorClass.ToString()) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].VendorClass.ToString()) + "')";
        //                    changeCount++;
        //                }
        //                if (member.ProvinceID != ProjectTask_CSTMember[i].ProvinceID || member.CountyID != ProjectTask_CSTMember[i].CountyID || member.CityID != ProjectTask_CSTMember[i].CityID)
        //                {
        //                    if (changeCount > 0)
        //                    {
        //                        auditInfo.ContrastInfo += ",";
        //                        auditInfo.ContrastInfoInside += ",";
        //                    }
        //                    auditInfo.ContrastInfo += "车商通地区 省由：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(member.ProvinceID) +
        //                        "，改为：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(ProjectTask_CSTMember[i].ProvinceID) + ",市由：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(member.CityID) +
        //                        "，改为：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(ProjectTask_CSTMember[i].CityID) + ",区由：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(member.CountyID) +
        //                        "，改为：" + Crm2009.BLL.AreaInfo.Instance.GetAreaName(ProjectTask_CSTMember[i].CountyID);
        //                    auditInfo.ContrastInfoInside += "ProvinceID:('" + BLL.Util.EscapeString(member.ProvinceID) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].ProvinceID) + "')," +
        //                        "CityID:('" + BLL.Util.EscapeString(member.CityID) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].CityID) + "')," +
        //                        "CountyID:('" + BLL.Util.EscapeString(member.CountyID) + "','" + BLL.Util.EscapeString(ProjectTask_CSTMember[i].CountyID) + "')";
        //                    changeCount++;
        //                }
        //                if (ProjectTask_Cust.TypeID == ((int)EnumCustomType.FourS).ToString())
        //                {
        //                    DataSet ds = Crm2009.BLL.CSTMember_Brand.Instance.GetList(" CSTRecID='" + member.CSTRecID+"'");
        //                    int num = 0;
        //                    string crmBrandIDs = string.Empty;
        //                    foreach (DataRow dr in ds.Tables[0].Rows)
        //                    {
        //                        crmBrandIDs += dr["BrandID"].ToString();
        //                        if (num < ds.Tables[0].Rows.Count-1)
        //                        {
        //                            crmBrandIDs += ",";
        //                        }
        //                        num++;
        //                    }
        //                    string ccBrandIDs = BLL.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_BrandIDs(ProjectTask_CSTMember[i].ID);
        //                    if (crmBrandIDs != ccBrandIDs)
        //                    {
        //                        if (changeCount > 0)
        //                        {
        //                            auditInfo.ContrastInfo += ",";
        //                            auditInfo.ContrastInfoInside += ",";
        //                        }
        //                        auditInfo.ContrastInfo += "车商通主营品牌由：" + Crm2009.BLL.CarBrand.Instance.GetBrandNames(crmBrandIDs) +
        //                            "，改为：" + Crm2009.BLL.CarBrand.Instance.GetBrandNames(ccBrandIDs);
        //                        auditInfo.ContrastInfoInside += "BrandIDS:('" + BLL.Util.EscapeString(crmBrandIDs) + "','" + BLL.Util.EscapeString(ccBrandIDs) + "')";
        //                        changeCount++;

        //                        //更新主营品牌
        //                        Crm2009.BLL.CSTMember_Brand.Instance.DeleteByCSTRecID(member.CSTRecID);
        //                        foreach (string brandId in ccBrandIDs.Split(','))
        //                        {
        //                            Crm2009.Entities.CSTMember_Brand cstBrandModel = new CSTMember_Brand();
        //                            cstBrandModel.BrandID = int.Parse(brandId);
        //                            cstBrandModel.CreateTime = DateTime.Now;
        //                            cstBrandModel.CSTRecID = member.CSTRecID;
        //                            Crm2009.BLL.CSTMember_Brand.Instance.Add(cstBrandModel);
        //                        }
        //                    }
        //                }
        //                auditInfo.ContrastType = 7;
        //                if (changeCount > 0)
        //                {
        //                    BLL.ProjectTask_AuditContrastInfo.Instance.Insert(auditInfo);
        //                }
        //                #endregion

        //                member.Address = ProjectTask_CSTMember[i].Address;
        //                member.ProvinceID = ProjectTask_CSTMember[i].ProvinceID;
        //                member.CityID = ProjectTask_CSTMember[i].CityID;
        //                member.CountyID = ProjectTask_CSTMember[i].CountyID;
        //                member.FullName = ProjectTask_CSTMember[i].FullName;
        //                member.PostCode = ProjectTask_CSTMember[i].PostCode;
        //                member.ShortName = ProjectTask_CSTMember[i].ShortName;
        //                member.SuperId = (int)ProjectTask_CSTMember[i].SuperId;
        //                member.TrafficInfo = ProjectTask_CSTMember[i].TrafficInfo;
        //                member.VendorClass = (int)ProjectTask_CSTMember[i].VendorClass;
        //                member.VendorCode = ProjectTask_CSTMember[i].VendorCode;
        //                //更新会员信息
        //                Crm2009.BLL.CstMember.Instance.UpdateCstMember(member);

        //                //更新联系人信息
        //                Entities.ProjectTask_CSTLinkMan ccCstLinkManInfo = BLL.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkManModel(ProjectTask_CSTMember[i].ID);
        //                if (ccCstLinkManInfo != null)
        //                {
        //                    Crm2009.Entities.CSTLinkMan cstLinkManInfo = Crm2009.BLL.CSTLinkMan.Instance.GetModel((int)ccCstLinkManInfo.OriginalCSTLinkManID);
        //                    if (cstLinkManInfo != null)
        //                    {
        //                        cstLinkManInfo.Department = ccCstLinkManInfo.Department;
        //                        cstLinkManInfo.Email = ccCstLinkManInfo.Email;
        //                        cstLinkManInfo.Mobile = ccCstLinkManInfo.Email;
        //                        cstLinkManInfo.Name = ccCstLinkManInfo.Name;
        //                        cstLinkManInfo.Email = ccCstLinkManInfo.Email;
        //                        cstLinkManInfo.Position = ccCstLinkManInfo.Position;

        //                        Crm2009.BLL.CSTLinkMan.Instance.Update(cstLinkManInfo);
        //                    }
        //                    else
        //                    {
        //                         cstLinkManInfo=new Crm2009.Entities.CSTLinkMan();
        //                         cstLinkManInfo.Department = ccCstLinkManInfo.Department;
        //                         cstLinkManInfo.Email = ccCstLinkManInfo.Email;
        //                         cstLinkManInfo.Mobile = ccCstLinkManInfo.Email;
        //                         cstLinkManInfo.Name = ccCstLinkManInfo.Name;
        //                         cstLinkManInfo.Email = ccCstLinkManInfo.Email;
        //                         cstLinkManInfo.CSTRecID = member.CSTRecID;
        //                         cstLinkManInfo.CreateTime = DateTime.Now;
        //                         cstLinkManInfo.Position = ccCstLinkManInfo.Position;

        //                         ccCstLinkManInfo.OriginalCSTLinkManID = Crm2009.BLL.CSTLinkMan.Instance.Add(cstLinkManInfo);
        //                         ccCstLinkManInfo.CSTMemberID = ProjectTask_CSTMember[i].ID;
        //                         ccCstLinkManInfo.CreateUserID = BLL.Util.GetLoginUserID();
        //                         BLL.ProjectTask_CSTLinkMan.Instance.Update(ccCstLinkManInfo);
        //                    }
        //                }
        //                //如果是4s客户更新主营品牌
        //                if (ProjectTask_Cust.TypeID == ((int)EnumCustomType.FourS).ToString())
        //                {
        //                    List<Entities.ProjectTask_CSTMember_Brand> brandInfoList = BLL.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_Brand(ProjectTask_CSTMember[i].ID);
        //                    if (brandInfoList.Count > 0)
        //                    {
        //                        Crm2009.BLL.CSTMember_Brand.Instance.DeleteByCSTRecID(ProjectTask_CSTMember[i].OriginalCSTRecID);
        //                        foreach (Entities.ProjectTask_CSTMember_Brand brandInfo in brandInfoList)
        //                        {
        //                            Crm2009.Entities.CSTMember_Brand brandModel = new CSTMember_Brand();
        //                            brandModel.BrandID = brandInfo.BrandID;
        //                            brandModel.CreateTime = DateTime.Now;
        //                            brandModel.CSTRecID = ProjectTask_CSTMember[i].OriginalCSTRecID;
        //                            Crm2009.BLL.CSTMember_Brand.Instance.Add(brandModel);
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            #region 如果不是crm中的会员
        //            //插入会员信息
        //            member = new CstMember();
        //            member.Address = ProjectTask_CSTMember[i].Address;
        //            member.CityID = ProjectTask_CSTMember[i].CityID;
        //            member.CountyID = ProjectTask_CSTMember[i].CountyID;
        //            member.FullName = ProjectTask_CSTMember[i].FullName;
        //            member.PostCode = ProjectTask_CSTMember[i].PostCode;
        //            member.ShortName = ProjectTask_CSTMember[i].ShortName;
        //            member.SuperId = (int)ProjectTask_CSTMember[i].SuperId;
        //            member.TrafficInfo = ProjectTask_CSTMember[i].TrafficInfo;
        //            member.VendorClass = (int)ProjectTask_CSTMember[i].VendorClass;
        //            member.VendorCode = ProjectTask_CSTMember[i].VendorCode;
        //            member.ProvinceID = ProjectTask_CSTMember[i].ProvinceID;
        //            member.CustID = custID;
        //            member.Status = 1;
        //            member.ApplyTime = DateTime.Now;
        //            member.SyncStatus = (int)Crm2009.Entities.EnumCSTSyncStatus.ApplyFor;
        //            member.CreateTime = DateTime.Now;
        //            member.CreateUserID = BLL.Util.GetLoginUserID();
        //            member.CreateSource = 1;
        //            string cstRecId = Crm2009.BLL.CstMember.Instance.InsertCstMember(member);
        //            //插入开通日志
        //            Crm2009.BLL.CSTMemberSyncLog.Instance.AddSynclog(cstRecId, member.SyncStatus, "申请成功(来自呼叫中心)", member.CreateUserID, member.CreateTime);

        //            Entities.ProjectTask_CSTMember ccCstLinkMan = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(ProjectTask_CSTMember[i].ID);
        //            ccCstLinkMan.OriginalCSTRecID = cstRecId;
        //            //回写crm车商通会员ID
        //            BLL.ProjectTask_CSTMember.Instance.Update(ccCstLinkMan);

        //            //插入公司联系人
        //            Entities.ProjectTask_CSTLinkMan linkManInfo = BLL.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkManModel(ProjectTask_CSTMember[i].ID);
        //            if (linkManInfo != null)
        //            {
        //                Crm2009.Entities.CSTLinkMan cstLinkManInfo = new CSTLinkMan();
        //                cstLinkManInfo.CreateTime = DateTime.Now;
        //                cstLinkManInfo.CSTRecID = cstRecId;
        //                cstLinkManInfo.Department = linkManInfo.Department;
        //                cstLinkManInfo.Email = linkManInfo.Email;
        //                cstLinkManInfo.Mobile = linkManInfo.Mobile;
        //                cstLinkManInfo.Name = linkManInfo.Name;
        //                cstLinkManInfo.Position = linkManInfo.Position;
        //                int  linkManCstRecID = Crm2009.BLL.CSTLinkMan.Instance.Add(cstLinkManInfo);

        //                //回写crm联系人ID
        //                linkManInfo.CSTRecID = cstRecId;
        //                linkManInfo.OriginalCSTLinkManID = linkManCstRecID;
        //                BLL.ProjectTask_CSTLinkMan.Instance.Update(linkManInfo);
        //            }
        //            //插入主营品牌
        //            if (ProjectTask_Cust.TypeID == ((int)EnumCustomType.FourS).ToString())
        //            {
        //                List<Entities.ProjectTask_CSTMember_Brand> ccBrandInfos = BLL.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_Brand(ProjectTask_CSTMember[i].ID);

        //                foreach (Entities.ProjectTask_CSTMember_Brand info in ccBrandInfos)
        //                {
        //                    Crm2009.Entities.CSTMember_Brand cstMemberBrand = new CSTMember_Brand();
        //                    cstMemberBrand.CreateTime = DateTime.Now;
        //                    cstMemberBrand.CSTRecID = cstRecId;
        //                    cstMemberBrand.BrandID = info.BrandID;

        //                    Crm2009.BLL.CSTMember_Brand.Instance.Add(cstMemberBrand);
        //                }
        //            }
        //            #endregion
        //        }

        //    }
        //    return 1;
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