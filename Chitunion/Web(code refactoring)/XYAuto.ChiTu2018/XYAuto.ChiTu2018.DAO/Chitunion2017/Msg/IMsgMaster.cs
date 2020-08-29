using XYAuto.ChiTu2018.Entities.Chitunion2017;

namespace XYAuto.ChiTu2018.DAO.Chitunion2017.Msg
{
    /// <summary>
    /// 注释：业务接口
    /// 作者：guansl
    /// 日期：2018/5/7 20:20:53
    /// </summary>
    public interface IMsgMaster
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="dto">实体</param>
        /// <returns></returns>
        int Add(Msg_Master dto);
        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Msg_Master GetMsgMaster(int id);
    }
}
