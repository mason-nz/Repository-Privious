using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Text;


namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    /// <summary>
    /// BatchExportTask 的摘要说明
    /// </summary>
    public class BatchExportTask : IHttpHandler
    {
        private string ProjectID
        {
            get
            {
                return HttpContext.Current.Request["projectid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
            }
        }

        private string ProjectName = "";
        private string ErrInfo = "";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string msg = "";

            ExportTaskToFile(out ErrInfo);

            msg = "{\"ProjectID\":\"" + ProjectID + "\",\"ProjectName\":\"" + ProjectName + "\",\"ErrInfo\":\"" + ErrInfo + "\",}";

            context.Response.Write(msg);
        }

        private void ExportTaskToFile(out string msg)
        {
            msg = "";

            try
            {
                ExportTask(out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

        }

        private void ExportTask(out string msg)
        {
            msg = "";

            #region 定义变量

            int totalCount = 0;
            string TTName = "";
            string TTCode = "";
            string selectDataIDs = "";

            Entities.TTable ttable;
            List<Entities.TField> fieldList = new List<Entities.TField>();
            DataTable DbdataDt;//自定义数据表中的数据

            #endregion

            #region 获取项目、自定义数据表、字段信息

            Entities.ProjectInfo model = BLL.ProjectInfo.Instance.GetProjectInfo(int.Parse(ProjectID));

            if (model == null)
            {
                msg += "没找到对应的项目";
                return;
            }
            ProjectName = model.Name;
            if (model.Source != 4)
            {
                msg += "此类型项目不允许导出";
                return;
            }

            TTCode = model.TTCode;

            ttable = BLL.TTable.Instance.GetTTableByTTCode(TTCode); //

            if (ttable.TTName == "" || ttable.TTName == null)
            {
                msg += "没有找到定义的自定义数据表名称";
                return;
            }

            TTName = ttable.TTName;

            fieldList = BLL.TField.Instance.GetTFieldListByTTCode(TTCode);
            #endregion

            #region 获取自定义数据表中的关联数据

            Entities.QueryProjectDataSoure query = new Entities.QueryProjectDataSoure();
            query.ProjectID = int.Parse(ProjectID);
            DataTable dt = BLL.ProjectDataSoure.Instance.GetProjectDataSoure(query, "", 1, 9999999, out totalCount);
            foreach (DataRow dr in dt.Rows)
            {
                selectDataIDs += dr["RelationID"].ToString() + ",";
            }
            if (selectDataIDs != "")
            {
                selectDataIDs = selectDataIDs.Substring(0, selectDataIDs.Length - 1);
            }
            DbdataDt = BLL.TTable.Instance.GetDataByIDs(selectDataIDs, TTName, TTCode, model.ProjectID.ToString(), out msg);
            if (msg != "" || DbdataDt == null)
            {
                msg = "读取自定义数据出错！" + msg;
                return;
            }

            #endregion

            #region 处理列

            Entities.TField tempfield;
            for (int i = DbdataDt.Columns.Count - 1; i >= 0; i--)
            {
                tempfield = null;

                #region 删除不用导出的字段

                if (DbdataDt.Columns[i].ColumnName.Split('_').Length == 2)
                {
                    if (DbdataDt.Columns[i].ColumnName.Split('_')[1] == "Province")
                    {
                        DbdataDt.Columns.RemoveAt(i);
                    }
                    else if (DbdataDt.Columns[i].ColumnName.Split('_')[1] == "City")
                    {
                        DbdataDt.Columns.RemoveAt(i);
                    }
                    else if (DbdataDt.Columns[i].ColumnName.Split('_')[1] == "Country")
                    {
                        DbdataDt.Columns.RemoveAt(i);
                    }
                    else if (DbdataDt.Columns[i].ColumnName.Split('_')[1] == "checkid")
                    {
                        DbdataDt.Columns.RemoveAt(i);
                    }
                    else if (DbdataDt.Columns[i].ColumnName.Split('_')[1] == "radioid")
                    {
                        DbdataDt.Columns.RemoveAt(i);
                    }
                    else if (DbdataDt.Columns[i].ColumnName.Split('_')[1] == "selectid")
                    {
                        DbdataDt.Columns.RemoveAt(i);
                    }
                }

                if (DbdataDt.Columns[i].ColumnName == "RecID"
                      || DbdataDt.Columns[i].ColumnName == "Status"
                      || DbdataDt.Columns[i].ColumnName == "CreateTime"
                      || DbdataDt.Columns[i].ColumnName == "CreateUserID"
                    )
                {
                    DbdataDt.Columns.RemoveAt(i);
                }

                #endregion

                #region 替换字段名

                if (DbdataDt.Columns[i].ColumnName.IndexOf("_startdata") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName + "（起）";
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_enddata") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName + "（止）";
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_starttime") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName + "（起）";
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_endtime") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName + "（止）";
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_Province_name") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName + "（省）";
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_City_name") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName + "（市）";
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_Country_name") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName + "（县）";
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_checkid_name") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName;
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_radioid_name") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName;
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_selectid_name") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName;
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName == "LastOptTime")
                {
                    DbdataDt.Columns[i].ColumnName = "操作时间";
                }

                else
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName;
                    }
                }


                #endregion
            }

            #region 任务ID列

            DbdataDt.Columns["PTID"].SetOrdinal(0);
            DbdataDt.Columns["PTID"].ColumnName = "任务ID";
            #endregion

            #region 增加列

            DbdataDt.Columns.Add("操作人");
            DbdataDt.Columns.Add("任务状态");

            #endregion

            #endregion

            #region 处理数据

            if (DbdataDt.Columns.Contains("LastOptUserID"))
            {
                int intVal = 0;
                string statusName = "";

                foreach (DataRow dr in DbdataDt.Rows)
                {
                    if (int.TryParse(dr["LastOptUserID"].ToString(), out intVal))
                    {
                        dr["操作人"] = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(intVal);
                    }

                    switch (dr["TaskStatus"].ToString())
                    {
                        case "1": statusName = "未分配"; break;
                        case "2": statusName = "未处理"; break;
                        case "3": statusName = "处理中"; break;
                        case "4": statusName = "已处理"; break;
                        case "5": statusName = "已结束"; break;
                    }
                    dr["任务状态"] = statusName;

                    foreach (DataColumn col in DbdataDt.Columns)
                    {
                        if (col.DataType == typeof(DateTime) && dr[col.ColumnName].ToString() == "1900-1-1 0:00:00")
                        {
                            dr[col.ColumnName] = DBNull.Value;
                        }
                    }
                }
            }

            #endregion

            #region 删除用户ID列

            DbdataDt.Columns.Remove("LastOptUserID");
            DbdataDt.Columns.Remove("TaskStatus");

            #endregion

            #region 保存到服务器根目录下

            var exportString = BLL.Util.TableToSCV(DbdataDt);

            string Path = System.AppDomain.CurrentDomain.BaseDirectory + @"Excel201307";
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            string filePath = Path + "\\" + model.Name + ".csv";
            File.WriteAllText(filePath, exportString.ToString(), Encoding.GetEncoding("GB2312"));


            #endregion

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