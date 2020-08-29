using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.CallReport;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Dal
{
    public class Routepoint : DataBase
    {
        private const string P_Page = "P_Page";
        TelNumManage manage = null;
        TelNumManage Manage
        {
            get
            {
                if (manage == null)
                {
                    manage = Dal.CallDisplay.Instance.CreateTelNumManage();
                }
                return manage;
            }
        }

        private Routepoint()
        {
        }

        private static Routepoint instance = null;

        public static Routepoint Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Routepoint();
                }
                return instance;
            }
        }

        #region 热线数据报表
        /// 获取数据库连接字符串
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public string GetConnectionstrings()
        {
            return CONNECTIONSTRINGS;
        }
        /// 获取最大时间从表中
        /// <summary>
        /// 获取最大时间从表中
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="datecol"></param>
        /// <returns></returns>
        public DateTime GetMaxDateTimeFromTable(string tablename, string datecol, string where)
        {
            string sql = "select max(" + datecol + ") as maxdate from " + tablename + " where 1=1" + where;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count != 0)
                return CommonFunction.ObjectToDateTime(ds.Tables[0].Rows[0]["maxdate"], new DateTime());
            else return new DateTime();
        }
        /// 清楚数据
        /// <summary>
        /// 清楚数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public int ClearDataFormBeginToEnd(string tablename, DateTime st, DateTime et, string where)
        {
            string sql = @"DELETE FROM " + tablename + " WHERE BeginTime>='"
                + CommonFunction.GetDateTimeStr(st) + "' AND BeginTime<='"
                + CommonFunction.GetDateTimeStr(et) + "' "
                + where;
            int num = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            //如果数据为空，从1记数
            string chongzhi = @"IF NOT EXISTS (SELECT * FROM " + tablename + @")
                                            BEGIN
                                            DBCC CHECKIDENT(" + tablename + @",RESEED,0)
                                            END";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, chongzhi);
            return num;
        }
        /// 获取热点路由数据
        /// <summary>
        /// 获取热点路由数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="total"></param>
        /// <param name="hzdt"></param>
        /// <returns></returns>
        public DataTable GetRoutepointData(QueryRoutepoint query, int pageindex, int pagesize, out int total, out DataTable hzdt)
        {
            string hzsql = "";
            string sql = GetRoutepointSql(query, out hzsql);
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                //设置周起始星期
                SqlHelper.ExecuteNonQuery(conn, CommandType.Text, "SET DATEFIRST 1");
                //查询详情
                SqlParameter[] parameters = {
					new SqlParameter("@SQL", SqlDbType.NVarChar, 4000),
					new SqlParameter("@Order", SqlDbType.NVarChar, 200),
					new SqlParameter("@CurPage", SqlDbType.Int, 4),
					new SqlParameter("@PageRows", SqlDbType.Int, 4),
					new SqlParameter("@TotalRecorder", SqlDbType.Int, 4)
					};
                parameters[0].Value = sql;
                parameters[1].Value = "ordertime desc";
                parameters[2].Value = pageindex;
                parameters[3].Value = pagesize;
                parameters[4].Direction = ParameterDirection.Output;
                ds = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, P_Page, parameters);
                total = (int)(parameters[4].Value);
                //汇总数据查询
                hzdt = SqlHelper.ExecuteDataset(conn, CommandType.Text, hzsql).Tables[0];
            }
            return ds.Tables[0];
        }
        /// 组装各个统计方法的查询语句
        /// <summary>
        /// 组装各个统计方法的查询语句
        /// </summary>
        /// <param name="query"></param>
        /// <param name="hzsql"></param>
        /// <returns></returns>
        private string GetRoutepointSql(QueryRoutepoint query, out string hzsql)
        {
            string hourwhere = FromAndWhere(query.StartTime, query.EndTime, query.BusinessType);
            hzsql = "select " + SumFieldSql() + hourwhere;
            string sql = "";
            switch (query.ShowTime.ToLower())
            {
                case "hour":
                    sql = "select " + ShowHourTime() + EnumSql() + DetailFieldSql() + hourwhere;
                    break;
                case "day":
                    sql = "select " + DaySelect() + " * " + hourwhere;
                    sql = "select " + ShowDayTime() + EnumSql() + SumFieldSql() + " from (" + sql + ") tmp group by " + GroupByDay() + "objecttype";
                    break;
                case "week":
                    sql = "select " + WeekSelect() + " * " + hourwhere;
                    sql = "select " + ShowWeekTime(query.StartTime, query.EndTime) + EnumSql() + SumFieldSql() + " from (" + sql + ") tmp group by " + GroupByWeek() + "objecttype";
                    break;
                case "month":
                    sql = "select " + MonthSelect() + " * " + hourwhere;
                    sql = "select " + ShowMonthTime(query.StartTime, query.EndTime) + EnumSql() + SumFieldSql() + " from (" + sql + ") tmp group by " + GroupByMonth() + "objecttype";
                    break;
            }
            //修改转人工量指标 2015-3-4 强斐
            return @"select begintime,objecttype,n_entered,n_entered_out,n_answered," +
                        "rtrim(convert(decimal(18,2), pc_n_answered))+'%' as pc_n_answered," +
                        "rtrim(convert(decimal(18,2), pc_n_distrib_in_tr))+'%' as pc_n_distrib_in_tr," +
                //"rtrim(convert(decimal(18,2), av_t_distributed)) as av_t_distributed, " +
                        "rtrim(convert(decimal(18,2), av_t_answered)) as av_t_answered " +
                        "YanFaFROM (" + sql + ") t";
        }

        /// 明细查询sql
        /// <summary>
        /// 明细查询sql
        /// </summary>
        /// <returns></returns>
        private string DetailFieldSql()
        {
            //修改转人工量指标 2015-3-4 强斐
            return @"n_entered ,
                        n_entered_out ,
                        n_answered ,
                        case n_entered_out when 0 then 0 else round(n_answered * 100.0 / n_entered_out, 2) end as pc_n_answered ,
                        case n_entered_out when 0 then 0 else round(n_distrib_in_tr * 100.0 / n_entered_out, 2) end as pc_n_distrib_in_tr ," +
                //"round(av_t_distributed, 2) as av_t_distributed," +
                        "round(av_t_answered, 2) as av_t_answered";
        }
        /// 汇总查询sql
        /// <summary>
        /// 汇总查询sql
        /// </summary>
        /// <returns></returns>
        private string SumFieldSql()
        {
            //修改转人工量指标 2015-3-4 强斐
            return @"sum(n_entered) as n_entered ,
                        sum(n_entered_out) as n_entered_out ,
                        sum(n_answered) as  n_answered,
                        case sum(n_entered_out) when 0 then 0 else round(sum(n_answered) * 100.0 / sum(n_entered_out), 2) end as pc_n_answered ,        
                        case sum(n_entered_out) when 0 then 0 else round(sum(n_distrib_in_tr) * 100.0 / sum(n_entered_out), 2) end as pc_n_distrib_in_tr ," +
                //"round(avg(av_t_distributed), 2) as av_t_distributed,"+
                        "round(avg(av_t_answered), 2) as av_t_answered";
        }


        /// 查询表和条件的sql
        /// <summary>
        /// 查询表和条件的sql
        /// </summary>
        /// <param name="name"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string FromAndWhere(string st, string et, string type)
        {
            return @" from dbo.report_routepoint_hour
                            where 1 = 1
                            and begintime >= '" + StringHelper.SqlFilter(st) + @" 00:00:00'
                            and begintime <= '" + StringHelper.SqlFilter(et) + @" 23:59:59'
                            and objecttype = " + StringHelper.SqlFilter(type);
        }
        /// 枚举类型
        /// <summary>
        /// 枚举类型
        /// </summary>
        /// <returns></returns>
        private string EnumSql()
        {
            //001~099 是Genesys
            //101~199 是Holly
            string str = "";
            foreach (TelNum item in Manage.TelNumList)
            {
                str += " when " + ((int)item.HotlineID).ToString() + " then '" + item.Name + "' ";
            }
            string str_genesys = "case objecttype " + str + " else '' end";

            //Holly 正常的id+100
            str = "";
            foreach (TelNum item in Manage.TelNumList)
            {
                str += " when " + ((int)item.HotlineID + 100).ToString() + " then '" + item.Name + "' ";
            }
            string str_holly = "case objecttype " + str + " else '' end";

            return "case when objecttype<100 then (" + str_genesys + ") else (" + str_holly + ") end as objecttype,";
        }

        /// 天 日期相关字段
        /// <summary>
        /// 天 日期相关字段
        /// </summary>
        /// <returns></returns>
        private string DaySelect()
        {
            return @"cast(cast (begintime as date) as datetime) as daytime,";
        }
        /// 周 日期相关字段
        /// <summary>
        /// 周 日期相关字段
        /// </summary>
        /// <returns></returns>
        private string WeekSelect()
        {
            return @"cast(cast (begintime as date) as datetime)-datepart(weekday,cast(cast (begintime as date) as datetime))+1 as weekbegin,"
                           + "cast(cast (begintime as date) as datetime)-datepart(weekday,cast(cast (begintime as date) as datetime))+7 as weekend,";
        }
        /// 月 日期相关字段
        /// <summary>
        /// 月 日期相关字段
        /// </summary>
        /// <returns></returns>
        private string MonthSelect()
        {
            return @"dateadd(dd,-day(cast(cast (begintime as date) as datetime))+1,cast(cast (begintime as date) as datetime)) as monthbegin,"
                           + "DATEADD(mm,1,DATEADD(dd,-day(cast(cast (begintime as date) as datetime))+1,cast(cast (begintime as date) as datetime)))-1 as monthend,";
        }

        /// 显示时间-小时
        /// <summary>
        /// 显示时间-小时
        /// </summary>
        /// <returns></returns>
        private string ShowHourTime()
        {
            return "convert(varchar(13), begintime, 20) + '时至' + convert(varchar(2), dateadd(hour, 1, begintime), 8) + '时' as begintime,"
                + "begintime as ordertime,";
        }
        /// 显示时间-天
        /// <summary>
        /// 显示时间-天
        /// </summary>
        /// <returns></returns>
        private string ShowDayTime()
        {
            return @"convert(varchar(100), daytime, 23) as begintime,"
                + "daytime as ordertime,";
        }
        /// 显示时间-周
        /// <summary>
        /// 显示时间-周
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        private string ShowWeekTime(string st, string et)
        {
            st = StringHelper.SqlFilter(st);
            et = StringHelper.SqlFilter(et);
            string st_str = "case when weekbegin<'" + st + "' then '" + st + "' else weekbegin end";
            string et_str = "case when weekend>'" + et + "' then '" + et + "' else weekend end";
            return "convert(varchar(100), " + st_str + ", 23)+'至'+convert(varchar(100), " + et_str + " , 23) as begintime,"
                + "weekbegin as ordertime,";
        }
        /// 显示时间-月
        /// <summary>
        /// 显示时间-月
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        private string ShowMonthTime(string st, string et)
        {
            st = StringHelper.SqlFilter(st);
            et = StringHelper.SqlFilter(et);
            string st_str = "case when monthbegin<'" + st + "' then '" + st + "' else monthbegin end";
            string et_str = "case when monthend>'" + et + "' then '" + et + "' else monthend end";
            return "convert(varchar(100), " + st_str + ", 23)+'至'+convert(varchar(100), " + et_str + " , 23) as begintime,"
                + "monthbegin as ordertime,";
        }

        /// 按天分组
        /// <summary>
        /// 按天分组
        /// </summary>
        /// <returns></returns>
        private string GroupByDay()
        {
            return "daytime,";
        }
        /// 按周分组
        /// <summary>
        /// 按周分组
        /// </summary>
        /// <returns></returns>
        private string GroupByWeek()
        {
            return "weekbegin,weekend,";
        }
        /// 按月分组
        /// <summary>
        /// 按月分组
        /// </summary>
        /// <returns></returns>
        private string GroupByMonth()
        {
            return "monthbegin,monthend,";
        }
        #endregion

        #region 大屏报表
        /// 获取实时队列数据
        /// <summary>
        /// 获取实时队列数据
        /// </summary>
        /// <param name="type">1：企业；2：个人</param>
        /// <returns></returns>
        public DataTable GetRealTimeQueueData(int type, string bgid)
        {
            //修改转人工量指标 2015-3-4 强斐
            string sql = @"select  tmpb.* ,
                                            tmpa.*
                                    from    ( select    isnull(sum(call_num), 0) as call_num ,
                                                        isnull(sum(free_num), 0) as free_num ,
                                                        isnull(sum(busy_num), 0) as busy_num ,
                                                        isnull(sum(acw_num), 0) as acw_num ,
                                                        isnull(sum(online_num), 0) as online_num
                                              from      ( select    c.agentid ,
						                                        case c.state when 9 then 1 else 0 end as call_num ,
						                                        case c.state when 3 then 1 else 0 end as free_num ,
						                                        case c.state when 4 then 1 else 0 end as busy_num ,
						                                        case c.state when 5 then 1 else 0 end as acw_num ,
						                                        1 as online_num
                                                              from  cagent c
                                                              where c.agentid in (select userid from employeeagent a 
                                                                                                  where 1=1 
                                                                                                  and a.bgid in ('" + Dal.Util.SqlFilterByInCondition(bgid) + @"')
                                                                                                )
                                                            ) tmp
                                            ) tmpa ,
                                            ( select    n_entered_out
                                              from      dbo.report_routepoint_15minutes
                                              where     begintime =(select max(begintime) from report_routepoint_15minutes)
                                                        and objecttype = " + type + @"
                                            ) tmpb";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        /// 获取指标完成数据
        /// <summary>
        /// 获取指标完成数据
        /// </summary>
        /// <param name="type">1：企业；2：个人</param>
        /// <returns></returns>
        public DataTable GetIndicatorsComData(int type, string bgid, string date)
        {
            //修改转人工量指标 2015-3-4 强斐
            string sql = @"select  tmpa.* ,
                                            tmpb.*
                                    from    ( select    case sum(n_entered_out) when 0 then 0 else cast(round(sum(n_answered) * 100.0/ sum(n_entered_out), 2) as float)
                                                        end as pc_n_answered ,
                                                        case sum(n_entered_out) when 0 then 0 else cast(round(sum(n_distrib_in_tr) * 100.0/ sum(n_entered_out), 2) as float)
                                                        end as pc_n_distrib_in_tr
                                              from      dbo.report_routepoint_15minutes
                                              where     1 = 1
                                                        and begintime >= cast(getdate() as date)
                                                        and objecttype = " + type + @"
                                            ) tmpa ,
                                            ( select    case when sum(jjcpn) > 0 then 
						                                    cast(round(sum(jjn) * 100.0 / ( sum(jjcpn) + 0.0 ),2) as float)
                                                        else 0 end as jiejue_rate ,
                                                        case when sum(mydcpn) > 0 then 
						                                    cast(round(sum(myn) * 100.0 / ( sum(mydcpn) + 0.0 ), 2) as float)
                                                        else 0 end as manyi_rate
                                              from      ( select    y.callid ,
                                                                    case when i.score = 11 or i.score = 12 or i.score = 13
                                                                                   or i.score = 21 or i.score = 22 or i.score = 23
                                                                                   or i.score = 10 or i.score = 20
									                                               then 1 else 0 end as jjcpn ,
                                                                    case when i.score = 11 or i.score = 12 or i.score = 13 or i.score = 10
									                                               then 1 else 0 end as jjn ,
                                                                    case when i.score = 11 or i.score = 12 or i.score = 13
                                                                                   or i.score = 21 or i.score = 22 or i.score = 23
									                                               then 1 else 0 end as mydcpn ,
                                                                    case when i.score = 11 or i.score = 21
									                                               then 1 else 0 end as myn
                                                          from      dbo.callrecord_orig y
                                                                    inner join employeeagent e on y.createuserid = e.userid
                                                                    inner join dbo.ivrsatisfaction as i on y.callid = i.callrecordid
                                                          where     1 = 1
                                                                    and cast(y.createtime as date) = cast('" + StringHelper.SqlFilter(date) + @"' as date)
                                                                    and e.bgid in ( '" + Dal.Util.SqlFilterByInCondition(bgid) + @"' )
                                                        ) zz
                                            ) tmpb";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        /// 获取实时监控数据
        /// <summary>
        /// 获取实时监控数据
        /// </summary>
        /// <param name="bgid">组id</param>
        /// <returns></returns>
        public DataTable GetRealTimeMonitoring(string bgid)
        {
            string sql = @"select    isnull(sum(call_num), 0) as call_num ,
                                        isnull(sum(free_num), 0) as free_num ,
                                        isnull(sum(busy_num), 0) as busy_num ,
                                        isnull(sum(online_num), 0) as online_num
                                    from  ( select    c.agentid ,
		                                    case c.state when 9 then 1 else 0 end as call_num ,
		                                    case c.state when 3 then 1 else 0 end as free_num ,
		                                    case c.state when 4 then 1 else 0 end as busy_num ,
		                                    case c.state when 5 then 1 else 0 end as acw_num ,
		                                    1 as online_num
                                          from  cagent c
                                          where c.agentid in (select userid from employeeagent a 
                                          where 1=1 
                                          and a.bgid in ('" + Dal.Util.SqlFilterByInCondition(bgid) + @"')
                                         )
                                    ) tmp";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        /// 获取呼出业务接通情况实时监控
        /// <summary>
        /// 获取呼出业务接通情况实时监控
        /// </summary>
        /// <param name="SelectedDate">查询的日期</param>
        /// <param name="BGID">要查询的组</param>
        /// <returns></returns>
        public DataTable GetCallOutConnectedInfoData(string SelectedDate, string BGID)
        {
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter ("@SelectedDate",SelectedDate),
                new SqlParameter("@BGID",BGID), 
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "dbo.p_GetMonitorCallOut", paras);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        /// 获取达人排名数据
        /// <summary>
        /// 获取达人排名数据
        /// </summary>
        /// <param name="bgid"></param>
        /// <returns></returns>
        public DataTable GetFourScreenData(string bgid, string date)
        {
            string strSql = @"select top 10 
                case when isnull(d.truename,'')<>'' then d.truename else d.username end as truename,
                count(d.truename) as totalcount,MAX(a.CreateTime) AS LastCreateTime
                from cc2012.dbo.callrecord_orig as a inner join cc2012.dbo.employeeagent as b 
                on a.createuserid=b.userid
                inner join cc2012.dbo.businessgroup as c on b.bgid=c.bgid
                inner join CRM2009.dbo.v_userinfo as d on a.createuserid=d.userid
                where c.bgid='" + StringHelper.SqlFilter(bgid) + @"' and a.callstatus=2 
                and a.initiatedtime is not null
                and cast(a.createtime as date) = cast('" + StringHelper.SqlFilter(date) + @"' as date)
                group by c.name,d.userid,d.truename, d.username
                order by totalcount desc,LastCreateTime ASC";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
            if (ds != null && ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }

        /// 获取实时热线数据-合力
        /// <summary>
        /// 获取实时热线数据-合力
        /// </summary>
        /// <param name="hoteLine"></param>
        /// <returns></returns>
        public DataTable GetHotLineRealInfo(string hotLine)
        {
            //修改转人工量指标 2015-3-4 强斐
            string sql = @"GetHotLineInfo";

            var para = new SqlParameter[]
            {
                new SqlParameter("@hotline",hotLine), 
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, sql, para);

            if (ds != null && ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        /// 获取实时热线数据-合力北京测试热线
        /// <summary>
        /// 获取实时热线数据-合力北京测试热线
        /// </summary>
        /// <param name="maxExtensionNum"></param>
        /// <param name="minExtensionNum"></param>
        /// <returns></returns>
        public DataTable GetHotLineRealInfo_BJTest(int maxExtensionNum, int minExtensionNum)
        {
            string sql = @"GetHotLineInfo_Test";

            var para = new SqlParameter[]
            {
                new SqlParameter("@MaxExtensionNum",maxExtensionNum), 
                new SqlParameter("@MinExtensionNum",minExtensionNum), 
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, sql, para);

            if (ds != null && ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        /// 获取热线指标完成数据-合力
        /// <summary>
        /// 获取热线指标完成数据-合力
        /// </summary>
        /// <param name="hoteLine"></param>
        /// <returns></returns>
        public DataTable GetHotLineStateInfo(string hotLine)
        {
            //修改转人工量指标 2015-3-4 强斐
            string sql = @"GetState_Hotline";

            var para = new SqlParameter[]
            {
                new SqlParameter("@hotline",hotLine), 
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, sql, para);

            if (ds != null && ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        #endregion
    }
}
