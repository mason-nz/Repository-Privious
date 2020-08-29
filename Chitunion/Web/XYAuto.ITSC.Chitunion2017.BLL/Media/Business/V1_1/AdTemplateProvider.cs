/********************************************************
*创建人：lixiong
*创建时间：2017/6/6 11:09:58
*说明：app模板的提供类。套用模板点击过来的属于添加、样式和城市组只能多不能减少
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto.V1_1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Dal.Media;
using XYAuto.ITSC.Chitunion2017.Entities.AdTemplate;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.Temp;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.AdTemplate;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1
{
    public class AdTemplateProvider : CurrentOperateBase, IMediaOperate
    {
        private const string IsBaseTemplateKey = "isBaseTemplate";
        private readonly ConfigEntity _configEntity;
        private readonly RequestTemplateDto _requestTemplateDto;
        private RequestTemplateInfoDto _requestTemplateInfoDto;

        private bool IsBaseTemplate { get; set; }
        public Entities.AdTemplate.AppAdTemplate UpdateBeforeTemplateInfo { get; set; }

        public AdTemplateProvider(ConfigEntity configEntity, RequestTemplateDto requestTemplateDto)
        {
            _configEntity = configEntity;
            _requestTemplateDto = requestTemplateDto;
        }

        public AdTemplateProvider(ConfigEntity configEntity, RequestTemplateInfoDto requestTemplateInfoDto)
        {
            _configEntity = configEntity;
            _requestTemplateInfoDto = requestTemplateInfoDto;
        }

        public ReturnValue Excute()
        {
            SetDataByRole();
            if (_configEntity.CureOperateType == OperateType.Insert)
            {
                return DoInsert();
            }
            else if (_configEntity.CureOperateType == OperateType.Edit)
            {
                return DoUpdate();
            }
            else
            {
                return CreateFailMessage(new ReturnValue(), "50031", "请确定是新增还是编辑操作");
            }
        }

        #region 模板操作

        /// <summary>
        /// 套用模板点击过来的属于编辑、样式和城市组只能多不能减少
        /// 从公共模板过来编辑-名称不让修改
        /// </summary>
        /// <returns></returns>
        private ReturnValue DoInsert()
        {
            var retValue = new ReturnValue();

            retValue = VerifyCreateBusiness(retValue);
            if (retValue.HasError)
                return retValue;

            var execId = Dal.AdTemplate.AppAdTemplate.Instance.Insert(AutoMapper.Mapper.Map<RequestTemplateDto,
                Entities.AdTemplate.AppAdTemplate>(_requestTemplateDto));
            if (execId <= 0)
            {
                return CreateFailMessage(retValue, "70026", "添加模板失败");
            }

            //异步处理
            TaskToRunAdTemplate(retValue, execId, _requestTemplateDto.BaseMediaId);

            retValue.ReturnObject = execId;
            return retValue;
        }

        private ReturnValue DoUpdate()
        {
            var retValue = new ReturnValue();

            retValue = VerifyUpdateBusiness(retValue);
            if (retValue.HasError)
                return retValue;

            var execId = Dal.AdTemplate.AppAdTemplate.Instance.Update(AutoMapper.Mapper.Map<RequestTemplateDto,
                Entities.AdTemplate.AppAdTemplate>(_requestTemplateDto));
            if (execId <= 0)
            {
                return CreateFailMessage(retValue, "70029", "编辑模板失败");
            }

            //异步处理
            TaskToRunAdTemplate(retValue, _requestTemplateDto.TemplateId, _requestTemplateDto.BaseMediaId);

            retValue.ReturnObject = _requestTemplateDto.TemplateId;

            return retValue;
        }

        private void SetDataByRole()
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate
                || _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
            {
                if (!_requestTemplateDto.IsModified)//修正的情况下是待审核
                    _requestTemplateDto.AuditStatus = (int)AppTemplateEnum.已通过;
            }
        }

        #endregion 模板操作

        #region 异步处理广告样式、城市组

        public void TaskToRunAdTemplate(ReturnValue retValue, int adTemplateId, int mediaId)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate
                || _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
            {
                TaskToRunAdTemplateRelationByAdmin(retValue, adTemplateId, mediaId);
            }
            else
            {
                TaskToRunAdTemplateRelation(retValue, adTemplateId, mediaId);
            }
        }

        public void TaskToRunAdTemplateRelationByAdmin(ReturnValue retValue, int adTemplateId, int mediaId)
        {
            //广告样式
            try
            {
                AdTemplateStyle(retValue, adTemplateId, mediaId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat(
                    "adTemplate TaskToRunAdTemplateRelation is error:{0},参数：adTemplateId={1}&mediaId={2}|{3}",
                    exception.Message + (exception.StackTrace ?? string.Empty), adTemplateId, mediaId,
                    JsonConvert.SerializeObject(_requestTemplateDto.AdTempStyle));
            }

            //城市组
            try
            {
                AdTemplateGroupCitys(retValue, adTemplateId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat(
                    "adTemplate AdTemplateGroupCitys is error:{0},参数：adTemplateId={1}&mediaId={2}",
                    exception.Message + (exception.StackTrace ?? string.Empty), adTemplateId, mediaId);
            }
            //审核信息
            try
            {
                AuditMessageOperate(retValue, adTemplateId, AppTemplateEnum.已通过);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("adTemplate AuditMessageOperate is error:{0},参数：adTemplateId={1}&mediaId={2}",
                    exception.Message + (exception.StackTrace ?? string.Empty), adTemplateId, mediaId);
            }

            //修正即是审核通过，修改此模板的信息追加到公共模板上，现在的逻辑是修改模板Id为公共模板id，把IsPublic 修改为 1
            //AdTemplateGroupCityOnModified(retValue, adTemplateId);

            retValue.HasError = false;
            retValue.Message = string.Empty;
        }

        public void TaskToRunAdTemplateRelation(ReturnValue retValue, int adTemplateId, int mediaId)
        {
            //广告样式
            try
            {
                AdTemplateStyle(retValue, adTemplateId, mediaId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat(
                    "adTemplate TaskToRunAdTemplateRelation is error:{0},参数：adTemplateId={1}&mediaId={2}|{3}",
                    exception.Message + (exception.StackTrace ?? string.Empty), adTemplateId, mediaId,
                    JsonConvert.SerializeObject(_requestTemplateDto.AdTempStyle));
            }

            //城市组
            try
            {
                AdTemplateGroupCitys(retValue, adTemplateId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat(
                    "adTemplate AdTemplateGroupCitys is error:{0},参数：adTemplateId={1}&mediaId={2}",
                    exception.Message + (exception.StackTrace ?? string.Empty), adTemplateId, mediaId);
            }
            //审核信息
            try
            {
                AuditMessageOperate(retValue, adTemplateId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("adTemplate AuditMessageOperate is error:{0},参数：adTemplateId={1}&mediaId={2}",
                    exception.Message + (exception.StackTrace ?? string.Empty), adTemplateId, mediaId);
            }
            try
            {
                UploadFileManage(mediaId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("App_AdTemplate UploadFileManage is exception:{0}{1}",
                    System.Environment.NewLine, exception + (exception.StackTrace ?? string.Empty));
            }

            retValue.HasError = false;
            retValue.Message = string.Empty;
        }

        private void UploadFileManage(int templateId)
        {
            var urlList = _requestTemplateDto.AdLegendUrl.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var retValue = BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(urlList, _configEntity.CreateUserId,
                   UploadFileEnum.AppAdTemplate, templateId, "App_AdTemplate");
            if (retValue.HasError)
            {
                Loger.Log4Net.ErrorFormat("App_AdTemplate UploadFileManage is error:{0}{1}",
               System.Environment.NewLine, retValue.Message);
            }
        }

        /// <summary>
        /// 模板广告样式操作（套用了模板的模板不允许减少选择项）
        /// 模板样式、城市组都是增量
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="adTemplateId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        private ReturnValue AdTemplateStyle(ReturnValue retValue, int adTemplateId, int mediaId)
        {
            //先删除自己的模板样式 IsPublic = 0，再插入
            Loger.Log4Net.InfoFormat("app AdTemplateStyle 没有套用模板，先删除，再插入");
            var exdelete = Dal.AdTemplate.AppAdTemplateStyle.Instance.Delete(adTemplateId);
            if (exdelete <= 0)
            {
                Loger.Log4Net.InfoFormat("app AdTemplateStyle 没有套用模板，先删除--失败:{0}", adTemplateId);
            }
            if (_requestTemplateDto.AdTempStyle == null || _requestTemplateDto.AdTempStyle.Count == 0)
            {
                return retValue;
            }

            //获取自己的模板样式 IsPublic = 0
            var data = _requestTemplateDto.AdTempStyle
                .Where(s => s.IsPublic == 0).Select(s => new AppAdTemplateStyleTable
                {
                    BaseMediaID = mediaId,
                    AdStyle = s.AdStyle,
                    AdTemplateID = adTemplateId,
                    CreateUserID = GetTemplateUserId(),
                    IsPublic = GetIsPublicByAdmin(s.IsPublic)//s.IsPublic
                });
            Dal.AdTemplate.AppAdTemplateStyle.Instance.Insert_BulkCopyToDB(data.ToList());
            return retValue;
        }

        /// <summary>
        /// 校验样式名称（套用模板的情况下才需要），注意这里校验的templateId 是所引用的公共模板id
        /// 还存在一种情况：运营角色直接对已通过的模板进行编辑或者新增呢？校验的templateId 就是当前id
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        public ReturnValue VerifyOfAdTemplateStyle(ReturnValue retValue)
        {
            //todo:不套用模板，完全新增，编辑不需要校验

            var templateId = _requestTemplateDto.BaseAdId;

            if (templateId <= 0)
            {
                if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate ||
                    _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
                {         //我也当作是操作的公共模板
                    templateId = _requestTemplateDto.TemplateId;
                }
                else
                {
                    if (UpdateBeforeTemplateInfo == null) return retValue;
                    if (UpdateBeforeTemplateInfo.AuditStatus == (int)Entities.Enum.AppTemplateEnum.已通过)
                    {
                        //我也当作是操作的公共模板
                        templateId = _requestTemplateDto.TemplateId;
                    }
                }
            }
            if (templateId <= 0)
            {
                return retValue;
            }

            if (_requestTemplateDto.AdTempStyle == null)
            {
                return retValue;
            }

            //todo:套用模板，且修正套用模板的时候需要校验
            //找到公共模板下面已经审核通过的样式列表
            var tempStyleList = Dal.AdTemplate.AppAdTemplateStyle.Instance.GetList(new AdTemplateQuery<AppAdTemplateStyle>()
            {
                TemplateId = templateId,
                BaseMediaId = _requestTemplateDto.BaseMediaId,
                IsPublic = 1
            }).Select(s => s.AdStyle);

            //做交集处理，样式名称是否存在交集，存在，则证明有重复
            var argusAdStyleList = _requestTemplateDto.AdTempStyle.Where(s => s.IsPublic == 0).Select(s => s.AdStyle).ToList();

            var intersectList = tempStyleList.Intersect(argusAdStyleList).ToList();
            if (intersectList.Count > 0)
            {
                return CreateFailMessage(retValue, "70061", GetAdStyleFailMessage(string.Join(",", intersectList)));
            }

            return retValue;
        }

        private string GetAdStyleFailMessage(string joinStyles)
        {
            if (_requestTemplateDto.IsModified)
            {
                return string.Format("广告样式:【{0}】与已通过的模板中的重复,请重新修正或驳回", joinStyles);
            }
            return string.Format("广告样式:【{0}】与已通过的模板中的重复,请修改！请勿重复", joinStyles);
        }

        private string GetAdTemplateGroupCitysFailMessage(string groupName, string joinCitys, bool isGroup)
        {
            if (isGroup)
            {
                if (_requestTemplateDto.IsModified)
                {
                    return string.Format("售卖区域中的城市组:【{0}】与已通过的模板中的重复，请重新修正或驳回", groupName);
                }
                return string.Format("售卖区域中的城市组:【{0}】与已通过的模板中的重复,请修改！请勿重复", groupName);
            }
            else
            {
                if (_requestTemplateDto.IsModified)
                {
                    return string.Format("售卖区域中的城市组:【{0}】中的城市:【{1}】与已通过模板中的重复，请重新修正或驳回", groupName, joinCitys);
                }
                return string.Format("售卖区域中的城市组:【{0}】中的城市:【{1}】与已通过模板中的重复,请修改！请勿重复", groupName, joinCitys);
            }
        }

        public List<AdSaleAreaGroupDto> GetAreaGroupDtoList(int templateId)
        {
            var groupCitysStr = Dal.AdTemplate.SaleAreaInfo.Instance.GetListBy(templateId);

            return MapperToAdSaleAreaGroup(groupCitysStr);
        }

        /// <summary>
        /// 城市组名称校验，同样式一样，（套用模板的情况下才需要），注意这里校验的templateId 是所引用的公共模板id
        /// 还存在一种情况：运营角色直接对已通过的模板进行编辑或者新增呢？校验的templateId 就是当前id
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        public ReturnValue VerifyOfAdTemplateGroupCitys(ReturnValue retValue)
        {
            //todo:不套用模板，完全新增，编辑不需要校验

            //todo:套用模板，且修正套用模板的时候需要校验
            var templateId = _requestTemplateDto.BaseAdId;

            #region 不套用模板，完全新增，编辑不需要校验

            if (templateId <= 0)
            {
                if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate ||
                    _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
                {         //我也当作是操作的公共模板
                    templateId = _requestTemplateDto.TemplateId;
                }
                else
                {
                    if (UpdateBeforeTemplateInfo == null) return retValue;
                    if (UpdateBeforeTemplateInfo.AuditStatus == (int)Entities.Enum.AppTemplateEnum.已通过)
                    {
                        //我也当作是操作的公共模板
                        templateId = _requestTemplateDto.TemplateId;
                    }
                }
            }
            if (templateId <= 0)
            {
                return retValue;
            }
            if (_requestTemplateDto.AdSaleAreaGroup == null)
            {
                return retValue;
            }

            #endregion 不套用模板，完全新增，编辑不需要校验

            //找的是公共模板的已通过和未通过的，城市组及城市列表（不包括全国，其他城市）
            var groupCitysList = GetAreaGroupDtoList(templateId);
            //所有的城市，因为交叉也不能重复
            var citysList = groupCitysList.Where(s => s.GroupType == (int)SaleAreaGroupTypeEnum.Citys && s.IsPublic == 1);
            var allCitys = new List<int>();
            foreach (var item in citysList)
            {
                allCitys.AddRange(item.DetailArea.Where(s => s.IsPublic == 1).Select(s => s.CityId));
            }

            foreach (var item in _requestTemplateDto.AdSaleAreaGroup)
            {
                bool isVerify = false;
                if (item == null) continue;
                if (Convert.ToBoolean(item.IsPublic))
                {
                    //增量添加：只需要除去公共部分的自己添加的内容
                    continue;
                }
                if (item.GroupType == (int)SaleAreaGroupTypeEnum.AllCountry || item.GroupType == (int)SaleAreaGroupTypeEnum.Other)
                {
                    continue;//全国是固定的存在，不处理，程序后面会自动添加一个全国数据进去（先删除）
                }

                if (item.GroupId > 0)
                {
                    //存在2种情况
                    //1:套用了公共模板，在公共模板基础上加了城市
                    //2.将源数据回传给我
                    if (groupCitysList == null || groupCitysList.Count == 0)
                    {
                        continue;
                    }
                    //todo:找到当前GroupId在数据库中是否是已经通过的状态，如果是：则说明是在这个城市组中添加了城市,否则就说明是回传回来的参数
                    retValue = VerifyOfAdTemplateGroupCitysPublicCitys(retValue, groupCitysList, item, allCitys, out isVerify);
                    if (retValue.HasError)
                    {
                        return retValue;
                    }
                    //todo:验证城市组的名称
                }
                //todo:同样得验证城市组的名称
                if (!isVerify)
                {
                    if (groupCitysList == null || groupCitysList.Count == 0)
                    {
                        continue;
                    }
                    var inersectList = groupCitysList.Where(s => s.GroupName.Equals(item.GroupName) && s.IsPublic == 1).ToList();
                    if (inersectList.Count > 0)
                    {
                        //有交集，存在重复的城市
                        return CreateFailMessage(retValue, "70066",
                            GetAdTemplateGroupCitysFailMessage(item.GroupName, string.Empty, true));
                    }

                    //继续验证新添加的城市组中的城市
                    var inersectListCitys = allCitys.Intersect(item.DetailArea.Select(s => s.CityId)).ToList();
                    if (inersectList.Count > 0)
                    {
                        //有交集，存在重复的城市
                        return CreateFailMessage(retValue, "70069", GetAdTemplateGroupCitysFailMessage(item.GroupName, string.Join(",", GetTipsCitys(inersectListCitys)), false));
                    }
                    return retValue;
                }
            }

            return retValue;
        }

        /// <summary>
        /// 获取重复添加、或者已存在通过的城市
        /// </summary>
        /// <param name="cityIds"></param>
        /// <returns></returns>
        public List<string> GetTipsCitys(List<int> cityIds)
        {
            //用于重复的城市提示
            var citysList = AdTemplateRelationDataProvider.GetAreaList();
            var listStr = new List<string>();
            cityIds.ForEach(item =>
            {
                listStr.Add(citysList.Where(s => s.AreaID.Equals(item.ToString())).Select(s => s.AreaName).FirstOrDefault());
            });
            return listStr;
        }

        private ReturnValue VerifyOfAdTemplateGroupCitysPublicCitys(ReturnValue retValue,
            List<AdSaleAreaGroupDto> groupCitysList, AdSaleAreaGroupDto item, List<int> allCitys
            , out bool isVerify)
        {
            var currentItemGroup = groupCitysList.FirstOrDefault(s => s.GroupId == item.GroupId);

            if (currentItemGroup != null)
            {
                //公共模板+自己的
                if (currentItemGroup.IsPublic == 1)
                {
                    isVerify = true;
                    //公共模板，那就要校验里面追加的城市了
                    if (currentItemGroup.DetailArea != null && item.DetailArea != null)
                    {
                        var inersectList = allCitys.Intersect(item.DetailArea.Select(s => s.CityId)).ToList();
                        if (inersectList.Count > 0)
                        {
                            //有交集，存在重复的城市
                            return CreateFailMessage(retValue, "70064", GetAdTemplateGroupCitysFailMessage(item.GroupName, string.Join(",", GetTipsCitys(inersectList)), false));
                        }
                        return retValue;
                    }
                    return retValue;
                }
                else
                {
                    //不是公共模板的城市组，也要继续校验,只需要校验城市组，不需要具体到城市
                    isVerify = true;
                    var inersectList = groupCitysList.Where(s => s.GroupName.Equals(item.GroupName) && s.IsPublic == 1).ToList();
                    if (inersectList.Count > 0)
                    {
                        //有交集，存在重复的城市
                        return CreateFailMessage(retValue, "70065", GetAdTemplateGroupCitysFailMessage(item.GroupName, string.Empty, true));
                    }
                    return retValue;
                }
            }
            else
            {
                //因为这里找的是父模板id 的信息，即找的是所引用的模板的城市组及城市，找不到正常
                //return CreateFailMessage(retValue, "70063", string.Format("当前城市组GroupId：{0} 名称：{1} 在数据库不存在，数据可能过期，请刷新页面",
                //    item.GroupId, item.GroupName));
            }
            isVerify = false;
            return retValue;
        }

        public int GetIsPublicByAdmin(int isPublic)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.SupperAdmin ||
                _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
                if (!_requestTemplateDto.IsModified)//不是修正
                    return 1;
            }
            return isPublic;
        }

        private int GetTemplateUserId()
        {
            if (_configEntity.CureOperateType == OperateType.Insert)
            {
                return _configEntity.CreateUserId;
            }
            return UpdateBeforeTemplateInfo != null ?
                UpdateBeforeTemplateInfo.CreateUserID : _configEntity.CreateUserId;
        }

        /// <summary>
        /// 模板城市组操作
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="adTemplateId">模板id</param>
        /// <returns></returns>
        private ReturnValue AdTemplateGroupCitys(ReturnValue retValue, int adTemplateId)
        {
            bool isDeleteOperate = false;
            Loger.Log4Net.InfoFormat("AdTemplateGroupCitys 开始...");

            Loger.Log4Net.InfoFormat("AdTemplateGroupCitys 开始...AdSaleAreaGroup：{0}", JsonConvert.SerializeObject(_requestTemplateDto.AdSaleAreaGroup));

            if (_requestTemplateDto.AdSaleAreaGroup == null)
            {
                //证明是完全新增或编辑，加入全国,或者是全部删除
                Dal.AdTemplate.SaleAreaInfo.Instance.Delete(adTemplateId);
                return AdTemplateGroupAllCountry(retValue, adTemplateId, _requestTemplateDto.BaseAdId);
            }

            List<SaleAreaRelationTable> saleAreaRelationTableList = new List<SaleAreaRelationTable>();
            List<SaleAreaRelationTable> saleAreaRelationTablePublicList = new List<SaleAreaRelationTable>();
            var saleAreaInfoList = new List<AdSaleAreaGroupDto>();
            var info = GetItem(adTemplateId);//找到一个问题所在：为了区分是前端回传回来的数据还是套用模板的数据，只能去根据当前模板id找城市组信息，判断是否是审核通过的状态，IsPublic=1
            if (info != null)
            {
                saleAreaInfoList = info.AdSaleAreaGroup;//公共+自己的
                Loger.Log4Net.InfoFormat("AdTemplateGroupCitys.GetItem saleAreaInfoList:{0}...", JsonConvert.SerializeObject(saleAreaInfoList));
            }
            _requestTemplateDto.AdSaleAreaGroup.ForEach(item =>
            {
                if (item == null) return;
                bool isAdd = false;
                Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach 开始...{0}", JsonConvert.SerializeObject(item));
                if (Convert.ToBoolean(item.IsPublic))
                {
                    //增量添加：只需要除去公共部分的自己添加的内容
                    Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach return.item.IsPublic=1...");
                    return;
                }
                if (item.GroupType == (int)SaleAreaGroupTypeEnum.AllCountry || item.GroupType == (int)SaleAreaGroupTypeEnum.Other)
                {
                    Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach return. SaleAreaGroupTypeEnum.AllCountry...");
                    return;//全国是固定的存在，不处理，程序后面会自动添加一个全国数据进去（先删除）
                }

                if (item.GroupId > 0)
                {
                    //说明是在套用模板城市组基础添加了城市,要获取当前GroupId、添加到SaleAreaRelationTable中，且IsPublic = 0
                    //还有一种情况，就是前端没有修改内容，原封不动的数据给我传回来了（没有套用模板的情况下）
                    Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach item.GroupId > 0 开始...");
                    //既然当前GroupId存在，则去当前模板下面找这个GroupId的信息，如果是IsPublic=1 则是公共模板，则不能删除，否则是可以删除，重新添加
                    if (!AdTemplateGroupCitysSaleAreaRelation(retValue, saleAreaInfoList, item, saleAreaRelationTablePublicList,
                        out isAdd, adTemplateId))
                    {
                        //前端传过来的数据，在当前模板下的城市组不存在，不用继续

                        Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach item.GroupId > 0 return.AdTemplateGroupCitysSaleAreaRelation..");
                        return;
                    }

                    Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach item.GroupId > 0 结束...");
                }
                //前面item.GroupId > 0 已经处理过DetailArea == null的情况，但是不全面，如果GroupId <= 0 的情况下还存在DetailArea == null ,则视为无效数据（全国，其它城市后面一起处理）
                if (item.DetailArea == null)
                {
                    Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach return. DetailArea为null...");
                    return;//如果DetailArea为null 则证明该城市组准备删除
                }

                #region SaleAreaInfo

                if (!isDeleteOperate)
                {
                    Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach 删除再更新(只删除一次，否则后面的会删除之前的),城市也要清空数据...");
                    //删除再更新(只删除一次，否则后面的会删除之前的),城市也要清空数据
                    isDeleteOperate = true;
                    Dal.AdTemplate.SaleAreaInfo.Instance.Delete(adTemplateId);
                    Dal.AdTemplate.SaleAreaInfo.Instance.DeleteSaleAreaRelation(saleAreaInfoList.Where(s => s.IsPublic == 0)
                        .Select(s => s.GroupId).ToList());
                }
                Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach SaleAreaInfo.Insert...");
                if (isAdd)
                {
                    //因为在groupId>0 逻辑处理已经处理过在已,可能会导致，在公共模板城市组添加了该城市，然后又重复添加了一个城市组和城市
                    return;
                }
                var saleAreaId = Dal.AdTemplate.SaleAreaInfo.Instance.Insert(new SaleAreaInfo()
                {
                    GroupName = item.GroupName,
                    CreateUserID = GetTemplateUserId(),
                    GroupType = item.GroupType,
                    TemplateID = adTemplateId,
                    IsPublic = GetIsPublicByAdmin(0) == 1//s.IsPublic
                });

                if (saleAreaId > 0)
                {
                    //定义最后的table类，一起批量添加到表

                    saleAreaRelationTableList.AddRange(item.DetailArea.Where(s => s.IsPublic == 0)
                        .Select(s => new SaleAreaRelationTable()
                        {
                            CityID = s.CityId,
                            ProvinceID = s.ProvinceId,
                            CreateUserID = GetTemplateUserId(),
                            GroupID = saleAreaId,
                            IsPublic = GetIsPublicByAdmin(s.IsPublic)//s.IsPublic
                        }));
                }

                #endregion SaleAreaInfo
            });
            if (_requestTemplateDto.BaseAdId <= 0)
            {
                //只要是完全新增，且不是套用模板则可以当作一个新的模板，默认有全国
                //现在约定，全国的不传过来，程序默认加上
                Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach _requestTemplateDto.BaseAdId <= 0..AdTemplateGroupAllCountry.");
                AdTemplateGroupAllCountry(retValue, adTemplateId, _requestTemplateDto.BaseAdId);
            }
            saleAreaRelationTableList.AddRange(saleAreaRelationTablePublicList.Distinct());//将已存在城市组的追加列表添加到此list中
            if (saleAreaRelationTableList.Count > 0)
            {
                Loger.Log4Net.InfoFormat("saleAreaRelationTableList:{0}",
                    JsonConvert.SerializeObject(saleAreaRelationTableList));
                //SaleAreaRelation
                Dal.AdTemplate.SaleAreaInfo.Instance.Insert_SaleAreaRelation_BulkCopyToDB(
                    saleAreaRelationTableList.ToDataTable(),
                    adTemplateId);
                //计算其他城市
                AdTemplateGroupOtherCitysCalculation(retValue, adTemplateId, _requestTemplateDto.BaseAdId, saleAreaRelationTableList);
            }
            Loger.Log4Net.InfoFormat("AdTemplateGroupCitys 结束...");
            return retValue;
        }

        private bool AdTemplateGroupCitysSaleAreaRelation(ReturnValue retValue,
            List<AdSaleAreaGroupDto> saleAreaInfoList, AdSaleAreaGroupDto item
            , List<SaleAreaRelationTable> saleAreaRelationTableList, out bool isAdd, int templateId)
        {
            Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach item.GroupId > 0 AdTemplateGroupCitysSaleAreaRelation 开始...");
            var info = saleAreaInfoList.FirstOrDefault(s => s.GroupId == item.GroupId);
            if (info != null)
            {
                Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach item.GroupId > 0 AdTemplateGroupCitysSaleAreaRelation info != null...");
                //回传回来的groupId,在当前模板中存在（或者在公共+自己的模板中存在）
                if (info.IsPublic == 1)
                {
                    Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach item.GroupId > 0 AdTemplateGroupCitysSaleAreaRelation info.IsPublic == 1...");

                    //需要先删除：groupId+用户id+isPublic=0
                    Dal.AdTemplate.SaleAreaInfo.Instance.DeleteSaleAreaRelation(item.GroupId, GetTemplateUserId(), templateId);

                    //则是公共模板(说明之前在套用模板的城市组里面加了城市，现在已经删除了，不需要了)
                    if (item.DetailArea == null)
                    {
                        isAdd = false;
                        Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach item.GroupId > 0 AdTemplateGroupCitysSaleAreaRelation item.DetailArea == null...");
                        return false;
                    }
                    isAdd = true;
                    //可能是在套用模板的城市组里面加了城市
                    //saleAreaRelationTableList = new List<SaleAreaRelationTable>();
                    saleAreaRelationTableList.AddRange(item.DetailArea.Where(s => s.IsPublic == 0)
                        .Select(s => new SaleAreaRelationTable()
                        {
                            CityID = s.CityId,
                            ProvinceID = s.ProvinceId,
                            CreateUserID = GetTemplateUserId(),
                            GroupID = item.GroupId,
                            TemplateID = templateId,//所属哪个模板下面
                            IsPublic = GetIsPublicByAdmin(s.IsPublic) // s.IsPublic
                        }));
                    return true;
                }
                else
                {
                    Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach item.GroupId > 0 AdTemplateGroupCitysSaleAreaRelation DeleteForGroupId..");
                    //可以删除，再添加(根据GroupId)
                    Dal.AdTemplate.SaleAreaInfo.Instance.DeleteForGroupId(item.GroupId);
                }
            }
            else
            {
                isAdd = false;
                Loger.Log4Net.InfoFormat("AdTemplateGroupCitys._requestTemplateDto.AdSaleAreaGroup.ForEach item.GroupId > 0 AdTemplateGroupCitysSaleAreaRelation info == null...");
                return false;
            }
            isAdd = false;
            return true;
        }

        /// <summary>
        /// 其他城市计算，全国的城市减去当前的城市数量
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="templateId"></param>
        /// <param name="baseTemplateId"></param>
        /// <param name="saleAreaRelationTableList">城市列表</param>
        /// <returns></returns>
        private ReturnValue AdTemplateGroupOtherCitysCalculation(ReturnValue retValue, int templateId, int baseTemplateId,
            List<SaleAreaRelationTable> saleAreaRelationTableList)
        {
            if (baseTemplateId > 0)
                return retValue;//已套用了公共模板，不需要再添加其他城市

            var citysCount = AdTemplateRelationDataProvider.GetAreaList().Where(s => !s.PID.Equals("0")).ToList().Count;

            if (citysCount > saleAreaRelationTableList.GroupBy(s => s.CityID).ToList().Count)
            {
                //城市列表的城市小于全国的城市
                //todo:添加其他城市
                Dal.AdTemplate.SaleAreaInfo.Instance.InsertOtherCitys(templateId, GetTemplateUserId(), GetIsPublicByAdmin(0) == 1);
            }

            return retValue;
        }

        private ReturnValue AdTemplateGroupAllCountry(ReturnValue retValue, int templateId, int baseTemplateId)
        {
            Loger.Log4Net.InfoFormat("AdTemplateGroupCitys.AdTemplateGroupAllCountry 添加全国...templateId={0},userId={1}", templateId, GetTemplateUserId());
            if (baseTemplateId > 0)
                return retValue;//已套用了公共模板，不需要再添加其他城市
            //todo:添加全国
            Dal.AdTemplate.SaleAreaInfo.Instance.InsertAllCountry(templateId, GetTemplateUserId(), GetIsPublicByAdmin(0));
            Loger.Log4Net.InfoFormat("AdTemplateGroupCitys.AdTemplateGroupAllCountry 添加全国.结束...templateId={0},userId={1}", templateId, GetTemplateUserId());
            return retValue;
        }

        public ReturnValue AdTemplateGroupCityOnModified(ReturnValue retValue, int adTemplateId)
        {
            //是否是修正，修正就意味着就是审核通过
            if (!_requestTemplateDto.IsModified)
            {
                return retValue;
            }

            //todo:将添加的城市组和样式追加到公共模板上
            //必须是修正状态，而且必须有BaseTemplateId

            try
            {
                Dal.AdTemplate.AppAdTemplate.Instance.TemplateModified(adTemplateId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("adTemplate AdTemplateGroupCityOnModified TemplateModified is error:{0},参数：adTemplateId={1}",
                exception.Message + (exception.StackTrace ?? string.Empty), adTemplateId);
            }

            return retValue;
        }

        /// <summary>
        /// 审核信息入库
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="templateId"></param>
        /// <param name="templateEnum"></param>
        /// <returns></returns>
        private ReturnValue AuditMessageOperate(ReturnValue retValue, int templateId,
            AppTemplateEnum templateEnum = AppTemplateEnum.待审核)
        {
            OperateAuditMsg.Instance.OperateAuditMsgInsert(templateId, (int)OperateAuditMsgType.AuditTemplate,
                (int)templateEnum, _configEntity.CreateUserId);
            return retValue;
        }

        /// <summary>
        /// 运营模板审核修正信息-取缺少信息部分（减少选项部分）
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        public ReturnValue AdminAuditUpdateInfo(ReturnValue retValue)
        {
            //todo:
            //--1:广告形式（单选），删除条件 ADStyle != 当前传过来的修改值()
            //2:广告样式（多选，可新增）:找到不是公共模板部分的信息，IsPublic = 0,获取到差值，新增的部分是没有id的，先取库里的源数据，和编辑的信息取交集，然后用结果集和源数据取反集
            //3:轮播数（递增）：与源数据库对比，修改之前的如果为4，现在为7，则删除4的价格
            //4:售卖平台（与的结果）：前端传过来的是与的结果：1，2，4，8，数据库存的也是这样，现在需要解析出对应的值，12002|IOS,然后和修改的部分取交集，用交集去删除not in
            //5:售卖方式：同上
            //6:区域:

            var sqlWhereCondition = GetDeletePriceWhereCondition();

            if (string.IsNullOrWhiteSpace(sqlWhereCondition))
            {
                Loger.Log4Net.InfoFormat("AdminAuditUpdateInfo 审核修正操作，删除app价格信息，没有获取到获取条件，不执行删除操作，模板TemplateID={0}",
                    _requestTemplateDto.TemplateId);

                return retValue;
            }
            Loger.Log4Net.InfoFormat("AdminAuditUpdateInfo 审核修正操作，删除app价格信息，获取条件为：模板TemplateID={0} AND {1}",
                _requestTemplateDto.TemplateId, sqlWhereCondition);
            //删除相关信息，并且返回删除的list
            var execList = Dal.AdTemplate.SaleAreaInfo.Instance.DeleteAdPrice(_requestTemplateDto.TemplateId, sqlWhereCondition);

            if (execList.Count <= 0)
            {
                Loger.Log4Net.ErrorFormat("AdminAuditUpdateInfo 审核修正操作，删除app价格信息，删除失败！！！获取条件为：模板TemplateID={0} AND {1}",
                    _requestTemplateDto.TemplateId, sqlWhereCondition);

                return retValue;
            }
            Loger.Log4Net.InfoFormat("AdminAuditUpdateInfo 审核修正操作，删除app价格信息，删除执行完成，获取条件为：模板TemplateID={0} AND {1}",
           _requestTemplateDto.TemplateId, sqlWhereCondition);

            //删除相关刊例
            var pubIdList = execList.Select(s => s.PubID).Distinct().ToList();
            Loger.Log4Net.InfoFormat("AdminAuditUpdateInfo 审核修正操作，删除相关刊例信息，刊例id={0}", JsonConvert.SerializeObject(pubIdList));

            var execCount = Dal.AdTemplate.SaleAreaInfo.Instance.DeletePublishBasicInfo(pubIdList);
            if (execCount <= 0)
            {
                Loger.Log4Net.ErrorFormat("AdminAuditUpdateInfo 审核修正操作，删除相关刊例信息，删除刊例失败！！！，" +
                                         "刊例id={0}", JsonConvert.SerializeObject(pubIdList));
            }
            Loger.Log4Net.InfoFormat("AdminAuditUpdateInfo 审核修正操作，删除相关刊例信息，执行完成，" +
                                   "刊例id={0}", JsonConvert.SerializeObject(pubIdList));
            return retValue;
        }

        private string GetDeletePriceWhereCondition()
        {
            var sbCondition = new StringBuilder();
            //获取广告样式id
            var adStyleId = GetAdStyleId(_requestTemplateDto.TemplateId);
            if (adStyleId.Count > 0)
            {
                sbCondition.AppendFormat(" OR ADStyle IN ({0})", string.Join(",", adStyleId));
            }
            //轮播数
            var offestCarouselNumber = GetCarouselNumber();
            if (offestCarouselNumber.Count > 0)
            {
                sbCondition.AppendFormat(" OR (CarouselNumber >= {0} AND CarouselNumber < {1})",
                    offestCarouselNumber[0], offestCarouselNumber[1]);
            }
            //售卖平台
            var salePlatformListId = GetSalePlatform();
            //排除
            if (salePlatformListId.Item1.Count > 0)
            {
                sbCondition.AppendFormat(" OR ADStyle NOT IN ({0})", string.Join(",", salePlatformListId.Item1));
            }
            //删除
            if (salePlatformListId.Item2.Count > 0)
            {
                sbCondition.AppendFormat(" OR ADStyle IN ({0})", string.Join(",", salePlatformListId.Item2));
            }

            //售卖方式
            var saleModeListId = GetSaleMode();
            //排除
            if (saleModeListId.Item1.Count > 0)
            {
                sbCondition.AppendFormat(" OR SaleType NOT IN ({0})", string.Join(",", saleModeListId.Item1));
            }
            //删除
            if (saleModeListId.Item2.Count > 0)
            {
                sbCondition.AppendFormat(" OR SaleType IN ({0})", string.Join(",", saleModeListId.Item2));
            }
            //售卖区域id
            var adSaleGroupId = GetAdSaleAreaGroupId(_requestTemplateDto.TemplateId);
            if (adSaleGroupId.Count > 0)
            {
                sbCondition.AppendFormat(" OR SaleArea IN ({0})", string.Join(",", adSaleGroupId));
            }
            return sbCondition.ToString();
        }

        private List<int> GetCarouselNumber()
        {
            var listId = new List<int>();
            if (UpdateBeforeTemplateInfo.CarouselCount > _requestTemplateDto.CarouselCount)
            {
                return listId;
            }
            listId.Add(_requestTemplateDto.CarouselCount);
            listId.Add(UpdateBeforeTemplateInfo.CarouselCount);
            return listId;
        }

        private List<int> GetAdSaleAreaGroupId(int adTemplateId)
        {
            var listId = new List<int>();
            if (_requestTemplateDto.AdSaleAreaGroup.Count == 0)
                return listId;

            //筛选前端传过来的数据
            var adSaleAreaGroupList = _requestTemplateDto.AdSaleAreaGroup.Where(s => s.IsPublic == 0).Select(s => s.GroupName).ToList();
            //获取数据库原始数据(非公共模板售卖区域)
            var tempSaleAreaGroupList = Dal.AdTemplate.SaleAreaInfo.Instance.GetList(new AdTemplateQuery<SaleAreaInfo>()
            {
                TemplateId = adTemplateId,
                IsPublic = 0
            });
            var dataSourceAdStyleList = tempSaleAreaGroupList.Select(s => s.GroupName).ToList();
            //先取交集（1，2，4，8，16） Intersect （1，8，32，64） == （1，8）
            var intersectList = adSaleAreaGroupList.Intersect(dataSourceAdStyleList);
            //再用户数据库数据与交集取差集 (dataSourceAdStyleList有，intersectList沒有)，就是获取少的部分
            var lastExceptadSaleAreaGroupList = dataSourceAdStyleList.Except(intersectList).ToList();
            //用取到的结果集去获取tempSaleAreaGroupList的id
            tempSaleAreaGroupList.ForEach(item =>
            {
                lastExceptadSaleAreaGroupList.ForEach(s =>
                {
                    if (item.GroupName.Equals(s))
                    {
                        listId.Add(item.GroupID);
                    }
                });
            });
            return listId;
        }

        private List<int> GetAdStyleId(int adTemplateId)
        {
            var listId = new List<int>();
            if (_requestTemplateDto.AdTempStyle.Count == 0)
                return listId;
            //筛选前端传过来的数据
            var adStyleList = _requestTemplateDto.AdTempStyle.Where(s => s.IsPublic == 0).Select(s => s.AdStyle).ToList();
            //获取数据库原始数据(非公共模板样式)
            var tempStyleList =
                 Dal.AdTemplate.AppAdTemplateStyle.Instance.GetList(new AdTemplateQuery<AppAdTemplateStyle>()
                 {
                     TemplateId = adTemplateId,
                     IsPublic = 0
                 });

            var dataSourceAdStyleList = tempStyleList.Select(s => s.AdStyle).ToList();
            //先取交集（1，2，4，8，16） Intersect （1，8，32，64） == （1，8）
            var intersectList = adStyleList.Intersect(dataSourceAdStyleList);
            //再用户数据库数据与交集取差集 (dataSourceAdStyleList有，intersectList沒有)，就是获取少的部分
            var lastExceptAdStyleList = dataSourceAdStyleList.Except(intersectList).ToList();
            //用取到的结果集去获取tempStyleList的id

            tempStyleList.ForEach(item =>
            {
                lastExceptAdStyleList.ForEach(s =>
                {
                    if (item.AdStyle.Equals(s))
                    {
                        listId.Add(item.RecID);
                    }
                });
            });

            return listId;
        }

        /// <summary>
        /// 返回售卖平台信息，item1：NOT INT 需要排除的id，item2：需要IN() 删除的id
        /// </summary>
        /// <returns></returns>
        private Tuple<List<int>, List<int>> GetSalePlatform()
        {
            var listId = new List<int>();
            //当前修改的部分List:12002|IOS
            var updateList = AdTemplateRelationDataProvider.GetSellingPlatformList(_requestTemplateDto.SellingPlatform);
            //数据源list
            var dataSourceList = AdTemplateRelationDataProvider.GetSellingPlatformList(UpdateBeforeTemplateInfo.SellingPlatform);

            //取交集
            var intersectList = dataSourceList.Intersect(updateList).ToList();
            if (intersectList.Count > 0)
            {
                intersectList.ForEach(item =>
                {
                    var str = GetAppContent(item.Split(new string[] { "|" }, StringSplitOptions.None), 0);
                    listId.Add(str.ToInt());
                });

                return new Tuple<List<int>, List<int>>(listId.Where(s => s > 0).ToList(), new List<int>());
            }

            //要是没有交集怎么办？
            //删除全部的售卖平台，数据源的
            dataSourceList.ForEach(item =>
            {
                var str = GetAppContent(item.Split(new string[] { "|" }, StringSplitOptions.None), 0);
                listId.Add(str.ToInt());
            });
            return new Tuple<List<int>, List<int>>(new List<int>(), listId.Where(s => s > 0).ToList());
        }

        private Tuple<List<int>, List<int>> GetSaleMode()
        {
            var listId = new List<int>();
            if (_requestTemplateDto.SellingMode == UpdateBeforeTemplateInfo.SellingMode)
            {
                return new Tuple<List<int>, List<int>>(new List<int>(), new List<int>());
            }

            //当前修改的部分List:11001|CPD,11002|CPM
            var updateList = AdTemplateRelationDataProvider.GetDicSaleModeList(_requestTemplateDto.SellingMode);
            //数据源list
            var dataSourceList = AdTemplateRelationDataProvider.GetSellingPlatformList(UpdateBeforeTemplateInfo.SellingMode);
            //取交集
            var intersectList = dataSourceList.Intersect(updateList).ToList();
            if (intersectList.Count > 0)
            {
                intersectList.ForEach(item =>
                {
                    var str = GetAppContent(item.Split(new string[] { "|" }, StringSplitOptions.None), 0);
                    listId.Add(str.ToInt());
                });

                return new Tuple<List<int>, List<int>>(listId.Where(s => s > 0).ToList(), new List<int>());
            }

            //要是没有交集怎么办？
            //删除全部的售卖平台，数据源的
            dataSourceList.ForEach(item =>
            {
                var str = GetAppContent(item.Split(new string[] { "|" }, StringSplitOptions.None), 0);
                listId.Add(str.ToInt());
            });
            return new Tuple<List<int>, List<int>>(new List<int>(), listId.Where(s => s > 0).ToList());
        }

        #endregion 异步处理广告样式、城市组

        #region 模板操作验证相关

        private ReturnValue VerifyBusiness(ReturnValue retValue)
        {
            //基础参数校验
            retValue = VerifyOfNecessaryParameters(_requestTemplateDto);
            if (retValue.HasError)
                return retValue;

            if (_configEntity.CureOperateType == OperateType.Edit)
            {
                if (_requestTemplateDto.TemplateId <= 0)
                {
                    return CreateFailMessage(retValue, "70001", "编辑模式下，请输入TemplateId");
                }
            }

            retValue = VerifyOfSellingModeOrPlatform(retValue);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyParamsByRole(retValue);
            if (retValue.HasError)
                return retValue;

            //retValue = VerifyAdForm(retValue);
            //if (retValue.HasError)
            //    return retValue;

            return retValue;
        }

        private ReturnValue VerifyBusinessTemplateName(ReturnValue retValue)
        {
            SetVerifyBusinessTemplateName();
            if (_configEntity.CureOperateType == OperateType.Insert)
            {
                return VerifyInsertAdTemplateNameByRole(retValue);
            }
            else
            {
                if (_requestTemplateInfoDto.AdTempId <= 0)
                {
                    return CreateFailMessage(retValue, "70044", "编辑状态下，校验模板名称必须传TemplateId");
                }
                return VerifyInsertAdTemplateNameByRole(retValue);
            }
        }

        private void SetVerifyBusinessTemplateName()
        {
            _requestTemplateInfoDto = _requestTemplateInfoDto ?? new RequestTemplateInfoDto();

            _requestTemplateInfoDto.AdTempName = _requestTemplateDto.AdTemplateName;
            _requestTemplateInfoDto.AdBaseTempId = _requestTemplateDto.BaseAdId;
            _requestTemplateInfoDto.AdTempId = _requestTemplateDto.TemplateId;
            _requestTemplateInfoDto.BaseMediaId = _requestTemplateDto.BaseMediaId;
        }

        public ReturnValue VerifyCreateBusiness(ReturnValue retValue)
        {
            retValue = VerifyBusiness(retValue);
            if (retValue.HasError)
                return retValue;

            //校验媒体信息（包括附表，基表）
            retValue = VerifyOfTemplateMediaInfo(retValue);
            if (retValue.HasError)
                return retValue;

            //如果是套用公共模板，属于新增，则不用校验模板名称（模板名称也不能修改）
            if (_requestTemplateDto.BaseAdId <= 0)
            {
                //单纯的添加模板
                retValue = VerifyBusinessTemplateName(retValue);
                if (retValue.HasError)
                    return retValue;

                if (_requestTemplateDto.AdTempStyle == null ||
                    _requestTemplateDto.AdTempStyle.Count == 0)
                {
                    return CreateFailMessage(retValue, "70050", string.Format("系统检测到此操作是完全新增的模型，样式信息没有传到后台，请检查参数"));
                }
                //if (_requestTemplateDto.AdSaleAreaGroup == null
                //    || _requestTemplateDto.AdSaleAreaGroup.Count == 0)
                //{
                //    return CreateFailMessage(retValue, "70051", string.Format("系统检测到此操作是完全新增的模型，城市组信息没有传到后台，请检查参数"));
                //}
            }
            else
            {
                //套用模板，一个媒体下，一个用户只能有一个
                retValue = VerifyInsertBaseAdTemplateIdByRole(retValue);
                if (retValue.HasError)
                    return retValue;
            }

            //如果是套用公共模板过来的，需要验证公共模板信息
            Entities.AdTemplate.AppAdTemplate appBaseAdTemplate = null;
            retValue = VerifyUpdateBaseTemplateInfo(retValue, out appBaseAdTemplate);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyUpdateCarouselCount(retValue, null, appBaseAdTemplate);
            if (retValue.HasError)
                return retValue;

            //最好是在校验所引用的模板信息之后再校验城市，样式
            retValue = VerifyOfAdTemplateStyle(retValue);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyOfAdTemplateGroupCitys(retValue);
            if (retValue.HasError)
                return retValue;

            return retValue;
        }

        /// <summary>
        /// 编辑-套用模板情况下：名称不让修改
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        public ReturnValue VerifyUpdateBusiness(ReturnValue retValue)
        {
            retValue = VerifyBusiness(retValue);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyUpdateBefore(retValue);
            if (retValue.HasError)
                return retValue;

            var info = retValue.ReturnObject as Entities.AdTemplate.AppAdTemplate;
            if (info == null)
                return CreateFailMessage(retValue, "70030", "拆箱失败");
            UpdateBeforeTemplateInfo = info;
            //编辑模式下
            //如果是套用公共模板被驳回，则不用校验模板名称（模板名称也不能修改）
            if (info.BaseAdID <= 0)
            {
                retValue = VerifyBusinessTemplateName(retValue);
                if (retValue.HasError)
                    return retValue;
            }

            //编辑状态下媒体id不能变
            _requestTemplateDto.BaseAdId = info.BaseAdID;
            _requestTemplateDto.BaseMediaId = info.BaseMediaID;

            retValue = VerifyUpdateCarouselCount(retValue, info, null);
            if (retValue.HasError)
                return retValue;

            //现在都是递增，需要校验公共模板信息（因为只需要自己修改或者新增的部分）
            //retValue = VerifyUpdateAdStyle(retValue, _requestTemplateDto.TemplateId, info.BaseAdID > 0);
            //if (retValue.HasError)
            //    return retValue;

            //最好是在校验所引用的模板信息之后再校验城市，样式
            retValue = VerifyOfAdTemplateStyle(retValue);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyOfAdTemplateGroupCitys(retValue);
            if (retValue.HasError)
                return retValue;

            return retValue;
        }

        private ReturnValue VerifyOfSellingModeOrPlatform(ReturnValue retValue)
        {
            var saleModeValue = AdTemplateRelationDataProvider.GetDicSaleMode(_requestTemplateDto.SellingMode);
            if (string.IsNullOrWhiteSpace(saleModeValue))
            {
                return CreateFailMessage(retValue, "70052", "SellingMode 售卖方式参数有错误，请仔细检查");
            }
            var salePlatform = AdTemplateRelationDataProvider.GetSellingPlatform(_requestTemplateDto.SellingPlatform);
            if (string.IsNullOrWhiteSpace(salePlatform))
            {
                return CreateFailMessage(retValue, "70053", "SellingPlatform 售卖平台参数有错误，请仔细检查");
            }
            return retValue;
        }

        /// <summary>
        /// 校验媒体信息
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyOfTemplateMediaInfo(ReturnValue retValue)
        {
            if (_requestTemplateDto.BaseMediaId > 0)
            {
                //基表id
                retValue = VerifyOfBaseMediaInfo(retValue, _requestTemplateDto.BaseMediaId);
                if (retValue.HasError)
                    return retValue;
            }
            else
            {
                //校验附表媒体id，获取基表媒体id
                var baseMediaId = 0;
                retValue = VerifyOfMediaInfo(retValue, _requestTemplateDto.MediaId, out baseMediaId);
                if (retValue.HasError)
                    return retValue;
                _requestTemplateDto.BaseMediaId = baseMediaId;
            }
            return retValue;
        }

        private ReturnValue VerifyOfMediaInfo(ReturnValue retValue, int mediaId, out int baseMediaId)
        {
            baseMediaId = 0;
            var info = Dal.Media.MediaPCAPP.Instance.GetEntity(mediaId);
            if (info == null)
                return CreateFailMessage(retValue, "70051", "当前媒体信息不存在，MediaId=" + mediaId);

            if (info.AuditStatus != (int)Entities.Enum.MediaAuditStatusEnum.AlreadyPassed)
                return CreateFailMessage(retValue, "70052", "当前媒体审核状态不是审核已通过状态，不能添加模板");

            if (info.BaseMediaID <= 0)
                return CreateFailMessage(retValue, "70053", "当前媒体下没有基表媒体id");

            baseMediaId = info.BaseMediaID;

            return retValue;
        }

        private ReturnValue VerifyOfBaseMediaInfo(ReturnValue retValue, int baseMediaId)
        {
            var info = Dal.Media.MediaBasePCAPP.Instance.GetEntity(baseMediaId);
            if (info == null)
                return CreateFailMessage(retValue, "70055", "当前媒体信息不存在，BaseMediaId=" + baseMediaId);

            return retValue;
        }

        private ReturnValue VerifyAdForm(ReturnValue retValue)
        {
            if (!Enum.IsDefined(typeof(AppTemplateAdFormEnum), _requestTemplateDto.AdForm))
            {
                return CreateFailMessage(retValue, "70033", "AdForm参数错误");
            }
            return retValue;
        }

        private ReturnValue VerifyUpdateBaseTemplateInfo(ReturnValue retValue, out Entities.AdTemplate.AppAdTemplate appBaseAdTemplate)
        {
            if (_requestTemplateDto.BaseAdId > 0)
            {
                var tempBaseInfo = Dal.AdTemplate.AppAdTemplate.Instance.GetEntity(_requestTemplateDto.BaseAdId);
                appBaseAdTemplate = tempBaseInfo;
                if (tempBaseInfo == null)
                {
                    return CreateFailMessage(retValue, "70036", "套用的模板信息不存在");
                }
                if (tempBaseInfo.AuditStatus != (int)Entities.Enum.AppTemplateEnum.已通过)
                {
                    return CreateFailMessage(retValue, "70057", "套用模板情况下：此模板还没有审核通过，不能套用该模板使用，BaseAdId=" + _requestTemplateDto.BaseAdId);
                }
                if (!tempBaseInfo.AdTemplateName.Equals(_requestTemplateDto.AdTemplateName))
                {
                    return CreateFailMessage(retValue, "70035", "套用模板情况下：名称不让修改");
                }
            }
            else
            {
                appBaseAdTemplate = null;
            }
            return retValue;
        }

        /// <summary>
        /// 套用公共模板属于添加
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="appAdTemplate"></param>
        /// <param name="appBaseAdTemplate"></param>
        /// <returns></returns>
        private ReturnValue VerifyUpdateCarouselCount(ReturnValue retValue,
            Entities.AdTemplate.AppAdTemplate appAdTemplate, Entities.AdTemplate.AppAdTemplate appBaseAdTemplate)
        {
            if (_requestTemplateDto.BaseAdId > 0)
            {
                if (_configEntity.CureOperateType == OperateType.Insert)
                {
                    if (_requestTemplateDto.CarouselCount != 1 &&
                         _requestTemplateDto.CarouselCount < appBaseAdTemplate.CarouselCount)
                    {
                        return CreateFailMessage(retValue, "70032", "轮播数不能小于公共模板的数量");
                    }
                }
            }
            else
            {
                if (_configEntity.CureOperateType == OperateType.Edit)
                {
                    if (_requestTemplateDto.IsModified)
                    {
                        if (_requestTemplateDto.BaseAdId > 0)
                        {
                            //修正，只要不比公共模板大 就满足
                            if (_requestTemplateDto.CarouselCount != 1 &&
                            _requestTemplateDto.CarouselCount < appBaseAdTemplate.CarouselCount)
                            {
                                return CreateFailMessage(retValue, "70032", "轮播数不能小于公共模板的数量");
                            }
                        }
                    }
                    //if (_requestTemplateDto.CarouselCount != 1 &&
                    //_requestTemplateDto.CarouselCount < appAdTemplate.CarouselCount)
                    //{
                    //    return CreateFailMessage(retValue, "70031", "轮播数不能比更改之前的小");
                    //}
                }
                //  //新增：默认必须大于0
            }

            return retValue;
        }

        private ReturnValue VerifyUpdateBefore(ReturnValue retValue)
        {
            //校验id要在name之前
            var info = Dal.AdTemplate.AppAdTemplate.Instance.GetEntity(_requestTemplateDto.TemplateId);
            if (info == null)
            {
                return CreateFailMessage(retValue, "70027", "当前模板信息不存在，AdTemplateId:" + _requestTemplateDto.TemplateId);
            }

            retValue.ReturnObject = info;

            if (_configEntity.RoleTypeEnum == RoleEnum.SupperAdmin
                || _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
                return retValue;
            }

            if (info.AuditStatus != (int)AppTemplateEnum.已驳回)
            {
                return CreateFailMessage(retValue, "70030", string.Format("当前媒体审核状态不可编辑，状态为：{0}", (MediaAuditStatusEnum)info.AuditStatus));
            }

            return retValue;
        }

        /// <summary>
        /// 注意：此方法只能用于修正,价格修改
        /// 如果是自己提交的模板，样式是可以编辑减少
        /// 如果是套用的公共模板，样式只能增，不能减少
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="adTemplateId"></param>
        /// <param name="isBaseTemplate">是否有父模板（是否套用了模板）</param>
        /// <returns></returns>
        private ReturnValue VerifyUpdateAdPriceAdStyle(ReturnValue retValue, int adTemplateId, bool isBaseTemplate)
        {
            //if (isBaseTemplate)
            //{
            //筛选前端传过来的数据
            var adStyleList = _requestTemplateDto.AdTempStyle.Where(s => s.IsPublic == 0).Select(s => s.AdStyle).ToList();
            //获取数据库原始数据(公共模板样式)
            var tempStyleList =
                 Dal.AdTemplate.AppAdTemplateStyle.Instance.GetList(new AdTemplateQuery<AppAdTemplateStyle>()
                 {
                     TemplateId = adTemplateId,
                     IsPublic = 0
                 });

            //提取公共模板的样式
            var tempDataBasePublic = tempStyleList.Where(s => s.IsPublic).Select(s => s.AdStyle).ToList();
            //编辑的参数：公共的模板样式与原有数据库公共的模板样式对比
            //取修改的数据和原始数据之间的交集，如果交集的数目少于原始的，那就说明减少了数据
            if (tempDataBasePublic.Count == 0)
                return retValue;
            var tempPublic = tempDataBasePublic.Intersect(_requestTemplateDto.AdTempStyle.Where(s => s.IsPublic == 1)
                .Select(s => s.AdStyle)).ToList();
            if (tempPublic.Count < tempDataBasePublic.Count)
            {
                return CreateFailMessage(retValue, "70028", "套用了公共模板，不能减少公共模板样式选项，只能追加");
            }
            ////取修改的数据和原始数据之间的交集，如果交集的数目少于原始的，那就说明减少了数据
            //var intersectList = tempStyleList.Select(s => s.AdStyle).Intersect(adStyleList).ToList();
            //if (intersectList.Count < adStyleList.Count)
            //{
            //    return CreateFailMessage(retValue, "70028", "套用了公共模板，不能减少模板样式选项，只能追加");
            //}

            retValue.ReturnObject = tempStyleList;//后面操作的时候需要用到
            //}
            IsBaseTemplate = isBaseTemplate;
            //retValue.PutValue(IsBaseTemplateKey, isBaseTemplate);//是否是套用的模板
            return retValue;
        }

        /// <summary>
        /// 校验模板名称是否存在
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="adTemplateName"></param>
        /// <param name="filterTemplateId"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public ReturnValue VerifyTemplateName(ReturnValue retValue, string adTemplateName, int filterTemplateId = -2, int mediaId = -2)
        {
            var info = Dal.AdTemplate.AppAdTemplate.Instance.GetList(new AdTemplateQuery<AppAdTemplate>()
            {
                TemplateName = adTemplateName,//_requestTemplateDto.AdTemplateName,
                PageSize = 1,
                FilterTemplateId = filterTemplateId,
                BaseMediaId = mediaId
            });
            if (info.Count > 0)
            {
                retValue.PutValue("verify_name_id", new { AdTempId = info[0].RecID });
                return CreateFailMessage(retValue, "70026", "此模板名称已存在");
            }

            return retValue;
        }

        /// <summary>
        /// 除了运营角色，AdLegendUrl示例图片是必填，其他都是非必须
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyParamsByRole(ReturnValue retValue)
        {
            if (_requestTemplateDto.IsModified) return retValue;
            if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate ||
                _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
            {
                if (string.IsNullOrWhiteSpace(_requestTemplateDto.AdLegendUrl))
                    return CreateFailMessage(retValue, "70025", "请输入示例图片");
                if (_requestTemplateDto.AdLegendUrl.Contains("http://") ||
                    _requestTemplateDto.AdLegendUrl.Contains("https://"))
                {
                    var spAdUrl = _requestTemplateDto.AdLegendUrl.Split(',');
                    var sbUrl = new StringBuilder();
                    foreach (var item in spAdUrl)
                    {
                        if (string.IsNullOrWhiteSpace(item)) continue;
                        sbUrl.Append(item.ToAbsolutePath(true) + ",");
                    }
                    _requestTemplateDto.AdLegendUrl = sbUrl.ToString().TrimEnd(',');
                }
            }
            else
            {
                //刊例原件
                if (string.IsNullOrWhiteSpace(_requestTemplateDto.OriginalFile))
                {
                    return CreateFailMessage(retValue, "70026", "请输入刊例原件");
                }
            }

            return retValue;
        }

        public ReturnValue VerifyOfRoleModule(ReturnValue retValue)
        {
            throw new NotImplementedException();
        }

        #endregion 模板操作验证相关

        #region 查询详情相关

        /// <summary>
        /// 广告模板查询详情（编辑渲染页面）
        /// 场景一：编辑自己的驳回的
        /// 场景二：点击套用模板过来的编辑渲染
        /// </summary>
        /// <returns></returns>
        public RespAdTemplateItemDto GetInfo()
        {
            var templateId = _requestTemplateInfoDto.AdBaseTempId > 0
                ? _requestTemplateInfoDto.AdBaseTempId
                : _requestTemplateInfoDto.AdTempId;
            if (templateId <= 0)
            {
                return null;
            }
            return GetItem(templateId);
        }

        private RespAdTemplateItemDto GetItem(int templateId)
        {
            return BLL.AdTemplate.AppAdTemplate.Instance.GetInfoV1(templateId, 0, 0, _configEntity.CreateUserId);
        }

        /// <summary>
        /// 解析广告样式
        /// </summary>
        /// <param name="adTemplate"></param>
        /// <returns></returns>
        public static List<AdTempStyleDto> MapperToAdTempStyle(string adTemplate)
        {
            /*
             示例：1,STYLE_1,1|2,STYLE_2,1|3,STYLE_3,1,1(IsPublic),1211(userId)
             */
            if (string.IsNullOrWhiteSpace(adTemplate)) return null;
            var list = new List<AdTempStyleDto>();

            foreach (var item in adTemplate.Split(new string[] { "|" }, StringSplitOptions.None))
            {
                //47004,科技
                if (string.IsNullOrWhiteSpace(item)) continue;
                var itemClass = item.Split(new string[] { "," }, StringSplitOptions.None);
                var adTemp = new AdTempStyleDto
                {
                    AdStyleId = GetAppContent(itemClass, 0).ToInt(-2),
                    AdStyle = GetAppContent(itemClass, 1),
                    BaseMediaID = GetAppContent(itemClass, 2).ToInt(-2),
                    IsPublic = GetAppContent(itemClass, 3).ToInt(0),
                    CreateUserId = GetAppContent(itemClass, 4).ToInt()
                };
                list.Add(adTemp);
            }

            return list;
        }

        /// <summary>
        /// 解析城市组数据
        /// </summary>
        /// <param name="adSaleAreaGroup"></param>
        /// <returns></returns>
        public static List<AdSaleAreaGroupDto> MapperToAdSaleAreaGroup(string adSaleAreaGroup)
        {
            /*
             示例：1,城市组1,0(GroupType),0(IsPublic)$=10,河南省,0(IsPublic)@=1001,郑州市,1121(CreateUserId)
                |1,城市组1$=10,河南省,0(IsPublic)@=1002,洛阳市
                |1,城市组1$=10,河南省,0(IsPublic)@=1003,周口市
                |2,城市组2$=1,安徽省,0(IsPublic)@=101,合肥市
                |2,城市组2$=1,安徽省,0(IsPublic)@=102,安庆市
                |2,城市组2$=1,安徽省,0(IsPublic)@=103,蚌埠市

             */
            if (string.IsNullOrWhiteSpace(adSaleAreaGroup)) return null;
            var list = new List<AdSaleAreaGroupDto>();

            foreach (var item in adSaleAreaGroup.Split(new string[] { "|" }, StringSplitOptions.None))
            {
                //1,城市组1$=10,河南省@=1001,郑州市
                if (string.IsNullOrWhiteSpace(item)) continue;
                var groupItemSp = item.Split(new string[] { "$=" }, StringSplitOptions.None);
                //1,城市组
                var groupItemSp0 = groupItemSp[0].Split(',');
                var groupItem = new AdSaleAreaGroupDto
                {
                    GroupId = GetAppContent(groupItemSp0, 0).ToInt(-2),
                    GroupName = GetAppContent(groupItemSp0, 1),
                    GroupType = GetAppContent(groupItemSp0, 2).ToInt(-2),
                    IsPublic = GetAppContent(groupItemSp0, 3).ToInt(0),
                };
                if (!string.IsNullOrWhiteSpace(groupItemSp[1]))
                {
                    //10,河南省,0(IsPublic)@=1002,洛阳市,1121(UserId)
                    var provinceItemSp = groupItemSp[1].Split(new string[] { "@=" }, StringSplitOptions.None);
                    var spprovinceItemSp0 = provinceItemSp[0].Split(',');
                    var spprovinceItemSp1 = provinceItemSp[1].Split(',');
                    groupItem.DetailArea = new List<AdSaleAreaGroupDetailDto>()
                    {
                      new AdSaleAreaGroupDetailDto()
                      {
                          ProvinceId = GetAppContent(spprovinceItemSp0, 0).ToInt(-2),
                          ProvinceName =  GetAppContent(spprovinceItemSp0, 1),
                          IsPublic = GetAppContent(spprovinceItemSp0, 2).ToInt(0),
                          CityId =GetAppContent(spprovinceItemSp1, 0).ToInt(-2),
                          CityName =  GetAppContent(spprovinceItemSp1, 1),
                          CreateUserId = GetAppContent(spprovinceItemSp1, 2).ToInt()
                      }
                    };
                }
                list.Add(groupItem);
            }
            //分组之后的数据处理
            var gg = list.GroupBy(y => y.GroupId);
            var returnList = new List<AdSaleAreaGroupDto>();

            foreach (var item in gg)
            {
                var rtItem = new AdSaleAreaGroupDto
                {
                    GroupId = item.Key,
                    GroupType = item.Select(s => s.GroupType).FirstOrDefault(),
                    GroupName = item.Select(s => s.GroupName).FirstOrDefault(),
                    IsPublic = item.Select(s => s.IsPublic).FirstOrDefault(),
                    DetailArea = new List<AdSaleAreaGroupDetailDto>()
                };
                item.Select(s => s.DetailArea).ToList().ForEach(o =>
                {
                    if (o != null)
                        rtItem.DetailArea.AddRange(o);
                });

                returnList.Add(rtItem);
            };

            return returnList;
        }

        /// <summary>
        /// 添加、编辑操作之前，验证模板名称，返回已存在的模板id
        /// </summary>
        /// <returns></returns>
        public ReturnValue VerifyAdTemplateName()
        {
            var retValue = new ReturnValue();
            if (string.IsNullOrWhiteSpace(_requestTemplateInfoDto.AdTempName))
                return CreateFailMessage(retValue, "70033", "请输入模板名称");
            if (_requestTemplateInfoDto.BaseMediaId <= 0 && _requestTemplateInfoDto.MediaId <= 0)
            {
                return CreateFailMessage(retValue, "70035", "请输入媒体BaseMediaId或者MediaId");
            }
            //为了兼容基表媒体id和附表媒体id
            retValue = VerifyAdTemplateNameMediaInfo(retValue);
            if (retValue.HasError)
            {
                return retValue;
            }
            if (_configEntity.CureOperateType == OperateType.Insert)
            {
                //先校验已通过的模板名称
                return VerifyInsertAdTemplateNameByRole(retValue);
            }
            else
            {
                if (_requestTemplateInfoDto.AdTempId <= 0)
                {
                    return CreateFailMessage(retValue, "70034", "编辑状态下，校验模板名称必须传TemplateId");
                }
                return VerifyInsertAdTemplateNameByRole(retValue);
            }
        }

        /// <summary>
        /// 模板添加、编辑。校验模板名称逻辑（为了兼容基表id，附表id）
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyAdTemplateNameMediaInfo(ReturnValue retValue)
        {
            if (_requestTemplateInfoDto.BaseMediaId > 0)
            {
                //基表id
                retValue = VerifyOfBaseMediaInfo(retValue, _requestTemplateInfoDto.BaseMediaId);
                if (retValue.HasError)
                    return retValue;
            }
            else
            {
                //校验附表媒体id，获取基表媒体id
                var baseMediaId = 0;
                retValue = VerifyOfMediaInfo(retValue, _requestTemplateInfoDto.MediaId, out baseMediaId);
                if (retValue.HasError)
                    return retValue;
                _requestTemplateInfoDto.BaseMediaId = baseMediaId;
            }
            return retValue;
        }

        private ReturnValue VerifyInsertAdTemplateNameByRole(ReturnValue retValue)
        {
            //todo:所有的角色都必须先校验：名称+媒体id+已通过
            //todo:然后再校验：
            //1.媒体主：名称 +媒体id+用户id+（待审核，驳回）
            //2.AE:名称+媒体id+角色用户id组+（待审核，驳回）

            //运营：名称+媒体id+已通过
            //
            var infoList = Dal.AdTemplate.AppAdTemplate.Instance.GetList(new AdTemplateQuery<AppAdTemplate>()
            {
                TemplateName = _requestTemplateInfoDto.AdTempName,
                PageSize = 1,
                BaseMediaId = _requestTemplateInfoDto.BaseMediaId,
                AuditStatus = (int)AppTemplateEnum.已通过,
                FilterTemplateId = _requestTemplateInfoDto.AdTempId
            });

            if (infoList.Count > 0)
            {
                var templateId = infoList.Select(s => s.RecID).FirstOrDefault();

                return CreateFailMessage(retValue, "70057", "库中已经存在该模板名称",
                    new
                    {
                        AdTemplateId = _requestTemplateInfoDto.AdTempId > 0 ? _requestTemplateInfoDto.AdTempId : templateId
                    ,
                        BaseMediaId = _requestTemplateInfoDto.BaseMediaId
                    });
            }

            if (_configEntity.RoleTypeEnum == RoleEnum.SupperAdmin || _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
            }
            else if (_configEntity.RoleTypeEnum == RoleEnum.AE)
            {
                //AE:名称+媒体id+已通过+角色用户id组
                var info = Dal.AdTemplate.AppAdTemplate.Instance.VerifyOfTemplateNameByRole(RoleInfoMapping.AE,
                     _requestTemplateInfoDto.AdTempName, _requestTemplateInfoDto.BaseMediaId,
                     (((int)AppTemplateEnum.待审核) + "," + ((int)AppTemplateEnum.已驳回)), _requestTemplateInfoDto.AdTempId);
                if (info != null)
                {
                    return CreateFailMessage(retValue, "70036", "您已经添加了该模板名称",
                        new
                        {
                            AdTemplateId = _requestTemplateInfoDto.AdTempId > 0 ? _requestTemplateInfoDto.AdTempId : info.RecID
                        ,
                            BaseMediaId = _requestTemplateInfoDto.BaseMediaId
                        });
                }
            }
            else if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                if (_configEntity.CreateUserId <= 0)
                    return CreateFailMessage(retValue, "70035", "媒体主需要校验用户id，此处用户并为获取到");
                //媒体主角色：名称+媒体id+已通过+用户id

                infoList = Dal.AdTemplate.AppAdTemplate.Instance.GetList(new AdTemplateQuery<AppAdTemplate>()
                {
                    TemplateName = _requestTemplateInfoDto.AdTempName,
                    PageSize = 1,
                    BaseMediaId = _requestTemplateInfoDto.BaseMediaId,
                    AuditStatusStr = (((int)AppTemplateEnum.待审核) + "," + ((int)AppTemplateEnum.已驳回)),
                    CreateUserId = _configEntity.CreateUserId,
                    FilterTemplateId = _requestTemplateInfoDto.AdTempId
                });
                if (infoList.Count > 0)
                {
                    return CreateFailMessage(retValue, "70035", "您已经添加了该模板名称",
                        new
                        {
                            AdTemplateId = _requestTemplateInfoDto.AdTempId > 0 ? _requestTemplateInfoDto.AdTempId : infoList.Select(s => s.RecID).FirstOrDefault(),
                            BaseMediaId = _requestTemplateInfoDto.BaseMediaId
                        });
                }
            }
            else
            {
                return CreateFailMessage(retValue, "70038", "当前角色？貌似不可以操作吧：" + _configEntity.RoleTypeEnum);
            }

            retValue.HasError = false;
            return retValue;
        }

        private ReturnValue VerifyInsertBaseAdTemplateIdByRole(ReturnValue retValue)
        {
            var templateId = _configEntity.CureOperateType == OperateType.Insert
                       ? -2
                       : _requestTemplateDto.TemplateId;
            if (_configEntity.RoleTypeEnum == RoleEnum.SupperAdmin || _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
                retValue.HasError = false;
                return retValue;
            }
            else if (_configEntity.RoleTypeEnum == RoleEnum.AE)
            {
                var info = Dal.AdTemplate.AppAdTemplate.Instance.VerifyOfBaseAdTemplateIdByRole(RoleInfoMapping.AE,
                      _requestTemplateDto.BaseAdId, _requestTemplateDto.BaseMediaId,
                      (((int)AppTemplateEnum.待审核) + "," + ((int)AppTemplateEnum.已驳回)), templateId);
                if (info != null)
                {
                    return CreateFailMessage(retValue, "70066", string.Format("您已经引用了此模板，请勿重复添加，公共模板id：{0},已存在的模板id:{1}",
                        _requestTemplateDto.BaseAdId, info.RecID));
                }
            }
            else if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                var infoList = Dal.AdTemplate.AppAdTemplate.Instance.GetList(new AdTemplateQuery<AppAdTemplate>()
                {
                    BaseAdId = _requestTemplateDto.BaseAdId,
                    PageSize = 1,
                    BaseMediaId = _requestTemplateDto.BaseMediaId,
                    AuditStatusStr = (((int)AppTemplateEnum.待审核) + "," + ((int)AppTemplateEnum.已驳回)),
                    CreateUserId = _configEntity.CreateUserId,
                    FilterTemplateId = templateId
                });

                if (infoList.Count > 0)
                {
                    var cuTemplateId = infoList.Select(s => s.RecID).FirstOrDefault();
                    return CreateFailMessage(retValue, "70067", string.Format("您已经引用了此模板，请勿重复添加，公共模板id：{0},已存在的模板id:{1}", _requestTemplateDto.BaseAdId, cuTemplateId));
                }
            }
            else
            {
                return CreateFailMessage(retValue, "70058", "当前角色？貌似不可以操作吧：" + _configEntity.RoleTypeEnum);
            }
            retValue.HasError = false;
            return retValue;
        }

        #endregion 查询详情相关

        #region 审核详情查询（区分左右2侧）

        public dynamic GetAuditViewList(bool leftParent)
        {
            /*
             场景一：首先判断带过来的adTempateId 是否存在BaseAdId,是套用的公共模板，存在就展示在页面，然后继续获取引用这个公共模板的列表
             场景二：如果页面是多个勾选过来的adTempateId，则展示这些id的列表
             */

            //以下是2个不同的场景，不同的接口进来的
            if (leftParent)
            {
                return GetLeftParentTemplateInfo();
            }
            else
            {
                var respTemplateList = new RespTemplateAuditDto<RespAdTemplateItemDto>();
                if (!string.IsNullOrWhiteSpace(_requestTemplateInfoDto.AdTempIdList))
                {
                    //找参数传过来的所有模板id
                    var tuple = GetRightTemplateList(new AdTemplateQuery<AppAdTemplate>()
                    {
                        AdTempIdList = _requestTemplateInfoDto.AdTempIdList,
                        PageSize = _requestTemplateInfoDto.PageSize,
                        PageIndex = _requestTemplateInfoDto.PageIndex,
                        CreateUserId = _configEntity.CreateUserId
                    });
                    respTemplateList.List = tuple.Item1;
                    respTemplateList.TotleCount = tuple.Item2;
                }
                else
                {
                    //找公共模板的所有引用
                    var tempInfo = BLL.AdTemplate.AppAdTemplate.Instance.GetInfoV1(_requestTemplateInfoDto.AdTempId, 0, 0, _configEntity.CreateUserId);
                    if (tempInfo == null) return null;

                    if (tempInfo.BaseAdID > 0)
                    {
                        //查询所有引用此公共模板的待审核模板信息，但是：当前模板id的信息要默认展示在第一个，
                        //所以，在查询的时候要过滤掉此模板信息，最好再插入到第一个
                        if (_requestTemplateInfoDto.PageIndex == 1)
                        {
                            _requestTemplateInfoDto.PageSize--;
                        }
                        var tuple = GetRightTemplateList(new AdTemplateQuery<AppAdTemplate>()
                        {
                            BaseAdId = tempInfo.BaseAdID,
                            BaseMediaId = _requestTemplateInfoDto.BaseMediaId,
                            FilterTemplateId = _requestTemplateInfoDto.AdTempId,//过滤当前模板id
                            PageSize = _requestTemplateInfoDto.PageSize,
                            PageIndex = _requestTemplateInfoDto.PageIndex,
                            CreateUserId = _configEntity.CreateUserId
                        });
                        if (_requestTemplateInfoDto.PageIndex == 1)
                            tuple.Item1.Insert(0, tempInfo);//添加到头部
                        respTemplateList.List = tuple.Item1;
                        respTemplateList.TotleCount = tuple.Item2;
                    }
                    else
                    {
                        //这种情况下，没有公共模板，只能显示一条数据
                        respTemplateList.List = new List<RespAdTemplateItemDto>() { tempInfo };
                        respTemplateList.TotleCount = respTemplateList.List.Count;
                    }
                }
                //因为头部要返回媒体信息
                if (_requestTemplateInfoDto.BaseMediaId > 0)
                {
                    var appInfo = Dal.Media.MediaBasePCAPP.Instance.GetEntity(_requestTemplateInfoDto.BaseMediaId);
                    if (appInfo != null)
                    {
                        respTemplateList.MediaInfo = new RespAdBaseDto
                        {
                            MediaID = _requestTemplateInfoDto.BaseMediaId,
                            HeadIconURL = appInfo.HeadIconURL,
                            Name = appInfo.Name
                        };
                    }
                }

                return respTemplateList;
            }
        }

        /// <summary>
        /// 公共模板
        /// </summary>
        /// <returns></returns>
        private RespAdTemplateItemDto GetLeftParentTemplateInfo()
        {
            if (_requestTemplateInfoDto.AdTempId <= 0) return null;
            var tempInfo = Dal.AdTemplate.AppAdTemplate.Instance.GetEntity(_requestTemplateInfoDto.AdTempId);
            if (tempInfo == null) return null;
            if (tempInfo.BaseAdID > 0)
            {
                //是套用的模板，正常
                //找到公共模板信息
                var baseTempInfo = BLL.AdTemplate.AppAdTemplate.Instance.GetInfoV1(tempInfo.BaseAdID, 0, 0, _configEntity.CreateUserId);

                if (baseTempInfo != null)
                {
                    if (baseTempInfo.AuditStatus != (int)Entities.Enum.AppTemplateEnum.已通过)
                    {
                        Loger.Log4Net.InfoFormat("GetLeftParentTemplateInfo此模板还没有审核通过，不能用于审核对比");
                        return null;
                    }
                    return baseTempInfo;
                }
            }
            return null;
        }

        private Tuple<List<RespAdTemplateItemDto>, int> GetRightTemplateList(AdTemplateQuery<AppAdTemplate> query)
        {
            query.AuditStatus = (int)AppTemplateEnum.待审核;
            var auditList = BLL.AdTemplate.AppAdTemplate.Instance.GetAuditInfoListV1(query);
            return new Tuple<List<RespAdTemplateItemDto>, int>(auditList, query.Total);
        }

        #endregion 审核详情查询（区分左右2侧）
    }

    public class RespTemplateAuditDto<T> : BaseResponseEntity
    {
        public List<T> List { get; set; }
        public RespAdBaseDto MediaInfo { get; set; }
    }
}