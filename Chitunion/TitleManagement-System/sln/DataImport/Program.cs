using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImport
{
    class Program
    {
        //static string excelPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["CarMapIP_ExcelPath"]);
        //static string excelPath0921 = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["CarMapIP_ExcelPath0921"]);
        //static string excelPath1017 = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["CarMapIP_ExcelPath1017"]);
        //static string APP_ExcelPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["APP_ExcelPath"]);
        //static string WECHAT_ExcelPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["WECHAT_ExcelPath"]);
        //static string WECHATTWO_ExcelPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["WECHATTWO_ExcelPath"]);
        //static string TOUTIAO_ExcelPath = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["TOUTIAO_ExcelPath"]);
        static string excelPath1110 = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["CarMapIP_ExcelPath1110"]);
        static string APP_ExcelPath1206 = Path.Combine(Directory.GetCurrentDirectory(), ConfigurationManager.AppSettings["APP_ExcelPath1206"]);
        static void Main(string[] args)
        {

            Console.WriteLine("***************开始****************");
            //ImportData.ImportTitleCarMapping.Instance.ImportCarLabel(3, excelPath);
            //ImportData.ImportTitleCarMapping.Instance.ImportCarLabel(4, excelPath0921);
            //ImportData.ImportTitleCarMapping.Instance.ImportCarLabel(3, excelPath1017);
            //ImportData.ImportAppLabel.Instance.ImportAppLabell(5, 2, 5, 5, 5, APP_ExcelPath);
            //ImportData.ImportWechatLabel.Instance.ImportWechatLabell(5, 2, 5, 5, WECHAT_ExcelPath);
            //ImportData.ImportWechatTwo.Instance.ImportWechatTwoLabel(5, 2, 5, 5, 5, WECHATTWO_ExcelPath);
            //ImportData.ImportTouTiao.Instance.ImportTouTiaoLabel(5, 2, 5, 3, TOUTIAO_ExcelPath);

            ImportData.ImportTitleCarMapping.Instance.ImportCarLabel(4, excelPath1110);
            ImportData.ImportAppLabel.Instance.ImportAppLabell(5, 2, 5, 5, 5, APP_ExcelPath1206);
            Console.WriteLine("***************结束****************");
            Console.ReadKey();
        }
    }
}
