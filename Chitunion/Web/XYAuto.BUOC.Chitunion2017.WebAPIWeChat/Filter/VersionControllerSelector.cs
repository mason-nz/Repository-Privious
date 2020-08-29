using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dispatcher;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Filter
{
    public class VersionControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _config;

        public VersionControllerSelector(HttpConfiguration config)
            : base(config)
        {
            _config = config;
        }

        //设计就是返回HttpControllerDesriptor的过程
        public override System.Web.Http.Controllers.HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            //获取所有的controller键值集合
            var controllers = GetControllerMapping();

            //获取路由数据
            var routeData = request.GetRouteData();
            string controllerName1 = this.GetControllerName(request);
            //从路由中获取当前controller的名称
            var controllerName = (string)routeData.Values["controller"];

            HttpControllerDescriptor descriptor;

            if (controllers.TryGetValue(controllerName, out descriptor))
            {
                //从QueryString中获取版本号
                var version = GetVersionFromQueryString(request);

                var newName = string.Concat(controllerName, "V", version);

                HttpControllerDescriptor versionedDescriptor;

                if (controllers.TryGetValue(newName, out versionedDescriptor))
                {
                    return versionedDescriptor;
                }
                return descriptor;
            }
            return null;
        }

        /// <summary>
        /// 从QueryString中获取版本号
        /// </summary>
        /// <param name="request">Request请求对象</param>
        /// <returns>返回url参数v的内容，若找不到则返回空</returns>
        private string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            var version = query["v"];
            if (version != null)
            {
                return version;
            }
            return string.Empty;
        }
    }
}