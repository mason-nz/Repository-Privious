using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Wechat.Impl
{
    public sealed class LeInviteRecord : RepositoryImpl<LE_InviteRecord>, ILeInviteRecord
    {
        public int GetBeInvitedCount(int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LE_InviteRecord> GetBeInvitedList(int userId, int recID, int topCount)
        {
            throw new NotImplementedException();
        }

        public int GetInvitedMaxId(int userId)
        {
            throw new NotImplementedException();
        }

        public int GetRedEvesCountByStatus(int userID, int redEvesStatus)
        {
            throw new NotImplementedException();
        }

        public int GetRedEvesStatus(int userId, int recId)
        {
            throw new NotImplementedException();
        }

        public bool IsBeInserted(int userID)
        {
            throw new NotImplementedException();
        }

        public int UpdateRedEves(int userId, int beforeStatus, int afterStatus)
        {
            throw new NotImplementedException();
        }

        public int UpdateRedEves(int userId, int recId, int beforeStatus, int afterStatus)
        {
            throw new NotImplementedException();
        }

        public int UpdateRedEvesPrice(decimal redEvesPrice, int userId, int recID)
        {
            throw new NotImplementedException();
        }

        int ILeInviteRecord.Add(LE_InviteRecord dto)
        {
            throw new NotImplementedException();
        }
    }
}
