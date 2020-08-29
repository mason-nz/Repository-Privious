using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ExcelOperate
{
    public class ExcelInOut
    {

        /// <summary>
        /// 导出EXCEL（HTML格式的）
        /// </summary>
        /// <param name="ds">要导出的DataSet</param>
        /// <param name="fileName"></param>
        public static void CreateEXCEL(DataSet ds, string fileName)
        {
            System.Web.UI.WebControls.DataGrid dg = new DataGrid();           
            dg.DataSource = ds;
            dg.DataBind();

            for (int i = 0; i < dg.Items.Count; i++)
            {
                for (int j = 0; j < dg.Items[i].Cells.Count; j++)
                {
                    dg.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }

            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "GB2312";
             Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8) + ";charset=GB2312");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
            Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            System.Web.UI.WebControls.DataGrid DG = dg;
            DG.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
             Response.End();
        }


        /// <summary>
        /// 导出EXCEL（HTML格式的）
        /// </summary>
        /// <param name="dt">要导出的DataTable</param>
        /// <param name="fileName"></param>
        public static void CreateEXCEL(DataTable dt, string fileName, string RequestBrowser)
        {
            System.Web.UI.WebControls.DataGrid dg = new DataGrid();
            dg.DataSource = dt;
            dg.DataBind();

            for (int i = 0; i < dg.Items.Count; i++)
            {
                for (int j = 0; j < dg.Items[i].Cells.Count; j++)
                {
                    dg.Items[i].Cells[j].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }

            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "UTF-8";

            if (RequestBrowser == "IE")
            {
                Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8) + "\"");
            }
            else
            {
                Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + fileName + ".xls\"");
            }

            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            System.Web.UI.WebControls.DataGrid DG = dg;
            DG.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }
 
    }
}