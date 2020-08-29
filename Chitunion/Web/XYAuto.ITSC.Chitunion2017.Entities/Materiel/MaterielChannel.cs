using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.Materiel
{
    //MaterielChannel
    public class MaterielChannel
    {
        public MaterielChannel()
        {
            this.PayType = (int)Enum.MaterielChannelPayTypeEnum.收费;
            this.PayMode = (int)Enum.MaterielChannelPayModelEnum.CPM;
            this.ChannelType = (int)Enum.MaterielChannelTypeEnum.众包;
        }

        //渠道ID
        public int ChannelID { get; set; }

        //物料ID
        public int MaterielID { get; set; }

        //媒体类型
        public string MediaTypeName { get; set; }

        //渠道类型
        public int ChannelType { get; set; }

        //媒体账号
        public string MediaNumber { get; set; }

        //媒体名称
        public string MediaName { get; set; }

        //费用类型
        public int PayType { get; set; }

        //付费模式
        public int PayMode { get; set; }

        //单位成本
        public decimal UnitCost { get; set; }

        //推广地址
        public string PromotionUrl { get; set; }

        //推广url地址code码
        public string PromotionUrlCode { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        //更新时间
        public DateTime LastUpdateTime { get; set; }

        #region 冗余

        public string ChannelTypeName { get; set; }
        public string PayTypeName { get; set; }
        public string PayModeName { get; set; }

        #endregion 冗余
    }
}