using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.WebAPI.Helper;
using BitAuto.ISDC.CC2012.WebAPI.WebServices;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.WebAPI.HTMLS
{
    public partial class HLBigScreenDataCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CommonHelper.CheckIP())
            {
                Bindata();
            }
        }

        private void Bindata()
        {
            Dictionary<string, SyncHelyBigScreenData.HotLineInf> dicHotLineInf = SyncHelyBigScreenData.getHotLineDictionary();

            DataTable dt = new DataTable();
            dt.Columns.Add("热线名称");
            dt.Columns.Add("接通率");
            dt.Columns.Add("[30S服务水平]");
            dt.Columns.Add("[Waiting]");
            foreach (KeyValuePair<string, SyncHelyBigScreenData.HotLineInf> keyValuePair in dicHotLineInf)
            {
                var dr = dt.NewRow();
                dr["热线名称"] = keyValuePair.Value.strHotLineNun + "->" + keyValuePair.Value.strHLName;
                dr["接通率"] = keyValuePair.Value.pc_n_answered;
                dr["[30S服务水平]"] = keyValuePair.Value.pc_n_distrib_in_tr;
                dr["[Waiting]"] = keyValuePair.Value.ContactInQueueNum;
                dt.Rows.Add(dr);
            }
            GV.DataSource = dt;
            GV.DataBind();
        }
    }
}