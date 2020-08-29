using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustCategory
{
    /// <summary>
    /// DealerInfo 的摘要说明
    /// </summary>
    public class DealerInfo : IHttpHandler, IRequiresSessionState
    {
        #region
        /// <summary>
        /// 显示方式
        /// </summary>
        public string ShowType
        {
            get
            {
                return HttpContext.Current.Request["ShowType"].ToString();
            }
        }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string MemberName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["MemberName"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["MemberName"]);
            }
        }
        /// <summary>
        /// 请求分类
        /// </summary>
        private string RequestAction
        {
            get
            {
                return HttpContext.Current.Request["Action"].ToString();
            }
        }

        private string MemberCode
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["MemberCode"]) == true ? string.Empty : HttpContext.Current.Request["MemberCode"];
            }
        }
        private string Remark
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Remark"]) == true ? string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Remark"]);
            }
        }
        private int CityScope
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CityScope"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["CityScope"]);
            }
        }
        private int MemberType
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["MemberType"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["MemberType"]);
            }
        }

        //经营范围
        private int CarType
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CarType"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["CarType"]);
            }
        }
        //经销商状态
        private int MemberStatus
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["MemberStatus"]) == true ? -2 : Convert.ToInt32(HttpContext.Current.Request["MemberStatus"]);
            }
        }
        //客户id
        private string CustID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["CustID"]) == true ? string.Empty : HttpContext.Current.Request["CustID"];
            }
        }

        private string BrandID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["BrandID"]) == true ?
                   string.Empty : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["BrandID"]);
            }
        }
        #endregion

        #region 属性

        private string ContactID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["ContactID"]) == true ? string.Empty : HttpContext.Current.Request["ContactID"];
            }
        }
        private string NewEmail
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["NewEmail"]) == true ? string.Empty : HttpContext.Current.Request["NewEmail"];
            }
        }
        private string OldEmail
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["OldEmail"]) == true ? string.Empty : HttpContext.Current.Request["OldEmail"];
            }
        }

        #endregion
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (RequestAction)
            {
                case "GetCityScope":
                    GetHtml(typeof(CityScope), out msg);
                    break;
                case "GetMemberType":
                    GetHtml(typeof(DealerCategory), out msg);
                    break;
                case "GetCarType":
                    GetHtml(typeof(CarType), out msg);
                    break;
                case "GetMemberStatus":
                    GetHtml(typeof(MemberStatus), out msg);
                    break;
                //case "GetMemberInfo":
                //    GetMemberInfo(out msg);
                //    break;
                case "SubDealerInfo":
                    SubDealerInfo(out msg);
                    break;
                case "GetYiPaiMember": GetYiPaiMember(out msg);//得到易湃会员负责人
                    break;
                case "UpdateEmail": UpdateEmail(out msg);
                    break;
                case "SendEmail": SendEmail(out msg);
                    break;
                case "GetCustInfo":
                    GetCustInfo(out msg);
                    break;
                case "GetCustInfoByMemberCode":
                    GetCustInfoByMemberCode(out msg);
                    break;
            }
            context.Response.Write(msg);
        }
        private void GetCustInfo(out string msg)
        {
            msg = string.Empty;
            if (!string.IsNullOrEmpty(CustID))
            {
                BitAuto.YanFa.Crm2009.Entities.CustInfo custinfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(CustID);
                if (custinfo != null)
                {
                    msg = "{result:'true',CustName:'" + custinfo.CustName + "',PrivinceID:'" + custinfo.ProvinceID + "',CityID:'" + custinfo.CityID + "',CountyID:'" + custinfo.CountyID + "'}";
                }
                else
                {
                    msg = "{result:'false'}";
                }
            }

        }
        private void GetCustInfoByMemberCode(out string msg)
        {
            msg = string.Empty;
            if (!string.IsNullOrEmpty(MemberCode))
            {
                BitAuto.YanFa.Crm2009.Entities.DMSMember model = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByMemberCode(MemberCode);

                if (model != null)
                {
                    BitAuto.YanFa.Crm2009.Entities.CustInfo custinfo = BitAuto.YanFa.Crm2009.BLL.CustInfo.Instance.GetCustInfo(model.CustID);
                    if (custinfo != null)
                    {
                        msg = "{result:'true',CustName:'" + custinfo.CustName + "',CustID:'" + model.CustID + "',PrivinceID:'" + custinfo.ProvinceID + "',CityID:'" + custinfo.CityID + "',CountyID:'" + custinfo.CountyID + "'}";
                    }
                    else
                    {
                        msg = "{result:'false'}";
                    }
                }
                else
                {
                    msg = "{result:'false'}";
                }
            }

        }

        //得到易湃会员负责人
        private void GetYiPaiMember(out string msg)
        {
            msg = string.Empty;

            if (MemberCode == string.Empty)
            {
                msg = "{result:'false',msg:'会员编码不能为空！操作失败'}";
            }

            try
            {
                //DataTable dt = WebService.CRMCustomerServiceHelper.Instance.GetMemberContactByMemberCode(MemberCode);
                string userName = WebService.EasypassRestPwdServiceHelper.Instance.GetAccountLoginNameByDealerId(int.Parse(MemberCode));
                if (!string.IsNullOrEmpty(userName))
                {
                    msg = "{result:'true',Email:'" + userName + "'}";
                }
                else
                {
                    msg = "{result:'false',msg:'经销商已经不可用<br/>经销商不存在或未同步'}";
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'false',msg:'" + ex.Message + "'}";
            }
        }

        //修改联系人的email
        private void UpdateEmail(out string msg)
        {
            msg = "{result:'false',msg:'email或联系人ID出错！操作失败'}";

            //int _contactID = 0;
            if (NewEmail == "" || MemberCode == "")
            {
                return;
            }

            //String EmailReg = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            String EmailReg = @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(NewEmail, EmailReg))
            {
                msg = "{result:'false',msg:'邮箱格式错误'}";
                return;
            }

            //DataTable dt = WebService.CRMCustomerServiceHelper.Instance.GetMemberContactByMemberCode(MemberCode);
            //if (dt.Rows.Count == 0)
            //{
            //    return;
            //}

            //if (!int.TryParse(dt.Rows[0]["ContactID"].ToString(), out _contactID))
            //{
            //    return;
            //}

            string returnMsg = string.Empty;
            string logDesc = "";

            bool updateResult = false;
            try
            {
                //updateResult = WebService.CRMCustomerServiceHelper.Instance.UpdateContactEmail(_contactID, Email, out returnMsg);
                returnMsg = WebService.EasypassRestPwdServiceHelper.Instance.UpdateAccountLoginNameByDealerId(int.Parse(MemberCode), NewEmail);
                updateResult = string.IsNullOrEmpty(returnMsg) ? true : false;
            }
            catch (Exception ex)
            {
                logDesc = "【客户池易湃会员修改邮箱】失败！出现异常，异常信息：" + ex.Message;
                BLL.Util.InsertUserLog(logDesc);
                return;
            }

            if (!updateResult)
            {
                msg = "{result:'false',msg:'" + returnMsg + "'}";
                logDesc = "【客户池易湃会员修改邮箱】失败，接口返回失败信息为：" + returnMsg + "：修改前邮箱为【" + OldEmail + "】，会员编码为【" + MemberCode + "】修改邮箱失败！";
            }
            else
            {
                logDesc = "【客户池易湃会员修改邮箱】成功：修改前邮箱为【" + OldEmail + "】，修改后邮箱为【" + NewEmail + "】，会员编码为【" + MemberCode + "】";
                msg = "{result:'true',msg:'操作成功'}";
            }

            //记日志
            BLL.Util.InsertUserLog(logDesc);

        }

        //发送修改密码邮件
        private void SendEmail(out string msg)
        {
            msg = string.Empty;
            if (MemberCode == "")
            {
                msg = "{result:'false',msg:'会员编码不能为空！操作失败'}";
                return;
            }

            try
            {
                bool sendResult = WebService.EasypassRestPwdServiceHelper.Instance.GetLinkByUserLoginName(MemberCode);
                string logDesc = string.Empty;
                if (sendResult)
                {
                    logDesc = "【客户池发送修改密码邮件】接口返回成功！会员编码为：【" + MemberCode + "】";
                    msg = "{result:'true',msg:'已发送邮件！操作成功'}";
                }
                else
                {
                    logDesc = "【客户池发送修改密码邮件】接口返回失败！会员编码为：【" + MemberCode + "】";
                    msg = "{result:'false',msg:'调用接口返回失败！发送邮件失败'}";
                }

                BLL.Util.InsertUserLog(logDesc);
            }
            catch (Exception ex)
            {
                msg = "{result:'true',msg:'MemberCode是" + MemberCode + "；" + ex.Message + "'}";
                BLL.Util.InsertUserLog("【客户池发送修改密码邮件】接口出现异常！异常信息为：" + ex.Message);
            }
        }

        /// <summary>
        /// 提交经销商信息
        /// </summary>
        /// <param name="msg"></param>
        public void SubDealerInfo(out string msg)
        {
            msg = string.Empty;
            Entities.DealerInfo Model = new Entities.DealerInfo();
            Model.Name = MemberName;
            BitAuto.YanFa.Crm2009.Entities.QueryDMSMember query = new YanFa.Crm2009.Entities.QueryDMSMember();
            query.Name = Model.Name;
            int DMSCount = 0;
            //在提交时根据经销商名称判断会员编号是否存在
            DataTable dtMember = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(query, "", 1, 100000, out DMSCount);
            if (dtMember != null && dtMember.Rows.Count > 0)
            {
                Model.MemberCode = dtMember.Rows[0]["MemberCode"].ToString();
            }
            else
            {
                Model.MemberCode = "";
            }
            Model.Remark = Remark;
            Model.CustID = CustID;
            Model.Status = 0;
            Model.CreateTime = System.DateTime.Now;
            Model.CreateUserID = (int?)BLL.Util.GetLoginUserID();
            Model.CarType = CarType;
            Model.CityScope = CityScope;
            Model.MemberStatus = MemberStatus;
            Model.MemberType = MemberType;
            try
            {
                //判断是否有已购车，未购车记录
                if (BLL.BuyCarInfo.Instance.IsExistsByCustID(Model.CustID))
                {
                    BLL.BuyCarInfo.Instance.Delete(Model.CustID);
                    //BLL.Util.InsertUserLog("删除客户id为" + Model.CustID + "，的已购车未购车。");
                }
                int DealerID = 0;
                //判断是否存在经销商信息
                if (BLL.DealerInfo.Instance.IsExistsByCustID(Model.CustID))
                {
                    DealerID = BLL.DealerInfo.Instance.Update(Model);
                    BLL.Util.InsertUserLog("更新客户id为" + Model.CustID + "，的经销商信息。");
                }
                else
                {
                    DealerID = BLL.DealerInfo.Instance.Insert(Model);
                    BLL.Util.InsertUserLog("插入客户id为" + Model.CustID + "，的经销商信息。");
                }
                BLL.DealerBrandInfo.Instance.Delete(Model.CustID);
                //BLL.Util.InsertUserLog("删除客户id为" + Model.CustID + "，的经销商品牌信息。");
                if (BrandID != string.Empty)
                {
                    string[] brandids = BrandID.Split(',');
                    if (brandids != null && brandids.Length > 0)
                    {
                        for (int i = 0; i < brandids.Length; i++)
                        {
                            DealerBrandInfo Brandmodel = new DealerBrandInfo();
                            Brandmodel.CustID = CustID;
                            Brandmodel.DealerID = DealerID;
                            if (brandids[i] != "")
                            {
                                Brandmodel.BrandID = Convert.ToInt32(brandids[i]);
                            }
                            BLL.DealerBrandInfo.Instance.Insert(Brandmodel);
                        }
                        BLL.Util.InsertUserLog("插入客户id为" + Model.CustID + "，的经销商品牌信息。");
                    }
                }
                msg = "success";
            }
            catch (Exception ex)
            {
                msg = "经销商信息保存失败，失败原因：" + ex.Message.ToString();
            }
        }
        /// <summary>
        /// 取车易通会员信息
        /// </summary>
        /// <param name="msg"></param>
        //private void GetMemberInfo(out string msg)
        //{
        //    //msg = string.Empty;
        //    //if (!string.IsNullOrEmpty(MemberName))
        //    //{
        //    //    DataTable dt = BLL.DealerInfo.Instance.GetMemberInfo(MemberName);
        //    //    if (dt != null && dt.Rows.Count > 0)
        //    //    {
        //    //        msg += "{root:[";

        //    //        msg += "{'MemberCode':'" + dt.Rows[0]["MemberCode"] + "',";
        //    //        msg += "'Brandids':'" + dt.Rows[0]["brandids"] + "',";
        //    //        msg += "'MemberType':'" + dt.Rows[0]["membertype"] + "',";
        //    //        msg += "'BrandNames':'" + dt.Rows[0]["brandnames"] + "'}";
        //    //        msg += "]}";
        //    //    }
        //    //}
        //}

        public string GetHtml(Type enumName, out string msg)
        {
            DataTable dt = null;
            msg = string.Empty;
            dt = BLL.Util.GetDataFromEnum(enumName);
            //if (ShowType == "select")
            //{
            //    msg = "<span><select id='memberType' class='w195'><option value='-2' selected='selected'>请选择</option>";
            //}
            foreach (DataRow dr in dt.Rows)
            {
                if (ShowType == "rdo")
                {
                    msg = msg + "<span><input  id='" + enumName.Name + "_" + dr["ID"].ToString() + "' class='marginleft' value='" + dr["ID"].ToString() + "' type='radio'  name='" + enumName.Name + "'/>" + dr["Name"].ToString() + "</span>&nbsp;";
                }
                else if (ShowType == "cbx")
                {
                    msg = msg + "<li><input id='" + enumName.Name + "_" + dr["ID"].ToString() + "' class='marginleft' value='" + dr["ID"].ToString() + "'  type='checkbox' name='" + enumName.Name + "'>" + dr["Name"].ToString();
                }
                else if (ShowType == "select")
                {
                    msg += "<option  value='" + dr["ID"] + "'>" + dr["Name"] + "</option>";
                }
            }
            return msg;
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