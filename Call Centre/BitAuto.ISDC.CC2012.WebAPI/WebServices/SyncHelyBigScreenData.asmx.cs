using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.ISDC.CC2012.WebAPI.Helper;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.WebAPI.WebServices
{
    /// <summary>
    /// SyncHelyBigScreenData 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class SyncHelyBigScreenData : System.Web.Services.WebService
    {

        public static void Log(string msg, Exception ex = null)
        {
            if (!string.IsNullOrEmpty(msg))
                CommonHelper.Log(msg);
            if (ex != null)
            {
                CommonHelper.Log(ex.Message);
                CommonHelper.Log(ex.StackTrace);
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

        public class HotLineInf
        {

            public HotLineInf(string key, string strName)
            {
                this.strHotLineNun = key;
                this.strHLName = strName;
            }

            public string strHLName;
            public string strHotLineNun;


            public int ContactInQueueNum;
            /// <summary>
            /// 接通率
            /// </summary>
            public double pc_n_answered;

            /// <summary>
            /// 30接通率
            /// </summary>
            public double pc_n_distrib_in_tr;

        }

        /// 从Application中取数据
        /// <summary>
        /// 从Application中取数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, HotLineInf> getHotLineDictionary()
        {
            try
            {
                Dictionary<string, string> HotLineDic = new Dictionary<string, string>();
                HotLineDic = CommonFunction.GetAllNodeContentByFile<string, string>(AppDomain.CurrentDomain.BaseDirectory + "1-热线数据统计.xml", "key", "name", null);
                CommonHelper.Log("读取配置文件【1-热线数据统计.xml】热线数据：" + HotLineDic.Count);

                Dictionary<string, HotLineInf> dicHotLineInf = null;
                if (HttpContext.Current.Application["HotLineLiveData"] == null)
                {
                    dicHotLineInf = new Dictionary<string, HotLineInf>();

                    foreach (string key in HotLineDic.Keys)
                    {
                        dicHotLineInf.Add(key, new HotLineInf(key, HotLineDic[key]));
                    }

                    HttpContext.Current.Application["HotLineLiveData"] = dicHotLineInf;
                }
                else
                {
                    dicHotLineInf = HttpContext.Current.Application["HotLineLiveData"] as Dictionary<string, HotLineInf>;
                }

                return dicHotLineInf;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                return null;
            }

        }

        [WebMethod(Description = "更新MongoDB中坐席状态数据到北京")]
        public bool UpdateSkillGropLiveData4Daping(string key, byte[] data, out string msg)
        {
            Dictionary<string, HotLineInf> dicHotLineInf = getHotLineDictionary();
            string title = "更新明细表从西安到北京";
            bool result = false;
            msg = "";
            try
            {
                if (Verify(key))
                {
                    msg = title + "：";
                    //解压数据
                    msg += "(1) 清零历史数据；";
                    foreach (KeyValuePair<string, HotLineInf> inf in dicHotLineInf)
                    {
                        inf.Value.ContactInQueueNum = 0;
                    }
                    DataTable dt = BLL.Util.BinaryToDataTable(data);
                    foreach (DataRow row in dt.Rows)
                    {
                        string strHl = row["hotline"].ToString();
                        if (dicHotLineInf.ContainsKey(strHl))
                        {
                            Log("热线：" + strHl + ",ContactInQueueNum赋值为：" + row["ContactInQueue"].ToString());
                            dicHotLineInf[strHl].ContactInQueueNum = Convert.ToInt32(row["ContactInQueue"]);
                        }
                    }
                    msg += "(2) 更新内存数据；";

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
            Dictionary<string, HotLineInf> dicHotLineInf = getHotLineDictionary();
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
                    msg += "(1) 清零历史数据；";
                    foreach (KeyValuePair<string, HotLineInf> inf in dicHotLineInf)
                    {
                        inf.Value.pc_n_answered = 0;
                        inf.Value.pc_n_distrib_in_tr = 0;
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        string strHl = row["HOTLINENO"].ToString();
                        if (dicHotLineInf.ContainsKey(strHl))
                        {
                            dicHotLineInf[strHl].pc_n_answered = Convert.ToDouble(row["pc_n_answered"]);
                            dicHotLineInf[strHl].pc_n_distrib_in_tr = Convert.ToDouble(row["pc_n_distrib_in_tr"]);
                        }
                    }

                    msg += "(2) 更新内存数据；";
                    result = true;
                }
                else
                {
                    msg += " 身份验证失败";
                }
            }
            catch (Exception ex)
            {
                msg = title + "-异常：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            Log(msg);
            return result;
        }

        [WebMethod(Description = "获取大屏监控中热线与技能组关系")]
        public byte[] GetHotLineREFSkillGroup(string key, out string msg)
        {
            string title = "获取大屏监控中热线与技能组关系";
            msg = "";
            try
            {
                if (Verify(key))
                {
                    Dictionary<string, string> HotLineDic = new Dictionary<string, string>();
                    HotLineDic = CommonFunction.GetAllNodeContentByFile<string, string>(AppDomain.CurrentDomain.BaseDirectory + "1-热线数据统计.xml", "key", "name", null);
                    CommonHelper.Log("读取配置文件【1-热线数据统计.xml】热线数据：" + HotLineDic.Count);

                    string hotlineids = "'" + string.Join("','", HotLineDic.Keys.ToArray()) + "'";
                    CommonHelper.Log("查询热线数据：" + hotlineids);

                    var dataHotLine = BitAuto.ISDC.CC2012.BLL.SkillGroupDataRight.Instance.GetHotlineRelationSkillInfo(hotlineids);
                    return BitAuto.ISDC.CC2012.BLL.Util.DataTableToBinary(dataHotLine);
                }
                else
                {
                    msg += " 身份验证失败";
                }
            }
            catch (Exception ex)
            {
                msg = title + "-异常：" + ex.Message + "\r\n" + ex.StackTrace;
            }
            Log(msg);
            return null;
        }

        [WebMethod(Description = "清空缓存")]
        public void ClearCacheHotLineInfData(string key)
        {
            try
            {
                if (Verify(key))
                {
                    Log("[ClearCacheHotLineInfData] 清除缓存");
                    if (HttpContext.Current.Application["HotLineLiveData"] != null)
                    {
                        HttpContext.Current.Application["HotLineLiveData"] = null;
                    }
                }
                else
                {
                    Log("[ClearCacheHotLineInfData] 身份验证失败");
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }
    }
}
