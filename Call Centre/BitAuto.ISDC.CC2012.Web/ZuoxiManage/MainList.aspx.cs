using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage;
namespace BitAuto.ISDC.CC2012.Web.ZuoxiManage
{
    public partial class MainList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write("----" + BLL.UserDataRigth.Instance.GetUserDataRigth(1).RightType + "--------");
            //if (BLL.UserDataRigth.Instance.GetUserDataRigth(1) == null)
            //{
            //    Response.Write("------------");
            //}
            //BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(0, ConfigurationUtil.GetAppSettingValue("ThisSysID"));


            DataTable db = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoBySysID(ConfigurationUtil.GetAppSettingValue("ThisSysID"));
            Rpt_Role.DataSource = db;
            Rpt_Role.DataBind();

            AreaManageConfig config = new AreaManageConfig(HttpContext.Current.Server);
           
            List<string> list = config.GetCurrentUserArea();

            DataTable tbArea = new DataTable();
            DataColumn dcName = new DataColumn("Name", typeof(string));
            DataColumn dcValue = new DataColumn("Value", typeof(string));
            tbArea.Columns.Add(dcName);
            tbArea.Columns.Add(dcValue);

            if (list != null && list.Count > 0)
            {

                foreach (string s in list)
                {
                    DataRow dr = tbArea.NewRow();
                    int value = 1;
                    if (s == "西安")
                    {
                        value = 2;
                    }
                    dr["Name"] = s;
                    dr["Value"] = value;

                    tbArea.Rows.Add(dr);
                }
            }

            rptArea.DataSource = tbArea;
            rptArea.DataBind();
        }
    }
}