using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.Export
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("开始执行导入APP操作...");

            string Path = ConfigurationManager.AppSettings["MediaAppPath"];

            string FilePath = Path + "\\上传模板_APP.xlsx";

            bool resultBool = ExcelHelper.InsertMediaApp(FilePath);
            Console.WriteLine();
            Console.WriteLine(resultBool ? "导入成功" : "导入失败");
        }
    }
}
