using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.YanFa.DMSInterface;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.DetailV
{
    public partial class UCCstMemberFromCC : System.Web.UI.UserControl
    {
        #region internal


        public Entities.ProjectTask_CSTMember Member = null;
        public string ControlName = "";
        public string Lat = "", Lng = "";
        public bool FullNameHavHistory = false;

        #endregion
        public UCCstMemberFromCC()
        {
            this.ID = Guid.NewGuid().ToString().Replace("-", "_");
        }

        public UCCstMemberFromCC(Entities.ProjectTask_CSTMember member)
            : this()
        {
            this.Member = member;
            this.ControlName = "车商通会员(" + Member.FullName + ")";
        }
        protected string clientStatus = "";
        protected string clientFullName = "";
        protected string clientShortName = "";
        protected string clientCode = "";
        protected string clientType = "";
        protected string upCompanyName = "";
        protected string region = "";
        protected string detailAddr = "";
        protected string postCode = "";
        protected string contact = "";
        protected string depart = "";
        protected string position = "";
        protected string tele = "";
        protected string phone = "";
        protected string email = "";
        protected string syncTime = "";
        protected string UCount = "";
        protected string lastAddUbTime = "";
        protected string UbTotalAmount = "";
        protected string UbTotalExpend = "";
        protected string activeTime = "";
        protected string productOpenKey = "";
        protected string CustID = "";
        protected string TrafficInfo = "";
        protected string SyncStatus = "";
        protected string Status = "";
        protected string SyncStatusValue = "";
        protected string CreateTime = "";
        protected string BrandIdsName = "";
        protected string vendorClass = "";
        protected string CSTMemberID = "";
        protected string SyncTime = "";
        public string HasRight = "1";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetProductOpenKey();
                //如果是cc车商通会员
                if (this.Member != null)
                {
                    BindData();
                    HasRight = "0";
                }
            }
        }

        private void BindData()
        {
            //把对象信息存入页面变量中
            Entities.ProjectTask_CSTMember cstMember = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberModel(this.Member.ID);
            if (cstMember != null)
            {
                vendorClass = cstMember.VendorClass.ToString();
                if (cstMember.OriginalCSTRecID != null)
                {
                    BitAuto.YanFa.Crm2009.Entities.CstMember info = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCSTMember(cstMember.OriginalCSTRecID);
                    if (info != null)
                    {
                        SyncStatusValue = info.SyncStatus.ToString();
                        CSTMemberID = info.CstMemberID.ToString();
                    }

                    if (BLL.ProjectTask_CSTMember.Instance.IsOpenSuccessMember(this.Member.ID))
                    {
                        //是否有名称变更
                        DataTable dtFullName = BLL.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(cstMember.OriginalCSTRecID, 7, "FullName");
                        if (dtFullName.Rows.Count > 0)
                        {
                            FullNameHavHistory = true;
                        }
                    }
                }

                this.clientFullName = cstMember.FullName;
                this.clientShortName = cstMember.ShortName;
                this.clientCode = cstMember.VendorCode;
                this.clientType = GetCstTypeString((int)cstMember.VendorClass);
                this.upCompanyName = GetSuperCompanyString((int)cstMember.SuperId);
                this.region = GetRegionString(cstMember.ProvinceID, cstMember.CityID, cstMember.CountyID);
                this.detailAddr = cstMember.Address;
                this.postCode = cstMember.PostCode;
                this.TrafficInfo = cstMember.TrafficInfo;

                //主营品牌
                List<Entities.ProjectTask_CSTMember_Brand> listCstMember = BLL.ProjectTask_CSTMember_Brand.Instance.GetProjectTask_CSTMember_Brand(Member.ID);
                if (listCstMember != null)
                {
                    string BrandIds = "";
                    foreach (Entities.ProjectTask_CSTMember_Brand cstMemberBrand in listCstMember)
                    {
                        BrandIds += cstMemberBrand.BrandID + ",";
                    }
                    BrandIds = BrandIds.TrimEnd(',');
                    BrandIdsName = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetBrandNames(BrandIds);
                }

                //联系人
                Entities.ProjectTask_CSTLinkMan linkManInfo = BLL.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkManModel(cstMember.ID);
                if (linkManInfo != null)
                {
                    this.contact = linkManInfo.Name;
                    this.depart = linkManInfo.Department;
                    this.position = linkManInfo.Position;
                    this.phone = linkManInfo.Mobile;
                    this.email = linkManInfo.Email;
                }

                //车商币信息
                if (Member.OriginalCSTRecID != null)
                {
                    BitAuto.YanFa.Crm2009.Entities.CSTExpandInfo dstCSTExpandInfo = BitAuto.YanFa.Crm2009.BLL.CSTExpandInfo.Instance.GetModelByCSTRecID(Member.OriginalCSTRecID);
                    if (dstCSTExpandInfo != null)
                    {
                        if (dstCSTExpandInfo.UCount != Constant.INT_INVALID_VALUE)
                        {
                            this.UCount = dstCSTExpandInfo.UCount.ToString();
                        }
                        if (dstCSTExpandInfo.LastAddUbTime != Constant.DATE_INVALID_VALUE)
                        {
                            this.lastAddUbTime = dstCSTExpandInfo.LastAddUbTime.ToShortDateString();
                        }
                        if (dstCSTExpandInfo.UBTotalAmount != Constant.INT_INVALID_VALUE)
                        {
                            this.UbTotalAmount = dstCSTExpandInfo.UBTotalAmount.ToString();
                        }
                        if (dstCSTExpandInfo.UBTotalExpend != Constant.INT_INVALID_VALUE)
                        {
                            this.UbTotalExpend = dstCSTExpandInfo.UBTotalExpend.ToString();
                        }
                        if (dstCSTExpandInfo.ActiveTime != Constant.DATE_INVALID_VALUE)
                        {
                            this.activeTime = dstCSTExpandInfo.ActiveTime.ToShortDateString();
                        }
                    }
                }
            }
        }



        //用于生成显示在外部的上级公司的字符串，如果从数据库中可以查出
        private string GetSuperCompanyString(int superId)
        {
            DataTable dt = new DataTable();
            if (Cache["SuperVerdor"] != null)
            {
                dt = (DataTable)Cache["SuperVerdor"];
            }
            else
            {
                string strMsg = string.Empty;
                dt = WebService.CstMemberServiceHelper.Instance.GetSuperVendor(out strMsg);
                Cache.Insert("SuperVerdor", dt, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
            }
            string CSTFullName = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["DVTID"].ToString() == superId.ToString())
                    {
                        CSTFullName = row["Name"].ToString();
                    }
                }
            }
            else if (superId == -2)
            {
                CSTFullName = "";
            }
            else
            {
                CSTFullName = "[" + superId.ToString() + "]";
            }
            return CSTFullName;
        }

        private string GetCstStatusString(int statusNum)
        {
            string StatusString = "";
            switch (statusNum)
            {
                case 0:
                    StatusString = "禁用";
                    break;
                case 1:
                    StatusString = "启用";
                    break;
                case -1:
                    StatusString = "已删除";
                    break;
            }
            return StatusString;
        }

        //获取CST客户的种类：2.4S店;3.专业公司;4.厂商;5.其它
        private string GetCstTypeString(int vendorClass)
        {
            string strResult = "";
            switch (vendorClass)
            {
                case 1:
                    strResult = "个人用户";
                    break;
                case 2:
                    strResult = "4S店";
                    break;
                case 3:
                    strResult = "专业公司";
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

        //获取地区字符串
        private string GetRegionString(string provinceID, string cityID, string CountyID)
        {
            string regionStr = "";
            string provinceName = "";
            string cityName = "";
            string countyName = "";
            BitAuto.YanFa.Crm2009.Entities.AreaInfo provinceAreaInfo = new BitAuto.YanFa.Crm2009.Entities.AreaInfo();
            provinceAreaInfo = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaInfo(provinceID);
            if (provinceAreaInfo != null)
            {
                provinceName = provinceAreaInfo.AreaName;
            }
            else
            {
                provinceName = provinceID;
            }
            BitAuto.YanFa.Crm2009.Entities.AreaInfo cityAreaInfo = new BitAuto.YanFa.Crm2009.Entities.AreaInfo();
            cityAreaInfo = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaInfo(cityID);
            if (cityAreaInfo != null)
            {
                cityName = cityAreaInfo.AreaName;
            }
            else
            {
                cityName = cityID;
            }
            BitAuto.YanFa.Crm2009.Entities.AreaInfo countyAreaInfo = new BitAuto.YanFa.Crm2009.Entities.AreaInfo();
            countyAreaInfo = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaInfo(CountyID);
            if (countyAreaInfo != null)
            {
                countyName = countyAreaInfo.AreaName;
            }
            else
            {
                countyName = CountyID;
            }
            regionStr = provinceName + " " + cityName + " " + countyName;
            return regionStr;


        }

        //获取引用链接所需要的“Key”
        private void GetProductOpenKey()
        {
            this.productOpenKey = ConfigurationManager.AppSettings["CSTMemberServiceKey"].ToString();
        }

        /// <summary>
        /// 获取会员开通状态
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string GetSyncStatusDesc(string s)
        {
            string SyncStatus = "";
            int status = 0;
            if (!string.IsNullOrEmpty(s))
            {
                if (int.TryParse(s, out status))
                {
                    SyncStatus = BitAuto.YanFa.Crm2009.BLL.Util.GetEnumOptTextCSTMember(status);
                }
            }
            return SyncStatus;
        }

        public string CstMemberID()
        {
            return BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.SelectCstMemberIDByCSTRecID(Member.OriginalCSTRecID);
        }

        public string GetRejectedStr(string SyncStatusValue)
        {
            int recid = 0;
            if (SyncStatusValue == "170008")
            {
                string result = "";
                DataTable dstTable = new DataTable();
                dstTable = BitAuto.YanFa.Crm2009.BLL.CSTMemberSyncLog.Instance.GetSyncLog(Member.OriginalCSTRecID);
                for (int i = 0; i < dstTable.Rows.Count; i++)
                {
                    if (dstTable.Rows[i]["SyncStatus"].ToString() == "170008")
                    {
                        if (int.Parse(dstTable.Rows[i]["RecID"].ToString()) > recid)
                        {
                            result = "<br><div style= \"word-wrap:break-word;word-break:break-all;width:200px;\">(" + dstTable.Rows[i]["Description"].ToString() + ")</div>";
                            recid = int.Parse(dstTable.Rows[i]["RecID"].ToString());
                        }
                    }
                }
                return result;
            }
            else
            {
                return "";
            }
        }
    }
}