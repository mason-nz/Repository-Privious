using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class WOrderInfo : DataBase
    {
        public static WOrderInfo Instance = new WOrderInfo();

        /// ����3�����ں����������
        /// <summary>
        /// ����3�����ں����������
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int CalcCallInCountByPhone(string phone)
        {
            string sql = @"SELECT COUNT(*) FROM dbo.CallRecord_ORIG 
                                    WHERE CallStatus=1 
                                    AND EstablishedTime>'1970-1-1'
                                    AND PhoneNum='" + SqlFilter(phone) + "'";
            return (int)SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        /// ��ѯ�����û���Ϣ
        /// <summary>
        /// ��ѯ�����û���Ϣ
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public DataTable GetCBInfoByPhone(string custid, string phone)
        {
            string sql = @"SELECT 
                                    TOP 1 
                                    a.CustID,
                                    a.CustName,
                                    CASE a.Sex WHEN 1 THEN 1 WHEN 2 THEN 2 ELSE 1 END AS Sex, --������Чʱ��Ĭ�ϡ�������
                                    CASE a.CustCategoryID WHEN 3 THEN 3 WHEN 4 THEN 4 ELSE 3 END AS CustCategoryID, --������Чʱ��Ĭ�ϡ����ˡ�
                                    CASE WHEN ISNULL(a.ProvinceID,-1)>0 THEN a.ProvinceID ELSE -1 END AS ProvinceID, --������Чʱ��Ĭ�ϡ�-1��
                                    CASE WHEN ISNULL(a.CityID,-1)>0 THEN a.CityID ELSE -1 END AS CityID, --������Чʱ��Ĭ�ϡ�-1��
                                    CASE WHEN ISNULL(a.CountyID,-1)>0 THEN a.CountyID ELSE -1 END AS CountyID, --������Чʱ��Ĭ�ϡ�-1��
                                    b.Tel AS MainTel,--������
                                    (STUFF((SELECT ','+Tel FROM CustTel WHERE CustID=a.CustID ORDER BY a.CreateTime DESC FOR XML PATH('')),1,1,'')) AS AllTel,--ȫ������
                                    c.MemberCode,--������
                                    c.Name AS MemberName --����������
                                    FROM dbo.CustBasicInfo a 
                                    INNER JOIN dbo.CustTel b ON (a.CustID=b.CustID)
                                    LEFT JOIN dbo.DealerInfo c ON(a.CustID=c.CustID AND a.CustCategoryID=3)
                                    WHERE a.Status=0";
            if (!string.IsNullOrEmpty(custid))
            {
                sql += " AND a.CustID='" + SqlFilter(custid) + "'";
            }
            if (!string.IsNullOrEmpty(phone))
            {
                sql += " AND b.Tel ='" + SqlFilter(phone) + "'";
            }
            sql += " ORDER BY a.CreateTime DESC,b.CreateTime DESC";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// ��ѯ����
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public WOrderInfoInfo GetWOrderInfoInfo(string orderid)
        {
            string sql = @"SELECT * FROM dbo.WOrderInfo WHERE OrderID='" + SqlFilter(orderid) + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                return new WOrderInfoInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// ��ȡ��ѯ����
        /// <summary>
        /// ��ȡ��ѯ����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetWorkOrderListWhere(QueryWOrderV2DataInfo query)
        {
            string where = string.Empty;

            if (query.BGID > 0)
            {
                where += " AND a.BGID=" + query.BGID;
            }
            if (!string.IsNullOrEmpty(query.BeginCreateTime))
            {
                where += " AND a.CreateTime>='" + SqlFilter(query.BeginCreateTime) + " 0:00:00'";
            }
            if (!string.IsNullOrEmpty(query.EndCreateTime))
            {
                where += "AND a.CreateTime<='" + SqlFilter(query.EndCreateTime) + " 23:59:59'";
            }
            if (query.BusiType > 0)
            {
                where += " AND a.BusinessType=" + query.BusiType;
            }
            if (!string.IsNullOrEmpty(query.ComplaintLevel))
            {
                where += " AND a.ComplaintLevel in  (" + SqlFilter(query.ComplaintLevel.Replace('-', ',')) + ") ";
            }
            if (!string.IsNullOrEmpty(query.Phone))
            {
                where += " AND a.Phone = '" + SqlFilter(query.Phone) + "'";
            }
            if (query.CreateUserID > 0)
            {
                where += " AND a.CreateUserID = " + query.CreateUserID;
            }
            if (!string.IsNullOrEmpty(query.CreateUserName) && query.CreateUserName.Trim() != "")
            {
                where += " AND vuser.TrueName LIKE '%" + query.CreateUserName.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(query.OrderID))
            {
                where += " AND a.OrderID = '" + SqlFilter(query.OrderID) + "'";
            }
            if (query.CategoryID > 0)
            {
                where += " AND a.CategoryID=" + query.CategoryID;
            }
            if (query.WorkOrderStatus > 0)
            {
                where += " AND a.WorkOrderStatus=" + query.WorkOrderStatus;
            }
            if (!string.IsNullOrEmpty(query.WorkOrderStatusList))
            {
                where += " AND a.WorkOrderStatus IN (" + Dal.Util.SqlFilterByInCondition(query.WorkOrderStatusList) + ")";
            }
            if (!string.IsNullOrEmpty(query.ReVisitStr))
            {
                where += " AND a.OrderID in ( SELECT OrderID FROM WOrderProcess WHERE ProcessType=" + (int)WOrderOperTypeEnum.L05_�ط� + " AND IsReturnVisit IN (" + SqlFilter(query.ReVisitStr.Replace('-', ',')) + " ))";
            }
            if (!string.IsNullOrEmpty(query.MemberName) && query.MemberName.Trim() != "")
            {
                where += " AND d.CustCategoryID=" + (int)CustTypeEnum.T02_������ + " AND d.CustID IN (SELECT CustID FROM dbo.DealerInfo WHERE Name LIKE '%" + SqlFilter(query.MemberName.Trim()) + "%' AND Status=0)";
            }
            if (query.BigTagID > 0)
            {
                where += " AND c.PID=" + query.BigTagID;
            }
            if (query.TagID > 0)
            {
                where += " AND c.RecID=" + query.TagID;
            }
            //crm�ӿ�ʹ��
            if (!string.IsNullOrEmpty(query.CRMCustID))
            {
                where += " AND a.CRMCustID='" + SqlFilter(query.CRMCustID) + "'";
            }
            if (!string.IsNullOrEmpty(query.CRMCustName) && query.CRMCustName.Trim() != "")
            {
                where += " AND crmcust.CustName LIKE '%" + SqlFilter(query.CRMCustName.Trim()) + "%'";
            }
            if (query.CRMCountyID > 0)
            {
                where += " AND crmcust.CountyID=" + query.CRMCountyID;
            }
            else if (query.CRMCityID > 0)
            {
                where += " AND crmcust.CityID=" + query.CRMCityID;
            }
            else if (query.CRMProvinceID > 0)
            {
                where += " AND crmcust.ProvinceID=" + query.CRMProvinceID;
            }
            if (!string.IsNullOrEmpty(query.ProcessUserName) && query.ProcessUserName.Trim() != "")
            {
                where += @" AND EXISTS (SELECT 1 FROM dbo.WOrderToAndCC 
                        WHERE ReceiverID=a.LastReceiverID AND PersonType=" + (int)WOrderPersonTypeEnum.P01_������ + @"
                        AND UserID IN (SELECT UserID FROM dbo.v_userinfo WHERE Status=0 AND TrueName LIKE '%" + SqlFilter(query.ProcessUserName.Trim()) + "%'))";
            }
            if (query.RightZN != null && !string.IsNullOrEmpty(query.RightZN.CRM_LoginDepartID))
            {
                where += " AND a.OrderID IN (SELECT OrderID FROM dbo.WOrderToAndCC WHERE PersonType=" + (int)WOrderPersonTypeEnum.P01_������
                    + " AND UserID IN ("
                    + "SELECT UserID FROM dbo.v_userinfo WHERE DepartID IN (SELECT id FROM Crm2009.[dbo].[f_GetChildDepartid]('" + SqlFilter(query.RightZN.CRM_LoginDepartID) + "'))"
                    + ")) ";
            }
            if (query.RightGR != null && query.RightGR.CRM_LoginID > 0 && !string.IsNullOrEmpty(query.RightGR.CRM_LoginDepartID))
            {
                where += GetRightGR(query.RightGR.CRM_LoginID, query.RightGR.CRM_LoginDepartID);
            }
            #region ����Ȩ���ж�
            if (query.CC_LoginID > 0)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstrByOrderWhere("a", "BGID", "CreateUserID", query.CC_LoginID,
                    " a.OrderID IN (SELECT OrderID FROM dbo.WOrderToAndCC WHERE UserID ='" + query.CC_LoginID + "') ");
                where += whereDataRight;
            }
            #endregion

            return where;
        }
        /// CRM�˸��˹�����Ȩ�޿���
        /// <summary>
        /// CRM�˸��˹�����Ȩ�޿���
        /// </summary>
        /// <param name="crmUserId"></param>
        /// <param name="crmUserDepartId"></param>
        /// <returns></returns>
        public string GetRightGR(int crmUserId, string crmUserDepartId)
        {
            string strRightSql = "";
            //��¼��Ȩ�޿���
            string strSql = "SELECT ISNULL(RightType,0) FROM Crm2009.dbo.UserCustDataRigth WHERE USERID=" + crmUserId + " AND ModuleType = 1";
            int strRightType = CommonFunction.ObjectToInteger(SqlHelper.ExecuteScalar(ConnectionStrings_CRM, CommandType.Text, strSql));
            switch (strRightType)
            {
                case 0: //����
                    strRightSql += " AND a.CrmCustID IN (SELECT CustID FROM Crm2009.dbo.CustUserMapping WHERE UserID =" + crmUserId + ")";
                    break;
                case 4: //����Ա
                    strRightSql += " AND ISNULL(a.CrmCustID,'')<>''";
                    break;
                case 1: //������
                    strRightSql += " AND (a.CrmCustID  IN (SELECT CustID FROM Crm2009.dbo.CustUserMapping WHERE UserID =" + crmUserId + ") "
                        + " OR a.CrmCustID IN (SELECT CustID FROM Crm2009.dbo.CustDepartMapping WHERE DepartID ='" + SqlFilter(crmUserDepartId) + "'))";
                    break;
                case 2: //�����ż��Ӳ���
                    strRightSql += " AND (a.CrmCustID  IN (SELECT CustID FROM Crm2009.dbo.CustUserMapping WHERE UserID =" + crmUserId + ") "
                        + " OR a.CrmCustID IN (SELECT CustID FROM Crm2009.dbo.CustDepartMapping WHERE DepartID IN (SELECT id FROM Crm2009.[dbo].[f_GetChildDepartid]('"
                        + SqlFilter(crmUserDepartId) + "'))))";
                    break;
                case 3: //ָ������
                    strRightSql += " AND (a.CrmCustID  IN (SELECT CustID FROM Crm2009.dbo.CustUserMapping WHERE UserID =" + crmUserId + ") "
                        + " OR a.CrmCustID IN (SELECT CustID FROM Crm2009.dbo.CustDepartMapping WHERE DepartID IN ("
                        + " SELECT DepartID FROM Crm2009.dbo.UserDepMentMapping WHERE UserID=" + crmUserId + " and ModuleType = 1)))";
                    break;
            }
            //��ȡ��ǰ�û���Ʒ��Ȩ�� ����crmcustid�ķ�Χ
            if (GetBrandCount(crmUserId) > 0)
            {
                strRightSql += @" AND (
			    a.CrmCustID IN (SELECT CustID FROM Crm2009.dbo.Cust_Brand WHERE BrandID IN(SELECT  DISTINCT
                                                DetailID AS BrandID
                                        FROM    Crm2009.dbo.V_DataRightDetail
                                        WHERE   ModuleType = 1
                                                AND type = 3
                                                AND UserID = " + crmUserId +
                                                    @" AND EXISTS ( SELECT  DISTINCT
                                                                    DetailID AS BrandID
                                                             FROM   SysRightsManager.dbo.DataRightDetail
                                                             WHERE  ModuleType = 1
                                                                    AND type = 3
                                                                    AND UserID = " + crmUserId + @" ))
						 ) OR a.CrmCustID IN (SELECT CustID FROM Crm2009.dbo.CustUserMapping WHERE UserID =" + crmUserId + "))";
            }
            return strRightSql;
        }
        /// ��ȡ�û���Ʒ��Ȩ������
        /// <summary>
        /// ��ȡ�û���Ʒ��Ȩ������
        /// </summary>
        /// <param name="crmUserId"></param>
        /// <returns></returns>
        public int GetBrandCount(int crmUserId)
        {
            string strSql = @"SELECT  COUNT(DetailID)
                            FROM    Crm2009.dbo.V_DataRightDetail
                            WHERE   ModuleType = 1
                                    AND type = 3
                                    AND UserID = " + crmUserId + @"
                                    AND EXISTS ( SELECT  DISTINCT
                                                        DetailID AS BrandID
                                                 FROM   SysRightsManager.dbo.DataRightDetail
                                                 WHERE  ModuleType = 1
                                                        AND type = 3
                                                        AND UserID =" + crmUserId + ")";
            object obj = SqlHelper.ExecuteScalar(ConnectionStrings_CRM, CommandType.Text, strSql);
            return CommonFunction.ObjectToInteger(obj, 0);
        }
        /// ���ղ�ѯ������ѯ(��������Ȩ��)
        /// <summary>
        /// ���ղ�ѯ������ѯ(��������Ȩ��)
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetWorkOrderInfoForList(QueryWOrderV2DataInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetWorkOrderListWhere(query);
            string sqlstr = @"
                            SELECT 
                            a.*,
                            ISNULL(STUFF((SELECT ',' + RTRIM(UserID) + ':'+UserName FROM dbo.WOrderToAndCC 
                            WHERE ReceiverID =a.LastReceiverID  and PersonType=1 FOR XML PATH('')), 1, 1, ''), '') LastReceivers,
                            isnull(b.BusiTypeName,'--') BusiTypeName,
                            CONVERT(NVARCHAR(50),isnull(e.TagName+'/','-')+isnull(c.TagName,'-'))  TagName, 
                            d.CustCategoryID,
                            vuser.TrueName,
                            ISNULL(crmcust.CustName,'--') AS CRMCustName
                            YanFaFROM WOrderInfo a  
                            LEFT JOIN dbo.WOrderBusiType b ON a.BusinessType=b.RecID
                            LEFT JOIN dbo.WOrderTag c ON a.BusinessTag=c.RecID
                            LEFT JOIN dbo.WOrderTag e ON c.PID=e.RecID
                            LEFT JOIN dbo.CustBasicInfo d ON a.CBID=d.CustID
                            LEFT JOIN CRM2009.dbo.CustInfo crmcust ON a.CRMCustID=crmcust.CustID
                            INNER JOIN v_userinfo vuser ON a.CreateUserID=vuser.UserID
                            WHERE a.Status=0  " + where;

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
					new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = sqlstr;
            parameters[1].Value = order;
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_Page", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        /// ��ѯ�¾ɹ���-�ͻ��ط�
        /// <summary>
        /// ��ѯ�¾ɹ���-�ͻ��ط�
        /// </summary>
        /// <param name="crmcustid"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetWorkOrderInfoForV1V2Info(string crmcustid, int loginuserid, int currentPage, int pageSize, out int totalCount)
        {
            string where = "";
            // ����Ȩ���ж�
            if (loginuserid > 0)
            {
                where += Dal.UserGroupDataRigth.Instance.GetSqlRightstr("a", "BGID", "CreateUserID", loginuserid);
            }
            //�ͻ�id
            where += " AND a.CRMCustID='" + crmcustid + "'";
            string sql_old = @"SELECT 'v1' AS [version],OrderID,CRMCustID,CreateUserID,CreateTime,BGID,Content,WorkOrderStatus 
                                            FROM dbo.WorkOrderInfo a
                                            WHERE 1=1 " + where;
            string sql_new = @"SELECT 'v2' AS [version],OrderID,CRMCustID,CreateUserID,CreateTime,BGID,Content,WorkOrderStatus 
                                            FROM dbo.WOrderInfo a
                                            WHERE 1=1 " + where;
            string sql = @"
                                SELECT tmp.* ,
                                v.TrueName
                                YanFaFROM (
                                SELECT * FROM (" + sql_old + @") tmp1 
                                UNION ALL
                                SELECT * FROM (" + sql_new + @") tmp2
                                ) tmp
                                INNER JOIN v_userinfo v ON tmp.CreateUserID=v.UserID";

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
					new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = sql;
            parameters[1].Value = "CreateTime desc";
            parameters[2].Value = currentPage;
            parameters[3].Value = pageSize;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_Page", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// ������¼�������ݲ�ѯ
        /// <summary>
        /// ������¼�������ݲ�ѯ
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public DataTable GetExportWorkOrderList(QueryWOrderV2DataInfo query, string order)
        {
            string where = GetWorkOrderListWhere(query);
            SqlParameter[] parmas = new SqlParameter[]{
                new SqlParameter("@where", SqlDbType.NVarChar,2000){ Value=where }
            };
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_WOrderInfo_Export", parmas).Tables[0];
        }

        /// ��ѯCRM���ʷ���
        /// <summary>
        /// ��ѯCRM���ʷ���
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetCrmVistType()
        {
            string sqlstr = @"SELECT * FROM CRM2009..DictInfo WHERE  DictType =101";

            Dictionary<int, string> dict = new Dictionary<int, string>();
            DataSet ds;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dict.Add(CommonFunction.ObjectToInteger(dr["DictID"]), CommonFunction.ObjectToString(dr["DictName"]));
            }
            return dict;
        }
    }
}
