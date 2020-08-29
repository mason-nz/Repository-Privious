using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities.Constants;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.SeniorManage
{
    public partial class AddFreProblem : System.Web.UI.Page
    {
        #region 属性
        /// 操作方式
        /// <summary>
        /// 操作方式
        /// </summary>
        public string Action { get { return (RecID == Constant.INT_INVALID_VALUE) ? "add" : "mod"; } }
        /// 操作方式中文
        /// <summary>
        /// 操作方式中文
        /// </summary>
        public string ActionName { get { return (RecID == Constant.INT_INVALID_VALUE) ? "新增" : "修改"; } }
        /// ID
        /// <summary>
        /// ID
        /// </summary>
        public int RecID { get { return BLL.Util.GetCurrentRequestQueryInt("RecID"); } }
        #endregion

        public string title = "";
        public string url = "";
        public string Remark = "";
        public string SourceTypes = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Action == "mod")
                {
                    Entities.FreProblem info = BLL.FreProblem.Instance.GetComAdoInfo<Entities.FreProblem>(RecID);
                    title = info.Title;
                    url = info.Url;
                    Remark = info.Remark;
                    SourceTypes = info.SourceType;
                }
            }
        }

    }
}