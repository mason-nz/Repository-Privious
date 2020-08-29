using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.Chitunion2018.StaticWechatData
{
    public class NpoiExcel
    {
        public static bool ExportToExcel(Dictionary<string, DataTable> DicDt, string fileName, out string backFilePath)
        {
            backFilePath = "";
            try
            {
                string rootPath = ConfigurationManager.AppSettings["rootPath"] == "" ? "D:\\WeChatData" : ConfigurationManager.AppSettings["rootPath"];
                string pagePath = string.Format("{0}\\{1}\\{2}\\{3}\\{4}",
                    rootPath,
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day,
                    DateTime.Now.Hour);

                if (!Directory.Exists(pagePath))
                    Directory.CreateDirectory(pagePath);
                backFilePath = pagePath + "\\" + fileName;
                HSSFWorkbook workbook = new HSSFWorkbook();
                foreach (var itemTable in DicDt)
                {
                    DataTable Table = itemTable.Value;
                    if (Table != null && Table.Rows.Count > 0)
                    {
                        ISheet sheet = workbook.CreateSheet(itemTable.Key);
                        //填充表头
                        IRow dataRow = sheet.CreateRow(0);
                        for (int i = 0; i < Table.Columns.Count; i++)
                            dataRow.CreateCell(i).SetCellValue(Table.Columns[i].ColumnName);
                        #region 填充行数据
                        int rownumber = 1;
                        foreach (DataRow item in Table.Rows)
                        {
                            dataRow = sheet.CreateRow(rownumber);
                            for (int i = 0; i < Table.Columns.Count; i++)
                            {
                                dataRow.CreateCell(i).SetCellValue(item[i] == null ? "" : item[i].ToString());
                            }
                            rownumber++;
                        }
                        #endregion
                    }
                }
                if (workbook.NumberOfSheets > 0)
                {
                    using (FileStream fs = new FileStream(backFilePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(fs);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error("生成excel错误", ex);
                return false;
            }


        }
    }
}
