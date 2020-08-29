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
    /// ScoringManage_CC 的摘要说明
    /// </summary>
    public class ScoringManage_CC : BaseScoring
    {
        /// 自定义校验
        /// <summary>
        /// 自定义校验
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="msg"></param>
        public override void CheckDataForDataID(QS_ResultData sInfoData, ref string msg)
        {
            Int64 intVal64 = 0;
            if (!Int64.TryParse(sInfoData.CallID, out intVal64))
            {
                msg += "录音ID格式不正确<br>";
            }
        }
        /// 保存逻辑
        /// <summary>
        /// 保存逻辑
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="userID"></param>
        /// <param name="QS_RTID"></param>
        /// <returns></returns>
        public override int SaveResult(QS_ResultData sInfoData, int userID, out int QS_RTID)
        {
            //待评分
            return InsertOrUpdateResult(sInfoData, userID, out QS_RTID, QSResultStatus.WaitScore, tableEndName);
        }
        /// 提交逻辑
        /// <summary>
        /// 提交逻辑
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="userID"></param>
        /// <param name="QS_RTID"></param>
        /// <returns></returns>
        public override int SubmitResult(QS_ResultData sInfoData, int userID, out int QS_RTID)
        {
            //已提交
            return InsertOrUpdateResult(sInfoData, userID, out QS_RTID, QSResultStatus.Submitted, tableEndName);
        }
        /// 保存成绩
        /// <summary>
        /// 保存成绩
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="HaveDead"></param>
        /// <param name="QS_RID"></param>
        public override void SaveScoreResult(QS_ResultData sInfoData, bool HaveDead, int QS_RID)
        {
            //取成绩实体
            Entities.QS_Result model = BLL.QS_Result.Instance.GetQS_Result(QS_RID);
            //计算成绩
            decimal? Score;
            int? IsQualified;
            CalcResult(sInfoData, HaveDead, model.QS_RID, model.QS_RTID, model.ScoreType, out  Score, out IsQualified);

            model.Score = Score;
            model.IsQualified = IsQualified;
            model.ModifyTime = DateTime.Now;
            model.ModifyUserID = BLL.Util.GetLoginUserID();
            //保存
            BLL.QS_Result.Instance.Update(model);
        }
        /// 审核保存
        /// <summary>
        /// 审核保存
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="sInfoData"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override int AuditSave(int userID, QS_ResultData sInfoData, QSResultStatus status)
        {
            Entities.QS_Result model = null;
            model = BLL.QS_Result.Instance.GetQS_Result(Convert.ToInt32(sInfoData.QS_RID));
            model.ModifyTime = System.DateTime.Now;
            model.ModifyUserID = userID;
            model.ScoreType = Convert.ToInt32(sInfoData.ScoreType);
            model.Status = (Int32)status;
            model.StateResult = 1;
            BLL.QS_Result.Instance.Update(model);
            return model.QS_RID;
        }

        /// 维护成绩表
        /// <summary>
        /// 维护成绩表
        /// </summary>
        /// <param name="sInfoData"></param>
        /// <param name="userID"></param>
        /// <param name="QS_RTID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private int InsertOrUpdateResult(QS_ResultData sInfoData, int userID, out int QS_RTID, QSResultStatus status, string tableEndName)
        {
            Entities.QS_Result model = null;
            //由于易湃外呼业务签入cc系统，易湃不入库CallRecordInfo表，所以此处改成从CallRecord_ORIG_Business取录音创建坐席 modify by qizq 2014-4-17
            Entities.CallRecord_ORIG_Business callModel = BLL.CallRecord_ORIG_Business.Instance.GetByCallID(Convert.ToInt64(sInfoData.CallID), tableEndName);
            //说明是初次
            if (sInfoData.QS_RID == "0")
            {
                if (status == QSResultStatus.Submitted)
                {
                    if (BLL.QS_Result.Instance.HasScored(sInfoData.CallID))
                    {
                        // "此话务已评分过分了，不能再次评分！";
                        QS_RTID = 0;
                        return -9999;
                    }
                }
                model = new Entities.QS_Result();
                model.CallReCordID = Convert.ToInt64(sInfoData.CallID);
                model.CallID = Convert.ToInt64(sInfoData.CallID);
                model.QS_RTID = Convert.ToInt32(sInfoData.QS_RTID);
                model.SeatID = callModel.CreateUserID.ToString();
                model.ScoreType = Convert.ToInt32(sInfoData.ScoreType);
                model.CreateTime = System.DateTime.Now;
                model.CreateUserID = userID;
                model.QualityAppraisal = sInfoData.QualityAppraisal;
                model.Status = (Int32)status;
                model.QS_RID = BLL.QS_Result.Instance.Insert(model, tableEndName);
                if (model.QS_RID == -9999)
                {
                    // "此话务已评分过分了，不能再次评分！";
                    QS_RTID = 0;
                    return -9999;
                }
            }
            //再次保存
            else
            {
                model = BLL.QS_Result.Instance.GetQS_Result(Convert.ToInt32(sInfoData.QS_RID));
                model.QualityAppraisal = sInfoData.QualityAppraisal;
                model.ModifyTime = System.DateTime.Now;
                model.ModifyUserID = userID;
                model.ScoreType = Convert.ToInt32(sInfoData.ScoreType);
                model.Status = (Int32)status;
                BLL.QS_Result.Instance.Update(model);
            }
            //返回结果
            QS_RTID = model.QS_RTID;
            return model.QS_RID;
        }
    }
}