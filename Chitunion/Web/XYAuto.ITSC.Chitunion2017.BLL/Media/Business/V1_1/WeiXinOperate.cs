using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Senparc.Weixin.MP.Helpers;
using XYAuto.ITSC.Chitunion2017.BLL.ImportData.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_4;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.ResponseDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Query;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1
{
    public class WeiXinOperate : CurrentOperateBase, IMediaOperate
    {
        public const int CategoryIdWeight = 1;
        public const int VersionRevision = 118;
        private readonly RequestGetMeidaInfoDto _requestGetInfoDto;
        private readonly RequestWeiXinDto _requestDto;
        private ConfigEntity _configEntity; //= new ConfigEntity() { BusinessType = MediaType.WeiXin };
        private readonly Dictionary<int, int> _dicCategoryIDs = new Dictionary<int, int>();
        private readonly List<string[]> _fansAreas = new List<string[]>();

        public Entities.Media.MediaWeixin UpdateBeforeInfo { get; set; }
        public Entities.WeixinOAuth.WeixinInfo InsertBaseWeixinInfo { get; set; }

        public WeiXinOperate(RequestWeiXinDto requestDto, ConfigEntity configEntity)
        {
            _requestDto = requestDto;
            _configEntity = configEntity;
        }

        public WeiXinOperate(RequestGetMeidaInfoDto requestGetInfoDto, ConfigEntity configEntity)
        {
            _requestGetInfoDto = requestGetInfoDto;
            _configEntity = configEntity;
        }

        public ReturnValue Excute()
        {
            if (_requestDto.Version >= VersionRevision)
            {
                //版本迭代，V1.1.8慢慢改版
                return Transition();
            }
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

        public ReturnValue Transition()
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.YunYingOperate ||
                _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin)
            {
                //todo:管理员角色，只用更新基表信息相关
                return DoTransitionByAdmin();
            }
            else if (_configEntity.RoleTypeEnum == RoleEnum.AE)
            {
                //todo:AE角色，先更新附表，再同步到基表，逻辑可复用之前的
                return DoTransition();
            }
            else
            {
                return CreateFailMessage(new ReturnValue(), "50051", "最新版本控制V1.1.8，还未迭代涉及到媒体主角色");
            }
        }

        public ReturnValue DoInsert()
        {
            var retValue = new ReturnValue();
            retValue = VerifyCreateBusiness(retValue);
            if (retValue.HasError)
                return retValue;
            var info = SetAuditPublishStatus(GetInsertEntity());
            var mediaId = Dal.Media.MediaWeixin.Instance.Insert(info);
            if (mediaId <= 0)
                return CreateFailMessage(retValue, "50002", "微信添加失败");
            _requestDto.MediaID = mediaId;
            //操作主表授权表
            retValue = InsertWeiXinAuth(retValue, info, mediaId);
            if (retValue.HasError)
                return retValue;
            //待审核消息
            WehchatAudit_Insert(_requestDto, OperateType.Insert, _configEntity.CreateUserId);
            //task
            TaskToRun(retValue, mediaId);
            retValue.ReturnObject = mediaId;
            return retValue;
        }

        /// <summary>
        /// 如果是AE、编辑的页面参数会比媒体主多
        /// 媒体主：页面查询、编辑都是副表
        /// AE：编辑主表和副表
        /// </summary>
        /// <returns></returns>
        public ReturnValue Update()
        {
            var retValue = new ReturnValue();

            retValue = VerifyUpdateBusiness(retValue);
            if (retValue.HasError)
                return retValue;
            var mediaInfo = retValue.ReturnObject as Entities.Media.MediaWeixin;
            if (mediaInfo == null)
                return CreateFailMessage(retValue, "50030", "拆箱失败");
            //根据角色编辑
            retValue = UpdateByRole(retValue);
            if (retValue.HasError)
                return retValue;
            //待审核消息
            WehchatAudit_Insert(_requestDto, OperateType.Edit, mediaInfo.CreateUserID);
            //task
            TaskToRun(retValue, _requestDto.MediaID);
            return retValue;
        }

        public ReturnValue DoInsertByAdmin()
        {
            var retValue = new ReturnValue();
            retValue = VerifyUpdateBusinessByAdmin(retValue);
            if (retValue.HasError)
                return retValue;

            var wxAuthId = Dal.WeixinOAuth.Instance.AddWeixinInfo(GetWeiXinAuthInfoByAdmin());
            if (wxAuthId <= 0)
            {
                var errorMsg = $"SupperAdmin 添加主表微信信息,number={_requestDto.Number}&recId={_requestDto.MediaID}";
                Loger.Log4Net.ErrorFormat(errorMsg);
                return CreateFailMessage(retValue, "50034", errorMsg);
            }

            //task
            TaskToRunWeiXinOuath(retValue, wxAuthId); //异步处理分类、粉丝区域
            return retValue;
        }

        public ReturnValue UpdateByAdmin()
        {
            var retValue = new ReturnValue();
            retValue = VerifyUpdateBusinessByAdmin(retValue);
            if (retValue.HasError)
                return retValue;
            var baseWeiXinInfo = Dal.WeixinOAuth.Instance.GetWeixinInfoByID(_requestDto.MediaID);
            if (baseWeiXinInfo == null)
                return CreateFailMessage(retValue, "50022", string.Format("主表ID={0} 不存在", _requestDto.MediaID));
            baseWeiXinInfo = GetUpdateWeiXinAuthInfoByAdmin(baseWeiXinInfo);
            if (Dal.WeixinOAuth.Instance.UpdateWeixinInfo(baseWeiXinInfo) == 0)
            {
                var errorMsg = string.Format("SupperAdmin 编辑主表微信信息,number={0}&recId={1}", _requestDto.Number,
                    _requestDto.MediaID);
                Loger.Log4Net.ErrorFormat(errorMsg);
                return CreateFailMessage(retValue, "50033", errorMsg);
            }

            //task
            TaskToRunWeiXinOuath(retValue, baseWeiXinInfo.RecID); //异步处理分类、粉丝区域
            return retValue;
        }

        /// <summary>
        /// 原始数据存储（为了对比编辑的信息不同之处）,即先存储编辑之前的数据
        /// </summary>
        /// <param name="mediaInfo"></param>
        public void SetMediaEditBeforeInfo(Entities.Media.MediaWeixin mediaInfo)
        {
            //处理需要对比的数据字段
            //GetCommonlyCalssAndArea(mediaInfo);
            //存入的原始数据
            //var info = Mapper.Map<Entities.Media.MediaWeixin, MediaDifferenceInfo>(mediaInfo);

            //Dal.PublishAuditInfo.Instance.Insert(new Entities.PublishAuditInfo
            //{
            //    MediaType = (int)MediaType.WeiXin,
            //    CreateTime = DateTime.Now,
            //    CreateUserID = _requestDto.CreateUserID,
            //    MediaID = mediaInfo.MediaID,
            //    //OptType =
            //});
        }

        /// <summary>
        /// 添加媒体微信的操作（如果是AE 则不用审核，直接添加副表和主表）
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="entity"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        private ReturnValue InsertWeiXinAuth(ReturnValue retValue, Entities.Media.MediaWeixin entity, int mediaId)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.AE)
            {
                //主表
                var wxAuthId = 0;
                var baseWeiXinInfo = Dal.WeixinOAuth.Instance.GetEntity(entity.Number);
                if (baseWeiXinInfo != null)
                {
                    wxAuthId = baseWeiXinInfo.RecID;
                    baseWeiXinInfo = GetUpdateWeiXinAuthInfo(baseWeiXinInfo, entity);
                    if (Dal.WeixinOAuth.Instance.UpdateWeixinInfo(baseWeiXinInfo) == 0)
                    {
                        Loger.Log4Net.ErrorFormat("AE/SupperAdmin 手工添加微信信息，主表存在--编辑微信主表信息失败，微信号：{0}", _requestDto.Number);
                    }
                }
                else
                {
                    //添加到主表
                    var authInfo = GetWeiXinAuthInfo(entity);
                    wxAuthId = Dal.WeixinOAuth.Instance.AddWeixinInfo(authInfo);
                    if (wxAuthId <= 0)
                    {
                        Loger.Log4Net.ErrorFormat("AE/SupperAdmin 手工添加微信信息，添加微信主表信息失败，微信号：{0}", _requestDto.Number);
                    }
                }
                if (wxAuthId > 0)
                {
                    //编辑 将wxId 写到 副表中
                    Dal.Media.MediaWeixin.Instance.UpdateWxId(mediaId, wxAuthId);
                    TaskToRunWeiXinOuath(retValue, wxAuthId); //异步处理分类、粉丝区域
                }
            }
            return retValue;
        }

        /// <summary>
        /// 媒体消息通知记录（待审核的消息表）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <param name="operateType"></param>
        /// <param name="userId"></param>
        public void WehchatAudit_Insert(RequestWeiXinDto requestDto, OperateType operateType, int userId)
        {
            if (_configEntity.RoleTypeEnum != RoleEnum.MediaOwner)
            {
                return;
            }
            string submitTrueName;
            int submitUserId = 0;
            if (operateType == OperateType.Insert)
            {
                var userInfo = BLL.UserDetailInfo.Instance.GetUserInfoByUserID(_configEntity.CreateUserId);
                submitTrueName = userInfo["TrueName"].ToString();
                submitUserId = _configEntity.CreateUserId;
            }
            else
            {
                var userInfo = BLL.UserDetailInfo.Instance.GetUserInfoByUserID(userId);
                submitTrueName = userInfo["TrueName"].ToString();
                submitUserId = userId;
            }

            var message = BLL.WeChatOperateMsg.Instance.WehchatAudit_InsertV1_1(requestDto.MediaID, requestDto.Name,
                submitTrueName, submitUserId, EnumWeChatAuditStatus.待审核);
            Loger.Log4Net.InfoFormat("WehchatAudit_Insert 媒体操作审核日志信息添加：{0}", message);
        }

        private Entities.Media.MediaWeixin SetAuditPublishStatus(Entities.Media.MediaWeixin entity)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.AE)
            {
                entity.AuditStatus = (int)MediaAuditStatusEnum.AlreadyPassed;
                entity.PublishStatus = (int)MediaPublishStatusEnum.UpOnshelf;
            }
            //if (
            //    _configEntity.CureOperateType == OperateType.Insert &&
            //    (int)SourceTypeEnum.手工 == _requestDto.AuthType)
            //{
            //    //wxId 只有在添加、且授权方式（除去手工）才维护入库，手工添加不理会=0
            //    entity.WxID = 0;
            //}
            return entity;
        }

        private ReturnValue UpdateByRole(ReturnValue retValue)
        {
            //var roleEnum = _configEntity.RoleTypeEnum; //RoleInfoMapping.GetUserRole(_requestDto.CreateUserID);
            //编辑副表

            var entity = GetUpdateEntity();
            SetAuditPublishStatus(entity);
            if (Dal.Media.MediaWeixin.Instance.Update(entity) == 0)
                return CreateFailMessage(retValue, "50012", "媒体信息编辑失败,错误码：50012");
            return InsertWeiXinAuth(retValue, entity, entity.MediaID);
        }

        private ReturnValue VerifyStringLength(ReturnValue retValue)
        {
            var count = CurrentOperateBase.GetLength(_requestDto.Sign);

            if (count > 2000)
            {
                return CreateFailMessage(retValue, "50044", "字段[Sign] 文本长度不能大于2000");
            }

            return retValue;
        }

        #region V1.1.8 新模式逻辑

        private ReturnValue DoTransition()
        {
            var mediaId = Dal.Media.MediaWeixin.Instance.VerifyMediaCountByRole(_requestDto.Number, RoleInfoMapping.AE);

            if (mediaId > 0)
            {
                //todo:证明AE角色下已存在附表媒体Number
                _requestDto.MediaID = mediaId;
                _configEntity.CureOperateType = OperateType.Edit;
                return Update();
            }
            else
            {
                //todo:没有则新增，同步基表
                _configEntity.CureOperateType = OperateType.Insert;
                return DoInsert();
            }
        }

        private ReturnValue DoTransitionByAdmin()
        {
            var baseWeiXinInfo = Dal.WeixinOAuth.Instance.GetEntity(_requestDto.Number);

            if (baseWeiXinInfo != null)
            {
                //todo:编辑
                _requestDto.MediaID = baseWeiXinInfo.RecID;
                _configEntity.CureOperateType = OperateType.Edit;
                return UpdateByAdmin();
            }
            else
            {
                //todo:新增
                _configEntity.CureOperateType = OperateType.Insert;
                return DoInsertByAdmin();
            }
        }

        #endregion V1.1.8 新模式逻辑

        private ReturnValue VerifyBaseWeiXinInfoNumber(ReturnValue retValue)
        {
            var filterId = _requestDto.MediaID;
            if (_configEntity.CureOperateType == OperateType.Insert)
            {
                filterId = 0;//如果是新增，不去过滤当前id，参数传的太混乱
            }
            var baseWeiXinInfo = Dal.WeixinOAuth.Instance.GetEntity(_requestDto.Number, filterId);
            if (baseWeiXinInfo != null)
            {
                return CreateFailMessage(retValue, "50015", string.Format("主表中已存在wxNumber：{0} 错误码code:50015", _requestDto.Number));
            }
            return retValue;
        }

        private ReturnValue VerifyUpdateBusinessByAdmin(ReturnValue retValue)
        {
            retValue = VerifyOfNecessaryParameters(_requestDto);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyStringLength(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyBaseWeiXinInfoNumber(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifycategoryIDsParams(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyFansAreaParams(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyFansSexScaleParams(retValue);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyAreaMedia(retValue);
            if (retValue.HasError)
                return retValue;
            //retValue.ReturnObject = info;
            return retValue;
        }

        public ReturnValue VerifyUpdateBusiness(ReturnValue retValue)
        {
            retValue = VerifyOfNecessaryParameters(_requestDto);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyStringLength(retValue);
            if (retValue.HasError)
                return retValue;
            //retValue = VerifyUpdateForAE(retValue);
            //if (retValue.HasError) return retValue;

            retValue = VerifyUpdateWeiXinInfo(retValue);
            if (retValue.HasError)
                return retValue;
            object info = retValue.ReturnObject;
            UpdateBeforeInfo = retValue.ReturnObject as Entities.Media.MediaWeixin;
            if (UpdateBeforeInfo == null)
                return CreateFailMessage(retValue, "50033", "拆箱失败");

            if (UpdateBeforeInfo.AuditStatus != 43004)
            {
                retValue = VerifyForAERole(retValue);
                if (retValue.HasError && UpdateBeforeInfo.FansCount != _requestDto.FansCount)
                    return retValue;
                retValue.HasError = false;
                retValue.Message = string.Empty;
            }

            retValue = VerifycategoryIDsParams(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyFansAreaParams(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyFansSexScaleParams(retValue);
            if (retValue.HasError)
                return retValue;

            retValue = VerifyAreaMedia(retValue);
            if (retValue.HasError)
                return retValue;
            retValue.ReturnObject = info;
            return retValue;
        }

        public ReturnValue VerifyOfRoleModule(ReturnValue retValue)
        {
            throw new NotImplementedException();
        }

        private ReturnValue VerifyUpdateWeiXinInfo(ReturnValue retValue)
        {
            var info = Dal.Media.MediaWeixin.Instance.GetEntity(_requestDto.MediaID);
            if (info == null)
            {
                return CreateFailMessage(retValue, "50007", string.Format("当前媒体信息不存在Id:{0}", _requestDto.MediaID));
            }
            if (info.Number != _requestDto.Number)
            {
                return CreateFailMessage(retValue, "50008", string.Format("微信帐号不允许修改"));
            }
            retValue.ReturnObject = info;
            return retValue;
        }

        /// <summary>
        /// 如果是AE，则需要验证更多参数
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyUpdateForAE(ReturnValue retValue)
        {
            var roleEnum = RoleInfoMapping.GetUserRole(_requestDto.CreateUserID);
            if (roleEnum != RoleEnum.AE)
                return retValue;
            if (string.IsNullOrWhiteSpace(_requestDto.Sign))
            {
                return CreateFailMessage(retValue, "50009", "请输入Sign描述／签名");
            }
            if (string.IsNullOrWhiteSpace(_requestDto.TwoCodeURL))
            {
                return CreateFailMessage(retValue, "50010", "请输入TwoCodeURL二维码");
            }
            if (_requestDto.LevelType <= 0)
            {
                return CreateFailMessage(retValue, "50011", "请输入LevelType媒体级别");
            }
            return retValue;
        }

        /// <summary>
        /// 添加常见分类/粉丝分布区域
        /// 增加资质表维护
        /// </summary>
        /// <param name="retValue"></param>
        private void TaskToRun(ReturnValue retValue, int mediaId)
        {
            var task = Task.Factory.StartNew(() =>
            {
                //添加常见分类
                try
                {
                    MediaCommonlyClassInsertByBulk(_dicCategoryIDs, retValue);
                }
                catch (Exception exception)
                {
                    Loger.Log4Net.ErrorFormat("MediaCommonlyClassInsertByBulk is exception:{0}{1}",
                        System.Environment.NewLine, exception);
                }
            })
                .ContinueWith(s =>
                {
                    //粉丝分布区域
                    try
                    {
                        MediaFansAreaInsert(retValue);
                    }
                    catch (Exception exception)
                    {
                        Loger.Log4Net.ErrorFormat("MediaFansAreaInsert is exception:{0}{1}", System.Environment.NewLine,
                            exception);
                    }
                })
                .ContinueWith(s =>
                {
                    try
                    {
                        MediaQualificationInsert(retValue);
                    }
                    catch (Exception exception)
                    {
                        Loger.Log4Net.ErrorFormat("MediaQualificationInsert is exception:{0}{1}",
                            System.Environment.NewLine, exception);
                    }
                })
                .ContinueWith(s =>
                {
                    try
                    {
                        UploadFileManage(mediaId);
                    }
                    catch (Exception exception)
                    {
                        Loger.Log4Net.ErrorFormat("weixin UploadFileManage is exception:{0}{1}",
                  System.Environment.NewLine, exception + (exception.StackTrace ?? string.Empty));
                    }
                });

            try
            {
                MediaOrderRemarkInsertByBulk(retValue, mediaId, MediaRelationType.Attached);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("weixin MediaOrderRemarkInsertByBulk is exception:{0}{1}",
                System.Environment.NewLine, exception + (exception.StackTrace ?? string.Empty));
            }

            //覆盖区域
            try
            {
                MediaAreaMappingInsertByBulk(retValue, mediaId, MediaRelationType.Attached);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("weixin Task to MediaAreaMappingInsertByBulk is Exception:{0}",
                    exception.Message + (exception.StackTrace ?? string.Empty));
            }
            retValue.HasError = false;
            retValue.Message = string.Empty;
        }

        private void UploadFileManage(int mediaId)
        {
            var urlList = new List<string>()
            {
                _requestDto.HeadIconURL,
                _requestDto.FansCountUrl,
                _requestDto.FansAreaShotUrl,
                _requestDto.FansSexScaleUrl
            };
            var retValue = BLL.UploadFileInfo.UploadFileInfo.Instance.Excute(urlList, _configEntity.CreateUserId,
                   UploadFileEnum.MediaManage, mediaId, "Media_Weixin");
            if (retValue.HasError)
            {
                Loger.Log4Net.ErrorFormat("weixin UploadFileManage is error:{0}{1}",
                    System.Environment.NewLine, retValue.Message);
            }
        }

        /// <summary>
        /// 主表相关的、添加常见分类/粉丝分布区域
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="authWxId"></param>
        private void TaskToRunWeiXinOuath(ReturnValue retValue, int authWxId)
        {
            Task.Factory.StartNew(() =>
            {
                //添加常见分类
                try
                {
                    MediaCategoryInsertByBulk(_dicCategoryIDs, retValue, authWxId);
                }
                catch (Exception exception)
                {
                    Loger.Log4Net.ErrorFormat("MediaCategoryInsertByBulk is exception:{0}{1}",
                        System.Environment.NewLine, exception);
                }
            })
            .ContinueWith(s =>
            {
                //粉丝分布区域
                try
                {
                    MediaFansAreaInsertByAuthWeiXin(retValue, authWxId);
                }
                catch (Exception exception)
                {
                    Loger.Log4Net.ErrorFormat("MediaFansAreaInsertByAuthWeiXin is exception:{0}{1}",
                        System.Environment.NewLine, exception);
                }
            });
            try
            {
                MediaOrderRemarkInsertByBulk(retValue, authWxId, MediaRelationType.BaseTable);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("weixin BaseTable MediaOrderRemarkInsertByBulk is exception:{0}{1}",
                System.Environment.NewLine, exception + (exception.StackTrace ?? string.Empty));
            }
            //覆盖区域
            try
            {
                MediaAreaMappingInsertByBulk(retValue, authWxId, MediaRelationType.BaseTable);
            }
            catch (Exception exception)
            {
                Loger.Log4Net.ErrorFormat("weixin BaseTable Task to MediaAreaMappingInsertByBulk is Exception:{0}",
                    exception.Message + (exception.StackTrace ?? string.Empty));
            }

            retValue.HasError = false;
            retValue.Message = string.Empty;
        }

        public ReturnValue VerifyCreateBusiness(ReturnValue retValue)
        {
            // retValue = retValue ?? new ReturnValue();
            retValue = VerifyOfNecessaryParameters(_requestDto);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyStringLength(retValue);
            if (retValue.HasError)
                return retValue;
            //todo business
            //retValue = VerifyOfRoleModule(retValue, "");
            //if (retValue.HasError) return retValue;

            retValue = VerifyNumber(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyCreateParams(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifycategoryIDsParams(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyFansAreaParams(retValue);
            if (retValue.HasError)
                return retValue;
            retValue = VerifyFansSexScaleParams(retValue);

            retValue = VerifyAreaMedia(retValue);
            if (retValue.HasError)
                return retValue;

            return retValue;
        }

        private ReturnValue VerifyCreateParams(ReturnValue retValue)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                retValue = VerifyOfInsertByCompare(retValue);

                if (!retValue.HasError)
                {
                    retValue = VerifyForAERole(retValue);
                    if (retValue.HasError && InsertBaseWeixinInfo.FansCount != _requestDto.FansCount)
                    {
                        return CreateFailMessage(retValue, "50044", "粉丝数已更改.请输入粉丝数截图");
                    }
                }
                retValue.HasError = false;
                retValue.Message = string.Empty;
            }
            return retValue;
        }

        private ReturnValue VerifyOfInsertByCompare(ReturnValue retValue)
        {
            var baseInfo = Dal.WeixinOAuth.Instance.GetEntity(_requestDto.Number);
            if (baseInfo == null)
            {
                return CreateFailMessage(retValue, "50043", string.Format("微信基表信息不存在，微信号：{0}", _requestDto.Number));
            }

            InsertBaseWeixinInfo = baseInfo;

            return retValue;
        }

        private ReturnValue VerifyForAERole(ReturnValue retValue)
        {
            if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
            {
                if (string.IsNullOrWhiteSpace(_requestDto.FansCountUrl))
                {
                    return CreateFailMessage(retValue, "50013", "请输入粉丝数截图");
                }
            }

            return retValue;
        }

        private ReturnValue VerifyAreaMedia(ReturnValue retValue)
        {
            if (_requestDto.IsAreaMedia)
            {
                if (_requestDto.AreaMedia == null || _requestDto.AreaMedia.Count == 0)
                {
                    return CreateFailMessage(retValue, "50050", "选择了区域媒体，必须输入区域媒体所在的城市");
                }
            }
            return retValue;
        }

        private ReturnValue VerifycategoryIDsParams(ReturnValue retValue)
        {
            var spLit = _requestDto.CommonlyClass.Split(',');//1101-0,1102-1
            foreach (var item in spLit)
            {
                //item:1101-0
                if (string.IsNullOrWhiteSpace(item)) continue;
                var spItem = item.Split('-');
                var spItemKey = spItem[0].ToInt();
                if (spItemKey > 0)
                    _dicCategoryIDs.Add(spItemKey, spItem[1].ToInt());
            }
            if (_dicCategoryIDs.Count < 1 || _dicCategoryIDs.Count > 5)
            {
                return CreateFailMessage(retValue, "50004", "常见分类数量必须是1-5个");
            }

            if (_dicCategoryIDs.All(s => s.Value != CategoryIdWeight))
            {
                return CreateFailMessage(retValue, "50013", "常见分类必须选择一个重要分类");
            }
            return retValue;
        }

        /// <summary>
        /// 粉丝分布区域校验
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyFansAreaParams(ReturnValue retValue)
        {
            if (!string.IsNullOrWhiteSpace(_requestDto.FansArea))
            {
                var spLit = _requestDto.FansArea.Split(',');
                foreach (var item in spLit)
                {
                    if (string.IsNullOrWhiteSpace(item)) continue;
                    var currentSplit = item.Split('-');
                    _fansAreas.Add(currentSplit);
                }

                if (_fansAreas.Count == 0)
                {
                    return CreateFailMessage(retValue, "50029", "请输入粉丝分布区域");
                }

                var listFansUserScale = _fansAreas.Select(item => item[1].ToDecimal(0)).ToList();
                if (listFansUserScale.Sum() > 100)
                {
                    return CreateFailMessage(retValue, "50030", "粉丝分布区域比例必须小于100%");
                }

                //校验截图（更改了内容则需要上传截图）
                if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
                {
                    if (_configEntity.CureOperateType == OperateType.Edit)
                    {
                        if (UpdateBeforeInfo.AuditStatus == 43004)
                        {
                            return retValue;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(_requestDto.FansAreaShotUrl))
                    {
                        if (_configEntity.CureOperateType == OperateType.Insert)
                        {
                            if (InsertBaseWeixinInfo == null) return retValue;
                            var tup = GetFansAreaAndFansProportionForBaseWeiXin(InsertBaseWeixinInfo.RecID);
                            if (tup.Item1 != null)
                            {
                                if (_fansAreas.Count != tup.Item1.Count)
                                {
                                    return CreateFailMessage(retValue, "50045", "粉丝分布区域已更改.请上传上传截图");
                                }
                                var requestFansAreaDto = _fansAreas.Select(s => new FansAreaDto
                                {
                                    ProvinceID = s[0].ToInt(),
                                    UserScale = s[1].ToDecimal(),
                                    ProvinceName = string.Empty
                                }).Where(s => s.ProvinceID > 0);
                                var baseFansAreaDto = tup.Item1.Select(s => new FansAreaDto
                                {
                                    ProvinceID = s.ProvinceID,
                                    UserScale = s.UserScale,
                                    ProvinceName = string.Empty
                                });

                                if (baseFansAreaDto.Except(requestFansAreaDto).Any())
                                {
                                    return CreateFailMessage(retValue, "50046", "粉丝分布区域已更改.请上传上传截图");
                                }
                            }
                        }
                        else
                        {
                            if (!UpdateBeforeInfo.IsAuth)
                                return CreateFailMessage(retValue, "50005", "粉丝分布区域输入必须上传截图");
                        }
                    }
                }
            }

            return retValue;
        }

        /// <summary>
        /// 粉丝男女比例校验
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue VerifyFansSexScaleParams(ReturnValue retValue)
        {
            if (_requestDto.FansFemalePer > 0 || _requestDto.FansMalePer > 0)
            {
                if (_requestDto.FansFemalePer + _requestDto.FansMalePer > 100)
                {
                    return CreateFailMessage(retValue, "50028", "男女粉丝比例不能大于100%");
                }

                if (_configEntity.RoleTypeEnum == RoleEnum.MediaOwner)
                {
                    if (_configEntity.CureOperateType == OperateType.Edit)
                    {
                        if (UpdateBeforeInfo.AuditStatus == 43004)
                        {
                            return retValue;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(_requestDto.FansSexScaleUrl))
                    {
                        if (_configEntity.CureOperateType == OperateType.Insert)
                        {
                            if (InsertBaseWeixinInfo == null) return retValue;
                            var tup = GetFansAreaAndFansProportionForBaseWeiXin(InsertBaseWeixinInfo.RecID);

                            if (tup.Item2 != null)
                            {
                                if (_requestDto.FansFemalePer != tup.Item2.FansFemalePer
                                    || _requestDto.FansMalePer != tup.Item2.FansMalePer)
                                {
                                    return CreateFailMessage(retValue, "50046", "粉丝男女比例已更改.请上传截图");
                                }
                            }
                        }
                        else
                        {
                            if (!UpdateBeforeInfo.IsAuth)
                                return CreateFailMessage(retValue, "50006", "粉丝男女比例输入必须上传截图");
                        }
                    }
                }
            }

            return retValue;
        }

        /// <summary>
        /// v1.1.1版本里微信在添加的时侯，需要看一下媒体主的身份是个人还是企业，如果是个人的话，资质入驻就不能点击了
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue MediaQualificationInsert(ReturnValue retValue)
        {
            if (_configEntity.UserType == UserTypeEnum.个人)
            {
                return retValue;
            }
            if (_configEntity.RoleTypeEnum == RoleEnum.AE || _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin
                || _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
                return retValue;
            }
            if (string.IsNullOrWhiteSpace(_requestDto.EnterpriseName))
            {
                return CreateFailMessage(retValue, "50013", "EnterpriseName 名称为null，不添加资质信息");
            }
            var mediaQualification = GetMediaQualificationInfo();
            return BLL.Media.MediaQualification.Instance.Update(mediaQualification, MediaType.WeiXin);
        }

        /// <summary>
        /// 覆盖区域（区域媒体）添加入库
        /// </summary>
        /// <param name="retValue"></param>
        /// <param name="mediaId"></param>
        /// <param name="mediaRelationType"></param>
        /// <returns></returns>
        public ReturnValue MediaAreaMappingInsertByBulk(ReturnValue retValue, int mediaId, MediaRelationType mediaRelationType)
        {
            retValue = BLL.Media.MediaAreaMapping.Instance.InsertByBulk(new AreaMapping()
            {
                CoverageArea = _requestDto.AreaMedia,
                CreateUserId = _configEntity.CreateUserId,
                MediaType = MediaType.WeiXin,
                AreaMappingType = MediaAreaMappingType.AreaMedia,
                MediaId = mediaId
            }, mediaRelationType);
            if (retValue.HasError)
            {
                var msg = string.Format("WeiXin(区域媒体) MediaAreaMappingInsertByBulk is error，错误信息：{0}", retValue.Message);
                Loger.Log4Net.ErrorFormat(msg);
                return CreateFailMessage(retValue, "70015", msg);
            }
            return retValue;
        }

        /// <summary>
        /// 批量添加常见分类信息
        /// </summary>
        /// <param name="categoryIDs">分类Id集合</param>
        /// <param name="retValue"></param>
        /// <returns></returns>
        public ReturnValue MediaCommonlyClassInsertByBulk(Dictionary<int, int> categoryIDs, ReturnValue retValue)
        {
            //分类Id集合
            var categoryIdList = categoryIDs.Select(s => s.Key).ToList();
            //权重Id,重要的分类
            var weightId = categoryIDs.Where(s => s.Value == CategoryIdWeight).Select(s => s.Key).FirstOrDefault();
            if (Dal.Media.MediaCommonlyClass.Instance.InsertByBulk(new Entities.Media.MediaCommonlyClass()
            {
                MediaID = _requestDto.MediaID,
                MediaType = (int)_configEntity.BusinessType,
                CategoryIDs = categoryIdList,
                CreateTime = DateTime.Now,
                CreateUserID = _configEntity.CreateUserId
            }, weightId) == 0)

            {
                return CreateFailMessage(retValue, "50003", "常见分类添加失败，错误码：" + retValue.ErrorCode);
            }

            return retValue;
        }

        public ReturnValue MediaCategoryInsertByBulk(Dictionary<int, int> categoryIDs, ReturnValue retValue, int authWxId)
        {
            //分类Id集合
            var categoryIdList = categoryIDs.Select(s => s.Key).ToList();
            //权重Id,重要的分类
            var weightId = categoryIDs.Where(s => s.Value == CategoryIdWeight).Select(s => s.Key).FirstOrDefault();
            if (Dal.Media.MediaCommonlyClass.Instance.InsertMediaCategoryByBulk(new Entities.Media.MediaCommonlyClass()
            {
                MediaID = authWxId,
                MediaType = (int)_configEntity.BusinessType,
                CategoryIDs = categoryIdList,
                CreateTime = DateTime.Now,
                CreateUserID = _configEntity.CreateUserId
            }, weightId) == 0)
            {
                return CreateFailMessage(retValue, "50033", "MediaCategory常见分类添加失败，错误码：" + retValue.ErrorCode);
            }
            return retValue;
        }

        public ReturnValue MediaOrderRemarkInsertByBulk(ReturnValue retValue, int mediaId, MediaRelationType mediaRelationType)
        {
            string errorMsg;
            if (_requestDto.OrderRemark == null || _requestDto.OrderRemark.Count == 0)
            {
                var msg = string.Format("weixin {0} MediaOrderRemarkInsertByBulk OrderRemark 为null，删除",
                    mediaRelationType);
                Loger.Log4Net.Info(msg);
                Dal.Media.MediaPCAPP.Instance.DelereOrderRemark(mediaId, 45002, mediaRelationType);
                return CreateFailMessage(retValue, "50015", msg);
            }
            var otherRemark = _requestDto.OrderRemark.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.Descript));

            var exec = MediaCommonInfo.Instance.InsertMediaRemark(mediaRelationType, 45002, mediaId,
                 _requestDto.OrderRemark.Select(s => s.Id).ToList(),
                 otherRemark == null ? string.Empty : otherRemark.Descript
                 , out errorMsg);
            if (exec <= 0)
            {
                var msg = string.Format("weixin {0} MediaOrderRemarkInsertByBulk is error:{1}", mediaRelationType, errorMsg);
                Loger.Log4Net.ErrorFormat(msg);
                return CreateFailMessage(retValue, "50015", msg);
            }
            return retValue;
        }

        private ReturnValue MediaFansAreaInsert(ReturnValue retValue)
        {
            var entity = new MediaFansArea()
            {
                CreateUserID = _requestDto.CreateUserID,
                MediaID = _requestDto.MediaID,
                MediaType = (int)_configEntity.BusinessType
            };
            var recId = Dal.Media.MediaFansArea.Instance.InsertByBulk(entity, _fansAreas);

            //if (recId == 0)
            //{
            //    Loger.Log4Net.ErrorFormat("MediaFansAreaInsert 微信媒体粉丝区域分布添加失败！参数：{0}", JsonConvert.SerializeObject(entity));
            //}
            return retValue;
        }

        private ReturnValue MediaFansAreaInsertByAuthWeiXin(ReturnValue retValue, int authWxId)
        {
            var entity = new MediaFansArea()
            {
                CreateUserID = _requestDto.CreateUserID,
                MediaID = authWxId,
                MediaType = (int)_configEntity.BusinessType
            };
            if (_fansAreas.Count > 0)
            {
                var recId = Dal.Media.MediaFansArea.Instance.InsertAuthFansWeixinByBulk(entity, _fansAreas);
                Dal.Media.MediaFansArea.Instance.InsertAuthFansWeixinByFansSex(entity, _requestDto.FansMalePer,
                    _requestDto.FansFemalePer);
            }

            //if (recId == 0)
            //{
            //    Loger.Log4Net.ErrorFormat("MediaFansAreaInsert 微信媒体粉丝区域分布添加失败！参数：{0}", JsonConvert.SerializeObject(entity));
            //}
            return retValue;
        }

        /// <summary>
        /// 验证Number是否在副表中存在，如果存在就提示，去添加刊例，不允许添加
        /// AE角色下面只能允许有一个微信Number,媒体主针对到个人
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        public ReturnValue VerifyNumber(ReturnValue retValue)
        {
            var createUserId = _configEntity.RoleTypeEnum == RoleEnum.AE
                ? Entities.Constants.Constant.INT_INVALID_VALUE
                : _requestDto.CreateUserID;

            if (_configEntity.RoleTypeEnum == RoleEnum.AE)
            {
                if (Dal.Media.MediaWeixin.Instance.VerifyMediaCountByRole(_requestDto.Number, RoleInfoMapping.AE) > 0)
                {
                    return CreateFailMessage(retValue, "50002", "当前用户在副表中已经存在微信号,请去添加刊例");
                }
            }
            else
            {
                var infoList = Dal.Media.MediaWeixin.Instance.GetList(new MediaQuery<Entities.Media.MediaWeixin>()
                {
                    PageSize = 1,
                    Number = _requestDto.Number,
                    CreateUserId = createUserId
                });
                if (infoList.Count > 0)
                {
                    return CreateFailMessage(retValue, "50001", "当前用户在副表中已经存在微信号,请去添加刊例");
                }
            }

            retValue.HasError = false;
            retValue.ErrorCode = string.Empty;
            retValue.Message = string.Empty;
            return retValue;
        }

        private Entities.Media.MediaWeixin GetInsertEntity()
        {
            var info = new Entities.Media.MediaWeixin()
            {
                MediaID = _requestDto.MediaID,
                //AreaID = _requestDto.AreaID,
                //CategoryID = _requestDto.CategoryID,
                CityID = _requestDto.CityID,
                FansCount = _requestDto.FansCount,
                FansCountURL = _requestDto.FansCountUrl.ToAbsolutePath(true),
                FansFemalePer = _requestDto.FansFemalePer,
                FansMalePer = _requestDto.FansMalePer,
                HeadIconURL = _requestDto.HeadIconURL.ToAbsolutePath(true),
                Number = _requestDto.Number,
                Name = _requestDto.Name,
                Sign = _requestDto.Sign,
                ProvinceID = _requestDto.ProvinceID,
                TwoCodeURL = _requestDto.TwoCodeURL.ToAbsolutePath(true),
                LevelType = _requestDto.LevelType,
                IsAuth = _requestDto.IsAuth,
                //OrderRemark = _requestDto.OrderRemark,
                //IsReserve = _requestDto.IsReserve,
                Source = _requestDto.Source,
                AuthType = _requestDto.AuthType,
                PublishStatus = (int)MediaPublishStatusEnum.Initialization,
                FansSexScaleUrl = _requestDto.FansSexScaleUrl,
                FansAreaShotUrl = _requestDto.FansAreaShotUrl,
                AuditStatus = (int)MediaAuditStatusEnum.PendingAudit,
                Status = 0,
                IsAreaMedia = _requestDto.IsAreaMedia,
                WxID = _requestDto.MediaID,
                CreateTime = _requestDto.CreateTime,
                CreateUserID = _requestDto.CreateUserID,
                LastUpdateTime = _requestDto.LastUpdateTime,
                LastUpdateUserID = _requestDto.CreateUserID
            };

            return info;
        }

        private Entities.Media.MediaWeixin GetUpdateEntity()
        {
            var info = GetInsertEntity();

            info.ADName = UpdateBeforeInfo.ADName;
            info.AreaID = UpdateBeforeInfo.AreaID;
            info.CategoryID = UpdateBeforeInfo.CategoryID;
            info.IsReserve = UpdateBeforeInfo.IsReserve;

            return info;
        }

        private Entities.WeixinOAuth.WeixinInfo GetWeiXinAuthInfo(Entities.Media.MediaWeixin mediaWeixin)
        {
            var info = new Entities.WeixinOAuth.WeixinInfo
            {
                WxNumber = mediaWeixin.Number,
                CityID = mediaWeixin.CityID,
                NickName = mediaWeixin.Name,
                IsVerify = mediaWeixin.IsAuth,
                HeadImg = mediaWeixin.HeadIconURL,
                QrCodeUrl = mediaWeixin.TwoCodeURL,
                FansCount = mediaWeixin.FansCount,
                SourceType = (int)SourceTypeEnum.手工,//为什么是手工类型？AE添加的是手工
                CreateTime = DateTime.Now,
                Sign = mediaWeixin.Sign,
                IsAreaMedia = mediaWeixin.IsAreaMedia,
                LevelType = mediaWeixin.LevelType,
                ProvinceID = mediaWeixin.ProvinceID,
                Status = 0 //-1：删除数据   1：未审核   0：正常
            };

            return info;
        }

        private Entities.WeixinOAuth.WeixinInfo GetWeiXinAuthInfoByAdmin()
        {
            var info = new Entities.WeixinOAuth.WeixinInfo
            {
                WxNumber = _requestDto.Number,
                CityID = _requestDto.CityID,
                NickName = _requestDto.Name,
                IsVerify = _requestDto.IsAuth,
                HeadImg = _requestDto.HeadIconURL,
                QrCodeUrl = _requestDto.TwoCodeURL,
                FansCount = _requestDto.FansCount,
                SourceType = (int)SourceTypeEnum.手工,//为什么是手工类型？AE添加的是手工
                CreateTime = DateTime.Now,
                Sign = _requestDto.Sign,
                LevelType = _requestDto.LevelType,
                ProvinceID = _requestDto.ProvinceID,
                IsAreaMedia = _requestDto.IsAreaMedia,
                Status = 0 //-1：删除数据   1：未审核   0：正常
            };

            return info;
        }

        private Entities.Media.MediaQualification GetMediaQualificationInfo()
        {
            return new Entities.Media.MediaQualification()
            {
                BusinessLicense = _requestDto.BusinessLicense.ToAbsolutePath(true),
                CreateUserID = _requestDto.CreateUserID,
                EnterpriseName = _requestDto.EnterpriseName,
                MediaID = _requestDto.MediaID,
                QualificationOne = _requestDto.QualificationOne.ToAbsolutePath(true),
                QualificationTwo = _requestDto.QualificationTwo.ToAbsolutePath(true),
                MediaType = (int)MediaType.WeiXin,
                Status = 0
            };
        }

        private Entities.WeixinOAuth.WeixinInfo GetUpdateWeiXinAuthInfo(Entities.WeixinOAuth.WeixinInfo authWeixinInfo,
            Entities.Media.MediaWeixin entity)
        {
            authWeixinInfo.NickName = entity.Name;
            authWeixinInfo.FansCount = entity.FansCount > authWeixinInfo.FansCount
                ? entity.FansCount
                : authWeixinInfo.FansCount;
            authWeixinInfo.HeadImg = entity.HeadIconURL;
            authWeixinInfo.QrCodeUrl = entity.TwoCodeURL;
            authWeixinInfo.LevelType = entity.LevelType;
            authWeixinInfo.ProvinceID = entity.ProvinceID;
            authWeixinInfo.CityID = entity.CityID;
            authWeixinInfo.IsVerify = entity.IsAuth;
            authWeixinInfo.LevelType = entity.LevelType;
            authWeixinInfo.Sign = entity.Sign;
            authWeixinInfo.ModifyTime = DateTime.Now;
            authWeixinInfo.WxNumber = entity.Number;
            authWeixinInfo.Status = 0;
            authWeixinInfo.IsAreaMedia = _requestDto.IsAreaMedia;

            return authWeixinInfo;
        }

        private Entities.WeixinOAuth.WeixinInfo GetUpdateWeiXinAuthInfoByAdmin(Entities.WeixinOAuth.WeixinInfo authWeixinInfo)
        {
            authWeixinInfo.NickName = _requestDto.Name;
            authWeixinInfo.FansCount = _requestDto.FansCount;
            authWeixinInfo.HeadImg = _requestDto.HeadIconURL;
            authWeixinInfo.QrCodeUrl = _requestDto.TwoCodeURL;
            authWeixinInfo.LevelType = _requestDto.LevelType;
            authWeixinInfo.ProvinceID = _requestDto.ProvinceID;
            authWeixinInfo.CityID = _requestDto.CityID;
            authWeixinInfo.IsVerify = _requestDto.IsAuth;
            authWeixinInfo.LevelType = _requestDto.LevelType;
            authWeixinInfo.Sign = _requestDto.Sign;
            authWeixinInfo.ModifyTime = DateTime.Now;
            authWeixinInfo.WxNumber = _requestDto.Number;
            authWeixinInfo.Status = 0;
            authWeixinInfo.IsAreaMedia = _requestDto.IsAreaMedia;

            return authWeixinInfo;
        }

        #region 查询相关

        /// <summary>
        /// OperateType	1：添加（是从授权之后调整到页面、查询是基础表信息）
        /// 2：编辑（从后台媒体列表跳转到页面、查询是副表信息,媒体已通过的是主表，待审核或驳回是副表）
        /// </summary>
        /// <returns></returns>
        public RespGetWeiXinDto GetQueryInfo()
        {
            var retValue = VerifyOfNecessaryParameters(_requestGetInfoDto);
            if (retValue.HasError) return null;
            if (_requestGetInfoDto.OperateType == (int)OperateType.Insert)
            {
                //是从授权之后调整到页面、查询是基础表信息
                var baseInfo = Dal.WeixinOAuth.Instance.GetInfo(_requestGetInfoDto.RecId,
                    _requestGetInfoDto.CreateUserId);

                if (baseInfo == null) return null;
                var wxInfo = Mapper.Map<Entities.WeixinOAuth.WeixinInfo, RespGetWeiXinDto>(baseInfo);
                wxInfo.WxId = wxInfo.MediaID;
                wxInfo.IsExist = baseInfo.IsExist;
                return GetCommonlyCalssByMediaCategory(wxInfo);
            }
            else
            {
                //运营只能修改主表信息，故渲染页面用主表信息
                if (_configEntity.RoleTypeEnum == RoleEnum.SupperAdmin ||
                    _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
                {
                    //查询的是主表
                    var baseInfo = Dal.WeixinOAuth.Instance.GetWeixinInfoByID(_requestGetInfoDto.RecId);
                    if (baseInfo == null) return null;
                    var wxInfo = Mapper.Map<Entities.WeixinOAuth.WeixinInfo, RespGetWeiXinDto>(baseInfo);
                    wxInfo.WxId = wxInfo.MediaID;
                    return GetCommonlyCalssByMediaCategory(wxInfo);
                }
                //从后台媒体列表跳转到页面、查询是副表信息
                if (_requestGetInfoDto.IsAuditPass)
                {
                    //如果是审核通过，则查询主表信息
                    //查询的是主表
                    var baseInfo = GetReturnBaseInfo();
                    return GetCommonlyCalssByMediaCategory(baseInfo);
                }
                var info = Dal.Media.MediaWeixin.Instance.GetEntity(_requestGetInfoDto.RecId);
                if (info == null) return null;
                GetCommonlyCalssAndArea(info);
                return Mapper.Map<Entities.Media.MediaWeixin, RespGetWeiXinDto>(info);
            }
        }

        public RespGetWeiXinDto GetInfo()
        {
            //慢慢的修改查询这部分的逻辑
            if (_requestGetInfoDto.BaseMediaId > 0)
            {
                //基表信息
                var baseInfo = Dal.WeixinOAuth.Instance.GetWeixinInfoByID(_requestGetInfoDto.BaseMediaId);
                if (baseInfo == null) return null;
                var wxInfo = Mapper.Map<Entities.WeixinOAuth.WeixinInfo, RespGetWeiXinDto>(baseInfo);
                wxInfo.WxId = wxInfo.MediaID;
                return GetCommonlyCalssByMediaCategory(wxInfo);
            }
            else if (_requestGetInfoDto.MediaId > 0)
            {
                var info = Dal.Media.MediaWeixin.Instance.GetEntity(_requestGetInfoDto.MediaId);
                if (info == null) return null;
                GetCommonlyCalssAndArea(info);
                return Mapper.Map<Entities.Media.MediaWeixin, RespGetWeiXinDto>(info);
            }
            else
            {
                return null;
            }
        }

        public RespGetWeiXinDto GetBaseInfoByNumber()
        {
            var baseInfo = Dal.WeixinOAuth.Instance.GetEntity(_requestGetInfoDto.MediaName);
            if (baseInfo == null) return null;
            var wxInfo = Mapper.Map<Entities.WeixinOAuth.WeixinInfo, RespGetWeiXinDto>(baseInfo);
            wxInfo.WxId = wxInfo.MediaID;
            return GetCommonlyCalssByMediaCategory(wxInfo);
        }

        ///  <summary>
        ///  获取详情页的基础参数
        /// zlb修改：添加当前人条件，查询是否是当前人的收藏或拉黑
        ///  </summary>
        ///  <param name="mediaId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RespWeiXinItemDto GetItemInfo(int mediaId, int userId)
        {
            var item = Dal.Media.MediaWeixin.Instance.GetItem(mediaId, userId);
            if (item == null) return null;

            var tup = GetFansAreaAndFansProportionForBaseWeiXin(item.WxId);

            item.FansArea = tup.Item1;
            if (tup.Item2 != null)
            {
                item.FansFemalePer = tup.Item2.FansFemalePer;
                item.FansMalePer = tup.Item2.FansMalePer;
            }
            item.AreaMedia = AppOperate.MapperToCoverageArea(item.AreaMapping);
            return item;
        }

        /// <summary>
        /// 后台查询媒体详情
        /// </summary>
        /// <returns></returns>
        public RespGetWeiXinForBackDto GetItemForBack(RequestGetCommonlyCalssDto dto)
        {
            if (dto.Wx_Status == (int)MediaAuditStatusEnum.Initialization)
            {
                #region 基表Id

                var baseInfo = Dal.WeixinOAuth.Instance.GetWeixinInfoByAudit(dto.MediaId);
                if (baseInfo == null)
                    return null;
                var wxInfo = Mapper.Map<Entities.WeixinOAuth.WeixinInfo, Entities.Media.MediaWeixin>(baseInfo);
                wxInfo.WxID = dto.MediaId;

                var tup = GetFansAreaAndFansProportionForBaseWeiXin(wxInfo.WxID);

                wxInfo.FansArea = tup.Item1;
                if (tup.Item2 != null)
                {
                    wxInfo.FansFemalePer = tup.Item2.FansFemalePer;
                    wxInfo.FansMalePer = tup.Item2.FansMalePer;
                }

                return Mapper.Map<Entities.Media.MediaWeixin, RespGetWeiXinForBackDto>(wxInfo);

                #endregion 基表Id
            }
            else if (dto.Wx_Status == (int)MediaAuditStatusEnum.AlreadyPassed)
            {
                //联合查询(用mediaId)返回主表信息字段

                #region 联合查询(用mediaId)

                var item = Dal.Media.MediaWeixin.Instance.GetItemForBackBase(dto.MediaId);
                if (item == null) return null;
                var tup = GetFansAreaAndFansProportionForBaseWeiXin(item.WxID);

                item.FansArea = tup.Item1;
                if (tup.Item2 != null)
                {
                    item.FansFemalePer = tup.Item2.FansFemalePer;
                    item.FansMalePer = tup.Item2.FansMalePer;
                }

                return Mapper.Map<Entities.Media.MediaWeixin, RespGetWeiXinForBackDto>(item);

                #endregion 联合查询(用mediaId)
            }
            else
            {
                //不通过
                //副表

                #region 副表

                var item = Dal.Media.MediaWeixin.Instance.GetItemForBack(dto.MediaId);
                return GetItemForBack(item);

                #endregion 副表
            }
        }

        /// <summary>
        /// 获取副表相关的粉丝区域信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private RespGetWeiXinForBackDto GetItemForBack(Entities.Media.MediaWeixin entity)
        {
            if (entity == null) return null;
            entity.FansArea = Dal.Media.MediaFansArea.Instance.GetList(new MediaQuery<MediaFansArea>()
            {
                MediaType = (int)MediaType.WeiXin,
                MediaId = entity.MediaID,
                OrderBy = " MF.UserScale DESC"
            }).Select(x => new FansAreaDto()
            {
                ProvinceID = x.ProvinceID,
                ProvinceName = x.ProvinceName,
                UserScale = x.UserScale
            }).ToList();
            return Mapper.Map<Entities.Media.MediaWeixin, RespGetWeiXinForBackDto>(entity);
        }

        private RespGetWeiXinDto GetReturnBaseInfo()
        {
            //var baseInfo = Dal.WeixinOAuth.Instance.GetWeixinInfoByID(_requestGetInfoDto.RecId);

            var baseInfo = Dal.Media.MediaWeixin.Instance.GetInnerJoinEntity(_requestGetInfoDto.RecId);

            return baseInfo == null
                ? null
                : Mapper.Map<Entities.Media.MediaWeixin, RespGetWeiXinDto>(baseInfo);
        }

        /// <summary>
        /// 获取基表的粉丝分布区域，粉丝男女比例
        /// </summary>
        /// <param name="baseWxMediaId"></param>
        /// <returns>item1：粉丝分布区域   item2：粉丝男女比例</returns>
        public Tuple<List<FansAreaDto>, FansSexProportionDto> GetFansAreaAndFansProportionForBaseWeiXin(int baseWxMediaId)
        {
            var fansArea = Dal.Media.MediaFansArea.Instance.GetListByWeiXinAuth(new MediaQuery<MediaFansArea>()
            {
                MediaType = (int)MediaType.WeiXin,
                //StatisticType = StatisticTypeEnum.FansAreaMapper,
                WxId = baseWxMediaId
            });
            if (fansArea.Count == 0)
            {
                return new Tuple<List<FansAreaDto>, FansSexProportionDto>(null, null);
            }
            //区域分布
            var fansAreaList = fansArea.Where(s => s.MediaType == (int)StatisticTypeEnum.FansAreaMapper)
                 .Select(x => new FansAreaDto()
                 {
                     ProvinceID = x.ProvinceID,
                     ProvinceName = x.ProvinceName,
                     UserScale = x.UserScale
                 }).ToList();
            //男女比例
            var fansSexProportion = fansArea.Where(s => s.MediaType == (int)StatisticTypeEnum.FansSexProportion).ToList();
            //FansSexProportionDto
            var fansSexProportionDto = new FansSexProportionDto()
            {
                FansMalePer = fansSexProportion.Where(s => s.ProvinceID == (int)SexEnum.男).Select(s => s.UserScale).FirstOrDefault(),
                FansFemalePer = fansSexProportion.Where(s => s.ProvinceID == (int)SexEnum.女).Select(s => s.UserScale).FirstOrDefault()
            };
            return new Tuple<List<FansAreaDto>, FansSexProportionDto>(fansAreaList, fansSexProportionDto);
        }

        public RespGetWeiXinDto GetCommonlyCalssByMediaCategory(RespGetWeiXinDto respGetWeiXinDto)
        {
            if (respGetWeiXinDto != null)
            {
                respGetWeiXinDto.CommonlyClass = Dal.Media.MediaCommonlyClass.Instance.GetListByMediaCategory(new MediaQuery<MediaCommonlyClass>()
                {
                    WxId = respGetWeiXinDto.WxId,
                    MediaType = (int)MediaType.WeiXin,
                    //OrderBy =
                }).Select(x => x.CategoryID).ToList();

                var tup = GetFansAreaAndFansProportionForBaseWeiXin(respGetWeiXinDto.WxId);

                respGetWeiXinDto.FansArea = tup.Item1;
                if (tup.Item2 != null)
                {
                    respGetWeiXinDto.FansFemalePer = tup.Item2.FansFemalePer;
                    respGetWeiXinDto.FansMalePer = tup.Item2.FansMalePer;
                }
            }
            return respGetWeiXinDto;
        }

        public Entities.Media.MediaWeixin GetCommonlyCalssAndArea(Entities.Media.MediaWeixin entityWeixin)
        {
            if (entityWeixin != null)
            {
                entityWeixin.CommonlyClass =
                    Dal.Media.MediaCommonlyClass.Instance.GetList(new MediaQuery<MediaCommonlyClass>()
                    {
                        MediaType = (int)MediaType.WeiXin,
                        MediaId = entityWeixin.MediaID
                    }).Select(x => x.CategoryID).ToList();
                entityWeixin.FansArea = Dal.Media.MediaFansArea.Instance.GetList(new MediaQuery<MediaFansArea>()
                {
                    MediaType = (int)MediaType.WeiXin,
                    MediaId = entityWeixin.MediaID
                }).Select(x => new FansAreaDto()
                {
                    ProvinceID = x.ProvinceID,
                    ProvinceName = x.ProvinceName,
                    UserScale = x.UserScale
                }).ToList();
            }
            return entityWeixin;
        }

        /// <summary>
        /// 审核页面详情接口（对比编辑前与现在的数据）
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public RespMediaAuditDetailViewDto GetAuditDetailInfo(int mediaId)
        {
            var respMediaAuditDetailViewDto = new RespMediaAuditDetailViewDto();
            //副表数据
            var item = Dal.Media.MediaWeixin.Instance.GetItemForBack(mediaId);
            if (item != null)
            {
                respMediaAuditDetailViewDto.MediaInfo = GetItemForBack(item);
                //主表数据
                var baseInfo = Dal.WeixinOAuth.Instance.GetWeixinInfoByAudit(item.WxID);
                if (baseInfo == null)
                    return respMediaAuditDetailViewDto;
                var wxInfo = Mapper.Map<Entities.WeixinOAuth.WeixinInfo, Entities.Media.MediaWeixin>(baseInfo);
                wxInfo.MediaID = item.MediaID;
                wxInfo.WxID = item.WxID;

                var tup = GetFansAreaAndFansProportionForBaseWeiXin(wxInfo.WxID);

                wxInfo.FansArea = tup.Item1;
                if (tup.Item2 != null)
                {
                    wxInfo.FansFemalePer = tup.Item2.FansFemalePer;
                    wxInfo.FansMalePer = tup.Item2.FansMalePer;
                }

                respMediaAuditDetailViewDto.BaseMediaInfo = Mapper.Map<Entities.Media.MediaWeixin, RespGetWeiXinForBackDto>(wxInfo);
                return respMediaAuditDetailViewDto;
            }
            return respMediaAuditDetailViewDto;
        }

        private string GetEditDifferenceInfo(Entities.Media.MediaWeixin mediaWeixin)
        {
            return string.Empty;
        }

        #endregion 查询相关

        #region 相关分类推荐查询

        /// <summary>
        /// 1)	根据媒体设定的常用分类，推荐相同分类下得媒体，取所有分类下粉丝数量前五的媒体，做排重处理
        /// a)	推荐审核状态“已通过”、且媒体下有“上架”状态刊例的媒体
        /// b)	若相同分类下的主分类推荐结果媒体数小于1，则取一般分类
        /// c)  若相同分类下的一般分类推荐结果媒体数小于1，则取全局微信类媒体粉丝数前五的做推荐
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<GetRecommendClassListDto> GetRecommendClass(PublishSearchAutoQuery<GetRecommendClassListDto> query)
        {
            var list = Dal.Media.MediaWeixin.Instance.GetRecommendClassMain(query);
            if (list.Count == 0)
            {
                //b)	若相同分类下的主分类推荐结果媒体数小于1，则取一般分类
                list = Dal.Media.MediaWeixin.Instance.GetRecommendClass(query);
            }
            if (list.Count == 0)
            {
                //c)	若推荐结果媒体数小于1，，则取全局微信类媒体粉丝数前五的做推荐
                return Dal.Media.MediaWeixin.Instance.GetRecommendClassOther(query);
            }
            return list;
        }

        #endregion 相关分类推荐查询

        #region 模糊匹配下拉

        ///  <summary>
        /// --1.媒体主：只显示该媒体主下所有审核通过的刊例下含键关词创建时间最新的10个
        /// --2.AE：只显示该AE下审核通过的刊例下含键关词创建时间最新的10个
        /// --3.运营：只显示当前处于待审刊例下的刊例中含键关词创建时间最新的10个
        ///  </summary>
        ///  <param name="query"></param>
        /// <param name="roleEnum"></param>
        /// <returns></returns>
        public List<SearchTitleResponse> GetSearchTitle(PublishSearchAutoQuery<SearchTitleResponse> query, RoleEnum roleEnum)
        {
            // RoleInfoMapping.GetUserRole();
            if (roleEnum == RoleEnum.YunYingOperate || roleEnum == RoleEnum.SupperAdmin)
            {
                //处于待审刊例
                query.Wx_Status = (int)PublishBasicStatusEnum.待审核;
                query.CreateUserId = Entities.Constants.Constant.INT_INVALID_VALUE;
            }
            return Dal.Media.MediaWeixin.Instance.GetSearchTitle(query);
        }

        #endregion 模糊匹配下拉
    }
}