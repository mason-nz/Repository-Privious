using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using BitAuto.EP.Common.Lib.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using XYAuto.ITSC.Chitunion2017.BLL.ImportData;
using XYAuto.ITSC.Chitunion2017.BLL.ImportData.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Test.Helper;

namespace XYAuto.ITSC.Chitunion2017.Test
{
    [TestClass]
    public class ImportDataTest
    {
        [TestMethod]
        public void ReadFromExcelFileTest()
        {
            var filePath = @"E:\导入数据测试\test.xlsx";

            var listSheetName = new List<string>() { "微信", "微博" };


            var excelHelper = new ExcelHelper(filePath);

            var dataTable = excelHelper.ExcelToDataTable(listSheetName[0], true);

            excelHelper.DataTableToExcel(dataTable, "", true);

            Console.WriteLine(JsonConvert.SerializeObject(dataTable));

        }

        [TestMethod]
        public void ReadDataListClientTest()
        {
            AutoMapperConfiguration.Configure();
            var client = new ReadDataListClient();
            client.LoadFileDir();
        }


        [TestMethod]
        public void MoveTest()
        {
            var pathD = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReadFile");
            var pathMove = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StoreFile");
            pathD = Path.Combine(pathD, "微信.xlsx");

            //new ReadDataListClient().MoveFile(pathD,);
        }

        [TestMethod]
        public void Test1()
        {
            AutoMapperConfiguration.Configure();
            var filePath = @"E:\导入数据测试\test.xlsx";
            var exc = new ExcelHelper<ImportMediaWeiXinDto>();
            var list = exc.GetListFromFile(filePath).ToList();

            list[0].AdPosition1单图文硬广原创发布 = 888;
            list[0].AdPosition1单图文硬广发布 = 900;
            list[0].MediaType = 14001;
            var dto = new Import_PCAPPDTO();
            //媒体
            dto.MediaWeixin = Mapper.Map<ImportMediaWeiXinDto, Entities.Media.MediaWeixin>(list[0]);

            Console.WriteLine(JsonConvert.SerializeObject(dto.MediaWeixin));
            //刊例base
            dto.PubBasicInfo = Mapper.Map<ImportMediaWeiXinDto, Entities.Publish.PublishBasicInfo>(list[0]);
            //互动参数
            dto.InteractionWeixin =
                  Mapper.Map<ImportMediaWeiXinDto, Entities.Interaction.InteractionWeixin>(list[0]);
            dto.PubDetailList = Mapper.Map<List<Entities.Publish.PublishDetailInfo>, List<Entities.Publish.PublishDetailInfo>>(list[0].PublishDetailList);
            Console.WriteLine("\r\n");
            Console.WriteLine(JsonConvert.SerializeObject(dto.PubDetailList));
        }

        public void ReadFromExcelFile(string filePath)
        {
            IWorkbook wk = null;
            string extension = System.IO.Path.GetExtension(filePath);
            try
            {
                FileStream fs = File.OpenRead(filePath);
                if (extension.Equals(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    wk = new XSSFWorkbook(fs);
                }

                fs.Close();
                //读取当前表数据
                //ISheet sheet = wk.GetSheetAt(0);
                ISheet sheet = wk.GetSheet("");

                IRow row = sheet.GetRow(0);  //读取当前行数据
                //LastRowNum 是当前表的总行数-1（注意）
                int offset = 0;
                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);  //读取当前行数据
                    if (row != null)
                    {
                        //LastCellNum 是当前行的总列数
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            //读取该行的第j列数据
                            string value = row.GetCell(j).ToString();
                            Console.Write(value.ToString() + " ");
                        }
                        Console.WriteLine("\n");
                    }
                }
            }

            catch (Exception e)
            {
                //只在Debug模式下才输出
                Console.WriteLine(e.Message);
            }
        }

    }
}
