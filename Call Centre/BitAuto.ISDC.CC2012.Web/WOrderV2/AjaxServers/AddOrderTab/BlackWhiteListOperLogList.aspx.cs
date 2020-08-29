using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Web.Base;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.AddOrderTab
{
    public partial class BlackWhiteListOperLogList : PageBase
    {
        /// 电话号码列表
        /// <summary>
        /// 电话号码列表
        /// </summary>
        private string PhoneNums
        {
            get { return BLL.Util.GetCurrentRequestStr("PhoneNums"); }
        }

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindListData();
            }
        }

        private void BindListData()
        {
            PageSize = 50;
            if (PhoneNums != "")
            {
                Entities.QueryBlackWhiteListOperLog query = new Entities.QueryBlackWhiteListOperLog();
                query.PhoneNum = PhoneNums;
                query.BWType = 0;//黑名单
                DataTable dt = BLL.BlackWhiteListOperLog.Instance.GetBlackWhiteListOperLog(query, "a.PhoneNum,a.OperTime desc", PageCommon.Instance.PageIndex, PageSize, out RecordCount);
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
            }
            else
            {
                repeaterTableList.DataSource = null;
                repeaterTableList.DataBind();
            }
        }
        /// 取操作类型
        /// <summary>
        /// 取操作类型
        /// </summary>
        /// <param name="operType"></param>
        /// <returns></returns>
        public string GetOperTypesName(string operType)
        {
            string operName = string.Empty;
            switch (operType)
            {
                case "3":
                    operName = "删除记录";
                    break;
                default:
                    operName = "保存记录";
                    break;
            }
            return operName;
        }
    }
}