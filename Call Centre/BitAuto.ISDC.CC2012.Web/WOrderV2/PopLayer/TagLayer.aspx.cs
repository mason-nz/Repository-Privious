using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer
{
    public partial class TagLayer : PageBase
    {

        public string BusiTypeID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("busitypeid");
            }
        }
        public string PID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("pid");
            }
        }
        public string Level
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("level");
            }
        }
        public string LevelName
        {
            get
            {
                if (Level == "1")
                {
                    return "一级标签";
                }
                else if (Level == "2")
                {
                    return "二级标签";
                }
                else
                {
                    return "标签";
                }
            }
        }
        public int UserID
        {
            get
            {
                return BLL.Util.GetLoginUserID();
            }
        }

        /// <summary>
        /// 状态 “,”分割
        /// </summary>
        public string StrStatus
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("status");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            try
            {
                string sta = StrStatus.Trim(',');
                //
                Entities.QueryWOrderTag query = new Entities.QueryWOrderTag();
                query.Status = sta;
                query.BusiTypeID = BusiTypeID;
                query.PID = PID;
                query.IsPIDSearch = true;
                //
                DataTable dt = BLL.WOrderTag.Instance.GetAllData(query);

                rpt.DataSource = dt;
                rpt.DataBind();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("读取标签层出错，" + ex.Message.ToString());
            }
        }

        public string GetZYTitle(string value, string type)
        {
            if (type == "1")
            {
                if (value.Trim() == "1")
                {
                    return "在用";
                }
                else if (value.Trim() == "0")
                {
                    return "停用";
                }
                else if (value.Trim() == "-1")
                {
                    return "已删除";
                }
            }
            else if (type == "2")
            {
                if (value.Trim() == "1")
                {
                    return "停用";
                }
                else if (value.Trim() == "0")
                {
                    return "启用";
                }
            }
            return "";
        }

        public bool IsDel(string value)
        {
            if (value.Trim() == "-1")
            {
                return false;
            }
            else
            {
                return true; //删除
            }
        }
        public string GetDelTitle(string value, string str)
        {
            if (value.Trim() == "-1")
            {
                return "";
            }
            else
            {
                return str; //删除
            }
        }


    }
}