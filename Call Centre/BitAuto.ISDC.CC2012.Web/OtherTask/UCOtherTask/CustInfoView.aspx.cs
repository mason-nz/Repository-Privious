using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.OtherTask.UCOtherTask
{
    public partial class CustInfoView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string CustID
        {
            get { return BLL.Util.GetCurrentRequestStr("CustID"); }
        }

        public string IsShowBtn
        {
            get { return BLL.Util.GetCurrentRequestStr("IsShowBtn"); }
        }
        public string CustType = "";
        public string OfficeTel = "";
        public string ContactName = "";
        public string FirstMemberCode = "";
        public string FirstMemberName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CustInfoShow();
            }
        }

        private void CustInfoShow()
        {
            BitAuto.YanFa.Crm2009.Entities.CustInfo ci = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(CustID);
            if (ci != null)
            {
                spanCustName.InnerText = ci.CustName;
                spanCustType.InnerText = BLL.Util.GetEnumOptText(typeof(BitAuto.YanFa.Crm2009.Entities.EnumCustomType), CommonFunction.ObjectToInteger(ci.TypeID));
                CustType = ci.TypeID;
                //客户主营品牌
                BitAuto.YanFa.Crm2009.Entities.QueryBrandInfo queryBrandInfo = new BitAuto.YanFa.Crm2009.Entities.QueryBrandInfo();
                queryBrandInfo.CustID = ci.CustID;
                int o;
                string s = "";
                DataTable dt = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetCustBrandInfo(queryBrandInfo, "", 1, 10000, out o);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        s += dt.Rows[i]["name"] + " ";
                    }
                }
                spanBrandName.InnerText = s;

                //客户地区
                string provinceCity = "";
                if (!string.IsNullOrEmpty(ci.ProvinceID))
                {
                    provinceCity += BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(ci.ProvinceID);
                }
                if (!string.IsNullOrEmpty(ci.CityID))
                {
                    provinceCity += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(ci.CityID);
                }
                if (!string.IsNullOrEmpty(ci.CountyID))
                {
                    provinceCity += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(ci.CountyID);
                }
                spanArea.InnerText = provinceCity;

                spanAddress.InnerText = ci.Address;
                spanContactName.InnerText = ci.contactName;
                //crm客户的电话
                spanOfficeTel.InnerText = ci.Officetel;
                OfficeTel = ci.Officetel;
                ContactName = ci.contactName;
                spanZipcode.InnerText = ci.zipcode;

                if (ci.Lock == 1)
                {
                    //spanLock.InnerText = "是";
                    spanLock.Style["display"] = "block";
                }
                else
                {
                    //spanLock.InnerText = "否";
                    spanLock.Style["display"] = "none";
                }
                if (ci.Status == 0)
                {
                    //spanStatus.InnerText = "在用";
                    spanStatus.Style["display"] = "none";
                }
                else
                {
                    //spanStatus.InnerText = "停用";
                    spanStatus.Style["display"] = "block";
                }

                List<BitAuto.YanFa.Crm2009.Entities.DMSMember> list = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(ci.CustID);
                rptMember.DataSource = list;
                rptMember.DataBind();

                if (list != null && list.Count > 0)
                {
                    FirstMemberCode = list[0].MemberCode;
                    FirstMemberName = list[0].Name;
                }
            }
            else
            {

                Response.Write(@"<script language='javascript'>javascript:alert('该Crm客户可能不存在或者已经删除！');
                                            try {
                                                  window.external.MethodScript('/browsercontrol/closepage');
                                                } catch (e) {
                                                    window.opener = null; window.open('', '_self'); window.close();
                                                };</script>");
            }
        }

        //用于生成显示在外部的上级公司的字符串，如果从数据库中可以查出
        public string GetSuperCompanyString(string superId)
        {
            int _superID;
            if (!int.TryParse(superId, out _superID))
            {
                return "";
            }
            DataTable dt = new DataTable();
            if (Cache["SuperVerdor"] != null)
            {
                dt = (DataTable)Cache["SuperVerdor"];
            }
            else
            {
                string strMsg = string.Empty;
                dt = WebService.CstMemberServiceHelper.Instance.GetSuperVendor(out strMsg);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Cache.Insert("SuperVerdor", dt, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
                }
            }
            string CSTFullName = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["DVTId"].ToString() == superId.ToString())
                    {
                        CSTFullName = row["Name"].ToString();
                    }
                }
            }
            else if (_superID == -2)
            {
                CSTFullName = "";
            }
            else
            {
                CSTFullName = "[" + superId.ToString() + "]";
            }
            return CSTFullName;
        }

        //获取CST客户的种类：2.4S店;3.专业公司;4.厂商;5.其它
        public string GetCstTypeString(string vendorClass)
        {
            string strResult = "";
            int _vendorClass;
            if (!int.TryParse(vendorClass, out _vendorClass))
            {
                return strResult;
            }

            switch (_vendorClass)
            {
                case 1:
                    strResult = "个人用户";
                    break;
                case 2:
                    strResult = "4S店";
                    break;
                case 3:
                    strResult = "经纪公司";
                    break;
                case 4:
                    strResult = "厂商";
                    break;
                case 5:
                    strResult = "其他";
                    break;
            }
            return strResult;
        }

        public string GetAreaStr(string provinceID, string cityID, string countyID)
        {
            string area = "";
            if (!string.IsNullOrEmpty(provinceID))
            {
                area += BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(provinceID);
            }
            if (!string.IsNullOrEmpty(cityID))
            {
                area += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(cityID);
            }
            if (!string.IsNullOrEmpty(countyID))
            {
                area += " " + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(countyID);
            }

            return area;
        }

        public string GetMemberTypeStr(string memberType)
        {
            string memberName = string.Empty;
            if (string.IsNullOrEmpty(memberType))
            {
                memberName = "";
            }
            else
            {
                memberName = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetMemberTypeText(memberType);
            }
            return memberName;
        }
    }
}