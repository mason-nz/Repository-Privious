using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling
{
    public partial class AddDelCustRelation : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性定义
        public string Action
        {
            get { return BLL.Util.GetCurrentRequestFormStr("Action"); }
        }
        public string CustName
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustName"); }
        }
        public int ProvinceID
        {
            get { return BLL.Util.GetCurrentRequestFormInt("ProvinceID"); }
        }
        public int CityID
        {
            get { return BLL.Util.GetCurrentRequestFormInt("CityID"); }
        }
        public int CountyID
        {
            get { return BLL.Util.GetCurrentRequestFormInt("CountyID"); }
        }
        public int ExceptCustID
        {
            get { return BLL.Util.GetCurrentRequestFormInt("ExceptCustID"); }
        }
        public string TID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("TID"); }
        }
        public string CustAddress
        {
            get { return BLL.Util.GetCurrentRequestFormStr("Address"); }
        }
        public string TradeMarketID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("TradeMarketID"); }
        }

        /// <summary>
        /// 客户联系人姓名（全名匹配）
        /// </summary>
        public string CustContactName
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustContactName"); }
        }
        /// <summary>
        /// 客户名称完全匹配，yes或no
        /// </summary>
        public string CustNameAllMatch
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustNameAllMatch"); }
        }
        /// <summary>
        /// 是否显示删除关联客户信息
        /// </summary>
        public bool IsShowdDelCustRelation
        {
            get
            {
                string t = BLL.Util.GetCurrentRequestFormStr("IsShowdDelCustRelation");
                if (!string.IsNullOrEmpty(t))
                {
                    return bool.Parse(t);
                }
                return false;
            }
        }
        //页面初始化时的地区
        public string InitialProvinceID = "";
        public string InitialCityID = "";
        public string InitialCountyID = "";
        public int CarType = -2;// 经营范围 1-新车，2-二手车，3,-新车、二手车
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsCallback)
            {
                BindCarType(TID);
                if ((Action.ToLower().Equals("search") &&
                    !string.IsNullOrEmpty(CustName)) ||
                    (Action.ToLower().Equals("search") && IsShowdDelCustRelation) ||
                    (CarType == 2 && (//TradeMarketID != string.Empty ||
                    CustContactName != string.Empty ||
                     CustAddress != string.Empty || CustName != string.Empty ||
                     ProvinceID > 0)))
                {
                    BindData();
                }
            }
        }

        private void BindCarType(string tid)
        {
            Entities.ProjectTask_Cust model = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(tid);
            if (model != null)
            {
                CarType = model.CarType;
            }
        }

        private void BindData()
        {
            int count = -1;
            BitAuto.YanFa.Crm2009.Entities.QueryCustInfo query = new BitAuto.YanFa.Crm2009.Entities.QueryCustInfo();
            Entities.ProjectTask_DelCustRelation model = BLL.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelationByTID(TID);
            if (IsShowdDelCustRelation && model != null &&
                !string.IsNullOrEmpty(model.DelCustIDs))
            {
                query.DelCustIDs = model.DelCustIDs;
            }
            else
            {
                if (!string.IsNullOrEmpty(CustName))
                {
                    if (!string.IsNullOrEmpty(CustNameAllMatch)
                        && CustNameAllMatch.ToLower().Equals("yes"))
                    {
                        query.ExistCustName = CustName;
                    }
                    else
                    {
                        query.CustName = CustName;
                    }
                }
                if (ProvinceID > 0)
                {
                    query.ProvinceID = ProvinceID.ToString();
                }
                if (CityID > 0)
                {
                    query.CityID = CityID.ToString();
                }
                if (CountyID > 0)
                {
                    query.CountyID = CountyID.ToString();
                }
                if (ExceptCustID > 0)
                {
                    query.ExistCustID = ExceptCustID.ToString();
                }
                if (!string.IsNullOrEmpty(CustAddress))
                {
                    query.Address = CustAddress;
                }
                //if (!string.IsNullOrEmpty(TradeMarketID))
                //{
                //    query.TradeMarketID = TradeMarketID;
                //}
                if (!string.IsNullOrEmpty(CustContactName))
                {
                    query.ContactAllName = CustContactName;
                }
            }
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(query, "CustName", 1, 100, out count);

            repterCustInfo.DataSource = dt;
            repterCustInfo.DataBind();
        }

        protected void repterCustInfo_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int status = int.Parse(DataBinder.Eval(e.Item.DataItem, "Status").ToString().Trim());
                int lockStatus = int.Parse(DataBinder.Eval(e.Item.DataItem, "Lock").ToString().Trim());
                int carType = int.Parse(DataBinder.Eval(e.Item.DataItem, "CarType").ToString().Trim());
                Literal litCustStatus = e.Item.FindControl("litCustStatus") as Literal;
                Literal litCustCarType = e.Item.FindControl("litCustCarType") as Literal;
                Literal litCustLockStatus = e.Item.FindControl("litCustLockStatus") as Literal;
                switch (status)
                {
                    case 0:
                        litCustStatus.Text = "<img style='margin-left: 5px;' title='在用' src='/Images/xt.gif'>";
                        break;
                    case 1:
                        litCustStatus.Text = "<img style='margin-left: 5px;' title='停用' src='/Images/xt_1.gif'>";
                        break;
                    default:
                        break;
                }
                if (lockStatus == 1)
                {
                    litCustLockStatus.Text = "<img style='margin-left: 5px;' status='" + lockStatus + "' title='锁定' src='/Images/lock.gif'>";
                }
                else if (lockStatus == 0)
                {
                    litCustLockStatus.Text = "<img style='margin-left: 5px;' status='" + lockStatus + "' title='未锁定' src='/Images/unlock.gif'>";
                }
                switch (carType)
                {
                    case 1: litCustCarType.Text = "新车";
                        break;
                    case 2: litCustCarType.Text = "二手车";
                        break;
                    case 3: litCustCarType.Text = "新车/二手车";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}