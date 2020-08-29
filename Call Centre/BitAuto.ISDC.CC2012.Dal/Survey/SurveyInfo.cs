using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ���ݷ�����SurveyInfo��
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-10-24 10:32:17 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class SurveyInfo : DataBase
    {
        #region Instance
        public static readonly SurveyInfo Instance = new SurveyInfo();
        #endregion

        #region const
        private const string P_SURVEYINFO_SELECT = "p_SurveyInfo_Select";
        private const string P_SURVEYINFO_INSERT = "p_SurveyInfo_Insert";
        private const string P_SURVEYINFO_UPDATE = "p_SurveyInfo_Update";
        private const string P_SURVEYINFO_DELETE = "p_SurveyInfo_Delete";
        #endregion

        #region Contructor
        protected SurveyInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetSurveyInfo(QuerySurveyInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            //����Ȩ���ж�
            //if ((query.OwnGroup != Constant.STRING_INVALID_VALUE && query.OwnGroup != string.Empty) || (query.OneSelf != Constant.STRING_INVALID_VALUE && query.OneSelf != string.Empty))
            //{
            //    if (query.LoginID != Constant.INT_INVALID_VALUE)
            //    {
            //        where += " AND (";

            //        if (query.OwnGroup != Constant.STRING_INVALID_VALUE && query.OwnGroup != string.Empty)
            //        {
            //            //ɸѡ��½�˹��������ҵ����Ȩ���� ���� ����Ϣ
            //            where += " SurveyCategory.BGID IN ( " + query.OwnGroup + ") ";
            //        }

            //        if (query.OwnGroup != Constant.STRING_INVALID_VALUE && query.OwnGroup != string.Empty && query.OneSelf != Constant.STRING_INVALID_VALUE && query.OneSelf != string.Empty)
            //        {
            //            where += " OR ";
            //        }

            //        if (query.OneSelf != Constant.STRING_INVALID_VALUE && query.OneSelf != string.Empty)
            //        {
            //            //ɸѡ��½�˹��������ҵ����Ȩ���� ���� ����Ϣ 
            //            where += " (SurveyCategory.BGID IN (" + query.OneSelf + ") AND SurveyInfo.CreateUserID=" + query.LoginID + ")";
            //        }

            //        where += ")";
            //    }
            //}

            #region ����Ȩ���ж�
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                where = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("SurveyCategory", "SurveyInfo", "BGID", "CreateUserID", query.LoginID);
            }
            #endregion

            //״̬��0-δ��ɣ�1-δʹ�ã�2-��ʹ�ã���Ҫ�жϣ�
            if (query.Statuss != Constant.STRING_INVALID_VALUE)
            {
                if (query.Statuss.Contains("0") || query.Statuss.Contains("1") || query.Statuss.Contains("2"))
                {

                    string[] array_status = Dal.Util.SqlFilterByInCondition(query.Statuss).Split(',');

                    where += " AND (";

                    string whereStr = string.Empty;

                    for (int i = 0; i < array_status.Length; i++)
                    {
                        if (array_status[i] == "0")
                        {
                            whereStr += "OR ";

                            whereStr += " (SurveyInfo.Status = 0 ";

                            //������SurveyProjectInfo���в����ڣ����������ʹ�õ�
                            whereStr += " AND SurveyInfo.SIID NOT IN (Select top 1 SIID From SurveyProjectInfo where SurveyProjectInfo.SIID = SurveyInfo.SIID) )";
                        }
                        if (array_status[i] == "1")
                        {
                            whereStr += "OR ";

                            whereStr += " (SurveyInfo.Status = 1";

                            //������SurveyProjectInfo���в����ڣ����������ʹ�õ�
                            whereStr += @" AND SurveyInfo.SIID NOT IN (Select top 1 SIID From SurveyProjectInfo where SurveyProjectInfo.SIID = SurveyInfo.SIID 
                  UNION
                  SELECT TOP 1
                            SIID
                  FROM      ProjectSurveyMapping
                  WHERE     ProjectSurveyMapping.SIID = SurveyInfo.SIID) )";
                        }
                        if (array_status[i] == "2")
                        {
                            whereStr += "OR ";

                            whereStr += @" ( SurveyInfo.SIID IN (Select top 1 SIID From SurveyProjectInfo where SurveyProjectInfo.SIID = SurveyInfo.SIID
                  UNION
                  SELECT TOP 1
                            SIID
                  FROM      ProjectSurveyMapping
                  WHERE     ProjectSurveyMapping.SIID = SurveyInfo.SIID)";
                            //������SurveyInfo�е�״̬����Ϊ1-δʹ�ã��������п�����SurveyProjectInfo���д���
                            whereStr += " AND SurveyInfo.Status = 1 )";

                        }

                        if (i == array_status.Length - 1)
                        {
                            where += whereStr.TrimStart('O', 'R');
                        }
                    }

                    where += ")";
                }

            }

            if (query.SIID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyInfo.SIID=" + query.SIID;
            }

            if (query.Name != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SurveyInfo.Name like '%" + StringHelper.SqlFilter(query.Name) + "%'";
            }

            if (query.BGID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyInfo.BGID=" + query.BGID;
            }

            if (query.SCID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyInfo.SCID=" + query.SCID;
            }

            if (query.IsAvailable != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyInfo.IsAvailable =" + query.IsAvailable;
            }

            if (query.IsAvailables != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SurveyInfo.IsAvailable IN (" + Dal.Util.SqlFilterByInCondition(query.IsAvailables) + ")";
            }

            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND SurveyInfo.CreateUserID =" + query.CreateUserID;
            }

            if (query.BeginTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SurveyInfo.CreateTime >='" + StringHelper.SqlFilter(query.BeginTime) + " 0:0:0'";
            }

            if (query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " AND SurveyInfo.CreateTime <='" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59'";
            }

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYINFO_SELECT, parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.SurveyInfo GetSurveyInfo(int SIID)
        {
            QuerySurveyInfo query = new QuerySurveyInfo();
            query.SIID = SIID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSurveyInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleSurveyInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.SurveyInfo LoadSingleSurveyInfo(DataRow row)
        {
            Entities.SurveyInfo model = new Entities.SurveyInfo();

            if (row["SIID"].ToString() != "")
            {
                model.SIID = int.Parse(row["SIID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            if (row["SCID"].ToString() != "")
            {
                model.SCID = int.Parse(row["SCID"].ToString());
            }
            model.Description = row["Description"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["IsAvailable"].ToString() != "")
            {
                model.IsAvailable = int.Parse(row["IsAvailable"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            return model;
        }
        #endregion

        #region Insert
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(Entities.SurveyInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsAvailable", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.IsAvailable;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.SurveyInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsAvailable", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.IsAvailable;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYINFO_INSERT, parameters);
            return (int)parameters[0].Value;
        }
        #endregion

        #region Update
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(Entities.SurveyInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsAvailable", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SIID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.IsAvailable;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYINFO_UPDATE, parameters);
        }
        /// <summary>
        ///  ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.SurveyInfo model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@SCID", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.NVarChar,500),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsAvailable", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SIID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.BGID;
            parameters[3].Value = model.SCID;
            parameters[4].Value = model.Description;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.IsAvailable;
            parameters[7].Value = model.CreateTime;
            parameters[8].Value = model.CreateUserID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYINFO_UPDATE, parameters);
        }
        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int SIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4)};
            parameters[0].Value = SIID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_SURVEYINFO_DELETE, parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int SIID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@SIID", SqlDbType.Int,4)};
            parameters[0].Value = SIID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, P_SURVEYINFO_DELETE, parameters);
        }
        #endregion


        #region ��ȡ���д�����
        public DataTable getCreateUser()
        {

            //            string strSql = @"
            //                        SELECT DISTINCT a.CreateUserID FROM dbo.SurveyInfo a
            //                         JOIN dbo.EmployeeAgent b ON a.CreateUserID = b.UserID
            //                         WHERE b.RegionID  =(SELECT RegionID FROM dbo.EmployeeAgent WHERE UserID=" + userId.ToString() + ") ";
            //            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, "SELECT DISTINCT CreateUserID FROM dbo.SurveyInfo");
            return ds.Tables[0];
        }
        #endregion
    }
}

