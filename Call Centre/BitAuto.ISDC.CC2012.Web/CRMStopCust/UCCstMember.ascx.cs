using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils;
using BitAuto.YanFa.DMSInterface;
using System.Configuration;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public partial class UCCstMember : System.Web.UI.UserControl
    {
        #region internal

        public BitAuto.YanFa.Crm2009.Entities.CstMember Member = null;
        public string ControlName = "";
        public string Lat = "", Lng = "";

        #endregion

        public UCCstMember()
        {
            this.ID = Guid.NewGuid().ToString().Replace("-", "_");
        }

        public UCCstMember(BitAuto.YanFa.Crm2009.Entities.CstMember member)
            : this()
        {
            this.Member = member;
            this.ControlName = "车商通会员(" + Member.FullName + ")";
            if (member.Status == -1)
            {
                this.ControlName += " -已停用删除";
            }
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
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                //如果是crm车商通会员
                if (this.Member != null)
                {
                    BindData();
                    HasRight = "0";
                }
            }
        }

        private void BindData()
        {
            //获取查询对象信息
            BitAuto.YanFa.Crm2009.Entities.QueryCstMember query = new BitAuto.YanFa.Crm2009.Entities.QueryCstMember();
            try
            {
                query.CSTRecID = Member.CSTRecID;
            }
            catch
            {
                query.CSTRecID = "";
            }

            //把对象信息存入页面变量中
            DataTable dtCstMember = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetAllTableByRecID(query);
            if (dtCstMember.Rows.Count == 0)
            {
                ScriptHelper.ShowCustomScript(this.Page, "$.jAlert('您要查看的会员不存在',function(){window.close();});");
            }
            else
            {
                if (dtCstMember.Rows[0]["FullName"] == DBNull.Value || dtCstMember.Rows[0]["FullName"].ToString() == "")
                {
                    //如果未同步过，就同步后再次车商通会员信息
                    BitAuto.YanFa.Crm2009.Entities.CstMember liCstMember = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetCstMemberModel(Member.CSTRecID);
                    if (liCstMember.SyncStatus == 170002)
                    {
                        query.CstMemberID = liCstMember.CstMemberID;
                        string msg = string.Empty;
                        BitAuto.YanFa.Crm2009.BLL.SyncCstMember.SyncCstMemberInfo(query.CstMemberID, query.CSTRecID, out msg);
                        BitAuto.YanFa.Crm2009.BLL.SyncCstMember.SyncCstExpandInfo(query.CstMemberID, query.CSTRecID, out msg);
                        BitAuto.YanFa.Crm2009.BLL.SyncCstMember.SyncCstLinkMan(query.CstMemberID, query.CSTRecID, out msg);
                        dtCstMember = BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.GetTableByRecID(query);
                    }
                }
                if (dtCstMember.Rows[0]["FullName"] == DBNull.Value || dtCstMember.Rows[0]["FullName"].ToString() == "")
                {
                    ScriptHelper.ShowCustomScript(this.Page, "$.jAlert('您要查看的会员不存在',function(){window.close();});");
                }
                else
                {

                    GetCSTMemberProperties(dtCstMember);

                    //获取联系人列表，并把列表的第一个人放入页面变量中
                    string strWhereLinkMan = "CSTRecID = '" + dtCstMember.Rows[0]["CSTRecID"].ToString() + "'";
                    DataSet dsLinkMan = BitAuto.YanFa.Crm2009.BLL.CSTLinkMan.Instance.GetList(strWhereLinkMan);
                    GetLinkManProperties(dsLinkMan);
                }
            }
        }

        //用于把查询出的数据存入页面变量（客户基本信息）
        private void GetCSTMemberProperties(DataTable dt)
        {
            if (!string.IsNullOrEmpty(dt.Rows[0]["Status"].ToString()))
            {
                this.clientStatus = GetCstStatusString(Convert.ToInt32(dt.Rows[0]["Status"].ToString()));
            }
            this.Status = dt.Rows[0]["Status"].ToString();
            this.clientFullName = dt.Rows[0]["FullName"].ToString();
            this.clientShortName = dt.Rows[0]["ShortName"].ToString();
            this.clientCode = dt.Rows[0]["VendorCode"].ToString();
            if (!string.IsNullOrEmpty(dt.Rows[0]["VendorClass"].ToString()))
            {
                this.clientType = GetCstTypeString(Convert.ToInt32(dt.Rows[0]["VendorClass"].ToString()));
                vendorClass = dt.Rows[0]["VendorClass"].ToString();
            }
            if (!string.IsNullOrEmpty(dt.Rows[0]["SuperId"].ToString()))
            {
                this.upCompanyName = GetSuperCompanyString(Convert.ToInt32(dt.Rows[0]["SuperId"].ToString()));
            }
            //this.region = dt.Rows[0]["ProvinceID"].ToString();
            this.region = GetRegionString(dt.Rows[0]["ProvinceID"].ToString(), dt.Rows[0]["CityID"].ToString(), dt.Rows[0]["CountyID"].ToString());
            this.detailAddr = dt.Rows[0]["Address"].ToString();

            this.postCode = dt.Rows[0]["PostCode"].ToString();

            if (dt.Rows[0]["SyncTime"] != DBNull.Value)
            {
                this.syncTime = Convert.ToDateTime(dt.Rows[0]["SyncTime"]).ToShortDateString();
            }
            BitAuto.YanFa.Crm2009.Entities.CSTExpandInfo dstCSTExpandInfo = BitAuto.YanFa.Crm2009.BLL.CSTExpandInfo.Instance.GetModelByCSTRecID(Member.CSTRecID);
            if (dstCSTExpandInfo != null)
            {
                if (dstCSTExpandInfo.UCount != BitAuto.YanFa.Crm2009.Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    this.UCount = dstCSTExpandInfo.UCount.ToString();
                }
                if (dstCSTExpandInfo.LastAddUbTime != BitAuto.YanFa.Crm2009.Entities.Constants.Constant.DATE_INVALID_VALUE)
                {
                    this.lastAddUbTime = dstCSTExpandInfo.LastAddUbTime.ToShortDateString();
                }
                if (dstCSTExpandInfo.UBTotalAmount != BitAuto.YanFa.Crm2009.Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    this.UbTotalAmount = dstCSTExpandInfo.UBTotalAmount.ToString();
                }
                if (dstCSTExpandInfo.UBTotalExpend != BitAuto.YanFa.Crm2009.Entities.Constants.Constant.INT_INVALID_VALUE)
                {
                    this.UbTotalExpend = dstCSTExpandInfo.UBTotalExpend.ToString();
                }
                if (dstCSTExpandInfo.ActiveTime != BitAuto.YanFa.Crm2009.Entities.Constants.Constant.DATE_INVALID_VALUE)
                {
                    this.activeTime = dstCSTExpandInfo.ActiveTime.ToShortDateString();
                }
            }
            this.CustID = dt.Rows[0]["CustID"].ToString();
            this.SyncStatus = GetSyncStatusDesc(dt.Rows[0]["SyncStatus"].ToString());
            this.SyncStatusValue = dt.Rows[0]["SyncStatus"].ToString();
            this.TrafficInfo = dt.Rows[0]["TrafficInfo"].ToString();
            this.CreateTime = string.Format("{0:yyyy-MM-dd}", dt.Rows[0]["CreateTime"]);

            DataTable dstTable = BitAuto.YanFa.Crm2009.BLL.CSTMember_Brand.Instance.GetList(" CSTRecID ='" + Member.CSTRecID + "'").Tables[0];
            if (dstTable.Rows.Count > 0)
            {
                string BrandIds = "";
                foreach (DataRow row in dstTable.Rows)
                {
                    BrandIds += row["BrandID"].ToString() + ",";
                }
                BrandIds = BrandIds.TrimEnd(',');
                BrandIdsName = BitAuto.YanFa.Crm2009.BLL.CarBrand.Instance.GetBrandNames(BrandIds);
            }
            CSTMemberID = dt.Rows[0]["CSTMemberID"].ToString();
        }

        //用于把查询出的数据存入页面变量（客户联系人信息）
        private void GetLinkManProperties(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.contact = ds.Tables[0].Rows[0]["Name"].ToString();
                this.depart = ds.Tables[0].Rows[0]["Department"].ToString();
                this.position = ds.Tables[0].Rows[0]["Position"].ToString();
                this.tele = ds.Tables[0].Rows[0]["Tel"].ToString();
                this.phone = ds.Tables[0].Rows[0]["Mobile"].ToString();
                this.email = ds.Tables[0].Rows[0]["Email"].ToString();
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
            return BitAuto.YanFa.Crm2009.BLL.CstMember.Instance.SelectCstMemberIDByCSTRecID(Member.CSTRecID);
        }


        public string GetStatus(string status)
        {
            if (status == "1")
            {
                return "<img src=\"/Images/xt.gif\" alt=\"启用\" title ='启用'>";
            }
            else if (status == "0")
            {
                return "<img src=\"/Images/jinyong.ico\" alt=\"禁用\" title ='禁用'>";
            }
            else
            {
                return "";
            }
        }

        public string GetRejectedStr(string SyncStatusValue)
        {
            int recid = 0;
            if (SyncStatusValue == "170008")
            {
                string result = "";
                DataTable dstTable = new DataTable();
                dstTable = BitAuto.YanFa.Crm2009.BLL.CSTMemberSyncLog.Instance.GetSyncLog(Member.CSTRecID);
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