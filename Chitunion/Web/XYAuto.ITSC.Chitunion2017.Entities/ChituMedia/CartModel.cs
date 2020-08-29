using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class CartModel
    {
        /// <summary>
        /// 媒体ID
        /// </summary>
        public int MediaID { get; set; }
        /// <summary>
        /// 媒体名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string LogUrl { get; set; }
        /// <summary>
        /// 微信号、日活、粉丝数
        /// </summary>
        public string MediaText { get; set; }
        /// <summary>
        /// 媒体类型
        /// </summary>
        public int MediaType { get; set; }
    }
    public class ReqCartInfoDto
    {
        public List<CartModel> CartInfoList { get; set; }
    }
    public class DeleteCartInfoDto
    {
        public List<CartModel> CartInfoList { get; set; }
    }

    public class QueryTaskargs
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class TaskListModel
    {
        //任务名称
        public string TaskName
        {
            get; set;
        }
        //物料链接URL地址
        public string MaterialUrl { get; set; }
        //图片地址
        public string ImgUrl { get; set; }

        //简介
        public string Synopsis { get; set; }
    }
}
