using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.DTO;
using XYAuto.BUOC.IP2017.Entities.Examine;
using XYAuto.BUOC.IP2017.Entities.Result;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace XYAuto.BUOC.IP2017.BLL.ExamineLabel
{
    public class ExamineLableOperate
    {
        #region 当前登录人UserID
        private int _currentUserID;
        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    _currentUserID = 1225;
                }
                return _currentUserID;
            }
        }
        private static string LoginPwdKey = Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        #endregion        

        public static readonly ExamineLableOperate Instance = new ExamineLableOperate();

        /// <summary>
        /// zlb 2017-10-26
        /// 审核标签或修改标签
        /// </summary>
        /// <param name="auditLabel"></param>
        /// <returns></returns>
        public string ExamineOrUpdateLabel(AuditLabel auditLabel)
        {
            if (auditLabel == null)
            {
                return "参数错误";
            }
            if (!Enum.IsDefined(typeof(EnumTaskType), auditLabel.TaskType))
            {
                return "参数类型错误";
            };
            int resultID = Dal.Result.ResultLabel.Instance.GetResultID(auditLabel.BatchID, auditLabel.TaskType);
            #region 非空判断和去重
            if (auditLabel.TaskType == (int)EnumTaskType.媒体)
            {
                if ((auditLabel.OperateType == 1 && resultID <= 0) || auditLabel.OperateType != 1)
                {
                    //if (auditLabel.Category == null || auditLabel.Category.Count <= 0)
                    //{
                    //    return "分类不能为空";
                    //}
                    //auditLabel.Category = auditLabel.Category.Where((x, i) => auditLabel.Category.FindIndex(z => z.DictName.ToUpper() == x.DictName.ToUpper()) == i).ToList();
                    if (auditLabel.MarketScene == null || auditLabel.MarketScene.Count <= 0)
                    {
                        return "市场场景不能为空";
                    }
                    auditLabel.MarketScene = auditLabel.MarketScene.Where((x, i) => auditLabel.MarketScene.FindIndex(z => z.DictName.ToUpper() == x.DictName.ToUpper()) == i).ToList();
                    if (auditLabel.DistributeScene == null || auditLabel.DistributeScene.Count <= 0)
                    {
                        return "分发场景不能为空";
                    }
                    auditLabel.DistributeScene = auditLabel.DistributeScene.Where((x, i) => auditLabel.DistributeScene.FindIndex(z => z.DictName.ToUpper() == x.DictName.ToUpper()) == i).ToList();
                }
            }
            if (auditLabel.IPLabel == null || auditLabel.IPLabel.Count <= 0)
            {
                return "IP标签不能为空";
            }
            auditLabel.IPLabel = auditLabel.IPLabel.Where((x, i) => auditLabel.IPLabel.FindIndex(z => z.DictName.ToUpper() == x.DictName.ToUpper()) == i).ToList();
            foreach (var item in auditLabel.IPLabel)
            {
                if (item.SubIP == null || item.SubIP.Count <= 0)
                {
                    return "子IP标签不能为空";
                }
                item.SubIP = item.SubIP.Where((x, i) => item.SubIP.FindIndex(z => z.DictName.ToUpper() == x.DictName.ToUpper()) == i).ToList();
                foreach (var itemSonIp in item.SubIP)
                {
                    if (itemSonIp.Label == null || itemSonIp.Label.Count <= 0)
                    {
                        return "标签不能为空";
                    }
                    itemSonIp.Label = itemSonIp.Label.Where((x, i) => itemSonIp.Label.FindIndex(z => z.DictName.ToUpper() == x.DictName.ToUpper()) == i).ToList();
                }
            }
            #endregion
            string strResult = "";
            if (auditLabel.OperateType == 1)
            {
                strResult = ExamineLabel(auditLabel, resultID);
            }
            else
            {
                strResult = UpdateLabel(auditLabel);
            }
            if (string.IsNullOrEmpty(strResult))
            {
                BasicLabel.Instance.InsertTitleBasicInfo(auditLabel.IPLabel, currentUserID);
            }
            return strResult;
        }
        /// <summary>
        /// zlb 2017-10-26
        /// 修改最终结果标签
        /// </summary>
        /// <param name="auditLabel"></param>
        /// <returns></returns>
        public string UpdateLabel(AuditLabel auditLabel)
        {
            if (Dal.Result.ResultLabel.Instance.UpdateResultLastInfo(auditLabel.BatchID, currentUserID) <= 0)
            {
                return "修改失败，请重试";
            };
            Dal.Result.ResultLabel.Instance.InsertResultLabel(auditLabel, auditLabel.BatchID, currentUserID, true);
            ResultLabelJson result = new ResultLabelJson();
            result.TaskType = auditLabel.TaskType;
            result.MediaResultID = auditLabel.BatchID;
            //result.Category = auditLabel.Category;
            result.DistributeScene = auditLabel.DistributeScene;
            result.MarketScene = auditLabel.MarketScene;
            result.IPLabel = auditLabel.IPLabel;
            InsertReslutOperateInfo(result);
            return "";
        }
        /// <summary>
        /// zlb 2017-10-26
        /// 审核标签
        /// </summary>
        /// <param name="auditLabel"></param>
        /// <returns></returns>
        public string ExamineLabel(AuditLabel auditLabel, int resultID)
        {
            DataTable dt = Dal.ExamineLabel.ExamineLableOperate.Instance.GetAuditStatus(auditLabel.BatchID);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["Status"]) != (int)EnumBatchMediaStatus.审核中)
                {
                    return "当前任务正被别人审核,请选择其他媒体或品牌车型";
                }
            }
            else
            {
                return "审核批次不存在";
            }
            if (Dal.ExamineLabel.ExamineLableOperate.Instance.UpdateAuditStatus(auditLabel.BatchID, (int)EnumBatchMediaStatus.已审, currentUserID) <= 0)
            {
                return "审核失败，请重试";
            };
            Dal.ExamineLabel.ExamineLableOperate.Instance.InsertAuditLabel(auditLabel, currentUserID);
            if (resultID <= 0)
            {
                resultID = Dal.Result.ResultLabel.Instance.InsertMediaLabelResult(auditLabel.BatchID, currentUserID);
            }
            if (resultID <= 0)
            {
                Loger.Log4Net.Error($"审核批次ID：{auditLabel.BatchID}->插入最终结果媒体失败");
            }
            else
            {
                DataSet ds = Dal.ExamineLabel.BasicLabel.Instance.QueryResultLabelList(resultID);

                List<IpLabelInfo> ipLabelList = Util.DataTableToList<IpLabelInfo>(ds.Tables[1]);
                if (!(auditLabel.MarketScene == null || auditLabel.DistributeScene == null))
                {
                    List<BasicLabelInfo> labelList = Util.DataTableToList<BasicLabelInfo>(ds.Tables[0]);
                    if (labelList != null)
                    {
                        foreach (var item in labelList)
                        {
                            //BasicLabelInfo infoCategory = auditLabel.Category.Where(t => item.Type == (int)EnumLabelType.分类 && t.DictName.ToUpper() == item.DictName.ToUpper()).FirstOrDefault();
                            //if (infoCategory != null)
                            //{
                            //    auditLabel.Category.Remove(infoCategory);
                            //}
                            BasicLabelInfo infoMarket = auditLabel.MarketScene.Where(t => item.Type == (int)EnumLabelType.市场场景 && t.DictName.ToUpper() == item.DictName.ToUpper()).FirstOrDefault();
                            if (infoMarket != null)
                            {
                                auditLabel.MarketScene.Remove(infoMarket);
                            }
                            BasicLabelInfo infoDistribute = auditLabel.DistributeScene.Where(t => item.Type == (int)EnumLabelType.分发场景 && t.DictName.ToUpper() == item.DictName.ToUpper()).FirstOrDefault();
                            if (infoDistribute != null)
                            {
                                auditLabel.DistributeScene.Remove(infoDistribute);
                            }
                        }
                    }
                }
                if (ipLabelList != null)
                {
                    var varIpList = ipLabelList.GroupBy(t => new { t.PIPID, t.IpName });
                    foreach (var item in varIpList)
                    {
                        IpLabel infoIPLabel = auditLabel.IPLabel.Where(t => t.DictName.ToUpper() == item.Key.IpName.ToUpper()).FirstOrDefault();
                        if (infoIPLabel != null)
                        {
                            var varSonIpList = item.GroupBy(t => new { t.SubIPID, t.SonIpName });
                            foreach (var itemSonIp in varSonIpList)
                            {
                                SonIpLabel InfoSonIp = infoIPLabel.SubIP.Where(t => t.DictName.ToUpper() == itemSonIp.Key.SonIpName.ToUpper()).FirstOrDefault();
                                if (InfoSonIp != null)
                                {
                                    foreach (var itemLabel in itemSonIp)
                                    {
                                        BasicLabelInfo labelInfo = InfoSonIp.Label.Where(t => t.DictName.ToUpper() == itemLabel.LabelName.ToUpper()).FirstOrDefault();
                                        if (labelInfo != null)
                                        {
                                            InfoSonIp.Label.Remove(labelInfo);
                                        }
                                    }
                                    if (InfoSonIp.Label.Count <= 0)
                                    {
                                        infoIPLabel.SubIP.Remove(InfoSonIp);
                                    }
                                    else
                                    {
                                        InfoSonIp.SubIPID = itemSonIp.Key.SubIPID;
                                    }
                                }
                            }
                            if (infoIPLabel.SubIP.Count <= 0)
                            {
                                auditLabel.IPLabel.Remove(infoIPLabel);
                            }
                            else
                            {
                                infoIPLabel.PIPID = item.Key.PIPID;
                            }
                        }
                    }
                }
                Dal.Result.ResultLabel.Instance.InsertResultLabel(auditLabel, resultID, currentUserID, false);
                ResultLabelJson result = new ResultLabelJson();
                result.TaskType = auditLabel.TaskType;
                result.MediaResultID = resultID;
                //result.Category = auditLabel.Category;
                result.DistributeScene = auditLabel.DistributeScene;
                result.MarketScene = auditLabel.MarketScene;
                result.IPLabel = auditLabel.IPLabel;
                InsertReslutOperateInfo(result);
            }
            return "";
        }
        /// <summary>
        /// zlb 2017-10-26
        /// 插入最终结果操作日志
        /// </summary>
        /// <param name="result"></param>
        public void InsertReslutOperateInfo(ResultLabelJson result)
        {

            string jsonData = JsonConvert.SerializeObject(result);
            if (Dal.Result.ResultLabel.Instance.InsertReslutOperateInfo(result.MediaResultID, result.TaskType, jsonData, currentUserID) <= 0)
            {
                Loger.Log4Net.Error($"插入结果日志失败：" + jsonData);
            }
        }
        /// <summary>
        /// zlb 2017-10-26
        ///修改审核状态为审核中或待审核
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <returns></returns>
        public string UpdateMediaStatus(ReqsMediaStatusDto ReqDTO, out int ArticleCount)
        {
            ArticleCount = 0;
            if (ReqDTO == null || (ReqDTO.ExamineStatus != (int)EnumBatchMediaStatus.审核中 && ReqDTO.ExamineStatus != (int)EnumBatchMediaStatus.待审))
            {
                return "参数错误";
            }
            ArticleCount = Dal.ExamineLabel.ExamineLableOperate.Instance.SelectArticleCount(ReqDTO.BatchAuditID);
            DataTable dt = Dal.ExamineLabel.ExamineLableOperate.Instance.GetAuditStatus(ReqDTO.BatchAuditID);
            if (ReqDTO.ExamineStatus == (int)EnumBatchMediaStatus.审核中)
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["Status"]) == (int)EnumBatchMediaStatus.待审)
                    {
                        Dal.ExamineLabel.ExamineLableOperate.Instance.UpdateAuditStatus(ReqDTO.BatchAuditID, (int)EnumBatchMediaStatus.审核中, currentUserID);
                    }
                    else if (Convert.ToInt32(dt.Rows[0]["Status"]) == (int)EnumBatchMediaStatus.审核中)
                    {
                        if (dt.Rows[0]["AuditUserID"].ToString() != currentUserID.ToString())
                        {
                            return "当前任务正被别人审核,请选择其他媒体或品牌车型";
                        }
                    }
                    else
                    {
                        return "该批次无法审核";
                    }
                }
                else
                {
                    return "审核批次不存在";
                }
                return "";
            }
            else
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["Status"]) == (int)EnumBatchMediaStatus.审核中)
                    {
                        Dal.ExamineLabel.ExamineLableOperate.Instance.UpdateAuditStatus(ReqDTO.BatchAuditID, (int)EnumBatchMediaStatus.待审, currentUserID);
                    }
                }
                return "";
            }

        }
    }
}
