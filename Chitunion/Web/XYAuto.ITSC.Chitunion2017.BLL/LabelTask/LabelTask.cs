using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.LabelTask.DTO;

namespace XYAuto.ITSC.Chitunion2017.BLL.LabelTask
{
    public class LabelTask
    {
        #region 当前登录人UserID
        private int _currentUserID;
        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = Chitunion2017.Common.UserInfo.GetLoginUserID();
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
        public static readonly LabelTask Instance = new LabelTask();
        #region 查看标签任务审核人列表
        public List<ResponseAuditUserDTO> GetAuditUser()
        {
            return Dal.LabelTask.LabelTask.Instance.GetAuditUser();
        }
        #endregion
        #region 提交项目相关
        public void LabelProjectCreate(ReqProjectCreateDTO reqDto, out int projectID, out string msg)
        {
            Loger.Log4Net.Info("[LabelProjectCreate]提交项目-----方法开始");
            projectID = -2;
            msg = string.Empty;

            if (!reqDto.CheckSelfModel(out msg))
                return;

            //string filePath = string.Empty;
            //string _uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
            //filePath = $"{_uploadFilePath}/UploadFiles/上传媒体账号查模板.xls";
            //if (!File.Exists(filePath))
            //{
            //    msg = "上传媒体帐号文件不存在!";
            //    Loger.Log4Net.Info($"[LabelProjectCreate]提交项目-----{msg}");
            //    return;
            //}

            //DataSet ds = ConvertExcelToDataSet(new string[] { "APP", "头条", "微信", "微博", "视频", "直播" }, filePath);
            //if (ds == null || ds.Tables.Count == 0)
            //{
            //    msg = "上传媒体帐号文件sheet不存在!";
            //    Loger.Log4Net.Info($"[LabelProjectCreate]提交项目-----{msg}");
            //    return;
            //}
            DataSet ds = null;
            msg += ValidateExcel(out ds, reqDto.UploadFileURL);
            if (!string.IsNullOrEmpty(msg))
                return;

            if (reqDto.ProjectType == (int)Entities.LabelTask.ENUM.EnumProjectType.媒体)
                ImportMedia(reqDto, ds, out msg);
            else if (reqDto.ProjectType == (int)Entities.LabelTask.ENUM.EnumProjectType.文章)
                ImportArticle(reqDto, ds, out msg);
        }
        public DataSet ConvertExcelToDataSet(string[] sheetNames,string excelPath)
        {
            DataSet ds = new DataSet();
            //HSSFWorkbook hssfworkbook;//只能讯取2007以前的
            IWorkbook hssfworkbook;
            using (FileStream file = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //hssfworkbook = new HSSFWorkbook(file);
                hssfworkbook = WorkbookFactory.Create(file);
            }
            for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
            {
                #region 遍历Sheet
                ISheet sheet = hssfworkbook.GetSheetAt(i);
                if (!sheetNames.Contains(sheet.SheetName))
                    continue;
                DataTable dt = new DataTable();
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    dt.Columns.Add(cell == null ? string.Empty : cell.ToString().Trim());
                }
                for (int j = (sheet.FirstRowNum + 1); j <= sheet.LastRowNum; j++)
                {
                    IRow row = sheet.GetRow(j);
                    if (row == null)
                        continue;
                    DataRow dataRow = dt.NewRow();

                    for (int k = row.FirstCellNum; k < cellCount; k++)
                    {
                        if (row.GetCell(k) != null)
                            dataRow[k] = row.GetCell(k).ToString().Trim();
                    }
                    dt.Rows.Add(dataRow);
                }
                dt.TableName = sheet.SheetName;
                ds.Tables.Add(dt);
                #endregion
            }
            return ds;
        }
        public void ImportMedia(ReqProjectCreateDTO reqDto,DataSet ds,out string msg)
        {
            Entities.LabelTask.LB_Project modelProject = new Entities.LabelTask.LB_Project() {
                Name=reqDto.Name,
                ProjectType=reqDto.ProjectType,
                TaskCount=reqDto.TaskCount,
                Status=(int)Entities.LabelTask.ENUM.EnumProjectStatus.待执行,
                UploadFileURL=reqDto.UploadFileURL,
                CreateUserID=currentUserID
            };
            int projectID = Dal.LabelTask.LB_Project.Instance.Insert(modelProject);
            int geTaskCount = 0;
            msg = string.Empty;
            List<Entities.LabelTask.LB_TaskModel> listTask = new List<Entities.LabelTask.LB_TaskModel>();
            DataView dataView = ds.Tables["微信"].DefaultView;
            DataTable dt_mediaNum = dataView.ToTable(true, "账号", "名称");
            #region 微信
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["账号"].ToString().Trim();
                #region 代码中验证
                int mediaID = Dal.LabelTask.LabelTask.Instance.GetWxIDByWxNumber(mediaNum);
                if (mediaID == 0)
                {
                    Loger.Log4Net.Info($"[ImportMedia]微信帐号:{mediaNum},在基表中不存在");
                    continue;
                }

                if (Dal.LabelTask.LabelTask.Instance.IsGeneratedTaskByMediaNum(mediaNum, (int)Entities.LabelTask.ENUM.EnumMediaType.微信))
                {
                    Loger.Log4Net.Info($"[ImportMedia]微信帐号:{mediaNum},已经生成标签任务");
                    continue;
                }
                #endregion

                listTask.Add(new Entities.LabelTask.LB_TaskModel()
                {
                    ProjectID = projectID,
                    MediaType = (int)Entities.LabelTask.ENUM.EnumMediaType.微信,
                    MediaID = mediaID,
                    MediaName = dr["名称"].ToString().Trim(),
                    MediaNum = mediaNum,
                    Status = (int)Entities.LabelTask.ENUM.EnumTaskStatus.待领,
                    CreateUserID = currentUserID,
                    CreateTime = DateTime.Now
                });
                geTaskCount++;
                if (geTaskCount >= reqDto.TaskCount)
                    break;
            }
            #endregion
            #region 头条
            dataView = ds.Tables["头条"].DefaultView;
            dt_mediaNum = dataView.ToTable(true, "账号", "名称");
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["账号"].ToString().Trim();
                #region 代码中验证                
                if (Dal.LabelTask.LabelTask.Instance.IsGeneratedTaskByMediaNum(mediaNum, (int)Entities.LabelTask.ENUM.EnumMediaType.头条))
                {
                    Loger.Log4Net.Info($"[ImportMedia]头条帐号:{mediaNum},已经生成标签任务");
                    continue;
                }
                #endregion

                listTask.Add(new Entities.LabelTask.LB_TaskModel()
                {
                    ProjectID = projectID,
                    MediaType = (int)Entities.LabelTask.ENUM.EnumMediaType.头条,
                    MediaNum = mediaNum,
                    MediaName = dr["名称"].ToString().Trim(),
                    Status = (int)Entities.LabelTask.ENUM.EnumTaskStatus.待领,
                    CreateUserID = currentUserID,
                    CreateTime = DateTime.Now
                });
                geTaskCount++;
                if (geTaskCount >= reqDto.TaskCount)
                    break;
            }
            #endregion
            #region APP
            dataView = ds.Tables["APP"].DefaultView;
            dt_mediaNum = dataView.ToTable(true, "名称");
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["名称"].ToString().Trim();
                #region 代码中验证   
                int mediaID = Dal.LabelTask.LabelTask.Instance.GetMediaIDByAPPName(mediaNum);
                if (mediaID == 0)
                {
                    Loger.Log4Net.Info($"[ImportMedia]APP:{mediaNum},在基表中不存在");
                    continue;
                }
                if (Dal.LabelTask.LabelTask.Instance.IsGeneratedTaskByMediaNum(mediaNum, (int)Entities.LabelTask.ENUM.EnumMediaType.APP))
                {
                    Loger.Log4Net.Info($"[ImportMedia]APP:{mediaNum},已经生成标签任务");
                    continue;
                }
                #endregion

                listTask.Add(new Entities.LabelTask.LB_TaskModel()
                {
                    ProjectID = projectID,
                    MediaType = (int)Entities.LabelTask.ENUM.EnumMediaType.APP,
                    MediaID = mediaID,
                    MediaNum = mediaNum,
                    MediaName = mediaNum,
                    Status = (int)Entities.LabelTask.ENUM.EnumTaskStatus.待领,
                    CreateUserID = currentUserID,
                    CreateTime = DateTime.Now
                });
                geTaskCount++;
                if (geTaskCount >= reqDto.TaskCount)
                    break;
            }
            #endregion
            #region 微博
            dataView = ds.Tables["微博"].DefaultView;
            dt_mediaNum = dataView.ToTable(true, "账号", "名称");
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["账号"].ToString().Trim();
                #region 代码中验证                
                if (Dal.LabelTask.LabelTask.Instance.IsGeneratedTaskByMediaNum(mediaNum, (int)Entities.LabelTask.ENUM.EnumMediaType.微博))
                {
                    Loger.Log4Net.Info($"[ImportMedia]微博:{mediaNum},已经生成标签任务");
                    continue;
                }
                #endregion

                listTask.Add(new Entities.LabelTask.LB_TaskModel()
                {
                    ProjectID = projectID,
                    MediaType = (int)Entities.LabelTask.ENUM.EnumMediaType.微博,
                    MediaNum = mediaNum,
                    MediaName = dr["名称"].ToString().Trim(),
                    Status = (int)Entities.LabelTask.ENUM.EnumTaskStatus.待领,
                    CreateUserID = currentUserID,
                    CreateTime = DateTime.Now
                });
                geTaskCount++;
                if (geTaskCount >= reqDto.TaskCount)
                    break;
            }
            #endregion

            if (listTask.Count > 0)
            {
                if (listTask.Count >= reqDto.TaskCount)
                {
                    geTaskCount = reqDto.TaskCount;
                    listTask = listTask.Take(reqDto.TaskCount).ToList();
                }
                Dal.LabelTask.LB_Task.Instance.SyncData(BLL.Util.ListToDataTable<Entities.LabelTask.LB_TaskModel>(listTask), "LB_Task", out msg);
            }                
            else
            {
                msg = "任务数为0不能生成项目";
                Dal.LabelTask.LB_Project.Instance.Delete(projectID);
            }
                
            Dal.LabelTask.LB_Project.Instance.UpdateGeTaskCount(projectID, geTaskCount);
        }
        public void ImportArticle(ReqProjectCreateDTO reqDto, DataSet ds, out string msg)
        {
            Entities.LabelTask.LB_Project modelProject = new Entities.LabelTask.LB_Project()
            {
                Name = reqDto.Name,
                ProjectType = reqDto.ProjectType,
                TaskCount = reqDto.TaskCount,
                Status = (int)Entities.LabelTask.ENUM.EnumProjectStatus.待执行,
                UploadFileURL = reqDto.UploadFileURL,
                CreateUserID = currentUserID
            };
            int projectID = Dal.LabelTask.LB_Project.Instance.Insert(modelProject);
            int geTaskCount = 0;
            msg = string.Empty;
            DataView dataView = ds.Tables["微信"].DefaultView;
            DataTable dt_mediaNum = dataView.ToTable(true, "账号", "名称");
            #region 微信
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["账号"].ToString().Trim();
                Entities.LabelTask.LB_Task modelTask = new Entities.LabelTask.LB_Task()
                {
                    ProjectID = projectID,
                    MediaType = (int)Entities.LabelTask.ENUM.EnumMediaType.微信,
                    MediaNum = mediaNum,
                    MediaName = dr["名称"].ToString().Trim(),
                    CreateUserID = currentUserID,
                    TaskCount = reqDto.TaskCount - geTaskCount
                };
                int tcount = Dal.LabelTask.LB_Task.Instance.Insert(modelTask, out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    Loger.Log4Net.Info($"[ImportArticle]微信帐号:{mediaNum},生成文章类型任务出错：{msg}");
                    msg = string.Empty;
                    continue;
                }
                geTaskCount += tcount;
                if (geTaskCount >= reqDto.TaskCount)
                    break;
            }
            if (geTaskCount >= reqDto.TaskCount)
            {
                Dal.LabelTask.LB_Project.Instance.UpdateGeTaskCount(projectID, geTaskCount);
                return;
            }
            
            #endregion
            #region 头条
            dataView = ds.Tables["头条"].DefaultView;
            dt_mediaNum = dataView.ToTable(true, "账号", "名称");
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["账号"].ToString().Trim();
                Entities.LabelTask.LB_Task modelTask = new Entities.LabelTask.LB_Task()
                {
                    ProjectID = projectID,
                    MediaType = (int)Entities.LabelTask.ENUM.EnumMediaType.头条,
                    MediaNum = mediaNum,
                    MediaName = dr["名称"].ToString().Trim(),
                    CreateUserID = currentUserID,
                    TaskCount = reqDto.TaskCount - geTaskCount
                };
                int tcount = Dal.LabelTask.LB_Task.Instance.Insert(modelTask, out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    Loger.Log4Net.Info($"[ImportArticle]头条:{mediaNum},生成文章类型任务出错：{msg}");
                    msg = string.Empty;
                    continue;
                }
                geTaskCount += tcount;
                if (geTaskCount >= reqDto.TaskCount)
                    break;
            }
            #endregion

            if (geTaskCount == 0)
            {
                msg = "任务数为0不能生成项目";
                Dal.LabelTask.LB_Project.Instance.Delete(projectID);
            }
            else
                Dal.LabelTask.LB_Project.Instance.UpdateGeTaskCount(projectID, geTaskCount);
        }
        #endregion
        #region 任务统计
        public void LabelStatistics(int projectType,string uploadFileURL,out string msg,out int taskCount)
        {
            Loger.Log4Net.Info("[LabelStatistics]任务统计-----方法开始");
            taskCount = 0;
            msg = string.Empty;
            if (!Enum.IsDefined(typeof(Entities.LabelTask.ENUM.EnumProjectType), projectType))
                msg = "参数项目类型错误/n";

            if(string.IsNullOrEmpty(uploadFileURL))
                msg = "上传媒体帐号文件URL必填/n";

            //"http://www.chitunion.com/UploadFiles/2017/8/10/10/媒体账号$401e2534-cae8-42a6-bf88-88bb6be8a181.xlsx"

            DataSet ds = null;            
            msg += ValidateExcel(out ds, uploadFileURL);
            if (!string.IsNullOrEmpty(msg))
                return;

            if (projectType == (int)Entities.LabelTask.ENUM.EnumProjectType.文章)
            {
                #region 文章
                Tuple<int, int> tpArticle = StatisticsArticleCount(ds);
                taskCount = tpArticle.Item1 + tpArticle.Item2;
                #endregion
            }
            else if (projectType == (int)Entities.LabelTask.ENUM.EnumProjectType.媒体)
            {
                #region 媒体帐号
                Tuple<int, int, int, int> tpMediaNum = StatisticsMediaNumCount(ds);
                taskCount = tpMediaNum.Item1 + tpMediaNum.Item2 + tpMediaNum.Item3 + tpMediaNum.Item4;
                #endregion
            }
        }
        public string ValidateExcel(out DataSet ds,string uploadFileURL)
        {
            //"http://www.chitunion.com/UploadFiles/2017/8/10/10/媒体账号$401e2534-cae8-42a6-bf88-88bb6be8a181.xlsx"
            uploadFileURL = uploadFileURL.Replace("http://www.chitunion.com", "");
            string msg = string.Empty;
            string filePath = string.Empty;
            string _uploadFilePath = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false);
            //filePath = $"{_uploadFilePath}/UploadFiles/上传媒体账号查模板.xls";
            filePath = $"{_uploadFilePath}{uploadFileURL}";
            Loger.Log4Net.Info($"[LabelStatistics]任务统计-----filePath：{filePath}");
            if (!File.Exists(filePath))
                msg += "上传媒体帐号文件不存在!/n";

            ds = ConvertExcelToDataSet(new string[] { "APP", "头条", "微信", "微博", "视频", "直播" }, filePath);
            if (ds == null || ds.Tables.Count == 0)
                msg += "上传媒体帐号文件sheet不存在!/n";

            if (!string.IsNullOrEmpty(msg))
                Loger.Log4Net.Info($"[ValidateExcel]验证错误：{msg}");

            return msg;
        }
        public Tuple<int, int> StatisticsArticleCount(DataSet ds)
        {
            int weChatCount = 0;
            int topLineCount = 0;
            string mediaNumbers = string.Empty;
            #region 查询微信帐号
            DataView dataView = ds.Tables["微信"].DefaultView;
            DataTable dt_mediaNum = dataView.ToTable(true, "账号");
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["账号"].ToString().Trim();
                int mediaID = Dal.LabelTask.LabelTask.Instance.GetWxIDByWxNumber(mediaNum);
                if (mediaID == 0)
                {
                    Loger.Log4Net.Info($"[ImportMedia]微信帐号:{mediaNum},在基表中不存在");
                    continue;
                }
                mediaNumbers += $"'{mediaNum}',";
            }
            if (mediaNumbers.EndsWith(","))
                mediaNumbers = mediaNumbers.Substring(0, mediaNumbers.Length - 1);

            if (!string.IsNullOrEmpty(mediaNumbers))
                weChatCount = Dal.LabelTask.LabelTask.Instance.StatisticsArticleCount((int)Entities.LabelTask.ENUM.EnumMediaType.微信, (int)Entities.LabelTask.ENUM.EnumResourceType.微信, mediaNumbers);
            #endregion
            #region 查询头条帐号
            mediaNumbers = string.Empty;
            dataView = ds.Tables["头条"].DefaultView;
            dt_mediaNum = dataView.ToTable(true, "账号");
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["账号"].ToString().Trim();                
                mediaNumbers += $"'{mediaNum}',";
            }
            if (mediaNumbers.EndsWith(","))
                mediaNumbers = mediaNumbers.Substring(0, mediaNumbers.Length - 1);

            if (!string.IsNullOrEmpty(mediaNumbers))
                topLineCount = Dal.LabelTask.LabelTask.Instance.StatisticsArticleCount((int)Entities.LabelTask.ENUM.EnumMediaType.头条, (int)Entities.LabelTask.ENUM.EnumResourceType.今日头条, mediaNumbers);
            #endregion
            return new Tuple<int, int>(weChatCount, topLineCount);
        }
        public Tuple<int, int, int, int> StatisticsMediaNumCount(DataSet ds)
        {
            int weChatCount = 0;
            int topLineCount = 0;
            int appCount = 0;
            int weiboCount = 0;
            int noExistsCount = 0;
            string mediaNumbers = string.Empty;
            #region 查询微信帐号
            DataView dataView = ds.Tables["微信"].DefaultView;
            DataTable dt_mediaNum = dataView.ToTable(true, "账号");
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["账号"].ToString().Trim();
                int mediaID = Dal.LabelTask.LabelTask.Instance.GetWxIDByWxNumber(mediaNum);
                if (mediaID == 0)
                {
                    Loger.Log4Net.Info($"[ImportMedia]微信帐号:{mediaNum},在基表中不存在");
                    noExistsCount++;
                    continue;
                }
                mediaNumbers += $"'{mediaNum}',";
            }
            if (mediaNumbers.EndsWith(","))
                mediaNumbers = mediaNumbers.Substring(0, mediaNumbers.Length - 1);

            if (!string.IsNullOrEmpty(mediaNumbers))
                weChatCount = dt_mediaNum.Rows.Count - noExistsCount - Dal.LabelTask.LabelTask.Instance.StatisticsMediaNumCount((int)Entities.LabelTask.ENUM.EnumMediaType.微信, mediaNumbers);

            #endregion
            #region 查询头条帐号
            mediaNumbers = string.Empty;
            dataView = ds.Tables["头条"].DefaultView;
            dt_mediaNum = dataView.ToTable(true, "账号");
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["账号"].ToString().Trim();
                mediaNumbers += $"'{mediaNum}',";
            }
            if (mediaNumbers.EndsWith(","))
                mediaNumbers = mediaNumbers.Substring(0, mediaNumbers.Length - 1);

            if (!string.IsNullOrEmpty(mediaNumbers))
                topLineCount = dt_mediaNum.Rows.Count - Dal.LabelTask.LabelTask.Instance.StatisticsMediaNumCount((int)Entities.LabelTask.ENUM.EnumMediaType.头条, mediaNumbers);
            #endregion
            #region 查询APP帐号
            noExistsCount = 0;
            mediaNumbers = string.Empty;
            dataView = ds.Tables["APP"].DefaultView;
            dt_mediaNum = dataView.ToTable(true, "名称");
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["名称"].ToString().Trim();
                int mediaID = Dal.LabelTask.LabelTask.Instance.GetMediaIDByAPPName(mediaNum);
                if (mediaID == 0)
                {
                    Loger.Log4Net.Info($"[StatisticsMediaNumCount]APP:{mediaNum},在基表中不存在");
                    noExistsCount++;
                    continue;
                }
                mediaNumbers += $"'{mediaNum}',";
            }
            if (mediaNumbers.EndsWith(","))
                mediaNumbers = mediaNumbers.Substring(0, mediaNumbers.Length - 1);

            if (!string.IsNullOrEmpty(mediaNumbers))
                appCount = dt_mediaNum.Rows.Count - noExistsCount - Dal.LabelTask.LabelTask.Instance.StatisticsMediaNumCount((int)Entities.LabelTask.ENUM.EnumMediaType.APP, mediaNumbers);
            #endregion
            #region 查询微博帐号
            mediaNumbers = string.Empty;
            dataView = ds.Tables["微博"].DefaultView;
            dt_mediaNum = dataView.ToTable(true, "账号");
            foreach (DataRow dr in dt_mediaNum.Rows)
            {
                string mediaNum = dr["账号"].ToString().Trim();
                mediaNumbers += $"'{mediaNum}',";
            }
            if (mediaNumbers.EndsWith(","))
                mediaNumbers = mediaNumbers.Substring(0, mediaNumbers.Length - 1);

            if (!string.IsNullOrEmpty(mediaNumbers))
                weiboCount = dt_mediaNum.Rows.Count - Dal.LabelTask.LabelTask.Instance.StatisticsMediaNumCount((int)Entities.LabelTask.ENUM.EnumMediaType.微博, mediaNumbers);
            #endregion
            return new Tuple<int, int, int, int>(weChatCount, topLineCount, appCount, weiboCount);
        }
        #endregion
        #region 标签任务列表
        public ResLabelListQueryDTO LabelListQuery(ReqLabelListQueryDTO query,out string msg)
        {
            msg = string.Empty;
            Common.UserRole currentRole = Chitunion2017.Common.UserInfo.GetUserRole();
            if (!currentRole.IsLabelOpt && !currentRole.IsLabelAudit)
            {
                msg = $"当前角色无数据权限!";
                return null;
            }
            return Dal.LabelTask.LabelTask.Instance.LabelListQuery(query);
        }
        #endregion
        #region 领取任务
        public List<ResLabelModelQueryDTO> LabelReceiveTask(out string msg)
        {
            msg = string.Empty;
            Common.UserRole currentRole = Chitunion2017.Common.UserInfo.GetUserRole();            
            if (!currentRole.IsLabelOpt)
            {
                msg = $"不能领取任务，只有打标签角色可以领取任务!";
                return null;
            }                
            return Dal.LabelTask.LabelTask.Instance.LabelReceiveTask();
        }
        #endregion
        #region 删除停止项目
        public void LabelProjectStatus(int projectID, int status, out string msg)
        {
            msg = string.Empty;
            if (status != (int)Entities.LabelTask.ENUM.EnumProjectStatus.删除 && status != (int)Entities.LabelTask.ENUM.EnumProjectStatus.已停止)
                msg = "状态参数错误";
            else
            {
                Dal.LabelTask.LabelTask.Instance.LabelProjectStatus(projectID, status);
            }
        }
        #endregion
        #region 任务列表审核领取
        public int LabelTaskAuditQuery(int taskID, out string msg)
        {
            msg = string.Empty;
            Common.UserRole currentRole = Chitunion2017.Common.UserInfo.GetUserRole();
            if (!currentRole.IsLabelAudit)
            {
                msg = $"不能审核任务，只有审核角色可以审核任务!";
                return -2;
            }
            return Dal.LabelTask.LabelTask.Instance.LabelTaskAuditQuery(taskID, out msg);
        }
        #endregion
        #region 取消任务审核
        public int LabelTaskAuditCancel(int taskID)
        {
            return Dal.LabelTask.LabelTask.Instance.LabelTaskAuditCancel(taskID);
        }
        #endregion
        #region 获取文章摘要关键词
        public dynamic GetSummaryKeyWord(int articleID, int summarySize, int keyWordSize)
        {
            string SummaryKeyWordUrl = ConfigurationManager.AppSettings["InterfaceGetSummaryKeyWord"];
            string url = string.Format(SummaryKeyWordUrl, articleID, summarySize, keyWordSize);
            var obj = Util.HttpWebRequestCreate<ResSummaryKeyWordDTO>(url);
            if (obj.code == 10000)
            {
                return obj.data;
            }
            else
                return "没有数据";
        }
        #endregion
    }
}
