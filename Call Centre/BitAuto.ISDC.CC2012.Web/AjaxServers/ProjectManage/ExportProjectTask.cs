using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    public class ExportProjectTask
    {
        /// <summary>
        /// 导出的客户信息项
        /// </summary>
        private string[] array_CustColumns = new string[7] { "客户名称", "客户省份", "客户城市", "客户区县", "客户所属大区", "会员ID", "会员名称" };

        private string crmCustIDFieldName = string.Empty;

        public DataTable ExportTask(int ProjectID, out string msg, out string exportName, string taskcreatestart, string taskcreateend, string tasksubstart, string tasksubend)
        {
            msg = "";
            exportName = "";

            #region 定义变量

            int totalCount = 0;
            string TTName = "";
            string TTCode = "";
            string selectDataIDs = "";

            Entities.TTable ttable = null;
            Entities.ProjectInfo model = null;
            List<Entities.TField> fieldList = null;
            DataTable DbdataDt = null;//自定义数据表中的数据

            #endregion

            #region 获取项目、自定义数据表、字段信息

            GetBaseInfo(ProjectID, out model, out TTCode, out ttable, out TTName, out fieldList, out msg);

            if (msg != "")
            {
                return null;
            }

            exportName = model.Name;

            #endregion

            #region 获取自定义数据表中的关联数据

            GetTemptRelationData(ProjectID, TTName, TTCode, model, out selectDataIDs, out DbdataDt, taskcreatestart, taskcreateend, tasksubstart, tasksubend, out msg);

            if (msg != "")
            {
                return null;
            }

            #endregion

            #region 是否成功,是否接通,接通后失败原因,未接通原因列的转换

            if (DbdataDt.Columns.Contains("IsSuccess") && DbdataDt.Columns.Contains("IsEstablish") && DbdataDt.Columns.Contains("NotSuccessReason") && DbdataDt.Columns.Contains("NotEstablishReason"))
            {
                DataColumn colIsSuccess = new DataColumn("IsSucName", Type.GetType("System.String"));
                DbdataDt.Columns.Add(colIsSuccess);
                DataColumn colIsJT = new DataColumn("IsJTName", Type.GetType("System.String"));
                DbdataDt.Columns.Add(colIsJT);
                DataColumn colNotSuccessReason = new DataColumn("NotSuccessReasonName", Type.GetType("System.String"));
                DbdataDt.Columns.Add(colNotSuccessReason);
                DataColumn colNotExport = new DataColumn("NotExportName", Type.GetType("System.String"));
                DbdataDt.Columns.Add(colNotExport);

                for (int i = 0; i < DbdataDt.Rows.Count; i++)
                {

                    DbdataDt.Rows[i]["IsSucName"] = BLL.Util.GetIsNotStatus(DbdataDt.Rows[i]["IsSuccess"].ToString());
                    DbdataDt.Rows[i]["IsJTName"] = BLL.Util.GetIsNotStatus(DbdataDt.Rows[i]["IsEstablish"].ToString());

                    string notSuc = DbdataDt.Rows[i]["NotSuccessReason"].ToString();
                    string notExport = DbdataDt.Rows[i]["NotEstablishReason"].ToString();

                    string notExportName = String.Empty;
                    if (!(String.IsNullOrEmpty(notExport) || notExport.Trim() == "-1" || notExport.Trim() == "-2" || DbdataDt.Rows[i]["IsEstablish"].ToString() == "1"))
                    {
                        //notExport=空，-1，-2 或者 接通了 是没有值的
                        notExportName = BLL.Util.GetEnumOptText(typeof(Entities.NotEstablishReason), Int32.Parse(notExport));
                    }

                    string notSuccessReasonName = String.Empty;
                    if (!(String.IsNullOrEmpty(notSuc) || notSuc.Trim() == "-1" || notSuc.Trim() == "-2"))
                    {
                        //notSuc=空，-1，-2 是没有值的
                        notSuccessReasonName = BLL.Util.GetEnumOptText(typeof(Entities.NotSuccessReason), Int32.Parse(notSuc));
                    }

                    DbdataDt.Rows[i]["NotSuccessReasonName"] = notSuccessReasonName;
                    DbdataDt.Rows[i]["NotExportName"] = notExportName;

                }
            }

            #endregion
            #region 处理列

            DbdataDt = ProcessExportDataColoumns(TTName, TTCode, model, fieldList, DbdataDt);

            #endregion

            #region 处理数据

            DbdataDt = ProcessData(DbdataDt);

            #endregion

            #region 关联crm客户信息

            if (crmCustIDFieldName != string.Empty)
            {
                GetTogetherCustInfo(crmCustIDFieldName, ProjectID, DbdataDt);
            }

            #endregion

            #region 删除用户ID列

            DbdataDt.Columns.Remove("LastOptUserID");
            DbdataDt.Columns.Remove("TaskStatus");

            #endregion

            return DbdataDt;
        }

        /// <summary>
        /// 获取项目、自定义数据表、字段信息 基本信息
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="model"></param>
        /// <param name="TTCode"></param>
        /// <param name="ttable"></param>
        /// <param name="TTName"></param>
        /// <param name="fieldList"></param>
        /// <param name="msg"></param>
        private void GetBaseInfo(int ProjectID, out Entities.ProjectInfo model, out string TTCode, out Entities.TTable ttable, out string TTName, out List<Entities.TField> fieldList, out string msg)
        {
            msg = string.Empty;

            model = BLL.ProjectInfo.Instance.GetProjectInfo(ProjectID);

            TTCode = string.Empty;

            ttable = new Entities.TTable();

            TTName = string.Empty;

            fieldList = new List<Entities.TField>();

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

            if (ttable.TTName == "" || ttable.TTName == null)
            {
                msg += "没有找到定义的自定义数据表名称";
                return;
            }

            TTName = ttable.TTName;

            fieldList = BLL.TField.Instance.GetTFieldListByTTCode(TTCode);
        }

        /// <summary>
        /// 获取自定义数据表中的关联数据
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="TTName"></param>
        /// <param name="TTCode"></param>
        /// <param name="model"></param>
        /// <param name="selectDataIDs"></param>
        /// <param name="DbdataDt"></param>
        /// <param name="msg"></param>
        private void GetTemptRelationData(int ProjectID, string TTName, string TTCode, Entities.ProjectInfo model, out string selectDataIDs, out DataTable DbdataDt, string taskcreatestart, string taskcreateend, string tasksubstart, string tasksubend, out string msg)
        {
            msg = string.Empty;
            selectDataIDs = string.Empty;
            DbdataDt = new DataTable();

            Entities.QueryProjectDataSoure query = new Entities.QueryProjectDataSoure();
            query.ProjectID = ProjectID;
            int totalCount = 0;
            DataTable dt = BLL.ProjectDataSoure.Instance.GetProjectDataSoure(query, "", 1, -1, out totalCount);

            foreach (DataRow dr in dt.Rows)
            {
                selectDataIDs += dr["RelationID"].ToString() + ",";
            }
            if (selectDataIDs != "")
            {
                selectDataIDs = selectDataIDs.Substring(0, selectDataIDs.Length - 1);
            }
            //modify by qizq 2014-11-24 给项目导出加任务的创建时间，任务提交时间等过滤条件
            //DbdataDt = BLL.TTable.Instance.GetDataByIDs(selectDataIDs, TTName, TTCode, model.ProjectID.ToString(), out msg);
            DbdataDt = BLL.TTable.Instance.GetDataByIDs(selectDataIDs, TTName, TTCode, model.ProjectID.ToString(), taskcreatestart, taskcreateend, tasksubstart, tasksubend, out msg);

            if (msg != "" || DbdataDt == null)
            {
                msg = "读取自定义数据出错！";
                return;
            }
        }

        /// <summary>
        /// 处理列
        /// </summary>
        /// <param name="TTName"></param>
        /// <param name="TTCode"></param>
        /// <param name="model"></param>
        /// <param name="fieldList"></param>
        /// <param name="OriginalDbdataDt"></param>
        /// <returns></returns>
        private DataTable ProcessExportDataColoumns(string TTName, string TTCode, Entities.ProjectInfo model, List<Entities.TField> fieldList, DataTable OriginalDbdataDt)
        {
            DataTable DbdataDt = new DataTable();
            DbdataDt = OriginalDbdataDt;

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
                    else if (DbdataDt.Columns[i].ColumnName.Split('_')[1] == "XDBrand" || DbdataDt.Columns[i].ColumnName.Split('_')[1] == "YXBrand" || DbdataDt.Columns[i].ColumnName.Split('_')[1] == "CSBrand")
                    {
                        DbdataDt.Columns.RemoveAt(i);
                    }
                    else if (DbdataDt.Columns[i].ColumnName.Split('_')[1] == "XDSerial" || DbdataDt.Columns[i].ColumnName.Split('_')[1] == "YXSerial" || DbdataDt.Columns[i].ColumnName.Split('_')[1] == "CSSerial")
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
                    //删除推荐活动Guid串列
                    else if (DbdataDt.Columns[i].ColumnName.Split('_')[1] == "Activity")
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

                else if (DbdataDt.Columns[i].ColumnName == "IsSuccess"
                  || DbdataDt.Columns[i].ColumnName == "IsEstablish"
                  || DbdataDt.Columns[i].ColumnName == "NotSuccessReason"
                  || DbdataDt.Columns[i].ColumnName == "NotEstablishReason"
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
                else if (DbdataDt.Columns[i].ColumnName == "TrueName")
                {
                    DbdataDt.Columns[i].ColumnName = "操作人";
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_crmcustid_name") != -1)
                {
                    crmCustIDFieldName = DbdataDt.Columns[i].ColumnName;
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("Brand_Name") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName + "（品牌）";
                    }
                }
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("Serial_Name") != -1)
                {
                    tempfield = fieldList.Find(delegate(Entities.TField o) { return o.TFName == DbdataDt.Columns[i].ColumnName.Split('_')[0]; });
                    if (tempfield != null)
                    {
                        DbdataDt.Columns[i].ColumnName = tempfield.TFDesName + "（车型）";
                    }
                }
                //把推荐活动的列名改成汉字
                else if (DbdataDt.Columns[i].ColumnName.IndexOf("_Activity_Name") != -1)
                {
                    DbdataDt.Columns[i].ColumnName = "推荐活动";
                }


                else if (DbdataDt.Columns[i].ColumnName == "IsSucName")
                {
                    DbdataDt.Columns[i].ColumnName = "是否成功";
                }
                else if (DbdataDt.Columns[i].ColumnName == "IsJTName")
                {
                    DbdataDt.Columns[i].ColumnName = "是否接通";
                }
                else if (DbdataDt.Columns[i].ColumnName == "NotSuccessReasonName")
                {
                    DbdataDt.Columns[i].ColumnName = "失败原因";
                }
                else if (DbdataDt.Columns[i].ColumnName == "NotExportName")
                {
                    DbdataDt.Columns[i].ColumnName = "未接通原因";
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


            #region 增加客户信息列

            if (crmCustIDFieldName != string.Empty)
            {
                DbdataDt.Columns[crmCustIDFieldName].SetOrdinal(1);
                DbdataDt.Columns[crmCustIDFieldName].ColumnName = "客户ID";

                for (int k = 0; k < array_CustColumns.Length; k++)
                {
                    string columnName = array_CustColumns[k];

                    //如果之前导出的列中存在该列名，则修改下现在的列名
                    if (DbdataDt.Columns.Contains(columnName))
                    {
                        columnName += "（固定项）";
                        array_CustColumns[k] = columnName;
                    }

                    DbdataDt.Columns.Add(columnName);
                    DbdataDt.Columns[columnName].SetOrdinal(k + 2);
                }
            }

            #endregion

            #region 增加列

            DbdataDt.Columns.Add("任务状态");

            #endregion

            return DbdataDt;
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="OriginalDbdataDt"></param>
        /// <returns></returns>
        private DataTable ProcessData(DataTable OriginalDbdataDt)
        {
            DataTable DbdataDt = new DataTable();
            DbdataDt = OriginalDbdataDt;

            if (DbdataDt.Columns.Contains("LastOptUserID"))
            {
                int intVal = 0;
                string statusName = "";

                foreach (DataRow dr in DbdataDt.Rows)
                {
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
                        if (col.DataType == typeof(DateTime) && CommonFunction.GetDateTimeStrForPage(dr[col.ColumnName].ToString()) == "")
                        {
                            dr[col.ColumnName] = DBNull.Value;
                        }
                    }
                }

            }
            return DbdataDt;
        }

        int exportCount = 0, actualCount = 0;

        private delegate string[] DeleCustInfoByPTID(string ptid, DataRow dr);

        private Dictionary<string, string[]> dic_CustInfo = new Dictionary<string, string[]>();

        /// <summary>
        /// 将CustInfo的信息拼接到所要导出的DataTable中
        /// </summary>
        /// <param name="TTName"></param>
        /// <param name="custIDColumnName"></param>
        /// <param name="projectID"></param>
        /// <param name="DbdataDt"></param>
        private void GetTogetherCustInfo(string custIDColumnName, int projectID, DataTable DbdataDt)
        {
            DataTable dt_CustInfo = BLL.ProjectInfo.Instance.GetExportCustInfoByTempt(custIDColumnName, projectID.ToString());
            int custInfoCount = DbdataDt.Rows.Count;
            for (int i = 0; i < custInfoCount; i++)
            {
                string PTID = DbdataDt.Rows[i]["任务ID"].ToString();

                DataRow[] dt_custInfoByPTID = dt_CustInfo.Select("  ptid='" + PTID + "'");
                if (dt_custInfoByPTID.Length == 1)
                {
                    exportCount += 1;

                    DeleCustInfoByPTID deleGetCustInfo = new DeleCustInfoByPTID(GetCustInfoDataRow);

                    deleGetCustInfo.BeginInvoke(PTID, dt_custInfoByPTID[0], CallBack, null);
                }

            }

            while (actualCount != exportCount)
            {
                System.Threading.Thread.Sleep(500);
            }

            for (int p = 0; p < custInfoCount; p++)
            {
                string PTID = DbdataDt.Rows[p]["任务ID"].ToString();

                if (dic_CustInfo.ContainsKey(PTID))
                {
                    string[] array_CustDatas = dic_CustInfo[PTID];
                    for (int k = 0; k < array_CustColumns.Length; k++)
                    {
                        DbdataDt.Rows[p][array_CustColumns[k]] = array_CustDatas[k + 1];
                    }
                }

            }

        }

        private string[] GetCustInfoDataRow(string PTID, DataRow dr)
        {
            string[] array_CustDatas = new string[8] { PTID, "", "", "", "", "", "", "" };

            //获取省 市 区
            int proviceID = CommonFunction.ObjectToInteger(dr["ProvinceID"]);
            int cityID = CommonFunction.ObjectToInteger(dr["CityID"]);
            int CountyID = CommonFunction.ObjectToInteger(dr["CountyID"]);
            //获取大区名称
            //强斐 2014-12-17
            BitAuto.YanFa.Crm2009.Entities.AreaInfo info = BLL.Util.GetAreaInfoByPCC(proviceID.ToString(), cityID.ToString(), CountyID.ToString());
            string areaName = info == null ? "" : info.DistinctName;

            string provinceName = dr["ProvinceName"].ToString();
            string cityName = dr["CityName"].ToString();
            string countryName = dr["CountryName"].ToString();

            string custName = dr["CustName"].ToString();
            string[] MmeberIDs = dr["MemberID"].ToString().Split(',');

            //易湃会员
            string DMSMemberID = "";
            string DMSMemberName = "";
            for (int m = 0; m < MmeberIDs.Length; m++)
            {
                string[] mid = MmeberIDs[m].Split('|');
                if (mid.Length == 2)
                {
                    DMSMemberID += mid[0] + ",";
                    DMSMemberName += mid[1] + ",";
                }
            }
            DMSMemberID = DMSMemberID.TrimEnd(',');
            DMSMemberName = DMSMemberName.TrimEnd(',');

            //车商通会员
            string[] cstMmeberIDs = dr["CstMemberID"].ToString().Split(',');
            string cstMemberID = "";
            string cstMemberName = "";
            for (int m = 0; m < cstMmeberIDs.Length; m++)
            {
                string[] mid = cstMmeberIDs[m].Split('|');
                if (mid.Length == 2)
                {
                    cstMemberID += mid[0] + ",";
                    cstMemberName += mid[1] + ",";
                }
            }
            cstMemberID = cstMemberID.TrimEnd(',');
            cstMemberName = cstMemberName.TrimEnd(',');

            DMSMemberID += ("," + cstMemberID).TrimEnd(',');
            DMSMemberName += ("," + cstMemberName).TrimEnd(',');

            array_CustDatas = new string[8] { PTID, custName, provinceName, cityName, countryName, areaName, DMSMemberID, DMSMemberName };

            return array_CustDatas;
        }

        private void CallBack(IAsyncResult iar)
        {
            DeleCustInfoByPTID dele = ((System.Runtime.Remoting.Messaging.AsyncResult)iar).AsyncDelegate as DeleCustInfoByPTID;
            if (dele != null)
            {
                lock (dic_CustInfo)
                {
                    actualCount += 1;
                    string[] custInfo = dele.EndInvoke(iar);

                    dic_CustInfo.Add(custInfo[0], custInfo);
                }
            }
        }
    }
}