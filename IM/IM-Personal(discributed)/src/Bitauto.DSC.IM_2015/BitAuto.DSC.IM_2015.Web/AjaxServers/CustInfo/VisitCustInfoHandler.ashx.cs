using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.DSC.IM_2015.Web.Channels;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.CustInfo
{
    /// <summary>
    /// VisitCustInfoHandler 的摘要说明
    /// </summary>
    public class VisitCustInfoHandler : IHttpHandler, IRequiresSessionState
    {
        public string custid
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["custid"]))
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["custid"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string usernamevalue
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["usernamevalue"]))
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["usernamevalue"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string selProvince
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["selProvince"]))
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["selProvince"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string selCity
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["selCity"]))
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["selCity"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string sex
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["sex"]))
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["sex"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string tel
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["tel"]))
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["tel"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string Remark
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["Remark"]))
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Remark"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string action
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["action"]))
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["action"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string VisitID
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request["VisitID"]))
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["VisitID"]);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (action)
            {
                case "save":
                    save(out msg);
                    break;
                default:
                    break;
            }
            context.Response.Write(msg);
        }
        private void save(out string msg)
        {
            string Rcustid = custid;
            msg = string.Empty;
            Entities.UserVisitLog model = null;
            if (!string.IsNullOrEmpty(VisitID))
            {
                int _visitid = 0;
                if (int.TryParse(VisitID, out _visitid))
                {
                    model = BLL.UserVisitLog.Instance.GetUserVisitLog(_visitid);
                    if (model == null)
                    {
                        msg = "访问记录不存在";
                        return;
                    }
                    model.Remark = Remark;
                    model.UserName = usernamevalue;
                    int _cityid = 0;
                    if (int.TryParse(selCity, out _cityid))
                    {
                        model.CityID = _cityid;
                    }
                    int _provinceid = 0;
                    if (int.TryParse(selProvince, out _provinceid))
                    {
                        model.ProvinceID = _provinceid;
                    }
                    int _sex = 0;
                    if (int.TryParse(sex, out _sex))
                    {
                        if (_sex == 1)
                        {
                            model.Sex = true;
                        }
                        else
                        {
                            model.Sex = false;
                        }
                    }
                    model.Phone = tel;
                    //姓名或手机为空，不调用cc接口
                    if (string.IsNullOrEmpty(tel) || string.IsNullOrEmpty(usernamevalue))
                    {
                        //不调用cc接口
                    }
                    else
                    {
                        //调用cc接口,把信息推到cc，cc返回custid，与是否成功
                        bool flag = BitAuto.DSC.IM_2015.WebService.CC.CCWebServiceHepler.Instance.CCDataInterface_InsertCustData(out msg, model, out Rcustid);
                        if (flag)
                        {
                            msg = "";
                        }
                    }
                    model.CustID = Rcustid;
                    BLL.UserVisitLog.Instance.UpdateUserVisitLog(model);
                    //填充内存网友访问信息
                    Core.CometClient modelclient = DefaultChannelHandler.StateManager.GetCometClient(model.LoginID);
                    if (modelclient != null)
                    {
                        modelclient.Userloginfo = model;
                    }
                }
                else
                {
                    msg = "访问记录不存在";
                    return;
                }
            }
            else
            {
                msg = "访问记录不存在";
                return;
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