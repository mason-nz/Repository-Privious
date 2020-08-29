using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.CTUtils.Config;
using XYAuto.Utils;
using XYAuto.Utils.Data;

namespace XYAuto.ChiTu2018.WeChat.CleanArticleImgConsole.DAL
{
    /// <summary>
    /// 注释：ArticleInfo
    /// 作者：masj
    /// 日期：2018/5/28 10:38:06
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class ArticleInfo : XYAuto.ITSC.Chitunion2017.Dal.DataBase
    {
        public static readonly ArticleInfo Instance = new ArticleInfo();
        private string Conn_BaseData = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_BaseData", true);//ConfigurationManager.AppSettings["ConnectionStrings_BaseData"];
        private int QueryArticleInfo_RecID = XYAuto.CTUtils.Sys.ConverHelper.ObjectToInteger(ConfigurationUtil.GetAppSettingValue("QueryArticleInfo_RecID", true));
        private int QueryArticleInfo_TopNum = XYAuto.CTUtils.Sys.ConverHelper.ObjectToInteger(ConfigurationUtil.GetAppSettingValue("QueryArticleInfo_TopNum", true));
        //private DateTime QueryArticleInfo_CreateTime = XYAuto.CTUtils.Sys.ConverHelper.ObjectToDateTime(ConfigurationUtil.GetAppSettingValue("QueryArticleInfo_CreateTime", true));
        private int QueryArticleInfo_Days = XYAuto.CTUtils.Sys.ConverHelper.ObjectToInteger(ConfigurationUtil.GetAppSettingValue("QueryArticleInfo_Days", true));

        internal DataTable GetArticleDataByRecID(int? recId = null)
        {
            string sql = string.Format(@"SELECT TOP {0} 
RecID,
HeadImg,
HeadImgNew,
HeadImgNew2,
HeadImgNew3,
Content,
CreateTime
FROM BaseData2017.dbo.ArticleInfo WITH(NOLOCK)
WHERE RecID>{1} and CreateTime<'{2}'
ORDER BY RecID", QueryArticleInfo_TopNum, recId ?? QueryArticleInfo_RecID, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 0:0:0")).AddDays(-QueryArticleInfo_Days));
            DataSet ds = SqlHelper.ExecuteDataset(Conn_BaseData, CommandType.Text, sql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }



    }
}
