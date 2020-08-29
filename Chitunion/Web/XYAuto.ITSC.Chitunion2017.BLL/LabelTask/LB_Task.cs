using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.LabelTask;
using XYAuto.ITSC.Chitunion2017.Entities.LabelTask.DTO;
using static XYAuto.ITSC.Chitunion2017.Entities.LabelTask.ENUM;

namespace XYAuto.ITSC.Chitunion2017.BLL.LabelTask
{
    public class LB_Task
    {
        public static readonly LB_Task Instance = new LB_Task();

        /// <summary>
        /// zlb 2017-08-08
        /// 验证参数及打标签
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <param name="Messege">错误参数</param>
        /// <returns></returns>
        public int InserLableTakeInfo(ReqLableTaskDTO ReqDTO, out string Messege)
        {
            Messege = "";
            if (ReqDTO == null)
            {
                Messege = "参数错误";
                return -1;
            }
            if (ReqDTO.TaskID <= 0)
            {
                Messege = "参数错误";
                return -1;
            }
            #region 验证角色
            string UserRole = Common.UserInfo.GetLoginUserRoleIDs();
            if (!UserRole.Contains("SYS001RL00009"))
            {
                Messege = "您没有权限打标签";
                return -1;
            }

            #endregion
            #region 验证分类
            if (ReqDTO.CategoryInfo == null || ReqDTO.CategoryInfo.Count <= 0)
            {
                Messege = "分类不能为空";
                return -1;
            }
            foreach (var item in ReqDTO.CategoryInfo)
            {
                if (string.IsNullOrWhiteSpace(item.DictName))
                {
                    Messege = "参数类型错误";
                    return -1;
                }
            }
            ReqDTO.CategoryInfo = ReqDTO.CategoryInfo.Distinct().ToList();
            if (ReqDTO.CategoryInfo.Count > 5)
            {
                Messege = "最多选择5个分类";
                return -1;
            }
            #endregion
            #region 验证场景
            if (ReqDTO.SceneInfo == null)
            {
                ReqDTO.SceneInfo = new List<LableScene>();
            }
            if (ReqDTO.CustomSceneInfo == null)
            {
                ReqDTO.CustomSceneInfo = new List<LableCustomScene>();
            }
            if (ReqDTO.SceneInfo.Count + ReqDTO.CustomSceneInfo.Count <= 0)
            {
                Messege = "请至少选择或填写一个场景";
                return -1;
            }
            foreach (var item in ReqDTO.SceneInfo)
            {
                if (string.IsNullOrWhiteSpace(item.DictName))
                {
                    Messege = "参数类型错误";
                    return -1;
                }
            }
            ReqDTO.SceneInfo = ReqDTO.SceneInfo.Distinct().ToList();
            ReqDTO.CustomSceneInfo = ReqDTO.CustomSceneInfo.Distinct().ToList();
            foreach (var item in ReqDTO.CustomSceneInfo)
            {
                if (string.IsNullOrWhiteSpace(item.DictName))
                {
                    Messege = "场景不能为空";
                    return -1;
                }
            }
            if (ReqDTO.SceneInfo.Count + ReqDTO.CustomSceneInfo.Count > 100)
            {
                Messege = "最多选择或填写100个场景";
                return -1;
            }
            #endregion
            #region 验证IP
            if (ReqDTO.IPInfo == null || ReqDTO.IPInfo.Count <= 0)
            {
                Messege = "IP或子IP不能为空请选择或填写";
                return -1;
            }
            ReqDTO.IPInfo = ReqDTO.IPInfo.Distinct().ToList();
            if (ReqDTO.IPInfo.Count > 5)
            {
                Messege = "最多选择5个IP";
                return -1;
            }
            foreach (var item in ReqDTO.IPInfo)
            {
                if (string.IsNullOrWhiteSpace(item.DictName))
                {
                    Messege = "参数类型错误";
                    return -1;
                }
                if (item.SonIP == null || item.SonIP.Count <= 0)
                {
                    Messege = "IP或子IP不为空请选择或填写";
                    return -1;
                }
                item.SonIP = item.SonIP.Distinct().ToList();
                if (item.SonIP.Count > 10)
                {
                    Messege = "最多填写10个子IP";
                    return -1;
                }
                foreach (var item1 in item.SonIP)
                {
                    if (string.IsNullOrWhiteSpace(item1.DictName))
                    {
                        Messege = "子IP不能为空";
                        return -1;
                    }
                    if (item1.CustomLableInfo == null || item1.CustomLableInfo.Count <= 0)
                    {
                        Messege = "标签不能为空";
                        return -1;
                    }
                    item1.CustomLableInfo = item1.CustomLableInfo.Distinct().ToList();
                    if (item1.CustomLableInfo.Count > 100)
                    {
                        Messege = "最多填写100个标签";
                        return -1;
                    }

                    foreach (var item2 in item1.CustomLableInfo)
                    {
                        if (string.IsNullOrWhiteSpace(item2.DictName))
                        {
                            Messege = "标签不能为空";
                            return -1;
                        }
                    }
                }
            }
            #endregion
            #region 验证标签
            //if (ReqDTO.CustomLableInfo == null || ReqDTO.CustomLableInfo.Count <= 0)
            //{
            //    Messege = "标签不能为空";
            //    return -1;
            //}
            //foreach (var item in ReqDTO.CustomLableInfo)
            //{
            //    if (string.IsNullOrWhiteSpace(item.DictName))
            //    {
            //        Messege = "标签不能为空";
            //        return -1;
            //    }
            //}
            //ReqDTO.CustomLableInfo = ReqDTO.CustomLableInfo.Distinct().ToList();
            //if (ReqDTO.CustomLableInfo.Count > 100)
            //{
            //    Messege = "最多选择100个标签";
            //    return -1;
            //}
            #endregion
            #region 验证该用户是否领取了任务
            int UserID = Common.UserInfo.GetLoginUserID();
            int TaskCount = Dal.LabelTask.LB_Task.Instance.SelectSelfTaskCount(ReqDTO.TaskID, UserID);
            if (TaskCount <= 0)
            {
                Messege = "您还未领取该任务！";
                return -1;
            }
            #endregion
            #region 验证该用户是否已打过标签
            int PlayLableCount = Dal.LabelTask.LB_Task.Instance.SelectSelfLableTaskeCount(ReqDTO.TaskID, UserID);
            if (PlayLableCount > 0)
            {
                Messege = "您已打过此标签,请勿重复打标签！";
                return -1;
            }
            #endregion
            #region 进行打标签
            int result = Dal.LabelTask.LB_Task.Instance.InserLableTakeInfo(ReqDTO, UserID);
            if (result <= 0)
            {
                Messege = "提交失败";
            }
            else
            {
                Dal.LabelTask.LB_Task.Instance.UpdateTaskAssignStatus(ReqDTO.TaskID, (int)EnumTaskStatus.已打标签, ReqDTO.Summary, UserID);
                int lableManCount = Dal.LabelTask.LB_Task.Instance.SelectLableTaskeCount(ReqDTO.TaskID, (int)EnumTaskStatus.已打标签);
                int roleIDCount = Dal.LabelTask.LB_Task.Instance.SlelectUserCountByRoleID("SYS001RL00009");
                int TaskStatus = 0;
                if (roleIDCount > lableManCount)
                {
                    TaskStatus = (int)EnumTaskStatus.已打标签;
                }
                else
                {
                    TaskStatus = (int)EnumTaskStatus.待审;
                    Dal.LabelTask.LB_Task.Instance.UpdateTaskAssignStatus(ReqDTO.TaskID, TaskStatus, ReqDTO.Summary, UserID);
                }
                Dal.LabelTask.LB_Task.Instance.UpdateTaskStatus(ReqDTO.TaskID, TaskStatus, 0, 0);
                int projectID = Dal.LabelTask.LB_Task.Instance.SelectProjectID(ReqDTO.TaskID);
                Dal.LabelTask.LB_Task.Instance.UpdateProjectStatus(projectID, (int)EnumProjectStatus.执行中);
                Dal.LabelTask.LB_Task.Instance.InsertTaskOperateInfo(ReqDTO.TaskID, UserID, (int)EnumTaskOptType.提交, "");
            }
            return result;
            #endregion
        }
        /// <summary>
        /// zlb 2017-08-08
        /// 查询媒体或文章信息
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public Dictionary<string, object> SelectMediaOrArticleInfo(int TaskID)
        {

            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("MediaOrArticle", "");
            dicAll.Add("MediaName", "");
            dicAll.Add("MediaType", 0);
            dicAll.Add("HeadImg", "");
            dicAll.Add("ArticleID", 0);
            dicAll.Add("Summary", 5);
            dicAll.Add("KeyWord", 50);
            dicAll.Add("Content", "");
            DataTable dtType = Dal.LabelTask.LB_Task.Instance.SelectMediaTypeByTaskID(TaskID);
            int MediaType = 0;
            int ProjectType = 0;
            if (dtType != null && dtType.Rows.Count > 0)
            {
                MediaType = Convert.ToInt32(dtType.Rows[0]["MediaType"]);
                ProjectType = Convert.ToInt32(dtType.Rows[0]["ProjectType"]);
            }
            if (!Enum.IsDefined(typeof(ENUM.EnumMediaType), MediaType))
            {
                return dicAll;
            }
            if (!Enum.IsDefined(typeof(ENUM.EnumProjectType), ProjectType))
            {
                return dicAll;
            }
            DataTable dt = Dal.LabelTask.LB_Task.Instance.SelectMediaOrArticleInfo((ENUM.EnumMediaType)MediaType, (ENUM.EnumProjectType)ProjectType, TaskID);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                dicAll["MediaOrArticle"] = dr["MediaOrArticle"].ToString();
                dicAll["MediaName"] = dr["MediaName"].ToString();
                dicAll["MediaType"] = Convert.ToInt32(dr["MediaType"]);
                dicAll["HeadImg"] = dr["HeadImg"].ToString();
                dicAll["ArticleID"] = Convert.ToInt32(dr["ArticleID"]);
                dicAll["KeyWord"] = Convert.ToInt32(dr["KeyWord"]);
                dicAll["Summary"] = Convert.ToInt32(dr["Summary"]);
                dicAll["Content"] = dr["Content"].ToString();
            }
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-08-09
        /// 查询媒体或文章的标签信息
        /// </summary>
        /// <param name="TaskID">任务ID</param>
        /// <param name="SelectType">查询类型（1：查看或审核；2：修改）</param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public Dictionary<string, object> SelectMediaOrArticleLable(int TaskID, int SelectType, out string ErrorMessage)
        {
            ErrorMessage = "";
            Dictionary<string, object> dicAll = SelectMediaOrArticleInfo(TaskID);
            dicAll.Add("CategoryInfo", new List<Dictionary<string, object>>());
            dicAll.Add("SceneInfo", new List<Dictionary<string, object>>());
            dicAll.Add("CustomSceneInfo", new List<Dictionary<string, object>>());
            dicAll.Add("IPInfo", new List<Dictionary<string, object>>());
            dicAll.Add("CustomLableInfo", new List<Dictionary<string, object>>());
            if (SelectType != 1 && SelectType != 2)
            {
                ErrorMessage = "查询类型错误";
            }
            string UserRole = Common.UserInfo.GetLoginUserRoleIDs();
            if (SelectType == 1 || (SelectType == 2 && !UserRole.Contains("SYS001RL00010")))
            {

                int UserID = 0;
                if (!UserRole.Contains("SYS001RL00010"))
                {
                    UserID = Common.UserInfo.GetLoginUserID();
                }
                DataSet ds = Dal.LabelTask.LB_Task.Instance.SelectTaskLableInfo(TaskID, UserID);
                if (UserID > 0)
                {
                    if (ds.Tables[3] != null && ds.Tables[3].Rows.Count > 0)
                    {
                        dicAll["Summary"] = Convert.ToInt32(ds.Tables[3].Rows[0]["Summary"]);
                    }
                    DataTable dtSubmitInfo = ds.Tables[4];
                    if (dtSubmitInfo.Rows.Count > 0)

                    {
                        dicAll["SubmitMan"] = dtSubmitInfo.Rows[0]["Creater"].ToString();
                        dicAll["SubmitTime"] = dtSubmitInfo.Rows[0]["CreateTime"].ToString();
                    }
                    DataTable dtExaminInfo = ds.Tables[5];
                    if (dtExaminInfo.Rows.Count > 0)
                    {
                        dicAll["ExamineMan"] = dtExaminInfo.Rows[0]["Creater"].ToString();
                        dicAll["ExamineTime"] = dtExaminInfo.Rows[0]["CreateTime"].ToString();
                    }
                }
                List<LablesModel> listTagInfo = Common.Util.DataTableToList<LablesModel>(ds.Tables[0]);
                List<SonIpModel> listSonIpInfo = Common.Util.DataTableToList<SonIpModel>(ds.Tables[1]);
                List<SonIpModel> listSonIpLableInfo = Common.Util.DataTableToList<SonIpModel>(ds.Tables[2]);
                var listClass = listTagInfo.GroupBy(a => new { a.DictType }).ToList();

                foreach (var item in listClass)
                {
                    switch ((EnumLableType)item.Key.DictType)
                    {
                        case EnumLableType.分类:
                            var varTagInfo = item.ToList().GroupBy(i => new { i.DictId, i.Name }).ToList();
                            List<Dictionary<string, object>> listTag = new List<Dictionary<string, object>>();
                            DataTable dtCategory = Dal.LabelTask.LB_Task.Instance.SelectLableInfo((int)EnumLableType.分类);
                            for (int i = 0; i < dtCategory.Rows.Count; i++)
                            {
                                DataRow dr = dtCategory.Rows[i];
                                if (varTagInfo.Where(t => t.Key.DictId == Convert.ToInt32(dr["DictId"])).Count() <= 0)
                                {
                                    Dictionary<string, object> dicCategroy = new Dictionary<string, object>();
                                    dicCategroy.Add("DictId", Convert.ToInt32(dr["DictId"]));
                                    dicCategroy.Add("DictName", dr["DictName"].ToString());
                                    dicCategroy.Add("Creater", null);
                                    dicCategroy.Add("CreatCount", 0);
                                    listTag.Add(dicCategroy);
                                }
                            }
                            foreach (var itemTagInfo in varTagInfo)
                            {
                                Dictionary<string, object> dicCategroy = new Dictionary<string, object>();
                                dicCategroy.Add("DictId", itemTagInfo.Key.DictId);
                                dicCategroy.Add("DictName", itemTagInfo.Key.Name);
                                dicCategroy.Add("Creater", itemTagInfo.Select(t => t.Creater));
                                dicCategroy.Add("CreatCount", itemTagInfo.Count());
                                listTag.Add(dicCategroy);

                            }
                            dicAll["CategoryInfo"] = listTag;
                            break;
                        case EnumLableType.场景:
                            var varScene = item.ToList().GroupBy(i => new { i.IsCustom }).ToList();
                            List<Dictionary<string, object>> listCustomScene = new List<Dictionary<string, object>>();
                            List<Dictionary<string, object>> listSceneInfo = new List<Dictionary<string, object>>();
                            DataTable dtScene = Dal.LabelTask.LB_Task.Instance.SelectLableInfo((int)EnumLableType.场景);
                            foreach (var itemScene in varScene)
                            {
                                if (itemScene.Key.IsCustom == true)
                                {
                                    var varCustomScene = itemScene.ToList().GroupBy(i => new { i.Name }).ToList();
                                    foreach (var itemCustomScene in varCustomScene)
                                    {
                                        Dictionary<string, object> dicCustomScene = new Dictionary<string, object>();
                                        dicCustomScene.Add("DictName", itemCustomScene.Key.Name);
                                        dicCustomScene.Add("Creater", itemCustomScene.Select(t => t.Creater));
                                        dicCustomScene.Add("CreatCount", itemCustomScene.Count());
                                        listCustomScene.Add(dicCustomScene);
                                    }
                                }
                                else
                                {
                                    var varSceneInfo = itemScene.ToList().GroupBy(i => new { i.DictId, i.Name }).ToList();
                                    foreach (var itemSceneInfo in varSceneInfo)
                                    {
                                        Dictionary<string, object> dicSceneInfo = new Dictionary<string, object>();
                                        dicSceneInfo.Add("DictId", itemSceneInfo.Key.DictId);
                                        dicSceneInfo.Add("DictName", itemSceneInfo.Key.Name);
                                        dicSceneInfo.Add("Creater", itemSceneInfo.Select(t => t.Creater));
                                        dicSceneInfo.Add("CreatCount", itemSceneInfo.Count());
                                        listSceneInfo.Add(dicSceneInfo);
                                    }
                                    for (int i = 0; i < dtScene.Rows.Count; i++)
                                    {
                                        DataRow dr = dtScene.Rows[i];
                                        if (varSceneInfo.Where(t => t.Key.DictId == Convert.ToInt32(dr["DictId"])).Count() <= 0)
                                        {
                                            Dictionary<string, object> dicSceneInfo = new Dictionary<string, object>();
                                            dicSceneInfo.Add("DictId", Convert.ToInt32(dr["DictId"]));
                                            dicSceneInfo.Add("DictName", dr["DictName"].ToString());
                                            dicSceneInfo.Add("Creater", null);
                                            dicSceneInfo.Add("CreatCount", 0);
                                            listSceneInfo.Add(dicSceneInfo);
                                        }
                                    }
                                }
                            }
                            dicAll["SceneInfo"] = listSceneInfo;
                            dicAll["CustomSceneInfo"] = listCustomScene;
                            break;
                        case EnumLableType.IP:
                            var varIPInfo = item.ToList().GroupBy(i => new { i.DictId, i.Name }).ToList();
                            List<Dictionary<string, object>> listIPInfo = new List<Dictionary<string, object>>();
                            foreach (var itemIPInfo in varIPInfo)
                            {
                                DataTable dtSonIp = Dal.LabelTask.LB_Task.Instance.SelectSonIpInfo(itemIPInfo.Key.DictId);
                                Dictionary<string, object> dicIPInfo = new Dictionary<string, object>();
                                dicIPInfo.Add("DictId", itemIPInfo.Key.DictId);
                                dicIPInfo.Add("DictName", itemIPInfo.Key.Name);
                                dicIPInfo.Add("Creater", itemIPInfo.Select(t => t.Creater));
                                dicIPInfo.Add("CreatCount", itemIPInfo.Count());
                                List<Dictionary<string, object>> listSonIp = new List<Dictionary<string, object>>();
                                List<SonIpModel> sonIpList = new List<SonIpModel>();
                                foreach (var itemSonIPInfo in itemIPInfo.ToList())
                                {
                                    sonIpList.AddRange(listSonIpInfo.Where(t => t.LabelID == itemSonIPInfo.LabelID).ToList());
                                }
                                var varSonIP = sonIpList.GroupBy(i => new { i.DictId, i.Name }).ToList();
                                foreach (var itemSonIP in varSonIP)
                                {
                                    List<SonIpModel> sonIpLableList = new List<SonIpModel>();
                                    Dictionary<string, object> dicSonIPInfo = new Dictionary<string, object>();
                                    dicSonIPInfo.Add("DictId", itemSonIP.Key.DictId);
                                    dicSonIPInfo.Add("DictName", itemSonIP.Key.Name);
                                    dicSonIPInfo.Add("Creater", itemSonIP.Select(t => t.Creater));
                                    dicSonIPInfo.Add("CreatCount", itemSonIP.Count());
                                    List<Dictionary<string, object>> listSonIpLable = new List<Dictionary<string, object>>();
                                    foreach (var itemSonIpLabelInfo in itemSonIP.ToList())
                                    {
                                        sonIpLableList.AddRange(listSonIpLableInfo.Where(t => t.SonIpID == itemSonIpLabelInfo.SonIpID).ToList());
                                    }
                                    var varSonIpLable = sonIpLableList.GroupBy(i => new { i.Name }).ToList();
                                    foreach (var itemSonLable in varSonIpLable)
                                    {
                                        Dictionary<string, object> dicSonIPLableInfo = new Dictionary<string, object>();
                                        dicSonIPLableInfo.Add("DictName", itemSonLable.Key.Name);
                                        dicSonIPLableInfo.Add("Creater", itemSonLable.Select(t => t.Creater));
                                        dicSonIPLableInfo.Add("CreatCount", itemSonLable.Count());
                                        listSonIpLable.Add(dicSonIPLableInfo);
                                    }
                                    dicSonIPInfo.Add("CustomLableInfo", listSonIpLable);
                                    listSonIp.Add(dicSonIPInfo);
                                }
                                for (int i = 0; i < dtSonIp.Rows.Count; i++)
                                {
                                    DataRow dr = dtSonIp.Rows[i];
                                    if (listSonIp.Where(t => Convert.ToInt32(t["DictId"]) == Convert.ToInt32(dr["DictId"])).Count() <= 0)
                                    {
                                        Dictionary<string, object> dicSonIpInfo = new Dictionary<string, object>();
                                        dicSonIpInfo.Add("DictId", Convert.ToInt32(dr["DictId"]));
                                        dicSonIpInfo.Add("DictName", dr["DictName"].ToString());
                                        dicSonIpInfo.Add("Creater", null);
                                        dicSonIpInfo.Add("CreatCount", 0);
                                        listSonIp.Add(dicSonIpInfo);
                                    }
                                }
                                dicIPInfo.Add("SonIP", listSonIp);
                                listIPInfo.Add(dicIPInfo);
                            }
                            DataTable dtIPInfo = Dal.LabelTask.LB_Task.Instance.SelectLableInfo((int)EnumLableType.IP);
                            for (int i = 0; i < dtIPInfo.Rows.Count; i++)
                            {
                                DataRow dr = dtIPInfo.Rows[i];
                                if (varIPInfo.Where(t => t.Key.DictId == Convert.ToInt32(dr["DictId"])).Count() <= 0)
                                {
                                    Dictionary<string, object> dicCategroy = new Dictionary<string, object>();
                                    dicCategroy.Add("DictId", Convert.ToInt32(dr["DictId"]));
                                    dicCategroy.Add("DictName", dr["DictName"].ToString());
                                    dicCategroy.Add("Creater", null);
                                    dicCategroy.Add("CreatCount", 0);
                                    listIPInfo.Add(dicCategroy);
                                }
                            }
                            dicAll["IPInfo"] = listIPInfo;
                            break;
                            //case EnumLableType.标签:
                            //    var varLableInfo = item.ToList().GroupBy(i => new { i.Name }).ToList();
                            //    List<Dictionary<string, object>> listLableInfo = new List<Dictionary<string, object>>();
                            //    foreach (var itemLableInfo in varLableInfo)
                            //    {
                            //        Dictionary<string, object> dicLableInfo = new Dictionary<string, object>();
                            //        dicLableInfo.Add("DictName", itemLableInfo.Key.Name);
                            //        dicLableInfo.Add("Creater", itemLableInfo.Select(t => t.Creater));
                            //        dicLableInfo.Add("CreatCount", itemLableInfo.Count());
                            //        listLableInfo.Add(dicLableInfo);
                            //    }
                            //    dicAll["CustomLableInfo"] = listLableInfo;
                            //    break;

                    }
                }
            }
            else
            {
                DataSet ds = Dal.LabelTask.LB_Task.Instance.SelectTaskAuditLableInfo(TaskID);
                #region 查询基表标签信息
                List<LablesModel> listTagInfo = Common.Util.DataTableToList<LablesModel>(ds.Tables[0]);
                List<SonIpModel> listSonIpInfo = Common.Util.DataTableToList<SonIpModel>(ds.Tables[1]);
                List<SonIpModel> listSonIpLableInfo = Common.Util.DataTableToList<SonIpModel>(ds.Tables[2]);
                var listClass = listTagInfo.GroupBy(a => new { a.DictType }).ToList();
                foreach (var item in listClass)
                {
                    switch ((EnumLableType)item.Key.DictType)
                    {
                        case EnumLableType.分类:
                            List<Dictionary<string, object>> listTag = new List<Dictionary<string, object>>();
                            DataTable dtCategory = Dal.LabelTask.LB_Task.Instance.SelectLableInfo((int)EnumLableType.分类);
                            for (int i = 0; i < dtCategory.Rows.Count; i++)
                            {
                                DataRow dr = dtCategory.Rows[i];
                                if (item.ToList().Where(t => t.DictId == Convert.ToInt32(dr["DictId"])).Count() <= 0)
                                {
                                    Dictionary<string, object> dicCategroy = new Dictionary<string, object>();
                                    dicCategroy.Add("DictId", Convert.ToInt32(dr["DictId"]));
                                    dicCategroy.Add("DictName", dr["DictName"].ToString());
                                    dicCategroy.Add("Creater", null);
                                    dicCategroy.Add("CreatCount", 0);
                                    listTag.Add(dicCategroy);
                                }
                            }
                            foreach (var itemTagInfo in item.ToList())
                            {
                                Dictionary<string, object> dicCategroy = new Dictionary<string, object>();
                                dicCategroy.Add("DictId", itemTagInfo.DictId);
                                dicCategroy.Add("DictName", itemTagInfo.Name);
                                dicCategroy.Add("Creater", null);
                                dicCategroy.Add("CreatCount", 1);
                                listTag.Add(dicCategroy);

                            }
                            dicAll["CategoryInfo"] = listTag;
                            break;
                        case EnumLableType.场景:
                            List<Dictionary<string, object>> listCustomScene = new List<Dictionary<string, object>>();
                            List<Dictionary<string, object>> listSceneInfo = new List<Dictionary<string, object>>();
                            DataTable dtScene = Dal.LabelTask.LB_Task.Instance.SelectLableInfo((int)EnumLableType.场景);
                            foreach (var itemCustomScene in item.ToList())
                            {
                                Dictionary<string, object> dicCustomScene = new Dictionary<string, object>();
                                dicCustomScene.Add("DictName", itemCustomScene.Name);
                                dicCustomScene.Add("Creater", null);
                                dicCustomScene.Add("CreatCount", 1);
                                if (itemCustomScene.IsCustom)
                                {
                                    listCustomScene.Add(dicCustomScene);
                                }
                                else
                                {
                                    dicCustomScene.Add("DictId", itemCustomScene.DictId);
                                    listSceneInfo.Add(dicCustomScene);
                                }
                            }
                            for (int i = 0; i < dtScene.Rows.Count; i++)
                            {
                                DataRow dr = dtScene.Rows[i];
                                if (item.ToList().Where(t => t.DictId == Convert.ToInt32(dr["DictId"])).Count() <= 0)
                                {
                                    Dictionary<string, object> dicSceneInfo = new Dictionary<string, object>();
                                    dicSceneInfo.Add("DictId", Convert.ToInt32(dr["DictId"]));
                                    dicSceneInfo.Add("DictName", dr["DictName"].ToString());
                                    dicSceneInfo.Add("Creater", null);
                                    dicSceneInfo.Add("CreatCount", 0);
                                    listSceneInfo.Add(dicSceneInfo);
                                }
                            }
                            dicAll["SceneInfo"] = listSceneInfo;
                            dicAll["CustomSceneInfo"] = listCustomScene;
                            break;
                        case EnumLableType.IP:
                            var varIPInfo = item.ToList().GroupBy(i => new { i.DictId, i.Name }).ToList();
                            List<Dictionary<string, object>> listIPInfo = new List<Dictionary<string, object>>();
                            foreach (var itemIPInfo in varIPInfo)
                            {
                                DataTable dtSonIp = Dal.LabelTask.LB_Task.Instance.SelectSonIpInfo(itemIPInfo.Key.DictId);
                                Dictionary<string, object> dicIPInfo = new Dictionary<string, object>();
                                dicIPInfo.Add("DictId", itemIPInfo.Key.DictId);
                                dicIPInfo.Add("DictName", itemIPInfo.Key.Name);
                                dicIPInfo.Add("Creater", itemIPInfo.Select(t => t.Creater));
                                dicIPInfo.Add("CreatCount", itemIPInfo.Count());
                                List<Dictionary<string, object>> listSonIp = new List<Dictionary<string, object>>();
                                List<SonIpModel> sonIpList = new List<SonIpModel>();
                                foreach (var itemSonIPInfo in itemIPInfo.ToList())
                                {
                                    sonIpList.AddRange(listSonIpInfo.Where(t => t.LabelID == itemSonIPInfo.LabelID).ToList());
                                }
                                var varSonIP = sonIpList.GroupBy(i => new { i.DictId, i.Name });
                                foreach (var itemSonIP in varSonIP)
                                {
                                    List<SonIpModel> sonIpLableList = new List<SonIpModel>();
                                    Dictionary<string, object> dicSonIPInfo = new Dictionary<string, object>();
                                    dicSonIPInfo.Add("DictId", itemSonIP.Key.DictId);
                                    dicSonIPInfo.Add("DictName", itemSonIP.Key.Name);
                                    dicSonIPInfo.Add("Creater", itemSonIP.Select(t => t.Creater));
                                    dicSonIPInfo.Add("CreatCount", itemSonIP.Count());
                                    List<Dictionary<string, object>> listSonIpLable = new List<Dictionary<string, object>>();
                                    foreach (var itemSonIpLabelInfo in itemSonIP.ToList())
                                    {
                                        sonIpLableList.AddRange(listSonIpLableInfo.Where(t => t.SonIpID == itemSonIpLabelInfo.SonIpID).ToList());
                                    }
                                    var varSonIpLable = sonIpLableList.GroupBy(i => new { i.Name });
                                    foreach (var itemSonLable in varSonIpLable)
                                    {
                                        Dictionary<string, object> dicSonIPLableInfo = new Dictionary<string, object>();
                                        dicSonIPLableInfo.Add("DictName", itemSonLable.Key.Name);
                                        dicSonIPLableInfo.Add("Creater", itemSonLable.Select(t => t.Creater));
                                        dicSonIPLableInfo.Add("CreatCount", itemSonLable.Count());
                                        listSonIpLable.Add(dicSonIPLableInfo);
                                    }
                                    dicSonIPInfo.Add("CustomLableInfo", listSonIpLable);
                                    listSonIp.Add(dicSonIPInfo);
                                }
                                for (int i = 0; i < dtSonIp.Rows.Count; i++)
                                {
                                    DataRow dr = dtSonIp.Rows[i];
                                    if (listSonIp.Where(t => Convert.ToInt32(t["DictId"]) == Convert.ToInt32(dr["DictId"])).Count() <= 0)
                                    {
                                        Dictionary<string, object> dicSonIpInfo = new Dictionary<string, object>();
                                        dicSonIpInfo.Add("DictId", Convert.ToInt32(dr["DictId"]));
                                        dicSonIpInfo.Add("DictName", dr["DictName"].ToString());
                                        dicSonIpInfo.Add("Creater", null);
                                        dicSonIpInfo.Add("CreatCount", 0);
                                        listSonIp.Add(dicSonIpInfo);
                                    }
                                }
                                dicIPInfo.Add("SonIP", listSonIp);
                                listIPInfo.Add(dicIPInfo);
                            }
                            DataTable dtIPInfo = Dal.LabelTask.LB_Task.Instance.SelectLableInfo((int)EnumLableType.IP);
                            for (int i = 0; i < dtIPInfo.Rows.Count; i++)
                            {
                                DataRow dr = dtIPInfo.Rows[i];
                                if (varIPInfo.Where(t => t.Key.DictId == Convert.ToInt32(dr["DictId"])).Count() <= 0)
                                {
                                    Dictionary<string, object> dicCategroy = new Dictionary<string, object>();
                                    dicCategroy.Add("DictId", Convert.ToInt32(dr["DictId"]));
                                    dicCategroy.Add("DictName", dr["DictName"].ToString());
                                    dicCategroy.Add("Creater", null);
                                    dicCategroy.Add("CreatCount", 0);
                                    listIPInfo.Add(dicCategroy);
                                }
                            }
                            dicAll["IPInfo"] = listIPInfo;
                            break;
                            //case EnumLableType.标签:
                            //    List<Dictionary<string, object>> listLableInfo = new List<Dictionary<string, object>>();
                            //    foreach (var itemLableInfo in item.ToList())
                            //    {
                            //        Dictionary<string, object> dicLableInfo = new Dictionary<string, object>();
                            //        dicLableInfo.Add("DictName", itemLableInfo.Name);
                            //        dicLableInfo.Add("Creater", null);
                            //        dicLableInfo.Add("CreatCount", 1);
                            //        listLableInfo.Add(dicLableInfo);
                            //    }
                            //    dicAll["CustomLableInfo"] = listLableInfo;
                            //    break;
                    }
                }
                #endregion
                DataTable dtSubmitInfo = ds.Tables[3];
                StringBuilder sbSubmitUser = new StringBuilder();
                StringBuilder sbSubmitTime = new StringBuilder();
                for (int i = 0; i < dtSubmitInfo.Rows.Count; i++)
                {
                    sbSubmitUser.Append(dtSubmitInfo.Rows[i]["Creater"].ToString() + ",");
                }
                if (dtSubmitInfo.Rows.Count > 0)
                {
                    string strSubmitUser = sbSubmitUser.ToString();
                    dicAll["SubmitMan"] = strSubmitUser.Substring(0, strSubmitUser.Length - 1);
                    dicAll["SubmitTime"] = dtSubmitInfo.Rows[0]["CreateTime"].ToString();
                }
                DataTable dtExaminInfo = ds.Tables[4];
                if (dtExaminInfo.Rows.Count > 0)
                {
                    dicAll["ExamineMan"] = dtExaminInfo.Rows[0]["Creater"].ToString();
                    dicAll["ExamineTime"] = dtExaminInfo.Rows[0]["CreateTime"].ToString();
                }
                DataTable UpdateInfo = ds.Tables[5];
                if (UpdateInfo.Rows.Count > 1)
                {
                    dicAll["UpdateMan"] = UpdateInfo.Rows[0]["Creater"].ToString();
                    dicAll["UpdateTime"] = UpdateInfo.Rows[0]["CreateTime"].ToString();
                }
                if (UpdateInfo.Rows.Count > 1)
                {
                    ReqLableTaskDTO lableTaskInfoNew = JsonConvert.DeserializeObject<ReqLableTaskDTO>(UpdateInfo.Rows[0]["OptContent"].ToString());
                    ReqLableTaskDTO lableTaskInfoOld = JsonConvert.DeserializeObject<ReqLableTaskDTO>(UpdateInfo.Rows[1]["OptContent"].ToString());

                    StringBuilder sbCategoryNew = new StringBuilder();
                    StringBuilder sbCategoryOld = new StringBuilder();
                    foreach (var itemCategory in lableTaskInfoNew.CategoryInfo)
                    {
                        sbCategoryNew.Append(itemCategory.DictName + ",");
                    }
                    foreach (var itemCategory in lableTaskInfoOld.CategoryInfo)
                    {
                        sbCategoryOld.Append(itemCategory.DictName + ",");
                    }
                    string strCategoryNew = sbCategoryNew.ToString();
                    dicAll["NowCategory"] = strCategoryNew.Substring(0, strCategoryNew.Length - 1);
                    string strCategoryOld = sbCategoryOld.ToString();
                    dicAll["OldCategory"] = strCategoryOld.Substring(0, strCategoryOld.Length - 1);

                    StringBuilder sbSceneNew = new StringBuilder();
                    StringBuilder sbSceneOld = new StringBuilder();
                    foreach (var itemScene in lableTaskInfoNew.SceneInfo)
                    {
                        sbSceneNew.Append(itemScene.DictName + ",");
                    }
                    if (lableTaskInfoNew.CustomSceneInfo != null)
                    {
                        foreach (var itemScene in lableTaskInfoNew.CustomSceneInfo)
                        {
                            sbSceneNew.Append(itemScene.DictName + ",");
                        }
                    }

                    foreach (var itemScene in lableTaskInfoOld.SceneInfo)
                    {
                        sbSceneOld.Append(itemScene.DictName + ",");
                    }
                    if (lableTaskInfoOld.CustomSceneInfo != null)
                    {
                        foreach (var itemScene in lableTaskInfoOld.CustomSceneInfo)
                        {
                            sbSceneOld.Append(itemScene.DictName + ",");
                        }
                    }
                    string strSceneNew = sbSceneNew.ToString();
                    dicAll["NewScene"] = strSceneNew.Substring(0, strSceneNew.Length - 1);
                    string strSceneOld = sbSceneOld.ToString();
                    dicAll["OldScene"] = strSceneOld.Substring(0, strSceneOld.Length - 1);

                    StringBuilder sbIPNew = new StringBuilder();
                    StringBuilder sbIPOld = new StringBuilder();
                    foreach (var itemIP in lableTaskInfoNew.IPInfo)
                    {
                        sbIPNew.Append(itemIP.DictName + ":");
                        if (itemIP.SonIP != null)
                        {
                            foreach (var itemSonIP in itemIP.SonIP)
                            {
                                sbIPNew.Append(itemSonIP.DictName + " ");
                                if (itemSonIP.CustomLableInfo != null)
                                {
                                    sbIPNew.Append("[");
                                    foreach (var item in itemSonIP.CustomLableInfo)
                                    {
                                        sbIPNew.Append(item.DictName + " ");
                                    }
                                    sbIPNew.Append("]");
                                }
                            }
                        }
                        sbIPNew.Append(",");
                    }
                    foreach (var itemIP in lableTaskInfoOld.IPInfo)
                    {
                        sbIPOld.Append(itemIP.DictName + ":");
                        if (itemIP.SonIP != null)
                        {
                            foreach (var itemSonIP in itemIP.SonIP)
                            {
                                sbIPOld.Append(itemSonIP.DictName + " ");
                                if (itemSonIP.CustomLableInfo != null)
                                {
                                    sbIPOld.Append("[");
                                    foreach (var item in itemSonIP.CustomLableInfo)
                                    {
                                        sbIPOld.Append(item.DictName + " ");
                                    }
                                    sbIPOld.Append("]");
                                }
                            }
                        }
                        sbIPOld.Append(",");
                    }
                    string strIPNew = sbIPNew.ToString();
                    dicAll["NewIP"] = strIPNew.Substring(0, strIPNew.Length - 1);
                    string strIPOld = sbIPOld.ToString();
                    dicAll["OldIP"] = strIPOld.Substring(0, strIPOld.Length - 1);

                    //StringBuilder sbLableNew = new StringBuilder();
                    //StringBuilder sbLableOld = new StringBuilder();
                    //foreach (var itemCustomLable in lableTaskInfoNew.CustomLableInfo)
                    //{
                    //    sbLableNew.Append(itemCustomLable.DictName + ",");
                    //}
                    //foreach (var itemCustomLable in lableTaskInfoOld.CustomLableInfo)
                    //{
                    //    sbLableOld.Append(itemCustomLable.DictName + ",");
                    //}
                    //string strLableNew = sbLableNew.ToString();
                    //dicAll["NewLabel"] = strLableNew.Substring(0, strLableNew.Length - 1);
                    //string strLableOld = sbLableOld.ToString();
                    //dicAll["OldLabel"] = strLableOld.Substring(0, strLableOld.Length - 1);
                }
            }
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-08-11
        /// 审核标签
        /// </summary>
        /// <param name="ReqDTO"></param>
        /// <param name="Messege"></param>
        /// <returns></returns>
        public int ExmainLableInfo(ReqLableTaskDTO ReqDTO, out string Messege)
        {
            Messege = "";
            if (ReqDTO == null || ReqDTO.TaskID <= 0 || (ReqDTO.OptType != 1 && ReqDTO.OptType != 2))
            {
                Messege = "参数错误";
                return -1;
            }
            string UserRole = Common.UserInfo.GetLoginUserRoleIDs();
            if (!UserRole.Contains("SYS001RL00010"))
            {
                if (ReqDTO.OptType == 2)
                {
                    Messege = "您没有审核的权限";
                }
                else
                {
                    Messege = "您没有修改的权限";
                }
                return -1;
            }
            int ProjectStatus = 0;
            if (ReqDTO.OptType == 2)
            {
                ProjectStatus = (int)EnumTaskStatus.待审;
            }
            else
            {
                ProjectStatus = (int)EnumTaskStatus.已审;
            }
            int ProjectType = Dal.LabelTask.LB_Task.Instance.SelectProjectTypeByTaskID(ReqDTO.TaskID, ProjectStatus);
            if (ProjectType <= 0)
            {
                if (ReqDTO.OptType == 2)
                {
                    Messege = "无法审核该任务";
                }
                else
                {
                    Messege = "无法修改该任务";
                }
                return -1;
            }
            int UserID = Common.UserInfo.GetLoginUserID();
            DataTable dtIP = Dal.LabelTask.LB_Task.Instance.SelectLableInfoByTaskID(ReqDTO.TaskID, (int)EnumLableType.IP);
            StringBuilder sbIP = new StringBuilder();
            string strIP = "";
            if (dtIP != null && dtIP.Rows.Count > 0)
            {
                for (int i = 0; i < dtIP.Rows.Count; i++)
                {
                    sbIP.Append(dtIP.Rows[i][0] + ",");
                }
                strIP = sbIP.ToString();
                strIP = strIP.Substring(0, strIP.Length - 1);
            }

            int result = Dal.LabelTask.LB_Task.Instance.ExmainLableInfo(ReqDTO, strIP, ProjectType, UserID);
            if (result > 0)
            {
                int ExmainStatus = 0;
                if (ReqDTO.OptType == 2)
                {
                    Dal.LabelTask.LB_Task.Instance.UpdateTaskAssignStatus(ReqDTO.TaskID, (int)EnumTaskStatus.已审);
                    Dal.LabelTask.LB_Task.Instance.UpdateTaskAuditTime(ReqDTO.TaskID, UserID);
                    Dal.LabelTask.LB_Task.Instance.UpdateTaskStatus(ReqDTO.TaskID, (int)EnumTaskStatus.已审, ReqDTO.KeyWord, ReqDTO.Summary);
                    int projectID = Dal.LabelTask.LB_Task.Instance.SelectProjectID(ReqDTO.TaskID);
                    int NoTaskCount = Dal.LabelTask.LB_Task.Instance.SelectTaskCount(projectID);
                    int PStatus = 0;
                    if (NoTaskCount > 0)
                    {
                        PStatus = (int)EnumProjectStatus.执行中;
                    }
                    else
                    {
                        PStatus = (int)EnumProjectStatus.执行完毕;
                    }
                    Dal.LabelTask.LB_Task.Instance.UpdateProjectStatus(projectID, PStatus);
                    ExmainStatus = (int)EnumTaskOptType.审核;
                }
                else
                {
                    ExmainStatus = (int)EnumTaskOptType.修改;
                    Dal.LabelTask.LB_Task.Instance.UpdateTaskKeyAndSummary(ReqDTO.TaskID, ReqDTO.KeyWord, ReqDTO.Summary);
                }
                Dal.LabelTask.LB_Task.Instance.InsertTaskOperateInfo(ReqDTO.TaskID, UserID, ExmainStatus, JsonConvert.SerializeObject(ReqDTO));
                InsertTitleBasicInfo(ReqDTO.IPInfo);
                return result;
            }
            else
            {
                if (ReqDTO.OptType == 2)
                {
                    Messege = "审核失败";
                }
                else
                {
                    Messege = "修改失败";
                }
                return -1;
            }
        }
        /// <summary>
        /// zlb 2017-08-28
        /// 查询标签信息
        /// </summary>
        /// <param name="LableType">标签类型</param>
        /// <returns></returns>
        public DataTable SelectLableInfo(int LableType)
        {
            return Dal.LabelTask.LB_Task.Instance.SelectLableInfo(LableType);
        }
        /// <summary>
        /// zlb 2017-08-29
        /// 查询子Ip信息
        /// </summary>
        /// <param name="IpId">父IpId</param>
        /// <returns></returns>
        public DataTable SelectSonIpInfo(int IpId)
        {
            return Dal.LabelTask.LB_Task.Instance.SelectSonIpInfo(IpId);
        }
        public void InsertTitleBasicInfo(List<LableIP> ListIp)
        {
            StringBuilder sb = new StringBuilder();
            List<string> listCustomLable = new List<string>();
            foreach (var itemIp in ListIp)
            {
                //item.DictId
                foreach (var itemSonIp in itemIp.SonIP)
                {
                    foreach (var itemCustomLable in itemSonIp.CustomLableInfo)
                    {
                        if (!listCustomLable.Contains(itemCustomLable.DictName))
                        {
                            listCustomLable.Add(itemCustomLable.DictName);
                            sb.Append("'" + itemCustomLable.DictName + "',");
                        }
                    }
                }
            }
            string listLableName = sb.ToString();
            listLableName = listLableName.Substring(0, listLableName.Length - 1);
            DataTable dt = Dal.LabelTask.LB_Task.Instance.SelectTitleBasicInfo(listLableName);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (listCustomLable.Contains(dt.Rows[i]["Name"].ToString()))
                    {
                        listCustomLable.Remove(dt.Rows[i]["Name"].ToString());
                    }
                }
            }
            int userID = Common.UserInfo.GetLoginUserID();
            if (listCustomLable.Count > 0)
            {
                Dal.LabelTask.LB_Task.Instance.InsertTitleBasicInfo(listCustomLable, userID);
            }
            foreach (var itemIp in ListIp)
            {
                foreach (var itemSonIp in itemIp.SonIP)
                {
                    StringBuilder sbLableName = new StringBuilder();
                    foreach (var itemCustomLable in itemSonIp.CustomLableInfo)
                    {
                        sbLableName.Append("'" + itemCustomLable.DictName + "',");
                    }
                    listLableName = sbLableName.ToString();
                    listLableName = listLableName.Substring(0, listLableName.Length - 1);
                    Dal.LabelTask.LB_Task.Instance.DeleteIPTitleInfo(itemIp.DictId, itemSonIp.DictId, listLableName);
                    DataTable dtLableInfo = Dal.LabelTask.LB_Task.Instance.SelectTitleBasicInfo(listLableName);
                    if (dtLableInfo != null && dtLableInfo.Rows.Count > 0)
                    {
                        Dal.LabelTask.LB_Task.Instance.InsertIPTitleInfo(itemIp.DictId, itemSonIp.DictId, userID, dtLableInfo);
                    }
                }
            }
        }
    }
}