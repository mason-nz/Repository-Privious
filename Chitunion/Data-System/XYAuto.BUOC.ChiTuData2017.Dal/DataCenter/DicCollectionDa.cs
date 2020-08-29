using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.DataCenter
{
    public class DicCollectionDa
    {
        #region 单例
        private DicCollectionDa() { }

        static DicCollectionDa instance = null;
        static readonly object padlock = new object();

        public static DicCollectionDa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new DicCollectionDa();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

    }
}
