using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Dal.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;

namespace XYAuto.ITSC.Chitunion2017.BLL.ChituMedia
{
    public class TagVehicleBll
    {
        #region 初始化
        private TagVehicleBll() { }

        public static TagVehicleBll instance = null;
        public static readonly object padlock = new object();

        public static TagVehicleBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new TagVehicleBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        /// <summary>
        /// 获取标签车型对应数据
        /// </summary>
        /// <returns></returns>
        public List<TagVehicleModel> GetTagVehicleList()
        {
            return TagVehicleDa.Instance.GetTagVehicleList();
        }
    }
}
