using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Web.Util;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using Newtonsoft.Json;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_Contact
{
    public class CC_Contact_Helper
    {
        #region Query Properties

        private string contactID;
        /// <summary>
        /// 联系人ID
        /// </summary>
        public string ContactID
        {
            get
            {
                if (contactID == null)
                {
                    contactID = HttpUtility.UrlDecode((Request["ContactID"] + "").Trim());
                }
                return contactID;
            }
        }

        private string contactPID;
        /// <summary>
        /// 联系人ID
        /// </summary>
        public string ContactPID
        {
            get
            {
                if (contactPID == null)
                {
                    contactPID = HttpUtility.UrlDecode((Request["PID"] + "").Trim());
                }
                return contactPID;
            }
        }

        private string tID;
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TID
        {
            get
            {
                if (tID == null)
                {
                    tID = HttpUtility.UrlDecode((Request["TID"] + "").Trim());
                }
                return tID;
            }
        }

        private string cName;
        /// <summary>
        /// 姓名
        /// </summary>
        public string CName
        {
            get
            {
                if (cName == null)
                {
                    cName = HttpUtility.UrlDecode((Request["CName"] + "").Trim());
                }
                return cName;
            }
        }

        private string eName;
        /// <summary>
        /// 英文名
        /// </summary>
        public string EName
        {
            get
            {
                if (eName == null)
                {
                    eName = HttpUtility.UrlDecode((Request["EName"] + "").Trim());
                }
                return eName;
            }
        }

        private string sex;
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex
        {
            get
            {
                if (sex == null)
                {
                    sex = HttpUtility.UrlDecode((Request["Sex"] + "").Trim());
                }
                return sex;
            }
        }

        private string departMent;
        /// <summary>
        /// 部门
        /// </summary>
        public string DepartMent
        {
            get
            {
                if (departMent == null)
                {
                    departMent = HttpUtility.UrlDecode((Request["DepartMent"] + "").Trim());
                }
                return departMent;
            }
        }

        private string officeTypeCode;
        /// <summary>
        /// 职级
        /// </summary>
        public string OfficeTypeCode
        {
            get
            {
                if (officeTypeCode == null)
                {
                    officeTypeCode = HttpUtility.UrlDecode((Request["OfficeTypeCode"] + "").Trim());
                }
                return officeTypeCode;
            }
        }

        private string title;
        /// <summary>
        /// 职务
        /// </summary>
        public string Title
        {
            get
            {
                if (title == null)
                {
                    title = HttpUtility.UrlDecode((Request["Title"] + "").Trim());
                }
                return title;
            }
        }

        private string officeTel;
        /// <summary>
        /// 办公室电话
        /// </summary>
        public string OfficeTel
        {
            get
            {
                if (officeTel == null)
                {
                    officeTel = HttpUtility.UrlDecode((Request["OfficeTel"] + "").Trim());
                }
                return officeTel;
            }
        }

        private string phone;
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone
        {
            get
            {
                if (phone == null)
                {
                    phone = HttpUtility.UrlDecode((Request["Phone"] + "").Trim());
                }
                return phone;
            }
        }

        private string email;
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            get
            {
                if (email == null)
                {
                    email = HttpUtility.UrlDecode((Request["Email"] + "").Trim());
                }
                return email;
            }
        }

        private string fax;
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax
        {
            get
            {
                if (fax == null)
                {
                    fax = HttpUtility.UrlDecode((Request["Fax"] + "").Trim());
                }
                return fax;
            }
        }

        private string reamrk;
        /// <summary>
        /// 备注
        /// </summary>
        public string Reamrk
        {
            get
            {
                if (reamrk == null)
                {
                    reamrk = HttpUtility.UrlDecode((Request["Reamrk"] + "").Trim());
                }
                return reamrk;
            }
        }


        private string address;
        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            get
            {
                if (address == null)
                {
                    address = HttpUtility.UrlDecode((Request["Address"] + "").Trim());
                }
                return address;
            }
        }

        private string zipCode;
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode
        {
            get
            {
                if (zipCode == null)
                {
                    zipCode = HttpUtility.UrlDecode((Request["ZipCode"] + "").Trim());
                }
                return zipCode;
            }
        }

        private string mSN;
        /// <summary>
        /// MSN、QQ
        /// </summary>
        public string MSN
        {
            get
            {
                if (mSN == null)
                {
                    mSN = HttpUtility.UrlDecode((Request["MSN"] + "").Trim());
                }
                return mSN;
            }
        }

        private string birthday;
        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday
        {
            get
            {
                if (birthday == null)
                {
                    birthday = HttpUtility.UrlDecode((Request["Birthday"] + "").Trim());
                }
                return birthday;
            }
        }
        private string hobby;
        /// <summary>
        /// 
        /// </summary>
        public string Hobby
        {
            get
            {
                if (hobby == null)
                {
                    hobby = HttpUtility.UrlDecode((Request["Hobby"] + "").Trim());
                }
                return hobby;
            }
        }
        private string responsible;
        /// <summary>
        /// 
        /// </summary>
        public string Responsible
        {
            get
            {
                if (responsible == null)
                {
                    responsible = HttpUtility.UrlDecode((Request["Responsible"] + "").Trim());
                }
                return responsible;
            }
        }

        private string custType;
        /// <summary>
        /// 
        /// </summary>
        public string CustType
        {
            get
            {
                if (custType == null)
                {
                    custType = HttpUtility.UrlDecode((Request["CustType"] + "").Trim());
                }
                return custType;
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

        #endregion

        private string MemberID
        {
            get { return HttpContext.Current.Request["MemberID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["MemberID"].ToString()); }
        }
        /// <summary>
        /// 选择的会员编号
        /// </summary>
        private string YesMemberIDs
        {
            get { return HttpContext.Current.Request["YesMemberIDs"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["YesMemberIDs"].Trim()); }
        }
        /// <summary>
        /// 未选择的会员编号
        /// </summary>
        private string NotMemberIDs
        {
            get { return HttpContext.Current.Request["NotMemberIDs"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["NotMemberIDs"].Trim()); }
        }

        /// <summary>
        /// 选择的会员编号
        /// </summary>
        private string YesMainIDs
        {
            get { return HttpContext.Current.Request["YesMainIDs"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["YesMainIDs"].Trim()); }
        }
        /// <summary>
        /// 未选择的会员编号
        /// </summary>
        private string NotMainIDs
        {
            get { return HttpContext.Current.Request["NotMainIDs"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["NotMainIDs"].Trim()); }
        }


        internal void AddContact()
        {
            Entities.ProjectTaskInfo task = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(this.TID);
            if (task == null) { throw new Exception("无法得到相应任务"); }

            //int i = -1;
            Entities.ProjectTask_Cust_Contact c = new Entities.ProjectTask_Cust_Contact();
            c.PTID = task.PTID;
            //c.OriginalContactID = 
            int _PID = -2;
            if (int.TryParse(this.ContactPID, out _PID))
            {

            }
            c.PID = _PID;
            c.CName = this.CName;
            c.EName = this.EName;
            c.Sex = this.Sex;
            if (this.CustType != "20010")
            {
                c.DepartMent = this.DepartMent;
            }
            c.OfficeTypeCode = int.Parse(this.OfficeTypeCode.Trim());
            c.Title = this.Title;
            c.OfficeTel = this.OfficeTel;
            c.Phone = this.Phone;
            c.Email = this.Email;
            c.Fax = this.Fax;
            c.Remarks = this.Reamrk;
            c.Address = this.Address;
            c.ZipCode = this.ZipCode;
            c.MSN = this.MSN;
            c.Birthday = this.Birthday;
            c.CreateTime = DateTime.Now;
            c.ModifyTime = DateTime.Now;
            c.CreateUserID = BLL.Util.GetLoginUserID();
            c.Status = 0;
            c.Hobby = this.Hobby;
            c.Responsible = this.Responsible;

            try
            {
                int contactID = BLL.ProjectTask_Cust_Contact.Instance.InsertContact(c);
                if (contactID > 0)
                {
                    //负责会员处理(按顺序执行)
                    YesSelectMemberHandle(contactID);
                    NotSelectMemberHandle(contactID);
                    YesSelectMainHandle(contactID);
                    NotSelectMainHandle(contactID);
                }
            }
            catch (Exception ex)
            {

            }

        }

        internal void EditContact()
        {
            int _ContranctID = -1;
            if (int.TryParse(this.ContactID, out _ContranctID))
            {

            }
            Entities.ProjectTask_Cust_Contact c = BLL.ProjectTask_Cust_Contact.Instance.GetContactInfo(_ContranctID);
            if (c == null) { throw new Exception("无法得到此联系人"); }

            int _PID = -2;
            if (int.TryParse(this.ContactPID, out _PID))
            {

            }
            c.PID = _PID;
            c.CName = this.CName;
            c.EName = this.EName;
            c.Sex = this.Sex;
            if (this.CustType != "20010")
            {
                c.DepartMent = this.DepartMent;
            }
            else
            {
                c.DepartMent = string.Empty;
            }
            //c.DepartMent = this.DepartMent;
            c.OfficeTypeCode = int.Parse(this.OfficeTypeCode.Trim());
            c.Title = this.Title;
            c.OfficeTel = this.OfficeTel;
            c.Phone = this.Phone;
            c.Email = this.Email;
            c.Fax = this.Fax;
            c.Remarks = this.Reamrk;
            c.Address = this.Address;
            c.ZipCode = this.ZipCode;
            c.MSN = this.MSN;
            c.Birthday = this.Birthday;
            c.ModifyTime = DateTime.Now;
            c.ModifyUserID = BLL.Util.GetLoginUserID();

            c.Hobby = this.Hobby;
            c.Responsible = this.Responsible;

            try
            {
                BLL.ProjectTask_Cust_Contact.Instance.UpdateContact(c);

                //负责会员处理(按顺序执行)
                YesSelectMemberHandle(_ContranctID);
                NotSelectMemberHandle(_ContranctID);
                YesSelectMainHandle(_ContranctID);
                NotSelectMainHandle(_ContranctID);
            }
            catch (Exception ex)
            { }
        }

        internal void DeleteContact()
        {
            int _ContranctID = -1;
            if (int.TryParse(this.ContactID, out _ContranctID))
            {

            }
            BLL.ProjectTask_Cust_Contact.Instance.DeleteContact(_ContranctID);
            //根据联系人编号删除关联
            BLL.ProjectTask_MemberContactMapping.Instance.DeleteByContactID(_ContranctID);
        }

        /// <summary>
        /// 获取会员的主负责人 lxw 12.12.11
        /// </summary>
        /// <returns></returns>
        internal string GetManageContactInfo()
        {
            string strResult = string.Empty;
            string strWhere = " CC_MCM.MemberID='" + MemberID + "' AND CC_MCM.IsMain = 1 ";
            //如果是0，则代表是新增联系人，不需要在判断负责会员是否被使用时除掉自己的ContactID
            if (ContactID != "0")
            {
                strWhere += " AND ContactID!=" + ContactID;
            }
            DataTable dt = BLL.ProjectTask_MemberContactMapping.Instance.GetList(strWhere);
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
        /// 获取关联的会员
        /// </summary>
        /// <returns>string</returns>
        internal string GetMappingMember()
        {
            string strResult = string.Empty;
            DataTable dt = BLL.ProjectTask_MemberContactMapping.Instance.GetList("CC_MCM.ContactID=" + ContactID);
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
        /// 绑定“直接上级联系”下拉框
        /// </summary>
        internal static void BindParentContactToSelectEle(System.Web.UI.HtmlControls.HtmlSelect select, string tid, int contactId)
        {
            select.Items.Clear();
            select.Items.Add(new System.Web.UI.WebControls.ListItem("请选择", "-1"));
            DataTable dt = BLL.ProjectTask_Cust_Contact.Instance.GetContactInfoExcept(tid, contactId);
            if (dt == null || dt.Rows.Count == 0) { return; }

            foreach (DataRow dr in dt.Rows)
            {
                select.Items.Add(new System.Web.UI.WebControls.ListItem(dr["CName"].ToString(), dr["ID"].ToString()));
            }
        }


        internal DataTable GetAllContactOfCust()
        {
            throw new NotImplementedException();
        }

        internal string BindContactDepartment()
        {
            string message = string.Empty;
            string strWhere = "";
            string filedOrder = "";
            int tid = -1;
            //StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(TID))
            {
                message = "{'ID':'0','Name':'error'}";
                return message;
            }
            else
            {
                Entities.ProjectTask_Cust model = new Entities.ProjectTask_Cust();
                model = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(TID);

                if (!string.IsNullOrEmpty(CustType))
                {
                    strWhere = " TypeID='" + Utils.StringHelper.SqlFilter(CustType) + "'";
                }

                //if (model != null && !string.IsNullOrEmpty(model.TypeID))
                //{
                //    //绑定部门时，若客户类别为：非4S，则都按照4S逻辑处理
                //    if (model.TypeID.Trim() != Convert.ToString((int)Crm2009.Entities.EnumCustomType.FourS))
                //    {
                //        strWhere = "TypeID='" + (int)Crm2009.Entities.EnumCustomType.FourS + "'";
                //    }
                //    else
                //    {
                //        strWhere = "TypeID='" + model.TypeID + "'";
                //    }

                //    filedOrder = " sort asc";
                //}
                //else
                //{
                //    message = "{'ID':'0','Name':'error'}";
                //    return message;
                //}

                DataSet dt = BitAuto.YanFa.Crm2009.BLL.ContactUserDepartment.Instance.GetList(-1, strWhere, filedOrder);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            message += "[";
                        }
                        message += ("{'ID':'" + dt.Tables[0].Rows[i]["DepartmentID"] + "','Name':'" + dt.Tables[0].Rows[i]["DepartmentName"] + "'},");
                        if (i == dt.Tables[0].Rows.Count - 1)
                        {
                            message = message.TrimEnd(',');
                            message += "]";
                        }
                    }
                }
                else
                {
                    message = "{'ID':'0','Name':'error'}";

                }
            }
            return message;
        }
        internal string ShowContact()
        {
            string message = string.Empty;
            Entities.ProjectTask_Cust_Contact model = new Entities.ProjectTask_Cust_Contact();
            model = BLL.ProjectTask_Cust_Contact.Instance.GetContactInfo(int.Parse(this.ContactID));
            if (model != null)
            {
                model.CreateUserTrueName = GetUserInfoTrueName(model.CreateUserID.ToString());
                model.CreateTimeFormat = string.Format("{0:yyyy-MM-dd}", model.CreateTime);
                model.ModifyUserTrueName = GetUserInfoTrueName(model.ModifyUserID.ToString());
                model.ModifyTimeFormat = string.Format("{0:yyyy-MM-dd}", model.ModifyTime);

                message = JavaScriptConvert.SerializeObject(model);
            }
            return message;
        }
        private string GetUserInfoTrueName(string userid)
        {
            string TrueName = "";
            //查询
            int totalCount = 0;
            BitAuto.YanFa.Crm2009.Entities.QueryUserInfo newUserInfo = new BitAuto.YanFa.Crm2009.Entities.QueryUserInfo();
            if (userid != "")
            {
                try
                {
                    newUserInfo.UserID = int.Parse(userid);
                    DataTable table = BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.GetUserInfo(newUserInfo, "", 1, 10, out totalCount);

                    //设置数据源
                    if (table != null && table.Rows.Count > 0)
                    {
                        TrueName = table.Rows[0]["TrueName"].ToString();
                    }
                }
                catch
                {
                }
            }
            return TrueName;
        }


        /// <summary>
        /// 选择的会员处理
        /// </summary>
        internal void YesSelectMemberHandle(int contactid)
        {
            if (!string.IsNullOrEmpty(YesMemberIDs))
            {
                string[] memberids = YesMemberIDs.Split(',');
                foreach (string memberid in memberids)
                {
                    Entities.ProjectTask_MemberContactMapping model = BLL.ProjectTask_MemberContactMapping.Instance.GetModel(memberid, contactid);
                    if (model == null)
                    {
                        model = new Entities.ProjectTask_MemberContactMapping();
                        model.MemberID = new Guid(memberid);
                        model.ContactID = contactid;
                        model.IsMain = 0;
                        model.CreateTime = DateTime.Now;
                        BLL.ProjectTask_MemberContactMapping.Instance.AddMemberContactMapping(model);
                    }
                }
            }
        }

        /// <summary>
        /// 未选择会员处理
        /// </summary>
        internal void NotSelectMemberHandle(int contactid)
        {
            if (!string.IsNullOrEmpty(NotMemberIDs))
            {
                string[] memberids = NotMemberIDs.Split(',');
                foreach (string memberid in memberids)
                {
                    Entities.ProjectTask_MemberContactMapping model = BLL.ProjectTask_MemberContactMapping.Instance.GetModel(memberid, contactid);
                    if (model != null)
                    {
                        BLL.ProjectTask_MemberContactMapping.Instance.DeleteMemberContactMapping(model.RecID);
                    }
                }
            }
        }

        /// <summary>
        /// 选择主要负责人的会员处理
        /// </summary>
        internal void YesSelectMainHandle(int contactid)
        {
            if (!string.IsNullOrEmpty(YesMainIDs))
            {
                string[] memberids = YesMainIDs.Split(',');
                foreach (string memberid in memberids)
                {
                    //取消原有的负责人
                    DataTable dt = BLL.ProjectTask_MemberContactMapping.Instance.GetList("CC_MCM.MemberID='" + memberid + "' AND CC_MCM.IsMain = 1");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Entities.ProjectTask_MemberContactMapping mapping = BLL.ProjectTask_MemberContactMapping.Instance.GetModel(Convert.ToInt32(dt.Rows[0]["RecID"]));
                        if (mapping != null)
                        {
                            mapping.IsMain = 0;
                            BLL.ProjectTask_MemberContactMapping.Instance.UpdateMemberContactMapping(mapping);
                        }
                    }
                    //设置新的负责人
                    Entities.ProjectTask_MemberContactMapping model = BLL.ProjectTask_MemberContactMapping.Instance.GetModel(memberid, contactid);
                    if (model != null)
                    {
                        model.IsMain = 1;
                        BLL.ProjectTask_MemberContactMapping.Instance.UpdateMemberContactMapping(model);
                    }
                }
            }
        }

        /// <summary>
        /// 未选择主要负责人的会员处理
        /// </summary>
        internal void NotSelectMainHandle(int contactid)
        {
            if (!string.IsNullOrEmpty(NotMainIDs))
            {
                string[] memberids = NotMainIDs.Split(',');
                foreach (string memberid in memberids)
                {
                    Entities.ProjectTask_MemberContactMapping model = BLL.ProjectTask_MemberContactMapping.Instance.GetModel(memberid, contactid);
                    if (model != null)
                    {
                        model.IsMain = 0;
                        BLL.ProjectTask_MemberContactMapping.Instance.UpdateMemberContactMapping(model);
                    }
                }
            }
        }
    }
}