using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImport
{
    public class Import
    {
        public DataSet ConvertExcelToDataSet(string[] sheetNames,string CarMapIP_ExcelPath)
        {
            DataSet ds = new DataSet();
            //HSSFWorkbook hssfworkbook;//只能讯取2007以前的
            IWorkbook hssfworkbook;
            using (FileStream file = new FileStream(CarMapIP_ExcelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
    }
}
