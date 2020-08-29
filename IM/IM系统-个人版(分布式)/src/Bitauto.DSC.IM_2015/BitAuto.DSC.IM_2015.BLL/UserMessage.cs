using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���UserMessage ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:04 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class UserMessage
    {
        #region Instance
        public static readonly UserMessage Instance = new UserMessage();
        #endregion

        #region Contructor
        protected UserMessage()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetUserMessage(QueryUserMessage query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.UserMessage.Instance.GetUserMessage(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.UserMessage.Instance.GetUserMessage(new QueryUserMessage(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        //public Entities.UserMessage GetUserMessage(int RecID)
        //{

        //    return Dal.UserMessage.Instance.GetUserMessage(RecID);
        //}

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryUserMessage query = new QueryUserMessage();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetUserMessage(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.UserMessage model)
        {
            return Dal.UserMessage.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.UserMessage model)
        {
            return Dal.UserMessage.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.UserMessage model)
        {
            return Dal.UserMessage.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.UserMessage model)
        {
            return Dal.UserMessage.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.UserMessage.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.UserMessage.Instance.Delete(sqltran, RecID);
        }

        #endregion

        public int InsertUserMessage(Entities.UserMessage model)
        {
            return Dal.UserMessage.Instance.InsertUserMessage(model);
        }

        public int UpdateUserMessageInfoByRecID(QueryUserMessage query)
        {
            return Dal.UserMessage.Instance.UpdateUserMessageInfoByRecID(query);
        }

        public string GetUserNameByUserID(int UserID)
        {
            return Dal.UserMessage.Instance.GetUserNameByUserID(UserID);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BitAuto.DSC.IM_2015.Entities.UserMessage GetModel(int RecID)
        {
            return Dal.UserMessage.Instance.GetModel(RecID);
        }


        #region ��ȡ��ѯ����

        /// <summary>
        ///  ��ȡ��ѯ����
        /// </summary>
        /// <param name="tyepID">��ѯ����ID</param>
        /// <returns></returns>
        public string GetTypeName(string tyepID)
        {
            string typeName = string.Empty;
            switch (tyepID)
            {
                case "1":
                    typeName = "������ѯ";
                    break;
                case "2":
                    typeName = "������ѯ";
                    break;
                case "3":
                    typeName = "���ѯ";
                    break;
                case "4":
                    typeName = "��վ����";
                    break;
                case "5":
                    typeName = "����";
                    break;
                default:
                    break;
            }
            return typeName;
        }


        #endregion
    }
}

