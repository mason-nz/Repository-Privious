using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.Examine;
using XYAuto.Utils.Data;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace XYAuto.BUOC.IP2017.Dal.ExamineLabel
{
    public class ExamineLableOperate : DataBase
    {
        public static readonly ExamineLableOperate Instance = new ExamineLableOperate();

        /// <summary>
        /// zlb 2017-10-25
        ///更新审核状态
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <param name="Status">审核状态</param>
        /// <param name="UserId">操作人</param>
        /// <returns></returns>
        public int UpdateAuditStatus(int BatchAuditID, int Status, int UserId)
        {
            string strSql = $"UPDATE BatchMediaAudit SET AuditTime=GETDATE(),AuditUserID={UserId},status={Status} WHERE BatchAuditID={BatchAuditID}";
            strSql += $" UPDATE BatchMedia SET AuditTime=GETDATE(),AuditUserID={UserId},status={Status} WHERE BatchAuditID={BatchAuditID}";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-10-25
        /// 增加审核普通标签
        /// </summary>
        /// <param name="labelOperate">标签</param>
        /// <param name="UserId">用户</param>
        /// <returns></returns>
        public void InsertAuditLabel(AuditLabel labelOperate, int UserId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" DELETE FROM  BatchAuditPassed WHERE BatchAuditID={0}", labelOperate.BatchID);
            sb.AppendFormat(" DELETE FROM  IPSubLabelAuditPassed WHERE BatchAuditID={0}", labelOperate.BatchID);
            sb.AppendFormat(" DELETE FROM  SonIPLabelAuditPassed WHERE BatchAuditID={0}", labelOperate.BatchID);
            string strSql1 = "";
            string strSql2 = "";
            if (labelOperate.TaskType == (int)EnumTaskType.媒体)
            {
                DateTime dtNow = DateTime.Now;
                strSql2 = string.Format(" INSERT INTO dbo.BatchAuditPassed (BatchAuditID,TitleID,Type,IsCustom,Name,CreateTime,CreateUserID) VALUES", labelOperate.BatchID);
                if (labelOperate.Category != null)
                {
                    foreach (var item in labelOperate.Category)
                    {
                        strSql1 += string.Format("({0},{1},{2},{3},'{4}','{5}',{6}),", labelOperate.BatchID, item.DictId, (int)EnumLabelType.分类, item.DictId <= 0 ? 1 : 0, SqlFilter(item.DictName), dtNow, UserId);
                    }
                }
                if (labelOperate.MarketScene != null)
                {
                    foreach (var item in labelOperate.MarketScene)
                    {
                        strSql1 += string.Format("({0},{1},{2},{3},'{4}','{5}',{6}),", labelOperate.BatchID, item.DictId, (int)EnumLabelType.市场场景, item.DictId <= 0 ? 1 : 0, SqlFilter(item.DictName), dtNow, UserId);
                    }
                }
                if (labelOperate.DistributeScene != null)
                {
                    foreach (var item in labelOperate.DistributeScene)
                    {
                        strSql1 += string.Format("({0},{1},{2},{3},'{4}','{5}',{6}),", labelOperate.BatchID, item.DictId, (int)EnumLabelType.分发场景, item.DictId <= 0 ? 1 : 0, SqlFilter(item.DictName), dtNow, UserId);
                    }
                }
            }

            string strSql = sb.ToString();
            if (strSql1 != "")
            {
                strSql = strSql + strSql2 + strSql1;
                if (strSql.Contains(","))
                {
                    strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
                }
            }
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
            InserAuditLableIpInfo(labelOperate.IPLabel, labelOperate.BatchID, UserId);
        }
        /// <summary>
        /// zlb 2017-10-25
        /// 增加审核IP标签
        /// </summary>
        /// <param name="IpLabel"></param>
        /// <param name="BatchAuditID"></param>
        /// <param name="UserId"></param>
        public void InserAuditLableIpInfo(List<IpLabel> IpLabel, int BatchAuditID, int UserId)
        {
            if (IpLabel != null)
            {
                DateTime dtNow = DateTime.Now;
                foreach (var item in IpLabel)
                {
                    string strSqlIP = "INSERT INTO dbo.BatchAuditPassed (BatchAuditID,TitleID,Type,IsCustom,Name,CreateTime,CreateUserID) VALUES ";
                    strSqlIP += string.Format("({0},{1},{2},{3},'{4}','{5}',{6}); SELECT @@Identity", BatchAuditID, item.DictId, (int)EnumLabelType.IP, item.DictId <= 0 ? 1 : 0, SqlFilter(item.DictName), dtNow, UserId);
                    object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSqlIP);
                    int lableID = obj == null ? 0 : Convert.ToInt32(obj);
                    if (lableID > 0)
                    {
                        if (item.SubIP != null)
                        {
                            foreach (var itemSonIP in item.SubIP)
                            {
                                string strSqlSonIp = "INSERT INTO dbo.IPSubLabelAuditPassed (BatchAuditID,AuditLabelID ,TitleID , CreateTime ,CreateUserID) VALUES ";
                                strSqlSonIp += string.Format("({0},{1},{2},'{3}',{4});SELECT @@Identity", BatchAuditID, lableID, itemSonIP.DictId, dtNow, UserId);
                                object objSonIp = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSqlSonIp);
                                int sonIpID = objSonIp == null ? 0 : Convert.ToInt32(objSonIp);
                                if (itemSonIP.Label != null)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    foreach (var itemLabel in itemSonIP.Label)
                                    {
                                        sb.Append("INSERT INTO dbo.SonIPLabelAuditPassed (BatchAuditID,AuditSubIPID, Name,TitleID, CreateTime,CreateUserID) VALUES ");
                                        sb.AppendFormat("({0},{1},'{2}',{3},'{4}',{5});", BatchAuditID, sonIpID, SqlFilter(itemLabel.DictName), 0, dtNow, UserId);
                                    }
                                    SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// zlb 2017-10-25
        /// 根据审核批次ID查询审核状态
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <returns></returns>
        public DataTable GetAuditStatus(int BatchAuditID)
        {
            string strSql = $"SELECT AuditUserID,Status FROM BatchMediaAudit WHERE BatchAuditID={BatchAuditID}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        /// <summary>
        /// zlb 2017-11-27
        /// 修改批次表状态
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public int UpdateBatchMediaStatus(string StrWhere, int Status)
        {
            string strSql = $"UPDATE BatchMediaAudit SET status={Status} " + StrWhere;
            strSql += $" UPDATE BatchMedia SET status={Status} " + StrWhere;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        public int SelectArticleCount(int BatchAuditID)
        {
            string strSql = $" SELECT COUNT (1) FROM BatchMedia B INNER JOIN  BatchMediaArticle M ON B.BatchMediaID=M.BatchMediaID WHERE B.BatchAuditID={BatchAuditID}";
            Object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
    }
}
