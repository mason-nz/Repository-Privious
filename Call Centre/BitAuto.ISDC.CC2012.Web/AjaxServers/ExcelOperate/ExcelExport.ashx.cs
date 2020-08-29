using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;


namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ExcelOperate
{
    /// <summary>
    /// ExcelExport 的摘要说明
    /// </summary>
    public class ExcelExport : IHttpHandler
    {

        /// <summary>
        /// 导出的数据分类
        /// </summary>
        public string DataType
        {
            get
            {
                if (HttpContext.Current.Request["t"] != null)
                {
                    return HttpContext.Current.Request["t"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 要导出的字段
        /// </summary>
        public string Fields
        {
            get
            {
                if (HttpContext.Current.Request["field"] != null)
                {
                    return System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request["field"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string Where
        {
            get
            {
                if (HttpContext.Current.Request["where"] != null)
                {
                    return System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request["where"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string FilePath = context.Server.MapPath("../ExcelOperate/UpLoad");
            DataTable dt = new DataTable();

            switch (DataType)
            {
                case "cust": ExcportCustom(FilePath); break;
                case "task": ExcportTask(FilePath); break;
            }


          context.Response.Write("succeed");
        }

        /// <summary>
        /// 导出任务数据
        /// </summary>
        private void ExcportTask(string FilePath)
        {
           // DataSet ds = BLL.CustHistoryInfo.Instance.GetDataToExport(Fields, Where);
            //if (ds != null)
            //{
            //    //ExcelInOut.CreateEXCEL(ds, DateTime.Now.ToString("YYYY-MM-DD-HH-mm-SS"));
            //    string err = "";
            //   // ExcelInOut.ExportDataExcel(ds.Tables[0], FilePath, DateTime.Now.ToString("YYYY-MM-DD-HH-mm-SS"), ref err);
            //}
            
        }

      

        /// <summary>
        /// 导出客户数据
        /// </summary>
        private void ExcportCustom(string FilePath)
        {
            DataSet ds = BLL.CustBasicInfo.Instance.GetDataToExport(Fields, Where);
            if (ds != null)
            {
               // ExcelInOut.CreateEXCEL(ds, DateTime.Now.ToString("YYYY-MM-DD-HH-mm-SS"));
                string err = "";
              
               // ExcelInOut.ExportDataExcel(ds.Tables[0], "", "", ref err);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}