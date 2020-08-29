using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class ProjectTask_MemberContactMapping
    {
        #region Instance
        public static readonly ProjectTask_MemberContactMapping Instance = new ProjectTask_MemberContactMapping();
        #endregion

        #region Contructor
        protected ProjectTask_MemberContactMapping()
        {

        }
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int AddMemberContactMapping(Entities.ProjectTask_MemberContactMapping model)
        {
            return Dal.ProjectTask_MemberContactMapping.Instance.AddMemberContactMapping(model);
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateMemberContactMapping(Entities.ProjectTask_MemberContactMapping model)
        {
            return Dal.ProjectTask_MemberContactMapping.Instance.UpdateMemberContactMapping(model);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteMemberContactMapping(int recid)
        {
            return Dal.ProjectTask_MemberContactMapping.Instance.DeleteMemberContactMapping(recid);
        }

        /// <summary>
        /// 根据联系人编号删除关联
        /// </summary>
        /// <param name="contactid">联系人编号</param>
        /// <returns>bool</returns>
        public bool DeleteByContactID(int contactid)
        {
            return Dal.ProjectTask_MemberContactMapping.Instance.DeleteByContactID(contactid);
        }
        #endregion

        #region Select
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_MemberContactMapping GetModel(int recid)
        {
            return Dal.ProjectTask_MemberContactMapping.Instance.GetModel(recid);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_MemberContactMapping GetModel(string memberid, int contactid)
        {
            return Dal.ProjectTask_MemberContactMapping.Instance.GetModel(memberid, contactid);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            return Dal.ProjectTask_MemberContactMapping.Instance.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataTable GetList(int top, string strWhere, string filedOrder)
        {
            return Dal.ProjectTask_MemberContactMapping.Instance.GetList(top, strWhere, filedOrder);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return Dal.ProjectTask_MemberContactMapping.Instance.GetRecordCount(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Entities.ProjectTask_MemberContactMapping> GetModelList(string strWhere)
        {
            DataTable dt = Dal.ProjectTask_MemberContactMapping.Instance.GetList(strWhere);
            return DataTableToList(dt);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        private List<Entities.ProjectTask_MemberContactMapping> DataTableToList(DataTable dt)
        {
            List<Entities.ProjectTask_MemberContactMapping> modelList = new List<Entities.ProjectTask_MemberContactMapping>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Entities.ProjectTask_MemberContactMapping model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Entities.ProjectTask_MemberContactMapping();
                    if (dt.Rows[n]["RecID"] != null && dt.Rows[n]["RecID"].ToString() != "")
                    {
                        model.RecID = int.Parse(dt.Rows[n]["RecID"].ToString());
                    }
                    if (dt.Rows[n]["MemberID"] != null && dt.Rows[n]["MemberID"].ToString() != "")
                    {
                        model.MemberID = new Guid(dt.Rows[n]["MemberID"].ToString());
                    }
                    if (dt.Rows[n]["ContactID"] != null && dt.Rows[n]["ContactID"].ToString() != "")
                    {
                        model.ContactID = int.Parse(dt.Rows[n]["ContactID"].ToString());
                    }
                    if (dt.Rows[n]["IsMain"] != null && dt.Rows[n]["IsMain"].ToString() != "")
                    {
                        model.IsMain = int.Parse(dt.Rows[n]["IsMain"].ToString());
                    }
                    if (dt.Rows[n]["CreateTime"] != null && dt.Rows[n]["CreateTime"].ToString() != "")
                    {
                        model.CreateTime = DateTime.Parse(dt.Rows[n]["CreateTime"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 根据会员编号获取会员的负责人(联系人)
        /// </summary>
        /// <param name="membercode">会员编号</param>
        /// <returns>DataTable</returns>
        public DataTable GetMemberManager(string membercode)
        {
            return Dal.ProjectTask_MemberContactMapping.Instance.GetMemberManager(membercode);
        }

        #endregion

        #region Other

        #endregion
    }
}
