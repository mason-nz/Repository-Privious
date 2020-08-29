using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailV
{
    public partial class TaskCallRecordBind : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region  属性定义
        public string RequestTID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("TID"); }
        }
        private int crid = -1;
        /// <summary>
        /// 录音ID
        /// </summary>
        public int RequestCRID
        {
            get { return crid; }
            set { crid = value; }
        }
        /// <summary>
        /// 录音SessionID唯一标识
        /// </summary>
        public string RequestSessionID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("SessionID"); }
        }
        ///// <summary>
        ///// 录音唯一标识SessionID
        ///// </summary>
        //public string RequestSessionID
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("SessionID"); }
        //}
        public string DataSource = "";//数据来源，1-Excel新增，2-CRM库
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(RequestTID))
                {
                    RequestCRID = BLL.Util.GetCurrentRequestFormInt("CRID");
                    if (RequestCRID < 0)
                    {
                        Entities.CallRecordInfo model = BLL.CallRecordInfo.Instance.GetCallRecordInfoByTaskID(RequestTID);
                        if (model != null)
                        {
                            RequestCRID = (int)model.RecID;
                        }
                    }
                    BindData();
                }
            }
        }

        private void BindData()
        {
            DataTable dtMember = new DataTable();
            Entities.ProjectTaskInfo model = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(RequestTID);
            if (model != null)
            {
                DataSource = model.Source.ToString();
                if (model.Source == 1)//Excel导入客户
                {
                    Entities.ProjectTaskInfo cccustModel = BLL.ProjectTaskInfo.Instance.GetProjectTaskInfo(RequestTID);
                    if (cccustModel != null)
                    {
                        txtCustName.Value = cccustModel.CustName;
                        List<Entities.ProjectTask_DMSMember> ccmemberList = new List<Entities.ProjectTask_DMSMember>();
                        ccmemberList = BitAuto.ISDC.CC2012.BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMemberByTID(RequestTID);

                        //*add by qizhiqiang 取新建车商通会员信息 2012-4-20
                        List<Entities.ProjectTask_CSTMember> cc_cstmemberList = new List<Entities.ProjectTask_CSTMember>();
                        cc_cstmemberList = BitAuto.ISDC.CC2012.BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(RequestTID);
                        //*

                        //*updateby qizhiqiang 2012-4-20
                        BindMemberDDL(ccmemberList, cc_cstmemberList);
                        //*

                        hidCustID.Value = model.RelationID;
                    }
                }
                else if (model.Source == 2)//CRM库客户
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo custModel = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(model.RelationID);
                    if (custModel != null)
                    {
                        txtCustName.Value = custModel.CustName;
                        spanCustID.InnerHtml = custModel.CustID;
                        hidCustID.Value = custModel.CustID;

                        List<Entities.ProjectTask_DMSMember> ccmemberList = new List<Entities.ProjectTask_DMSMember>();
                        ccmemberList = BitAuto.ISDC.CC2012.BLL.ProjectTask_DMSMember.Instance.GetProjectTask_DMSMemberByTID(RequestTID);

                        //*add byqizhiqiang 2012-4-20
                        List<Entities.ProjectTask_CSTMember> cc_cstmemberList = new List<Entities.ProjectTask_CSTMember>();
                        cc_cstmemberList = BitAuto.ISDC.CC2012.BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(RequestTID);
                        //*

                        //*updateby qizhiqiang 2012-4-20
                        BindMemberDDL(ccmemberList, cc_cstmemberList);
                        //*
                    }
                }
            }
        }

        private void BindMemberDDL(List<Entities.ProjectTask_DMSMember> ccmemberList, List<Entities.ProjectTask_CSTMember> cc_cstmemberList)
        {
            ddlMember.DataTextField = "Name";
            ddlMember.DataValueField = "BindID";
            DataTable dt = new DataTable();
            DataTable dtcyt = BLL.Util.ListToDataTable(ccmemberList);
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("BindID", typeof(string));
            if (dtcyt != null)
            {
                foreach (DataRow dr in dtcyt.Rows)
                {
                    DataRow drlist = dt.NewRow();
                    string OriginalDMSMemberID = dr["OriginalDMSMemberID"].ToString().Trim();
                    string membercode = string.Empty;
                    drlist["Name"] = dr["name"].ToString();
                    if (!string.IsNullOrEmpty(OriginalDMSMemberID))
                    {
                        BitAuto.YanFa.Crm2009.Entities.DMSMember memberModel = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(new Guid(OriginalDMSMemberID));
                        if (memberModel != null)
                        {
                            membercode = memberModel.MemberCode;
                            //1为车易通
                            drlist["BindID"] = memberModel.ID + "|" + membercode + "|" + "1";
                        }
                        else
                        {
                            drlist["BindID"] = dr["MemberID"] + "|" + "|" + "1";
                        }
                    }
                    else
                    {
                        drlist["BindID"] = dr["MemberID"] + "|" + "|" + "1";
                    }
                    dt.Rows.Add(drlist);
                }
            }
            if (cc_cstmemberList != null && cc_cstmemberList.Count > 0)
            {
                DataTable dtcst = BLL.Util.ListToDataTable(cc_cstmemberList);
                if (dtcst != null)
                {
                    foreach (DataRow dr in dtcst.Rows)
                    {
                        DataRow drlist = dt.NewRow();
                        drlist["Name"] = dr["FULLName"].ToString();
                        string OriginalCstRecID = dr["originalcstRecid"].ToString().Trim();
                        if (!string.IsNullOrEmpty(OriginalCstRecID) && OriginalCstRecID.Length > 0)
                        {
                            BitAuto.YanFa.Crm2009.Entities.CstMember cstmemberModel = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCstMemberModel(OriginalCstRecID);
                            //2为车商通
                            if (cstmemberModel != null)
                            {
                                string membercode = string.Empty;
                                if (cstmemberModel.CstMemberID.ToString() != "0" && cstmemberModel.CstMemberID.ToString() != "-2")
                                {
                                    membercode = cstmemberModel.CstMemberID.ToString();
                                }

                                drlist["BindID"] = cstmemberModel.CSTRecID + "|" + membercode + "|" + "2";
                            }
                            else
                            {
                                drlist["BindID"] = dr["ID"] + "|" + "|" + "2";
                            }
                        }
                        else
                        {
                            drlist["BindID"] = dr["ID"] + "|" + "|" + "2";
                        }
                        dt.Rows.Add(drlist);
                    }
                }
            }
            ddlMember.DataSource = dt;
            ddlMember.DataBind();
            ddlMember.Items.Insert(0, new ListItem("选择会员", "-1"));
        }

        //private void BindMemberDDL(List<Crm2009.Entities.DMSMember> memberList)
        //{
        //    ddlMember.DataTextField = "Name";
        //    ddlMember.DataValueField = "BindID";
        //    DataTable dt = BLL.Util.ListToDataTable(memberList);
        //    if (dt != null)
        //    {
        //        dt.Columns.Add("BindID", typeof(string));
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            dr["BindID"] = dr["ID"] + "|" + dr["MemberCode"] + "|";
        //        }
        //    }
        //    ddlMember.DataSource = dt;
        //    ddlMember.DataBind();
        //    ddlMember.Items.Insert(0, new ListItem("选择会员", "-1"));
        //}
    }
}