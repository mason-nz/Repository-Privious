using System.Collections.Generic;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Wechat
{
    /// <summary>
    /// 注释：用户邀请功能接口
    /// 作者：zhanglb
    /// 日期：2018/5/9 20:20:53
    /// </summary>
    public interface ILeInviteRecord
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        LE_InviteRecord AddInviteRecord(LE_InviteRecord dto);

        /// <summary>
        /// 获取被邀请用户记录信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        LE_InviteRecord GetBeInviter(int userId);

        /// <summary>
        /// 获取邀请用户recId对应日期RedEvesStatus状态下红包的个数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="redEvesStatus">红包状态（104001 领取红包;104002 已领取;104003 当日红包已领完,104004 尚未完成分享）</param>
        /// <param name="recId">主键Id</param>
        /// <returns></returns>
        int GetRedEvesCount(int userId, int redEvesStatus, int recId);

        /// <summary>
        /// 获取用户当日RedEvesStatus状态下红包的个数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="redEvesStatus"></param>
        /// <returns></returns>
        int GetTodayRedEvesCount(int userId, int redEvesStatus);

        /// <summary>
        /// 更新用户红包金额（领取红包）
        /// </summary>
        /// <param name="redEvesPrice">红包金额</param>
        /// <param name="userId">用户ID</param>
        /// <param name="recId">主键ID</param>
        /// <param name="beforeStatus">当前状态</param>
        /// <param name="afterStatus">更新状态</param>
        /// <returns></returns>
        int UpdateRedEvesPrice(int recId, int userId, decimal redEvesPrice, int beforeStatus, int afterStatus);

        /// <summary>
        /// 分页获取邀请好友列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="recId">主键ID</param>
        /// <param name="topCount">行数</param>
        /// <returns></returns>
        IEnumerable<LE_InviteRecord> GetInvitedList(int userId, int recId, int topCount);

        /// <summary>
        /// 获取邀请好友数量
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        int GetInvitedCount(int userId);

        /// <summary>
        /// 更新用户主键ID对应日期下红包状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="recId">主键ID</param>
        /// <param name="beforeStatus">当前状态</param>
        /// <param name="afterStatus">更新状态</param>
        /// <returns></returns>
        int UpdateRedEves(int userId, int recId);

        /// <summary>
        /// 更新用户红包状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="beforeStatus">当前状态（104001 领取红包;104002 已领取;104003 当日红包已领完,104004 尚未完成分享）</param>
        /// <param name="afterStatus">更新状态（104001 领取红包;104002 已领取;104003 当日红包已领完,104004 尚未完成分享）</param>
        /// <returns></returns>
        int UpdateRedEves(int userId);

        /// <summary>
        /// 获取用户最大主键ID
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        int GetInvitedMaxRecId(int userId);

        /// <summary>
        /// 获取用户对应红包状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="recId">主键ID</param>
        /// <returns></returns>
        int GetRedEvesStatus(int userId, int recId);

    }
}
