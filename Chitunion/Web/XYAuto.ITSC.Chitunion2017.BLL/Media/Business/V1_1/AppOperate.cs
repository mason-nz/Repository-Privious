/********************************************************
*创建人：lixiong
*创建时间：2017/6/5 10:16:45
*说明：app媒体操作相关类，错误码：70000 开始
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_4;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_1;
using XYAuto.ITSC.Chitunion2017.Dal.Media;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.AdTemplate;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query.AdTemplate;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1
{
    public class AppOperate : CurrentOperateBase, IMediaOperate
    {
        private readonly ConfigEntity _configEntity;
        private readonly RequestAppInfoDto _requestAppInfoDto;
        private readonly RequestAppDto _requestAppDto;

        private const int ResultCodeInsertQuft = 1002;//单独添加资质
        private string MeidaVerifyOfAddName { get; set; }

        public AppOperate(ConfigEntity configEntity, RequestAppDto requestAppDto)
        {
            _configEntity = configEntity;
            _requestAppDto = requestAppDto;
        }

        public AppOperate(ConfigEntity configEntity, RequestAppInfoDto requestAppInfoDto)
        {
            _configEntity = configEntity;
            _requestAppInfoDto = requestAppInfoDto;
        }

        public ReturnValue Excute()
        {
            if (_configEntity.CureOperateType == OperateType.Insert)
            {
                if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate ||
                  _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
                {
                    //添加主表信息
                    return DoInsertByAdmin();
                }
                return DoInsert();
            }
            else if (_configEntity.CureOperateType == OperateType.Edit)
            {
                if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate ||
                    _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
                {
                    //修改主表信息
                    return UpdateByAdmin();
                }
                return Update();
            }
            else
            {
                return CreateFailMessage(new ReturnValue(), "50031", "请确定是新增还是编辑操作");
            }
        }

        #region 添加app媒体操作

        private ReturnValue DoInsert()
        {
            var retValue = new ReturnValue();
            retValue = VerifyCreateBusiness(retValue);
            if (retValue.HasError)
                return retValue;
            var mediaId = BLL.Media.MediaPCAPP.Instance.Insert(SetDataByRole());
            if (mediaId <= 0)
            {
                return CreateFailMessage(retValue, "70005", "PcApp添加失败");
            }
            //处理AE角色逻辑
            retValue = InsertBaseAppRelation(retValue, mediaId);
            if (retValue.HasError)
                return retValue;

            //附表相关信息：常见分类等等
            TaskToRunMediaRelationInfo(retValue, mediaId);

            retValue.ReturnObject = mediaId;
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        private ReturnValue Update()
        {
            var retValue = new ReturnValue();
            retValue = VerifyUpdateBusiness(retValue);
            if (retValue.HasError)
                return retValue;

            var info = SetDataByRole();
            var exec = Dal.Media.MediaPCAPP.Instance.Update(info);
            if (exec <= 0)
                return CreateFailMessage(retValue, "70011", "编辑失败");

            //处理AE角色逻辑
            retValue = InsertBaseAppRelation(retValue, info.MediaID);
            if (retValue.HasError)
                return retValue;

            //附表相关信息：常见分类等等
            TaskToRunMediaRelationInfo(retValue, info.MediaID);

            retValue.ReturnObject = info.MediaID;
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        private ReturnValue DoInsertByAdmin()
        {
            /*
                admin:只是操作基表相关数据
             */
            var retValue = new ReturnValue();
            retValue = VerifyBusinessByAdmin(retValue);
            if (retValue.HasError)
                return retValue;

            var excInfo = AutoMapper.Mapper.Map<RequestAppDto, Entities.Media.MediaBasePCAPP>(_requestAppDto);
            //添加入库
            var baseAppId = Dal.Media.MediaBasePCAPP.Instance.Insert(excInfo);
            if (baseAppId <= 0)
                return CreateFailMessage(retValue, "70013", "添加失败");

            //附表相关信息：常见分类等等
            TaskToRunBaseMediaRelationInfo(retValue, baseAppId);

            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        private ReturnValue UpdateByAdmin()
        {
            /*
                admin:只是操作基表相关数据
             */
            if (_requestAppDto.BaseMediaID > 0)
                _requestAppDto.MediaID = _requestAppDto.BaseMediaID;
            var retValue = new ReturnValue();
            retValue = VerifyBusinessByAdmin(retValue);
            if (retValue.HasError)
                return retValue;

            var excInfo = AutoMapper.Mapper.Map<RequestAppDto, Entities.Media.MediaBasePCAPP>(_requestAppDto);
            var exec = Dal.Media.MediaBasePCAPP.Instance.Update(excInfo);
            if (exec <= 0)
                return CreateFailMessage(retValue, "70014", "编辑失败");

            //附表相关信息：常见分类等等
            TaskToRunBaseMediaRelationInfo(retValue, excInfo.RecID);

            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        private Entities.Media.MediaPcApp SetDataByRole()
        {
            var info = AutoMapper.Mapper.Map<RequestAppDto, Entities.Media.MediaPcApp>(_requestAppDto);
            if (_configEntity.RoleTypeEnum == RoleEnum.AE)
            {
                if (info != null)
                {
                    info.AuditStatus = (int)MediaAuditStatusEnum.AlreadyPassed;
                }
            }
            else
            {
                if (info != null)
                {
                    info.AuditStatus = (int)MediaAuditStatusEnum.PendingAudit;
                }
            }
            return info;
        }

        #endregion 添加app媒体操作

        #region 添加app基表操作

        private ReturnValue InsertBaseAppRelation(ReturnValue retValue, int mediaId)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                /*
                    媒体主添加：如果基表中存在，在添加页面渲染的内容是基表的数据，So,不用处理
                    1.但是媒体主有资质信息
                */
            }

            if (_configEntity.RoleTypeEnum == RoleEnum.AE)
            {
                //AE直接是审核通过状态
                var baseAppId = 0;
                var baseInfo = Dal.Media.MediaBasePCAPP.Instance.GetEntity(_requestAppDto.Name);
                var excInfo = AutoMapper.Mapper.Map<RequestAppDto, Entities.Media.MediaBasePCAPP>(_requestAppDto);
                if (baseInfo == null)
                {
                    #region 添加入库

                    //添加入库
                    baseAppId = Dal.Media.MediaBasePCAPP.Instance.Insert(excInfo);
                    if (baseAppId > 0)
                    {
                        //回写到app附表中的BaseMediaId
                        if (Dal.Media.MediaPCAPP.Instance.UpdateBaseMediaId(mediaId, baseAppId) == 0)
                        {
                            var errorMsg = string.Format("添加入库-回写到app附表中的BaseMediaId出错，mediaId={0}&baseMediaId={1}", mediaId, baseAppId);
                            Loger.Log4Net.ErrorFormat(errorMsg);
                            return CreateFailMessage(retValue, "70006", errorMsg);
                        }
                    }

                    #endregion 添加入库
                }
                else
                {
                    #region 编辑入库

                    //编辑入库（基表中存在数据，且附表中存在关联,则不更新基表数据,因为AE角色默认是已通过审核的，用不到副表数据）
                    //if (_requestAppDto.BaseMediaID > 0)
                    //{
                    //    //如果参数中的BaseMediaID存在，则证明基表中
                    //    return retValue;
                    //}

                    baseAppId = baseInfo.RecID;
                    Dal.Media.MediaBasePCAPP.Instance.Update(excInfo);
                    //回写到app附表中的BaseMediaId
                    if (Dal.Media.MediaPCAPP.Instance.UpdateBaseMediaId(mediaId, baseInfo.RecID) == 0)
                    {
                        var errorMsg = string.Format("编辑入库-回写到app附表中的BaseMediaId出错，mediaId={0}&baseMediaId={1}", mediaId,
                            baseInfo.RecID);
                        Loger.Log4Net.ErrorFormat(errorMsg);
                        return CreateFailMessage(retValue, "70007", errorMsg);
                    }

                    #endregion 编辑入库
                }
                if (baseAppId > 0)//基表相关的常见分类
                    TaskToRunBaseMediaRelationInfo(retValue, baseAppId);
            }
            return retValue;
        }

        #endregion 添加app基表操作

        #region 添加app媒体相关校验

        public ReturnValue VerifyCreateBusiness(ReturnValue retValue)
        {
            //基础参数校验
            retValue = VerifyOfNecessaryParameters(_requestAppDto);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyBaseAppByName(retValue, _requestAppDto.Name);
            var resultCode = retValue.ReturnObject as VerfiyAppResultCode;
            if (resultCode != null && resultCode.ResultCode != ResultCodeInsertQuft)
            {
                //如果是不等于1002、就代表是资质和媒体一起添加
                //资质参数校验
                retValue = VerifyMediaQualificationParams(retValue);
                if (retValue.HasError)
                    return retValue;
            }
            if (resultCode != null && resultCode.ResultCode == ResultCodeInsertQuft)
            {
                //代表基表存在，则需要把基表id写到附表中
                if (resultCode.BaseMediaId > 0)
                    _requestAppDto.BaseMediaID = resultCode.BaseMediaId;
            }
            retValue.Message = string.Empty;

            //添加角色校验
            retValue = VerifyInsertOperateAppNameByRole(retValue);
            if (retValue.HasError)
                return retValue;

            //常见分类参数校验
            retValue = VerifycategoryIDsParams(retValue);
            if (retValue.HasError)
                return retValue;

            return retValue;
        }

        public ReturnValue VerifyUpdateBusiness(ReturnValue retValue)
        {
            retValue = VerifyOfNecessaryParameters(_requestAppDto);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyMediaQualificationParams(retValue);
            if (retValue.HasError)
                return retValue;

            retValue = VerifycategoryIDsParams(retValue);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyUpdateBeforeInfo(retValue);
            if (retValue.HasError)
                return retValue;
            object info = retValue.ReturnObject;

            retValue.ReturnObject = info;
            return retValue;
        }

        private ReturnValue VerifyBusinessByAdmin(ReturnValue retValue)
        {
            /* admin 只操作基表相关数据  */
            retValue = VerifyOfNecessaryParameters(_requestAppDto);
            if (retValue.HasError)
                return retValue;

            //校验名称
            retValue = VerifyOfNameInsertByAdmin(retValue);
            if (retValue.HasError)
                return retValue;

            retValue = VerifycategoryIDsParams(retValue);
            if (retValue.HasError)
                return retValue;

            if (_configEntity.CureOperateType == OperateType.Edit)
            {
                retValue = VerifyUpdateBeforeBaseInfo(retValue);
                if (retValue.HasError)
                    return retValue;
            }

            object info = retValue.ReturnObject;
            retValue.ReturnObject = info;
            return retValue;
        }

        private ReturnValue VerifyMediaQualificationParams(ReturnValue retValue)
        {
            if (_configEntity.RoleTypeEnum != RoleEnum.MediaOwner)
            {
                return retValue;
            }
            if (_requestAppDto.Qualification == null)
            {
                return CreateFailMessage(retValue, "70016", "请输入资质相关信息");
            }
            var qualificationInfo = _requestAppDto.Qualification;
            if (qualificationInfo.MediaRelations == (int)MediaRelationsEnum.Proxy
                && qualificationInfo.OperatingType == (int)UserTypeEnum.企业)
            {
                #region 代理+企业

                //代理+企业：需填写的资质内容有公司名称、营业执照、代理合同扫描件（首页、尾页）
                if (string.IsNullOrWhiteSpace(qualificationInfo.EnterpriseName)
                    || string.IsNullOrWhiteSpace(qualificationInfo.BusinessLicense))
                {
                    return CreateFailMessage(retValue, "70017", "代理+企业：需填写的资质内容有公司名称、营业执照");
                }

                if (string.IsNullOrWhiteSpace(qualificationInfo.AgentContractBackURL)
                    || string.IsNullOrWhiteSpace(qualificationInfo.AgentContractFrontURL))
                {
                    return CreateFailMessage(retValue, "70018", "代理+企业：需填写代理合同扫描件（首页、尾页）");
                }

                #endregion 代理+企业
            }
            if (qualificationInfo.MediaRelations == (int)MediaRelationsEnum.Own)
            {
                #region 自有+个人,自有+企业

                if (qualificationInfo.OperatingType == (int)UserTypeEnum.个人)
                {
                    if (string.IsNullOrWhiteSpace(qualificationInfo.EnterpriseName))
                    {
                        return CreateFailMessage(retValue, "70019", "自有+个人：需填写的资质内容有真实姓名");
                    }
                    if (string.IsNullOrWhiteSpace(qualificationInfo.IDCardFrontURL))
                    {
                        return CreateFailMessage(retValue, "70018", "自有+个人：身份证扫描件（正面或者反面）");
                    }
                }
                if (qualificationInfo.OperatingType == (int)UserTypeEnum.企业)
                {
                    if (string.IsNullOrWhiteSpace(qualificationInfo.EnterpriseName)
                        || string.IsNullOrWhiteSpace(qualificationInfo.BusinessLicense))
                    {
                        return CreateFailMessage(retValue, "70019", "自有+企业：需填写的资质内容有公司名称、营业执照扫描件");
                    }
                }

                #endregion 自有+个人,自有+企业
            }
            return retValue;
        }

        /// <summary>
        /// 添加操作：校验附表app名称的逻辑，媒体主角色：到个人、AE角色：到角色
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyInsertOperateAppNameByRole(ReturnValue retValue)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.AE)
            {
                //AE角色
                var info = Dal.Media.MediaPCAPP.Instance.GetInfoByRoleAe(_requestAppDto.Name, RoleInfoMapping.AE);
                if (info != null)
                {
                    //同一角色下已存在相同名称，不让添加
                    return CreateFailMessage(retValue, "70001", "当前AE角色下已存在相同的app名称，不允许重复添加");
                }
            }
            else //(_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                //媒体主角色
                var info = Dal.Media.MediaPCAPP.Instance.GetEntity(_requestAppDto.Name, 0,
                    _configEntity.CreateUserId);
                if (info != null)
                {
                    //同一个人已存在相同名称，不让添加
                    return CreateFailMessage(retValue, "70002", "当前媒体主已存在相同的app名称，不允许重复添加");
                }
            }
            return retValue;
        }

        private ReturnValue VerifyUpdateBeforeInfo(ReturnValue retValue)
        {
            var info = Dal.Media.MediaPCAPP.Instance.GetEntity(_requestAppDto.MediaID);
            if (info == null)
            {
                return CreateFailMessage(retValue, "70008", string.Format("当前媒体信息不存在Id:{0}", _requestAppDto.MediaID));
            }
            if (info.Name != _requestAppDto.Name)
            {
                return CreateFailMessage(retValue, "70009", string.Format("app名称不允许修改"));
            }
            if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                if (info.AuditStatus != (int)MediaAuditStatusEnum.RejectNotPass)
                {
                    return CreateFailMessage(retValue, "70010", string.Format("当前媒体审核状态不可编辑，状态为：{0}", (MediaAuditStatusEnum)info.AuditStatus));
                }
            }
            retValue.ReturnObject = info;
            return retValue;
        }

        private ReturnValue VerifyUpdateBeforeBaseInfo(ReturnValue retValue)
        {
            var info = Dal.Media.MediaBasePCAPP.Instance.GetEntity(_requestAppDto.MediaID);
            if (info == null)
            {
                return CreateFailMessage(retValue, "70012", string.Format("当前媒体信息不存在Id:{0}", _requestAppDto.MediaID));
            }
            //if (info.Name != _requestAppDto.Name)
            //{
            //    return CreateFailMessage(retValue, "70013", string.Format("app名称不允许修改"));
            //}

            retValue.ReturnObject = info;
            return retValue;
        }

        private ReturnValue VerifyOfNameInsertByAdmin(ReturnValue retValue)
        {
            var filterId = _configEntity.CureOperateType == OperateType.Insert ? -2 : _requestAppDto.MediaID;
            var baseInfo = Dal.Media.MediaBasePCAPP.Instance.GetEntity(_requestAppDto.Name, filterId);
            if (baseInfo != null)
            {
                return CreateFailMessage(retValue, "70052", string.Format("当前媒体名称已存在基表中 ，不允许重复添加，name:{0}", _requestAppDto.Name));
            }
            return retValue;
        }

        /// <summary>
        /// 校验常见分类参数
        /// 1.不允许添加超过5个
        /// 2.必须有一个主分类
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifycategoryIDsParams(ReturnValue retValue)
        {
            if (_requestAppDto.CommonlyClass.Count > 5 || _requestAppDto.CommonlyClass.Count < 1)
            {
                return CreateFailMessage(retValue, "70003", "常见分类数量必须是1-5个");
            }

            if (_requestAppDto.CommonlyClass.All(s => s.SortNumber != WeiXinOperate.CategoryIdWeight))
            {
                return CreateFailMessage(retValue, "70004", "常见分类必须选择一个重要分类");
            }
            return retValue;
        }

        /// <summary>
        /// 校验覆盖区域参数
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyCoverageAreaParams(ReturnValue retValue)
        {
            return retValue;
        }

        /// <summary>
        /// 校验操作权限
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        public ReturnValue VerifyOfRoleModule(ReturnValue retValue)
        {
            var moduleId = new Dictionary<int, string>()
                {
                    {1,"SYS001BUT400401"},{2,"SYS001BUT400402"}
                }.FirstOrDefault(s => s.Key == (int)_configEntity.CureOperateType).Value;
            return base.VerifyOfRoleModule(retValue, moduleId);
        }

        #endregion 添加app媒体相关校验

        #region 添加资质信息操作

        /// <summary>
        /// 添加资质操作方法（媒体主）
        /// 存在这种场景的原因
        /// 1.app名称附表中没有，基表有的情况
        /// 2.添加资质之后，还要把基表的数据同步到附表，基表媒体id存在附表中做关联信息
        /// </summary>
        /// <returns></returns>
        public ReturnValue DoQualificationExcute()
        {
            var retValue = new ReturnValue();

            retValue = VerifyDoQualificationExcute(retValue);
            if (retValue.HasError)
                return retValue;

            var baseInfo = retValue.ReturnObject as Entities.Media.MediaBasePCAPP;
            if (baseInfo == null)
                return CreateFailMessage(retValue, "70024", "拆箱失败");
            //todo:
            //1.把基表数据同步到附表
            retValue = InsertAppRelation(retValue, baseInfo);
            if (retValue.HasError)
                return retValue;

            var mediaId = Convert.ToInt32(retValue.GetValue("mediaId"));

            if (mediaId <= 0)
            {
                return CreateFailMessage(retValue, "70023", "GetValue(mediaId)失败");
            }

            //异步处理同步数据
            TaskToRunDoQualification(retValue, baseInfo.RecID, mediaId);

            return retValue;
        }

        private ReturnValue VerifyDoQualificationExcute(ReturnValue retValue)
        {
            if (_requestAppDto.BaseMediaID <= 0)
            {
                return CreateFailMessage(retValue, "70021", "基表baseMediaId不存在");
            }
            _requestAppDto.MediaID = _requestAppDto.BaseMediaID;

            //资质参数校验
            retValue = VerifyMediaQualificationParams(retValue);
            if (retValue.HasError)
                return retValue;

            //校验基表id
            retValue = VerifyUpdateBeforeBaseInfo(retValue);
            if (retValue.HasError)
                return retValue;

            return retValue;
        }

        private void TaskToRunDoQualification(ReturnValue retValue, int baseMediaId, int mediaId)
        {
            Task.Factory.StartNew(() =>
            {
                //2.同步相关分类信息
                CopyBaseMediaToAttachTable(retValue, new CopyBaseMediaToAttach()
                {
                    MediaType = MediaType.APP,
                    BaseMediaId = baseMediaId,
                    MediaId = mediaId,
                    CreateUserId = _configEntity.CreateUserId
                });
            })
            .ContinueWith(s =>
            {
                //资质信息
                try
                {
                    MediaQualificationInsert(retValue, mediaId);
                }
                catch (Exception exception)
                {
                    Loger.Log4Net.ErrorFormat("app Task to TaskToRunDoQualification is Exception:{0}",
                        exception.Message + (exception.StackTrace ?? string.Empty));
                }
            });

            retValue.HasError = false;
            retValue.Message = string.Empty;
        }

        /// <summary>
        /// 基表数据同步到附表中
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="copyBaseMediaToAttach"></param>
        /// <returns></returns>
        private ReturnValue CopyBaseMediaToAttachTable(ReturnValue retValue, CopyBaseMediaToAttach copyBaseMediaToAttach)
        {
            try
            {
                Dal.Media.MediaAreaMapping.Instance.CopyBaseMediaToAttachTable(copyBaseMediaToAttach, 45003);
            }
            catch (Exception exception)
            {
                retValue.HasError = true;
                Loger.Log4Net.ErrorFormat("app CopyBaseMediaToAttachTable is error:{0},参数:{1}",
                    exception.Message + (exception.StackTrace ?? string.Empty),
                    JsonConvert.SerializeObject(copyBaseMediaToAttach));
            }
            return retValue;
        }

        /// <summary>
        /// 媒体主添加资质信息，同步基表数据到附表（待审核、基表id存储关联）
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="baseInfo"></param>
        /// <returns></returns>
        private ReturnValue InsertAppRelation(ReturnValue retValue, Entities.Media.MediaBasePCAPP baseInfo)
        {
            var info = Dal.Media.MediaPCAPP.Instance.GetEntity(baseInfo.Name, 0, _configEntity.CreateUserId);
            int mediaId = 0;
            if (info == null)
            {
                //附表正常流程是没有数据

                var infoInsert = AutoMapper.Mapper.Map<Entities.Media.MediaBasePCAPP, Entities.Media.MediaPcApp>(baseInfo);
                mediaId = BLL.Media.MediaPCAPP.Instance.Insert(infoInsert);
                if (mediaId <= 0)
                {
                    Loger.Log4Net.InfoFormat("媒体主添加资质信息，同步基表数据到附表--附表没有数据--添加失败:{0}", JsonConvert.SerializeObject(infoInsert));
                    //return CreateFailMessage()
                }
            }
            else
            {
                var infoUpdate = AutoMapper.Mapper.Map<Entities.Media.MediaBasePCAPP, Entities.Media.MediaPcApp>(baseInfo);
                mediaId = Dal.Media.MediaPCAPP.Instance.Update(infoUpdate);
                if (mediaId <= 0)
                    Loger.Log4Net.InfoFormat("媒体主添加资质信息，同步基表数据到附表--附表存在数据--编辑失败:{0}", JsonConvert.SerializeObject(infoUpdate));
            }

            retValue.HasError = mediaId <= 0;
            retValue.PutValue("mediaId", mediaId);
            return retValue;
        }

        #endregion 添加资质信息操作

        #region Task异步操作

        /// <summary>
        /// app副表：常见分类、覆盖区域、下单备注相关信息维护
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="mediaId">附表媒体id</param>
        private void TaskToRunMediaRelationInfo(ReturnValue retValue, int mediaId)
        {
            //常见分类
            try
            {
                MediaCommonlyClassInsertByBulk(retValue, mediaId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("app Task to MediaCommonlyClassInsertByBulk is Exception:{0}",
                    exception.Message + (exception.StackTrace ?? string.Empty));
            }
            //覆盖区域
            try
            {
                MediaAreaMappingInsertByBulk(retValue, mediaId, MediaRelationType.Attached);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("app Task to MediaAreaMappingInsertByBulk is Exception:{0}",
                    exception.Message + (exception.StackTrace ?? string.Empty));
            }

            //消息提示
            try
            {
                AuditMessageOperate(retValue, mediaId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("app Task to AuditMessageOperate is Exception:{0}",
                    exception.Message + (exception.StackTrace ?? string.Empty));
            }

            //下单备注
            try
            {
                MediaOrderRemarkInsertByBulk(retValue, mediaId, MediaRelationType.Attached);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("app Task to MediaOrderRemarkInsertByBulk is Exception:{0}",
                    exception.Message + (exception.StackTrace ?? string.Empty));
            }
            //资质
            try
            {
                MediaQualificationInsert(retValue, mediaId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("app Task to MediaQualificationInsert is Exception:{0}",
                    exception.Message + (exception.StackTrace ?? string.Empty));
            }

            try
            {
                UploadFileManage(mediaId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("Media_PCAPP UploadFileManage is exception:{0}{1}",
                    System.Environment.NewLine, exception + (exception.StackTrace ?? string.Empty));
            }

            retValue.HasError = false;
            retValue.Message = string.Empty;

            #region MyRegion

            //Task.Factory.StartNew(() =>
            //{
            //    //常见分类
            //    try
            //    {
            //        MediaCommonlyClassInsertByBulk(retValue, mediaId);
            //    }
            //    catch (Exception exception)
            //    {
            //        Loger.Log4Net.ErrorFormat("app Task to MediaCommonlyClassInsertByBulk is Exception:{0}",
            //            exception.Message + (exception.StackTrace ?? string.Empty));
            //    }
            //})
            //.ContinueWith(s =>
            //{
            //    //覆盖区域
            //    try
            //    {
            //        MediaAreaMappingInsertByBulk(retValue, mediaId, MediaRelationType.Attached);
            //    }
            //    catch (Exception exception)
            //    {
            //        Loger.Log4Net.ErrorFormat("app Task to MediaAreaMappingInsertByBulk is Exception:{0}",
            //            exception.Message + (exception.StackTrace ?? string.Empty));
            //    }
            //})
            //.ContinueWith(s =>
            //{
            //    //消息提示
            //    try
            //    {
            //        MediaAuditInfoInsert(_configEntity.CureOperateType, mediaId, _requestAppDto.Name, _configEntity.CreateUserId);
            //    }
            //    catch (Exception ex)
            //    {
            //        Loger.Log4Net.ErrorFormat("app Task to MediaAuditInfoInsert is Exception:{0}", ex.Message);
            //    }
            //})
            //.ContinueWith(s =>
            //{
            //    //下单备注
            //    try
            //    {
            //        MediaOrderRemarkInsertByBulk(retValue, mediaId, MediaRelationType.Attached);
            //    }
            //    catch (Exception exception)
            //    {
            //        Loger.Log4Net.ErrorFormat("app Task to MediaOrderRemarkInsertByBulk is Exception:{0}", exception.Message);
            //    }
            //})
            //.ContinueWith(s =>
            //{
            //    try
            //    {
            //        MediaQualificationInsert(retValue);
            //    }
            //    catch (Exception exception)
            //    {
            //        Loger.Log4Net.ErrorFormat("app Task to MediaQualificationInsert is Exception:{0}", exception.Message);
            //    }
            //})
            ;

            #endregion MyRegion
        }

        /// <summary>
        /// app基表：常见分类、覆盖区域、下单备注相关信息维护
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="baseMediaId">附表媒体id</param>
        private void TaskToRunBaseMediaRelationInfo(ReturnValue retValue, int baseMediaId)
        {
            //常见分类
            try
            {
                MediaBaseCommonlyClassInsertByBulk(retValue, baseMediaId);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("app Task to MediaBaseCommonlyClassInsertByBulk is Exception:{0}",
                    exception.Message + (exception.StackTrace ?? string.Empty));
            }
            //覆盖区域
            try
            {
                MediaAreaMappingInsertByBulk(retValue, baseMediaId, MediaRelationType.BaseTable);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("app Task to MediaAreaMappingInsertByBulk is Exception:{0}",
                    exception.Message + (exception.StackTrace ?? string.Empty));
            }
            //下单备注
            try
            {
                MediaOrderRemarkInsertByBulk(retValue, baseMediaId, MediaRelationType.BaseTable);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("app Task to MediaOrderRemarkInsertByBulk is Exception:{0}",
                    exception.Message + (exception.StackTrace ?? string.Empty));
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;

            #region 异步

            //Task.Factory.StartNew(() =>
            //{
            //    //常见分类
            //    try
            //    {
            //        MediaBaseCommonlyClassInsertByBulk(retValue, baseMediaId);
            //    }
            //    catch (Exception exception)
            //    {
            //        Loger.Log4Net.ErrorFormat("app Task to MediaBaseCommonlyClassInsertByBulk is Exception:{0}", exception.Message);
            //    }
            //})
            //.ContinueWith(s =>
            //{
            //    //覆盖区域
            //    try
            //    {
            //        MediaAreaMappingInsertByBulk(retValue, baseMediaId, MediaRelationType.BaseTable);
            //    }
            //    catch (Exception ex)
            //    {
            //        Loger.Log4Net.ErrorFormat("app Task to MediaAreaMappingInsertByBulk is Exception:{0}", ex.Message);
            //    }
            //})
            //.ContinueWith(s =>
            // {
            //     //下单备注
            //     try
            //     {
            //         MediaOrderRemarkInsertByBulk(retValue, baseMediaId, MediaRelationType.BaseTable);
            //     }
            //     catch (Exception exception)
            //     {
            //         Loger.Log4Net.ErrorFormat("app Task to MediaOrderRemarkInsertByBulk is Exception:{0}", exception.Message);
            //     }
            // })
            //.ContinueWith(s =>
            //{
            //    //文件关联
            //});

            #endregion 异步
        }

        private void UploadFileManage(int mediaId)
        {
            var urlList = new List<string>()
            {
                _requestAppDto.HeadIconURL
            };
            var retValue = BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(urlList, _configEntity.CreateUserId,
                   UploadFileEnum.MediaManage, mediaId, "Media_PCAPP");
            if (retValue.HasError)
            {
                Loger.Log4Net.ErrorFormat("Media_PCAPP UploadFileManage is error:{0}{1}",
               System.Environment.NewLine, retValue.Message);
            }
        }

        /// <summary>
        /// app媒体资质（媒体主角色才会有）
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public ReturnValue MediaQualificationInsert(ReturnValue retValue, int mediaId)
        {
            if (_configEntity.RoleTypeEnum != RoleEnum.MediaOwner)
            {
                Loger.Log4Net.InfoFormat("app MediaQualificationInsert 角色不是媒体主，不执行下一步，跳出");
                return retValue;
            }

            var mediaQualification = GetMediaQualificationInfo(mediaId);
            if (mediaQualification == null)
            {
                var errorMsg = string.Format("app MediaQualificationInsert GetMediaQualificationInfo is error. is null.");
                Loger.Log4Net.ErrorFormat(errorMsg);
                return CreateFailMessage(retValue, "70020", errorMsg);
            }

            retValue = BLL.Media.MediaQualification.Instance.Update(mediaQualification, MediaType.APP);

            if (retValue.HasError)
            {
                Loger.Log4Net.ErrorFormat("app MediaQualificationInsert update is error,{0}", JsonConvert.SerializeObject(retValue));
            }

            return retValue;
        }

        private Entities.Media.MediaQualification GetMediaQualificationInfo(int mediaId)
        {
            var info = AutoMapper.Mapper.Map<RequestQualificationDto, Entities.Media.MediaQualification>(_requestAppDto.Qualification);

            if (info == null)
            {
                return null;
            }
            info.MediaID = mediaId;
            info.MediaType = (int)MediaType.APP;
            info.CreateUserID = _configEntity.CreateUserId;
            return info;
        }

        /// <summary>
        /// 审核信息入库（待审核的消息表）
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="mediaId"></param>
        /// <param name="mediaAudit"></param>
        /// <returns></returns>
        private ReturnValue AuditMessageOperate(ReturnValue retValue, int mediaId, OperateAuditMsgOptType mediaAudit = OperateAuditMsgOptType.AuditMediaAppending)
        {
            OperateAuditMsg.Instance.OperateAuditMsgInsert(mediaId, (int)OperateAuditMsgType.AuditMedia,
                (int)mediaAudit, _configEntity.CreateUserId);
            return retValue;
        }

        /// <summary>
        /// app媒体附表常见分类操作
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="mediaId">副表媒体id</param>
        /// <returns></returns>
        private ReturnValue MediaCommonlyClassInsertByBulk(ReturnValue retValue, int mediaId)
        {
            var categoryIDs = _requestAppDto.CommonlyClass.ToDictionary(s => s.CategoryId, s => s.SortNumber);
            retValue = new Business.V1_1.WeiXinOperate(new RequestWeiXinDto() { MediaID = mediaId }, _configEntity)
                     .MediaCommonlyClassInsertByBulk(categoryIDs, retValue);
            if (retValue.HasError)
            {
                var msg = string.Format("app MediaCommonlyClassInsertByBulk is error:{0}", retValue.Message);
                Loger.Log4Net.ErrorFormat(msg);
                return CreateFailMessage(retValue, "70015", msg);
            }
            return retValue;
        }

        /// <summary>
        /// app媒体基表常见分类操作
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="baseMediaId">基表媒体id</param>
        /// <returns></returns>
        private ReturnValue MediaBaseCommonlyClassInsertByBulk(ReturnValue retValue, int baseMediaId)
        {
            var categoryIDs = _requestAppDto.CommonlyClass.ToDictionary(s => s.CategoryId, s => s.SortNumber);
            retValue = new Business.V1_1.WeiXinOperate(new RequestWeiXinDto() { MediaID = baseMediaId }, _configEntity)
                    .MediaCategoryInsertByBulk(categoryIDs, retValue, baseMediaId);
            if (retValue.HasError)
            {
                var msg = string.Format("app MediaBaseCommonlyClassInsertByBulk is error：{0}", retValue.Message);
                Loger.Log4Net.ErrorFormat(msg);
                return CreateFailMessage(retValue, "70015", msg);
            }
            return retValue;
        }

        /// <summary>
        /// app 媒体附表（基表）下单备注
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="mediaId">附表媒体id/基表id</param>
        /// <param name="mediaRelationType">操作基表 or 附表</param>
        /// <returns></returns>
        public ReturnValue MediaOrderRemarkInsertByBulk(ReturnValue retValue, int mediaId, MediaRelationType mediaRelationType)
        {
            string errorMsg;
            if (_requestAppDto.OrderRemark == null || _requestAppDto.OrderRemark.Count == 0)
            {
                var msg = string.Format("app {0} MediaOrderRemarkInsertByBulk OrderRemark 为null，删除", mediaRelationType);
                Loger.Log4Net.Info(msg);
                Dal.Media.MediaPCAPP.Instance.DelereOrderRemark(mediaId, 45003, mediaRelationType);
                return CreateFailMessage(retValue, "70015", msg);
            }
            var otherRemark = _requestAppDto.OrderRemark.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.Descript));

            var exec = MediaCommonInfo.Instance.InsertMediaRemark(mediaRelationType, 45003, mediaId,
                 _requestAppDto.OrderRemark.Select(s => s.Id).ToList(),
                 otherRemark == null ? string.Empty : otherRemark.Descript
                 , out errorMsg);
            if (exec <= 0)
            {
                var msg = string.Format("app {0} MediaOrderRemarkInsertByBulk is error:{1}", mediaRelationType, errorMsg);
                Loger.Log4Net.ErrorFormat(msg);
                return CreateFailMessage(retValue, "70015", msg);
            }
            return retValue;
        }

        private ReturnValue MediaAreaMappingInsertByBulk(ReturnValue retValue, int mediaId, MediaRelationType mediaRelationType)
        {
            retValue = BLL.Media.MediaAreaMapping.Instance.InsertByBulk(new AreaMapping()
            {
                CoverageArea = _requestAppDto.CoverageArea,
                CreateUserId = _configEntity.CreateUserId,
                MediaType = MediaType.APP,
                AreaMappingType = MediaAreaMappingType.CoverageArea,
                MediaId = mediaId
            }, mediaRelationType);
            if (retValue.HasError)
            {
                var msg = string.Format("app MediaAreaMappingInsertByBulk is error，错误信息：{0}", retValue.Message);
                Loger.Log4Net.ErrorFormat(msg);
                return CreateFailMessage(retValue, "70015", msg);
            }
            return retValue;
        }

        #endregion Task异步操作

        #region 媒体主、AE 添加媒体入口校验

        public ReturnValue VerifyOfAdd(string mediaName)
        {
            MeidaVerifyOfAddName = mediaName;
            return VerifyOfAddByRole(new ReturnValue());
        }

        /// <summary>
        /// 校验当前媒体下是否有已审核通过的模板或自己添加的未通过的模板
        /// </summary>
        /// <param name="mediaId"></param>
        /// <param name="userId"></param>
        /// <param name="mediaName"></param>
        /// <returns></returns>
        public ReturnValue VerfiyOfAppTemplate(int mediaId, int userId, string mediaName)
        {
            var retValue = new ReturnValue();
            int baseMediaId = 0;
            if (!string.IsNullOrWhiteSpace(mediaName))
            {
                var baseInfo = Dal.Media.MediaBasePCAPP.Instance.GetEntity(mediaName);
                if (baseInfo == null)
                    return CreateFailMessage(retValue, "70051", "当前基表媒体信息不存在，mediaName=" + mediaName);
                baseMediaId = baseInfo.RecID;
            }
            else
            {
                if (mediaId <= 0)
                    return CreateFailMessage(retValue, "70040", "请输入MediaId");
                var info = Dal.Media.MediaPCAPP.Instance.GetEntity(mediaId);
                if (info == null)
                    return CreateFailMessage(retValue, "70041", "当前媒体信息不存在，MediaId=" + mediaId);
                if (info.AuditStatus != (int)Entities.Enum.MediaAuditStatusEnum.AlreadyPassed)
                    return CreateFailMessage(retValue, "70042", "当前媒体审核状态不是审核已通过状态，不能添加广告或者模板");

                if (info.BaseMediaID <= 0)
                    return CreateFailMessage(retValue, "70043", "当前媒体下没有基表媒体id");
                baseMediaId = info.BaseMediaID;
            }
            return VerifyOfMediaIsTemplate(retValue, baseMediaId, userId);
        }

        private ReturnValue VerifyOfMediaIsTemplate(ReturnValue retValue, int baseMediaId, int userId)
        {
            var info = Dal.Media.MediaBasePCAPP.Instance.GetInfo(baseMediaId, userId);
            if (info == null)
                return CreateFailMessage(retValue, "70044", "当前媒体信息对应的基表信息不存在，baseMediaId=" + baseMediaId);
            if (info.AdTemplateId > 0)
            {
                //则存在/当前媒体下是否有已审核通过的模板或自己添加的未通过的模板
                return CreateSuccessMessage(retValue, "", "存在已审核通过的模板或自己添加的未通过的模板，不可添加模板",
                    new { AdTemplateId = info.AdTemplateId, BaseMediaId = baseMediaId });
            }
            return CreateSuccessMessage(retValue, "", "当前媒体下没有模板信息,可添加模板", new { AdTemplateId = 0, BaseMediaId = baseMediaId });
        }

        private ReturnValue VerifyOfAddByRole(ReturnValue retValue)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                //媒体主
                return VerifyOfAddByRoleMediaOwner(retValue);
            }
            else if (_configEntity.RoleTypeEnum == RoleEnum.AE)
            {
                //AE
                return VerifyOfAddByRoleAe(retValue);
            }
            else
            {
                return CreateFailMessage(retValue, "70000", "当前角色不走此通道（媒体主、AE角色走此通道）");
            }
        }

        /// <summary>
        /// 校验是否可以添加媒体，角色：媒体主
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyOfAddByRoleMediaOwner(ReturnValue retValue)
        {
            var info = Dal.Media.MediaPCAPP.Instance.GetEntity(MeidaVerifyOfAddName, 0, _configEntity.CreateUserId);
            if (info != null)
            {
                //附表存在的情况下，需要找模板信息
                //if (info.AuditStatus == (int)MediaAuditStatusEnum.AlreadyPassed)
                //{
                //    //模板是和审核通过的媒体联系上的
                //    var temp = Dal.AdTemplate.AppAdTemplate.Instance.GetList(new AdTemplateQuery<AppAdTemplate>()
                //    {
                //        BaseMediaId = info.BaseMediaID,
                //        PageSize = 1,
                //        OrderBy = ""
                //    }).FirstOrDefault();
                //    return CreateSuccessMessage(retValue, string.Empty, "已经添加过此媒体（跳转添加广告或者模板）",
                //            new VerfiyAppResultCode
                //            {
                //                ResultCode = 1001,
                //                BaseMediaId = info.BaseMediaID,
                //                MediaId = info.MediaID,
                //                AdTemplateId = temp != null ? temp.RecID : 0
                //            });
                //}
                return CreateSuccessMessage(retValue, string.Empty, "当前媒体主角色已添加此媒体名称",
                  new VerfiyAppResultCode { ResultCode = 1001 });
            }

            return VerifyBaseAppByName(retValue, MeidaVerifyOfAddName);
        }

        private ReturnValue VerifyBaseAppByName(ReturnValue retValue, string mediaName)
        {
            var baseInfo = Dal.Media.MediaBasePCAPP.Instance.GetEntity(mediaName);
            if (baseInfo != null)
            {
                return CreateSuccessMessage(retValue, string.Empty, "跳转添加资质页面，调用接口属于添加操作（OperateType=1）",
                    new VerfiyAppResultCode { ResultCode = ResultCodeInsertQuft, BaseMediaId = baseInfo.RecID });
            }
            else
            {
                return CreateSuccessMessage(retValue, string.Empty, "跳转添加媒体+资质页面，调用接口属于添加操作（OperateType=1）",
                   new VerfiyAppResultCode { ResultCode = 1003 });
            }
        }

        /// <summary>
        /// 校验是否可以添加媒体，AE角色没有资质、角色:AE
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyOfAddByRoleAe(ReturnValue retValue)
        {
            var baseInfo = Dal.Media.MediaBasePCAPP.Instance.GetEntity(MeidaVerifyOfAddName);
            if (baseInfo != null)
            {
                //AE 在基表存在媒体信息、继续判断附表信息
                var info = Dal.Media.MediaPCAPP.Instance.GetInfoByRoleAe(MeidaVerifyOfAddName, RoleInfoMapping.AE);
                if (info != null)
                {
                    //附表存在-->跳转编辑页面
                    return CreateSuccessMessage(retValue, string.Empty, "跳转编辑媒体页面（渲染数据为附表）调用接口属于编辑操作（OperateType=2）",
                            new VerfiyAppResultCode { ResultCode = 1004, MediaId = info.MediaID });
                }
                else
                {
                    //附表不存在-->跳转添加媒体页面、End:入库附表信息-->同步到基表信息（相关信息）
                    return CreateSuccessMessage(retValue, string.Empty, "跳转编辑媒体页面（渲染数据为基表、AE角色资质不用），调用接口属于新增操作（OperateType=1）",
                            new VerfiyAppResultCode { ResultCode = 1004, BaseMediaId = baseInfo.RecID });
                }
            }
            else
            {
                //AE 在基表存在（没有）媒体信息、继续判断附表信息
                var info = Dal.Media.MediaPCAPP.Instance.GetInfoByRoleAe(MeidaVerifyOfAddName, RoleInfoMapping.AE);
                if (info != null)
                {
                    //附表存在-->跳转编辑页面
                    return CreateSuccessMessage(retValue, string.Empty, "跳转编辑媒体页面（渲染数据为附表）调用接口属于编辑操作（OperateType=2）",
                            new VerfiyAppResultCode { ResultCode = 1004, MediaId = info.MediaID });
                }
                else
                {
                    //附表不存在-->跳转添加媒体页面、End:入库附表信息-->同步到基表信息（相关信息）
                    return CreateSuccessMessage(retValue, string.Empty, "跳转添加媒体页面（AE角色资质不用）调用接口属于添加操作（OperateType=1）",
                            new VerfiyAppResultCode { ResultCode = 1005 });
                }
            }
        }

        #endregion 媒体主、AE 添加媒体入口校验

        #region 媒体详情渲染

        /// <summary>
        /// 获取app媒体详情(AE:新增、编辑显示继续添加模板按钮)
        /// 1.适用于编辑页面渲染
        /// 2.适用于查询详情页面渲染
        /// </summary>
        /// <returns></returns>
        public RespAppItemDto GetInfo()
        {
            if (_requestAppInfoDto.BaseMediaId > 0)
            {
                //查询基表数据
                var baseInfo = Dal.Media.MediaBasePCAPP.Instance.GetInfo(_requestAppInfoDto.BaseMediaId, _configEntity.CreateUserId);
                if (baseInfo != null)
                {
                    return AutoMapper.Mapper.Map<Entities.Media.MediaBasePCAPP, RespAppItemDto>(baseInfo);
                }
            }
            else if (_requestAppInfoDto.MediaId > 0)
            {
                //查询副表数据
                var info = Dal.Media.MediaPCAPP.Instance.GetInfo(_requestAppInfoDto.MediaId);
                if (info != null)
                {
                    return AutoMapper.Mapper.Map<Entities.Media.MediaPcApp, RespAppItemDto>(info);
                }
            }
            return null;
        }

        /// <summary>
        /// 解析覆盖区域字符串
        /// </summary>
        /// <param name="areaMapping"></param>
        /// <returns></returns>
        public static List<CoverageAreaDto> MapperToCoverageArea(string areaMapping)
        {
            /*
             示例：0,全国@=|1,安徽省@=101,合肥市|1,安徽省@=102,安庆市|1,安徽省@=103,蚌埠市
             */
            if (string.IsNullOrWhiteSpace(areaMapping)) return null;
            var list = new List<CoverageAreaDto>();
            //areaMapping = areaMapping.TrimStart('@').Trim('=');

            foreach (var item in areaMapping.Split(new string[] { "|" }, StringSplitOptions.None))
            {
                //'|'分割：1,安徽省@=101,合肥市
                if (string.IsNullOrWhiteSpace(item)) continue;
                var itemArea = item.Split(new string[] { "@=" }, StringSplitOptions.None);
                var coverageAreaDto = new CoverageAreaDto();
                var itemProvince = itemArea[0];
                var itemCity = itemArea[1];
                if (!string.IsNullOrWhiteSpace(itemProvince))
                {
                    //省份，'@='分割：   1,安徽省
                    var itemB = itemProvince.Split(',');
                    coverageAreaDto.ProvinceId = GetAppContent(itemB, 0).ToInt(-2);
                    coverageAreaDto.ProvinceName = GetAppContent(itemB, 1);
                }
                if (!string.IsNullOrWhiteSpace(itemCity))
                {
                    //城市 101,合肥市
                    var itemB = itemCity.Split(',');
                    coverageAreaDto.CityId = GetAppContent(itemB, 0).ToInt(-2);
                    coverageAreaDto.CityName = GetAppContent(itemB, 1);
                }

                list.Add(coverageAreaDto);
            }
            return list;
        }

        /// <summary>
        /// 解析常见分类字符串
        /// </summary>
        /// <param name="commonlyClass"></param>
        /// <returns></returns>
        public static List<CommonlyClassDto> MapperToCommonlyClass(string commonlyClass)
        {
            /*
             示例：47004,科技,1|47004,科技,0|47009,教育,0
             */
            if (string.IsNullOrWhiteSpace(commonlyClass)) return null;
            var list = new List<CommonlyClassDto>();

            foreach (var item in commonlyClass.Split(new string[] { "|" }, StringSplitOptions.None))
            {
                //47004,科技
                if (string.IsNullOrWhiteSpace(item)) continue;
                var itemClass = item.Split(new string[] { "," }, StringSplitOptions.None);
                var commonlyClassDto = new CommonlyClassDto
                {
                    CategoryId = GetAppContent(itemClass, 0).ToInt(-2),
                    CategoryName = GetAppContent(itemClass, 1),
                    SortNumber = GetAppContent(itemClass, 2).ToInt(-2)
                };
                list.Add(commonlyClassDto);
            }

            return list;
        }

        /// <summary>
        /// 解析下单备注字符串
        /// </summary>
        /// <param name="orderRemark"></param>
        /// <returns></returns>
        public static List<OrderRemarkDto> MapperToOrderRemark(string orderRemark)
        {
            /*
                示例：40001,不接竞品,|40002,不接硬广,|40009,其他,订单备注其他其他
           */
            if (string.IsNullOrWhiteSpace(orderRemark)) return null;
            var list = new List<OrderRemarkDto>();
            foreach (var item in orderRemark.Split(new string[] { "|" }, StringSplitOptions.None))
            {
                //40001,不接竞品,
                if (string.IsNullOrWhiteSpace(item)) continue;
                var itemRemark = item.Split(new string[] { "," }, StringSplitOptions.None);
                var orderRemarkDto = new OrderRemarkDto
                {
                    Id = GetAppContent(itemRemark, 0).ToInt(-2),
                    Name = GetAppContent(itemRemark, 1),
                    Descript = GetAppContent(itemRemark, 2)
                };
                list.Add(orderRemarkDto);
            }

            return list;
        }

        #endregion 媒体详情渲染
    }

    public class VerfiyAppResultCode
    {
        public int ResultCode { get; set; }
        public int MediaId { get; set; }
        public int BaseMediaId { get; set; }
        public int AdTemplateId { get; set; }
    }
}