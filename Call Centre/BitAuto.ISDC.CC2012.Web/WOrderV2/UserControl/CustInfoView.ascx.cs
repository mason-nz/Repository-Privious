using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl
{
    public partial class CustInfoView : System.Web.UI.UserControl
    {
        private string custId;
        /// 传值：客户id
        /// <summary>
        /// 传值：客户id
        /// </summary>
        public string CustID
        {
            get
            {
                return custId;
            }
            set
            {
                custId = value;
                //数据查询
                dtCBInfo = BLL.WOrderInfo.Instance.GetCBInfoByPhone(custId, "");
            }
        }
        /// 返回：客户类型
        /// <summary>
        /// 返回：客户类型
        /// </summary>
        public CustTypeEnum CustType
        {
            get
            {
                if (!string.IsNullOrEmpty(CustID) && dtCBInfo != null && dtCBInfo.Rows.Count > 0)
                {
                    return (CustTypeEnum)CommonFunction.ObjectToInteger(dtCBInfo.Rows[0]["CustCategoryID"], (int)CustTypeEnum.T01_个人);
                }
                else return CustTypeEnum.None;
            }
        }
        private string telphone;
        public string Telphone
        {
            get { return telphone; }
            set { telphone = value; }
        }
        private bool canseetelimg = false;
        public bool CanSeeTelImg
        {
            get { return canseetelimg; }
            set { canseetelimg = value; }
        }
        private bool needlinktocustinfo = true;
        public bool NeedLinkToCustinfo
        {
            get { return needlinktocustinfo; }
            set { needlinktocustinfo = value; }
        }
        //TODO:CodeReview 2016-08-10 控件名称规范(首字母应大写)
        public DataTable dtCBInfo = null;
        public string CustName = string.Empty;
        public string Sex = string.Empty;
        public string Tels = string.Empty;
        public string PlaceStr = string.Empty;
        public string CustCategoryStr = string.Empty;
        public string MemberName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(CustID) && dtCBInfo != null && dtCBInfo.Rows.Count > 0)
                {
                    if (NeedLinkToCustinfo)
                    {
                        CustName = "<a href='/TaskManager/CustInformation.aspx?CustID=" + CustID + "' target='_blank' class='linkBlue'>"
                        + CommonFunction.ObjectToString(dtCBInfo.Rows[0]["CustName"]) + "</a>";
                    }
                    else
                    {
                        CustName = CommonFunction.ObjectToString(dtCBInfo.Rows[0]["CustName"]);
                    }
                    Sex = CommonFunction.ObjectToInteger(dtCBInfo.Rows[0]["Sex"], 1) == 2 ? "女士" : "先生";
                    if (!string.IsNullOrEmpty(Telphone))
                    {
                        Tels = AddTelImgToTels(Telphone);
                    }
                    else
                    {
                        Tels = AddTelImgToTels(CommonFunction.ObjectToString(dtCBInfo.Rows[0]["AllTel"]));
                    }

                    PlaceStr = GetAreaName(CommonFunction.ObjectToInteger(dtCBInfo.Rows[0]["ProvinceID"]),
                        CommonFunction.ObjectToInteger(dtCBInfo.Rows[0]["CityID"]),
                        CommonFunction.ObjectToInteger(dtCBInfo.Rows[0]["CountyID"]));
                    //设置客户类别
                    SetCustType(dtCBInfo);
                }
            }
        }

        ///  给电话号码增加外呼图标
        /// </summary>
        /// <param name="tels"></param>
        /// <returns></returns>
        private string AddTelImgToTels(string tels)
        {
            string backVal = "";
            if (canseetelimg && !string.IsNullOrEmpty(tels))
            {
                tels = tels.Trim().Replace("，", ",");
                string[] telArr = tels.Split(',');
                foreach (string tel in telArr)
                {
                    backVal += tel + "<img style='margin:0px 15px 0px 5px; cursor:pointer;' name='TelImg' src='/images/phone.gif' alt='" + tel + "' tel='" + tel + "' />";
                }
            }
            else
            {
                backVal = tels;
            }
            return backVal;
        }

        /// 设置客户类别
        /// <summary>
        /// 设置客户类别
        /// </summary>
        /// <param name="dtCBInfo"></param>
        private void SetCustType(DataTable dtCBInfo)
        {
            int custcategoryid = CommonFunction.ObjectToInteger(dtCBInfo.Rows[0]["CustCategoryID"], (int)CustTypeEnum.T01_个人);
            if (custcategoryid == (int)CustTypeEnum.T01_个人)
            {
                CustCategoryStr = "个人";
                MemberNameTd.InnerText = "";
                MemberName = "";
            }
            else
            {
                CustCategoryStr = "经销商";
                MemberNameTd.InnerText = "所属经销商：";
                string membername = CommonFunction.ObjectToString(dtCBInfo.Rows[0]["MemberName"]);
                string membercode = CommonFunction.ObjectToString(dtCBInfo.Rows[0]["MemberCode"]);
                if (!string.IsNullOrEmpty(membername))
                {
                    if (!string.IsNullOrEmpty(membercode))
                    {
                        YanFa.Crm2009.Entities.DMSMember dmsMember = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByMemberCode(membercode);
                        if (dmsMember != null)
                        {
                            MemberName = "<a href='/CustCheck/CrmCustSearch/CustDetail.aspx?CustID=" + dmsMember.CustID + "' target='_blank'>"
                                         + dmsMember.Name + "</a>";
                        }
                        else
                        {
                            //查不到的情况
                            MemberName = membername;
                        }
                    }
                    else
                    {
                        //没有id的情况
                        MemberName = membername;
                    }
                }
                else
                {
                    //没有名称的情况
                    MemberName = "";
                }
            }
        }
        /// 设置省市区县
        /// <summary>
        /// 设置省市区县
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public string GetAreaName(int province, int city, int county)
        {
            string pname = DictionaryDataCache.Instance.AreaInfo_Province.ContainsKey(province.ToString()) ?
                DictionaryDataCache.Instance.AreaInfo_Province[province.ToString()]["AreaName"].ToString() : "";
            string cname = DictionaryDataCache.Instance.AreaInfo_City.ContainsKey(city.ToString()) ?
                 DictionaryDataCache.Instance.AreaInfo_City[city.ToString()]["AreaName"].ToString() : "";
            string uname = DictionaryDataCache.Instance.AreaInfo_County.ContainsKey(county.ToString()) ?
                 DictionaryDataCache.Instance.AreaInfo_County[county.ToString()]["AreaName"].ToString() : "";

            string str = pname;
            if (!string.IsNullOrEmpty(cname))
            {
                str += "-" + cname;
            }
            if (!string.IsNullOrEmpty(uname))
            {
                str += "-" + uname;
            }
            return str;
        }
    }
}