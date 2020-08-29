using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage
{
    public partial class FreProblemList : System.Web.UI.Page
    {
        public int MinRecID = 0;
        public int MaxRecID = 0;
        public int RecordCount;
        public string RequestSourceType
        {
            get
            {
                return HttpContext.Current.Request["SourceType"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["SourceType"].ToString());
            }
        }
        public string RequestTitle
        {
            get
            {
                return HttpContext.Current.Request["Title"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Title"].ToString());
            }
        }
        public string RequestRemark
        {
            get
            {
                return HttpContext.Current.Request["Remark"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["Remark"].ToString());
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
            RecordCount = 0;
            DataTable dt = BLL.FreProblem.Instance.GetFreProblem(new QueryFreProblem() { Remark = RequestRemark, Title = RequestTitle, SourceType = RequestSourceType }, " SortNum,CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
            if (dt.Rows.Count > 0)
            {
                MinRecID = CommonFunc.ObjectToInteger(dt.Rows[0]["RecID"]);
                MaxRecID = CommonFunc.ObjectToInteger(dt.Rows[dt.Rows.Count - 1]["RecID"]);
            }
            dataRepeater.DataSource = dt;
            dataRepeater.DataBind();
        }

        /// <summary>
        /// 获取业务线名称
        /// </summary>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        protected string GetSourceTypeName(string sourceType)
        {
            var strName = "";
            var list = BLL.Util.GetAllSourceType(false);
            var sourceTypeArray = sourceType.Split(',');
            for (int i = 0; i < sourceTypeArray.Length; i++)
            {
                var inof = list.FirstOrDefault(x => x.SourceTypeValue == sourceTypeArray[i]);
                if (inof != null)
                    strName += inof.SourceTypeName + " ";
            }
            return strName;
        }
    }
}