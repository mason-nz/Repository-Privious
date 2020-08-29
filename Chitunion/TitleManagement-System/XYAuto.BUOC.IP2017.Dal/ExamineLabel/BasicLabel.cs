using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace XYAuto.BUOC.IP2017.Dal.ExamineLabel
{
    public class BasicLabel : DataBase
    {
        public static readonly BasicLabel Instance = new BasicLabel();
        /// <summary>
        /// zlb 2017-10-23
        /// 查询待审核标签
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <returns></returns>
        public DataSet QueryPendingAuditLabelList(int BatchAuditID)
        {
            string strSql = $@"
        SELECT  BH.Type ,
                BH.TitleID AS DictId ,
                BH.Name AS DictName ,
                U.UserName AS Creater
        FROM    BatchLabelHistory BH
                LEFT JOIN Chitunion2017.dbo.UserInfo U ON BH.CreateUserID = U.UserID
        WHERE   BH.BatchMediaID IN (SELECT BatchMediaID FROM dbo.BatchMedia WHERE BatchAuditID={BatchAuditID} )
                AND BH.Type IN ( {(int)EnumLabelType.分类},
                                 {(int)EnumLabelType.分发场景},
                                 {(int)EnumLabelType.市场场景} )
        SELECT MRL.BatchMediaID AS BatchID, MRL.TitleID AS IpId ,
                TB1.Name AS IpName ,
                IPL.TitleID AS SonIpId ,
                TB2.Name AS SonIpName ,
                SIP.Name AS LabelName ,
                U1.UserName AS IpCreater ,
                U2.UserName AS SopIpCreater
        FROM    BatchLabelHistory MRL
                INNER JOIN IPSubLabel IPL ON MRL.LabelID = IPL.LabelID
                LEFT JOIN SonIPLabel SIP ON IPL.SubIPID = SIP.SubIPID
                LEFT JOIN TitleBasicInfo TB1 ON MRL.TitleID = TB1.TitleID
                                                AND TB1.Status = 0
                LEFT JOIN TitleBasicInfo TB2 ON IPL.TitleID = TB2.TitleID
                                                AND TB2.Status = 0
                LEFT JOIN Chitunion2017.dbo.UserInfo U1 ON MRL.CreateUserID = U1.UserID
                LEFT JOIN Chitunion2017.dbo.UserInfo U2 ON IPL.CreateUserID = U2.UserID
        WHERE   MRL.BatchMediaID IN (SELECT BatchMediaID FROM dbo.BatchMedia WHERE BatchAuditID={BatchAuditID})
                AND MRL.Type = {(int)EnumLabelType.IP}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-10-23
        /// 查询已审核标签
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <returns></returns>
        public DataSet QueryAuditedLabelList(int BatchAuditID)
        {
            string strSql = $@"  SELECT  BH.Type ,
                BH.TitleID AS DictId ,
                BH.Name AS DictName ,
                U.UserName AS Creater
        FROM    BatchAuditPassed BH
                LEFT JOIN Chitunion2017.dbo.UserInfo U ON BH.CreateUserID = U.UserID
        WHERE   BH.BatchAuditID = {BatchAuditID}
                AND BH.Type IN ( {(int)EnumLabelType.分类},
                                 {(int)EnumLabelType.分发场景},
                                 {(int)EnumLabelType.市场场景} )
        SELECT  MRL.TitleID AS IpId ,
                TB1.Name AS IpName ,
                IPL.TitleID AS SonIpId ,
                TB2.Name AS SonIpName ,
                SIP.Name AS LabelName ,
                U1.UserName AS IpCreater ,
                U2.UserName AS SopIpCreater
        FROM    BatchAuditPassed MRL
                INNER JOIN IPSubLabelAuditPassed IPL ON MRL.AuditLabelID = IPL.AuditLabelID
                LEFT JOIN SonIPLabelAuditPassed SIP ON IPL.AuditSubIPID = SIP.AuditSubIPID
                LEFT JOIN TitleBasicInfo TB1 ON MRL.TitleID = TB1.TitleID
                                                AND TB1.Status = 0
                LEFT JOIN TitleBasicInfo TB2 ON IPL.TitleID = TB2.TitleID
                                                AND TB2.Status = 0
                LEFT JOIN Chitunion2017.dbo.UserInfo U1 ON MRL.CreateUserID = U1.UserID
                LEFT JOIN Chitunion2017.dbo.UserInfo U2 ON IPL.CreateUserID = U2.UserID
        WHERE   MRL.BatchAuditID =  {BatchAuditID}
                AND MRL.Type = {(int)EnumLabelType.IP}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-10-25
        /// 查询最终结果标签
        /// </summary>
        /// <param name="MediaResultID"></param>
        /// <returns></returns>
        public DataSet QueryResultLabelList(int MediaResultID)
        {
            string strSql = $@"SELECT  BH.Type ,
                BH.TitleID AS DictId ,
                BH.Name AS DictName ,
                U.UserName AS Creater
        FROM    ResultLabel BH
                LEFT JOIN Chitunion2017.dbo.UserInfo U ON BH.CreateUserID = U.UserID
        WHERE   BH.MediaResultID = {MediaResultID}
                AND BH.Type IN ( {(int)EnumLabelType.分类},
                                 {(int)EnumLabelType.分发场景},
                                 {(int)EnumLabelType.市场场景} )
          SELECT    MRL.ResultLabelID AS PIPID ,
            IPL.ResultSubIPID AS SubIPID ,
            MRL.MediaResultID AS BatchID ,
            MRL.TitleID AS IpId ,
            TB1.Name AS IpName ,
            IPL.TitleID AS SonIpId ,
            TB2.Name AS SonIpName ,
            SIP.Name AS LabelName ,
            U1.UserName AS IpCreater ,
            U2.UserName AS SopIpCreater
  FROM      ResultLabel MRL
            INNER JOIN MediaResultIPSubLabe IPL ON MRL.ResultLabelID = IPL.ResultLabelID
            LEFT JOIN MediaResultSonIPLabel SIP ON IPL.ResultSubIPID = SIP.ResultSubIPID
            LEFT JOIN TitleBasicInfo TB1 ON MRL.TitleID = TB1.TitleID
                                            AND TB1.Status = 0
            LEFT JOIN TitleBasicInfo TB2 ON IPL.TitleID = TB2.TitleID
                                            AND TB2.Status = 0
            LEFT JOIN Chitunion2017.dbo.UserInfo U1 ON MRL.CreateUserID = U1.UserID
            LEFT JOIN Chitunion2017.dbo.UserInfo U2 ON IPL.CreateUserID = U2.UserID
        WHERE   MRL.MediaResultID = {MediaResultID}
                AND MRL.Type = {(int)EnumLabelType.IP}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// ZLB 2017-10-27
        /// 根据标签类型查询基础标签
        /// </summary>
        /// <param name="LabelType">标签类型</param>
        /// <returns></returns>
        public DataTable SelectBasicLabel(int LabelType)
        {
            string strSql = $@"SELECT TBI.TitleID DictId,TBI.Name DictName,TBI.Type FROM dbo.TitleBasicInfo TBI WHERE TBI.Type = { LabelType}; ";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }

        /// <summary>
        /// ZLB 2017-10-27
        /// 查询所有子ip信息
        /// </summary>
        /// <returns></returns>
        public DataTable SelectIpLabel()
        {
            string strSql = $@"SELECT IP.PIP IpId,TBI.TitleID SonIpId,TBI.Name SonIpName FROM IPTitleInfo IP INNER JOIN dbo.TitleBasicInfo TBI ON IP.SubIP=TBI.TitleID WHERE TBI.Type={(int)EnumLabelType.子IP} GROUP BY IP.PIP,TBI.TitleID,TBI.Name";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        public DataTable SelectTitleBasicInfo(string ListLableName)
        {
            string strSql = $"SELECT T.TitleID,T.Name  FROM TitleBasicInfo T WHERE T.Name in ({ListLableName}) and Type={65004}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        public int InsertTitleBasicInfo(List<string> listLableName, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT  INTO dbo.TitleBasicInfo (Name,Type,Status,CreateTime,CreateUserID) VALUES ");
            DateTime dtNow = DateTime.Now;
            foreach (var item in listLableName)
            {
                sb.AppendFormat(" ('{0}',{1},{2},'{3}',{4}), ", SqlFilter(item), 65004, 0, dtNow, UserID);
            }
            string strSql = sb.ToString();
            strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        public int DeleteIPTitleInfo(int PIP, int SubPIP, string ListLableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"DELETE FROM  IPTitleInfo WHERE PIP={PIP} AND SubIP={SubPIP} AND TitleID IN (SELECT TitleID FROM dbo.TitleBasicInfo WHERE type=65004  AND  Name IN ({ListLableName})) ");
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
        }
        public int InsertIPTitleInfo(int PIP, int SubPIP, int UserID, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT  INTO dbo.IPTitleInfo (PIP,SubIP,TitleID,Status,CreateTime,CreateUserID) VALUES ");
            DateTime dtNow = DateTime.Now;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.AppendFormat(" ({0},{1},{2},{3},'{4}',{5}), ", PIP, SubPIP, Convert.ToInt32(dt.Rows[i]["TitleID"]), 0, dtNow, UserID);
            }
            string strSql = sb.ToString();
            strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        public DataTable QueryPendingLabelList(int BatchMediaID)
        {
            string strSql = $@"        
                    SELECT  MRL.BatchMediaID AS BatchID ,
                            MRL.TitleID AS IpId ,
                            TB1.Name AS IpName ,
                            IPL.TitleID AS SonIpId ,
                            TB2.Name AS SonIpName ,
                            SIP.Name AS LabelName ,
                            U1.UserName AS IpCreater ,
                            U2.UserName AS SopIpCreater
                    FROM    BatchLabelHistory MRL
                            INNER JOIN IPSubLabel IPL ON MRL.LabelID = IPL.LabelID
                            LEFT JOIN SonIPLabel SIP ON IPL.SubIPID = SIP.SubIPID
                            LEFT JOIN TitleBasicInfo TB1 ON MRL.TitleID = TB1.TitleID
                                                            AND TB1.Status = 0
                            LEFT JOIN TitleBasicInfo TB2 ON IPL.TitleID = TB2.TitleID
                                                            AND TB2.Status = 0
                            LEFT JOIN Chitunion2017.dbo.UserInfo U1 ON MRL.CreateUserID = U1.UserID
                            LEFT JOIN Chitunion2017.dbo.UserInfo U2 ON IPL.CreateUserID = U2.UserID
                    WHERE   MRL.BatchMediaID = {BatchMediaID}
                            AND MRL.Type = {(int)EnumLabelType.IP}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
        public DataTable QueryLabelList(int BatchMediaID)
        {
            string strSql = $@" 
                            SELECT  MRL.TitleID AS IpId ,
                                    TB1.Name AS IpName ,
                                    IPL.TitleID AS SonIpId ,
                                    TB2.Name AS SonIpName ,
                                    SIP.Name AS LabelName ,
                                    U1.UserName AS IpCreater ,
                                    U2.UserName AS SopIpCreater
                            FROM    BatchAuditPassed MRL
                                    INNER JOIN IPSubLabelAuditPassed IPL ON MRL.AuditLabelID = IPL.AuditLabelID
                                    LEFT JOIN SonIPLabelAuditPassed SIP ON IPL.AuditSubIPID = SIP.AuditSubIPID
                                    LEFT JOIN TitleBasicInfo TB1 ON MRL.TitleID = TB1.TitleID
                                                                    AND TB1.Status = 0
                                    LEFT JOIN TitleBasicInfo TB2 ON IPL.TitleID = TB2.TitleID
                                                                    AND TB2.Status = 0
                                    LEFT JOIN Chitunion2017.dbo.UserInfo U1 ON MRL.CreateUserID = U1.UserID
                                    LEFT JOIN Chitunion2017.dbo.UserInfo U2 ON IPL.CreateUserID = U2.UserID
                            WHERE   MRL.BatchAuditID = ( SELECT TOP 1
                                                                BatchAuditID
                                                         FROM   dbo.BatchMedia
                                                         WHERE  BatchMediaID = {BatchMediaID}
                                                       )
                                    AND MRL.Type = {(int)EnumLabelType.IP}";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql).Tables[0];
        }
    }
}
