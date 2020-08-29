using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter
{
    public class ListQueryArgs : Pagination
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public int MaterialID { get; set; }
        /// <summary>
        /// 物料类型
        /// </summary>
        public int MaterielTypeID { get; set; }

        /// <summary>
        /// 渠道ID
        /// </summary>
        public int ChannelID { get; set; }
        /// <summary>
        /// 场景ID
        /// </summary>
        public int SceneID { get; set; }
        /// <summary>
        /// 账号分值
        /// </summary>
        public int AccountID { get; set; }
        /// <summary>
        /// 状态ID
        /// </summary>
        public int ConditionID { get; set; }
        /// <summary>
        /// 账号分值名称
        /// </summary>
        public string AccountName { get; set; }

        public string ListType { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

    }

    public class WorkloadQuery:Pagination
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 操作类型 1:初筛 2:清洗 3:封装
        /// </summary>
        public int Operator { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserName { get; set; }
    }
}
