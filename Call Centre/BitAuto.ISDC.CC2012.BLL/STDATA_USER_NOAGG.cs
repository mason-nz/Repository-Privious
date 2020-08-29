using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class STDATA_USER_NOAGG
    {
        #region Instance
        public static readonly STDATA_USER_NOAGG Instance = new STDATA_USER_NOAGG();
        #endregion

        #region Contructor
        protected STDATA_USER_NOAGG()
        { }
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.STDATA_USER_NOAGG model)
        {
            return Dal.STDATA_USER_NOAGG.Instance.Insert(model);
        }
        #endregion
    }
}
