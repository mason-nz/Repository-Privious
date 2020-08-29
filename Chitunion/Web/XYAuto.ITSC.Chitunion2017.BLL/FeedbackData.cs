using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using System.Data;
using System.IO;
using System.Configuration;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Dal.SysRight;
using XYAuto.ITSC.Chitunion2017.BLL.UploadFileInfo;
using XYAuto.ITSC.Chitunion2017.Entities;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class FeedbackData
    {
        public static readonly BLL.FeedbackData Instance = new BLL.FeedbackData();
        /// <summary>
        /// 2017-02-26张立彬
        /// 添加或修改反馈数据
        /// </summary>
        /// <param name="feedbackData"></param>
        /// <returns></returns>
        public string InserFeedbackData(Entities.FeedbackData feedbackData)
        {
            if (feedbackData == null)
            {
                return "请添加数据";
            }
            else if (feedbackData.ADDetailID == 0)
            {
                return "请传入广告位ID";

            }
            else if (string.IsNullOrWhiteSpace(feedbackData.SubOrderID))
            {
                return "请传入订单编号";
            }
            switch ((MediaType)feedbackData.MediaType)
            {
                case MediaType.WeiXin:
                    if (feedbackData.@ReadCount <= 0)
                    {
                        return "阅读数不能为空或小于等于零";
                    }
                    break;
                case MediaType.APP:
                    if (feedbackData.PVCount <= 0 || feedbackData.UVCount <= 0)
                    {
                        return "PV数和UV数不能为空或小于等于零";
                    }
                    break;

                case MediaType.WeiBo:
                    if (feedbackData.ReadCount <= 0)
                    {
                        return "阅读数不能为空或小于等于零";
                    }
                    break;

                case MediaType.Video:
                    if (feedbackData.ReadCount <= 0)
                    {
                        return "观看人数不能为空或小于等于零";
                    }
                    break;
                case MediaType.Broadcast:
                    if (feedbackData.ReadCount <= 0)
                    {
                        return "总观看人数不能为空或小于等于零";
                    }
                    break;
            }
            if (((MediaType)feedbackData.MediaType) == MediaType.WeiXin)
            {

                if (string.IsNullOrWhiteSpace(feedbackData.FeedbackEndDate))
                {
                    return "截止时间不能为空";
                }
                if (feedbackData.FilePathList == null || feedbackData.FilePathList.Count <= 0)
                {
                    return "未选择文件";
                }
                feedbackData.FeedbackBeginDate = feedbackData.FeedbackEndDate;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(feedbackData.FeedbackBeginDate) || string.IsNullOrWhiteSpace(feedbackData.FeedbackEndDate))
                {
                    return "数据日期不能为空";
                }
                if (Convert.ToDateTime(feedbackData.FeedbackBeginDate) > Convert.ToDateTime(feedbackData.FeedbackEndDate))
                {
                    return "开始日期不能大于结束日期";
                }
                if (string.IsNullOrWhiteSpace(feedbackData.FilePath))
                {
                    return "未选择文件";

                }
            }
            if (Dal.FeedbackData.Instance.SelectGreaterThanSomeDay(feedbackData.SubOrderID, Convert.ToDateTime(feedbackData.FeedbackBeginDate)) <= 0)
            {
                return "数据日期必须大于执行时间";
            }
            if (UserDataPower.FeedbackDataVerification(feedbackData.SubOrderID, feedbackData.MediaType) != 1)
            {
                return "无上传此订单反馈数据的权限";
            }
            int IsSuccess = Dal.FeedbackData.Instance.InserFeedbackData(feedbackData);
            if (IsSuccess <= 0)
            {
                if (IsSuccess == -11)
                {
                    return "数据信息交叉,不能上传";
                }
                //else if (IsSuccess == -12)
                //{
                //    if (feedbackData.MediaType == 14002)
                //    {
                //        return "数据日期必须在执行周期内！";
                //    }
                //    else
                //    {
                //        return "数据日期必须大于等于执行时间";
                //    }
                //}
                else if (IsSuccess == -13)
                {
                    return "媒体类型错误";
                }
                else
                {
                    return "上传失败,请确保数据正确并重试";
                }
            }
            else
            {

                var urlList = new List<string>();
                if (feedbackData.MediaType == (int)MediaType.WeiXin)
                {
                    Dal.FeedbackData.Instance.InsertFeedBackFile(feedbackData.FilePathList, MediaType.WeiXin, IsSuccess);
                    urlList = feedbackData.FilePathList;
                }
                else
                {
                    urlList.Add(feedbackData.FilePath);

                }
                string mediaTable = "";
                switch (feedbackData.MediaType)
                {
                    case 14001:
                        mediaTable = "OrderFeedbackData_Weixin";
                        break;
                    case 14002:
                        mediaTable = "OrderFeedbackData_PC";
                        break;
                    case 14003:
                        mediaTable = "OrderFeedbackData_Weibo";
                        break;
                    case 14004:
                        mediaTable = "OrderFeedbackData_Video";
                        break;
                    case 14005:
                        mediaTable = "OrderFeedbackData_Live";
                        break;
                    default:
                        break;
                }
                if (mediaTable != "")
                {
                    int userId = Common.UserInfo.GetLoginUserID();
                    UploadFileInfo.UploadFileInfo.Instance.Excute(urlList, userId, UploadFileEnum.OrderFeedbackData, IsSuccess, mediaTable);
                }
                return "";
            }
        }
        /// <summary>
        /// 2017-02-27张立彬
        /// 根据子单编号和媒体类型查询反馈数据
        /// </summary>
        /// <param name="subOrderID">子单编号</param>
        /// <param name="mediaType">媒体类型</param>
        /// <returns></returns>
        public List<Entities.SelectFeedbackData> SelectFeedbackData(string subOrderID, int mediaType)
        {
            string strWhere = "";
            string roleIdList = Common.UserInfo.GetLoginUserRoleIDs();
            int userId = Common.UserInfo.GetLoginUserID();
            string strSelectMediaID = "";
            switch (mediaType)
            {
                case 14001:
                    strSelectMediaID = " (AD.MediaID in (select MediaID from Media_Weixin where CreateUserID=" + userId + ") and AD.MediaType=14001)";
                    break;
                case 14002:
                    strSelectMediaID = " (AD.MediaID in (select MediaID from Media_PCAPP where CreateUserID=" + userId + ") and  AD.MediaType=14002)";
                    break;
                case 14003:
                    strSelectMediaID = " (AD.MediaID in (select MediaID from Media_Weibo where CreateUserID=" + userId + ") and  AD.MediaType=14003)";
                    break;
                case 14004:
                    strSelectMediaID = " (AD.MediaID in (select MediaID from Media_Video where CreateUserID=" + userId + ") and  AD.MediaType=14004)";
                    break;
                case 14005:
                    strSelectMediaID = " (AD.MediaID in (select MediaID from Media_Broadcast where CreateUserID=" + userId + ") and  AD.MediaType=14005)";
                    break;
                default:
                    strSelectMediaID = " 1=2";
                    break;
            }
            if (roleIdList.Contains("SYS001RL00004") || roleIdList.Contains("SYS001RL00001") || roleIdList.Contains("SYS001RL00005"))
            {
                strWhere = "";
            }
            //else if ()
            //{
            //    string userIdList = UserDataPower.GetUseridListByUserID(userId, 0);
            //    strWhere = " and (AD.CreateUserID in " + userIdList + " or " + strSelectMediaID + ")";
            //}
            else if (roleIdList.Contains("SYS001RL00003"))
            {

                strWhere = " and (AD.CreateUserID=" + userId + " or " + strSelectMediaID + ")";
            }
            else if (roleIdList.Contains("SYS001RL00002")|| roleIdList.Contains("SYS001RL00008"))
            {
                strWhere = " and AD.CreateUserID=" + userId;
            }
            else
            {
                strWhere = " and 1=2";
            }
            List<Entities.SelectFeedbackData> listFeedbackData = new List<Entities.SelectFeedbackData>();

            DataTable dt = Dal.FeedbackData.Instance.SelectFeedbackData(subOrderID, mediaType, strWhere);
            if (dt != null && dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (dt.Rows[i]["FeedbackID"] != DBNull.Value)
                    {
                        sb.Append(Convert.ToInt32(dr["FeedbackID"]) + ",");
                    }
                }
                string FeedBackIdList = sb.ToString();
                DataTable dtFile = null;
                if (FeedBackIdList.Length > 0)
                {
                    FeedBackIdList = FeedBackIdList.Substring(0, FeedBackIdList.Length - 1);
                    dtFile = Dal.FeedbackData.Instance.SelectFeedBackFile(mediaType, FeedBackIdList);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string Describe = dt.Rows[i]["Describe"] == null ? "" : dt.Rows[i]["Describe"].ToString();
                    //TODO:代码走查-逻辑问题&命名规范：SelectFeed命名,Add=masj,Date=2017-05-10
                    Entities.SelectFeedbackData selectFeed = listFeedbackData.Where(t => t.Describe == Describe).FirstOrDefault();
                    Entities.FeedbackData FeedbackData = null;
                    if (dt.Rows[i]["FeedbackID"] != DBNull.Value)
                    {
                        FeedbackData = new Entities.FeedbackData();
                        FeedbackData.FeedbackID = Convert.ToInt32(dt.Rows[i]["FeedbackID"]);
                        FeedbackData.ADDetailID = dt.Rows[i]["ADDetailID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["ADDetailID"]);
                        FeedbackData.ReadCount = dt.Rows[i]["ReadCount"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["ReadCount"]);
                        FeedbackData.TransmitCount = dt.Rows[i]["TransmitCount"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["TransmitCount"]);
                        FeedbackData.ClickCount = dt.Rows[i]["ClickCount"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["ClickCount"]);
                        FeedbackData.CommentCount = dt.Rows[i]["CommentCount"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["CommentCount"]);
                        FeedbackData.LinkCount = dt.Rows[i]["LinkCount"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["LinkCount"]);
                        FeedbackData.PVCount = dt.Rows[i]["PVCount"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["PVCount"]);
                        FeedbackData.UVCount = dt.Rows[i]["UVCount"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["UVCount"]);
                        FeedbackData.OrderCount = dt.Rows[i]["OrderCount"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["OrderCount"]);
                        FeedbackData.Value = dt.Rows[i]["Value"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["Value"]);
                        FeedbackData.ClickRate = dt.Rows[i]["ClickRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[i]["ClickRate"]);
                        FeedbackData.FilePath = dt.Rows[i]["FilePath"] == DBNull.Value ? "" : dt.Rows[i]["FilePath"].ToString();
                        FeedbackData.DeliveredCount = dt.Rows[i]["DeliveredCount"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["DeliveredCount"]);
                        FeedbackData.FileName = Util.GetFileNameByUpload(FeedbackData.FilePath);
                        FeedbackData.FeedbackBeginDate = dt.Rows[i]["FeedbackBeginDate"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["FeedbackBeginDate"]).ToString("yyyy-MM-dd");
                        FeedbackData.FeedbackEndDate = dt.Rows[i]["FeedbackEndDate"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[i]["FeedbackEndDate"]).ToString("yyyy-MM-dd HH:mm");
                        if (dtFile != null && dtFile.Rows.Count > 0)
                        {
                            DataRow[] drArr = dtFile.Select("FeedBackID=" + FeedbackData.FeedbackID);
                            if (drArr != null)
                            {
                                FeedbackData.FileInfoList = new List<FeedBackFile>();
                                for (int j = 0; j < drArr.Count(); j++)
                                {
                                    FeedBackFile file = new FeedBackFile() { FilePath = drArr[j]["UploadFileURL"].ToString(), FileName = Util.GetFileNameByUpload(drArr[j]["UploadFileURL"].ToString()) };
                                    FeedbackData.FileInfoList.Add(file);

                                }
                            }
                        }

                    }

                    if (selectFeed != null)
                    {
                        if (FeedbackData != null)
                        {
                            selectFeed.DataList.Add(FeedbackData);
                        }
                    }
                    else
                    {
                        Entities.SelectFeedbackData selectFeedNew = new Entities.SelectFeedbackData();
                        selectFeedNew.ADDetailID = Convert.ToInt32(dt.Rows[i]["ADDetailID"]);
                        selectFeedNew.Describe = Describe;
                        if (FeedbackData != null)
                        {
                            selectFeedNew.DataList.Add(FeedbackData);
                        }
                        listFeedbackData.Add(selectFeedNew);
                    }
                }


            }
            return listFeedbackData;
        }
        /// <summary>
        /// 2017-03-10 张立彬
        /// 删除反馈数据
        /// </summary>
        /// <param name="FeedbackID">反馈ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <returns></returns>
        public string DeleteFeedbackData(int MediaType, int FeedbackID, string FileUrl)
        {
            string URL = ConfigurationManager.AppSettings["ExitAddress"];
            string ServerPath = ConfigurationManager.AppSettings["UploadFilePath"];
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("isSuccess", -1);
            //dic.Add("ErrorDescribe", "");
            if (MediaType != 14001 && string.IsNullOrEmpty(FileUrl))
            {
                return "请填写附件路径";
            }
            else
            {
                string MessageStr = "无删除此订单反馈数据的权限"; ;
                string SubOrderID = UserDataPower.GetSubOrderID(MediaType, FeedbackID);
                if (string.IsNullOrEmpty(SubOrderID))
                {
                    return MessageStr;
                }
                if (UserDataPower.FeedbackDataVerification(SubOrderID, MediaType) != 1)
                {
                    return MessageStr;
                }
                if (MediaType == 14001)
                {
                    DataTable dtFeedBackFile = Dal.FeedbackData.Instance.SelectFeedBackFile(MediaType, FeedbackID.ToString());
                    if (dtFeedBackFile != null && dtFeedBackFile.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtFeedBackFile.Rows.Count; i++)
                        {
                            DataRow dr = dtFeedBackFile.Rows[i];
                            string path = ServerPath + dr["UploadFileURL"].ToString().Replace(URL, "").Replace("/", "\\");
                            if (File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                    }
                    int result = Dal.FeedbackData.Instance.DeleteFeedBackFileByID(FeedbackID);
                }
                else
                {
                    string path = ServerPath + FileUrl.Replace(URL, "").Replace("/", "\\");
                    if (File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                if (Dal.FeedbackData.Instance.DeleteFeedbackData(MediaType, FeedbackID) > 0)
                {
                    string mediaTable = "";
                    switch (MediaType)
                    {
                        case 14001:
                            mediaTable = "OrderFeedbackData_Weixin";
                            break;
                        case 14002:
                            mediaTable = "OrderFeedbackData_PC";
                            break;
                        case 14003:
                            mediaTable = "OrderFeedbackData_Weibo";
                            break;
                        case 14004:
                            mediaTable = "OrderFeedbackData_Video";
                            break;
                        case 14005:
                            mediaTable = "OrderFeedbackData_Live";
                            break;
                        default:
                            break;
                    }
                    if (mediaTable != "")
                    {
                        int userId = Common.UserInfo.GetLoginUserID();

                        UploadFileInfo.UploadFileInfo.Instance.Delete(FeedbackID, mediaTable, (int)UploadFileEnum.OrderFeedbackData);
                    }
                    return "";
                }
                else
                {
                    return "删除失败,请重试";
                }


            }
        }
    }
}
