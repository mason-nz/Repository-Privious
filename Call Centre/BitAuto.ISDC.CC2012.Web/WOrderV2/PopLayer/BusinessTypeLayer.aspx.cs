using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer
{
    public partial class BusinessTypeLayer : PageBase
    {

        public int UserID
        {
            get { return BLL.Util.GetLoginUserID(); }
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
            BindData();

        }

        private void BindData()
        {
            try
            {
                string str = StrStatus.Trim(',');

                DataTable dt = BLL.WOrderBusiType.Instance.GetAllData(new Entities.QueryWOrderBusiTypeInfo() { Status = str });
                rpt.DataSource = dt;
                rpt.DataBind();
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("读取业务类型出错，" + ex.ToString());
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
        //TODO:CodeReview 2016-08-10 在前端控制是否要显示“删除”按钮，最好在<a>标签外面做判断
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