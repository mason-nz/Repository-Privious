using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.HBaseDataSync.Thrift;

namespace XYAuto.ITSC.Chitunion2017.HBaseDataSync
{
    public class QueryHbaseHelper
    {
        public readonly static QueryHbaseHelper Instance = new QueryHbaseHelper();

        /// <summary>
        /// 根据日期，查询Hbase数据，返回统计信息
        /// </summary>
        /// <param name="dt">日期，默认为当前系统时间</param>
        /// <returns>返回以天维度的小时统计数据</returns>
        public DataTable GetStatWeixin_ArticleByDate(DateTime dtime, out int wXArticleCount, out ArrayList al_WXNum)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Hour", typeof(string));
            dt.Columns.Add("WXNumCount", typeof(int));
            dt.Columns.Add("WXArticleCount", typeof(int));
            string date = dtime.ToString("yyyyMMdd");
            //int XNum = 0;//
            //ArrayList al_WXArticle = new ArrayList();
            wXArticleCount = 0;
            al_WXNum = new ArrayList();
            for (int i = 0; i < 24; i++)
            {
                string hour = i.ToString("00");
                var data = GetStatWeixin_ArticleByHour(date + hour,ref al_WXNum);

                DataRow dr = dt.NewRow();
                dr["Date"] = date;
                dr["Hour"] = hour;
                dr["WXNumCount"] = data["WXNumCount"];
                dr["WXArticleCount"] = data["WXArticleCount"];
                wXArticleCount += int.Parse(dr["WXArticleCount"].ToString());
                dt.Rows.Add(dr);
            }
            return dt;
        }


        /// <summary>
        /// 根据小时内容，查询Hbase中的微信公众号文章统计数据
        /// </summary>
        /// <param name="hour">小时内容，格式为：yyyyMMddHH</param>
        /// <returns>返回字典列表，只有2个内容（WXNumCount、WXArticleCount）</returns>
        private Dictionary<string, int> GetStatWeixin_ArticleByHour(string hour, ref ArrayList al_WXNum)
        {
            BLL.Loger.Log4Net.Info("从Hbasee中根据RowFilter中小时：" + hour + "，获取微信公众号文章明细数据。=============开始============");
            Dictionary<string, int> dict = new Dictionary<string, int>();
            ArrayList dict_WXNum = new ArrayList();
            ArrayList dict_WXArticle = new ArrayList();
            try
            {
                HBaseThriftHelper test = new HBaseThriftHelper();
                List<string> dd = test.GetTables();
                string tableName = "llbx:spider_data";
                List<string> cols = new List<string> { "ct:wxid", "ct:sn" };//, "ct:farther_sn", "ct:pub_time", "ct:is_multi", "ct:location", "ct:push_time" 
                string filterString = "RowFilter(=,'regexstring:.*" + hour + ".*_wx_ct_.*')";
                var list = test.ScannerOpenWithScan(tableName, filterString, cols);

                foreach (var item in list)
                {
                    string wxid = item.Value["ct:wxid"].ToString();
                    if (!dict_WXNum.Contains(wxid))
                    {
                        dict_WXNum.Add(wxid);
                    }
                    if (!al_WXNum.Contains(wxid))
                    {
                        al_WXNum.Add(wxid);
                    }

                    string sn = item.Value["ct:sn"].ToString();
                    if (!dict_WXArticle.Contains(sn))
                    {
                        dict_WXArticle.Add(sn);
                    }
                }
                dict.Add("WXNumCount", dict_WXNum.Count);
                dict.Add("WXArticleCount", dict_WXArticle.Count);
            }
            catch (Exception e)
            {
                BLL.Loger.Log4Net.Error("根据" + hour + "，从Hbase中获取数据出错", e);
                dict = null;
            }
            BLL.Loger.Log4Net.Info(string.Format("从Hbasee中根据RowFilter中小时：{0}，获取微信公众号文章明细数据。=============结束============,WXNumCount={1},WXArticleCount={2}", hour, dict_WXNum.Count, dict_WXArticle.Count));
            return dict;
        }
    }
}
