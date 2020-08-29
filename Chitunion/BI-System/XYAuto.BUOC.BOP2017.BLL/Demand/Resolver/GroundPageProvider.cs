/********************************************************
*创建人：lixiong
*创建时间：2017/9/29 10:51:37
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Request;
using XYAuto.BUOC.BOP2017.BLL.Demand.Dto.Response;
using XYAuto.BUOC.BOP2017.BLL.GDT;
using XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Request;
using XYAuto.BUOC.BOP2017.BLL.QueryPage.Entity;
using XYAuto.BUOC.BOP2017.Entities.Demand;
using XYAuto.BUOC.BOP2017.Entities.Enum.GDT;
using XYAuto.BUOC.BOP2017.Entities.Query.Demand;
using XYAuto.BUOC.BOP2017.Infrastruction;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;

namespace XYAuto.BUOC.BOP2017.BLL.Demand.Resolver
{
    /// <summary>
    /// auth:lixiong
    /// desc:落地页提供方法类
    /// </summary>
    public class GroundPageProvider : Infrastruction.Verification.VerifyOperateBase
    {
        private readonly RequestGroundPageDto _contextGroundPageDto;
        private readonly RequestDeletePageDto _contextDeletePageDto;
        private readonly ConfigEntity _configEntity;

        public GroundPageProvider(RequestGroundPageDto contextGroundPageDto,
            RequestDeletePageDto contextDeletePageDto,
            ConfigEntity configEntity)
        {
            _contextGroundPageDto = contextGroundPageDto;
            _contextDeletePageDto = contextDeletePageDto;
            _configEntity = configEntity;
        }

        #region 添加落地页相关

        /// <summary>
        /// 添加落地页
        /// </summary>
        /// <returns></returns>
        public ReturnValue ExcutePage()
        {
            var retValue = VerifyOfExcutePage();
            if (retValue.HasError)
                return retValue;
            var entity = GetDemandGroundPage();
            if (Dal.Demand.DemandGroundPage.Instance.Insert(entity) <= 0)
            {
                Loger.Log4Net.Error($"添加落地页失败,参数：{JsonConvert.SerializeObject(entity)}");
                return CreateFailMessage(retValue, "50003", "添加落地页失败");
            }
            return retValue;
        }

        public ReturnValue VerifyOfExcutePage()
        {
            var retValue = VerifyOfNecessaryParameters<RequestGroundPageDto>(_contextGroundPageDto);
            if (retValue.HasError)
                return retValue;
            VerifyOfExcuteByCarInfoAndCitys(retValue);
            if (retValue.HasError)
                return retValue;
            VerifyOfDemandAuditStatus(retValue);
            if (retValue.HasError)
                return retValue;

            return retValue;
        }

        public ReturnValue VerifyOfDemandAuditStatus(ReturnValue retValue)
        {
            Entities.GDT.GdtDemand entityDemand;
            retValue = new LogicTransferProvider().VerifyOfDemandBillNo(retValue, _contextGroundPageDto.DemandBillNo,
                out entityDemand);
            if (retValue.HasError || entityDemand == null)
                return retValue;
            if (entityDemand.AuditStatus != DemandAuditStatusEnum.Puting
                && entityDemand.AuditStatus != DemandAuditStatusEnum.PendingPutIn)
            {
                return CreateFailMessage(retValue, "50004", $"当前需求状态下不允许添加落地页操作，必须为：待投放/投放中状态");
            }
            return retValue;
        }

        public ReturnValue VerifyOfExcuteByCarInfoAndCitys(ReturnValue retValue)
        {
            //todo:（同一需求下）
            //1.品牌id+车型id+省份id+城市id 唯一
            //2.url唯一
            var query = new DemandGroundQuery<DemandGroundPage>()
            {
                DemandBillNo = _contextGroundPageDto.DemandBillNo,
                //BrandId = _contextGroundPageDto.BrandId,
                //SerielId = _contextGroundPageDto.SerielId,
                //CityId = _contextGroundPageDto.CityId,
                //ProvinceId = _contextGroundPageDto.ProvinceId
            };
            var list = Dal.Demand.DemandGroundPage.Instance.GetGroundPages(query);
            var brandSerielCitysList =
                list.Where(
                    s => s.BrandId == _contextGroundPageDto.BrandId && s.SerielId == _contextGroundPageDto.SerielId &&
                     s.ProvinceId == _contextGroundPageDto.ProvinceId && s.CityId == _contextGroundPageDto.CityId);
            if (brandSerielCitysList.Any())
            {
                return CreateFailMessage(retValue, "50001", "同一需求下,品牌id+车型id+省份id+城市id 必须唯一");
            }

            if (
                list.Any(s => !string.IsNullOrWhiteSpace(s.PromotionUrl)
                && s.PromotionUrl.Equals(_contextGroundPageDto.PromotionUrl, StringComparison.OrdinalIgnoreCase)))
            {
                return CreateFailMessage(retValue, "50002", "同一需求下,url 必须唯一");
            }
            return retValue;
        }

        private Entities.Demand.DemandGroundPage GetDemandGroundPage()
        {
            return new DemandGroundPage()
            {
                BrandId = _contextGroundPageDto.BrandId,
                SerielId = _contextGroundPageDto.SerielId,
                CityId = _contextGroundPageDto.CityId,
                ProvinceId = _contextGroundPageDto.ProvinceId,
                DemandBillNo = _contextGroundPageDto.DemandBillNo,
                PromotionUrl = _contextGroundPageDto.PromotionUrl,
                CreateUserId = _configEntity.CreateUserId
            };
        }

        #endregion 添加落地页相关

        #region 落地页删除相关

        public ReturnValue Delete()
        {
            var retValue = VerifyOfDelete();
            if (retValue.HasError)
                return retValue;
            Dal.Demand.DemandGroundPage.Instance.Delete(_contextDeletePageDto.GroundId,
                _contextDeletePageDto.DemandBillNo);
            return retValue;
        }

        public ReturnValue VerifyOfDelete()
        {
            var retValue = VerifyOfNecessaryParameters<RequestDeletePageDto>(_contextDeletePageDto);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyOfDeleteByDemand(retValue);
            if (retValue.HasError)
                return retValue;
            return retValue;
        }

        public ReturnValue VerifyOfDeleteByDemand(ReturnValue retValue)
        {
            Entities.GDT.GdtDemand entityDemand;
            retValue = new LogicTransferProvider().VerifyOfDemandBillNo(retValue, _contextDeletePageDto.DemandBillNo,
                out entityDemand);
            if (retValue.HasError || entityDemand == null)
                return retValue;
            if (entityDemand.AuditStatus == DemandAuditStatusEnum.IsOver || entityDemand.AuditStatus == DemandAuditStatusEnum.Terminated)
                return CreateFailMessage(retValue, "50005", "需求已结束/已终止，不能删除");
            return retValue;
        }

        #endregion 落地页删除相关

        #region 查询相关

        /// <summary>
        /// 获取落地页列表
        /// </summary>
        /// <param name="demandBillNo"></param>
        /// <returns></returns>
        public List<RespDemandGroundPageDto> GetGroundPages(int demandBillNo)
        {
            var list = Dal.Demand.DemandGroundPage.Instance.GetList(new DemandGroundQuery<DemandGroundPage>()
            {
                DemandBillNo = demandBillNo
            });
            var respList = new List<RespDemandGroundPageDto>();
            list.ForEach(s =>
            {
                var resp = new RespDemandGroundPageDto
                {
                    DemandBillNo = s.DemandBillNo,
                    AreaInfo = DemandGroundDeliveryProvider.GetCitysInfoDtos(s.CityInfo)
                        .FilterCitys(s.ProvinceId, s.CityId),
                    CarInfo = DemandGroundDeliveryProvider.GetCarInfoDtos(s.CarInfo)
                        .FilterCarInfo(s.BrandId, s.SerielId),
                    PromotionUrl = s.PromotionUrl,
                    DeliveryCount = s.DeliveryCount,
                    GroundId = s.GroundId
                };
                respList.Add(resp);
            });
            return respList;
        }

        #endregion 查询相关
    }
}