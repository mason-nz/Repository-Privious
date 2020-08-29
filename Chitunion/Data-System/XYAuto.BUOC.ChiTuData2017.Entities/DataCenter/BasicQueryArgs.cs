using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter
{
    public class BasicQueryArgs
    {
        //// <summary>
        /// 时间类型 1：查询前7天  2：查询前30天
        /// </summary>
        public int DateType { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        private string begintime;
        public string BeginTime
        {
            get
            {

               
                return DateTime.Now.AddDays(-DateType).ToString("yyyy-MM-dd");
             

                //return begintime;
            }
            set
            {
                begintime = value;
            }
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        private string endtime;
        public string EndTime
        {
            get
            {
                //if (DateType > 0)
                //{
                //    return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                //}
                return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                /*return endtime*/;
            }
            set
            {
                endtime = value;
            }
        }
        /// <summary>
        /// 图表类型
        /// </summary>
        public string ChartType { get; set; }
        /// <summary>
        /// 物料类型
        /// </summary>
        //public int MaterielTypeID { get; set; }


        /// <summary>
        /// 渠道ID
        /// </summary>
        public int ChannelID { get; set; }

        /// <summary>
        /// 线索类型
        /// </summary>
        public int ClueTypeID { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int Operator { get; set; }

    }
}
