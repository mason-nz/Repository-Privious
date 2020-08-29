using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Services;
using BitAuto.ISDC.CC2012.BLL;
namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_Contact
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Handler : IHttpHandler, IRequiresSessionState
    {
        CC_Contact_Helper helper = new CC_Contact_Helper();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                bool success = true;
                string result = "";
                string msg = "";

                switch (helper.Action.ToLower().Trim())
                {
                    case "addcontact":
                        helper.AddContact();
                        break;
                    case "editcontact":
                        helper.EditContact();
                        break;
                    case "deletecontact":
                        helper.DeleteContact();
                        break;
                    case "showcontact"://查询联系人
                        msg = helper.ShowContact();
                        break;
                    case "getallcontactofcust":
                        helper.GetAllContactOfCust();
                        break;
                    case "bindcontactdepartment"://绑定联系人部门
                        msg = helper.BindContactDepartment();
                        break;
                    case "getmanagecontactinfo"://判断会员是否存在车易通负责人
                        msg = helper.GetManageContactInfo();
                        break;
                    case "getmember"://获取关联会员
                        msg = helper.GetMappingMember();
                        break;
                    default:
                        success = false;
                        msg = "请求参数错误";
                        break;
                }
                AJAXHelper.WrapJsonResponse(success, result, msg);
            }
            catch (Exception ex)
            {
                AJAXHelper.WrapJsonResponse(false, "", ex.Message);
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