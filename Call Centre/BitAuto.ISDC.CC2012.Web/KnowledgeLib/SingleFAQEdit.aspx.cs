﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.KnowledgeLib
{

    public partial class SingleFAQEdit : PageBase
    {
        public string KFAQID
        {
            get
            {
                if (HttpContext.Current.Request["kfaqid"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["kfaqid"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string KCPID;
        public string KCID;

        protected void Page_Load(object sender, EventArgs e)
        {
            bindKnowledgeCategory();
            GetFLFAQ();
        }
        //获取分类下拉列表
        private void bindKnowledgeCategory()
        {
            //ListItem firstItem = new ListItem();
            //firstItem.Text = "";
            //firstItem.Value = "";

            QueryKnowledgeCategory query = new QueryKnowledgeCategory();
            query.Level = 1;
            query.Pid = 0;
            int count;
            DataTable dt = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(query, "ISNULL(SortNum,99999),KCID", 1, 10000, out count);

            selKCID1.DataSource = dt;
            selKCID1.DataTextField = "Name";
            selKCID1.DataValueField = "KCID";
            selKCID1.DataBind();
            selKCID1.Items.Insert(0, new ListItem() { Text = "请选择", Value = "-1" });

        }

        private void GetFLFAQ()
        {
            long nid = -1;
            if (string.IsNullOrEmpty(KFAQID) || !long.TryParse(KFAQID, out nid))
            {
                return;
            }
            QueryKLFAQ faq = new QueryKLFAQ() { KLFAQID = nid };
            int nCount;
            DataTable dt = BLL.KLFAQ.Instance.GetKLFAQ(faq, "", 1, 100, out nCount);
            if (dt != null && dt.Rows.Count < 1) return;
            this.FAQ_A.InnerText = dt.Rows[0]["Ask"].ToString();
            this.FAQ_Q.InnerText = dt.Rows[0]["Question"].ToString();
            KCPID = dt.Rows[0]["KCPID"].ToString();
            KCID = dt.Rows[0]["KCID"].ToString();
        }

        public string CommonSingleKLID
        {
            get
            {
                QueryKnowledgeLib query = new QueryKnowledgeLib();
                query.Status = 5;
                int kcidd = 0;
                try
                {
                    return BLL.KnowledgeLib.Instance.GetKnowledgeLib(query, "", 1, 1, out kcidd).Rows[0]["klid"].ToString();
                }
                catch (Exception)
                {
                    return "-1";
                }

            }

        }

        /// <summary>
        /// 是否是管理员 1、管理员  0、普通用户
        /// </summary>
        public string IsManager
        {
            get
            {
                try
                {
                    if (BitAuto.YanFa.SysRightManager.Common.UserInfo.IsLogin())
                    {

                        int userID = BLL.Util.GetLoginUserID();
                        bool right_Approval = BLL.Util.CheckRight(userID, "SYS024BUT3101");//审核通过
                        bool right_Reject = BLL.Util.CheckRight(userID, "SYS024BUT3102");//驳回
                        bool right_Disable = BLL.Util.CheckRight(userID, "SYS024BUT3104");//停用

                        if (!right_Approval && !right_Reject && !right_Disable)     //如果不具备 审核通过、驳回、停用的权限 则是普通用户
                        {
                            return "0";
                        }
                        else
                        {
                            return "1";
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }
                catch (Exception ex)
                {
                    return "0";
                }


            }
        }


    }
}