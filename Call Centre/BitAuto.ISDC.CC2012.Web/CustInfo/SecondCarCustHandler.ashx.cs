using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.Web.Util;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.CustInfo
{
    /// <summary>
    /// SecondCarCustHandler 的摘要说明
    /// </summary>
    public class SecondCarCustHandler : IHttpHandler, IRequiresSessionState
    {

        SecondCarCustInfoHelper helper = new SecondCarCustInfoHelper();

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
                bool success = true;
                string result = "";
                string msg = "";

                switch (helper.Action)
                {
                    case "verifysavecheckinfo"://验证保存核实信息
                        helper.VerifySaveCheckInfo();
                        break;
                    case "savecheckinfo"://保存核实信息
                        helper.SaveCheckInfo();
                        break;
                    //case "savecustinfo"://保存核实中的客户信息
                    //    helper.SaveCustInfo();
                    //    break;
                    case "submitcheckinfo"://提交核实信息
                        helper.SubmitCheckInfo();
                        break;
                    case "deletecheckinfo"://提交删除核实信息
                        helper.DeleteCheckInfo();
                        break;
                    case "custnameexist"://用户名是否存在
                        result = helper.CustNameExist(out msg) ? "1" : "0";
                        break;
                    case "searchcustnamesamelist"://客户名称重复列表，包含（客户名称、客户编号、客户状态、锁定状态）
                        result = helper.GetCustNameList();
                        break;
                    case "adddeletecustrelationinfo"://添加删除客户关系记录
                        helper.AddDeleteCustRelationInfo();
                        break;
                    case "stopcheckinfo"://提交停用客户信息
                        helper.StopCustInfo();
                        break;
                    case "checkislock":
                        helper.CustIsLock();
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