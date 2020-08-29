using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.BUOC.ChiTuData2017.BLL.Query.Dto.RequestDto.Statistics;
using XYAuto.BUOC.ChiTuData2017.BLL.ReportOperation.Statistics;
using XYAuto.BUOC.ChiTuData2017.Infrastruction.Verification;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.ExcelOperation
{
    public partial class ExportDaily : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var request = new ReqDailyDto
            {
                TabType = Request["TabType"],
                StartDate = Request["StartDate"],
                EndDate = Request["EndDate"],
                ArticleType = Request["ArticleType"].ToInt(-2),
                ChannelId = Request["ChannelId"].ToInt(-2),
            };
            var retValue = new StatDailyExportProvider(request).DoExport();

            if (retValue.HasError)
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MessageBox('数据量过大');</script>");
            }
            else
            {
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}",
                    HttpUtility.UrlEncode(retValue.Message, System.Text.Encoding.UTF8)));
                Response.BinaryWrite((byte[])retValue.ReturnObject);
                Response.End();
            }
        }
    }
}