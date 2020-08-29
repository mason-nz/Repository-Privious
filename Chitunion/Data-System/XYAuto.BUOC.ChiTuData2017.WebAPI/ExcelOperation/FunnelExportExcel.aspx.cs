using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Funnel;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.ExcelOperation
{
    public partial class FunnelExportExcel : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string dateType = Request["DateType"] + string.Empty; //数据类型
                string strOperator = Request["Operator"] + string.Empty;//操作类型
                string chartType = Request["ChartType"] + string.Empty;//操作人
                var Tuple = FunnelMaterialExcelBll.Instance.ReturnExcel(new BasicQueryArgs
                {
                    DateType = !string.IsNullOrEmpty(dateType) ? Convert.ToInt32(dateType) : 0,
                    Operator = !string.IsNullOrEmpty(strOperator) ? Convert.ToInt32(strOperator) : 0,
                    ChartType = chartType

                });
                if (Tuple == null)
                {
                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MessageBox('导出失败');</script>");
                }
                else
                {
                    if (!Tuple.Item3)
                    {
                        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MessageBox('数据量过大');</script>");

                    }
                    else
                    {
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode(Tuple.Item2.ToString(), System.Text.Encoding.UTF8)));
                        Response.BinaryWrite(Tuple.Item1.ToArray());
                        Response.End();
                        Tuple.Item1.Close();
                        Tuple.Item1.Dispose();
                    }
                }
            }
            catch (Exception x)
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MessageBox('导出异常');</script>");
                Loger.Log4Net.Error($"导出漏斗列表出错", x);
            }
        }
    }
}