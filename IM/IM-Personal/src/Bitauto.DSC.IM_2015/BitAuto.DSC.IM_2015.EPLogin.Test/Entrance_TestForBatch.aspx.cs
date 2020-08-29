using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.Utils;
using System.Data;
namespace BitAuto.DSC.IM_2015.EPLogin.Test
{
    public partial class Entrance_TestForBatch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Random r = new Random();
                int rnum = r.Next(1000, 9999);
                int[] rnumarray = new int[10];

                for (int i = 0; i < rnumarray.Length; i++)
                {
                    rnumarray[i] = rnum;
                    txtUserID.Value += rnumarray[i].ToString() + ",";
                    rnum++;
                }
                txtUserID.Value = txtUserID.Value.Substring(0, txtUserID.Value.Length - 1);
                txtMemberCode.Value = "50002218";
                //btnChanged_Click(null, null);
            }
        }


    }
}