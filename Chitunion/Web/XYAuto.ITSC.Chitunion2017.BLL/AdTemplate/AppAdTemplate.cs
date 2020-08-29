/********************************************************
*创建人：lixiong
*创建时间：2017/6/29 11:59:51
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Temp;
using XYAuto.ITSC.Chitunion2017.Entities.Query.AdTemplate;

namespace XYAuto.ITSC.Chitunion2017.BLL.AdTemplate
{
    public class AppAdTemplate
    {
        #region Instance

        public static readonly AppAdTemplate Instance = new AppAdTemplate();

        #endregion Instance

        #region Contructor

        protected AppAdTemplate()
        { }

        #endregion Contructor

        public RespAdTemplateItemDto GetInfoV1(int templateId, int mediaId = 0, int baseAdTempateId = 0,
            int userId = 0)
        {
            var info = Dal.AdTemplate.AppAdTemplate.Instance.GetInfoV1(templateId, mediaId, baseAdTempateId);

            if (info != null)
            {
                var respAdTemplateItem = AutoMapper.Mapper.Map<Entities.AdTemplate.AppAdTemplate, RespAdTemplateItemDto>(info);

                respAdTemplateItem.AdSaleAreaGroup =
                    respAdTemplateItem.AdSaleAreaGroup.FilterSaleGroup(respAdTemplateItem);

                return respAdTemplateItem;
            }

            return null;
        }

        public List<RespAdTemplateItemDto> GetAuditInfoListV1(
            AdTemplateQuery<Entities.AdTemplate.AppAdTemplate> query)
        {
            var auditList = Dal.AdTemplate.AppAdTemplate.Instance.GetAuditInfoListV1(query);
            var respAdTemplateList = AutoMapper.Mapper.Map<List<Entities.AdTemplate.AppAdTemplate>, List<RespAdTemplateItemDto>>(auditList);
            respAdTemplateList.ForEach(item =>
            {
                item.AdSaleAreaGroup =
                 item.AdSaleAreaGroup.FilterSaleGroup(item);
            });
            return respAdTemplateList;
        }

        public void OperateAuditMsgInsert(int relationId, int msgType, int optType, int createUserId)
        {
            Dal.Media.OperateAuditMsg.Instance.OperateAuditMsgInsert(relationId, msgType, optType, createUserId);
        }
    }
}