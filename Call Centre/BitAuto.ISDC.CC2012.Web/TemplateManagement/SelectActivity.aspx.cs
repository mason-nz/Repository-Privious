using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.WebService.Market;
using System.Reflection;
using System.Collections;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class SelectActivity : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性

        public string RequestProvinceID
        {
            get { return HttpContext.Current.Request["ProvinceID"] != null ? System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request["ProvinceID"].Trim()) : string.Empty; }
        }
        public string RequestCityID
        {
            get { return HttpContext.Current.Request["CityID"] != null ? HttpContext.Current.Request["CityID"].Trim() : string.Empty; }
        }
        public string RequestBrandID
        {
            get { return HttpContext.Current.Request["BrandID"] != null ? HttpContext.Current.Request["BrandID"].Trim() : string.Empty; }
        }        
        public string RequestActivityIDs
        {
            get { return HttpContext.Current.Request["ActivityIDs"] != null ? HttpContext.Current.Request["ActivityIDs"].Trim() : string.Empty; }
        }
        //public string RequestBrandIDs
        //{
        //    get { return HttpContext.Current.Request["BrandIDs"] != null ? HttpContext.Current.Request["BrandIDs"].Trim() : string.Empty; }
        //}

        //当前页数
        public string PageIndex
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["page"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["page"]);
            }
        }

        //是否分页查询Paging
        public string Paging
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Paging"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Paging"]);
            }
        }

        //是否点击查询按钮操作
        public string IsSearch
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["IsSearch"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["IsSearch"]);
            }
        }

        public string CacheBrandID = "";
        public int GroupLength = 5;
        public int PageSize = 10;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                string userid = BLL.Util.GetLoginUserID().ToString();                

                //如果是分页
                if (!string.IsNullOrEmpty(Paging))
                {
                    //如果缓存不为空
                    if (Cache[userid + "_DataTableActivity"] != null)
                    {
                        DataTable dtnew = null;
                        DataTable dt = (DataTable)Cache[userid + "_DataTableActivity"];
                        dtnew = BLL.Util.GetPagedTable(dt, PageSize, Convert.ToInt32(PageIndex));
                        this.repterFriendCustMappingList.DataSource = dtnew;
                        this.repterFriendCustMappingList.DataBind();
                        
                        litPagerDown1.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, dt.Rows.Count, PageSize, PageCommon.Instance.PageIndex, 3);
                    }
                    
                }                
                else
                {
                    if (!string.IsNullOrEmpty(IsSearch))
                    {
                        if (Cache[userid + "_BrandIDActivity"] != null)
                        {
                            CacheBrandID = (string)Cache[userid + "_BrandIDActivity"];
                        }
                    }
                    else
                    {
                        Cache.Remove(userid + "_BrandIDActivity");
                        //把Datatable 放在ViewState
                        Cache.Insert(userid + "_BrandIDActivity", RequestBrandID, null, DateTime.Now.AddMinutes(800), System.Web.Caching.Cache.NoSlidingExpiration);
                    }

                    
                    DataBinds();
                }
            }
        }

        private void DataBinds()
        {            
            int brandid;
            if (!int.TryParse(RequestBrandID, out brandid))
                brandid = -1;

            if (!string.IsNullOrEmpty(IsSearch))
            {
                string userid = BLL.Util.GetLoginUserID().ToString();
                if (Cache[userid + "_BrandIDActivity"] != null)
                {
                    CacheBrandID = (string)Cache[userid + "_BrandIDActivity"];
                }

                if (!int.TryParse(CacheBrandID, out brandid))
                    brandid = -1;
            }

            int provinceid;
            if (!int.TryParse(RequestProvinceID, out provinceid))
                provinceid = -1;

            int cityid;
            if (!int.TryParse(RequestCityID, out cityid))
                cityid = -1;

            if (cityid > 0)
            {
                provinceid = cityid;
            }
            DataSet ds = null;
            try
            {
                ds = MarketServiceHelper.Instance.GetDataXml(brandid, provinceid);
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("推荐活动弹出层接口调用:ErrorMessage is:" + ex.Message);
                BLL.Loger.Log4Net.Info("推荐活动弹出层接口调用:ErrorStackTrace is:" + ex.StackTrace);
                return;
            }
            //DataSet ds = MarketServiceHelper.Instance.GetDataXml(brandid, provinceid);
            if (ds.Tables.Count > 0)
            {                

                DataTable dt = FormatDataByLINQ(ds);
                int totalCount;
                

                //设置数据源
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region 将数据保存到缓存
                    string userid = BLL.Util.GetLoginUserID().ToString();
                    Cache.Remove(userid + "_DataTableActivity");
                    //把Datatable 放在ViewState
                    Cache.Insert(userid + "_DataTableActivity", dt, null, DateTime.Now.AddMinutes(800), System.Web.Caching.Cache.NoSlidingExpiration);
                    #endregion

                    totalCount = dt.Rows.Count;
                    DataTable dtnew = null;
                    dtnew = BLL.Util.GetPagedTable(dt, PageSize, PageCommon.Instance.PageIndex);
                    this.repterFriendCustMappingList.DataSource = dtnew;
                    this.repterFriendCustMappingList.DataBind();

                    litPagerDown1.Text = PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, totalCount, PageSize, PageCommon.Instance.PageIndex, 3);
                }
                

            }

            if (!string.IsNullOrEmpty(RequestActivityIDs))
            {                
                string[] guids = RequestActivityIDs.Split(',');
                DataSet ds2 = null;
                try
                {
                    ds2 = MarketServiceHelper.Instance.GetDataXml(guids);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info("推荐活动弹出层接口调用:ErrorMessage is:" + ex.Message);
                    BLL.Loger.Log4Net.Info("推荐活动弹出层接口调用:ErrorStackTrace is:" + ex.StackTrace);
                    return;
                }
                

                DataTable dttemp = FormatDataByLINQ(ds2);

                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    literalEditCont.Text += "<tr><td><a href=\"javascript:DelSelectCustBrand('" + dttemp.Rows[i]["ActivityGuid"] + "');\"  name='" + dttemp.Rows[i]["AvtivityName"] + "' id='" + dttemp.Rows[i]["ActivityGuid"] + "'  ><img src=\"/Images/close.png\" title=\"删除\"/></a></td>";
                    literalEditCont.Text += "<td class=\"l\"><a href='" + dttemp.Rows[i]["url"] + "' target='_blank'>" + dttemp.Rows[i]["AvtivityName"] + "></a></td>";
                    literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["BrandName"] + "</td>";
                    literalEditCont.Text += "<td class=\"l\">" + dttemp.Rows[i]["ProvinceName"] + " "+ dttemp.Rows[i]["CityName"] + "</td>";
                    literalEditCont.Text += "<td class=\"l\">" + FormatDateTimeString(dttemp.Rows[i]["StartTime"].ToString()) + "</td>";
                    literalEditCont.Text += "<td class=\"l\">" + FormatDateTimeString(dttemp.Rows[i]["EndTime"].ToString()) + "</td>";
                }
            }



        }

        private DataTable FormatDataByLINQ(DataSet ds)
        {
            DataTable activities = ds.Tables["Activity"];

            DataRow[] rows = activities.Select("StartTime<=#" + DateTime.Now + "# and EndTime>=#" + DateTime.Now + "#");
            
            if (rows.Length == 0)
            {
                return null;
            }
            activities = rows.CopyToDataTable();
            DataTable brandactivitys = ds.Tables["Brands"];
            //DataTable brands = ds.Tables["Brand"];
            DataTable brands = SpliceBrand(ds.Tables["Brand"]);

            var query = activities.AsEnumerable().Join(brandactivitys.AsEnumerable(),
                activity => activity.Field<Int32>("Activity_Id"),
                brandactivity => brandactivity.Field<Int32>("Activity_Id"),
                (activity, brandactivity) => new
                {
                    ActivityGuid = activity.Field<string>("Guid"),
                    url = activity.Field<string>("url"),
                    name = activity.Field<string>("Name"),
                    StartTime = activity.Field<string>("StartTime"),
                    EndTime = activity.Field<string>("EndTime"),
                    Brands_Id = brandactivity.Field<Int32>("Brands_Id"),
                    Activity_Id = brandactivity.Field<Int32>("Activity_Id")
                });

            DataTable dt1s = GetDataTable(query);//关联品牌名称

            var query1 = dt1s.AsEnumerable().Join(brands.AsEnumerable(),
                dt1 => dt1.Field<Int32>("Brands_Id"),
                brand => brand.Field<Int32>("Brands_Id"),
                (dt1, brand) => new
                {
                    ActivityGuid = dt1.Field<string>("ActivityGuid"),
                    url = dt1.Field<string>("url"),
                    Activity_Id = dt1.Field<Int32>("Activity_Id"),
                    AvtivityName = dt1.Field<string>("name"),
                    StartTime = dt1.Field<string>("StartTime"),
                    EndTime = dt1.Field<string>("EndTime"),
                    BrandId = dt1.Field<Int32>("Brands_Id"),
                    BrandName = brand.Field<string>("name")
                });

            DataTable dt2s = GetDataTable(query1);//关联省份、活动中间表
            DataTable provinceactivitys = ds.Tables["Pros"];

            var query2 = dt2s.AsEnumerable().Join(provinceactivitys.AsEnumerable(),
                dt2 => dt2.Field<Int32>("Activity_Id"),
                provinceactivity => provinceactivity.Field<Int32>("Activity_Id"),
                (dt2, provinceactivity) => new
                {
                    ActivityGuid = dt2.Field<string>("ActivityGuid"),
                    url = dt2.Field<string>("url"),
                    Activity_Id = dt2.Field<Int32>("Activity_Id"),
                    AvtivityName = dt2.Field<string>("AvtivityName"),
                    StartTime = dt2.Field<string>("StartTime"),
                    EndTime = dt2.Field<string>("EndTime"),
                    BrandId = dt2.Field<Int32>("BrandId"),
                    BrandName = dt2.Field<string>("BrandName"),
                    ProvinceID = provinceactivity.Field<Int32>("Pros_Id")
                });

            DataTable dt3s = GetDataTable(query2);//关联省份中间表
            DataTable provinces = SpliceProvince(ds.Tables["Pro"]);



            var query3 = dt3s.AsEnumerable().Join(provinces.AsEnumerable(),
                dt3 => dt3.Field<Int32>("ProvinceID"),
                province => province.Field<Int32>("Pros_Id"),
                (dt3, province) => new
                {
                    ActivityGuid = dt3.Field<string>("ActivityGuid"),
                    url = dt3.Field<string>("url"),
                    Activity_Id = dt3.Field<Int32>("Activity_Id"),
                    AvtivityName = dt3.Field<string>("AvtivityName"),
                    StartTime = dt3.Field<string>("StartTime"),
                    EndTime = dt3.Field<string>("EndTime"),
                    BrandId = dt3.Field<Int32>("BrandId"),
                    BrandName = dt3.Field<string>("BrandName"),
                    ProvinceID = province.Field<Int32>("Pros_Id"),
                    ProvinceName = province.Field<string>("name"),
                    CityName = ""
                });

            DataTable dt4s = GetDataTable(query3);//关联城市表
            DataTable citys = ds.Tables["City"];
            
            //当省份为 全国时 没有Ctiy表
            if (citys == null)
            {
                return dt4s;
            }

            var query4 = dt4s.AsEnumerable().Join(citys.AsEnumerable(),
                dt4 => dt4.Field<Int32>("ProvinceID"),
                city => city.Field<Int32>("Pro_Id"),
                (dt4, city) => new
                {
                    ActivityGuid = dt4.Field<string>("ActivityGuid"),
                    url = dt4.Field<string>("url"),
                    Activity_Id = dt4.Field<Int32>("Activity_Id"),
                    AvtivityName = dt4.Field<string>("AvtivityName"),
                    StartTime = dt4.Field<string>("StartTime"),
                    EndTime = dt4.Field<string>("EndTime"),
                    BrandId = dt4.Field<Int32>("BrandId"),
                    BrandName = dt4.Field<string>("BrandName"),
                    ProvinceID = dt4.Field<Int32>("ProvinceID"),
                    ProvinceName = dt4.Field<string>("ProvinceName"),
                    CityID = city.Field<string>("id"),
                    CityName = city.Field<string>("name")
                });

            return GetDataTable(query4);
        }

        private System.Data.DataTable GetDataTable(IEnumerable list)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            bool schemaIsBuild = false;
            PropertyInfo[] props = null;

            foreach (object item in list)
            {
                if (!schemaIsBuild)
                {
                    props = item.GetType().GetProperties();
                    foreach (var pi in props)
                        dt.Columns.Add(new DataColumn(pi.Name, pi.PropertyType));
                }

                schemaIsBuild = true;

                var row = dt.NewRow();
                foreach (var pi in props)
                {
                    row[pi.Name] = pi.GetValue(item, null);
                }
                dt.Rows.Add(row);
                dt.AcceptChanges();
            }
            return dt;
        }

        public string FormatDateTimeString(string dtime)
        {
            DateTime dt = new DateTime();

            if (!DateTime.TryParse(dtime, out dt))
            {
                return dtime;
            }
            return dt.ToString("yyyy-MM-dd");
        }

        private DataTable SpliceProvince(DataTable dt)
        {
            IEnumerable<IGrouping<string, DataRow>> result = dt.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["Pros_Id"].ToString());//按A分组
            string rows = string.Empty;

            foreach (IGrouping<string, DataRow> ig in result)
            {
                
                var row1 = string.Empty;
                var row2 = string.Empty;
                foreach (var dr in ig)
                {
                    row1 += dr["id"].ToString() + "、";
                    row2 += dr["name"].ToString() + "、";
                }
                rows += row1.TrimEnd('、') + ";";
                rows += row2.TrimEnd('、') + ";";
                rows += ig.Key + ";";
                rows = rows.TrimEnd(';') + "|";
            }
            rows = rows.TrimEnd('|');

            //DataTable dtNew = dt.Clone();
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("id", typeof(string));
            dtNew.Columns.Add("name",typeof(string));
            dtNew.Columns.Add("Pros_Id",typeof(Int32));
            string[] arr_rows = rows.Split('|');
            foreach (string name in arr_rows)
            {
                if (name == "")
                {
                    continue;
                }
                string[] aNames = name.Split(';');
                DataRow drNew = dtNew.NewRow();
                for (int i = 0; i < aNames.Length; i++)
                {
                    drNew[i] = aNames[i];
                }
                dtNew.Rows.Add(drNew);
            }
            return dtNew;
        }

        private DataTable SpliceBrand(DataTable dt)
        {
            IEnumerable<IGrouping<string, DataRow>> result = dt.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["Brands_Id"].ToString());//按A分组
            string rows = string.Empty;

            foreach (IGrouping<string, DataRow> ig in result)
            {

                var row1 = string.Empty;
                var row2 = string.Empty;
                foreach (var dr in ig)
                {
                    row1 += dr["id"].ToString() + "、";
                    row2 += dr["name"].ToString() + "、";
                }
                rows += row1.TrimEnd('、') + ";";
                rows += row2.TrimEnd('、') + ";";
                rows += ig.Key + ";";
                rows = rows.TrimEnd(';') + "|";
            }
            rows = rows.TrimEnd('|');

            //DataTable dtNew = dt.Clone();
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("id", typeof(string));
            dtNew.Columns.Add("name", typeof(string));
            dtNew.Columns.Add("Brands_Id", typeof(Int32));
            string[] arr_rows = rows.Split('|');
            foreach (string name in arr_rows)
            {
                if (name == "")
                {
                    continue;
                }
                string[] aNames = name.Split(';');
                DataRow drNew = dtNew.NewRow();
                for (int i = 0; i < aNames.Length; i++)
                {
                    drNew[i] = aNames[i];
                }
                dtNew.Rows.Add(drNew);
            }
            return dtNew;
        }

        public string getOperator(string url, string activityName)
        {
            string operStr = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                operStr = "<a target='_blank' href='" + url + "' >" + activityName + "</a>";
            }
            else
            {
                //operStr = "<a target='_blank' style='color:#000000' >" + activityName + "</a>";
                operStr = activityName;
            }
            
            return operStr;
        }
    }
}