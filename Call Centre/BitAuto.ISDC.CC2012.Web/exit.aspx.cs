using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils.Config;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web
{
    public partial class exit : CC2012.Web.Base.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userName = Session["UserName"].ToString();
            BitAuto.YanFa.SysRightManager.Common.UserInfo.clear();
            if (userName.Contains("WB"))
            {
                Response.Redirect(ConfigurationUtil.GetAppSettingValue("ExitAddressWB"));
            }
            else
            {
                Response.Redirect(ConfigurationUtil.GetAppSettingValue("ExitAddress"));
            }
            //CallRecordInfoCondition m = new CallRecordInfoCondition();
            //m.BGID = 19;
            //m.SCID = 161;
            //m.BusinessID = "1405211536091042474";
            //int count = 0;
            //string Msg = "";
            //DataTable dt = GetDemandCallDetail("F84EB544-D927-4699-8051-12098C806FD8", m, 2, 2, out count, out Msg);

        }
        //public DataTable GetDemandCallDetail(string Verifycode, CallRecordInfoCondition model, int PageIndex, int PageSize, out int RecordCount, out string Msg)
        //{
        //    RecordCount = 0;
        //    Msg = string.Empty;
        //    DataTable dt = new DataTable("callRecordInfo");
        //    string[] arr = new string[7] { "CallID", "BeginDateTime", "EndDateTime", "CreateUserID", "AudioURL", "EstablishedTime", "Content" };
        //    for (var i = 0; i < arr.Length; i++)
        //    {
        //        dt.Columns.Add(arr[i]);
        //    }

        //    try
        //    {
        //        if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref Msg, "易集客调用获取话务数据，授权失败。"))
        //        {
        //            BLL.Loger.Log4Net.Info("【拉取需求单客户通知接口】出错：验证失败，" + Msg);
        //            return dt;
        //        }

        //        //验证
        //        if (model.BGID == 0 || model.SCID == 0 || model.BusinessID == string.Empty)
        //        {
        //            Msg = "业务组ID、分类ID和业务ID不能为空！";
        //            BLL.Loger.Log4Net.Info("【易集客获取话务数据接口】出错：" + Msg);
        //            return dt;
        //        }

        //        string where = " And CallID in (select CallID From CallRecord_ORIG_Business cob where cob.BGID=" + model.BGID + " and cob.SCID=" + model.SCID + " and BusinessID=" + model.BusinessID + ")";
        //        DataTable dt_ORIG = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByYiJiKe(where, "", PageIndex, PageSize, out RecordCount);
        //        for (int i = 0, len = dt_ORIG.Rows.Count; i < len; i++)
        //        {
        //            string endTime = dt_ORIG.Rows[i]["CustomerReleaseTime"].ToString();
        //            if (string.IsNullOrEmpty(endTime))
        //            {
        //                endTime = dt_ORIG.Rows[i]["AgentReleaseTime"].ToString();
        //            }
        //            DateTime time1;
        //            DateTime time2;
        //            if (DateTime.TryParse(dt_ORIG.Rows[i]["CustomerReleaseTime"].ToString(), out time1) && DateTime.TryParse(dt_ORIG.Rows[i]["AgentReleaseTime"].ToString(), out time2))
        //            {
        //                if (time1 > time2)
        //                {
        //                    endTime = dt_ORIG.Rows[i]["AgentReleaseTime"].ToString();
        //                }
        //            }
        //            DataRow dr = dt.NewRow();
        //            dr["CallID"] = dt_ORIG.Rows[i]["CallID"].ToString();
        //            dr["BeginDateTime"] = dt_ORIG.Rows[i]["InitiatedTime"].ToString();
        //            dr["EndDateTime"] = endTime;
        //            dr["CreateUserID"] = dt_ORIG.Rows[i]["CreateUserID"].ToString();
        //            dr["AudioURL"] = dt_ORIG.Rows[i]["AudioURL"].ToString();
        //            dr["EstablishedTime"] = dt_ORIG.Rows[i]["EstablishedTime"].ToString();
        //            dr["Content"] = dt_ORIG.Rows[i]["Content"].ToString();
        //            dt.Rows.Add(dr);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Msg = ex.Message;
        //        BLL.Loger.Log4Net.Info("【易集客获取话务数据接口】出错：" + Msg);
        //        return dt;
        //    }

        //    BLL.Loger.Log4Net.Info("【易集客获取话务数据接口】成功，BGID：" + model.BGID + "，SCID：" + model.SCID + "，BusinessID：" + model.BusinessID + "，总记录数：" + RecordCount);
        //    return dt;

        //}

    }
    //public class CallRecordInfoCondition
    //{
    //    private int _bgid = 0;
    //    private int _scid = 0;
    //    private string _businessid = "";

    //    /// <summary>
    //    /// 按业务组查询
    //    /// </summary>
    //    public int BGID
    //    {
    //        get { return _bgid; }
    //        set { _bgid = value; }
    //    }

    //    /// <summary>
    //    /// 按分类查询
    //    /// </summary>
    //    public int SCID
    //    {
    //        get { return _scid; }
    //        set { _scid = value; }
    //    }

    //    /// <summary>
    //    /// 按业务ID查询
    //    /// </summary>
    //    public string BusinessID
    //    {
    //        get { return _businessid; }
    //        set { _businessid = value; }
    //    }
    //}

}