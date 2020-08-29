using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class WeChatOperateMsg
    {
        #region Instance

        public static readonly WeChatOperateMsg Instance = new WeChatOperateMsg();

        #endregion Instance

        #region Contructor

        protected WeChatOperateMsg()
        { }

        #endregion Contructor

        #region Insert

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Insert(Entities.WeChatOperateMsg model)
        {
            Dal.WeChatOperateMsg.Instance.Insert(model);
        }

        #endregion Insert

        #region Select

        public DataTable GetWeChatOperateMsg(int userid, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.WeChatOperateMsg.Instance.GetWeChatOperateMsg(userid, currentPage, pageSize, out totalCount);
        }

        #endregion Select

        public Entities.WeChatOperateMsg DataRowToModel(DataRow row)
        {
            return Dal.WeChatOperateMsg.Instance.DataRowToModel(row);
        }

        #region 更新或查询消息数量V1.1

        public string p_WeChatOperateMsg_UpdateReadV1_1(int OptType, int userid, out int totalCount)
        {
            return Dal.WeChatOperateMsg.Instance.p_WeChatOperateMsg_UpdateReadV1_1(OptType, userid, out totalCount);
        }

        #endregion 更新或查询消息数量V1.1

        #region 查询消息数量V1.1

        public int GetCount_MsgNoRead()
        {
            int totalCount = 0;
            Dal.WeChatOperateMsg.Instance.p_WeChatOperateMsg_UpdateReadV1_1(1, Chitunion2017.Common.UserInfo.GetLoginUserID(), out totalCount);
            return totalCount;
        }

        #endregion 查询消息数量V1.1

        #region 微信新增媒体时记录个人信息

        /// <summary>
        /// 微信新增媒体时记录个人信息
        /// </summary>
        /// <param name="MediaID">媒体ID</param>
        /// <param name="MediaName">媒体名称</param>
        /// <param name="SubmitUserName">优先取媒体创建人的姓名，没有则取UserName</param>
        /// <param name="SubmitUserID">媒体创建人UserID</param>
        /// <param name="OptType">待审43001，通过43002，驳回43003，见枚举Entities.EnumWeChatAuditStatus</param>
        /// <param name="mediaType">默认是微信</param>
        /// <returns>出错返回异常信息，成功返回空</returns>
        public string WehchatAudit_InsertV1_1(int MediaID, string MediaName, string SubmitUserName, int SubmitUserID,
            Entities.EnumWeChatAuditStatus OptType, MediaType mediaType = MediaType.WeiXin)
        {
            try
            {
                Entities.WeChatOperateMsg model = new Entities.WeChatOperateMsg()
                {
                    MediaType = (int)mediaType,
                    MediaID = MediaID,
                    MediaName = MediaName,
                    SubmitUserName = SubmitUserName,
                    SubmitUserID = SubmitUserID,
                    OptType = (int)OptType,
                    MsgType = (int)Entities.EnumWeChatOperateMsg.媒体审核,
                    CreateUserID = Chitunion2017.Common.UserInfo.GetLoginUserID()
                };

                Dal.WeChatOperateMsg.Instance.Insert(model);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        #endregion 微信新增媒体时记录个人信息

        #region 查询Table转List
        public List<Entities.WeChatOperateMsg> GetWeChatOperateMsgList(int userid, int currentPage, int pageSize, out int totalCount)
        {
            return Util.DataTableToList<Entities.WeChatOperateMsg>(Dal.WeChatOperateMsg.Instance.GetWeChatOperateMsg(userid, currentPage, pageSize, out totalCount));
        }
        #endregion

        #region 查询媒体是否已有通用模板
        public bool HasPulblicTemplate(int mediaID)
        {
            return Dal.WeChatOperateMsg.Instance.HasPulblicTemplate(mediaID);
        }
        #endregion
    }
}