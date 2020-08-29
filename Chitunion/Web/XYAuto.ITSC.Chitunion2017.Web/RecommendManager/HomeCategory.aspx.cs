using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
namespace XYAuto.ITSC.Chitunion2017.Web.RecommendManager
{

    public partial class HomeCategory : System.Web.UI.Page
    {
        public DataTable AllCategoryList = new DataTable();
        public DataTable HomeCategoryList = null;

        public string MediaTypeID;
        protected void Page_Load(object sender, EventArgs e)
        {
            MediaTypeID = Request.QueryString["MediaTypeID"];
            DataTable AllCategoryList2 = Chitunion2017.Common.DictInfo.Instance.GetDictInfoByTypeID(Convert.ToInt32(Request.QueryString["CategoryID"]));
            HomeCategoryList = BLL.HomeCategory.Instance.SelectSelectedCategory(Convert.ToInt32(MediaTypeID), 0);
            if (AllCategoryList2 != null && HomeCategoryList != null)
            {
                var normalReceive = from r in AllCategoryList2.AsEnumerable()
                                    where
                                        !(from rr in HomeCategoryList.AsEnumerable() select rr.Field<Int32>("CategoryID")).Contains(
                                        r.Field<Int32>("DictId"))
                                    select r;
                AllCategoryList = AllCategoryList2.Clone();
                foreach (var item in normalReceive)
                {
                    AllCategoryList.Rows.Add(item.ItemArray);
                }
            }
            else
            {
                AllCategoryList = AllCategoryList2.Copy();
            }
            //AllCategoryList = AllCategoryList2.Clone();
            //if (AllCategoryList2 != null && HomeCategoryList != null)
            //{

            //    AllCategoryList.Rows.Clear();
            //    foreach (DataRow item in AllCategoryList2.Rows)
            //    {
            //        foreach (DataRow item1 in HomeCategoryList.Rows)
            //        {
            //            if (item["DictId"].ToString() != item1["CategoryID"].ToString())
            //            {
            //                AllCategoryList.Rows.Add(item1);
            //            }
            //        }
            //    }
            //}

        }

        protected void InsertCategory()
        {

        }
    }
}