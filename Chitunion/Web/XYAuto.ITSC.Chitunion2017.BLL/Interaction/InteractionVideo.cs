using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Interaction
{
    public class InteractionVideo
    {
        #region Instance
        public static readonly InteractionVideo Instance = new InteractionVideo();
        #endregion

        #region Contructor
        protected InteractionVideo()
        { }
        #endregion

        public Entities.Interaction.InteractionVideo GetEntity(int mediaId)
        {
            return Dal.Interaction.InteractionVideo.Instance.GetEntity(mediaId);
        }

        public int Insert(Entities.Interaction.InteractionVideo entity)
        {
            return Dal.Interaction.InteractionVideo.Instance.Insert(entity);
        }

        public int Update(Entities.Interaction.InteractionVideo entity)
        {
            return Dal.Interaction.InteractionVideo.Instance.Update(entity);
        }
    }
}
