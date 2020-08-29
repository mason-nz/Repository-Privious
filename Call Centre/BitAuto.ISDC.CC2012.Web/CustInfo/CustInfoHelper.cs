using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using BitAuto.ISDC.CC2012.Web.Util;

namespace BitAuto.ISDC.CC2012.Web.CustInfo
{
    /// <summary>
    /// 客户信息相关工具
    /// </summary>
    public class CustInfoHelper
    {
        #region Query Properties

        private string tId;
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TID
        {
            get
            {
                if (tId == null)
                {
                    tId = HttpUtility.UrlDecode((Request["TID"] + "").Trim());
                }
                return tId;
            }
        }

        private string custId;
        /// <summary>
        /// 客户ID
        /// </summary>
        public string CustID
        {
            get
            {
                if (custId == null)
                {
                    custId = HttpUtility.UrlDecode((Request["CustID"] + "").Trim());
                }
                return custId;
            }
        }
        private string crmCustId;
        /// <summary>
        /// CRM客户ID
        /// </summary>
        public string CrmCustID
        {
            get
            {
                if (custId == null)
                {
                    custId = HttpUtility.UrlDecode((Request["CrmCustID"] + "").Trim());
                }
                return custId;
            }
        }
        public string contactId;

        /// <summary>
        /// 联系人ID
        /// </summary>
        public string ContactID
        {
            get
            {
                if (contactId == null)
                {
                    contactId = HttpUtility.UrlDecode((Request["ContactID"] + "").Trim());
                }
                return contactId;
            }
        }

        private string delrelationcustids;
        /// <summary>
        /// 删除客户关联ID串
        /// </summary>
        public string DelRelationCustIDs
        {
            get
            {
                if (delrelationcustids == null)
                {
                    delrelationcustids = HttpUtility.UrlDecode((Request["DelRelationCustIDs"] + "").Trim());
                }
                return delrelationcustids;
            }
        }

        private string custName;
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustName
        {
            get
            {
                if (custName == null)
                {
                    custName = HttpUtility.UrlDecode((Request["CustName"] + "").Trim());
                }
                return custName;
            }
        }


        private string memberId;
        /// <summary>
        /// 会员ID
        /// </summary>
        public string MemberID
        {
            get
            {
                if (memberId == null)
                {
                    memberId = HttpUtility.UrlDecode((Request["MemberID"] + "").Trim());
                }
                return memberId;
            }
        }

        private string memberCode;
        private string MemberCode
        {
            get
            {
                if (memberCode == null)
                {
                    memberCode = HttpUtility.UrlDecode((Request["MemberCode"] + "").Trim());
                }
                return memberCode;
            }
        }

        private string cstMemberName;
        /// <summary>
        /// 会员ID
        /// </summary>
        public string CstMemberName
        {
            get
            {
                if (cstMemberName == null)
                {
                    cstMemberName = HttpUtility.UrlDecode((Request["CstMemberName"] + "").Trim());
                }
                return cstMemberName;
            }
        }
        //--------------------------
        private string checkedInfo;
        /// <summary>
        /// 核实信息
        /// </summary>
        public string CheckedInfo
        {
            get
            {
                if (checkedInfo == null)
                {
                    checkedInfo = (Request["CheckedInfo"] + "").Trim();
                }
                return checkedInfo;
            }
        }

        private string descriptionWhenDelete;
        /// <summary>
        /// 删除核实信息时的备注信息
        /// </summary>
        public string DescriptionWhenDelete
        {
            get
            {
                if (descriptionWhenDelete == null)
                {
                    descriptionWhenDelete = HttpUtility.UrlDecode((Request["DescriptionWhenDelete"] + "").Trim());
                }
                return descriptionWhenDelete;
            }
        }


        private string additionalStatus;
        /// <summary>
        /// 保存核实信息时的附加状态
        /// </summary>
        public string AdditionalStatus
        {
            get
            {
                if (additionalStatus == null)
                {
                    additionalStatus = HttpUtility.UrlDecode((Request["AdditionalStatus"] + "").Trim());
                }
                return additionalStatus;
            }
        }

        private string descriptionWhenSave;
        /// <summary>
        /// 保存核实信息时的附加信息
        /// </summary>
        public string DescriptionWhenSave
        {
            get
            {
                if (descriptionWhenSave == null)
                {
                    descriptionWhenSave = HttpUtility.UrlDecode((Request["G_description"] + "").Trim());
                }
                return descriptionWhenSave;
            }
        }

        private string descriptionWhenStop;
        /// <summary>
        /// 停用客户理由
        /// </summary>
        public string DescriptionWhenStop
        {
            get
            {
                if (descriptionWhenStop == null)
                {
                    descriptionWhenStop = HttpUtility.UrlDecode((Request["DescriptionWhenStop"] + "").Trim());
                }
                return descriptionWhenStop;
            }
        }

        #endregion

        #region Common Properties

        private int currentPage = -1;
        public int CurrentPage
        {
            get
            {
                if (currentPage <= 0) { currentPage = PagerHelper.GetCurrentPage(); }
                return currentPage;
            }
            set { currentPage = value; }
        }

        private int pageSize = -1;
        public int PageSize
        {
            get
            {
                if (pageSize <= 0) { pageSize = PagerHelper.GetPageSize(); }
                return pageSize;
            }
            set { pageSize = value; }
        }

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string Action { get { return (Request["Action"] + "").Trim().ToLower(); } }

        public string RequestIsPartShow
        {
            get { return BLL.Util.GetCurrentRequestFormStr("IsPartShow"); }
        }
        #endregion

        /// <summary>
        /// 从CRM得到一个客户信息
        /// </summary>
        internal BitAuto.YanFa.Crm2009.Entities.CustInfo GetOneCustFromCRM()
        {
            if (string.IsNullOrEmpty(this.CustID))
            {
                //throw new Exception("ContractID参数为空");
                return null;
            }
            return BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(this.CustID);
        }

        /// <summary>
        /// 从CRM得到一个会员信息
        /// </summary>
        internal BitAuto.YanFa.Crm2009.Entities.DMSMember GetOneMemberFromCRM()
        {
            if (!string.IsNullOrEmpty(this.MemberID))
            {
                return BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(new Guid(this.MemberID));
            }
            if (!string.IsNullOrEmpty(this.MemberCode))
            {
                return BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByMemberCode(this.MemberCode);
            }
            return null;
        }

        /// <summary>
        /// 删除CC_Member会员
        /// </summary>
        internal void DeleteCCMember()
        {
            BLL.ProjectTask_DMSMember.Instance.Delete(WebUtil.Converter.String2Int(this.MemberID, -1));
        }

        internal void DeleteCCCstMember()
        {
            BLL.ProjectTask_CSTMember.Instance.Delete(WebUtil.Converter.String2Int(this.MemberID, -1));
        }
        /// <summary>
        /// 验证保存核实信息
        /// </summary>
        internal void VerifySaveCheckInfo()
        {
            string d = this.CheckedInfo;
            CheckedInfo ci = (CheckedInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(d, typeof(CheckedInfo));

            Entities.ProjectTask_Cust cust = null;
            List<Entities.ProjectTask_DMSMember> members = null;
            List<Entities.ProjectTask_CSTMember> cstMembers = null;
            List<Entities.ProjectTask_CSTLinkMan> linkMans = null;
            List<string> cstMemberBrands = null;
            //校验信息
            ci.Validate(false, out cust, out members, out cstMembers, out linkMans, out cstMemberBrands);
            ci.ValidateOperation();
            if (string.IsNullOrEmpty(ci.CustInfo.DelRelationCustIDs))
            {
                //清空删除申请数据
                BLL.ProjectTask_DelCustRelation.Instance.DeleteByTID(cust.PTID);
            }
        }


        /// <summary>
        /// 保存核实信息
        /// </summary>
        internal void SaveCheckInfo()
        {
            string d = this.CheckedInfo;
            CheckedInfo ci = (CheckedInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(d, typeof(CheckedInfo));

            Entities.ProjectTask_Cust cust = null;
            List<Entities.ProjectTask_DMSMember> members = null;
            List<Entities.ProjectTask_CSTMember> cstMembers = null;
            List<Entities.ProjectTask_CSTLinkMan> linkMans = null;
            List<string> cstMemberBrands = null;
            //校验信息
            ci.Validate(false, out cust, out members, out cstMembers, out linkMans, out cstMemberBrands);
            if (BLL.ProjectTaskInfo.Instance.TaskBelongToUser(cust.PTID, BLL.Util.GetLoginUserID()) == false)
            {
                throw new Exception("此任务不属于你");
            }
            ci.ValidateOperation();
            DateTime dtime = DateTime.Now;
            //保存核实的信息
            BLL.ProjectTask_Cust.Instance.SaveCheckedInfo(cust, members, cstMembers, linkMans, cstMemberBrands, dtime);
            //更新附加状态
            BLL.ProjectTaskInfo.Instance.InsertOrUpdateTaskAdditionalStatus(cust.PTID, this.AdditionalStatus, this.DescriptionWhenSave);
        }

        /// <summary>
        /// 提交核实信息
        /// </summary>
        internal void SubmitCheckInfo()
        {
            string d = this.CheckedInfo;
            CheckedInfo ci = (CheckedInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(d, typeof(CheckedInfo));
            Entities.ProjectTask_Cust cust = null;
            List<Entities.ProjectTask_DMSMember> members = null;
            List<Entities.ProjectTask_CSTMember> cstMembers = null;
            List<Entities.ProjectTask_CSTLinkMan> linkMans = null;
            List<string> cstMemberBrands = null;
            //校验信息
            ci.Validate(true, out cust, out members, out cstMembers, out linkMans, out cstMemberBrands);
            ci.ValidateOperation();
            //验证是否有权限提交数据
            int currentUserID = BLL.Util.GetLoginUserID();
            //if (BLL.CallRecordInfo.Instance.IsExistByTID(currentUserID, cust.PTID, 1))
            //{
            //    throw new Exception("必须要有本人的录音才可提交");
            //}
            //if (BLL.CallRecordInfo.Instance.IsExistUnBindByTID(cust.PTID) && cust.CarType != 2)
            //{
            //    throw new Exception("所有录音绑定之后才可提交");
            //}
            //验证是否新车有车商通信息
            if (BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(cust.PTID).Count > 0 && cust.CarType == 1)
            {
                throw new Exception("客户经营范围为：新车时，只能核实易湃会员；如需核实车商通会员请选择二手车后保存");
            }
            DateTime dtime = DateTime.Now;
            //保存核实的信息
            BLL.ProjectTask_Cust.Instance.SubmitCheckedInfo(cust, members, cstMembers, linkMans, cstMemberBrands, dtime);
            if (string.IsNullOrEmpty(ci.CustInfo.DelRelationCustIDs))
            {
                //清空删除申请数据
                BLL.ProjectTask_DelCustRelation.Instance.DeleteByTID(cust.PTID);
            }
        }

        /// <summary>
        /// 删除核实的客户信息
        /// </summary>
        internal void DeleteCheckInfo()
        {
            Entities.ProjectTaskInfo task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(this.TID);
            if (task == null) { throw new Exception("无此任务"); }
            ValidateOperation(this.TID);
            BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(task.PTID, Entities.EnumProjectTaskStatus.DelFinsh, Entities.EnumProjectTaskOperationStatus.TaskDel, this.DescriptionWhenDelete, DateTime.Now);
        }

        /// <summary>
        /// 客户名称是否存在
        /// </summary>
        internal bool CustNameExist(out string msg)
        {
            msg = "";
            if (string.IsNullOrEmpty(this.CustID))
            {
                return BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.IsExistCustName(this.CustName, out msg);
            }
            else
            {
                return BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.IsExistCustName(this.CustName, this.CustID, out msg);
            }
        }

        /// <summary>
        /// 客户是否停用
        /// </summary>
        /// <returns></returns>
        internal void CustIsLock()
        {
            Entities.ProjectTaskInfo task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(TID);
            BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(task.CrmCustID);
            if (custInfo != null)
            {
                if (custInfo.Lock == 1)
                {
                    throw new Exception("此会员已锁定，无法进行停用操作！");
                }
            }

        }
        /// <summary>
        /// 若任务状态为已处理（提交处理成功、提交处理失败、删除成功、删除失败），则不能操作了
        /// </summary>
        internal void ValidateOperation(string tid)
        {
            if (string.IsNullOrEmpty(tid)) { throw new Exception("TID不可为空"); }
            Entities.ProjectTaskInfo model = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(tid);
            if (model != null)
            {
                if (model.TaskStatus == (int)Entities.EnumProjectTaskStatus.SubmitFinsih ||
                    model.TaskStatus == (int)Entities.EnumProjectTaskStatus.DelFinsh ||
                    //model.TaskStatus == (int)Entities.EnumProjectTaskStatus.SubmitManageSuccess ||
                    //model.TaskStatus == (int)Entities.EnumProjectTaskStatus.SubmitManageFail ||
                    //model.TaskStatus == (int)Entities.EnumProjectTaskStatus.DelFail ||
                    //model.TaskStatus == (int)Entities.EnumProjectTaskStatus.DelSuccess ||
                    model.TaskStatus == (int)Entities.EnumProjectTaskStatus.Stop ||
                    //model.TaskStatus == (int)Entities.EnumProjectTaskStatus.StopSuccess||
                    //model.TaskStatus==(int)Entities.EnumProjectTaskStatus.StayAudit||
                    model.TaskStatus == (int)Entities.EnumProjectTaskStatus.StayReview ||
                    model.TaskStatus == (int)Entities.EnumProjectTaskStatus.Finshed)
                {
                    throw new Exception("任务状态已处理，不能再操作了");
                }
            }
            else
            {
                throw new Exception("无此任务");
            }
        }

        /// <summary>
        /// 客户名称重复列表，包含（客户名称、客户编号、客户状态、锁定状态）
        /// </summary>
        /// <returns>返回格式:客户编号_客户名称_客户状态_锁定状态</returns>
        internal string GetCustNameList()
        {
            string msg = "";
            int totalCount = 0;
            BitAuto.YanFa.Crm2009.Entities.QueryCustInfo qci = new BitAuto.YanFa.Crm2009.Entities.QueryCustInfo();
            qci.ExistCustName = this.CustName;
            if (!string.IsNullOrEmpty(this.CustID))
            {
                qci.ExistCustID = this.CustID;
            }
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(qci, "", 1, 100000, out totalCount);
            if (totalCount > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string custID = dt.Rows[i]["CustID"].ToString().Trim();
                    string custName = dt.Rows[i]["CustName"].ToString().Trim();
                    string status = dt.Rows[i]["Status"].ToString().Trim();
                    string lockStatus = dt.Rows[i]["Lock"].ToString().Trim();
                    msg += custID + "_" + BLL.Util.EscapeString(custName) + "_" + status + "_" + lockStatus + ",";
                }
            }
            return msg.TrimEnd(',');
        }

        internal string GetCustNameListSame()
        {
            string msg = "";

            int totalCount = 0;
            Entities.QueryProjectTask_DelCustRelation query = new Entities.QueryProjectTask_DelCustRelation();
            // query.CustID = this.CustID;
            query.PTID = this.TID;

            DataTable dt = BLL.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelation(query, "", 1, 9999, out totalCount);

            if (dt != null && dt.Rows.Count > 0)
            {
                BitAuto.YanFa.Crm2009.Entities.QueryCustInfo qci = new BitAuto.YanFa.Crm2009.Entities.QueryCustInfo();

                string[] idList = dt.Rows[0]["DelCustIDs"].ToString().Split(',');
                foreach (string id in idList)
                {
                    qci.CustID = id;
                    DataTable custdt = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(qci, "", 1, 100000, out totalCount);
                    if (totalCount > 0)
                    {
                        for (int i = 0; i < custdt.Rows.Count; i++)
                        {
                            string custID = custdt.Rows[i]["CustID"].ToString().Trim();
                            string custName = custdt.Rows[i]["CustName"].ToString().Trim();
                            string status = custdt.Rows[i]["Status"].ToString().Trim();
                            string lockStatus = custdt.Rows[i]["Lock"].ToString().Trim();
                            msg += custID + "_" + BLL.Util.EscapeString(custName) + "_" + status + "_" + lockStatus + ",";
                        }
                    }
                }


            }
            return msg.TrimEnd(',');
        }

        internal string GetCustContactInfo()
        {
            string msg = string.Empty;
            int id = -1;
            if (int.TryParse(this.ContactID, out id))
            {
                Entities.ProjectTask_Cust_Contact contactInfo = BLL.ProjectTask_Cust_Contact.Instance.GetContactInfo(id);
                if (contactInfo != null)
                {
                    msg += "{";
                    msg += "CnName:'" + contactInfo.CName + "',";
                    msg += "Title:'" + contactInfo.Title + "',";
                    msg += "Department:'" + contactInfo.DepartMent + "',";
                    msg += "Mobile:'" + contactInfo.Phone + "',";
                    msg += "Email:'" + contactInfo.Email + "'";
                    msg += "}";
                }
            }

            return msg;
        }
        /// <summary>
        /// 添加或编辑删除客户关系信息
        /// </summary>
        internal void AddDeleteCustRelationInfo()
        {
            if (!string.IsNullOrEmpty(this.TID) && !string.IsNullOrEmpty(this.DelRelationCustIDs))
            {
                Entities.ProjectTask_DelCustRelation model = BLL.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelationByTID(this.TID);
                if (model != null)
                {
                    model.CustID = this.CustID;
                    model.DelCustIDs = this.DelRelationCustIDs;
                    model.Remark = this.DescriptionWhenDelete;
                    BLL.ProjectTask_DelCustRelation.Instance.Update(model);
                }
                else
                {
                    model = new Entities.ProjectTask_DelCustRelation();
                    model.PTID = this.TID;
                    model.CustID = this.CustID;
                    model.DelCustIDs = this.DelRelationCustIDs;
                    model.Remark = this.DescriptionWhenDelete;
                    model.CreateTime = DateTime.Now;
                    model.CreateUserID = BLL.Util.GetLoginUserID();
                    BLL.ProjectTask_DelCustRelation.Instance.Insert(model);
                }
            }
            //else
            //{
            //    throw new Exception("任务ID无效");
            //}
        }

        /// <summary>
        /// 停用客户信息
        /// </summary>
        internal void StopCustInfo()
        {
            if (!string.IsNullOrEmpty(TID))
            {
                BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(TID, Entities.EnumProjectTaskStatus.Stop, Entities.EnumProjectTaskOperationStatus.TaskDel, DescriptionWhenStop, DateTime.Now);
            }
            else
            {
                throw new Exception("任务ID不能为空");
            }
        }

        /// <summary>
        /// 根据任务ID删除新增车商通信息
        /// </summary>
        /// <param name="tid"></param>
        internal void DeleteNewProjectTask_CSTMemberByTID()
        {
            if (!string.IsNullOrEmpty(TID))
            {
                BLL.ProjectTask_CSTMember.Instance.DeleteNewProjectTask_CSTMemberByTID(TID);
            }
            else
            {
                throw new Exception("任务ID不能为空");
            }
        }

        /// <summary>
        /// 删除crm易湃会员
        /// </summary>
        internal void DeleteDMSMember()
        {
            //增加“客户池--易湃客户”删除功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            if (!BLL.Util.CheckRight(userId, "SYS024BUG200305"))
            {
                throw new Exception("没有删除操作权限");
            }
            if (!string.IsNullOrEmpty(MemberID))
            {
                BitAuto.YanFa.Crm2009.Entities.DMSMember model = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(new Guid(MemberID));
                if (model != null)
                {
                    if (model.SyncStatus == (int)BitAuto.YanFa.Crm2009.Entities.EnumDMSSyncStatus.Rejected)
                    {
                        BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.Delete(model.ID.ToString());
                        string content = string.Format("为【{0}(ID:{1})】删除会员【{2}(ID:{3})】成功。", model.CustName, model.CustID,
                    model.Name, model.MemberCode);
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(BitAuto.YanFa.Crm2009.BLL.LogModule.DMSMember, (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Delete, content);
                        BLL.Util.InsertUserLog(content);
                    }
                    else
                    {
                        throw new Exception("只有打回的易湃会员才能进行删除操作");
                    }
                }
                else
                {
                    throw new Exception("不存在此会员");
                }
            }
            else
            {
                throw new Exception("会员编号不能为空");
            }
        }
        /// <summary>
        /// 删除crm车商通会员
        /// </summary>
        internal void DeleteCSTMember()
        {
            if (!string.IsNullOrEmpty(MemberID))
            {
                BitAuto.YanFa.Crm2009.Entities.CstMember model = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCstMemberModel(MemberID);
                if (model != null)
                {
                    if (model.SyncStatus == (int)BitAuto.YanFa.Crm2009.Entities.EnumCSTSyncStatus.Rejected)
                    {
                        BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.Delete(model.ID.ToString());
                        string content = string.Format("为【{0}(ID:{1})】删除会员【{2}(ID:{3})】成功。", model.CustName, model.CustID,
                    model.FullName, model.CSTRecID);
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(BitAuto.YanFa.Crm2009.BLL.LogModule.CSTMember, (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Delete, content);
                        BLL.Util.InsertUserLog(content);
                    }
                    else
                    {
                        throw new Exception("只有打回的易湃会员才能进行删除操作");
                    }
                }
                else
                {
                    throw new Exception("不存在此会员");
                }
            }
            else
            {
                throw new Exception("会员编号不能为空");
            }
        }
    }


    public class CheckedInfo
    {
        public CheckedCustInfo CustInfo;

        public CheckedMemberInfo[] MemberInfoArray;

        public CheckedCstMemberInfo[] CstMemberInfoArray;

        public void Validate(bool whenSubmit, out Entities.ProjectTask_Cust cust, out List<Entities.ProjectTask_DMSMember> members, out List<Entities.ProjectTask_CSTMember> cstMembers, out List<Entities.ProjectTask_CSTLinkMan> cstLinkMans, out List<string> cstMemberBrandsArry)//save or submit
        {
            Regex reMemberPhoneAndFax = new Regex(@"(^0[0-9]{2,3}-[0-9]{7,8}$)|(^0[0-9]{2,3}-[0-9]{7,8}-[0-9]{1,5}$)|(^13[0-9]{9}$)|(^14[0-9]{9}$)|(^15[0-9]{9}$)|(^18[0-9]{9}$)|(^400\d{7}$)");

            int i = -1;

            #region 客户信息验证
            if (CustInfo == null) { throw new Exception("必须要有客户信息"); }

            if (string.IsNullOrEmpty(CustInfo.TID)) { throw new Exception("TID不可为空"); }
            cust = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(CustInfo.TID);
            if (cust == null) { throw new Exception("找不到此客户的核对信息"); }

            if (string.IsNullOrEmpty(CustInfo.CustName)) { throw new Exception("客户名称不可为空"); }
            if (CustInfo.CustName.Length > 50) { throw new Exception("客户名称超长"); }
            cust.CustName = CustInfo.CustName;

            if (CustInfo.CustAbbr.Length > 50) { throw new Exception("客户简称超长"); }
            cust.AbbrName = CustInfo.CustAbbr;

            if (string.IsNullOrEmpty(CustInfo.LevelID) == false && int.TryParse(CustInfo.LevelID, out i) == false) { throw new Exception("LevelID无法转换成int类型"); }
            cust.LevelID = CustInfo.LevelID;

            if (string.IsNullOrEmpty(CustInfo.IndustryID) == false && int.TryParse(CustInfo.IndustryID, out i) == false) { throw new Exception("IndustryID无法转换成int类型"); }
            if (whenSubmit && i <= 0) { throw new Exception("客户行业必填"); }
            cust.IndustryID = CustInfo.IndustryID;

            if (string.IsNullOrEmpty(CustInfo.TypeID) == false && int.TryParse(CustInfo.TypeID, out i) == false) { throw new Exception("TypeID无法转换成int类型"); }
            if (whenSubmit && i <= 0) { throw new Exception("客户类别必填"); }
            cust.TypeID = CustInfo.TypeID;

            if ((!string.IsNullOrEmpty(CustInfo.CustPid)) && CustInfo.CustPid.Length > 50) { throw new Exception("CustPid超长"); }
            cust.CustPid = CustInfo.CustPid;

            if (whenSubmit && cust.CustPid == Convert.ToString((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Showroom)
                && string.IsNullOrEmpty(CustInfo.CustPid))
            {
                throw new Exception("当客户类别为“展厅”时，必须选择所属厂商");
            }

            if ((!string.IsNullOrEmpty(CustInfo.Pid)) && CustInfo.Pid.Length > 50) { throw new Exception("Pid超长"); }
            cust.Pid = CustInfo.Pid;

            if (string.IsNullOrEmpty(CustInfo.ShopLevel) == false && int.TryParse(CustInfo.ShopLevel, out i) == false) { throw new Exception("ShopLevel无法转换成int类型"); }
            cust.ShopLevel = CustInfo.ShopLevel;

            if (string.IsNullOrEmpty(CustInfo.ProvinceID) == false && int.TryParse(CustInfo.ProvinceID, out i) == false) { throw new Exception("ProvinceID无法转换成int类型"); }
            if (whenSubmit && i <= 0) { throw new Exception("客户省份必填"); }
            cust.ProvinceID = CustInfo.ProvinceID;

            if (string.IsNullOrEmpty(CustInfo.CityID) == false && int.TryParse(CustInfo.CityID, out i) == false) { throw new Exception("CityID无法转换成int类型"); }
            if (whenSubmit && i <= 0) { throw new Exception("客户城市必填"); }
            cust.CityID = CustInfo.CityID;

            if (string.IsNullOrEmpty(CustInfo.CountyID) == false && int.TryParse(CustInfo.CountyID, out i) == false) { throw new Exception("CountyID无法转换成int类型"); }
            if (CustInfo.IsHasCounty.Equals("0"))
            {
                if (whenSubmit && i <= 0) { throw new Exception("客户区县必填"); }
            }
            cust.CountyID = CustInfo.CountyID;

            if (CustInfo.Address.Length > 400) { throw new Exception("客户地址超长"); }
            if (whenSubmit && string.IsNullOrEmpty(CustInfo.Address)) { throw new Exception("客户地址必填"); }
            cust.Address = CustInfo.Address;

            if (CustInfo.Zipcode.Length > 10) { throw new Exception("客户邮编超长"); }
            if (whenSubmit && string.IsNullOrEmpty(CustInfo.Zipcode)) { throw new Exception("客户邮编必填"); }
            cust.Zipcode = CustInfo.Zipcode;

            if (CustInfo.OfficeTel.Length > 50) { throw new Exception("客户电话超长"); }
            cust.OfficeTel = CustInfo.OfficeTel;

            if (CustInfo.Fax.Length > 50) { throw new Exception("客户传真超长"); }
            cust.Fax = CustInfo.Fax;

            if (CustInfo.Notes.Length > 2000) { throw new Exception("客户备注超长"); }
            cust.Notes = CustInfo.Notes;

            if (CustInfo.ContactName.Length > 50) { throw new Exception("客户联系人超长"); }
            cust.ContactName = CustInfo.ContactName;

            if ((!string.IsNullOrEmpty(CustInfo.Brand)) && CustInfo.Brand.Length > 0)//主营品牌
            {
                cust.BrandIDs = WebUtil.Converter.String2IntList(CustInfo.Brand, ",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                cust.BrandIDs = null;
            }
            int carType = -1;
            if (HttpUtility.UrlDecode(CustInfo.CarType) == "1,2")
            {
                cust.CarType = 3;
                int usedCarBusinessType = -1;
                if (!int.TryParse(CustInfo.UsedCarBusinessType, out usedCarBusinessType))
                {
                    throw new Exception("请选择二手车经营类型！");
                }
                else
                {
                    //当经营范围选择了二手车选项并且客户类别为4s 则二手车类型必须选择！
                    if (cust.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.FourS).ToString() && usedCarBusinessType == -1)
                    {
                        throw new Exception("请选择二手车经营类型！");
                    }
                    else if (usedCarBusinessType > 0)
                    {
                        cust.UsedCarBusinessType = usedCarBusinessType.ToString();
                    }
                }
                if (!string.IsNullOrEmpty(CustInfo.TradeMarketID))
                {
                    int tradeMarketId = -1;
                    if (int.TryParse(CustInfo.TradeMarketID, out tradeMarketId))
                    {
                        if (tradeMarketId > 0)
                        {
                            cust.TradeMarketID = tradeMarketId.ToString();
                        }
                    }
                }
            }
            else if (int.TryParse(CustInfo.CarType, out carType))
            {
                cust.CarType = carType;
            }
            else
            {
                throw new Exception("请选择经营范围！");
            }

            if ((!string.IsNullOrEmpty(CustInfo.FoursPid)) && CustInfo.FoursPid.Length > 50) { throw new Exception("FoursPid超长"); }
            cust.FoursPid = CustInfo.FoursPid;
            #endregion
            //---------------------------------------------------
            members = new List<Entities.ProjectTask_DMSMember>();
            cstMembers = new List<Entities.ProjectTask_CSTMember>();
            cstLinkMans = new List<Entities.ProjectTask_CSTLinkMan>();
            cstMemberBrandsArry = new List<string>();

            #region 会员信息验证
            if (whenSubmit && CustInfo.CarType != "2" && CustInfo.TypeID != "20004" && CustInfo.TypeID != "20005" && CustInfo.TypeID != "20009")
            {
                //20004 特许经销商、20005综合店 、20009展厅时，可以没有会员

                if (this.MemberInfoArray == null || this.MemberInfoArray.Length == 0)
                {
                    throw new Exception("至少要有一条易湃会员信息");
                }
            }

            Hashtable memberNames = new Hashtable();
            if (this.MemberInfoArray != null)
            {
                foreach (CheckedMemberInfo m in this.MemberInfoArray)
                {
                    Entities.ProjectTask_DMSMember member = null;
                    if (string.IsNullOrEmpty(m.MemberID) == false && int.TryParse(m.MemberID, out i) == false) { throw new Exception("MemberID无法转换成int类型"); }
                    if (string.IsNullOrEmpty(m.MemberID))//新增会员
                    {
                        member = new Entities.ProjectTask_DMSMember();
                    }
                    else//会员已存在
                    {
                        member = BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMember(i);
                        if (member == null) { throw new Exception("找不到会员的核对信息"); }
                    }

                    if (string.IsNullOrEmpty(m.MemberName)) { throw new Exception("会员名称不可为空"); }
                    if (m.MemberName.Length > 256) { throw new Exception("会员名称超长"); }
                    //if (memberNames.ContainsKey(m.MemberName)) { throw new Exception("会员名称不可重复"); }
                    //else { memberNames.Add(m.MemberName, m.MemberName); }
                    member.Name = m.MemberName;

                    if (whenSubmit && string.IsNullOrEmpty(m.MemberAbbr)) { throw new Exception("会员简称不可为空"); }
                    if (BLL.Util.GetLength(m.MemberAbbr) > 14) { throw new Exception("会员简称不能超过14个字符"); }//Modify=Masj,Date=2012-08-28
                    if (memberNames.ContainsKey(m.MemberAbbr)) { throw new Exception("会员简称不可重复"); }
                    else { memberNames.Add(m.MemberAbbr, m.MemberAbbr); }
                    member.Abbr = m.MemberAbbr;
                    if (whenSubmit)
                    {
                        if (string.IsNullOrEmpty(member.OriginalDMSMemberID) == false)
                        {

                            if (BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.IsExistsByAbbrName(member.OriginalDMSMemberID, m.MemberAbbr, 1))
                            {
                                BitAuto.YanFa.Crm2009.Entities.QueryDMSMember query = new BitAuto.YanFa.Crm2009.Entities.QueryDMSMember();
                                query.Abbr = m.MemberAbbr;
                                int total = 0;
                                DataTable dt = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetMemberAndCust(query, "", 1, 1000000, out total);
                                string msg = string.Empty;
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    msg += "客户名称：" + dt.Rows[j]["CustName"].ToString() + ";会员全称：" + dt.Rows[j]["Name"].ToString() + ";会员简称" + dt.Rows[j]["Abbr"].ToString() + "<br>";
                                    if (j < dt.Rows.Count)
                                    {
                                        msg += "--------------------------------";
                                    }
                                }
                                throw new Exception("已存在此会员简称<br>" + msg);
                            }
                        }
                        else
                        {
                            if (BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.IsExistsByAbbrName(m.MemberAbbr, 1))
                            {
                                BitAuto.YanFa.Crm2009.Entities.QueryDMSMember query = new BitAuto.YanFa.Crm2009.Entities.QueryDMSMember();
                                query.Abbr = m.MemberAbbr;
                                int total = 0;
                                DataTable dt = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetMemberAndCust(query, "", 1, 1000000, out total);
                                string msg = string.Empty;
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    msg += "客户名称：" + dt.Rows[j]["CustName"].ToString() + ",会员全称：" + dt.Rows[j]["Name"].ToString() + ",简称" + dt.Rows[j]["Abbr"].ToString() + "<br>";
                                }
                                throw new Exception("已存在此会员简称<br>" + msg);
                            }
                        }
                    }
                    if (whenSubmit && string.IsNullOrEmpty(m.MemberType)) { throw new Exception("会员类型不能为空"); }

                    member.MemberType = m.MemberType;

                    if (m.Phone.Length > 256) { throw new Exception("会员电话超长"); }

                    if (!string.IsNullOrEmpty(m.Phone) &&
                        m.Phone.Split(',').Length > 0)
                    {
                        foreach (string phone in m.Phone.Split(','))
                        {
                            if ((!string.IsNullOrEmpty(phone)) && (!reMemberPhoneAndFax.IsMatch(phone)))
                            {
                                throw new Exception("会员电话格式不正确，<br/>固话：010-68492345 010-68492345-8217 手机：直接输入13245687921 特殊号码：4000689960");
                            }
                        }
                    }
                    member.Phone = m.Phone.TrimEnd(',').TrimStart(',');

                    member.BrandIDs = m.Brand;

                    if (m.Fax.Length > 256) { throw new Exception("会员传真超长"); }
                    if (!string.IsNullOrEmpty(m.Fax) &&
                        m.Fax.Split(',').Length > 0)
                    {
                        foreach (string fax in m.Fax.Split(','))
                        {
                            if ((!string.IsNullOrEmpty(fax)) && (!reMemberPhoneAndFax.IsMatch(fax)))
                            {
                                throw new Exception("会员传真格式不正确，<br/>固话：010-68492345 010-68492345-8217 手机：直接输入13245687921 特殊号码：4000689960");
                            }
                        }
                    }
                    member.Fax = m.Fax.TrimEnd(',').TrimStart(',');

                    if (m.CompanyWebSite.Length > 256) { throw new Exception("会员公司网址超长"); }
                    member.CompanyWebSite = m.CompanyWebSite;

                    if (m.Email.Length > 256) { throw new Exception("会员Email超长"); }
                    member.Email = m.Email;

                    if (m.Postcode.Length > 32) { throw new Exception("会员邮编超长"); }
                    member.Postcode = m.Postcode;

                    if (whenSubmit && string.IsNullOrEmpty(member.BrandIDs) && member.MemberType != "3") { throw new Exception("主营品牌必填"); }

                    if (string.IsNullOrEmpty(m.ProvinceID) == false && int.TryParse(m.ProvinceID, out i) == false) { throw new Exception("ProvinceID无法转换成int类型"); }
                    if (whenSubmit && i <= 0) { throw new Exception("会员省份必填"); }
                    member.ProvinceID = m.ProvinceID;

                    if (string.IsNullOrEmpty(m.CityID) == false && int.TryParse(m.CityID, out i) == false) { throw new Exception("CityID无法转换成int类型"); }
                    if (whenSubmit && i <= 0) { throw new Exception("会员城市必填"); }
                    member.CityID = m.CityID;

                    if (string.IsNullOrEmpty(m.CountyID) == false && int.TryParse(m.CountyID, out i) == false) { throw new Exception("CountyID无法转换成int类型"); }
                    if (m.IsHasCounty.Equals("0"))
                    {
                        if (whenSubmit && i <= 0) { throw new Exception("会员区县必填"); }
                    }
                    member.CountyID = m.CountyID;

                    if (m.Address.Length > 256) { throw new Exception("会员地址超长"); }
                    if (whenSubmit && string.IsNullOrEmpty(m.Address)) { throw new Exception("会员地址必填"); }
                    member.ContactAddress = m.Address;

                    float l = -1;
                    if (string.IsNullOrEmpty(m.Longitude) == false && float.TryParse(m.Longitude, out l) == false) { throw new Exception("Longitude无法转换成float类型"); }
                    if (whenSubmit && l <= 0) { throw new Exception("地图标点必填"); }
                    member.Longitude = m.Longitude;

                    if (string.IsNullOrEmpty(m.Lantitude) == false && float.TryParse(m.Lantitude, out l) == false) { throw new Exception("Lantitude无法转换成float类型"); }
                    if (whenSubmit && l <= 0) { throw new Exception("地图标点必填"); }
                    member.Lantitude = m.Lantitude;

                    member.SerialIds = m.SerialIds;

                    if (m.TrafficInfo.Length > 1024) { throw new Exception("会员交通信息超长"); }
                    member.TrafficInfo = m.TrafficInfo;

                    if (m.EnterpriseBrief.Length > 1024) { throw new Exception("会员企业简介超长"); }
                    member.EnterpriseBrief = m.EnterpriseBrief;

                    if (m.Notes.Length > 1024) { throw new Exception("会员备注超长"); }
                    member.Remarks = m.Notes;

                    members.Add(member);
                }
            }
            #endregion

            #region 车商通会员信息验证
            if (cust.CarType != 1)
            {
                Hashtable htMemberName = new Hashtable();
                Hashtable htMemberAbbr = new Hashtable();
                Hashtable htVendorCode = new Hashtable();
                foreach (CheckedCstMemberInfo m in this.CstMemberInfoArray)
                {

                    Entities.ProjectTask_CSTMember cstMemberInfo = null;
                    if (string.IsNullOrEmpty(m.MemberID))
                    {
                        cstMemberInfo = new Entities.ProjectTask_CSTMember();
                    }
                    else
                    {
                        int memberId = -1;
                        if (int.TryParse(m.MemberID, out memberId))
                        {
                            cstMemberInfo = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(memberId);
                        }
                    }
                    cstMemberInfo.Address = m.Address;
                    int tempId = -1;
                    if (!int.TryParse(m.CityID, out tempId) && whenSubmit)
                    {
                        throw new Exception(m.MemberName + ",请选择会员地区！");

                    }

                    cstMemberInfo.CityID = m.CityID;

                    cstMemberInfo.CountyID = m.CountyID;
                    cstMemberInfo.CreateTime = DateTime.Now;
                    cstMemberInfo.CreateUserID = BLL.Util.GetLoginUserID();
                    if (int.TryParse(m.MemberID, out tempId))
                    {
                        cstMemberInfo.ID = int.Parse(m.MemberID);
                    }
                    if (whenSubmit)
                    {
                        if (!htMemberName.Contains(m.MemberName))
                        {
                            htMemberName.Add(m.MemberName, m.MemberName);
                        }
                        else
                        {
                            throw new Exception("同一客户下，不能存在相同名称的车商通会员");
                        }
                        if (m.MemberName.Length == 0)
                        {
                            throw new Exception(m.MemberName + ",会员名称不能为空！");
                        }
                        else if (m.MemberName.Length > 25)
                        {
                            throw new Exception(m.MemberName + ",会员名称不能超过25个汉字！");
                        }
                    }
                    cstMemberInfo.FullName = m.MemberName;
                    if (m.MemberAbbr.Length > 16)//Modify=Masj,Date=2012-08-28
                    {
                        throw new Exception(m.MemberAbbr + ",会员简称不能超过16个字符！");
                    }
                    if (whenSubmit)
                    {
                        if (!htMemberAbbr.Contains(m.MemberAbbr))
                        {
                            htMemberAbbr.Add(m.MemberAbbr, m.MemberAbbr);
                        }
                        else
                        {
                            throw new Exception("同一客户下，不能存在相同简称的车商通会员");
                        }
                        if (m.MemberAbbr.Length == 0)
                        {
                            throw new Exception(m.MemberAbbr + ",会员简称不能为空！");
                        }
                        else
                        {
                            if (int.TryParse(m.MemberID, out tempId))
                            {
                                //是否存在重名的会员简称
                                Entities.ProjectTask_CSTMember memberInfo = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(tempId);

                                //yyh查重
                                if (!string.IsNullOrEmpty(memberInfo.OriginalCSTRecID))
                                {
                                    string where = " ShortName='" + Utils.StringHelper.SqlFilter(m.MemberAbbr) + "' and CSTRecID!='" + memberInfo.OriginalCSTRecID + "'";
                                    if (BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                                    {
                                        throw new Exception(m.MemberName + ",有重名的会员简称！");
                                    }
                                }
                                else
                                {
                                    string where = " ShortName='" + Utils.StringHelper.SqlFilter(m.MemberAbbr) + "'";
                                    if (BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                                    {
                                        throw new Exception(m.MemberName + ",有重名的会员简称！");
                                    }
                                }
                            }
                            else
                            {
                                string where = " ShortName='" + Utils.StringHelper.SqlFilter(m.MemberAbbr) + "'";
                                if (BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.ExistsCstMember(where) > 0)
                                {
                                    throw new Exception(m.MemberName + ",有重名的会员简称！");
                                }
                            }
                        }
                    }
                    cstMemberInfo.ShortName = m.MemberAbbr;
                    if (m.PostCode.Length > 6)
                    {
                        throw new Exception(m.MemberName + ",会员邮编不能超过6个字符！");
                    }

                    cstMemberInfo.PostCode = m.PostCode;
                    cstMemberInfo.ProvinceID = m.ProvinceID;
                    if (whenSubmit)
                    {
                        if (int.TryParse(m.SuperID, out tempId) && whenSubmit)
                        {
                            if (int.Parse(m.SuperID) <= 0)
                            {
                                throw new Exception(m.MemberName + ",请选择上级公司！");
                            }
                        }
                        else
                        {
                            throw new Exception(m.MemberName + ",请选择上级公司！");
                        }
                    }
                    cstMemberInfo.SuperId = int.Parse(m.SuperID);
                    cstMemberInfo.VendorClass = int.Parse(m.MemberType);

                    //cstMemberInfo.TrafficInfo = m.TrafficInfo;
                    cstMembers.Add(cstMemberInfo);

                    if (whenSubmit)
                    {
                        if (m.MemberType == "2" & m.Brand.Length == 0) //4S类型客户
                        {
                            //4S客户，没有填写品牌

                            #region 修改老数据时，不用判断品牌为空，新增会员时，要判断品牌

                            int mid = -1;

                            if (int.TryParse(m.MemberID, out mid))
                            {
                                //是修改
                                Entities.ProjectTask_CSTMember cstMember = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMember(mid);
                                if (cstMember.CreateTime > Convert.ToDateTime("2012-07-02 00:00:01"))
                                {
                                    //如果会员的创建时间小于指定时间，是修改老数据，不用要判断品牌
                                    //如果是新数据，要判断
                                    throw new Exception(m.MemberName + ",至少选择一个主营品牌！");

                                }
                            }
                            else
                            {
                                //新增会员

                                throw new Exception(m.MemberName + ",至少选择一个主营品牌！");
                            }

                            #endregion
                        }
                    }
                    string[] brandIds = m.Brand.Split(',');
                    if (m.Brand.Length > 0)
                    {
                        cstMemberBrandsArry.Add(m.Brand);
                    }
                    else
                    {
                        cstMemberBrandsArry.Add(string.Empty);
                    }

                    Entities.ProjectTask_CSTLinkMan linkManInfo = new Entities.ProjectTask_CSTLinkMan();
                    linkManInfo.CreateTime = DateTime.Now;
                    if (whenSubmit)
                    {
                        bool isOpen = false;
                        if (int.TryParse(m.MemberID, out tempId))
                        {
                            isOpen = BLL.ProjectTask_CSTMember.Instance.IsOpenSuccessMember(tempId);
                        }
                        if (!isOpen)
                        {
                            if (m.Email.Length == 0)
                            {
                                throw new Exception(m.MemberName + ",企业联系人邮件不能为空！");
                            }
                            if (m.Mobile.Length == 0)
                            {
                                throw new Exception(m.MemberName + ",企业联系人手机不能为空！");
                            }
                            if (m.LinkManName.Length == 0)
                            {
                                throw new Exception(m.MemberName + ",企业联系人名称不能为空！");
                            }
                        }
                    }

                    //linkManInfo.Department = m.Department;
                    linkManInfo.Email = m.Email;
                    linkManInfo.Mobile = m.Mobile;
                    linkManInfo.Name = m.LinkManName;
                    //linkManInfo.Position = m.Position;
                    linkManInfo.PTID = cust.PTID;
                    linkManInfo.CreateUserID = BLL.Util.GetLoginUserID();
                    linkManInfo.Status = 0;
                    linkManInfo.CreateTime = DateTime.Now;
                    cstLinkMans.Add(linkManInfo);
                }
            }
            #endregion
        }

        /// <summary>
        /// 若任务状态为已处理（提交处理成功、提交处理失败、删除成功、删除失败），则不能操作了
        /// </summary>
        public void ValidateOperation()
        {
            if (string.IsNullOrEmpty(CustInfo.TID)) { throw new Exception("TID不可为空"); }
            Entities.ProjectTaskInfo model = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(CustInfo.TID);
            if (model != null)
            {
                if (model.TaskStatus == (int)Entities.EnumProjectTaskStatus.SubmitFinsih ||
                    model.TaskStatus == (int)Entities.EnumProjectTaskStatus.DelFinsh ||
                    model.TaskStatus == (int)Entities.EnumProjectTaskStatus.StayReview
                    //model.TaskStatus == (int)Entities.EnumProjectTaskStatus.DelFail ||
                    //model.TaskStatus == (int)Entities.EnumProjectTaskStatus.DelSuccess||
                    //model.TaskStatus==(int)Entities.EnumProjectTaskStatus.StayAudit
                    )
                {
                    throw new Exception("任务状态已处理，不能再操作了");
                }
            }
            else
            {
                throw new Exception("无此任务");
            }
        }

        public class CheckedCustInfo
        {
            private string tid;
            public string TID { get { return tid; } set { tid = HttpUtility.UrlDecode(value); } }

            private string custName;
            public string CustName { get { return custName; } set { custName = HttpUtility.UrlDecode(value); } }

            private string custAbbr;
            public string CustAbbr { get { return custAbbr; } set { custAbbr = HttpUtility.UrlDecode(value); } }

            private string typeID;
            public string TypeID { get { return typeID; } set { typeID = HttpUtility.UrlDecode(value); } }

            private string industryID;
            public string IndustryID { get { return industryID; } set { industryID = HttpUtility.UrlDecode(value); } }

            private string provinceID;
            public string ProvinceID { get { return provinceID; } set { provinceID = HttpUtility.UrlDecode(value); } }

            private string cityID;
            public string CityID { get { return cityID; } set { cityID = HttpUtility.UrlDecode(value); } }

            private string countyID;
            public string CountyID { get { return countyID; } set { countyID = HttpUtility.UrlDecode(value); } }

            private string ishascounty;
            /// <summary>
            /// 区县是否有内容，1=有，0=没有
            /// </summary>
            public string IsHasCounty { get { return ishascounty; } set { ishascounty = HttpUtility.UrlDecode(value); } }

            private string address;
            public string Address { get { return address; } set { address = HttpUtility.UrlDecode(value); } }

            private string zipcode;
            public string Zipcode { get { return zipcode; } set { zipcode = HttpUtility.UrlDecode(value); } }

            private string levelID;
            public string LevelID { get { return levelID; } set { levelID = HttpUtility.UrlDecode(value); } }

            private string custPid;
            public string CustPid { get { return custPid; } set { custPid = HttpUtility.UrlDecode(value); } }

            private string pid;
            public string Pid { get { return pid; } set { pid = HttpUtility.UrlDecode(value); } }

            private string shopLevel;
            public string ShopLevel { get { return shopLevel; } set { shopLevel = HttpUtility.UrlDecode(value); } }

            private string brand;
            public string Brand { get { return brand; } set { brand = HttpUtility.UrlDecode(value); } }

            private string officeTel;
            public string OfficeTel { get { return officeTel; } set { officeTel = HttpUtility.UrlDecode(value); } }

            private string fax;
            public string Fax { get { return fax; } set { fax = HttpUtility.UrlDecode(value); } }

            private string contactName;
            public string ContactName { get { return contactName; } set { contactName = HttpUtility.UrlDecode(value); } }

            private string notes;
            public string Notes { get { return notes; } set { notes = HttpUtility.UrlDecode(value); } }

            private string delrelationcustids;
            /// <summary>
            /// 建立删除客户关系表（客户ID）
            /// </summary>
            public string DelRelationCustIDs { get { return delrelationcustids; } set { delrelationcustids = HttpUtility.UrlDecode(value); } }

            private string delrelationcustidscontainlock;
            /// <summary>
            /// 在删除客户关系表中，是否存在锁定状态
            /// </summary>
            public string DelRelationCustIDsContainLock { get { return delrelationcustidscontainlock; } set { delrelationcustidscontainlock = HttpUtility.UrlDecode(value); } }

            private string carType;
            public string CarType
            {
                get { return carType; }
                set { carType = value; }
            }

            private string ctsMemberID;
            public string CtsMemberID
            {
                get { return ctsMemberID; }
                set { ctsMemberID = value; }
            }

            private string usedCarBusinessType;
            public string UsedCarBusinessType
            {
                get { return usedCarBusinessType; }
                set { usedCarBusinessType = value; }
            }

            private string tradeMarketID;
            public string TradeMarketID
            {
                get { return tradeMarketID; }
                set { tradeMarketID = value; }
            }
            private string foursPid;
            public string FoursPid
            {
                get { return foursPid; }
                set { foursPid = value; }
            }
        }

        public class CheckedMemberInfo
        {
            private string memberID;
            public string MemberID { get { return memberID; } set { memberID = HttpUtility.UrlDecode(value); } }

            private string memberName;
            public string MemberName { get { return memberName; } set { memberName = HttpUtility.UrlDecode(value); } }

            private string memberAbbr;
            public string MemberAbbr { get { return memberAbbr; } set { memberAbbr = HttpUtility.UrlDecode(value); } }

            private string memberType;
            public string MemberType { get { return memberType; } set { memberType = HttpUtility.UrlDecode(value); } }

            private string phone;
            public string Phone { get { return phone; } set { phone = HttpUtility.UrlDecode(value); } }

            private string brand;
            public string Brand { get { return brand; } set { brand = HttpUtility.UrlDecode(value); } }

            private string provinceID;
            public string ProvinceID { get { return provinceID; } set { provinceID = HttpUtility.UrlDecode(value); } }

            private string cityID;
            public string CityID { get { return cityID; } set { cityID = HttpUtility.UrlDecode(value); } }

            private string countyID;
            public string CountyID { get { return countyID; } set { countyID = HttpUtility.UrlDecode(value); } }

            private string ishascounty;
            /// <summary>
            /// 区县是否有内容，1=有，0=没有
            /// </summary>
            public string IsHasCounty { get { return ishascounty; } set { ishascounty = HttpUtility.UrlDecode(value); } }

            private string address;
            public string Address { get { return address; } set { address = HttpUtility.UrlDecode(value); } }

            private string longitude;
            public string Longitude { get { return longitude; } set { longitude = HttpUtility.UrlDecode(value); } }

            private string lantitude;
            public string Lantitude { get { return lantitude; } set { lantitude = HttpUtility.UrlDecode(value); } }

            private string fax;
            public string Fax { get { return fax; } set { fax = HttpUtility.UrlDecode(value); } }

            private string companyWebSite;
            public string CompanyWebSite { get { return companyWebSite; } set { companyWebSite = HttpUtility.UrlDecode(value); } }

            private string email;
            public string Email { get { return email; } set { email = HttpUtility.UrlDecode(value); } }


            private string postcode;
            public string Postcode { get { return postcode; } set { postcode = HttpUtility.UrlDecode(value); } }

            private string serialIds;
            public string SerialIds { get { return serialIds; } set { serialIds = HttpUtility.UrlDecode(value); } }

            private string trafficInfo;
            public string TrafficInfo { get { return trafficInfo; } set { trafficInfo = HttpUtility.UrlDecode(value); } }

            private string enterpriseBrief;
            public string EnterpriseBrief { get { return enterpriseBrief; } set { enterpriseBrief = HttpUtility.UrlDecode(value); } }

            private string notes;
            public string Notes { get { return notes; } set { notes = HttpUtility.UrlDecode(value); } }

            private string spanMemberName;
            public string SpanMemberName { get { return spanMemberName; } set { spanMemberName = HttpUtility.UrlDecode(value); } }
        }

        public class CheckedCstMemberInfo
        {
            private string memberID;
            public string MemberID { get { return memberID; } set { memberID = HttpUtility.UrlDecode(value); } }

            private string memberName;
            public string MemberName { get { return memberName; } set { memberName = HttpUtility.UrlDecode(value); } }

            private string memberAbbr;
            public string MemberAbbr { get { return memberAbbr; } set { memberAbbr = HttpUtility.UrlDecode(value); } }

            private string vendorCode;
            public string VendorCode { get { return vendorCode; } set { vendorCode = HttpUtility.UrlDecode(value); } }

            private string memberType;
            public string MemberType { get { return memberType; } set { memberType = HttpUtility.UrlDecode(value); } }

            private string provinceID;
            public string ProvinceID { get { return provinceID; } set { provinceID = HttpUtility.UrlDecode(value); } }

            private string cityID;
            public string CityID { get { return cityID; } set { cityID = HttpUtility.UrlDecode(value); } }

            private string countyID;
            public string CountyID { get { return countyID; } set { countyID = HttpUtility.UrlDecode(value); } }

            private string superID;
            public string SuperID { get { return superID; } set { superID = HttpUtility.UrlDecode(value); } }

            private string address;
            public string Address { get { return address; } set { address = HttpUtility.UrlDecode(value); } }

            private string postcode;
            public string PostCode { get { return postcode; } set { postcode = HttpUtility.UrlDecode(value); } }

            private string brand;
            public string Brand { get { return brand; } set { brand = HttpUtility.UrlDecode(value); } }

            private string linkManName;
            public string LinkManName { get { return linkManName; } set { linkManName = HttpUtility.UrlDecode(value); } }

            private string mobile;
            public string Mobile { get { return mobile; } set { mobile = HttpUtility.UrlDecode(value); } }

            private string email;
            public string Email { get { return email; } set { email = HttpUtility.UrlDecode(value); } }
        }

    }

}