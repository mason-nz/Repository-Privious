using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TemplateCategory
{
    public partial class TemplateCategory_List : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int userID = BLL.Util.GetLoginUserID();

            if (!BLL.Util.CheckRight(userID, "SYS024MOD5101"))
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
            BindData();
        }

        private void BindData()
        {
            int pageIndex;
            if (Request.QueryString["page"] != null)
            {
                pageIndex = Convert.ToInt32(Request.QueryString["page"].Trim());
            }
            else
            {
                pageIndex = 1;
            }

            int totalCount = 0;
            QueryTemplateInfo query = new QueryTemplateInfo();
            string TcID = "";
            int CateTotal = 0;
            Entities.QueryTemplateCategory CateQuery = new Entities.QueryTemplateCategory();
            if (Request.QueryString["type"] != null)
            {
                CateQuery.Type = int.Parse(Request.QueryString["type"]);
                DataTable dt2 = BLL.TemplateCategory.Instance.GetTemplateCategory(CateQuery, "", 1, 100, out CateTotal);
                foreach (DataRow row in dt2.Rows)
                {
                    TcID += row["RecID"].ToString() + ",";
                }
            }
            if (Request.QueryString["class1"] != null)
            {
                if (Request.QueryString["class1"].ToString() != "-1")
                {
                    TcID = Request.QueryString["class1"].ToString() + ",";
                    CateQuery.Pid = int.Parse(Request.QueryString["class1"].ToString());
                    DataTable dt2 = BLL.TemplateCategory.Instance.GetTemplateCategory(CateQuery, "", 1, 100, out CateTotal);
                    foreach (DataRow row in dt2.Rows)
                    {
                        TcID += row["RecID"].ToString() + ",";
                    }
                }
            }
            if (Request.QueryString["class2"] != null)
            {
                if (Request.QueryString["class2"].ToString() != "-1")
                {
                    TcID = Request.QueryString["class2"].ToString() + ",";
                }
            }

            if (TcID.Length > 0)
            {
                TcID = TcID.Substring(0, TcID.Length - 1);
                query.Content = TcID;
            }

            DataTable dt = BLL.TemplateInfo.Instance.GetTemplateInfo(query, "TemplateInfo.CreateTime desc", pageIndex, 20, out totalCount);
            dt.Columns.Add("CreateName");
            dt.Columns.Add("EmailServers");
            int intVal = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (int.TryParse(dr["CreateUserIDB"].ToString(), out intVal))
                {
                    dr["CreateName"] = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(intVal);
                }

                #region 邮件接收人
                DataTable dtEmailServers = BLL.TemplateInfo.Instance.getEmailServers(Convert.ToInt32(dr["RecID"]));
                if (dtEmailServers != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow drEmailServers in dtEmailServers.Rows)
                    {
                        if (int.TryParse(drEmailServers["UserID"].ToString(), out intVal))
                        {
                            dr["EmailServers"] += BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(intVal) + ",";
                        }
                    }
                    if (dr["EmailServers"].ToString().Length > 0)
                    {
                        dr["EmailServers"] = dr["EmailServers"].ToString().Substring(0, dr["EmailServers"].ToString().Length - 1);
                    }
                }
                #endregion

                #region 分类两级添加
                DataTable db = new DataTable(); //Template.TCID;
                QueryTemplateCategory queryCate = new QueryTemplateCategory();
                queryCate.RecID = Convert.ToInt32(dr["TcID"]);
                int totle;
                db = BLL.TemplateCategory.Instance.GetTemplateCategory(queryCate, "RecID", 1, 1, out totle);
                if (db != null && db.Rows.Count > 0)
                {
                    if (db.Rows[0]["Level"].ToString() == "2")
                    {
                        DataTable dbOri = new DataTable(); //Template.TCID;
                        QueryTemplateCategory queryCatedbOri = new QueryTemplateCategory();
                        queryCatedbOri.RecID = Convert.ToInt32(db.Rows[0]["PID"]);
                        int totledbOri;
                        dbOri = BLL.TemplateCategory.Instance.GetTemplateCategory(queryCatedbOri, "RecID", 1, 1, out totledbOri);
                        dr["Name"] = dbOri.Rows[0]["Name"].ToString() + "-" + dr["Name"].ToString();
                    }
                }
                #endregion
            }
            Rpt_TempList.DataSource = dt;
            Rpt_TempList.DataBind();                                  //返回总条数 
            //litPage.Text = BLL.PageCommon.Instance.LinkString("", 10, totalCount,10); //(totalCount);
            //分组页容量      每页条数   
            litPage.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, totalCount, 20, BLL.PageCommon.Instance.PageIndex, 1);
        }
    }
}