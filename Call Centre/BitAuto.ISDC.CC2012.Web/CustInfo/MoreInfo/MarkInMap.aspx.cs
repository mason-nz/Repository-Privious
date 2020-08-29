using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class MarkInMap : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        /// 弹出框名称
        /// </summary>
        public string PopupName
        {
            get
            {
                string str = (Request["PopupName"] + "").Trim();
                if (string.IsNullOrEmpty(str)) { str = "AnonymousPopup"; }
                return str;
            }
        }

        private string dynamicView = null;
        /// <summary>
        /// 动态察看，不可标记
        /// </summary>
        public string DynamicView
        {
            get
            {
                if (this.dynamicView == null)
                {
                    string s = (Request["DynamicView"] + "").Trim();
                    this.dynamicView = (s == "true" ? "1" : "0");
                }
                return this.dynamicView;
            }
        }

        /// <summary>
        /// 纬度
        /// </summary>
        public string Marker_lat
        {
            get
            {
                return (Request["marker_lat"] + "").Trim();
            }
        }

        /// <summary>
        /// 经度
        /// </summary>
        public string Marker_lng
        {
            get
            {
                return (Request["marker_lng"] + "").Trim();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}