using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web
{
    public partial class Default5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            int CarSerialID = 0;
            int CarMasterID = 0;
            //BLL.CarTypeAPI.Instance.GetSerialIDAndMasterBrandIDByCarTypeID(14569, out CarSerialID, out CarMasterID);


            //Response.Write(Request["q1"].ToString());
            //DataSet ds=new DataSet();
            //ds=;
            int s = 0;
            int m = 0;
            //BLL.CarTypeAPI.Instance.GetSerialIDAndMasterBrandIDByCarTypeID(16263, out s, out m);
            //Response.Write("<script>alert('" + s + "|" + m + "')</script>");
            //Response.Write("<script>alert('" + BLL.CarTypeAPI.Instance.GetMasterBrandNameByMasterBrandID(8) + "|" +
            //    BLL.CarTypeAPI.Instance.GetSerialNameBySerialID(1905) + "|" +
            //    BLL.CarTypeAPI.Instance.GetCarTypeNameByCarTypeID(6165)
            //    + "');</script>");
        }
    }
}