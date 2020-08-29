using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TaskManager.NoDealerOrder
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {

        /// <summary>
        /// 车型ID
        /// </summary>
        public string CarTypeID
        {
            get
            {
                return HttpContext.Current.Request["CarTypeID"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["CarTypeID"].ToString());
            }
        }

        /// <summary>
        /// 车的价格
        /// </summary>
        public string CarPrice
        {
            get
            {
                return HttpContext.Current.Request["CarPrice"] == null ? string.Empty : HttpUtility.UrlDecode(HttpContext.Current.Request["CarPrice"].ToString());
            }
        }


        public void ProcessRequest(HttpContext context)
        {
            CustBaseInfoHelper helper = new CustBaseInfoHelper();
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (helper.Action.ToLower())
            {
                case "revoketask":
                    try
                    {
                        helper.RevokeTask(out msg);
                    }
                    catch (Exception ex)
                    {
                        msg = "{Result:'no',CustID:'0',ErrorMsg:'" + ex.Message + "'}";
                    }
                    break;
                case "assigntask":
                    try
                    {
                        helper.AssignTask(out msg);
                    }
                    catch (Exception ex)
                    {
                        msg = "{Result:'no',CustID:'0',ErrorMsg:'" + ex.Message + "'}";
                    }
                    break;
                case "updatecustbaseinfo":
                    try
                    {
                        helper.SubmitCheckInfo(true, out msg);
                    }
                    catch (Exception ex)
                    {
                        msg = "{Result:'no',CustID:'0',ErrorMsg:'" + ex.Message + "'}";
                    }
                    break;
                case "checkprice":

                    msg = CheckPrice(CarTypeID, CarPrice);
                    break;
            }
            context.Response.Write(msg);
        }


        public string CheckPrice(string CarTypeID, string CarPrice)
        {
            string msg = "";

            int intVal = 0;
            decimal decVal = 0;

            if (!int.TryParse(CarTypeID, out intVal))
            {
                msg = "车型ID不是数字格式";
            }
            else if (!decimal.TryParse(CarPrice, out decVal))
            {
                msg = "预售价格不是数字格式";
            }
            else
            {
                decimal zdPrice = BLL.CarTypeAPI.Instance.GetCarReferPriceByCarTypeID(int.Parse(CarTypeID));

                //这里 CarPrice 是预售价格
                if (decimal.Parse(CarPrice) > zdPrice * (decimal)1.5 || decimal.Parse(CarPrice) < 0)
                {
                    msg += "预售价格不能小于0，并且不能大于厂商指导价的1.5倍（" + (zdPrice * (decimal)1.5).ToString() + "）【厂商指导价：" + (zdPrice).ToString() + "】<br/>";
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