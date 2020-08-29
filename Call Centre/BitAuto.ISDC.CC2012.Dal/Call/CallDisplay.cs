using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Data;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class CallDisplay : DataBase
    {
        public static CallDisplay Instance = new CallDisplay();

        /// 创建热线管理类
        /// <summary>
        /// 创建热线管理类
        /// </summary>
        /// <returns></returns>
        public TelNumManage CreateTelNumManage()
        {
            DataTable dt_call = GetAllXianCallDisplay();
            DataTable dt_skill = GetAllSkillGroup();

            TelSkillGroup skillinfo = new TelSkillGroup();
            skillinfo.SkillDic = new Dictionary<int, List<TelSkill>>();
            foreach (DataRow dr_call in dt_call.Select("", "CDID"))
            {
                int CDID = CommonFunction.ObjectToInteger(dr_call["CDID"]);
                List<TelSkill> list = new List<TelSkill>();
                foreach (DataRow dr_skill in dt_skill.Select("CDID=" + CDID, "SGID"))
                {
                    string key = dr_skill["ManufacturerSGID"].ToString();
                    string name = dr_skill["Name"].ToString();
                    list.Add(new TelSkill(key, name));
                }
                skillinfo.SkillDic[CDID] = list;
            }

            TelNumManage manage = new TelNumManage(skillinfo);
            manage.TelNumList = new List<TelNum>();
            foreach (DataRow dr_call in dt_call.Select("", "OrderNum"))
            {
                int CDID = CommonFunction.ObjectToInteger(dr_call["CDID"]);
                string tel = dr_call["TelMainNum"].ToString();
                string _out = dr_call["OutCallNum"].ToString();
                string name = dr_call["Remark"].ToString();
                string longname = dr_call["CallNum"].ToString();
                int no = CommonFunction.ObjectToInteger(dr_call["HotlineID"]);
                int mutilid = CommonFunction.ObjectToInteger(dr_call["MutilID"]);
                string areacode = dr_call["AreaCode"].ToString();

                WorkOrderDataSource datasource = WorkOrderDataSource.Other;
                int d = CommonFunction.ObjectToInteger(dr_call["WorkOrderDataSourceID"]);
                if (Enum.IsDefined(typeof(WorkOrderDataSource), d))
                {
                    datasource = (WorkOrderDataSource)d;
                }

                manage.TelNumList.Add(new TelNum()
                {
                    Tel = tel,
                    Out = _out,
                    Name = name,
                    LongName = longname,
                    HotlineID = no,
                    Skills = skillinfo.SkillDic[CDID],
                    DataSource = datasource,
                    MutilID = mutilid,
                    AreaCode = areacode
                });
            }

            return manage;
        }

        /// 获取在用的热线
        /// <summary>
        /// 获取在用的热线
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllXianCallDisplay()
        {
            string sql = "SELECT * FROM CallDisplay where HotlineID>0 ORDER BY OrderNum";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// 获取技能组
        /// <summary>
        /// 获取技能组
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllSkillGroup()
        {
            string sql = "SELECT * FROM SkillGroup ORDER BY SGID";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// 根据技能组获取热线
        /// <summary>
        /// 根据技能组获取热线
        /// </summary>
        /// <param name="ManufacturerSGID"></param>
        /// <returns></returns>
        public DataTable GetCallDisplayByManufacturerSGID(string ManufacturerSGID)
        {
            string sql = "SELECT b.AreaCode,b.TelMainNum FROM dbo.SkillGroup a INNER JOIN dbo.CallDisplay b ON  b.CDID = a.CDID WHERE a.ManufacturerSGID='" + ManufacturerSGID + "'";
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }

        /// 获取北京库变化的热线表数据
        /// <summary>
        /// 获取北京库变化的热线表数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetChangedCallDisplayFromBJ(long maxid)
        {
            string sql = "SELECT *, CAST(TIMESTAMP AS BIGINT) AS long_TIMESTAMP FROM dbo.CallDisplay WHERE CAST(TIMESTAMP AS BIGINT)>" + maxid;
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
        }
        /// 获取西安库热线表最大时间戳
        /// <summary>
        /// 获取西安库热线表最大时间戳
        /// </summary>
        /// <returns></returns>
        public long GetCallDisplayMaxTimeStamp_XA()
        {
            string sql = "SELECT ISNULL(MAX(TIMESTAMP),0) FROM dbo.CallDisplay";
            return CommonFunction.ObjectToLong(SqlHelper.ExecuteScalar(ConnectionStrings_Holly_Business, CommandType.Text, sql));
        }
        /// 清除热线数据
        /// <summary>
        /// 清除热线数据
        /// </summary>
        public void ClearCallDisplayTemp_XA()
        {
            string sql = "DELETE FROM dbo.CallDisplay_Temp";
            SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, sql);
        }
        /// 从临时表更新正式表
        /// <summary>
        /// 从临时表更新正式表
        /// </summary>
        /// <returns></returns>
        public int[] UpdateCallDisplayFromTemp_XA()
        {
            int mod = 0;
            int add = 0;
            //更新
            string modsql = @"UPDATE CallDisplay SET
                                        CallNum=tmp.CallNum,
                                        OutCallNum=tmp.OutCallNum,
                                        Remark=tmp.Remark,
                                        TelMainNum=tmp.TelMainNum,
                                        HotlineID=tmp.HotlineID,
                                        WorkOrderDataSourceID=tmp.WorkOrderDataSourceID,
                                        AreaCode=tmp.AreaCode,
                                        Status=tmp.Status,
                                        OrderNum=tmp.OrderNum,
                                        CreateTime=tmp.CreateTime,
                                        MutilID=tmp.MutilID,
                                        [TIMESTAMP]=tmp.[TIMESTAMP],
                                        LastSyncTime=GetDate()
                                        FROM CallDisplay_Temp tmp
                                        WHERE  CallDisplay.CDID=tmp.CDID";
            mod = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, modsql);
            //新增
            string addsql = @"INSERT INTO CallDisplay (
                                        CDID,CallNum,OutCallNum,Remark,TelMainNum,HotlineID,WorkOrderDataSourceID,AreaCode,Status,OrderNum,
                                        MutilID,[TIMESTAMP],LastSyncTime)
                                        SELECT 
                                        CDID,CallNum,OutCallNum,Remark,TelMainNum,HotlineID,WorkOrderDataSourceID,AreaCode,Status,OrderNum,
                                        MutilID,[TIMESTAMP],GetDate()
                                        FROM CallDisplay_Temp
                                        WHERE CDID NOT IN (SELECT CDID FROM CallDisplay)";
            add = SqlHelper.ExecuteNonQuery(ConnectionStrings_Holly_Business, CommandType.Text, addsql);
            return new int[] { mod, add };
        }
    }
}
