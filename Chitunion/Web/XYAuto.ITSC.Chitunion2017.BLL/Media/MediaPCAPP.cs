using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media
{
    public class MediaPCAPP
    {
        #region Instance
        public static readonly MediaPCAPP Instance = new MediaPCAPP();
        #endregion

        #region Contructor
        protected MediaPCAPP()
        { }
        #endregion

        public Entities.Media.MediaPcApp GetEntity(int mediaId)
        {
            return Dal.Media.MediaPCAPP.Instance.GetEntity(mediaId);
        }

        public Entities.Media.MediaPcApp GetEntity(string name, int filterMediaId = 0)
        {
            return Dal.Media.MediaPCAPP.Instance.GetEntity(name, filterMediaId);
        }

        public int Insert(Entities.Media.MediaPcApp entity)
        {
            return Dal.Media.MediaPCAPP.Instance.Insert(entity);
        }

        public int Update(Entities.Media.MediaPcApp entity)
        {
            return Dal.Media.MediaPCAPP.Instance.Update(entity);
        }
    }
}
