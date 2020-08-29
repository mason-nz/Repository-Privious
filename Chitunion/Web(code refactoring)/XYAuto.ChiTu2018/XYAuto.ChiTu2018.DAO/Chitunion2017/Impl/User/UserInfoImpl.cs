/**
*
*创建人：lixiong
*创建时间：2018/5/9 10:56:22
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/

using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;
using XYAuto.ChiTu2018.DAO.Chitunion2017.User;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Extend.User;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Impl.User
{
    public sealed class UserInfoImpl : RepositoryImpl<UserInfo>, IUserInfo
    {
        public UserInfo Update(UserInfo model)
        {
            return Put(model);
        }

        /// <summary>
        /// 合并用户基本数据（老的用户覆盖新的)
        /// </summary>
        /// <param name="oldUserId">老用户Id</param>
        /// <param name="newUserId">新用户Id</param>
        /// <returns>item1:id item2:提交过程 id>0 && true 才是真正的成功</returns>
        public Tuple<int, bool> CleanUserInfoForM(int oldUserId, int newUserId)
        {
            using (var db = new Chitunion2017DbContext())
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))//开启事务
                {
                    var sbSql = new StringBuilder();

                    sbSql.Append($"UPDATE dbo.UserInfo SET CleanStatus=1 WHERE UserID={oldUserId};");//修改老用户清洗状态为清洗中

                    sbSql.Append($"UPDATE dbo.UserInfo SET  Status=-1 WHERE UserID={newUserId} ");//修改新用户为删除状态

                    sbSql.Append($"UPDATE dbo.LE_WeiXinUser SET UserID={oldUserId} WHERE UserID={newUserId} ");//把新用户ID改为为老用户ID（用户微信表）

                    sbSql.Append($"UPDATE dbo.LE_WXUserScene SET UserID={oldUserId} WHERE UserID={newUserId} ");//把新用户ID改为为老用户ID（用户分类表）

                    sbSql.Append($"UPDATE dbo.LE_WXUserScene SET Status=-1 WHERE RecID IN (SELECT MAX(RecID)  FROM dbo.LE_WXUserScene  WHERE UserID={oldUserId} GROUP BY UserID,SceneID HAVING COUNT(1)>1)");//分类去重

                    sbSql.Append($"UPDATE dbo.LE_UserDetailInfo SET Status=-1 WHERE UserID={newUserId}");// 逻辑删除用户认证表

                    sbSql.Append($"UPDATE dbo.LE_UserBankAccount SET Status=-1 WHERE UserID={newUserId} ");// 逻辑删除支付账号表

                    sbSql.Append($"UPDATE dbo.HD_LuckDrawRecord SET UserId={newUserId}  WHERE UserID=@NewUserId ");// 把新用户ID改为老用户ID（抽奖记录表）

                    //todo:暂时不用管每条sql是否执行成功，默认全部执行成功，除非异常
                    db.Database.ExecuteSqlCommand(sbSql.ToString());
                    //if (executeCount == count)
                    //{
                    //    result = true;
                    //    scope.Complete(); 
                    //}
                    scope.Complete();
                }
            }
            return Tuple.Create(1, true);
        }

        public bool UpdateUserMobile(UserInfo userInfo)
        {
            var returnInt = 0;
            using (var db = new Chitunion2017DbContext())
            {
                db.Entry(userInfo).State = EntityState.Modified;
                db.Entry(userInfo).Property(p => p.Mobile).IsModified = true;
                returnInt = db.SaveChanges();
            }
            return returnInt > 0;
        }
        public int UpdateCarUser(CarToChiTuUser carUser)
        {
            string strSql = $@"UPDATE dbo.UserInfo SET  ChannelUserID=@channelUserId,UserName=@userName,HeadimgURL=@headimgUrl,LastUpdateTime=@lastUpdateTime
                               WHERE  UserID={carUser.UserId} and (UserName!=@userName or HeadimgURL!=@headimgUrl)";

            if (carUser.Sex == 0 || carUser.Sex == 1)
            {
                strSql += $" UPDATE dbo.UserDetailInfo SET   Sex={carUser.Sex} WHERE UserID={carUser.UserId} and Sex!={carUser.Sex}";
            }

            object[] paras = {
                 new SqlParameter("@channelUserId",carUser.ChannelUserId),
                 new SqlParameter("@userName",carUser.UserName),
                 new SqlParameter("@headimgUrl",carUser.HeadimgUrl),
                 new SqlParameter("@lastUpdateTime",carUser.LastUpdateTime)
                };
            return context.Database.ExecuteSqlCommand(strSql, paras);
        }


    }
}
