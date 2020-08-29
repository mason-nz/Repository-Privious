using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //媒体-微博公众号信息
    public class LeWeibo
    {

        //主键
        public int RecID { get; set; }

        //微信号
        public string Number { get; set; }

        //微信名称
        public string Name { get; set; }

        public string Sex { get; set; }

        //头像的URL地址
        public string HeadIconURL { get; set; }

        //粉丝数
        public int FansCount { get; set; }

        //粉丝数截图
        public string FansCountURL { get; set; }

        //粉丝男比例
        public string FansSex { get; set; }

        //行业分类枚举ID
        public int CategoryID { get; set; }

        //媒体领域枚举
        public int AreaID { get; set; }

        public int Profession { get; set; }

        public int ProvinceID { get; set; }

        public int CityID { get; set; }

        //媒体级别（意见领袖或普通）
        public int LevelType { get; set; }

        //是否为微信认证
        public int AuthType { get; set; }

        //描述、签名
        public string Sign { get; set; }

        //下单备注（枚举）
        public int OrderRemark { get; set; }

        //是否预约
        public bool IsReserve { get; set; }

        //媒体状态
        public int Status { get; set; }

        //枚举值
        public int Source { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserID { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int LastUpdateUserID { get; set; }

        //平均转发数
        public int ForwardAvg { get; set; }

        //平均评论数
        public int CommentAvg { get; set; }

        //平均点赞数
        public int LikeAvg { get; set; }

        //直发参考价
        public decimal DirectPrice { get; set; }

        //转发参考价
        public decimal ForwardPrice { get; set; }

        public Byte[] TimestampSign { get; set; }


    }
}