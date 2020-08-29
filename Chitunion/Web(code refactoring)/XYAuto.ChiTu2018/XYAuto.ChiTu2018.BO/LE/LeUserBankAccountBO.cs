/**
*
*创建人：lixiong
*创建时间：2018/5/10 15:28:07
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.BO.User;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.DAO.Chitunion2017.User;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;

namespace XYAuto.ChiTu2018.BO.LE
{
    public class LeUserBankAccountBO
    {
        private static ILeUserBankAccount UserBankAccount()
        {
            return IocMannager.Instance.Resolve<ILeUserBankAccount>();
        }
        public List<LE_UserBankAccount> GetByUserId(int userId, UserBankAccountTypeEnum userBankAccountType = UserBankAccountTypeEnum.Zfb)
        {
            var queryable = IocMannager.Instance.Resolve<ILeUserBankAccount>().Queryable().AsNoTracking().Where(s => s.UserID == userId);
            if (userBankAccountType != UserBankAccountTypeEnum.None)
            {
                queryable = queryable.Where(s => s.AccountType == (int)userBankAccountType);
            }
            return queryable.ToList();
        }
        public LE_UserBankAccount GetBankAccountByUserId(int userId)
        {
            return UserBankAccount().Retrieve(t => t.UserID == userId);
        }

        public LE_UserBankAccount GetBankAccountInfo(int accountType, string accountName)
        {
            return UserBankAccount().Retrieve(t => t.Status == 0 && t.AccountType == accountType && t.AccountName == accountName);
        }

        public LE_UserBankAccount UpdateBankAccount(LE_UserBankAccount bankAccount)
        {
            return UserBankAccount().Put(bankAccount);
        }

        public LE_UserBankAccount AddBankAccount(LE_UserBankAccount bankAccount)
        {
            return UserBankAccount().Add(bankAccount);
        }
        public IEnumerable<LE_UserBankAccount> Queryable()
        {
            return UserBankAccount().Queryable();
        }

        public int TransUpdate(int UserID, int AccountType, string AccountName)
        {

            return UserBankAccount().TransUpdate(UserID, AccountType, AccountName);
        }


        public bool IsExistsAccount(List<int> listUserId, int category, int accountType, string accountName)
        {
            return IocMannager.Instance.Resolve<IUserInfo>().Queryable().Where(t => t.Status == 0).Join(UserBankAccount().Queryable(),
                u => u.UserID, d => d.UserID, (u, d) => new { u, d }).Any(x => x.u.Category == category &&
                                                                               x.d.AccountType == accountType && x.d.AccountName == accountName &&
                                                                               !listUserId.Contains(x.u.UserID));
        }
    }
}
