using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement
{
    /// <summary>
    /// GenerateTemplate 的摘要说明
    /// </summary>
    public class GenerateTemplate : IHttpHandler, IRequiresSessionState
    {
        public string Action
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["Action"]) ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Action"].ToString());
            }
        }
        public string recid
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["recid"]) ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["recid"].ToString());
            }
        }
        public string ttCode
        {
            get
            {
                return String.IsNullOrEmpty(HttpContext.Current.Request["ttCode"]) ? "" :
                    HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["ttCode"].ToString());
            }
        }

        private bool createFileIsOk = true;
        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            int userID = BLL.Util.GetLoginUserID();
            string msg = "";

            if (Action == "Generate")
            {
                if (!BLL.Util.CheckRight(userID, "SYS024MOD510203"))
                {
                    msg = "result:'false',msg:'您没有权限执行此操作！'";
                }
                else if (recid != "")
                {
                    Generate(out msg);
                }
                else
                {
                    msg += "参数格式不正确";
                }
            }
            else if (Action == "SaveExcelTemplate")
            {
                int RecID = int.Parse(recid);
                Entities.TPage tpageModel = BLL.TPage.Instance.GetTPage(RecID);
                SaveExcelTemplate(tpageModel, out msg);
            }

            context.Response.Write(msg);
        }

        private void Generate(out string msg)
        {
            msg = "";

            int RecID = int.Parse(recid);
            Entities.TTable ttableModel = new Entities.TTable();
            List<Entities.TField> fieldList = new List<Entities.TField>();
            Entities.TPage tpageModel = BLL.TPage.Instance.GetTPage(RecID);
            if (tpageModel != null)
            {
                ttableModel = BLL.TTable.Instance.GetTTableByTTCode(tpageModel.TTCode);
                fieldList = BLL.TField.Instance.GetTFieldListByTTCode(tpageModel.TTCode);
                if (fieldList.Count > 0)
                {
                    GenerateFields(tpageModel, ttableModel, fieldList, out msg);
                }
                else
                {
                    msg += "模板没有字段，不能生成";
                }
            }
            else
            {
                msg += "没有找到对应的模板信息";
            }

        }

        /// <summary>
        /// 生成字段
        /// </summary>
        /// <param name="tpageModel"></param>
        /// <param name="ttableModel"></param>
        /// <param name="fieldList"></param>
        /// <param name="msg"></param>
        private void GenerateFields(Entities.TPage tpageModel, Entities.TTable ttableModel, List<Entities.TField> fieldList, out string msg)
        {
            msg = "";
            #region 删除模板文件
            try
            {
                string root = BLL.Util.GetUploadWebRoot() + BLL.Util.GetUploadProject(BLL.Util.ProjectTypePath.Template, "\\");
                string fullname = root + tpageModel.TPName + "_" + tpageModel.RecID + ".xls";
                if (File.Exists(fullname))
                {
                    File.Delete(fullname);
                }
            }
            catch (Exception ex)
            {
                msg = "删除模板文件出现异常：" + ex.Message;
            }

            #endregion

            #region 创建表
            string sqlStr = CreateSql(ttableModel, fieldList);
            BLL.TTable.Instance.CreateTable(sqlStr, out msg);
            #endregion

            #region 生成Excel文件
            if (msg != string.Empty)
            {
                return;
            }
            SaveExcelTemplate(tpageModel, out msg);
            #endregion

            #region 修改模板的状态和路径
            BLL.Util.InsertUserLog("【生成模板结束】createFileIsOk为" + createFileIsOk + "，msg为" + msg);
            if (!createFileIsOk)
            {
                BLL.Util.InsertUserLog("【生成模板失败】createFileIsOk为False，准备删除【" + ttableModel.TTName + "】模板表");
                //生成模板失败，则删除刚才创建的表
                string dropSql = " DROP Table " + ttableModel.TTName;
                string dropMsg = string.Empty;
                try
                {
                    BLL.TTable.Instance.CreateTable(dropSql, out dropMsg);
                    //记录日志
                    BLL.Util.InsertUserLog("【生成模板失败】【删除】刚创建的表【" + ttableModel.TTName + "】【删除模板表成功】");
                }
                catch (Exception ex)
                {
                    //记录日志
                    BLL.Util.InsertUserLog("【生成模板失败】【删除】刚创建的表【" + ttableModel.TTName + "】【删除模板表失败】，异常信息：" + ex.Message);
                }
                return;
            }

            //修改TPage表的IsUsed“是否可用”字段：修改成1：启用
            //add lxw 13.12.16
            tpageModel.IsUsed = 1;
            tpageModel.Status = 1;
            //更新路径到模板表
            tpageModel.GenTempletPath = tpageModel.TPName + "_" + tpageModel.RecID + ".xls";
            BLL.TPage.Instance.Update(tpageModel);
            #endregion
        }

        /// <summary>
        /// 创建表的语句
        /// </summary>
        /// <param name="ttableModel"></param>
        /// <param name="fieldList"></param>
        /// <returns></returns>
        public string CreateSql(Entities.TTable ttableModel, List<Entities.TField> fieldList)
        {
            string sqlStr = "";
            sqlStr += "create table " + ttableModel.TTName + " (";
            sqlStr += " [RecID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, ";
            sqlStr += " [Status] [int] , ";
            sqlStr += " [CreateTime] [datetime] NULL,";
            sqlStr += " [CreateUserID] [int] NULL,";

            //是否有个人用户字段
            List<Entities.TField> listFind = fieldList.FindAll(delegate(Entities.TField field)
            {
                return field.TFShowCode == "100015";
            });


            if (listFind.Count > 0)
            {
                fieldList.RemoveAll(delegate(Entities.TField field)
                {
                    return field.TFShowCode == "100015";
                });

                foreach (Entities.TField item in listFind)
                {
                    switch (item.TFDesName)
                    {
                        case "姓名":
                            sqlStr += item.TFName + " " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                            break;
                        case "性别":
                            sqlStr += item.TFName + "_radioid " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                            sqlStr += item.TFName + "_radioid_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                            break;
                        case "电话":
                            sqlStr += item.TFName + " " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                            break;
                        default:
                            break;
                    }
                }
            }

            foreach (Entities.TField item in fieldList)
            {
                if (item.TFShowCode == "100009")
                {
                    //日期段

                    sqlStr += item.TFName + "_startdata " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tdatetime) + ",";
                    sqlStr += item.TFName + "_enddata " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tdatetime) + ",";
                }
                if (item.TFShowCode == "100011")
                {
                    //时间段

                    sqlStr += item.TFName + "_starttime " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tdatetime) + ",";
                    sqlStr += item.TFName + "_endtime " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tdatetime) + ",";
                }
                else if (item.TFShowCode == "100012")
                {
                    //二级省市

                    sqlStr += item.TFName + "_Province " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_Province_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";

                    sqlStr += item.TFName + "_City " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_City_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100013")
                {
                    //三级省市县

                    sqlStr += item.TFName + "_Province " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_Province_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";

                    sqlStr += item.TFName + "_City " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_City_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";

                    sqlStr += item.TFName + "_Country " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_Country_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100004")//复选
                {
                    sqlStr += item.TFName + "_checkid " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                    sqlStr += item.TFName + "_checkid_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100003")//单选
                {
                    sqlStr += item.TFName + "_radioid " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_radioid_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100005")//下拉
                {
                    sqlStr += item.TFName + "_selectid " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_selectid_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100014")//CRM CustID
                {
                    sqlStr += item.TFName + "_crmcustid_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100015")//个人用户
                {
                    sqlStr += item.TFName + "_crmcustid_name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100016")
                {
                    //下单车型
                    sqlStr += item.TFName + "_XDBrand " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_XDBrand_Name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";

                    sqlStr += item.TFName + "_XDSerial " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_XDSerial_Name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100017")
                {
                    //意向车型
                    sqlStr += item.TFName + "_YXBrand " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_YXBrand_Name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";

                    sqlStr += item.TFName + "_YXSerial " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_YXSerial_Name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100018")
                {
                    //出售车型
                    sqlStr += item.TFName + "_CSBrand " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_CSBrand_Name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";

                    sqlStr += item.TFName + "_CSSerial " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                    sqlStr += item.TFName + "_CSSerial_Name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100019")//推荐活动
                {
                    sqlStr += item.TFName + "_Activity " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                    sqlStr += item.TFName + "_Activity_Name " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                }
                else if (item.TFShowCode == "100020")
                {
                    sqlStr += item.TFName + " " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tint) + ",";
                }
                else
                {
                    switch (item.TFShowCode)
                    {
                        case "100001":
                        case "100002":

                        case "100006":
                        case "100007":
                            sqlStr += item.TFName + " " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tnvarchar) + "(" + item.TFLen + "),";
                            break;

                        case "100008":
                        case "100010":
                            sqlStr += item.TFName + " " + BLL.Util.GetEnumOptText(typeof(EnumTFieldType), (int)EnumTFieldType.Tdatetime) + ",";
                            break;
                    }
                }
            }
            sqlStr = sqlStr.Substring(0, sqlStr.Length - 1);
            sqlStr += " )";

            return sqlStr;
        }
        
        /// <summary>
        /// 保存excel文件
        /// </summary>
        /// <param name="tPage"></param>
        public void SaveExcelTemplate(Entities.TPage tPage, out string msg)
        {
            msg = string.Empty;
            // 获得数据
            if (ttCode == "")
            {
                return;
            }
            // 在服务器上生成Excel文件
            string FilePath = BLL.Util.GetUploadWebRoot() + BLL.Util.GetUploadProject(BLL.Util.ProjectTypePath.Template, "\\");
            string FileName = tPage.TPName + "_" + tPage.RecID;
            string fullname = FilePath + FileName + ".xls";
            if (!File.Exists(fullname))
            {
                BLL.Loger.Log4Net.Info("生成模板日志------------------------------------------开始");
                ArrayList array_TagRed = new ArrayList();
                DataTable dt = getTemplateColumn(ttCode, out array_TagRed);

                Write(dt, fullname);
                BLL.Loger.Log4Net.Info("准备垃圾回收：");
                //垃圾回收
                GC.Collect();
                BLL.Loger.Log4Net.Info("垃圾回收结束");

                BLL.Loger.Log4Net.Info("生成模板日志------------------------------------------结束");
            }
        }

        /// <summary>
        /// 通过ttcode获取需要导出的excel表
        /// </summary>
        /// <param name="ttCode"></param>
        /// <param name="tagRed">需要标红的字段</param>
        /// <returns></returns>
        public DataTable getTemplateColumn(string ttCode, out ArrayList tagRed)
        {
            DataTable dt_ExcelColumn = new DataTable();//需要导出的真实列名表,返回值
            int excelColumn_Count = 0;                          //列名表的个数
            tagRed = new ArrayList();       //需要标红的列

            DataRow dr = dt_ExcelColumn.NewRow();

            DataTable dt_TemptColumn = BLL.TField.Instance.GetTemptColumnNameByTableName(ttCode);
            int tempt_Count = dt_TemptColumn.Columns.Count;

            BLL.Loger.Log4Net.Info("ttCode为" + ttCode + "的新生成的表一共有：" + tempt_Count + " 列");

            for (int k = 0; k < tempt_Count; k++)
            {
                //column_Name格式：Tempf316_Province或Tempf316_Province_name
                string column_Name = dt_TemptColumn.Columns[k].ToString();
                string[] columns = column_Name.Split('_');

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"Tempf.*");
                bool m = reg.IsMatch(column_Name);

                if (m)
                {

                    string tfDesName = string.Empty;//字段名

                    //需要特殊处理的列
                    if (columns.Length >= 2)
                    {
                        //如果是 推荐活动字段列 则不生成EXCEL模板列
                        if (columns[1] == "Activity")
                        {
                            BLL.Loger.Log4Net.Info("DataTable正在生成，推荐活动字段不生成列名!");
                            continue;
                        }

                        string addDes = string.Empty;

                        //如果为Province且有columns有三个，则表示是省份名；
                        if (columns[1] == "Province" && columns.Length == 3)
                        {
                            addDes = "（省）";
                        }
                        //如果为City且有columns有三个，则表示是城市名；
                        else if (columns[1] == "City" && columns.Length == 3)
                        {
                            addDes = "（市）";
                        }
                        //如果为Country且有columns有三个，则表示是县名；
                        else if (columns[1] == "Country" && columns.Length == 3)
                        {
                            addDes = "（县）";
                        }
                        //如果为startdata或starttime，则表示是开始日期/时间段；
                        else if (columns[1] == "startdata" || columns[1] == "starttime")
                        {
                            addDes = "（起）";
                        }
                        //如果为enddata或endtime，则表示是结束日期/时间段；
                        else if (columns[1] == "enddata" || columns[1] == "endtime")
                        {
                            addDes = "（止）";
                        }

                        else if (columns[1] == "XDBrand" && columns.Length == 3)
                        {
                            addDes = "（品牌）";
                        }
                        else if (columns[1] == "XDSerial" && columns.Length == 3)
                        {
                            addDes = "（车型）";
                        }
                        else if (columns[1] == "YXBrand" && columns.Length == 3)
                        {
                            addDes = "（品牌）";
                        }
                        else if (columns[1] == "YXSerial" && columns.Length == 3)
                        {
                            addDes = "（车型）";
                        }
                        else if (columns[1] == "CSBrand" && columns.Length == 3)
                        {
                            addDes = "（品牌）";
                        }
                        else if (columns[1] == "CSSerial" && columns.Length == 3)
                        {
                            addDes = "（车型）";
                        }

                        //columns.Length == 3：是判断是前面几项 还包括radio、check、select
                        //addDes != string.Empty：如果是时间段/点columns.Length =2，所以加上这个条件
                        if (columns.Length == 3 || addDes != string.Empty)
                        {
                            DataTable dt_TField = BLL.TField.Instance.GetTFieldTableByTFName(columns[0]);
                            tfDesName = dt_TField.Rows[0]["TFDesName"].ToString() + addDes;
                            dt_ExcelColumn.Columns.Add(excelColumn_Count.ToString());
                            dr[excelColumn_Count] = tfDesName;

                            //如果导入字段不能为空，则插入tagRed中
                            if (dt_TField.Rows[0]["TFInportIsNull"].ToString() == "0")
                            {
                                tagRed.Add(excelColumn_Count + 1);
                            }
                            excelColumn_Count++;
                            BLL.Loger.Log4Net.Info("DataTable正在生成，列名：" + tfDesName);
                        }
                    }
                    else
                    {
                        DataTable dt_TField = BLL.TField.Instance.GetTFieldTableByTFName(columns[0]);
                        tfDesName = dt_TField.Rows[0]["TFDesName"].ToString();
                        dt_ExcelColumn.Columns.Add(excelColumn_Count.ToString());
                        dr[excelColumn_Count] = tfDesName;
                        //如果导入字段不能为空，则插入tagRed中
                        if (dt_TField.Rows[0]["TFInportIsNull"].ToString() == "0")
                        {
                            tagRed.Add(excelColumn_Count + 1);
                        }
                        excelColumn_Count++;

                        BLL.Loger.Log4Net.Info("DataTable正在生成，列名：" + tfDesName);
                    }
                }
            }
            dt_ExcelColumn.Rows.Add(dr);
            BLL.Loger.Log4Net.Info("DataTable生成结束");
            return dt_ExcelColumn;
        }

        /// <summary>
        /// 将数据集中的数据导出到EXCEL文件
        /// </summary>
        /// <param name="dataSet">输入数据集</param>
        /// <param name="isShowExcle">是否显示该EXCEL文件</param>
        /// <returns></returns>
        public bool DataSetToExcel(DataTable dt, string FileName, string FilePath, bool isShowExcle, ArrayList tagRed, out string msg)
        {
            msg = string.Empty;

            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            int FormatNum = -4143;//保存excel文件的格式 
            try
            {
                DataTable dataTable = dt;
                int rowNumber = dataTable.Rows.Count;//不包括字段名
                int columnNumber = dataTable.Columns.Count;
                int colIndex = 0;

                BLL.Loger.Log4Net.Info("准备建立excel对象：");

                //建立Excel对象
                excel.DefaultFilePath = FilePath;
                //excel.Application.Workbooks.Add(true);
                excel.Visible = isShowExcle;
                //Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)excel.Worksheets[1];
                Microsoft.Office.Interop.Excel.Range range;
                //生成字段名称
                foreach (DataColumn col in dataTable.Columns)
                {
                    colIndex++;
                    if (colIndex == 1)
                    {
                        BLL.Loger.Log4Net.Info("excel对象建立完毕，生成第一行提示信息：" + col.ColumnName);

                        excel.Cells[1, colIndex] = col.ColumnName;
                    }
                    else
                    {
                        //第一行是提示；标红为导入时必填项；需要清空，否则在合并单元格时有问题
                        excel.Cells[1, colIndex] = "";
                    }
                }

                object[,] objData = new object[rowNumber, columnNumber];

                BLL.Loger.Log4Net.Info("准备生成第二行列名信息：");

                for (int r = 0; r < rowNumber; r++)
                {
                    for (int c = 0; c < columnNumber; c++)
                    {
                        objData[r, c] = dataTable.Rows[r][c];

                        BLL.Loger.Log4Net.Info("第" + (c + 1) + "列信息：" + dataTable.Rows[r][c]);

                    }
                }

                // 写入Excel 
                BLL.Loger.Log4Net.Info("生成模版列完成");
                BLL.Loger.Log4Net.Info("准备合并单元格");

                //列 合并单元格
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[1, dt.Columns.Count]).Merge(0);
                BLL.Loger.Log4Net.Info("合并单元格完成");
                //设置字体颜色，非空字段要标红
                for (int k = 0; k < tagRed.Count; k++)
                {
                    BLL.Loger.Log4Net.Info("准备标红字段");
                    BLL.Loger.Log4Net.Info("设置标红字段，第" + tagRed[k] + "个");

                    worksheet.get_Range(excel.Cells[2, tagRed[k]], excel.Cells[2, tagRed[k]]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);//设置字体颜色 
                    BLL.Loger.Log4Net.Info("准备标红字段结束");
                }

                //worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[rowNumber + 2, columnNumber]).Columns.AutoFit();//设置单元格宽度为自适应
                range = worksheet.get_Range(excel.Cells[2, 1], excel.Cells[rowNumber + 1, columnNumber]);
                //range.NumberFormat = "@";//设置单元格为文本格式
                BLL.Loger.Log4Net.Info("excel数据赋值到range");
                range.Value2 = objData;
                //worksheet.get_Range(excel.Cells[2, 1], excel.Cells[rowNumber + 1, 1]).NumberFormat = "yyyy-m-d h:mm";

                string Version;//excel版本号 
                Version = excel.Version;//获取你使用的excel 的版本号 
                BLL.Loger.Log4Net.Info("excel的版本号为：" + Version);
                if (Convert.ToDouble(Version) < 12)//You use Excel 97-2003
                {
                    FormatNum = -4143;
                }
                else//you use excel 2007 or later
                {
                    FormatNum = 56;
                }
                BLL.Loger.Log4Net.Info("生成模板准备数据完毕.开始生成：");

            }
            catch (Exception ex)
            {
                msg = "生成模板出现异常，请再次生成";
                createFileIsOk = false;
                BLL.Loger.Log4Net.Info("生成模板时报错：" + ex.Message);
            }
            finally
            {
                workbook.SaveAs(FileName + ".xls", FormatNum);
                BLL.Loger.Log4Net.Info("生成模板结束.路径" + FilePath + ",文件名：" + FileName + ".xls");
                workbook.Close();
                excel.Quit();
                workbook = null;
                worksheet = null;
                excel = null;
            }

            return true;
        }

        public HSSFWorkbook DataTableToExcel(DataTable dt, string sheetName, string headerText)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet(sheetName);

            #region 设置文件属性信息
            ArrayList tagRed = new ArrayList();

            //创建一个摘要信息实体。
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Title = headerText;
            si.Subject = headerText;
            si.CreateDateTime = DateTime.Now;
            hssfworkbook.SummaryInformation = si;


            ICellStyle dateStyle = hssfworkbook.CreateCellStyle();
            IDataFormat format = hssfworkbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            #endregion


            #region 获取列名
            List<string> columnNameArry = new List<string>();
            DataTable dt_TemptColumn = BLL.TField.Instance.GetTemptColumnNameByTableName(ttCode);
            int tempt_Count = dt_TemptColumn.Columns.Count;

            int excelColumn_Count = 0;
            for (int k = 0; k < tempt_Count; k++)
            {
                string column_Name = dt_TemptColumn.Columns[k].ToString();
                string[] columns = column_Name.Split('_');

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"Tempf.*");
                bool m = reg.IsMatch(column_Name);

                if (m)
                {

                    string tfDesName = string.Empty;//字段名

                    //需要特殊处理的列
                    if (columns.Length >= 2)
                    {
                        //如果是 推荐活动字段列 则不生成EXCEL模板列
                        if (columns[1] == "Activity")
                        {
                            BLL.Loger.Log4Net.Info("DataTable正在生成，推荐活动字段不生成列名!");
                            continue;
                        }

                        string addDes = string.Empty;

                        //如果为Province且有columns有三个，则表示是省份名；
                        if (columns[1] == "Province" && columns.Length == 3)
                        {
                            addDes = "（省）";
                        }
                        //如果为City且有columns有三个，则表示是城市名；
                        else if (columns[1] == "City" && columns.Length == 3)
                        {
                            addDes = "（市）";
                        }
                        //如果为Country且有columns有三个，则表示是县名；
                        else if (columns[1] == "Country" && columns.Length == 3)
                        {
                            addDes = "（县）";
                        }
                        //如果为startdata或starttime，则表示是开始日期/时间段；
                        else if (columns[1] == "startdata" || columns[1] == "starttime")
                        {
                            addDes = "（起）";
                        }
                        //如果为enddata或endtime，则表示是结束日期/时间段；
                        else if (columns[1] == "enddata" || columns[1] == "endtime")
                        {
                            addDes = "（止）";
                        }
                        else if (columns[1] == "XDBrand" && columns.Length == 3)
                        {
                            addDes = "（品牌）";
                        }
                        else if (columns[1] == "XDSerial" && columns.Length == 3)
                        {
                            addDes = "（车型）";
                        }
                        else if (columns[1] == "YXBrand" && columns.Length == 3)
                        {
                            addDes = "（品牌）";
                        }
                        else if (columns[1] == "YXSerial" && columns.Length == 3)
                        {
                            addDes = "（车型）";
                        }
                        else if (columns[1] == "CSBrand" && columns.Length == 3)
                        {
                            addDes = "（品牌）";
                        }
                        else if (columns[1] == "CSSerial" && columns.Length == 3)
                        {
                            addDes = "（车型）";
                        }

                        //columns.Length == 3：是判断是前面几项 还包括radio、check、select
                        //addDes != string.Empty：如果是时间段/点columns.Length =2，所以加上这个条件
                        if (columns.Length == 3 || addDes != string.Empty)
                        {
                            DataTable dt_TField = BLL.TField.Instance.GetTFieldTableByTFName(columns[0]);
                            tfDesName = dt_TField.Rows[0]["TFDesName"].ToString() + addDes;
                            columnNameArry.Add(tfDesName);

                            //如果导入字段不能为空，则插入tagRed中
                            if (dt_TField.Rows[0]["TFInportIsNull"].ToString() == "0")
                            {
                                tagRed.Add(tfDesName);
                            }
                            excelColumn_Count++;
                        }

                    }

                    else
                    {
                        DataTable dt_TField = BLL.TField.Instance.GetTFieldTableByTFName(columns[0]);
                        tfDesName = dt_TField.Rows[0]["TFDesName"].ToString();
                        columnNameArry.Add(tfDesName);
                        //如果导入字段不能为空，则插入tagRed中
                        if (dt_TField.Rows[0]["TFInportIsNull"].ToString() == "0")
                        {
                            tagRed.Add(tfDesName);
                        }
                        excelColumn_Count++;

                        BLL.Loger.Log4Net.Info("DataTable正在生成，列名：" + tfDesName);
                    }
                }
            }
            #endregion

            #region 设置列的宽度
            int[] colWidth = new int[columnNameArry.Count];
            for (int i = 0; i < columnNameArry.Count; i++)
            {
                colWidth[i] = Encoding.GetEncoding(936).GetBytes(columnNameArry[i]).Length;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < columnNameArry.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dt.Rows[i][j].ToString()).Length;
                    if (intTemp > colWidth[j])
                    {
                        colWidth[j] = intTemp;
                    }
                }
            }
            #endregion

            int rowIndex = 0;
            foreach (DataRow row in dt.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = hssfworkbook.CreateSheet(sheetName + ((int)rowIndex / 65535).ToString());
                    }

                    #region 表头及样式
                    //if (!string.IsNullOrEmpty(headerText))
                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(headerText);

                        ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.CENTER;
                        IFont font = hssfworkbook.CreateFont();
                        font.FontHeightInPoints = 14;
                        font.Boldweight = 400;
                        font.Color = HSSFColor.RED.index;

                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;
                        //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1)); 
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));
                    }
                    #endregion

                    #region 列头及样式
                    {
                        //HSSFRow headerRow = sheet.CreateRow(1); 
                        IRow headerRow;
                        //if (!string.IsNullOrEmpty(headerText))
                        //{
                        //    headerRow = sheet.CreateRow(0);
                        //}
                        //else
                        //{
                        headerRow = sheet.CreateRow(1);
                        //}

                        for (int i = 0; i < columnNameArry.Count; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(columnNameArry[i]);
                            ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                            headStyle.Alignment = HorizontalAlignment.CENTER;

                            IFont font = hssfworkbook.CreateFont();
                            font.FontHeightInPoints = 10;
                            font.Boldweight = 700;
                            if (tagRed.Contains(columnNameArry[i]))
                            {
                                font.Color = HSSFColor.RED.index;
                                headStyle.SetFont(font);
                            }
                            else
                            {
                                headStyle.SetFont(font);
                            }
                            headerRow.GetCell(i).CellStyle = headStyle;
                            //设置列宽 
                            if ((colWidth[i] + 1) * 256 > 30000)
                            {
                                sheet.SetColumnWidth(i, 10000);
                            }
                            else
                            {
                                sheet.SetColumnWidth(i, (colWidth[i] + 1) * 256);
                            }
                        }
                        /* 
                        foreach (DataColumn column in dtSource.Columns) 
                        { 
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName); 
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle; 
   
                            //设置列宽    
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256); 
                        } 
                         * */
                    }
                    #endregion
                    //if (!string.IsNullOrEmpty(headerText))
                    //{
                    //    rowIndex = 1;
                    //}
                    //else
                    //{
                    rowIndex = 2;
                    //}

                }
                #endregion

                rowIndex++;
            }


            return hssfworkbook;
        }

        public void Write(DataTable table, string fileName)
        {
            string path = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            HSSFWorkbook hssfworkbook = DataTableToExcel(table, "数据模版", "红色标记的字段为导入时必填项");
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = WriteToStream(hssfworkbook).GetBuffer().ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();

                data = null;
            }
        }

        private static MemoryStream WriteToStream(HSSFWorkbook hssfworkbook)
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            return file;
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