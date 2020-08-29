using System;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers
{
    public partial class DealerInfoList_YP : PageBase
    {
        public string ProvinceID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("ProvinceID");
            }
        }
        public string CityID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("CityID");
            }
        }
        public int TypeId
        {
            get
            {
                return BLL.Util.GetCurrentRequestInt("TypeId");
            }
        }
        public string PriceIds
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("PriceIds");
            }
        }
        //为分页
        public string PageIndex
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("PageIndex");
            }
        }

        public int PageSize = 99999;
        public string DealerPageURL;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DealerPageURL = ConfigurationUtil.GetAppSettingValue("DealerPageURL");
                BindData(TypeId, Convert.ToInt32(CityID), -1);
            }
        }
        /// 增加经销商类型
        /// <summary>
        /// 增加经销商类型
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable GetMemberType(DataTable dt)
        {
            if (dt != null)
            {
                //给表加一列销售类型
                dt.Columns.Add("MemberSaleType", typeof(string));
                //把想要取销售类型的经销商编号放在字符串数组里
                string[] membercodelist = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DealerId"].ToString() != "")
                    {
                        membercodelist[i] = dt.Rows[i]["DealerId"].ToString();
                    }
                }
                if (membercodelist != null && membercodelist.Length > 0)
                {
                    //根据经销商编号数组取对应的销售类型键值对
                    CRMCytMemberService.CYTMember CRMCytMemberHelper = new CRMCytMemberService.CYTMember();
                    CRMCytMemberService.DictionaryEntry[] membertypelist = CRMCytMemberHelper.GetMemberTypeByMemCode(membercodelist);
                    //遍历键值对把销售类型付给相应的Table行
                    if (membertypelist != null && membertypelist.Length > 0)
                    {
                        for (int i = 0; i < membertypelist.Length; i++)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (membertypelist[i].Key.ToString().Trim() == dt.Rows[j]["DealerId"].ToString().Trim())
                                {
                                    dt.Rows[j]["MemberSaleType"] = membertypelist[i].Value;
                                }
                            }
                        }
                    }
                }
                //按销售类型排序
                dt.DefaultView.Sort = "MemberSaleType Desc";
                return dt.DefaultView.ToTable();
            }
            else
            {
                return dt;
            }
        }
        /// 根据车id，地区id,经销商级别取数据
        /// <summary>
        /// 根据车id，地区id,经销商级别取数据
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="locationid"></param>
        /// <param name="level"></param>
        private void BindData(int carid, int locationid, int level)
        {
            DateTime dtTime = DateTime.Now;
            BitAuto.ISDC.CC2012.WebService.NoDealerOrderHelper OrderHelper = new BitAuto.ISDC.CC2012.WebService.NoDealerOrderHelper();
            DataSet ds = OrderHelper.GetDealerListByLocationId(carid, locationid, level);
            if (ds.Tables[0] != null)
            {
                //有报价
                if (Request["IsPrice"] != null)
                {
                    if (Request["IsPrice"] == "1")
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (ds.Tables[0].Rows[i]["ishavePrice"].ToString() != "1")
                            {
                                ds.Tables[0].Rows.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    else if (Request["IsPrice"] == "0")
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (ds.Tables[0].Rows[i]["ishavePrice"].ToString() == "1")
                            {
                                ds.Tables[0].Rows.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
                //过滤电话或地址为空的记录
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["vendorTel"] == DBNull.Value || ds.Tables[0].Rows[i]["vendorTel"].ToString() == "" || ds.Tables[0].Rows[i]["vendorSaleAddr"] == DBNull.Value || ds.Tables[0].Rows[i]["vendorSaleAddr"].ToString() == "")
                    {
                        ds.Tables[0].Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            DataTable dt = null;
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count >= 6)
            {
                dt = BLL.Util.GetPagedTable(ds.Tables[0], PageSize, 1);
            }
            else
            {
                dt = ds.Tables[0];
            }
            dtTime = DateTime.Now;
            dt = GetMemberType(dt);
            if (dt != null)
            {
                if (string.IsNullOrEmpty(PriceIds))
                {
                    this.DealerList.DataSource = dt;
                    this.DealerList.DataBind();
                }
                else
                {
                    DataTable newdt = new DataTable();
                    newdt = dt.Clone();
                    // 克隆dt 的结构，包括所有 dt 架构和约束,并无数据； 
                    DataRow[] rows = dt.Select("isHavePrice in (" + PriceIds + ")");
                    if (rows.Length <= 0)
                    {
                        this.DealerList.DataSource = newdt;
                        this.DealerList.DataBind();
                    }
                    else
                    {
                        foreach (DataRow row in rows)  // 将查询的结果添加到dt中； 
                        {
                            newdt.Rows.Add(row.ItemArray);
                        }
                        this.DealerList.DataSource = newdt;
                        this.DealerList.DataBind();
                    }
                }
            }
        }

        public string GetCopyParaStr(object funnName, object memberCode, object dealerType, object address)
        {
            string strMembertype = CommonFunction.ObjectToString(dealerType);
            if (strMembertype == "1")
            {
                strMembertype = "4s";
            }
            else
            {
                strMembertype = "综合";
            }
            return "javascript:CopyDealerInfoToOrder(\"" + funnName + "\",\"" + memberCode + "\",\"" + strMembertype + "\",\"" + address + "\");";
        }
    }
}