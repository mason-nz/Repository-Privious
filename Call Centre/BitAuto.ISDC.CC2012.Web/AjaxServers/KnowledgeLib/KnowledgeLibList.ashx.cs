using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Config;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    /// <summary>
    /// KnowledgeLibList 的摘要说明
    /// </summary>
    public class KnowledgeLibList : IHttpHandler, IRequiresSessionState
    {
        #region 属性
        public string RequestAction
        {
            get { return HttpContext.Current.Request["Action"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Action"].ToString()); }
        }
        public string RequestKCID
        {
            get { return HttpContext.Current.Request["KCID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["KCID"].ToString()); }
        }
        public string RequestKLID
        {
            get { return HttpContext.Current.Request["KLID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["KLID"].ToString()); }
        }
        public string RequestCategory
        {
            get { return HttpContext.Current.Request["Category"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Category"].ToString()); }
        }
        public string RequestRegionId
        {
            get { return HttpContext.Current.Request["regionid"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["regionid"].ToString()); }
        }
        public string RequestLevel
        {
            get { return HttpContext.Current.Request["Level"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Level"].ToString()); }
        }
        public string RequestRejectReason
        {
            get { return HttpContext.Current.Request["RejectReason"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["RejectReason"].ToString()); }
        }
        public string RequestRemark
        {
            get { return HttpContext.Current.Request["Remark"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["Remark"].ToString()); }
        }

        public string RequestSearchKey
        {
            get { return HttpContext.Current.Request["SearchKey"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SearchKey"].ToString()); }
        }
        public string RequestSearchType
        {
            get { return HttpContext.Current.Request["SearchType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SearchType"].ToString()); }
        }

        private int type
        {
            get { return HttpContext.Current.Request["type"] == null ? -1 : Convert.ToInt32(HttpUtility.UrlDecode(HttpContext.Current.Request["type"].ToString())); }
        }

        public string RequestKLQID
        {
            get { return HttpContext.Current.Request["KLQID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["KLQID"].ToString()); }
        }

        #endregion
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            string msg = string.Empty;
            int userID = BLL.Util.GetLoginUserID();
            switch (RequestAction.ToLower())
            {
                case "allmarkread": allMarkRead(out msg);
                    break;
                case "bindknowledgecategory": bindKnowledgeCategory(out msg);
                    break;
                case "bindknowledgecategoryexceptdel": bindKnowledgeCategoryExceptDel(out msg);
                    break;
                case "updateknowledgecategory": 
                    if (!BLL.Util.CheckRight(userID, "SYS024BUT3103"))
                    {
                        msg = "fail";
                    }
                    else
                    {
                        updateknowledgecategory(out msg);
                    } 
                    break;
                case "approvalknowledgelib": approvalKnowledgeLib(out msg);
                    break;
                case "rejectknowledgelib": rejectKnowledgeLib(out msg);
                    break;
                case "disableknowledgelib":
                    if (!BLL.Util.CheckRight(userID, "SYS024BUT3104"))
                     {
                         msg = "{'msg':'您没有权限进行此操作！'}";
                     }
                     else
                     {
                         disableKnowledgeLib(out msg);
                     }
                    break;
                case "deleteknowledgelib":
                    if (!BLL.Util.CheckRight(userID, "SYS024BUT3105"))
                    {
                        msg = "{'msg':'您没有权限进行此操作！'}";
                    }
                    else
                    {
                        deleteKnowledgeLib(out msg);
                    }
                    break;
                case "insertsearchlog": insertSearchLog();
                    break;
                case "questiondisableknowledgelib":
                    if (!BLL.Util.CheckRight(userID, "SYS024BUT3107"))
                    {
                        msg = "{'msg':'您没有权限进行此操作！'}";
                    }
                    else
                    {
                        questionDisableKnowledgeLib(out msg);   //停用试题
                    }
                    break;
                case "addklclickanddownloadcount":
                    AddKLClickAndDownloadCount(out msg);
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(msg))
            {
                context.Response.Write(msg);
            }

        }

        //如果有输入关键字，则插入一条数据到 搜索日志表
        public void insertSearchLog()
        {
            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
            try
            {
                if (RequestSearchKey != "" && RequestSearchType != "")
                {
                    Entities.SearchLog model = new SearchLog();
                    model.SearchKey = RequestSearchKey;
                    model.Type = int.Parse(RequestSearchType);
                    model.CreateTime = DateTime.Now;
                    model.CreateUserID = BLL.Util.GetLoginUserID();

                    BLL.SearchLog.Instance.Insert(tran, model);
                    BLL.Util.InsertUserLog(tran, "【知识库】搜索日志表【插入】搜索关键字：【" + model.SearchKey + "】类型：【知识库】的记录");
                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
            }
            finally
            {
                connection.Close();
            }
        }

        //全部标记为 已读
        public void allMarkRead(out string msg)
        {
            msg = string.Empty;

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
            try
            {
                QueryKnowledgeLib query = new QueryKnowledgeLib();
                query.Status = 2;
                query.Content = "";
                int count;
                DataTable dt = BLL.KnowledgeLib.Instance.GetKnowledgeLib(query, "", 1, 100000, out count);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    long klid = long.Parse(dt.Rows[i]["KLID"].ToString());
                    int userID = BLL.Util.GetLoginUserID();

                    //删除 该登陆者在所有知识点的标记记录
                    BLL.KLReadTag.Instance.DeleteByUserID(tran, userID, klid);

                    //新增 该登陆者在所有知识点的标记记录
                    Entities.KLReadTag model = new KLReadTag();
                    model.KLID = klid;
                    model.UserID = userID;
                    model.ReadTag = 1;
                    model.CreateUserID = userID;
                    model.CreateTime = DateTime.Now;

                    BLL.KLReadTag.Instance.Insert(tran, model);
                    BLL.Util.InsertUserLog(tran, "【全部标记为已读】知识库阅读标记表【插入】知识点ID：【" + model.KLID + "】阅读标记：【已读】的记录");
                }
                //事务提交
                tran.Commit();
                msg = "{msg:'操作成功，已全部标记为已读'}";
            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = "{msg:'" + ex.Message.ToString() + "'}";
            }
            finally
            {
                connection.Close();
            }
        }

        //审核通过
        public void approvalKnowledgeLib(out string msg)
        {
            msg = string.Empty;

            #region 审核通过 事务操作

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
            try
            {
                operateKnowledgeLib(1, tran, out msg);

                //插入一条记录到KLOptionLog知识库操作日志表
                string[] KLIDs = RequestKLID.Split(',');
                for (int i = 0; i < KLIDs.Length; i++)
                {
                    InsertKLOptionLog(tran, (int)Entities.EnumOptStatus.Release, KLIDs[i]);
                }
                //事务提交
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = "{'msg':'" + ex.Message.ToString() + "'}";
            }
            finally
            {
                connection.Close();
            }

            #endregion
        }

        /// <summary>
        /// 更新知识点下载或者单击次数
        /// </summary>
        /// <param name="type">0：点击次数，1：下载次数</param>
        /// <param name="klid"></param>
        public void AddKLClickAndDownloadCount(out string msg)
        {
            msg = string.Empty;
            BLL.KnowledgeLib.Instance.AddClickAndDownloadCounts(type, Convert.ToInt32(RequestKLID));
        }

        //驳回
        public void rejectKnowledgeLib(out string msg)
        {
            msg = string.Empty;

            #region 驳回 事务操作

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
            try
            {
                operateKnowledgeLib(2, tran, out msg);
                //插入一条记录到KLOptionLog知识库操作日志表
                string[] KLIDs = RequestKLID.Split(',');
                for (int i = 0; i < KLIDs.Length; i++)
                {
                    InsertKLOptionLog(tran, (int)Entities.EnumOptStatus.Reject, KLIDs[i]);
                }
                //事务提交
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = "{'msg':'" + ex.Message.ToString() + "'}";
            }
            finally
            {
                connection.Close();
            }

            #endregion
        }

        //停用
        public void disableKnowledgeLib(out string msg)
        {
            msg = string.Empty;

            #region 停用 事务操作

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
            try
            {
                operateKnowledgeLib(3, tran, out msg);
                //插入一条记录到KLOptionLog知识库操作日志表
                string[] KLIDs = RequestKLID.Split(',');
                for (int i = 0; i < KLIDs.Length; i++)
                {
                    InsertKLOptionLog(tran, (int)Entities.EnumOptStatus.Stop, KLIDs[i]);
                }
                //事务提交
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = "{'msg':'" + ex.Message.ToString() + "'}";
            }
            finally
            {
                connection.Close();
            }

            #endregion
        }

        //删除
        public void deleteKnowledgeLib(out string msg)
        {
            msg = string.Empty;
            string[] KLIDs = RequestKLID.Split(',');
            if (KLIDs.Length == 0)
            {
                msg += "{'msg':'没有对应的ID值，操作失败'}";
                return;
            }
            string klqids = string.Empty;   //记录试题库中的主键信息；已 1,2,3|4,5,6|7,8,9 形式隔开
            for (int j = 0; j < KLIDs.Length; j++)
            {
                //根据KLID找到试题库中的信息
                QueryKLQuestion query = new QueryKLQuestion();
                query.KLID = long.Parse(KLIDs[j]);
                int count;
                DataTable dt = BLL.KLQuestion.Instance.GetKLQuestion(query, "", 1, 10000, out count);
                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    klqids += dt.Rows[m]["KLQID"].ToString() + ",";
                }
                klqids = klqids.TrimEnd(',') + "|";
            }
            klqids = klqids.TrimEnd('|');

            #region 删除 事务操作

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");
            try
            {
                //1 将知识库信息状态修改为 4已删除
                operateKnowledgeLib(4, tran, out msg);
                for (int i = 0; i < KLIDs.Length; i++)
                {
                    //2 将KLQuestion知识库试题记录修改为 -1已删除
                    string[] klqid1 = klqids.Split('|');
                    for (int n = 0; n < klqid1.Length; n++)
                    {
                        string[] klqid2 = klqid1[n].Split(',');
                        for (int p = 0; p < klqid2.Length; p++)
                        {
                            deleteKLQuestionByKLID(tran, klqid2[p]);
                        }
                    }
                    //3 将KLUploadFile 知识库上传文件信息表记录 修改为 -1已删除
                    deleteKLUploadFileByKLID(tran, KLIDs[i]);
                    //4 将KLFAQ知识库FAQ表记录修改为 -1已删除
                    deleteKLFAQByKLID(tran, KLIDs[i]);
                    //5 插入一条记录到KLOptionLog知识库操作日志表
                    InsertKLOptionLog(tran, (int)Entities.EnumOptStatus.Delete, KLIDs[i]);
                }

                //事务提交
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                msg = "{'msg':'" + ex.Message.ToString() + "'}";
            }
            finally
            {
                connection.Close();
            }

            #endregion
        }

        /// <summary>
        /// 转移
        /// </summary>
        /// <param name="msg"></param>
        private void updateknowledgecategory(out string msg)
        {
            string[] KLIDArray = null;
            //是否批量转移
            try
            {
                if (!string.IsNullOrEmpty(RequestKLID) && !string.IsNullOrEmpty(RequestKCID))
                {
                    if (RequestKLID.IndexOf(',') > 0)
                    {
                        KLIDArray = RequestKLID.Split(',');
                    }
                    //是否批量转移
                    if (KLIDArray != null && KLIDArray.Length > 0)
                    {
                        for (int i = 0; i < KLIDArray.Length; i++)
                        {
                            BLL.KnowledgeCategory.Instance.Update(RequestKCID, KLIDArray[i]);
                        }
                    }
                    else
                    {
                        BLL.KnowledgeCategory.Instance.Update(RequestKCID, RequestKLID);
                    }
                    msg = "success";
                }
                else
                {
                    msg = "fail";
                }


            }
            catch (Exception ex)
            {
                msg = "fail";
            }


        }
        //获取分类下拉列表
        private void bindKnowledgeCategory(out string msg)
        {
            msg = string.Empty;
            int kcid;
            int level;
            if (int.TryParse(RequestKCID, out kcid) && int.TryParse(RequestLevel, out level))
            {
                QueryKnowledgeCategory query = new QueryKnowledgeCategory();
                query.Level = level;
                query.Pid = kcid;
                int regionId;
                if (!string.IsNullOrEmpty(RequestRegionId) && int.TryParse(RequestRegionId, out regionId))
                {
                    query.Regionid = regionId;
                }

                int count;
                DataTable dt = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(query, " ISNULL(SortNum,99999),KCID ", 1, 10000, out count);
                if (dt.Rows.Count > 0)
                {
                    msg += "{root:[";
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg += "{name:'" + dt.Rows[i]["Name"].ToString() + "',kcid:'" + dt.Rows[i]["KCID"].ToString() + "'},";
                }
                if (dt.Rows.Count > 0)
                {
                    msg = msg.TrimEnd(',') + "]}";
                }
            }
        }
        private void bindKnowledgeCategoryExceptDel(out string msg)
        {
            msg = string.Empty;
            int kcid;
            int level;
            if (int.TryParse(RequestKCID, out kcid) && int.TryParse(RequestLevel, out level))
            {
                QueryKnowledgeCategory query = new QueryKnowledgeCategory();
                query.Level = level;
                query.Pid = kcid;
                int regionId;
                if (!string.IsNullOrEmpty(RequestRegionId) && int.TryParse(RequestRegionId, out regionId))
                {
                    query.Regionid = regionId;
                }

                int count;
                //DataTable dt = BLL.KnowledgeCategory.Instance.GetKnowledgeCategory(query, "", 1, 10000, out count);
                DataTable dt = BLL.KnowledgeCategory.Instance.GetKnowledgeCategoryForSearch(query, " isnull(sortNum,999),KCID ", 1, 10000, out count);
                if (dt.Rows.Count > 0)
                {
                    msg += "{root:[";
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msg += "{name:'" + dt.Rows[i]["Name"].ToString() + "',kcid:'" + dt.Rows[i]["KCID"].ToString() + "'},";
                }
                if (dt.Rows.Count > 0)
                {
                    msg = msg.TrimEnd(',') + "]}";
                }
            }
        }

        //审批通过、驳回、停用、删除公用方法：将得到的KLID串进行分解，分别单条操作，同时进行提醒；
        //n=1:审批通过；n=2:驳回；n=3:停用；n=4:删除
        private void operateKnowledgeLib(int n, SqlTransaction sqltran, out string msg)
        {
            msg = string.Empty;
            if (RequestKLID != "")
            {
                string[] KLIDs = RequestKLID.Split(',');
                for (int i = 0; i < KLIDs.Length; i++)
                {
                    long _klid;
                    if (long.TryParse(KLIDs[i], out _klid))
                    {
                        Entities.KnowledgeLib model = BLL.KnowledgeLib.Instance.GetKnowledgeLib(_klid);
                        if (model != null)
                        {
                            switch (n)
                            {
                                case 1:
                                    //审核通过前的状态必须为1：待审核；
                                    if (model.Status == 1)
                                    {
                                        model.Status = 2;
                                        updateKnowledgeLib(sqltran, model);

                                        #region 设置所有用户对当前知识点为未读

                                        BLL.KLReadTag.Instance.UpdateTagByKLID(sqltran, _klid, 0);

                                        #endregion
                                        BLL.Util.InsertUserLog(sqltran, "【审核通过】知识库信息表【审核通过】知识点ID：【" + _klid + "】将操作状态【待审核】修改为：【审核通过】的记录");
                                    }
                                    msg = "{'msg':'审批通过操作成功'}";
                                    break;
                                case 2:
                                    //驳回前的状态必须为1：待审核；
                                    if (model.Status == 1)
                                    {
                                        model.Status = -1;
                                        model.RejectReason = HttpUtility.UrlDecode(RequestRejectReason);   //驳回理由
                                        updateKnowledgeLib(sqltran, model);
                                        BLL.Util.InsertUserLog(sqltran, "【驳回】知识库信息表【驳回】知识点ID：【" + _klid + "】将操作状态【待审核】修改为：【驳回】驳回理由：【" + RequestRejectReason + "】的记录");
                                    }
                                    msg = "{'msg':'驳回操作成功'}";
                                    break;
                                case 3:
                                    //停用前的状态必须为2：审核通过；
                                    if (model.Status == 2)
                                    {
                                        model.Status = 3;
                                        updateKnowledgeLib(sqltran, model);
                                        BLL.Util.InsertUserLog(sqltran, "【停用】知识库信息表【停用】知识点ID：【" + _klid + "】将操作状态【审核通过】修改为：【停用】的记录");
                                    }
                                    msg = "{'msg':'停用操作成功'}";
                                    break;
                                case 4:
                                    //删除前的状态只要不为删除都可以进行删除
                                    if (model.Status != 4)
                                    {
                                        model.Status = 4;
                                        updateKnowledgeLib(sqltran, model);
                                        BLL.Util.InsertUserLog(sqltran, "【删除】知识库信息表【删除】知识点ID：【" + _klid + "】将操作状态【" + getStatusNameBylib(int.Parse(model.Status.ToString())) + "】修改为：【删除】的记录");
                                    }
                                    msg = "{'msg':'删除操作成功'}";
                                    break;
                            }

                        }
                    }
                }
            }
        }

        //修改KnowledgeLib表
        private void updateKnowledgeLib(SqlTransaction sqltran, Entities.KnowledgeLib model)
        {
            if (model != null)
            {
                BLL.KnowledgeLib.Instance.Update(sqltran, model);
            }
        }

        //根据KLID删除KLUploadFile表记录-将状态修改为-1
        private void deleteKLUploadFileByKLID(SqlTransaction sqltran, string klid)
        {
            long _klid;
            if (long.TryParse(klid, out _klid))
            {
                BLL.KLUploadFile.Instance.DeleteByKLID(sqltran, _klid);
                BLL.Util.InsertUserLog(sqltran, "【上传文件信息】知识库上传文件信息表【删除】知识点ID：【" + _klid + "】的记录");
            }
        }

        //根据KLID删除KLFAQ表记录-将状态修改为-1
        private void deleteKLFAQByKLID(SqlTransaction sqltran, string klid)
        {
            long _klid;
            if (long.TryParse(klid, out _klid))
            {
                BLL.KLFAQ.Instance.DeleteByKLID(sqltran, _klid);
                BLL.Util.InsertUserLog(sqltran, "【FAQ】知识库FAQ表【删除】知识点ID：【" + _klid + "】的记录");
            }
        }

        //根据KLID删除KLQuestion表记录-将状态修改为-1，同时将题目答案删除
        private void deleteKLQuestionByKLID(SqlTransaction sqltran, string klqid)
        {
            long _klqid;
            if (long.TryParse(klqid, out _klqid))
            {
                BLL.KLQuestion.Instance.Delete(sqltran, _klqid);
                BLL.Util.InsertUserLog(sqltran, "【试题】知识库试题表【删除】试题ID：【" + _klqid + "】及相关问题答案记录");
            }
        }

        //插入KLOptionLog知识库操作日志表记录
        private void InsertKLOptionLog(SqlTransaction sqltran, int optStatus, string KLID)
        {
            Entities.KLOptionLog model = new KLOptionLog();
            model.KLID = int.Parse(KLID);
            model.OptStatus = optStatus;
            model.Remark = HttpUtility.UrlDecode(RequestRejectReason);
            model.CreateTime = DateTime.Now;
            model.CreateUserID = BLL.Util.GetLoginUserID();
            BLL.KLOptionLog.Instance.Insert(sqltran, model);
            BLL.Util.InsertUserLog(sqltran, "【审核通过】知识库操作日志表【插入】知识点ID：【" + model.KLID + "】操作状态：【" + getStatusName(optStatus) + "】的记录");
        }

        //根据知识库信息的操作状态，得到状态名称
        private string getStatusNameBylib(int status)
        {
            string statusName = string.Empty;
            switch (status)
            {
                case 0: statusName = "未提交";
                    break;
                case 1: statusName = "待审核";
                    break;
                case -1: statusName = "驳回";
                    break;
                case 2: statusName = "已发布";
                    break;
                case 3: statusName = "已停用";
                    break;
                case 4: statusName = "已删除";
                    break;
            }
            return statusName;
        }

        //根据（枚举）操作状态，得到状态名称
        private string getStatusName(int status)
        {
            string statusName = string.Empty;
            switch (status)
            {
                case (int)Entities.EnumOptStatus.NoSubmit: statusName = "未提交";
                    break;
                case (int)Entities.EnumOptStatus.WaitingAudit: statusName = "待审核";
                    break;
                case (int)Entities.EnumOptStatus.Release: statusName = "已发布";
                    break;
                case (int)Entities.EnumOptStatus.Stop: statusName = "停用";
                    break;
                case (int)Entities.EnumOptStatus.Reject: statusName = "驳回";
                    break;
                case (int)Entities.EnumOptStatus.Delete: statusName = "删除";
                    break;
            }
            return statusName;
        }

        //停用试题
        private void questionDisableKnowledgeLib(out string msg)
        {
            msg = string.Empty;

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction sqltran = connection.BeginTransaction("SampleTransaction");

            try
            {

                if (RequestKLQID != "")
                {
                    string[] KLQIDs = RequestKLQID.Split(',');
                    for (int i = 0; i < KLQIDs.Length; i++)
                    {
                        long _klqid;
                        if (long.TryParse(KLQIDs[i], out _klqid))
                        {
                            Entities.KLQuestion model = BLL.KLQuestion.Instance.GetKLQuestion(_klqid);
                            if (model.Status == 0)
                            {
                                model.Status = -2;
                                BLL.KLQuestion.Instance.Update(sqltran, model);
                                BLL.Util.InsertUserLog(sqltran, "【停用】知识库试题表【停用】主键：【" + _klqid + "】将操作状态【正常】修改为：【停用】的记录");
                            }
                            msg = "{'msg':'停用操作成功'}";
                        }
                    }
                }
                //事务提交
                sqltran.Commit();
            }
            catch (Exception ex)
            {
                sqltran.Rollback();
                msg = "{'msg':'" + ex.Message.ToString() + "'}";
            }
            finally
            {
                connection.Close();
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