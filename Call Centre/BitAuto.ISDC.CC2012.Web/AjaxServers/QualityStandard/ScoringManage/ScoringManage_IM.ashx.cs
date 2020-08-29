using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityStandard.ScoringManage
{
    /// 对话质检相关
    /// <summary>
    /// 对话质检相关
    /// </summary>
    public class ScoringManage_IM : BaseScoring
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
            if (!Int64.TryParse(sInfoData.CSID, out intVal64))
            {
                msg += "对话ID格式不正确<br>";
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
            return InsertOrUpdateResult(sInfoData, userID, out QS_RTID, QSResultStatus.WaitScore);
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
            return InsertOrUpdateResult(sInfoData, userID, out QS_RTID, QSResultStatus.Submitted);
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
            QS_IM_ResultInfo model = CommonBll.Instance.GetComAdoInfo<QS_IM_ResultInfo>(CommonFunction.ObjectToInteger(QS_RID));
            //计算成绩
            decimal? Score;
            int? IsQualified;
            CalcResult(sInfoData, HaveDead, model.QS_RID.Value, model.QS_RTID.Value, model.ScoreType, out  Score, out IsQualified);

            model.Score = Score;
            model.IsQualified = IsQualified; 
            model.ModifyTime = DateTime.Now;
            model.ModifyUserID = BLL.Util.GetLoginUserID();
            //保存
            CommonBll.Instance.UpdateComAdoInfo<QS_IM_ResultInfo>(model);
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
            QS_IM_ResultInfo model = CommonBll.Instance.GetComAdoInfo<QS_IM_ResultInfo>(CommonFunction.ObjectToInteger(sInfoData.QS_RID));
            model.ModifyTime = System.DateTime.Now;
            model.ModifyUserID = userID;
            model.ScoreType = Convert.ToInt32(sInfoData.ScoreType);
            model.Status = (Int32)status;
            model.StateResult = 1;
            CommonBll.Instance.UpdateComAdoInfo<QS_IM_ResultInfo>(model);
            return model.QS_RID.Value;
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
        private int InsertOrUpdateResult(QS_ResultData sInfoData, int userID, out int QS_RTID, QSResultStatus status)
        {
            QS_IM_ResultInfo model = null;
            DataRow dr = BLL.QS_IM_Result.Instance.GetQS_IM_ResultForCSID(sInfoData.CSID);
            if (dr == null)
            {
                throw new Exception("会话不存在！");
            }
            //说明是初次
            if (sInfoData.QS_RID == "0")
            {
                if (status == QSResultStatus.Submitted)
                {
                    if (BLL.QS_IM_Result.Instance.HasScored(sInfoData.CSID))
                    {
                        // "此话务已评分过分了，不能再次评分！";
                        QS_RTID = 0;
                        return -9999;
                    }
                }
                model = new QS_IM_ResultInfo();
                //QS_RID根据QS_Result表中的自增列生成，两个表的主键不能重复
                model.QS_RID = BLL.QS_IM_Result.Instance.CreateQS_RID();
                model.CSID = Convert.ToInt64(sInfoData.CSID);
                model.QS_RTID = Convert.ToInt32(sInfoData.QS_RTID);
                model.SeatID = dr["AgentUserID"].ToString();
                model.ScoreType = Convert.ToInt32(sInfoData.ScoreType);
                model.CreateTime = System.DateTime.Now;
                model.CreateUserID = userID;
                model.QualityAppraisal = sInfoData.QualityAppraisal;
                model.Status = (Int32)status;
                CommonBll.Instance.InsertComAdoInfo<QS_IM_ResultInfo>(model);
            }
            //再次保存
            else
            {
                model = CommonBll.Instance.GetComAdoInfo<QS_IM_ResultInfo>(CommonFunction.ObjectToInteger(sInfoData.QS_RID));
                model.QualityAppraisal = sInfoData.QualityAppraisal;
                model.ModifyTime = System.DateTime.Now;
                model.ModifyUserID = userID;
                model.ScoreType = Convert.ToInt32(sInfoData.ScoreType);
                model.Status = (Int32)status;
                CommonBll.Instance.UpdateComAdoInfo<QS_IM_ResultInfo>(model);
            }
            //返回结果
            QS_RTID = model.QS_RTID.Value;
            return model.QS_RID.Value;
        }
    }
}