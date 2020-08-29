using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMSMember = BitAuto.YanFa.Crm2009.Entities.DMSMember;
namespace BitAuto.ISDC.CC2012.Web.ReturnVisit.CC_Contact
{
    public partial class Add : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性字段
        public string StrFillPage = "";
        public string Addurl = "";

        public string CustID
        {
            get { return Request["CustID"] + ""; }
        }

        public string PopupName
        {
            get { return Request["PopupName"] + ""; }
        }

        public string DepartmenDisplay = "block";
        public string TypeID = "";

        /// <summary>
        /// 来源（0：客户信息；1：客户联系人列表;3:访问记录添加）
        /// </summary>
        public string Source
        {
            get { return Request["Source"] == null ? "0" : Request["Source"].Trim(); }
        }

        private bool _isShowMember = false;
        /// <summary>
        /// 是否显示负责会员
        /// </summary>
        public bool IsShowMember
        {
            set { _isShowMember = value; }
            get { return _isShowMember; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region 判断部门是否显示功能 个人不显示部门
                BitAuto.YanFa.Crm2009.Entities.CustInfo model = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(Request["CustID"]);
                if (model != null)
                {
                    TypeID = model.TypeID;
                    if (model.TypeID == "20010")
                    {
                        DepartmenDisplay = "none";
                    }
                }
                #endregion

                //是否显示负责会员列表
                _isShowMember = CheckShowMember(CustID);

                FillPage();
            }
        }

        /// <summary>
        /// 检查客户类别，显示是否分配会员及负责会员
        /// </summary>
        /// <param name="custid">客户编号</param>
        /// <returns></returns>
        private bool CheckShowMember(string custid)
        {
            bool isShow = false;
            BitAuto.YanFa.Crm2009.Entities.CustInfo model = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(custid);
            if (model != null)
            {
                //判断客户类型：综合店、特许经销商、4S、展厅、集团显示
                if (model.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.SynthesizedShop).ToString()
                    || model.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence).ToString()
                    || model.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.FourS).ToString()
                    || model.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Showroom).ToString()
                    || model.TypeID == ((int)BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Bloc).ToString())
                {
                    isShow = true;
                }
            }
            //判断客户下面有开通的易湃会员显示
            List<DMSMember> list = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByCustID(custid);
            if (list.Count > 0 && isShow)
            {
                rtpMemberList.DataSource = list;
                rtpMemberList.DataBind();
            }
            else
            {
                isShow = false;
            }
            return isShow;
        }

        private void FillPage()
        {
            if (Request["ID"] != null && Request["ID"] != "0")
            {
                StrFillPage = "fillpage();";
                Addurl = "edit=yes&ID=" + Request["ID"];
            }
            else
            {
                Addurl = "add=yes";
            }
        }
    }
}