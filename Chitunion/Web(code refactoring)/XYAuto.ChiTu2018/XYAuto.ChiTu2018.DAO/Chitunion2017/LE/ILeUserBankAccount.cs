/**
*
*创建人：lixiong
*创建时间：2018/5/10 15:23:57
*说明：
*版权所有：Copyright  2018 行圆汽车-分发业务中心
*/

using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.LE
{
    public interface ILeUserBankAccount : Repository<LE_UserBankAccount>
    {
        int TransUpdate(int UserID, int AccountType, string AccountName);
    }
}
