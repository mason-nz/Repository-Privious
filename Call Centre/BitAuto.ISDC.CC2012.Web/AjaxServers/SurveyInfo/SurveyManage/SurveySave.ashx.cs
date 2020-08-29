using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Entities;
using System.Text;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyManage
{
    /// <summary>
    /// SurveySave 的摘要说明
    /// </summary>
    public class SurveySave : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public string SIID
        {
            get
            {
                return HttpContext.Current.Request["siid"] == null ? string.Empty :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["siid"].ToString());
            }
        }

        /// <summary>
        /// 动作
        /// </summary>
        public string Action
        {
            get
            {
                return HttpContext.Current.Request["action"] == null ? string.Empty :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["action"].ToString());
            }
        }

        public string DataStr
        {
            get
            {
                return HttpContext.Current.Request["data"] == null ? string.Empty :
                    HttpContext.Current.Request["data"].ToString();
            }
            set
            {

            }
        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string msg = "";
            int userID = 0;
            int retSiid = 0;

            try
            {

                if (BLL.Util.CheckButtonRight("SYS024BUT5003") || BLL.Util.CheckButtonRight("SYS024BUT5001"))
                {
                    CheckMsg(out msg);

                    if (msg == "")
                    {
                        userID = BitAuto.ISDC.CC2012.BLL.Util.GetLoginUserID();
                        Submit(out msg, userID, out retSiid);
                    }
                }
                else
                {
                    msg += "您没有添加和编辑问卷的权限！";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }

            if (msg == "")
            {
                msg = "success_" + retSiid.ToString();
            }

            context.Response.Write(msg);
        }

        private void Submit(out string msg, int userID, out int retSiid)
        {
            msg = "";
            retSiid = 0;
            int retsqid = 0;

            string datainfoStr = DataStr;
            SurveyInfoData sInfoData = null;
            sInfoData = (SurveyInfoData)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(datainfoStr, typeof(SurveyInfoData));

            #region 验证数据正确性

            if (sInfoData != null)
            {
                CheckData(sInfoData, out msg);

                if (msg != "")
                {
                    return;
                }

            }
            else
            {
                msg += "获取数据出错";
                return;
            }
            #endregion

            #region 准备数据

            #region 定义变量

            Entities.SurveyInfo sModel = null;
            Entities.SurveyQuestion qModel = null;
            Entities.SurveyOption oModel = null;
            Entities.SurveyMatrixTitle mModel = null;

            List<Entities.SurveyQuestion> qList = null;
            List<Entities.SurveyOption> oList = null;
            List<Entities.SurveyMatrixTitle> mList = null;
            List<Entities.SurveyOptionSkipQuestion> skipNewList = new List<SurveyOptionSkipQuestion>();//要新加入的
            List<Entities.SurveyOptionSkipQuestion> skipUpdateList = new List<SurveyOptionSkipQuestion>();//要更新的
            List<Entities.SurveyOptionSkipQuestion> skipDeleteList = new List<SurveyOptionSkipQuestion>();//要删除的

            List<Entities.SurveyQuestion> addQudstionList = new List<SurveyQuestion>();
            List<Entities.SurveyOption> addOptionList = new List<SurveyOption>();
            List<Entities.SurveyMatrixTitle> addMatrixList = new List<SurveyMatrixTitle>();

            List<StringBuilder> listLogStr = new List<StringBuilder>(); //用户操作日志
            StringBuilder sblogstr = new StringBuilder();

            string logstr = "";
            int intVal = 0;
            int existflog = 0;

            OptionSkip skipTemp = null;
            List<OptionSkip> skipListTemp = new List<OptionSkip>();

            #endregion

            if (sInfoData.siid != "" && int.TryParse(sInfoData.siid, out intVal))
            {
                int siid = int.Parse(sInfoData.siid);

                #region 编辑

                #region 获取问卷

                sModel = BLL.SurveyInfo.Instance.GetSurveyInfo(siid);

                #region 判断状态

                if ((sModel.Status != 0 && sModel.Status != 1) && Action == "sub")
                {
                    msg += "当前状态下不允许提交操作";
                    return;
                }
                if (sModel.Status != 0 && Action == "save")
                {
                    msg += "当前状态下不允许保存操作";
                    return;
                }
                if (sModel.Status != 0 && Action == "preview")  // || Action == "preview")
                {
                    msg += "当前状态下不允许预览操作";
                    return;
                }

                #endregion

                #endregion

                #region 获取问题

                qList = BLL.SurveyQuestion.Instance.GetSurveyQuestionList(siid);

                #endregion

                #region 获取选项

                oList = BLL.SurveyOption.Instance.GetSurveyOptionList(siid);

                #endregion

                #region 获取矩阵标题

                mList = BLL.SurveyMatrixTitle.Instance.GetMatrixTitleList(siid);

                #endregion

                if (sModel != null)
                {
                    #region 修改问卷信息

                    sModel.Name = sInfoData.name;
                    sModel.BGID = int.Parse(sInfoData.bgid);
                    sModel.SCID = int.Parse(sInfoData.scid);
                    sModel.Description = sInfoData.desc;
                    if (Action == "sub")
                    {
                        sModel.Status = 1;
                        sModel.IsAvailable = 1;//提交后是启用状态
                    }
                    else
                    {
                        sModel.Status = 0;
                        sModel.IsAvailable = -1;//不是提交，可用状态为空

                    }

                    #region 记日志

                    sblogstr = new StringBuilder();
                    logstr = "更新了调查问卷‘" + sInfoData.name + "’的信息【ID：" + sModel.SIID + "】";
                    sblogstr.Append(logstr);
                    listLogStr.Add(sblogstr);

                    #endregion

                    #endregion

                    #region 修改问题

                    #region 判断是 新增 or 编辑 or 删除

                    #region 判断编辑和删除的

                    foreach (Entities.SurveyQuestion qitem in qList)
                    {
                        existflog = 0;
                        if (sInfoData.questList != null)
                        {
                            foreach (SurveyQuestionInfoData qDataItem in sInfoData.questList)
                            {
                                if (qDataItem.sqid == qitem.SQID.ToString())
                                {
                                    //有，就是编辑
                                    existflog = 1;
                                    break;
                                }
                            }
                        }

                        if (existflog == 0)
                        {
                            //在页面传来的对象中没有，是删除
                            qitem.actionFlog = -1;

                            #region 记日志

                            sblogstr = new StringBuilder();
                            logstr = "删除了调查问卷【" + sInfoData.name + "】下的问题‘" + qitem.Ask + "’【SQID：" + qitem.SQID + ",SIID:" + sInfoData.siid + "】";
                            sblogstr.Append(logstr);
                            listLogStr.Add(sblogstr);

                            #endregion
                        }
                        else
                        {
                            qitem.actionFlog = 0;
                        }
                    }

                    #endregion

                    #region 判断新加的问题

                    if (sInfoData.questList != null)
                    {
                        foreach (SurveyQuestionInfoData qDataItem in sInfoData.questList)
                        {
                            existflog = 0;
                            foreach (Entities.SurveyQuestion qitem in qList)
                            {
                                if (qitem.SQID.ToString() == qDataItem.sqid)
                                {
                                    existflog = 1;
                                }
                            }
                            if (existflog == 0)
                            {
                                //没找到，就是新加的
                                qModel = new SurveyQuestion();
                                qModel.SIID = siid;
                                qModel.Ask = qDataItem.ask;
                                qModel.AskCategory = int.Parse(qDataItem.askcategory);
                                qModel.SQID = int.Parse(qDataItem.sqid);
                                qModel.ShowColumnNum = int.Parse(qDataItem.showcolumnnum);
                                qModel.MaxTextLen = int.Parse(qDataItem.maxtextlen);
                                qModel.MinTextLen = int.Parse(qDataItem.mintextlen);
                                qModel.Status = 0;
                                qModel.OrderNum = int.Parse(qDataItem.ordernum);
                                qModel.CreateTime = DateTime.Now;
                                qModel.CreateUserID = userID;
                                qModel.ModifyTime = DateTime.Now;
                                qModel.ModifyUserID = userID;
                                qModel.IsMustAnswer = int.Parse(qDataItem.IsMustAnswer);
                                qModel.IsStatByScore = int.Parse(qDataItem.IsStatByScore);
                                qModel.QuestionLinkId = int.Parse(qDataItem.QuestionLinkId);

                                addQudstionList.Add(qModel);

                                #region 记日志

                                sblogstr = new StringBuilder();
                                logstr = "添加了调查问卷【" + sInfoData.name + "】下的问题‘" + qDataItem.ask + "’【SIID:" + sInfoData.siid + "】";
                                sblogstr.Append(logstr);
                                listLogStr.Add(sblogstr);

                                #endregion
                            }
                        }
                    }

                    #endregion

                    #endregion

                    #region 修改编辑的问题

                    foreach (Entities.SurveyQuestion qitem in qList)
                    {
                        if (qitem.actionFlog == 0) //是编辑
                        {
                            if (sInfoData.questList != null)
                            {
                                foreach (SurveyQuestionInfoData qDataItem in sInfoData.questList)
                                {
                                    if (qDataItem.sqid == qitem.SQID.ToString())
                                    {
                                        qitem.Ask = qDataItem.ask;
                                        qitem.ShowColumnNum = int.Parse(qDataItem.showcolumnnum);
                                        qitem.MaxTextLen = int.Parse(qDataItem.maxtextlen);
                                        qitem.MinTextLen = int.Parse(qDataItem.mintextlen);
                                        qitem.OrderNum = int.Parse(qDataItem.ordernum);
                                        qitem.ModifyTime = DateTime.Now;
                                        qitem.ModifyUserID = userID;
                                        qitem.IsMustAnswer = int.Parse(qDataItem.IsMustAnswer);
                                        qitem.IsStatByScore = int.Parse(qDataItem.IsStatByScore);
                                        qitem.QuestionLinkId = int.Parse(qDataItem.QuestionLinkId);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #endregion

                    #region 修改选项

                    #region 判断编辑和删除

                    foreach (Entities.SurveyOption oItem in oList)
                    {
                        existflog = 0;

                        if (sInfoData.questList != null)
                        {
                            foreach (SurveyQuestionInfoData qDataItem in sInfoData.questList)
                            {
                                if (qDataItem.sqid == oItem.SQID.ToString())
                                {
                                    if (qDataItem.option != null)
                                    {
                                        foreach (SurveyOptionInfoData oinfoItem in qDataItem.option)
                                        {
                                            if (oinfoItem.sqid == oItem.SQID.ToString() && oinfoItem.soid == oItem.SOID.ToString())
                                            {
                                                existflog = 1;//找到了，就是编辑
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (existflog == 1)
                        {
                            //编辑
                            oItem.actionFlog = 0;
                        }
                        else
                        {
                            //没找到，就是删除
                            oItem.actionFlog = -1;

                            #region 记日志

                            sblogstr = new StringBuilder();
                            logstr = "删除了调查问卷【" + sInfoData.name + "】中的选项‘" + oItem.OptionName + "’【SOID:" + oItem.SOID + ",SIID:" + sInfoData.siid + "】";
                            sblogstr.Append(logstr);
                            listLogStr.Add(sblogstr);

                            #endregion
                        }

                    }

                    #endregion

                    #region 判断新增

                    if (sInfoData.questList != null)
                    {
                        foreach (SurveyQuestionInfoData qDataItem in sInfoData.questList)
                        {
                            if (qDataItem.option != null)
                            {
                                foreach (SurveyOptionInfoData oinfoItem in qDataItem.option)
                                {
                                    if (int.Parse(oinfoItem.soid) < 0)
                                    {
                                        //新增的
                                        oModel = new SurveyOption();
                                        oModel.SOID = int.Parse(oinfoItem.soid);
                                        oModel.SIID = siid;
                                        oModel.SQID = int.Parse(oinfoItem.sqid);
                                        oModel.OptionName = oinfoItem.optionname;
                                        oModel.IsBlank = int.Parse(oinfoItem.isblank);
                                        oModel.Score = int.Parse(oinfoItem.score);
                                        oModel.OrderNum = int.Parse(oinfoItem.ordernum);
                                        oModel.Status = 0;
                                        oModel.CreateTime = DateTime.Now;
                                        oModel.CreateUserID = userID;
                                        oModel.ModifyTime = DateTime.Now;
                                        oModel.ModifyUserID = userID;
                                        oModel.linkid = int.Parse(oinfoItem.linkid);

                                        addOptionList.Add(oModel);

                                        #region 记日志

                                        sblogstr = new StringBuilder();
                                        logstr = "添加了调查问卷【" + sInfoData.name + "】中的选项‘" + oModel.OptionName + "’【SIID:" + sInfoData.siid + "】";
                                        sblogstr.Append(logstr);
                                        listLogStr.Add(sblogstr);

                                        #endregion
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region 修改编辑的选项

                    foreach (Entities.SurveyOption oitem in oList)
                    {
                        if (oitem.actionFlog == 0) //是编辑
                        {
                            if (sInfoData.questList != null)
                            {
                                foreach (SurveyQuestionInfoData qDataItem in sInfoData.questList)
                                {
                                    if (qDataItem.option != null)
                                    {
                                        foreach (SurveyOptionInfoData oDataItem in qDataItem.option)
                                        {
                                            if (oDataItem.soid == oitem.SOID.ToString())
                                            {
                                                oitem.OptionName = oDataItem.optionname;
                                                oitem.IsBlank = int.Parse(oDataItem.isblank);
                                                oitem.Score = int.Parse(oDataItem.score);
                                                oitem.OrderNum = int.Parse(oDataItem.ordernum);
                                                oitem.ModifyTime = DateTime.Now;
                                                oitem.ModifyUserID = userID;
                                                oitem.linkid = int.Parse(oDataItem.linkid);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #endregion

                    #region 修改矩阵

                    #region 判断编辑和删除

                    foreach (Entities.SurveyMatrixTitle mItem in mList)
                    {
                        existflog = 0;//从页面对象中找
                        if (sInfoData.questList != null)
                        {
                            foreach (SurveyQuestionInfoData qDataItem in sInfoData.questList)
                            {
                                if (qDataItem.sqid == mItem.SQID.ToString())
                                {
                                    if (qDataItem.matrix != null)
                                    {
                                        foreach (SurveyMatiexInfoData minfoItem in qDataItem.matrix)
                                        {
                                            if (minfoItem.sqid == mItem.SQID.ToString() && minfoItem.smtid == mItem.SMTID.ToString())
                                            {
                                                existflog = 1;//找到了，就是编辑
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (existflog == 1)
                        {
                            //编辑
                            mItem.actionFlog = 0;
                        }
                        else
                        {
                            //没找到，就是删除
                            mItem.actionFlog = -1;

                            #region 记日志

                            sblogstr = new StringBuilder();
                            logstr = "删除了调查问卷【" + sInfoData.name + "】中的矩阵标题‘" + mItem.TitleName + "’【SMTID:" + mItem.SMTID + ",SIID:" + sInfoData.siid + "】";
                            sblogstr.Append(logstr);
                            listLogStr.Add(sblogstr);

                            #endregion
                        }

                    }

                    #endregion

                    #region 判断新增

                    if (sInfoData.questList != null)
                    {
                        foreach (SurveyQuestionInfoData qDataItem in sInfoData.questList)
                        {
                            if (qDataItem.matrix != null)
                            {
                                foreach (SurveyMatiexInfoData minfoItem in qDataItem.matrix)
                                {
                                    if (minfoItem.smtid == "")
                                    {
                                        //新增的
                                        mModel = new SurveyMatrixTitle();
                                        mModel.SIID = siid;
                                        mModel.SQID = int.Parse(minfoItem.sqid);
                                        mModel.TitleName = minfoItem.titlename;
                                        mModel.Status = 0;
                                        mModel.Type = int.Parse(minfoItem.type);
                                        mModel.CreateUserID = userID;
                                        mModel.CreateTime = DateTime.Now;

                                        addMatrixList.Add(mModel);

                                        #region 记日志

                                        sblogstr = new StringBuilder();
                                        logstr = "添加了调查问卷【" + sInfoData.name + "】中的矩阵标题‘" + mModel.TitleName + "’【SIID:" + sInfoData.siid + "】";
                                        sblogstr.Append(logstr);
                                        listLogStr.Add(sblogstr);

                                        #endregion
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region 修改编辑的矩阵

                    foreach (Entities.SurveyMatrixTitle mitem in mList)
                    {
                        if (mitem.actionFlog == 0) //是编辑
                        {
                            if (sInfoData.questList != null)
                            {
                                foreach (SurveyQuestionInfoData qDataItem in sInfoData.questList)
                                {
                                    if (qDataItem.matrix != null)
                                    {
                                        foreach (SurveyMatiexInfoData mDataItem in qDataItem.matrix)
                                        {
                                            if (mDataItem.smtid == mitem.SMTID.ToString())
                                            {
                                                mitem.TitleName = mDataItem.titlename;
                                                mitem.Type = int.Parse(mDataItem.type);

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    msg += "没找到相应问卷<br/>";
                }

                #endregion
            }
            else
            {
                //新增

                #region 问卷实体类

                sModel = new Entities.SurveyInfo();
                sModel.Name = sInfoData.name;
                sModel.BGID = int.Parse(sInfoData.bgid);
                sModel.SCID = int.Parse(sInfoData.scid);
                sModel.Description = sInfoData.desc;
                if (Action == "sub")
                {
                    sModel.Status = 1;
                    sModel.IsAvailable = 1;//提交后是启用状态
                }
                else
                {
                    sModel.Status = 0;
                    sModel.IsAvailable = -1;//不是提交，可用状态为空
                }

                sModel.CreateTime = DateTime.Now;
                sModel.CreateUserID = userID;

                #region 记日志

                sblogstr = new StringBuilder();
                logstr = "添加了调查问卷【" + sInfoData.name + "】";
                sblogstr.Append(logstr);
                listLogStr.Add(sblogstr);

                #endregion

                #endregion

                #region 问卷问题

                qList = new List<Entities.SurveyQuestion>();
                oList = new List<Entities.SurveyOption>();
                mList = new List<Entities.SurveyMatrixTitle>();

                if (sInfoData.questList != null)
                {
                    foreach (SurveyQuestionInfoData item in sInfoData.questList)
                    {
                        #region 问卷问题

                        qModel = new Entities.SurveyQuestion();
                        qModel.SQID = int.Parse(item.sqid);
                        qModel.Ask = item.ask;
                        qModel.AskCategory = int.Parse(item.askcategory);
                        qModel.ShowColumnNum = int.Parse(item.showcolumnnum);
                        qModel.MaxTextLen = int.Parse(item.maxtextlen);
                        qModel.MinTextLen = int.Parse(item.mintextlen);
                        qModel.Status = 0;
                        qModel.OrderNum = int.Parse(item.ordernum);
                        qModel.CreateTime = DateTime.Now;
                        qModel.CreateUserID = userID;
                        qModel.ModifyTime = DateTime.Now;
                        qModel.ModifyUserID = userID;
                        qModel.IsMustAnswer = int.Parse(item.IsMustAnswer);
                        qModel.IsStatByScore = int.Parse(item.IsStatByScore);
                        qModel.QuestionLinkId = int.Parse(item.QuestionLinkId);

                        qList.Add(qModel);

                        #endregion

                        #region 问卷选项

                        if (item.option != null)
                        {
                            foreach (SurveyOptionInfoData oitem in item.option)
                            {
                                if (oitem.sqid == item.sqid)
                                {
                                    oModel = new Entities.SurveyOption();
                                    oModel.SOID = int.Parse(oitem.soid);
                                    oModel.SQID = int.Parse(oitem.sqid);
                                    oModel.OptionName = oitem.optionname;
                                    oModel.IsBlank = int.Parse(oitem.isblank);
                                    oModel.Score = int.Parse(oitem.score);
                                    oModel.OrderNum = int.Parse(oitem.ordernum);
                                    oModel.Status = 0;
                                    oModel.CreateTime = DateTime.Now;
                                    oModel.CreateUserID = userID;
                                    oModel.ModifyTime = DateTime.Now;
                                    oModel.ModifyUserID = userID;
                                    oModel.linkid = int.Parse(oitem.linkid);

                                    oList.Add(oModel);
                                }
                            }
                        }

                        #endregion

                        #region 问卷矩阵标题

                        if (item.matrix != null)
                        {
                            foreach (SurveyMatiexInfoData mitem in item.matrix)
                            {
                                if (mitem.sqid == item.sqid)
                                {
                                    mModel = new Entities.SurveyMatrixTitle();
                                    mModel.SQID = int.Parse(mitem.sqid);
                                    mModel.TitleName = mitem.titlename;
                                    mModel.Status = 0;
                                    mModel.Type = int.Parse(mitem.type);
                                    mModel.CreateTime = DateTime.Now;
                                    mModel.CreateUserID = userID;

                                    mList.Add(mModel);
                                }
                            }
                        }

                        #endregion

                    }
                }

                #endregion

            }



            #endregion

            #region 建立选项与跳题对照

            if (oList != null)
            {
                foreach (Entities.SurveyOption oItem in oList)
                {
                    skipTemp = new OptionSkip();
                    skipTemp.Soid = oItem.SOID;
                    skipTemp.Sqid = (int)oItem.SQID;
                    skipTemp.LinkId = oItem.linkid;

                    skipListTemp.Add(skipTemp);
                }
            }

            if (addOptionList != null)
            {
                foreach (Entities.SurveyOption oItem in addOptionList)
                {
                    skipTemp = new OptionSkip();
                    skipTemp.Soid = oItem.SOID;
                    skipTemp.Sqid = (int)oItem.SQID;
                    skipTemp.LinkId = oItem.linkid;

                    skipListTemp.Add(skipTemp);
                }
            }


            #endregion

            #region 提交事务

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");

            try
            {
                if (sInfoData.siid != "" && int.TryParse(sInfoData.siid, out intVal))
                {
                    //编辑
                    retSiid = int.Parse(SIID);

                    #region 编辑问卷

                    BLL.SurveyInfo.Instance.Update(tran, sModel);

                    #endregion

                    #region 编辑问题

                    #region 删除、编辑问题

                    foreach (Entities.SurveyQuestion qitem in qList)
                    {
                        if (qitem.actionFlog == 0)
                        {
                            //编辑
                            BLL.SurveyQuestion.Instance.Update(tran, qitem);

                            //维护临时对照表
                            foreach (OptionSkip skipItem in skipListTemp)
                            {
                                if (skipItem.LinkId == qitem.QuestionLinkId)
                                {
                                    skipItem.LinkId = qitem.SQID;
                                }
                            }
                        }
                        else if (qitem.actionFlog == -1)
                        {
                            //删除
                            retsqid = BLL.SurveyQuestion.Instance.Delete(tran, qitem.SQID);
                        }
                    }

                    #endregion

                    #region 添加问题

                    foreach (Entities.SurveyQuestion qItem in addQudstionList)
                    {
                        retsqid = BLL.SurveyQuestion.Instance.Insert(tran, qItem);

                        if (retsqid > 0)
                        {
                            //维护临时对照表
                            foreach (OptionSkip skipItem in skipListTemp)
                            {
                                if (skipItem.Sqid == qItem.SQID)//新增的SQID都是 临时负数
                                {
                                    skipItem.Sqid = retsqid;
                                }
                                if (skipItem.LinkId == qItem.QuestionLinkId)
                                {
                                    skipItem.LinkId = retsqid;
                                }
                            }

                            //维护选项的问卷ID
                            foreach (Entities.SurveyOption oItem in addOptionList)
                            {
                                if (oItem.SQID == qItem.SQID)
                                {
                                    oItem.SQID = retsqid;
                                }
                            }
                            //维护矩阵的问卷ID
                            foreach (Entities.SurveyMatrixTitle mItem in addMatrixList)
                            {
                                if (mItem.SQID == qItem.SQID)
                                {
                                    mItem.SQID = retsqid;
                                }
                            }
                        }
                        else
                        {
                            msg += "添加问题失败";
                            return;
                        }
                    }

                    #endregion

                    #endregion

                    #region 添加选项

                    #region 删除、编辑选项
                    foreach (Entities.SurveyOption oItem in oList)
                    {
                        if (oItem.actionFlog == 0)
                        {
                            //编辑选项
                            BLL.SurveyOption.Instance.Update(tran, oItem);

                        }
                        else if (oItem.actionFlog == -1)
                        {
                            BLL.SurveyOption.Instance.Delete(tran, oItem.SOID);
                        }
                    }

                    #endregion

                    #region 添加选项

                    foreach (Entities.SurveyOption oItem in addOptionList)
                    {
                        int retSOID = BLL.SurveyOption.Instance.Insert(tran, oItem);
                        if (retSOID > 0)
                        {
                            //维护临时对照表
                            foreach (OptionSkip skipItem in skipListTemp)
                            {
                                if (skipItem.Soid == oItem.SOID)
                                {
                                    skipItem.Soid = retSOID;
                                }
                            }
                        }
                    }

                    #endregion

                    #endregion

                    #region 添加矩阵标题

                    #region 删除、编辑矩阵标题
                    foreach (Entities.SurveyMatrixTitle mItem in mList)
                    {
                        if (mItem.actionFlog == 0)
                        {
                            //编辑选项
                            BLL.SurveyMatrixTitle.Instance.Update(tran, mItem);

                        }
                        else if (mItem.actionFlog == -1)
                        {
                            BLL.SurveyMatrixTitle.Instance.Delete(tran, mItem.SMTID);
                        }
                    }

                    #endregion

                    #region 添加矩阵标题

                    foreach (Entities.SurveyMatrixTitle mItem in addMatrixList)
                    {
                        BLL.SurveyMatrixTitle.Instance.Insert(tran, mItem);
                    }

                    #endregion

                    #endregion
                }
                else
                {

                    #region 保存问卷

                    retSiid = BLL.SurveyInfo.Instance.Insert(tran, sModel);

                    #endregion

                    #region 保存问题

                    foreach (SurveyQuestion qitem in qList)
                    {
                        qitem.SIID = retSiid;
                        retsqid = BLL.SurveyQuestion.Instance.Insert(tran, qitem);

                        //维护临时对照表
                        foreach (OptionSkip skipItem in skipListTemp)
                        {
                            if (skipItem.Sqid == qitem.SQID)//新增的SQID都是 临时负数
                            {
                                skipItem.Sqid = retsqid;
                            }
                            if (skipItem.LinkId == qitem.QuestionLinkId)
                            {
                                skipItem.LinkId = retsqid;
                            }
                        }

                        #region 保存问题选项

                        foreach (SurveyOption oitem in oList)
                        {
                            if (oitem.SQID == qitem.SQID)
                            {
                                oitem.SIID = retSiid;
                                oitem.SQID = retsqid;

                                int retSoid = BLL.SurveyOption.Instance.Insert(tran, oitem);

                                //维护临时对照表
                                foreach (OptionSkip skipItem in skipListTemp)
                                {
                                    if (skipItem.Soid == oitem.SOID)
                                    {
                                        skipItem.Soid = retSoid;
                                    }
                                }

                            }
                        }

                        #endregion

                        #region  保存矩阵标题

                        foreach (SurveyMatrixTitle mitem in mList)
                        {
                            if (mitem.SQID == qitem.SQID)
                            {
                                mitem.SIID = retSiid;
                                mitem.SQID = retsqid;

                                BLL.SurveyMatrixTitle.Instance.Insert(tran, mitem);
                            }
                        }

                        #endregion

                    }
                    #endregion

                }
                #region 更新选项跳题对照表

                foreach (OptionSkip item in skipListTemp)
                {
                    Entities.SurveyOptionSkipQuestion curskipModel = null;
                    curskipModel = BLL.SurveyOptionSkipQuestion.Instance.GetModelBySoid(item.Soid);
                    if (curskipModel != null)
                    {
                        //存在跳题

                        if (item.LinkId == 0)
                        {
                            skipDeleteList.Add(curskipModel);
                        }
                        else
                        {
                            if (curskipModel.SQID != item.LinkId)
                            {
                                //如果跳题有改变
                                curskipModel.SQID = item.LinkId;
                                skipUpdateList.Add(curskipModel);

                                // BLL.SurveyOptionSkipQuestion.Instance.Update(tran,curskipModel);
                            }
                        }
                    }
                    else
                    {
                       //不存在
                        if (item.LinkId != 0)
                        {
                            //,就插入
                            curskipModel = new SurveyOptionSkipQuestion();
                            curskipModel.SOID = item.Soid;
                            curskipModel.SQID = item.LinkId;
                            curskipModel.Status = 0;
                            skipNewList.Add(curskipModel);
                            // BLL.SurveyOptionSkipQuestion.Instance.Insert(tran,curskipModel);
                        }
                    }
                }

                foreach (Entities.SurveyOptionSkipQuestion item in skipDeleteList)
                {
                    BLL.SurveyOptionSkipQuestion.Instance.Delete(tran, item.RecID);
                }
                foreach (Entities.SurveyOptionSkipQuestion item in skipUpdateList)
                {
                    BLL.SurveyOptionSkipQuestion.Instance.Update(tran, item);
                }
                foreach (Entities.SurveyOptionSkipQuestion item in skipNewList)
                {
                    BLL.SurveyOptionSkipQuestion.Instance.Insert(tran, item);
                }

                #endregion


                #region 保存用户操作日志

                foreach (StringBuilder sbStr in listLogStr)
                {
                    BLL.Util.InsertUserLog(tran, sbStr.ToString());
                }
                #endregion

                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran.Connection != null)
                {
                    tran.Rollback();
                }
                msg = ex.Message.ToString();
            }
            finally
            {
                connection.Close();
            }


            #endregion

        }

        private void CheckData(SurveyInfoData sInfoData, out string msg)
        {
            msg = "";

            int intVal = 0;

            if (sInfoData.name == "")
            {
                msg += "问卷名称不能为空<br>";
            }
            if (!int.TryParse(sInfoData.bgid, out intVal))
            {
                msg += "业务分组ID格式不正确";
            }
            if (!int.TryParse(sInfoData.scid, out intVal))
            {
                msg += "业务分组ID格式不正确";
            }
            if (sInfoData.bgid == "-1")
            {
                msg += "问卷名称不能为空<br>";
            }
            if (sInfoData.scid == "-1")
            {
                msg += "分类ID不能为空<br>";
            }


            if (sInfoData.questList != null)
            {
                foreach (SurveyQuestionInfoData qItem in sInfoData.questList)
                {
                    #region 检查问卷问题数据

                    if (qItem.ask == "")
                    {
                        msg += "问题名称不能为空";
                    }
                    if (!int.TryParse(qItem.askcategory, out intVal))
                    {
                        msg += "问题分类ID格式不正确";
                    }
                    if (!int.TryParse(qItem.showcolumnnum, out intVal))
                    {
                        msg += "问题显示列数格式不正确";
                    }
                    if (!int.TryParse(qItem.maxtextlen, out intVal))
                    {
                        msg += "问题文本最大长度格式不正确";
                    }
                    if (!int.TryParse(qItem.mintextlen, out intVal))
                    {
                        msg += "问题文本最小长度格式不正确";
                    }
                    if (!int.TryParse(qItem.ordernum, out intVal))
                    {
                        msg += "问题排序数字格式不正确";
                    }
                    if (!int.TryParse(qItem.IsMustAnswer, out intVal))
                    {
                        msg += "是否必答数字格式不正确";
                    }
                    if (!int.TryParse(qItem.IsStatByScore, out intVal))
                    {
                        msg += "是否按评分统计数字格式不正确";
                    }

                    #endregion

                    #region 检查问卷选项数据

                    if (qItem.option != null)
                    {
                        foreach (SurveyOptionInfoData oItem in qItem.option)
                        {

                            if (oItem.optionname == "")
                            {
                                msg += "选项名称不能为空";
                            }
                            if (!int.TryParse(oItem.isblank, out intVal))
                            {
                                msg += "选项中是否显示输入框标志格式不正确";
                            }
                            if (!int.TryParse(oItem.score, out intVal))
                            {
                                msg += "选项中分数格式不正确";
                            }
                            if (!int.TryParse(oItem.ordernum, out intVal))
                            {
                                msg += "选项中排序数字格式不正确";
                            }
                            if (!int.TryParse(oItem.linkid, out intVal))
                            {
                                msg += "选项中跳题链接数字格式不正确";
                            }
                        }
                    }

                    #endregion

                    #region 检查问卷矩阵标题数据

                    if (qItem.matrix != null)
                    {
                        foreach (SurveyMatiexInfoData mItem in qItem.matrix)
                        {

                            if (mItem.titlename == "")
                            {
                                msg += "矩阵标题名称不能为空";
                            }
                            if (!int.TryParse(mItem.type, out intVal))
                            {
                                msg += "矩阵标题类型格式不正确";
                            }

                        }
                    }

                    #endregion

                }
            }
        }

        private void CheckMsg(out string msg)
        {
            msg = "";
            int intVal = 0;

            if (Action == string.Empty || (Action != "save" && Action != "sub" && Action != "preview"))
            {
                msg += "操作类型参数不正确";
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