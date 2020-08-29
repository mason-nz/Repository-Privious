using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.YanFa.Crm2009.Entities;
using Newtonsoft.Json;
using System.Web.SessionState;
using System.Data;
using System.Text;
using BitAuto.ISDC.CC2012.Web.Util;
using System.Diagnostics;
namespace BitAuto.ISDC.CC2012.Web.ReturnVisit.CC_Contact
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        bool success = true;
        string result = "";
        string message = "";
        private HttpContext currentContext;

        #region 属性
        /// <summary>
        /// ID
        /// </summary>
        private int RequestID
        {
            get { return currentContext.Request["ID"] == null ? 0 : Convert.ToInt32(currentContext.Request["ID"]); }
        }
        /// <summary>
        /// 直接上级
        /// </summary>
        private int RequestPid
        {
            get
            {
                int pid = 0;
                if (currentContext.Request["PID"] != null)
                {
                    Int32.TryParse(currentContext.Request["PID"], out pid);
                }
                return pid;
            }
        }
        /// <summary>
        /// 客户ID
        /// </summary>
        private string RequestCustID
        {
            get { return currentContext.Request["CustID"] == null ? string.Empty : currentContext.Request["CustID"].Trim(); }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        private string RequestCName
        {
            get { return currentContext.Request["CName"] == null ? string.Empty : currentContext.Request["CName"].Trim(); }
        }
        /// <summary>
        /// 英文名
        /// </summary>
        private string RequestEName
        {
            get { return currentContext.Request["EName"] == null ? string.Empty : currentContext.Request["EName"].Trim(); }
        }
        /// <summary>
        /// 性别
        /// </summary>
        private string RequestSex
        {
            get { return currentContext.Request["Sex"] == null ? string.Empty : currentContext.Request["Sex"].Trim(); }
        }
        /// <summary>
        /// 部门
        /// </summary>
        private string RequestDepartMent
        {
            get { return currentContext.Request["DepartMent"] == null ? string.Empty : currentContext.Request["DepartMent"].Trim(); }
        }
        /// <summary>
        /// 职级
        /// </summary>
        private string RequestOfficeTypeCode
        {
            get { return currentContext.Request["OfficeTypeCode"] == null ? string.Empty : currentContext.Request["OfficeTypeCode"].Trim(); }
        }
        /// <summary>
        /// 职务
        /// </summary>
        private string RequestTitle
        {
            get { return currentContext.Request["Title"] == null ? string.Empty : currentContext.Request["Title"].Trim(); }
        }
        /// <summary>
        /// 办公室电话
        /// </summary>
        private string RequestOfficeTel
        {
            get { return currentContext.Request["OfficeTel"] == null ? string.Empty : currentContext.Request["OfficeTel"].Trim(); }
        }
        /// <summary>
        /// 手机
        /// </summary>
        private string RequestPhone
        {
            get { return currentContext.Request["Phone"] == null ? string.Empty : currentContext.Request["Phone"].Trim(); }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        private string RequestEmail
        {
            get { return currentContext.Request["Email"] == null ? string.Empty : currentContext.Request["Email"].Trim(); }
        }
        /// <summary>
        /// 传真
        /// </summary>
        private string RequestFax
        {
            get { return currentContext.Request["Fax"] == null ? string.Empty : currentContext.Request["Fax"].Trim(); }
        }
        /// <summary>
        /// 备注
        /// </summary>
        private string RequestReamrk
        {
            get { return currentContext.Request["Reamrk"] == null ? string.Empty : currentContext.Request["Reamrk"].Trim(); }
        }

        private string RequestAddress
        {
            get { return currentContext.Request["Address"] == null ? string.Empty : currentContext.Request["Address"].Trim(); }
        }

        private string RequestZipCode
        {
            get { return currentContext.Request["ZipCode"] == null ? string.Empty : currentContext.Request["ZipCode"].Trim(); }
        }

        private string RequestMsn
        {
            get { return currentContext.Request["MSN"] == null ? string.Empty : currentContext.Request["MSN"].Trim(); }
        }

        private string RequestBirthday
        {
            get { return currentContext.Request["Birthday"] == null ? string.Empty : currentContext.Request["Birthday"].Trim(); }
        }

        private string ContactUserS
        {
            get { return currentContext.Request["hid_ContactUserS"] == null ? string.Empty : currentContext.Request["hid_ContactUserS"].Trim(); }
        }

        private string Hobby
        {
            get { return currentContext.Request["Hobby"] == null ? string.Empty : currentContext.Request["Hobby"].Trim(); }
        }

        private string Responsible
        {
            get { return currentContext.Request["Responsible"] == null ? string.Empty : currentContext.Request["Responsible"].Trim(); }
        }

        /// <summary>
        /// 选择的会员编号
        /// </summary>
        private string YesMemberIDs
        {
            get { return currentContext.Request["YesMemberIDs"] == null ? string.Empty : currentContext.Request["YesMemberIDs"].Trim(); }
        }
        /// <summary>
        /// 未选择的会员编号
        /// </summary>
        private string NotMemberIDs
        {
            get { return currentContext.Request["NotMemberIDs"] == null ? string.Empty : currentContext.Request["NotMemberIDs"].Trim(); }
        }

        /// <summary>
        /// 选择的会员编号
        /// </summary>
        private string YesMainIDs
        {
            get { return currentContext.Request["YesMainIDs"] == null ? string.Empty : currentContext.Request["YesMainIDs"].Trim(); }
        }
        /// <summary>
        /// 未选择的会员编号
        /// </summary>
        private string NotMainIDs
        {
            get { return currentContext.Request["NotMainIDs"] == null ? string.Empty : currentContext.Request["NotMainIDs"].Trim(); }
        }

        /// <summary>
        /// 会员编号
        /// </summary>
        private string MemberID
        {
            get { return currentContext.Request["MemberID"] == null ? string.Empty : currentContext.Request["MemberID"].Trim(); }
        }
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            currentContext = context;
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            //添加用户信息
            if (context.Request.QueryString["add"] != null && context.Request.QueryString["add"] == "yes")
            {
                BitAuto.YanFa.Crm2009.Entities.ContactInfo model = new BitAuto.YanFa.Crm2009.Entities.ContactInfo();
                model.PID = Convert.ToInt32(RequestPid);
                model.CustID = RequestCustID;
                model.CName = RequestCName;
                model.EName = RequestEName;
                model.Sex = RequestSex;
                model.DepartMent = RequestDepartMent;
                model.OfficeTypeCode = int.Parse(RequestOfficeTypeCode.Trim());
                model.Title = RequestTitle;
                model.OfficeTel = RequestOfficeTel;
                model.Phone = RequestPhone;
                model.Email = RequestEmail;
                model.Fax = RequestFax;
                model.Remark = RequestReamrk;
                model.CreateTime = DateTime.Now;
                model.Status = 0;
                model.Address = RequestAddress;
                model.ZipCode = RequestZipCode;
                model.MSN = RequestMsn;
                model.Birthday = RequestBirthday;
                model.Hobby = Hobby;
                model.Responsible = Responsible;
                model.CreateUserID = BLL.Util.GetLoginUserID();

                int contactID = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.InsertContactInfo(model);
                if (contactID > 0)
                {
                    int userid = model.CreateUserID;
                    //插入CC添加联系人记录日志
                    BLL.ProjectTask_ReturnVisit.Instance.InsertCCAddCRMContractLogForRV(contactID, userid);

                    //




                    message = "Add:'yes'";
                    AddContactUserMapping(contactID);

                    //负责会员处理(按顺序执行)
                    YesSelectMemberHandle(contactID);
                    NotSelectMemberHandle(contactID);
                    YesSelectMainHandle(contactID);
                    NotSelectMainHandle(contactID);
                    //添加日志记录 BY cheng ON 2011-08-12
                    string content = string.Format("为【{0}(ID:{1})】添加联系人【{2}】成功。", BitAuto.YanFa.Crm2009.BLL.AllCusts.Instance.GetCustName(model.CustID), model.CustID, model.CName);
                    BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(BitAuto.YanFa.Crm2009.BLL.LogModule.ContactInfo, (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Add, content);
                    message = "Add:'yes',Edit:'no'";

                }
                else
                {
                    message = "Add:'no'";
                }
                context.Response.Write("{" + message + "}");
                context.Response.End();
            }
            //编辑用户信息
            else if (context.Request.QueryString["edit"] != null && context.Request.QueryString["edit"] == "yes")
            {
                BitAuto.YanFa.Crm2009.Entities.ContactInfo model = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfoByUserID(RequestID);
                if (model != null)
                {
                    model.PID = Convert.ToInt32(RequestPid);
                    model.CName = RequestCName;
                    model.EName = RequestEName;
                    model.Sex = RequestSex;
                    model.DepartMent = RequestDepartMent;
                    int typecode = 0;
                    Int32.TryParse(RequestOfficeTypeCode, out typecode);
                    model.OfficeTypeCode = typecode;
                    model.Title = RequestTitle;
                    model.OfficeTel = RequestOfficeTel;
                    model.Phone = RequestPhone;
                    model.Email = RequestEmail;
                    model.Fax = RequestFax;
                    model.Remark = RequestReamrk;
                    model.Address = RequestAddress;
                    model.ZipCode = RequestZipCode;
                    model.MSN = RequestMsn;
                    model.Birthday = RequestBirthday;

                    model.Hobby = Hobby;
                    model.Responsible = Responsible;
                    model.ModifyTime = DateTime.Now;
                    model.ModifyUserID = BLL.Util.GetLoginUserID();

                    //添加日志记录 BY cheng ON 2011-08-12
                    string content = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetUpdateContent(model, "{0}由【{1}】修改为【{2}】", '，');

                    if (BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.UpdateContactInfo(model) > 0)
                    {
                        message = "Add:'yes'";

                        EditContactUserMapping(model.ID);

                        //负责会员处理(按顺序执行)
                        YesSelectMemberHandle(RequestID);
                        NotSelectMemberHandle(RequestID);
                        YesSelectMainHandle(RequestID);
                        NotSelectMainHandle(RequestID);

                        try
                        {
                            content = string.Format("为【{0}(ID:{1})】修改联系人【{2}】：{3}。",
                                BitAuto.YanFa.Crm2009.BLL.AllCusts.Instance.GetCustName(model.CustID), model.CustID, model.CName, content);
                            BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(BitAuto.YanFa.Crm2009.BLL.LogModule.ContactInfo, (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Update, content);
                            message = "Add:'yes',Edit:'yes'";
                        }
                        catch (Exception ex)
                        {
                            message = ex.Message;
                        }
                    }
                    else
                    {
                        message = "Add:'no'";
                    }
                }
                else
                {
                    message = "Add:'no'";
                }
                context.Response.Write("{" + message + "}");
                context.Response.End();
            }
            //获取一条记录
            else if (context.Request.QueryString["show"] != null && context.Request.QueryString["show"] == "yes")
            {
                BitAuto.YanFa.Crm2009.Entities.ContactInfo model = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfoByUserID(RequestID);
                if (model != null)
                {
                    model.CreateUserTrueName = GetUserInfoTrueName(model.CreateUserID);
                    model.CreateTimeFormat = string.Format("{0:yyyy-MM-dd}", model.CreateTime);
                    model.ModifyUserTrueName = GetUserInfoTrueName(model.ModifyUserID);
                    model.ModifyTimeFormat = string.Format("{0:yyyy-MM-dd}", model.ModifyTime);


                    string strwhere = " ContactID = '" + model.ID.ToString() + "' and Status=0 ";
                    DataSet dstSet = BitAuto.YanFa.Crm2009.BLL.ContactUserMapping.Instance.GetList(strwhere);
                    for (int i = 0; i < dstSet.Tables[0].Rows.Count; i++)
                    {
                        model.ContactUserS += GetTrueName(dstSet.Tables[0].Rows[i]["UserID"].ToString()) + ",";
                        model.hid_ContactUserS += dstSet.Tables[0].Rows[i]["UserID"] + ",";
                    }
                    result = JavaScriptConvert.SerializeObject(model);
                    message = "yes";
                }
                else
                {
                    success = false;
                    message = "no";
                }
                AJAXHelper.WrapJsonResponse(success, result, message);
            }
            //删除一条记录
            else if (context.Request["action"] != null && context.Request["action"] == "DeleteContact")
            {
                BitAuto.YanFa.Crm2009.Entities.ContactInfo contact = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfoByUserID(RequestID);
                if (contact != null)
                {
                    //删除CC添加联系人记录日志
                    BLL.ProjectTask_ReturnVisit.Instance.DeleteCCAddCRMContractLogForRV(RequestID);

                    //


                    BitAuto.YanFa.Crm2009.BLL.ContactUserMapping.Instance.DeleteByContactID(contact.ID);
                    //删除会员联系人关联关系
                    BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.DeleteByContactID(contact.ID);
                    if (BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.Delete(RequestID) > 0)
                    {
                        message = "yes";//成功
                        //添加日志记录 BY cheng ON 2011-08-12
                        string content = string.Format("为【{0}(ID:{1})】删除联系人【{2}】。", BitAuto.YanFa.Crm2009.BLL.AllCusts.Instance.GetCustName(contact.CustID), contact.CustID, contact.CName);
                        BitAuto.YanFa.SysRightManager.Common.LogInfo.Instance.InsertLog(BitAuto.YanFa.Crm2009.BLL.LogModule.ContactInfo, (int)BitAuto.YanFa.SysRightManager.Common.LogInfo.ActionType.Delete, content);
                    }
                    else
                    {
                        success = false;
                        message = "no";//失败
                    }
                }

                AJAXHelper.WrapJsonResponse(success, result, message);
            }
            //通过CustID获取获取直接上级
            else if ((context.Request["action"] + "").Trim() == "DropDownListPID")
            {
                StringBuilder sb = new StringBuilder();
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfoPID(RequestCustID);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        sb.Append("{'ID':'" + dr["ID"] + "','Name':'" + dr["CName"] + "'},");
                    }
                    message = sb.ToString().TrimEnd(",".ToCharArray());
                }
                else
                {
                    message = sb.ToString();
                }
                context.Response.Write("[" + message + "]");
                context.Response.End();
            }
            //通过CustID获取获取直接上级
            else if ((context.Request["action"] + "").Trim() == "BindContactDepartment")
            {
                string strWhere = "";
                string filedOrder = "";
                StringBuilder sb = new StringBuilder();
                if ((context.Request["type"] + "").Trim() == "friend")
                {
                    FriendInfo model = BitAuto.YanFa.Crm2009.BLL.FriendInfo.Instance.GetFriendInfo(context.Request["CustID"]);

                    if (context.Request["CustID"] != "0")
                    {
                        if (model != null)
                        {
                            strWhere = "TypeID='" + model.FriendCategoryID + "'";
                            filedOrder = " sort asc";

                            if (model.FriendCategoryID.Trim() == "")
                            {
                                sb.Append("{'ID':'1','Name':'error'},");
                            }
                        }
                        else
                        {
                            strWhere = "lb='0'";
                        }
                    }
                    else
                    {
                        strWhere = "lb='0'";
                    }
                }
                else if ((context.Request["type"] + "").Trim() == "customer")
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo model = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(context.Request["CustID"]);
                    if (context.Request["CustID"] != "0")
                    {
                        if (model != null)
                        {
                            strWhere = "TypeID='" + model.TypeID + "'";
                            filedOrder = " sort asc";

                            if (model.TypeID.Trim() == "")
                            {
                                sb.Append("{'ID':'0','Name':'error'},");
                            }
                        }
                        else
                        {
                            strWhere = "lb='1'";
                        }
                    }
                    else
                    {
                        strWhere = "lb='1'";
                    }
                }

                DataSet dt = BitAuto.YanFa.Crm2009.BLL.ContactUserDepartment.Instance.GetList(-1, strWhere, filedOrder);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        sb.Append("{'ID':'" + dr["DepartmentID"] + "','Name':'" + dr["DepartmentName"] + "'},");
                    }
                    message = sb.ToString().TrimEnd(",".ToCharArray());
                }
                else
                {
                    message = sb.ToString();
                }
                context.Response.Write("[" + message + "]");
                context.Response.End();

            }
            else if ((context.Request["showContactInfo"] + "").Trim() == "yes")
            {
                StringBuilder sb = new StringBuilder();
                QueryContactInfo queryContactInfo = new QueryContactInfo();
                queryContactInfo.CustID = RequestCustID;
                int o;
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfo(queryContactInfo, "", 1, 10000, out o);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string tel = "";
                        if (dr["Phone"] != DBNull.Value)
                        {
                            tel += dr["Phone"].ToString().Trim() + ",";
                        }
                        if (dr["OfficeTel"] != DBNull.Value)
                        {
                            tel += dr["OfficeTel"].ToString().Trim() + ",";
                        }
                        sb.Append("{'ID':'" + dr["ID"] + "','Name':'" + dr["CName"] + "','Tel':'" + tel.Trim(',') + "'},");
                    }
                    message = sb.ToString().TrimEnd(",".ToCharArray());
                }
                else
                {
                    message = sb.ToString();
                }
                context.Response.Write("[" + message + "]");
                context.Response.End();
            }
            else if ((context.Request["showContactInfobycustid"] + "").Trim() == "yes")
            {
                StringBuilder sb = new StringBuilder();
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.ContactInfo.Instance.GetContactInfoByCustID(RequestCustID);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string tel = "";
                        if (dr["Phone"] != DBNull.Value)
                        {
                            tel += dr["Phone"].ToString().Trim() + ",";
                        }
                        if (dr["OfficeTel"] != DBNull.Value)
                        {
                            tel += dr["OfficeTel"].ToString().Trim() + ",";
                        }
                        sb.Append("{'ID':'" + dr["ID"] + "','Name':'" + dr["CName"] + "','Tel':'" + tel.Trim(',') + "','DepartMent':'" + dr["DepartMent"] + "'},");
                    }
                    message = sb.ToString().TrimEnd(",".ToCharArray());
                }
                else
                {
                    message = sb.ToString();
                }
                context.Response.Write("[" + message + "]");
                context.Response.End();
            }
            else if (context.Request["action"].Trim() == "getmember")
            {
                message = GetMappingMember();
                context.Response.Write(message);
                context.Response.End();
            }
            else if (context.Request["action"].Trim() == "ishasmanager")
            {
                message = GetManageContactInfo();
                context.Response.Write(message);
                context.Response.End();
            }
            else
            {
                success = false;
                message = "request error";
                AJAXHelper.WrapJsonResponse(success, result, message);
            }
        }

        /// <summary>
        /// 获取关联的会员
        /// </summary>
        /// <returns>string</returns>
        private string GetMappingMember()
        {
            string strResult = string.Empty;
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.GetList("MCM.ContactID=" + RequestID);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    strResult += "{'MemberID':'" + row["MemberID"] + "','IsMain':'" + row["IsMain"] + "'},";
                }
                strResult = strResult.TrimEnd(',');
            }
            strResult = "[" + strResult + "]";
            return strResult;
        }

        /// <summary>
        /// 获取会员的主负责人
        /// </summary>
        /// <returns></returns>
        private string GetManageContactInfo()
        {
            string strResult = string.Empty;
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.GetList("MCM.MemberID='" + MemberID + "' AND MCM.IsMain = 1");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    strResult += "{'ContactID':'" + row["ContactID"] + "','CName':'" + row["CName"] + "'},";
                }
                strResult = strResult.TrimEnd(',');
            }
            else
            {
                strResult += "{'ContactID':'','CName':''}";
            }
            return strResult;
        }

        /// <summary>
        /// 编辑联系人和人员关联
        /// </summary>
        /// <param name="contactid"></param>
        private void EditContactUserMapping(int contactid)
        {
            if (!string.IsNullOrEmpty(ContactUserS))
            {
                BitAuto.YanFa.Crm2009.BLL.ContactUserMapping.Instance.DeleteByContactID(contactid);

                string[] contactUserSArray = ContactUserS.Split(',');
                foreach (string str in contactUserSArray)
                {
                    if (str != "")
                    {
                        ContactUserMapping dstCum = new ContactUserMapping();
                        dstCum.ContactID = contactid;
                        try
                        {
                            dstCum.UserID = Convert.ToInt32(str);
                        }
                        catch
                        {
                            continue;
                        }
                        dstCum.Status = 0;
                        dstCum.CreateTime = DateTime.Now;
                        BitAuto.YanFa.Crm2009.BLL.ContactUserMapping.Instance.Add(dstCum);
                    }
                }
            }
        }

        /// <summary>
        /// 添加联系人和人员关联
        /// </summary>
        /// <param name="contactid"></param>
        private void AddContactUserMapping(int contactid)
        {
            if (!string.IsNullOrEmpty(ContactUserS))
            {
                string[] contactUserSArray = ContactUserS.Split(',');
                foreach (string str in contactUserSArray)
                {
                    if (str != "")
                    {
                        ContactUserMapping dstCum = new ContactUserMapping();
                        dstCum.ContactID = contactid;
                        try
                        {
                            dstCum.UserID = Convert.ToInt32(str);
                        }
                        catch
                        {
                            continue;
                        }
                        dstCum.Status = 0;
                        dstCum.CreateTime = DateTime.Now;
                        BitAuto.YanFa.Crm2009.BLL.ContactUserMapping.Instance.Add(dstCum);
                    }
                }
            }
        }

        private string GetTrueName(string userid)
        {
            string trueName = "";
            //查询
            int totalCount = 0;
            QueryCustUserMapping qcum = new QueryCustUserMapping();
            qcum.UserID = int.Parse(userid);
            DataTable table = BitAuto.YanFa.Crm2009.BLL.CustUserMapping.Instance.GetCustUserMapping(qcum, "CreateTime DESC", 1, 10, out totalCount);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                trueName = table.Rows[0]["TrueName"].ToString();
            }
            return trueName;
        }

        private string GetUserInfoTrueName(int userid)
        {
            string trueName = "";
            //查询
            trueName = BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.GetTrueNameByUserID(userid);
            return trueName;
        }

        /// <summary>
        /// 选择的会员处理
        /// </summary>
        private void YesSelectMemberHandle(int contactid)
        {
            if (!string.IsNullOrEmpty(YesMemberIDs))
            {
                string[] memberids = YesMemberIDs.Split(',');
                foreach (string memberid in memberids)
                {
                    MemberContactMapping model = BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.GetModel(memberid, contactid);
                    if (model == null)
                    {
                        model = new MemberContactMapping();
                        model.MemberID = new Guid(memberid);
                        model.ContactID = contactid;
                        model.IsMain = 0;
                        model.CreateTime = DateTime.Now;
                        BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.AddMemberContactMapping(model);
                    }
                }
            }
        }

        /// <summary>
        /// 未选择会员处理
        /// </summary>
        private void NotSelectMemberHandle(int contactid)
        {
            if (!string.IsNullOrEmpty(NotMemberIDs))
            {
                string[] memberids = NotMemberIDs.Split(',');
                foreach (string memberid in memberids)
                {
                    MemberContactMapping model = BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.GetModel(memberid, contactid);
                    if (model != null)
                    {
                        BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.DeleteMemberContactMapping(model.RecID);
                    }
                }
            }
        }

        /// <summary>
        /// 选择主要负责人的会员处理
        /// </summary>
        private void YesSelectMainHandle(int contactid)
        {
            if (!string.IsNullOrEmpty(YesMainIDs))
            {
                string[] memberids = YesMainIDs.Split(',');
                foreach (string memberid in memberids)
                {
                    //取消原有的负责人
                    DataTable dt = BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.GetList("MCM.MemberID='" + memberid + "' AND MCM.IsMain = 1");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        MemberContactMapping mapping = BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.GetModel(Convert.ToInt32(dt.Rows[0]["RecID"]));
                        if (mapping != null)
                        {
                            mapping.IsMain = 0;
                            BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.UpdateMemberContactMapping(mapping);
                        }
                    }
                    //设置新的负责人
                    MemberContactMapping model = BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.GetModel(memberid, contactid);
                    if (model != null)
                    {
                        model.IsMain = 1;
                        BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.UpdateMemberContactMapping(model);
                    }
                }
            }
        }

        /// <summary>
        /// 未选择主要负责人的会员处理
        /// </summary>
        private void NotSelectMainHandle(int contactid)
        {
            if (!string.IsNullOrEmpty(NotMainIDs))
            {
                string[] memberids = NotMainIDs.Split(',');
                foreach (string memberid in memberids)
                {
                    MemberContactMapping model = BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.GetModel(memberid, contactid);
                    if (model != null)
                    {
                        model.IsMain = 0;
                        BitAuto.YanFa.Crm2009.BLL.MemberContactMapping.Instance.UpdateMemberContactMapping(model);
                    }
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