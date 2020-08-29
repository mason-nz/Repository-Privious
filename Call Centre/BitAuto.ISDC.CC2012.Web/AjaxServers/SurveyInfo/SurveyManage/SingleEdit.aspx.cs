using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyManage
{
    public partial class SingleEdit : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        ///  问题字符串
        /// </summary>
        public string JsonStr
        {
            get
            {
                //return HttpContext.Current.Request["JsonStr"] == null ? string.Empty : 
                //    HttpUtility.UrlDecode(HttpContext.Current.Request["JsonStr"].ToString()).Replace(@"\", @"\\");
                return HttpContext.Current.Request["JsonStr"] == null ? string.Empty :
                 HttpContext.Current.Request["JsonStr"].ToString();
            }
        }

        /// <summary>
        /// 问题ID
        /// </summary>
        public string SQID
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["sqid"]) ? (int.Parse(IndexNum) * -1).ToString() :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["sqid"].ToString()).Replace(@"\", @"\\");
            }
        }

        /// <summary>
        ///  顺序，用来区分问题
        /// </summary>
        public string IndexNum
        {
            get
            {
                return HttpContext.Current.Request["indexNum"] == null ? string.Empty :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["indexNum"].ToString()).Replace(@"\", @"\\");
            }
        }

        /// <summary>
        /// 问卷ID
        /// </summary>
        public string SIID
        {
            get
            {
                return HttpContext.Current.Request["siid"] == null ? string.Empty :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["siid"].ToString()).Replace(@"\", @"\\");
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string action
        {
            get
            {
                return HttpContext.Current.Request["action"] == null ? string.Empty :
                    HttpUtility.UrlDecode(HttpContext.Current.Request["action"].ToString()).Replace(@"\", @"\\");
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //string str = JsonStr;
            //int intVal = 0;
            //if (SQID != string.Empty && int.TryParse(SQID, out intVal))
            //{
            //    this.hidSingleSqid.Value = SQID;
            //    Entities.SurveyQuestion model = BLL.SurveyQuestion.Instance.GetSurveyQuestion(int.Parse(SQID));
            //    if (model != null)
            //    {
            //        this.hidSingleSiid.Value = model.SIID.ToString();
            //    }

            //}
        }
    }
}