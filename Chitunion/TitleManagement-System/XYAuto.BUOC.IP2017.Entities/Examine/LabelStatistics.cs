using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.Entities.Examine
{
    /// <summary>
    /// zlb 2017-10-23
    /// 标签统计类
    /// </summary>
    public class LabelStatistics
    {
        public Dictionary<string, object> listCategory = new Dictionary<string, object>();
        public Dictionary<string, object> listMarketScene = new Dictionary<string, object>();
        public Dictionary<string, object> listDistributeScene = new Dictionary<string, object>();
        public Dictionary<string, object> listIPLabel = new Dictionary<string, object>();
    }
    /// <summary>
    /// 不同标签所有集合类
    /// </summary>
    public class LabelList
    {
        public List<Dictionary<string, object>> listCategory = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> listMarketScene = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> listDistributeScene = new List<Dictionary<string, object>>();
        public List<Dictionary<string, object>> listIPLabel = new List<Dictionary<string, object>>();
    }
}
