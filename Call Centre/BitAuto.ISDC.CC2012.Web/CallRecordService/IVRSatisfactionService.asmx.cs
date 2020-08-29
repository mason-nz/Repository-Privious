using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace BitAuto.ISDC.CC2012.Web.CallRecordService
{
    /// <summary>
    /// Summary description for CallRecordService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class IVRSatisfactionService : System.Web.Services.WebService
    {

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        [WebMethod(Description = "新增IVR满意度数据")]
        public bool InsertIVRSatisfaction(Entities.IVRSatisfaction model, ref string msg)
        {
            if (model == null || (model != null && model.CallID == null))
            {
                msg = "参数IVRSatisfaction的Model为空"; return false;
            }

            InsertIVRSatisfactionRecord(model);

            return true;
        }

        [WebMethod(Description = "新增IVR满意度数据（仅适用于Genesys）")]
        public bool InsertIVRSatisfactionByGenesys(string NewCallID, string CallID, int score, ref string msg)
        {
            BLL.Loger.Log4Net.Info("[InsertIVRSatisfactionByGenesys]：IVR满意度数据插入开始,CallID：" + CallID + ",Score:"+ score + ",NewCallID:"+ NewCallID);
            bool flag=false;
            try
            {
                if (string.IsNullOrEmpty(NewCallID) || string.IsNullOrEmpty(CallID))
                {
                    msg = "参数NewCallID或CallID为空";
                }

                Entities.IVRSatisfaction model = new Entities.IVRSatisfaction();
                Int64 _callid = 0;
                if (Int64.TryParse(NewCallID, out _callid))
                {
                    model.CallRecordID = _callid;
                    if (Int64.TryParse(CallID, out _callid))
                    {
                        model.CallID = _callid;
                        model.Score = score;
                        model.CreateTime = DateTime.Now;
                       flag= InsertIVRSatisfactionRecord(model)>0?true:false;
                    }
                    else
                    {
                        msg = "参数CallID，格式不正确，必须是Long类型的"; 
                    }
                }
                else
                {
                    msg = "参数NewCallID，格式不正确，必须是Long类型的";
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[IVRSatifactionService.asmx]新增满意度数据出错",ex); return false;
            }
            if (!string.IsNullOrEmpty(msg))
            { BLL.Loger.Log4Net.Info("[IVRSatifactionService.asmx]新增满意度数据出错："+msg); }

            BLL.Loger.Log4Net.Info("[InsertIVRSatisfactionByGenesys]：IVR满意度数据插入结束,CallID：" + CallID + ",Score:" + score);
            return flag;
        }

        [WebMethod(Description = "新增IVR满意度数据（仅适用于青牛）")]
        public bool InsertIVRSatisfactionByHYUC(string NewCallID, string CallID, int score, ref string msg)
        {
            BLL.Loger.Log4Net.Info("[InsertIVRSatisfactionByHYUC]：IVR满意度数据插入开始,CallID：" + CallID + ",Score:" + score + ",NewCallID:" + NewCallID);
            bool flag = false;
            try
            {
                if (string.IsNullOrEmpty(NewCallID) || string.IsNullOrEmpty(CallID))
                {
                    msg = "参数NewCallID或CallID为空";
                }
                //根据青牛传过来的SessionID查话务总表拿到CallID
                Entities.IVRSatisfaction model = new Entities.IVRSatisfaction();
                Entities.CallRecord_ORIG orig = new Entities.CallRecord_ORIG();
                orig = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGBySessionID(NewCallID);
                if (orig == null)
                {
                    msg = "没有找到话务记录";
                    BLL.Loger.Log4Net.Info("[InsertIVRSatisfactionByHYUC]：没有找到话务记录,SessionID：" + NewCallID + ",Score:" + score);
                    return flag;
                }

                Int64 _callid = 0;
                model.CallRecordID = Convert.ToInt64(orig.CallID);
                if (Int64.TryParse(CallID, out _callid))
                {
                    model.CallID = _callid;
                    model.Score = score;
                    model.CreateTime = DateTime.Now;
                    flag = InsertIVRSatisfactionRecord(model) > 0 ? true : false;
                }
                else
                {
                    msg = "参数CallID，格式不正确，必须是Long类型的";
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("[InsertIVRSatisfactionByHYUC]新增满意度数据出错", ex); return false;
            }
            if (!string.IsNullOrEmpty(msg))
            { BLL.Loger.Log4Net.Info("[InsertIVRSatisfactionByHYUC]新增满意度数据出错：" + msg); }

            BLL.Loger.Log4Net.Info("[InsertIVRSatisfactionByHYUC]：IVR满意度数据插入结束,CallID：" + CallID + ",Score:" + score);
            return flag;
        }
        #region 插入

        private int InsertIVRSatisfactionRecord(Entities.IVRSatisfaction model)
        {
            return BitAuto.ISDC.CC2012.BLL.IVRSatisfaction.Instance.Insert(model);
        }

        #endregion
    }
}
