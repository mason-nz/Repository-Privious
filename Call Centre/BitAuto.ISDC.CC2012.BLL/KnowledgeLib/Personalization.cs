using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class Personalization
    {
        #region Instance
		public static readonly Personalization Instance = new Personalization();
		#endregion

		#region Contructor
        protected Personalization()
		{}
		#endregion

        #region   个人收藏功能
        public DataTable GetCollectedKnowledgeData(QueryKLFavorites query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Personalization.Instance.GetCollectedKnowledgeData(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetCollectedFAQData(QueryKLFavorites query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Personalization.Instance.GetCollectedFAQData(query, order, currentPage, pageSize, out  totalCount);
        }
        public int Insert(Entities.QueryKLFavorites model)
        {
            return Dal.Personalization.Instance.Insert(model);
        }
        public bool IsCollected(Entities.QueryKLFavorites model)
        {
            return Dal.Personalization.Instance.IsCollected(model);
        }
        public int Delete(int Id)
        {
            return Dal.Personalization.Instance.Delete(Id);
        }

        #endregion

        #region  问题解答功能
        public DataTable GetKLRaiseQuestionModelDataById(int id)
        {
            return Dal.Personalization.Instance.GetKLRaiseQuestionModelDataById(id);
        }
        /// <summary>
        /// 增加一条问题数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回值大于零表示执行成功</returns>
        public int InsertKLRaiseQuestion(Entities.KLRaiseQuestions model)
        {
            return Dal.Personalization.Instance.InsertKLRaiseQuestion(model);
        }
        /// <summary>
        /// 修改一条问题数据，同时添加日志数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回值 1：执行成功；0：执行失败</returns>
        public int UpdateKLRaiseQuestion(Entities.KLRaiseQuestions model)
        {
            return Dal.Personalization.Instance.UpdateKLRaiseQuestion(model);
        }
        /// <summary>
        /// 删除一条问题数据，同时删除日志数据
        /// </summary>
        /// <param name="Id">问题ID</param>
        /// <returns>返回值 1：执行成功；0：执行失败</returns>
        public int DeleteKLRaiseQuestionById(int Id)
        {
            return Dal.Personalization.Instance.DeleteKLRaiseQuestionById(Id);
        }
        /// <summary>
        /// 通过条件查询收藏的KLRaiseQuestion数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetKLRaiseQuestionData(KLRaiseQuestions query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Personalization.Instance.GetKLRaiseQuestionData(query, order, currentPage, pageSize,out totalCount);
        }
        /// <summary>
        /// 通过条件查询提问的KLRaiseQuestion数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetKLRaiseQuestionDataForManage(QueryKLRaiseQuestions query, string QuestionedUserName, string order, int currentPage, int pageSize, out int totalCount)
        {
            int userid = BLL.Util.GetLoginUserID();
            return Dal.Personalization.Instance.GetKLRaiseQuestionDataForManage(query, QuestionedUserName, order, currentPage, pageSize, out totalCount, userid);
        }
        /// <summary>
        /// 获取解答人信息列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAnswerUserListData()
        {
            return Dal.Personalization.Instance.GetAnswerUserListData();
        }
        /// <summary>
        /// 根据提问id获取提问明细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetQuestionDetailsById(string id)
        {
            return Dal.Personalization.Instance.GetQuestionDetailsById(id);
        }
        /// <summary>
        /// 更具提问id 获取提问操作明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetQuestionOperationLogById(string id)
        {
            return Dal.Personalization.Instance.GetQuestionOperationLogById(id);
        }
        /// <summary>
        /// 通过KLRQId查询KLRaiseQuestionLog数据
        /// </summary>
        /// <param name="KLRQId"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetKLRaiseQuestionLogDataByKLRQId(int KLRQId, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.Personalization.Instance.GetKLRaiseQuestionLogDataByKLRQId(KLRQId,order,currentPage,pageSize,out totalCount);
        }
        #endregion

    }
}
