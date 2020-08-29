using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.V1_8_3.Dto;
using AutoMapper;

namespace XYAuto.ITSC.Chitunion2017.BLL.V1_8_3
{
    public class IntelligenceADOrderInfo
    {
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
        string LoginPwdKey = Utils.Config.ConfigurationUtil.GetAppSettingValue("LoginPwdKey");
        #region Instance
        public static readonly IntelligenceADOrderInfo Instance = new IntelligenceADOrderInfo();
        #endregion
        public Tuple<string, string> IntelligenceRecommendExport(BLL.V1_8_3.Dto.IRecommendExportDto exportDto, out string msg, string orderID = "")
        {
            msg = string.Empty;
            if (!exportDto.CheckSelfModel(out msg))
                return new Tuple<string, string>(string.Empty, string.Empty);
            string carModel = $"{exportDto.CarBrand}-{exportDto.CarSerial}";
            Dictionary<string, string> dict = new Dictionary<string, string>() {
                {"推广车型：",carModel },
                {"推广预算：",$"￥{exportDto.BudgetTotal}" },
                {"预计投放日期：",$"{exportDto.LaunchTime.Date}" },
                {"集客入口：",string.Format("{0}",exportDto.JKEntrance==true?"带经销商":"不带经销商") }
            };

            string fileName = $"推荐媒体明细{DateTime.Now.ToString("yyyy-MM-dd")}.xlsx";
            if (!string.IsNullOrEmpty(orderID))
                fileName = $"{exportDto.ExcelName.Replace("/", "-").Replace(@"\", "-")}.xlsx";

            string relativeDir = $"/UploadFiles/RecommendExport/{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Hour}";
            string uploadDir = XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("UploadFilePath", false) + relativeDir;
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);
            string filePath = $"{uploadDir}/{fileName}";
            IWorkbook workbook = null;
            FileStream fs = null;
            ISheet sheet = null;

            fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (filePath.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (filePath.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            if (workbook != null)
            {
                sheet = workbook.CreateSheet("媒体列表");
            }
            IRow row = null;
            for (int i = 0; i < dict.Count; i++)
            {
                row = sheet.CreateRow(i);
                row.Height = 30 * 20;
                for (int j = 0; j < 9; j++)
                {
                    ICell cell = row.CreateCell(j);
                    setCellStyle1(workbook, cell);
                    if (j <= 1)
                        cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;//水平对齐

                    if (j == 0)
                        cell.SetCellValue(dict.ToList()[i].Key);

                    if (j == 2)
                    {
                        cell.SetCellValue(dict.ToList()[i].Value);
                        if (i == 1)
                        {
                            IDataFormat format = workbook.CreateDataFormat();
                            cell.CellStyle.DataFormat = format.GetFormat("¥#,##0");
                            cell.SetCellValue((double)exportDto.BudgetTotal);
                        }
                        else if (i == 2)
                        {
                            IDataFormat format = workbook.CreateDataFormat();
                            cell.CellStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
                            cell.SetCellValue(exportDto.LaunchTime);
                        }
                    }

                    if (j == 8)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(i, i, 0, 1));
                        sheet.AddMergedRegion(new CellRangeAddress(i, i, 2, 8));
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                sheet.SetColumnWidth(i, 12 * 256);
            }

            int rowIndex = dict.Count;
            int totalCostPrice = 0;
            //int totalMedias = 0;
            foreach (var areaInfo in exportDto.AreaInfo)
            {
                row = sheet.CreateRow(rowIndex);
                row.Height = 30 * 20;
                //row.RowStyle.FillBackgroundColor= NPOI.HSSF.Util.HSSFColor.Brown.Index;

                for (int j = 0; j < 9; j++)
                {
                    ICell cell = row.CreateCell(j);
                    setCellStyle1(workbook, cell);
                    cell.CellStyle.FillForegroundColor = HSSFColor.Grey25Percent.Index;
                    cell.CellStyle.FillPattern = FillPattern.SolidForeground;
                    if (j == 0)
                        cell.SetCellValue(string.IsNullOrWhiteSpace(areaInfo.CityName) == true ? areaInfo.ProvinceName : areaInfo.ProvinceName + "-" + areaInfo.CityName);

                    if (j == 8)
                        sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, 8));
                }

                rowIndex++;
                row = sheet.CreateRow(rowIndex);
                row.Height = 30 * 20;
                //row.RowStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Brown.Index;
                for (int j = 0; j < 9; j++)
                {
                    ICell cell = row.CreateCell(j);
                    setCellStyle1(workbook, cell);
                    cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平对齐
                    if (j == 0)
                        cell.SetCellValue("媒体信息");
                    if (j == 2)
                        cell.SetCellValue("广告信息");
                    if (j == 4)
                        cell.SetCellValue("粉丝数");
                    if (j == 5)
                        cell.SetCellValue("投放日期");
                    if (j == 6)
                        cell.SetCellValue("投放次数");
                    if (j == 7)
                        cell.SetCellValue("成本参考价");
                    if (j == 8)
                    {
                        cell.SetCellValue("原创参考价");
                        sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, 1));
                        sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 2, 3));
                    }
                }

                int cityCostPrice = (int)areaInfo.PublishDetails.Sum(x => x.CostReferencePrice);
                int cityOriginalPrice = (int)areaInfo.PublishDetails.Sum(x => x.OriginalReferencePrice);
                foreach (var pubDetail in areaInfo.PublishDetails)
                {
                    rowIndex++;
                    row = sheet.CreateRow(rowIndex);
                    row.Height = 30 * 20;
                    //row.RowStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Brown.Index;
                    for (int j = 0; j < 9; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        setCellStyle1(workbook, cell);
                        cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平对齐
                        cell.CellStyle.WrapText = true;
                        if (j == 0)
                            cell.SetCellValue($"{pubDetail.MediaName}\n{pubDetail.MediaNumber}");
                        if (j == 2)
                        {
                            cell.SetCellValue($"广告位置:{pubDetail.ADPosition}\n广告样式:{pubDetail.CreateType}");
                            cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//水平对齐
                            cell.CellStyle.WrapText = true;
                        }
                        if (j == 4)
                            cell.SetCellValue(pubDetail.FansCount >= 10000 ? $"{pubDetail.FansCount / 10000}万" : pubDetail.FansCount.ToString());
                        if (j == 5)
                        {
                            IDataFormat format = workbook.CreateDataFormat();
                            cell.CellStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
                            cell.SetCellValue(exportDto.LaunchTime);
                        }

                        if (j == 6)
                            cell.SetCellValue(pubDetail.ADLaunchDays);
                        if (j == 7)
                        {
                            IDataFormat format = workbook.CreateDataFormat();
                            cell.CellStyle.DataFormat = format.GetFormat("¥#,##0");
                            cell.SetCellValue((double)pubDetail.CostReferencePrice);
                        }
                        if (j == 8)
                        {
                            IDataFormat format = workbook.CreateDataFormat();
                            cell.CellStyle.DataFormat = format.GetFormat("¥#,##0");
                            cell.SetCellValue((double)pubDetail.OriginalReferencePrice);
                            sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, 1));
                            sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 2, 3));
                        }
                    }
                }
                rowIndex++;
                row = sheet.CreateRow(rowIndex);
                row.Height = 30 * 20;
                for (int j = 0; j < 9; j++)
                {
                    ICell cell = row.CreateCell(j);
                    setCellStyle1(workbook, cell);
                    cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平对齐

                    if (j == 7)
                    {
                        IDataFormat format = workbook.CreateDataFormat();
                        cell.CellStyle.DataFormat = format.GetFormat("¥#,##0");
                        cell.SetCellValue(cityCostPrice);
                    }
                    if (j == 8)
                    {
                        IDataFormat format = workbook.CreateDataFormat();
                        cell.CellStyle.DataFormat = format.GetFormat("¥#,##0");
                        cell.SetCellValue(cityOriginalPrice);
                        sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, 6));
                    }
                }
                rowIndex++;
                totalCostPrice += cityCostPrice + cityOriginalPrice;
            }
            row = sheet.CreateRow(rowIndex);
            row.Height = 30 * 20;
            for (int j = 0; j < 9; j++)
            {
                ICell cell = row.CreateCell(j);
                setCellStyle1(workbook, cell);
                cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                if (j == 0)
                {
                    IFont ffont = workbook.CreateFont();
                    ffont.FontName = "DengXian";
                    ffont.FontHeight = 16;
                    cell.CellStyle.SetFont(ffont);
                    cell.SetCellValue($"共{exportDto.AreaInfo.Sum(x => x.PublishDetails.Count)}个媒体 成本参考价总计：");
                }

                if (j == 8)
                {
                    cell.CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    IFont ffont = workbook.CreateFont();
                    ffont.FontName = "DengXian";
                    ffont.FontHeight = 16;
                    cell.CellStyle.SetFont(ffont);
                    IDataFormat format = workbook.CreateDataFormat();
                    cell.CellStyle.DataFormat = format.GetFormat("¥#,##0");
                    cell.SetCellValue(totalCostPrice);
                    //cell.SetCellValue((double)(exportDto.AreaInfo.Sum(x=>x.PublishDetails.Sum(p=>p.CostReferencePrice))+ exportDto.AreaInfo.Sum(x => x.PublishDetails.Sum(p => p.OriginalReferencePrice))));
                    sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex, 0, 7));
                }
            }

            workbook.Write(fs);
            return new Tuple<string, string>($"http://www.chitunion.com/" + $"{relativeDir}/{fileName}", fileName);
        }
        private void setCellStyle1(IWorkbook workbook, ICell cell)
        {
            ICellStyle fCellStyle = workbook.CreateCellStyle();
            fCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            fCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            fCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            fCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;

            IFont ffont = workbook.CreateFont();
            ffont.FontName = "DengXian";
            ffont.FontHeight = 12;
            fCellStyle.SetFont(ffont);
            fCellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//垂直对齐
            fCellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//水平对齐
            cell.CellStyle = fCellStyle;
        }
        public string IntelligenceRecommendADOrderExport(string orderID, string orderName, out string msg)
        {
            msg = string.Empty;
            if (string.IsNullOrWhiteSpace(orderID))
                msg = "项目号为必填项!";
            if (string.IsNullOrWhiteSpace(orderName))
                msg = "项目名称为必填项!";
            var resDto = BLL.ADOrderInfo.Instance.IntelligenceADOrderInfoQuery(orderID);
            if (resDto == null)
                msg = "没有数据";
            if (!string.IsNullOrEmpty(msg))
                return msg;
            //Mapper.CreateMap<Entities.DTO.V1_1_8.ResponseIntelligenceADOrderDto, IRecommendExportDto>()
            //    .ForMember(
            //                dest => dest.ExcelName,
            //                opt => { opt.MapFrom(src => src.OrderName); }
            //               );

            //var exp = Mapper.CreateMap<Entities.DTO.V1_1_8.ResponseIntelligenceADOrderDto, IRecommendExportDto>();
            //exp.ForMember(dest => dest.ExcelName, opt => { opt.MapFrom(src => src.OrderName); });
            //exp.ForMember(dest => dest.MasterBrand, opt => { opt.MapFrom(src => src.MasterName); });
            //exp.ForMember(dest => dest.CarBrand, opt => { opt.MapFrom(src => src.BrandName); });
            //exp.ForMember(dest => dest.CarSerial, opt => { opt.MapFrom(src => src.SerialName); });

            BLL.V1_8_3.Dto.IRecommendExportDto exportDto = new IRecommendExportDto()
            {
                ExcelName = orderName,
                MasterBrand = resDto.ADOrderInfo.MasterName,
                CarBrand = resDto.ADOrderInfo.BrandName,
                CarSerial = resDto.ADOrderInfo.SerialName,
                BudgetTotal = resDto.ADOrderInfo.BudgetTotal,
                LaunchTime = resDto.ADOrderInfo.LaunchTime,
                JKEntrance = resDto.ADOrderInfo.JKEntrance
            };
            exportDto.AreaInfo = new List<IRecommendExportAreaInfoDto>();
            foreach (var itemAreaInfo in resDto.AreaInfos)
            {
                IRecommendExportAreaInfoDto area = new IRecommendExportAreaInfoDto()
                {
                    ProvinceName = itemAreaInfo.ProvinceName,
                    CityName = itemAreaInfo.CityName
                };
                area.PublishDetails = new List<IRecommendExportPublishDetailDto>();
                foreach (var itemPublishDetail in itemAreaInfo.PublishDetails)
                {
                    area.PublishDetails.Add(new IRecommendExportPublishDetailDto()
                    {
                        MediaName = itemPublishDetail.MediaName,
                        MediaNumber = itemPublishDetail.MediaNumber,
                        ADPosition = itemPublishDetail.ADPosition,
                        CreateType = itemPublishDetail.CreateType,
                        FansCount = itemPublishDetail.FansCount,
                        ADLaunchDays = itemPublishDetail.ADLaunchDays,
                        CostReferencePrice = itemPublishDetail.CostReferencePrice,
                        OriginalReferencePrice = itemPublishDetail.EnableOriginPrice == true ? itemPublishDetail.OriginalReferencePrice : 0
                    });
                }
                exportDto.AreaInfo.Add(area);
            }
            Tuple<string, string> tp = IntelligenceRecommendExport(exportDto, out msg, orderID);
            return tp.Item1;
        }
    }
}
