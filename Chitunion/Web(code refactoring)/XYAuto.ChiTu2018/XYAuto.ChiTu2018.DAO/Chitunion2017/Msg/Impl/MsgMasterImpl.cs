using XYAuto.ChiTu2018.DAO.Infrastructure;
using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Msg.Impl
{
    /// <summary>
    /// 注释：业务实现类，不能被继承
    /// 作者：guansl
    /// 日期：2018/5/7 20:20:53
    /// </summary>
    public sealed class MsgMasterImpl : RepositoryImpl<Msg_Master>, IMsgMaster
    {
        public int Add(Msg_Master dto)
        {
            return Add(dto);
        }
        public Msg_Master GetMsgMaster(int id)
        {
            return Retrieve(w => w.RecID == id);
        }
    }
}
