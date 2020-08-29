using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class BusinessGroup : DataBase
    {
        public const string P_BusinessGroup_SELECT = "p_BusinessGroup_Select";

        #region Instance
        public static readonly BusinessGroup Instance = new BusinessGroup();
        #endregion

        public DataTable GetAllBusinessGroup()
        {
            string sqlStr = @"SELECT bg.*,CallNum FROM BusinessGroup as bg Left Join CallDisplay as cd on bg.CDID=cd.CDID 
                                            where bg.Status>=0 order by bg.Status,bg.Name";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetBusinessGroupByAreaID(int regionId)
        {
            string sqlStr = @"SELECT bg.*,CallNum FROM BusinessGroup as bg Left Join CallDisplay as cd on bg.CDID=cd.CDID 
                                            where bg.Status=0 And RegionID=@RegionID 
                                            order by bg.Name";
            SqlParameter parameter = new SqlParameter("RegionID", SqlDbType.Int);
            parameter.Value = regionId;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public DataTable GetCallDisplay(bool hastel)
        {
            string sqlStr = "SELECT * FROM CallDisplay WHERE Status>=0 ORDER BY OrderNum";
            if (hastel)
            {
                sqlStr = "SELECT * FROM CallDisplay WHERE Status>=0 AND ISNULL(TelMainNum,'')<>''  ORDER BY OrderNum";
            }
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr).Tables[0];
        }

        public DataTable GetBusinessLine()
        {
            string sqlStr = "SELECT * FROM dbo.BusinessLine ORDER BY RecID";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr).Tables[0];
        }

        public DataTable GetBusinessLine(int bgid)
        {
            string sqlStr = "SELECT a.* FROM dbo.BusinessLine a,dbo.BusinessGroupLineMapping b WHERE a.RecID=b.LineID AND b.BGID=" + bgid + " ORDER BY a.RecID";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr).Tables[0];
        }

        public bool CheckGroupNameIsNotUsed(int bgid, string name)
        {
            string sql = String.Empty;
            name = name.Trim();
            if (bgid <= 0)
            {
                sql = "SELECT COUNT(*) FROM dbo.BusinessGroup WHERE Name='" + StringHelper.SqlFilter(name) + "'";
            }
            else
            {
                sql = "SELECT COUNT(*) FROM dbo.BusinessGroup WHERE Name='" + StringHelper.SqlFilter(name) + "' AND BGID<>'" + bgid + "'";
            }
            int count = (int)SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, sql);
            return count == 0;
        }

        public DataTable GetInUseBusinessGroup(int userid)
        {
            string sqlStr = @"SELECT bg.*,CallNum FROM BusinessGroup as bg Left Join CallDisplay as cd on bg.CDID=cd.CDID where bg.Status=0 AND bg.BGID IN ( SELECT BGID
                         FROM   dbo.UserGroupDataRigth ugd
                         WHERE  ugd.UserID =  " + userid + ") ";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr).Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Entities.BusinessGroup model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,20),
					new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CDID", SqlDbType.Int,4),
                    new SqlParameter("@RegionID",SqlDbType.Int,4),
					new SqlParameter("@CreateTime", SqlDbType.VarChar,50),
                    new SqlParameter("@BusinessType",SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Status;
            parameters[3].Value = model.CDID;
            parameters[4].Value = model.RegionID;
            parameters[5].Value = model.CreateTime;
            parameters[6].Value = model.BusinessType;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_BusinessGroup_Insert", parameters);
            return (int)parameters[0].Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Entities.BusinessGroup model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@BGID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,20),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CDID", SqlDbType.Int,4),
                    new SqlParameter("@RegionID",SqlDbType.Int,4),
                    new SqlParameter("@BusinessType",SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.BGID;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Status;
            parameters[3].Value = model.CDID;
            parameters[4].Value = model.RegionID;
            parameters[5].Value = model.BusinessType;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_BusinessGroup_Update", parameters);
            return (int)parameters[0].Value;
        }


        public DataTable GetBusinessGroupByBGID(int bgId)
        {
            string sqlStr = "SELECT * FROM BusinessGroup WHERE BGID=@BGID";
            SqlParameter parameter = new SqlParameter("@BGID", bgId);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            return ds.Tables[0];
        }

        public Entities.BusinessGroup GetBusinessGroupInfoByBGID(int bgId)
        {
            DataTable dt = GetBusinessGroupByBGID(bgId);
            if (dt != null && dt.Rows.Count > 0)
            {
                return LoadSingleBusinessGroup(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        private Entities.BusinessGroup LoadSingleBusinessGroup(DataRow row)
        {
            Entities.BusinessGroup model = new Entities.BusinessGroup();

            if (row["BGID"].ToString() != "")
            {
                model.BGID = int.Parse(row["BGID"].ToString());
            }
            model.Name = row["Name"].ToString();
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CDID"].ToString() != "")
            {
                model.CDID = int.Parse(row["CDID"].ToString());
            }
            if (row["RegionID"].ToString() != "")
            {
                model.RegionID = int.Parse(row["RegionID"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["BusinessType"].ToString() != "")
            {
                model.BusinessType = int.Parse(row["BusinessType"].ToString());
            }
            return model;
        }
        /// <summary>
        /// 按名称查询业务组信息（精确查询）
        /// </summary>
        /// <param name="name">业务组名称</param>
        /// <returns></returns>
        public DataTable GetBusinessGroupByName(string name)
        {
            string sqlStr = "SELECT * FROM BusinessGroup WHERE Name=@Name";
            SqlParameter parameter = new SqlParameter("@Name", name);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameter);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据userid获取业务组标签(加数据权限)
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="OnlyByGroup">是否只是按所在分组查询数据</param>
        /// <param name="OnlyByGroup">是否显示停用的标签</param>
        /// <returns></returns>
        public DataTable GetBusinessGroupTagsByUserID(int UserID, bool OnlyByGroup = false, bool IsShowStop = true)
        {

            //            string where = "";

            //            where += string.Format(" And bg.BGID in (SELECT BGID FROM dbo.EmployeeAgent WHERE UserID={0} UNION select BGID from UserGroupDataRigth ugdr where ugdr.userid={1})", UserID, UserID);


            //            if (!IsShowStop)
            //            {
            //                //不显示
            //                where += " And tag.Status=0";
            //            }

            //            string sqlStr = @"SELECT bg.Name AS GroupName,bg.BGID,tag.TagName,tag.RecID AS TagID, tag.pid,
            //                                                    CASE tag.Status WHEN 0 THEN 'true' WHEN 1 THEN 'false' ELSE NULL END AS IsUsed ,ordernum
            //                                                    FROM BusinessGroup bg 
            //                                                    left JOIN WorkOrderTag tag  ON tag.BGID=bg.BGID " +
            //                                    "WHERE bg.status=0 " + where +
            //                                    " order by GroupName,pid,ordernum ";
            SqlParameter parameter = new SqlParameter("@userId", UserID);

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "GetBusinessGroupTagsByUserID", parameter);

            return ds.Tables[0];
        }


        /// <summary>
        /// 根据userid获取其所在业务组标签
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataTable GetInBusinessGroupTagsByUserID(int UserID)
        {
            string where = "";

            where += string.Format(" And tag.Status=0 And bg.BGID in (SELECT BGID FROM dbo.EmployeeAgent WHERE UserID={0})", UserID);

            string sqlStr = @"SELECT bg.Name AS GroupName,bg.BGID,tag.TagName,tag.RecID AS TagID, tag.pid,
                                                    CASE tag.Status WHEN 0 THEN 'true' WHEN 1 THEN 'false' ELSE NULL END AS IsUsed ,ordernum
                                                    FROM BusinessGroup bg 
                                                    left JOIN WorkOrderTag tag  ON tag.BGID=bg.BGID " +
                                    "WHERE 1=1 " + where +
                                    " order by GroupName,pid,ordernum ";
            //SqlParameter parameter = new SqlParameter("@UserID", UserID);

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);

            return ds.Tables[0];
        }

        /// <summary>
        /// 获取业务组标签,添加一级分类,删除的数据不可见
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="pid">父类别</param>
        /// <param name="OnlyByGroup">是否只是按所在分组查询数据</param>
        /// <param name="OnlyByGroup">是否显示停用的标签</param>
        /// <returns></returns>
        public DataTable GetTagsByBG_Pid(string BGID, string Pid, string strStatus)
        {
            if (string.IsNullOrEmpty(BGID) || string.IsNullOrEmpty(Pid))
                return null;
            if (string.IsNullOrEmpty(strStatus))
            {
                strStatus = "0,1";
            }
            string where = string.Format(" where  tag.pid={0} and tag.bgid={1} and tag.status in({2}) ORDER BY tag.OrderNum,tag.TagName ", StringHelper.SqlFilter(Pid), StringHelper.SqlFilter(BGID), Dal.Util.SqlFilterByInCondition(strStatus));


            string sqlStr = @"	
                	SELECT 
		                tag.BGID,p.TagName AS pName,p.RecID AS pid,tag.TagName,tag.RecID AS TagID, 
		                tag.Status,CASE tag.Status WHEN 0 THEN '在用' WHEN 1 THEN '停用' ELSE '删除' END AS IsUsed 
	                FROM WorkOrderTag tag  
		                LEFT JOIN WorkOrderTag p ON tag.pid = p.RecID " + where;


            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);

            return ds.Tables[0];
        }

        public DataTable GetTagsCatalogsByBG_Pid(string BGID, string Pid)
        {
            string where = string.Format(" where tag.status in(0,1) and tag.pid={0} and tag.bgid={1} ORDER BY OrderNum,TagName ", StringHelper.SqlFilter(Pid), StringHelper.SqlFilter(BGID));

            string sqlStr = @"	
	            SELECT 
		            tag.RecID AS id,tag.TagName
	            FROM WorkOrderTag tag  	" + where;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);

            return ds.Tables[0];
        }

        /// <summary>
        /// 根据UserID,BGID获取某个用户某个业务组标签
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroupTagsByBGID(int UserID, int BGID)
        {
            string sqlStr = "SELECT bg.Name AS GroupName,bg.BGID,tag.TagName,tag.RecID AS TagID, " +
                            "CASE tag.Status WHEN 0 THEN 'true' WHEN 1 THEN 'false' ELSE 'true' END AS IsUsed " +
                            "FROM BusinessGroup bg " +
                            "LEFT JOIN UserGroupDataRigth ugdr ON ugdr.BGID=bg.BGID " +
                            "LEFT JOIN WorkOrderTag tag ON tag.BGID=bg.BGID " +
                            "WHERE ugdr.BGID=@BGID AND ugdr.UserID=@UserID";

            SqlParameter[] parameters = {
					new SqlParameter("@UserID", UserID),
					new SqlParameter("@BGID", BGID)
					};

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据BGID获取某个业务组标签
        /// </summary>
        /// <param name="BGID"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroupTagsByBGIDOnly(int BGID)
        {
            string sqlStr = "SELECT bg.Name AS GroupName,bg.BGID,tag.TagName,tag.RecID AS TagID, " +
                            "CASE tag.Status WHEN 0 THEN 'true' WHEN 1 THEN 'false' ELSE 'true' END AS IsUsed,tag.OrderNum " +
                            "FROM BusinessGroup bg " +
                            "LEFT JOIN WorkOrderTag tag ON tag.BGID=bg.BGID " +
                            "WHERE bg.BGID=@BGID order by tag.OrderNum ";

            SqlParameter[] parameters = {
					new SqlParameter("@BGID", BGID)
					};

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr, parameters);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据组串获取业务组数据
        /// </summary>
        /// <param name="bgids"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroupByBGIDS(string bgids)
        {
            string sqlStr = "SELECT Name,BGID FROM dbo.BusinessGroup WHERE BGID IN(" + Dal.Util.SqlFilterByInCondition(bgids) + ") ";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
            return ds.Tables[0];
        }

        /// 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetBusinessGroup(QueryBusinessGroup query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = GetWhere(query);
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
                    new SqlParameter("@Employeewhere", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = GetEmployeewhere();
            parameters[2].Value = order;
            parameters[3].Value = pageSize;
            parameters[4].Value = currentPage;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_BusinessGroup_SELECT, parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }

        /// 获取查询条件
        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string GetWhere(QueryBusinessGroup query)
        {
            string where = "";
            //业务分类 （热线=1 在线=2 全部=3）
            if (!string.IsNullOrEmpty(query.BusinessType))
            {
                where += " AND bg.BusinessType in (" + Dal.Util.SqlFilterByInCondition(query.BusinessType) + ") ";
            }
            //状态
            if (!string.IsNullOrEmpty(query.Status))
            {
                where += " AND bg.Status in (" + Dal.Util.SqlFilterByInCondition(query.Status) + ") ";
            }
            //区域
            if (!string.IsNullOrEmpty(query.Region))
            {
                where += " AND bg.RegionID in (" + Dal.Util.SqlFilterByInCondition(query.Region) + ") ";
            }
            return where;
        }
        /// 获取员工查询条件
        /// <summary>
        /// 获取员工查询条件
        /// </summary>
        /// <returns></returns>
        private string GetEmployeewhere()
        {
            string where = "";
            string PartIDs = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("PartID");
            int DepCount = PartIDs.Split(',').Length;
            if (DepCount > 0)
            {
                where += " and (";
                for (int i = 0; i < DepCount; i++)
                {
                    if (i != 0)
                    {
                        where += " or ";
                    }
                    where += " DepartID in (select ID from SysRightsManager.dbo.f_Cid('" + PartIDs.Split(',')[i] + "')) ";
                }
                where += " )";
            }
            return where;
        }

        public DataTable GetBusinessGroupByUserIDs(string userids)
        {
            if (!string.IsNullOrEmpty(userids))
            {
                string sqlStr = @"SELECT bg.BGID,bg.Name AS BGName,ea.UserID FROM dbo.BusinessGroup AS bg
                            JOIN dbo.EmployeeAgent AS ea ON bg.BGID=ea.BGID
                            WHERE bg.Status=0 AND ea.UserID IN (" + Dal.Util.SqlFilterByInCondition(userids) + @")";

                DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlStr);
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].TableName = "dtUserGroup";
                    return ds.Tables[0];
                }
            }
            return null;
        }
    }
}
