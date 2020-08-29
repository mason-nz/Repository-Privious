
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.HD;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.View;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.DAO
{
    public class Chitunion2017DbContext : DbContext
    {
        //在config中配置数据库的名称
        //private static string dbName = System.Configuration.ConfigurationManager.AppSettings["Chitunion2017"];
        public Chitunion2017DbContext()
            : base("name=Chitunion2017")
        {
            Database.Initialize(true);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //todo:这里加了日志，用于测试。以后可以考虑留着
            //Database.Log = log => Log4NetHelper.Default("DBLog").Info($"{DateTime.Now}{Environment.NewLine}{log}");

            Database.Log = s => ThreadPool.QueueUserWorkItem(
                 log => Log4NetHelper.Default("DBLog").Info($"{DateTime.Now}{Environment.NewLine}{s}"));

            //表名不用复数形式
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //移除一对多的级联删除约定，想要级联删除可以在 EntityTypeConfiguration<TEntity>的实现类中进行控制
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //多对多启用级联删除约定，不想级联删除可以在删除前判断关联的数据进行拦截
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            this.Configuration.UseDatabaseNullSemantics = true; //关闭Ef自动生成的null判断语句

            base.OnModelCreating(modelBuilder);

            #region 多组合主键采用new 的方式定义
            modelBuilder.Entity<v_AppADList>().HasKey(t => new { t.PubID, t.MediaID, t.MinPrice, t.MaxPrice, t.PriceCount, t.PubStatusName });
            modelBuilder.Entity<v_RankList>().HasKey(t => new { t.RecID, t.WxNum, t.MaLiIndex });
            modelBuilder.Entity<v_UserInfo>().HasKey(t => new { t.UserID, t.UserName, t.Mobile, t.Pwd, t.Source, t.IsAuthAE, t.Status, t.RegisterType });

            modelBuilder.Entity<Publish_DetailInfo>().HasKey(t => new { t.RecID, t.MediaID, t.ADPosition1, t.Price });
            modelBuilder.Entity<Car_Serial>().HasKey(t => new { t.MasterBrandID, t.BrandID, t.SerialID, t.Name, t.CreateTime });
            modelBuilder.Entity<Car_Brand>().HasKey(t => new { t.MasterBrandID, t.BrandID, t.Name, t.CreateTime });
            modelBuilder.Entity<LE_chitu_blackip_stat>().HasKey(t => new { t.dt, t.ip, t.channel });
            modelBuilder.Entity<AreaInfo>().HasKey(t => new { t.AreaID, t.PID, t.AreaName, t.AbbrName, t.CreateTime });
            modelBuilder.Entity<UserRole>().HasKey(t => new { t.UserID, t.RoleID });
            modelBuilder.Entity<RoleModule>().HasKey(t => new { t.RoleID, t.ModuleID });
            modelBuilder.Entity<LE_WeiXinUser>().HasKey(t => new { t.WeiXinUserID, t.AdvertiserUserId });
            //在这里添加，设置主键映射自增
            modelBuilder.Entity<LE_WeiXinUser>().Property(b => b.WeiXinUserID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            #endregion
        }

        #region db
        public virtual DbSet<AccountArticle> AccountArticle { get; set; }
        public virtual DbSet<ADDetailInfo> ADDetailInfo { get; set; }
        public virtual DbSet<ADOrderCityExtend> ADOrderCityExtend { get; set; }
        public virtual DbSet<ADOrderInfo> ADOrderInfo { get; set; }
        public virtual DbSet<ADOrderInfoExtend> ADOrderInfoExtend { get; set; }
        public virtual DbSet<ADOrderOperateInfo> ADOrderOperateInfo { get; set; }
        public virtual DbSet<ADScheduleInfo> ADScheduleInfo { get; set; }
        public virtual DbSet<App_AdTemplate> App_AdTemplate { get; set; }
        public virtual DbSet<App_AdTemplateStyle> App_AdTemplateStyle { get; set; }
        public virtual DbSet<App_Device> App_Device { get; set; }
        public virtual DbSet<AppPushMsgSwitchLog> AppPushMsgSwitchLog { get; set; }
        public virtual DbSet<AppPriceInfo> AppPriceInfo { get; set; }
        public virtual DbSet<Article_Weixin> Article_Weixin { get; set; }
        public virtual DbSet<AuditInfo> AuditInfo { get; set; }
        public virtual DbSet<AuthorizeLogin_Token> AuthorizeLogin_Token { get; set; }
        public virtual DbSet<BusinessLine> BusinessLine { get; set; }
        public virtual DbSet<CartInfo> CartInfo { get; set; }
        public virtual DbSet<CartScheduleInfo> CartScheduleInfo { get; set; }
        public virtual DbSet<ChannelCost> ChannelCost { get; set; }
        public virtual DbSet<ChannelCostDetail> ChannelCostDetail { get; set; }
        public virtual DbSet<ChannelInfo> ChannelInfo { get; set; }
        public virtual DbSet<ChannelPolicy> ChannelPolicy { get; set; }
        public virtual DbSet<DepartMent> DepartMent { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<DomainInfo> DomainInfo { get; set; }
        public virtual DbSet<Fans_Weixin> Fans_Weixin { get; set; }
        public virtual DbSet<GDT_AccessToken> GDT_AccessToken { get; set; }
        public virtual DbSet<GDT_AccountBalance> GDT_AccountBalance { get; set; }
        public virtual DbSet<GDT_AccountInfo> GDT_AccountInfo { get; set; }
        public virtual DbSet<GDT_AccountRelation> GDT_AccountRelation { get; set; }
        public virtual DbSet<GDT_AdGroup> GDT_AdGroup { get; set; }
        public virtual DbSet<GDT_Ads> GDT_Ads { get; set; }
        public virtual DbSet<GDT_Campaign> GDT_Campaign { get; set; }
        public virtual DbSet<GDT_DailyRrport> GDT_DailyRrport { get; set; }
        public virtual DbSet<GDT_Demand> GDT_Demand { get; set; }
        public virtual DbSet<GDT_DemandRelation> GDT_DemandRelation { get; set; }
        public virtual DbSet<GDT_HourlyRrport> GDT_HourlyRrport { get; set; }
        public virtual DbSet<GDT_HourlyRrportForZHY> GDT_HourlyRrportForZHY { get; set; }
        public virtual DbSet<GDT_RealtimeCost> GDT_RealtimeCost { get; set; }
        public virtual DbSet<GDT_RechargeRelation> GDT_RechargeRelation { get; set; }
        public virtual DbSet<GDT_RechargeStatementRelation> GDT_RechargeStatementRelation { get; set; }
        public virtual DbSet<GDT_RoleUser> GDT_RoleUser { get; set; }
        public virtual DbSet<GDT_StatementDaily> GDT_StatementDaily { get; set; }
        public virtual DbSet<GDT_StatementsDetailed> GDT_StatementsDetailed { get; set; }
        public virtual DbSet<GDT_UserOrganize> GDT_UserOrganize { get; set; }
        public virtual DbSet<HolidaysInfo> HolidaysInfo { get; set; }
        public virtual DbSet<Home_Category> Home_Category { get; set; }
        public virtual DbSet<Home_Media> Home_Media { get; set; }
        public virtual DbSet<Interaction_Broadcast> Interaction_Broadcast { get; set; }
        public virtual DbSet<Interaction_Video> Interaction_Video { get; set; }
        public virtual DbSet<Interaction_Weibo> Interaction_Weibo { get; set; }
        public virtual DbSet<Interaction_Weixin> Interaction_Weixin { get; set; }
        public virtual DbSet<IPLimitList> IPLimitList { get; set; }
        public virtual DbSet<IPTitleInfo> IPTitleInfo { get; set; }
        public virtual DbSet<LB_ArticleInfo> LB_ArticleInfo { get; set; }
        public virtual DbSet<LB_IPSubLabel> LB_IPSubLabel { get; set; }
        public virtual DbSet<LB_Project> LB_Project { get; set; }
        public virtual DbSet<LB_ReceiveAuditLable> LB_ReceiveAuditLable { get; set; }
        public virtual DbSet<LB_SonIPLabel> LB_SonIPLabel { get; set; }
        public virtual DbSet<LB_Task> LB_Task { get; set; }
        public virtual DbSet<LB_TaskAssign> LB_TaskAssign { get; set; }
        public virtual DbSet<LB_TaskAudit_IPSubLabel> LB_TaskAudit_IPSubLabel { get; set; }
        public virtual DbSet<LB_TaskAudit_SonIPLabel> LB_TaskAudit_SonIPLabel { get; set; }
        public virtual DbSet<LB_TaskAuditPassed> LB_TaskAuditPassed { get; set; }
        public virtual DbSet<LB_TaskLabel> LB_TaskLabel { get; set; }
        public virtual DbSet<LB_TaskOperateInfo> LB_TaskOperateInfo { get; set; }
        public virtual DbSet<LE_AccountBalance> LE_AccountBalance { get; set; }
        public virtual DbSet<LE_ADOrderInfo> LE_ADOrderInfo { get; set; }
        public virtual DbSet<LE_APP> LE_APP { get; set; }
        public virtual DbSet<LE_APP_Repea> LE_APP_Repea { get; set; }
        public virtual DbSet<LE_APP_Temp> LE_APP_Temp { get; set; }
        public virtual DbSet<LE_Area_Promotion> LE_Area_Promotion { get; set; }
        public virtual DbSet<LE_Car_Promotion> LE_Car_Promotion { get; set; }
        public virtual DbSet<LE_CartInfo> LE_CartInfo { get; set; }
        public virtual DbSet<LE_ChannelStatMonthRelation> LE_ChannelStatMonthRelation { get; set; }
        public virtual DbSet<LE_ContentDistribute> LE_ContentDistribute { get; set; }
        public virtual DbSet<LE_DaySign> LE_DaySign { get; set; }
        public virtual DbSet<LE_DisbursementPay> LE_DisbursementPay { get; set; }
        public virtual DbSet<LE_Feedback> LE_Feedback { get; set; }
        public virtual DbSet<LE_Generalize_OperationLog> LE_Generalize_OperationLog { get; set; }
        public virtual DbSet<LE_IncomeDetail> LE_IncomeDetail { get; set; }
        public virtual DbSet<LE_IncomeStatisticsCategory> LE_IncomeStatisticsCategory { get; set; }
        public virtual DbSet<LE_InviteRecord> LE_InviteRecord { get; set; }
        public virtual DbSet<LE_IP_Blacklist> LE_IP_Blacklist { get; set; }
        public virtual DbSet<LE_IP_StatisticsDetail> LE_IP_StatisticsDetail { get; set; }
        public virtual DbSet<LE_MediaArea_Mapping> LE_MediaArea_Mapping { get; set; }
        public virtual DbSet<LE_MediaPromotion> LE_MediaPromotion { get; set; }
        public virtual DbSet<LE_PromotionChannel_Dict> LE_PromotionChannel_Dict { get; set; }
        public virtual DbSet<LE_PublishDetailInfo> LE_PublishDetailInfo { get; set; }
        public virtual DbSet<LE_PublishDetailInfo_Repea> LE_PublishDetailInfo_Repea { get; set; }
        public virtual DbSet<LE_ShareDetail> LE_ShareDetail { get; set; }
        public virtual DbSet<LE_SmartSearch> LE_SmartSearch { get; set; }
        public virtual DbSet<LE_SmartSearchMapping> LE_SmartSearchMapping { get; set; }
        public virtual DbSet<LE_TaskInfo> LE_TaskInfo { get; set; }
        public virtual DbSet<LE_User_Blacklist> LE_User_Blacklist { get; set; }
        public virtual DbSet<LE_UserBankAccount> LE_UserBankAccount { get; set; }
        public virtual DbSet<LE_UserDetailInfo> LE_UserDetailInfo { get; set; }
        public virtual DbSet<LE_UserInfo> LE_UserInfo { get; set; }
        public virtual DbSet<LE_Weibo> LE_Weibo { get; set; }
        public virtual DbSet<LE_Weibo_Repea> LE_Weibo_Repea { get; set; }
        public virtual DbSet<LE_Weixin> LE_Weixin { get; set; }
        public virtual DbSet<LE_Weixin_Repea> LE_Weixin_Repea { get; set; }
        public virtual DbSet<LE_WeiXinVisvit_Log> LE_WeiXinVisvit_Log { get; set; }
        public virtual DbSet<LE_WithdrawalsDetail> LE_WithdrawalsDetail { get; set; }
        public virtual DbSet<LE_WithdrawalsStatistics> LE_WithdrawalsStatistics { get; set; }
        public virtual DbSet<LE_WXUserScene> LE_WXUserScene { get; set; }
        public virtual DbSet<LogInfo> LogInfo { get; set; }
        public virtual DbSet<MaterielChannel> MaterielChannel { get; set; }
        public virtual DbSet<MaterielChannelData> MaterielChannelData { get; set; }
        public virtual DbSet<MaterielExtend> MaterielExtend { get; set; }
        public virtual DbSet<MaterielExtendFoot> MaterielExtendFoot { get; set; }
        public virtual DbSet<Media_Area_Mapping> Media_Area_Mapping { get; set; }
        public virtual DbSet<Media_Area_Mapping_Basic> Media_Area_Mapping_Basic { get; set; }
        public virtual DbSet<Media_BasePCAPP> Media_BasePCAPP { get; set; }
        public virtual DbSet<Media_Broadcast> Media_Broadcast { get; set; }
        public virtual DbSet<Media_CollectionBlacklist> Media_CollectionBlacklist { get; set; }
        public virtual DbSet<Media_CommonlyClass> Media_CommonlyClass { get; set; }
        public virtual DbSet<Media_FansArea> Media_FansArea { get; set; }
        public virtual DbSet<Media_PCAPP> Media_PCAPP { get; set; }
        public virtual DbSet<Media_Qualification> Media_Qualification { get; set; }
        public virtual DbSet<Media_Remark_Basic> Media_Remark_Basic { get; set; }
        public virtual DbSet<Media_SouHu> Media_SouHu { get; set; }
        public virtual DbSet<Media_TouTiao> Media_TouTiao { get; set; }
        public virtual DbSet<Media_Video> Media_Video { get; set; }
        public virtual DbSet<Media_Weibo> Media_Weibo { get; set; }
        public virtual DbSet<Media_Weixin> Media_Weixin { get; set; }
        public virtual DbSet<MediaCategory> MediaCategory { get; set; }
        public virtual DbSet<MediaOrderInfo> MediaOrderInfo { get; set; }
        public virtual DbSet<ModuleInfo> ModuleInfo { get; set; }
        public virtual DbSet<Msg_ActiveUser> Msg_ActiveUser { get; set; }
        public virtual DbSet<Msg_Classify> Msg_Classify { get; set; }
        public virtual DbSet<Msg_InteractiveEvent> Msg_InteractiveEvent { get; set; }
        public virtual DbSet<Msg_Master> Msg_Master { get; set; }
        public virtual DbSet<Msg_SendLog> Msg_SendLog { get; set; }
        public virtual DbSet<Msg_SendRecord> Msg_SendRecord { get; set; }
        public virtual DbSet<Msg_SendScope> Msg_SendScope { get; set; }
        public virtual DbSet<Notice> Notice { get; set; }
        public virtual DbSet<OAuth_Detail> OAuth_Detail { get; set; }
        public virtual DbSet<OAuth_History> OAuth_History { get; set; }
        public virtual DbSet<OrderFeedbackData_File> OrderFeedbackData_File { get; set; }
        public virtual DbSet<OrderFeedbackData_Live> OrderFeedbackData_Live { get; set; }
        public virtual DbSet<OrderFeedbackData_PC> OrderFeedbackData_PC { get; set; }
        public virtual DbSet<OrderFeedbackData_Video> OrderFeedbackData_Video { get; set; }
        public virtual DbSet<OrderFeedbackData_Weibo> OrderFeedbackData_Weibo { get; set; }
        public virtual DbSet<OrderFeedbackData_Weixin> OrderFeedbackData_Weixin { get; set; }
        public virtual DbSet<PubDeatil_ImgInfo> PubDeatil_ImgInfo { get; set; }
        public virtual DbSet<Publish_BasicInfo> Publish_BasicInfo { get; set; }
        public virtual DbSet<Publish_ExtendInfoPCAPP> Publish_ExtendInfoPCAPP { get; set; }
        public virtual DbSet<Publish_FileInfo> Publish_FileInfo { get; set; }
        public virtual DbSet<Publish_Remark> Publish_Remark { get; set; }
        public virtual DbSet<PublishAuditInfo> PublishAuditInfo { get; set; }
        public virtual DbSet<ReadStatistic_Weixin> ReadStatistic_Weixin { get; set; }
        public virtual DbSet<RoleInfo> RoleInfo { get; set; }
        public virtual DbSet<RoleModule> RoleModule { get; set; }
        public virtual DbSet<SaleAreaInfo> SaleAreaInfo { get; set; }
        public virtual DbSet<SaleAreaRelation> SaleAreaRelation { get; set; }
        public virtual DbSet<Stat_WeixinRank_List> Stat_WeixinRank_List { get; set; }
        public virtual DbSet<SubADInfo> SubADInfo { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<SysInfo> SysInfo { get; set; }
        public virtual DbSet<SysOperationLog> SysOperationLog { get; set; }
        public virtual DbSet<TaskScheduler_User> TaskScheduler_User { get; set; }
        public virtual DbSet<Title_Car_Mapping> Title_Car_Mapping { get; set; }
        public virtual DbSet<Title_Media_Mapping> Title_Media_Mapping { get; set; }
        public virtual DbSet<TitleBasicInfo> TitleBasicInfo { get; set; }
        public virtual DbSet<TwoBarCodeHistory> TwoBarCodeHistory { get; set; }
        public virtual DbSet<UpdateStatistic_Weixin> UpdateStatistic_Weixin { get; set; }
        public virtual DbSet<UploadFileInfo> UploadFileInfo { get; set; }
        public virtual DbSet<UserActionLog> UserActionLog { get; set; }
        public virtual DbSet<UserBroker> UserBroker { get; set; }
        public virtual DbSet<UserDetailInfo> UserDetailInfo { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<UserLockInfo> UserLockInfo { get; set; }
        public virtual DbSet<UserLoginInfo> UserLoginInfo { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<WeChat_Case> WeChat_Case { get; set; }
        public virtual DbSet<WeChatAuditInfo> WeChatAuditInfo { get; set; }
        public virtual DbSet<WeChatOperateMsg> WeChatOperateMsg { get; set; }
        public virtual DbSet<Weixin_Component> Weixin_Component { get; set; }
        public virtual DbSet<Weixin_OAuth> Weixin_OAuth { get; set; }
        public virtual DbSet<WxGroupRanking> WxGroupRanking { get; set; }
        public virtual DbSet<WxHistoryStatus> WxHistoryStatus { get; set; }
        public virtual DbSet<WxMaterial_Article> WxMaterial_Article { get; set; }
        public virtual DbSet<WxMaterial_ArticleGroup> WxMaterial_ArticleGroup { get; set; }
        public virtual DbSet<WxMaterial_ArticleTemplate> WxMaterial_ArticleTemplate { get; set; }
        public virtual DbSet<WxPictureMaterial> WxPictureMaterial { get; set; }
        public virtual DbSet<WxStaticHitory> WxStaticHitory { get; set; }
        public virtual DbSet<AreaInfo> AreaInfo { get; set; }
        public virtual DbSet<Car_Brand> Car_Brand { get; set; }
        public virtual DbSet<Car_Serial> Car_Serial { get; set; }
        public virtual DbSet<DictInfo> DictInfo { get; set; }
        public virtual DbSet<LE_chitu_blackip_stat> LE_chitu_blackip_stat { get; set; }
        public virtual DbSet<LE_WeiXinUser> LE_WeiXinUser { get; set; }
        public virtual DbSet<newLB_TaskAssign> newLB_TaskAssign { get; set; }
        public virtual DbSet<Publish_DetailInfo> Publish_DetailInfo { get; set; }
        public virtual DbSet<tmp_order> tmp_order { get; set; }
        public virtual DbSet<tmptb> tmptb { get; set; }
        public virtual DbSet<DictScene> DictScene { get; set; }


        public virtual DbSet<LE_IP_RequestLog> LE_IP_RequestLog { get; set; }
        public virtual DbSet<HD_LuckDrawActivity> HD_LuckDrawActivity { get; set; }
        public virtual DbSet<HD_LuckDrawPrize> HD_LuckDrawPrize { get; set; }
        public virtual DbSet<HD_LuckDrawRecord> HD_LuckDrawRecord { get; set; }

        public virtual DbSet<HD_LuckDrawRecord_Moni> HD_LuckDrawRecord_Moni { get; set; }
        #endregion

        #region view

        public virtual DbSet<v_AppADList> v_AppADList { get; set; }
        public virtual DbSet<v_LE_WeiXinUser> v_LE_WeiXinUser { get; set; }
        public virtual DbSet<v_RankList> v_RankList { get; set; }
        public virtual DbSet<v_UserInfo> v_UserInfo { get; set; }

        #endregion
    }
}
