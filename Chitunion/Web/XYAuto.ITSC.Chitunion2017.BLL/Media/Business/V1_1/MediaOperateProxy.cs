using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_1
{
    /// <summary>
    /// Auth:lixiong
    /// 媒体操作相关代理类
    /// </summary>
    public class MediaOperateProxy : CurrentOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly RequestGetMeidaInfoDto _requestGetDto;
        private readonly RequestMediaDto _requestMediaDto;
        private readonly RequestGetCommonlyCalssDto _requestGetItemDto;
        private Dictionary<int, Func<string, dynamic>> _dictionary; //接口泛型集合
        private readonly Lazy<Dictionary<int, Func<string, dynamic>>> _lazyDic;//lazy加载
        private readonly Lazy<Dictionary<int, Func<string, dynamic>>> _lazyQueryDic;//lazy加载
        private readonly Lazy<Dictionary<int, Func<string, dynamic>>> _lazyQueryItemDic;//lazy加载

        /// <summary>
        /// 媒体操作相关
        /// </summary>
        /// <param name="requestMediaDto"></param>
        /// <param name="configEntity"></param>
        public MediaOperateProxy(RequestMediaDto requestMediaDto, ConfigEntity configEntity)
        {
            _configEntity = configEntity;
            _requestMediaDto = requestMediaDto;
            _lazyDic = new Lazy<Dictionary<int, Func<string, dynamic>>>(Initialization);
        }

        /// <summary>
        /// 媒体查询相关
        /// </summary>
        /// <param name="requestGetDto"></param>
        /// <param name="configEntity"></param>
        public MediaOperateProxy(RequestGetMeidaInfoDto requestGetDto, ConfigEntity configEntity)
        {
            _configEntity = configEntity;
            _requestGetDto = requestGetDto;
            _lazyQueryDic = new Lazy<Dictionary<int, Func<string, dynamic>>>(InitializationByQuery);
        }

        /// <summary>
        /// 媒体查询详情相关（详情页面的基本参数）
        /// </summary>
        /// <param name="requestGetItemDto"></param>
        /// <param name="configEntity"></param>
        public MediaOperateProxy(RequestGetCommonlyCalssDto requestGetItemDto, ConfigEntity configEntity)
        {
            _configEntity = configEntity;
            _requestGetItemDto = requestGetItemDto;
            _lazyQueryItemDic = new Lazy<Dictionary<int, Func<string, dynamic>>>(InitializationByQueryItem);
        }

        /// <summary>
        /// 初始化接口泛型集合：媒体新增、编辑
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, Func<string, dynamic>> Initialization()
        {
            return _dictionary = new Dictionary<int, Func<string, dynamic>>()
            {
                {(int) MediaType.WeiXin, s =>
                {
                        _requestMediaDto.WeiXin.CreateUserID = _configEntity.CreateUserId;
                        _requestMediaDto.WeiXin.Source = (int)_configEntity.SourceTypeEnum;
                        _configEntity.BusinessType = MediaType.WeiXin;
                        _configEntity.CureOperateType =(OperateType)_requestMediaDto.OperateType;
                        return new WeiXinOperate(_requestMediaDto.WeiXin,_configEntity).Excute();
                }},
                {
                    (int)MediaType.APP, s =>
                    {
                        _requestMediaDto.App.CreateUserId = _configEntity.CreateUserId;
                        _requestMediaDto.App.Source = (int)_configEntity.SourceTypeEnum;
                        _configEntity.BusinessType = MediaType.APP;
                        _configEntity.CureOperateType =(OperateType)_requestMediaDto.OperateType;
                        return new AppOperate(_configEntity,_requestMediaDto.App).Excute();
                    }
                },
                {
                    (int)MediaType.Template, s =>
                    {
                        _requestMediaDto.Temp.CreateUserId = _configEntity.CreateUserId;
                        _configEntity.BusinessType = MediaType.Template;
                        _configEntity.CureOperateType =(OperateType)_requestMediaDto.OperateType;
                        return new AdTemplateProvider(_configEntity,_requestMediaDto.Temp).Excute();
                    }
                }
            };
        }

        /// <summary>
        /// 初始化接口泛型集合：媒体查询详情
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, Func<string, dynamic>> InitializationByQuery()
        {
            return new Dictionary<int, Func<string, dynamic>>()
            {
                {(int) MediaType.WeiXin, s =>
                {  _configEntity.BusinessType = MediaType.WeiXin;
                    if (_requestGetDto.Version >= 118)
                    {
                        return new WeiXinOperate(_requestGetDto,_configEntity).GetInfo();
                    }
                    else
                    {
                        return new WeiXinOperate(_requestGetDto,_configEntity).GetQueryInfo();
                    }
                }},
                { (int)MediaType.APP, s =>
                {
                    _configEntity.BusinessType = MediaType.APP;
                    var requestAppInfoDto = new RequestAppInfoDto() {
                        MediaId = _requestGetDto.MediaId,
                        BaseMediaId = _requestGetDto.BaseMediaId};
                  return new AppOperate(_configEntity,requestAppInfoDto).GetInfo();
                }},
                { (int)MediaType.Template , s =>
                {
                     _configEntity.BusinessType = MediaType.Template;
                    var requestAppInfoDto = new RequestTemplateInfoDto() {AdTempId = _requestGetDto.AdTempId,
                        BaseMediaId = _requestGetDto.BaseMediaId,
                        AdBaseTempId = _requestGetDto.AdBaseTempId};
                  return new AdTemplateProvider(_configEntity,requestAppInfoDto).GetInfo();
                }}
            };
        }

        private ConfigEntity SetConfig(ConfigEntity configEntity)
        {
            var config = configEntity ?? new ConfigEntity();
            return config;
        }

        /// <summary>
        /// 详情页面的基础参数查询
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, Func<string, dynamic>> InitializationByQueryItem()
        {
            return new Dictionary<int, Func<string, dynamic>>()
            {
                {(int) MediaType.WeiXin, s =>
                {   _configEntity.BusinessType = MediaType.WeiXin;
                  return  new WeiXinOperate(_requestGetDto,_configEntity).GetItemInfo(_requestGetItemDto.MediaId,_configEntity.CreateUserId);
                }}
            };
        }

        public ReturnValue Excute()
        {
            var retValue = VerifyOfNecessaryParameters<RequestMediaDto>(_requestMediaDto);
            if (retValue.HasError)
            {
                return retValue;
            }

            return _lazyDic.Value.ContainsKey(_requestMediaDto.BusinessType)
                       ? _lazyDic.Value[_requestMediaDto.BusinessType].Invoke(string.Empty)
                       : CreateFailMessage(retValue, "50001", "请输入合法的BusinessType参数");
        }

        public dynamic QueryInfo()
        {
            var retValue = VerifyOfNecessaryParameters<RequestGetMeidaInfoDto>(_requestGetDto);
            if (retValue.HasError)
            {
                if (_requestGetDto.BusinessType == (int)MediaType.WeiXin)
                {
                    //由于早期的考虑问题不全面，微信架构业务逻辑改了，还是用于早期的调用逻辑
                    if (_requestGetDto.Version >= 118)
                    {
                        //微信的架构不全面，得慢慢的修改，先从1.1.8开始修改
                    }
                    else
                    {
                        return retValue;
                    }
                }
            }

            return _lazyQueryDic.Value.ContainsKey(_requestGetDto.BusinessType)
                     ? _lazyQueryDic.Value[_requestGetDto.BusinessType].Invoke(string.Empty)
                      : CreateFailMessage(retValue, "50002", "请输入合法的BusinessType参数");
        }

        public dynamic GetItem()
        {
            var retValue = VerifyOfNecessaryParameters<RequestGetCommonlyCalssDto>(_requestGetItemDto);
            if (retValue.HasError)
            {
                return retValue;
            }
            return _lazyQueryItemDic.Value.ContainsKey(_requestGetItemDto.BusinessType)
                   ? _lazyQueryItemDic.Value[_requestGetItemDto.BusinessType].Invoke(string.Empty)
                   : CreateFailMessage(retValue, "50003", "请输入合法的BusinessType参数");
        }
    }
}