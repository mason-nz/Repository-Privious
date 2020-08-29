using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    public class DealerInfoService
    {
        #region Instance
        public static readonly DealerInfoService Instance = new DealerInfoService();
        #endregion

        #region Contructor
        protected DealerInfoService()
        { }
        #endregion

        /// <summary>
        /// 插入到审核对照表，类型为：会员信息变化,若会员有变化，则生成记录
        /// </summary>
        /// <param name="recid">表ProjectTask_AuditContrastInfo主键</param>
        /// <param name="cC_DMSMember">呼叫中心会员实体</param>
        /// <param name="DMSMember">CRM系统会员实体</param>
        /// <returns></returns>
        public void UpdateMemberInfo(int recid, Entities.ProjectTask_DMSMember cc_DMSMember, BitAuto.YanFa.Crm2009.Entities.DMSMember DMSMember)
        {
            if (cc_DMSMember != null && DMSMember != null && !string.IsNullOrEmpty(DMSMember.MemberCode))
            {
                //判断会员除去4个字段，是否有过更改
                if (cc_DMSMember.Phone != DMSMember.Phone ||
                    cc_DMSMember.Fax != DMSMember.Fax ||
                    cc_DMSMember.CompanyWebSite != DMSMember.CompanyWebSite ||
                    cc_DMSMember.Email != DMSMember.Email ||
                    cc_DMSMember.Postcode != DMSMember.Postcode ||
                    cc_DMSMember.ContactAddress != DMSMember.ContactAddress
                    //cc_DMSMember.TrafficInfo != DMSMember.TrafficInfo ||
                    //cc_DMSMember.EnterpriseBrief != DMSMember.EnterpriseBrief ||
                    //cc_DMSMember.Remarks != DMSMember.Remarks
                    )
                {
                    int result = WebService.DealerInfoServiceHelper.Instance.UpdateDealerInfo(int.Parse(DMSMember.MemberCode),
                        cc_DMSMember.ContactAddress, cc_DMSMember.Postcode,
                        cc_DMSMember.Phone, cc_DMSMember.Fax,
                        cc_DMSMember.CompanyWebSite, cc_DMSMember.Email);
                    string resultStr = string.Empty;
                    int disposeStatus = 1;//已处理
                    switch (result)
                    {
                        case 0:
                            resultStr = "更新失败";
                            disposeStatus = 2;//未修改
                            break;
                        case 1:
                            resultStr = "更新成功";
                            break;
                        case -1:
                            resultStr = "该经销商在DMS中不存在";
                            disposeStatus = 2;//未修改
                            break;
                        case -2:
                            resultStr = "该经销商不是免费或者导入的";
                            disposeStatus = 2;//未修改
                            break;
                        case -3:
                            resultStr = "该免费经销商已经有排期";
                            disposeStatus = 2;//未修改
                            break;
                        case -4:
                            resultStr = "销售电话格式不正确";
                            disposeStatus = 2;//未修改
                            break;
                        case -5:
                            resultStr = "传真格式不正确";
                            disposeStatus = 2;//未修改
                            break;
                        default:
                            resultStr = "其他异常";
                            disposeStatus = 2;//未修改
                            break;
                    }
                    Entities.ProjectTask_AuditContrastInfo model = BLL.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(recid);
                    if (model != null)
                    {
                        model.Remark = resultStr;
                        model.DisposeTime = DateTime.Now;
                        model.DisposeStatus = disposeStatus;
                        BLL.ProjectTask_AuditContrastInfo.Instance.Update(model);
                    }
                }
            }
        }
    }
}