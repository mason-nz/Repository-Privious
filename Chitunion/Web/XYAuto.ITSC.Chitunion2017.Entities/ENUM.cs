using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.Utils;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    /// <summary>
    /// 智投项目
    /// </summary>
    public enum EnumIntelligenceADOrderInfoCrudOptType
    {
        [EnumTextValue("新增智投项目")]
        ADD = 1,
        [EnumTextValue("修改智投项目")]
        Modify = 2,
        [EnumTextValue("智投填写项目需求")]
        ADDADOrderNote = 3
    }
    /// <summary>
    /// 订单状态
    /// </summary>
    public enum EnumOrderStatus
    {
        [EnumTextValue("草稿")]
        Draft = 16001,
        [EnumTextValue("待审")]
        PendingAudit = 16002,
        [EnumTextValue("驳回")]
        Reject = 16006,
        [EnumTextValue("取消")]
        Cancel = 16005,
        [EnumTextValue("待执行")]
        PendingExecute = 16003,
        [EnumTextValue("执行中")]
        Executing = 16004,
        [EnumTextValue("执行完毕")]
        Executed = 16008,
        [EnumTextValue("订单完成")]
        OrderFinished = 16009,
        [EnumTextValue("删除")]
        Deleted = 16007
    }

    /// <summary>
    /// 允许接口修改的订单状态
    /// </summary>
    public enum EnumInterfaceOrderStatus
    {        
        [EnumTextValue("取消")]
        Cancel = 16005,
        [EnumTextValue("待执行")]
        PendingExecute = 16003,
        [EnumTextValue("执行中")]
        Executing = 16004,
        [EnumTextValue("执行完毕")]
        Executed = 16008,
        [EnumTextValue("订单完成")]
        OrderFinished = 16009,
        [EnumTextValue("删除")]
        Deleted = 16007
    }

    /// <summary>
    /// 订单提交修改2个状态
    /// </summary>
    public enum EnumAddModify
    {
        [EnumTextValue("新增")]
        ADD = 1,
        [EnumTextValue("修改")]
        Modify = 2
    }
    /// <summary>
    /// 刊例状态
    /// </summary>
    public enum EnumPublishStatus
    {
        [EnumTextValue("新建")]
        New = 15001,
        [EnumTextValue("待审核")]
        PendingAudit = 15002,
        [EnumTextValue("已通过")]
        Passed = 15003,
        [EnumTextValue("驳回")]
        Reject = 15004,
        [EnumTextValue("已上架")]
        OnSold = 15005,
        [EnumTextValue("已下架")]
        UnSold = 15006
    }

    /// <summary>
    /// 审核操作类型状态
    /// </summary>
    public enum EnumOptType
    {
        [EnumTextValue("通过")]
        Passed = 27001,
        [EnumTextValue("驳回")]
        Reject = 27002
    }

    public enum EnumMediaType
    {
        [EnumTextValue("微信公众号")]
        WeChat = 14001,
        [EnumTextValue("APP")]
        APP = 14002,
        [EnumTextValue("新浪微博")]
        SinaWeibo = 14003,
        [EnumTextValue("视频")]
        Video = 14004,
        [EnumTextValue("直播")]
        Broadcast = 14005
    }
    public enum EnumCPTCPM
    {
        [EnumTextValue("CPT")]
        CPT = 11001,
        [EnumTextValue("CPM")]
        CPM = 11002
    }

    public enum EnumJSONCartOptType
    {
        [EnumTextValue("新增")]
        ADD = 1,
        [EnumTextValue("删除")]
        Delete = 2,
        [EnumTextValue("清空")]
        ClearAll = 3
    }

    public enum EnumResourceType
    {
        [EnumTextValue("媒体和刊例")]
        MediaORPublish = 1,
        [EnumTextValue("项目")]
        ADOrderInfo = 2,
        [EnumTextValue("订单")]
        SubADInfo = 3,


        媒体存在验证 = 10,
        添加刊例,
        修改刊例
    }

    public enum EnumRoleInfo
    {
        [EnumTextValue("超级管理员")]
        SYS001RL00001 = 1,
        [EnumTextValue("广告主")]
        SYS001RL00002 = 2,
        [EnumTextValue("媒体主")]
        SYS001RL00003 = 3,
        [EnumTextValue("运营")]
        SYS001RL00004 = 4,
        [EnumTextValue("AE")]
        SYS001RL00005 = 5,
        [EnumTextValue("策划")]
        SYS001RL00006 = 6
    }

    public enum EnumWeChatOperateMsg
    {
        媒体审核 = 46001,
        刊例审核 = 46002,
        刊例过期 = 46003,
        广告下架 = 46004,
        模板审核 = 46005,
        待审核 = 43001,
        已通过 = 43002,
        驳回 = 43003,
        模板待审核 = 48001,
        模板已通过 = 48002,
        模板驳回 = 48003,
        刊例过期1天 = 42007,
        刊例过期3天 = 42008,
        刊例过期7天 = 42009,
        刊例过期30天 = 42010,        
        发布刊例 = 1001,
        修改 = 1002,
        看看 = 1003,
        去审 = 1004,
        无链接 = 1000,
    }

    public enum EnumWeChatAuditStatus
    {        
        待审核 = 43001,
        已通过 = 43002,
        驳回 = 43003
    }
}
