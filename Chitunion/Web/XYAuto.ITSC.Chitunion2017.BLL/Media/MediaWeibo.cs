using System;
using System.Collections.Generic;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media
{
    public class MediaWeibo
    {
        #region Instance
        public static readonly MediaWeibo Instance = new MediaWeibo();
        #endregion

        #region Contructor
        protected MediaWeibo()
        { }
        #endregion

        public Entities.Media.MediaWeibo GetEntity(int mediaId)
        {
            return Dal.Media.MediaWeibo.Instance.GetEntity(mediaId);
        }

        public Entities.Media.MediaWeibo GetEntity(string number, int filterMediaId = 0)
        {
            return Dal.Media.MediaWeibo.Instance.GetEntity(number, string.Empty, filterMediaId);
        }
        public Entities.Media.MediaWeibo GetEntityByName(string name, int filterMediaId = 0)
        {
            return Dal.Media.MediaWeibo.Instance.GetEntity(number: string.Empty, name: name, filterMediaId: filterMediaId);
        }
        public int Insert(Entities.Media.MediaWeibo entity)
        {
            return Dal.Media.MediaWeibo.Instance.Insert(entity);
        }

        public int Update(Entities.Media.MediaWeibo entity)
        {
            return Dal.Media.MediaWeibo.Instance.Update(entity);
        }
    }
}
