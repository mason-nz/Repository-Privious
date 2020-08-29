using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Channel;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8
{
    public class ModifyChannelReqDTO : ChannelInfo
    {
        public List<PolicyInfo> PolicyList { get; set; }

        public bool CheckSelfModel(out string msg)
        {
            StringBuilder sb = new StringBuilder();

            #region 渠道验证
            if (string.IsNullOrWhiteSpace(this.ChannelName))
            {
                sb.AppendLine("渠道名称不能为空!");
            }
            else if(this.ChannelName.Length > 20)
            {
                sb.AppendLine("渠道名称不能超过20字!");
            }

            if (this.CooperateBeginDate.Date > this.CooperateEndDate.Date)
            {
                sb.AppendLine("开始日期必须小于等于结束日期!");
            }
            if (DateTime.Now > this.CooperateEndDate.Date)
            {
                sb.AppendLine("结束日期必须大于等于今天!");
            }
            if (!string.IsNullOrWhiteSpace(this.Remark) && this.Remark.Length > 200)
            {
                sb.AppendLine("备注不能超过200字!");
            }
            #endregion

            if (this.PolicyList == null || this.PolicyList.Count.Equals(0))
            {
                sb.AppendLine("请至少添加一个政策!");
            }
            else
            {
                #region  政策验证

                List<string> existsConditionList = new List<string>();
                SingleAccountSumTypeEnum sastEnum;
                foreach (var policy in this.PolicyList)
                {
                    #region 每一项
                    if (policy.Quota < 0)
                    {
                        sb.AppendLine("满额只能输入大于等于0的整数!");
                        break;
                    }

                    if (policy.SingleAccountSum < 0)
                    {
                        sb.AppendLine("单个账号金额只能输入大于0的整数!");
                        break;
                    }
                    else if (policy.SingleAccountSum.Equals(0))
                    {
                        policy.SingleAccountSumType = Constants.Constant.INT_INVALID_VALUE;
                    }
                    else if (!System.Enum.TryParse(policy.SingleAccountSumType.ToString(), out sastEnum))
                    {
                        sb.AppendLine("单个账号金额比较类型错误!");
                        break;
                    }

                    if (policy.PurchaseDiscount < 0)
                    {
                        sb.AppendLine("采购折扣只能输入大于等于0的数字!");
                        break;
                    }
                    if (policy.RebateType1.Equals((int)RebateType1Enum.无))
                    {
                        policy.RebateDateType = Constants.Constant.INT_INVALID_VALUE;
                        policy.RebateValue = 0;
                    }
                    else if (policy.RebateType1.Equals((int)RebateType1Enum.返现) || policy.RebateType1.Equals((int)RebateType1Enum.返货))
                    {
                        #region 返现、返货
                        if (policy.RebateType1.Equals((int)RebateType1Enum.返现))
                        {
                            if (policy.RebateType2.Equals((int)RebateType2Enum.比例))
                            {
                                if (policy.RebateValue < 0)
                                {
                                    sb.AppendLine("返现比例错误!");
                                    break;
                                }
                            }
                            else if (policy.RebateType2.Equals((int)RebateType2Enum.金额))
                            {
                                if (policy.RebateValue < 0 || (policy.RebateValue - (int)policy.RebateValue) > 0)
                                {
                                    sb.AppendLine("返现金额错误!");
                                    break;
                                }
                            }
                            else
                            {
                                sb.AppendLine("返点类型2错误!");
                                break;
                            }
                        }
                        else if (policy.RebateType1.Equals((int)RebateType1Enum.返货))
                        {
                            policy.RebateType2 = (int)RebateType2Enum.比例;
                            if (policy.RebateValue < 0)
                            {
                                sb.AppendLine("返货比例错误!");
                                break;
                            }
                        }
                        #endregion

                        if (!policy.RebateDateType.Equals((int)RebateDateTypeEnum.立返) && !policy.RebateDateType.Equals((int)RebateDateTypeEnum.到期返))
                        {
                            sb.AppendLine("返点时间错误!");
                            break;
                        }
                    }
                    else
                    {
                        sb.AppendLine("返点类型1错误!");
                        break;
                    }
                    #endregion
                }

                #region 组合重复验证
                var res = (from p in this.PolicyList
                           group p by new { p.Quota, p.SingleAccountSum, p.SingleAccountSumType } into g
                           where g.Count() > 1
                           select 1).Count();
                if (res > 0)
                {
                    sb.AppendLine("满额与单个账号金额的组合不能重复!");
                }
                else
                {
                    #region 总额相同情况下 小于等于的数字不能大于大于的数字
                    bool flag = false;
                    foreach (IGrouping<int, PolicyInfo> group in this.PolicyList.GroupBy(p => p.Quota))
                    {
                        if (flag)
                            break;
                        foreach (PolicyInfo p in group)
                        {
                            if (p.SingleAccountSumType.Equals((int)SingleAccountSumTypeEnum.大于))
                            {
                                if (group.Count(g => g.SingleAccountSumType.Equals((int)SingleAccountSumTypeEnum.小于等于) && g.SingleAccountSum > p.SingleAccountSum ) > 0)
                                {
                                    sb.AppendLine("满额与单个账号金额的组合不能重复!");
                                    flag = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (group.Count(g => g.SingleAccountSumType.Equals((int)SingleAccountSumTypeEnum.大于) && g.SingleAccountSum < p.SingleAccountSum) > 0)
                                {
                                    sb.AppendLine("满额与单个账号金额的组合不能重复!");
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                }
                #endregion
                #endregion
            }
            msg = sb.ToString();
            return msg.Length.Equals(0);
        }
    }
}
