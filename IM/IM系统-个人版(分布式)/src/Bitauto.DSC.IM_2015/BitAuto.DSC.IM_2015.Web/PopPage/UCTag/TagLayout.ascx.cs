using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;
//using BitAuto.ISDC.CC2012.BLL;
//using BitAuto.ISDC.CC2012.Entities;
//using BitAuto.ISDC.CC2012.Web.Base;
//using NPOI.HPSF;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class TagLayout : System.Web.UI.UserControl
    {
        public string IsEdit
        {
            get { return BitAuto.DSC.IM_2015.BLL.Util.GetCurrentRequestQueryStr("isedit"); }
        }
        //public string BGID
        //{
        //    //get { return BLL.Util.GetCurrentRequestQueryStr("bgid"); }
        //    get { return "9"; }
        //}

        public bool GetAll { get; set; }

        public string BGID { get; set; }

        /// <summary>
        /// 标记是否是取用户所在的及管辖的组1,0
        /// </summary>
        public string UserAll
        {
            get { return BitAuto.DSC.IM_2015.BLL.Util.GetCurrentRequestQueryStr("bgid"); }
        }

        //public string GetAll
        //{
        //    get { return BLL.Util.GetCurrentRequestQueryStr("all"); }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = null;
            if (GetAll)
            {
                //取所有标签，忽略BGID
                //dt = BLL.WorkOrderTag.Instance.GetWorkOrderTagByBGID(0, true);
                dt = BitAuto.DSC.IM_2015.BLL.BusinessGroup.Instance.GetBusinessGroupTagsByUserID(BitAuto.DSC.IM_2015.BLL.Util.GetLoginUserID());
                ForGlobleManage(dt);
                return;
            }

            //if (!string.IsNullOrEmpty(BGID))
            //{
            //    int nBgid = Convert.ToInt32(BGID);
            //    //取指定BGID下所有的标签
            //    dt = BLL.WorkOrderTag.Instance.GetWorkOrderTagByBGID(nBgid, false);
            //}
            else
            {
                //取当前用户管辖下所有的在用分组，
                dt = BitAuto.DSC.IM_2015.BLL.BusinessGroup.Instance.GetBusinessGroupTagsByUserID(BitAuto.DSC.IM_2015.BLL.Util.GetLoginUserID());
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                GetHtml(dt);
            }
        }



        private void GetHtml(DataTable dt)
        {
            StringBuilder sbBody = new StringBuilder();
            StringBuilder sbTitle = new StringBuilder();
            StringBuilder sbSubLi = new StringBuilder();

            //Hashtable htBGID = new Hashtable();
            Dictionary<string, string> htBGID = new Dictionary<string, string>();

            foreach (DataRow dr in dt.Rows)
            {
                if (!htBGID.ContainsKey(dr["BGID"].ToString()))
                {
                    htBGID.Add(dr["BGID"].ToString(), dr["GroupName"].ToString());
                }
            }

            int iTitle = 0;
            int nIndex = 0;
            foreach (var bgidT in htBGID)
            {
                sbTitle = new StringBuilder();
                sbSubLi = new StringBuilder();
                sbBody.Append("<div did='" + bgidT.Key + "' class='bq1'><span style='font-weight: bold;'>");
                sbBody.Append(bgidT.Value);
                sbBody.Append("</span>");
                if (nIndex == 0)
                {
                    sbBody.Append("<dd class=\"hide\" onclick='toggleUl(this);'></dd></div>");
                }
                else
                {
                    sbBody.Append("<dd  class=\"toggle\" onclick='toggleUl(this);'></dd></div>");
                }

                sbBody.Append("<div class=\"clearfix\"></div>");

                //切换标签
                sbBody.Append("<div class='hw_tab'>");
                sbBody.Append("<div class='Tab2' ");
                if (nIndex > 0)
                {
                    sbBody.Append(" style='display: none;' ");
                }
                sbBody.Append("><div class=\"Menubox\"><ul did='ulTagC'>");

                //sbBody.Append("<div class=\"clearfix\"></div>");

                //sbBody.Append(
                //    "<a href='javascript:void(0)' class='toggle aEdit' name='linkUporDown' style='*top: 36px;' onclick='toggleUl(this);'></a></div><div class='hw_tab'><div  style='display: none' class='Tab2' did='Tab2'>");
                //sbBody.Append("<div class='Menubox'>");

                //处理一级分类
                GenerateBody(dt, bgidT.Key.ToString(), ref sbTitle, ref sbSubLi, out iTitle);

                if (iTitle > 11)
                {
                    sbBody.Append("<div class='l_arrow l_arrow_g' style='left: -5px; position: absolute;' onclick='checkLeft(this);' ondblclick='return false;' onselectstart='return false;'  did='ln'></div>");
                    sbBody.Append(sbTitle.ToString());
                    sbBody.Append(
                        "<div class='r_arrow' style='margin-left:-10px;' onclick='checkRight(this);' ondblclick='return false;' onselectstart='return false;' did='rn'></div>");
                }
                else
                {
                    sbBody.Append(sbTitle.ToString());
                }
                sbBody.Append(
                    "</ul><div class='clearfix'></div></div><div class='clearfix'></div><div class='bgline'></div><div class='Contentbox'>");


                sbBody.Append(sbSubLi.ToString());


                sbBody.Append("</div>");  // end context

                //结束标志
                sbBody.Append("</div></div>");
                nIndex++;
            }
            lbT.InnerHtml = sbBody.ToString();
        }

        private void GenerateBody(DataTable dt, string BGID, ref StringBuilder sbTitle, ref StringBuilder sbSubLi, out int iTitle)
        {

            iTitle = 0;


            string strPid = string.Empty;
            var drRows = dt.Select("BGID=" + BGID + " and pid=0", "ordernum").ToArray();
            for (int i = 0; i < drRows.Length; i++)
            {
                DataRow drT = drRows[i];
                bool isLast = ((i == drRows.Length - 1) ? true : false);
                if (!isLast)
                {
                    sbTitle.Append("<li style='");
                }
                else
                {
                    sbTitle.Append("<li style='border-right:1px solid #ccc; ");
                }

                strPid = drT["TagID"].ToString();
                iTitle++;
                if (iTitle == 1)
                {
                    //sbTitle.Append("<li ll='1' did='" + strPid + "' onmouseover='setTab(this," + iTitle.ToString() + ",16)' class='hover' >" + drT["TagName"].ToString() + "</li>");
                    sbTitle.Append(string.Format("' ll='1' did='{0}' onmouseover='setTabPop(this,1,16)' class='hover' title='{1}'>{2}</li>", strPid, drT["TagName"], drT["TagName"]));
                }
                else if (iTitle == 11)
                {
                    //sbTitle.Append("<li rr='1' did='" + strPid + "' onmouseover='setTabPop(this," + iTitle.ToString() + ",16)'>" + drT["TagName"].ToString() + "</li>");
                    sbTitle.Append(string.Format("' rr='1' did='{0}' onmouseover='setTabPop(this,11,16)'  title='{1}'>{2}</li>", strPid, drT["TagName"], drT["TagName"]));

                }
                else if (iTitle > 11)
                {
                    sbTitle.Append(string.Format(" display: none;' did='{0}' onmouseover='setTabPop(this,{1},16)' title='{2}'>{3}</li>", strPid, iTitle, drT["TagName"], drT["TagName"]));
                }
                else
                {
                    //sbTitle.Append("<li  did='" + strPid + "' onmouseover='setTabPop(this," + iTitle.ToString() + ",16)'>" + drT["TagName"].ToString() + "</li>");
                    sbTitle.Append(string.Format("' did='{0}' onmouseover='setTabPop(this,{1},16)' title='{2}'>{3}</li>", strPid, iTitle, drT["TagName"], drT["TagName"]));
                }
                //sbSubLi.Append(@"</ul><div class='clearfix'></div></div><div class='clearfix'></div><div class='bgline'></div><div class='Contentbox'>");
                #region 处理二级分类

                sbSubLi.Append("<div did='con_two_" + iTitle.ToString() + "' ");
                if (iTitle > 1)
                {
                    sbSubLi.Append(" style='display: none;' ");
                }
                sbSubLi.Append("><ul>");


                foreach (DataRow drSub in dt.Select("BGID=" + BGID + " and pid=" + strPid, "ordernum"))
                {
                    sbSubLi.Append("<li did='" + drSub["TagID"].ToString() + "'>" + drSub["TagName"] + "</li>");
                }
                sbSubLi.Append("</ul>");
                sbSubLi.Append("<div class='clearfix'></div>");
                sbSubLi.Append("</div>");

                #endregion
            }


        }

        private void GetHtmlOld(DataTable dt)
        {
            StringBuilder sbBody = new StringBuilder();
            StringBuilder sbTitle = new StringBuilder();
            StringBuilder sbSubLi = new StringBuilder();

            //Hashtable htBGID = new Hashtable();
            Dictionary<string, string> htBGID = new Dictionary<string, string>();

            foreach (DataRow dr in dt.Rows)
            {
                if (!htBGID.ContainsKey(dr["BGID"].ToString()))
                {
                    htBGID.Add(dr["BGID"].ToString(), dr["GroupName"].ToString());
                }
            }

            int iTitle = 0;
            foreach (var bgidT in htBGID)
            {
                sbTitle = new StringBuilder();
                sbSubLi = new StringBuilder();
                sbBody.Append("<div class='xz_bq' did='" + bgidT.Key + "'><div class='bq1 clearfix'><span style='font-weight: bold;'>");
                sbBody.Append(bgidT.Value);
                sbBody.Append("</span>");
                if (IsEdit == "1")
                {
                    sbBody.Append("<a href='#'>编辑</a>");
                }
                sbBody.Append(
                    "<a href='javascript:void(0)' class='toggle aEdit' name='linkUporDown' style='*top: 36px;' onclick='toggleUl(this);'></a></div><div class='hw_tab'><div  style='display: none' class='Tab2' did='Tab2'>");
                sbBody.Append("<div class='Menubox'>");

                //处理一级分类
                GenerateBody(dt, bgidT.Key.ToString(), ref sbTitle, ref sbSubLi, out iTitle);

                if (iTitle > 11)
                {
                    sbBody.Append("<div class='l_arrow l_arrow_g' style='left: 0px; position: relative; margin-right: 10px;' onclick='checkLeft(this);' ondblclick='return false;' onselectstart='return false;'  did='ln'></div>");
                    sbBody.Append(sbTitle.ToString());
                    sbBody.Append(
                        "<div class='r_arrow ' style='position: absolute; left: 780px;' onclick='checkRight(this);' ondblclick='return false;' onselectstart='return false;' did='rn'></div>");
                }
                else
                {
                    sbBody.Append(sbTitle.ToString());
                }

                sbBody.Append("<div class='clearfix'></div></div>");
                sbBody.Append("<div class='clearfix'></div>");
                sbBody.Append("<div class='bgline'></div>");
                sbBody.Append("<div class='Contentbox'>");  //开始Content


                sbBody.Append(sbSubLi.ToString());


                sbBody.Append("</div>");  // end context

                //结束标志
                sbBody.Append("</div></div></div>");
            }
            lbT.InnerHtml = sbBody.ToString();
            //lbResult.Text = sb.ToString();

        }


        private void GenerateBodyOld(DataTable dt, string BGID, ref StringBuilder sbTitle, ref StringBuilder sbSubLi, out int iTitle)
        {

            iTitle = 0;
            sbTitle.Append("<ul did='ulTagC'>");

            string strPid = string.Empty;
            foreach (DataRow drT in dt.Select("BGID=" + BGID + " and pid=0", "ordernum"))
            {
                strPid = drT["TagID"].ToString();
                iTitle++;
                if (iTitle == 1)
                {
                    //sbTitle.Append("<li ll='1' did='" + strPid + "' onmouseover='setTabPop(this," + iTitle.ToString() + ",16)' class='hover' >" + drT["TagName"].ToString() + "</li>");
                    sbTitle.Append(string.Format("<li ll='1' did='{0}' onmouseover='setTabPop(this,1,16)' class='hover' title='{1}'>{2}</li>", strPid, drT["TagName"], drT["TagName"]));
                }
                else if (iTitle == 11)
                {
                    //sbTitle.Append("<li rr='1' did='" + strPid + "' onmouseover='setTabPop(this," + iTitle.ToString() + ",16)'>" + drT["TagName"].ToString() + "</li>");
                    sbTitle.Append(string.Format("<li rr='1' did='{0}' onmouseover='setTabPop(this,11,16)'  title='{1}'>{2}</li>", strPid, drT["TagName"], drT["TagName"]));

                }
                else if (iTitle > 11)
                {
                    //sbTitle.Append("<li  style='display: none;'  did='" + strPid + "' onmouseover='setTab(this," + iTitle.ToString() + ",16)'>" + drT["TagName"].ToString() + "</li>");
                    sbTitle.Append(string.Format("<li style='display: none;' did='{0}' onmouseover='setTabPop(this,{1},16)' title='{2}'>{3}</li>", strPid, iTitle, drT["TagName"], drT["TagName"]));
                }
                else
                {
                    //sbTitle.Append("<li  did='" + strPid + "' onmouseover='setTabPop(this," + iTitle.ToString() + ",16)'>" + drT["TagName"].ToString() + "</li>");
                    sbTitle.Append(string.Format("<li did='{0}' onmouseover='setTabPop(this,{1},16)' title='{2}'>{3}</li>", strPid, iTitle, drT["TagName"], drT["TagName"]));
                }

                #region 处理二级分类

                sbSubLi.Append("<div did='con_two_" + iTitle.ToString() + "' ");
                if (iTitle > 1)
                {
                    sbSubLi.Append(" style='display: none;' ");
                }
                sbSubLi.Append("><ul>");

                foreach (DataRow drSub in dt.Select("BGID=" + BGID + " and pid=" + strPid, "ordernum"))
                {
                    sbSubLi.Append("<li did='" + drSub["TagID"].ToString() + "'>" + drSub["TagName"] + "</li>");
                }
                sbSubLi.Append("</ul>");
                sbSubLi.Append("<div class='clearfix'></div>");
                sbSubLi.Append("</div>");
                #endregion
            }

            sbTitle.Append("</ul>");
        }

        private void ForGlobleManage(DataTable dt)
        {
            StringBuilder sbBody = new StringBuilder();
            StringBuilder sbSubLi = new StringBuilder();
            StringBuilder sbTitle = new StringBuilder();
            //Hashtable htBGID = new Hashtable();
            Dictionary<string, string> htBGID = new Dictionary<string, string>();

            foreach (DataRow dr in dt.Rows)
            {
                if (!htBGID.ContainsKey(dr["BGID"].ToString()))
                {
                    htBGID.Add(dr["BGID"].ToString(), dr["GroupName"].ToString());
                }
            }
            int iTitle = 0;
            foreach (var bgidT in htBGID)
            {

                sbSubLi = new StringBuilder();
                sbTitle = new StringBuilder();

                sbBody.Append("<div class='bq1' did='" + bgidT.Key + "'><span>");
                sbBody.Append(bgidT.Value);
                sbBody.Append("</span><a href='#' onclick='ModifyTagGroup(this);'>编辑</a><a href='javascript:void(0)' class='toggle aEdit'  name='linkUporDown'  onclick='toggleUl(this);'></a></div>");
                sbBody.Append(" <div class='hw_tab hw_tab2'>");

                sbBody.Append(
                    "<div class='Tab2' did='Tab2' style='display: none'>");
                sbBody.Append("<div class='Menubox'>");


                GenerateBody(dt, bgidT.Key.ToString(), ref sbTitle, ref sbSubLi, out iTitle);

                //拼装Body
                //数量超过11时，添加左右移动按钮
                if (iTitle > 11)
                {
                    sbBody.Append("<div class='l_arrow l_arrow_g' style='left: 0px; position: relative; margin-right: 10px;' onclick='checkLeft(this);' ondblclick='return false;' onselectstart='return false;'  did='ln'></div>");
                    sbBody.Append(sbTitle.ToString());
                    sbBody.Append(
                        "<div class='r_arrow '  onclick='checkRight(this);' ondblclick='return false;' onselectstart='return false;' did='rn'></div>");
                }
                else
                {
                    sbBody.Append(sbTitle.ToString());
                }

                sbBody.Append("<div class='clearfix'></div></div>");
                //sb.Append("<div class='bgline'></div>");
                sbBody.Append("<div class='Contentbox'>");  //开始Content

                sbBody.Append(sbSubLi.ToString());

                sbBody.Append("</div>");  // end context

                //结束标志
                sbBody.Append("</div></div>");
            }
            lbT.InnerHtml = sbBody.ToString();
        }
    }
}