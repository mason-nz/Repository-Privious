using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoringManage
{
    /// <summary>
    /// BaseScoring 的摘要说明
    /// </summary>
    public abstract class BaseScoring : IHttpHandler, IRequiresSessionState
    {
        /// 动作
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
        /// 评分表id
        /// <summary>
        /// 评分表id
        /// </summary>
        public string QS_RTID
        {
            get
            {
                return HttpContext.Current.Request["QS_RTID"] == null ? string.Empty :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["QS_RTID"].ToString());
            }
        }
        /// 数据
        /// <summary>
        /// 数据
        /// </summary>
        public string DataStr
        {
            get
            {
                return HttpContext.Current.Request["data"] == null ? string.Empty :
                    HttpContext.Current.Request["data"].ToString();
            }
        }
        /// 话务表后缀名称
        /// <summary>
        /// 话务表后缀名称
        /// </summary>
        public string tableEndName
        {
            get
            {
                return HttpContext.Current.Request["tableEndName"] == null ? string.Empty :
                    HttpContext.Current.Request["tableEndName"].ToString();
            }
        }

        /// 入口
        /// <summary>
        /// 入口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";
            int retQS_RID = 0;
            try
            {
                CheckMsg(out msg);
                if (msg == "")
                {
                    int userId = BLL.Util.GetLoginUserID();
                    switch (Action)
                    {
                        case "save":
                            Save(out msg, userId, out retQS_RID);
                            break;
                        case "subsave":
                            SubInfo(out msg, userId, out retQS_RID);
                            break;
                        case "substate":
                            SubState(out msg, userId, out retQS_RID);
                            break;
                        case "scoretablerereview":
                            ScoreTableRereview(out msg, userId);
                            break;
                        case "scoretablereject":
                            ScoreTableReject(out msg, userId);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                BLL.Loger.Log4Net.Error("【保存/提交异常】",ex);
            }
            if (msg == "" && retQS_RID != 0)
            {
                msg = "success_" + retQS_RID.ToString();
            }
            else if (msg == "")
            {
                msg = "success";
            }
            context.Response.Write(msg);
        }

        #region 辅助
        /// 验证请求是否正确
        /// <summary>
        /// 验证请求是否正确
        /// </summary>
        /// <param name="msg"></param>
        private void CheckMsg(out string msg)
        {
            msg = "";
            if (Action == string.Empty || (Action != "save" && Action != "subsave" && Action != "substate" && Action != "scoretablerereview" && Action != "scoretablereject"))
            {
                msg += "操作类型参数不正确";
                return;
            }
        }
        /// 验证数据格式
        /// <summary>
        /// 验证数据格式
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="msg"></param>
        private void CheckData(QS_ResultData sInfoData, out string msg)
        {
            msg = "";
            int intVal = 0;
            if (!int.TryParse(sInfoData.QS_RID, out intVal))
            {
                msg += "评分成绩主键ID格式不正确<br>";
            }
            //自定义校验
            CheckDataForDataID(sInfoData, ref msg);
            if (!int.TryParse(sInfoData.QS_RTID, out intVal))
            {
                msg += "质检表主键ID不正确<br>";
            }
            if (!int.TryParse(sInfoData.ScoreType, out intVal))
            {
                msg += "质检表类型不正确<br>";
            }
            if (!int.TryParse(sInfoData.NoDeadNum, out intVal))
            {
                msg += "质检表非致命项数格式不正确<br>";
            }
            if (!int.TryParse(sInfoData.DeadNum, out intVal))
            {
                msg += "质检表致命项数格式不正确<br>";
            }

            if (sInfoData.QS_ResultDetailList != null)
            {
                foreach (QS_ResultDetailData qItem in sInfoData.QS_ResultDetailList)
                {
                    #region 检查评分明细数据
                    if (!string.IsNullOrEmpty(qItem.QS_RDID))
                    {
                        if (!int.TryParse(qItem.QS_RDID, out intVal))
                        {
                            msg += "评分结果明细表主键ID数据格式不正确<br>";
                        }
                    }
                    if (!int.TryParse(qItem.ScoreType, out intVal))
                    {
                        msg += "评分结果明细表评分表类型数据格式不正确<br>";
                    }
                    if (!int.TryParse(qItem.QS_RTID, out intVal))
                    {
                        msg += "质检表主键ID数据格式不正确<br>";
                    }
                    if (!int.TryParse(qItem.QS_RID, out intVal))
                    {
                        msg += "评分结果主键ID数据格式不正确<br>";
                    }

                    if (!string.IsNullOrEmpty(qItem.QS_CID))
                    {
                        if (!int.TryParse(qItem.QS_CID, out intVal))
                        {
                            msg += "明细表中评分分类数字格式不正确<br>";
                        }
                    }
                    if (!string.IsNullOrEmpty(qItem.QS_IID))
                    {
                        if (!int.TryParse(qItem.QS_IID, out intVal))
                        {
                            msg += "明细表中质检项目ID数字格式不正确<br>";
                        }
                    }
                    if (!string.IsNullOrEmpty(qItem.QS_SID))
                    {
                        if (!int.TryParse(qItem.QS_SID, out intVal))
                        {
                            msg += "明细表中质检标准ID数字格式不正确<br>";
                        }
                    }
                    if (!string.IsNullOrEmpty(qItem.QS_MID))
                    {
                        if (!int.TryParse(qItem.QS_MID, out intVal))
                        {
                            msg += "明细表中评分扣分项ID数字格式不正确<br>";
                        }
                    }
                    if (!string.IsNullOrEmpty(qItem.QS_MID_End))
                    {
                        if (!int.TryParse(qItem.QS_MID_End, out intVal))
                        {
                            msg += "明细表中评分扣分项最终ID数字格式不正确<br>";
                        }
                    }
                    if (!string.IsNullOrEmpty(qItem.ScoreDeadID))
                    {
                        if (!int.TryParse(qItem.ScoreDeadID, out intVal))
                        {
                            msg += "明细表中评分致命项ID数字格式不正确<br>";
                        }
                    }
                    if (!string.IsNullOrEmpty(qItem.ScoreDeadID_End))
                    {
                        if (!int.TryParse(qItem.ScoreDeadID_End, out intVal))
                        {
                            msg += "明细表中评分致命项ID数字格式不正确<br>";
                        }
                    }
                    #endregion
                }
            }
        }
        /// 解析界面传递的数据，返回实体类型
        /// <summary>
        /// 解析界面传递的数据，返回实体类型
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="sInfoData"></param>
        /// <returns></returns>
        private bool GetData(out string msg, out QS_ResultData sInfoData)
        {
            bool ischeck = false;
            msg = "";
            string datainfoStr = DataStr;
            sInfoData = null;
            sInfoData = (QS_ResultData)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(datainfoStr, typeof(QS_ResultData));

            #region 验证数据正确性
            if (sInfoData != null)
            {
                CheckData(sInfoData, out msg);
                if (msg != "")
                {
                    ischeck = true;
                }
            }
            else
            {
                msg += "获取数据出错";
                ischeck = true;
            }
            #endregion
            return ischeck;
        }
        /// 明细表维护
        /// <summary>
        /// 明细表维护
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="sInfoData"></param>
        /// <param name="QS_RID"></param>
        /// <param name="HaveDead"></param>
        private void InsertUpdateResultDetail(int userID, QS_ResultData sInfoData, int QS_RID, out bool HaveDead)
        {
            HaveDead = false;
            BLL.QS_ResultDetail.Instance.DeleteByQS_RID(QS_RID);
            if (sInfoData.QS_ResultDetailList != null && sInfoData.QS_ResultDetailList.Length > 0)
            {
                for (int i = 0; i < sInfoData.QS_ResultDetailList.Length; i++)
                {
                    Entities.QS_ResultDetail QS_ResultDetailModel = new Entities.QS_ResultDetail();
                    QS_ResultDetailModel.ScoreType = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].ScoreType);
                    QS_ResultDetailModel.QS_RTID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_RTID);
                    QS_ResultDetailModel.QS_RID = Convert.ToInt32(QS_RID);

                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_CID))
                    {
                        QS_ResultDetailModel.QS_CID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_CID);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_IID))
                    {
                        QS_ResultDetailModel.QS_IID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_IID);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_SID))
                    {
                        QS_ResultDetailModel.QS_SID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_SID);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_MID))
                    {
                        QS_ResultDetailModel.QS_MID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_MID);
                        QS_ResultDetailModel.QS_MID_End = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_MID);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].Type))
                    {
                        QS_ResultDetailModel.Type = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].Type);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].ScoreDeadID))
                    {
                        QS_ResultDetailModel.ScoreDeadID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].ScoreDeadID);
                        QS_ResultDetailModel.ScoreDeadID_End = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].ScoreDeadID);
                        HaveDead = true;
                    }
                    QS_ResultDetailModel.Remark = sInfoData.QS_ResultDetailList[i].Remark;
                    QS_ResultDetailModel.Status = 0;
                    QS_ResultDetailModel.CreateTime = System.DateTime.Now;
                    QS_ResultDetailModel.CreateUserID = userID;
                    BLL.QS_ResultDetail.Instance.Insert(QS_ResultDetailModel);
                }
            }
        }
        /// 计算成绩
        /// <summary>
        /// 计算成绩
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="modelsub"></param>
        /// <param name="HaveDead"></param>
        public void CalcResult(QS_ResultData sInfoData, bool HaveDead,
            int QS_RID, int QS_RTID, int? ScoreType,
            out decimal? Score, out int? IsQualified)
        {
            IsQualified = null;
            Score = null;

            //计算成绩
            if (sInfoData.QS_ResultDetailList != null && sInfoData.QS_ResultDetailList.Length > 0)
            {
                if (HaveDead)
                {
                    //有致命项得分为0
                    Score = 0;
                }
                //没有致命项
                else
                {
                    Score = 0;
                    //评分型/五级质检
                    if (ScoreType == 1 || ScoreType == 3)
                    {
                        string ccOrIm = "";
                        Int64 callid, csid;
                        if (Int64.TryParse(sInfoData.CallID, out callid))
                        {
                            if (callid > 0)
                            {
                                ccOrIm = "cc";
                            }
                        }
                        if (ccOrIm == "" && Int64.TryParse(sInfoData.CSID, out csid))
                        {
                            if (csid > 0)
                            {
                                ccOrIm = "im";
                            }
                        }
                        DataTable dtCalculate = BLL.QS_ResultDetail.Instance.GetQS_ResultForCalculate(ScoreType.ToString(), QS_RID, QS_RTID, ccOrIm);
                        if (dtCalculate != null && dtCalculate.Rows.Count > 0)
                        {
                            for (int n = 0; n < dtCalculate.Rows.Count; n++)
                            {
                                decimal outscore = 0;
                                if (decimal.TryParse(dtCalculate.Rows[n]["enscore"].ToString(), out outscore))
                                {
                                    Score += outscore;
                                }
                            }
                        }
                    }
                    //合格型
                    else
                    {
                        string ccOrIm = "";
                        Int64 callid, csid;
                        if (Int64.TryParse(sInfoData.CallID, out callid))
                        {
                            if (callid > 0)
                            {
                                ccOrIm = "cc";
                            }
                        }
                        if (ccOrIm == "" && Int64.TryParse(sInfoData.CSID, out csid))
                        {
                            if (csid > 0)
                            {
                                ccOrIm = "im";
                            }
                        }
                        DataTable dtCalculate = BLL.QS_ResultDetail.Instance.GetQS_ResultForCalculate("2", QS_RID, QS_RTID, ccOrIm);
                        //判断致命项，非致命项判断是否合格
                        if (dtCalculate != null && dtCalculate.Rows.Count > 0)
                        {
                            int outDeadcount = 0;
                            int outNoDeadcount = 0;
                            for (int i = 0; i < dtCalculate.Rows.Count; i++)
                            {
                                int _outDeadcount = 0;
                                if (int.TryParse(dtCalculate.Rows[i]["deadcount"].ToString(), out _outDeadcount))
                                {
                                    outDeadcount += _outDeadcount;
                                }
                                int _outNoDeadcount = 0;
                                if (int.TryParse(dtCalculate.Rows[i]["nodeadcount"].ToString(), out _outNoDeadcount))
                                {
                                    outNoDeadcount += _outNoDeadcount;
                                }
                            }
                            if (sInfoData.DeadNum == "0")
                            {
                                if (outDeadcount > 0)
                                {
                                    IsQualified = -1;
                                }
                                else
                                {
                                    IsQualified = 1;
                                    if (outNoDeadcount >= Convert.ToInt32(sInfoData.NoDeadNum))
                                    {
                                        IsQualified = -1;
                                    }
                                }
                            }
                            else if (sInfoData.NoDeadNum == "0")
                            {
                                if (outNoDeadcount > 0)
                                {
                                    IsQualified = -1;
                                }
                                else
                                {
                                    IsQualified = 1;
                                    if (outDeadcount >= Convert.ToInt32(sInfoData.DeadNum))
                                    {
                                        IsQualified = -1;
                                    }
                                }
                            }
                            else
                            {

                                if (outDeadcount >= Convert.ToInt32(sInfoData.DeadNum) || outNoDeadcount >= Convert.ToInt32(sInfoData.NoDeadNum))
                                {
                                    IsQualified = -1;
                                }
                                else
                                {
                                    IsQualified = 1;
                                }
                            }
                        }
                        else
                        {
                            IsQualified = 1;
                        }
                    }
                }
            }
            else
            {
                if (ScoreType == 1 || ScoreType == 3)
                {
                    Score = BLL.QS_Category.Instance.GetSumScore(QS_RTID);
                }
                else
                {
                    IsQualified = 1;
                }
            }
        }
        /// 历史表维护
        /// <summary>
        /// 历史表维护
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="QS_RTID"></param>
        /// <param name="QS_RID"></param>
        /// <param name="qsapprovaltype"></param>
        private void InsertApprovalHistory(int userID, int QS_RTID, int QS_RID, QSApprovalType qsapprovaltype)
        {
            Entities.QS_ApprovalHistory historyModel = new Entities.QS_ApprovalHistory();
            historyModel.ApprovalType = Convert.ToInt32(qsapprovaltype).ToString();
            historyModel.QS_RTID = QS_RTID;
            historyModel.QS_RID = QS_RID;
            historyModel.Type = "2";
            historyModel.CreateTime = System.DateTime.Now;
            historyModel.CreateUserID = userID;
            BLL.QS_ApprovalHistory.Instance.Insert(historyModel);
        }
        /// 审核时保存明细数据
        /// <summary>
        /// 审核时保存明细数据
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="sInfoData"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool AuditResultDetail(int userID, QS_ResultData sInfoData, int QS_RID)
        {
            bool HaveDead = false;
            if (sInfoData.QS_ResultDetailList != null && sInfoData.QS_ResultDetailList.Length > 0)
            {
                //插入新明细
                for (int i = 0; i < sInfoData.QS_ResultDetailList.Length; i++)
                {
                    Entities.QS_ResultDetail QS_ResultDetailModel = new Entities.QS_ResultDetail();
                    QS_ResultDetailModel.ScoreType = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].ScoreType);
                    QS_ResultDetailModel.QS_RTID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_RTID);
                    QS_ResultDetailModel.QS_RID = Convert.ToInt32(QS_RID);

                    if (sInfoData.QS_ResultDetailList[i].QS_MID == "-2" && sInfoData.QS_ResultDetailList[i].QS_MID_End == "-2" && sInfoData.QS_ResultDetailList[i].ScoreDeadID == "" && sInfoData.QS_ResultDetailList[i].ScoreDeadID_End == "")
                    {
                        continue;
                    }
                    if (sInfoData.QS_ResultDetailList[i].ScoreDeadID == "-2" && sInfoData.QS_ResultDetailList[i].ScoreDeadID_End == "-2" && sInfoData.QS_ResultDetailList[i].QS_MID == "" && sInfoData.QS_ResultDetailList[i].QS_MID_End == "")
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_CID))
                    {
                        QS_ResultDetailModel.QS_CID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_CID);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_IID))
                    {
                        QS_ResultDetailModel.QS_IID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_IID);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_SID))
                    {
                        QS_ResultDetailModel.QS_SID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_SID);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_MID))
                    {
                        QS_ResultDetailModel.QS_MID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_MID);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_MID_End))
                    {
                        QS_ResultDetailModel.QS_MID_End = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_MID_End);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].Type))
                    {
                        QS_ResultDetailModel.Type = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].Type);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].ScoreDeadID))
                    {
                        QS_ResultDetailModel.ScoreDeadID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].ScoreDeadID);
                    }
                    if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].ScoreDeadID_End))
                    {
                        QS_ResultDetailModel.ScoreDeadID_End = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].ScoreDeadID_End);
                        if (sInfoData.QS_ResultDetailList[i].ScoreDeadID_End != "-2")
                        {
                            HaveDead = true;
                        }
                    }
                    if (sInfoData.QS_ResultDetailList[i].ScoreType == "3" && !string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].Remark))
                    {
                        QS_ResultDetailModel.Remark = sInfoData.QS_ResultDetailList[i].Remark;
                    }
                    //更新
                    if (sInfoData.QS_ResultDetailList[i].QS_RDID != "" && sInfoData.QS_ResultDetailList[i].QS_RDID != "0")
                    {
                        Entities.QS_ResultDetail modelstate = BLL.QS_ResultDetail.Instance.GetQS_ResultDetail(Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_RDID));
                        if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_MID_End))
                        {
                            modelstate.QS_MID_End = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_MID_End);
                        }
                        if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].ScoreDeadID_End))
                        {
                            modelstate.ScoreDeadID_End = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].ScoreDeadID_End);
                        }
                        if (sInfoData.QS_ResultDetailList[i].ScoreType == "3")
                        {
                            if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].Remark))
                            {
                                modelstate.Remark = sInfoData.QS_ResultDetailList[i].Remark;
                            }
                            if (!string.IsNullOrEmpty(sInfoData.QS_ResultDetailList[i].QS_SID))
                            {
                                modelstate.QS_SID = Convert.ToInt32(sInfoData.QS_ResultDetailList[i].QS_SID);
                            }
                        }
                        modelstate.ModifyTime = System.DateTime.Now;
                        modelstate.ModifyUserID = userID;
                        BLL.QS_ResultDetail.Instance.Update(modelstate);
                    }
                    //插入
                    else
                    {
                        QS_ResultDetailModel.Status = 0;
                        QS_ResultDetailModel.CreateTime = System.DateTime.Now;
                        QS_ResultDetailModel.CreateUserID = userID;
                        BLL.QS_ResultDetail.Instance.Insert(QS_ResultDetailModel);
                    }
                }
            }
            return HaveDead;
        }
        #endregion

        #region 功能
        /// 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="userID"></param>
        /// <param name="retQS_RID"></param>
        private void Save(out string msg, int userID, out int retQS_RID)
        {
            QS_ResultData sInfoData;
            retQS_RID = 0;
            if (GetData(out msg, out sInfoData))
            {
                return;
            }

            try
            {
                //成绩表维护
                //返回评分表id
                int QS_RTID = 0;
                //返回结果id
                int QS_RID = SaveResult(sInfoData, userID, out QS_RTID);
                retQS_RID = QS_RID;
                if (QS_RID == -9999)
                {
                    msg = "页面过期，不能评分，请关闭重新打开！";
                    return;
                }

                //明细表维护
                //是否有致命项
                bool HaveDead = false;
                //保存明细
                InsertUpdateResultDetail(userID, sInfoData, QS_RID, out HaveDead);

                QSApprovalType qsapprovaltype = QSApprovalType.ScoreSave;
                //插入操作记录
                InsertApprovalHistory(userID, QS_RTID, QS_RID, qsapprovaltype);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        /// 提交
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="userID"></param>
        /// <param name="retQS_RID"></param>
        private void SubInfo(out string msg, int userID, out int retQS_RID)
        {
            QS_ResultData sInfoData;
            retQS_RID = 0;
            if (GetData(out msg, out sInfoData))
            {
                return;
            }

            try
            {
                //成绩表维护
                //返回评分表id
                int QS_RTID = 0;
                //返回结果id
                int QS_RID = SubmitResult(sInfoData, userID, out QS_RTID);
                if (QS_RID == -9999)
                {
                    msg += "此话务已评分过分了，不能再次评分！";
                    return;
                }
                retQS_RID = QS_RID;


                //是否有致命项
                bool HaveDead = false;
                //保存明细
                InsertUpdateResultDetail(userID, sInfoData, QS_RID, out HaveDead);
                //保存成绩
                SaveScoreResult(sInfoData, HaveDead, QS_RID);

                QSApprovalType qsapprovaltype = QSApprovalType.ScoreSubmit;
                //插入操作记录
                InsertApprovalHistory(userID, QS_RTID, QS_RID, qsapprovaltype);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        /// 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="userID"></param>
        /// <param name="retQS_RID"></param>
        private void SubState(out string msg, int userID, out int retQS_RID)
        {
            QS_ResultData sInfoData;
            retQS_RID = 0;
            if (GetData(out msg, out sInfoData))
            {
                return;
            }

            try
            {
                int QS_RID = 0;
                //说明是初次提交
                if (sInfoData.QS_RID == "0")
                {
                    msg = "没有找到该评分结果";
                    return;
                }
                //再次保存
                else
                {
                    QSResultStatus status = QSResultStatus.Claimed;
                    QS_RID = AuditSave(userID, sInfoData, status);
                }
                //返回成绩表主键
                retQS_RID = QS_RID;
                //是否有致命项
                bool HaveDead = AuditResultDetail(userID, sInfoData, QS_RID);
                //保存成绩
                SaveScoreResult(sInfoData, HaveDead, QS_RID);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

        /// 评分表复审通过
        /// <summary>
        /// 评分表复审通过
        /// </summary>
        private void ScoreTableRereview(out string msg, int userId)
        {
            msg = "";
            if (!string.IsNullOrEmpty(QS_RTID))
            {
                int valint = 0;
                if (int.TryParse(QS_RTID, out valint))
                {
                    Entities.QS_RulesTable model = BLL.QS_RulesTable.Instance.GetQS_RulesTable(valint);
                    //评分表复审通过是已完成。
                    model.Status = (Int32)Entities.QSRulesTableStatus.Finished;
                    BLL.QS_RulesTable.Instance.Update(model);

                    Entities.QS_ApprovalHistory historyModel = new Entities.QS_ApprovalHistory();
                    historyModel.ApprovalType = Convert.ToInt32(Entities.QSApprovalType.TableAduit).ToString();
                    historyModel.ApprovalResult = 1;
                    historyModel.QS_RTID = valint;
                    historyModel.Type = "1";
                    historyModel.CreateTime = System.DateTime.Now;
                    historyModel.CreateUserID = userId;

                    BLL.QS_ApprovalHistory.Instance.Insert(historyModel);
                    //add by qizq 添加日志2013-5-8
                    StringBuilder sbStr = new StringBuilder();
                    sbStr.Append("评分表 " + model.Name + " 审核通过");
                    BLL.Util.InsertUserLog(sbStr.ToString());
                }
            }
            else
            {
                msg = "评分表参数错误";
            }
        }
        /// 评分表复审驳回
        /// <summary>
        /// 评分表复审驳回
        /// </summary>
        private void ScoreTableReject(out string msg, int userId)
        {
            msg = "";
            if (!string.IsNullOrEmpty(QS_RTID))
            {
                int valint = 0;
                if (int.TryParse(QS_RTID, out valint))
                {
                    Entities.QS_RulesTable model = BLL.QS_RulesTable.Instance.GetQS_RulesTable(valint);
                    model.Status = (Int32)Entities.QSRulesTableStatus.Unfinished;
                    BLL.QS_RulesTable.Instance.Update(model);
                    Entities.QS_ApprovalHistory historyModel = new Entities.QS_ApprovalHistory();
                    historyModel.ApprovalType = Convert.ToInt32(Entities.QSApprovalType.TableAduit).ToString();
                    historyModel.ApprovalResult = 2;
                    historyModel.QS_RTID = valint;
                    historyModel.Type = "1";
                    historyModel.CreateTime = System.DateTime.Now;
                    historyModel.CreateUserID = userId;
                    BLL.QS_ApprovalHistory.Instance.Insert(historyModel);

                    //add by qizq 添加日志2013-5-8
                    StringBuilder sbStr = new StringBuilder();
                    sbStr.Append("评分表 " + model.Name + " 审核驳回");
                    BLL.Util.InsertUserLog(sbStr.ToString());
                }
            }
            else
            {
                msg = "评分表参数错误";
            }
        }
        #endregion

        #region 抽象
        /// 自定义校验
        /// <summary>
        /// 自定义校验
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="msg"></param>
        public abstract void CheckDataForDataID(QS_ResultData sInfoData, ref string msg);
        /// 保存逻辑
        /// <summary>
        /// 保存逻辑
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="userID"></param>
        /// <param name="QS_RTID"></param>
        /// <returns></returns>
        public abstract int SaveResult(QS_ResultData sInfoData, int userID, out int QS_RTID);
        /// 提交逻辑
        /// <summary>
        /// 提交逻辑
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="userID"></param>
        /// <param name="QS_RTID"></param>
        /// <returns></returns>
        public abstract int SubmitResult(QS_ResultData sInfoData, int userID, out int QS_RTID);
        /// 保存成绩
        /// <summary>
        /// 保存成绩
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="HaveDead"></param>
        /// <param name="QS_RID"></param>
        public abstract void SaveScoreResult(QS_ResultData sInfoData, bool HaveDead, int QS_RID);
        /// 审核保存
        /// <summary>
        /// 审核保存
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="sInfoData"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public abstract int AuditSave(int userID, QS_ResultData sInfoData, QSResultStatus status);
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}