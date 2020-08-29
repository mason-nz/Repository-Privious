using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO
{
    /// <summary>
    /// 查询媒体列表参数实体
    /// ls
    /// </summary>
    public class QueryMediaListParamsDTO
    {
        public QueryMediaListParamsDTO()
        {
            CategoryID = Entities.Constants.Constant.INT_INVALID_VALUE;
            LevelType = Entities.Constants.Constant.INT_INVALID_VALUE;
            IsAuth = Entities.Constants.Constant.INT_INVALID_VALUE;
            Source = Entities.Constants.Constant.INT_INVALID_VALUE;
            Platform = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        /// <summary>
        /// 媒体类型
        /// </summary>
        public MediaTypeEnum MediaType { get; set; }

        /// <summary>
        /// 平台ID
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 来源ID
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 创建人名
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }

        private int pageindex = 1;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return pageindex; }
            set { pageindex = value; }
        }

        private int pagesize = 20;

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value; }
        }

        #region weixin

        public int CategoryID { get; set; }
        public int LevelType { get; set; }
        public int IsAuth { get; set; }

        #endregion weixin

        public int OrderBy { get; set; }
    }
}