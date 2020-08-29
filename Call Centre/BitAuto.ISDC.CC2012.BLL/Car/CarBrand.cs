using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CarBrand
    {
        #region Instance
        public static readonly CarBrand Instance = new CarBrand();
        #endregion

        #region Contructor
        protected CarBrand()
        { }
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.CarBrand model)
        {
            Dal.CarBrand.Instance.Insert(model);
        }

      

        #endregion        

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllCarBrandFormCrm2009()
        {
            return Dal.CarBrand.Instance.GetAllCarBrandFormCrm2009();
        }

        internal void DeleteTable()
        {
            Dal.CarBrand.Instance.DeleteTable();
        }

        internal DataTable GetAllList()
        {
            return Dal.CarBrand.Instance.GetAllList();
        }
    }
}
