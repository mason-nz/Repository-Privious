using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Recommend
{
    public class HomeMedia
    {
        #region Instance

        public static readonly HomeMedia Instance = new HomeMedia();

        #endregion Instance

        public int Insert(Entities.Recommend.HomeMedia entity)
        {
            return Dal.Recommend.HomeMedia.Instance.Insert(entity);
        }
    }
}