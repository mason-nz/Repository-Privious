using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.GroupOrder.ImportExcelWriteBack
{
    public partial class ExcelHelper : PageBase
    {
        public string RequestStr
        {
            get { return BLL.Util.GetCurrentRequestStr("hidData"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ExprotExcel(RequestStr);
            }
        }

        public void ExprotExcel(List<ExcelData> datalist)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("电话号码\t,处理结果\t,任务ID\t,错误信息");

            foreach (ExcelData d in datalist)
            {
                sw.WriteLine(d.Phone + "\t," + d.ReturnVisit + "\t," + d.TaskID + "\t," + d.Msg);
            }
            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename=FailExportData.csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();

        }

        public void ExprotExcel(string strData)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("电话号码\t,任务ID\t,错误信息");

            string[] arrayMain = strData.Split(';');
            foreach (string main in arrayMain)
            {
                string[] arraySub = main.Split(',');
                sw.WriteLine(arraySub[0] + "\t," + arraySub[1] + "\t," + arraySub[2]);
            }

            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename=FailExportData.csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();

        }
    }

    public class ExcelData
    {
        public string Phone;
        public string ReturnVisit;
        public string TaskID;
        public string Msg;
    }
}