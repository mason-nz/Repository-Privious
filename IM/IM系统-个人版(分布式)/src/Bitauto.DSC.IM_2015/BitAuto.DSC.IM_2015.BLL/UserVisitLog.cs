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
    /// ҵ���߼���EPVisitLog ��ժҪ˵����
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

        /// ���ղ�ѯ������ѯ
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetUserVisitLog(QueryUserVisitLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.UserVisitLog.Instance.GetUserVisitLog(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.UserVisitLog GetUserVisitLog(int VisitID)
        {
            return Dal.UserVisitLog.Instance.GetUserVisitLog(VisitID);
        }
        /// ��������б�
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.UserVisitLog.Instance.GetUserVisitLog(new QueryUserVisitLog(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// ����ҵ���߽ӿ�ȡ�ÿ���Ϣ
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
                //����ҵ���߽ӿ�
                if (sourcetype == BLL.Util.GetSourceType("����"))
                {
                    BLL.Loger.Log4Net.Info("���û�����Ա��Ϣ�ӿڿ�ʼ��loginid:" + System.Web.HttpContext.Current.Server.UrlEncode(loginid));
                    Entities.HMC_Entity model = BitAuto.DSC.IM_2015.WebService.HMC_Interface.Instance.GetUserInfoByCookie(System.Web.HttpContext.Current.Server.UrlEncode(loginid));
                    loginid = "hmc_" + loginid;
                    username = "�ÿ�" + timestr;
                    if (model != null && model.IsSuccess)
                    {
                        BLL.Loger.Log4Net.Info("���û�����Ա��Ϣ�ӿڽ������Ƿ�ɹ�:" + model.IsSuccess.ToString());
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
                        BLL.Loger.Log4Net.Info("���û�����Ա��Ϣ�ӿ�ʧ��");
                    }
                }
                else
                {
                    Entities.HMC_Entity model = null;
                    try
                    {
                        BLL.Loger.Log4Net.Info("�����̳�ȡ��Ա��Ϣ�ӿڿ�ʼ��loginid:" + loginid);
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
                            BLL.Loger.Log4Net.Info(string.Format("�����̳�ȡ��Ա��Ϣ�ӿڽ�����loginid:{0},���óɹ���", loginid));
                        }
                        else
                        {
                            if (sourcetype == BLL.Util.GetSourceType("�׳��̳�"))
                            {
                                loginid = "sc_" + loginid;
                            }
                            if (sourcetype == BLL.Util.GetSourceType("�׳���") || sourcetype == BLL.Util.GetSourceType("�׳���Ƶ") || sourcetype == BLL.Util.GetSourceType("�׳�����") || sourcetype == BLL.Util.GetSourceType("�׳�����") || sourcetype == BLL.Util.GetSourceType("�׳��ʴ�") || sourcetype == BLL.Util.GetSourceType("�׳��ڱ�") || sourcetype == BLL.Util.GetSourceType("�׳�����") || sourcetype == BLL.Util.GetSourceType("�׳���̳") || sourcetype == BLL.Util.GetSourceType("�׳����ѻ�") || sourcetype == BLL.Util.GetSourceType("�ҵ��׳�") || sourcetype == BLL.Util.GetSourceType("�׳�������"))
                            {
                                loginid = "yc_" + loginid;
                            }
                            if (sourcetype == BLL.Util.GetSourceType("���ֳ�"))
                            {
                                loginid = "er_" + loginid;
                            }
                            if (sourcetype == BLL.Util.GetSourceType("�׳���"))
                            {
                                loginid = "ych_" + loginid;
                            }
                            username = "�ÿ�" + timestr;
                            BLL.Loger.Log4Net.Info(string.Format("�����̳�ȡ��Ա��Ϣ�ӿڽ�����loginid:{0},����ʧ��,ʧ��ԭ��{1}", loginid, msg));
                        }
                    }
                    catch (Exception ex)
                    {
                        if (sourcetype == BLL.Util.GetSourceType("�׳��̳�"))
                        {
                            loginid = "sc_" + loginid;
                        }
                        if (sourcetype == BLL.Util.GetSourceType("�׳���") || sourcetype == BLL.Util.GetSourceType("�׳���Ƶ") || sourcetype == BLL.Util.GetSourceType("�׳�����") || sourcetype == BLL.Util.GetSourceType("�׳�����") || sourcetype == BLL.Util.GetSourceType("�׳��ʴ�") || sourcetype == BLL.Util.GetSourceType("�׳��ڱ�") || sourcetype == BLL.Util.GetSourceType("�׳�����") || sourcetype == BLL.Util.GetSourceType("�׳���̳") || sourcetype == BLL.Util.GetSourceType("�׳����ѻ�") || sourcetype == BLL.Util.GetSourceType("�ҵ��׳�") || sourcetype == BLL.Util.GetSourceType("�׳�������"))
                        {
                            loginid = "yc_" + loginid;
                        }
                        if (sourcetype == BLL.Util.GetSourceType("���ֳ�"))
                        {
                            loginid = "er_" + loginid;
                        }
                        if (sourcetype == BLL.Util.GetSourceType("�׳���"))
                        {
                            loginid = "ych_" + loginid;
                        }
                        username = "�ÿ�" + timestr;
                        BLL.Loger.Log4Net.Info(string.Format("�����̳�ȡ��Ա��Ϣ�ӿ��쳣��loginid:{0},ʧ��ԭ��{1}", loginid, ex.Message));
                    }

                }
            }
            else
            {
                //Random rand = new Random();
                //string xx = timestr + rand.Next(1, 10000);
                if (sourcetype == BLL.Util.GetSourceType("����"))
                {
                    loginid = "hmc_" + Guid.NewGuid().ToString();
                }
                if (sourcetype == BLL.Util.GetSourceType("�׳��̳�"))
                {
                    loginid = "sc_" + Guid.NewGuid().ToString();
                }
                if (sourcetype == BLL.Util.GetSourceType("�׳���") || sourcetype == BLL.Util.GetSourceType("�׳���Ƶ") || sourcetype == BLL.Util.GetSourceType("�׳�����") || sourcetype == BLL.Util.GetSourceType("�׳�����") || sourcetype == BLL.Util.GetSourceType("�׳��ʴ�") || sourcetype == BLL.Util.GetSourceType("�׳��ڱ�") || sourcetype == BLL.Util.GetSourceType("�׳�����") || sourcetype == BLL.Util.GetSourceType("�׳���̳") || sourcetype == BLL.Util.GetSourceType("�׳����ѻ�") || sourcetype == BLL.Util.GetSourceType("�ҵ��׳�") || sourcetype == BLL.Util.GetSourceType("�׳�������"))
                {
                    loginid = "yc_" + Guid.NewGuid().ToString();
                }
                if (sourcetype == BLL.Util.GetSourceType("���ֳ�"))
                {
                    loginid = "er_" + Guid.NewGuid().ToString();
                }
                if (sourcetype == BLL.Util.GetSourceType("�׳���"))
                {
                    loginid = "ych_" + Guid.NewGuid().ToString();
                }
                username = "�ÿ�" + timestr;
            }
            //���ʼ�¼���
            Entities.UserVisitLog info = InsertEPVisitLog(title, posturl, loginid, sourcetype, username, sex, tel, provinceid, cityid);
            return info;
        }
        /// <summary>
        /// ���ݷ���id���Ựidȡ���ʻỰ��Ϣ
        /// </summary>
        /// <param name="visitid"></param>
        /// <param name="csid"></param>
        /// <returns></returns>
        public DataTable GetVisitAndCs(int visitid, int csid)
        {
            return Dal.UserVisitLog.Instance.GetVisitAndCs(visitid, csid);
        }


        /// ����һ�����ʼ�¼
        /// <summary>
        /// ����һ�����ʼ�¼
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
        /// ���ݷ���id�������ط��ʼ�¼ʵ��list
        /// </summary>
        /// <param name="visitids"></param>
        /// <returns></returns>
        public List<Entities.UserVisitLog> GetUserVisitLogListByVisitIDS(string visitids)
        {
            return Dal.UserVisitLog.Instance.GetUserVisitLogListByVisitIDS(visitids);
        }
        /// <summary>
        /// ���ݷ���id�������Ŷӷ���ʱ��
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

