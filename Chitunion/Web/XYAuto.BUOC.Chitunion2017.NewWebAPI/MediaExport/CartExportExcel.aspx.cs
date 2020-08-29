using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.BUOC.Chitunion2017.NewWebAPI.Common;
using XYAuto.ITSC.Chitunion2017.BLL;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.MediaExport
{
    public partial class CartExportExcel : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MemoryStream memoryStream = CartBll.Instance.GetCartExport();
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode("媒体列表", System.Text.Encoding.UTF8)));
                Response.BinaryWrite(memoryStream.ToArray());
                Response.End();
                memoryStream.Close();
                memoryStream.Dispose();
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("购物车导出出错：" + ex.ToString());
            }
        }
    }
}