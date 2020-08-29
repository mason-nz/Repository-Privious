﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XYAuto.BUOC.Chitunion2017.NewWeb
{
    public partial class CheckCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Common.ValidateCode vc = new Common.ValidateCode();
            string code = vc.GetRandomCode(4);
            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.ContentType = "image/Gif";
            System.Web.HttpContext.Current.Response.BinaryWrite(vc.CreateValidateGraphic(code));
            Session["ValidateCode"] = code;
        }
    }
}