using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.BLL;
using System.Xml;
using System.Data;
using System.Collections;
using System.Threading;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    public partial class YpDealerSelect : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        public string ProvinceID
        {
            get
            {
                return (HttpContext.Current == null || string.IsNullOrEmpty(HttpContext.Current.Request["ProvinceID"]) == true) ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ProvinceID"]);
            }
        }
        public string CityID
        {
            get
            {
                return (HttpContext.Current == null || string.IsNullOrEmpty(HttpContext.Current.Request["CityID"]) == true) ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CityID"]);
            }
        }

        public string NeedPageLoad
        {
            get
            {
                return (HttpContext.Current == null || string.IsNullOrEmpty(HttpContext.Current.Request["npd"]) == true) ? "0" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["npd"]);
            }
        }


        //public string CarID
        //{
        //    get
        //    {
        //        return string.IsNullOrEmpty(HttpContext.Current.Request["carid"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["carid"]);
        //    }
        //}
        private int _carid;
        public int CarID
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return _carid;
                }

                if (!string.IsNullOrEmpty(HttpContext.Current.Request["carid"]))
                {
                    if (int.TryParse(HttpContext.Current.Request["carid"], out _carid))
                    {

                    }
                }
                return _carid;
            }
        }

        //为分页
        public string PageIndex
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["PageIndex"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["PageIndex"]);
            }
        }
        public string levelstr
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["level"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["level"]);
            }
        }

        public int GroupLength = 5;
        public int PageSize = 6;
        public string strhtml = string.Empty;
        #endregion


        private DateTime stTimer;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (NeedPageLoad == "0") return;

            ManualResetEvent arBlockGetCount = new ManualResetEvent(false);
            stTimer = DateTime.Now;
            if (!IsPostBack)
            {
                
            }
            try
            {
                //如果是分页
                if (!string.IsNullOrEmpty(PageIndex))
                {
                    string userid = BLL.Util.GetLoginUserID().ToString();
                    //如果缓存不为空
                    if (Cache[userid + "_DataTable"] != null)
                    {
                        DataTable dtnew = null;
                        dtnew = BLL.Util.GetPagedTable((DataTable)Cache[userid + "_DataTable"], PageSize, Convert.ToInt32(PageIndex));
                        dtnew = GetMemberType(dtnew);
                        this.DealerList.DataSource = dtnew;
                        this.DealerList.DataBind();
                        litPagerDown.Text = PageCommon.Instance.LinkStringByPostForHC(GetWhere(), GroupLength, ((DataTable)Cache[userid + "_DataTable"]).Rows.Count, PageSize, Convert.ToInt32(PageIndex), 1);
                    }

                }
                else
                {
                    int level = -1;
                    if (!string.IsNullOrEmpty(levelstr))
                    {
                        if (levelstr.IndexOf(',') > 0 && levelstr.Split(',').Length > 1)
                        {
                            level = -1;
                        }
                        else
                        {
                            level = Convert.ToInt32(levelstr);
                        }
                    }
                    var prid = ProvinceID;
                    var crrid = CarID;
                    //展示经销商数量
                    ThreadPool.QueueUserWorkItem(obj =>
                    {
                        strhtml = GetDealCount(prid, crrid, level);
                        arBlockGetCount.Set();
                    });

                    //BLL.Loger.Log4Net.Info(string.Format("在页面YpDealerSelect.aspx，页面总耗时{0}", (DateTime.Now - stTimer).TotalMilliseconds));
                    //DateTime dtTT = DateTime.Now;
                    BindData(CarID, Convert.ToInt32(CityID), level);

                    //等待线程全部结束后再继续
                    arBlockGetCount.WaitOne();
                    //BLL.Loger.Log4Net.Info(string.Format("在页面YpDealerSelect.aspx，绑定数据耗时{0}", (DateTime.Now - dtTT).TotalMilliseconds));

                }

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert(\"" + ex.Message.ToString() + "\");$.closePopupLayer('YpDealerSelectAjaxPopup');</script>");
                Response.End();
            }
            TimeSpan tsT = DateTime.Now - stTimer;
            BLL.Loger.Log4Net.Info(string.Format("【在推荐经销商页面YpDealerSelect.aspx, --Page_Load总耗时】{0}毫秒", tsT.TotalMilliseconds));
        }

        /// <summary>
        /// 根据车id，地区id,经销商级别取数据
        /// </summary>
        /// <param name="carid"></param>
        /// <param name="locationid"></param>
        /// <param name="level"></param>
        private void BindData(int carid, int locationid, int level)
        {
            //locationid = 110105;
            DateTime dtTime = DateTime.Now;
            BitAuto.ISDC.CC2012.WebService.NoDealerOrderHelper OrderHelper = new BitAuto.ISDC.CC2012.WebService.NoDealerOrderHelper();
            DataSet ds = OrderHelper.GetDealerListByLocationId(carid, locationid, level);
            //if (ds != null && ds.Tables.Count > 0)
            //{

            BLL.Loger.Log4Net.Info(string.Format("【在推荐经销商页面YpDealerSelect.aspx,方法BindData中，Step1--调用接口GetDealerListByLocationId耗时】{0}毫秒,carid:{1},locationid:{2},level:{3}", (DateTime.Now - dtTime).TotalMilliseconds, carid, locationid, level));

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
            //add by qizq 2013-7-19 根据经销商id取合作类型，通过crm接口取
            dtTime = DateTime.Now;
            dt = GetMemberType(dt);
            BLL.Loger.Log4Net.Info(string.Format("【在推荐经销商页面YpDealerSelect.aspx,方法BindData中，Step2--调用CRM接口（GetMemberType根据经销商id取合作类型）耗时】{0}毫秒", (DateTime.Now - dtTime).TotalMilliseconds));
            if (dt != null)
            {
                this.DealerList.DataSource = dt;
                this.DealerList.DataBind();
                litPagerDown.Text = PageCommon.Instance.LinkStringByPostForHC(GetWhere(), GroupLength, ds.Tables[0].Rows.Count, PageSize, PageCommon.Instance.PageIndex, 1);
                string userid = BLL.Util.GetLoginUserID().ToString();
                Cache.Remove(userid + "_DataTable");
                //把Datatable 放在ViewState
                Cache.Insert(userid + "_DataTable", ds.Tables[0], null, DateTime.Now.AddMinutes(800), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            //

            //ViewState["DataTable"] = ds.Tables[0]; 
        }


        /// <summary>
        /// 根据xmlstr取各地区经销商数量
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        private string GetDealCount(string Provinceid, int Carid, int level)
        {

            DateTime dtTime = DateTime.Now;

            BitAuto.ISDC.CC2012.WebService.NoDealerOrderHelper OrderHelper = new BitAuto.ISDC.CC2012.WebService.NoDealerOrderHelper();

            //通过接口取经销商数量
            string xmlstr = OrderHelper.GetCarDealerXML(Carid);

            BLL.Loger.Log4Net.Info(string.Format("【在推荐经销商页面YpDealerSelect.aspx,方法GetDealCount(取经销商数量)中--调用接口】耗时{0}毫秒，参数Carid:{1}", (DateTime.Now - dtTime).TotalMilliseconds, Carid));
            var dtTimeNew = DateTime.Now;

            string Returnstr = string.Empty;
            if (!string.IsNullOrEmpty(xmlstr))
            {
                Returnstr = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaName(Provinceid) + "：";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlstr);
                //取省份集合
                XmlNodeList elementlist = doc.GetElementsByTagName("p");
                foreach (XmlNode node in elementlist)
                {
                    if (node.Attributes.GetNamedItem("d") != null)
                    {
                        if (node.Attributes.GetNamedItem("d").Value == Provinceid)
                        {
                            XmlNodeList CityNodeList = node.ChildNodes;
                            if (CityNodeList != null && CityNodeList.Count > 0)
                            {
                                foreach (XmlNode CityNode in CityNodeList)
                                {
                                    if (CityNode.Attributes.GetNamedItem("d") != null && CityNode.Attributes.GetNamedItem("o") != null && CityNode.Attributes.GetNamedItem("n") != null && CityNode.Attributes.GetNamedItem("s") != null)
                                    {
                                        int sum = 0;
                                        if (level == -1)
                                        {
                                            sum = Convert.ToInt32(CityNode.Attributes.GetNamedItem("o").Value) + Convert.ToInt32(CityNode.Attributes.GetNamedItem("s").Value);

                                        }
                                        else if (level == 1)
                                        {
                                            sum = Convert.ToInt32(CityNode.Attributes.GetNamedItem("s").Value);
                                        }
                                        else
                                        {
                                            sum = Convert.ToInt32(CityNode.Attributes.GetNamedItem("o").Value);
                                        }

                                        Returnstr += "<a href='javascript:void(0)' onclick='GetDealerListByLocationId(" + CityNode.Attributes.GetNamedItem("d").Value + "," + Provinceid + ")'>" + CityNode.Attributes.GetNamedItem("n").Value + "（" + sum + "）</a>&nbsp;&nbsp;";
                                    }
                                }
                            }
                        }
                    }

                }
            }
            //BLL.Loger.Log4Net.Info(string.Format("在页面YpDealerSelect.aspx，方法GetDealCount中解析数据耗时{0}，整个方法耗时{1}，参数Carid:{2}", (DateTime.Now - dtTimeNew).TotalMilliseconds, (DateTime.Now - dtTime).TotalMilliseconds, Carid));
            return Returnstr;
        }

        private string GetWhere()
        {
            //BLL.Util.GetPagedTable

            string where = "";
            //string query = Request.Url.Query;

            //if ((!MemberName.Equals("")) || MemberName != null)
            //{
            //    where += "&MemberName=" + Util.EscapeString(MemberName);
            //}
            //where += "&random=" + (new Random()).Next().ToString();
            return where;
        }

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
                //
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
                    //
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
    }
}