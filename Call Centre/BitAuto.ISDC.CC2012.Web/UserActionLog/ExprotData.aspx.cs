using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.UserActionLog
{
    public partial class ExprotData : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性定义
        public string RequestAction
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormStr("Action").ToString();
            }
        }
        public int RequestYear
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormInt("Year");
            }
        }
        public int RequestMonth
        {
            get
            {
                return BLL.Util.GetCurrentRequestFormInt("Month");
            }
        }
        protected static string ConnectionStrings_CRM = ConfigurationUtil.GetAppSettingValue("ConnectionStrings");

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD5004"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    if (RequestAction.ToLower() == "exprotdataby4sandnot4s" &&
                    RequestYear > 0 && RequestMonth > 0)
                    {
                        DataSet ds = GetData4sAndNot4s(RequestYear, RequestMonth);
                        BLL.Util.ExportToCSV("导出结果", ds.Tables[0]);
                    }
                }
            }

        }

        private DataSet GetData4sAndNot4s(int year, int month)
        {
            string sql = string.Format(@"SELECT a.AreaName1 AS 省份,a.AreaName2 AS 城市,--COUNT(*) ,
                        SUM(CASE a.TypeID WHEN '20011' THEN 1 ELSE 0 END) AS [经纪公司],
                        SUM(CASE a.TypeID WHEN '20012' THEN 1 ELSE 0 END) AS [交易市场]

                        FROM (

                        SELECT area1.AreaName AS AreaName1,area2.AreaName AS AreaName2,ci.* 
                        FROM dbo.CustInfo AS ci

                        left join dbo.AreaInfo area1 on area1.AreaID = ci.ProvinceID
                        left join dbo.AreaInfo area2 on area2.AreaID = ci.CityID

                        WHERE ci.Status=0 AND ci.TypeID IN ('20011','20012')
                        AND ci.CreateTime BETWEEN '1900-00-01 0:0:0' AND '{0} 23:59:59'
                        ) AS a
                        GROUP BY a.AreaName1,a.AreaName2
                        ORDER BY a.AreaName1,a.AreaName2", new DateTime(year, month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"));
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(ConnectionStrings_CRM, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            return null;
        }
    }
}