using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace BitAuto.ISDC.CC2012.Web.CallRecordService
{
    /// <summary>
    /// Summary description for STDATA_USER_NOAGGService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class STDATA_USER_NOAGGService : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        [WebMethod(Description = "新增坐席登录时长统计数据")]
        public bool InsertSTDATA_USER_NOAGG(Entities.STDATA_USER_NOAGG model, ref string msg)
        {
            if (model == null || (model != null && model.User_Name == ""))
            {
                msg = "参数STDATA_USER_NOAGG的Model为空"; return false;
            }

            InsertSTDATA_USER_NOAGGRecord(model);

            return true;
        }

        #region 插入

        private int InsertSTDATA_USER_NOAGGRecord(Entities.STDATA_USER_NOAGG model)
        {
            return BitAuto.ISDC.CC2012.BLL.STDATA_USER_NOAGG.Instance.Insert(model);
        }

        #endregion
    }
}
