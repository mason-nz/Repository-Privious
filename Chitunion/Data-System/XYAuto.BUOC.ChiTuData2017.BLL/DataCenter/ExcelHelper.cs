
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using XYAuto.Utils.Config;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter
{

    public class ExcelHelper
    {
        private static readonly string fielPath = ConfigurationUtil.GetAppSettingValue("UploadFilePath");

        /// <summary>      
        /// DataTable导出到Excel文件      
        /// </summary>      
        /// <param name="dtSource">源DataTable</param>      
        /// <param name="strHeaderText">表头文本</param>      
        /// <param name="strFileName">保存位置</param>   
        /// <param name="strSheetName">工作表名称</param>   
        /// <Author>CallmeYhz 2015-11-26 10:13:09</Author>      
        public static string ExportEcel(DataTable dtSource, string strFileName, Dictionary<string, string> titleDic)
        {

            string filelPath = Path.Combine($"\\UploadFiles\\{DateTime.Now.Year}\\{DateTime.Now.Month}\\{DateTime.Now.Day}\\");

            string CarMapIP_ExcelPath = Path.Combine(fielPath + filelPath);

            if (!Directory.Exists(CarMapIP_ExcelPath))
                Directory.CreateDirectory(CarMapIP_ExcelPath);
            //重新生成文件名称
            strFileName = $"{ strFileName + Guid.NewGuid()}.xlsx";

            CarMapIP_ExcelPath = CarMapIP_ExcelPath + strFileName;
            if (titleDic != null)
            {
                using (MemoryStream ms = Export(dtSource, titleDic))
                {
                    using (FileStream fs = new FileStream(CarMapIP_ExcelPath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                }
            }
            else
            {
                using (MemoryStream ms = Export(dtSource))
                {
                    using (FileStream fs = new FileStream(CarMapIP_ExcelPath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                }
            }
            return Path.Combine( filelPath + strFileName).Replace("\\", "/");
        }


        /// <summary>
        /// DataTable导出Excel
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="strSheetName"></param>
        /// <param name="oldColumnNames"></param>
        /// <param name="newColumnNames"></param>
        /// <returns></returns>
        public static MemoryStream Export(DataTable dtSource, Dictionary<string, string> titleDic)
        {

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = null;
            if (workbook != null)
            {
                sheet = workbook.CreateSheet();
            }
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            #region 取得列宽
            int[] arrColWidth = new int[titleDic.Count];
            int m = 0;
            foreach (var item in titleDic)
            {
                arrColWidth[m] = Encoding.GetEncoding(936).GetBytes(item.Value).Length;
                m++;
            }
            #endregion

            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 表头
                if (rowIndex == 0)
                {


                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(0);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        int dicNum = 0;
                        foreach (var dic in titleDic)
                        {
                            headerRow.CreateCell(dicNum).SetCellValue(dic.Value);
                            headerRow.GetCell(dicNum).CellStyle = headStyle;
                            //设置列宽   
                            sheet.SetColumnWidth(dicNum, (arrColWidth[dicNum] + 1) * 600);
                            dicNum++;
                        }
                    }
                    #endregion

                    rowIndex = 1;
                }
                #endregion


                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);
                int rowLin = 0;
                foreach (var dicItem in titleDic)
                {
                    //for (int i = 0; i < dtSource.Columns.Count; i++)
                    //{
                    ICell newCell = dataRow.CreateCell(rowLin);

                    string drValue = row[dicItem.Key].ToString();

                    switch (dtSource.Columns[dicItem.Key].DataType.ToString())
                    {
                        case "System.String"://字符串类型      
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型      
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示      
                            break;
                        case "System.Boolean"://布尔型      
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型      
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型      
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理      
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                    rowLin++;
                }
                #endregion

                rowIndex++;
            }


            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                sheet = null;
                workbook = null;
                return ms;
            }
        }

        public static MemoryStream Export(DataTable dtSource)
        {

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = null;
            if (workbook != null)
            {
                sheet = workbook.CreateSheet();
            }


            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            #region 取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];

            for (int i = 0; i < dtSource.Columns.Count; i++)
            {
                arrColWidth[i] = Encoding.GetEncoding(936).GetBytes(dtSource.Columns[i].ColumnName).Length;

            }

            #endregion

            int rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 表头
                if (rowIndex == 0)
                {


                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(0);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);

                        for (int i = 0; i < dtSource.Columns.Count; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(dtSource.Columns[i].ColumnName);
                            headerRow.GetCell(i).CellStyle = headStyle;
                            //设置列宽   
                            sheet.SetColumnWidth(i, (arrColWidth[i] + 1) * 600);

                        }
                    }
                    #endregion

                    rowIndex = 1;
                }
                #endregion


                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);

                //foreach (var dicItem in titleDic)
                //{
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    ICell newCell = dataRow.CreateCell(i);

                    string drValue = row[i].ToString();

                    switch (dtSource.Columns[i].DataType.ToString())
                    {
                        case "System.String"://字符串类型      
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型      
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示      
                            break;
                        case "System.Boolean"://布尔型      
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型      
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型      
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理      
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                #endregion

                rowIndex++;
            }


            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                sheet = null;
                workbook = null;
                return ms;
            }
        }

        /// <summary>   
        /// 导出DataTable到Excel   
        /// </summary>   
        /// <param name="dtSource">要导出的DataTable</param>   
        /// <param name="strHeaderText">标题文字</param>   
        /// <param name="strFileName">文件名，包含扩展名</param>   
        /// <param name="strSheetName">工作表名</param>   
        /// <param name="oldColumnNames">要导出的DataTable列数组</param>   
        /// <param name="newColumnNames">导出后的对应列名</param>   
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName, string strSheetName, string[] oldColumnNames, string[] newColumnNames)
        {
            HttpContext curContext = HttpContext.Current;

            // 设置编码和附件格式      
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

            // curContext.Response.BinaryWrite(Export(dtSource, strHeaderText, strSheetName, oldColumnNames, newColumnNames).GetBuffer());
            curContext.Response.End();
        }


    }
}

