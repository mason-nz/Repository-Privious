using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class PositionModel
    {
        /// <summary>
        /// 位置名称
        /// </summary>
        private string _positionname;
        public string PositionName
        {
            get { return _positionname; }
            set { _positionname = value; }
        }

        /// <summary>
        /// 发布价格
        /// </summary>
        private decimal _issueprice;
        public decimal IssuePrice
        {
            get { return _issueprice; }
            set { _issueprice = value; }
        }
        /// <summary>
        /// 原创+发布价格
        /// </summary>
        private decimal _totalprice;
        public decimal TotalPrice
        {
            get { return _totalprice; }
            set { _totalprice = value; }
        }
        
        public string Score { get; set; }
    }
}
