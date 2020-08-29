using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.WebAPI.Models
{
    public class CallRecordQueryModel
    {
        /// 查询方式
        /// <summary>
        /// 查询方式
        /// </summary>
        public int QueryType { get; set; }
        /// 主叫号码
        /// <summary>
        /// 主叫号码
        /// </summary>
        public string Oriani { get; set; }
        /// 被叫号码
        /// <summary>
        /// 被叫号码
        /// </summary>
        public string OriDnis { get; set; }
        /// 话务ID
        /// <summary>
        /// 话务ID
        /// </summary>
        public string CallID { get; set; }
        /// 返回条目
        /// <summary>
        /// 返回条目
        /// </summary>
        public int Top { get; set; }

        public CallRecordQueryModel()
        {
            Top = 10000;
        }

        /// 校验参数
        /// <summary>
        /// 校验参数
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Validate(out string error)
        {
            error = "";
            if (QueryType == 1)
            {
                //1.	Oriani = '号码' or OriDnis = '号码'
                if (string.IsNullOrEmpty(Oriani) && string.IsNullOrEmpty(OriDnis))
                {
                    error = "主叫[Oriani]和被叫[OriDnis]不能同时为空";
                    return false;
                }
            }
            else if (QueryType == 2)
            {
                //2.	CallStatus = '呼叫方向（1） and Oriani = 主叫号码
                if (string.IsNullOrEmpty(Oriani))
                {
                    error = "主叫[Oriani]不能为空";
                    return false;
                }
            }
            else if (QueryType == 3)
            {
                //3.	CallStatus = '呼叫方向（2） and OriDnis = 被叫号码
                if (string.IsNullOrEmpty(OriDnis))
                {
                    error = "被叫[OriDnis]不能为空";
                    return false;
                }
            }
            else if (QueryType == 4)
            {
                //4.	CallID = '录音ID'
                if (string.IsNullOrEmpty(CallID))
                {
                    error = "话务ID[CallID]不能为空";
                    return false;
                }
            }
            else
            {
                error = "参数个数或名称错误";
                return false;
            }

            if (!string.IsNullOrEmpty(this.Oriani) && BLL.Util.IsTelephone(this.Oriani) == false)
            {
                error = "主叫[Oriani]不是一个有效的手机或座机号码";
                return false;
            }
            if (!string.IsNullOrEmpty(this.OriDnis) && BLL.Util.IsTelephone(this.OriDnis) == false)
            {
                error = "被叫[OriDnis]不是一个有效的手机或座机号码";
                return false;
            }
            return true;
        }
        /// 获取查询类
        /// <summary>
        /// 获取查询类
        /// </summary>
        /// <returns></returns>
        public CallRecordQuery GetCallRecordQuery()
        {
            CallRecordQuery query = new CallRecordQuery();
            query.pageSize = Top;

            switch (QueryType)
            {
                case 1:
                    //主叫or被叫
                    if (!string.IsNullOrEmpty(this.Oriani))
                    {
                        query.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.Oriani, this.Oriani, ConditionHB.Equal, RelationshipHB.OR));
                    }
                    if (!string.IsNullOrEmpty(this.OriDnis))
                    {
                        query.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.OriDnis, this.OriDnis, ConditionHB.Equal, RelationshipHB.OR));
                    }
                    break;
                case 2:
                    //呼入且主叫
                    query.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.Oriani, this.Oriani, ConditionHB.Equal, RelationshipHB.AND));
                    query.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.CallStatus, "1", ConditionHB.Equal, RelationshipHB.AND));
                    break;
                case 3:
                    //呼出且被叫
                    query.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.OriDnis, this.OriDnis, ConditionHB.Equal, RelationshipHB.AND));
                    query.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.CallStatus, "2", ConditionHB.Equal, RelationshipHB.AND));
                    break;
                case 4:
                    //话务ID
                    query.QueryConditionList.Add(new QueryCondition(ConFiledHBCallRecord.CallID, this.CallID, ConditionHB.Equal, RelationshipHB.AND));
                    break;
            }

            return query;
        }
    }
}