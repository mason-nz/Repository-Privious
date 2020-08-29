﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace XYAuto.BUOC.ChiTuData2017.TaskAPI.Filter
{
    public class JsonCallbackAttribute: ActionFilterAttribute
    {
        private const string CallbackQueryParameter = "jsonpcallback";

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            var callback = string.Empty;

            if (IsJsonp(out callback))
            {
                var jsonBuilder = new StringBuilder(callback);
                jsonBuilder.AppendFormat("({0})", context.Response.Content.ReadAsStringAsync().Result);
                context.Response.Content = new StringContent(jsonBuilder.ToString());
            }

            base.OnActionExecuted(context);
        }

        private bool IsJsonp(out string callback)
        {
            callback = System.Web.HttpContext.Current.Request.QueryString[CallbackQueryParameter];
            return !string.IsNullOrEmpty(callback);
        }
    }
}