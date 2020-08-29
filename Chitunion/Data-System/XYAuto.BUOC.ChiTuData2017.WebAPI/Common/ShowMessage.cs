using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.Common
{
    public class ShowMessage
    {
       
        public static void ShowMessageBox(string strMsg)
        {
            System.Web.HttpContext.Current.Response.Write("<script src=\"../ Content/layer/jquery.1.11.3.min.js\"></script><script src=\"../Content/layer/layer.js\" ></script ><script type='text/javascript'>alert('" + strMsg + "');</script>");
        }

    }
}