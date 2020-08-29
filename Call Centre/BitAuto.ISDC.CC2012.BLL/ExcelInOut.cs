using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Web.UI.WebControls;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using System.Web;


namespace BitAuto.ISDC.CC2012.BLL
{
    public class ExcelInOut
    {
        #region 导出,操作excel

        /// <summary>
        /// 将数据集中的数据导出到EXCEL文件
        /// </summary>
        /// <param name="dt">需要导出的DataTable</param>
        /// <param name="FileName">文件名</param>
        /// <param name="FilePath">文件存放路径(可为空)</param>
        /// <param name="tagRed">需要标红的列(可为空)</param>
        /// <param name="isShowExcle">是否显示该tagRed文件(可为空)</param>
        /// <returns></returns>
        public static bool DataSetToExcel(System.Data.DataTable dt, string FileName, string FilePath, bool isShowExcle, ArrayList tagRed, out string msg)
        {
            msg = string.Empty;

            try
            {
                System.Data.DataTable dataTable = dt;
                int rowNumber = dataTable.Rows.Count;//不包括字段名
                int columnNumber = dataTable.Columns.Count;
                int colIndex = 0;

                //建立Excel对象
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                if (FilePath != string.Empty)
                {
                    excel.DefaultFilePath = FilePath;
                }
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
                excel.Visible = isShowExcle;
                Microsoft.Office.Interop.Excel.Range range;
                //生成字段名称
                //foreach (DataColumn col in dataTable.Columns)
                //{
                //    colIndex++;
                //    if (colIndex == 1)
                //    {
                //        excel.Cells[1, colIndex] = col.ColumnName;
                //    }
                //    else
                //    {
                //        //第一行是提示；标红为导入时必填项；需要清空，否则在合并单元格时有问题
                //        excel.Cells[1, colIndex] = "";
                //    }
                //}

                object[,] objData = new object[rowNumber, columnNumber];

                for (int r = 0; r < rowNumber; r++)
                {
                    for (int c = 0; c < columnNumber; c++)
                    {
                        objData[r, c] = dataTable.Rows[r][c];
                    }
                }

                // 写入Excel 

                //列 合并单元格
                //worksheet.get_Range(excel.Cells[1, 1], excel.Cells[1, dt.Columns.Count]).Merge(0);
                //设置字体颜色，非空字段要标红
                for (int k = 0; k < tagRed.Count; k++)
                {
                    worksheet.get_Range(excel.Cells[2, tagRed[k]], excel.Cells[2, tagRed[k]]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);//设置字体颜色 
                }

                range = worksheet.get_Range(excel.Cells[1, 1], excel.Cells[rowNumber, columnNumber]);
                //range.NumberFormat = "@";//设置单元格为文本格式
                range.Value2 = objData;

                int FormatNum;//保存excel文件的格式 
                string Version;//excel版本号 
                Version = excel.Version;//获取你使用的excel 的版本号 
                if (Convert.ToDouble(Version) < 12)//You use Excel 97-2003
                {
                    FormatNum = -4143;
                }
                else//you use excel 2007 or later
                {
                    FormatNum = 56;
                }
                if (FileName != string.Empty)
                {
                    workbook.SaveAs(FileName + ".xls", FormatNum);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            finally
            {
                Process[] procs = Process.GetProcessesByName("Excel");
                foreach (Process pro in procs)
                {
                    //本地测试，删除excel进程不可用（需要管理员权限）；更新到测试环境和正式环境需要使用
                    pro.Kill();
                }
            }

            return true;
        }


        #endregion

        #region 导出

        /// <summary>
        /// 导出dbf格式
        /// </summary>
        /// <param name="path">路径(这里只有导出dbf用到了,其它格式的导出没有用到path)</param>
        /// <param name="fileName">文件名(这里只有导出dbf用到了，其它格式的导出没有用到fileName)</param>
        /// <param name="dataSet">导出的集合</param>
        /// <param name="Leixing">导出类型1:DBF格式   2:XML格式    3:TXT格式   4:Microsoft.Office.Interop.Excel格式</param>
        /// <returns></returns>
        public static void ExportData(string fileName, DataSet dataSet, string Leixing)
        {
            string zhuangtai = "";

            #region EXCEL导出

            if (Leixing == "4")
            {
                CreateEXCEL(dataSet, fileName);
            }

            #endregion

            #region DBF格式导出
            if (Leixing == "1")
            {
                fileName += ".dbf";
                string path = System.Web.HttpContext.Current.Server.MapPath("~/dbf/");
                ArrayList list = new ArrayList();//用集合保存需导出的字段

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);//创建该文件夹 
                }

                if (File.Exists(path + "xs.dbf"))
                {
                    File.Delete(path + "xs.dbf");
                }

                string createSql = "create table " + "xs.dbf" + " (";

                foreach (DataColumn dc in dataSet.Tables[0].Columns)
                {
                    string fieldName = dc.ColumnName;

                    string type = dc.DataType.ToString();

                    type = "varchar(" + dc.MaxLength.ToString() + ")";

                    createSql = createSql + fieldName + " " + type + ",";

                    list.Add(fieldName);
                }

                createSql = createSql.Substring(0, createSql.Length - 1) + ")";

                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";
                OleDbConnection con = new OleDbConnection(conStr);

                OleDbCommand cmd = new OleDbCommand();

                try
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = createSql;
                    cmd.ExecuteNonQuery();//创建新表

                    foreach (DataRow row in dataSet.Tables[0].Rows)//将数据导出到DBF文件中
                    {
                        string insertSql = "insert into " + "xs.dbf" + " values(";
                        for (int i = 0; i < list.Count; i++)
                        {
                            insertSql = insertSql + "'" + row[list[i].ToString()].ToString() + "',";
                        }
                        insertSql = insertSql.Substring(0, insertSql.Length - 1) + ")";
                        cmd.CommandText = insertSql;
                        cmd.ExecuteNonQuery();
                    }

                    StreamReader objReader = new StreamReader(path + "xs.dbf");
                    zhuangtai = objReader.ReadToEnd();
                    objReader.Close();
                    objReader.Dispose();
                    File.Delete(path + "xs.dbf");
                }

                catch (Exception ex)
                {
                    zhuangtai = ex.Message;
                }

                finally
                {
                    con.Close();
                }
            }
            #endregion

            #region XML格式导出
            if (Leixing == "2")
            {
                fileName += ".xml";
                try
                {
                    zhuangtai = dataSet.GetXml();
                }
                catch (Exception ex)
                {
                    zhuangtai = ex.Message;
                }
            }
            #endregion

            #region TXT格式导出
            if (Leixing == "3")
            {
                fileName += ".txt";
                try
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < dataSet.Tables[0].Columns.Count; i++)
                    {
                        sb.Append(dataSet.Tables[0].Columns[i].ColumnName + ",");
                    }
                    sb.Append("\r\n");

                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < dataSet.Tables[0].Columns.Count; j++)
                        {
                            sb.Append(dataSet.Tables[0].Rows[i][j] + ",");
                        }
                        sb.Append("\r\n");
                    }

                    zhuangtai = sb.ToString();
                }
                catch (Exception ex)
                {
                    zhuangtai = ex.Message;
                }
            }
            #endregion

            #region XML和TXT格式导出
            RenderToDaoChu(zhuangtai, fileName);
            #endregion
        }

        /// <summary>
        /// 导出EXCEL（HTML格式的）
        /// </summary>
        /// <param name="ds">要导出的DataSet</param>
        /// <param name="fileName"></param>
        public static void CreateEXCEL(DataSet ds, string fileName)
        {
            System.Web.UI.WebControls.DataGrid dg = new DataGrid();
            dg.DataSource = ds;
            dg.DataBind();

            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "GB2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8) + ";charset=GB2312");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
            Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            System.Web.UI.WebControls.DataGrid DG = dg;
            DG.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        /// <summary>
        /// 导出文本
        /// </summary>
        /// <param name="exportStr">导出的文本</param>
        /// <param name="fileName">导出的文件名</param>
        private static void RenderToDaoChu(string exportStr, string fileName)
        {
            System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
            if ("" == exportStr)
            {
                return;
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "GB2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ";charset=GB2312");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
            Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。

            System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

            oHtmlTextWriter.Write(exportStr);
            Response.Write(oStringWriter.ToString());

            Response.End();

        }

        /// <summary>
        /// 导出到Microsoft.Office.Interop.Excel(Microsoft.Office.Interop.Excel格式的)
        /// </summary>
        /// <param name="tmpDataTable"></param>
        /// <param name="FilePath"></param>
        /// <param name="strFileName"></param>
        public static void ExportDataExcel(System.Data.DataTable tmpDataTable, string FilePath, string strFileName, ref string err)
        {
            int rowNum = tmpDataTable.Rows.Count;
            int columnNum = tmpDataTable.Columns.Count;
            int rowIndex = 1;
            int columnIndex = 0;
            err = "";

            try
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlApp.DefaultFilePath = FilePath;
                xlApp.DisplayAlerts = true;
                xlApp.SheetsInNewWorkbook = 1;
                Microsoft.Office.Interop.Excel.Workbook xlBook = xlApp.Workbooks.Add(true);

                //将DataTable的列名导入Microsoft.Office.Interop.Excel表第一行
                foreach (DataColumn dc in tmpDataTable.Columns)
                {
                    columnIndex++;
                    xlApp.Cells[rowIndex, columnIndex] = dc.ColumnName;
                    //xlApp.get_Range(xlApp.Cells[rowIndex, columnIndex]).Font.Color = "red";
                }
                //将DataTable中的数据导入Microsoft.Office.Interop.Excel中
                for (int i = 0; i < rowNum; i++)
                {
                    rowIndex++;
                    columnIndex = 0;
                    for (int j = 0; j < columnNum; j++)
                    {
                        columnIndex++;
                        xlApp.Cells[rowIndex, columnIndex] = tmpDataTable.Rows[i][j].ToString();
                    }
                }
                xlBook.SaveCopyAs(strFileName + ".xls");

            }
            catch (Exception ex)
            {
                err = ex.Message.ToString();
            }
            finally
            {
                Process[] procs = Process.GetProcessesByName("Excel");
                foreach (Process pro in procs)
                {
                    //pro.Kill();
                }
            }
        }



        #endregion

        #region 导入

        /// <summary>
        /// 从EXCEL中读取数据
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public DataSet GetDate(string fileFullName, ref string err)
        {
            DataSet ds = new DataSet();

            try
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet." +
                                      "OLEDB.4.0;Extended Properties=\"Microsoft.Office.Interop.Excel 8.0\";Data Source=" + fileFullName))
                {
                    conn.Open();
                    System.Data.DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string tableName = dt.Rows[0][2].ToString().Trim();

                    string sql = "select * from [" + tableName + "]";
                    OleDbDataAdapter myCommand = new OleDbDataAdapter(sql, conn);

                    myCommand.Fill(ds, "[Sheet1$]");

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

                err = ex.Message.ToString();
            }

            return ds;
        }

        /// <summary>
        /// 从Microsoft.Office.Interop.Excel中获取数据
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public DataSet GetDataFromExcel(string fileFullName, ref string err)
        {
            DataSet ds = new DataSet();

            try
            {
                // Start a new workbook in Microsoft.Office.Interop.Excel.
                Microsoft.Office.Interop.Excel.Application objexcel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbooks objBooks = (Microsoft.Office.Interop.Excel.Workbooks)objexcel.Workbooks;
                Object o = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel._Workbook objBook = objBooks.Open(fileFullName, o, o, o, o, o, o, o, o, o, o, o, o);

                // Add data to cells of the first worksheet in the new workbook.
                Microsoft.Office.Interop.Excel.Sheets objSheets = (Microsoft.Office.Interop.Excel.Sheets)objBook.Worksheets;
                int count = objSheets.Count;
                if (count <= 0)
                    return ds;

                Microsoft.Office.Interop.Excel._Worksheet objSheet;

                for (int i = 1; i <= count; i++)
                {
                    objSheet = (Microsoft.Office.Interop.Excel._Worksheet)(objSheets.get_Item(i));
                    System.Data.DataTable dt = FillExcelData(objSheet);  //在这里获取表格中的数据
                    ds.Tables.Add(dt);
                }

                objBooks.Application.Quit();
                objexcel.Quit();
                objSheets = null;
                objSheet = null;
                objBooks = null;
                objBook = null;
                objexcel = null;
            }
            catch (Exception ex)
            {
                err = "从Microsoft.Office.Interop.Excel中获取数据过程出错！" + ex.Message.ToString();
            }
            finally
            {
                //杀掉Microsoft.Office.Interop.Excel进程
                Process[] procs = Process.GetProcessesByName("Microsoft.Office.Interop.Excel.exe");
                foreach (Process pro in procs)
                {
                    pro.Kill();
                }
                GC.Collect();
            }

            return ds;

        }

        private System.Data.DataTable FillExcelData(Microsoft.Office.Interop.Excel._Worksheet m_objSheet)
        {
            System.Data.DataTable Tables = new System.Data.DataTable(m_objSheet.Name);
            string tmp = "";

            Microsoft.Office.Interop.Excel.Range m_objRange = m_objSheet.UsedRange;//m_objSheet.Cells.CurrentRegion;
            int col = m_objRange.Columns.Count;
            int row = m_objRange.Rows.Count;

            int trueCol = 0;


            for (int j = 1; j <= col; j++)
            {
                Microsoft.Office.Interop.Excel.Range rx = m_objSheet.Cells.get_Range(GetAscii(j) + "1", System.Reflection.Missing.Value);

                tmp = "Col" + j.ToString();
                if (rx.Value != null)
                {
                    trueCol += 1;
                    tmp = rx.Value.ToString();
                }
                if (Tables.Columns.Contains(tmp))
                {
                    tmp = rx.Value.ToString() + j.ToString();
                }
                Tables.Columns.Add(new DataColumn(tmp, System.Type.GetType("System.String")));
            }

            for (int i = 2; i <= row; i++)
            {
                DataRow row1 = Tables.NewRow();
                for (int j = 1; j <= trueCol; j++)
                {
                    Microsoft.Office.Interop.Excel.Range rx = m_objSheet.Cells.get_Range(GetAscii(j) + i.ToString(), System.Reflection.Missing.Value);
                    if (rx.Value == null)
                        row1[j - 1] = "";
                    else
                    {
                        tmp = rx.Value.ToString();
                        tmp = tmp.Trim();
                        row1[j - 1] = tmp;
                    }
                }
                Tables.Rows.Add(row1);
            }
            return Tables;
        }

        private string GetAscii(int i)
        {
            i -= 1;
            int ii = i / 26;
            int left = i % 26;
            string last = "";
            if (ii != 0)
                last = GetAscii(ii);
            left += 65;
            char c = (char)left;
            return last + c.ToString();
        }

        /// <summary>
        /// 上传保存文件
        /// </summary>
        /// <param name="myfileBrowser"></param>
        /// <param name="filename"></param>
        /// <param name="err"></param>
        public void SaveExcelFile(System.Web.UI.HtmlControls.HtmlInputFile myfileBrowser, ref string fileFullName, ref string err)
        {

            HttpPostedFile hpf = myfileBrowser.PostedFile;

            if (fileFullName.Split('.')[fileFullName.Split('.').Length - 1].ToUpper() != "XLS")
            {
                err = "文件格式不正确，应该为 .XSL文件";
                return;
            }

            #region 对于没有建立相应文件夹的由系统给于建立

            FileInfo finfo = new FileInfo(fileFullName);

            if (!finfo.Directory.Exists)
            {
                try
                {
                    Directory.CreateDirectory(finfo.DirectoryName);
                }
                catch (Exception ex)
                {
                    err = "系统没有建立用户的上传文件存放的文件夹，并且在尝试建立时失败！应该建立" + finfo.DirectoryName + "文件夹！" + ex.Message;
                    return;
                }
            }

            #endregion

            try
            {
                hpf.SaveAs(fileFullName);
            }
            catch
            {
                err = "文件保存到服务器过程中出错";
            }
        }

        #endregion
    }
}
