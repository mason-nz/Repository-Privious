using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media
{
    public class MediaBroadcast
    {
        #region Instance
        public static readonly MediaBroadcast Instance = new MediaBroadcast();
        #endregion

        #region Contructor
        protected MediaBroadcast()
        { }
        #endregion

        public Entities.Media.MediaBroadcast GetEntity(int mediaId)
        {
            return Dal.Media.MediaBroadcast.Instance.GetEntity(mediaId);
        }

        public Entities.Media.MediaBroadcast GetEntity(string number, int platform, int filterMediaId = 0)
        {
            return Dal.Media.MediaBroadcast.Instance.GetEntity(number, platform, filterMediaId);
        }

        public int Insert(Entities.Media.MediaBroadcast entity)
        {
            return Dal.Media.MediaBroadcast.Instance.Insert(entity);
        }

        public int Update(Entities.Media.MediaBroadcast entity)
        {
            return Dal.Media.MediaBroadcast.Instance.Update(entity);
        }
    }
}
