using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Common;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia.Dto;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia
{
    public class ExcelHelper
    {
        public static MemoryStream Export(Dictionary<string, BasicResultDto> query)
        {

            BasicResultDto WX = query["微信"];

            IWorkbook workbook = new XSSFWorkbook();

            #region 微信
            ISheet WeiXinSheet;
            if (WX.List != null)
            {

                WeiXinSheet = workbook.CreateSheet("微信");
                IRow row0 = WeiXinSheet.CreateRow(0);
                row0.CreateCell(0).SetCellValue("类型");
                row0.CreateCell(1).SetCellValue("账号");
                row0.CreateCell(2).SetCellValue("分类");
                row0.CreateCell(3).SetCellValue("粉丝数");
                row0.CreateCell(4).SetCellValue("平均阅读数");
                row0.CreateCell(5).SetCellValue("位置");
                row0.CreateCell(6).SetCellValue("发布参考价");
                row0.CreateCell(7).SetCellValue("原创+发布参考价");
                List<WeiXinListModel> WxList = (List<WeiXinListModel>)WX.List;
                int num = 0;
                for (int x = 0; x < WxList.Count; x++)
                {
                    num++;
                    IRow row = WeiXinSheet.CreateRow(num);
                    row.CreateCell(0).SetCellValue("微信");
                    row.CreateCell(1).SetCellValue(WxList[x].WxNumber);
                    row.CreateCell(2).SetCellValue(WxList[x].CategoryName);
                    row.CreateCell(3).SetCellValue(ConvertHelper.ToNumCount(WxList[x].FansCount));
                    row.CreateCell(4).SetCellValue(WxList[x].ReadNum > 10000 ? "10万+" : WxList[x].ReadNum.ToString());
                    if (WxList[x].Position.Count > 0)
                    {
                        for (int t = 0; t < WxList[x].Position.Count; t++)
                        {
                            if (t > 0)
                            {
                                num++;
                                IRow rowtem = WeiXinSheet.CreateRow(num);
                                rowtem.CreateCell(5).SetCellValue(WxList[x].Position[t].PositionName);
                                rowtem.CreateCell(6).SetCellValue(ConvertHelper.ToPrice(WxList[x].Position[t].IssuePrice));
                                rowtem.CreateCell(7).SetCellValue(ConvertHelper.ToPrice(WxList[x].Position[t].TotalPrice));

                            }
                            else
                            {
                                row.CreateCell(5).SetCellValue(WxList[x].Position[t].PositionName);
                                row.CreateCell(6).SetCellValue(ConvertHelper.ToPrice(WxList[x].Position[t].IssuePrice));
                                row.CreateCell(7).SetCellValue(ConvertHelper.ToPrice(WxList[x].Position[t].TotalPrice));
                            }
                        }
                    }
                }
            }
            #endregion


            #region 微博
            BasicResultDto WB = query["微博"];
            ISheet WeiBoSheet;
            if (WB.List != null)
            {

                WeiBoSheet = workbook.CreateSheet("微博");
                IRow row0 = WeiBoSheet.CreateRow(0);
                row0.CreateCell(0).SetCellValue("类型");
                row0.CreateCell(1).SetCellValue("账号");
                row0.CreateCell(2).SetCellValue("分类");
                row0.CreateCell(3).SetCellValue("粉丝数");
                row0.CreateCell(4).SetCellValue("平均转发数");
                row0.CreateCell(5).SetCellValue("平均评论数");
                row0.CreateCell(6).SetCellValue("平均点赞数");
                row0.CreateCell(7).SetCellValue("直发参考价");
                row0.CreateCell(8).SetCellValue("转发参考价");
                List<WeiBoModel> WbList = (List<WeiBoModel>)WB.List;
                int num = 0;
                for (int x = 0; x < WbList.Count; x++)
                {
                    IRow row = WeiBoSheet.CreateRow(x + 1);
                    row.CreateCell(0).SetCellValue("微博");
                    row.CreateCell(1).SetCellValue(WbList[x].Number);
                    row.CreateCell(2).SetCellValue(WbList[x].CategoryName);
                    row.CreateCell(3).SetCellValue(ConvertHelper.ToNumCount(WbList[x].FansCount));
                    row.CreateCell(4).SetCellValue(WbList[x].ForwardAvg > 10000 ? "10万+" : WbList[x].ForwardAvg.ToString());
                    row.CreateCell(5).SetCellValue(WbList[x].CommentAvg > 10000 ? "10万+" : WbList[x].CommentAvg.ToString());
                    row.CreateCell(6).SetCellValue(WbList[x].LikeAvg > 10000 ? "10万+" : WbList[x].LikeAvg.ToString());
                    row.CreateCell(7).SetCellValue(ConvertHelper.ToPrice(WbList[x].DirectPrice));

                    row.CreateCell(8).SetCellValue(ConvertHelper.ToPrice(WbList[x].ForwardPrice));

                }
            }
            #endregion

            #region APP
            BasicResultDto APP = query["APP"];
            ISheet APPSheet;
            if (APP.List != null)
            {

                APPSheet = workbook.CreateSheet("APP");
                IRow row0 = APPSheet.CreateRow(0);
                row0.CreateCell(0).SetCellValue("类型");
                row0.CreateCell(1).SetCellValue("媒体名称");
                row0.CreateCell(2).SetCellValue("媒体介绍");
                row0.CreateCell(3).SetCellValue("分类");
                row0.CreateCell(4).SetCellValue("日活");
                row0.CreateCell(5).SetCellValue("用户总量");
                row0.CreateCell(6).SetCellValue("是否可监测");
                row0.CreateCell(7).SetCellValue("是否可定向");
                List<AppModel> APPList = (List<AppModel>)APP.List;
                for (int x = 0; x < APPList.Count; x++)
                {
                    IRow row = APPSheet.CreateRow(x + 1);
                    row.CreateCell(0).SetCellValue("APP");
                    row.CreateCell(1).SetCellValue(APPList[x].Name);
                    row.CreateCell(2).SetCellValue(APPList[x].Remark);
                    row.CreateCell(3).SetCellValue(APPList[x].CategoryID);
                    row.CreateCell(4).SetCellValue(ConvertHelper.ToLongNumCount(APPList[x].DailyLive));
                    row.CreateCell(5).SetCellValue(ConvertHelper.ToLongNumCount(APPList[x].TotalUser));
                    row.CreateCell(6).SetCellValue(APPList[x].IsMonitor == 1 ? "是" : "否");
                    row.CreateCell(6).SetCellValue(APPList[x].IsLocate == 1 ? "是" : "否");
                }
            }
            #endregion
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                APPSheet = null;
                WeiBoSheet = null;
                workbook = null;
                return ms;
            }
        }
        /// <summary>
        /// 资源管理导出，返回数据流
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="titleDic"></param>
        /// <returns></returns>
        public static MemoryStream ResourcesExport(DataTable dtSource, Dictionary<string, string> titleDic)
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
        public static DataSet ConvertExcelToDataSet(string ExcelPath)
        {
            DataSet ds = new DataSet();
            ExcelPath = ExcelPath.Replace("/", "\\");

            //string CarMapIP_ExcelPath = Path.Combine(ConfigurationManager.AppSettings["UploadFilePath"]) + ExcelPath;
            IWorkbook hssfworkbook;
            using (FileStream file = new FileStream(ExcelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                hssfworkbook = WorkbookFactory.Create(file);
            }
            for (int i = 0; i < 1; i++)
            {
                #region 遍历Sheet
                ISheet sheet = hssfworkbook.GetSheetAt(1);
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
                    for (int k = 0; k < 8; k++)
                    {
                        if (row.GetCell(k) != null)
                        {
                            dataRow[k] = row.GetCell(k).ToString().Replace("\n", "").Replace(" ", "").Replace("\t", "").Trim();
                        }
                    }
                    dt.Rows.Add(dataRow);

                }
                dt.TableName = sheet.SheetName;
                ds.Tables.Add(dt);
                #endregion
            }
            return ds;
        }


        public static bool InsertMediaApp(string excelPath)
        {
            //获取DataSet
            DataSet dataSet = ConvertExcelToDataSet(excelPath);

            string LogoPath = ConfigurationManager.AppSettings["ImgFilePath"];
            var query = (from q in dataSet.Tables[0].AsEnumerable()
                         select new LEAPPTemp
                         {
                             Name = q.Field<string>("媒体名称"),
                             CategoryName = q.Field<string>("行业分类"),
                             DailyLive = !string.IsNullOrEmpty(q.Field<string>("APP日活")) ? ConvertHelper.ToInt32(q.Field<string>("APP日活")) : 0,
                             TotalUser = !string.IsNullOrEmpty(q.Field<string>("用户总量")) ? ConvertHelper.ToInt32(q.Field<string>("用户总量")):0,
                             Remark = q.Field<string>("媒体介绍"),
                             IsMonitor = q.Field<string>("可监测") == "Y" ? true :  false,
                             IsLocate = q.Field<string>("可定向") == "Y" ? true : false,
                             HeadIconURL = !string.IsNullOrEmpty(q.Field<string>("Logo"))? "http://www.chitunion.com/UploadFiles/app_logos/"+ q.Field<string>("Logo"):string.Empty,
                             CreateTime=DateTime.Now
                         }).ToList();

            //转换Table数据
            DataTable table = ListToDataTable(query);
            //批量插入
            bool result = MediaImportDa.Instance.InsertAppMedai(table);

            return result;

        }
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                //throw new Exception("需转换的集合为空");
                return null;
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
    }
}
