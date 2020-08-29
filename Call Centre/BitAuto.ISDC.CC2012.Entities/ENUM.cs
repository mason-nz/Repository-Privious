using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// <summary>
    /// 客户回访-访问信息->访问方式
    /// </summary>
    [Serializable]
    public enum EnumRVType
    {
        [EnumTextValue("短信联系")]
        SMSContact = 1,
        [EnumTextValue("电话联系")]
        PhoneContact = 2,
        [EnumTextValue("发送传真")]
        SendFax = 3,
        [EnumTextValue("电子邮件")]
        SendEmail = 4,
        [EnumTextValue("信件邮递")]
        LetterPost = 5,
        [EnumTextValue("一般会议")]
        GeneralMeeting = 6,
        [EnumTextValue("上门拜访")]
        HomeVisits = 7,
        [EnumTextValue("网络即时通信")]
        InstantMsg = 8
    }

    /// <summary>
    /// 任务状态
    /// </summary>
    [Serializable]
    public enum EnumTaskStatus
    {
        [EnumTextValue("待处理")]
        TaskStatusWait = 150001,

        [EnumTextValue("处理中")]
        TaskStatusNow = 150002,

        [EnumTextValue("已处理")]
        TaskStatusOver = 150003
    }

    /// <summary>
    /// 问题性质
    /// </summary>
    [Serializable]
    public enum QuestionNature
    {
        [EnumTextValue("紧急")]
        NatureUrgent = 160004,

        [EnumTextValue("普通")]
        NatureCommon = 160005
    }

    /// <summary>
    /// 处理状态
    /// </summary>
    [Serializable]
    public enum ProcessStatus
    {
        [EnumTextValue("已解决")]
        ProcessSolve = 110001,

        [EnumTextValue("未解决")]
        ProcessUnresolved = 110003,

        [EnumTextValue("不解决")]
        ProcessNotSolve = 110002
    }

    /// <summary>
    /// 咨询类型
    /// </summary>
    [Serializable]
    public enum ConsultType
    {
        [EnumTextValue("新车")]
        NewCar = 60001,

        [EnumTextValue("二手车")]
        SecondCar = 60002,

        [EnumTextValue("个人反馈")]
        PFeedback = 60003,

        //[EnumTextValue("活动")]
        //Activity = 60004,

        [EnumTextValue("个人用车")]
        PUseCar = 60005,

        [EnumTextValue("个人其他")]
        POther = 60006,

        [EnumTextValue("经销商合作")]
        DCoop = 60007,

        [EnumTextValue("经销商反馈")]
        DCoopFeedback = 60008,

        [EnumTextValue("经销商其他")]
        DCoopOther = 60009
    }

    /// <summary>
    /// 动作
    /// </summary>
    [Serializable]
    public enum Action
    {
        [EnumTextValue("申请转出")]
        ActionApplyTurn = 120001,

        [EnumTextValue("提交")]
        ActionSumbit = 120003,

        [EnumTextValue("转出")]
        ActionTurnOut = 120004,

        [EnumTextValue("结束")]
        ActionTurnOver = 120005,

        [EnumTextValue("同意转出")]
        ActionAgreeApplyTurn = 120006,
        [EnumTextValue("呼出")]
        ActionCallOut = 120007
    }

    /// <summary>
    /// 客户职业
    /// </summary>
    [Serializable]
    public enum CustVocation
    {
        [EnumTextValue("一般职业")]
        normal = 130001,
        [EnumTextValue("农牧业")]
        Agricultureing = 130002,
        [EnumTextValue("渔业")]
        Fisheriesing = 130003,
        [EnumTextValue("木材森林业")]
        Forestrying = 130004,
        [EnumTextValue("矿业采掘业")]
        Mining = 130005,
        [EnumTextValue("交通运输业")]
        Transporting = 130006,
        [EnumTextValue("餐饮旅游业")]
        Dining = 130007,
        [EnumTextValue("建筑工程")]
        Building = 130008,
        [EnumTextValue("制造加工维修业")]
        Manufacturing = 130009,
        [EnumTextValue("出版广告业")]
        Publishing = 130010,
        [EnumTextValue("医药卫生保健")]
        Medicine = 130011,
        [EnumTextValue("娱乐业")]
        Recreation = 130012,
        [EnumTextValue("文教机构")]
        Educational = 130013,
        [EnumTextValue("宗教机构")]
        Religious = 130014,
        [EnumTextValue("邮政通信电力自来水")]
        Municipal = 130015,
        [EnumTextValue("零售批发业")]
        Retail = 130016,
        [EnumTextValue("金融保险证券")]
        Financial = 130017,
        [EnumTextValue("家庭管理")]
        Home = 130018,
        [EnumTextValue("公检法等执法检查机关")]
        Law = 130019,
        [EnumTextValue("军人")]
        Soldier = 130020,
        [EnumTextValue("IT业（软硬件开发制作）")]
        IT = 130021,
        [EnumTextValue("职业运动")]
        Sports = 130022,
        [EnumTextValue("无业人员")]
        Unemployed = 130023,
        [EnumTextValue("其他")]
        Other = 130024
    }
    /// <summary>
    /// 客户收入
    /// </summary>
    [Serializable]
    public enum CustInCome
    {
        [EnumTextValue("1000元/月以下")]
        OneK = 140001,
        [EnumTextValue("1000-2000元/月")]
        TwoK = 140002,
        [EnumTextValue("2001-4000元/月")]
        ThridK = 140003,
        [EnumTextValue("4001-6000元/月")]
        FourK = 140004,
        [EnumTextValue("6001-8000元/月")]
        FiveK = 140005,
        [EnumTextValue("8001-10000元/月")]
        SixK = 140006,
        [EnumTextValue("10001-15000元/月")]
        SevenK = 140007,
        [EnumTextValue("15001-25000元/月")]
        EightK = 140008,
        [EnumTextValue("25000元/月以上")]
        NineK = 140009,
        [EnumTextValue("保密")]
        TenK = 140010
    }

    #region 客户联系信息相关枚举

    /// <summary>
    /// 新车中的购车时间
    /// </summary>
    [Serializable]
    public enum NewCarBuyTime
    {

        [EnumTextValue("一周内")]
        ThisWeekly = 50001,

        [EnumTextValue("一月内")]
        ThisMonth = 50002,

        [EnumTextValue("半年内")]
        HalfYear = 50003,

        [EnumTextValue("无计划")]
        UnPlanned = 50004

    }

    /// <summary>
    /// 二手车下二级分类
    /// </summary>
    [Serializable]
    public enum SecondCarType
    {
        [EnumTextValue("买车")]
        BuyCar = 70001,

        [EnumTextValue("卖车")]
        SaleCar = 70002,

        [EnumTextValue("删除")]
        DelCar = 70003
    }

    /// <summary>
    /// 个人反馈下的二级分类
    /// </summary>
    [Serializable]
    public enum FeedBackTyle
    {
        [EnumTextValue("论坛")]
        BBS = 80001,

        [EnumTextValue("编辑")]
        WebEditor = 80002,

        [EnumTextValue("经销商")]
        Dealer = 80003,

        [EnumTextValue("产品")]
        Product = 80004,

        [EnumTextValue("活动")]
        Active = 80005,

        [EnumTextValue("呼叫中心")]
        CallCenter = 80006

    }

    /// <summary>
    /// 活动下的二级分类
    /// </summary>
    [Serializable]
    public enum ActiveType
    {
        [EnumTextValue("活动前")]
        AfterActive = 1,

        [EnumTextValue("活动后")]
        BeforeActive = 2

    }

    /// <summary>
    /// 个人用车下二级分类
    /// </summary>
    [Serializable]
    public enum PersonalCarType
    {
        [EnumTextValue("信贷")]
        Credit = 90001,

        [EnumTextValue("保险")]
        Insure = 90002,

        [EnumTextValue("养护维修")]
        Repair = 90003,

        [EnumTextValue("自驾游")]
        TravelMyself = 90004,

        [EnumTextValue("其他")]
        Other = 90005

    }

    /// <summary>
    /// 经销商合作下的二级分类
    /// </summary>
    [Serializable]
    public enum DealerType
    {
        [EnumTextValue("新车")]
        NewCar = 100001,

        [EnumTextValue("二手车")]
        SecondCar = 100002,

        [EnumTextValue("汽车用品周边")]
        AutoParts = 100003,

        [EnumTextValue("DSA")]
        DSA = 100006,
    }

    /// <summary>
    /// 经销商反馈下的二级分类
    /// </summary>
    [Serializable]
    public enum DealerTypeDCoop
    {
        [EnumTextValue("咨询")]
        Consult = 100004,

        [EnumTextValue("投诉")]
        Complain = 100005
    }



    /// <summary>
    /// 经销商反馈中的来电渠道
    /// </summary>
    [Serializable]
    public enum TelSource
    {
        [EnumTextValue("易湃")]
        YiPai = 100001,

        [EnumTextValue("DSA")]
        DSA = 100002

    }

    /// <summary>
    /// 经销商反馈中的问题内容
    /// </summary>
    [Serializable]
    public enum CallRecordType
    {
        [EnumTextValue("硬件")]
        HardWare = 100001,

        [EnumTextValue("软件")]
        SoftWare = 100002,

        [EnumTextValue("服务")]
        Service = 100003,

    }


    #endregion

    #region 客户扩展信息

    /// <summary>
    /// 城市范围
    /// </summary>
    [Serializable]
    public enum CityScope
    {
        [EnumTextValue("111城区")]
        CityArea = 10001,
        [EnumTextValue("111郊区")]
        SuburbsArea = 10002,
        [EnumTextValue("6城区")]
        SixALLCityArea = 10003,
        [EnumTextValue("224城区")]
        ALLCityArea = 10004
    }


    /// <summary>
    /// 经销商类别
    /// </summary>
    [Serializable]
    public enum DealerCategory
    {
        //[EnumTextValue("厂商")]
        //Company = 20001,
        //[EnumTextValue("厂商大区")]
        //VendorRegion = 20002,
        //[EnumTextValue("集团")]
        //Group = 20003,
        [EnumTextValue("4s")]
        fourS = 20004,
        [EnumTextValue("特许经销商")]
        Franchisees = 20005,
        [EnumTextValue("综合店")]
        IntegratedShop = 20006,
        //[EnumTextValue("汽车服务商")]
        //AutomotiveService = 20007,
        //[EnumTextValue("展厅")]
        //Hall = 20008,
        //[EnumTextValue("个人")]
        //Personal = 20009,
        [EnumTextValue("经纪公司")]
        Economics = 20010
        //[EnumTextValue("交易市场")]
        //TradingMarket = 20011,
        //[EnumTextValue("其他")]
        //Others = 20012
    }
    /// <summary>
    /// 经营范围
    /// </summary>
    [Serializable]
    public enum CarType
    {
        [EnumTextValue("新车")]
        NewCar = 30001,
        [EnumTextValue("二手车")]
        SecondCar = 30002,
        [EnumTextValue("新车/二手车")]
        ALLCar = 30003
    }

    /// <summary>
    /// 经销商状态
    /// </summary>
    [Serializable]
    public enum MemberStatus
    {
        [EnumTextValue("会员页")]
        MembersPage = 40001,
        [EnumTextValue("旺店页")]
        ShopPage = 40002,
        [EnumTextValue("待创建")]
        WaitCreate = 40003
    }

    [Serializable]
    public enum EnumArea
    {
        //mod by yangyh 把北京大区和北方大区合并
        //[EnumTextValue("北京大区")]
        //BeiJing = 170001,
        [EnumTextValue("北京北方大区")]
        BeiFang = 170002,
        [EnumTextValue("南方大区")]
        NanFang = 170003,
        [EnumTextValue("华东大区")]
        HuaDong = 170004,
        [EnumTextValue("西部大区")]
        XiBu = 170005
    }
    /// <summary>
    /// Modify=masj，这个枚举之前放在CustBaseInfo表中保存，由于业务调整，现在已经不维护，前端已经隐藏掉，不用了
    /// </summary>
    [Serializable]
    public enum EnumDataSource
    {
        //modify by qizq 2012-12-20 把呼叫中心替换成168,719
        //[EnumTextValue("呼叫中心")]
        //CC = 180001,

        [EnumTextValue("168")]
        CC = 180001,
        [EnumTextValue("719")]
        CC719 = 180006,

        [EnumTextValue("在线")]
        Online = 180002,
        [EnumTextValue("汽车通")]
        CarPass = 180003,
        [EnumTextValue("车易通")]
        CMS = 180004,
        [EnumTextValue("易湃")]
        EP = 180005
    }

    public enum EnumOptStatus
    {
        [EnumTextValue("保存")]
        NoSubmit = 200001,
        [EnumTextValue("提交")]
        WaitingAudit = 200002,
        [EnumTextValue("审核通过")]
        Release = 200003,
        [EnumTextValue("停用")]
        Stop = 200004,
        [EnumTextValue("驳回")]
        Reject = 200005,
        [EnumTextValue("删除")]
        Delete = 200006
    }
    #endregion

    #region 试卷库管理相关

    /// <summary>
    /// 试卷状态
    /// </summary>
    [Serializable]
    public enum ExamPaperState
    {
        [EnumTextValue("未使用")]
        NotUsed = 0,
        [EnumTextValue("已使用")]
        InUsed = 1,
        [EnumTextValue("未完成")]
        NotComplete = 2,
        //[EnumTextValue("删除")]
        //Delete = -1
    }

    #endregion

    #region 无主订单相关

    /// <summary>
    /// 车辆颜色
    /// </summary>
    [Serializable]
    public enum CarColor
    {
        请选择颜色 = -1,
        黑色,
        银灰色,
        白色,
        红色,
        蓝色,
        深灰色,
        香槟色,
        绿色,
        黄色,
        橙色,
        咖啡色,
        紫色,
        多彩色
    }
    /// <summary>
    /// 任务操作：操作状态
    /// </summary>
    [Serializable]
    public enum OperationStatus
    {
        [EnumTextValue("分配")]
        Allocation = 1,
        [EnumTextValue("保存")]
        Save,
        [EnumTextValue("回收")]
        Recover,
        [EnumTextValue("提交")]
        Submit,
        [EnumTextValue("呼出")]
        CallOut,
        [EnumTextValue("删除")]
        Delete
    }


    /// <summary>
    /// 任务操作：任务状态
    /// </summary>
    [Serializable]
    public enum TaskStatus
    {
        [EnumTextValue("待分配")]
        NoAllocation = 1,
        [EnumTextValue("待处理")]
        NoProcess,
        [EnumTextValue("处理中")]
        Processing,
        [EnumTextValue("已处理")]
        Processed
    }

    [Serializable]
    public enum NoDealerReason
    {
        [EnumTextValue("不接受转换")]
        Reason1 = 1,
        [EnumTextValue("已去别处下单")]
        Reason2 = 2,
        [EnumTextValue("购车意向不明确")]
        Reason3 = 3,
        [EnumTextValue("无效沟通")]
        Reason4 = 4,
        [EnumTextValue("数据重复")]
        Reason5 = 5
    }


    #endregion

    #region 业务调查

    /// <summary>
    /// 问题类型
    /// </summary>
    [Serializable]
    public enum AskCategory
    {
        [EnumTextValue("单选")]
        RadioT = 1,
        [EnumTextValue("复选")]
        CheckBoxT = 2,
        [EnumTextValue("文本")]
        TextT = 3,
        [EnumTextValue("矩阵单选")]
        MatrixRadioT = 4,
        [EnumTextValue("矩阵评分题")]
        MatrixDropDownT = 5
    }

    #endregion

    #region 话务管理-作废 使用ProjectSource替换
    //qiangfei 2016-7-31

    /// 电话记录所属 任务分类 作废
    /// <summary>
    /// 电话记录所属 任务分类 作废
    /// </summary>
    [Serializable]
    public enum TaskTypeID
    {
        [EnumTextValue("数据清洗")]
        DataCleansing = 1,
        [EnumTextValue("无主订单")]
        NoDealerOrder = 2,
        [EnumTextValue("个人业务")]
        TaskProcess = 3,
        [EnumTextValue("客户回访")]
        ReturnVisit = 4,
        [EnumTextValue("其他任务")]
        OtherTask = 5,
        [EnumTextValue("客户核实")]
        StopCustTask = 6,
        [EnumTextValue("团购订单")]
        GroupOrderTask = 7
    }
    #endregion

    /// <summary>
    /// 任务状态 对应旧呼叫中心系统的枚举EnumTaskStatus
    /// </summary>
    [Serializable]
    public enum EnumProjectTaskStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [EnumTextValue("未处理")]
        NoAssign = 180000,

        /// <summary>
        /// 处理中
        /// </summary>
        [EnumTextValue("处理中")]
        Assigning = 180001,

        ///// <summary>
        ///// 审核拒绝
        ///// </summary>
        //[EnumTextValue("审核拒绝")]
        //AuditRefuse = 180002,

        /// <summary>
        /// 提交完成
        /// </summary>
        [EnumTextValue("提交完成")]
        SubmitFinsih = 180003,

        /// <summary>
        /// 删除完成
        /// </summary>
        [EnumTextValue("删除完成")]
        DelFinsh = 180004,

        ///// <summary>
        ///// 审核提交成功
        ///// </summary>
        //[EnumTextValue("审核提交成功")]
        //SubmitManageSuccess = 180005,

        ///// <summary>
        ///// 审核提交失败
        ///// </summary>
        //[EnumTextValue("审核提交失败")]
        //SubmitManageFail = 180006,

        ///// <summary>
        ///// 审核删除成功
        ///// </summary>
        //[EnumTextValue("审核删除成功")]
        //DelSuccess = 180007,

        ///// <summary>
        ///// 审核删除失败
        ///// </summary>
        //[EnumTextValue("审核删除失败")]
        //DelFail = 180008,

        ///// <summary>
        ///// 已驳回
        ///// </summary>
        //[EnumTextValue("已驳回")]
        //Rejected = 180009,

        /// <summary>
        /// 停用
        /// </summary>
        [EnumTextValue("停用")]
        Stop = 180010,

        ///// <summary>
        ///// 审核停用成功
        ///// </summary>
        //[EnumTextValue("停用成功")]
        //StopSuccess = 180011,

        /// <summary>
        /// 未分配
        /// </summary>
        [EnumTextValue("未分配")]
        NoSelEmployee = 180012,

        /// <summary>
        /// 待复核
        /// </summary>
        [EnumTextValue("待复核")]
        StayReview = 180014,

        /// <summary>
        /// 已完成
        /// </summary>
        [EnumTextValue("已完成")]
        Finshed = 180015,

        /// <summary>
        /// 已结束
        /// </summary>
        [EnumTextValue("已结束")]
        STOPTask = 180016

    }
    /// <summary>
    /// 项目任务操作状态
    /// </summary>
    [Serializable]
    public enum EnumProjectTaskOperationStatus
    {
        [EnumTextValue("分配")]
        TaskAllot = 1,
        [EnumTextValue("收回")]
        TaskBack = 2,
        [EnumTextValue("保存")]
        TaskSave = 3,
        [EnumTextValue("删除")]
        TaskDel = 4,
        [EnumTextValue("提交")]
        TaskSubmit = 5,
        [EnumTextValue("审核通过")]
        TaskAuditSuccess = 6,
        [EnumTextValue("审核拒绝")]
        TaskAuditReject = 7,
        [EnumTextValue("复核通过")]
        TaskReviewSuccess = 8,
        [EnumTextValue("复核拒绝")]
        TaskReviewReject = 9,
        [EnumTextValue("结束")]
        TaskFinish = 10,
        [EnumTextValue("打回")]
        TaskCallBack = 11
    }

    /// <summary>
    /// 业务类型
    /// </summary>
    [Serializable]
    public enum BusinessType
    {
        /// <summary>
        /// 新车
        /// </summary>
        [EnumTextValue("新车")]
        NewCar = 1,
        /// <summary>
        /// 二手车
        /// </summary>
        [EnumTextValue("二手车")]
        HandCar = 2,
        /// <summary>
        /// 易卡
        /// </summary>
        [EnumTextValue("易卡")]
        YiKa = 4
    }

    #region 模板定制

    /// <summary>
    /// 控件显示类型
    /// </summary>
    [Serializable]
    public enum EnumTFieldShow
    {
        /// <summary>
        /// 单行文本
        /// </summary>
        [EnumTextValue("单行文本")]
        SingleText = 100001,

        /// <summary>
        /// 多行文本
        /// </summary>
        [EnumTextValue("多行文本")]
        MultiRowText = 100002,

        /// <summary>
        /// 单选
        /// </summary>
        [EnumTextValue("单选")]
        Radio = 100003,

        /// <summary>
        /// 复选
        /// </summary>
        [EnumTextValue("复选")]
        Check = 100004,

        /// <summary>
        /// 下拉框
        /// </summary>
        [EnumTextValue("下拉")]
        Select = 100005,

        /// <summary>
        /// 电话号码
        /// </summary>
        [EnumTextValue("电话号码")]
        Tel = 100006,

        /// <summary>
        /// 邮箱
        /// </summary>
        [EnumTextValue("邮箱")]
        Email = 100007,

        /// <summary>
        /// 日期点
        /// </summary>
        [EnumTextValue("日期点")]
        DataPoint = 100008,

        /// <summary>
        /// 日期段
        /// </summary>
        [EnumTextValue("日期段")]
        DataBetween = 100009,


        /// <summary>
        /// 时间点
        /// </summary>
        [EnumTextValue("时间点")]
        TimePoint = 100010,

        /// <summary>
        /// 时间段
        /// </summary>
        [EnumTextValue("时间段")]
        TimeBetween = 100011,

        /// <summary>
        /// 二级省市
        /// </summary>
        [EnumTextValue("二级省市")]
        ProAndCity = 100012,

        /// <summary>
        /// 三级省市县
        /// </summary>
        [EnumTextValue("三级省市县")]
        ProCityAndCoun = 100013,

        /// <summary>
        /// 客户ID
        /// </summary>
        [EnumTextValue("客户ID")]
        CustID = 100014,

        /// <summary>
        /// 个人用户
        /// </summary>
        [EnumTextValue("个人用户")]
        IndividualUser = 100015,

        /// <summary>
        /// 下单车型
        /// </summary>
        [EnumTextValue("下单车型")]
        XDBrandSerial = 100016,

        /// <summary>
        /// 意向车型
        /// </summary>
        [EnumTextValue("意向车型")]
        YXBrandSerial = 100017,

        /// <summary>
        /// 出售车型
        /// </summary>
        [EnumTextValue("出售车型")]
        CSBrandSerial = 100018,

        /// <summary>
        /// 推荐活动
        /// </summary>
        [EnumTextValue("推荐活动")]
        Activities = 100019,

        /// <summary>
        /// 话务结果
        /// </summary>
        [EnumTextValue("话务结果")]
        CallResult = 100020,
    }

    /// <summary>
    /// 控件数据类型
    /// </summary>
    [Serializable]
    public enum EnumTFieldType
    {
        /// <summary>
        /// 字符型
        /// </summary>
        [EnumTextValue("nvarchar")]
        Tnvarchar = 1,

        /// <summary>
        /// 浮点型
        /// </summary>
        [EnumTextValue("decimal")]
        Tdecimal = 2,

        /// <summary>
        /// 数字型
        /// </summary>
        [EnumTextValue("int")]
        Tint = 3,

        /// <summary>
        /// 时间类型
        /// </summary>
        [EnumTextValue("datetime")]
        Tdatetime = 4
    }

    /// <summary>
    /// 其他任务
    /// </summary>
    [Serializable]
    public enum OtheTaskStatus
    {
        /// <summary>
        /// 未分配
        /// </summary>
        [EnumTextValue("未分配")]
        Unallocated = 1,
        /// <summary>
        /// 未处理
        /// </summary>
        [EnumTextValue("未处理")]
        Untreated = 2,
        /// <summary>
        /// 处理中
        /// </summary>
        [EnumTextValue("处理中")]
        Processing = 3,
        /// <summary>
        /// 已处理
        /// </summary>
        [EnumTextValue("已处理")]
        Processed = 4,
        /// <summary>
        /// 已结束
        /// </summary>
        [EnumTextValue("已结束")]
        StopTask = 5
    }
    #endregion

    #region 质检评分

    /// <summary>
    /// 质检评分状态
    /// </summary>
    [Serializable]
    public enum QSResultStatus
    {
        /// <summary>
        /// 待评分
        /// </summary>
        [EnumTextValue("待评分")]
        WaitScore = 20001,
        /// <summary>
        /// 已提交
        /// </summary>
        [EnumTextValue("已提交")]
        Submitted = 20002,
        /// <summary>
        /// 待初审
        /// </summary>
        [EnumTextValue("待初审")]
        TobeFirstInstance = 20003,
        /// <summary>
        /// 待复审
        /// </summary>
        [EnumTextValue("待复审")]
        TobeReviewInstance = 20004,
        /// <summary>
        /// 已评分
        /// </summary>
        [EnumTextValue("已评分")]
        RatedScore = 20005,
        /// <summary>
        /// 已申诉
        /// </summary>
        [EnumTextValue("已申诉")]
        Claimed = 20006
    }

    /// <summary>
    /// 质检评分状态
    /// </summary>
    [Serializable]
    public enum QSApprovalType
    {
        /// <summary>
        /// 评分表保存
        /// </summary>
        [EnumTextValue("评分表保存")]
        TableSave = 30001,
        /// <summary>
        /// 评分表删除
        /// </summary>
        [EnumTextValue("评分表删除")]
        TableDel = 30002,
        /// <summary>
        /// 评分表提交
        /// </summary>
        [EnumTextValue("评分表提交")]
        TableSubmit = 30003,
        /// <summary>
        /// 评分表复审
        /// </summary>
        [EnumTextValue("评分表复审")]
        TableAduit = 30004,
        /// <summary>
        /// 质检成绩保存
        /// </summary>
        [EnumTextValue("质检成绩保存")]
        ScoreSave = 30005,
        /// <summary>
        /// 质检成绩提交
        /// </summary>
        [EnumTextValue("质检成绩提交")]
        ScoreSubmit = 30006,
        /// <summary>
        /// 质检成绩申诉
        /// </summary>
        [EnumTextValue("质检成绩申诉")]
        ScoreAppeal = 30007,
        /// <summary>
        /// 申诉初审
        /// </summary>
        [EnumTextValue("申诉初审")]
        AppealFirstAduit = 30008,
        /// <summary>
        /// 申诉复审
        /// </summary>
        [EnumTextValue("申诉复审")]
        AppealAgainAduit = 30009,
        /// <summary>
        /// 系统已评分
        /// </summary>
        [EnumTextValue("系统已评分")]
        SystemSet = 30010
    }

    /// <summary>
    /// 评分表状态
    /// </summary>
    [Serializable]
    public enum QSRulesTableStatus
    {
        /// <summary>
        /// 未完成
        /// </summary>
        [EnumTextValue("未完成")]
        Unfinished = 10001,
        /// <summary>
        /// 未审核
        /// </summary>
        [EnumTextValue("未审核")]
        Audit = 10002,
        /// <summary>
        /// 已完成
        /// </summary>
        [EnumTextValue("已完成")]
        Finished = 10003,
        [EnumTextValue("已删除")]
        Deleted = 10004
    }

    #endregion

    #region 客户核实
    /// 客户核实操作状态
    /// <summary>
    /// 客户核实操作状态
    /// </summary>
    [Serializable]
    public enum StopCustTaskOperStatus
    {
        /// <summary>
        /// 分配
        /// </summary>
        [EnumTextValue("分配")]
        Allocation = 1,
        /// <summary>
        /// 处理
        /// </summary>
        [EnumTextValue("处理")]
        Save = 2,
        /// <summary>
        /// 回收
        /// </summary>
        [EnumTextValue("回收")]
        Recover = 3,
        /// <summary>
        /// 提交
        /// </summary>
        [EnumTextValue("提交")]
        Finished = 4,

        /// <summary>
        /// 同步
        /// </summary>
        [EnumTextValue("同步")]
        Sync = 5,
        /// <summary>
        /// 停用
        /// </summary>
        [EnumTextValue("停用")]
        Disabled = 6,
        /// <summary>
        /// 驳回
        /// </summary>
        [EnumTextValue("驳回")]
        Reject = 7,
        /// <summary>
        /// 启用
        /// </summary>
        [EnumTextValue("启用")]
        Enable = 8
    }
    /// 客户核实任务状态
    /// <summary>
    /// 客户核实任务状态
    /// </summary>
    [Serializable]
    public enum StopCustTaskStatus
    {
        /// <summary>
        /// 未分配
        /// </summary>
        [EnumTextValue("未分配")]
        NoAllocation = 1,
        /// <summary>
        /// 待处理
        /// </summary>
        [EnumTextValue("待处理")]
        NoProcess = 2,
        /// <summary>
        /// 处理中
        /// </summary>
        [EnumTextValue("处理中")]
        Processing = 3,
        /// <summary>
        /// 已处理
        /// </summary>
        [EnumTextValue("已处理")]
        Sumbit = 4
    }
    /// 客户状态
    /// <summary>
    /// 客户状态
    /// </summary>
    [Serializable]
    public enum StopCustStopStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [EnumTextValue("待审核")]
        Pending = 1,
        /// <summary>
        /// 待停用/待启用
        /// </summary>
        [EnumTextValue("待停用/待启用")]
        NoDisabled = 2,
        /// <summary>
        /// 已停用/已启用
        /// </summary>
        [EnumTextValue("已停用/已启用")]
        Disabled = 3,
        /// <summary>
        /// 已驳回
        /// </summary>
        [EnumTextValue("已驳回")]
        Reject = 4,
        /// <summary>
        /// 待复议
        /// </summary>
        [EnumTextValue("待复议")]
        Reviewing = 5,
        /// <summary>
        /// 复议驳回
        /// </summary>
        [EnumTextValue("复议驳回")]
        ReviewReject = 6
    }
    /// 停用原因
    /// <summary>
    /// 停用原因
    /// </summary>
    [Serializable]
    public enum StopApplyReason
    {
        /// <summary>
        /// 重复
        /// </summary>
        [EnumTextValue("重复")]
        First = 360001,
        /// <summary>
        /// 此公司已倒闭
        /// </summary>
        [EnumTextValue("此公司已倒闭")]
        Second = 360002,
        /// <summary>
        /// 此公司已转行不再销售汽车
        /// </summary>
        [EnumTextValue("此公司已转行不再销售汽车")]
        Thrid = 360003,
        /// <summary>
        /// 此公司只销售重型卡车
        /// </summary>
        [EnumTextValue("此公司只销售重型卡车")]
        Four = 360004,
        /// <summary>
        /// 呼叫中心提报且区域同意删除
        /// </summary>
        [EnumTextValue("呼叫中心提报且区域同意删除")]
        Five = 360005,
        /// <summary>
        /// 区域复议
        /// </summary>
        [EnumTextValue("区域复议")]
        Six = 360007
    }
    /// 启用原因
    /// <summary>
    /// 启用原因
    /// </summary>
    [Serializable]
    public enum EnableApplyReason
    {
        /// <summary>
        /// 停用错误
        /// </summary>
        [EnumTextValue("停用错误")]
        First = 150001,
        /// <summary>
        /// 客户重新开业
        /// </summary>
        [EnumTextValue("客户重新开业")]
        Second = 150002,
        /// <summary>
        /// 客户经营新品牌
        /// </summary>
        [EnumTextValue("客户经营新品牌")]
        Thrid = 150003,
        /// <summary>
        /// 其他业务部门使用
        /// </summary>
        [EnumTextValue("其他业务部门使用")]
        Four = 150004,
        /// <summary>
        /// 不清楚停用原因
        /// </summary>
        [EnumTextValue("不清楚停用原因")]
        Five = 150005
    }
    /// 停用说明
    /// <summary>
    /// 停用说明
    /// </summary>
    [Serializable]
    public enum StopRemark
    {
        /// <summary>
        /// 只停用会员信息
        /// </summary>
        [EnumTextValue("只停用会员信息")]
        First = 370005,
        /// <summary>
        /// 停用会员信息同时停用客户信息
        /// </summary>
        [EnumTextValue("停用会员信息同时停用客户信息")]
        Second = 370006,
        /// <summary>
        /// 只停用客户信息
        /// </summary>
        [EnumTextValue("只停用客户信息")]
        Thrid = 370007
    }
    /// 启用说明
    /// <summary>
    /// 启用说明
    /// </summary>
    [Serializable]
    public enum EnableRemark
    {
        /// <summary>
        /// 只启用客户信息
        /// </summary>
        [EnumTextValue("只启用客户信息")]
        First = 190001,
        /// <summary>
        /// 只启用会员信息
        /// </summary>
        [EnumTextValue("只启用会员信息")]
        Second = 190002,
        /// <summary>
        /// 客户信息启用的同时启用会员信息
        /// </summary>
        [EnumTextValue("客户信息启用的同时启用会员信息")]
        Thrid = 190003
    }
    #endregion

    #region 工单

    /// <summary>
    /// 优先级
    /// </summary>
    [Serializable]
    public enum PriorityLevelEnum
    {
        /// <summary>
        /// 普通
        /// </summary>
        [EnumTextValue("普通")]
        Common = 1,
        /// <summary>
        /// 紧急
        /// </summary>
        [EnumTextValue("紧急")]
        Urgent = 2
    }




    #endregion

    #region 团购订单相关
    /// <summary>
    /// 任务操作：任务状态
    /// </summary>
    [Serializable]
    public enum GroupTaskStatus
    {
        [EnumTextValue("待分配")]
        NoAllocation = 1,
        [EnumTextValue("待处理")]
        NoProcess,
        [EnumTextValue("处理中")]
        Processing,
        [EnumTextValue("已处理")]
        Processed
    }

    /// <summary>
    /// 处理状态：是否回访
    /// </summary>
    [Serializable]
    public enum IsReturnVisit
    {
        [EnumTextValue("是")]
        Yes = 1,
        [EnumTextValue("否")]
        No = 2,
        [EnumTextValue("未知")]
        UnKnown = 0
    }

    /// <summary>
    /// 任务操作：操作状态
    /// </summary>
    [Serializable]
    public enum GO_OperationStatus
    {
        [EnumTextValue("分配")]
        Allocation = 1,
        [EnumTextValue("保存")]
        Save,
        [EnumTextValue("回收")]
        Recover,
        [EnumTextValue("提交")]
        Submit,
        [EnumTextValue("呼出")]
        CallOut
    }

    /// <summary>
    ///预计购车时间
    /// </summary>
    [Serializable]
    public enum GO_PlanBuyCarTime
    {
        [EnumTextValue("一周")]
        OneWeek = 1,
        [EnumTextValue("一个月")]
        OneMonth,
        [EnumTextValue("三个月")]
        ThreeMonths,
        [EnumTextValue("半年")]
        SixMonths,
        [EnumTextValue("半年以上")]
        MoreSixMonths
    }

    #endregion

    /// <summary>
    /// 发短信错误提示
    /// </summary>
    [Serializable]
    public enum SendSMSInfo
    {
        [EnumTextValue("Md5校验不合法")]
        One = -1,
        [EnumTextValue("传入的参数productcode不合法")]
        Two = -2,
        [EnumTextValue("记录发送信息入库错误")]
        Three = -3,
        [EnumTextValue("发送请求到短信通道的时候失败")]
        Four = -4,
        [EnumTextValue("短信发送成功，但通道返回数据无法记录")]
        Five = -5,
        [EnumTextValue("短信内包含非法字符")]
        Six = -6,
        [EnumTextValue("手机号码不合法")]
        Seven = -7,
        [EnumTextValue("未知错误")]
        Eight = -99
    }

    #region 线索处理相关
    /// <summary>
    /// 线索处理失败原因
    /// </summary>
    [Serializable]
    public enum LeadTaskFailReason
    {
        [EnumTextValue("无人接听/占线/无法接通")]
        One = 1,
        [EnumTextValue("空号/未接通挂断")]
        Two = 2,
        [EnumTextValue("停机/关机")]
        Three = 3,
        [EnumTextValue("已购车")]
        Four = 4,
        [EnumTextValue("非本人")]
        Five = 5,
        [EnumTextValue("拒绝参加活动/接通后挂断")]
        Six = 6,
        [EnumTextValue("暂无购车计划")]
        Seven = 7,
        [EnumTextValue("自行联系经销商")]
        Eight = 8,
        [EnumTextValue("异地订单")]
        Nine = 9,
        [EnumTextValue("其他")]
        Ten = 10
    }

    /// <summary>
    /// 任务操作：任务状态
    /// </summary>
    [Serializable]
    public enum LeadsTaskStatus
    {
        [EnumTextValue("待分配")]
        NoAllocation = 1,
        [EnumTextValue("待处理")]
        NoProcess,
        [EnumTextValue("处理中")]
        Processing,
        [EnumTextValue("已处理")]
        Processed,
        [EnumTextValue("已撤销")]
        ReBack
    }

    /// <summary>
    /// 线索处理任务操作
    /// </summary>
    [Serializable]
    public enum Leads_OperationStatus
    {
        [EnumTextValue("生成")]
        Gen = 1,
        [EnumTextValue("分配")]
        Allocation,
        [EnumTextValue("保存")]
        Save,
        [EnumTextValue("回收")]
        Recover,
        [EnumTextValue("提交")]
        Submit,
        [EnumTextValue("撤销")]
        End
    }


    /// <summary>
    /// 线索处理任务操作
    /// </summary>
    [Serializable]
    public enum LeadPlanBuyCarTime
    {
        [EnumTextValue("一个月内")]
        One = 1,
        [EnumTextValue("三个月内")]
        Two = 2,
        [EnumTextValue("半年内")]
        Three = 3,
        [EnumTextValue("一年以内")]
        Four = 4,
        [EnumTextValue("一年以上")]
        Five = 5,
        [EnumTextValue("不确定")]
        Six = 6
    }

    #endregion

    #region 易团购任务处理相关
    /// <summary>
    /// 易团购任务处理失败原因
    /// </summary>
    [Serializable]
    public enum YTGActivityTaskFailReason
    {
        [EnumTextValue("无人接听/占线/无法接通")]
        One = 1,
        [EnumTextValue("空号/未接通挂断")]
        Two = 2,
        [EnumTextValue("停机/关机")]
        Three = 3,
        [EnumTextValue("已购车")]
        Four = 4,
        [EnumTextValue("非本人")]
        Five = 5,
        [EnumTextValue("拒绝参加活动/接通后挂断")]
        Six = 6,
        [EnumTextValue("暂无购车计划")]
        Seven = 7,
        [EnumTextValue("自行联系经销商")]
        Eight = 8,
        [EnumTextValue("时间原因/异地订单")]
        Nine = 9,
        [EnumTextValue("其他")]
        Ten = 10,
        [EnumTextValue("无音/单通")]
        Eleven = 11,
        [EnumTextValue("一个月内外呼过")]
        Twelve = 12
    }

    /// <summary>
    /// 易团购任务状态
    /// </summary>
    [Serializable]
    public enum YTGActivityTaskStatus
    {
        [EnumTextValue("待分配")]
        NoAllocation = 1,
        [EnumTextValue("待处理")]
        NoProcess,
        [EnumTextValue("处理中")]
        Processing,
        [EnumTextValue("已处理")]
        Processed,
        [EnumTextValue("已撤销")]
        ReBack
    }

    /// <summary>
    ///易团购任务操作状态
    /// </summary>
    [Serializable]
    public enum YTGActivityOperationStatus
    {
        [EnumTextValue("生成")]
        Gen = 1,
        [EnumTextValue("分配")]
        Allocation,
        [EnumTextValue("保存")]
        Save,
        [EnumTextValue("回收")]
        Recover,
        [EnumTextValue("提交")]
        Submit,
        [EnumTextValue("撤销")]
        End
    }


    /// <summary>
    /// 易团购预计购车时间
    /// </summary>
    [Serializable]
    public enum YTGActivityPlanBuyCarTime
    {
        [EnumTextValue("一个月内")]
        One = 1,
        [EnumTextValue("三个月内")]
        Two = 2,
        [EnumTextValue("半年内")]
        Three = 3,
        [EnumTextValue("一年以内")]
        Four = 4,
        [EnumTextValue("一年以上")]
        Five = 5,
        [EnumTextValue("不确定")]
        Six = 6
    }
    /// <summary>
    /// 易团购活动状态
    /// </summary>
    [Serializable]
    public enum YTGActivityStatus
    {
        [EnumTextValue("删除")]
        MinusOne = -1,
        [EnumTextValue("未开始")]
        Zero = 0,
        [EnumTextValue("进行中")]
        One = 1,
        [EnumTextValue("已结束")]
        Two = 2,
        [EnumTextValue("已终止")]
        Three = 3,
    }

    #endregion

    #region RegionID
    /// <summary>
    /// QS_RuleTable分组
    /// </summary>
    [Serializable]
    public enum RegionID
    {
        [EnumTextValue("北京")]
        BeiJ = 1,
        [EnumTextValue("西安")]
        XiAn
    }
    #endregion

    #region 客户端
    /// 业务类型
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BusinessTypeEnum
    {
        [EnumTextValue("热线")]
        CC = 1,
        [EnumTextValue("在线")]
        IM = 2
    }
    /// 厂家列表
    /// <summary>
    /// 厂家列表
    /// </summary>
    public enum Vender { Genesys = 0, Holly = 1 }
    /// 坐席状态
    /// <summary>
    /// 坐席状态
    /// </summary>
    public enum AgentStateForListen
    {
        无 = 0,
        置忙 = 1,
        置闲 = 2,
        振铃 = 3,
        通话中 = 4,
        话后 = 5,
        离线 = 6
    }
    /// 操作类型
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperForListen
    {
        无 = 0,
        强制置忙 = 1,
        强制置闲 = 2,
        强制签出 = 3,
        监听 = 4,
        强拆 = 5,
        强插 = 6,
        接管 = 7
    }
    #endregion

    /// 项目日志
    /// <summary>
    /// 项目日志
    /// </summary>
    public enum ProjectLogOper
    {
        [EnumTextValue("新建项目")]
        L1_新建项目 = 1,
        [EnumTextValue("编辑项目")]
        L2_编辑项目 = 2,
        [EnumTextValue("导入数据")]
        L3_导入数据 = 3,
        [EnumTextValue("结束项目")]
        L4_结束项目 = 4,
        [EnumTextValue("修改问卷")]
        L5_修改问卷 = 5,
        [EnumTextValue("生成任务")]
        L6_生成任务 = 6,
        [EnumTextValue("启动项目")]
        L7_启动项目 = 7,

        [EnumTextValue("设置自动外呼")]
        Z1_设置自动外呼,
        [EnumTextValue("修改自动外呼")]
        Z2_修改自动外呼,
        [EnumTextValue("启动自动外呼")]
        Z3_启动自动外呼,
        [EnumTextValue("暂停自动外呼")]
        Z4_暂停自动外呼,
        [EnumTextValue("继续自动外呼")]
        Z5_继续自动外呼,
        [EnumTextValue("结束自动外呼")]
        Z6_结束自动外呼
    }
    /// 话务类型
    /// <summary>
    /// 话务类型
    /// </summary>
    public enum CallStatus
    {
        [EnumTextValue("呼入")]
        C01_呼入 = 1,
        [EnumTextValue("呼出")]
        C02_呼出 = 2,
        [EnumTextValue("转接")]
        C03_转接 = 3,
        [EnumTextValue("接管")]
        C04_接管 = 4
    }

    #region 黑名单
    /// 黑名单验证类型
    /// <summary>
    /// 黑名单验证类型
    /// </summary>
    public enum BlackListCheckType
    {
        [EnumTextValue("无验证")]
        BT0_None = 0,
        [EnumTextValue("CRM验证")]
        BT1_CRM = 1,
        [EnumTextValue("CC验证")]
        BT2_CC = 2,
        [EnumTextValue("全部验证")]
        BT3_ALL = BT1_CRM | BT2_CC
    }
    /// （黑名单）免打扰原因
    /// <summary>
    /// （黑名单）免打扰原因
    /// </summary>
    public enum NoDisturbReason
    {
        [EnumTextValue("客户主动要求不再联系")]
        NDR01 = 1,
        [EnumTextValue("疑似经销商/经纪人")]
        NDR02 = 2,
        [EnumTextValue("疑似调研咨询公司")]
        NDR03 = 3,
        [EnumTextValue("疑似公司内部员工测试")]
        NDR04 = 4,
        [EnumTextValue("其他")]
        NDR05 = 5
    }
    #endregion

    #region 自动外呼
    /// 自动外呼项目状态
    /// <summary>
    /// 自动外呼项目状态
    /// </summary>
    public enum ProjectACStatus
    {
        [EnumTextValue("未开始")]
        P00_未开始 = 0,
        [EnumTextValue("进行中")]
        P01_进行中 = 1,
        [EnumTextValue("暂停中")]
        P02_暂停中 = 2,
        [EnumTextValue("已结束")]
        P03_已结束 = 3
    }
    /// 自动外呼任务状态
    /// <summary>
    /// 自动外呼任务状态
    /// </summary>
    public enum TaskACStatus
    {
        [EnumTextValue("未开始")]
        T00_未开始 = 0,
        [EnumTextValue("锁定中")]
        T01_锁定中 = 1,
        [EnumTextValue("外呼中")]
        T02_外呼中 = 2,
        [EnumTextValue("已结束")]
        T03_已结束 = 3,
        [EnumTextValue("已完成")]
        T04_已完成 = 4,
        [EnumTextValue("未回写")]
        T05_未回写 = 5,
        [EnumTextValue("已回写")]
        T06_已回写 = 6
    }
    /// 任务明细结果
    /// <summary>
    /// 任务明细结果
    /// </summary>
    public enum DetailACResult
    {
        [EnumTextValue("未接通")]
        D00_未接通 = 0,
        [EnumTextValue("已接通")]
        D01_已接通 = 1
    }
    #endregion

    #region 话务结果
    /// 项目类型：任务结果表 CallResult_ORIG_Task，任务话务表CallRecord_ORIG_Task，来电去电表中的枚举 CallRecordInfo，短信表SMSSendHistory
    /// <summary>
    /// 项目类型：任务结果表 CallResult_ORIG_Task，任务话务表CallRecord_ORIG_Task，来电去电表中的枚举 CallRecordInfo，短信表SMSSendHistory
    /// </summary>
    public enum ProjectSource
    {
        [EnumTextValue(null)]
        None = -1,
        [EnumTextValue("客户核实")]
        S2_客户核实 = 2,
        [EnumTextValue("工单（客户回访）")]
        S3_工单 = 3,
        [EnumTextValue("其他任务")]
        S4_其他任务 = 4,
        [EnumTextValue("易集客")] //作废
        S5_易集客 = 5,
        [EnumTextValue("厂家集客")]
        S6_厂家集客 = 6,
        [EnumTextValue("易团购")] //作废
        S7_易团购 = 7
    }
    /// CustPhoneVisitBusiness表的任务类型
    /// <summary>
    /// CustPhoneVisitBusiness表的任务类型
    /// </summary>
    public enum VisitBusinessTypeEnum
    {
        [EnumTextValue(null)]
        None = -1,
        [EnumTextValue("其他系统")]
        S0_其他系统 = 0,
        [EnumTextValue("工单")]
        S1_工单 = 1,
        [EnumTextValue("客户核实")]
        S3_客户核实 = 3,
        [EnumTextValue("其他任务")]
        S4_其他任务 = 4,
        [EnumTextValue("易集客")] //作废
        S5_易集客 = 5,
        [EnumTextValue("厂家集客")]
        S6_厂家集客 = 6,
        [EnumTextValue("易团购")] //作废
        S7_易团购 = 7
    }
    /// 未接通原因
    /// <summary>
    /// 未接通原因
    /// </summary>
    public enum NotEstablishReason
    {
        [EnumTextValue("停机/空号/错号")]
        N01_停机空号错号 = 1,
        [EnumTextValue("关机/占线/无法接通")]
        N02_关机占线无法接通 = 2,
        [EnumTextValue("无人接听")]
        N03_无人接听 = 3,
        [EnumTextValue("未接通挂断")]
        N04_未接通挂断 = 4,
        [EnumTextValue("免打扰屏蔽")]
        N05_免打扰屏蔽 = 5
    }
    /// 未成功原因
    /// <summary>
    /// 未成功原因
    /// </summary>
    public enum NotSuccessReason
    {
        [EnumTextValue("用户拒绝")]
        N01_用户拒绝 = 1,
        [EnumTextValue("城市未覆盖")]
        N02_城市未覆盖 = 2,
        [EnumTextValue("车型未覆盖")]
        N03_车型未覆盖 = 3,
        [EnumTextValue("无贷款需求")]
        N04_无贷款需求 = 4,
        //[EnumTextValue("信用记录不符")]
        //N05_信用记录不符 = 5,
        [EnumTextValue("非本人")]
        N06_非本人 = 6,
        //[EnumTextValue("年龄不符")]
        //N07_年龄不符 = 7,
        [EnumTextValue("接通后挂断")]
        N08_接通后挂断 = 8,
        [EnumTextValue("一个月内外呼过")]
        N09_一个月内外呼过 = 9,
        [EnumTextValue("自行联系经销商")]
        N10_自行联系经销商 = 10,
        [EnumTextValue("考虑其他品牌")]
        N11_考虑其他品牌 = 11,
        [EnumTextValue("暂无购车计划")]
        N12_暂无购车计划 = 12,
        [EnumTextValue("时间原因/异地订单")]
        N13_时间原因异地订单 = 13,
        [EnumTextValue("疑似经销商")]
        N14_疑似经销商 = 14,
        [EnumTextValue("暂无卖车计划")]
        N15_暂无卖车计划 = 15,
        [EnumTextValue("需要买二手车")]
        N16_需要买二手车 = 16,
        [EnumTextValue("无声/单通")]
        N17_无声单通 = 17,
        [EnumTextValue("有兴趣参会时间不定")]
        N18_有兴趣参会时间不定 = 18,
        [EnumTextValue("咨询其他业务")]
        N19_咨询其他业务 = 19,
        [EnumTextValue("无法找到负责人")]
        N20_无法找到负责人 = 20,
        [EnumTextValue("公用电话")]
        N21_公用电话 = 21,
        [EnumTextValue("重复订单")]
        N22_重复订单 = 22,
        [EnumTextValue("其他")]
        N23_其他 = 23,
        //线索邀约(CJK)中增加
        [EnumTextValue("已购车")]
        N24_其他 = 24,

        [EnumTextValue("开场白拒绝")]
        N25_开场白拒绝 = 25,
        [EnumTextValue("产品介绍拒绝")]
        N26_产品介绍拒绝 = 26,
        [EnumTextValue("购车意向度低")]
        N27_购车意向度低 = 27,
        [EnumTextValue("经销商已联系并满意")]
        N28_经销商已联系并满意 = 28
        //[EnumTextValue("自行联系经销商")]
        //N27_自行联系经销商 = 27,
        //[EnumTextValue("暂无购车计划")]
        //N28_暂无购车计划 = 28,
        //[EnumTextValue("时间原因/异地订单")]
        //N29_时间原因异地订单 = 29,
    }
    #endregion

    #region 大数据
    /// 大数据查询方式
    /// <summary>
    /// 大数据查询方式
    /// </summary>
    public enum QueryTypeHB
    {
        std = 0,
        sql = 1
    }
    /// 条件枚举
    /// <summary>
    /// 条件枚举
    /// </summary>
    public class ConditionHB
    {
        /// 名称
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// 公式
        /// <summary>
        /// 公式
        /// </summary>
        public string Expression { get; set; }

        private ConditionHB(string name, string exp)
        {
            Name = name;
            Expression = exp;
        }
        public override string ToString()
        {
            return Expression.ToString();
        }

        #region 枚举
        //@为Value的占位符//
        /// <summary>
        /// 等于
        /// </summary>
        public static ConditionHB Equal = new ConditionHB("等于", "=");
        /// <summary>
        /// 在
        /// </summary>
        public static ConditionHB In = new ConditionHB("在", "in");
        /// <summary>
        /// 范围-闭闭
        /// </summary>
        public static ConditionHB RangeBB = new ConditionHB("范围-闭闭", "range[]");
        /// <summary>
        /// 范围-开开
        /// </summary>
        public static ConditionHB RangeKK = new ConditionHB("范围-开开", "range()");
        /// <summary>
        /// 范围-闭开
        /// </summary>
        public static ConditionHB RangeBK = new ConditionHB("范围-闭开", "range[)");
        /// <summary>
        /// 范围-开闭
        /// </summary>
        public static ConditionHB RangeKB = new ConditionHB("范围-开闭", "range(]");
        #endregion
    }
    /// 条件关系
    /// <summary>
    /// 条件关系
    /// </summary>
    public enum RelationshipHB
    {
        AND, OR
    }
    #endregion


    /// 报表-临时表类型
    /// <summary>
    /// 报表-临时表类型
    /// </summary>
    public enum ReportTempType
    {
        Hour = 1,
        Day = 2,
        All = 3
    }


    /// <summary>
    /// 五级质检
    /// </summary>
    [Serializable]
    public enum StandardFiveLevel
    {
        /// <summary>
        /// 很差
        /// </summary>
        [EnumTextValue("很差")]
        HenCha = 1,
        /// <summary>
        /// 较差
        /// </summary>
        [EnumTextValue("较差")]
        JiaoCha = 2,
        /// <summary>
        /// 合格
        /// </summary>
        [EnumTextValue("合格")]
        HeGe = 3,
        /// <summary>
        /// 良好
        /// </summary>
        [EnumTextValue("良好")]
        LiangHao = 4,
        /// <summary>
        /// 优秀
        /// </summary>
        [EnumTextValue("优秀")]
        YouXiu = 5
    }

    /// <summary>
    /// 数据模板自定义数据的预留字段名称
    /// </summary>
    [Serializable]
    public enum TFieldReservedTFName
    {
        [EnumTextValue("操作时间")]
        LastOptTime = 1,
        [EnumTextValue("操作人")]
        TrueName = 2,
        [EnumTextValue("推荐活动")]
        _Activity_Name = 3,
        [EnumTextValue("是否成功")]
        IsSucName = 4,
        [EnumTextValue("是否接通")]
        IsJTName = 5,
        [EnumTextValue("失败原因")]
        NotSuccessReasonName = 6,
        [EnumTextValue("未接通原因")]
        NotExportName = 7,
        [EnumTextValue("任务ID")]
        TaskID = 8,
        [EnumTextValue("客户ID")]
        CustID = 9,
        [EnumTextValue("任务状态")]
        TaskStatus = 10
    }

    /// 通用是否枚举
    /// <summary>
    /// 通用是否枚举
    /// </summary>
    public enum YesNO
    {
        是 = 1, 否 = 0
    }

    #region 工单V2-WOrderV2
    /// 通话来源
    /// <summary>
    /// 通话来源
    /// </summary>
    public enum CallSourceEnum
    {
        [EnumTextValue("无通话")]
        C00_无 = 0,
        [EnumTextValue("呼入")]
        C01_呼入 = 1,
        [EnumTextValue("呼出")]
        C02_呼出 = 2,
        [EnumTextValue("IM对话")]
        C03_IM对话 = 3,
        [EnumTextValue("IM留言")]
        C04_IM留言 = 4,
        [EnumTextValue("短信")]
        C05_短信 = 5,
    }
    /// 功能来源
    /// <summary>
    /// 功能来源
    /// </summary>
    public enum ModuleSourceEnum
    {
        [EnumTextValue("无")]
        M00_无 = 0,
        [EnumTextValue("个人用户")]
        M01_客户池 = 1,
        [EnumTextValue("工单记录")]
        M02_工单 = 2,
        [EnumTextValue("客户回访")]
        M03_客户回访 = 3,
        [EnumTextValue("未接来电")]
        M04_未接来电 = 4,
        [EnumTextValue("IM个人")]
        M05_IM个人 = 5,
        [EnumTextValue("IM经销商_新车")]
        M06_IM经销商_新车 = 6,
        [EnumTextValue("IM经销商_二手车")]
        M07_IM经销商_二手车 = 7,
    }
    /// 工单数据来源
    /// <summary>
    /// 工单数据来源
    /// </summary>
    [Serializable]
    public enum WorkOrderDataSource
    {
        //不转换，不呈现
        [EnumTextValue(null)]
        None = -1,
        [EnumTextValue("其他")]
        Other = 0,
        [EnumTextValue("电话")]
        TeleContact = 5,
        [EnumTextValue("IM")]
        IMOnLine = 8,
        [EnumTextValue("邮件")]
        Email = 3,

        //按照数字排序
        [EnumTextValue("130购车")]
        GouCheLine = 14,
        [EnumTextValue("167二手车")]
        UsedCarHotLine = 12,
        [EnumTextValue("168个人")]
        OSEHotLine = 2,
        [EnumTextValue("169金融")]
        FinancialLine = 11,
        [EnumTextValue("588乐车宝")]
        LeCheBaoLine = 15,
        [EnumTextValue("591惠买车")]
        HMCLine = 10,
        [EnumTextValue("598易鑫")]
        YIXinLine = 13,
        [EnumTextValue("599惠买车")]
        LeadsShouJi = 18,
        [EnumTextValue("605帮买")]
        BangMaiLine2 = 17,
        [EnumTextValue("658易车惠")]
        BitShopLine = 7,
        [EnumTextValue("719企业")]
        SONHotLine = 1,
        [EnumTextValue("920帮买")]
        BangMaiLine1 = 16,

        //测试热线，上线需删除
        //[EnumTextValue("7722测试")]
        //TEST_7722测试 = 100,
        //[EnumTextValue("2026测试")]
        //TEST_2026测试 = 101,
    }
    /// 工单状态
    /// <summary>
    /// 工单状态
    /// </summary>
    [Serializable]
    public enum WorkOrderStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        [EnumTextValue("待审核")]
        Pending = 1,
        /// <summary>
        /// 待处理
        /// </summary>
        [EnumTextValue("待处理")]
        Untreated = 2,
        /// <summary>
        /// 处理中
        /// </summary>
        [EnumTextValue("处理中")]
        Processing = 3,
        /// <summary>
        /// 已处理
        /// </summary>
        [EnumTextValue("已处理")]
        Processed = 4,
        /// <summary>
        /// 已完成
        /// </summary>
        [EnumTextValue("已完成")]
        Completed = 5,
        /// <summary>
        /// 已关闭
        /// </summary>
        [EnumTextValue("已关闭")]
        Closed = 6
    }
    /// 工单分类
    /// <summary>
    /// 工单分类
    /// </summary>
    public enum WOrderCategoryEnum
    {
        //不转换，不呈现
        [EnumTextValue(null)]
        None = -1,
        [EnumTextValue("问题处理")]
        W01_问题处理 = 1,
        [EnumTextValue("咨询")]
        W02_咨询 = 2,
        [EnumTextValue("投诉")]
        W03_投诉 = 3,
        [EnumTextValue("回访")]
        W04_回访 = 4,
        [EnumTextValue("合作")]
        W05_合作 = 5,
        [EnumTextValue("建议")]
        W06_建议 = 6,
        [EnumTextValue("其他")]
        W07_其他 = 7
    }
    /// 投诉级别
    /// <summary>
    /// 投诉级别
    /// </summary>
    public enum ComplaintLevelEnum
    {
        [EnumTextValue(null)]
        None = -1,
        [EnumTextValue("A级")]
        L01_A级 = 1,
        [EnumTextValue("B级")]
        L02_B级 = 2,
        [EnumTextValue("常规")]
        L03_常规 = 3
    }
    /// 操作类型
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum WOrderOperTypeEnum
    {
        [EnumTextValue(null)]
        None = -1,
        [EnumTextValue("添加")]
        L01_添加 = 1,
        [EnumTextValue("转出")]
        L02_转出 = 2,
        [EnumTextValue("审核")]
        L03_审核 = 3,
        [EnumTextValue("处理")]
        L04_处理 = 4,
        [EnumTextValue("回访")]
        L05_回访 = 5
    }
    /// 人员类型
    /// <summary>
    /// 人员类型
    /// </summary>
    public enum WOrderPersonTypeEnum
    {
        [EnumTextValue("接收人")]
        P01_接收人 = 1,
        [EnumTextValue("抄送人")]
        P02_抄送人 = 2
    }
    /// 客户类型
    /// <summary>
    /// 客户类型
    /// </summary>
    public enum CustTypeEnum
    {
        //1,2废弃
        [EnumTextValue(null)]
        None = -1,
        [EnumTextValue("个人")]
        T01_个人 = 4,
        [EnumTextValue("经销商")]
        T02_经销商 = 3
    }
    //工单关联数据表的数据类型 参考 BusinessTypeEnum

    /// 工单处理权限控制
    /// <summary>
    /// 工单处理权限控制
    /// </summary>
    public enum WOrderProcessRightTypeEnum
    {
        R01_人员ID = 1,
        R02_部门ID = 2,
        R03_员工编号 = 3,
        R04_功能权限 = 4
    }
    /// 电话用户类别
    /// <summary>
    /// 电话用户类别
    /// </summary>
    public enum CallUserType
    {
        用户 = 0, 坐席 = 1
    }
    #endregion
}

