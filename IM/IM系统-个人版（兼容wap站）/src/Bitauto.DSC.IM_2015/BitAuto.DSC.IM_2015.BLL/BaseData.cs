using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.Utils.Config;

namespace BitAuto.DSC.IM_2015.BLL
{
    /// 查询基础数据
    /// <summary>
    /// 查询基础数据
    /// </summary>
    public class TimeModelClss
    {
        public string SourceTypeName
        { set; get; }
        public string SourceType
        { set; get; }
        public string ST { set; get; }
        public string ET { set; get; }
    }
    public class MoreURlModelClss
    {
        public string SourceTypeName
        { set; get; }
        public string SourceType
        { set; get; }
        public string MoreURL { set; get; }
    }
    public class BaseData
    {
        private BaseData() { }
        private static BaseData instance = null;
        public static BaseData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BaseData();
                }
                return instance;
            }
        }

        /// 获取所有大区
        /// <summary>
        /// 获取所有大区
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllDistrict()
        {
            return Dal.BaseData.Instance.GetAllDistrict();
        }

        /// 获取所有的省市区数据
        /// <summary>
        /// 获取所有的省市区数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllAreaInfo()
        {
            return Dal.BaseData.Instance.GetAllAreaInfo();
        }



        /// 通过会员code获取会员信息
        /// <summary>
        /// 通过会员code获取会员信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDMSMemberByCode(string memberCode)
        {
            return Dal.BaseData.Instance.GetDMSMemberByCode(memberCode);
        }

        #region 获取坐席相关信息
        /// 根据坐席UserID，获取CC系统中，AgentID
        /// <summary>
        /// 根据坐席UserID，获取CC系统中，AgentID
        /// </summary>
        /// <param name="userid">坐席UserID</param>
        /// <returns>返回CC系统中，AgentID，若找不到，则返回字符串空</returns>
        public string GetAgentNumByUserID(string userid)
        {
            return Dal.BaseData.Instance.GetAgentNumByUserID(userid);
        }

        /// <summary>
        /// 根据UserID，返回坐席的所在分组ID
        /// </summary>
        /// <param name="userid">坐席UserID</param>
        /// <returns>返回坐席所在分组ID，座机</returns>
        public int GetAgentBGIDByUserID(int userid)
        {
            return Dal.BaseData.Instance.GetAgentBGIDByUserID(userid);
        }

        /// 查询有权限的坐席
        /// <summary>
        /// 查询有权限的坐席
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetAgentInfoData(QueryAgentInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.BaseData.Instance.GetAgentInfoData(query, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// 获取坐席所在区域
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetAgentRegionByUserID(string userid)
        {
            return Dal.BaseData.Instance.GetAgentRegionByUserID(userid);
        }
        #endregion

        /// 获取当前用户下所有的管辖分组
        /// <summary>
        /// 获取当前用户下所有的管辖分组
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetUserGroupDataRigth(int userid)
        {
            return Dal.BaseData.Instance.GetUserGroupDataRigth(userid);
        }

        public DataTable GetUserGroupDataRigth()
        {
            return Dal.BaseData.Instance.GetUserGroupDataRigth();
        }
        /// 获取坐席所属区域的全部业务组
        /// <summary>
        /// 获取坐席所属区域的全部业务组
        /// </summary>
        /// <param name="userid">坐席userid></param>
        /// <returns></returns>
        public DataTable GetUserGroupByUserID(int userid)
        {
            return Dal.BaseData.Instance.GetUserGroupByUserID(userid);
        }
        //文件名，服务时间文件名
        private const string File_Label = "ServerTimeConfig";
        //private const string StartTime_Label = "StartTime";
        //private const string EndTime_Label = "EndTime";
        private const string ServerTime_Label = "ServerTime";
        //头像，log       
        private const string SourceTypeFile_Label = "SourceType";
        private const string LogoURL_Label = "LogoURL";
        private const string AgentHeadURL_Label = "AgentHeadURL";
        private const string UserHeadURL_Label = "UserHeadURL";
        //业务线更多
        private const string MoreURLFile_Label = "MoreURLConfig";
        private const string MoreURL_Label = "MoreURL";

        //虚拟目录地址
        public string CommonLoadDomain = ConfigurationUtil.GetAppSettingValue("CommonLoadFile");

        /// 保存时间
        /// <summary>
        /// 保存时间
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        public void SaveTime(List<TimeModelClss> ListMTimeModel)
        {
            try
            {
                Dictionary<string, Dictionary<string, string>> Listdic = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                for (int i = 0; i < ListMTimeModel.Count; i++)
                {
                    TimeModelClss model = ListMTimeModel[i];
                    if (model.SourceType == BLL.Util.GetSourceType("惠买车"))
                    {
                        dic.Add("hmc_StartTime", model.ST);
                        dic.Add("hmc_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车商城"))
                    {
                        dic.Add("sc_StartTime", model.ST);
                        dic.Add("sc_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车网"))
                    {
                        dic.Add("yc_StartTime", model.ST);
                        dic.Add("yc_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车视频"))
                    {
                        dic.Add("ysp_StartTime", model.ST);
                        dic.Add("ysp_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车报价"))
                    {
                        dic.Add("ybj_StartTime", model.ST);
                        dic.Add("ybj_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车贷款"))
                    {
                        dic.Add("ydk_StartTime", model.ST);
                        dic.Add("ydk_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车问答"))
                    {
                        dic.Add("ywd_StartTime", model.ST);
                        dic.Add("ywd_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车口碑"))
                    {
                        dic.Add("ykb_StartTime", model.ST);
                        dic.Add("ykb_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车养护"))
                    {
                        dic.Add("yyh_StartTime", model.ST);
                        dic.Add("yyh_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车论坛"))
                    {
                        dic.Add("ylt_StartTime", model.ST);
                        dic.Add("ylt_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车车友会"))
                    {
                        dic.Add("ycyh_StartTime", model.ST);
                        dic.Add("ycyh_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("我的易车"))
                    {
                        dic.Add("ywdyc_StartTime", model.ST);
                        dic.Add("ywdyc_EndTime", model.ET);
                    }
                    //if (model.SourceType == BLL.Util.GetSourceType("易车车币"))
                    //{
                    //    dic.Add("ycb_StartTime", model.ST);
                    //    dic.Add("ycb_EndTime", model.ET);
                    //}
                    if (model.SourceType == BLL.Util.GetSourceType("易车购车类"))
                    {
                        dic.Add("ygcl_StartTime", model.ST);
                        dic.Add("ygcl_EndTime", model.ET);
                    }


                    if (model.SourceType == BLL.Util.GetSourceType("二手车"))
                    {
                        dic.Add("er_StartTime", model.ST);
                        dic.Add("er_EndTime", model.ET);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车惠"))
                    {
                        dic.Add("ych_StartTime", model.ST);
                        dic.Add("ych_EndTime", model.ET);
                    }
                }
                Listdic.Add(ServerTime_Label, dic);
                string path = CommonLoadDomain + "/" + File_Label + ".xml";
                CommonFunc.SaveDictionaryToFile(Listdic, path);
            }
            catch
            {
            }
        }
        /// <summary>
        /// 取所有业务线时间配置
        /// </summary>
        /// <param name="docServer"></param>
        public void ReadTimeAll(out List<TimeModelClss> docServer)
        {
            docServer = new List<TimeModelClss>();
            string path = CommonLoadDomain + "/" + File_Label + ".xml";
            Dictionary<string, string> dic = CommonFunc.GetAllNodeContentByFile<string, string>(path, "/root/" + ServerTime_Label);
            try
            {
                TimeModelClss htime = new TimeModelClss();
                htime.ST = dic["hmc_StartTime"].ToString();
                htime.ET = dic["hmc_EndTime"].ToString();
                htime.SourceType = BLL.Util.GetSourceType("惠买车");
                htime.SourceTypeName = "惠买车";
                docServer.Add(htime);

                TimeModelClss stime = new TimeModelClss();
                stime.ST = dic["sc_StartTime"].ToString();
                stime.ET = dic["sc_EndTime"].ToString();
                stime.SourceTypeName = "易车商城";
                stime.SourceType = BLL.Util.GetSourceType("易车商城");
                docServer.Add(stime);

                TimeModelClss ytime = new TimeModelClss();
                ytime.ST = dic["yc_StartTime"].ToString();
                ytime.ET = dic["yc_EndTime"].ToString();
                ytime.SourceTypeName = "易车网";
                ytime.SourceType = BLL.Util.GetSourceType("易车网");
                docServer.Add(ytime);

                TimeModelClss ysptime = new TimeModelClss();
                ysptime.ST = dic["ysp_StartTime"].ToString();
                ysptime.ET = dic["ysp_EndTime"].ToString();
                ysptime.SourceTypeName = "易车视频";
                ysptime.SourceType = BLL.Util.GetSourceType("易车视频");
                docServer.Add(ysptime);


                TimeModelClss ybjtime = new TimeModelClss();
                ybjtime.ST = dic["ybj_StartTime"].ToString();
                ybjtime.ET = dic["ybj_EndTime"].ToString();
                ybjtime.SourceTypeName = "易车报价";
                ybjtime.SourceType = BLL.Util.GetSourceType("易车报价");
                docServer.Add(ybjtime);


                TimeModelClss ydktime = new TimeModelClss();
                ydktime.ST = dic["ydk_StartTime"].ToString();
                ydktime.ET = dic["ydk_EndTime"].ToString();
                ydktime.SourceTypeName = "易车贷款";
                ydktime.SourceType = BLL.Util.GetSourceType("易车贷款");
                docServer.Add(ydktime);

                TimeModelClss ywdtime = new TimeModelClss();
                ywdtime.ST = dic["ywd_StartTime"].ToString();
                ywdtime.ET = dic["ywd_EndTime"].ToString();
                ywdtime.SourceTypeName = "易车问答";
                ywdtime.SourceType = BLL.Util.GetSourceType("易车问答");
                docServer.Add(ywdtime);

                TimeModelClss ykbtime = new TimeModelClss();
                ykbtime.ST = dic["ykb_StartTime"].ToString();
                ykbtime.ET = dic["ykb_EndTime"].ToString();
                ykbtime.SourceTypeName = "易车口碑";
                ykbtime.SourceType = BLL.Util.GetSourceType("易车口碑");
                docServer.Add(ykbtime);

                TimeModelClss yyhtime = new TimeModelClss();
                yyhtime.ST = dic["yyh_StartTime"].ToString();
                yyhtime.ET = dic["yyh_EndTime"].ToString();
                yyhtime.SourceTypeName = "易车养护";
                yyhtime.SourceType = BLL.Util.GetSourceType("易车养护");
                docServer.Add(yyhtime);

                TimeModelClss ylttime = new TimeModelClss();
                ylttime.ST = dic["ylt_StartTime"].ToString();
                ylttime.ET = dic["ylt_EndTime"].ToString();
                ylttime.SourceTypeName = "易车论坛";
                ylttime.SourceType = BLL.Util.GetSourceType("易车论坛");
                docServer.Add(ylttime);

                TimeModelClss ycyhtime = new TimeModelClss();
                ycyhtime.ST = dic["ycyh_StartTime"].ToString();
                ycyhtime.ET = dic["ycyh_EndTime"].ToString();
                ycyhtime.SourceTypeName = "易车车友会";
                ycyhtime.SourceType = BLL.Util.GetSourceType("易车车友会");
                docServer.Add(ycyhtime);


                TimeModelClss ywdyctime = new TimeModelClss();
                ywdyctime.ST = dic["ywdyc_StartTime"].ToString();
                ywdyctime.ET = dic["ywdyc_EndTime"].ToString();
                ywdyctime.SourceTypeName = "我的易车";
                ywdyctime.SourceType = BLL.Util.GetSourceType("我的易车");
                docServer.Add(ywdyctime);

                //TimeModelClss ycbtime = new TimeModelClss();
                //ycbtime.ST = dic["ycb_StartTime"].ToString();
                //ycbtime.ET = dic["ycb_EndTime"].ToString();
                //ycbtime.SourceTypeName = "易车车币";
                //ycbtime.SourceType = BLL.Util.GetSourceType("易车车币");
                //docServer.Add(ycbtime);

                TimeModelClss ygcltime = new TimeModelClss();
                ygcltime.ST = dic["ygcl_StartTime"].ToString();
                ygcltime.ET = dic["ygcl_EndTime"].ToString();
                ygcltime.SourceTypeName = "易车购车类";
                ygcltime.SourceType = BLL.Util.GetSourceType("易车购车类");
                docServer.Add(ygcltime);



                TimeModelClss etime = new TimeModelClss();
                etime.ST = dic["er_StartTime"].ToString();
                etime.ET = dic["er_EndTime"].ToString();
                etime.SourceTypeName = "二手车";
                etime.SourceType = BLL.Util.GetSourceType("二手车");
                docServer.Add(etime);

                TimeModelClss ychtime = new TimeModelClss();
                ychtime.ST = dic["ych_StartTime"].ToString();
                ychtime.ET = dic["ych_EndTime"].ToString();
                ychtime.SourceTypeName = "易车惠";
                ychtime.SourceType = BLL.Util.GetSourceType("易车惠");
                docServer.Add(ychtime);

            }
            catch
            { }

        }

        /// 保存更多
        /// <summary>
        /// 保存更多
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        public void SaveMoreURL(List<MoreURlModelClss> ListMTimeModel)
        {
            try
            {
                Dictionary<string, Dictionary<string, string>> Listdic = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                for (int i = 0; i < ListMTimeModel.Count; i++)
                {
                    MoreURlModelClss model = ListMTimeModel[i];
                    if (model.SourceType == BLL.Util.GetSourceType("惠买车"))
                    {
                        dic.Add("hmc_MoreURL", model.MoreURL);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车商城"))
                    {
                        dic.Add("sc_MoreURL", model.MoreURL);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车网"))
                    {
                        dic.Add("yc_MoreURL", model.MoreURL);
                    }

                    if (model.SourceType == BLL.Util.GetSourceType("易车视频"))
                    {
                        dic.Add("ysp_MoreURL", model.MoreURL);

                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车报价"))
                    {
                        dic.Add("ybj_MoreURL", model.MoreURL);

                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车贷款"))
                    {
                        dic.Add("ydk_MoreURL", model.MoreURL);

                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车问答"))
                    {
                        dic.Add("ywd_MoreURL", model.MoreURL);

                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车口碑"))
                    {
                        dic.Add("ykb_MoreURL", model.MoreURL);

                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车养护"))
                    {
                        dic.Add("yyh_MoreURL", model.MoreURL);

                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车论坛"))
                    {
                        dic.Add("ylt_MoreURL", model.MoreURL);

                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车车友会"))
                    {
                        dic.Add("ycyh_MoreURL", model.MoreURL);

                    }
                    if (model.SourceType == BLL.Util.GetSourceType("我的易车"))
                    {
                        dic.Add("ywdyc_MoreURL", model.MoreURL);

                    }
                    //if (model.SourceType == BLL.Util.GetSourceType("易车车币"))
                    //{
                    //    dic.Add("ycb_MoreURL", model.MoreURL);

                    //}
                    if (model.SourceType == BLL.Util.GetSourceType("易车购车类"))
                    {
                        dic.Add("ygcl_MoreURL", model.MoreURL);

                    }


                    if (model.SourceType == BLL.Util.GetSourceType("二手车"))
                    {
                        dic.Add("er_MoreURL", model.MoreURL);
                    }
                    if (model.SourceType == BLL.Util.GetSourceType("易车惠"))
                    {
                        dic.Add("ych_MoreURL", model.MoreURL);
                    }
                }
                Listdic.Add(MoreURL_Label, dic);
                string path = CommonLoadDomain + "/" + MoreURLFile_Label + ".xml";
                CommonFunc.SaveDictionaryToFile(Listdic, path);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 取所有业务线更多配置
        /// </summary>
        /// <param name="docServer"></param>
        public void ReadMoreURLAll(out List<MoreURlModelClss> docServer)
        {
            docServer = new List<MoreURlModelClss>();
            string path = CommonLoadDomain + "/" + MoreURLFile_Label + ".xml";
            Dictionary<string, string> dic = CommonFunc.GetAllNodeContentByFile<string, string>(path, "/root/" + MoreURL_Label);
            try
            {
                MoreURlModelClss htime = new MoreURlModelClss();
                htime.MoreURL = dic["hmc_MoreURL"].ToString();
                htime.SourceType = BLL.Util.GetSourceType("惠买车");
                htime.SourceTypeName = "惠买车";
                docServer.Add(htime);

                MoreURlModelClss stime = new MoreURlModelClss();
                stime.MoreURL = dic["sc_MoreURL"].ToString();
                stime.SourceTypeName = "易车商城";
                stime.SourceType = BLL.Util.GetSourceType("易车商城");
                docServer.Add(stime);

                MoreURlModelClss ytime = new MoreURlModelClss();
                ytime.MoreURL = dic["yc_MoreURL"].ToString();
                ytime.SourceTypeName = "易车网";
                ytime.SourceType = BLL.Util.GetSourceType("易车网");
                docServer.Add(ytime);

                MoreURlModelClss ysptime = new MoreURlModelClss();
                ysptime.MoreURL = dic["ysp_MoreURL"].ToString();
                ysptime.SourceTypeName = "易车视频";
                ysptime.SourceType = BLL.Util.GetSourceType("易车视频");
                docServer.Add(ysptime);


                MoreURlModelClss ybjtime = new MoreURlModelClss();
                ybjtime.MoreURL = dic["ybj_MoreURL"].ToString();
                ybjtime.SourceTypeName = "易车报价";
                ybjtime.SourceType = BLL.Util.GetSourceType("易车报价");
                docServer.Add(ybjtime);


                MoreURlModelClss ydktime = new MoreURlModelClss();
                ydktime.MoreURL = dic["ydk_MoreURL"].ToString();
                ydktime.SourceTypeName = "易车贷款";
                ydktime.SourceType = BLL.Util.GetSourceType("易车贷款");
                docServer.Add(ydktime);

                MoreURlModelClss ywdtime = new MoreURlModelClss();
                ywdtime.MoreURL = dic["ywd_MoreURL"].ToString();
                ywdtime.SourceTypeName = "易车问答";
                ywdtime.SourceType = BLL.Util.GetSourceType("易车问答");
                docServer.Add(ywdtime);

                MoreURlModelClss ykbtime = new MoreURlModelClss();
                ykbtime.MoreURL = dic["ykb_MoreURL"].ToString();
                ykbtime.SourceTypeName = "易车口碑";
                ykbtime.SourceType = BLL.Util.GetSourceType("易车口碑");
                docServer.Add(ykbtime);

                MoreURlModelClss yyhtime = new MoreURlModelClss();
                yyhtime.MoreURL = dic["yyh_MoreURL"].ToString();
                yyhtime.SourceTypeName = "易车养护";
                yyhtime.SourceType = BLL.Util.GetSourceType("易车养护");
                docServer.Add(yyhtime);

                MoreURlModelClss ylttime = new MoreURlModelClss();
                ylttime.MoreURL = dic["ylt_MoreURL"].ToString();
                ylttime.SourceTypeName = "易车论坛";
                ylttime.SourceType = BLL.Util.GetSourceType("易车论坛");
                docServer.Add(ylttime);

                MoreURlModelClss ycyhtime = new MoreURlModelClss();
                ycyhtime.MoreURL = dic["ycyh_MoreURL"].ToString();
                ycyhtime.SourceTypeName = "易车车友会";
                ycyhtime.SourceType = BLL.Util.GetSourceType("易车车友会");
                docServer.Add(ycyhtime);


                MoreURlModelClss ywdyctime = new MoreURlModelClss();
                ywdyctime.MoreURL = dic["ywdyc_MoreURL"].ToString();
                ywdyctime.SourceTypeName = "我的易车";
                ywdyctime.SourceType = BLL.Util.GetSourceType("我的易车");
                docServer.Add(ywdyctime);

                //MoreURlModelClss ycbtime = new MoreURlModelClss();
                //ycbtime.MoreURL = dic["ycb_MoreURL"].ToString();
                //ycbtime.SourceTypeName = "易车车币";
                //ycbtime.SourceType = BLL.Util.GetSourceType("易车车币");
                //docServer.Add(ycbtime);

                MoreURlModelClss ygcltime = new MoreURlModelClss();
                ygcltime.MoreURL = dic["ygcl_MoreURL"].ToString();
                ygcltime.SourceTypeName = "易车购车类";
                ygcltime.SourceType = BLL.Util.GetSourceType("易车购车类");
                docServer.Add(ygcltime);



                MoreURlModelClss etime = new MoreURlModelClss();
                etime.MoreURL = dic["er_MoreURL"].ToString();
                etime.SourceTypeName = "二手车";
                etime.SourceType = BLL.Util.GetSourceType("二手车");
                docServer.Add(etime);

                MoreURlModelClss ychtime = new MoreURlModelClss();
                ychtime.MoreURL = dic["ych_MoreURL"].ToString();
                ychtime.SourceTypeName = "易车惠";
                ychtime.SourceType = BLL.Util.GetSourceType("易车惠");
                docServer.Add(ychtime);
            }
            catch
            { }

        }

        /// 读取业务线更多
        /// <summary>
        /// 读取业务线更多
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        public string ReadMoreUrl(string sourcetype)
        {
            string moreurl = string.Empty;
            try
            {
                string moreurlname = string.Empty;
                if (sourcetype == BLL.Util.GetSourceType("惠买车"))
                {
                    moreurlname = "hmc_MoreURL";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车商城"))
                {
                    moreurlname = "sc_MoreURL";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车网"))
                {
                    moreurlname = "yc_MoreURL";
                }

                if (sourcetype == BLL.Util.GetSourceType("易车视频"))
                {
                    moreurlname = "ysp_MoreURL";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车报价"))
                {
                    moreurlname = "ybj_MoreURL";

                }
                if (sourcetype == BLL.Util.GetSourceType("易车贷款"))
                {
                    moreurlname = "ydk_MoreURL";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车问答"))
                {
                    moreurlname = "ywd_MoreURL";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车口碑"))
                {
                    moreurlname = "ykb_MoreURL";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车养护"))
                {
                    moreurlname = "yyh_MoreURL";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车论坛"))
                {
                    moreurlname = "ylt_MoreURL";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车车友会"))
                {
                    moreurlname = "ycyh_MoreURL";
                }
                if (sourcetype == BLL.Util.GetSourceType("我的易车"))
                {
                    moreurlname = "ywdyc_MoreURL";
                }
                //if (sourcetype == BLL.Util.GetSourceType("易车车币"))
                //{
                //    moreurlname = "ycb_MoreURL";
                //}
                if (sourcetype == BLL.Util.GetSourceType("易车购车类"))
                {
                    moreurlname = "ygcl_MoreURL";
                }


                if (sourcetype == BLL.Util.GetSourceType("二手车"))
                {
                    moreurlname = "er_MoreURL";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车惠"))
                {
                    moreurlname = "ych_MoreURL";
                }

                string path = CommonLoadDomain + "/" + MoreURLFile_Label + ".xml";
                Dictionary<string, string> dic = CommonFunc.GetAllNodeContentByFile<string, string>(path, "/root/" + MoreURL_Label);
                moreurl = dic[moreurlname];
            }
            catch
            {

            }
            return moreurl;
        }

        /// 读取时间
        /// <summary>
        /// 读取时间
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        public void ReadTime(out ServeTime st, out ServeTime et, string sourcetype)
        {
            st = new ServeTime(9, 0);
            et = new ServeTime(18, 0);
            try
            {
                string startime = string.Empty;
                string endtime = string.Empty;
                if (sourcetype == BLL.Util.GetSourceType("惠买车"))
                {
                    startime = "hmc_StartTime";
                    endtime = "hmc_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车商城"))
                {
                    startime = "sc_StartTime";
                    endtime = "sc_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车网"))
                {
                    startime = "yc_StartTime";
                    endtime = "yc_EndTime";
                }

                if (sourcetype == BLL.Util.GetSourceType("易车视频"))
                {
                    startime = "ysp_StartTime";
                    endtime = "ysp_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车报价"))
                {
                    startime = "ybj_StartTime";
                    endtime = "ybj_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车贷款"))
                {
                    startime = "ydk_StartTime";
                    endtime = "ydk_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车问答"))
                {
                    startime = "ywd_StartTime";
                    endtime = "ywd_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车口碑"))
                {
                    startime = "ykb_StartTime";
                    endtime = "ykb_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车养护"))
                {
                    startime = "yyh_StartTime";
                    endtime = "yyh_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车论坛"))
                {
                    startime = "ylt_StartTime";
                    endtime = "ylt_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("易车车友会"))
                {
                    startime = "ycyh_StartTime";
                    endtime = "ycyh_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("我的易车"))
                {
                    startime = "ywdyc_StartTime";
                    endtime = "ywdyc_EndTime";
                }
                //if (sourcetype == BLL.Util.GetSourceType("易车车币"))
                //{
                //    startime="ycb_StartTime";
                //    endtime="ycb_EndTime";
                //}
                if (sourcetype == BLL.Util.GetSourceType("易车购车类"))
                {
                    startime = "ygcl_StartTime";
                    endtime = "ygcl_EndTime";
                }
                if (sourcetype == BLL.Util.GetSourceType("二手车"))
                {
                    startime = "er_StartTime";
                    endtime = "er_EndTime";
                }

                if (sourcetype == BLL.Util.GetSourceType("易车惠"))
                {
                    startime = "ych_StartTime";
                    endtime = "ych_EndTime";
                }

                string path = CommonLoadDomain + "/" + File_Label + ".xml";
                Dictionary<string, string> dic = CommonFunc.GetAllNodeContentByFile<string, string>(path, "/root/" + ServerTime_Label);
                st = new ServeTime(dic[startime]);
                et = new ServeTime(dic[endtime]);
            }
            catch
            {

            }
        }
        /// <summary>
        /// 读取业务线配置
        /// </summary>
        /// <param name="logoURL"></param>
        /// <param name="AgentHeadURL"></param>
        /// <param name="UserHeadURL"></param>
        public void ReadSourceSet(out string logoURL, out string AgentHeadURL, out string UserHeadURL, string Set_Label)
        {
            logoURL = "";
            AgentHeadURL = "";
            UserHeadURL = "";
            try
            {
                string path = CommonLoadDomain + "/" + SourceTypeFile_Label + ".xml";
                Dictionary<string, string> dic = CommonFunc.GetAllNodeContentByFile<string, string>(path, "/root/" + Set_Label);
                logoURL = dic[LogoURL_Label];
                AgentHeadURL = dic[AgentHeadURL_Label];
                UserHeadURL = dic[UserHeadURL_Label];
            }
            catch
            {

            }
        }
        /// <summary>
        /// 获取所有在线分组包括组人数
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnlineBgIDHaveUserCount()
        {
            return Dal.BaseData.Instance.GetOnlineBgIDHaveUserCount();
        }
        /// <summary>
        /// 读取图片验证xml
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ReadYanZhengXml()
        {
            string path = CommonLoadDomain + "/yanzheng.xml";
            Dictionary<string, string> dic = CommonFunc.GetAllNodeContentByFile<string, string>(path, "/root/yanzheng", "key", "value");
            return dic;
        }
    }
}
