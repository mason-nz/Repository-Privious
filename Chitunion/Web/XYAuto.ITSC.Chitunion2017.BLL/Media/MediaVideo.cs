using System;
using System.Collections.Generic;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media
{
    public class MediaVideo
    {
        #region Instance
        public static readonly MediaVideo Instance = new MediaVideo();
        #endregion

        #region Contructor
        protected MediaVideo()
        { }
        #endregion

        public Entities.Media.MediaVideo GetEntity(int mediaId)
        {
            return Dal.Media.MediaVideo.Instance.GetEntity(mediaId);
        }

        public Entities.Media.MediaVideo GetEntity(string number, int platform, int filterMediaId = 0)
        {
            return Dal.Media.MediaVideo.Instance.GetEntity(number, platform, filterMediaId);
        }

        public int Insert(Entities.Media.MediaVideo entity)
        {
            return Dal.Media.MediaVideo.Instance.Insert(entity);
        }

        public int Update(Entities.Media.MediaVideo entity)
        {
            return Dal.Media.MediaVideo.Instance.Update(entity);
        }
    }
}
