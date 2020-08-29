using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer
{
    public partial class SelectTagLayer : PageBase
    {
        public string BusiTypeId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("busitypeid");
            }
        }
        public string TagId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("tagid");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BindData();
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("选择标签出错，" + ex.Message.ToString());
                }
            }
        }

        private void BindData()
        {
            Entities.QueryWOrderTag query = new Entities.QueryWOrderTag();
            query.BusiTypeID = BusiTypeId;
            query.Status = "1";

            DataTable dt = BLL.WOrderTag.Instance.GetAllData(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                //一级标签
                DataRow[] drs = dt.Select(" PID IS NULL or PID='0' ", " BusiTypeID asc, SortNum asc");

                //选中标签
                DataRow drselect = null;
                if (!string.IsNullOrEmpty(TagId))
                {
                    //一级、二级都存在
                    try
                    {
                        drselect = dt.Select("RecID=" + TagId)[0];
                        var drp = dt.Select("RecID=" + drselect["PID"].ToString())[0];
                    }
                    catch { drselect = null; }
                }


                Lit_left.Text = GetLeveOne(drs, drselect);
                Lit_right.Text = GetLeveTwo(dt, drs, drselect);
            }

        }
        /// <summary>
        /// 一级标签
        /// </summary>
        /// <param name="drs"></param>
        /// <returns></returns>
        private string GetLeveOne(DataRow[] drs, DataRow drselect)
        {
            if (drs == null || drs.Length == 0)
            {
                return "";
            }


            StringBuilder html = new StringBuilder();

            html.Append("<ul>");
            string selectStyle = "";
            for (int i = 0; i < drs.Length; i++)
            {
                //-----------------------------------------------------------------
                selectStyle = "";
                if (drselect == null)
                {
                    //选中第一个
                    if (i == 0)
                    {
                        selectStyle = " class='hover'";
                    }
                }
                else
                {
                    //选中参数
                    if (drselect["PID"].ToString() == drs[i]["RecID"].ToString())
                    {
                        selectStyle = " class='hover'";
                    }
                }
                //-----------------------------------------------------------------

                html.AppendFormat("<li id='one{0}' onclick=\"setTab('one',{0},{1})\" {3} >{2}</li>", i + 1, drs.Length, drs[i]["TagName"].ToString(), selectStyle);
            }
            html.Append("</ul>");
            return html.ToString();
        }
        /// <summary>
        /// 二级标签
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="drsone"></param>
        /// <returns></returns>
        private string GetLeveTwo(DataTable dt, DataRow[] drsone, DataRow drselect)
        {
            if (drsone == null || drsone.Length == 0 || dt == null || dt.Rows.Count == 0)
            {
                return "";
            }

            string recid = "";
            string selectStyle = "";
            StringBuilder html = new StringBuilder();
            DataRow[] drstwo;
            html.Append("<ul>");
            for (int i = 0; i < drsone.Length; i++)
            {
                drstwo = dt.Select(" PID='" + drsone[i]["RecID"].ToString() + "'", " sortnum asc");

                //-----------------------------------------------------------------
                selectStyle = "style='display: none;'";
                if (drselect == null)
                {
                    //选中第一个
                    if (i == 0)
                    {
                        selectStyle = " class='hover'";
                    }
                }
                else
                {
                    //选中参数
                    if (drselect["PID"].ToString() == drsone[i]["RecID"].ToString())
                    {
                        selectStyle = " class='hover' style='display: block;'";
                    }
                }
                //-----------------------------------------------------------------

                html.AppendFormat("<div id='con_one_{0}' {1} >", i + 1, selectStyle);
                html.Append("<ul>");
                for (int j = 0; j < drstwo.Length; j++)
                {
                    selectStyle = "";
                    recid = drstwo[j]["RecID"].ToString();

                    //-----------------------------------------------------------------
                    selectStyle = "";
                    if (drselect != null)
                    {
                        //选中参数
                        if (drselect["RecID"].ToString() == recid)
                        {
                            selectStyle = " class='current'";
                        }
                    }
                    //-----------------------------------------------------------------

                    html.AppendFormat(" <li   tagname='{0}' tagid='{1}' {2}><a href='#'>{0}</a></li>", drstwo[j]["TagName"].ToString(), recid, selectStyle);
                }

                html.Append("</ul>");
                html.Append("</div>");

            }
            return html.ToString();


        }

    }
}