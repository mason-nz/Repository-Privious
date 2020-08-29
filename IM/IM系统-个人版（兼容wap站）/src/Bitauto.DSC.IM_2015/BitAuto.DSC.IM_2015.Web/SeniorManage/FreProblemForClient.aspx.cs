using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.DSC.IM_2015.Web.SeniorManage
{
    public partial class FreProblemForClient : System.Web.UI.Page
    {
        //业务线
        public int SourceType
        {
            get
            {
                int _sourcetype = 0;
                string sourcetype = HttpContext.Current.Request["SourceType"];
                if (!string.IsNullOrEmpty(sourcetype))
                {
                    if (int.TryParse(sourcetype, out _sourcetype))
                    {
                    }
                }
                return _sourcetype;
            }
        }
        public string MoreURL
        {
            get
            {
                return BLL.BaseData.Instance.ReadMoreUrl(SourceType.ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {

                Databound(SourceType);
            }
        }
        private void Databound(int SourceType)
        {
            DataTable dt = null;
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache["FroProblem_" + SourceType] != null)
            {
                dt = (DataTable)objCache["FroProblem_" + SourceType];
            }
            else
            {
                dt = BLL.FreProblem.Instance.GetAllFreProblem(15, SourceType);
                objCache.Insert("FroProblem_" + SourceType, dt, null, DateTime.Now.AddHours(2), TimeSpan.Zero);

            }
            this.repeaterList.DataSource = dt;
            this.repeaterList.DataBind();
        }
    }
}