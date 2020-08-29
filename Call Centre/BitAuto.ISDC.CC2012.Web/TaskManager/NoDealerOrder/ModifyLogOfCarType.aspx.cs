using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.AjaxServers.ExcelOperate;

namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder
{
    public partial class ModifyLogOfCarType : System.Web.UI.Page
    {

       
        public string Action
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["action"]) ? "":
                    HttpUtility.UrlDecode(HttpContext.Current.Request["action"].ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            if (Action == "export")
            {
                ExcelExport();
            }
            else
            {
                BindingData();
            }
       
        }

        private void ExcelExport()
        {
            DataTable dt = GetData();

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            if (dt != null)
            {
                ExcelInOut.CreateEXCEL(ds, "Log" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"));
            }
        }


        public void BindingData()
        {
            DataTable dt = GetData();

            if (dt != null)
            {
                this.rpCarList.DataSource = dt;
                this.rpCarList.DataBind();
            }
        }

        private static DataTable GetData()
        {
            DataTable dt = BLL.OrderNewCar.Instance.GetModifyCar();

            #region 计算车型名称

            dt.Columns.Add("原车款名称");
            dt.Columns.Add("修改后的车型名称");

            int intval = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (int.TryParse(dr["原车款ID"].ToString(), out intval))
                {
                    dr["原车款名称"] = BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(intval);
                }

                if (int.TryParse(dr["修改后的车型ID"].ToString(), out intval))
                {
                    dr["修改后的车型名称"] = BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(intval);
                }
            }

            #endregion
            return dt;
        }
    }
}