using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.Utils.Data;
using System.Data.SqlClient;
using System.Data;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class ProjectTask_MemberContactMapping : DataBase
    {
        #region Instance
        public static readonly ProjectTask_MemberContactMapping Instance = new ProjectTask_MemberContactMapping();
        #endregion

        public ProjectTask_MemberContactMapping()
        { }

        #region  Method

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddMemberContactMapping(Entities.ProjectTask_MemberContactMapping model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ProjectTask_MemberContactMapping(");
            strSql.Append("MemberID,ContactID,IsMain,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@MemberID,@ContactID,@IsMain,@CreateTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@MemberID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@ContactID", SqlDbType.Int,4),
					new SqlParameter("@IsMain", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.MemberID;
            parameters[1].Value = model.ContactID;
            parameters[2].Value = model.IsMain;
            parameters[3].Value = model.CreateTime;

            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateMemberContactMapping(Entities.ProjectTask_MemberContactMapping model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ProjectTask_MemberContactMapping set ");
            strSql.Append("MemberID=@MemberID,");
            strSql.Append("ContactID=@ContactID,");
            strSql.Append("IsMain=@IsMain,");
            strSql.Append("CreateTime=@CreateTime");
            strSql.Append(" where RecID=@RecID");
            SqlParameter[] parameters = {
					new SqlParameter("@MemberID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@ContactID", SqlDbType.Int,4),
					new SqlParameter("@IsMain", SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = model.MemberID;
            parameters[1].Value = model.ContactID;
            parameters[2].Value = model.IsMain;
            parameters[3].Value = model.CreateTime;
            parameters[4].Value = model.RecID;

            int rows = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return rows > 0;
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteMemberContactMapping(int recid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProjectTask_MemberContactMapping ");
            strSql.Append(" where RecID=@RecID");
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)
			};
            parameters[0].Value = recid;

            int rows = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return rows > 0;
        }

        /// <summary>
        /// 根据联系人编号删除关联
        /// </summary>
        /// <param name="contactid">联系人编号</param>
        /// <returns>bool</returns>
        public bool DeleteByContactID(int contactid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ProjectTask_MemberContactMapping ");
            strSql.Append(" where ContactID=@ContactID");
            SqlParameter[] parameters = {
					new SqlParameter("@ContactID", SqlDbType.Int,4)
			};
            parameters[0].Value = contactid;

            int rows = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return rows > 0;
        }

        #endregion

        #region Select
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_MemberContactMapping GetModel(int recid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RecID,MemberID,ContactID,IsMain,CreateTime from ProjectTask_MemberContactMapping ");
            strSql.Append(" where RecID=@RecID");
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)
			};
            parameters[0].Value = recid;

            Entities.ProjectTask_MemberContactMapping model = new Entities.ProjectTask_MemberContactMapping();
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["RecID"] != null && ds.Tables[0].Rows[0]["RecID"].ToString() != "")
                {
                    model.RecID = int.Parse(ds.Tables[0].Rows[0]["RecID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MemberID"] != null && ds.Tables[0].Rows[0]["MemberID"].ToString() != "")
                {
                    model.MemberID = new Guid(ds.Tables[0].Rows[0]["MemberID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactID"] != null && ds.Tables[0].Rows[0]["ContactID"].ToString() != "")
                {
                    model.ContactID = int.Parse(ds.Tables[0].Rows[0]["ContactID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsMain"] != null && ds.Tables[0].Rows[0]["IsMain"].ToString() != "")
                {
                    model.IsMain = int.Parse(ds.Tables[0].Rows[0]["IsMain"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CreateTime"] != null && ds.Tables[0].Rows[0]["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString());
                }
            }
            else
            {
                model = null;
            }
            return model;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_MemberContactMapping GetModel(string memberid, int contactid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RecID,MemberID,ContactID,IsMain,CreateTime from ProjectTask_MemberContactMapping ");
            strSql.Append(" where MemberID=@MemberID and ContactID =@ContactID");
            SqlParameter[] parameters = {
                    new SqlParameter("@MemberID", SqlDbType.UniqueIdentifier,16),
					new SqlParameter("@ContactID", SqlDbType.Int,4)};
            parameters[0].Value = new Guid(memberid);
            parameters[1].Value = contactid;

            Entities.ProjectTask_MemberContactMapping model = new Entities.ProjectTask_MemberContactMapping();
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["RecID"] != null && ds.Tables[0].Rows[0]["RecID"].ToString() != "")
                {
                    model.RecID = int.Parse(ds.Tables[0].Rows[0]["RecID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["MemberID"] != null && ds.Tables[0].Rows[0]["MemberID"].ToString() != "")
                {
                    model.MemberID = new Guid(ds.Tables[0].Rows[0]["MemberID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ContactID"] != null && ds.Tables[0].Rows[0]["ContactID"].ToString() != "")
                {
                    model.ContactID = int.Parse(ds.Tables[0].Rows[0]["ContactID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsMain"] != null && ds.Tables[0].Rows[0]["IsMain"].ToString() != "")
                {
                    model.IsMain = int.Parse(ds.Tables[0].Rows[0]["IsMain"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CreateTime"] != null && ds.Tables[0].Rows[0]["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(ds.Tables[0].Rows[0]["CreateTime"].ToString());
                }
            }
            else
            {
                model = null;
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT CC_MCM.RecID,CC_MCM.MemberID,ContactID,IsMain,CC_MCM.CreateTime,Name,Abbr,CName ");
            strSql.Append(" FROM ProjectTask_MemberContactMapping AS CC_MCM ");
            strSql.Append(" LEFT JOIN CRM2009.dbo.DMSMember ON CC_MCM.MemberID = CRM2009.dbo.DMSMember.ID");
            strSql.Append(" LEFT JOIN dbo.ProjectTask_Cust_Contact ON CC_MCM.ContactID =ProjectTask_Cust_Contact.ID");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataTable GetList(int top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (top > 0)
            {
                strSql.Append(" top " + top);
            }
            strSql.Append(" RecID,MemberID,ContactID,IsMain,CreateTime ");
            strSql.Append(" FROM ProjectTask_MemberContactMapping ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString()).Tables[0];
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM ProjectTask_MemberContactMapping ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString());
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        /// <summary>
        /// 根据会员编号获取会员的负责人(联系人)
        /// </summary>
        /// <param name="membercode">会员编号</param>
        /// <returns>DataTable</returns>
        public DataTable GetMemberManager(string membercode)
        {
            string strSql = @"IF EXISTS ( SELECT    RecID
                                            FROM    ProjectTask_MemberContactMapping AS CC_MCM
                                                    LEFT JOIN ProjectTask_DMSMember AS CC_DMS ON CC_MCM.MemberID = CC_DMS.ID
                                            WHERE   MemberCode = @MemberCode) 
                                    BEGIN
                                        SELECT  TOP 1 CI.CName ,
                                                CI.Email ,
                                                CI.Phone
                                        FROM    ProjectTask_MemberContactMapping AS CC_MCM
                                                LEFT JOIN ProjectTask_DMSMember AS CC_DMS ON CC_MCM.MemberID = CC_DMS.ID
                                                LEFT JOIN Crm2009.dbo.ContactInfo AS CI ON CC_MCM.ContactID = CI.ID
                                        WHERE   MemberCode = @MemberCode
                                                AND CI.Status = 0 AND CC_DMS.status=0 AND CC_DMS.syncstatus=170002 ORDER BY IsMain  DESC
                                    END
                                ELSE                                   
                                    BEGIN                                           
                                        SELECT  TOP 1
                                                CI.CName ,
                                                CI.Email ,
                                                CI.Phone
                                        FROM    ContactInfo AS CI
                                                LEFT JOIN ProjectTask_DMSMember AS CC_DMS ON CI.CustID = CC_DMS.CustID
                                        WHERE   CI.Status = 0
                                                AND MemberCode = @MemberCode AND CC_DMS.status=0 AND CC_DMS.syncstatus=170002
                                    END";
            SqlParameter[] parameters = {
					new SqlParameter("@MemberCode", SqlDbType.VarChar,50)};
            parameters[0].Value = membercode;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters).Tables[0];
        }

        #endregion

        #region Other

        #endregion
        #endregion  Method
    }
}
