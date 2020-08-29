using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers
{
    public partial class CustBasicInfoPop : PageBase
    {
        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }
        //---------url中的参数----------------------------
        protected string CRMCustID
        {
            get
            {
                if (Request["CRMCustID"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CRMCustID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected string RequestCustType
        {
            get
            {
                if (Request["CustType"] != null)
                {
                    return HttpUtility.UrlDecode(Request["CustType"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected int CustType
        {
            get
            {
                if (RequestCustType == "个人" || RequestCustType == "4")
                {
                    return (int)CustTypeEnum.T01_个人;
                }
                else if (RequestCustType == "经销商" || RequestCustType == "3")
                {
                    return (int)CustTypeEnum.T02_经销商;
                }
                else
                {
                    return (int)CustTypeEnum.None;
                }
            }
        }
        protected string Tel
        {
            get
            {
                if (Request["Tel"] != null)
                {
                    return HttpUtility.UrlDecode(Request["Tel"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string GetCustName()
        {
            if (Request["CustName"] != null)
            {
                return HttpUtility.UrlDecode(Request["CustName"].ToString());
            }
            else
            {
                return string.Empty;
            }
        }
        private int GetSex()
        {
            string value = "";
            if (Request["Sex"] != null)
            {
                value = HttpUtility.UrlDecode(Request["Sex"].ToString());
            }
            int a = CommonFunction.ObjectToInteger(value, -1);
            if (a == 1 || a == 2)
            {
                return a;
            }
            else return -1;
        }
        //---------url中的参数----------------------------

        /// 界面上显示的姓名：优先级：url参数>数据库
        /// <summary>
        /// 界面上显示的姓名：优先级：url参数>数据库
        /// </summary>
        protected string CustName { get; set; }
        /// 界面上显示的性别：优先级：url参数>数据库
        /// <summary>
        /// 界面上显示的性别：优先级：url参数>数据库
        /// </summary>
        protected int Sex { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindMember();
                //从url中获取
                CustName = GetCustName();
                Sex = GetSex();
                //读取数据库
                string custID = BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(Tel);
                if (!string.IsNullOrEmpty(custID))
                {
                    var model = BLL.CustBasicInfo.Instance.GetCustBasicInfo(custID);
                    if (model != null)
                    {
                        //url中为空，从数据库中取值
                        if (string.IsNullOrEmpty(CustName))
                        {
                            CustName = model.CustName;
                        }
                        //url中为空，从数据库中取值
                        if (Sex == -1)
                        {
                            Sex = model.Sex.GetValueOrDefault(-1);
                        }
                    }
                }
            }
        }
        //绑定经销商下拉列表
        protected void bindMember()
        {
            if (CustType != (int)CustTypeEnum.T02_经销商 || CRMCustID == string.Empty)
            {
                return;
            }

            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.SelectByCustID(CRMCustID);
            popSelMemberName.DataSource = dt;
            popSelMemberName.DataTextField = "FullName";
            popSelMemberName.DataValueField = "MemberCode";
            popSelMemberName.DataBind();
        }
    }
}