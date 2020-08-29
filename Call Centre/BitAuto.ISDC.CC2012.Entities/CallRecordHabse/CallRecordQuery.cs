using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class CallRecordQuery
    {
        /// 查询方式
        /// <summary>
        /// 查询方式
        /// </summary>
        public QueryTypeHB queryType { get; set; }
        /// 查询sql 
        /// <summary>
        /// 查询sql 
        /// </summary>
        public string SelectSql { get; set; }
        /// 条件sql 
        /// <summary>
        /// 条件sql 
        /// </summary>
        public string WhereSql { get; set; }
        /// 查询字段 多选
        /// <summary>
        /// 查询字段 多选
        /// </summary>
        public List<SelFiledHBCallRecord> selFieldList { get; set; }
        /// 描述分页所开始查找的第一条数据的rowkey值
        /// <summary>
        /// 描述分页所开始查找的第一条数据的rowkey值
        /// </summary>
        public string startKey { get; set; }
        /// 分页大小
        /// <summary>
        /// 分页大小
        /// </summary>
        public int pageSize { get; set; }
        /// 查询条件合集
        /// <summary>
        /// 查询条件合集
        /// </summary>
        public List<QueryCondition> QueryConditionList { get; set; }

        public CallRecordQuery()
        {
            queryType = QueryTypeHB.std;
            selFieldList = new List<SelFiledHBCallRecord>() { 
                SelFiledHBCallRecord.Otherdetail 
            };
            pageSize = 10000;
            QueryConditionList = new List<QueryCondition>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            //规范：sb.Append("\"key\":\"value\",");
            sb.Append("\"queryType\":\"" + queryType.ToString() + "\",");
            if (queryType == QueryTypeHB.sql)
            {
                sb.Append("\"sqlScript\":\"select " + SelectSql + " from tab_cc_traffic where " + WhereSql + "\",");
            }
            else if (queryType == QueryTypeHB.std)
            {
                sb.Append("\"startKey\":\"" + startKey + "\",");
                sb.Append("\"pageSize\":\"" + pageSize + "\",");
                sb.Append("\"selField\":\"" + CommonFunction.ListToString(selFieldList, ",") + "\",");
                sb.Append("\"trafficDatas\":[" + CommonFunction.ListToString(QueryConditionList, ",") + "],");
            }
            //移除多于的逗号
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
    }
    /// 查询条件
    /// <summary>
    /// 查询条件
    /// </summary>
    public class QueryCondition
    {
        /// 字段名称
        /// <summary>
        /// 字段名称
        /// </summary>
        public ConFiledHBCallRecord field { get; set; }
        /// 值 多个值用逗号,分隔 字符串用单引号'包裹
        /// <summary>
        /// 值 多个值用逗号,分隔 字符串用单引号'包裹
        /// </summary>
        public string value { get; set; }
        /// 查询条件
        /// <summary>
        /// 查询条件
        /// </summary>
        public ConditionHB condition { get; set; }
        /// 条件关系
        /// <summary>
        /// 条件关系
        /// </summary>
        public RelationshipHB relationship { get; set; }

        public QueryCondition()
        {
            field = ConFiledHBCallRecord.CallID;
            condition = ConditionHB.Equal;
            relationship = RelationshipHB.AND;
        }
        public QueryCondition(ConFiledHBCallRecord field, string value, ConditionHB condition, RelationshipHB relationship)
        {
            this.field = field;
            this.value = value;
            this.condition = condition;
            this.relationship = relationship;
        }

        public override string ToString()
        {
            string valuestr = value;
            if (condition.Expression.Contains("range"))
            {
                valuestr = value.Replace(",", ":");
            }
            return "{\"condition\":\"" + condition.Expression + "\","
                + "\"field\":\"" + field.ToString() + "\","
                + "\"relationship\":\"" + relationship.ToString() + "\","
                + "\"value\":\"" + valuestr + "\"}";
        }
    }

    #region 枚举
    /// 查询字段
    /// <summary>
    /// 查询字段
    /// </summary>
    public enum SelFiledHBCallRecord
    {
        Otherdetail,
        CallStatus,
        PhoneNumber,
        CallID,
        CreateTime
    }
    /// Std查询模式下可以查询的字段枚举
    /// <summary>
    /// Std查询模式下可以查询的字段枚举
    /// </summary>
    public enum ConFiledHBCallRecord
    {
        /// <summary>
        /// 主叫
        /// </summary>
        Oriani,
        /// <summary>
        /// 被叫
        /// </summary>
        OriDnis,
        /// <summary>
        /// 方向 1=呼入 2=呼出
        /// </summary>
        CallStatus,
        /// <summary>
        /// 话务ID
        /// </summary>
        CallID
    }
    #endregion
}
