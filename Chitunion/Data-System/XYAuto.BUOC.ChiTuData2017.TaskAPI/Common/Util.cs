/********************************************************
*创建人：hant
*创建时间：2017/12/18 15:34:53 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace XYAuto.BUOC.ChiTuData2017.TaskAPI.Common
{
    public class Util
    {
        /// <summary>
        /// 根据查询结果，返回通用Json格式
        /// </summary>
        /// <param name="obj">查询结果</param>
        /// <param name="message">返回Message信息，默认为：OK</param>
        /// <param name="status">返回status信息，默认为：0</param>
        /// <returns>返回Json数据</returns>
        public static JsonResult GetJsonDataByResult(object obj,string status,string message)
        {
            var jr = new JsonResult();
            jr.code = status;
            jr.msg = message;
            if (obj != null)
            {
                jr.data = obj;
            }
            return jr;
        }


    }
}