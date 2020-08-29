using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    /// <summary>
    /// EditProject 的摘要说明
    /// </summary>
    public class EditProject : IHttpHandler, IRequiresSessionState
    {
        private string ProjectID
        {
            get
            {
                return HttpContext.Current.Request["projectid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
            }
        }
        /// <summary>
        /// 选择的CRM客户IDs
        /// </summary>
        private string CrmSelectIDs
        {
            get
            {
                return HttpContext.Current.Request["CrmSelectIDs"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CrmSelectIDs"].ToString());
            }
        }

        public string DataStr
        {
            get
            {
                return HttpContext.Current.Request["data"] == null ? string.Empty :
                     HttpUtility.UrlDecode(HttpContext.Current.Request["data"].ToString());
            }

        }

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            string msg = "";
            int userID = 0;
            int retProjectID = 0;

            if (BLL.Util.CheckButtonRight("SYS024BUT500601") && BLL.Util.CheckButtonRight("SYS024BUT500602"))
            {
                userID = BitAuto.ISDC.CC2012.BLL.Util.GetLoginUserID();
                Submit(out msg, userID, out retProjectID);
            }
            else
            {
                msg += "您没有添加和编辑项目的权限！";
            }

            if (msg == "")
            {
                msg = "success_" + retProjectID.ToString();
            }
            context.Response.Write(msg);
        }

        private void Submit(out string msg, int userID, out int retProjectID)
        {
            msg = "";
            retProjectID = 0;
            string datainfoStr = DataStr;
            ProjectInfo sInfoData = null;
            sInfoData = (ProjectInfo)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(datainfoStr, typeof(ProjectInfo));

            Entities.ProjectInfo projectModel;
            Entities.ProjectDataSoure dsModel;
            List<Entities.ProjectDataSoure> dslist = new List<Entities.ProjectDataSoure>();
            List<Entities.ProjectDataSoure> AddDatalist = new List<Entities.ProjectDataSoure>();
            Entities.ProjectSurveyMapping psMap;
            List<Entities.ProjectSurveyMapping> psmaplist = new List<Entities.ProjectSurveyMapping>();
            List<Entities.ProjectSurveyMapping> oldMaplist = new List<ProjectSurveyMapping>();
            List<Entities.ProjectSurveyMapping> newMaplist = new List<ProjectSurveyMapping>();

            List<int> delIds = new List<int>();

            int sourceid = int.Parse(sInfoData.Source);
            int oldSourceId = -1;//原来的来源
            string oldTTCode = "";//原来的TTcode
            //add by qizq 2014-11-24 加原bgID,scid
            int oldbgid = 0;
            int oldpcatageid = 0;

            #region 判断项目名称是否重复
            if (ProjectID == "")
            {
                DataTable dt = BLL.ProjectInfo.Instance.GetDataByName(sInfoData.txtProjectName);
                if (dt.Rows.Count > 0)
                {
                    msg += "已经存在名为【" + sInfoData.txtProjectName + "】的项目！";
                    return;
                }
            }
            #endregion

            #region 项目关联数据
            string[] ids = null;
            //crm来源数据
            if (CrmSelectIDs != "")
            {
                ids = CrmSelectIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            //OTH 来源数据=优先级高
            if (sInfoData.hidExportSelectIDs != "")
            {
                ids = sInfoData.hidExportSelectIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            //hidExportAddIDs：OTH 追加数据
            //hidCrmAddIDs：CRM 追加数据

            //校验4个数据是否都为空
            if (sInfoData.hidExportSelectIDs == "" && sInfoData.hidExportAddIDs == "" && CrmSelectIDs == "" && sInfoData.hidCrmAddIDs == "")
            {
                msg += "没有选择关联数据";
                return;
            }

            //选择的数据 (来源：CRM或者OTH)
            foreach (string id in ids)
            {
                dsModel = new Entities.ProjectDataSoure();
                dsModel.ProjectID = -2;
                dsModel.RelationID = id;
                dsModel.Source = sourceid;
                dsModel.Status = 0;//0未生产任务
                dsModel.CreateTime = DateTime.Now;
                dsModel.CreateUserID = userID;

                dslist.Add(dsModel);
            }

            //OTH追加数据
            if (sInfoData.hidExportAddIDs != "")
            {
                foreach (string id in sInfoData.hidExportAddIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dsModel = new Entities.ProjectDataSoure();
                    dsModel.ProjectID = -2;
                    dsModel.RelationID = id;
                    dsModel.Source = sourceid;
                    dsModel.Status = 0;//0未生产任务
                    dsModel.CreateTime = DateTime.Now;
                    dsModel.CreateUserID = userID;
                    AddDatalist.Add(dsModel);
                }
            }

            #endregion

            if (ProjectID == "")
            {
                //新增
                #region 项目信息实体
                projectModel = new Entities.ProjectInfo();
                projectModel.BGID = int.Parse(sInfoData.sltUserGroup);
                projectModel.PCatageID = int.Parse(sInfoData.sltSurveyCategory);
                projectModel.Name = sInfoData.txtProjectName;
                projectModel.Notes = sInfoData.txtDescription;
                projectModel.Source = sourceid;
                projectModel.Status = 0;
                projectModel.CreateTime = DateTime.Now;
                projectModel.CreateUserID = userID;
                projectModel.TTCode = sInfoData.TTCode;
                projectModel.IsBlacklistCheck = sInfoData.IsBlacklistCheck;
                projectModel.BlacklistCheckType = sInfoData.BlackListCheckType == 0 ? null : (int?)sInfoData.BlackListCheckType;
                #endregion

                #region 项目管理问卷
                DateTime nowDt = DateTime.Now;
                if (sInfoData.SurveyList != null)
                {
                    int i = 0;
                    foreach (SurveryInfo item in sInfoData.SurveyList)
                    {
                        psMap = new Entities.ProjectSurveyMapping();
                        psMap.ProjectID = item.ProjectID == "" ? -2 : int.Parse(item.ProjectID);
                        psMap.SIID = int.Parse(item.hdnSIID);
                        psMap.BeginDate = item.beginTime + " 00:00:00";
                        psMap.EndDate = item.endTime + " 23:59:59";
                        psMap.Status = 0;//默认是0
                        psMap.CreateTime = nowDt.AddSeconds((i++) * 2);
                        psMap.CreateUserID = userID;

                        psmaplist.Add(psMap);
                    }
                }

                #endregion
            }
            else
            {
                projectModel = BLL.ProjectInfo.Instance.GetProjectInfo(int.Parse(ProjectID));
                if (projectModel == null)
                {
                    msg += "没有找到对应的项目";
                }
                else
                {
                    oldSourceId = (int)projectModel.Source;
                    retProjectID = (int)projectModel.ProjectID;
                    oldTTCode = projectModel.TTCode;

                    //add by qizq 2014-11-28
                    oldbgid = (int)projectModel.BGID;
                    oldpcatageid = (int)projectModel.PCatageID;

                    #region 编辑项目
                    projectModel.BGID = int.Parse(sInfoData.sltUserGroup);
                    projectModel.PCatageID = int.Parse(sInfoData.sltSurveyCategory);
                    projectModel.Name = sInfoData.txtProjectName;
                    projectModel.Notes = sInfoData.txtDescription;
                    projectModel.Source = sourceid;
                    projectModel.TTCode = sInfoData.TTCode;
                    #endregion

                    #region 编辑问卷

                    Entities.QueryProjectSurveyMapping query = new QueryProjectSurveyMapping();
                    query.ProjectID = int.Parse(ProjectID);

                    int totalCount = 0;
                    oldMaplist = BLL.ProjectSurveyMapping.Instance.GetProjectSurveyMappingList(query, out totalCount);
                    newMaplist = new List<ProjectSurveyMapping>();

                    DateTime nowDt = DateTime.Now;
                    int i = 0;
                    foreach (SurveryInfo surinfo in sInfoData.SurveyList)
                    {
                        //查找
                        ProjectSurveyMapping mapModel = oldMaplist.Find(
                              delegate(ProjectSurveyMapping map)
                              {
                                  return map.SIID == int.Parse(surinfo.hdnSIID);
                              }
                              );

                        if (mapModel != null)
                        {
                            //找到了,修改日期

                            mapModel.BeginDate = surinfo.beginTime + " 00:00:00"; ;
                            mapModel.EndDate = surinfo.endTime + " 23:59:59";

                            newMaplist.Add(mapModel);
                        }
                        else
                        {
                            //没找到，就是新加的

                            psMap = new ProjectSurveyMapping();
                            psMap.ProjectID = -2;
                            psMap.SIID = int.Parse(surinfo.hdnSIID);
                            psMap.BeginDate = surinfo.beginTime + " 00:00:00"; ;
                            psMap.EndDate = surinfo.endTime + " 23:59:59";
                            psMap.Status = 0;//默认是0
                            psMap.CreateTime = nowDt.AddSeconds((i++) * 2);
                            psMap.CreateUserID = userID;

                            newMaplist.Add(psMap);

                        }

                    }

                    //查找删除的
                    foreach (Entities.ProjectSurveyMapping item in oldMaplist)
                    {
                        int isExist = 0;
                        //查找
                        foreach (SurveryInfo surinfo in sInfoData.SurveyList)
                        {
                            if (item.SIID.ToString() == surinfo.hdnSIID)
                            {
                                isExist = 1;
                                break;
                            }
                        }
                        if (isExist == 0)
                        {
                            //没找到，就是删除了

                            delIds.Add(item.SIID);

                        }
                    }
                    #endregion
                }
            }

            #region 提交到数据库
            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("SampleTransaction");

            try
            {

                if (ProjectID == "")
                {
                    //新增
                    #region 新增项目
                    retProjectID = BLL.ProjectInfo.Instance.Insert(tran, projectModel);
                    BLL.ProjectLog.Instance.InsertProjectLog(retProjectID, ProjectLogOper.L1_新建项目, "新建项目-" + projectModel.Name, tran);
                    #endregion

                    #region 新增数据
                    foreach (Entities.ProjectDataSoure pds in dslist)
                    {
                        pds.ProjectID = retProjectID;
                        BLL.ProjectDataSoure.Instance.Insert(tran, pds);
                    }
                    //add by qizq 2014-11-24 加入数据导入日志
                    Entities.ProjectImportHistory pih = new ProjectImportHistory();
                    pih.ProjectID = retProjectID;
                    pih.ImportNumber = dslist.Count;
                    pih.CreateTime = System.DateTime.Now;
                    pih.CreateUserID = userID;
                    BLL.ProjectImportHistory.Instance.Insert(tran, pih);
                    #endregion

                    #region 新增问卷
                    foreach (Entities.ProjectSurveyMapping item in psmaplist)
                    {
                        item.ProjectID = retProjectID;
                        BLL.ProjectSurveyMapping.Instance.Insert(tran, item);
                    }
                    BLL.ProjectLog.Instance.InsertProjectLog(retProjectID, ProjectLogOper.L5_修改问卷, "新增问卷" + psmaplist.Count + "条", tran);
                    BLL.ProjectLog.Instance.InsertProjectLog(retProjectID, ProjectLogOper.L3_导入数据, "导入数据" + pih.ImportNumber + "条", tran);
                    #endregion
                }
                else
                {
                    #region 编辑项目
                    BLL.ProjectInfo.Instance.Update(tran, projectModel);
                    BLL.ProjectLog.Instance.InsertProjectLog(projectModel.ProjectID, ProjectLogOper.L2_编辑项目, "编辑项目-" + projectModel.Name, tran);
                    #endregion

                    #region 修改关联数据
                    if (projectModel.Status == 0)
                    {
                        //如果未生成任务，选择的数据时先删除、后插入
                        //modify by qizq 2014-11-28 取出所有关联数据，与前台传过来的数据做比较，如果不相同先删除后插入，如果相同不做修改
                        //如果分组，分类，涞源，或对应的模板变了就全部删除，然后插入
                        bool flag = true;
                        if (oldbgid != projectModel.BGID || oldpcatageid != projectModel.PCatageID || oldSourceId != projectModel.Source || oldTTCode != projectModel.TTCode.ToString())
                        {
                            flag = false;
                        }
                        //否则取出所有关联数据做比较，如果发生变化先全部删除后插入
                        else
                        {
                            flag = IsSameProjectDataSource(projectModel.ProjectID, dslist);
                        }
                        if (flag == false)
                        {
                            //删除原来的关联数据
                            BLL.ProjectDataSoure.Instance.DeleteByProjectID(tran, (int)projectModel.ProjectID);
                            //删除日志信息
                            BLL.ProjectImportHistory.Instance.Delete(tran, Convert.ToInt32(projectModel.ProjectID));

                            //插入新的关联数据
                            foreach (Entities.ProjectDataSoure pds in dslist)
                            {
                                pds.ProjectID = projectModel.ProjectID;
                                BLL.ProjectDataSoure.Instance.Insert(tran, pds);
                            }
                            // 添加插入数据日志
                            Entities.ProjectImportHistory pihadd = new ProjectImportHistory();
                            pihadd.ProjectID = Convert.ToInt32(projectModel.ProjectID);
                            pihadd.ImportNumber = dslist.Count;
                            pihadd.CreateTime = System.DateTime.Now;
                            pihadd.CreateUserID = userID;
                            BLL.ProjectImportHistory.Instance.Insert(tran, pihadd);

                            BLL.ProjectLog.Instance.InsertProjectLog(projectModel.ProjectID, ProjectLogOper.L3_导入数据, "覆盖数据" + pihadd.ImportNumber + "条", tran);
                        }
                    }
                    else
                    {
                        //如果已生成任务，不用管原来的DataSource
                    }

                    //补充的数据
                    foreach (Entities.ProjectDataSoure pds in AddDatalist)
                    {
                        pds.ProjectID = projectModel.ProjectID;
                        BLL.ProjectDataSoure.Instance.Insert(tran, pds);
                    }
                    //add by qizq 2014-11-24,当补充数据不为空，加补充数据导入日志
                    if (AddDatalist != null && AddDatalist.Count > 0)
                    {
                        Entities.ProjectImportHistory pihadd = new ProjectImportHistory();
                        pihadd.ProjectID = retProjectID;
                        pihadd.ImportNumber = AddDatalist.Count;
                        pihadd.CreateTime = System.DateTime.Now;
                        pihadd.CreateUserID = userID;
                        BLL.ProjectImportHistory.Instance.Insert(tran, pihadd);

                        BLL.ProjectLog.Instance.InsertProjectLog(retProjectID, ProjectLogOper.L3_导入数据, "追加数据" + pihadd.ImportNumber + "条", tran);
                    }

                    //补充客户回访数据  sInfoData.hidCrmAddIDs
                    if (sInfoData.hidCrmAddIDs != "")
                    {
                        Entities.ProjectDataSoure model = new Entities.ProjectDataSoure();
                        model.ProjectID = projectModel.ProjectID;
                        model.Source = projectModel.Source.Value;
                        model.Status = projectModel.Status;
                        model.CreateTime = DateTime.Now;
                        model.CreateUserID = userID;

                        int returnval = 0;
                        string strrelationIds = "";
                        int addNum = 0;
                        foreach (string relationid in sInfoData.hidCrmAddIDs.Split(','))
                        {
                            if (!string.IsNullOrEmpty(relationid))
                            {
                                model.RelationID = relationid;
                                returnval = BLL.ProjectDataSoure.Instance.Insert(tran, model);
                                if (returnval > 0)
                                {
                                    strrelationIds += "," + relationid;
                                    addNum++;
                                }
                            }
                        }
                        if (strrelationIds.Length > 0)
                        {
                            Entities.ProjectImportHistory pihadd = new ProjectImportHistory();
                            pihadd.ProjectID = Convert.ToInt32(model.ProjectID);
                            pihadd.ImportNumber = addNum;
                            pihadd.CreateTime = System.DateTime.Now;
                            pihadd.CreateUserID = userID;
                            BLL.ProjectImportHistory.Instance.Insert(tran, pihadd);
                            BLL.ProjectLog.Instance.InsertProjectLog(model.ProjectID, ProjectLogOper.L3_导入数据, "追加数据" + pihadd.ImportNumber + "条", tran);
                        }
                    }

                    #endregion

                    #region 编辑或删除问卷
                    int add = 0, mod = 0, del = 0;
                    //删除的问卷
                    foreach (int id in delIds)
                    {
                        BLL.ProjectSurveyMapping.Instance.Delete(long.Parse(ProjectID), id);
                        del++;
                    }
                    for (int i = newMaplist.Count - 1; i >= 0; i--)
                    {
                        if (newMaplist[i].ProjectID != -2)
                        {
                            //编辑了问卷
                            BLL.ProjectSurveyMapping.Instance.Update(tran, newMaplist[i]);
                            mod++;
                        }
                        else
                        {
                            //新增的问卷
                            newMaplist[i].ProjectID = int.Parse(ProjectID);
                            BLL.ProjectSurveyMapping.Instance.Insert(tran, newMaplist[i]);
                            add++;
                        }
                    }
                    if (add + del > 0)
                    {
                        string info = "";
                        if (add > 0)
                        {
                            info += "新增问卷" + add + "条；";
                        }
                        if (del > 0)
                        {
                            info += "删除问卷" + del + "条；";
                        }
                        BLL.ProjectLog.Instance.InsertProjectLog(projectModel.ProjectID, ProjectLogOper.L5_修改问卷, info, tran);
                    }
                    #endregion
                }
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
        /// <summary>
        /// 根据项目id取项目关联数据id，与dslist里id进行比较不同则返回false
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="dslist"></param>
        /// <returns></returns>
        private bool IsSameProjectDataSource(long projectID, List<ProjectDataSoure> dslist)
        {
            //默认相等
            bool flag = true;
            int rowscount = 0;
            Entities.QueryProjectDataSoure query = new QueryProjectDataSoure();
            query.ProjectID = projectID;
            DataTable dt = BLL.ProjectDataSoure.Instance.GetProjectDataSoure(query, "", 1, 100000, out rowscount);
            if (dt != null && dt.Rows.Count > 0)
            {
                //如果个数不相等，false;
                if (dt.Rows.Count == dslist.Count)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string _relationid = dt.Rows[i]["RelationID"].ToString();
                        int sameindex = dslist.Count;
                        for (int n = 0; n < dslist.Count; n++)
                        {

                            if (_relationid == dslist[n].RelationID)
                            {
                                sameindex = n;
                                break;
                            }
                        }
                        if (sameindex == dslist.Count)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                else
                {
                    flag = false;
                }

            }
            else
            {
                flag = false;
            }

            return flag;

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