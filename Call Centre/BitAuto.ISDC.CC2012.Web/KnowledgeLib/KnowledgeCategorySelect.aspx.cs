using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{
    public partial class KnowledgeCategorySelect : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string htmlstr = string.Empty;
        /// <summary>
        /// 知识点id，如果为多个用逗号隔开
        /// </summary>
        public string RequestKLID
        {
            get
            {
                if (Request["KLID"] != null)
                {
                    return Request["KLID"];
                }
                else
                {
                    return "";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //BindKnowledgeCategory();
            BoundSecondRepeater();
        }

        /// <summary>
        /// 绑定分类
        /// </summary>
        protected void BindKnowledgeCategory()
        {
            DataTable dt = null;
            htmlstr = string.Empty;
            //取一级分类
            dt = BLL.KnowledgeCategory.Instance.GetCategory(1);
            if (dt != null && dt.Rows.Count > 0)
            {
                htmlstr += "<ul class='clearfix '>";
            }
            foreach (DataRow dr in dt.Rows)
            {
                htmlstr += "<li><label>" + dr["Name"].ToString() + "：</label>";

                DataTable dtChild = null;
                dtChild = BLL.KnowledgeCategory.Instance.GetCategoryByPID(Convert.ToInt32(dr["KCID"].ToString()));

                if (dtChild != null && dtChild.Rows.Count > 0)
                {
                    //判断某节点是否有孙子节点
                    if (HaveChildChildByKCID(dr["KCID"].ToString()) == false)
                    {
                        //遍历二极节点
                        foreach (DataRow drchild in dtChild.Rows)
                        {
                            htmlstr = htmlstr + "<span><input  id='KnowledgeCategory_" + drchild["KCID"].ToString() + "' class='marginleft' value='" + drchild["KCID"].ToString() + "' type='radio'  name='KnowledgeCategory'/>" + drchild["Name"].ToString() + "</span>&nbsp;";
                        }
                        htmlstr += "</li>";
                    }
                    else
                    {
                        //遍历二极节点
                        int i = 0;
                        foreach (DataRow drchild in dtChild.Rows)
                        {
                            i = i + 1;
                            if (i == 1)
                            {
                                htmlstr += "<label>" + drchild["Name"].ToString() + "|</label>";
                            }
                            else
                            {
                                htmlstr += "<li><label>" + drchild["Name"].ToString() + "|</label>";
                            }

                            DataTable dtChildChild = BLL.KnowledgeCategory.Instance.GetCategoryByPID(Convert.ToInt32(drchild["KCID"].ToString()));
                            //遍历三级节点
                            foreach (DataRow drChildChild in dtChildChild.Rows)
                            {
                                htmlstr = htmlstr + "<span><input  id='KnowledgeCategory_" + drChildChild["KCID"].ToString() + "' class='marginleft' value='" + drChildChild["KCID"].ToString() + "' type='radio'  name='KnowledgeCategory'/>" + drChildChild["Name"].ToString() + "</span>&nbsp;";
                            }
                            htmlstr += "</li>";
                        }

                    }
                }



            }
        }
        /// <summary>
        /// 判断某节点是否有孙子节点
        /// </summary>
        /// <param name="kcid"></param>
        /// <returns></returns>
        public bool HaveChildChildByKCID(string kcid)
        {
            bool flag = false;
            DataTable dtChild = null;
            dtChild = BLL.KnowledgeCategory.Instance.GetCategoryByPID(Convert.ToInt32(kcid));

            if (dtChild != null && dtChild.Rows.Count > 0)
            {
                if (BLL.KnowledgeCategory.Instance.IsExistsChildByKCID(Convert.ToInt32(dtChild.Rows[0]["KCID"].ToString())))
                {
                    flag = true;
                }
            }
            return flag;
        }
        /// <summary>
        /// 绑定第一层repeater
        /// </summary>
        public void BoundFirstRepeater()
        {
            DataTable dt = null;
            //取一级分类
            dt = BLL.KnowledgeCategory.Instance.GetCategory(1);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

        }
        /// <summary>
        /// 绑定第一层repeater
        /// </summary>
        public void BoundSecondRepeater()
        {
            DataTable dt = null;
            //取一级分类
            dt = BLL.KnowledgeCategory.Instance.GetCategoryWithRegion(1);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

        }

        protected void repeatersonson_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string KCID = DataBinder.Eval(e.Item.DataItem, "KCID").ToString().Trim();

                Repeater rept = e.Item.FindControl("repeatersonsonson") as Repeater;
                DataTable dtchildchild = null;
                dtchildchild = BLL.KnowledgeCategory.Instance.GetCategoryByPID(Convert.ToInt32(KCID));
                rept.DataSource = dtchildchild;
                rept.DataBind();

            }
        }

        protected void repeaterTableList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string KCID = DataBinder.Eval(e.Item.DataItem, "KCID").ToString().Trim();
                //有三层节点
                if (HaveChildChildByKCID(KCID))
                {



                    Repeater reptson = e.Item.FindControl("repeaterson") as Repeater;
                    reptson.Visible = false;

                    DataTable dtChild = null;
                    dtChild = BLL.KnowledgeCategory.Instance.GetCategoryByPIDHaveSon(Convert.ToInt32(KCID));
                    Repeater reptMiddle = e.Item.FindControl("repeatersonson") as Repeater;
                    reptMiddle.DataSource = dtChild;
                    reptMiddle.DataBind();

                    //取是二层节点，不是三层节点的节点
                    Repeater repeaterLast = e.Item.FindControl("repeaterLast") as Repeater;
                    DataTable dtLast = null;
                    dtLast = BLL.KnowledgeCategory.Instance.GetCategoryByPIDNotSon(Convert.ToInt32(KCID));
                    if (dtLast != null && dtLast.Rows.Count > 0)
                    {
                        repeaterLast.Visible = true;
                        repeaterLast.DataSource = dtLast;
                        repeaterLast.DataBind();
                    }
                    else
                    {
                        repeaterLast.Visible = false;
                    }
                }
                else
                {
                    Repeater reptsonson = e.Item.FindControl("repeatersonson") as Repeater;
                    reptsonson.Visible = false;
                    Repeater repeaterLast = e.Item.FindControl("repeaterLast") as Repeater;
                    repeaterLast.Visible = false;
                    System.Web.UI.HtmlControls.HtmlGenericControl div = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvson");
                    div.Visible = false;
                    Repeater rept = e.Item.FindControl("repeaterson") as Repeater;
                    DataTable dtChild = null;
                    dtChild = BLL.KnowledgeCategory.Instance.GetCategoryByPID(Convert.ToInt32(KCID));
                    rept.DataSource = dtChild;
                    rept.DataBind();
                }
            }
        }
    }
}