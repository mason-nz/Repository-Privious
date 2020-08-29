using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;
using BitAuto.ISDC.CC2012.Web.AjaxServers.ExcelOperate;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    /// <summary>
    /// ExportProjectTask 的摘要说明
    /// </summary>
    public class ExportProjectTask : IHttpHandler, IRequiresSessionState
    {
        private string ProjectID
        {
            get
            {
                return HttpContext.Current.Request["projectid"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["projectid"].ToString());
            }
        }
        private string RequestBrowser
        {
            get
            {

                return HttpContext.Current.Request["Browser"] == null ? String.Empty :
                  HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Browser"].ToString());
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string msg = "";

            CheckPro(out msg);

            if (msg == "")
            {
                int userID = BLL.Util.GetLoginUserID();

                ExportTask(out msg);
            }
            if (msg == "")
            {
                msg = "success";
            }
            context.Response.Write(msg);
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
            DataTable ExportDataDt;//导出的数据

            #endregion

            #region 获取项目、自定义数据表、字段信息

            Entities.ProjectInfo model = BLL.ProjectInfo.Instance.GetProjectInfo(int.Parse(ProjectID));

            if (model == null)
            {
                msg += "没找到对应的项目";
                return;
            }
            if (model.Source != 4)
            {
                msg += "此类型项目不允许导出";
                return;
            }

            TTCode = model.TTCode;

            ttable = BLL.TTable.Instance.GetTTableByTTCode(TTCode); //

            if (ttable.TTName == "")
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
            DbdataDt = BLL.TTable.Instance.GetDataByIDs(selectDataIDs, TTName, out msg);
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

                if (DbdataDt.Columns[i].ColumnName == "RecID" || DbdataDt.Columns[i].ColumnName == "Status")
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
                else if (DbdataDt.Columns[i].ColumnName == "CreateTime")
                {
                    DbdataDt.Columns[i].ColumnName = "导入时间";
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
            #endregion

            #region 处理数据

            if (DbdataDt.Columns.Contains("CreateUserID"))
            {
                int intVal=0;
                DbdataDt.Columns.Add("导入人").SetOrdinal(0);
                string userName = "";

                foreach (DataRow dr in DbdataDt.Rows)
                {
                    if (int.TryParse(dr["CreateUserID"].ToString(), out intVal))
                    {
                        if (userName == "")
                        {
                            userName= BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(intVal);
                        }
                        dr["导入人"] = userName;
                    }
                }
            }

            #endregion

            #region 删除用户ID列
            
            DbdataDt.Columns.Remove("CreateUserID");

            #endregion

            #region 导出

            ExcelInOut.CreateEXCEL(DbdataDt, model.Name+ "的任务" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), RequestBrowser);
         
            #endregion

        }

        private void CheckPro(out string msg)
        {
            msg = "";
            int intval = 0;

            if (ProjectID == "")
            {
                msg += "缺少项目ID参数";
            }
            if (!int.TryParse(ProjectID, out intval))
            {
                msg += "项目ID参数格式不正确";
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