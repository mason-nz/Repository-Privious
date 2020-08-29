using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// <summary>
    /// 话务结果类
    /// </summary>
    [Serializable]
    public class CallRecordResult
    {
        /// 原始的json数据
        /// <summary>
        /// 原始的json数据
        /// </summary>
        public string JsonString { get; set; }

        #region 属性
        /// 满足条件的数量
        /// <summary>
        /// 满足条件的数量
        /// </summary>
        public int recordCount { get; set; }
        /// 本次查询的数量
        /// <summary>
        /// 本次查询的数量
        /// </summary>
        public int actionSize { get; set; }
        /// 查询字段列表
        /// <summary>
        /// 查询字段列表
        /// </summary>
        public string field { get; set; }
        /// 分页结束key值
        /// <summary>
        /// 分页结束key值
        /// </summary>
        public string endKey { get; set; }
        /// 数据区数组，每一行内容，逗号分隔，和字段列顺序一致
        /// <summary>
        /// 数据区数组，每一行内容，逗号分隔，和字段列顺序一致
        /// </summary>
        public List<string> trafficDetail { get; set; }
        #endregion

        /// 字段列表
        /// <summary>
        /// 字段列表
        /// </summary>
        public List<string> FieldList
        {
            get
            {
                return new List<string>(field.Split(','));
            }
        }

        #region 转换成其他类型的数据集
        /// 数据集合
        /// <summary>
        /// 数据集合
        /// </summary>
        public List<Dictionary<string, string>> TrafficDetailCollection
        {
            get
            {
                List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                List<string> cols = FieldList;
                //循环行
                foreach (string content in trafficDetail)
                {
                    List<string> values = new List<string>(content.Split(','));
                    if (cols.Count != values.Count)
                    {
                        continue;
                    }
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    //循环列
                    for (int i = 0; i < cols.Count; i++)
                    {
                        dic[cols[i]] = values[i];
                    }
                    list.Add(dic);
                }
                return list;
            }
        }
        /// 数据类集合
        /// <summary>
        /// 数据类集合
        /// </summary>
        public List<CallRecordData> CallRecordDataList
        {
            get
            {
                List<CallRecordData> list = new List<CallRecordData>();
                List<string> cols = FieldList;
                //循环行
                foreach (string content in trafficDetail)
                {
                    List<string> values = new List<string>(content.Split(','));
                    if (cols.Count != values.Count)
                    {
                        continue;
                    }
                    CallRecordData info = new CallRecordData();
                    //循环列
                    for (int i = 0; i < cols.Count; i++)
                    {
                        PropertyInfo pinfo = info.GetType().GetProperty(cols[i]);
                        if (pinfo != null)
                        {
                            object obj = GetPropertyInfoValue(pinfo, values[i]);
                            pinfo.SetValue(info, obj, null);
                        }
                    }
                    list.Add(info);
                }
                return list;
            }
        }
        /// 数据集
        /// <summary>
        /// 数据集
        /// </summary>
        public DataTable Data
        {
            get
            {
                List<string> cols = FieldList;
                //构建表
                DataTable dt = CreateTable(cols);
                //循环行
                foreach (string content in trafficDetail)
                {
                    List<string> values = new List<string>(content.Split(','));
                    if (cols.Count != values.Count)
                    {
                        continue;
                    }
                    //循环列
                    object[] objs = new object[dt.Columns.Count];
                    for (int i = 0; i < cols.Count; i++)
                    {
                        PropertyInfo pinfo = typeof(CallRecordData).GetProperty(cols[i]);
                        objs[i] = GetPropertyInfoValue(pinfo, values[i]);
                    }
                    dt.Rows.Add(objs);
                }
                return dt;
            }
        }
        #endregion

        #region 辅助
        /// 创建表
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="cols"></param>
        /// <returns></returns>
        private DataTable CreateTable(List<string> cols)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < cols.Count; i++)
            {
                PropertyInfo pinfo = typeof(CallRecordData).GetProperty(cols[i]);
                if (pinfo != null)
                {
                    if (pinfo.PropertyType == typeof(string))
                    {
                        dt.Columns.Add(cols[i], typeof(string));
                    }
                    else if (pinfo.PropertyType == typeof(long))
                    {
                        dt.Columns.Add(cols[i], typeof(long));
                    }
                    else if (pinfo.PropertyType == typeof(int) || pinfo.PropertyType == typeof(int?))
                    {
                        dt.Columns.Add(cols[i], typeof(int));
                    }
                    else if (pinfo.PropertyType == typeof(DateTime) || pinfo.PropertyType == typeof(DateTime?))
                    {
                        dt.Columns.Add(cols[i], typeof(DateTime));
                    }
                }
                else
                {
                    dt.Columns.Add(cols[i], typeof(string));
                }
            }
            return dt;
        }
        /// 获取转换后的值
        /// <summary>
        /// 获取转换后的值
        /// </summary>
        /// <param name="pinfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private object GetPropertyInfoValue(PropertyInfo pinfo, string value)
        {
            if (pinfo != null)
            {
                if (pinfo.PropertyType == typeof(string))
                {
                    return value;
                }
                else if (pinfo.PropertyType == typeof(long))
                {
                    return CommonFunction.ObjectToLong(value, -1);
                }
                else if (pinfo.PropertyType == typeof(int))
                {
                    return CommonFunction.ObjectToInteger(value);
                }
                else if (pinfo.PropertyType == typeof(int?))
                {
                    return CommonFunction.ObjectToIntegerOrNull(value);
                }
                else if (pinfo.PropertyType == typeof(DateTime?))
                {
                    return CommonFunction.ObjectToDateTimeOrNull(value);
                }
                else if (pinfo.PropertyType == typeof(DateTime))
                {
                    return CommonFunction.ObjectToDateTime(value);
                }
            }
            return null;
        }

        public override string ToString()
        {
            return "查询总数：" + recordCount + "；返回总数：" + actionSize + "；分页结束key：" + endKey + "。";
        }
        #endregion
    }

    /// 数据列表
    /// <summary>
    /// 数据列表
    /// </summary>
    public class CallRecordData
    {
        public string SessionID { get; set; }	//	录音流水号或厂商联络ID
        public long CallID { get; set; }	//	话务ID
        public string ExtensionNum { get; set; }	//	分机号码
        public string Oriani { get; set; }	//	主叫号码
        public string OriDnis { get; set; }	//	被叫号码 
        public int CallStatus { get; set; }	//	呼叫方向 枚举：CallStatus
        public string SwitchINNum { get; set; }	//	接入号码（落地号）
        public int OutBoundType { get; set; }	//	呼出类型   1：页面 2：客户端 4：自动外呼
        public string SkillGroup { get; set; }	//	技能组ID 

        public DateTime? InitiatedTime { get; set; }	//	外呼时-初始化时间；呼入时-进入热线时间
        public DateTime? RingingTime { get; set; }	//	振铃开始时间
        public DateTime? EstablishedTime { get; set; }	//	接通时间
        public DateTime? AgentReleaseTime { get; set; }	//	坐席挂断时间
        public DateTime? CustomerReleaseTime { get; set; }	//	客户挂断时间
        public DateTime? AfterWorkBeginTime { get; set; }	//	事后处理开始时间       
        public DateTime? ConsultTime { get; set; }	//	转接开始时间
        public DateTime? ReconnectCall { get; set; }	//	转接恢复时间
        public int TallTime { get; set; }	//	录音总时长（单位：秒）
        public int AfterWorkTime { get; set; }	//	事后处理时长（单位：秒）
        public string AudioURL { get; set; }	//	录音URL地址
        public DateTime CreateTime { get; set; }	//	创建记录时间
        public int CreateUserID { get; set; }	//	创建者ID
        public int? BGID { get; set; }	//	业务分组ID
        public int? SCID { get; set; }	//	业务分类ID
        public string BusinessID { get; set; }	//	业务ID
        public int BusinessSource { get; set; }	//	业务来源ID 枚举：ProjectSource 0 代表工单
        public int IVRScore { get; set; }	//	IVR满意度结果
        public string AgentNum { get; set; }	//	坐席当前工号
        public string CustName { get; set; }	//	客户姓名
        public int TotalTelTime { get; set; }	//	话务总时长=TallTime+ AfterWorkTime
        public string HotlineName { get; set; }	//	当前话务热线名称
        public string BusinessDetailURL { get; set; }	//	当前话务对应业务URL模板路径
        public string CustID { get; set; }	//	CRM系统客户ID，或CC客户池下客户ID
    }
}
