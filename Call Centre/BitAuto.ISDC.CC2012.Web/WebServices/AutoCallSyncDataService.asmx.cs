using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    /// <summary>
    /// AutoCallSyncDataService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class AutoCallSyncDataService : System.Web.Services.WebService
    {
        public const int MaxRow = 5000;

        public static void Log(string msg, Exception ex = null)
        {
            if (!string.IsNullOrEmpty(msg))
                BLL.Util.LogForWeb("info", msg);
            if (ex != null)
            {
                BLL.Util.LogForWeb("info", ex.Message);
                BLL.Util.LogForWeb("info", ex.StackTrace);
            }
        }

        private static bool Verify(string key)
        {
            string msg = "";
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(key, 0, ref msg, ""))
            {
                return true;
            }
            else
            {
                Log("权限验证错误：" + msg);
                return false;
            }
        }

        [WebMethod(Description = "获取项目表数据从北京到西安")]
        public byte[] GetAutoCallProjectFormBJToXiAn(string key, long timestamp, out string msg)
        {
            string title = "查询北京自动外呼项目表";
            byte[] data = null;
            msg = "";
            try
            {
                if (Verify(key))
                {
                    //查询大于timestamp的数据
                    DataTable dt = BLL.AutoCallSyncData.Instance.GetAutoCallByTimestamp_Project(timestamp, MaxRow);
                    msg = title + "（" + timestamp + "）,查询结果：" + dt.Rows.Count;
                    data = BitAuto.ISDC.CC2012.BLL.Util.DataTableToBinary(dt);
                    msg += " 压缩：" + (data.LongLength / 1024.0 / 1024.0).ToString("0.000") + " MB";
                }
            }
            catch (Exception ex)
            {
                msg = title + "-异常：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            Log(msg);
            return data;
        }
        [WebMethod(Description = "获取任务表数据从北京到西安")]
        public byte[] GetAutoCallTaskFormBJToXiAn(string key, long timestamp, out string msg)
        {
            string title = "查询北京自动外呼任务表";
            byte[] data = null;
            msg = "";
            try
            {
                if (Verify(key))
                {
                    //查询大于timestamp的数据
                    DataTable dt = BLL.AutoCallSyncData.Instance.GetAutoCallByTimestamp_Task(timestamp, MaxRow);
                    msg = title + "（" + timestamp + "）,查询结果：" + dt.Rows.Count;
                    data = BitAuto.ISDC.CC2012.BLL.Util.DataTableToBinary(dt);
                    msg += " 压缩：" + (data.LongLength / 1024.0 / 1024.0).ToString("0.000") + " MB";
                }
            }
            catch (Exception ex)
            {
                msg = title + "-异常：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            Log(msg);
            return data;
        }


        [WebMethod(Description = "获取统计表最大时间戳")]
        public long GetAutoCallStatMaxTimeStamp(string key, out string msg)
        {
            string title = "获取统计表最大时间戳";
            long maxid = 0;
            msg = "";
            try
            {
                if (Verify(key))
                {
                    //查询
                    maxid = BLL.AutoCallSyncData.Instance.GetAutoCallMaxTimeStamp_Stat();
                    msg = title + "：" + maxid;
                }
            }
            catch (Exception ex)
            {
                msg = title + "-异常：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            Log(msg);
            return maxid;
        }
        [WebMethod(Description = "获取明细表最大时间戳")]
        public long GetAutoCallDetailMaxTimeStamp(string key, out string msg)
        {
            string title = "获取明细表最大时间戳";
            long maxid = 0;
            msg = "";
            try
            {
                if (Verify(key))
                {
                    //查询
                    maxid = BLL.AutoCallSyncData.Instance.GetAutoCallMaxTimeStamp_Detail();
                    msg = title + "：" + maxid;
                }
            }
            catch (Exception ex)
            {
                msg = title + "-异常：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            Log(msg);
            return maxid;
        }

        [WebMethod(Description = "更新统计表从西安到北京")]
        public bool UpdateAutoCallStatDataFromXiAnToBJ(string key, byte[] data, out string msg)
        {
            string title = "更新统计表从西安到北京";
            bool result = false;
            msg = "";
            try
            {
                if (Verify(key))
                {
                    msg = title + "：";
                    //解压数据
                    DataTable dt = BLL.Util.BinaryToDataTable(data);
                    msg += "(1) 解压数据：" + dt.Rows.Count + "；";
                    //写入临时表
                    BLL.AutoCallSyncData.Instance.BulkCopyToDB_Stat(dt);
                    msg += "(2) 写入临时表：" + dt.Rows.Count + "；";
                    //更新数据
                    int[] a = BLL.AutoCallSyncData.Instance.UpdateFromTemp_Stat();
                    msg += "(3) 更新数据：" + a[0] + " 新增数据：" + a[1] + "；";

                    result = true;
                }
            }
            catch (Exception ex)
            {
                msg = title + "-异常：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            Log(msg);
            return result;
        }
        [WebMethod(Description = "更新明细表从西安到北京")]
        public bool UpdateAutoCallDetailDataFromXiAnToBJ(string key, byte[] data, out string msg)
        {
            string title = "更新明细表从西安到北京";
            bool result = false;
            msg = "";
            try
            {
                if (Verify(key))
                {
                    msg = title + "：";
                    //解压数据
                    DataTable dt = BLL.Util.BinaryToDataTable(data);
                    msg += "(1) 解压数据：" + dt.Rows.Count + "；";
                    //写入临时表
                    BLL.AutoCallSyncData.Instance.BulkCopyToDB_Detail(dt);
                    msg += "(2) 写入临时表：" + dt.Rows.Count + "；";
                    //更新数据
                    int[] a = BLL.AutoCallSyncData.Instance.UpdateFromTemp_Detail();
                    msg += "(3) 更新数据：" + a[0] + " 新增数据：" + a[1] + "；";

                    result = true;
                }
            }
            catch (Exception ex)
            {
                msg = title + "-异常：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            Log(msg);
            return result;
        }

        /*
        [WebMethod(Description = "更新MongoDB中坐席状态数据到北京")]
        public bool UpdateSkillGropLiveData4Daping(string key, byte[] data, out string msg)
        {
            string title = "更新明细表从西安到北京";
            bool result = false;
            msg = "";
            try
            {
                if (Verify(key))
                {
                    msg = title + "：";
                    //解压数据
                    DataTable dt = BLL.Util.BinaryToDataTable(data);
                    msg += "(1) 解压数据：" + dt.Rows.Count + "；";
                    if (dt.Rows.Count > 0)
                    {
                        //写入临时表
                        BitAuto.ISDC.CC2012.BLL.BigScreen.SkillGropLiveData4Daping.Instance.BulkCopyToDB_Detail(dt);

                        msg += "(2) 写入临时表：" + dt.Rows.Count + "；";
                        //更新数据
                        BitAuto.ISDC.CC2012.BLL.BigScreen.SkillGropLiveData4Daping.Instance
                            .CleanSkillGropLiveDataOldData();
                        msg += "(3) 删除历史数据。";
                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                msg = title + "-异常：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            Log(msg);
            return result;
        }


        [WebMethod(Description = "更新合力坐席统计数据到北京")]
        public bool UpdateState_Hotline4DaPing(string key, byte[] data, out string msg)
        {
            string title = "更新合力坐席统计数据到北京";
            bool result = false;
            msg = "";
            try
            {
                if (Verify(key))
                {
                    msg = title + "：";
                    //解压数据
                    DataTable dt = BLL.Util.BinaryToDataTable(data);
                    msg += "(1) 解压数据：" + dt.Rows.Count + "；";
                    //写入临时表
                    BitAuto.ISDC.CC2012.BLL.BigScreen.State_Hotline4DaPing.Instance.BulkCopyToDB_Detail(dt);

                    //BLL.AutoCallSyncData.Instance.BulkCopyToDB_Detail(dt);
                    msg += "(2) 写入临时表：" + dt.Rows.Count + "；";
                    BitAuto.ISDC.CC2012.BLL.BigScreen.State_Hotline4DaPing.Instance.CleanState_Hotline4DaPingOldData();
                    msg += "(3) 删除历史数据。";
                    //更新数据
                    //int[] a = BLL.AutoCallSyncData.Instance.UpdateFromTemp_Detail();
                    //msg += "(3) 更新数据：" + a[0] + " 新增数据：" + a[1] + "；";

                    result = true;
                }
            }
            catch (Exception ex)
            {
                msg = title + "-异常：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            Log(msg);
            return result;
        }

        */
    }
}
