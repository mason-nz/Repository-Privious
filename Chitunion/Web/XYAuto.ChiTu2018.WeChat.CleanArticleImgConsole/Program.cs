using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.WeChat.CleanArticleImgConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BusinessLL.ArticleInfo.Instance.GetImageList();
            XYAuto.CTUtils.Log.Log4NetHelper.Default().Info("获取图片完成");
        }
    }
}
