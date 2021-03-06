using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类EPVisitLog 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:03 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UserVisitLog
    {
        public static readonly new UserVisitLog Instance = new UserVisitLog();

        protected UserVisitLog()
        { }

        /// 按照查询条件查询
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetUserVisitLog(QueryUserVisitLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.UserVisitLog.Instance.GetUserVisitLog(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.UserVisitLog GetUserVisitLog(int VisitID)
        {
            return Dal.UserVisitLog.Instance.GetUserVisitLog(VisitID);
        }
        /// 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.UserVisitLog.Instance.GetUserVisitLog(new QueryUserVisitLog(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// 调用业务线接口取访客信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="posturl"></param>
        /// <param name="sourcetype"></param>
        /// <param name="loginid"></param>
        /// <returns></returns>
        public Entities.UserVisitLog GetUserInfo(string title, string posturl, string sourcetype, string loginid, string cityidstr, string provinceidstr)
        {

            string username = string.Empty;
            bool sex = true;
            string tel = string.Empty;
            int provinceid = 0;
            int cityid = 0;
            int.TryParse(cityidstr, out cityid);
            int.TryParse(provinceidstr, out provinceid);
            string timestr = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            if (!string.IsNullOrEmpty(loginid))
            {
                //调用业务线接口
                if (sourcetype == BLL.Util.GetSourceType("惠买车"))
                {
                    BLL.Loger.Log4Net.Info("调用惠买车人员信息接口开始，loginid:" + System.Web.HttpContext.Current.Server.UrlEncode(loginid));
                    Entities.HMC_Entity model = BitAuto.DSC.IM_2015.WebService.HMC_Interface.Instance.GetUserInfoByCookie(System.Web.HttpContext.Current.Server.UrlEncode(loginid));
                    loginid = "hmc_" + loginid;
                    username = "访客" + timestr;
                    if (model != null && model.IsSuccess)
                    {
                        BLL.Loger.Log4Net.Info("调用惠买车人员信息接口结束，是否成功:" + model.IsSuccess.ToString());
                        username = model.UserName;
                        tel = model.Mobile;
                        sex = true;
                        if (model.Gender == "Male")
                        {
                            sex = true;
                        }
                        else if (model.Gender == "Female")
                        {
                            sex = false;
                        }
                        loginid = model.UserID;
                    }
                    else
                    {
                        BLL.Loger.Log4Net.Info("调用惠买车人员信息接口失败");
                    }
                }
                else
                {
                    Entities.HMC_Entity model = null;
                    try
                    {
                        BLL.Loger.Log4Net.Info("调用商城取人员信息接口开始，loginid:" + loginid);
                        string msg = string.Empty;
                        model = BitAuto.DSC.IM_2015.WebService.SC_Interface.Instance.GetUserInfoByCookie(System.Web.HttpContext.Current.Server.UrlEncode(loginid), out msg);
                        if (model != null && model.IsSuccess)
                        {
                            username = model.UserName;
                            tel = model.Mobile;
                            sex = true;
                            if (model.Gender == "Male")
                            {
                                sex = true;
                            }
                            else if (model.Gender == "Female")
                            {
                                sex = false;
                            }
                            loginid = model.UserID;
                            BLL.Loger.Log4Net.Info(string.Format("调用商城取人员信息接口结束，loginid:{0},调用成功！", loginid));
                        }
                        else
                        {
                            if (sourcetype == BLL.Util.GetSourceType("易车商城"))
                            {
                                loginid = "sc_" + loginid;
                            }
                            if (sourcetype == BLL.Util.GetSourceType("易车网") || sourcetype == BLL.Util.GetSourceType("易车视频") || sourcetype == BLL.Util.GetSourceType("易车报价") || sourcetype == BLL.Util.GetSourceType("易车贷款") || sourcetype == BLL.Util.GetSourceType("易车问答") || sourcetype == BLL.Util.GetSourceType("易车口碑") || sourcetype == BLL.Util.GetSourceType("易车养护") || sourcetype == BLL.Util.GetSourceType("易车论坛") || sourcetype == BLL.Util.GetSourceType("易车车友会") || sourcetype == BLL.Util.GetSourceType("我的易车") || sourcetype == BLL.Util.GetSourceType("易车购车类"))
                            {
                                loginid = "yc_" + loginid;
                            }
                            if (sourcetype == BLL.Util.GetSourceType("二手车"))
                            {
                                loginid = "er_" + loginid;
                            }
                            if (sourcetype == BLL.Util.GetSourceType("易车惠"))
                            {
                                loginid = "ych_" + loginid;
                            }
                            username = "访客" + timestr;
                            BLL.Loger.Log4Net.Info(string.Format("调用商城取人员信息接口结束，loginid:{0},调用失败,失败原因：{1}", loginid, msg));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (sourcetype == BLL.Util.GetSourceType("易车商城"))
                        {
                            loginid = "sc_" + loginid;
                        }
                        if (sourcetype == BLL.Util.GetSourceType("易车网") || sourcetype == BLL.Util.GetSourceType("易车视频") || sourcetype == BLL.Util.GetSourceType("易车报价") || sourcetype == BLL.Util.GetSourceType("易车贷款") || sourcetype == BLL.Util.GetSourceType("易车问答") || sourcetype == BLL.Util.GetSourceType("易车口碑") || sourcetype == BLL.Util.GetSourceType("易车养护") || sourcetype == BLL.Util.GetSourceType("易车论坛") || sourcetype == BLL.Util.GetSourceType("易车车友会") || sourcetype == BLL.Util.GetSourceType("我的易车") || sourcetype == BLL.Util.GetSourceType("易车购车类"))
                        {
                            loginid = "yc_" + loginid;
                        }
                        if (sourcetype == BLL.Util.GetSourceType("二手车"))
                        {
                            loginid = "er_" + loginid;
                        }
                        if (sourcetype == BLL.Util.GetSourceType("易车惠"))
                        {
                            loginid = "ych_" + loginid;
                        }
                        username = "访客" + timestr;
                        BLL.Loger.Log4Net.Info(string.Format("调用商城取人员信息接口异常，loginid:{0},失败原因：{1}", loginid, ex.Message));
                    }

                }
            }
            else
            {
                //Random rand = new Random();
                //string xx = timestr + rand.Next(1, 10000);
                if (sourcetype == BLL.Util.GetSourceType("惠买车"))
                {
                    loginid = "hmc_" + Guid.NewGuid().ToString();
                }
                if (sourcetype == BLL.Util.GetSourceType("易车商城"))
                {
                    loginid = "sc_" + Guid.NewGuid().ToString();
                }
                if (sourcetype == BLL.Util.GetSourceType("易车网") || sourcetype == BLL.Util.GetSourceType("易车视频") || sourcetype == BLL.Util.GetSourceType("易车报价") || sourcetype == BLL.Util.GetSourceType("易车贷款") || sourcetype == BLL.Util.GetSourceType("易车问答") || sourcetype == BLL.Util.GetSourceType("易车口碑") || sourcetype == BLL.Util.GetSourceType("易车养护") || sourcetype == BLL.Util.GetSourceType("易车论坛") || sourcetype == BLL.Util.GetSourceType("易车车友会") || sourcetype == BLL.Util.GetSourceType("我的易车") || sourcetype == BLL.Util.GetSourceType("易车购车类"))
                {
                    loginid = "yc_" + Guid.NewGuid().ToString();
                }
                if (sourcetype == BLL.Util.GetSourceType("二手车"))
                {
                    loginid = "er_" + Guid.NewGuid().ToString();
                }
                if (sourcetype == BLL.Util.GetSourceType("易车惠"))
                {
                    loginid = "ych_" + Guid.NewGuid().ToString();
                }
                username = "访客" + timestr;
            }
            //访问记录入库
            Entities.UserVisitLog info = InsertEPVisitLog(title, posturl, loginid, sourcetype, username, sex, tel, provinceid, cityid);
            return info;
        }
        /// <summary>
        /// 根据访问id，会话id取访问会话信息
        /// </summary>
        /// <param name="visitid"></param>
        /// <param name="csid"></param>
        /// <returns></returns>
        public DataTable GetVisitAndCs(int visitid, int csid)
        {
            return Dal.UserVisitLog.Instance.GetVisitAndCs(visitid, csid);
        }


        /// 插入一条访问记录
        /// <summary>
        /// 插入一条访问记录
        /// </summary>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private Entities.UserVisitLog InsertEPVisitLog(string title, string posturl, string loginid, string sourcetype, string username, bool sex, string tel, int ProvinceID, int CityID)
        {
            Entities.UserVisitLog info = new Entities.UserVisitLog();
            info.LoginID = loginid;
            info.UserReferTitle = title;
            //if (!string.IsNullOrEmpty(posturl.Query))
            //{
            //    info.UserReferURL = posturl.AbsoluteUri.Replace(posturl.Query, "");
            //}
            //else
            //{
            //    info.UserReferURL = posturl.AbsoluteUri;
            //}
            info.UserReferURL = posturl;
            info.SourceType = sourcetype;
            info.UserName = username;
            info.ProvinceID = ProvinceID;
            info.CityID = CityID;
            info.CreatTime = DateTime.Now;
            info.UpdateTime = DateTime.Now;
            info.Phone = tel;
            info.Sex = sex;
            info.QueuefailTime = Convert.ToDateTime("9999-12-31 00:00:00.000");
            info.VisitID = Dal.UserVisitLog.Instance.InsertUserVisitLog(info);

            return info;
        }
        public void UpdateUserVisitLog(Entities.UserVisitLog model)
        {
            Dal.UserVisitLog.Instance.UpdateUserVisitLog(model);
        }

        /// <summary>
        /// 根据访问id串，返回访问记录实体list
        /// </summary>
        /// <param name="visitids"></param>
        /// <returns></returns>
        public List<Entities.UserVisitLog> GetUserVisitLogListByVisitIDS(string visitids)
        {
            return Dal.UserVisitLog.Instance.GetUserVisitLogListByVisitIDS(visitids);
        }
        /// <summary>
        /// 根据访问id，更新排队放弃时间
        /// </summary>
        /// <param name="visitid"></param>
        /// <param name="csid"></param>
        /// <returns></returns>
        public void UpdateQueueFailTime(int visitid)
        {
            Dal.UserVisitLog.Instance.UpdateQueueFailTime(visitid);
        }
    }
}

