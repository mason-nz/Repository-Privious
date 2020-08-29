using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.ErrorException;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business
{
    /// <summary>
    /// Auth:李雄
    /// 媒体新增、编辑接口操作代理类
    /// </summary>
    public class MediaBusinessProxy
    {
        private readonly RequestMediaPublicParam _requestMediaPublicParam;
        private readonly RequestMediaWeiXinDto _requestMediaWeiXinDto;
        private readonly RequestMediaWeiBoDto _requestMediaWeiBoDto;
        private readonly RequestMediaPcAppDto _requestMediaPcAppDto;
        private readonly RequestMediaVideoDto _requestMediaVideoDto;
        private readonly RequestMediaBroadcastDto _requestMediaBroadcastDto;

        private Dictionary<int, Func<string, ReturnValue>> _dictionary;

        /// <summary>
        /// 这里可直接将 NameValueCollection 作为参数传进来统一处理，外面调用即不需要GetMediaOperateRetValue方法
        /// 由于不好写单元测试，so直接将序列化完成的dto传进来
        /// </summary>
        /// <param name="publicParam"></param>
        /// <param name="requestMediaWeiXin"></param>
        /// <param name="requestMediaWeiBo"></param>
        /// <param name="requestMediaPcApp"></param>
        /// <param name="requestMediaVideo"></param>
        /// <param name="requestMediaBroadcast"></param>
        public MediaBusinessProxy(RequestMediaPublicParam publicParam, RequestMediaWeiXinDto requestMediaWeiXin,
            RequestMediaWeiBoDto requestMediaWeiBo, RequestMediaPcAppDto requestMediaPcApp,
            RequestMediaVideoDto requestMediaVideo,
            RequestMediaBroadcastDto requestMediaBroadcast)
        {
            _requestMediaPublicParam = publicParam;
            _requestMediaWeiXinDto = requestMediaWeiXin;
            _requestMediaWeiBoDto = requestMediaWeiBo;
            _requestMediaPcAppDto = requestMediaPcApp;
            _requestMediaVideoDto = requestMediaVideo;
            _requestMediaBroadcastDto = requestMediaBroadcast;
            Init();
        }

        private void Init()
        {
            _dictionary = new Dictionary<int, Func<string, ReturnValue>>()
            {
                {(int)MediaType.WeiXin,s => OperateWeiXin()},
                {(int)MediaType.WeiBo,s => OperateWeiBo()},
                {(int)MediaType.Video,s => OperateVideo()},
                {(int)MediaType.APP,s => OperateApp()},
                {(int)MediaType.Broadcast,s => OperateBroadcast()}
            };
        }

        /// <summary>
        /// 媒体操作新增，编辑执行方法
        /// </summary>
        /// <returns></returns>
        public ReturnValue Excute()
        {
            if (_dictionary.ContainsKey(_requestMediaPublicParam.BusinessType))
            {
                return _dictionary[_requestMediaPublicParam.BusinessType].Invoke(string.Empty);
            }
            return new ReturnValue() { HasError = true, ErrorCode = "-1", Message = "请传入合法的媒体类型BusinessType" };
        }

        private ReturnValue OperateWeiXin()
        {
            var business = new WeiXinOperate(_requestMediaPublicParam, _requestMediaWeiXinDto);

            return _requestMediaPublicParam.OperateType == (int)OperateType.Insert ? business.Create()
                : business.Update();
        }

        private ReturnValue OperateWeiBo()
        {
            var business = new WeiBoOperate(_requestMediaPublicParam, _requestMediaWeiBoDto);

            return _requestMediaPublicParam.OperateType == (int)OperateType.Insert ? business.Create()
                : business.Update();
        }

        private ReturnValue OperateApp()
        {
            var business = new PcAppOperate(_requestMediaPublicParam, _requestMediaPcAppDto);

            return _requestMediaPublicParam.OperateType == (int)OperateType.Insert ? business.Create()
                : business.Update();
        }

        private ReturnValue OperateVideo()
        {
            var business = new VideoOperate(_requestMediaPublicParam, _requestMediaVideoDto);
            return _requestMediaPublicParam.OperateType == (int)OperateType.Insert ? business.Create()
                : business.Update();
        }

        private ReturnValue OperateBroadcast()
        {
            var business = new BroadcastOperate(_requestMediaPublicParam, _requestMediaBroadcastDto);
            return _requestMediaPublicParam.OperateType == (int)OperateType.Insert ? business.Create()
           : business.Update();
        }
    }
}